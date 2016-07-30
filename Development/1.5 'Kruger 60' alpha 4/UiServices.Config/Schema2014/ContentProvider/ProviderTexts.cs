﻿// Copyright (C) 2014-2016, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Project.IpTv.UiServices.Configuration.Schema2014.ContentProvider
{
    [XmlType(Namespace=SerializationCommon.XmlNamespace)]
    public class LocalizedIdentification : LocalizedObject
    {
        public string Name;
        public string Description;
    } // class LocalizedIdentification
} // namespace
