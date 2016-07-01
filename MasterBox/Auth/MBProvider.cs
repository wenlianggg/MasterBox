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
using MasterBox.Auth.TOTP;

namespace MasterBox.Auth {
	public sealed class MBProvider : MembershipProvider {

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
			return new User(username);
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
			User MBusr = (User)user;
			MBusr.DbUpdateAllFields();
		}
		public override bool ValidateUser(string username, string password) {
			if (username == "bypass") // If is without SQL connection
				return true;

			SqlDataReader sqldr = SQLGetAuth(username);
			if (sqldr.Read()) {
				// Get byte array from database SHA512 string
				string storedHash = sqldr["hash"].ToString();
				byte[] saltBytes = Convert.FromBase64String(sqldr["salt"].ToString());
				byte[] userInputBytes = Encoding.UTF8.GetBytes(password);

				// Get both byte arrays and combine them together
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
					System.Diagnostics.Debug.WriteLine("Salt Value:" + sqldr["salt"].ToString());
					System.Diagnostics.Debug.WriteLine("Input Hash:" + userHash);
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
				throw new UserNotFoundException();
			}
		}

		public override bool ChangePassword(string username, string oldPassword, string newPassword) {
			// Validate user password entered first
			if (ValidateUser(username, oldPassword)) {
				// Get user from SQL
				SqlDataReader sqldr = SQLGetAuth(username);

				// New salt generation
				byte[] newSaltB = new byte[16];
				using (RNGCryptoServiceProvider cryptrng = new RNGCryptoServiceProvider()) {
					cryptrng.GetBytes(newSaltB);
				}
				
				// Convert salt to string
				string newSalt = Convert.ToBase64String(newSaltB);

				// Convert new password to byte array
				var newPasswordBytes = Encoding.UTF8.GetBytes(newPassword);
				
				// Join two arrays
				byte[] combinedBytes = new byte[newPasswordBytes.Length + newSaltB.Length];
				newPasswordBytes.CopyTo(combinedBytes, 0);
				newSaltB.CopyTo(combinedBytes, newPasswordBytes.Length);
				
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
				Array.Clear(newPasswordBytes, 0, newPasswordBytes.Length);
				return true;
			} else {
				// Existing password is wrong
				return false;
			}
		}

		public bool ValidateTOTP(string username, string otp) {
			int otpEntered;
			OTPTool otptool = new OTPTool();
			SqlDataReader sqldr = SQLGetAuth(username);
			if (sqldr.Read() && int.TryParse(otp, out otpEntered)) {
				string totpsecret = sqldr["totpsecret"].ToString();
				otptool.SecretBase32 = totpsecret;
				System.Diagnostics.Debug.WriteLine(string.Join(" ", otptool.OneTimePasswordRange));
				foreach (int SingleOTP in otptool.OneTimePasswordRange) {
					if (SingleOTP == otpEntered) {
						// Success
						System.Diagnostics.Debug.WriteLine(true);
						return true;
					}
				}
				// Failure
				return false;
			} else {
				return false;
			}
		}

		public bool ValidateBackupTOTP(string username, string backupcode) {
			return false;
		}

		public string GetCorrectCasingUN(string username) {
			SqlDataReader sqldr = SQLGetAuth(username);
			if (sqldr.Read()) {
				return sqldr["username"].ToString();
			} else {
				return null;
			}
		}

		private SqlDataReader SQLGetAuth(string username) {
			SqlCommand cmd = new SqlCommand(
				"SELECT DISTINCT * FROM mb_auth WHERE username = @uname",
				SQLGetMBoxConnection());
			SqlParameter unameParam = new SqlParameter("@uname", SqlDbType.VarChar, 30);
			cmd.Parameters.Add(unameParam);
			cmd.Prepare();
			cmd.Parameters["@uname"].Value = username;
			return cmd.ExecuteReader();
		}

		public SqlDataReader SQLGetUser(long userid) {
			if (userid == 0) throw new UserNotFoundException();
			SqlCommand cmd = new SqlCommand(
				"SELECT DISTINCT ma.username, mu.* FROM mb_users mu " +
				"JOIN mb_auth ma ON mu.userid = ma.userid " +
				"WHERE ma.userid = @uid",
				SQLGetMBoxConnection());

			SqlParameter unameParam = new SqlParameter("@uid", SqlDbType.BigInt, 0);
			cmd.Parameters.Add(unameParam);

			cmd.Prepare();
			cmd.Parameters["@uid"].Value = userid;
			return cmd.ExecuteReader();
		}

		public SqlDataReader SQLGetUser(string username) {
			return SQLGetUser(UsernameToId(username));
		}

		public long UsernameToId(string username) {
			SqlCommand cmd = new SqlCommand(
				"SELECT userid, username FROM mb_auth WHERE username = @uname;",
				SQLGetMBoxConnection());
			SqlParameter unameParam = new SqlParameter("@uname", SqlDbType.VarChar, 30);
			cmd.Parameters.Add(unameParam);
			cmd.Prepare();
			cmd.Parameters["@uname"].Value = username;
			SqlDataReader sqldr = cmd.ExecuteReader();
			if (sqldr.Read()) {
				return (long) sqldr["@userid"];
			} else {
			return 0;
			}
		}

		private static SqlConnection SQLGetMBoxConnection() {
			SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
			sqlConnection.Open();
			return sqlConnection;
		}
	}
}