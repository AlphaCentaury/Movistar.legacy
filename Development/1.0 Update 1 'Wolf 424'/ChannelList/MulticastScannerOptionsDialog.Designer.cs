﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

namespace Project.DvbIpTv.ChannelList
{
    partial class MulticastScannerOptionsDialog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MulticastScannerOptionsDialog));
            this.labelCaption = new System.Windows.Forms.Label();
            this.labelScanTimeout = new System.Windows.Forms.Label();
            this.numericTimeout = new System.Windows.Forms.NumericUpDown();
            this.buttonRequestCancel = new System.Windows.Forms.Button();
            this.buttonStart = new System.Windows.Forms.Button();
            this.groupScanWhat = new System.Windows.Forms.GroupBox();
            this.radioScanDead = new System.Windows.Forms.RadioButton();
            this.radioScanActive = new System.Windows.Forms.RadioButton();
            this.radioScanAll = new System.Windows.Forms.RadioButton();
            this.groupActionScan = new System.Windows.Forms.GroupBox();
            this.radioActionDelete = new System.Windows.Forms.RadioButton();
            this.radioActionDisable = new System.Windows.Forms.RadioButton();
            this.labelInfo = new System.Windows.Forms.Label();
            this.pictureIcon = new Project.DvbIpTv.UiServices.Controls.PictureBoxEx();
            ((System.ComponentModel.ISupportInitialize)(this.numericTimeout)).BeginInit();
            this.groupScanWhat.SuspendLayout();
            this.groupActionScan.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // labelCaption
            // 
            resources.ApplyResources(this.labelCaption, "labelCaption");
            this.labelCaption.Name = "labelCaption";
            // 
            // labelScanTimeout
            // 
            resources.ApplyResources(this.labelScanTimeout, "labelScanTimeout");
            this.labelScanTimeout.Name = "labelScanTimeout";
            // 
            // numericTimeout
            // 
            resources.ApplyResources(this.numericTimeout, "numericTimeout");
            this.numericTimeout.DecimalPlaces = 3;
            this.numericTimeout.Increment = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            this.numericTimeout.Maximum = new decimal(new int[] {
            30,
            0,
            0,
            0});
            this.numericTimeout.Minimum = new decimal(new int[] {
            250,
            0,
            0,
            196608});
            this.numericTimeout.Name = "numericTimeout";
            this.numericTimeout.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // buttonRequestCancel
            // 
            resources.ApplyResources(this.buttonRequestCancel, "buttonRequestCancel");
            this.buttonRequestCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.buttonRequestCancel.Image = global::Project.DvbIpTv.ChannelList.Properties.Resources.ActionCancel_16x16;
            this.buttonRequestCancel.Name = "buttonRequestCancel";
            this.buttonRequestCancel.UseVisualStyleBackColor = true;
            // 
            // buttonStart
            // 
            resources.ApplyResources(this.buttonStart, "buttonStart");
            this.buttonStart.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonStart.Image = global::Project.DvbIpTv.ChannelList.Properties.Resources.ActionRun_16x16;
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // groupScanWhat
            // 
            resources.ApplyResources(this.groupScanWhat, "groupScanWhat");
            this.groupScanWhat.Controls.Add(this.radioScanDead);
            this.groupScanWhat.Controls.Add(this.radioScanActive);
            this.groupScanWhat.Controls.Add(this.radioScanAll);
            this.groupScanWhat.Name = "groupScanWhat";
            this.groupScanWhat.TabStop = false;
            // 
            // radioScanDead
            // 
            resources.ApplyResources(this.radioScanDead, "radioScanDead");
            this.radioScanDead.Name = "radioScanDead";
            this.radioScanDead.UseVisualStyleBackColor = false;
            // 
            // radioScanActive
            // 
            resources.ApplyResources(this.radioScanActive, "radioScanActive");
            this.radioScanActive.Name = "radioScanActive";
            this.radioScanActive.UseVisualStyleBackColor = false;
            // 
            // radioScanAll
            // 
            resources.ApplyResources(this.radioScanAll, "radioScanAll");
            this.radioScanAll.Checked = true;
            this.radioScanAll.Name = "radioScanAll";
            this.radioScanAll.TabStop = true;
            this.radioScanAll.UseVisualStyleBackColor = false;
            // 
            // groupActionScan
            // 
            resources.ApplyResources(this.groupActionScan, "groupActionScan");
            this.groupActionScan.Controls.Add(this.radioActionDelete);
            this.groupActionScan.Controls.Add(this.radioActionDisable);
            this.groupActionScan.Name = "groupActionScan";
            this.groupActionScan.TabStop = false;
            // 
            // radioActionDelete
            // 
            resources.ApplyResources(this.radioActionDelete, "radioActionDelete");
            this.radioActionDelete.Name = "radioActionDelete";
            this.radioActionDelete.UseVisualStyleBackColor = true;
            // 
            // radioActionDisable
            // 
            resources.ApplyResources(this.radioActionDisable, "radioActionDisable");
            this.radioActionDisable.Checked = true;
            this.radioActionDisable.Name = "radioActionDisable";
            this.radioActionDisable.TabStop = true;
            this.radioActionDisable.UseVisualStyleBackColor = true;
            // 
            // labelInfo
            // 
            resources.ApplyResources(this.labelInfo, "labelInfo");
            this.labelInfo.Name = "labelInfo";
            // 
            // pictureIcon
            // 
            resources.ApplyResources(this.pictureIcon, "pictureIcon");
            this.pictureIcon.Image = global::Project.DvbIpTv.ChannelList.Properties.Resources.ScanTv_128x128;
            this.pictureIcon.Name = "pictureIcon";
            this.pictureIcon.TabStop = false;
            // 
            // MulticastScannerOptionsDialog
            // 
            this.AcceptButton = this.buttonStart;
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonRequestCancel;
            this.Controls.Add(this.labelInfo);
            this.Controls.Add(this.groupActionScan);
            this.Controls.Add(this.groupScanWhat);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonRequestCancel);
            this.Controls.Add(this.numericTimeout);
            this.Controls.Add(this.labelScanTimeout);
            this.Controls.Add(this.labelCaption);
            this.Controls.Add(this.pictureIcon);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MulticastScannerOptionsDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Load += new System.EventHandler(this.MulticastScannerOptionsDialog_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericTimeout)).EndInit();
            this.groupScanWhat.ResumeLayout(false);
            this.groupScanWhat.PerformLayout();
            this.groupActionScan.ResumeLayout(false);
            this.groupActionScan.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Project.DvbIpTv.UiServices.Controls.PictureBoxEx pictureIcon;
        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.Label labelScanTimeout;
        private System.Windows.Forms.NumericUpDown numericTimeout;
        private System.Windows.Forms.Button buttonRequestCancel;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.GroupBox groupScanWhat;
        private System.Windows.Forms.RadioButton radioScanDead;
        private System.Windows.Forms.RadioButton radioScanActive;
        private System.Windows.Forms.RadioButton radioScanAll;
        private System.Windows.Forms.GroupBox groupActionScan;
        private System.Windows.Forms.RadioButton radioActionDelete;
        private System.Windows.Forms.RadioButton radioActionDisable;
        private System.Windows.Forms.Label labelInfo;
    }
}