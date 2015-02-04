﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Project.DvbIpTv.Services.Record.Serialization
{
    [Serializable]
    [XmlType(Namespace = RecordTask.XmlNamespace)]
    public sealed class RecordRightNow: RecordSchedule
    {
        public override RecordScheduleKind Kind
        {
            get { return RecordScheduleKind.RightNow; }
        } // ScheduleKind

        public override void SetDefaultValues()
        {
            // nothing to initialize
        } // SetDefaultValues

        public override void Verbalize(bool pastTime, StringBuilder builder)
        {
            builder.AppendFormat(pastTime? Properties.Texts.VerbalizeRecordRightNowPast : Properties.Texts.VerbalizeRecordRightNow);
        } // Verbalize

        public override DateTime GetStartDateTime()
        {
            return DateTime.Now;
        } // GetStartDateTime

        public override TimeSpan GetSafetyMargin()
        {
            return TimeSpan.Zero;
        } // GetSafetyMargin
    } // class RecordRightNow
} // namespace
