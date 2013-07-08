using System;

namespace SoftwarePassion.Common.Core.TimeProviding
{
    /// <summary>
    /// Used for encapsulating a change of the DefaultTimeProvider. Very useful in tests.
    /// </summary>
    /// <remarks>
    /// Implements IDisposable so it figures out to restore the DefaultTimeProvider after use.
    /// Example:
    /// <code>
    ///   var timeProviderMock = new TimeProviderMock();
    ///   using (var timeSetter = new TimeSetter(timeProviderMock)
    ///   {
    ///     timeProviderMock.Now = new DateTime(2010, 1, 1);
    /// 
    ///     var now = TimeProvider.UtcNow; // Returns 2010-01-01.
    ///   }
    /// </code>
    /// </remarks>
    public sealed class TimeSetter : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSetter"/> class.
        /// </summary>
        /// <param name="timeProvider">The time provider to set as current TimeProvider.</param>
        public TimeSetter(TimeProvider timeProvider)
        {
            if (timeProvider == null)
            {
                throw new ArgumentNullException("timeProvider");
            }

            previousProvider = TimeProvider.Current;
            TimeProvider.Current = timeProvider;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeSetter" /> class with a TimeProviderMock as the
        /// provider.
        /// </summary>
        /// <param name="currentTime">The current time.</param>
        public TimeSetter(DateTime currentTime)
        {
            previousProvider = TimeProvider.Current;
            TimeProvider.Current = new TimeProviderMock(currentTime);
        }

        /// <summary>
        /// Restores the previous TimeProvider.
        /// </summary>
        public void Dispose()
        {
            TimeProvider.Current = this.previousProvider;
        }

        /// <summary>
        /// Sets the current time, if the provider is a TimeProviderMock.
        /// </summary>
        public static void SetNow(DateTime now)
        {
            var mock = TimeProvider.Current as TimeProviderMock;
            if (mock != null)
                mock.DateTime = now;
        }

        private readonly TimeProvider previousProvider;
    }
}