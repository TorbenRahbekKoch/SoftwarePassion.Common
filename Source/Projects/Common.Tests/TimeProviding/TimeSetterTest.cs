using System;
using SoftwarePassion.Common.TimeProviding.Ambient;
using Xunit;

namespace SoftwarePassion.Common.Core.Tests.Unit.TimeProviding
{
    public class TimeSetterTest
    {
        [Fact]
        public void TimeSetterScopeTest()
        {
            var timeProviderMock = new TimeProviderMock();
            DateTime mockTime;
            using (var timeSetter = new TimeSetter(timeProviderMock))
            {
                mockTime = TimeProvider.UtcNow;
            }

            var expectedTime = new DateTime(2010, 1, 1);
            Assert.Equal(expectedTime, mockTime);
            Assert.NotEqual(expectedTime, TimeProvider.UtcNow);
        }
    }
}