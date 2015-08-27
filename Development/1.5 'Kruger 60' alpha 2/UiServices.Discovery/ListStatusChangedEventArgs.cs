﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.DvbIpTv.UiServices.Discovery
{
    public class ListStatusChangedEventArgs: EventArgs
    {
        public ListStatusChangedEventArgs()
        {
            // no op
        } // constructor

        public ListStatusChangedEventArgs(bool hasItems)
        {
            HasItems = hasItems;
        } // constructor

        public bool HasItems
        {
            get;
            set;
        } // HasItems
    } // class ListStatusChangedEventArgs
} // namespace
