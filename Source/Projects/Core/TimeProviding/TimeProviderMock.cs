using System;

namespace SoftwarePassion.Common.Core.TimeProviding
{
    /// <summary>
    /// Convenience class for testing scenarios. Use TimeSetter to encapsulate it in a using statement.
    /// </summary>
    public class TimeProviderMock : TimeProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeProviderMock" /> class, setting the DateTime to DateTime.UtcNow.
        /// </summary>
        public TimeProviderMock()
        {
            DateTime = DateTime.UtcNow;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TimeProviderMock" /> class, setting the DateTime to the given value.
        /// </summary>
        public TimeProviderMock(DateTime now)
        {
            DateTime = now;
        }

        /// <summary>
        /// Gets or sets the date time.
        /// </summary>
        public DateTime DateTime { get; set; }

        /// <summary>
        /// Returns the mocked DateTime
        /// </summary>
        /// <returns>
        /// A DateTime.
        /// </returns>
        protected override DateTime RetrieveUtcNow()
        {
            return DateTime;
        }
    }
}
