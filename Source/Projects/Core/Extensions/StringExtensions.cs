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
        /// Short cut for string.Format(CultureInfo.InvariantCulture,...)
        /// </summary>
        /// <param name="formattable">The formattable string.</param>
        /// <param name="parameters">The parameters to format.</param>
        /// <returns>A formatted string.</returns>
        /// <remarks>
        /// It is very nice to use it like this:
        /// <code>
        ///   "My {0}th formattable string on this fine {1}.".FormatInvaraint(42, DateTime.Now.DayOfWeek);
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
        /// A fixed implementation of a string Hash, since the built-in string.GetHashCode() cannot be expected
        /// to be the same between framework versions.
        /// </summary>
        /// <param name="value"></param>
        /// <returns>The hash value.</returns>
        public static int Hash(this string value)
        {
            if (value == null) throw new ArgumentNullException("value");

            int num1 = 352654597;
            int num2 = num1;
            int index = 0;
            int numPtr0 = 0;
            int length = value.Length;
            while (length > 2)
            {
                numPtr0 = value[index] + (value[index + 1] << 16);
                int numPtr1;
                if (index + 3 < value.Length)
                    numPtr1 = value[index + 2] + (value[index + 3] << 16);
                else
                    numPtr1 = value[index + 2];
                num1 = (num1 << 5) + num1 + (num1 >> 27) ^ numPtr0;
                num2 = (num2 << 5) + num2 + (num2 >> 27) ^ numPtr1;
                index += 4;
                length -= 4;
            }
            if (length > 0)
            {
                if (length == 1)
                    numPtr0 = value[index];
                else
                    numPtr0 = value[index] + (value[index + 1] << 16);
                num1 = (num1 << 5) + num1 + (num1 >> 27) ^ numPtr0;
            }
            return num1 + num2 * 1566083941;
        }
    }
}