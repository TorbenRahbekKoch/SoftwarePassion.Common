using System;

namespace SoftwarePassion.Common.Core.ErrorHandling
{
    /// <summary>
    /// Exception thrown by the Data Access Layer when a time out occurs.
    /// </summary>
    [Serializable]
    public class TimeoutException : RecoverableException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutException" /> class.
        /// </summary>
        public TimeoutException()
        { }

         /// <summary>
        /// Initializes a new instance of the <see cref="RecoverableException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public TimeoutException(string message)
            : base(message)
        { }

       /// <summary>
        /// Initializes a new instance of the <see cref="TimeoutException" /> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public TimeoutException(Exception innerException)
            : base(string.Empty, innerException)
        { }
    }
}
