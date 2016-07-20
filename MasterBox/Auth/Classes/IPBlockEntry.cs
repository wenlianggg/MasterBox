using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterBox.Auth {
	internal class IPBlockEntry {
		private int _userid;
		private string _ipaddress;
		private DateTime _expiry;
		private string _reason;

		internal IPBlockEntry(User user, string ipaddress = null, DateTime? expiry = null, string reason = null) :
			this(user.UserId, ipaddress, expiry, reason) {}

		internal IPBlockEntry(int userid, string ipaddress = null, DateTime? expiry = null, string reason = null) {
			UserID = userid;
			IPAddress = ipaddress;
			if (expiry != null)
				Expiry = (DateTime) expiry;
			Reason = reason;
		}

		internal int UserID {
			get { return _userid; }
			private set { _userid = value; }
		}

		internal User User {
			get {
				return User.GetUser(_userid);
			}
		}

		internal string IPAddress {
			get { return _ipaddress; }
			private set {
				if (value.Length < 50)
					_ipaddress = value;
			}
		}

		internal DateTime Expiry {
			get { return _expiry; }
			private set { _expiry = value; }
		}

		internal string Reason {
			get { return _reason; }
			private set {
				if (value.Length < 255)
					_reason = value;
			}
		}
	}
}