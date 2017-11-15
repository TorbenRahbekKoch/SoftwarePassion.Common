using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using SoftwarePassion.Common.Core.Extensions;

namespace SoftwarePassion.Common.Core.PluginManagement
{
    /// <summary>
    /// A poor man's plugin manager used for finding an implementation of a 
    /// given interface or class.
    /// </summary>
    public static class PluginFinder
    {
        /// <summary>
        /// Tries to locate, in the current AppDomain, a class that implements
        /// TImplementee and, if found, returns an invocation of it.
        /// </summary>
        /// <typeparam name="TImplementee">The interface/class to implement/
        /// inherit from.</typeparam>
        /// <param name="configuration">The configuration for the PluginFinder</param>
        /// <param name="constructorParameters">The constructor parameters.</param>
        /// <returns>An invocation of the class found.</returns>
        /// <exception cref="System.InvalidOperationException">When none or too
        /// many implementations of the type is found.</exception>
        public static TImplementee FindAndActivate<TImplementee>(
            PluginFinderConfiguration configuration,
            params object[] constructorParameters) 
            where TImplementee : class
        {
            foreach (var assemblyName in configuration.AssembliesToLoadExplicitly)
            {
                AppDomain.CurrentDomain.Load(assemblyName);
            }

            var assembliesToExclude = configuration.AssembliesToExclude.ToDictionary(assembly => assembly.FullName);
            var currentAssembly = typeof (PluginFinder).Assembly;
            var assembliesToSearch = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly => !assembly.IsDynamic && 
                        assembly != currentAssembly && 
                        !assembliesToExclude.ContainsKey(assembly.FullName) &&
                        !(configuration.ExcludeSystemAssemblies == ExcludeSystemAssemblies.Yes && 
                            assembly.FullName.StartsWith("System", StringComparison.OrdinalIgnoreCase)))
                .ToList();

            var typeNamesToExclude = configuration.TypesToExclude.ToDictionary(t => t.FullName);
            var exportedTypes = new List<TypeInfo>();
            var composerTypes = AddPossibleComposerTypes<TImplementee>(
                assembliesToSearch, 
                exportedTypes, 
                typeNamesToExclude);

            // For some reason it sometimes happens that the above code loads two types types
            // with the same name. If this happens we simply choose to take the first of these.
            // Note: This is a workaround for a bug which origin we cannot figure out.
            // (Obviously the assembly with the TComposer implementation is for some reason loaded twice
            // into the AppDomain, but why IIS would choose to do this is beyond me)
            if (composerTypes.Select(ct => ct.FullName).Distinct().Count() == 1)
            {
                composerTypes = new List<TypeInfo>() { composerTypes.First() };
            };

            var composerTypeName = configuration.TypeToExplicitlyLookFor.IsSome
                ? configuration.TypeToExplicitlyLookFor.Value : 
                string.Empty;
            if (composerTypes.Count() > 1)
            {
                if (!composerTypeName.IsNullOrEmpty())
                    composerTypes = composerTypes
                        .Where(ct => ct.FullName == composerTypeName)
                        .ToList();

                if (composerTypes.Count() > 1)
                {
                    throw new InvalidOperationException(
                        "Too many ({0}) implementations of '{1}' found: '{2}'. ".FormatInvariant(
                            composerTypes.Count(),
                            typeof(TImplementee).FullName,
                            string.Join(
                                ", ",
                                composerTypes.Select(ct => ct.FullName))));
                }
            }

            // result: composerTypeName has value, composerType == null, composerType.Full == composerTypeName
            //   1              0                       0                       0 
            //   1              0                       0                       1 N/A
            //   0              0                       1                       0 
            //   0              0                       1                       1 N/A
            //   0              1                       0                       0 
            //   1              1                       0                       1
            //   0              1                       1                       0
            //   0              1                       1                       1 N/A
            Type composerType = composerTypes.SingleOrDefault();
            bool typeOfWantedComposerFoundMatchesFoundComposer =
                (composerType != null) &&
                ((composerTypeName.IsNullOrEmpty())
                ||
                ((composerTypeName.HasContent()) && composerType.FullName == composerTypeName));

            if ((composerType == null)
                || !typeOfWantedComposerFoundMatchesFoundComposer)
                throw new InvalidOperationException("No implementation for '{0}' found.".FormatInvariant(typeof(TImplementee).FullName));

            return Activator.CreateInstance(composerType, constructorParameters) as TImplementee;
        }

        private static List<TypeInfo> AddPossibleComposerTypes<TImplementee>(IList<Assembly> assembliesToSearch, List<TypeInfo> exportedTypes,
            Dictionary<string, Type> typeNamesToExclude) where TImplementee : class
        {
            var implementeeType = typeof (TImplementee);
            foreach (var assembly in assembliesToSearch)
            {
                try
                {
                    exportedTypes.AddRange(
                        assembly.DefinedTypes
                            .Where(ti => ti.IsClass &&
                                         !ti.IsAbstract &&
                                         ti.IsPublic &&
                                         implementeeType.IsAssignableFrom(ti) &&
                                         !typeNamesToExclude.ContainsKey(ti.FullName)));
                }
                catch (ReflectionTypeLoadException)
                {
                }
                catch (FileNotFoundException)
                {
                    // This typically arises when reflection over a type causes not-deployed-dlls to be requested.
                    // E.g. Unity implements IServiceLocator, but normally doesn't use it. When we're reflecting
                    // over it, it (.NET) tries to locate the assembly to provide the information, but this fails.
                    // We allow ourselves to ignore that.
                }
            }

            var composerTypes = exportedTypes
                .Distinct()
                .ToList();
            return composerTypes;
        }
    }
}
