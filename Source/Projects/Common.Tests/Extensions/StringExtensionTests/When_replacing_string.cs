using FluentAssertions;
using Xunit;
using SoftwarePassion.Common.Core.Extensions;

namespace SoftwarePassion.Common.Core.Tests.Extensions.StringExtensionTests
{
    public class When_replacing_string
    {
        [Fact]
        public void Verify_that_empty_replace_string_removes_part_of_string()
        {
            const string replacable = "The quick brown fox jumped over the lazy dog";

            string replaced = replacable.Replace(4, 6, string.Empty);

            replaced.Should().Be("The brown fox jumped over the lazy dog", because: "because 'quick ' was removed");
        }

        [Fact]
        public void Verify_that_specific_replace_string_replaces_part_of_string()
        {
            const string replacable = "The quick brown fox jumped over the lazy dog";

            string replaced = replacable.Replace(4, 5, "slow");

            replaced.Should().Be("The slow brown fox jumped over the lazy dog", because: "because 'quick' was replaced with 'slow'");
        }

        [Fact]
        public void Verify_that_index_plus_length_beyond_string_replaces_end_of_string()
        {
            const string replacable = "The quick brown fox jumped over the lazy dog";

            string replaced = replacable.Replace(41, 10, "horse");

            replaced.Should().Be("The quick brown fox jumped over the lazy horse", because: "because 'dog' was replaced with 'horse'");
        }

        [Fact]
        public void Verify_that_index_zero_replaces_start_of_string()
        {
            const string replacable = "The quick brown fox jumped over the lazy dog";

            string replaced = replacable.Replace(0, 3, "A");

            replaced.Should().Be("A quick brown fox jumped over the lazy dog", because: "because 'The' was replaced with 'A'");
        }
    }
}