using System;
using NUnit.Framework;
using SoftwarePassion.Common.Core.Utilities;

namespace SoftwarePassion.Common.Core.Tests.Unit.Utilities
{
    [TestFixture]
    public class UrlParserTest
    {
        [Test]
        public void Test()
        {
            string url = "http://www.somewhere.net/parts.path1/parts.path2/page.aspx/tests?parts.querystringthingy";
            Uri uri = new Uri(url);
            UrlParts parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.Host == "www.somewhere.net");
            Assert.IsTrue(parts.Path == "parts.path1/parts.path2");
            Assert.IsTrue(parts.Leaf == "page.aspx");
            Assert.IsTrue(parts.LeafExtension == "aspx");
            Assert.IsTrue(parts.UrlExtension == "tests");
            Assert.IsTrue(parts.Query == "parts.querystringthingy");

            url = "http://www.somewhere.net";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.Host == "www.somewhere.net");
            Assert.IsTrue(parts.Path == "");
            Assert.IsTrue(parts.Leaf == "");
            Assert.IsTrue(parts.LeafExtension == "");
            Assert.IsTrue(parts.UrlExtension == "");
            Assert.IsTrue(parts.Query == "");

            url = "http://www.somewhere.net?querystring";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.Host == "www.somewhere.net");
            Assert.IsTrue(parts.Path == "");
            Assert.IsTrue(parts.Leaf == "");
            Assert.IsTrue(parts.LeafExtension == "");
            Assert.IsTrue(parts.UrlExtension == "");
            Assert.IsTrue(parts.Query == "querystring");

            url = "http://www.somewhere.net/";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.Host == "www.somewhere.net");
            Assert.IsTrue(parts.Path == "");
            Assert.IsTrue(parts.Leaf == "");
            Assert.IsTrue(parts.LeafExtension == "");
            Assert.IsTrue(parts.UrlExtension == "");
            Assert.IsTrue(parts.Query == "");

            url = "http://www.somewhere.net/index.htm";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.Host == "www.somewhere.net");
            Assert.IsTrue(parts.Path == "");
            Assert.IsTrue(parts.Leaf == "index.htm");
            Assert.IsTrue(parts.LeafExtension == "htm");
            Assert.IsTrue(parts.UrlExtension == "");
            Assert.IsTrue(parts.Query == "");

            url = "http://www.somewhere.net/path/index.htm";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.Host == "www.somewhere.net");
            Assert.IsTrue(parts.Path == "path");
            Assert.IsTrue(parts.Leaf == "index.htm");
            Assert.IsTrue(parts.LeafExtension == "htm");
            Assert.IsTrue(parts.UrlExtension == "");
            Assert.IsTrue(parts.Query == "");

            url = "http://www.somewhere.net/index.htm/testing";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.Host == "www.somewhere.net");
            Assert.IsTrue(parts.Path == "");
            Assert.IsTrue(parts.Leaf == "index.htm");
            Assert.IsTrue(parts.LeafExtension == "htm");
            Assert.IsTrue(parts.UrlExtension == "testing");
            Assert.IsTrue(parts.Query == "");


            url = "http://www.somewhere.net/test/";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.Host == "www.somewhere.net");
            Assert.IsTrue(parts.Path == "test");
            Assert.IsTrue(parts.Leaf == "");
            Assert.IsTrue(parts.LeafExtension == "");
            Assert.IsTrue(parts.UrlExtension == "");
            Assert.IsTrue(parts.Query == "");

            url = "http://www.somewhere.net/test/test.ext/";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.Host == "www.somewhere.net");
            Assert.IsTrue(parts.Path == "test/test.ext");
            Assert.IsTrue(parts.Leaf == "");
            Assert.IsTrue(parts.LeafExtension == "");
            Assert.IsTrue(parts.UrlExtension == "");
            Assert.IsTrue(parts.Query == "");

            url = "http://www.somewhere.net/test/test.ext/?querythingy";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.Host == "www.somewhere.net");
            Assert.IsTrue(parts.Path == "test/test.ext");
            Assert.IsTrue(parts.Leaf == "");
            Assert.IsTrue(parts.LeafExtension == "");
            Assert.IsTrue(parts.UrlExtension == "");
            Assert.IsTrue(parts.Query == "querythingy");
        }
    }
}
