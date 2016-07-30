﻿// Copyright (C) 2014-2016, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Etsi.Ts102034.v010501.XmlSerialization.BroadcastDiscovery
{
    [GeneratedCode("myxsdtool", "0.0.0.0")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType(TypeName="FecLayerAddress", Namespace = "urn:dvb:metadata:iptv:sdns:2012-1")]
    public partial class FecLayerAddress
    {
        [XmlAttribute]
        public string Address;

        [XmlAttribute]
        public string Source;

        [XmlAttribute]
        public ushort Port;

        [XmlIgnore]
        public bool PortSpecified;

        [XmlAttribute(DataType = "positiveInteger")]
        public string MaxBitrate;

        [XmlAttribute("RTSPControlURL", DataType = "anyURI")]
        public string RtspControlUrl;

        [XmlAttribute("PayloadTypeNumber")]
        public uint PayloadTypeNumber;

        [XmlIgnore]
        public bool PayloadTypeNumberSpecified;

        [XmlAttribute("TransportProtocol")]
        public TransportProtocolKind TransportProtocol;

        [XmlIgnore]
        public bool TransportProtocolSpecified;
    } // class FecLayerAddress
} // namespace
