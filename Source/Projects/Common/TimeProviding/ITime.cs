using System;

namespace SoftwarePassion.Common.Core.TimeProviding
{
    public interface ITime
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }
}