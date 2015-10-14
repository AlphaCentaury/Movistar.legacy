﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

// This file contains code (c) Microsoft Corporation, licensed under Microsoft Public License
// See http://www.microsoft.com/opensource/licenses.mspx#Ms-PL.
// Original source code: http://1code.codeplex.com/SourceControl/changeset/view/39074#842775

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Project.IpTv.UiServices.Configuration
{
    public static class WindowsBitness
    {
        public static bool Is32BitWindows()
        {
            if (IntPtr.Size == 8)
            {
                // 64-bit programs run only on Win64
                return false;
            }
            else
            {
                // 32-bit programs run on both 32-bit and 64-bit Windows
                // Detect whether the current process is a 32-bit process 
                // running on a 64-bit system
                return !SafeIsWow64Process();
            } // if-else
        } // Is32BitWindows

        /// <summary>
        /// The function determines whether the current operating system is a 
        /// 64-bit operating system.
        /// </summary>
        /// <returns>
        /// The function returns true if the operating system is 64-bit; 
        /// otherwise, it returns false.
        /// </returns>
        public static bool Is64BitWindows()
        {
            if (IntPtr.Size == 8)
            {
                // 64-bit programs run only on Win64
                return true;
            }
            else
            {
                // 32-bit programs run on both 32-bit and 64-bit Windows
                // Detect whether the current process is a 32-bit process 
                // running on a 64-bit system.
                return SafeIsWow64Process();
            } // if-else
        } // Is64BitWindows

        /// <summary>
        /// The function determins whether a method exists in the export 
        /// table of a certain module.
        /// </summary>
        /// <param name="moduleName">The name of the module</param>
        /// <param name="methodName">The name of the method</param>
        /// <returns>
        /// The function returns true if the method specified by methodName 
        /// exists in the export table of the module specified by moduleName.
        /// </returns>
        private static bool DoesWin32MethodExist(string moduleName, string methodName)
        {
            IntPtr moduleHandle = GetModuleHandle(moduleName);
            if (moduleHandle == IntPtr.Zero)
            {
                return false;
            }
            return (GetProcAddress(moduleHandle, methodName) != IntPtr.Zero);
        } // DoesWin32MethodExist

        private static bool SafeIsWow64Process()
        {
            bool flag;

            if (!DoesWin32MethodExist("kernel32.dll", "IsWow64Process")) return false;
            IsWow64Process(GetCurrentProcess(), out flag);

            return flag;
        } // SafeIsWow64Process

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetCurrentProcess();

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetModuleHandle(string moduleName);

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern IntPtr GetProcAddress(IntPtr hModule, [MarshalAs(UnmanagedType.LPStr)]string procName);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsWow64Process(IntPtr hProcess, out bool wow64Process);
    } // WindowsBitness
} // namespace
