﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using Etsi.Ts102034.v010501.XmlSerialization.Common;
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
    [XmlType(AnonymousType = true, Namespace = "urn:dvb:metadata:iptv:sdns:2012-1")]
    public partial class MosaicDescriptionLogicalCell
    {
        [XmlElement("ElementaryCell")]
        public MosaicDescriptionElementaryCell[] ElementaryCell;

        [XmlElement("AudioLink")]
        public MosaicDescriptionAudioLink[] AudioLink;

        [XmlElement("DVBTriplet", typeof(DvbTriplet))]
        public DvbTriplet DvbTriplet;

        [XmlElement("PackageId", typeof(MosaicDescriptionPackageId))]
        public MosaicDescriptionPackageId PackageId;

        [XmlElement("TextualId", typeof(TextualIdentifier))]
        public TextualIdentifier TextualId;

        [XmlAttribute]
        public ushort CellId;

        [XmlAttribute]
        public string PresentationInfo;

        [XmlAttribute]
        public string LinkageInfo;

        [XmlAttribute]
        public string EventId;
    } // class MosaicDescriptionLogicalCell
} // namespace
