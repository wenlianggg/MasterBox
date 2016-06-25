using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Security.Cryptography;

namespace MasterBox.Auth.State {

	// Singleton Class
	public class CookieList {
		private static CookieList _instance;

		private CookieList() { }

		public static CookieList Instance {
			get {
				if (_instance == null) {
					_instance = new CookieList();
				}
				return _instance;
			}
		}

		private Dictionary<string, CookieValue> clist = new Dictionary<string, CookieValue>();

		// Methods
		public string Add(string username, string ipaddress) {
			string cookieid;
			CookieValue cookieval;
			using (RNGCryptoServiceProvider rngcsp = new RNGCryptoServiceProvider()) {
				Byte[] randombytes = new Byte[16];
				rngcsp.GetBytes(randombytes);
				cookieid = Convert.ToBase64String(randombytes);
			}
			cookieval = new CookieValue(username, ipaddress);
			clist.Add(cookieid, cookieval);
			return cookieid;
		}

		public string FindValid(string cookieid, string ipaddress) {
			CookieValue cookieval = null;
			try {
				clist.TryGetValue(cookieid, out cookieval);
				// Check if IP addresses match
				if (cookieval.IPAddress == ipaddress) {
					return cookieval.Username;
				} else {
					return null;
				}
			} catch (NullReferenceException) {
				return null;
			}
		}

		public bool Remove(string cookieid) {
			if (clist.ContainsKey(cookieid)) {
				// Could find key
				clist.Remove(cookieid);
				return true;
			} else {
				// Could not find key
				return false;
			}
		}
	}
}