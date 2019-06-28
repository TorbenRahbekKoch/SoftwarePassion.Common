using System;

namespace SoftwarePassion.Common.Core.TimeProviding
{
    public class TimeMock : ITime
    {
        public TimeMock(DateTime now, DateTime utcNow)
        {
            Now = now;
            UtcNow = utcNow;
        }

        public DateTime Now { get; }
        public DateTime UtcNow { get; }
    }
}