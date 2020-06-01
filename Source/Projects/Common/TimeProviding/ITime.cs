using System;

namespace SoftwarePassion.Common.TimeProviding
{
    /// <summary>
    /// An injectable time provider. 
    /// </summary>
    public interface ITime
    {
        DateTime Now { get; }

        DateTime UtcNow { get; }
    }
}