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
        /// <param name="retryCount">The inner retry count.</param>
        /// <param name="coolOffPeriod">The first cool off period.</param>
        /// <param name="coolOffPeriodAdjustmentFactor">The factor with which to adjust the coolOffPeriod after a failed attempt.</param>
        /// <returns>The created RetrySettings.</returns>
        public static RetrySettings Create(int retryCount, TimeSpan coolOffPeriod, float coolOffPeriodAdjustmentFactor)
        {
            return new RetrySettings(retryCount, coolOffPeriod, coolOffPeriodAdjustmentFactor);
        }

        /// <summary>
        /// Gets the cool off period. The period where no retries are attempted.
        /// </summary>
        public TimeSpan CoolOffPeriod { get; private set; }

        /// <summary>
        /// Gets the retry count - this is the maximum number of retries attempted.
        /// </summary>
        public int RetryCount { get; private set; }

        /// <summary>
        /// Gets the adjustment factor for the CoolOffPeriod. The CoolOffPeriod
        /// is adjusted between retries by multiplying itself with the factor.
        /// This is a cumulative process. Each new value is multiplied with the
        /// factor on further retries.
        /// </summary>
        public float CoolOffPeriodAdjustmentFactor{ get; private set; }

        private RetrySettings(int retryCount, TimeSpan coolOffPeriod, float coolOffPeriodAdjustmentFactor)
        {
            RetryCount = retryCount;
            CoolOffPeriod = coolOffPeriod;
            CoolOffPeriodAdjustmentFactor = coolOffPeriodAdjustmentFactor;
        }
    }
}