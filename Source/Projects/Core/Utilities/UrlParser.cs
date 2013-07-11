using System;
using System.Diagnostics.Contracts;

namespace SoftwarePassion.Common.Core.Utilities
{
    /// <summary>
    /// Splits a Uri into a UrlParts which contains the different constituents of the uri.
    /// </summary>
    /// <remarks>
    /// I made this class ages ago. Have no idea why, but keeping it just for nostalgic reasons. Just brushed
    /// it up in a few places.
    /// </remarks>
    public static class UrlParser
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
            Contract.Requires(uri != null);

            UrlParts urlParts = AssignParts(uri);

            // There are two markers that are definitive.
            // ? (question mark) marks the beginning of the query string. This is definitive. This case has been handled above
            // / at rightmost position without any ? indicates that the entire path is actually a path, no leaf etc.

            // Now any query string has been cut of, now check if the rightmost character is /:
            int slashPos = urlParts.Path.LastIndexOf('/');
            if (slashPos == urlParts.Path.Length - 1) // Yes, we have slash at the righmost position, this means no leaf, no ext, etc.
            {
                urlParts.Path = urlParts.Path.Substring(0, slashPos);
                return urlParts; // Nothing more to do
            }

            int dotPos = urlParts.Path.LastIndexOf('.');
            if (dotPos > 0) // there is a . - we may have a leaf then
            {
                slashPos = urlParts.Path.LastIndexOf('/', dotPos);      // Find the first slash before the dot
                int slashPosAfter = urlParts.Path.IndexOf('/', dotPos); // Find first slash, if any, after the dot
                if (slashPosAfter < 0) // There is no following slash
                {
                    urlParts.Leaf = urlParts.Path.Substring(slashPos + 1); // the leaf is the entire end
                    urlParts.LeafExtension = urlParts.Path.Substring(dotPos + 1);
                    if (slashPos >= 0) // This if is necessary because Substring does not accept negative lengths
                        urlParts.Path = urlParts.Path.Substring(0, slashPos);
                    else
                        urlParts.Path = "";
                }
                else if (slashPosAfter < urlParts.Path.Length - 1) // we have an url extension
                {
                    urlParts.Leaf = urlParts.Path.Substring(slashPos + 1, slashPosAfter - slashPos - 1);
                    urlParts.LeafExtension = urlParts.Path.Substring(dotPos + 1, slashPosAfter - dotPos - 1);
                    urlParts.UrlExtension = urlParts.Path.Substring(slashPosAfter + 1);
                    if (slashPos >= 0) // This if is necessary because Substring does not accept negative lengths
                        urlParts.Path = urlParts.Path.Substring(0, slashPos);
                    else
                        urlParts.Path = "";
                }
            }

            return urlParts;
        }

        private static UrlParts AssignParts(Uri uri)
        {
            UrlParts parts;

            // Get the full content path 
            string path = uri.AbsolutePath;
            if (path.Length > 1 && path[0] == '/')
                path = path.Substring(1);

            string leaf = "";
            string leafExtension = "";
            string urlExtension = "";
            string query = uri.Query;
            if (query.Length > 1 && query[0] == '?')
                query = query.Substring(1);

            return new UrlParts(
                uri.Scheme, 
                uri.Host,
                path,
                leaf,
                leafExtension,
                urlExtension,
                query);
        }
    }
}
