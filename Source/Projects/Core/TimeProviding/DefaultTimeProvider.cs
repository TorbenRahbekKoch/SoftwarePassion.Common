using System;

namespace SoftwarePassion.Common.Core.TimeProviding
{
    /// <summary>
    /// DefaultTimeProvider implementation, which simply returns a UtcNow DateTime.
    /// </summary>
    internal class DefaultTimeProvider : TimeProvider
    {
        private static readonly DefaultTimeProvider instance = new DefaultTimeProvider();

        private DefaultTimeProvider()
        {            
        }

        public static DefaultTimeProvider Instance
        {
            get { return DefaultTimeProvider.instance; }
        }

        protected override DateTime RetrieveUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}