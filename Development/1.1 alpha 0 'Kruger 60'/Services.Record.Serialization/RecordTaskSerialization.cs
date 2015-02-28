﻿using Project.DvbIpTv.Common;
using Project.DvbIpTv.Services.SqlServerCE;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Project.DvbIpTv.Services.Record.Serialization
{
    public static class RecordTaskSerialization
    {
        public const int MaxTaskNameLength = 128;

        #region Load methods

        public static RecordTask LoadFromDatabase(string dbFile, Guid taskId)
        {
            return DbServices.LoadFromDatabase<RecordTask>(dbFile, GetDbLoadCommand(taskId), "XmlData");
        } // LoadFromDatabase

        public static RecordTask LoadFromDatabase(SqlCeConnection cn, Guid taskId)
        {
            return DbServices.LoadFromDatabase<RecordTask>(cn, GetDbLoadCommand(taskId), "XmlData");
        } // LoadFromDatabase

        public static RecordTask LoadFromXmlFile(string filename)
        {
            return XmlSerialization.Deserialize<RecordTask>(filename);
        } // LoadFromXmlFile

        public static RecordTask LoadFromXmlString(string xmlText)
        {
            return XmlSerialization.DeserializeXmlText<RecordTask>(xmlText);
        } // LoadFromXmlFile

        #endregion

        #region Save methods

        public static void SaveToDatabase(this RecordTask task, string dbFile)
        {
            var saveCmd = GetDbSaveCommand(task.TaskId,
                task.Description.Name,
                task.Description.TaskSchedulerName,
                task.AdvancedSettings.TaskSchedulerFolder,
                null);

            DbServices.SaveToDatabase(dbFile, saveCmd, "@XmlData", task);
        } // SaveToDatabase

        public static void SaveTo(this RecordTask task, SqlCeConnection cn)
        {
            var saveCmd = GetDbSaveCommand(task.TaskId,
                task.Description.Name,
                task.Description.TaskSchedulerName,
                task.AdvancedSettings.TaskSchedulerFolder,
                null);

            DbServices.SaveToDatabase(cn, saveCmd, "@XmlData", task);
        } // SaveTo

        public static void SaveToXmlFile(this RecordTask task, string filename)
        {
            XmlSerialization.Serialize(filename, task);
        } // SaveToXmlFile

        public static void SaveTo(this RecordTask task, Stream stream)
        {
            XmlSerialization.Serialize(stream, task);
        } // SaveTo

        public static string SaveAsString(this RecordTask task)
        {
            var buffer = new StringBuilder();
            task.SaveTo(buffer);
            return buffer.ToString();
        } // SaveAsString

        public static void SaveTo(this RecordTask task, StringBuilder buffer)
        {
            using (var writer = XmlWriter.Create(buffer, new XmlWriterSettings() { Indent = true }))
            {
                task.SaveTo(writer);
            } // using
        } // SaveTo

        public static void SaveTo(this RecordTask task, XmlWriter writer)
        {
            XmlSerialization.Serialize(writer, task);
        } // SaveTo

        #endregion

        #region Delete methods

        public static void DeleteFromDatabase(Guid taskId, string dbFile)
        {
            // TODO: Implement
            throw new NotImplementedException();
        } // DeleteFromDatabase

        public static bool TryDeleteFromDatabase(Guid taskId, string dbFile)
        {
            try
            {
                DeleteFromDatabase(taskId, dbFile);
                return true;
            }
            catch
            {
                return false;
            } // try-catch
        } // DeleteFromDatabase

        #endregion

        private static SqlCeConnection GetDbConnection(string dbFile)
        {
            SqlCeConnectionStringBuilder builder;

            builder = new SqlCeConnectionStringBuilder();
            builder.DataSource = dbFile;
            builder.Password = "movistartv.codeplex.com";

            var cn = new SqlCeConnection(builder.ConnectionString);
            cn.Open();

            return cn;
        } // GetDbConnection

        private static SqlCeCommand GetDbLoadCommand()
        {
            var cmd = new SqlCeCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "SELECT [XmlData] FROM [Tasks] WHERE [TaskId] = ?";
            cmd.Parameters.Add("@TaskId", SqlDbType.UniqueIdentifier);

            return cmd;
        } // GetDbLoadCommand

        private static SqlCeCommand GetDbLoadCommand(Guid taskId)
        {
            var cmd = GetDbLoadCommand();
            cmd.Parameters["@TaskId"].Value = taskId;

            return cmd;
        } // GetDbLoadCommand

        private static SqlCeCommand GetDbSaveCommand()
        {
            var cmd = new SqlCeCommand();
            cmd.CommandType = CommandType.Text;
            cmd.CommandText = "INSERT INTO [Tasks] (TaskId, TaskName, SchedulerName, SchedulerFolder, XmlData) VALUES (?, ?, ?, ?, ?)";
            cmd.Parameters.Add("@TaskId", SqlDbType.UniqueIdentifier);
            cmd.Parameters.Add("@TaskName", SqlDbType.NVarChar, MaxTaskNameLength);
            cmd.Parameters.Add("@SchedulerName", SqlDbType.NVarChar, 150);
            cmd.Parameters.Add("@SchedulerFolder", SqlDbType.NVarChar, 128);
            cmd.Parameters.Add("@XmlData", SqlDbType.Image);

            return cmd;
        } // GetDbSaveCommand

        private static SqlCeCommand GetDbSaveCommand(Guid taskId, string taskName, string schedulerName, string schedulerFolder, byte[] xmlData)
        {
            var cmd = GetDbSaveCommand();
            cmd.Parameters["@TaskId"].Value = taskId;
            cmd.Parameters["@TaskName"].Value = taskName;
            cmd.Parameters["@SchedulerName"].Value = schedulerName;
            cmd.Parameters["@SchedulerFolder"].Value = schedulerFolder;
            if (xmlData != null) cmd.Parameters["@XmlData"].Value = xmlData;

            return cmd;
        } // GetDbLoadCommand
    } // class RecordTaskSerialization
} // namespace
