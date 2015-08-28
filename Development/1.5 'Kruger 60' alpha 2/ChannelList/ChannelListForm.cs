﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using Etsi.Ts102034.v010501.XmlSerialization;
using Etsi.Ts102034.v010501.XmlSerialization.BroadcastDiscovery;
using Etsi.Ts102034.v010501.XmlSerialization.PackageDiscovery;
using Microsoft.SqlServer.MessageBox;
using Project.DvbIpTv.ChannelList.Properties;
using Project.DvbIpTv.Common;
using Project.DvbIpTv.Common.Telemetry;
using Project.DvbIpTv.Services.Record;
using Project.DvbIpTv.Services.Record.Serialization;
using Project.DvbIpTv.UiServices.Common.Forms;
using Project.DvbIpTv.UiServices.Common.Start;
using Project.DvbIpTv.UiServices.Configuration;
using Project.DvbIpTv.UiServices.Configuration.Logos;
using Project.DvbIpTv.UiServices.Configuration.Schema2014.Config;
using Project.DvbIpTv.UiServices.Discovery;
using Project.DvbIpTv.UiServices.Discovery.BroadcastList;
using Project.DvbIpTv.UiServices.DvbStpClient;
using Project.DvbIpTv.UiServices.EPG;
using Project.DvbIpTv.UiServices.Forms;
using Project.DvbIpTv.UiServices.Record;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace Project.DvbIpTv.ChannelList
{
    public sealed partial class ChannelListForm : CommonBaseForm, ISplashScreenAwareForm
    {
        const int ListObsoleteAge = 30;
        const int ListOldAge = 15;
        UiServiceProvider SelectedServiceProvider;
        UiBroadcastDiscovery BroadcastDiscovery;
        MulticastScannerDialog MulticastScanner;
        UiBroadcastListManager ListManager;

        public ChannelListForm()
        {
            InitializeComponent();
            Icon = Properties.Resources.IPTV;
        } // constructor

        #region ISplashScreenAwareForm implementation

        public event EventHandler FormLoadCompleted;

        #endregion

        #region CommonBaseForm implementation

        protected override void OnExceptionThrown(object sender, CommonBaseFormExceptionThrownEventArgs e)
        {
            MyApplication.HandleException(sender as IWin32Window, e.Message, e.Exception);
        } // HandleException

        #endregion

        #region Form event handlers

        private void ChannelListForm_Load(object sender, EventArgs e)
        {
            if (!SafeCall(ChannelListForm_Load_Implementation, sender, e))
            {
                this.Close();
            } // if
        }  // ChannelListForm_Load

        private void ChannelListForm_Shown(object sender, EventArgs e)
        {
            BasicGoogleTelemetry.SendScreenHit(this, "Shown");
            if (SelectedServiceProvider == null)
            {
                SafeCall(SelectProvider);
            } // if
        } // ChannelListForm_Shown

        private void ChannelListForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // can't close the form if a services scan is in progress; the user must manually cancel it first
            e.Cancel = IsScanActive();
        } // ChannelListForm_FormClosing

        #endregion

        #region Form event handlers implementation

        private void ChannelListForm_Load_Implementation(object sender, EventArgs e)
        {
            BasicGoogleTelemetry.SendScreenHit(this, "Load");

            this.Text = Properties.Texts.AppCaption;

            // TODO: load from Configuration
            var settings = UiBroadcastListSettingsConfigurationRegistration.UserSettings;
            ListManager = new UiBroadcastListManager(listViewChannelList, settings, imageListChannels, imageListChannelsLarge, true);
            ListManager.SelectionChanged += ListManager_SelectionChanged;
            ListManager.StatusChanged += ListManager_StatusChanged;

            // set-up channel list control
            // TODO: move this code to UiBroadcastListManager
            listViewChannelList.TileSize = new Size((listViewChannelList.Width - SystemInformation.VerticalScrollBarWidth - 6) / 4,
                imageListChannelsLarge.ImageSize.Height + 6);

            SetupContextMenuList();

            // set-up EPG functionality
            epgMiniBar.IsDisabled = !AppUiConfiguration.Current.User.Epg.Enabled;
            if (epgMiniBar.IsDisabled)
            {
                foreach (ToolStripItem item in menuItemEpg.DropDownItems)
                {
                    item.Enabled = false;
                } // foreach
            } // if

            // load from cache, if available
            SelectedServiceProvider = SelectProviderDialog.GetLastUserSelectedProvider();
            ServiceProviderChanged();

            // notify Splash Screeen the form has finished loading and is about to be shown
            if (FormLoadCompleted != null)
            {
                FormLoadCompleted(this, e);
            } // if
        } // ChannelListForm_Load_Implementation

        #endregion

        #region ListManager event handlers

        private void ListManager_StatusChanged(object sender, ListStatusChangedEventArgs e)
        {
            SafeCall(ListManager_StatusChanged_Implementation, sender, e);
        } // ListManager_StatusChanged

        private void ListManager_SelectionChanged(object sender, ListSelectionChangedEventArgs e)
        {
            SafeCall(ListManager_SelectionChanged_Implementation, sender, e);
        } // ListManager_SelectionChanged

        private void ListManager_StatusChanged_Implementation(object sender, ListStatusChangedEventArgs e)
        {
            ListManager.ListView.Enabled = e.HasItems;
            menuItemChannelListView.Enabled = e.HasItems;
            menuItemChannelEditList.Enabled = e.HasItems;
            menuItemChannelFavorites.Enabled = e.HasItems;
            menuItemChannelVerify.Enabled = e.HasItems;
        } // ListManager_StatusChanged_Implementation

        private void ListManager_SelectionChanged_Implementation(object sender, ListSelectionChangedEventArgs e)
        {
            // save selection
            // TODO: save ListManager.SelectedService in user-config
            Properties.Settings.Default.LastSelectedService = (e.Item != null) ? e.Item.Key : null;
            Properties.Settings.Default.Save();

            var enable = e.Item != null;
            var enable2 = enable && !e.Item.IsHidden;
            menuItemChannelShow.Enabled = enable2;
            menuItemChannelShowWith.Enabled = enable2;
            menuItemChannelFavoritesAdd.Enabled = enable2;
            menuItemChannelDetails.Enabled = enable;
            menuItemRecordingsRecord.Enabled = enable2;
            buttonRecordChannel.Enabled = enable2;
            buttonDisplayChannel.Enabled = enable2;

            // EPG
            EnableEpgMenus(enable);
            if (enable)
            {
                ShowEpgMiniBar(true);
            }
            else
            {
                epgMiniBar.ClearEpgEvents();
            } // if-else
        } // ListManager_SelectionChanged_Implementation

        #endregion

        #region 'DVB-IPTV' menu event handlers

        private void menuItemDvbRecent_DropDownOpening(object sender, EventArgs e)
        {
            // TODO: update recent list
        }  // menuItemDvbRecent_DropDownOpening

        private void menuItemDvbRecent_Click(object sender, EventArgs e)
        {
            NotImplementedBox.ShowBox(this, "menuItemDvbRecent");
        }  // menuItemDvbRecent_Click

        private void menuItemDvbSettings_Click(object sender, EventArgs e)
        {
            NotImplementedBox.ShowBox(this, "menuItemDvbSettings");
        } // menuItemDvbSettings_Click

        private void menuItemDvbExport_Click(object sender, EventArgs e)
        {
            NotImplementedBox.ShowBox(this, "menuItemDvbExport");
        } // menuItemDvbExport_Click

        private void menuItemDvbExit_Click(object sender, EventArgs e)
        {
            this.Close();
        } // menuItemDvbExit_Click

        #endregion

        #region 'Provider' menu event handlers

        private void menuItemProviderSelect_Click(object sender, EventArgs e)
        {
            SafeCall(Implementation_menuItemProviderSelect_Click, sender, e);
        } // menuItemProviderSelect_Click

        private void menuItemProviderDetails_Click(object sender, EventArgs e)
        {
            SafeCall(Implementation_menuItemProviderDetails_Click, sender, e);
        } // menuItemProviderDetails_Click

        #endregion

        #region 'Provider' menu event handlers implementation

        [Browsable(false), EditorBrowsable(EditorBrowsableState.Never)]
        private void Implementation_menuItemProviderSelect_Click(object sender, EventArgs e)
        {
            // can't select a new provider if a services scan is in progress; the user must manually cancel it first
            if (IsScanActive()) return;
            SelectProvider();
        } // Implementation_menuItemProviderSelect_Click

        private void Implementation_menuItemProviderDetails_Click(object sender, EventArgs e)
        {
            if (SelectedServiceProvider == null) return;

            using (var dlg = new PropertiesDialog()
                {
                    Caption = Properties.Texts.SPProperties,
                    ItemProperties = SelectedServiceProvider.DumpProperties(),
                    Description = SelectedServiceProvider.DisplayName,
                    ItemIcon = SelectedServiceProvider.Logo.GetImage(LogoSize.Size64, true),
                })
            {
                dlg.ShowDialog(this);
            } // using
        } // Implementation_menuItemProviderDetails_Click

        private void SelectProvider()
        {
            using (var dialog = new SelectProviderDialog())
            {
                dialog.SelectedServiceProvider = SelectedServiceProvider;
                var result = dialog.ShowDialog(this);
                BasicGoogleTelemetry.SendScreenHit(this);
                if (result != DialogResult.OK) return;

                SelectedServiceProvider = dialog.SelectedServiceProvider;
                ServiceProviderChanged();
            } // dialog
        } // SelectProvider

        #endregion

        #region 'DVB-IPTV > Package' menu event handlers

        private void menuItemPackagesSelect_Click(object sender, EventArgs e)
        {
            SafeCall(Implementation_menuItemPackagesSelect_Click, sender, e);
        } // menuItemPackagesSelect_Click

        private void menuItemPackagesManage_Click(object sender, EventArgs e)
        {
            SafeCall(Implementation_menuItemPackagesManage_Click, sender, e);
        } // menuItemPackagesManage_Click

        #endregion

        #region 'DVB-IPTV > Package' menu event handlers implementation

        private void Implementation_menuItemPackagesSelect_Click(object sender, EventArgs e)
        {
            NotImplementedBox.ShowBox(this, "menuItemPackagesSelect");
        } // Implementation_menuItemPackagesSelect_Click

        private void Implementation_menuItemPackagesManage_Click(object sender, EventArgs e)
        {
            NotImplementedBox.ShowBox(this, "menuItemPackagesManage");
        } // Implementation_menuItemPackagesManage_Click

        #endregion

        #region Service-related event handlers

        private void menuItemChannelListView_Click(object sender, EventArgs e)
        {
            SafeCall(menuItemChannelListView_Click_Implementation, sender, e);
        } // menuItemChannelListView_Click

        private void menuItemChannelRefreshList_Click(object sender, EventArgs e)
        {
            SafeCall(menuItemChannelRefreshList_Click_Implementation, sender, e);
        } // menuItemChannelRefreshList_Click

        private void menuItemChannelVerify_Click(object sender, EventArgs e)
        {
            SafeCall(menuItemChannelVerify_Click_Implementation, sender, e);
        } // menuItemChannelVerify_Click

        private void menuItemChannelDetails_Click(object sender, EventArgs e)
        {
            SafeCall(menuItemChannelDetails_Click_Implementation, sender, e);
        } // menuItemChannelDetails_Click

        private void listViewChannelsList_DoubleClick(object sender, EventArgs e)
        {
            SafeCall(ShowTvChannel);
        } // listViewChannelsList_DoubleClick

        private void buttonRecordChannel_Click(object sender, EventArgs e)
        {
            SafeCall(buttonRecordChannel_Click_Implementation, sender, e);
        } // buttonRecordChannel_Click

        private void buttonDisplayChannel_Click(object sender, EventArgs e)
        {
            SafeCall(ShowTvChannel);
        } // buttonDisplayChannel_Click

        #endregion

        #region Service-related event handlers implementation

        private void menuItemChannelListView_Click_Implementation(object sender, EventArgs e)
        {
            ListManager.ShowSettingsEditor(this, true);
        } // menuItemChannelListView_Click_Implementation

        private void menuItemChannelRefreshList_Click_Implementation(object sender, EventArgs e)
        {
            // can't refresh the list if a services scan is in progress; the user must manually cancel it first
            if (IsScanActive()) return;

            LoadBroadcastDiscovery(false);
        } // menuItemChannelRefreshList_Click_Implementation

        private void menuItemChannelVerify_Click_Implementation(object sender, EventArgs e)
        {
            int timeout;
            MulticastScannerOptionsDialog.ScanWhatList list;
            MulticastScannerDialog.ScanDeadAction action;
            IEnumerable<UiBroadcastService> whatList;

            if ((MulticastScanner != null) && (!MulticastScanner.IsDisposed))
            {
                MulticastScanner.Activate();
                return;
            } // if

            using (var dialog = new MulticastScannerOptionsDialog())
            {
                var result = dialog.ShowDialog(this);
                BasicGoogleTelemetry.SendScreenHit(this);
                if (result != DialogResult.OK) return;
                timeout = dialog.Timeout;
                list = dialog.ScanList;
                action = dialog.DeadAction;
            } // using

            // filter whole list, if asked for
            switch (list)
            {
                case MulticastScannerOptionsDialog.ScanWhatList.ActiveServices:
                case MulticastScannerOptionsDialog.ScanWhatList.DeadServices:
                    whatList = from service in BroadcastDiscovery.Services
                               where service.IsInactive == (list == MulticastScannerOptionsDialog.ScanWhatList.DeadServices)
                               select service;
                    break;
                default:
                    whatList = BroadcastDiscovery.Services;
                    break;
            } // switch

            MulticastScanner = new MulticastScannerDialog()
            {
                Timeout = timeout,
                DeadAction = action,
                BroadcastServices = whatList,
            };
            MulticastScanner.ChannelScanResult += MulticastScanner_ChannelScanResult;
            MulticastScanner.Disposed += MulticastScanner_Disposed;
            MulticastScanner.ScanCompleted += MulticastScanner_ScanCompleted;
            MulticastScanner.ExceptionThrown += OnExceptionThrown;
            MulticastScanner.Show(this);
        }  // menuItemChannelVerify_Click_Implementation

        private void MulticastScanner_Disposed(object sender, EventArgs e)
        {
            MulticastScanner = null;
        } // MulticastScanner_Disposed

        private void MulticastScanner_ChannelScanResult(object sender, MulticastScannerDialog.ChannelScanResultEventArgs e)
        {
            if (e.IsSkipped) return;

            var service = e.Service;

            switch (e.DeadAction)
            {
                case MulticastScannerDialog.ScanDeadAction.Inactivate:
                    ListManager.EnableItem(service, e.IsDead, service.IsHidden);
                    break;
                case MulticastScannerDialog.ScanDeadAction.Hide:
                    ListManager.EnableItem(service, service.IsInactive, service.IsHidden || e.IsDead);
                    break;
                case MulticastScannerDialog.ScanDeadAction.Both:
                    ListManager.EnableItem(service, e.IsDead, service.IsHidden || e.IsDead);
                    break;
            } // switch
        }  // MulticastScanner_ChannelScanResult

        private void MulticastScanner_ScanCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // Save scan result in cache
            AppUiConfiguration.Current.Cache.SaveXml("UiBroadcastDiscovery", SelectedServiceProvider.Key, BroadcastDiscovery.Version, BroadcastDiscovery);
        } // MulticastScanner_ScanCompleted

        private void menuItemChannelDetails_Click_Implementation(object sender, EventArgs e)
        {
            if (ListManager.SelectedService == null) return;

            using (var dlg = new PropertiesDialog()
            {
                Caption = Properties.Texts.BroadcastServiceProperties,
                ItemProperties = ListManager.SelectedService.DumpProperties(),
                Description = string.Format("{0}\r\n{1}", ListManager.SelectedService.DisplayLogicalNumber, ListManager.SelectedService.DisplayName),
                ItemIcon = ListManager.SelectedService.Logo.GetImage(LogoSize.Size64, true),
            })
            {
                dlg.ShowDialog(this);
            } // using
        } // menuItemChannelDetails_Click_Implementation

        #endregion

        #region 'Recordings' menu event handlers

        private void menuItemRecordingsRecord_Click(object sender, EventArgs e)
        {
            SafeCall(buttonRecordChannel_Click_Implementation, sender, e);
        } // menuItemRecordingsRecord_Click

        private void menuItemRecordingsManage_Click(object sender, EventArgs e)
        {
            SafeCall(menuItemRecordingsManage_Click_Implementation, sender, e);
        } // menuItemRecordingsManage_Click

        private void menuItemRecordingsRepair_Click(object sender, EventArgs e)
        {
            SafeCall(menuItemRecordingsRepair_Click_Implementation, sender, e);
        } // menuItemRecordingsRepair_Click

        private void menuItemRecordingsImport_Click(object sender, EventArgs e)
        {
            SafeCall(menuItemRecordingsImport_Click_Implementation, sender, e);
        } // menuItemRecordingsImport_Click

        #endregion

        #region 'Recordings' menu event handlers implementation

        private void buttonRecordChannel_Click_Implementation(object sender, EventArgs e)
        {
            RecordTask task;

            if (ListManager.SelectedService == null) return;

            if (ListManager.SelectedService.IsInactive)
            {
                var box = new ExceptionMessageBox()
                {
                    Caption = this.Text,
                    Text = string.Format(Properties.Texts.RecordDeadTvChannel, ListManager.SelectedService.DisplayName),
                    Beep = true,
                    Symbol = ExceptionMessageBoxSymbol.Question,
                    Buttons = ExceptionMessageBoxButtons.YesNo,
                    DefaultButton = ExceptionMessageBoxDefaultButton.Button2,
                };
                if (box.Show(this) != System.Windows.Forms.DialogResult.Yes) return;
            } // if

            using (var dlg = new RecordChannelDialog())
            {
                dlg.ExceptionThrown += OnExceptionThrown;
                dlg.Task = RecordTask.CreateWithDefaultValues(new RecordChannel()
                {
                    LogicalNumber = ListManager.SelectedService.DisplayLogicalNumber,
                    Name = ListManager.SelectedService.DisplayName,
                    Description = ListManager.SelectedService.DisplayDescription,
                    ServiceKey = ListManager.SelectedService.Key,
                    ServiceName = ListManager.SelectedService.FullServiceName,
                    ChannelUrl = ListManager.SelectedService.LocationUrl,
                });
                dlg.IsNewTask = true;
                dlg.ShowDialog(this);
                task = dlg.Task;
                if (dlg.DialogResult != DialogResult.OK) return;
            } // using dlg

            var scheduler = new Scheduler(ExceptionHandler,
                AppUiConfiguration.Current.Folders.RecordTasks,
                MyApplication.RecorderLauncherPath);

            if (scheduler.CreateTask(task))
            {
                MessageBox.Show(this, Texts.SchedulerCreateTaskOk, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            } // if
        } // buttonRecordChannel_Click_Implementation

        private void menuItemRecordingsManage_Click_Implementation(object sender, EventArgs e)
        {
            using (var dlg = new RecordTasksDialog())
            {
                dlg.RecordTaskFolder = AppUiConfiguration.Current.Folders.RecordTasks;
                dlg.SchedulerFolders = GetTaskSchedulerFolders(AppUiConfiguration.Current.User.Record.TaskSchedulerFolders);
                dlg.ShowDialog(this);
            } // using
        } // menuItemRecordingsManage_Click_Implementation

        private IEnumerable<string> GetTaskSchedulerFolders(RecordTaskSchedulerFolder[] schedulerFolders)
        {
            var q = from folder in schedulerFolders
                    select folder.Path;

            return (new string[] { "\\" }).Concat(q);
        } // GetTaskSchedulerFolders

        private void menuItemRecordingsRepair_Click_Implementation(object sender, EventArgs e)
        {
            NotImplementedBox.ShowBox(this, "menuItemRecordingsRepair");
        } // menuItemRecordingsRepair_Click_Implementation

        private void menuItemRecordingsImport_Click_Implementation(object sender, EventArgs e)
        {
            NotImplementedBox.ShowBox(this, "menuItemRecordingsImport");
        } // menuItemRecordingsImport_Click_Implementation

        #endregion

        #region 'EPG' menu event handlers



        #endregion

        #region 'EPG' menu event handlers implementation



        #endregion

        #region 'Help' menu event handlers

        private void menuItemHelpDocumentation_Click(object sender, EventArgs e)
        {
            SafeCall(Implementation_menuItemHelpDocumentation_Click, sender, e);
        } // menuItemHelpDocumentation_Click

        private void menuItemHelpHomePage_Click(object sender, EventArgs e)
        {
            SafeCall(Implementation_menuItemHelpHomePage_Click, sender, e);
        } // menuItemHelpHomePage_Click

        private void menuItemHelpReportIssue_Click(object sender, EventArgs e)
        {
            SafeCall(Implementation_menuItemHelpReportIssue_Click, sender, e);
        } // menuItemHelpReportIssue_Click

        private void menuItemHelpCheckUpdates_Click(object sender, EventArgs e)
        {
            SafeCall(Implementation_menuItemHelpCheckUpdates_Click, sender, e);
        } // menuItemHelpCheckUpdates_Click

        private void menuItemHelpAbout_Click(object sender, EventArgs e)
        {
            SafeCall(Implementation_menuItemHelpAbout_Click, sender, e);
        } // menuItemHelpAbout_Click

        #endregion

        #region 'Help' menu event handlers

        private void Implementation_menuItemHelpDocumentation_Click(object sender, EventArgs e)
        {
            OpenUrl(Properties.InvariantTexts.UrlDocumentation);
        } // Implementation_menuItemHelpDocumentation_Click

        private void Implementation_menuItemHelpHomePage_Click(object sender, EventArgs e)
        {
            OpenUrl(Properties.InvariantTexts.UrlHomePage);
        } // Implementation_menuItemHelpHomePage_Click

        private void Implementation_menuItemHelpReportIssue_Click(object sender, EventArgs e)
        {
            OpenUrl(Properties.InvariantTexts.UrlReportIssue);
        } // Implementation_menuItemHelpReportIssue_Click

        private void Implementation_menuItemHelpCheckUpdates_Click(object sender, EventArgs e)
        {
            OpenUrl(Properties.InvariantTexts.UrlCheckForUpdatesManual);
        } // Implementation_menuItemHelpCheckUpdates_Click

        private void Implementation_menuItemHelpAbout_Click(object sender, EventArgs e)
        {
            using (var box = new AboutBox())
            {
                box.ExceptionThrown += OnExceptionThrown;
                box.ApplicationData = new AboutBoxApplicationData()
                {
                    Name = Texts.AppName,
                    Version = Texts.AppVersion,
                    Status = Texts.AppStatus,
                    LicenseTextRtf = Texts.SolutionLicenseRtf
                };
                box.ShowDialog(this);
            } // using box
        } // menuItemHelpAbout_Click_Implementation

        #endregion

        #region Auxiliary methods: providers

        private void ServiceProviderChanged()
        {
            Properties.Settings.Default.LastSelectedServiceProvider = (SelectedServiceProvider != null) ? SelectedServiceProvider.Key : null;
            Properties.Settings.Default.Save();

            if (SelectedServiceProvider == null)
            {
                labelProviderName.Text = Properties.Texts.NotSelectedServiceProvider;
                labelProviderDescription.Text = null;
                pictureProviderLogo.Image = null;
                menuItemProviderDetails.Enabled = false;
                menuItemChannelRefreshList.Enabled = false;
                menuItemChannelEditList.Enabled = false;
                SetBroadcastDiscovery(null);

                return;
            } // if

            labelProviderName.Text = SelectedServiceProvider.DisplayName;
            labelProviderDescription.Text = SelectedServiceProvider.DisplayDescription;
            pictureProviderLogo.Image = SelectedServiceProvider.Logo.GetImage(LogoSize.Size32, true);

            menuItemProviderDetails.Enabled = true;
            menuItemChannelRefreshList.Enabled = true;
            menuItemChannelEditList.Enabled = true;

            // TODO: clean-up
            UpdateEpgData();

            SetBroadcastDiscovery(null);
            LoadBroadcastDiscovery(true);
        } // ServiceProviderChanged

        #endregion

        #region Auxiliary methods: services

        private bool IsScanActive()
        {
            var isActive = (MulticastScanner != null) && (!MulticastScanner.IsDisposed);
            if ((isActive) && (MulticastScanner.ScanInProgress == false))
            {
                MulticastScanner.Close();
                isActive = false;
            } // if

            if (isActive)
            {
                MessageBox.Show(this, Properties.Texts.ChannelFormActiveScan, Properties.Texts.ChannelFormActiveScanCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                MulticastScanner.Activate();

                return true;
            } // if

            return false;
        } // IsScanActive

        private bool LoadBroadcastDiscovery(bool fromCache)
        {
            UiBroadcastDiscovery uiDiscovery;

            try
            {
                uiDiscovery = null;
                if (fromCache)
                {
                    var cachedDiscovery = AppUiConfiguration.Current.Cache.LoadXmlDocument<UiBroadcastDiscovery>("UiBroadcastDiscovery", SelectedServiceProvider.Key);
                    if (cachedDiscovery == null)
                    {
                        Notify(Properties.Resources.Error_24x24, Properties.Texts.ChannelListNoCache, 60000);
                        return false;
                    } // if
                    uiDiscovery = cachedDiscovery.Document;
                    NotifyChannelListAge((int)cachedDiscovery.Age.TotalDays);
                } // if

                if (uiDiscovery == null)
                {
                    var downloader = new UiDvbStpEnhancedDownloader()
                    {
                        Request = new UiDvbStpEnhancedDownloadRequest(2)
                        {
                            MulticastAddress = IPAddress.Parse(SelectedServiceProvider.Offering.Push[0].Address),
                            MulticastPort = SelectedServiceProvider.Offering.Push[0].Port,
                            Description = Properties.Texts.BroadcastObtainingList,
                            DescriptionParsing = Properties.Texts.BroadcastParsingList,
                            AllowXmlExtraWhitespace = false,
                            XmlNamespaceReplacer = NamespaceUnification.Replacer,
#if DEBUG
                            DumpToFolder = AppUiConfiguration.Current.Folders.Cache
#endif
                        },
                        TextUserCancelled = Properties.Texts.UserCancelListRefresh,
                        TextDownloadException = Properties.Texts.BroadcastListUnableRefresh,
                    };
                    downloader.Request.AddPayload(0x02, null, Properties.Texts.Payload02DisplayName, typeof(BroadcastDiscoveryRoot));
                    downloader.Request.AddPayload(0x05, null, Properties.Texts.Payload05DisplayName, typeof(PackageDiscoveryRoot));
                    downloader.Download(this);
                    BasicGoogleTelemetry.SendScreenHit(this);
                    if (!downloader.IsOk) return false;

                    var xmlDiscovery = downloader.Request.Payloads[0].XmlDeserializedData as BroadcastDiscoveryRoot;
                    uiDiscovery = new UiBroadcastDiscovery(xmlDiscovery, SelectedServiceProvider.DomainName, downloader.Request.Payloads[0].SegmentVersion);
                    UiBroadcastDiscoveryMergeResultDialog.Merge(this, BroadcastDiscovery, uiDiscovery);
                    
                    var xmlPackageDiscovery = downloader.Request.Payloads[1].XmlDeserializedData as PackageDiscoveryRoot;
                    GetLogicalNumbers(uiDiscovery, xmlPackageDiscovery);
                    AppUiConfiguration.Current.Cache.SaveXml("UiBroadcastDiscovery", SelectedServiceProvider.Key, uiDiscovery.Version, uiDiscovery);
                } // if

                ShowEpgMiniBar(false);
                SetBroadcastDiscovery(uiDiscovery);

                if (fromCache)
                {
                    if (BroadcastDiscovery.Services.Count <= 0)
                    {
                        Notify(Properties.Resources.Info_24x24, Properties.Texts.ChannelListCacheEmpty, 30000);
                    } // if
                } // if-else

                return true;
            }
            catch (Exception ex)
            {
                MyApplication.HandleException(this, null, Properties.Texts.BroadcastListUnableRefresh, ex);
                return false;
            } // try-catch
        } // LoadBroadcastDiscovery

        private void SetBroadcastDiscovery(UiBroadcastDiscovery broadcastDiscovery)
        {
            BroadcastDiscovery = broadcastDiscovery;
            ListManager.BroadcastServices = (BroadcastDiscovery != null)? BroadcastDiscovery.Services : null;
        } // SetBroadcastDiscovery

        private void ShowTvChannel()
        {
            if (ListManager.SelectedService == null) return;
            if (ListManager.SelectedService.IsHidden) return;

            if (ListManager.SelectedService.IsInactive)
            {
                var box = new ExceptionMessageBox()
                {
                    Caption = this.Text,
                    Text = string.Format(Properties.Texts.ShowDeadTvChannel, ListManager.SelectedService.DisplayName),
                    Beep = true,
                    Symbol = ExceptionMessageBoxSymbol.Question,
                    Buttons = ExceptionMessageBoxButtons.YesNo,
                    DefaultButton = ExceptionMessageBoxDefaultButton.Button2,
                };
                if (box.Show(this) != System.Windows.Forms.DialogResult.Yes) return;
            } // if

            // TODO: player should be user-selectable
            var player = AppUiConfiguration.Current.User.TvViewer.Players[0];

            ExternalPlayer.Launch(player, ListManager.SelectedService, true);
        } // ShowTvChannel

        private void NotifyChannelListAge(int daysAge)
        {
            if (daysAge > ListObsoleteAge)
            {
                Notify(Properties.Resources.HighPriority_24x24, string.Format(Properties.Texts.ChannelListAgeObsolete, ListObsoleteAge), 0);
            }
            else if (daysAge >= ListOldAge)
            {
                Notify(Properties.Resources.Warning_24x24, string.Format(Properties.Texts.ChannelListAgeOld, daysAge), 90000);
            }
            else
            {
                Notify(null, null, 0);
            } // if-else
        } // NotifyChannelListAge

        #endregion

        #region Auxiliary methods: common

        private void ExceptionHandler(string message, Exception ex)
        {
            MyApplication.HandleException(this, message, ex);
        } // ExceptionHandler

        private void Notify(Image icon, string text, int dismissTime)
        {
            timerDismissNotification.Enabled = false;

            if (pictureNotificationIcon.Image != null)
            {
                pictureNotificationIcon.Image.Dispose();
            } // if
            pictureNotificationIcon.Image = icon;
            pictureNotificationIcon.Visible = (icon != null);

            labelNotification.Text = text;
            labelNotification.Visible = (text != null);

            if ((text != null) && (dismissTime > 0))
            {
                timerDismissNotification.Interval = dismissTime;
                timerDismissNotification.Enabled = true;
            } // if
        } // Notify

        private void timerDismissNotification_Tick(object sender, EventArgs e)
        {
            try
            {
                Notify(null, null, 0);
            }
            catch
            {
                timerDismissNotification.Enabled = false;
            } // try-catch
        } // timerDismissNotification_Tick

        private void OpenUrl(string url)
        {
            Launcher.OpenUrl(this, url, HandleException, Properties.Texts.OpenUrlError);
        } // OpenUrl

        #endregion

        #region WORK IN PROGRESS - EXPERIMENTS - Code to clean-up and/or move to appropriate sections

        private void SetFullscreenMode(bool fullScreen, bool topMost)
        {
            if (fullScreen)
            {
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                this.TopMost = topMost;
            }
            else
            {
                this.WindowState = FormWindowState.Normal;
                this.FormBorderStyle = FormBorderStyle.Sizable;
                this.TopMost = false;
            } // if-else
        } // SetFullscreenMode

        private void menuItemChannelFavorites_Click(object sender, EventArgs e)
        {
            NotImplementedBox.ShowBox(this, "menuItemChannelFavorites");
        }  // menuItemChannelFavorites_Click

        private void menuItemChannelFavoritesEdit_Click(object sender, EventArgs e)
        {
            NotImplementedBox.ShowBox(this, "menuItemChannelFavorites");
        }  // menuItemChannelFavoritesEdit_Click

        private void menuItemEpgNow_Click(object sender, EventArgs e)
        {
            ShowEpgNowThenForm();
        } // menuItemEpgNow_Click

        private void menuItemEpgToday_Click(object sender, EventArgs e)
        {
            ShowEpgList(0);
        } // menuItemEpgToday_Click

        private void menuItemEpgTomorrow_Click(object sender, EventArgs e)
        {
            ShowEpgList(1);
        } // menuItemEpgTomorrow_Click

        private void menuItemEpgRefresh_Click(object sender, EventArgs e)
        {
            LaunchEpgLoader(false);
        }  // menuItemEpgRefresh_Click

        private void SetupContextMenuList()
        {
            contextMenuListMode.Text = menuItemChannelListView.Text;
            contextMenuListMode.Image = menuItemChannelListView.Image;
            contextMenuListProperties.Text = menuItemChannelDetails.Text;
            contextMenuListProperties.Image = menuItemChannelDetails.Image;
        } // SetupContextMenuList

        private void contextMenuList_Opening(object sender, CancelEventArgs e)
        {
            // sync Properties context item with main menu counterpart
            contextMenuListShow.Enabled = menuItemChannelShow.Enabled;
            contextMenuListRecord.Enabled = menuItemRecordingsRecord.Enabled;
            contextMenuListShowWith.Enabled = menuItemChannelShowWith.Enabled;
            contextMenuListProperties.Enabled = menuItemChannelDetails.Enabled;
        } // contextMenuList_Opening

        private void contextMenuListShowWith_Click(object sender, EventArgs e)
        {
            NotImplementedBox.ShowBox(this, "contextMenuListShowWith");
        } // contextMenuListShowWith_Click

        private void contextMenuListMode_Click(object sender, EventArgs e)
        {
            ListManager.ShowSettingsEditor(this, true);
        } // contextMenuListMode_Click

        private void contextMenuListCopy_DropDownOpening(object sender, EventArgs e)
        {
            contextMenuListCopyRow.Enabled = ListManager.SelectedService != null;
            contextMenuListCopyAll.Enabled = ListManager.HasItems;
        } // contextMenuListCopy_DropDownOpening

        private void contextMenuListCopyURL_Click(object sender, EventArgs e)
        {
            var service = ListManager.SelectedService;
            if (service == null) return;

            Clipboard.SetText(service.LocationUrl, TextDataFormat.UnicodeText);
        } // contextMenuListCopyURL_Click

        private void contextMenuListCopyRow_Click(object sender, EventArgs e)
        {
            StringBuilder buffer;

            var service = ListManager.SelectedService;
            if (service == null) return;

            buffer = new StringBuilder();
            DumpHeader(buffer);
            DumpBroadcastService(service, buffer);

            Clipboard.SetText(buffer.ToString(), TextDataFormat.UnicodeText);
        } // contextMenuListCopyRow_Click

        private void contextMenuListCopyAll_Click(object sender, EventArgs e)
        {
            StringBuilder buffer;

            buffer = new StringBuilder();

            DumpHeader(buffer);
            foreach (var service in ListManager.BroadcastServices)
            {
                DumpBroadcastService(service, buffer);
                buffer.AppendLine();
            } // foreach item

            Clipboard.SetText(buffer.ToString(), TextDataFormat.UnicodeText);
        } // contextMenuListCopyAll_Click

        private void DumpHeader(StringBuilder buffer)
        {
            buffer.Append("Number");
            buffer.Append("\t");
            buffer.Append("Name");
            buffer.Append("\t");
            buffer.Append("Description");
            buffer.Append("\t");
            buffer.Append("Type");
            buffer.Append("\t");
            buffer.Append("LocationUrl");
            buffer.Append("\t");
            buffer.Append("FullServiceId");
            buffer.Append("\t");
            buffer.Append("ReplacementServiceId");
            buffer.AppendLine();
        } // DumpHeader

        private void DumpBroadcastService(UiBroadcastService service, StringBuilder buffer)
        {
            buffer.Append(service.DisplayLogicalNumber);
            buffer.Append("\t");
            buffer.Append(service.DisplayName);
            buffer.Append("\t");
            buffer.Append(service.DisplayDescription);
            buffer.Append("\t");
            buffer.Append(service.DisplayServiceType);
            buffer.Append("\t");
            buffer.Append(service.DisplayLocationUrl);
            buffer.Append("\t");
            buffer.Append(service.FullServiceName);
            buffer.Append("\t");
            var replacement = service.ReplacementService;
            if (replacement != null)
            {
                if (string.IsNullOrEmpty(replacement.DomainName))
                {
                    buffer.Append(replacement.ServiceName);
                    buffer.Append('.');
                    buffer.Append(service.DomainName);
                }
                else
                {
                    buffer.Append(replacement.ServiceName);
                    buffer.Append('.');
                    buffer.Append(replacement.DomainName);
                } // if-else
            } // if
        } // DumpBroadcastService

        private void EnableEpgMenus(bool enable)
        {
            menuItemEpgNow.Enabled = enable;
            menuItemEpgToday.Enabled = enable;
            menuItemEpgTomorrow.Enabled = enable;
            menuItemEpgPrevious.Enabled = false;
            menuItemEpgNext.Enabled = false;
            menuItemEpgRefresh.Enabled = (AppUiConfiguration.Current.User.Epg.Enabled) && (SelectedServiceProvider != null);
        } // EnableEpgMenus

        private void ShowEpgMiniBar(bool display)
        {
            epgMiniBar.Visible = display;
            if (!display) return;

            // TODO: get dbFile from config
            var dbFile = Path.Combine(AppUiConfiguration.Current.Folders.Cache, "EPG.sdf");

            // TODO: do NOT assume .imagenio.es
            var fullServiceName = ListManager.SelectedService.ServiceName + ".imagenio.es";
            var replacement = ListManager.SelectedService.ReplacementService;
            var fullAlternateServiceName = (replacement == null) ? null : replacement.ServiceName + ".imagenio.es";

            // display mini bar
            epgMiniBar.DetailsButtonEnabled = false; // TODO: to be implemented
            epgMiniBar.DisplayEpgEvents(imageListChannelsLarge.Images[ListManager.SelectedService.Logo.Key], fullServiceName, fullAlternateServiceName, DateTime.Now, dbFile);
        }  // ShowEpgMiniBar

        private void ShowEpgNowThenForm()
        {
                FormEpgNowThen.ShowEpgEvents(ListManager.SelectedService,
                    epgMiniBar.GetEpgEvents(), this, epgMiniBar.ReferenceTime);
        } // ShowEpgNowThenForm


        private void epgMiniBar_ButtonClicked(object sender, EpgMiniBarButtonClickedEventArgs e)
        {
            if (e.Button == EpgMiniBar.Button.FullView)
            {
                ShowEpgNowThenForm();
            } // if
        }

        private void epgMiniBar_NavigationButtonsChanged(object sender, EpgMiniBarNavigationButtonsChangedEventArgs e)
        {
            menuItemEpgPrevious.Enabled = e.IsBackEnabled;
            menuItemEpgNext.Enabled = e.IsForwardEnabled;
        }

        private void menuItemEpgPrevious_Click(object sender, EventArgs e)
        {
            epgMiniBar.GoBack();
        }

        private void menuItemEpgNext_Click(object sender, EventArgs e)
        {
            epgMiniBar.GoForward();
        }

        private void UpdateEpgData()
        {
            if (!AppUiConfiguration.Current.User.Epg.Enabled) return;

            var dbFile = Path.Combine(AppUiConfiguration.Current.Folders.Cache, "EPG.sdf");
            var status = Project.DvbIpTv.Services.EPG.Serialization.EpgDbQuery.GetStatus(dbFile);

#if !DEBUG
            if (status.IsNew)
            {
                LaunchEpgLoader(true);
            }
            else if (!status.IsError)
            {
                var hours = AppUiConfiguration.Current.User.Epg.AutoUpdateHours;
                if (hours >= 0)
                {
                    var update = (DateTime.Now - status.Time.ToLocalTime()).TotalHours >= AppUiConfiguration.Current.User.Epg.AutoUpdateHours;
                    if (update)
                    {
                        LaunchEpgLoader(true);
                    } // if
                } // if
            } // if
#endif
        } // UpdateEgpData

        private void LaunchEpgLoader(bool foreground)
        {
            //TODO: avoid fixed paths & code clean-up
            //TODO: avoid updating if an update is already in progress

#if DEBUG
            MessageBox.Show(this, "EPG updating is not available in DEBUG builds");
#else
            var updater = Path.Combine(AppUiConfiguration.Current.Folders.Install, "ConsoleEPGLoader.exe");
            if (!File.Exists(updater))
            {
                HandleException("Unable to find EPG loader/updater utility", new FileNotFoundException(updater));
                return;
            } // if
            var args = string.Format("\"/Database:{0}\"", Path.Combine(AppUiConfiguration.Current.Folders.Cache, "EPG.sdf"));

            var processInfo = new System.Diagnostics.ProcessStartInfo()
            {
                FileName = updater,
                Arguments = args,
                ErrorDialog = true,
                ErrorDialogParentHandle = ((IWin32Window)this).Handle,
                WindowStyle = foreground? System.Diagnostics.ProcessWindowStyle.Normal :  System.Diagnostics.ProcessWindowStyle.Minimized
            };

            using (var process = System.Diagnostics.Process.Start(processInfo))
            {
                // no op
            } // using
#endif
        } // LaunchEpgLoader

        private void ShowEpgList(int daysDelta)
        {
            if (ListManager.SelectedService == null) return;

            using (var form = new EpgChannelPrograms())
            {
                // TODO: unify code with mini-bar code

                // TODO: get dbFile from config
                form.EpgDatabase = Path.Combine(AppUiConfiguration.Current.Folders.Cache, "EPG.sdf");

                // TODO: do NOT assume .imagenio.es
                form.FullServiceName = ListManager.SelectedService.ServiceName + ".imagenio.es";
                var replacement = ListManager.SelectedService.ReplacementService;
                form.FullAlternateServiceName = (replacement == null) ? null : replacement.ServiceName + ".imagenio.es";

                form.DaysDelta = daysDelta;
                form.Service = ListManager.SelectedService;

                form.ShowDialog(this);
            } // using form
        } // ShowEpgList

        private void GetLogicalNumbers(UiBroadcastDiscovery uiDiscovery, PackageDiscoveryRoot xmlPackage)
        {
            var packages = from discovery in xmlPackage.PackageDiscovery
                           from package in discovery.Packages
                           select package;

            var buffer = new StringBuilder();
            foreach (var package in packages)
            {
                buffer.Append(package.Id);
                buffer.Append("\t");
                buffer.Append(package.Name.SafeGetLanguageValue(AppUiConfiguration.Current.User.PreferredLanguagesList, AppUiConfiguration.Current.DisplayPreferredOrFirst, null));
                buffer.Append("\t");
                buffer.Append(package.Services.Count);
                buffer.AppendLine();
            } // foreach
            var list = buffer.ToString();

            buffer = new StringBuilder();
            foreach (var package in packages)
            {
                buffer.Append(package.Id);
                buffer.Append("\t");
                buffer.Append(package.Name.SafeGetLanguageValue(AppUiConfiguration.Current.User.PreferredLanguagesList, AppUiConfiguration.Current.DisplayPreferredOrFirst, null));
                buffer.Append("\t");
                buffer.Append(package.Services.Count);
                buffer.AppendLine();

                foreach (var service in package.Services)
                {
                    var fullName = service.TextualIdentifiers[0].ServiceName + "@" + SelectedServiceProvider.DomainName;
                    buffer.Append("\t");
                    buffer.Append("\t");
                    var refService = uiDiscovery.TryGetService(fullName);
                    buffer.Append(fullName);
                    buffer.Append("\t");
                    buffer.Append(service.LogicalChannelNumber);
                    buffer.Append("\t");
                    buffer.Append((refService != null) ? refService.DisplayName : "<null>");
                    buffer.Append("\t");
                    buffer.Append((refService != null) ? refService.DisplayServiceType : "<null>");
                    buffer.AppendLine();
                } // foreach service
            } // foreach

            var list2 = buffer.ToString();

            var sortedPackages = from package in packages
                                 orderby package.Services.Count descending
                                 select package;

            foreach (var package in sortedPackages)
            {
                foreach (var service in package.Services)
                {
                    var fullName = service.TextualIdentifiers[0].ServiceName + "@" + SelectedServiceProvider.DomainName;
                    var refService = uiDiscovery.TryGetService(fullName);
                    if (refService == null) continue;

                    int number;
                    string logical;
                    if (int.TryParse(service.LogicalChannelNumber, out number))
                    {
                        logical = string.Format("{0:000}", number);
                    }
                    else
                    {
                        logical = service.LogicalChannelNumber;
                    } // if-else

                    if (refService.ServiceLogicalNumber == null)
                    {
                        refService.ServiceLogicalNumber = logical;
                    }
                    else if (refService.ServiceLogicalNumber != logical)
                    {
                        
                    } // if-else
                } // foreach
            } // foreach

            var numbers = new Dictionary<string, UiBroadcastService>(uiDiscovery.Services.Count, StringComparer.CurrentCultureIgnoreCase);
            var noNumber = 1;
            foreach (var service in uiDiscovery.Services)
            {
                UiBroadcastService existing;

                if (service.ServiceLogicalNumber == null)
                {
                    service.ServiceLogicalNumber = string.Format("X{0:000}", noNumber++);
                    continue;
                } // if

                if (numbers.TryGetValue(service.ServiceLogicalNumber, out existing))
                {
                    if (service.IsHighDefinitionTv)
                    {
                        if (existing.IsStandardDefinitionTv)
                        {
                            numbers[service.ServiceLogicalNumber] = service;
                            existing.ServiceLogicalNumber = "S" + existing.ServiceLogicalNumber;
                            numbers[existing.ServiceLogicalNumber] = existing;
                        }
                        else
                        {
                            numbers[service.ServiceLogicalNumber] = existing;
                            service.ServiceLogicalNumber = "S" + existing.ServiceLogicalNumber;
                            numbers[service.ServiceLogicalNumber] = service;
                        } // if-else
                    }
                    else
                    {
                        if (existing.IsStandardDefinitionTv)
                        {
                            numbers[service.ServiceLogicalNumber] = service;
                            existing.ServiceLogicalNumber = "S" + existing.ServiceLogicalNumber;
                            numbers[existing.ServiceLogicalNumber] = existing;
                        }
                        else
                        {
                            numbers[service.ServiceLogicalNumber] = existing;
                            service.ServiceLogicalNumber = "S" + existing.ServiceLogicalNumber;
                            numbers[service.ServiceLogicalNumber] = service;
                        } // if-else
                    } // if-else
                }
                else
                {
                    numbers[service.ServiceLogicalNumber] = service;
                } // if-else
            } // foreach
        } // GetLogicalNumbers

        #endregion
    } // class ChannelListForm
} // namespace
