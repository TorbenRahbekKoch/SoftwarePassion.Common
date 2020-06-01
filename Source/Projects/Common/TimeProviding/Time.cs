using System;

namespace SoftwarePassion.Common.TimeProviding
{
    /// <summary>
    /// Default implementation of ITime, giving the current, real time.
    /// </summary>
    public class Time : ITime
    {
        /// <summary>
        /// Returns DateTime.Now
        /// </summary>
        public DateTime Now { get; } = DateTime.Now;

        /// <summary>
        /// Returns DateTime.UtcNow
        /// </summary>
        public DateTime UtcNow { get; } = DateTime.UtcNow;
    }
}