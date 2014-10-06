using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SoftwarePassion.Common.Core.Extensions;

namespace SoftwarePassion.Common.ErrorHandling
{
    /// <summary>
    /// Exception thrown by the Data Access Layer when trying to update data that has been deleted.
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
