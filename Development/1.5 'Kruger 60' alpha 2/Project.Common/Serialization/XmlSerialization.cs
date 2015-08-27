﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Project.DvbIpTv.Common.Serialization
{
    public static class XmlSerialization
    {
        #region Serialize

        public static void Serialize(Stream output, object o)
        {
            var serializer = new XmlSerializer(o.GetType());
            serializer.Serialize(output, o);
        } // Serialize

        public static void Serialize<T>(string filename, T o)
        {
            using (var output = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                Serialize<T>(output, o);
            } // using output
        } // Serialize<T>

        public static void Serialize<T>(string filename, Encoding encoding, T o)
        {
            using (var output = new FileStream(filename, FileMode.Create, FileAccess.ReadWrite))
            {
                Serialize<T>(output, encoding, o);
            } // using output
        } // Serialize<T>

        public static void Serialize<T>(Stream output, T o)
        {
            Serialize(output, o);
        } // Serialize<T>

        public static void Serialize<T>(Stream output, Encoding encoding, T o)
        {
            var serializer = new XmlSerializer(typeof(T));
            using (var outputWriter = new StreamWriter(output, encoding))
            {
                serializer.Serialize(outputWriter, o);
            } // using outputWriter
        } // Serialize<T>

        public static void Serialize<T>(XmlWriter output, T o)
        {
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(output, o);
        } // Serialize<T>

        #endregion

        #region Deserialize

        public static T DeserializeXmlText<T>(string xmlText, bool trimExtraWhitespace = false, Func<string, string> namespaceReplacer = null) where T : class
        {
            using (var input = new StringReader(xmlText))
            {
                using (var reader = CreateXmlReader(input, trimExtraWhitespace, namespaceReplacer))
                {
                    return Deserialize<T>(reader);
                } // using reader
            } // using input
        } // Deserialize<T>

        public static T Deserialize<T>(XmlReader reader) where T : class
        {
            var serializer = new XmlSerializer(typeof(T));
            return serializer.Deserialize(reader) as T;
        } // Deserialize<T>

        public static T Deserialize<T>(string filename, bool trimExtraWhitespace = false, Func<string, string> namespaceReplacer = null) where T : class
        {
            using (var input = new FileStream(filename, FileMode.Open, FileAccess.Read))
            {
                return Deserialize<T>(input, trimExtraWhitespace, namespaceReplacer);
            } // using input
        } // XmlDeserialize

        public static T Deserialize<T>(Stream input, bool trimExtraWhitespace = false, Func<string, string> namespaceReplacer = null) where T : class
        {
            return Deserialize(input, typeof(T), trimExtraWhitespace, namespaceReplacer) as T;
        } // Deserialize<T>

        public static object Deserialize(Stream input, Type type, bool trimExtraWhitespace = false, Func<string, string> namespaceReplacer = null)
        {
            var serializer = new XmlSerializer(type);

            using (var reader = CreateXmlReader(input, trimExtraWhitespace, namespaceReplacer))
            {
                return serializer.Deserialize(reader);
            } // using reader
        } // Deserialize

        public static T Deserialize<T>(byte[] data, bool trimExtraWhitespace = false, Func<string, string> namespaceReplacer = null) where T : class
        {
            using (var input = new MemoryStream(data))
            {
                return Deserialize<T>(input, trimExtraWhitespace, namespaceReplacer);
            } // using
        } // Deserialize<T>

        public static XmlReader CreateXmlReader(Stream input, bool trimExtraWhitespace, Func<string, string> namespaceReplacer)
        {
            if (trimExtraWhitespace)
            {
                var readerSettings = new XmlReaderSettings()
                {
                    IgnoreComments = true,
                    IgnoreWhitespace = true,
                };

                return new XmlTextReaderTrimExtraWhitespace(input, readerSettings, namespaceReplacer);
            }
            else
            {
                return XmlReader.Create(input);
            } // if-else
        } // CreateXmlReader

        public static XmlReader CreateXmlReader(TextReader input, bool trimExtraWhitespace, Func<string, string> namespaceReplacer)
        {
            if (trimExtraWhitespace)
            {
                var readerSettings = new XmlReaderSettings()
                {
                    IgnoreComments = true,
                    IgnoreWhitespace = true,
                };

                return new XmlTextReaderTrimExtraWhitespace(input, readerSettings, namespaceReplacer);
            }
            else
            {
                return XmlReader.Create(input);
            } // if-else
        } // CreateXmlReader

        #endregion
    } // class XmlSerialization
} // namespace
