﻿// Copyright (C) 2014-2016, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using Project.IpTv.Common.Telemetry;
using Project.IpTv.UiServices.Forms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Windows.Forms;

namespace Project.IpTv.ChannelList
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static int Main(string[] arguments)
        {
            // set thread name for debugging
            Thread.CurrentThread.Name = "Program main thread";

            //Application.ThreadException += Application_ThreadException;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var appContext = new MyApplicationContext();
            Application.Run(appContext);
            var exitCode = appContext.ExitCode;
            appContext.Dispose();

            BasicGoogleTelemetry.SendScreenHit("ChannelList_Main: End");
            BasicGoogleTelemetry.ManageSession(true);
            BasicGoogleTelemetry.EnsureHitsSents();

            return exitCode;
        } // Main

        static void Application_ThreadException(object sender, System.Threading.ThreadExceptionEventArgs e)
        {
            MyApplication.HandleException(null, e.Exception);
        } // Application_ThreadException
    } // class Program
} // namespace
