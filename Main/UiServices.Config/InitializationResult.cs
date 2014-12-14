﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.DvbIpTv.UiServices.Configuration
{
    public class InitializationResult
    {
        private static InitializationResult OkSingleton = new InitializationResult() { IsOk = true };

        /// <summary>
        /// Gets an <see cref="InitializationResult"/> with IsOk set to <see cref="true"/> and all remaining fields set to <see cref="null"/>
        /// </summary>
        public static InitializationResult Ok
        {
            get { return OkSingleton; }
        } // Ok

        public bool IsOk
        {
            get;
            set;
        } // IsOk

        public bool IsError
        {
            get { return !IsOk; }
        } // IsError

        public string Caption
        {
            get;
            set;
        } // Caption

        public string Message
        {
            get;
            set;
        } // Message

        public Exception InnerException
        {
            get;
            set;
        } // InnerException

        public InitializationResult()
        {
            // no op
        } // constructor

        public InitializationResult(string message)
        {
            Message = message;
        } // InitializationResult

        public InitializationResult(Exception exception)
        {
            InnerException = exception;
        } // InitializationResult
    } // InitializationResult
} // namespace
