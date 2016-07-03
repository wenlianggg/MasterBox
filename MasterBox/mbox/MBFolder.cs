using MasterBox.Auth;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace MasterBox.mbox {
	public class MBFolder {
		public string folderName { get; set; }
		public string folderusername { get; set; }
		public int folderencryption { get; set; }
        public string folderBlowFishKey { get; set; }
        public string folderBlowFishIV { get; set; }


        // Get Folder Information to display
		public static SqlDataReader GetFolderToDisplay(string username) {
            // Get User ID
            User user = User.GetUser(username);

			SqlCommand cmd = new SqlCommand("SELECT * FROM mb_folder WHERE userid = @userid", SQLGetMBoxConnection());
			SqlParameter unameParam = new SqlParameter("@userid", SqlDbType.BigInt, 8);
			cmd.Parameters.Add(unameParam);
			cmd.Parameters["@userid"].Value = user.UserId;
			cmd.Prepare();

			return cmd.ExecuteReader();
		}

		// Get list of all folder names
		public static ArrayList GenerateFolderLocation(String username) {
            // Get User ID
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand("SELECT distinct foldername FROM mb_folder WHERE userid=@userid", SQLGetMBoxConnection());
			cmd.Parameters.AddWithValue("@userid", user.UserId);
			SqlDataReader sqldr = cmd.ExecuteReader();
			ArrayList locationList = new ArrayList();
			locationList.Add("==Master Folder==");
			while (sqldr.Read()) {
				locationList.Add(sqldr["foldername"].ToString());
			}
			locationList.Sort();
			return locationList;
		}

		// Get list of folder names with passwords
		public static ArrayList GenerateEncryptedFolderLocation(String username) {
            // Get User ID
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand("SELECT distinct foldername FROM mb_folder WHERE userid=@userid and folderencryption=1", SQLGetMBoxConnection());
			cmd.Parameters.AddWithValue("@userid", user.UserId);
			SqlDataReader sqldr = cmd.ExecuteReader();
			ArrayList passwordlocationList = new ArrayList();
			passwordlocationList.Add("==Choose a Folder==");
			while (sqldr.Read()) {
				passwordlocationList.Add(sqldr["foldername"].ToString());
			}
			passwordlocationList.Sort();
			return passwordlocationList;
		}

		// Get list of folder names without password
		public static ArrayList GenerateUnencryptedFolderLocation(String username) {
            // Get User ID
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand("SELECT distinct foldername FROM mb_folder WHERE userid=@userid and folderencryption=0", SQLGetMBoxConnection());
			cmd.Parameters.AddWithValue("@userid", user.UserId);
			SqlDataReader sqldr = cmd.ExecuteReader();
			ArrayList passwordlocationList = new ArrayList();
			passwordlocationList.Add("==Choose a Folder==");
			while (sqldr.Read()) {
				passwordlocationList.Add(sqldr["foldername"].ToString());
			}
			passwordlocationList.Sort();
			return passwordlocationList;
		}

        // Generate BlowFish Key
        private string FolderKeyIVGeneration(int length,string input)
        {
            string valid = input;
            
            StringBuilder res = new StringBuilder();
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    res.Append(valid[(int)(num % (uint)valid.Length)]);
                }
            }
            
            System.Diagnostics.Debug.WriteLine(res.ToString());
            return res.ToString();
        }

        // Files in folder Blowfish448 Encryption
        private static byte[] EncryptionBlowfishFileFolder(byte[] filecontent, string key, string iv)
        {
            // Password is 256bits
            BlowfishEngine engine = new BlowfishEngine();

            //Testing
            
            byte[] encryptkey = Convert.FromBase64String(key);
            byte[] output = new byte[100];
            
            BufferedBlockCipher Blowfish = new BufferedBlockCipher(new BlowfishEngine());
            KeyParameter blowkey = new KeyParameter(encryptkey);
            
            Blowfish.Init(false, blowkey);
            Blowfish.ProcessBytes(filecontent, 0, (int)filecontent.Length, output, 0);
            string outputstring = Convert.ToBase64String(output);

            System.Diagnostics.Debug.WriteLine("Cipher Text: " + outputstring);

            return output;
        }
        // Files in folder Blowfish448 Decryption
        private static void DecryptionBlowfishFileFolder(byte[] filecontent, string key)
        {

            

        }

        public static bool UploadFileToFolder(MBFile file, string foldername) {
            
			try {
				// Get Folder ID
				SqlDataReader sqlFolderID = GetFolderInformation(file.fileusername, foldername);
				sqlFolderID.Read();
				int folderid = int.Parse(sqlFolderID["folderid"].ToString());

                // Get User ID
                User user = User.GetUser(file.fileusername);
                int userid = (int)user.UserId;

                if (CheckFolderEncryptionType(file.fileusername,foldername))
                { 
                    SqlDataReader encryptionInformation = GetFolderInformation(file.fileusername, foldername);
                    if (encryptionInformation.Read())
                    {
                        string key = encryptionInformation["folderkey"].ToString();
                        string iv = encryptionInformation["folderiv"].ToString();
                        file.filecontent = EncryptionBlowfishFileFolder(file.filecontent, key, iv);

                    }
                }
                else
                {

                }

                // Insert Database into database
                SqlCommand cmd = new SqlCommand(
					"INSERT INTO mb_file(folderid,userid,filename,filetype,filesize,filecontent) "
					+ "values(@folderid,@userid,@name,@type,@size,@data)", SQLGetMBoxConnection());
				cmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt, 8));
				cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
				cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 50));
				cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar, -1));
				cmd.Parameters.Add(new SqlParameter("@size", SqlDbType.Int, 4));
				cmd.Parameters.Add(new SqlParameter("@data", SqlDbType.VarBinary, -1));
				cmd.Prepare();
				cmd.Parameters["@folderid"].Value = folderid;
				cmd.Parameters["@userid"].Value = userid;
				cmd.Parameters["@name"].Value = file.fileName;
				cmd.Parameters["@type"].Value = file.fileType;
				cmd.Parameters["@size"].Value = file.fileSize;
				cmd.Parameters["@data"].Value = file.filecontent;

				cmd.ExecuteNonQuery();
                System.Diagnostics.Debug.WriteLine("Pass");
				return true;
			}
			catch
            {
                System.Diagnostics.Debug.WriteLine("fail");
                return false;
			}

		}

        private static bool CheckFolderEncryptionType(string username,string foldername)
        {
            SqlDataReader data=GetFolderInformation(foldername,username);
            MBFolder folder = new MBFolder();
            if (data.Read())
            {
                int check = (int)data["folderencryption"];
                if(check == 1)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
         
        }

		public bool CreateNewFolder(MBFolder folder) {
			try {
                // Get User ID
                User user = User.GetUser(folder.folderusername);
                int userid = (int)user.UserId;

                // Create Folder
                SqlCommand cmd = new SqlCommand("INSERT INTO mb_folder(userid,foldername,folderencryption) VALUES(@user,@name,@encryption)", SQLGetMBoxConnection());
				cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.BigInt, 8));
				cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 50));
				cmd.Parameters.Add(new SqlParameter("@encryption", SqlDbType.Bit, 1));
				cmd.Prepare();
				cmd.Parameters["@user"].Value = userid;
				cmd.Parameters["@name"].Value = folder.folderName;
				cmd.Parameters["@encryption"].Value = folder.folderencryption;
				cmd.ExecuteNonQuery();
				return true;
			}
			catch {
				return false;
			}
		}


		public bool CreateNewFolderWithPassword(MBFolder folder, string folderpassword) {
			try {
                // Get User ID
                User user = User.GetUser(folder.folderusername);
                long userid = user.UserId;

                //Generate New Salt
                byte[] newSalt = GenerateSaltFunction();
				string saltstring = Convert.ToBase64String(newSalt);

				// Convert Password to byte array
				var folderpassbyte = Encoding.UTF8.GetBytes(folderpassword);

				// Join Pass and salt together
				byte[] saltpassbyte = new byte[folderpassbyte.Length + newSalt.Length];
				folderpassbyte.CopyTo(saltpassbyte, 0);
				newSalt.CopyTo(saltpassbyte, folderpassbyte.Length);

                // Get key and iv for blowfish encryption
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(folderpassbyte,newSalt,16);
                byte[] blowkey= key.GetBytes(32); // 32bytes is 256bits
                byte[] blowiv = key.GetBytes(16); // 16bytes is 128bits

                //Debug purpose only
                System.Diagnostics.Debug.WriteLine("Blowfish Key Length: "+blowkey.Length);
                System.Diagnostics.Debug.WriteLine("Blowfish IV Length: " + blowiv.Length);

                // Convert Key and IV to String
                string blowkeystring = Convert.ToBase64String(blowkey);
                string blowivstring = Convert.ToBase64String(blowiv);

                //Debug
                System.Diagnostics.Debug.WriteLine("Blowfish key: " +blowkeystring);
                System.Diagnostics.Debug.WriteLine("Blowfish IV: " + blowivstring);

                // Get Password Hash
                string passhash;
				using (SHA512 shaCalc = new SHA512Managed()) {
					passhash = Convert.ToBase64String(shaCalc.ComputeHash(saltpassbyte));
				}

               
                // Create Folder
                SqlCommand cmd = new SqlCommand(
					"INSERT INTO mb_folder(userid,foldername,folderencryption,foldersaltfunction,folderpassword,folderkey,folderiv) "
					+ "VALUES(@user,@name,@encryption,@salt,@pass,@key,@iv)", SQLGetMBoxConnection());
				cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.BigInt, 8));
				cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 50));
				cmd.Parameters.Add(new SqlParameter("@encryption", SqlDbType.Bit, 1));
				cmd.Parameters.Add(new SqlParameter("@salt", SqlDbType.VarChar, 24));
				cmd.Parameters.Add(new SqlParameter("@pass", SqlDbType.VarChar, 88));
                cmd.Parameters.Add(new SqlParameter("@key", SqlDbType.VarChar, 64));
                cmd.Parameters.Add(new SqlParameter("@iv", SqlDbType.VarChar, 32));
                cmd.Prepare();
				cmd.Parameters["@user"].Value = userid;
				cmd.Parameters["@name"].Value = folder.folderName;
				cmd.Parameters["@encryption"].Value = folder.folderencryption;
				cmd.Parameters["@salt"].Value = saltstring;
				cmd.Parameters["@pass"].Value = passhash;
                cmd.Parameters["@key"].Value = blowkeystring;
                cmd.Parameters["@iv"].Value = blowivstring;
				cmd.ExecuteNonQuery();

				return true;
			}
			catch {
				return false;
			}
		}

		// Validate Folder Password
		private bool ValidateFolderPassword(MBFolder folder, string folderpassword) {
			SqlDataReader sqlFoldername = GetFolderInformation(folder.folderusername, folder.folderName);
			if (sqlFoldername.Read()) {
				// Database Folder Password and salt
				string folderHash = sqlFoldername["folderpassword"].ToString();
				byte[] saltBytes = Convert.FromBase64String(sqlFoldername["foldersaltfunction"].ToString());

				var validpassbyte = Encoding.UTF8.GetBytes(folderpassword);
				// Making password into byte arrays and combine

				byte[] combinedBytes = new byte[validpassbyte.Length + saltBytes.Length];
				validpassbyte.CopyTo(combinedBytes, 0);
				saltBytes.CopyTo(combinedBytes, validpassbyte.Length);
				// Get SHA512 value
				string userHash;
				using (SHA512 shaCalc = new SHA512Managed()) {
					userHash = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
				}

				string saltstring = Convert.ToBase64String(saltBytes);
				System.Diagnostics.Debug.WriteLine(saltstring);
				System.Diagnostics.Debug.WriteLine(folderHash);
				System.Diagnostics.Debug.WriteLine(folderpassword);
				System.Diagnostics.Debug.WriteLine(userHash);

				// Check if password mathces database
				if (userHash == (folderHash)) {
					// Empty out strings and sensitive data arrays
					Array.Clear(validpassbyte, 0, validpassbyte.Length);
					Array.Clear(combinedBytes, 0, combinedBytes.Length);
					folderpassword = string.Empty;
					userHash = string.Empty;
					// Password correct
					System.Diagnostics.Debug.WriteLine("Correct");
					return true;
				}
				else {
					// Empty out strings and sensitive data arrays
					Array.Clear(validpassbyte, 0, validpassbyte.Length);
					Array.Clear(combinedBytes, 0, combinedBytes.Length);
					folderpassword = string.Empty;
					userHash = string.Empty;
					// Password wrong
					System.Diagnostics.Debug.WriteLine("Wrong");
					return false;
				}
			}
			else {
				return false;
			}


		}

		// Make New password for exisiting folders
		public bool NewFolderPassword(MBFolder folder, string folderpassword) {
			SqlDataReader sqlFoldername = GetFolderInformation(folder.folderusername, folder.folderName);
			if (sqlFoldername.Read()) {
                // Get UserID
                User user = User.GetUser(folder.folderusername);
                long userid = user.UserId;

                // Generate New salt function
                byte[] newFolderSalt = GenerateSaltFunction();
				string newSalt = Convert.ToBase64String(newFolderSalt);

				// Convert new password to byte array
				var newPwBytes = Encoding.UTF8.GetBytes(folderpassword);

				// Join two arrays
				byte[] combinedBytes = new byte[newPwBytes.Length + newFolderSalt.Length];
				newPwBytes.CopyTo(combinedBytes, 0);
				newFolderSalt.CopyTo(combinedBytes, newPwBytes.Length);

				// New Hash combined arrays
				string folderhashpassword;
				using (SHA512 shaCalc = new SHA512Managed()) {
					folderhashpassword = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
				}
				SqlCommand cmd = new SqlCommand(
						   "UPDATE mb_folder SET folderpassword = @newfolderpass , foldersaltfunction = @newSalt, folderencryption=@folderencryption WHERE foldername = @foldername and userid= @userid",
						   SQLGetMBoxConnection());
				cmd.Parameters.Add(new SqlParameter("@newfolderpass", SqlDbType.VarChar, 88));
				cmd.Parameters.Add(new SqlParameter("@newSalt", SqlDbType.VarChar, 24));
				cmd.Parameters.Add(new SqlParameter("@folderencryption", SqlDbType.Bit, 1));
				cmd.Parameters.Add(new SqlParameter("@foldername", SqlDbType.VarChar, 50));
				cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
				cmd.Prepare();

				cmd.Parameters["@newfolderpass"].Value = folderhashpassword;
				cmd.Parameters["@newSalt"].Value = newSalt;
				cmd.Parameters["@folderencryption"].Value = folder.folderencryption;
				cmd.Parameters["@foldername"].Value = folder.folderName;
				cmd.Parameters["@userid"].Value = userid;
				cmd.ExecuteNonQuery();

				return true;
			}
			else {
				return false;
			}
		}

		// Change Folder Password
		public bool ChangeFolderPassword(MBFolder folder, string oldfolderpassword, string newfolderpassword) {
			if (ValidateFolderPassword(folder, oldfolderpassword)) {
                    // Get User id
                    User user = User.GetUser(folder.folderusername);

					// Generate New salt function
					byte[] newFolderSalt = GenerateSaltFunction();
					string newSalt = Convert.ToBase64String(newFolderSalt);

					// Convert new password to byte array
					var newPwBytes = Encoding.UTF8.GetBytes(newfolderpassword);

					// Join two arrays
					byte[] combinedBytes = new byte[newPwBytes.Length + newFolderSalt.Length];
					newPwBytes.CopyTo(combinedBytes, 0);
					newFolderSalt.CopyTo(combinedBytes, newPwBytes.Length);

					// New Hash combined arrays
					string folderhashpassword;
					using (SHA512 shaCalc = new SHA512Managed()) {
						folderhashpassword = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
					}

					// For debug purpose only
					System.Diagnostics.Debug.WriteLine(newfolderpassword);
					System.Diagnostics.Debug.WriteLine(newSalt);
					System.Diagnostics.Debug.WriteLine(folderhashpassword);

					// Update database password and salt
					SqlCommand cmd = new SqlCommand(
							"UPDATE mb_folder SET folderpassword = @newfolderpass , foldersaltfunction = @newSalt WHERE foldername = @foldername and userid= @userid",
							SQLGetMBoxConnection());
					cmd.Parameters.Add(new SqlParameter("@newfolderpass", SqlDbType.VarChar, 88));
					cmd.Parameters.Add(new SqlParameter("@newSalt", SqlDbType.VarChar, 24));
					cmd.Parameters.Add(new SqlParameter("@foldername", SqlDbType.VarChar, 50));
					cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
					cmd.Prepare();
					cmd.Parameters["@newfolderpass"].Value = folderhashpassword;
					cmd.Parameters["@newSalt"].Value = newSalt;
					cmd.Parameters["@foldername"].Value = folder.folderName;
					cmd.Parameters["@userid"].Value =user.UserId;
					cmd.ExecuteNonQuery();

					// Clear sensitive data
					Array.Clear(newPwBytes, 0, newPwBytes.Length);
					Array.Clear(combinedBytes, 0, combinedBytes.Length);
					newfolderpassword = string.Empty;
					folderhashpassword = string.Empty;

					return true;
				
			}
			else {
				return false;
			}
		}

		// Generate a salt
		private byte[] GenerateSaltFunction() {
			byte[] newSalt = new byte[16];
			using (RNGCryptoServiceProvider rngcsp = new RNGCryptoServiceProvider()) {
				rngcsp.GetBytes(newSalt);
			}
			return newSalt;
		}


		// Get User Information from Database
		private static SqlDataReader GetFolderInformation(string username, string foldername) {
            // Get Userid
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand("SELECT * FROM mb_folder WHERE foldername = @foldername and userid= @user", SQLGetMBoxConnection());
			cmd.Parameters.Add(new SqlParameter("@foldername", SqlDbType.VarChar, 50));
			cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.BigInt, 8));
			cmd.Prepare();
			cmd.Parameters["@foldername"].Value = foldername;
			cmd.Parameters["@user"].Value = user.UserId;

			return cmd.ExecuteReader();
		}



		private static SqlConnection SQLGetMBoxConnection() {
			SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
			sqlConnection.Open();
			return sqlConnection;
		}
	}
}
