﻿// Copyright (C) 2014-2015, Codeplex user AlphaCentaury
// All rights reserved, except those granted by the governing license of this software. See 'license.txt' file in the project root for complete license information.

namespace Project.DvbIpTv.UiServices.Record
{
    partial class RecordTasksDialog
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RecordTasksDialog));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.listViewTasks = new Project.DvbIpTv.UiServices.Controls.ListViewSortable();
            this.columnTaskChannel = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTaskName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTaskSchedule = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnTaskRecordings = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.textBoxTaskDetails = new System.Windows.Forms.TextBox();
            this.buttonEditTask = new System.Windows.Forms.Button();
            this.buttonDeleteTasks = new System.Windows.Forms.Button();
            this.buttonOk = new System.Windows.Forms.Button();
            this.buttonViewRecordings = new System.Windows.Forms.Button();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            resources.ApplyResources(this.splitContainer1, "splitContainer1");
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            resources.ApplyResources(this.splitContainer1.Panel1, "splitContainer1.Panel1");
            this.splitContainer1.Panel1.Controls.Add(this.listViewTasks);
            // 
            // splitContainer1.Panel2
            // 
            resources.ApplyResources(this.splitContainer1.Panel2, "splitContainer1.Panel2");
            this.splitContainer1.Panel2.Controls.Add(this.textBoxTaskDetails);
            // 
            // listViewTasks
            // 
            resources.ApplyResources(this.listViewTasks, "listViewTasks");
            this.listViewTasks.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnTaskChannel,
            this.columnTaskName,
            this.columnTaskSchedule,
            this.columnTaskRecordings});
            this.listViewTasks.FullRowSelect = true;
            this.listViewTasks.GridLines = true;
            this.listViewTasks.HeaderCustomFont = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.listViewTasks.HeaderCustomForeColor = System.Drawing.Color.Empty;
            this.listViewTasks.HeaderCustomTextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            this.listViewTasks.HeaderUsesCustomFont = true;
            this.listViewTasks.HeaderUsesCustomTextAlignment = true;
            this.listViewTasks.HideSelection = false;
            this.listViewTasks.IsDoubleBuffered = true;
            this.listViewTasks.Name = "listViewTasks";
            this.listViewTasks.OwnerDraw = true;
            this.listViewTasks.UseCompatibleStateImageBehavior = false;
            this.listViewTasks.View = System.Windows.Forms.View.Details;
            this.listViewTasks.SelectedIndexChanged += new System.EventHandler(this.listViewTasks_SelectedIndexChanged);
            // 
            // columnTaskChannel
            // 
            resources.ApplyResources(this.columnTaskChannel, "columnTaskChannel");
            // 
            // columnTaskName
            // 
            resources.ApplyResources(this.columnTaskName, "columnTaskName");
            // 
            // columnTaskSchedule
            // 
            resources.ApplyResources(this.columnTaskSchedule, "columnTaskSchedule");
            // 
            // columnTaskRecordings
            // 
            resources.ApplyResources(this.columnTaskRecordings, "columnTaskRecordings");
            // 
            // textBoxTaskDetails
            // 
            resources.ApplyResources(this.textBoxTaskDetails, "textBoxTaskDetails");
            this.textBoxTaskDetails.BackColor = System.Drawing.SystemColors.Window;
            this.textBoxTaskDetails.Name = "textBoxTaskDetails";
            this.textBoxTaskDetails.ReadOnly = true;
            // 
            // buttonEditTask
            // 
            resources.ApplyResources(this.buttonEditTask, "buttonEditTask");
            this.buttonEditTask.Name = "buttonEditTask";
            this.buttonEditTask.UseVisualStyleBackColor = true;
            this.buttonEditTask.Click += new System.EventHandler(this.buttonEditTask_Click);
            // 
            // buttonDeleteTasks
            // 
            resources.ApplyResources(this.buttonDeleteTasks, "buttonDeleteTasks");
            this.buttonDeleteTasks.CausesValidation = false;
            this.buttonDeleteTasks.Name = "buttonDeleteTasks";
            this.buttonDeleteTasks.UseVisualStyleBackColor = true;
            this.buttonDeleteTasks.Click += new System.EventHandler(this.buttonDeleteTasks_Click);
            // 
            // buttonOk
            // 
            resources.ApplyResources(this.buttonOk, "buttonOk");
            this.buttonOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.buttonOk.Name = "buttonOk";
            this.buttonOk.UseVisualStyleBackColor = true;
            // 
            // buttonViewRecordings
            // 
            resources.ApplyResources(this.buttonViewRecordings, "buttonViewRecordings");
            this.buttonViewRecordings.CausesValidation = false;
            this.buttonViewRecordings.Name = "buttonViewRecordings";
            this.buttonViewRecordings.UseVisualStyleBackColor = true;
            this.buttonViewRecordings.Click += new System.EventHandler(this.buttonViewRecordings_Click);
            // 
            // RecordTasksDialog
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.buttonOk;
            this.Controls.Add(this.buttonViewRecordings);
            this.Controls.Add(this.buttonOk);
            this.Controls.Add(this.buttonDeleteTasks);
            this.Controls.Add(this.buttonEditTask);
            this.Controls.Add(this.splitContainer1);
            this.MinimizeBox = false;
            this.Name = "RecordTasksDialog";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.Shown += new System.EventHandler(this.RecordTasksDialog_Shown);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private global::Project.DvbIpTv.UiServices.Controls.ListViewSortable listViewTasks;
        private System.Windows.Forms.ColumnHeader columnTaskName;
        private System.Windows.Forms.ColumnHeader columnTaskChannel;
        private System.Windows.Forms.TextBox textBoxTaskDetails;
        private System.Windows.Forms.ColumnHeader columnTaskSchedule;
        private System.Windows.Forms.Button buttonEditTask;
        private System.Windows.Forms.Button buttonDeleteTasks;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.ColumnHeader columnTaskRecordings;
        private System.Windows.Forms.Button buttonViewRecordings;
    }
}