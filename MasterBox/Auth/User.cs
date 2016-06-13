using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MasterBox {
	public class User : MembershipUser {
		private static AuthDBControlDataContext adb = new AuthDBControlDataContext();

		public User(String username, String ptPass) {
			

		}

	}
}