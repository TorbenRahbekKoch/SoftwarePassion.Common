using NUnit.Framework;
using SoftwarePassion.Common.Core.ErrorHandling;

namespace SoftwarePassion.Common.Core.Tests.Unit.ErrorHandling.SqlUniqueKeyCiolationTests
{
    [TestFixture]
    public class When_parsing_constraint_exception_message
    {
        [Test]
        public void Verify_that_2627_unique_constraint_is_parsed_correctly()
        {
            var message =
                "Violation of UNIQUE KEY constraint 'UQ_TableName_ColumnName'. Cannot insert duplicate key in object 'dbo.TableName'. The duplicate key value is (blablabla).";

            var violation = SqlUniqueKeyViolation.Parse(SqlErrorCodes.UniqueConstraint, message);

            Assert.AreEqual("TableName", violation.TableName, "TableName incorrect.");
            Assert.AreEqual("ColumnName", violation.KeyName, "KeyName incorrect.");
            Assert.AreEqual("blablabla", violation.DuplicateKeyValue, "DuplicateKeyValue incorrect.");
        }

        [Test]
        public void Verify_that_2627_unique_constraint_is_parsed_correctly_when_underscore_in_key_value()
        {
            var message =
                "Violation of UNIQUE KEY constraint 'UQ_TableName_ColumnName'. Cannot insert duplicate key in object 'dbo.TableName'. The duplicate key value is (bla_blabla).";

            var violation = SqlUniqueKeyViolation.Parse(SqlErrorCodes.UniqueConstraint, message);

            Assert.AreEqual("TableName", violation.TableName, "TableName incorrect.");
            Assert.AreEqual("ColumnName", violation.KeyName, "KeyName incorrect.");
            Assert.AreEqual("bla_blabla", violation.DuplicateKeyValue, "DuplicateKeyValue incorrect.");
        }

        [Test]
        public void Verify_that_2627_primary_key_constraint_is_parsed_correctly()
        {
            var message =
                "Violation of PRIMARY KEY constraint 'PK_TableName_ColumnName'. Cannot insert duplicate key in object 'dbo.TableName'. The duplicate key value is (blablabla).";

            var violation = SqlUniqueKeyViolation.Parse(SqlErrorCodes.UniqueConstraint, message);

            Assert.AreEqual("TableName", violation.TableName, "TableName incorrect.");
            Assert.AreEqual("ColumnName", violation.KeyName, "KeyName incorrect.");
            Assert.AreEqual("blablabla", violation.DuplicateKeyValue, "DuplicateKeyValue incorrect.");
        }

        [Test]
        public void Verify_that_2601_unique_index_is_parsed_correctly()
        {
            var message =
                "Cannot insert duplicate key row in object 'dbo.TableName' with unique index 'IDX_TableName_ColumnName'. The duplicate key value is (blablabla).";

            var violation = SqlUniqueKeyViolation.Parse(SqlErrorCodes.UniqueIndexConstraint, message);

            Assert.AreEqual("TableName", violation.TableName, "TableName incorrect.");
            Assert.AreEqual("ColumnName", violation.KeyName, "KeyName incorrect.");
            Assert.AreEqual("blablabla", violation.DuplicateKeyValue, "DuplicateKeyValue incorrect.");
        }

        [Test]
        public void Verify_that_2601_unique_index_is_parsed_correctly_when_lower_case()
        {
            var message =
                "Cannot insert duplicate key row in object 'dbo.TableName' with unique index 'idx_TableName_ColumnName'. The duplicate key value is (blablabla).";

            var violation = SqlUniqueKeyViolation.Parse(SqlErrorCodes.UniqueIndexConstraint, message);

            Assert.AreEqual("TableName", violation.TableName, "TableName incorrect.");
            Assert.AreEqual("ColumnName", violation.KeyName, "KeyName incorrect.");
            Assert.AreEqual("blablabla", violation.DuplicateKeyValue, "DuplicateKeyValue incorrect.");
        }

        [Test]
        public void Verify_that_2601_unique_index_is_parsed_correctly_when_underscore_in_key_value()
        {
            var message =
                "Cannot insert duplicate key row in object 'dbo.TableName' with unique index 'idx_TableName_ColumnName'. The duplicate key value is (+«ÌSµ…p¹¸'Ä£M_t¥G).";

            var violation = SqlUniqueKeyViolation.Parse(SqlErrorCodes.UniqueIndexConstraint, message);

            Assert.AreEqual("TableName", violation.TableName, "TableName incorrect.");
            Assert.AreEqual("ColumnName", violation.KeyName, "KeyName incorrect.");
            Assert.AreEqual("+«ÌSµ…p¹¸'Ä£M_t¥G", violation.DuplicateKeyValue, "DuplicateKeyValue incorrect.");
        }
    }
}