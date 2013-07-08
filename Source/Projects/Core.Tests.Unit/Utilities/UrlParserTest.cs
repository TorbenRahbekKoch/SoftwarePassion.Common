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
            UrlParts parts;
            string url = "http://www.somewhere.net/parts.path1/parts.path2/page.aspx/tests?parts.querystringthingy";
            Uri uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.host == "www.somewhere.net");
            Assert.IsTrue(parts.path == "parts.path1/parts.path2");
            Assert.IsTrue(parts.leaf == "page.aspx");
            Assert.IsTrue(parts.leafExtension == "aspx");
            Assert.IsTrue(parts.urlExtension == "tests");
            Assert.IsTrue(parts.query == "parts.querystringthingy");

            url = "http://www.somewhere.net";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.host == "www.somewhere.net");
            Assert.IsTrue(parts.path == "");
            Assert.IsTrue(parts.leaf == "");
            Assert.IsTrue(parts.leafExtension == "");
            Assert.IsTrue(parts.urlExtension == "");
            Assert.IsTrue(parts.query == "");

            url = "http://www.somewhere.net?querystring";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.host == "www.somewhere.net");
            Assert.IsTrue(parts.path == "");
            Assert.IsTrue(parts.leaf == "");
            Assert.IsTrue(parts.leafExtension == "");
            Assert.IsTrue(parts.urlExtension == "");
            Assert.IsTrue(parts.query == "querystring");

            url = "http://www.somewhere.net/";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.host == "www.somewhere.net");
            Assert.IsTrue(parts.path == "");
            Assert.IsTrue(parts.leaf == "");
            Assert.IsTrue(parts.leafExtension == "");
            Assert.IsTrue(parts.urlExtension == "");
            Assert.IsTrue(parts.query == "");

            url = "http://www.somewhere.net/index.htm";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.host == "www.somewhere.net");
            Assert.IsTrue(parts.path == "");
            Assert.IsTrue(parts.leaf == "index.htm");
            Assert.IsTrue(parts.leafExtension == "htm");
            Assert.IsTrue(parts.urlExtension == "");
            Assert.IsTrue(parts.query == "");

            url = "http://www.somewhere.net/path/index.htm";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.host == "www.somewhere.net");
            Assert.IsTrue(parts.path == "path");
            Assert.IsTrue(parts.leaf == "index.htm");
            Assert.IsTrue(parts.leafExtension == "htm");
            Assert.IsTrue(parts.urlExtension == "");
            Assert.IsTrue(parts.query == "");

            url = "http://www.somewhere.net/index.htm/testing";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.host == "www.somewhere.net");
            Assert.IsTrue(parts.path == "");
            Assert.IsTrue(parts.leaf == "index.htm");
            Assert.IsTrue(parts.leafExtension == "htm");
            Assert.IsTrue(parts.urlExtension == "testing");
            Assert.IsTrue(parts.query == "");


            url = "http://www.somewhere.net/test/";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.host == "www.somewhere.net");
            Assert.IsTrue(parts.path == "test");
            Assert.IsTrue(parts.leaf == "");
            Assert.IsTrue(parts.leafExtension == "");
            Assert.IsTrue(parts.urlExtension == "");
            Assert.IsTrue(parts.query == "");

            url = "http://www.somewhere.net/test/test.ext/";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.host == "www.somewhere.net");
            Assert.IsTrue(parts.path == "test/test.ext");
            Assert.IsTrue(parts.leaf == "");
            Assert.IsTrue(parts.leafExtension == "");
            Assert.IsTrue(parts.urlExtension == "");
            Assert.IsTrue(parts.query == "");

            url = "http://www.somewhere.net/test/test.ext/?querythingy";
            uri = new Uri(url);
            parts = UrlParser.SplitUrl(uri);
            Assert.IsTrue(parts.host == "www.somewhere.net");
            Assert.IsTrue(parts.path == "test/test.ext");
            Assert.IsTrue(parts.leaf == "");
            Assert.IsTrue(parts.leafExtension == "");
            Assert.IsTrue(parts.urlExtension == "");
            Assert.IsTrue(parts.query == "querythingy");
        }
    }
}
