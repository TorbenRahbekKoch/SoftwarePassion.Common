using System;

namespace SoftwarePassion.Common.Core.ErrorHandling
{
    /// <summary>
    /// Describes a unique key violation. 
    /// Assumes that unique constraints are named:
    ///   UQ_tablename_columnname.
    /// Assumes that primary key constraints are named:
    ///   PK_tablename_columnname.
    /// Assumes that unique indexes are named:
    ///   IDX_tablename_columnname.
    /// </summary>
    public class SqlUniqueKeyViolation
    {
        /// <summary>
        /// Creates the specified exception violation message.
        /// </summary>
        /// <param name="sqlExceptionNumber">The SQL error number.</param>
        /// <param name="exceptionViolationMessage">The SQL exception message.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">exceptionViolationMessage</exception>
        public static SqlUniqueKeyViolation Parse(int sqlExceptionNumber, string exceptionViolationMessage)
        {
            if (sqlExceptionNumber == SqlErrorCodes.UniqueConstraint)
            {
                // Violation of %ls constraint '%.*ls'. Cannot insert duplicate key in object '%.*ls'. The duplicate key value is %ls.
                var uqIndex = exceptionViolationMessage.IndexOf("'UQ_", StringComparison.OrdinalIgnoreCase);
                var pkIndex = exceptionViolationMessage.IndexOf("'PK_", StringComparison.OrdinalIgnoreCase);
                var keyIndex = uqIndex > 0 ? uqIndex : pkIndex;
                if (keyIndex > 0)
                {
                    var apostrophIndex = exceptionViolationMessage.IndexOf("'.", keyIndex + 4, StringComparison.OrdinalIgnoreCase);

                    var tableNameUnderscoreIndex = exceptionViolationMessage.LastIndexOf("_", apostrophIndex, apostrophIndex - keyIndex, StringComparison.OrdinalIgnoreCase);
                    var tableName = exceptionViolationMessage.Substring(keyIndex + 4, tableNameUnderscoreIndex - (keyIndex + 4));

                    string keyName = exceptionViolationMessage.Substring(tableNameUnderscoreIndex + 1,
                        apostrophIndex - (tableNameUnderscoreIndex + 1));

                    var valueStart =
                        exceptionViolationMessage.IndexOf("(", apostrophIndex, StringComparison.OrdinalIgnoreCase) + 1;
                    var value = exceptionViolationMessage.Substring(valueStart, exceptionViolationMessage.Length - valueStart - 2);
                    return new SqlUniqueKeyViolation(tableName, keyName, value);
                }
            }

            // Cannot insert duplicate key row in object '%.*ls' with unique index '%.*ls'. The duplicate key value is %ls.
            {
                var idxIndex = exceptionViolationMessage.IndexOf("'IDX_", StringComparison.OrdinalIgnoreCase);
                var apostrophIndex = exceptionViolationMessage.IndexOf("'.", idxIndex + 4, StringComparison.OrdinalIgnoreCase);

                var tableNameUnderscoreIndex = exceptionViolationMessage.LastIndexOf("_", apostrophIndex, apostrophIndex - idxIndex, StringComparison.OrdinalIgnoreCase);

                var tableName = exceptionViolationMessage.Substring(idxIndex + 5, tableNameUnderscoreIndex - idxIndex - 5);
                string keyName = exceptionViolationMessage.Substring(
                    tableNameUnderscoreIndex + 1,
                    apostrophIndex - (tableNameUnderscoreIndex + 1));

                var valueStart = exceptionViolationMessage.IndexOf("(", apostrophIndex, StringComparison.OrdinalIgnoreCase) + 1;
                var value = exceptionViolationMessage.Substring(valueStart, exceptionViolationMessage.Length - valueStart - 2);
                return new SqlUniqueKeyViolation(tableName, keyName, value);
            }
        }

        private SqlUniqueKeyViolation(string tableName, string keyName, string duplicateKeyValue)
        {
            TableName = tableName;
            KeyName = keyName;
            DuplicateKeyValue = duplicateKeyValue;
        }

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public string TableName { get; private set; }

        /// <summary>
        /// Gets the name of the key.
        /// </summary>
        public string KeyName { get; private set; }

        /// <summary>
        /// Gets the name of the key.
        /// </summary>
        public string DuplicateKeyValue { get; private set; }
    }
}