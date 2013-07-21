namespace SoftwarePassion.Common.Core.Utilities
{
    /// <summary>
    /// The result from <see cref="UrlParser" />.
    /// </summary>
    public struct UrlParts
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UrlParts"/> struct.
        /// </summary>
        /// <param name="scheme">The scheme.</param>
        /// <param name="host">The host.</param>
        /// <param name="path">The path.</param>
        /// <param name="leaf">The leaf.</param>
        /// <param name="leafExtension">The leaf extension.</param>
        /// <param name="urlExtension">The URL extension.</param>
        /// <param name="query">The query.</param>
        public UrlParts(string scheme, string host, string path, string leaf, string leafExtension, string urlExtension, string query)
            : this()
        {
            this.Query = query;
            this.UrlExtension = urlExtension;
            this.LeafExtension = leafExtension;
            this.Leaf = leaf;
            this.Path = path;
            this.Host = host;
            Scheme = scheme;
        }

        /// <summary>
        /// The Scheme part of the Uri.
        /// </summary>
        public string Scheme { get; internal set; }

        /// <summary>
        /// Gets the host part.
        /// </summary>
        public string Host { get; internal set; }

        /// <summary>
        /// Gets the path of the Uri.
        /// </summary>
        public string Path { get; internal set; }

        /// <summary>
        /// Gets the Leaf part.
        /// </summary>
        public string Leaf { get; internal set; }

        /// <summary>
        /// Gets the leaf extension.
        /// </summary>
        public string LeafExtension { get; internal set; }

        /// <summary>
        /// Gets the UrlExtension. <see cref="UrlParser"/>
        /// </summary>
        public string UrlExtension { get; internal set; }

        /// <summary>
        /// Gets the query part.
        /// </summary>
        public string Query { get; internal set; }
    }
}