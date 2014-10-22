using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;

namespace SoftwarePassion.Common.Core.ErrorHandling
{
    /// <summary>
    /// Thrown when there was no data returned. This can happen e.g. because search
    /// criteria are wrong which makes this, in general, a UserRecoverableException.
    /// </summary>
    [Serializable]
    public class NoDataException : UserRecoverableException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoDataException" /> class.
        /// </summary>
        public NoDataException()
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="NoDataException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public NoDataException(string message)
            : base(message)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="NoDataException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public NoDataException(string message, Exception innerException)
            : base(message, innerException)
        {}

        /// <summary>
        /// Initializes a new instance of the <see cref="NoDataException" /> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected NoDataException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
            Contract.Requires(serializationInfo != null);
        }
    }
}
