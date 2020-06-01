using System;
using SoftwarePassion.Common.TimeProviding.Ambient;

namespace SoftwarePassion.Common.Core.Tests.Unit.TimeProviding
{
    public class TimeProviderMock : TimeProvider
    {
        protected override DateTime RetrieveUtcNow()
        {
            return new DateTime(2010, 1, 1);
        }
    }
}