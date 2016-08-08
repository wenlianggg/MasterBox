using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MasterBox.Auth {
	public class IPBlock {

		private static volatile IPBlock _instance;
		private static object syncRoot = new object();
		internal List<IPBlockEntry> bList;

		private static Dictionary<string, int> _ipFailedLogins;

		public static IPBlock Instance {
			get {
				if (_instance == null) {
					lock (syncRoot)
						if (_instance == null)
							_instance = new IPBlock();
				}
				return _instance;
			}
		}

		private static IPBlock RefreshInstance() {
			_instance = null;
			return Instance;
		}

		public void FailedLoginAttempt(int userid) {
			if (_ipFailedLogins == null)
				_ipFailedLogins = new Dictionary<string, int>();
			int failedlogins;
			string ipaddress = GetIP();
			if (_ipFailedLogins.TryGetValue(ipaddress, out failedlogins)) {
				// Has entry in IP block list
				if (_ipFailedLogins[ipaddress] < 5) {
					// Less the 5 failed login attempts, increment
					_ipFailedLogins[GetIP()]++;
				} else if (_ipFailedLogins[ipaddress] >= 5 && _ipFailedLogins[ipaddress] <= 10) {
					// More than 5 attempts, ban IP address and name combination, then increment
					_ipFailedLogins[GetIP()]++;
					Instance.Create(userid, ipaddress, TimeSpan.FromMinutes(15), "You may not login to this user from this IP");
				} else {
					// More than 10 attempts, ban IP address entirely, then increment;
					Instance.Create(ipaddress, TimeSpan.FromHours(1), "IP blocked for too many failed attempts");
				}
			} else {
				// Add entry to failed login attempts
				_ipFailedLogins.Add(ipaddress, 1);
			}
		}

		// Interface for checking if user is blocked
		public string CheckUser(string username) {
            try {
                IPBlock ipb = RefreshInstance();
                int uid = User.ConvertToId(username);
                if (ipb.Check(GetIP()) != null) {
                    return ipb.Check(GetIP());
                } else if (ipb.Check(uid, GetIP()) != null) {
                    return ipb.Check(uid, GetIP());
                } else if (ipb.Check(uid) != null) {
                    return ipb.Check(uid);
                }
            } catch (UserNotFoundException) {
                return "Invalid credentials, please try again!";
            } catch (Exception) {
                return "Error occured while checking your user";
            }
			return null;
		}


		private IPBlock() {
			using (DataAccess da = new DataAccess()) {
				SqlDataReader sqldr = da.SqlGetBlockList();
				bList = new List<IPBlockEntry>();
				while (sqldr.Read()) {
					if ((DateTime) sqldr["expiry"] > DateTime.Now) {
						IPBlockEntry ipbe = new IPBlockEntry(
							userid: (int)sqldr["userid"],
							ipaddress: sqldr["address"].ToString(),
							expiry: (DateTime)sqldr["expiry"],
							reason: sqldr["reason"].ToString());
						bList.Add(ipbe);
					}
				}
			}
		}

		// Username and IP combination block check
		internal string Check(int userid, string ip) {
			foreach (IPBlockEntry ipbe in bList) {
				if (ipbe.UserID.Equals(userid) && ipbe.IPAddress.Equals(ip))
					return ipbe.Reason;
			}
			return null;
		}

		// Username only block check
		internal string Check(int userid) {
			foreach (IPBlockEntry ipbe in bList) {
				if (ipbe.UserID.Equals(userid) && ipbe.IPAddress == "")
					return ipbe.Reason;
			}
			return null;
		}

		// IP only block check
		internal string Check(string ip) {
			foreach (IPBlockEntry ipbe in bList) {
				if (ipbe.IPAddress.Equals(ip) && ipbe.UserID == 0)
					return ipbe.Reason;
			}
			return null;
		}

		// Adding to blocklist (IP Address Only)
		internal void Create(string ip, TimeSpan? ts = null, string reason = "Unspecified reason") {
			DateTime expiry;
			if (ts != null)
				expiry = DateTime.Now.Add(ts.Value);
			else
				expiry = DateTime.Now.AddYears(99);
			IPBlockEntry newipbe = new IPBlockEntry(0, ip, expiry, reason);
			using (DataAccess da = new DataAccess()) {
				da.SqlInsertBlockEntry(newipbe);
			}
			RefreshInstance();
		}

		// Adding to blocklist (User ID Only)
		internal void Create(int userid, TimeSpan? ts = null, string reason = "Unspecified reason") {
			DateTime expiry;
			if (ts != null)
				expiry = DateTime.Now.Add(ts.Value);
			else
				expiry = DateTime.Now.AddYears(99);
			IPBlockEntry newipbe = new IPBlockEntry(userid, expiry: expiry, reason: reason);
			using (DataAccess da = new DataAccess()) {
				da.SqlInsertBlockEntry(newipbe);
			}
			RefreshInstance();
		}

		// Adding to blocklist (User ID and IP address combination)
		internal void Create(int userid, string ipaddress, TimeSpan? ts = null, string reason = "Unspecified reason") {
			DateTime expiry;
			if (ts != null)
				expiry = DateTime.Now.Add(ts.Value);
			else
				expiry = DateTime.Now.AddYears(99);
			IPBlockEntry newipbe = new IPBlockEntry(userid, ipaddress, expiry, reason);
			using (DataAccess da = new DataAccess()) {
				da.SqlInsertBlockEntry(newipbe);
			}
			RefreshInstance();
		}

		internal string GetIP() {
			HttpContext context = HttpContext.Current;
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

			return userIp;
		}
	}
}