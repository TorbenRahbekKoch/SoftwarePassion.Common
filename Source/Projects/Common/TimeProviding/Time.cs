using System;

namespace SoftwarePassion.Common.Core.TimeProviding
{
    public class Time : ITime
    {
        public DateTime Now { get; } = DateTime.Now;

        public DateTime UtcNow { get; } = DateTime.UtcNow;
    }
}