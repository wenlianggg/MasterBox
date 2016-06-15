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

			SqlDataReader sqldr = SQLGetUserByID(username);
			if (sqldr.Read()) {
				// Get byte array from database SHA512 string
				string storedHash = sqldr["hash"].ToString();

				// Add padding to password to make Base64 Compatible
				int len = password.Length % 4;
				if (len > 0)
					password = password.PadRight(password.Length + (4 - len), '=');
				// Convert padded user input to byte array
				byte[] userInputBytes = Convert.FromBase64String(password);
				byte[] saltBytes = Convert.FromBase64String(sqldr["salt"].ToString());
				byte[] combinedBytes = new byte[userInputBytes.Length + saltBytes.Length];
				userInputBytes.CopyTo(combinedBytes, 0);
				saltBytes.CopyTo(combinedBytes, userInputBytes.Length);
				// Get SHA512 value from user input
				string userHash;
				using (SHA512 shaCalc = new SHA512Managed()) {
					userHash = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
				}
				if (userHash.Equals(storedHash)) {
					// Empty out strings and sensitive data arrays
					Array.Clear(userInputBytes, 0, userInputBytes.Length);
					Array.Clear(combinedBytes, 0, combinedBytes.Length);
					password = string.Empty;
					userHash = string.Empty;
					// Password correct
					return true;
				} else {
					// Debug information
					System.Diagnostics.Debug.WriteLine(userHash);
					System.Diagnostics.Debug.WriteLine(storedHash);
					// Empty out strings and sensitive data arrays
					Array.Clear(userInputBytes, 0, userInputBytes.Length);
					Array.Clear(combinedBytes, 0, combinedBytes.Length);
					password = string.Empty;
					userHash = string.Empty;
					// Password incorrect
					return false;
				}
			} else {
				// User not found
				return false;
			}
		}

		public override bool ChangePassword(string username, string oldPassword, string newPassword) {
			// Validate user password entered first
			if (ValidateUser(username, oldPassword)) {
				// Get user from SQL
				SqlDataReader sqldr = SQLGetUserByID(username);
				// New salt generation
				byte[] newSaltB = new byte[16];
				using (RNGCryptoServiceProvider rngcsp = new RNGCryptoServiceProvider()) {
					rngcsp.GetBytes(newSaltB);
				}
				// Do necessary padding work
				int len = newPassword.Length % 4;
				if (len > 0)
					newPassword = newPassword.PadRight(newPassword.Length + (4 - len), '=');
				// Convert new password to byte array
				byte[] newPwBytes = Convert.FromBase64String(newPassword);
				// Join two arrays
				byte[] combinedBytes = new byte[newPwBytes.Length + newSaltB.Length];
				newPwBytes.CopyTo(combinedBytes, 0);
				newSaltB.CopyTo(combinedBytes, newPwBytes.Length);
				// Convert salt to string
				string newSalt = Convert.ToBase64String(newSaltB);
				// Hash combined arrays
				string userHash;
				using (SHA512 shaCalc = new SHA512Managed()) {
					userHash = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
				}
				// Update database values
				SqlCommand cmd = new SqlCommand(
					"UPDATE mb_auth SET hash = @newHash , salt = @newSalt WHERE username = @uname",
					SQLGetMBoxConnection());
				cmd.Parameters.Add(new SqlParameter("@newHash", SqlDbType.VarChar, 88));
				cmd.Parameters.Add(new SqlParameter("@newSalt", SqlDbType.VarChar, 24));
				cmd.Parameters.Add(new SqlParameter("@uname", SqlDbType.VarChar, 30));
				cmd.Prepare();
				cmd.Parameters["@newHash"].Value = userHash;
				cmd.Parameters["@newSalt"].Value = newSalt;
				cmd.Parameters["@uname"].Value = username;
				cmd.ExecuteNonQuery();
				System.Diagnostics.Debug.WriteLine(newSalt);
				// Clean up all sensitive information
				oldPassword = string.Empty;
				newPassword = string.Empty;
				Array.Clear(combinedBytes, 0, combinedBytes.Length);
				Array.Clear(newPwBytes, 0, newPwBytes.Length);
				return true;
			} else {
				// Existing password is wrong
				return false;
			}
		}

		private static SqlDataReader SQLGetUserByID(String username) {
			SqlCommand cmd = new SqlCommand(
				"SELECT * FROM mb_auth WHERE username = @uname",
				SQLGetMBoxConnection());

			SqlParameter unameParam = new SqlParameter("@uname", SqlDbType.VarChar, 30);
			cmd.Parameters.Add(unameParam);

			cmd.Prepare();
			cmd.Parameters["@uname"].Value = username;
			return cmd.ExecuteReader();
		}

		private static SqlConnection SQLGetMBoxConnection() {
			SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
			sqlConnection.Open();
			return sqlConnection;
		}
	}
}