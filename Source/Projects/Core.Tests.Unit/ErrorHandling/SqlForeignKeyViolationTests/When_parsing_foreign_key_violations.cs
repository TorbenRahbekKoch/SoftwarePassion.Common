using NUnit.Framework;
using SoftwarePassion.Common.Core.ErrorHandling;

namespace SoftwarePassion.Common.Core.Tests.Unit.ErrorHandling.SqlForeignKeyViolationTests
{
    [TestFixture]
    public class When_parsing_foreign_key_violations
    {
        [Test]
        public void Verify_that_547_insert_conflict_is_parsed_correctly()
        {
            const string message = "The INSERT statement conflicted with the FOREIGN KEY constraint \"FK_LocalTable_ForeignTable\". The conflict occurred in database \"AnAmazingDatabase\", table \"dbo.ForeignTable\", column 'Id'.";

            var violation = SqlForeignKeyViolation.Parse(547, message);

            Assert.AreEqual("LocalTable", violation.LocalTable, "LocalTable incorrect.");
            Assert.AreEqual("dbo.ForeignTable", violation.ForeignTable, "ForeignTable incorrect.");
            Assert.AreEqual("Id", violation.ForeignColumn, "ForeignColumn incorrect.");
        }
    }
}