using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Transactions;
using SoftwarePassion.Common.Core.ErrorHandling;
using TimeoutException = SoftwarePassion.Common.Core.ErrorHandling.TimeoutException;

namespace SoftwarePassion.Common.Core.Data
{
    /// <summary>
    /// Encapsulates retry logic and error handling for calls to a Sql Server.
    /// Errors arising during the call is mapped into the standard set of 
    /// exceptions in the ErrorHandling namespace.
    /// </summary>
    /// <typeparam name="TReturn">The type of the return value from the typedExecute method.</typeparam>
    /// <code>
    ///     MyDataClass result = await SqlDataAccessHandler.Execute(
    ///         RetrySettings.Create(TimeSpan.FromMilliseconds(100), 3, 3),
    ///         () => MethodDescriptor.Describe(parameters to this method),
    ///         async() =>
    ///         {
    ///             using (var connection = new SqlConnection(connectionString))
    ///             {
    ///                 ... execute sql...
    ///                 return new MyDataClass(data...)
    ///             }
    ///         });
    /// </code>
    public class SqlDataAccessHandler<TReturn>
    {
        /// <summary>
        /// Executes the specified sql action with the specified settings. This is 
        /// for Sql statements which do not return any value.
        /// </summary>
        /// <param name="retrySettings">The retry settings.</param>
        /// <param name="methodDescriptor">The method descriptor.</param>
        /// <param name="sqlAction">The sql action.</param>
        public static Task Execute(
            RetrySettings retrySettings,
            Func<string> methodDescriptor,
            Func<Task> sqlAction)
        {
            return new SqlDataAccessHandler<bool>(
                retrySettings,
                methodDescriptor,
                async () => { await sqlAction(); return true; })
                .Execute();
        }

        /// <summary>
        /// Executes the specified sql action with the specified settings. This is
        /// for Sql data retrieval statements.
        /// </summary>
        /// <typeparam name="TReturnType">The type of the return value.</typeparam>
        /// <param name="retrySettings">The retry settings.</param>
        /// <param name="methodDescriptor">The method descriptor.</param>
        /// <param name="sqlAction">The sql action.</param>
        /// <returns></returns>
        public static async Task<TReturnType> Execute<TReturnType>(
            RetrySettings retrySettings,
            Func<string>
            methodDescriptor,
            Func<Task<TReturnType>> sqlAction)
        {
            return await new SqlDataAccessHandler<TReturnType>(
                retrySettings,
                methodDescriptor,
                sqlAction)
                .Execute();
        }

        /// <summary>
        /// Instantiates a SqlDataAccessHandlerImplementation of type TReturn.
        /// </summary>
        /// <param name="retrySettings">The retry settings.</param>
        /// <param name="methodDescriptor">The method descriptor. Used for error logging.</param>
        /// <param name="awaitableAction">The data access code that should be guarded.</param>
        /// <exception cref="System.ArgumentNullException">
        /// methodDescriptor or action
        /// </exception>
        public SqlDataAccessHandler(
            RetrySettings retrySettings,
            Func<string> methodDescriptor,
            Func<Task<TReturn>> awaitableAction)
        {
            if (methodDescriptor == null) throw new ArgumentNullException("methodDescriptor");
            if (awaitableAction == null) throw new ArgumentNullException("awaitableAction");

            this.retrySettings = retrySettings;
            this.methodDescriptor = methodDescriptor;
            this.awaitableAction = awaitableAction;
        }
        /// <summary>
        /// Tries to execute the guarded code.
        /// </summary>
        /// <returns>The return value of type T, on success.</returns>
        /// <exception cref="ProviderInaccessibleException">On unknown errors and when the circuit continues to trip.</exception>
        /// <exception cref="DuplicateKeyException">When duplicate keys are detected.</exception>
        /// <exception cref="DataUpdatedException">When an optimistic concurrency conflict is detected.</exception>
        /// <exception cref="ErrorHandling.TimeoutException">When an action timed out.</exception>
        /// <exception cref="DeadlockedException">When a deadlock is detected.</exception>
        /// <remarks>Only two exceptions need to be catched: ProviderInaccessibleException and RecoverableException, since
        /// DuplicateKeyException, DataUpdatedException, TimeoutException and DeadlockedException all inherit from
        /// RecoverableException.</remarks>
        public async Task<TReturn> Execute()
        {
            int tryNumber = 0;
            Func<bool> retryThresholdReached = () => tryNumber >= retrySettings.OuterRetryCount;
            var exceptionsCaught = new List<Exception>();

            while (!retryThresholdReached())
            {
                tryNumber++;

                try
                {
                    return await awaitableAction();
                }
                catch (InvalidOperationException exception)
                {
                    // InvalidOperationException most likely suggests that we tried to execute 
                    // when the curcuit breaker was in an invalid state.
                    exceptionsCaught.Add(exception);
                    if (retryThresholdReached())
                    {
                        throw new ProviderInaccessibleException(exceptionsCaught);
                    }
                }
                catch (SqlException sqlException)
                {
                    if (sqlException.Number == SqlErrorCodes.ForeignKeyConstraint)
                    {
                        // This is an error, which is not recoverable here, so throw a new exception instead of retrying.
                        var violation = SqlForeignKeyViolation.Parse(sqlException.Number, sqlException.Message);
                        throw new ForeignKeyException(violation.LocalTable, violation.ForeignTable, violation.ForeignColumn, sqlException.Message, sqlException);
                    }

                    if (sqlException.Number == SqlErrorCodes.UniqueConstraint || sqlException.Number == SqlErrorCodes.UniqueIndexConstraint)
                    {
                        // This is a duplicate key error, which is not recoverable here, so throw a new exception instead of retrying.
                        var violation = SqlUniqueKeyViolation.Parse(sqlException.Number, sqlException.Message);
                        throw new DuplicateKeyException(violation.TableName, violation.KeyName, violation.DuplicateKeyValue, sqlException.Message, sqlException);
                    }

                    if (sqlException.Number == SqlErrorCodes.Deadlock)
                    {
                        // This is an error, which may be recoverable, so only throw if no more retries are left.
                        if (retryThresholdReached())
                        {
                            throw new DeadlockedException(sqlException);
                        }
                    }

                    if (sqlException.Number == SqlErrorCodes.Timeout)
                    {
                        // This is an error, which may be recoverable, so only throw if no more retries are left.
                        if (retryThresholdReached())
                        {
                            throw new TimeoutException(sqlException);
                        }
                    }

                    if (sqlException.Number == SqlErrorCodes.StringOrBinaryDataTruncation)
                    {
                        throw new TruncatedDataException("Data truncated.", sqlException);
                    }

                    if (retryThresholdReached())
                    {
                        // Something is seriously wrong when we get to here. It is not recoverable.
                        throw new ProviderInaccessibleException(sqlException);
                    }
                }
                catch (TransactionAbortedException exception)
                {
                    exceptionsCaught.Add(exception);
                    // Something is seriously wrong when we get this exception. It is not recoverable.
                    throw new ProviderInaccessibleException(methodDescriptor(), exceptionsCaught);
                }
                catch (Exception exception)
                {
                    exceptionsCaught.Add(exception);
                    // A generic exception may come from the guarded code. But since we have no idea what went
                    // wrong, we must assume that the error is not recoverable.
                    throw new ProviderInaccessibleException(methodDescriptor(), exceptionsCaught);
                }

                await Task.Delay(retrySettings.CoolOffPeriod);
            }

            // We have exceeded the retryCount so report that we cannot access the provider
            throw new ProviderInaccessibleException("Retry count exceeded. " + methodDescriptor(), exceptionsCaught);
        }

        private readonly RetrySettings retrySettings;
        private readonly Func<string> methodDescriptor;
        private readonly Func<Task<TReturn>> awaitableAction;
    }
}