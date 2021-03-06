﻿using System;
using System.Runtime.Serialization;

namespace SoftwarePassion.Common.Core.ErrorHandling
{
    /// <summary>
    /// This signals that the error that happened can somehow be recovered. Possibly by the end-user.
    /// </summary>
    [Serializable]
    public class RecoverableException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RecoverableException" /> class.
        /// </summary>
        public RecoverableException()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecoverableException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public RecoverableException(string message)
            : base(message)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecoverableException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public RecoverableException(string message, Exception innerException)
            : base(message, innerException)
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="RecoverableException" /> class.
        /// </summary>
        /// <param name="serializationInfo">The serialization info.</param>
        /// <param name="context">The context.</param>
        protected RecoverableException(SerializationInfo serializationInfo, StreamingContext context)
            : base(serializationInfo, context)
        {
        }
    }
}