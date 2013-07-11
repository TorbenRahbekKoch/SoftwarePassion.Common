using System;

namespace SoftwarePassion.Common.Core.TimeProviding
{
    internal class TimeProviderAction : TimeProvider
    {
        public TimeProviderAction(Func<DateTime> timeProvider)
        {
            this.timeProvider = timeProvider;
            if (timeProvider == null) throw new ArgumentNullException("timeProvider");
        }

        protected override DateTime RetrieveUtcNow()
        {
            return timeProvider();
        }

        private readonly Func<DateTime> timeProvider;
    }
}