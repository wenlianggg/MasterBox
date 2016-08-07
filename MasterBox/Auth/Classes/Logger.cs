using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
			SqlDataReader sqldr;
			using (DataAccess da = new DataAccess()) {
				sqldr = da.SqlGetUserLogs(userid, logtype);

				dt.Columns.Add("Log Description", typeof(string));
				dt.Columns.Add("Browser Info", typeof(string));
				dt.Columns.Add("Severity", typeof(string));
				dt.Columns.Add("Date/Time", typeof(string));

				while (sqldr.Read()) {
					string logdesc = sqldr["logdesc"].ToString();
					string browserinfo = sqldr["logip"].ToString();
					string severity = GetSeverityName((byte) sqldr["loglevel"]);
					string logtime = ((DateTime) sqldr["logtime"]).ToLocalTime().ToString("d MMM yyyy HH:mm");
					dt.Rows.Add(logdesc, browserinfo, severity, logtime);
				}
			}
			return dt;
        }

		internal static DataTable GetAllLogs() {
			DataTable dt = new DataTable();
			SqlDataReader sqldr;
			using (DataAccess da = new DataAccess()) {
				sqldr = da.SqlGetServerLogs();

				dt.Columns.Add("Log ID", typeof(int));
				dt.Columns.Add("Username", typeof(string));
				dt.Columns.Add("Log Description", typeof(string));
				dt.Columns.Add("Browser Info", typeof(string));
				dt.Columns.Add("Severity", typeof(string));
				dt.Columns.Add("Date/Time", typeof(string));

				while (sqldr.Read()) {
					int logid = (int)sqldr["logid"];
					string username = User.ConvertToUserName((int)sqldr["userid"]);
					string logdesc = sqldr["logdesc"].ToString();
					string browserinfo = sqldr["logip"].ToString();
					string severity = GetSeverityName((byte)sqldr["loglevel"]);
					string logtime = ((DateTime)sqldr["logtime"]).ToLocalTime().ToString("d MMM yyyy HH:mm");
					dt.Rows.Add(logid, username, logdesc, browserinfo, severity, logtime);
				}
			}

			return dt;
		}

		protected static string GetSeverityName(int severity) {
			switch (severity) {
				case 0:
					return "Normal";
				case 1:
					return "Access";
				case 2:
					return "Change";
				case 3:
					return "ClientError";
				case 4:
					return "ServerError";
				default:
					return "-";
			}
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