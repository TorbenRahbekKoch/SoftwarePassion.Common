using System;
using NUnit.Framework;
using SoftwarePassion.Common.Core.TimeProviding;

namespace SoftwarePassion.Common.Core.Tests.Unit.TimeProviding
{
    [TestFixture]
    public class TimeSetterTest
    {
        [Test]
        public void TimeSetterScopeTest()
        {
            var timeProviderMock = new TimeProviderMock();
            DateTime mockTime;
            using (var timeSetter = new TimeSetter(timeProviderMock))
            {
                mockTime = TimeProvider.UtcNow;
            }

            var expectedTime = new DateTime(2010, 1, 1);
            Assert.AreEqual(expectedTime, mockTime);
            Assert.AreNotEqual(expectedTime, TimeProvider.UtcNow);
        }
    }
}