﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using Project.DvbIpTv.Common.Telemetry;
using Project.DvbIpTv.DvbStp.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Project.DvbIpTv.UiServices.DvbStpClient
{
    public partial class DvbStpSimpleDownloadDialog : Form
    {
        private string FormatProgressPercentage;
        private string FormatSectionProgress;
        private string FormatEllapsedTime;
        private char DataReceptionSymbol;
        private int DataReceptionCount;
        private BackgroundWorker Worker;
        private bool AllowFormToClose;
        private Action CancelDownloadRequest;
        private DateTime StartTime;

        public UiDvbStpSimpleDownloadRequest Request
        {
            get;
            set;
        } // Request

        public UiDvbStpSimpleDownloadResponse Response
        {
            get;
            private set;
        } // Response

        public DvbStpSimpleDownloadDialog()
        {
            InitializeComponent();
        } // constructor

        public DvbStpSimpleDownloadDialog(UiDvbStpSimpleDownloadRequest request)
            : this()
        {
            Request = request;
        } // constructor

        #region DownloadDlg events

        private void DownloadDlg_Load(object sender, EventArgs e)
        {
            if (Request == null) throw new ArgumentNullException();

            if (!string.IsNullOrEmpty(Request.Description))
            {
                labelDownloadingPayloadName.Text = Request.Description;
            } // if
            labelDownloadSource.Text = string.Format(labelDownloadSource.Text, Request.MulticastAddress, Request.MulticastPort);
            FormatProgressPercentage = labelProgressPct.Text;
            FormatSectionProgress = labelSectionProgress.Text;
            FormatEllapsedTime = labelEllapsedTime.Text;
            DataReceptionSymbol = labelDataReception.Text[0];

            labelProgressPct.Text = null;
            labelSectionProgress.Text = null;
            labelDataReception.Text = null;
            labelReceiving.Visible = false;
            labelEllapsedTime.Text = null;

            Response = new UiDvbStpSimpleDownloadResponse();
        }  // DownloadDlg_Load

        private void DownloadDlg_Shown(object sender, EventArgs e)
        {
            StartDownload();
            
            var screen = string.Format("{0}: {1}:{2} 0x{3:X2}", this.GetType().Name, Request.MulticastAddress, Request.MulticastPort, Request.PayloadId);
            BasicGoogleTelemetry.SendScreenHit(screen);
        } // DownloadDlg_Shown

        private void DownloadDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if ((e.CloseReason != CloseReason.UserClosing) && (e.CloseReason != CloseReason.None)) return;

            if (AllowFormToClose) return;

            e.Cancel = true;
            CancelDownload();
        } // DownloadDlg_FormClosing

        #endregion

        #region Controls events

        private void buttonRequestCancel_Click(object sender, EventArgs e)
        {
            // race condition
            if (CancelDownloadRequest == null)
            {
                MessageBox.Show(this, Properties.Texts.UnableCancelDownloadCaption,
                    Properties.Texts.UnableCancelDownloadMessage,
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            } // if

            CancelDownload();
        } // buttonRequestCancel_Click

        private void timerEllapsed_Tick(object sender, EventArgs e)
        {
            DisplayEllapsedTime();
        } // timerEllapsed_Tick

        private void timerClose_Tick(object sender, EventArgs e)
        {
            CloseForm();
        } // timerClose_Tick

        #endregion

        private void StartDownload()
        {
            StartTime = DateTime.Now;
            timerEllapsed.Enabled = true;
            DisplayEllapsedTime();

            Worker = new BackgroundWorker();
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;
            Worker.ProgressChanged += Worker_ProgressChanged;
            Worker.RunWorkerCompleted += Worker_RunWorkerCompleted;
            Worker.DoWork += Worker_DoWork;
            Worker.RunWorkerAsync(Thread.CurrentThread);
        } // StartDownload

        private void CancelDownload()
        {
            buttonRequestCancel.Enabled = false;
            labelDownloadingPayloadName.Text = Properties.Texts.CancellingDownloadOperation;

            Response.UserCancelled = true;
            Worker.CancelAsync();
            CancelDownloadRequest();
        } // CancelDownload

        private void StartClose()
        {
            if (Request.DialogCloseDelay <= 0) CloseForm();

            timerClose.Interval = Request.DialogCloseDelay;
            timerClose.Enabled = true;
        } // private StartClose

        private void CloseForm()
        {
            timerClose.Enabled = false;
            AllowFormToClose = true;
            Close();
        } // CloseForm

        private void DisplayEllapsedTime()
        {
            TimeSpan ellapsed = DateTime.Now - StartTime;
            TimeSpan ellapsedRounded = new TimeSpan(ellapsed.Days, ellapsed.Hours, ellapsed.Minutes, ellapsed.Seconds);

            labelEllapsedTime.Text = string.Format(FormatEllapsedTime, ellapsedRounded);
        } // DisplayEllapsedTime

        #region Worker events

        void Worker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            timerEllapsed.Enabled = false;
            Worker.Dispose();
            Worker = null;

            Response.UserCancelled = e.Cancelled;
            Response.DownloadException = e.Error;
            if ((!e.Cancelled) && (e.Error == null))
            {
                Response.PayloadData = e.Result as byte[];
            } // if

            StartClose();
        } // Worker_RunWorkerCompleted

        private void Worker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage < 0)
            {
                DisplayDataReception(e);
            }
            else if (e.ProgressPercentage < int.MaxValue)
            {
                DisplaySectionReception(e);
            }
            else
            {
                DisplayParsingData(e);
            } // if-else
        } // Worker_ProgressChanged

        private void DisplaySectionReception(ProgressChangedEventArgs e)
        {
            if (progressBar.Style == ProgressBarStyle.Marquee) progressBar.Style = ProgressBarStyle.Continuous;

            var sectionReceivedArgs = e.UserState as DvbStpSimpleClient.PayloadSectionReceivedEventArgs;
            labelProgressPct.Text = string.Format(FormatProgressPercentage, e.ProgressPercentage / 1000.0);
            labelSectionProgress.Text = string.Format(FormatSectionProgress, sectionReceivedArgs.SectionsReceived, sectionReceivedArgs.SectionCount);
            progressBar.Value = e.ProgressPercentage / 10;
        } // DisplaySectionReception

        private void DisplayDataReception(ProgressChangedEventArgs e)
        {
            DataReceptionCount++;
            labelReceiving.Visible = true;
            labelDataReception.Text = new string(DataReceptionSymbol, (DataReceptionCount % 6));
        }  // DisplayDataReception

        private void DisplayParsingData(ProgressChangedEventArgs e)
        {
            labelDownloadingPayloadName.Text = Request.DescriptionParsing;
            labelReceiving.Text = Properties.Texts.CompletedDownloadDataReception;
            labelReceiving.Font = new Font(labelReceiving.Font, FontStyle.Bold);
            labelDataReception.Text = null;
            labelDownloadSource.Visible = false;
        } // DisplayParsingData

        #endregion

        #region BackgroundWorker DoWork

        void Worker_DoWork(object sender, DoWorkEventArgs e)
        {
            DvbStpSimpleClient stpClient;
            byte[] payload;

            InitWorker(e);

            stpClient = new DvbStpSimpleClient(Request.MulticastAddress, Request.MulticastPort);
            CancelDownloadRequest = stpClient.CancelRequest;
            try
            {
                stpClient.SectionReceived += StpClient_SectionReceived;
                stpClient.PayloadSectionReceived += StpClient_PayloadSectionReceived;

                payload = stpClient.GetPayload(Request.PayloadId, Request.SegmentId);
                Response.Version = stpClient.SegmentVersion;
                e.Result = payload;
#if DEBUG
                if ((payload != null) && (Request.DumpToFile != null))
                {
                    File.WriteAllBytes(Request.DumpToFile, payload);
                } // if
#endif
                if ((Request.PayloadDataType != null) && (payload != null))
                {
                    Worker.ReportProgress(int.MaxValue);
                    Response.DeserializedPayloadData = UiDvbStpSimpleDownloadResponse.ParsePayload(Request.PayloadDataType, payload, Request.PayloadId, !Request.AllowXmlExtraWhitespace, Request.XmlNamespaceReplacer);
                } // if
            }
            finally
            {
                e.Cancel = Worker.CancellationPending;
                stpClient.Close();
            } // finally
        } // Worker_DoWork

        private void InitWorker(DoWorkEventArgs e)
        {
            // set worker thread name (for debugging purposes)
            var currentThread = Thread.CurrentThread;
            currentThread.Name = "DvbStpSimpleDownloadDialog BackgroundWorker";

            // inherit parent thead culture settings
            var parentThread = e.Argument as Thread;
            if (parentThread != null)
            {
                currentThread.CurrentCulture = parentThread.CurrentCulture; // matches regular application Culture; set again just-in-case
                currentThread.CurrentUICulture = parentThread.CurrentUICulture; // UICulture not inherited from spwawning thread
            } // if
        } // InitWorker

#endregion

        #region StpClient event handlers

        void StpClient_SectionReceived(object sender, DvbStpSimpleClient.SectionReceivedEventArgs e)
        {
            Worker.ReportProgress(-1, e);
        } // StpClient_SectionReceived

        void StpClient_PayloadSectionReceived(object sender, DvbStpSimpleClient.PayloadSectionReceivedEventArgs e)
        {
            Worker.ReportProgress((e.SectionsReceived * 1000) / e.SectionCount, e);
        } // StpClient_PayloadSectionReceived

        #endregion
    } // class DvbStpSimpleDownloadDialog
} // namespace
