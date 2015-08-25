﻿using Microsoft.SqlServer.MessageBox;
using Project.DvbIpTv.Common.Telemetry;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Windows.Forms;

namespace Project.DvbIpTv.UiServices.DvbStpClient
{
    public abstract class UiDvbStpBaseDownloader
    {
        // "Input" properties

        public string CaptionUserCancelled
        {
            get;
            set;
        } // CaptionUserCancelled

        public string TextUserCancelled
        {
            get;
            set;
        } // TextUserCancelled

        public string CaptionDownloadException
        {
            get;
            set;
        } // CaptionDownloadException

        public string TextDownloadException
        {
            get;
            set;
        } // TextDownloadException

        // "Return" properties

        public bool IsOk
        {
            get;
            private set;
        } // IsOk

        protected string TelemetryScreenName
        {
            get;
            set;
        } // TelemetryScreenName

        public UiDvbStpBaseDownloader()
        {
            CaptionUserCancelled = Properties.Texts.HelperUserCancelledCaption;
            CaptionDownloadException = Properties.Texts.HelperExceptionCaption;
        } // constructor

        public void Download(IWin32Window owner)
        {
            var response = ShowDialog(owner);

            IsOk = ((response.UserCancelled == false) && (response.DownloadException == null));

            if (response.UserCancelled)
            {
                var box = new ExceptionMessageBox()
                {
                    Caption = CaptionUserCancelled,
                    Text = TextUserCancelled,
                    Beep = true,
                    Symbol = ExceptionMessageBoxSymbol.Information
                };
                box.Show(owner);

                return;
            } // if

            if (response.DownloadException != null)
            {
                HandleException(owner, response.DownloadException);
            } // if
        } // Download

        protected abstract UiDvbStpBaseDownloadResponse ShowDialog(IWin32Window owner);

        private void HandleException(IWin32Window owner, Exception ex)
        {
            string message;

            BasicGoogleTelemetry.SendExtendedExceptionHit(ex, false, TelemetryScreenName, TelemetryScreenName);

            var isSocket = ex as SocketException;
            var isTimeout = ex as TimeoutException;

            if (isSocket != null) message = string.Format(Properties.Texts.SocketException, TextDownloadException, isSocket.SocketErrorCode);
            else if (isTimeout != null) message = string.Format(Properties.Texts.TimeoutException);
            else message = TextDownloadException;

            var box = new ExceptionMessageBox()
            {
                Buttons = ExceptionMessageBoxButtons.Custom,
                InnerException = ex,
                Text = message,
                DefaultButton = ExceptionMessageBoxDefaultButton.Button2,
                CustomSymbol = Properties.Resources.DvbStpDownload_Error_48x48
            };
            box.SetButtonText(ExceptionMessageBox.OKButtonText, Properties.Texts.HandleExceptionHelpButton);
            box.Show(owner);

            if (box.CustomDialogResult == ExceptionMessageBoxDialogResult.Button2)
            {
                BasicGoogleTelemetry.SendEventHit("ShowDialog", "UiServices.DvbStpClient.HelpDialog", TelemetryScreenName, TelemetryScreenName);
                using (var helpDialog = new HelpDialog())
                {
                    helpDialog.ShowDialog(owner);
                } // using
            } // if
        } // HandleException
    } // abstract class UiDvbStpBaseDownloader
} // namespace
