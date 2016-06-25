using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterBox.Auth.State {
	public class CookieValue {
		private readonly string _username;
		private readonly string _ipaddress;
		private readonly DateTime _creation;
		private readonly DateTime _expiry;

		public CookieValue(string username, string ipaddress) {
			_username = username;
			_ipaddress = ipaddress;
			_creation = DateTime.Now;
			_expiry = DateTime.Now.AddMinutes(3);
		}

		public string Username {
			get {
				return _username;
			}
		}

		public string IPAddress {
			get {
				return _ipaddress;
			}
		}

		public DateTime Creation {
			get {
				return _creation;
			}
		}

		public DateTime Expiry {
			get {
				return _expiry;
			}
		}

		
	}
}