using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MasterBox.Auth {

	public sealed class AuthLogger : Logger {

		private static volatile AuthLogger _instance;
		private static object syncRoot = new object();

		private AuthLogger() { }

		public static AuthLogger Instance {
			get {
				if (_instance == null) {
					lock (syncRoot)
						if (_instance == null)
							_instance = new AuthLogger();
				}
				return _instance;
			}
		}


		internal void FailedLoginAttempt(int userid) {
			string description = "Unsuccessful login attempt";
			TriggerFailedLogin(userid);
			using (DataAccess da = new DataAccess()) {
                LogAuthEntry(userid, GetIP(), description, LogLevel.Security);
			}
		}

		internal void FailedTotpAttempt(int userid) {
			string description = "Unsuccessful 2FA (TOTP) attempt";
			TriggerFailedLogin(userid);
			using (DataAccess da = new DataAccess()) {
                LogAuthEntry(userid, GetIP(), description, LogLevel.Security);
			}
		}

		internal void FailedTotpChangeAttempt(int userid) {
			string description = "Unsuccessful 2FA (TOTP) change attempt";
			TriggerFailedLogin(userid);
			using (DataAccess da = new DataAccess()) {
				LogAuthEntry(userid, GetIP(), description, LogLevel.Security);
			}
		}

		internal void TotpDisabled(int userid) {
			string description = "Unsuccessful 2FA (TOTP) disabled";
			TriggerFailedLogin(userid);
			using (DataAccess da = new DataAccess()) {
				LogAuthEntry(userid, GetIP(), description, LogLevel.Security);
			}
		}

		internal void UserTotpChanged(int userid) {
			string description = "2FA (TOTP) key was changed";
			TriggerFailedLogin(userid);
			using (DataAccess da = new DataAccess()) {
				LogAuthEntry(userid, GetIP(), description, LogLevel.Changed);
			}
		}

        internal void UserPassChanged(int userid) {
            string description = "User password was changed";
            using (DataAccess da = new DataAccess()) {
                LogAuthEntry(userid, GetIP(), description, LogLevel.Changed);
            }
        }

        internal void UserPassChangeFailed(int userid) {
            string description = "Unsuccessful password changing attempt";
            using (DataAccess da = new DataAccess()) {
                LogAuthEntry(userid, GetIP(), description, LogLevel.Error);
            }
        }

        internal void SuccessfulLogin(int userid) {
            string description = "Login was successful";
            using (DataAccess da = new DataAccess()) {
                LogAuthEntry(userid, GetIP(), description, LogLevel.Normal);
            }
        }


		private void TriggerFailedLogin(int userid) {
			IPBlock.Instance.FailedLoginAttempt(userid);
		}

        internal DataTable GetUserLogs(int userid) {
            // Get all log entries of type auth
            return GetUserLogs(userid, 0);
        }

	}
}