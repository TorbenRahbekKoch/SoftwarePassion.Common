using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SoftwarePassion.Common.Core.Data;

namespace SoftwarePassion.Common.Core.ErrorHandling
{
    /// <summary>
    /// Encapsulates retry logic and error handling for calling general code.
    /// Errors arising during the (repeated) calls are collected and if the retry
    /// limit as reached a ProviderInaccessibleException is thrown.
    /// </summary>
    /// <typeparam name="TReturn">The type of the return value from the typedExecute method.</typeparam>
    /// <code>
    ///     MyDataClass result = await RetryHandler.Execute(
    ///         RetrySettings.Create(3, TimeSpan.FromMilliseconds(100), 1.2),
    ///         () => Describe.Parameters(parameters to this method),
    ///         async() =>
    ///         {
    ///            Do dangerous stuff ....
    ///         });
    /// </code>
    public class RetryHandler<TReturn>
    {
        /// <summary>
        /// Executes the specified action with the given retry settings.
        /// </summary>
        /// <param name="retrySettings">The retry settings.</param>
        /// <param name="action">The action to automatically retry.</param>
        /// <returns>Task.</returns>
        /// <exception cref="ProviderInaccessibleException">When unsuccessful. Examine InnerExceptions
        /// for details.</exception>
        public static void Execute(
            RetrySettings retrySettings,
            Action action)
        {
            var handler = new RetryHandler<bool>(
                retrySettings,
                () => { action(); return Task.FromResult(false); });

                handler.ExecuteAsync().GetAwaiter();
        }

        /// <summary>
        /// Asynchronously executes the specified action with the given retry settings.
        /// </summary>
        /// <param name="retrySettings">The retry settings.</param>
        /// <param name="action">The action to automatically retry.</param>
        /// <returns>Task.</returns>
        /// <exception cref="ProviderInaccessibleException">When unsuccessful. Examine InnerExceptions
        /// for details.</exception>
        public static Task ExecuteAsync(
            RetrySettings retrySettings,
            Func<Task> action)
        {
            return new RetryHandler<bool>(
                retrySettings,
                async () => { await action(); return true; })
                .ExecuteAsync();
        }

        /// <summary>
        /// Asynchronously executes the specified action with the given retry settings.
        /// </summary>
        /// <param name="retrySettings">The retry settings.</param>
        /// <param name="parameterDescriptor">Not used.</param>
        /// <param name="action">The action to automatically retry.</param>
        /// <returns>Task.</returns>
        /// <exception cref="ProviderInaccessibleException">When unsuccessful. Examine InnerExceptions
        /// for details.</exception>
        [Obsolete("Will be removed in a later version. Use overload without parameterDescripter.")]
        public static Task ExecuteAsync(
            RetrySettings retrySettings,
            Func<string> parameterDescriptor,
            Func<Task> action)
        {
            return new RetryHandler<bool>(
                retrySettings,
                async () => { await action(); return true; })
                .ExecuteAsync();
        }

        /// <summary>
        /// Executes the specified action with the given retry settings.
        /// </summary>
        /// <param name="retrySettings">The retry settings.</param>
        /// <param name="action">The action to automatically retry.</param>
        /// <returns>A Task which will have the resulting value when done.</returns>
        /// <exception cref="ProviderInaccessibleException">When unsuccessful. Examine InnerExceptions
        /// for details.</exception>
        public static TReturnType Execute<TReturnType>(
            RetrySettings retrySettings,
            Func<TReturnType> action)
        {
            var handler = new RetryHandler<TReturnType>(
                retrySettings,
                () => Task.FromResult(action()));

            return handler.ExecuteAsync().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Asynchronously executes the specified action with the given retry settings.
        /// </summary>
        /// <param name="retrySettings">The retry settings.</param>
        /// <param name="action">The action to automatically retry.</param>
        /// <returns>A Task which will have the resulting value when done.</returns>
        /// <exception cref="ProviderInaccessibleException">When unsuccessful. Examine InnerExceptions
        /// for details.</exception>
        public static async Task<TReturnType> ExecuteAsync<TReturnType>(
            RetrySettings retrySettings,
            Func<Task<TReturnType>> action)
        {
            return await new RetryHandler<TReturnType>(
                retrySettings,
                action)
                .ExecuteAsync();
        }

        /// <summary>
        /// Asynchronously executes the specified action with the given retry settings.
        /// </summary>
        /// <param name="retrySettings">The retry settings.</param>
        /// <param name="parameterDescriptor">Not used.</param>
        /// <param name="action">The action to automatically retry.</param>
        /// <returns>A Task which will have the resulting value when done.</returns>
        /// <exception cref="ProviderInaccessibleException">When unsuccessful. Examine InnerExceptions
        /// for details.</exception>
        [Obsolete("Will be removed in a later version. Use overload without parameterDescriptor.")]
        public static async Task<TReturnType> ExecuteAsync<TReturnType>(
            RetrySettings retrySettings,
            Func<string> parameterDescriptor,
            Func<Task<TReturnType>> action)
        {
            return await new RetryHandler<TReturnType>(
                retrySettings,
                action)
                .ExecuteAsync();
        }

        /// <summary>
        /// Instantiates a RetryHandler of type TReturn.
        /// </summary>
        /// <param name="retrySettings">The retry settings.</param>
        /// <param name="awaitableAction">The data access code that should be guarded.</param>
        /// <exception cref="System.ArgumentNullException">
        /// retrySettings or awaitableAction
        /// </exception>
        public RetryHandler(
            RetrySettings retrySettings,
            Func<Task<TReturn>> awaitableAction)
        {
            this.retrySettings = retrySettings;
            this.awaitableAction = awaitableAction;
        }

        /// <summary>
        /// Instantiates a RetryHandler of type TReturn.
        /// </summary>
        /// <param name="retrySettings">The retry settings.</param>
        /// <param name="parameterDescriptor">Not used.</param>
        /// <param name="awaitableAction">The data access code that should be guarded.</param>
        /// <exception cref="System.ArgumentNullException">
        /// parameterDescriptor or action
        /// </exception>
        [Obsolete("Will be removed in a later version. Use overload without parameterDescriptor.")]
        public RetryHandler(
            RetrySettings retrySettings,
            Func<string> parameterDescriptor,
            Func<Task<TReturn>> awaitableAction)
        {
            this.retrySettings = retrySettings;
            this.awaitableAction = awaitableAction;
        }

        /// <summary>
        /// Tries to execute the guarded code.
        /// </summary>
        /// <returns>The value of type TReturn, when successful.</returns>
        /// <exception cref="ProviderInaccessibleException">When unsuccessful. 
        /// Examine InnerExceptions for details.</exception>
        public async Task<TReturn> ExecuteAsync()
        {
            int tryNumber = 0;
            var coolOffPeriod = retrySettings.CoolOffPeriod;
            Func<bool> retryThresholdReached = () => tryNumber >= retrySettings.RetryCount;
            var exceptionsCaught = new List<Exception>();

            while (!retryThresholdReached())
            {
                tryNumber++;

                try
                {
                    return await awaitableAction();
                }
                catch (UserRecoverableException exception)
                {
                    // A UserRecoverableException cannot be fixed by simply
                    // retrying, so we break.
                    exceptionsCaught.Add(exception);
                    break;
                }
                catch (RecoverableException exception)
                {
                    exceptionsCaught.Add(exception);
                }
                catch (UnrecoverableException exception)
                {
                    // An UnrecoverableException cannot be fixed by 
                    // simply retrying, so we break.
                    exceptionsCaught.Add(exception);
                    break;
                }
                catch (Exception exception)
                {
                    exceptionsCaught.Add(exception);
                    // A generic exception we must assume is not recoverable.
                    throw new ProviderInaccessibleException(exceptionsCaught);
                }

                await Task.Delay(retrySettings.CoolOffPeriod);
                coolOffPeriod = AdjustCoolOffPeriod(coolOffPeriod);
            }

            // We have exceeded the retryCount so report that we cannot access the provider
            throw new ProviderInaccessibleException("Retry count exceeded. ", exceptionsCaught);
        }

        private TimeSpan AdjustCoolOffPeriod(TimeSpan coolOffPeriod)
        {
            return TimeSpan.FromTicks(Convert.ToInt64(coolOffPeriod.Ticks*retrySettings.CoolOffPeriodAdjustmentFactor));
        }

        private readonly RetrySettings retrySettings;
        private readonly Func<Task<TReturn>> awaitableAction;
    }
}