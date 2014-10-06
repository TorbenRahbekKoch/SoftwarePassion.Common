using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SoftwarePassion.Common.ErrorHandling
{
    /// <summary>
    /// Constants for common Sql Error Codes.
    /// </summary>
    public static class SqlErrorCodes
    {
        /// <summary>
        /// Value for Timeout
        /// </summary>
        public const int Timeout = -2;

        /// <summary>
        /// Value for deadlock
        /// </summary>
        public const int Deadlock = 1205;

        /// <summary>
        /// Value for foreign key constraint violation.
        /// </summary>
        public const int ForeignKeyConstraint = 547;

        /// <summary>
        /// One Value for duplicate key
        /// </summary>
        public const int UniqueIndexConstraint = 2601;

        /// <summary>
        /// UNIQUE key and PRIMARY constraint violations
        /// </summary>
        public const int UniqueConstraint = 2627;

        /// <summary>
        /// Value for data truncation.
        /// </summary>
        public const int StringOrBinaryDataTruncation = 8152;
    }
}
