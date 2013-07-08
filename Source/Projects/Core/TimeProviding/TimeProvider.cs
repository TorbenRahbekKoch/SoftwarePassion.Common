using System;

namespace SoftwarePassion.Common.Core.TimeProviding
{
    /// <summary>
    /// Ambient TimeProvider with support for property injection to change the actual
    /// TimeProvider implementation, which is very useful in testing scenarios.
    /// </summary>
    public abstract class TimeProvider
    {
        /// <summary>
        /// Gets the current TimeProvider.
        /// </summary>
        public static TimeProvider Current
        {
            get
            {
                return TimeProvider.current;
            }

            internal set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }

                TimeProvider.current = value;
            }
        }

        /// <summary>
        /// Gets the current time in UTC.
        /// </summary>
        public static DateTime UtcNow
        {
            get
            {
                return TimeProvider.Current.RetrieveUtcNow();
            }
        }

        /// <summary>
        /// Creates a DateTime of DateTimeKind.Utc
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <returns>The created DateTime.</returns>
        public static DateTime CreateUtc(int year, int month, int day)
        {
            return DateTime.SpecifyKind(new DateTime(year, month, day), DateTimeKind.Utc);
        }

        /// <summary>
        /// Creates a DateTime of DateTimeKind.Utc
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <param name="second">The second.</param>
        /// <returns></returns>
        public static DateTime CreateUtc(int year, int month, int day, int hour, int minute, int second)
        {
            return DateTime.SpecifyKind(new DateTime(year, month, day, hour, minute, second), DateTimeKind.Utc);
        }

        /// <summary>
        /// Creates a DateTime of DateTimeKind.Utc
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        /// <param name="minute">The minute.</param>
        /// <param name="second">The second.</param>
        /// <param name="millisecond">The millisecond.</param>
        /// <returns></returns>
        public static DateTime CreateUtc(int year, int month, int day, int hour, int minute, int second, int millisecond)
        {
            return DateTime.SpecifyKind(new DateTime(year, month, day, hour, minute, second, millisecond), DateTimeKind.Utc);
        }

        /// <summary>
        /// Resets to using the default TimeProvider (which uses DateTime).
        /// </summary>
        public static void ResetToDefault()
        {
            TimeProvider.current = DefaultTimeProvider.Instance;
        }

        /// <summary>
        /// Override to implement the specific way to return a current DateTime of DateTimeKind.Utc.
        /// </summary>
        /// <returns>A DateTime.</returns>
        protected abstract DateTime RetrieveUtcNow();

        private static TimeProvider current = DefaultTimeProvider.Instance;
    }
}