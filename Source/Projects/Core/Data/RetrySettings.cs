using System;

namespace SoftwarePassion.Common.Core.Data
{
    /// <summary>
    /// Retry settings used by SqlDataAccessHandler.
    /// </summary>
    public class RetrySettings
    {
        /// <summary>
        /// Convenience method for creating RetrySettings
        /// </summary>
        /// <param name="coolOffPeriod">The cool off period.</param>
        /// <param name="innerRetryCount">The inner retry count.</param>
        /// <param name="outerRetryCount">The outer retry count.</param>
        /// <returns>The created RetrySettings.</returns>
        public static RetrySettings Create(TimeSpan coolOffPeriod, int innerRetryCount, int outerRetryCount)
        {
            return new RetrySettings(coolOffPeriod, innerRetryCount, outerRetryCount);
        }

        /// <summary>
        /// Gets the cool off period. The period where no retries are attempted.
        /// </summary>
        public TimeSpan CoolOffPeriod { get; private set; }

        /// <summary>
        /// Gets the inner retry count - this is the number of retries attempted without any pause in between.
        /// </summary>
        public int InnerRetryCount { get; private set; }

        /// <summary>
        /// Gets the outer retry count. The CoolOffPeriod elapses between each of these retries. And 
        /// for each of these retry attemps, InnerRetryCount attempts are made.
        /// </summary>
        /// <value>
        /// The outer retry count.
        /// </value>
        public int OuterRetryCount { get; private set; }

        private RetrySettings(TimeSpan coolOffPeriod, int innerRetryCount, int outerRetryCount)
        {
            CoolOffPeriod = coolOffPeriod;
            InnerRetryCount = innerRetryCount;
            OuterRetryCount = outerRetryCount;
        }
    }
}