﻿// Copyright (C) 2014-2016, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Project.IpTv.UiServices.Configuration.Editors
{
    public partial class ArgumentEditor : UserControl
    {
        public ArgumentEditor()
        {
            InitializeComponent();
        } // constructor

        public event EventHandler CommandLineChanged;

        public string OpenBraceText
        {
            get;
            set;
        } // OpenBraceText

        public string CloseBraceText
        {
            get;
            set;
        } // CloseBraceText

        public string ParametersList
        {
            get;
            set;
        } // ParametersList

        public string CommandLine
        {
            get { return textBoxCommandLine.Text; }
            set { textBoxCommandLine.Text = value; }
        } // CommandLine

        private void ParametersEditor_Load(object sender, EventArgs e)
        {
            char[] dataSeparator = new char[] { '|' };

            if (!string.IsNullOrEmpty(ParametersList))
            {
                var lines = ParametersList.Split(new string[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries);

                listParameters.BeginUpdate();
                foreach (var line in lines)
                {
                    var data = line.Split(dataSeparator, StringSplitOptions.RemoveEmptyEntries);
                    if (data.Length > 0)
                    {
                        var item = listParameters.Items.Add(data[0]);
                        if (data.Length > 1)
                        {
                            item.SubItems.Add(data[1]);
                        } // if
                    } // if
                } // foreach
                listParameters.EndUpdate();

                for (int index = 0; index < listParameters.Columns.Count; index++)
                {
                    listParameters.Columns[index].AutoResize(ColumnHeaderAutoResizeStyle.ColumnContent);
                } // for
            } // if

            buttonAddParam.Enabled = false;
            textBoxCommandLine.SelectionStart = textBoxCommandLine.Text.Length;
            textBoxCommandLine.SelectionLength = 0;
        } // ParametersEditor_Load

        private void textBoxCommandLine_TextChanged(object sender, EventArgs e)
        {
            if (CommandLineChanged != null)
            {
                CommandLineChanged(this, EventArgs.Empty);
            } // if
        } // textBoxCommandLine_TextChanged

        private void listParameters_SelectedIndexChanged(object sender, EventArgs e)
        {
            buttonAddParam.Enabled = listParameters.SelectedIndices.Count > 0;
        } // listParameters_SelectedIndexChanged

        private void buttonAddParam_Click(object sender, EventArgs e)
        {
            AddParameter();
        } // buttonAddParam_Click

        private void listParameters_DoubleClick(object sender, EventArgs e)
        {
            if (listParameters.SelectedIndices.Count > 0)
            {
                AddParameter();
            } // if
        } // listParameters_DoubleClick

        private void AddParameter()
        {
            var parameter = string.Format("{0}{1}{2}", OpenBraceText, listParameters.SelectedItems[0].Text, CloseBraceText);
            textBoxCommandLine.Paste(parameter);
        } // AddParameter
    } // class ArgumentEditor
} // namespace
