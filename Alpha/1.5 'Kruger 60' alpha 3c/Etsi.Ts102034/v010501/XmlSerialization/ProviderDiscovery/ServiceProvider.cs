﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using Etsi.Ts102034.v010501.XmlSerialization.Common;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Etsi.Ts102034.v010501.XmlSerialization.ProviderDiscovery
{
    [GeneratedCode("myxsdtool", "0.0.0.0")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(Namespace = "urn:dvb:metadata:iptv:sdns:2012-1")]
    public partial class ServiceProvider
    {
        /// <summary>
        /// Name of the Service Provider for display in one or more languages; one Service Provider name is allowed per
        /// language code, and at least one language shall be provided (though not necessarily more than one).
        /// </summary>
        [XmlElement("Name")]
        public MultilingualText[] Name;

        /// <summary>
        /// Description of the Service Provider for potential display in one or more languages; one description is allowed per language code.
        /// </summary>
        [XmlElement("Description")]
        public MultilingualText[] Description;

        /// <summary>
        /// Provides the Push and/or Pull Offering of the Service Provider
        /// </summary>
        public ProviderOffering Offering;

        [XmlAttribute]
        public string DomainName;

        [XmlAttribute]
        public string Version;

        /// <summary>
        /// Pointer to a Service Provider logo for potential display. The pointer shall be a URI
        /// </summary>
        [XmlAttribute("LogoURI", DataType = "anyURI")]
        public string LogoUrl;
    } // class ServiceProvider
} // namespace
