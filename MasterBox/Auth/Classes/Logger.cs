using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MasterBox.Auth {


	public class Logger {

		public enum LogLevel {Normal, Security, Changed, Error, Global}
		/*
		 * Log Levels
		 * 0 - Normal login logging
		 * 1 - Security related access
		 * 2 - Something was changed
		 * 3 - Client error occured
		 * 4 - Server error occured
		 */

		private DataAccess _da;
		private bool disposed = false;
		private static volatile Logger _instance;
		private static object syncRoot = new object();

		private Logger() { }

		public static Logger Instance {
			get {
				if (_instance == null) {
					lock (syncRoot)
						if (_instance == null)
							_instance = new Logger();
				}
				return _instance;
			}
		}

		internal Logger(DataAccess customDataAccess = null) { // Optional Parameter customDataAccess
			if (customDataAccess == null || _da == null) {
				_da = new DataAccess();
			}
		}

		protected void Dispose(bool disposing) {
			if (disposed)
				throw new ObjectDisposedException("Logger");
			if (disposing) {
				_da.Dispose();
				disposed = true;
			}
		}

		internal void FailedLoginAttempt(int userid) {
			string description = "Unsuccessful login attempt";
			string ipaddress = GetIP();
			using (DataAccess da = new DataAccess()) {
				da.SqlInsertLogEntry(userid, ipaddress, description, 1);
			} 
		}

		private string GetIP() {
			HttpContext context = HttpContext.Current;
			string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];

			if (!string.IsNullOrEmpty(ipAddress)) {
				string[] addresses = ipAddress.Split(',');
				if (addresses.Length != 0) {
					return addresses[0];
				}
			}

			return context.Request.ServerVariables["REMOTE_ADDR"];
		}

	}
}