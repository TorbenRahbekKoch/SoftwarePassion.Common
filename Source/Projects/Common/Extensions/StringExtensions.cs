using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Xml;

namespace SoftwarePassion.Common.Core.Extensions
{
    /// <summary>
    /// Extensions for the String class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Checks whether the string can be parsed as a Guid. 
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <returns>A boolean indicating whether the string could be parsed as a Guid. A null string results in false.</returns>
        public static bool IsGuid(this string value)
        {
            if (!value.HasContent())
                return false;

            Guid guid;
            return Guid.TryParse(value, out guid);
        }

        /// <summary>
        /// Returns a Uri instance parsed from the string, if the string contains a Uri-parsable value.
        /// </summary>
        /// <param name="value">The value with a Uri.</param>
        /// <returns>The parsed Uri.</returns>
        /// <exception cref="UriFormatException">If the value cannot be parsed as a Uri.</exception>
        public static Uri Uri(this string value)
        {
            return new Uri(value);
        }

        /// <summary>
        /// Checks whether the string has content and it is different from white space.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <returns>A boolean indicating whether the string has content. A null string results in false.</returns>
        public static bool HasContent(this string value)
        {
            return (value != null) && (!string.IsNullOrWhiteSpace(value));
        }

        /// <summary>
        /// Checks whether the string is null or is empty.
        /// </summary>
        /// <param name="value">The string value to check.</param>
        /// <returns>A boolean indicating whether the string is null or empty.</returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// Short cut for string.Format(CultureInfo.InvariantCulture,...)
        /// </summary>
        /// <param name="formattable">The formattable string.</param>
        /// <param name="parameters">The parameters to format.</param>
        /// <returns>A formatted string.</returns>
        /// <remarks>
        /// It is very nice to use it like this:
        /// <code>
        ///   "My {0}th formattable string on this fine {1}.".FormatInvariant(42, DateTime.Now.DayOfWeek);
        /// </code>
        /// </remarks>
        public static string FormatInvariant(this string formattable, params object[] parameters)
        {
            if (formattable == null)
                return null;

            return String.Format(CultureInfo.InvariantCulture, formattable, parameters);
        }


        /// <summary>
        /// Makes certain that all CR, CR LF and LF are replaced with Environment.NewLine.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns>A string with Environment.NewLines.</returns>
        public static string EnsureEnvironmentNewLines(this string s)
        {
            if (s == null)
                return null;

            return Regex.Replace(s, @"\r\n?|\n", Environment.NewLine);
        }

        /// <summary>
        /// Double quotes the string, escaping inner quotes at the same time.
        /// </summary>
        /// <param name="s">The string.</param>
        /// <returns></returns>
        public static string DoubleQuote(this string s)
        {
            if (s == null)
                return null;

            return "\"" + s.Replace("\"", "\\\"") + "\"";
        }

        /// <summary>
        /// Compares as XML.
        /// </summary>
        /// <param name="xml1">The XML1.</param>
        /// <param name="xml2">The XML2.</param>
        /// <returns>True if the to strings, loaded as xml, are equal.</returns>
        /// <remarks>Loads the two given strings into XmlDocument, takes InnerXml for each and compares them.</remarks>
        public static bool CompareAsXml(this string xml1, string xml2)
        {
            if (xml1 == null)
                return false;

            var xmlDoc1 = new XmlDocument();
            xmlDoc1.LoadXml(xml1);

            var xmlDoc2 = new XmlDocument();
            xmlDoc2.LoadXml(xml2);

            var formattedXml1 = xmlDoc1.InnerXml;
            var formattedXml2 = xmlDoc2.InnerXml;

            return formattedXml1 == formattedXml2;
        }

        /// <summary>
        /// Replaces the part of the string specified with index and length with the replaceValue.
        /// </summary>
        /// <param name="replacable">The replaceable.</param>
        /// <param name="index">The index of the part of the string to replace.</param>
        /// <param name="length">The length of the part of the string to replace.</param>
        /// <param name="replaceValue">The string value to replace the part of the string with.</param>
        /// <returns>The replaced string.</returns>
        public static string Replace(this string replacable, int index, int length, string replaceValue)
        {
            var endPartIndex = index + length;
            var replaced = replacable.Substring(0, index) + replaceValue;
            if (endPartIndex < replacable.Length)
                replaced += replacable.Substring(endPartIndex);
            return replaced;
        }
    }
}