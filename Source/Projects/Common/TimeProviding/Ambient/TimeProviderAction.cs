using System;

namespace SoftwarePassion.Common.TimeProviding.Ambient
{
    internal class TimeProviderAction : TimeProvider
    {
        public TimeProviderAction(Func<DateTime> timeProvider)
        {
            this.timeProvider = timeProvider;
        }

        protected override DateTime RetrieveUtcNow()
        {
            return timeProvider();
        }

        private readonly Func<DateTime> timeProvider;
    }
}