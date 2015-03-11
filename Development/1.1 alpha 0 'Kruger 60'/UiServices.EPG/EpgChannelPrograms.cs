﻿using Project.DvbIpTv.Common;
using Project.DvbIpTv.Services.EPG;
using Project.DvbIpTv.Services.EPG.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Project.DvbIpTv.UiServices.EPG
{
    public partial class EpgChannelPrograms : Form
    {
        private Image EpgLoadingProgramImage;
        private Image EpgNoProgramImage;
        private Font BoldListFont, ItalicListFont;

        public Image ChannelLogo
        {
            set { pictureChannelLogo.Image = value; }
        } // ChannelLog

        public string ChannelName
        {
            get { return labelChannelName.Text; }
            set { labelChannelName.Text = value; }
        } // ChannelName

        public string FullServiceName
        {
            get;
            set;
        } // FullServiceName

        public string FullAlternateServiceName
        {
            get;
            set;
        } // FullAlternateServiceName

        public string EpgDatabase
        {
            get;
            set;
        } // EpgDatabase

        public EpgChannelPrograms()
        {
            InitializeComponent();
        } // constructor

        private void DisposeForm(bool disposing)
        {
            if (!disposing) return;

            if (EpgLoadingProgramImage != null) EpgLoadingProgramImage.Dispose();
            if (EpgNoProgramImage != null) EpgNoProgramImage.Dispose();
            if (BoldListFont != null) BoldListFont.Dispose();
            if (ItalicListFont != null) ItalicListFont.Dispose();
        } // DisposeForm

        private void EpgChannelPrograms_Load(object sender, EventArgs e)
        {
            EpgLoadingProgramImage = Properties.Resources.EpgLoadingProgramImage;
            EpgNoProgramImage = Properties.Resources.EpgNoProgramImage;
            
            comboBoxDate.SelectedIndex = 0;
            labelNowTime.Visible = false;
            labelNowDetails.Visible = false;
            labelNowTitle.Text = "Leyendo información de guía de programas";
            pictureBoxNow.Image = EpgLoadingProgramImage;
            pictureBoxNow.ErrorImage = EpgNoProgramImage;

            BoldListFont = new Font(listPrograms.Font, FontStyle.Bold);
            ItalicListFont = new Font(listPrograms.Font, FontStyle.Italic);

            ThreadPool.QueueUserWorkItem(LoadEpg);
        }

        private void listPrograms_SelectedIndexChanged(object sender, EventArgs e)
        {
            var epgEvent = (listPrograms.SelectedItems.Count != 0) ? (EpgEvent)listPrograms.SelectedItems[0].Tag : null;
            DisplayData(epgEvent, labelNowTime, labelNowTitle, labelNowDetails, pictureBoxNow, DateTime.Now);
        } // listPrograms_SelectedIndexChanged

        private void LoadEpg(object state)
        {
            var now = DateTime.Now;
            var start = new DateTime(now.Year, now.Month, now.Day);
            var end = start.AddDays(1);
            var epgEvents = EpgDbSerialization.GetServiceEvents(EpgDatabase, FullServiceName, FullAlternateServiceName, start, end);
            this.BeginInvoke(new Action<IList<EpgEvent>>(ShowEpg), epgEvents);
        } // LoadEpg

        private void ShowEpg(IList<EpgEvent> epgEvents)
        {
            EpgEvent last;
            ListViewItem item, select;

            last = null;
            select = null;
            var now = DateTime.Now;

            listPrograms.BeginUpdate();
            listPrograms.Items.Clear();

            foreach (var epgEvent in epgEvents)
            {
                if ((last != null) && (last.LocalEndTime != epgEvent.LocalStartTime))
                {
                    item = new ListViewItem(last.LocalEndTime.ToShortTimeString());
                    item.UseItemStyleForSubItems = false;
                    item.SubItems.Add("Información de programa no disponible");
                    item.SubItems[1].Font = ItalicListFont;
                }
                else
                {
                    item = new ListViewItem(epgEvent.LocalStartTime.ToShortTimeString());
                    item.UseItemStyleForSubItems = false;
                    item.Font = BoldListFont;
                    item.Tag = epgEvent;
                    item.SubItems.Add(epgEvent.Title);
                } // if-else
                listPrograms.Items.Add(item);
                last = epgEvent;

                if ((epgEvent.LocalStartTime <= now) && (epgEvent.LocalEndTime > now))
                {
                    select = item;
                } // if
            } // foreach

            if (epgEvents.Count == 0)
            {
                item = new ListViewItem("--:--");
                item.SubItems.Add("Información EPG no disponible para este canal");
                item.UseItemStyleForSubItems = false;
                listPrograms.Items.Add(item);
            } // if
            columnHeaderTitle.AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);

            if (select != null)
            {
                select.Selected = true;
                select.EnsureVisible();
                listPrograms.Focus();
            }
            else
            {
                DisplayData(null, labelNowTime, labelNowTitle, labelNowDetails, pictureBoxNow, DateTime.Now);
                if (epgEvents.Count == 0)
                {
                    labelNowTitle.Text = "Información EPG no disponible para este canal";
                } // if
            } // if-else

            listPrograms.EndUpdate();
        } // ShowEpg

        private void DisplayData(EpgEvent epg, Label time, Label title, Label details, PictureBox picture, DateTime referenceTime)
        {
            time.Visible = (epg != null);
            details.Visible = (epg != null);
            picture.ImageLocation = null;
            title.Visible = (epg != null);

            if (epg == null) return;

            title.Text = epg.Title;
            time.Text = string.Format("{0} ({1})", FormatString.DateTimeFromToMinutes(epg.LocalStartTime, epg.LocalEndTime, referenceTime),
                FormatString.TimeSpanTotalMinutes(epg.Duration, FormatString.Format.Extended));
            details.Text = string.Format("{0} / {1}", (epg.Genre != null) ? epg.Genre.Description : Properties.Texts.EpgNoGenre,
                (epg.ParentalRating != null) ? epg.ParentalRating.Description : Properties.Texts.EpgNoParentalRating);

            picture.Image = EpgLoadingProgramImage;

            // TODO: clean-up code; avoid fixed URL; allow for bad CRIDs
            try
            {
                var cridUri = new Uri(epg.CRID);
                var path = cridUri.LocalPath.Split('/');
                var crid = path[2];
                var baseCrid = crid.Substring(0, 4);

                picture.ImageLocation = string.Format("http://172.26.22.23:2001/appclient/incoming/covers/programmeImages/landscape/big/{0}/{1}.jpg", baseCrid, crid);
            }
            catch
            {
                // ignore
            } // try-catch
        }  // DisplayData
    }
}
