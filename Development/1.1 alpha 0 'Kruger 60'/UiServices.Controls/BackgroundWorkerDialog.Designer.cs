﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

namespace Project.DvbIpTv.UiServices.Controls
{
    partial class BackgroundWorkerDialog
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
            if (worker != null) worker.Dispose();

            if (disposing && (components != null))
            {
                components.Dispose();
            } // if
            base.Dispose(disposing);
        } // Dispose

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackgroundWorkerDialog));
            this.pictureWaitIcon = new System.Windows.Forms.PictureBox();
            this.labelTaskDescription = new System.Windows.Forms.Label();
            this.progressBar = new System.Windows.Forms.ProgressBar();
            this.labelProgressText = new System.Windows.Forms.Label();
            this.buttonClose = new System.Windows.Forms.Button();
            this.buttonRequestCancel = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureWaitIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureWaitIcon
            // 
            this.pictureWaitIcon.Image = global::Project.DvbIpTv.UiServices.Controls.Properties.SharedResources.WaitClock_64x64;
            resources.ApplyResources(this.pictureWaitIcon, "pictureWaitIcon");
            this.pictureWaitIcon.Name = "pictureWaitIcon";
            this.pictureWaitIcon.TabStop = false;
            // 
            // labelTaskDescription
            // 
            resources.ApplyResources(this.labelTaskDescription, "labelTaskDescription");
            this.labelTaskDescription.Name = "labelTaskDescription";
            // 
            // progressBar
            // 
            resources.ApplyResources(this.progressBar, "progressBar");
            this.progressBar.Name = "progressBar";
            // 
            // labelProgressText
            // 
            resources.ApplyResources(this.labelProgressText, "labelProgressText");
            this.labelProgressText.Name = "labelProgressText";
            // 
            // buttonClose
            // 
            this.buttonClose.DialogResult = System.Windows.Forms.DialogResult.OK;
            resources.ApplyResources(this.buttonClose, "buttonClose");
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // buttonRequestCancel
            // 
            this.buttonRequestCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            resources.ApplyResources(this.buttonRequestCancel, "buttonRequestCancel");
            this.buttonRequestCancel.Name = "buttonRequestCancel";
            this.buttonRequestCancel.UseVisualStyleBackColor = true;
            this.buttonRequestCancel.Click += new System.EventHandler(this.buttonRequestCancel_Click);
            // 
            // BackgroundWorkerDialog
            // 
            this.AcceptButton = this.buttonClose;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonRequestCancel;
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonRequestCancel);
            this.Controls.Add(this.labelProgressText);
            this.Controls.Add(this.progressBar);
            this.Controls.Add(this.labelTaskDescription);
            this.Controls.Add(this.pictureWaitIcon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "BackgroundWorkerDialog";
            this.ShowInTaskbar = false;
            this.TopMost = true;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BackgroundWorkerDialog_FormClosing);
            this.Load += new System.EventHandler(this.BackgroundWorkerDialog_Load);
            this.Shown += new System.EventHandler(this.BackgroundWorkerDialog_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureWaitIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureWaitIcon;
        private System.Windows.Forms.Label labelTaskDescription;
        private System.Windows.Forms.ProgressBar progressBar;
        private System.Windows.Forms.Label labelProgressText;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.Button buttonRequestCancel;
    }
}