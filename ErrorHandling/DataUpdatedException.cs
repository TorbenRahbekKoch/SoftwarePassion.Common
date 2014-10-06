using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.Serialization;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using SoftwarePassion.Common.Core.Extensions;

namespace SoftwarePassion.Common.ErrorHandling
{
    /// <summary>
    /// Exception thrown by the Data Access Layer when a concurrency problem (data already deleted/updated) is 
    /// encountered. 
    /// </summary>
    [Serializable]
    public class DataUpdatedException : UserRecoverableException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataUpdatedException" /> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="id">The id.</param>
        /// <param name="innerException">The inner exception.</param>
        public DataUpdatedException(string entityName, string id, Exception innerException)
            : base("Entity with Id {0} updated or deleted in {1}.".FormatInvariant(entityName, id), innerException)
        {
            EntityName = entityName;
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataUpdatedException" /> class.
        /// </summary>
        /// <param name="entityName">Name of the table.</param>
        /// <param name="id">The id.</param>
        /// <param name="message">The message.</param>
        public DataUpdatedException(string entityName, string id, string message)
            : base(message)
        {
            EntityName = entityName;
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataUpdatedException" /> class.
        /// </summary>
        /// <param name="entityName">Name of the table.</param>
        /// <param name="id">The id.</param>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public DataUpdatedException(string entityName, string id, string message, Exception innerException)
            : base(message, innerException)
        {
            EntityName = entityName;
            Id = id;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataUpdatedException"/> class.
        /// </summary>
        /// <param name="info">The <see cref="T:System.Runtime.Serialization.SerializationInfo" /> that holds the serialized object data about the exception being thrown.</param>
        /// <param name="context">The <see cref="T:System.Runtime.Serialization.StreamingContext" /> that contains contextual information about the source or destination.</param>
        protected DataUpdatedException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            Contract.Requires(info != null);

            EntityName = info.GetString(EntityNameKey);
            Id = info.GetString(IdKey);
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
            info.AddValue(IdKey, Id);
        }

        /// <summary>
        /// Gets the name of the table .
        /// </summary>
        /// <value>
        /// The name of the table.
        /// </value>
        public string EntityName { get; private set; }

        /// <summary>
        /// A string representation of the primary key of the entity that was updated.
        /// </summary>
        public string Id { get; private set; }

        private const string EntityNameKey = "TableName";
        private const string IdKey = "Id";
    }
}