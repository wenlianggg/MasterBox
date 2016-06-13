using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MasterBox.Auth {
	public class User : MembershipUser {
		String username;
		Boolean isTOTPEnabled;
		String TOTPToken;
		Boolean isLoggedIn;
		Boolean is2FAEnabled;
		Boolean is2FAAuthed;
		private static AuthDBControlDataContext adb = new AuthDBControlDataContext();

		public User(String username, String ptPass) {
		

		}

	}
}