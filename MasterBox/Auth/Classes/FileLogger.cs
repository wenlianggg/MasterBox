using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MasterBox.Auth {

    public sealed class FileLogger : Logger {

        private static volatile FileLogger _instance;
        private static object syncRoot = new object();

        private FileLogger() { }

        public static FileLogger Instance {
            get {
                if (_instance == null) {
                    lock (syncRoot)
                        if (_instance == null)
                            _instance = new FileLogger();
                }
                return _instance;
            }
        }
    
        internal void FolderCreated(int userid, string foldername) {
            string description = "Folder created: " + foldername;
            using (DataAccess da = new DataAccess()) {
                LogFileEntry(userid, GetIP(), description, LogLevel.Normal);
            }
        }

        internal void FolderEncryptionChanged(int userid, string foldername) {
            string description = "Folder encryption password changed: " + foldername;
            using (DataAccess da = new DataAccess()) {
                LogFileEntry(userid, GetIP(), description, LogLevel.Changed);
            }
        }

        internal void FolderEncryptionAdded(int userid, string foldername) {
            string description = "Folder additionally encrypted: " + foldername;
            using (DataAccess da = new DataAccess()) {
                LogFileEntry(userid, GetIP(), description, LogLevel.Changed);
            }
        }

        internal void FolderEncryptionRemoved(int userid, string foldername) {
            string description = "Folder encryption removed: " + foldername;
            using (DataAccess da = new DataAccess()) {
                LogFileEntry(userid, GetIP(), description, LogLevel.Changed);
            }
        }

        internal void FolderDeleted(int userid, string foldername) {
            string description = "Folder deleted: " + foldername;
            using (DataAccess da = new DataAccess()) {
                LogFileEntry(userid, GetIP(), description, LogLevel.Normal);
            }
        }


        internal void FileDownloaded(int userid, string filename) {
            string description = "File downloaded: " + filename;
            using (DataAccess da = new DataAccess()) {
                LogFileEntry(userid, GetIP(), description, LogLevel.Normal);
            }
        }


        internal void FileUploaded(int userid, string filename) {
            string description = "File uploaded: " + filename;
            using (DataAccess da = new DataAccess()) {
                LogFileEntry(userid, GetIP(), description, LogLevel.Normal);
            }
        }

        internal void FileDeleted(int userid, string filename) {
            string description = "File deleted: " + filename;
            using (DataAccess da = new DataAccess()) {
                LogFileEntry(userid, GetIP(), description, LogLevel.Changed);
            }
        }

        internal DataTable GetUserLogs(int userid) {
            // Get all log entries of type file
            return GetUserLogs(userid, 1);
        }

    }
}