﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Etsi.Ts102034.v010501.XmlSerialization.ContentGuideDiscovery
{
    /// <summary>
    /// This element is used in the first stage of service discovery. It is sent by service providers and is used as a link to their
    /// own service discovery information.
    /// </summary>
    /// <remarks>
    /// An aggregating service provide may send multiple ServiceProvider elements in a single document.
    /// If the element Offering is missing, then the ServiceProvider is not currently providing any services, but simply
    /// announcing its presence.
    /// </remarks>
    [GeneratedCode("myxsdtool", "0.0.0.0")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(AnonymousType = true, Namespace = "urn:dvb:metadata:iptv:sdns:2012-1")]
    [XmlRoot(ElementName = "ServiceDiscovery", Namespace = "urn:dvb:metadata:iptv:sdns:2012-1", IsNullable = false)]
    public partial class BroadcastContentGuideDiscoveryRoot // ServiceDiscovery
    {
        /// <summary>
        /// Version of this record. A change in this value indicates a change in one of the ServiceProviderDiscovery Records
        /// </summary>
        [XmlAttribute]
        public string Version;

        [XmlElement("BCGDiscovery")]
        public BroadcastContentGuide[] BroadcastContentGuideOffering;
    } // class BroadcastContentGuideDiscoveryRoot
} // namespace
