using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MasterBox.Auth {
	public class IPBlock {

		private IPBlock _instance;
		private List<IPBlockEntry> _blocklist;

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
				_blocklist = new List<IPBlockEntry>();
				while (sqldr.Read()) {
					if ((DateTime) sqldr["expiry"] > DateTime.Now) {
						IPBlockEntry ipbe = new IPBlockEntry(
							userid: (int)sqldr["userid"],
							ipaddress: sqldr["address"].ToString(),
							expiry: (DateTime)sqldr["expiry"],
							reason: sqldr["reason"].ToString());
						_blocklist.Add(ipbe);
					}
				}
			}
		}

		// Username and IP combination block check
		internal bool Check(int userid, string ip) {
			foreach (IPBlockEntry ipbe in _blocklist) {
				if (ipbe.UserID.Equals(userid) && ipbe.IPAddress.Equals(ip))
					return true;
			}
			return false;
		}

		// Username only block check
		internal bool Check(int userid) {
			foreach (IPBlockEntry ipbe in _blocklist) {
				if (ipbe.UserID.Equals(userid) && ipbe.IPAddress == null)
					return true;
			}
			return false;
		}

		// IP only block check
		internal bool Check(string ip) {
			foreach (IPBlockEntry ipbe in _blocklist) {
				if (ipbe.IPAddress.Equals(ip) && ipbe.UserID == 0)
					return true;
			}
			return false;
		}

		// TODO: Adding to blocklist

		// TODO: Removing from blocklist

	}
}