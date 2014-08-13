using NUnit.Framework;
using SoftwarePassion.Common.Core.Extensions;

namespace SoftwarePassion.Common.Core.Tests.Unit.Extensions.StringExtensionTests
{
    [TestFixture]
    public class When_replacing_string
    {
        [Test]
        public void Verify_that_empty_replace_string_removes_part_of_string()
        {
            const string replacable = "The quick brown fox jumped over the lazy dog";

            string replaced = replacable.Replace(4, 6, string.Empty);

            Assert.AreEqual("The brown fox jumped over the lazy dog", replaced, "because 'quick ' was removed");
        }

        [Test]
        public void Verify_that_specific_replace_string_replaces_part_of_string()
        {
            const string replacable = "The quick brown fox jumped over the lazy dog";

            string replaced = replacable.Replace(4, 5, "slow");

            Assert.AreEqual("The slow brown fox jumped over the lazy dog", replaced, "because 'quick' was replaced with 'slow'");
        }

        [Test]
        public void Verify_that_index_plus_length_beyond_string_replaces_end_of_string()
        {
            const string replacable = "The quick brown fox jumped over the lazy dog";

            string replaced = replacable.Replace(41, 10, "horse");

            Assert.AreEqual("The quick brown fox jumped over the lazy horse", replaced, "because 'dog' was replaced with 'horse'");
        }

        [Test]
        public void Verify_that_index_zero_replaces_start_of_string()
        {
            const string replacable = "The quick brown fox jumped over the lazy dog";

            string replaced = replacable.Replace(0, 3, "A");

            Assert.AreEqual("A quick brown fox jumped over the lazy dog", replaced, "because 'The' was replaced with 'A'");
        }
    }
}