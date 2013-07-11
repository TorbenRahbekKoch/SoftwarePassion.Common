using System;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;

namespace SoftwarePassion.Common.Core.Serialization
{
    /// <summary>
    /// One-liners for the DataContractSerializer class.
    /// </summary>
    public static class DataContractSerialization
    {
        /// <summary>
        /// Serializes the specified value.
        /// </summary>
        /// <typeparam name="TType">The type of the value.</typeparam>
        /// <param name="value">The value.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns>A string (xml) with the serialized value.</returns>
        public static string Serialize<TType>(TType value, params Type[] knownTypes)
        {
            if (knownTypes == null)
            {
                knownTypes = new Type[] { };
            }

            var serializer = new DataContractSerializer(typeof(TType), knownTypes);
            var stringBuilder = new StringBuilder(1024);
            using (var stream = XmlWriter.Create(stringBuilder))
            {
                serializer.WriteObject(stream, value);
                stream.Flush();
            }

            return stringBuilder.ToString();
        }

        /// <summary>
        /// Deserializes the specified XML into an instance of the given TType.
        /// </summary>
        /// <typeparam name="TType">The type of the data expected to have be serialized in the given xml.</typeparam>
        /// <param name="xml">The XML.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns>A deserialized instance of the given TType.</returns>
        public static TType Deserialize<TType>(string xml, params Type[] knownTypes) where TType : class
        {
            if (string.IsNullOrEmpty(xml))
                return null;

            if (knownTypes == null)
            {
                knownTypes = new Type[] { };
            }

            var serializer = new DataContractSerializer(typeof(TType), knownTypes);

            using (var reader = new StringReader(xml))
            {
                var stream = XmlReader.Create(reader);
                return serializer.ReadObject(stream) as TType;
            }
        }

        /// <summary>
        /// Deserializes the specified XML into an instance of the given dataType.
        /// </summary>
        /// <param name="dataType">The type of the data expected to have be serialized in the given xml.</param>
        /// <param name="xml">The XML.</param>
        /// <param name="knownTypes">The known types.</param>
        /// <returns> A deserialized instance of the given dataType. </returns>
        public static object Deserialize(Type dataType, string xml, params Type[] knownTypes)
        {
            if (string.IsNullOrEmpty(xml))
                return null;

            if (knownTypes == null)
            {
                knownTypes = new Type[] { };
            }

            var serializer = new DataContractSerializer(dataType, knownTypes);

            using (var reader = new StringReader(xml))
            {
                var stream = XmlReader.Create(reader);
                return serializer.ReadObject(stream);
            }
        }
    }
}