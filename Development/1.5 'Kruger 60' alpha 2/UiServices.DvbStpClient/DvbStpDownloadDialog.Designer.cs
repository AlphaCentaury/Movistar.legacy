﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

namespace Project.DvbIpTv.UiServices.DvbStpClient
{
    partial class DvbStpDownloadDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DvbStpDownloadDialog));
            this.labelDownloadingPayloadName = new System.Windows.Forms.Label();
            this.labelDownloadSource = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelProgressPct = new System.Windows.Forms.Label();
            this.labelSectionProgress = new System.Windows.Forms.Label();
            this.labelReceiving = new System.Windows.Forms.Label();
            this.labelDataReception = new System.Windows.Forms.Label();
            this.timerClose = new System.Windows.Forms.Timer(this.components);
            this.labelEllapsedTime = new System.Windows.Forms.Label();
            this.timerEllapsed = new System.Windows.Forms.Timer(this.components);
            this.buttonRequestCancel = new System.Windows.Forms.Button();
            this.pictureBox1 = new Project.DvbIpTv.UiServices.Controls.PictureBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // labelDownloadingPayloadName
            // 
            this.labelDownloadingPayloadName.AutoEllipsis = true;
            resources.ApplyResources(this.labelDownloadingPayloadName, "labelDownloadingPayloadName");
            this.labelDownloadingPayloadName.Name = "labelDownloadingPayloadName";
            // 
            // labelDownloadSource
            // 
            this.labelDownloadSource.AutoEllipsis = true;
            resources.ApplyResources(this.labelDownloadSource, "labelDownloadSource");
            this.labelDownloadSource.Name = "labelDownloadSource";
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            this.progressBar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar.UseWaitCursor = true;
            // 
            // labelProgressPct
            // 
            resources.ApplyResources(this.labelProgressPct, "labelProgressPct");
            this.labelProgressPct.Name = "labelProgressPct";
            this.labelProgressPct.UseWaitCursor = true;
            // 
            // labelSectionProgress
            // 
            resources.ApplyResources(this.labelSectionProgress, "labelSectionProgress");
            this.labelSectionProgress.Name = "labelSectionProgress";
            // 
            // labelReceiving
            // 
            resources.ApplyResources(this.labelReceiving, "labelReceiving");
            this.labelReceiving.Name = "labelReceiving";
            // 
            // labelDataReception
            // 
            resources.ApplyResources(this.labelDataReception, "labelDataReception");
            this.labelDataReception.Name = "labelDataReception";
            // 
            // timerClose
            // 
            this.timerClose.Interval = 500;
            this.timerClose.Tick += new System.EventHandler(this.timerClose_Tick);
            // 
            // labelEllapsedTime
            // 
            resources.ApplyResources(this.labelEllapsedTime, "labelEllapsedTime");
            this.labelEllapsedTime.Name = "labelEllapsedTime";
            // 
            // timerEllapsed
            // 
            this.timerEllapsed.Interval = 1000;
            this.timerEllapsed.Tick += new System.EventHandler(this.timerEllapsed_Tick);
            // 
            // buttonRequestCancel
            // 
            this.buttonRequestCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonRequestCancel.Image = global::Project.DvbIpTv.UiServices.DvbStpClient.Properties.Resources.Action_Cancel_Red_16x16;
            resources.ApplyResources(this.buttonRequestCancel, "buttonRequestCancel");
            this.buttonRequestCancel.Name = "buttonRequestCancel";
            this.buttonRequestCancel.UseVisualStyleBackColor = true;
            this.buttonRequestCancel.Click += new System.EventHandler(this.buttonRequestCancel_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Project.DvbIpTv.UiServices.DvbStpClient.Properties.Resources.InternetDownload;
            resources.ApplyResources(this.pictureBox1, "pictureBox1");
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.TabStop = false;
            // 
            // DvbStpDownloadDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonRequestCancel;
            this.ControlBox = false;
            this.Controls.Add(this.labelEllapsedTime);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.buttonRequestCancel);
            this.Controls.Add(this.labelDataReception);
            this.Controls.Add(this.labelReceiving);
            this.Controls.Add(this.labelSectionProgress);
            this.Controls.Add(this.labelProgressPct);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelDownloadSource);
            this.Controls.Add(this.labelDownloadingPayloadName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DvbStpDownloadDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.DownloadDlg_FormClosing);
            this.Load += new System.EventHandler(this.DownloadDlg_Load);
            this.Shown += new System.EventHandler(this.DownloadDlg_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label labelDownloadingPayloadName;
        private System.Windows.Forms.Label labelDownloadSource;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelProgressPct;
        private System.Windows.Forms.Label labelSectionProgress;
        private System.Windows.Forms.Label labelReceiving;
        private System.Windows.Forms.Label labelDataReception;
        private System.Windows.Forms.Button buttonRequestCancel;
        private System.Windows.Forms.Timer timerClose;
        private Project.DvbIpTv.UiServices.Controls.PictureBoxEx pictureBox1;
        private System.Windows.Forms.Label labelEllapsedTime;
        private System.Windows.Forms.Timer timerEllapsed;
    }
}