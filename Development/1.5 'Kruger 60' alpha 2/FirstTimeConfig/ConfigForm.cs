﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using Project.DvbIpTv.Common.Telemetry;
using Project.DvbIpTv.Tools.FirstTimeConfig.Properties;
using Project.DvbIpTv.UiServices.Configuration;
using Project.DvbIpTv.UiServices.Configuration.Schema2014.Config;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Project.DvbIpTv.Tools.FirstTimeConfig
{
    public partial class ConfigForm : Form
    {
        private string defaultSavePath;
        private bool IsFormAllowedToClose;
        
        public ConfigForm()
        {
            InitializeComponent();
            this.Icon = Properties.Resources.InstallIcon;
            wizardControl.LabelTitle = labelStepTitle;
            wizardControl.PreviousButton = buttonPreviousPage;
            wizardControl.NextButton = buttonNextPage;
            wizardControl.IsPageAllowed[wizardPage1.Name] = true;
            wizardControl.SelectedIndexChanged += WizardControl_SelectedIndexChanged;
        } // constructor

        private void ConfigForm_Load(object sender, EventArgs e)
        {
            BasicGoogleTelemetry.SendScreenHit(this);

            selectFolder.Description = Properties.Texts.SelectFolderSaveDescription;
            openFile.Title = Properties.Texts.OpenFileVlcTitle;
            openFile.Filter = Properties.Texts.OpenFileVlcFilter;

            try
            {
                defaultSavePath = Installation.GetCurrentUserVideosFolder();
            }
            catch
            {
                defaultSavePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            } // try-catch

            Page1_Step0();
        } // ConfigForm_Load

        private void ConfigForm_Shown(object sender, EventArgs e)
        {
            Page1_Step1(false);
            labelStepTitle.Text = wizardControl.SelectedTab.ToolTipText;
        } // ConfigForm_Shown

        private void ConfigForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (IsFormAllowedToClose) return;

            DialogResult = DialogResult.None;
            e.Cancel = true;
            ConfirmUserCancel();
        } // ConfigForm_FormClosing

        private void WizardControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            BasicGoogleTelemetry.SendScreenHit(this, string.Format("Step{0:00}", wizardControl.SelectedIndex + 1));
        } // WizardControl_SelectedIndexChanged

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            buttonCancel.DialogResult = DialogResult.None;
            DialogResult = DialogResult.None;
            ConfirmUserCancel();
        } // buttonCancel_Click

        private void ConfirmUserCancel()
        {
            if (MessageBox.Show(this, Texts.ConfirmUserCancel, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
            EndWizard(DialogResult.Cancel, null, null);
        } // ConfirmUserCancel

        #region Page1

        private void linkLabelPrerequisiteNet_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Installation.OpenUrl(this, Properties.Texts.DownloadUrlNet);
        } // linkLabelPrerequisiteNet_LinkClicked

        private void linkLabelPrerequisiteEmb_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(this, Texts.DownloadEmbInfo, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Installation.OpenUrl(this, Properties.Texts.DownloadUrlEmb);
        } // linkLabelPrerequisiteEmb_LinkClicked

        private void linkLabelPrerequisiteSqlCe_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(this, Texts.DownloadSqlCeInfo, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            Installation.OpenUrl(this, Properties.Texts.DownloadUrlSqlCe);
        } // linkLabelPrerequisiteSqlCe_LinkClicked

        private void linkLabelPrerequisiteVlc_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Installation.OpenUrl(this, Properties.Texts.DownloadUrlVlc);
        } // linkLabelPrerequisiteVlc_LinkClicked

        private void buttonVerifyNet_Click(object sender, EventArgs e)
        {
            Page1_Step1(true);
        } // buttonVerifyNet_Click

        private void buttonVerifyEmb_Click(object sender, EventArgs e)
        {
            Page1_Step2(true);
        } // buttonVerifyEmb_Click

        private void buttonVerifySqlCe_Click(object sender, EventArgs e)
        {
            Page1_Step3(true);
        }  // buttonVerifySqlCe_Click

        private void buttonVerifyVlc_Click(object sender, EventArgs e)
        {
            Page1_Step4(true);
        } // buttonVerifyVlc_Click

        private void buttonFindVlc_Click(object sender, EventArgs e)
        {
            var oldValue = textBoxVlc.Text;

            if (!string.IsNullOrEmpty(textBoxVlc.Text))
            {
                openFile.InitialDirectory = Path.GetDirectoryName(textBoxVlc.Text);
                openFile.FileName = Path.GetFileName(textBoxVlc.Text); ;
            } // if

            if (openFile.ShowDialog(this) != DialogResult.OK) return;

            textBoxVlc.Text = openFile.FileName;
            buttonVerifyVlc.Visible = (oldValue != textBoxVlc.Text);
            buttonVerifyVlc.Focus();
        } // buttonFindVlc_Click

        private void buttonTestVlc_Click(object sender, EventArgs e)
        {
            string message;

            var testMedia = Installation.GetTestMedia();
            var installed = Installation.TestVlcInstallation(out message, textBoxVlc.Text, testMedia);
            MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK,
                installed ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

            if (!installed) return;
        } // buttonTestVlc_Click

        private void Page1_Step0()
        {
            BasicGoogleTelemetry.SendScreenHit(this, "Step01");

            buttonVerifyNet.Enabled = true;
            linkLabelPrerequisiteNet.Enabled = true;

            buttonVerifyEmb.Visible = false;
            linkLabelPrerequisiteEmb.Visible = false;

            buttonVerifySqlCe.Visible = false;
            linkLabelPrerequisiteSqlCe.Visible = false;

            buttonVerifyVlc.Visible = false;
            linkLabelPrerequisiteVlc.Visible = false;
            labelVlcPath.Enabled = false;
            textBoxVlc.Enabled = false;
            buttonFindVlc.Enabled = false;
            buttonTestVlc.Enabled = false;
        } // Page1_Step0

        private void Page1_Step1(bool withUi)
        {
            string message;

            var installed = Installation.IsNetInstalled(out message);
            if (withUi)
            {
                MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK,
                    installed ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            } // if
            if (!installed) return;

            pictureBoxNetOk.Image = Resources.Sucess_16x16;
            buttonVerifyNet.Visible = false;
            linkLabelPrerequisiteNet.Visible = false;

            buttonVerifyEmb.Visible = true;
            linkLabelPrerequisiteEmb.Visible = true;
            linkLabelPrerequisiteEmb.Focus();

            Page1_Step2(false);
        } // Page1_Step1

        private void Page1_Step2(bool withUi)
        {
            string message;

            var installed = Installation.IsEmbInstalled(out message);
            if (withUi)
            {
                MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK,
                    installed ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            } // if
            if (!installed) return;

            pictureBoxEmbOk.Image = Resources.Sucess_16x16;
            buttonVerifyEmb.Visible = false;
            linkLabelPrerequisiteEmb.Visible = false;

            buttonVerifySqlCe.Visible = true;
            linkLabelPrerequisiteSqlCe.Visible = true;
            linkLabelPrerequisiteSqlCe.Focus();

            Page1_Step3(false);
        } // Page1_Step2

        private void Page1_Step3(bool withUi)
        {
            string message;

            var installed = Installation.IsSqlCeInstalled(out message);
            if (withUi)
            {
                MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK,
                    installed ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            } // if
            if (!installed) return;

            pictureBoxSqlCeOk.Image = Resources.Sucess_16x16;
            buttonVerifySqlCe.Visible = false;
            linkLabelPrerequisiteSqlCe.Visible = false;

            buttonVerifyVlc.Visible = true;
            linkLabelPrerequisiteVlc.Visible = true;
            linkLabelPrerequisiteVlc.Focus();
            labelVlcPath.Enabled = true;
            textBoxVlc.Enabled = true;
            buttonFindVlc.Enabled = true;
            buttonTestVlc.Enabled = false;

            Page1_Step4(false);
        } // Page1_Step3

        private void Page1_Step4(bool withUi)
        {
            string message;
            string path;

            path = textBoxVlc.Text;
            var installed = Installation.IsVlcInstalled(out message, ref path);
            textBoxVlc.Text = path;
            if (withUi)
            {
                labelVlcInstallCheckResult.Text = null;
                MessageBox.Show(this, message, this.Text, MessageBoxButtons.OK,
                    installed ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            }
            else
            {
                if (!string.IsNullOrEmpty(path))
                {
                    labelVlcInstallCheckResult.Text = message;
                } // if
            } // if-else
            if (!installed) return;

            pictureBoxVlcOk.Image = Resources.Sucess_16x16;
            linkLabelPrerequisiteVlc.Visible = false;
            buttonVerifyVlc.Visible = false;
            //buttonFindVlc.Enabled = false;
            buttonTestVlc.Enabled = true;
            buttonTestVlc.Focus();

            Page1_Step5(false);
        } // Page1_Step4

        private void Page1_Step5(bool withUi)
        {
            wizardControl.IsPageAllowed[wizardPage2.Name] = true;
            wizardControl.UpdateWizardButtons();

            Page2_Step0();
        } // Page1_Step5

        #endregion

        #region Page 2

        private void Page2_Step0()
        {
            checkBoxFirewallDecoder.Checked = true;
            checkBoxFirewallVlc.Checked = true;

            wizardControl.IsPageAllowed[wizardPage3.Name] = true;
            wizardControl.UpdateWizardButtons();

            Page3_Step0();
        } // Page2_Step0

        private void checkBoxFirewall_CheckedChanged(object sender, EventArgs e)
        {
            var enabled = (checkBoxFirewallDecoder.Checked) || (checkBoxFirewallVlc.Checked);
            buttonFirewall.Enabled = enabled;
            labelFirewallWarning.Enabled = enabled;
        } // checkBoxFirewall_CheckedChanged

        private void buttonFirewall_Click(object sender, EventArgs e)
        {
            var result = Installation.RunSelfForFirewall(
                checkBoxFirewallDecoder.Checked ? Program.AppUiConfig.Folders.Install : null,
                checkBoxFirewallVlc.Checked ? textBoxVlc.Text : null);

            if (result.Message == null) return;
            if (result.InnerException != null)
            {
                MessageBox.Show(this, Texts.FirewallException + "\r\n" + result.InnerException.ToString(), this.Text,
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (result.IsOk)
            {
                buttonFirewall.Enabled = false;
                labelFirewallWarning.Enabled = false;
            }
            else
            {
                MessageBox.Show(this, result.Message, this.Text, MessageBoxButtons.OK, result.IsOk ? MessageBoxIcon.Information : MessageBoxIcon.Warning);
            } // if-else
        } // buttonFirewall_Click

        private void checkEnableAnalytics_CheckedChanged(object sender, EventArgs e)
        {
            if (!checkEnableAnalytics.Checked)
            {
                MessageBox.Show(this, Properties.Texts.AnalyticsKeepChecked, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            } // if

            checkAnalyticsUsage.Checked = checkEnableAnalytics.Checked;
            checkAnalyticsExceptions.Checked = checkEnableAnalytics.Checked;
        } // checkEnableAnalytics_CheckedChanged

        private void linkAnalyticsHelp_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(this, Properties.Texts.AnalyticsInfo, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        } // linkAnalyticsHelp_LinkClicked

        #endregion

        #region Page 3

        private void Page3_Step0()
        {
            labelSaveSubFolder.Enabled = false;
            textSaveSubFolder.Enabled = false;
            buttonConfig.Enabled = false;

            labelCreatingConfig.Visible = false;

            wizardControl.IsPageAllowed[wizardPage3.Name] = true;
            wizardControl.UpdateWizardButtons();
        } // Page3_Step0

        private void buttonBrowseSave_Click(object sender, EventArgs e)
        {
            selectFolder.NewStyle = true;
            selectFolder.SelectedPath = string.IsNullOrEmpty(textBoxSave.Text) ? defaultSavePath : textBoxSave.Text;
            selectFolder.RootFolder = Environment.SpecialFolder.Desktop;
            if (selectFolder.ShowDialog(this) != DialogResult.OK) return;

            textBoxSave.Text = selectFolder.SelectedPath;

            labelSaveSubFolder.Enabled = true;
            textSaveSubFolder.Enabled = true;
            buttonConfig.Enabled = true;
            buttonConfig.Focus();
        } // buttonBrowseSave_Click

        private void buttonConfig_Click(object sender, EventArgs e)
        {
            string message;
            string xmlConfigPath;

            var rootFolder = textBoxSave.Text;
            var subFolder = textSaveSubFolder.Text.Trim();
            if (subFolder != string.Empty)
            {
                rootFolder = Path.Combine(rootFolder, subFolder);
            } // if

            xmlConfigPath = Path.Combine(Program.AppUiConfig.Folders.Base, "user-config.xml");

            if (File.Exists(xmlConfigPath))
            {
                if (MessageBox.Show(this, Properties.Texts.OverwriteXmlConfigFile, this.Text,
                    MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
                {
                    return;
                } // if
            } // if

            labelCreatingConfig.Visible = true;
            labelCreatingConfig.Refresh();
            var success = Configuration.Create(textBoxVlc.Text,
                rootFolder,
                new AnalyticsConfig(checkEnableAnalytics.Checked, checkAnalyticsUsage.Checked, checkAnalyticsExceptions.Checked),
                new EpgConfig(checkEpg.Checked, checkEpgAutoUpdate.Checked? 24 : -1, 7),
                xmlConfigPath, out message);
            if (success)
            {
                EndWizard(DialogResult.OK, null, null);
            }
            else
            {
                EndWizard(DialogResult.Abort, message, null);
            } // if-else
        } // buttonConfig_Click

        #endregion

        #region End wizard

        private void EndWizard(DialogResult result, string message, Exception ex)
        {
            IsFormAllowedToClose = true;
            Program.SetWizardResult(result, message, ex);
            Close();
        }  // EndWizard

        #endregion
    } // class ConfigForm
} // namespace
