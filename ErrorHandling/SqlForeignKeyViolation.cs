using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SoftwarePassion.Common.ErrorHandling
{
    /// <summary>
    /// Parses/converts a sqlExceptionNumber and exception message into a SqlForeignKeyViolation structure
    /// </summary>
    public class SqlForeignKeyViolation
    {
        /// <summary>
        /// Gets or sets the name local table, which is referring the foreign table.
        /// </summary>
        public string LocalTable { get; private set; }

        /// <summary>
        /// Gets the name of the foreign table.
        /// </summary>
        public string ForeignTable { get; private set; }

        /// <summary>
        /// Gets name of the foreign column.
        /// </summary>
        public string ForeignColumn { get; private set; }

        /// <summary>
        /// Parses the specified SQL exception number and message. 
        /// </summary>
        /// <param name="sqlExceptionNumber">The SQL exception number.</param>
        /// <param name="exceptionViolationMessage">The exception violation message.</param>
        /// <returns></returns>
        public static SqlForeignKeyViolation Parse(int sqlExceptionNumber, string exceptionViolationMessage)
        {
            Contract.Requires(exceptionViolationMessage != null);

            if (sqlExceptionNumber == SqlErrorCodes.ForeignKeyConstraint)
            {
                var fkIndex = exceptionViolationMessage.IndexOf("\"FK_", StringComparison.OrdinalIgnoreCase);
                if (fkIndex > 0)
                {
                    // Sample:
                    // The INSERT statement conflicted with the FOREIGN KEY constraint "FK_SystemApplication_SystemOwner". The 
                    // conflict occurred in database "Servicekatalog_IntegrationTest", table "dbo.SystemOwner", column 'Id'.

                    var nextUnderscore = exceptionViolationMessage.IndexOf("_", fkIndex + 4, StringComparison.OrdinalIgnoreCase);
                    var localTable = exceptionViolationMessage.Substring(fkIndex + 4, nextUnderscore - fkIndex - 4);

                    var match = ForeignTableMatcher.Match(exceptionViolationMessage);
                    var foreignTable = match.Groups["foreigntable"].Value;
                    match = ForeignColumnMatcher.Match(exceptionViolationMessage);
                    var foreignColumn = match.Groups["foreigncolumn"].Value;
                    return new SqlForeignKeyViolation(localTable, foreignTable, foreignColumn);
                }

                return new SqlForeignKeyViolation(string.Empty, string.Empty, string.Empty);
            }

            return new SqlForeignKeyViolation(string.Empty, string.Empty, string.Empty);
        }

        private SqlForeignKeyViolation(string localTable, string foreignTable, string foreignColumn)
        {
            LocalTable = localTable;
            ForeignTable = foreignTable;
            ForeignColumn = foreignColumn;
        }

        private static readonly Regex ForeignTableMatcher = new Regex(@"table ""(?<foreigntable>.+)""");
        private static readonly Regex ForeignColumnMatcher = new Regex(@"column '(?<foreigncolumn>.+)'");
    }
}