﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Project.DvbIpTv.UiServices.Common.Controls;

namespace Project.DvbIpTv.UiServices.Discovery
{
    internal partial class SettingsEditorModeMultiColumn : UserControl, ISettingsEditorModeColumns
    {
        ListItemsManager<UiBroadcastListColumn> ItemsManager;

        public SettingsEditorModeMultiColumn()
        {
            InitializeComponent();
        } // constructor

        public List<KeyValuePair<UiBroadcastListColumn, string>> ColumnsList
        {
            private get;
            set;
        } // Columns

        public List<KeyValuePair<UiBroadcastListColumn, string>> ColumnsNoneList
        {
            private get;
            set;
        } // Columns

        public IList<UiBroadcastListColumn> Columns
        {
            private get;
            set;
        } // Columns

        public IList<UiBroadcastListColumn> SelectedColumns
        {
            get { return ItemsManager.GetListValues(); }
        } // SelectedColumns

        public Control GetControl()
        {
            return this;
        } // GetControl

        private void SettingsEditorModeMultiColumn_Load(object sender, EventArgs e)
        {
            ItemsManager = new ListItemsManager<UiBroadcastListColumn>(listSelectedColumns, buttonRemove, buttonMoveUp, buttonMoveDown);

            comboColumns.DataSource = ColumnsList.AsReadOnly();
            buttonAddColumn.Enabled = (comboColumns.Items.Count > 0);

            ItemsManager.SetValueDictionary(ColumnsList, null);
            ItemsManager.Add(Columns);
        }  // SettingsEditorModeMultiColumn_Load

        private void buttonAddColumn_Click(object sender, EventArgs e)
        {
            ItemsManager.Add((UiBroadcastListColumn)comboColumns.SelectedValue);
        } // buttonAddColumn_Click

        private void buttonRemove_Click(object sender, EventArgs e)
        {
            ItemsManager.RemoveSelection();
        } // buttonRemove_Click

        private void buttonMoveUp_Click(object sender, EventArgs e)
        {
            ItemsManager.MoveSelectionUp();
        } // buttonMoveUp_Click

        private void buttonMoveDown_Click(object sender, EventArgs e)
        {
            ItemsManager.MoveSelectionDown();
        } // buttonMoveDown_Click
    } // class SettingsEditorModeMultiColumn
} // namespace
