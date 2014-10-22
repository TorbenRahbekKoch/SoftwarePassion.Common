using System;
using SoftwarePassion.Common.Core.Extensions;

namespace SoftwarePassion.Common.Core.ErrorHandling
{
    /// <summary>
    /// Exception thrown when trying to update data that has been deleted.
    /// </summary>
    [Serializable]
    public class DataDeletedException : DataUpdatedException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataDeletedException" /> class.
        /// </summary>
        /// <param name="entityName">Name of the entity.</param>
        /// <param name="id">The id of the entity.</param>
        public DataDeletedException(string entityName, string id)
            : base(entityName, id, "Entity with Id {0} deleted from {1}.".FormatInvariant(entityName, id))
        { }
    }
}
