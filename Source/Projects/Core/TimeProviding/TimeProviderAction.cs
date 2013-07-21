using System;
using System.Diagnostics.Contracts;

namespace SoftwarePassion.Common.Core.TimeProviding
{
    internal class TimeProviderAction : TimeProvider
    {
        public TimeProviderAction(Func<DateTime> timeProvider)
        {
            Contract.Requires(timeProvider != null);

            this.timeProvider = timeProvider;
        }

        protected override DateTime RetrieveUtcNow()
        {
            return timeProvider();
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(timeProvider != null);
        }

        private readonly Func<DateTime> timeProvider;
    }
}