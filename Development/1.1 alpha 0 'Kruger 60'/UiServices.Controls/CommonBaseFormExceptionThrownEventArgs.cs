﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Project.DvbIpTv.UiServices.Controls
{
    public class CommonBaseFormExceptionThrownEventArgs : EventArgs
    {
        public CommonBaseFormExceptionThrownEventArgs(Exception exception)
        {
            Exception = exception;
        } // constructor

        public CommonBaseFormExceptionThrownEventArgs(string message, Exception exception)
        {
            Message = message;
            Exception = exception;
        } // constructor

        public string Message
        {
            get;
            private set;
        } // Message

        public Exception Exception
        {
            get;
            private set;
        } // Exception
    } // class CommonBaseFormExceptionThrownEventArgs
} // namespace
