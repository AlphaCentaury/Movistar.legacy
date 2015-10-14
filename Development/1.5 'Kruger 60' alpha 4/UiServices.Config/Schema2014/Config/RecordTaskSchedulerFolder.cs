﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Project.IpTv.UiServices.Configuration.Schema2014.Config
{
    [Serializable]
    [XmlType(TypeName = "RecordTaskSchedulerFolder", Namespace = ConfigCommon.ConfigXmlNamespace)]
    public class RecordTaskSchedulerFolder : StringPair
    {
        public RecordTaskSchedulerFolder()
            : base()
        {
            // no op
        } // constructor

        public RecordTaskSchedulerFolder(string name, string path)
        {
            Name = name;
            Path = path;
        } // constructor

        [XmlAttribute("displayName")]
        public string Name
        {
            get { return Item1; }
            set { Item1 = ConfigCommon.Normalize(value); }
        } // Name

        [XmlText()]
        public string Path
        {
            get { return Item2; }
            set { Item2 = ConfigCommon.Normalize(value); }
        } // Value

        public string Validate(string ownerTag)
        {
            if (string.IsNullOrEmpty(Name))
            {
                return ConfigCommon.ErrorMissingEmptyAttribute("name", ownerTag);
            } // if
            if (string.IsNullOrEmpty(Path))
            {
                return ConfigCommon.ErrorMissingEmpty(ownerTag);
            } // if
            if ((!Path.StartsWith("\\")) || (Path.EndsWith("\\")))
            {
                return string.Format(Properties.Texts.RecordTaskSchedulerFolderValidationPath, Path, ownerTag);
            } // if

            return null;
        } // Validate

        public static string ValidateArray(RecordTaskSchedulerFolder[] folders, string arrayElementTag, string arrayTag, string ownerTag)
        {
            if ((folders == null) || (folders.Length < 1)) return null;

            var set = new HashSet<string>();
            foreach (var folder in folders)
            {
                var validationError = folder.Validate(arrayElementTag);
                if (validationError != null) return validationError;

                if (!set.Add(folder.Name))
                {
                    return ConfigCommon.ErrorDuplicatedEntry(arrayTag, arrayElementTag, "name", folder.Name);
                } // if
            } // foreach

            return null;
        } // ValidateArray
    } // class RecordTaskSchedulerFolder
} // namespace
