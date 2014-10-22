using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace SoftwarePassion.Common.Core.ErrorHandling
{
    /// <summary>
    /// This signals that the error that happened can somehow be recovered. Possibly by the end-user.
    /// </summary>
    [Serializable]
    public class UserRecoverableException : RecoverableException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecoverableException" /> class.
        /// </summary>
        public UserRecoverableException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecoverableException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public UserRecoverableException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecoverableException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public UserRecoverableException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecoverableException" /> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected UserRecoverableException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            Contract.Requires(serializationInfo != null);
        }
    }
}