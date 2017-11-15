using System;
using System.Runtime.Serialization;

namespace SoftwarePassion.Common.Core.ErrorHandling
{
    /// <summary>
    /// InvalidDataException is thrown when data are inconsistent, logically wrong or in other ways just wrong. The data
    /// are probably result of a logic error somewhere and the exception is therefore considered Unrecoverable.
    /// </summary>
    [Serializable]
    public class InvalidDataException : UnrecoverableException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public InvalidDataException(string message)
            : base(message)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public InvalidDataException(string message, Exception innerException)
            : base(message, innerException)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="InvalidDataException" /> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected InvalidDataException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}