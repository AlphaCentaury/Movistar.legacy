﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Project.DvbIpTv.Core.IpTvProvider;
using Project.DvbIpTv.Core.IpTvProvider.EPG;
using Project.DvbIpTv.Services.EPG;
using Project.DvbIpTv.UiServices.Common.Forms;
using Project.DvbIpTv.UiServices.Configuration.Logos;
using Project.DvbIpTv.UiServices.Discovery;
using Project.DvbIpTv.UiServices.Discovery.BroadcastList;

namespace Project.DvbIpTv.UiServices.EPG
{
    public partial class EpgExtendedInfoDialog : Form
    {
        private Encoding Ansi1252Encoding;
        private DateTime ReferenceTime;

        public static void ShowExtendedInfo(IWin32Window owner, UiBroadcastService service, EpgEvent epgEvent)
        {
            using (var dialog = new EpgExtendedInfoDialog())
            {
                dialog.ProgramInfo = new ProgramEpgInfo()
                {
                    Service = service,
                    Base = epgEvent,
                };
                dialog.ShowDialog(owner);
            } // using dialog
        } // ShowExtendedInfo

        public EpgExtendedInfoDialog()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.Epg;
            Ansi1252Encoding = Encoding.GetEncoding(1252);
        } // constructor

        public ProgramEpgInfo ProgramInfo
        {
            get;
            set;
        } // ProgramInfo

        private void EpgExtendedInfoDialog_Load(object sender, EventArgs e)
        {
            ReferenceTime = DateTime.Now;
            buttonPrevious.Visible = false; //(NavigationCallback != null);
            buttonNext.Visible = false; // (NavigationCallback != null);

            if (ProgramInfo == null) return;

            /*
            if ((CurrentEpgInfo == null) && (NavigationCallback == null))
            {
                return;
            } // if

            if (CurrentEpgInfo == null)
            {
                CurrentEpgInfo = NavigationCallback(-1);
            } // if
            */

            pictureChannelLogo.Image = ProgramInfo.Service.Logo.GetImage(LogoSize.Size48, true);
            labelChannelName.Text = UiBroadcastListManager.GetColumnData(ProgramInfo.Service, UiBroadcastListColumn.NumberAndNameCrlf);
            richTextProgramData.Text = Properties.EpgRtf.LoadingProgramData;
            labelAdditionalDetails.Text = null;

            pictureProgramPreview.Image = Properties.Resources.EpgLoadingProgramImage;
            pictureProgramPreview.ImageLocation = null;
            pictureProgramPreview.ImageLocation = IpTvProvider.Current.EpgInfo.GetEpgProgramThumbnailUrl(ProgramInfo.Service, ProgramInfo.Base, true);

            buttonShowProgram.Enabled = false;
            buttonRecordProgram.Enabled = false;
        } // EpgExtendedInfoDialog_Load

        private void EpgExtendedInfoDialog_Shown(object sender, EventArgs e)
        {
            ProgramInfo.Extended = IpTvProvider.Current.EpgInfo.GetEpgInfo(ProgramInfo.Service, ProgramInfo.Base, true);
            if (ProgramInfo.Extended == null)
            {
                richTextProgramData.Text = Properties.EpgRtf.NullProgramData;
            }
            else
            {
                DisplayEpgInfo();
            } // if-else
        } // EpgExtendedInfoDialog_Shown

        private void DisplayEpgInfo()
        {
            StringBuilder rtf;
            
            //buttonPrevious.Enabled = CurrentEpgInfo.PreviousEnabled;
            //buttonNext.Enabled = CurrentEpgInfo.NextEnabled;

            var info = ProgramInfo.Extended;

            rtf = new StringBuilder();
            rtf.AppendLine(Properties.EpgRtf.Header);

            var title = ProgramInfo.Base.Title ?? Properties.EpgRtf.NullTitle;
            var originalTitle = info.OriginalTitle;
            if ((originalTitle != null) && (originalTitle != title))
            {
                rtf.AppendFormat(Properties.EpgRtf.ProgramTitleOriginalTitle, ToUnicodeRtf(title), ToUnicodeRtf(originalTitle));
            }
            else
            {
                rtf.AppendFormat(Properties.EpgRtf.ProgramTitle, title);
            } // if-else
            rtf.AppendLine();

            var description = Extract(info.Synopsis, Properties.EpgRtf.NullSynopsis);
            rtf.AppendFormat(Properties.EpgRtf.ProgramDescription, description);
            rtf.AppendLine();

            rtf.AppendLine(Properties.EpgRtf.ValuesHeader);
            AddValuesToRtf(rtf, Properties.EpgRtf.PropertyDirector, Properties.EpgRtf.PropertyDirectors, info.Directors);
            AddValuesToRtf(rtf, Properties.EpgRtf.PropertyActor, Properties.EpgRtf.PropertyActors, info.Actors);
            AddValuesToRtf(rtf, Properties.EpgRtf.PropertyCountry,Properties.EpgRtf.PropertyCountries, info.Country);
            AddValuesToRtf(rtf, Properties.EpgRtf.PropertyProductionDate, Properties.EpgRtf.PropertyProductionDates, info.ProductionDate);
            AddValuesToRtf(rtf, Properties.EpgRtf.PropertyProducer, Properties.EpgRtf.PropertyProducers, info.Producers);
            AddValuesToRtf(rtf, Properties.EpgRtf.PropertyScripWriter, Properties.EpgRtf.PropertyScripWriters, info.ScriptWriters);
            rtf.AppendLine(Properties.EpgRtf.ValuesFooter);

            rtf.AppendLine(Properties.EpgRtf.Footer);

            richTextProgramData.Rtf = rtf.ToString();
            richTextProgramData.ZoomFactor = 1.0f;

            labelAdditionalDetails.Text = string.Format("{0} | {1} / {2} | {3}\r\n{4:f}",
                ProgramInfo.Base.ParentalRating,
                ProgramInfo.Extended.Genre,
                ProgramInfo.Extended.SubGenre,
                ProgramInfo.Base.Duration,
                ProgramInfo.Base.LocalStartTime);

            buttonShowProgram.Enabled = (ProgramInfo.Base.LocalStartTime <= ReferenceTime) && (ProgramInfo.Base.LocalEndTime > ReferenceTime);
            buttonRecordProgram.Enabled = (ProgramInfo.Base.LocalEndTime >= ReferenceTime);
        } // DisplayEpgInfo

        private string Extract(string[] longText, string[] shortText, string defaultText)
        {
            string text;

            text = ((longText != null) && (longText.Length > 0)) ? longText[0] : null;
            if (string.IsNullOrEmpty(text)) text = null;

            if (text == null)
            {
                text = ((shortText != null) && (shortText.Length > 0)) ? shortText[0] : null;
                if (string.IsNullOrEmpty(text)) text = null;
            } // if

            return ToUnicodeRtf(text ?? defaultText);
        } // Extract

        private string Extract(string[] longText, string shortText, string defaultText)
        {
            string lngText;

            lngText = ((longText != null) && (longText.Length > 0)) ? longText[0] : null;
            if (string.IsNullOrEmpty(lngText)) lngText = null;
            var result = lngText ?? (shortText ?? defaultText);

            return ToUnicodeRtf(result);
        } // Extract

        private string Extract(string text, string defaultText)
        {
            if (string.IsNullOrEmpty(text)) text = null;
            var result = text ?? defaultText;

            return ToUnicodeRtf(result);
        } // Extract

        private string ToUnicodeRtf(string text)
        {
            if (text == null) return null;

            // quick check
            bool found = false;
            for (int index = 0; index < text.Length; index++)
            {
                if (text[index] > 127) { found = true; break; }
            } // for

            if (!found) return text;

            var buffer = new StringBuilder();
            for (int index = 0; index < text.Length; index++)
            {
                var c = text[index];
                if (c < 127)
                {
                    if (c == '\\')
                    {
                        buffer.Append("\\\\");
                    }
                    else if (c == '\n')
                    {
                        buffer.Append("\\line ");
                    }
                    else
                    {
                        buffer.Append(c);
                    }
                }
                else
                {
                    var ansiChar = Ansi1252Encoding.GetBytes(new char[] { c })[0];
                    var ansiCharRtf = (ansiChar <= 127) ? ansiChar.ToString() : string.Format("\\'{0:x0}", ansiChar);
                    buffer.AppendFormat("\\u{0}{1}", (c <= 32767) ? (int)c : ((int)c) - 32768, ansiCharRtf);
                } // if-else
            } // for

            return buffer.ToString();
        }  // ToUnicodeRtf

        private void AddValuesToRtf(StringBuilder buffer, string propertyName, string propertyNamePlural, string[] values)
        {
            if ((values == null) || (values.Length == 0)) return;

            buffer.AppendFormat(Properties.EpgRtf.Value,
                ToUnicodeRtf(values.Length == 1 ? propertyName : propertyNamePlural),
                ToUnicodeRtf(string.Join(", ", values)));
        } // AddValuesToRtf


        private void contextMenuRtf_Opening(object sender, CancelEventArgs e)
        {
            contextRtfMenuCopy.Enabled = (richTextProgramData.SelectionLength > 0);
        } // contextMenuRtf_Opening

        private void contextRtfMenuCopy_Click(object sender, EventArgs e)
        {
            richTextProgramData.Copy();
        }

        private void contextRtfMenuSelectAll_Click(object sender, EventArgs e)
        {
            richTextProgramData.SelectAll();
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            //CurrentEpgInfo = NavigationCallback(CurrentEpgInfo.Index - 1);
            DisplayEpgInfo();
        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            //CurrentEpgInfo = NavigationCallback(CurrentEpgInfo.Index + 1);
            DisplayEpgInfo();
        }

        private void buttonShowProgram_Click(object sender, EventArgs e)
        {
            ExternalTvPlayer.ShowTvChannel(this, ProgramInfo.Service);
        }

        private void buttonRecordProgram_Click(object sender, EventArgs e)
        {
            NotImplementedBox.ShowBox(this, "buttonRecordProgram");
        }

        private void pictureProgramPreview_LoadCompleted(object sender, AsyncCompletedEventArgs e)
        {
            if ((e.Error != null) || (e.Cancelled))
            {
                (sender as PictureBox).Image = Properties.Resources.EpgNoProgramImage;
            } // if
        }
    } // class 
} // namespace
