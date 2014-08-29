using System;
using System.Globalization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SoftwarePassion.Common.Utilities.Test.MSTest
{
    public static class AssertExtensions
    {
        /// <summary>
        /// Makes it easy to assert that a block of code throws an exception as expected.
        /// </summary>
        /// <typeparam name="TExceptionType">The type of the exception type.</typeparam>
        /// <param name="action">The code block to test.</param>
        /// <code>
        //  AssertExtensions.ThrowsException<ArgumentException>(
        ///   () =>
        ///   { 
        ///     // code to test 
        ///   });
        /// </code>
        public static void ThrowsException<TExceptionType>(Action action)
            where TExceptionType : Exception
        {

            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                if (!(e.GetType() == typeof(TExceptionType)))
                {
                    Assert.Fail(string.Format(
                        CultureInfo.InvariantCulture,
                        "Expected {0} exception but got {1}",
                        typeof(TExceptionType).Name,
                        e.GetType().Name));
                }

                return;
            }

            Assert.Fail(string.Format(
                CultureInfo.InvariantCulture,
                "Expected {0} exception but got none.",
                typeof(TExceptionType).Name));
        }

        /// <summary>
        /// Makes it easy to assert that a block of code does NOT throw an exception.
        /// </summary>
        /// <param name="action">The code block to test.</param>
        /// <code>
        /// AssertExtensions.DoesNotThrow(
        ///   () =>
        ///   { 
        ///     // code to test 
        ///   });
        /// </code>
        public static void DoesNotThrow(Action action)
        {
            try
            {
                action.Invoke();
            }
            catch (Exception e)
            {
                Assert.Fail(string.Format(
                    CultureInfo.InvariantCulture,
                    "Expected no exception but got {0}. Details: {1}",
                    e.GetType().Name,
                    e.ToString()));
            }
        }
    }
}
