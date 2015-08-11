﻿// Copyright (C) 2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Project.DvbIpTv.UiServices.Configuration.Schema2014.Config
{
    [Serializable]
    [XmlType(TypeName = "EpgConfig", Namespace = ConfigCommon.ConfigXmlNamespace)]
    public class EpgConfig
    {
        public EpgConfig()
        {
            // no op; default constructor
        } // constructor

        public EpgConfig(bool enabled, int autoUpdateHours, int maxDays)
        {
            Enabled = enabled;
            AutoUpdateHours = autoUpdateHours;
            MaxDays = maxDays;
        } // constructor

        public bool Enabled
        {
            get;
            set;
        } // Enabled

        public int AutoUpdateHours
        {
            get;
            set;
        } // AutoUpdateHours

        public int MaxDays
        {
            get;
            set;
        } // MaxDays
    } // class EpgConfig
} // namespace
