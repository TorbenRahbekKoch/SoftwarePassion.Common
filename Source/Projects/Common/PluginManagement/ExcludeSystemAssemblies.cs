namespace SoftwarePassion.Common.PluginManagement
{
    /// <summary>
    /// Tells whether to ignore System* assemblies in PlugFinder.
    /// </summary>
    public enum ExcludeSystemAssemblies
    {
        /// <summary>
        /// Do NOT ignore System* assemblies.
        /// </summary>
        No,

        /// <summary>
        /// DO ignore System* assemblies.
        /// </summary>
        Yes
    }
}