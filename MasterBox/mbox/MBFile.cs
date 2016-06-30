using MasterBox.Auth;
using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace MasterBox.mbox {
	public class MBFile {
		public string fileusername { get; set; }
		public string fileName { get; set; }
		public string fileType { get; set; }
		public int fileSize { get; set; }
		public byte[] filecontent { get; set; }
        private string filekey { get; set; }
        private string fileiv { get; set; }

		// Upload of file
		public static bool UploadNewFile(MBFile file) {
			try {
                // Get User ID
                User user = new User(file.fileusername);
                long userid = user.UserId;				

                file.filekey = KeyIvGenerator(32);
                file.fileiv = KeyIvGenerator(16);
                file.filecontent = MBFile.EncryptAES256File(file);

                // Storing of File
                SqlCommand cmd = new SqlCommand(
					"INSERT INTO mb_file(userid,filename,filetype,filesize,filecontent) "
					+ "values(@user,@name,@type,@size,@data)", SQLGetMBoxConnection());
				cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.BigInt, 8));
				cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, -1));
				cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar, -1));
				cmd.Parameters.Add(new SqlParameter("@size", SqlDbType.Int, 4));
				cmd.Parameters.Add(new SqlParameter("@data", SqlDbType.VarBinary, -1));
				cmd.Prepare();
				cmd.Parameters["@user"].Value = userid;
				cmd.Parameters["@name"].Value = file.fileName;
				cmd.Parameters["@type"].Value = file.fileType;
				cmd.Parameters["@size"].Value = file.fileSize;
				cmd.Parameters["@data"].Value = file.filecontent;

				cmd.ExecuteNonQuery();
                
				// Clear Sensitive Data
				file.fileName = "";
				file.fileType = "";
				file.fileSize = 0;
				file.filecontent = null;
                file.filekey = "";
                file.fileiv = "";
				return true;
			}
			catch {
				return false;
			}
		}

        // To generate Key and IV
        public static string KeyIvGenerator(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
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
            return res.ToString();
        }

        // AES256 Encryption for file
        private static byte[] EncryptAES256File(MBFile file) {

            // Convert PT to byte
            byte[] plainbyte = file.filecontent;

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(file.filekey);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(file.fileiv);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            ICryptoTransform crypto = aes.CreateEncryptor(aes.Key,aes.IV);
            byte[] encryptedstring = crypto.TransformFinalBlock(plainbyte, 0, plainbyte.Length);

            // debug purpose
            string keystring = Convert.ToBase64String(aes.Key);
            string keysizestring = aes.KeySize.ToString();
            string ivstring = Convert.ToBase64String(aes.IV);
            string encryptedtext = Convert.ToBase64String(encryptedstring);

            System.Diagnostics.Debug.WriteLine("Key: " + keystring);
            System.Diagnostics.Debug.WriteLine("IV: " + ivstring);
            System.Diagnostics.Debug.WriteLine("Plain Text: " + Convert.ToBase64String(file.filecontent));
            System.Diagnostics.Debug.WriteLine("Cipher Text: " + encryptedtext);

            return encryptedstring;
        }

        // AES256 Decryption for file
        public static byte[] DecryptAES256File(byte[] filecontent,string AESKEY,string AESIV)
        {
            byte[] encryptedtext = filecontent;

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(AESKEY);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(AESIV);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            ICryptoTransform crypto = aes.CreateDecryptor(aes.Key, aes.IV);
            byte[] plaintext = crypto.TransformFinalBlock(encryptedtext, 0, encryptedtext.Length);

            // Debug
            string originaltext = System.Text.ASCIIEncoding.ASCII.GetString(plaintext);
            System.Diagnostics.Debug.WriteLine("Plain Text: " + originaltext);

            return plaintext;         
        }

        // Retrieve to display file
        public static SqlDataReader GetFileToDisplay(string username) {
            // Get User ID
            User user = new User(username);
            long userid = user.UserId;

			SqlCommand cmd = new SqlCommand("SELECT * FROM mb_file WHERE userid = @userid and folderid is null", SQLGetMBoxConnection());
			SqlParameter unameParam = new SqlParameter("@userid", SqlDbType.BigInt, 8);
			cmd.Parameters.Add(unameParam);
			cmd.Parameters["@userid"].Value = userid;
			cmd.Prepare();
			return cmd.ExecuteReader();
		}

        public static SqlDataReader GetFileFromFolderToDisplay(string username,int folderid)
        {
            User user = new User(username);
            System.Diagnostics.Debug.WriteLine(folderid);
            System.Diagnostics.Debug.WriteLine(user.UserId);
            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM mb_file WHERE userid = @userid AND folderid=@folderid", SQLGetMBoxConnection());
            SqlParameter unameParam = new SqlParameter("@userid", SqlDbType.BigInt, 8);
            SqlParameter folderidParam = new SqlParameter("@folderid", SqlDbType.BigInt, 8);
            cmd.Parameters.Add(unameParam);
            cmd.Parameters.Add(folderidParam);
            cmd.Parameters["@userid"].Value = user.UserId;
            cmd.Parameters["@folderid"].Value = folderid;
            cmd.Prepare();


            /*
            SqlDataReader sqldr = cmd.ExecuteReader();
            MBFile mbf = new MBFile();
            if (sqldr.Read())
            {
                mbf.filecontent = (byte[])sqldr["filecontent"];
                mbf.fileName = sqldr["filename"].ToString();
                mbf.fileSize = (int)sqldr["filesize"];
                mbf.fileType = sqldr["filetype"].ToString();
            }
            System.Diagnostics.Debug.WriteLine(mbf.fileName);
            */
            return cmd.ExecuteReader();
        }

		public static MBFile RetrieveFile(string username, long fileid) {
			// Get User ID
			User user = new User(username);
			SqlCommand cmd = new SqlCommand("SELECT * FROM mb_file WHERE userid = @userid AND fileid = @fileid", SQLGetMBoxConnection());
			SqlParameter unameParam = new SqlParameter("@userid", SqlDbType.BigInt, 8);
			SqlParameter fileidParam = new SqlParameter("@fileid", SqlDbType.BigInt, 8);
			cmd.Parameters.Add(unameParam);
			cmd.Parameters.Add(fileidParam);
			cmd.Parameters["@userid"].Value = user.UserId;
			cmd.Parameters["@fileid"].Value = fileid;
			cmd.Prepare();

			// File Retrieval
			SqlDataReader sqldr = cmd.ExecuteReader();
			MBFile mbf = new MBFile();
			if (sqldr.Read()) {
				mbf.filecontent = MBFile.DecryptAES256File((byte[])sqldr["filecontent"],"","");
				mbf.fileName = sqldr["filename"].ToString();
				mbf.fileSize = (int)sqldr["filesize"];
				mbf.fileType = sqldr["filetype"].ToString();
			}
			if (mbf.fileSize == 0)
				return null;
			return mbf;
		}


		private static SqlConnection SQLGetMBoxConnection() {
			SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
			sqlConnection.Open();
			return sqlConnection;
		}
	}
}
