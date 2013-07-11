namespace SoftwarePassion.Common.Core.Utilities
{
    /// <summary>
    /// The result from <see cref="UrlParser"/>.
    /// </summary>
    public struct UrlParts
    {
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

        public string Scheme { get; internal set; }
        public string Host { get; internal set; }
        public string Path { get; internal set; }
        public string Leaf { get; internal set; }
        public string LeafExtension { get; internal set; }
        public string UrlExtension { get; internal set; }
        public string Query { get; internal set; }
    }
}