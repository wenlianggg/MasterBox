using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MasterBox.Auth {
	public class IPBlock {

		private IPBlock _instance;
		private List<IPBlockEntry> bList;

		public IPBlock Instance {
			get {
				if (_instance == null)
					_instance = new IPBlock();
				return _instance;
			}
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
		internal bool Check(int userid, string ip) {
			foreach (IPBlockEntry ipbe in bList) {
				if (ipbe.UserID.Equals(userid) && ipbe.IPAddress.Equals(ip))
					return true;
			}
			return false;
		}

		// Username only block check
		internal bool Check(int userid) {
			foreach (IPBlockEntry ipbe in bList) {
				if (ipbe.UserID.Equals(userid) && ipbe.IPAddress == null)
					return true;
			}
			return false;
		}

		// IP only block check
		internal bool Check(string ip) {
			foreach (IPBlockEntry ipbe in bList) {
				if (ipbe.IPAddress.Equals(ip) && ipbe.UserID == 0)
					return true;
			}
			return false;
		}

		// Adding to blocklist (IP Address Only)
		internal void Create(string ip, TimeSpan? ts = null, string reason = "Unspecified reason") {
			DateTime expiry;
			if (ts != null)
				expiry = DateTime.Now.Add(ts.Value);
			else
				expiry = DateTime.Now.AddYears(99);
			IPBlockEntry newipbe = new IPBlockEntry(0, ip, expiry, reason);
			
		}

		// Adding to blocklist (User ID Only)
		internal void Create(int userid, TimeSpan? ts = null, string reason = "Unspecified reason") {
			DateTime expiry;
			if (ts != null)
				expiry = DateTime.Now.Add(ts.Value);
			else
				expiry = DateTime.Now.AddYears(99);
			IPBlockEntry newipbe = new IPBlockEntry(0, expiry: expiry, reason: reason);
		}

		// Adding to blocklist (User ID and IP address combination)
		internal void Create(int userid, string ipaddress, TimeSpan? ts = null, string reason = "Unspecified reason") {
			DateTime expiry;
			if (ts != null)
				expiry = DateTime.Now.Add(ts.Value);
			else
				expiry = DateTime.Now.AddYears(99);
			IPBlockEntry newipbe = new IPBlockEntry(0, ipaddress, expiry, reason);
		}
		
		// TODO: Blocklist removal

	}
}