using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Globalization;
using System.Linq;
using SoftwarePassion.Common.Core.Extensions;

namespace SoftwarePassion.Common.Core.Utilities
{
    /// <summary>
    /// Highly configurable parser for command line options.
    /// Currently it supports command lines where the option name and the value is separated by space only.
    /// </summary>
    public class CommandLineOptionsParser
    {
        /// <summary>
        /// Constructs the parser with the given parameters.
        /// </summary>
        /// <param name="commandLine">The commandLine as given to Main().</param>
        /// <param name="allowedOptions">The names of the allowed options.</param>
        /// <param name="optionPrefixes">The allowed option prefixes, e.g. - or /</param>
        public CommandLineOptionsParser(
            IEnumerable<string> commandLine, 
            IEnumerable<string> allowedOptions, 
            IEnumerable<string> optionPrefixes)
        {
            Contract.Requires(commandLine != null);
            Contract.Requires(allowedOptions != null);
            Contract.Requires(optionPrefixes != null);

            this.commandLine = new List<string>(commandLine);
            this.allowedOptions = new List<string>(allowedOptions);
            this.optionPrefixes = new List<string>(optionPrefixes);
            
            ProcessCommandLine();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineOptionsParser"/> class. In this
        /// instance of the parser the list of allowed option prefixes is empty. So option prefixes
        /// are not allowed.
        /// </summary>
        /// <param name="commandLine">The commandLine as given to Main().</param>
        /// <param name="allowedOptions">The names of the allowed options.</param>
        public CommandLineOptionsParser(IEnumerable<string> commandLine, IEnumerable<string> allowedOptions)
            : this(commandLine, allowedOptions, new string[] {})
        {
            Contract.Requires(commandLine != null);
            Contract.Requires(allowedOptions != null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandLineOptionsParser"/> class. In this
        /// instance of the parser the list of allowed option prefixes, as well as the list of allowed
        /// options is empty. This means that there is no restrictions at all on the naming of the options.
        /// </summary>
        /// <param name="commandLine">The commandLine as given to Main().</param>
        public CommandLineOptionsParser(IEnumerable<string> commandLine)
            : this(commandLine, new string[] {}, new string[] {})
        {
            Contract.Requires(commandLine != null);
        }

        /// <summary>
        /// Gets the value of the option with the given name.
        /// </summary>
        /// <param name="optionName">Name of the option.</param>
        /// <returns>The value, if it exists. Otherwise an IndexOutOfRangeException is thrown.</returns>
        /// <exception cref="System.IndexOutOfRangeException">When the given optionName does not exist.</exception>
        public string GetValue(string optionName)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(optionName));

            string value;
            if (!commandLineOptions.TryGetValue(optionName, out value))
                throw  new IndexOutOfRangeException("Option {0} does not exist.".FormatInvariant(optionName));

            return commandLineOptions[optionName];
        }

        /// <summary>
        /// Gets the value of the option with the given name. Returns the given defaultValue if the option does not exist.
        /// </summary>
        /// <param name="optionName">Name of the option.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        public string GetValue(string optionName, string defaultValue)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(optionName));

            string optionValue;
            if (!commandLineOptions.TryGetValue(optionName, out optionValue))
                return defaultValue;

            return optionValue;
        }

        private void ProcessCommandLine()
        {
            for (int index = 0; index < commandLine.Count; index += 2)
            {
                if (index + 1 >= commandLine.Count)
                    throw new IndexOutOfRangeException("Option {0} has no value.".FormatInvariant(commandLine[index]));

                string option = commandLine[index];
                string value = commandLine[index + 1];
                ProcessOption(option, value);
            }
        }

        private void ProcessOption(string option, string value)
        {
            Contract.Requires(!string.IsNullOrWhiteSpace(option));

            var actualOption = option;
            foreach (var prefix in optionPrefixes)
            {
                if (actualOption.IndexOf(prefix, StringComparison.OrdinalIgnoreCase) == 0)
                {
                    actualOption = actualOption.Substring(prefix.Length);
                    
                }
            }

            // If the list of allowed options is empty then any option is allowed.
            if (allowedOptions.Any() && !allowedOptions.Contains(actualOption))
                throw  new IndexOutOfRangeException("Option {0} is not allowed.".FormatInvariant(option));
            
            commandLineOptions[actualOption] = value;
        }

        [ContractInvariantMethod]
        private void ObjectInvariant()
        {
            Contract.Invariant(commandLineOptions!= null);
        }

        private readonly Dictionary<string, string> commandLineOptions = new Dictionary<string, string>();
        private readonly List<string> allowedOptions;
        private readonly List<string> commandLine;
        private readonly List<string> optionPrefixes;
    }
}
