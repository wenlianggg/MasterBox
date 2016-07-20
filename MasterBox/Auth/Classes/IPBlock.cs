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

		// TODO: Checking against blocklist

		// TODO: Adding to blocklist

		// TODO: Removing from blocklist

	}
}