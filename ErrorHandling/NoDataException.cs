using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace SoftwarePassion.Common.ErrorHandling
{
    /// <summary>
    /// Thrown when there was no data returned. This can happen because search
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
