using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterBox.Auth {


	public class Logger {

		public enum LogLevel {Normal, Changed, Notable, Error, Global}
		/*
		 * Log Levels
		 * 1 - Normal login logging
		 * 2 - Something was changed
		 * 3 - Notable issue has occured
		 * 4 - A severe error occured
		 * 5 - A large change was done
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



	}
}