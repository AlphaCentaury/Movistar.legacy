﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Project.IpTv.UiServices.Discovery.BroadcastList
{
    [Serializable]
    public struct UiBroadcastListSortColumn
    {
        public UiBroadcastListSortColumn(UiBroadcastListColumn column, bool descending)
            : this()
        {
            Column = column;
            Descending = (column == UiBroadcastListColumn.None)? false : descending;
        } // constructor

        [XmlAttribute("name")]
        public UiBroadcastListColumn Column
        {
            get;
            set;
        } // Column

        [DefaultValue(false)]
        [XmlAttribute("descending")]
        public bool Descending
        {
            get;
            set;
        } // Descending

        [XmlIgnore]
        public bool IsAscending
        {
            get { return !Descending; }
            set { Descending = !value; }
        } // IsAscending

        public override bool Equals(object obj)
        {
            // if parameter is null return false
            if (object.ReferenceEquals(obj, null)) return false;

            try
            {
                // return true if the fields match
                return this.Equals((UiBroadcastListSortColumn)obj);
            }
            catch (InvalidCastException)
            {
                // if parameter cannot be cast to this type return false
                return false;
            } // try-catch
        } // Equals

        public bool Equals(UiBroadcastListSortColumn column)
        {
            if (this.Column != column.Column) return false;
            if (this.IsAscending == column.IsAscending) return true;
            // special case
            if (this.Column == UiBroadcastListColumn.None) return true;

            return false;
        } // Equals

        public override int GetHashCode()
        {
            return IsAscending ? (int)Column : -1 * (int)Column;
        } // GetHashCode

        public static bool operator == (UiBroadcastListSortColumn column1, UiBroadcastListSortColumn column2)
        {
            return column1.Equals(column2);
        } // operator ==

        public static bool operator !=(UiBroadcastListSortColumn column1, UiBroadcastListSortColumn column2)
        {
            return !column1.Equals(column2);
        } // operator !=
    } // class UiBroadcastListViewSortColumn
} // namespace
