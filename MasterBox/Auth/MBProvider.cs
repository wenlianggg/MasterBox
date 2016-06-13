﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.Collections.Specialized;
using System.Configuration.Provider;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Configuration;
using System.Web.Security;

namespace MasterBox.Auth {
	public class MBProvider : MembershipProvider {
		public override string ApplicationName {
			get { return "MasterBox"; }
			set { return; }
		}
		public override bool EnablePasswordReset {
			get { return true; }
		}
		public override bool EnablePasswordRetrieval {
			get { return false; }
		}
		public override int MaxInvalidPasswordAttempts {
			get { return 5; }
		}
		public override int MinRequiredNonAlphanumericCharacters {
			get { return 1;  }
		}
		public override int MinRequiredPasswordLength {
			get { return 8;  }
		}
		public override int PasswordAttemptWindow {
			get { return 5; }
		}
		public override MembershipPasswordFormat PasswordFormat {
			get {
				throw new NotImplementedException();
			}
		}
		public override string PasswordStrengthRegularExpression {
			get {
				throw new NotImplementedException();
			}
		}
		public override bool RequiresQuestionAndAnswer {
			get {
				throw new NotImplementedException();
			}
		}
		public override bool RequiresUniqueEmail {
			get {
				throw new NotImplementedException();
			}
		}
		public override bool ChangePassword(string username, string oldPassword, string newPassword) {
			throw new NotImplementedException();
		}
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
			throw new NotImplementedException();
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
		public override bool UnlockUser(string userName) {
			throw new NotImplementedException();
		}
		public override void UpdateUser(MembershipUser user) {
			throw new NotImplementedException();
		}
		public override bool ValidateUser(string username, string password) {
			throw new NotImplementedException();
		}
	}
}