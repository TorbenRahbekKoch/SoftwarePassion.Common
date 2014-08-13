using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Reflection;

namespace SoftwarePassion.Common.Core.PluginManagement
{
    /// <summary>
    /// Configuration for <see cref="PluginFinder"/>.
    /// </summary>
    public class PluginFinderConfiguration
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginFinderConfiguration"/> class.
        /// </summary>
        /// <param name="excludeSystemAssemblies">Whether to exlude System* assemblies.</param>
        /// <param name="typesToExclude">Types to exclude as candidates for a plugin.</param>
        /// <param name="assembliesToExclude">The assemblies to exclude, when searching for a plugin.</param>
        /// <param name="assembliesToLoadExplicitly">The assemblies to load explicitly.</param>
        /// <param name="typeToExplicitlyLookFor">A possible type to explicitly look for.</param>
        public PluginFinderConfiguration(
            ExcludeSystemAssemblies excludeSystemAssemblies, 
            IList<Type> typesToExclude,
            IList<AssemblyName> assembliesToExclude, 
            IList<AssemblyName> assembliesToLoadExplicitly, 
            Option<string> typeToExplicitlyLookFor)
        {
            Contract.Requires(typesToExclude != null);
            Contract.Requires(assembliesToExclude != null);
            Contract.Requires(assembliesToLoadExplicitly != null);
            
            TypeToExplicitlyLookFor = typeToExplicitlyLookFor;
            TypesToExclude = typesToExclude;
            ExcludeSystemAssemblies = excludeSystemAssemblies;
            AssembliesToExclude = assembliesToExclude;
            AssembliesToLoadExplicitly = assembliesToLoadExplicitly;
        }

        /// <summary>
        /// Gets whether to exclude System* assemblies
        /// </summary>
        /// <value>The exclude system assemblies.</value>
        public ExcludeSystemAssemblies ExcludeSystemAssemblies { get; private set; }

        /// <summary>
        /// Gets the assemblies to exclude, when searching for a plugin.
        /// </summary>
        /// <value>The assemblies to exclude.</value>
        public IList<AssemblyName> AssembliesToExclude { get; private set; }

        /// <summary>
        /// Gets the assemblies to load explicitly.
        /// </summary>
        /// <value>The assemblies to load explicitly.</value>
        public IList<AssemblyName> AssembliesToLoadExplicitly { get; private set; }

        /// <summary>
        /// Gets the types to exclude as candidates for a plugin.
        /// </summary>
        /// <value>The types to exclude.</value>
        public IList<Type> TypesToExclude { get; private set; }

        /// <summary>
        /// Gets a possible type to explicitly look for.
        /// </summary>
        /// <value>The type to explicitly look for.</value>
        public Option<string> TypeToExplicitlyLookFor { get; private set; }
    }
}