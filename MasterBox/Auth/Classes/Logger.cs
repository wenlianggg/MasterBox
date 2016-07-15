using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace MasterBox.Auth {

    internal enum LogLevel { Normal, Security, Changed, Error, Global }

    /*
    * Log Levels
    * 0 - Normal login logging
    * 1 - Security related access
    * 2 - Something was changed
    * 3 - Client error occured
    * 4 - Server error occured
    */

    public abstract class Logger {

        private static volatile Logger _instance;
        private static object syncRoot = new object();

        protected Logger() { }

        internal void LogAuthEntry(int userid, string ipaddress, string description, LogLevel loglevel) {
            using (DataAccess da = new DataAccess()) {
                da.SqlInsertLogEntry(userid, ipaddress, description, (int) loglevel, 0);
            }
        }

        internal void LogFileEntry(int userid, string ipaddress, string description, LogLevel loglevel) {
            using (DataAccess da = new DataAccess()) {
                da.SqlInsertLogEntry(userid, ipaddress, description, (int)loglevel, 1);
            }
        }

        internal void LogTransactEntry(int userid, string ipaddress, string description, LogLevel loglevel) {
            using (DataAccess da = new DataAccess()) {
                da.SqlInsertLogEntry(userid, ipaddress, description, (int)loglevel, 2);
            }
        }

        internal DataTable GetUserLogs(int userid, int logtype) {
            DataTable dt = new DataTable();
            using (DataAccess da = new DataAccess())
                dt.Load(da.SqlGetUserLogs(userid, logtype));
            dt.Columns["logdesc"].ColumnName = "Log Description";
            dt.Columns["logip"].ColumnName = "Logged IP";
            dt.Columns["loglevel"].ColumnName = "Severity";
            dt.Columns["logtime"].ColumnName = "Time";
            return dt;
        }

        protected string GetIP() {
            HttpContext context = HttpContext.Current;
            HttpBrowserCapabilities browser = HttpContext.Current.Request.Browser;
            string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
            string userIp = "NIL";

            if (!string.IsNullOrEmpty(ipAddress)) {
                string[] addresses = ipAddress.Split(',');
                if (addresses.Length != 0) {
                    userIp = addresses[0];
                }
            } else {
                userIp = context.Request.ServerVariables["REMOTE_ADDR"];
            }

            if (userIp.Equals("::1")) {
                userIp = "127.0.0.1";
            }

            return browser.Type + " FROM " + userIp;
        }
    }
}