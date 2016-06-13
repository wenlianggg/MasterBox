using System;
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
			get { return 1;  }
		} // 1
		public override int MinRequiredPasswordLength {
			get { return 8;  }
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
		public override bool UnlockUser(string username) {
			throw new NotImplementedException();
		}
		public override void UpdateUser(MembershipUser user) {
			throw new NotImplementedException();
		}
		public override bool ValidateUser(string username, string password) {
			if (username == "bypass") // If is without SQL connection
				return true;

			SqlDataReader sqldr = sqlGetUserByID(username);
			if (sqldr.Read()) {
				using (SHA512 shaCalc = new SHA512Managed()) {
					// Convert user input to byte array
					byte[] userInputBytes = Encoding.UTF8.GetBytes(password);
					// Get SHA512 value from user input
					string userInputHashString = Convert.ToBase64String(shaCalc.ComputeHash(userInputBytes));
					// Get byte array from database SHA512 string
					string targetValue = sqldr["hash"].ToString();

					if (userInputHashString.Equals(targetValue)) {
						return true;
						// Password correct
					} else {
						System.Diagnostics.Debug.WriteLine(userInputHashString);
						System.Diagnostics.Debug.WriteLine(targetValue);
						return false;
						// Password incorrect
					}
				}
			} else {
				// User not found
				return false;
			}
		}

		// TODO: Username case insensitivity
		public static SqlDataReader sqlGetUserByID(String username) {
			SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
			sqlConnection.Open();
			SqlCommand cmd = new SqlCommand("SELECT * FROM mb_auth WHERE username = @uname", sqlConnection);

			SqlParameter unameParam = new SqlParameter("@uname", SqlDbType.VarChar, 30);
			cmd.Parameters.Add(unameParam);

			cmd.Prepare();
			cmd.Parameters["@uname"].Value = username;
			return cmd.ExecuteReader();
		}
	}
}