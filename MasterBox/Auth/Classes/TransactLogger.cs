using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MasterBox.Auth {

	public sealed class TransactLogger : Logger {

		private static volatile TransactLogger _instance;
		private static object syncRoot = new object();

		private TransactLogger() { }

		public static TransactLogger Instance {
			get {
				if (_instance == null) {
					lock (syncRoot)
						if (_instance == null)
							_instance = new TransactLogger();
				}
				return _instance;
			}
		}


		internal void TransactionCompleted(int userid, int megs) {
			string description = "Transaction has been completed for " + megs + "MB";
			using (DataAccess da = new DataAccess()) {
                LogTransactEntry(userid, GetIP(), description, LogLevel.Normal);
			}
		}

		internal void TransactionFailed(int userid) {
			string description = "Transaction attempt failed for when attempting to purchase subscription";
			using (DataAccess da = new DataAccess()) {
                LogTransactEntry(userid, GetIP(), description, LogLevel.Error);
			}
		}

        internal void SubscriptionExpired(int userid) {
            string description = "MasterBox subscription has expired";
            using (DataAccess da = new DataAccess()) {
                LogTransactEntry(userid, GetIP(), description, LogLevel.Changed);
            }
        }

        internal void SubscriptionRenewed(int userid) {
            string description = "MasterBox subscription has been renewed";
            using (DataAccess da = new DataAccess()) {
                LogTransactEntry(userid, GetIP(), description, LogLevel.Error);
            }
        }

        internal void SubscriptionUpgraded(int userid, int newmegs) {
            string description = "MasterBox subscription has been upgraded to " + newmegs + "MB";
            using (DataAccess da = new DataAccess()) {
                LogTransactEntry(userid, GetIP(), description, LogLevel.Normal);
            }
        }

        internal void SubscriptionDowngraded(int userid, int newmegs) {
            string description = "MasterBox subscription has been downgraded to " + newmegs + "MB";
            using (DataAccess da = new DataAccess()) {
                LogTransactEntry(userid, GetIP(), description, LogLevel.Normal);
            }
        }


        internal DataTable GetUserLogs(int userid) {
            // Get all log entries of type transact
            return GetUserLogs(userid, 2);
        }

	}
}