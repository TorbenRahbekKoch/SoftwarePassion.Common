using System;
using NUnit.Framework;
using SoftwarePassion.Common.Core.Utilities;

namespace SoftwarePassion.Common.Core.Tests.Unit.Utilities
{
    [TestFixture]
    public class CommandLineOptionsParserTest
    {
        [Test]
        public void ShouldThrowWithUnevenNumberOfItemsOnCommandLine()
        {
            Assert.Throws(typeof(IndexOutOfRangeException),
                                  () =>
                                      {
                                          var items = new string[] {"Test"};                                          
                                          var parser = new CommandLineOptionsParser(items);
                                      },
                                  "Accepted uneven number of items"
                                  );
        }

        [Test]
        public void ShouldGiveCorrectOptionAndValue()
        {

            var options = new string[] {"option", "value"};
            var parser = new CommandLineOptionsParser(options);
            Assert.AreEqual("value", parser.GetValue("option"), "Wrong value for option");
        }

        [Test]
        public void ShouldThrowOnUnParsedOption()
        {
            var options = new string[] {"option", "value"};
            Assert.Throws(typeof (IndexOutOfRangeException),
                                  () =>
                                      {
                                          var parser = new CommandLineOptionsParser(options);
                                          parser.GetValue("option1");
                                      },
                                  "Did not throw on unparsed option"
                                  );
        }

        [Test]
        public  void ShouldThrowOnInvalidOptionOnCmdLine()
        {
            var options = new string[] {"-option", "value"};
            var allowedOptions = new string[] {"option"};
            Assert.Throws(typeof(IndexOutOfRangeException),
                                  () =>
                                  {
                                      var parser = new CommandLineOptionsParser(options, allowedOptions);
                                  },
                                  "Did not throw on invalid option"
                                  );

        }

        [Test]
        public void ShouldThrowOnInvalidOptionPrefixes()
        {
            var options = new string[] { "-option1", "value1", "/option2", "value2" };
            var allowedPrefixes = new string[] { "-" };
            var allowedOptions = new string[] { "option1", "option2" };
            Assert.Throws(typeof(IndexOutOfRangeException),
                                  () =>
                                  {
                                      var parser = new CommandLineOptionsParser(options, allowedOptions, allowedPrefixes);
                                  },
                                  "Did not throw on invalid option"
                                  );

        }

        [Test]
        public void ShouldUnderstandOptionPrefixes()
        {
            var options = new string[] {"-option1", "value1", "/option2", "value2"};
            var allowedPrefixes = new string[] {"-", "/"};
            Assert.DoesNotThrow(
                                  () =>
                                  {
                                      var parser = new CommandLineOptionsParser(options, new string[] {}, allowedPrefixes);
                                  },
                                  "Threw on valid prefix"
                                  );
            
        }

        [Test]
        public void ShouldReturnCorrectValuesForOptionsWithPrefixes()
        {
            var options = new string[] {"-option1", "value1", "/option2", "value2"};
            var allowedPrefixes = new string[] {"-", "/"};
            var parser = new CommandLineOptionsParser(options, new string[] {}, allowedPrefixes);
            Assert.AreEqual("value1", parser.GetValue("option1"));
            Assert.AreEqual("value2", parser.GetValue("option2"));            
        }
    }
}