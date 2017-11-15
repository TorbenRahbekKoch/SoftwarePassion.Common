using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;

namespace SoftwarePassion.Common.Core.ErrorHandling
{
    /// <summary>
    /// Thrown when the provider (e.g. service or database) is not available for performing the requested operation,
    /// due to some unspecified error, which cannot be narrowed down.
    /// </summary>
    [Serializable]
    public class ProviderInaccessibleException : UnrecoverableException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderInaccessibleException" /> class.
        /// </summary>
        public ProviderInaccessibleException()
            : base(string.Empty)
        {
            InnerExceptions = new List<Exception>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderInaccessibleException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public ProviderInaccessibleException(string message)
            : base(message)
        {
            InnerExceptions = new List<Exception>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderInaccessibleException" /> class.
        /// </summary>
        /// <param name="innerException">The inner exception.</param>
        public ProviderInaccessibleException(Exception innerException)
            : base(string.Empty, innerException)
        {
            InnerExceptions = new List<Exception>() { innerException };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderInaccessibleException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ProviderInaccessibleException(string message, Exception innerException)
            : base(message, innerException)
        {
            InnerExceptions = new List<Exception>() { innerException };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderInaccessibleException" /> class.
        /// </summary>
        /// <param name="innerExceptions">All the inner exceptions that happened while trying to access the provider.</param>
        public ProviderInaccessibleException(IEnumerable<Exception> innerExceptions)
            : base(string.Empty, innerExceptions.FirstOrDefault())
        {
            InnerExceptions = innerExceptions.ToList();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderInaccessibleException" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerExceptions">All the inner exceptions that happened while trying to access the provider.</param>
        public ProviderInaccessibleException(string message, IEnumerable<Exception> innerExceptions)
            : base(message, innerExceptions.FirstOrDefault())
        {
            InnerExceptions = innerExceptions.ToList();
        }

        /// <summary>
        /// Gets or sets the inner exceptions which caused the provider to be marked as inaccessible.
        /// </summary>
        public IEnumerable<Exception> InnerExceptions { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProviderInaccessibleException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ProviderInaccessibleException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            InnerExceptions = info.GetValue(InnerExceptionsKey, typeof(IEnumerable<Exception>)) as IEnumerable<Exception>;
        }

        /// <summary>
        /// When overridden in a derived class, sets the <see cref="T:System.Runtime.Serialization.SerializationInfo" /> with information about the exception.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        /// <PermissionSet>
        ///   <IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Read="*AllFiles*" PathDiscovery="*AllFiles*" />
        ///   <IPermission class="System.Security.Permissions.SecurityPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" Flags="SerializationFormatter" />
        ///   </PermissionSet>
        [SecurityCritical]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue(InnerExceptionsKey, InnerExceptions, InnerExceptions.GetType());
        }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>
        /// A string representation of the current exception.
        /// </returns>
        /// <PermissionSet><IPermission class="System.Security.Permissions.FileIOPermission, mscorlib, Version=2.0.3600.0, Culture=neutral, PublicKeyToken=b77a5c561934e089" version="1" PathDiscovery="*AllFiles*"/></PermissionSet>
        public override string ToString()
        {
            var builder = new StringBuilder(InnerExceptions.Count() * 512);

            builder.AppendLine(Message);
            builder.AppendLine();

            foreach (var innerException in InnerExceptions)
            {
                builder.AppendLine(innerException.ToString());
                builder.AppendLine();
            }

            return builder.ToString();
        }

        private const string InnerExceptionsKey = "InnerExceptions";
    }
}