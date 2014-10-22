using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace SoftwarePassion.Common.Core.ErrorHandling
{
    /// <summary>
    /// This signals that the error that happened cannot be recovered (at least
    /// without help from support staff/programmers etc.).
    /// Usually a more specific inherited exception should be thrown.
    /// </summary>
    [Serializable]
    public class UnrecoverableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UnrecoverableException" /> class.
        /// </summary>
        public UnrecoverableException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnrecoverableException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UnrecoverableException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnrecoverableException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UnrecoverableException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="UnrecoverableException" /> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected UnrecoverableException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            Contract.Requires(serializationInfo != null);
        }
    }
}