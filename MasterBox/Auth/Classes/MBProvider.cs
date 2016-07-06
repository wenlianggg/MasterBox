using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using MasterBox.Auth.TOTP;
using System.Collections.Generic;

/// Author: Goh Wen Liang (154473G) 

namespace MasterBox.Auth {
	public sealed partial class MBProvider : MembershipProvider {

		internal Dictionary<string, int> failedlogins = new Dictionary<string, int>();

		internal int CreateUser(string username, string password) {

			// New salt generation
			byte[] newSaltB = GenerateSalt();

			// Convert salt to string
			string newSalt = Convert.ToBase64String(newSaltB);

			// Convert new password to byte array
			var newPasswordBytes = Encoding.UTF8.GetBytes(password);

			// Join two arrays
			byte[] combinedBytes = new byte[newPasswordBytes.Length + newSaltB.Length];
			newPasswordBytes.CopyTo(combinedBytes, 0);
			newSaltB.CopyTo(combinedBytes, newPasswordBytes.Length);

			// Hash combined arrays
			string newHash;
			using (SHA512 shaCalc = new SHA512Managed())
				newHash = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
			using (DataAccess da = new DataAccess())
				da.SqlUpdateHashSalt(username, newHash, newSalt);
			System.Diagnostics.Debug.WriteLine(newSalt);

			// Clean up all sensitive information
			ClearSensitiveData();
			return User.ConvertToId(username);
		}

		internal bool HasOtp(string username) {
			throw new NotImplementedException();
		}

		public override void UpdateUser(MembershipUser user) {
			User MBusr = (User)user;
			MBusr.UpdateDB();
		}

		public override bool ValidateUser(string username, string password) {
			if (username.Equals("bypass")) // If is without SQL connection
				return true;
			using (DataAccess da = new DataAccess())
			using (SqlDataReader sqldr = da.SqlGetAuth(username))
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
						ClearSensitiveData(userInputBytes, combinedBytes, password, userHash);

						// Password correct
						return true;
					} else {
						// Debug information
						System.Diagnostics.Debug.WriteLine("Salt Value:" + sqldr["salt"].ToString());
						System.Diagnostics.Debug.WriteLine("Input Hash:" + userHash);
						System.Diagnostics.Debug.WriteLine(storedHash);

						// Empty out strings and sensitive data arrays
						ClearSensitiveData(userInputBytes, combinedBytes, password, userHash);

						// Call the logger to log a failed verification attempt
						Logger.Instance.FailedLoginAttempt(User.ConvertToId(username));
						
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
				using (DataAccess da = new DataAccess())
				using (SqlDataReader sqldr = da.SqlGetAuth(username)) {

					// New salt generation
					byte[] newSaltB = GenerateSalt();

					// Convert salt to string
					string newSalt = Convert.ToBase64String(newSaltB);

					// Convert new password to byte array
					var newPasswordBytes = Encoding.UTF8.GetBytes(newPassword);

					// Join two arrays
					byte[] combinedBytes = new byte[newPasswordBytes.Length + newSaltB.Length];
					newPasswordBytes.CopyTo(combinedBytes, 0);
					newSaltB.CopyTo(combinedBytes, newPasswordBytes.Length);

					// Hash combined arrays
					string newHash;
					using (SHA512 shaCalc = new SHA512Managed()) {
						newHash = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
					}

					// Update database values
					da.SqlUpdateHashSalt(username, newHash, newSalt);
					Logger.Instance.UserPassChanged(User.ConvertToId(username));

					// Clean up all sensitive information
					ClearSensitiveData(oldPassword, newPassword, combinedBytes, newPasswordBytes);
				}

				return true;

			} else {
				// Existing password is wrong
				Logger.Instance.UserPassChangeFailed(User.ConvertToId(username));
				return false;
			}
		}

		internal bool ValidateTOTP(string username, string otp) {
			int otpEntered;
			if (otp.Length == 6) {
				using (OTPTool otptool = new OTPTool())
				using (DataAccess da = new DataAccess())
				using (SqlDataReader sqldr = da.SqlGetAuth(username))
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
					}
			}
			// Failure
			return false;
		}

		internal bool IsTotpEnabled(string username) {
			using (DataAccess da = new DataAccess())
			using (SqlDataReader sqldr = da.SqlGetAuth(username)) {
				if (sqldr.Read()) {
					if (sqldr["totpsecret"].ToString().Length == 16) {
						return true;
					} else if (sqldr["totpsecret"] == null) {
						return false; 
					} else {
						throw new InvalidTOTPLength(sqldr["totpsecret"].ToString().Length.ToString());
					}
				} else {
					throw new UserNotFoundException(username);
				}
			}
		}
	
		internal void LoginSuccess(User usr, bool persistlogin) {
			Logger.Instance.SuccessfulLogin(usr.UserId);
			FormsAuthentication.RedirectFromLoginPage(usr.UserName, persistlogin);
		}

		internal bool ValidateBackupTOTP(string username, string backupcode) {
			return false;
		}

		internal string GetCorrectCasingUN(string username) {
			DataAccess da = new DataAccess();
			SqlDataReader sqldr = da.SqlGetAuth(username);
			if (sqldr.Read()) {
				return sqldr["username"].ToString();
			} else {
				return null;
			}
		}

		private static byte[] GenerateSalt() {
			var saltBytes = new byte[16];
			using (RNGCryptoServiceProvider cryptrng = new RNGCryptoServiceProvider()) {
				cryptrng.GetBytes(saltBytes);
			}
			return saltBytes;
		}

		private void ClearSensitiveData(params object[] objs) {
			for (int i = 0; i < objs.Length; i++) {
				if (objs[i] is string)
					objs[i] = string.Empty;
				if (objs[i] is byte[])
					Array.Clear((byte[]) objs[i], 0 , ((byte[])objs[i]).Length);
				objs[i] = null;
			}
		}
	}
}