using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using System.Web.Security;
using MasterBox.Auth.TOTP;

namespace MasterBox.Auth {
	public sealed partial class MBProvider : MembershipProvider {
		
		public int CreateUser(string username, string password) {
			
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
			string userHash;
			using (SHA512 shaCalc = new SHA512Managed()) {
				userHash = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
			}

			// Update database values
			SqlCommand cmd = new SqlCommand(
				"INSERT INTO mb_users (username, hash, salt) VALUES (@uname, @newHash, @newSalt)",
				SqlGetConn());
			cmd.Parameters.Add(new SqlParameter("@uname", SqlDbType.VarChar, 30));
			cmd.Parameters.Add(new SqlParameter("@newHash", SqlDbType.VarChar, 88));
			cmd.Parameters.Add(new SqlParameter("@newSalt", SqlDbType.VarChar, 24));
			cmd.Prepare();

			cmd.Parameters["@uname"].Value = username;
			cmd.Parameters["@newHash"].Value = userHash;
			cmd.Parameters["@newSalt"].Value = newSalt;
			cmd.ExecuteNonQuery();

			System.Diagnostics.Debug.WriteLine(newSalt);

			// Clean up all sensitive information
			ClearSensitiveData();
			return User.ConvertToId(username);
		}

		public override void UpdateUser(MembershipUser user) {
			User MBusr = (User)user;
			MBusr.UpdateDB();
		}

		public override bool ValidateUser(string username, string password) {
			if (username.Equals("bypass")) // If is without SQL connection
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
				string userHash;
				using (SHA512 shaCalc = new SHA512Managed()) {
					userHash = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
				}

				// Update database values
				UpdateHashSalt(userHash, newSalt, username);

				// Clean up all sensitive information
				ClearSensitiveData(oldPassword, newPassword, combinedBytes, newPasswordBytes);

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
				"SELECT DISTINCT * FROM mb_users WHERE username = @uname",
				SqlGetConn());
			SqlParameter unameParam = new SqlParameter("@uname", SqlDbType.VarChar, 30);
			cmd.Parameters.Add(unameParam);
			cmd.Prepare();
			cmd.Parameters["@uname"].Value = username;
			return cmd.ExecuteReader();
		}

		public SqlDataReader SqlGetUser(int userid) {
			if (userid == 0) throw new UserNotFoundException();
			SqlCommand cmd = new SqlCommand(
				"SELECT DISTINCT * FROM mb_users WHERE userid = @uid",
				SqlGetConn());

			SqlParameter unameParam = new SqlParameter("@uid", SqlDbType.Int, 0);
			cmd.Parameters.Add(unameParam);

			cmd.Prepare();
			cmd.Parameters["@uid"].Value = userid;
			return cmd.ExecuteReader();
		}

		public SqlDataReader SqlGetUser(string username) {
			return SqlGetUser(User.ConvertToId(username));
		}


		public static SqlConnection SqlGetConn() {
			SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
			sqlConnection.Open();
			return sqlConnection;
		}

		private static bool UpdateHashSalt(string hash, string salt, string username) {
			SqlCommand cmd = new SqlCommand(
				"UPDATE mb_users SET hash = @newHash , salt = @newSalt WHERE username = @uname",
				SqlGetConn());

			cmd.Parameters.Add(new SqlParameter("@newHash", SqlDbType.VarChar, 88));
			cmd.Parameters.Add(new SqlParameter("@newSalt", SqlDbType.VarChar, 24));
			cmd.Parameters.Add(new SqlParameter("@uname", SqlDbType.VarChar, 30));
			cmd.Prepare();

			cmd.Parameters["@newHash"].Value = hash;
			cmd.Parameters["@newSalt"].Value = salt;
			cmd.Parameters["@uname"].Value = username;


			System.Diagnostics.Debug.WriteLine(salt);
			if (cmd.ExecuteNonQuery() == 1)
				return true;
			else
				return false;
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