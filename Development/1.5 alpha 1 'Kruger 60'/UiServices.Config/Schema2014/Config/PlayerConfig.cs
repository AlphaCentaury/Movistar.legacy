﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Project.DvbIpTv.UiServices.Configuration.Schema2014.Config
{
    [Serializable]
    public class PlayerConfig
    {
        [XmlAttribute("name")]
        public string Name
        {
            get;
            set;
        } // Name

        public string Path
        {
            get;
            set;
        } // Path

        [XmlArray("Arguments", Namespace = ConfigCommon.ConfigXmlNamespace)]
        [XmlArrayItem("Arg")]
        public string[] Arguments
        {
            get;
            set;
        } // Parameters

        public string Validate(string ownerTag)
        {
            Name = ConfigCommon.Normalize(Name);
            if (string.IsNullOrEmpty(Name))
            {
                return ConfigCommon.ErrorMissingEmptyAttribute("name", ownerTag);
            } // if

            Path = ConfigCommon.Normalize(Path);
            if (string.IsNullOrEmpty(Path))
            {
                return ConfigCommon.ErrorMissingEmpty("Path", ownerTag, "name", Name);
            } // if
            if (!System.IO.File.Exists(Path))
            {
                return string.Format(Properties.Texts.PlayerConfigValidationPathNotFound, Name, Path);
            } // if

            var validationError = ConfigCommon.ValidateArray(Arguments, "Argument", "Arguments", ownerTag);
            if (validationError != null) return validationError;

            for (int index=0; index<Arguments.Length ; index++)
            {
                Arguments[index] = ConfigCommon.Normalize(Arguments[index]);
                if (string.IsNullOrEmpty(Arguments[index]))
                {
                    ConfigCommon.ErrorMissingEmpty("Argument", "Arguments"); ;
                } // if
            } // for

            return null;
        } // Validate

        public static string ValidateArray(PlayerConfig[] players, string arrayElementTag, string arrayTag, string ownerTag)
        {
            var validationError = ConfigCommon.ValidateArray(players, arrayElementTag, arrayTag, ownerTag);
            if (validationError != null) return validationError;

            foreach (var player in players)
            {
                validationError = player.Validate(arrayElementTag);
                if (validationError != null) return validationError;
            } // foreach

            return null;
        } // ValidateArray
    } // class PlayerConfig
} // namespace
