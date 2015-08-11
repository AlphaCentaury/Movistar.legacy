﻿// Copyright (C) 2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using Project.DvbIpTv.Common.Analytics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Project.DvbIpTv.Tools.FirstTimeConfig
{
    public partial class WizardWelcomeDialog : Form
    {
        public WizardWelcomeDialog()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.InstallIcon;
        } // constructor

        private void WizardWelcomeDialog_Load(object sender, EventArgs e)
        {
            labelWelcomeTitle.Text = string.Format(labelWelcomeTitle.Text, Properties.Texts.ProductShortName);
            labelWelcomeText.Text = string.Format(labelWelcomeText.Text, Properties.Texts.ProductFullName);
        } // WizardWelcomeDialog_Load

        private void WizardWelcomeDialog_Shown(object sender, EventArgs e)
        {
            BringToFront();
            buttonNext.Focus();
            TopMost = false;
        }  // WizardWelcomeDialog_Shown

        private void checkAnalytics_CheckedChanged(object sender, EventArgs e)
        {
            BasicGoogleAnalytics.Init(BasicGoogleAnalytics.TrackingId, BasicGoogleAnalytics.ClientId, checkAnalytics.Checked, true, true);
            if (!checkAnalytics.Checked)
            {
                MessageBox.Show(this, Properties.Texts.AnalyticsKeepChecked, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // if
        } // checkAnalytics_CheckedChanged

        private void linkAnalyticsHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(this, Properties.Texts.AnalyticsInfo, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        } // linkAnalyticsHelp_LinkClicked
    } // class WizardWelcomeDialog
} // namespace
