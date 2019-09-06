﻿using ALE.ETLBox.ConnectionManager;
using ALE.ETLBox.Helper;
using CF = ALE.ETLBox.ControlFlow;
using System;
using ALE.ETLBox.Logging;

namespace ALE.ETLBox {
    public abstract class GenericTask : ITask {
        public virtual string TaskType { get; set; } = "N/A";
        public virtual string TaskName { get; set; } = "N/A";
        public NLog.Logger NLogger { get; set; } = CF.ControlFlow.GetLogger();

        public virtual void Execute() {
            throw new Exception("Not implemented!");
        }

        public virtual IConnectionManager ConnectionManager { get; set; }

        internal virtual IDbConnectionManager DbConnectionManager {
            get {
                if (ConnectionManager == null)
                    return (IDbConnectionManager)ControlFlow.ControlFlow.CurrentDbConnection;
                else
                    return (IDbConnectionManager)ConnectionManager;
            }
        }

        public ConnectionManagerType ConnectionType
        {
            get
            {
                if (this.DbConnectionManager.GetType() == typeof(SqlConnectionManager) ||
                    this.DbConnectionManager.GetType() == typeof(SMOConnectionManager))
                    return ConnectionManagerType.SqlServer;
                else if (this.DbConnectionManager.GetType() == typeof(OdbcConnectionManager))
                    return ConnectionManagerType.Odbc;
                else if (this.DbConnectionManager.GetType() == typeof(AccessOdbcConnectionManager))
                    return ConnectionManagerType.Access;
                else if (this.DbConnectionManager.GetType() == typeof(AdomdConnectionManager))
                    return ConnectionManagerType.Adomd;
                else if (this.DbConnectionManager.GetType() == typeof(SQLiteConnectionManager))
                    return ConnectionManagerType.SQLLite;
                else return ConnectionManagerType.Unknown;
            }
        }

        public bool _disableLogging;
        public virtual bool DisableLogging {
            get {
                if (ControlFlow.ControlFlow.DisableAllLogging == false)
                    return _disableLogging;
                else
                    return ControlFlow.ControlFlow.DisableAllLogging;
            }
            set {
                _disableLogging = value;
            }
        }

        private string _taskHash;
        public virtual string TaskHash {
            get {
                if (_taskHash == null)
                    return HashHelper.Encrypt_Char40(this);
                else
                    return _taskHash;
            }
            set {
                _taskHash = value;
            }
        }
        internal virtual bool HasName => !String.IsNullOrWhiteSpace(TaskName);

        public GenericTask()
        { }

        public GenericTask(ITask callingTask)
        {
            TaskName = callingTask.TaskName;
            TaskHash = callingTask.TaskHash;
            ConnectionManager = callingTask.ConnectionManager;
            TaskType = callingTask.TaskType;
            DisableLogging = callingTask.DisableLogging;
        }
    }
}
