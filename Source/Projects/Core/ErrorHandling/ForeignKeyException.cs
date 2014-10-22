using System;
using System.Diagnostics.Contracts;
using System.Runtime.Serialization;
using System.Security;

namespace SoftwarePassion.Common.Core.ErrorHandling
{
    /// <summary>
    /// Exception thrown by the data access layer when a duplicate key situation is encountered.
    /// </summary>
    [Serializable]
    public class ForeignKeyException : RecoverableException
    {
        /// <summary>
        /// Gets the name of the local entity referring the foreign entity.
        /// </summary>
        public string LocalEntity { get; private set; }

        /// <summary>
        /// Gets the name of foreign entity.
        /// </summary>
        public string ForeignEntity { get; private set; }

        /// <summary>
        /// Gets the name of the foreign column.
        /// </summary>
        public string ForeignColumn { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyException" /> class.
        /// </summary>
        /// <param name="localEntity">The name of the local entity.</param>
        /// <param name="foreignEntity">The name of the foreign entity.</param>
        /// <param name="foreignColumn">The name of the foreign column.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public ForeignKeyException(string localEntity, string foreignEntity, string foreignColumn, string message, Exception innerException)
            : base(message, innerException)
        {
            LocalEntity = localEntity;
            ForeignEntity = foreignEntity;
            ForeignColumn = foreignColumn;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyException" /> class.
        /// </summary>
        /// <param name="localEntity">The name of the local entity.</param>
        /// <param name="foreignEntity">The name of the foreign entity.</param>
        /// <param name="foreignColumn">The name of the foreign column.</param>
        /// <param name="message">The message.</param>
        public ForeignKeyException(string localEntity, string foreignEntity, string foreignColumn, string message)
            : base(message)
        {
            LocalEntity = localEntity;
            ForeignEntity = foreignEntity;
            ForeignColumn = foreignColumn;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ForeignKeyException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected ForeignKeyException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Contract.Requires(info != null);

            LocalEntity = info.GetString(LocalEntityKey);
            ForeignEntity = info.GetString(ForeignEntityKey);
            ForeignColumn = info.GetString(ForeignColumnKey);
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
            info.AddValue(LocalEntityKey, LocalEntity);
            info.AddValue(ForeignEntityKey, ForeignEntity);
            info.AddValue(ForeignColumnKey, ForeignColumn);
        }

        private const string LocalEntityKey = "LocalEntity";
        private const string ForeignEntityKey = "ForeignEntity";
        private const string ForeignColumnKey = "ForeignColumn";
    }
}