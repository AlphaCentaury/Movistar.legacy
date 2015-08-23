﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using Project.DvbIpTv.Common.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Project.DvbIpTv.UiServices.DvbStpClient
{
    public class DvbStpDownloadResponse
    {
        public byte Version
        {
            get;
            set;
        } // Version

        public byte[] PayloadData
        {
            get;
            set;
        } // PayloadData

        public Exception DownloadException
        {
            get;
            set;
        } // DownloadException

        public bool UserCancelled
        {
            get;
            set;
        } // UserCancelled

        public object DeserializedPayloadData
        {
            get;
            set;
        } // DeserializedPayloadData

        public static object ParsePayload(Type payloadType, byte[] payloadData, byte payloadId, bool trimExtraWhitespace, Func<string, string> namespaceReplacer)
        {
            try
            {
                using (MemoryStream input = new MemoryStream(payloadData, false))
                {
                    return XmlSerialization.Deserialize(input, payloadType, trimExtraWhitespace, namespaceReplacer);
                } // using input
            }
            catch (Exception ex)
            {
                throw new ApplicationException(string.Format(Properties.Texts.ParsePayloadException,
                    payloadId, payloadType.FullName), ex);
            } // try-catch
        } // ParsePayload<T>

        public static T ParsePayload<T>(byte[] payloadData, byte payloadId, bool trimExtraWhitespace, Func<string, string> namespaceReplacer) where T : class
        {
            return ParsePayload(typeof(T), payloadData, payloadId, trimExtraWhitespace, namespaceReplacer) as T;
        } // ParsePayload<T>
    } // class DownloadDlgResponseData
} // namespace
