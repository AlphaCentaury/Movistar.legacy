﻿// Copyright (C) 2014-2016, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using Etsi.Ts102034.v010501.XmlSerialization.Common;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace Etsi.Ts102034.v010501.XmlSerialization.PackageDiscovery
{
    [GeneratedCode("myxsdtool", "0.0.0.0")]
    [Serializable]
    [DebuggerStepThrough]
    [DesignerCategory("code")]
    [XmlType("PackagedServiceType", Namespace = "urn:dvb:metadata:iptv:sdns:2012-1")]
    public partial class PackagedService
    {
        [XmlElement("TextualID")]
        public PackageTextualIdentifier[] TextualIdentifiers;

        [XmlElement("DVBTriplet")]
        public DvbTriplet DvbTriplet;

        public DescriptionLocation DescriptionLocation;

        [XmlElement(DataType = "positiveInteger")]
        public string LogicalChannelNumber;
    } // class PackagedService
} // namespace
