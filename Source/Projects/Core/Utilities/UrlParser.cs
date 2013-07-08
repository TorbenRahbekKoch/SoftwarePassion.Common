using System;

namespace SoftwarePassion.Common.Core.Utilities
{
    public struct UrlParts
    {
        public string scheme;
        public string host;
        public string path;
        public string leaf;
        public string leafExtension;
        public string urlExtension;
        public string query;
    }

    public class UrlParser
    {
        // A typical url concists of a scheme (http, ftp, etc)
        // A host (www.something.something)
        // A path - perhaps empty
        // A page - perhaps empty
        // The difficulty arises when you have to distinguish between a leaf in the path and a page. To overcome this
        // problem I have chosen that if the path ends with / there is no leaf. Otherwise the part of the path to the
        // right of the last / is called a leaf. If the leaf has an extension this is singled out. 
        // Consider: http://www.mywebserver.org/something/
        //   Scheme: http
        //   Host  : www.mywebserver.org
        //   Path  : something
        //   Leaf  : 
        //   Ext   :
        // http://www.webserver.org/somepath/leaf.ext
        //   Scheme: http
        //   Host  : www.webserver.org
        //   Path  : somepath
        //   Leaf  : leaf
        //   Ext   : ext
        // http://www.webserver.org/somepath/leaf.ext/
        //   Scheme: http
        //   Host  : www.webserver.org
        //   Path  : somepath/leaf.ext
        //   Leaf  : 
        //   Ext   : 
        // http://www.webserver.org/somepath/leaf.ext/urlextension?querystringthingy"
        //   Scheme: http
        //   Host  : www.webserver.org
        //   Path  : somepath
        //   Leaf  : leaf
        //   Ext   : ext
        //   UrlExt: urlextension
        //   Query : querystringthingy
        /// <summary>
        /// Splits the URL.
        /// </summary>
        /// <param name="uri">The URI to be splitted. The uri is expected to be valid</param>
        /// <returns></returns>
        public static UrlParts SplitUrl(Uri uri)
        {
            UrlParts urlParts = AssignParts(uri);

            // There are two markers that are definitive.
            // ? (question mark) marks the beginning of the query string. This is definitive. This case has been handled above
            // / at rightmost position without any ? indicates that the entire path is actually a path, no leaf etc.

            // Now any query string has been cut of, now check if the rightmost character is /:
            int slashPos = urlParts.path.LastIndexOf('/');
            if (slashPos == urlParts.path.Length - 1) // Yes, we have slash at the righmost position, this means no leaf, no ext, etc.
            {
                urlParts.path = urlParts.path.Substring(0, slashPos);
                return urlParts; // Nothing more to do
            }

            int dotPos = urlParts.path.LastIndexOf('.');
            if (dotPos > 0) // there is a . - we may have a leaf then
            {
                slashPos = urlParts.path.LastIndexOf('/', dotPos);      // Find the first slash before the dot
                int slashPosAfter = urlParts.path.IndexOf('/', dotPos); // Find first slash, if any, after the dot
                if (slashPosAfter < 0) // There is no following slash
                {
                    urlParts.leaf = urlParts.path.Substring(slashPos + 1); // the leaf is the entire end
                    urlParts.leafExtension = urlParts.path.Substring(dotPos + 1);
                    if (slashPos >= 0) // This if is necessary because Substring does not accept negative lengths
                        urlParts.path = urlParts.path.Substring(0, slashPos);
                    else
                        urlParts.path = "";
                }
                else if (slashPosAfter < urlParts.path.Length - 1) // we have an url extension
                {
                    urlParts.leaf = urlParts.path.Substring(slashPos + 1, slashPosAfter - slashPos - 1);
                    urlParts.leafExtension = urlParts.path.Substring(dotPos + 1, slashPosAfter - dotPos - 1);
                    urlParts.urlExtension = urlParts.path.Substring(slashPosAfter + 1);
                    if (slashPos >= 0) // This if is necessary because Substring does not accept negative lengths
                        urlParts.path = urlParts.path.Substring(0, slashPos);
                    else
                        urlParts.path = "";
                }
            }
            return urlParts;
        }

        private static UrlParts AssignParts(Uri uri)
        {
            UrlParts parts;
            parts.scheme = uri.Scheme;
            parts.host = uri.Host;

            // Get the full content path 
            parts.path = uri.AbsolutePath;
            if (parts.path.Length > 1 && parts.path[0] == '/')
                parts.path = parts.path.Substring(1);

            parts.leaf = "";
            parts.leafExtension = "";
            parts.urlExtension = "";
            parts.query = uri.Query;
            if (parts.query.Length > 1 && parts.query[0] == '?')
                parts.query = parts.query.Substring(1);
            return parts;
        }
    }
}
