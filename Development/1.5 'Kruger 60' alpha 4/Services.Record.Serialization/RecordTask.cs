﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Project.IpTv.Services.Record.Serialization
{
    [Serializable]
    [XmlRoot(ElementName = "RecordTask", Namespace = RecordTask.XmlNamespace)]
    public class RecordTask
    {
        public const string XmlNamespace = "urn:Project-DvbIpTV:2014:RecordTask";

        public RecordTask()
        {
            TaskId = Guid.NewGuid();
        } // constructor

        [XmlAttribute("id")]
        public Guid TaskId
        {
            get;
            set;
        } // TaskId

        public RecordChannel Channel
        {
            get;
            set;
        } // Channel

        [XmlElement("RightNow", typeof(RecordRightNow))]
        [XmlElement("OneTimeSchedule", typeof(RecordOneTime))]
        [XmlElement("DailySchedule", typeof(RecordDaily))]
        [XmlElement("WeeklySchedule", typeof(RecordWeekly))]
        [XmlElement("MonthlySchedule", typeof(RecordMonthly))]
        public RecordSchedule Schedule
        {
            get;
            set;
        } // Schedule

        public RecordDuration Duration
        {
            get;
            set;
        } // Duration

        public RecordDescription Description
        {
            get;
            set;
        } // Description

        public RecordAction Action
        {
            get;
            set;
        } // Action

        public RecordAdvancedSettings AdvancedSettings
        {
            get;
            set;
        } // AdvancedSettings

        public static RecordTask CreateWithDefaultValues(RecordChannel channel)
        {
            RecordTask task;

            task = new RecordTask()
            {
                Channel = (channel != null) ? channel : new RecordChannel(),
                Schedule = RecordSchedule.CreateWithDefaultValues(RecordScheduleKind.OneTime),
                Duration = RecordDuration.CreateWithDefaultValues(),
                Description = RecordDescription.CreateWithDefaultValues(),
                Action = RecordAction.CreateWithDefaultValues(),
                AdvancedSettings = RecordAdvancedSettings.CreateWithDefaultValues(),
            };

            return task;
        } // CreateWithDefaultValues

        public string BuildDescription(bool pastTime)
        {
            return BuildDescription(pastTime, true, false, true, true, true);
        } // BuildDescription

        public string BuildDescription(bool pastTime, bool withBasicDescription, bool withFullDescription, bool withChannel, bool withSchedule, bool withDuration)
        {
            var buffer = new StringBuilder();
            BuildDescription(pastTime, withBasicDescription, withFullDescription, withChannel, withSchedule, withDuration, null, buffer);

            return buffer.ToString();
        } // BuildDescription

        public void BuildDescription(bool pastTime, StringBuilder buffer)
        {
            BuildDescription(pastTime, true, false, true, true, true, null, buffer);
        } // BuildDescription

        public void BuildDescription(bool pastTime, bool withBasicDescription, bool withFullDescription, bool withChannel, bool withSchedule, bool withDuration, TimeSpan? overrideTotalRecordTime, StringBuilder buffer)
        {
            if ((withBasicDescription) || (withFullDescription))
            {
                var taskFormat = withFullDescription ? Properties.Texts.BuildDescriptionTaskDescription : Properties.Texts.BuildDescriptionTaskName;
                buffer.AppendFormat(taskFormat, Description.Name, Description.Description);
                buffer.AppendLine();
            } // if

            if (withChannel)
            {
                buffer.AppendFormat(Properties.Texts.BuildDescriptionChannel,
                    Channel.LogicalNumber, Channel.Name,
                    Channel.ChannelUrl,
                    Channel.ServiceName);
                buffer.AppendLine();
            } // if withChannel

            if (withSchedule)
            {
                buffer.AppendLine(Properties.Texts.BuildDescriptionScheduleHeader);
                Schedule.Verbalize(pastTime, buffer);
                buffer.AppendLine();
            } // if withSchedule

            if (withDuration)
            {
                buffer.AppendLine(Properties.Texts.BuildDescriptionDurationHeader);
                var scheduleTime = Schedule as RecordScheduleTime;
                var startSafetyMargin = (scheduleTime != null) ? scheduleTime.SafetyMarginTimeSpan : TimeSpan.Zero;
                var endSafetyMargin = Duration.SafetyMarginTimeSpan;
                var recordDuration = Duration.Length;
                var totalRecordTime = startSafetyMargin + recordDuration + endSafetyMargin;
                if (overrideTotalRecordTime.HasValue) totalRecordTime = overrideTotalRecordTime.Value;

                var formatDuration = pastTime ? Properties.Texts.BuildDescriptionDurationPast : Properties.Texts.BuildDescriptionDuration;
                buffer.AppendFormat(formatDuration,
                    recordDuration, (int)endSafetyMargin.TotalMinutes,
                    totalRecordTime);
                buffer.AppendLine();

                if (scheduleTime != null)
                {
                    string format;
                    var schedule = (RecordScheduleTime)Schedule;
                    var startDate = schedule.StartDate - schedule.SafetyMarginTimeSpan;
                    var endDate = startDate + totalRecordTime;
                    if (startDate.Day == endDate.Day)
                    {
                        format = pastTime ? Properties.Texts.BuildDescriptionDurationEndsSameDay : Properties.Texts.BuildDescriptionDurationEndsToday;
                    }
                    else
                    {
                        format = pastTime ? Properties.Texts.BuildDescriptionDurationEndsNextDay : Properties.Texts.BuildDescriptionDurationEndsTomorrow;
                    } // if-else
                    buffer.AppendFormat(format, endDate);
                    buffer.AppendLine();
                } // if
            } // if withDuration

            // remove last CRLF
            if (buffer.Length > 2)
            {
                buffer.Remove(buffer.Length - 2, 2);
            } // if
        } // BuildDescription
    } // class RecordTask
} // namespace
