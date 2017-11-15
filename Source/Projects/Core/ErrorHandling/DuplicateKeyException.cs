using System;
using System.Runtime.Serialization;
using System.Security;

namespace SoftwarePassion.Common.Core.ErrorHandling
{
    /// <summary>
    /// Exception thrown when a duplicate key situation is encountered.
    /// </summary>
    [Serializable]
    public class DuplicateKeyException : UserRecoverableException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyException" /> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="duplicateFieldName">Name of the duplicate field.</param>
        /// <param name="duplicateFieldValue">The duplicate field value.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DuplicateKeyException(string entityName, string duplicateFieldName, string duplicateFieldValue, string message, Exception innerException)
            : base(message, innerException)
        {
            EntityName = entityName;
            DuplicateFieldName = duplicateFieldName;
            DuplicateFieldValue = duplicateFieldValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyException" /> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="duplicateFieldName">Name of the duplicate field.</param>
        /// <param name="duplicateFieldValue">The duplicate field value.</param>
        /// <param name="message">The message.</param>
        public DuplicateKeyException(string entityName, string duplicateFieldName, string duplicateFieldValue, string message)
            : base(message)
        {
            EntityName = entityName;
            DuplicateFieldName = duplicateFieldName;
            DuplicateFieldValue = duplicateFieldValue;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DuplicateKeyException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected DuplicateKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            EntityName = info.GetString(EntityNameKey);
            DuplicateFieldName = info.GetString(DuplicateFieldNameKey);
            DuplicateFieldValue = info.GetString(DuplicateFieldValueKey);
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
            info.AddValue(EntityNameKey, EntityName);
            info.AddValue(DuplicateFieldNameKey, DuplicateFieldName);
            info.AddValue(DuplicateFieldValueKey, DuplicateFieldValue);
        }

        /// <summary>
        /// Gets the name of the entity in which the duplicate key was encountered.
        /// </summary>
        public string EntityName { get; private set; }

        /// <summary>
        /// Gets the name of the field in which the duplicate key was encountered.
        /// </summary>
        public string DuplicateFieldName { get; private set; }

        /// <summary>
        /// Gets the duplicate field value.
        /// </summary>
        public string DuplicateFieldValue { get; private set; }

        private const string EntityNameKey = "EntityName";
        private const string DuplicateFieldNameKey = "DuplicateFieldName";
        private const string DuplicateFieldValueKey = "DuplicateFieldValue";
    }
}