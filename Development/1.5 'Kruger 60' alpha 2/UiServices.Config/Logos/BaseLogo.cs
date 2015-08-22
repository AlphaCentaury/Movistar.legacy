﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.IO;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Project.DvbIpTv.UiServices.Configuration.Logos
{
    public abstract class BaseLogo
    {
        public static Size LogoSizeToSize(LogoSize logoSize)
        {
            switch (logoSize)
            {
                case LogoSize.Size32: return new Size(32, 32);
                case LogoSize.Size48: return new Size(48, 48);
                case LogoSize.Size64: return new Size(64,64);
                case LogoSize.Size96: return new Size(96,96);
                case LogoSize.Size128: return new Size(128, 128);
                case LogoSize.Size256: return new Size(256, 256);
                default:
                    throw new IndexOutOfRangeException();
            } // switch
        } // LogoSizeToSize

        public static string LogoSizeToString(LogoSize logoSize, bool withSize)
        {
            string text;

            switch (logoSize)
            {
                case LogoSize.Size32: text = Properties.Texts.LogoSize32; break;
                case LogoSize.Size48: text = Properties.Texts.LogoSize48; break;
                case LogoSize.Size64: text = Properties.Texts.LogoSize64; break;
                case LogoSize.Size96: text = Properties.Texts.LogoSize96; break;
                case LogoSize.Size128: text = Properties.Texts.LogoSize128; break;
                case LogoSize.Size256: text = Properties.Texts.LogoSize256; break;
                default:
                    throw new IndexOutOfRangeException();
            } // switch

            if (withSize)
            {
                var size = LogoSizeToSize(logoSize);
                return string.Format(Properties.Texts.LogoSizeWithSizeFormat, text, size.Width, size.Height);
            }
            else
            {
                return text;
            } // if-else
        } // LogoSizeToString

        public static List<KeyValuePair<LogoSize, string>> GetListLogoSizes(bool withSize)
        {
            var result = new List<KeyValuePair<LogoSize, string>>(6);
            result.Add(new KeyValuePair<LogoSize, string>(LogoSize.Size32, LogoSizeToString(LogoSize.Size32, withSize)));
            result.Add(new KeyValuePair<LogoSize, string>(LogoSize.Size48, LogoSizeToString(LogoSize.Size48, withSize)));
            result.Add(new KeyValuePair<LogoSize, string>(LogoSize.Size64, LogoSizeToString(LogoSize.Size64, withSize)));
            result.Add(new KeyValuePair<LogoSize, string>(LogoSize.Size96, LogoSizeToString(LogoSize.Size96, withSize)));
            result.Add(new KeyValuePair<LogoSize, string>(LogoSize.Size128, LogoSizeToString(LogoSize.Size128, withSize)));
            result.Add(new KeyValuePair<LogoSize, string>(LogoSize.Size256, LogoSizeToString(LogoSize.Size256, withSize)));

            return result;
        } // GetListLogoSizes

        public string File
        {
            get;
            internal set;
        } // File

        public string Path
        {
            get;
            internal set;
        } // Path

        public string Key
        {
            get;
            internal set;
        } // Key

        public Image GetImage(LogoSize logoSize, bool noExceptions)
        {
            string filename;

            if (!IsSizeAvailable(logoSize))
            {
                throw new NotSupportedException();
            } // if

            filename = File + GetSizeSufix(logoSize) + ".png";
            filename = System.IO.Path.Combine(Path, filename);
            try
            {
                return Image.FromFile(filename);
            }
            catch (FileNotFoundException ex)
            {
                if (noExceptions == false)
                {
                    throw new FileNotFoundException(ImageNotFoundExceptionText, ex);
                } // if
            }
            catch (OutOfMemoryException ex)
            {
                if (noExceptions == false)
                {
                    throw new InvalidOperationException(string.Format(ImageLoadExceptionText, filename), ex);
                }
            } // try-catch
            catch
            {
                if (noExceptions == false) throw;
            } // catch

            return GetBrokenFile(logoSize);
        } // GetImage

        public string GetLogoIconPath()
        {
            var path = System.IO.Path.Combine(Path, File + ".ico");

            return System.IO.File.Exists(path)? path : null;
        } // GetLogoIconPath

        public static Image GetBrokenFile(LogoSize logoSize)
        {
            switch (logoSize)
            {
                case LogoSize.Size32: return Properties.Resources.BrokenFile_32;
                case LogoSize.Size48: return Properties.Resources.BrokenFile_48;
                case LogoSize.Size64: return Properties.Resources.BrokenFile_64;
                case LogoSize.Size96: return Properties.Resources.BrokenFile_96;
                case LogoSize.Size128: return Properties.Resources.BrokenFile_128;
                case LogoSize.Size256: return Properties.Resources.BrokenFile_256;
            } // switch

            return null;
        } // GetBrokenFile

        protected virtual bool IsSizeAvailable(LogoSize logoSize)
        {
            switch (logoSize)
            {
                case LogoSize.Size32:
                case LogoSize.Size48:
                case LogoSize.Size64:
                case LogoSize.Size96:
                case LogoSize.Size128:
                case LogoSize.Size256:
                    return true;
                default:
                    return false;
            } // switch
        } // IsSizeAvailable

        protected virtual string GetSizeSufix(LogoSize logoSize)
        {
            switch (logoSize)
            {
                case LogoSize.Size32: return "@32";
                case LogoSize.Size48: return "@48";
                case LogoSize.Size64: return "@64";
                case LogoSize.Size96: return "@96";
                case LogoSize.Size128: return "@128";
                case LogoSize.Size256: return "@256";
                default:
                    throw new ArgumentOutOfRangeException("LogoSize logoSize");
            } // switch
        } // GetSizeSufix

        protected abstract string ImageNotFoundExceptionText
        {
            get;
        } // ImageNotFoundExceptionText

        protected abstract string ImageLoadExceptionText
        {
            get;
        } // ImageLoadExceptionText
    } // class BaseLogo
} // namespace
