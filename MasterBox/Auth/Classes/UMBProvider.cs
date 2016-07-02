using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MasterBox.Auth {
	public sealed partial class MBProvider : MembershipProvider {
		
		// Unimplemented methods and Singleton Design Pattern
		private static volatile MBProvider _instance;
		private static object syncRoot = new Object();

		private MBProvider() { }

		public static MBProvider Instance {
			get {
				if (_instance == null) {
					lock (syncRoot)
						if (_instance == null)
							_instance = new MBProvider();
				}
				return _instance;
			}
		}

		public override string ApplicationName {
			get { return "MasterBox"; }
			set { return; }
		} // MasterBox
		public override bool EnablePasswordReset {
			get { return true; }
		} // true
		public override bool EnablePasswordRetrieval {
			get { return false; }
		} // false
		public override int MaxInvalidPasswordAttempts {
			get { return 5; }
		} // 5
		public override int MinRequiredNonAlphanumericCharacters {
			get { return 1; }
		} // 1
		public override int MinRequiredPasswordLength {
			get { return 8; }
		} // 8
		public override int PasswordAttemptWindow {
			get { return 5; }
		} // 8
		public override MembershipPasswordFormat PasswordFormat {
			get {
				throw new NotImplementedException();
			}
		} // ???
		public override string PasswordStrengthRegularExpression {
			get {
				throw new NotImplementedException();
			}
		} // ???
		public override bool RequiresQuestionAndAnswer {
			get {
				throw new NotImplementedException();
			}
		} // ???
		public override bool RequiresUniqueEmail {
			get {
				throw new NotImplementedException();
			}
		} // Yes
		public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer) {
			throw new NotImplementedException();
		}
		public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out MembershipCreateStatus status) {
			throw new NotImplementedException();
		}
		public override bool DeleteUser(string username, bool deleteAllRelatedData) {
			throw new NotImplementedException();
		}
		public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords) {
			throw new NotImplementedException();
		}
		public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords) {
			throw new NotImplementedException();
		}
		public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords) {
			throw new NotImplementedException();
		}
		public override int GetNumberOfUsersOnline() {
			throw new NotImplementedException();
		}
		public override string GetPassword(string username, string answer) {
			throw new NotImplementedException();
		}
		public override MembershipUser GetUser(string username, bool userIsOnline) {
			return User.GetUser(username);
		}
		public override MembershipUser GetUser(object providerUserKey, bool userIsOnline) {
			throw new NotImplementedException();
		}
		public override string GetUserNameByEmail(string email) {
			throw new NotImplementedException();
		}
		public override string ResetPassword(string username, string answer) {
			throw new NotImplementedException();
		}
		public override bool UnlockUser(string username) {
			throw new NotImplementedException();
		}


	}
}