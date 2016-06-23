using MasterBox.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace MasterBox.mbox
{
    public class MBFile
    {
        public string fileusername { get; set; }
        public string fileName { get; set; }
        public string fileType { get; set; }
        public int fileSize { get; set; }
        public byte[] filecontent { get; set; }

        // Upload of file
        public static bool UploadNewFile(MBFile file)
        {
            try
            {
                // Get User ID
                SqlDataReader sqlUserID = GetUserInformation(file.fileusername);
                sqlUserID.Read();
                int userid = int.Parse(sqlUserID["userid"].ToString());
                // Storing of File
                SqlCommand cmd = new SqlCommand("INSERT INTO mb_file(userid,filename,filetype,filesize,filecontent)values(@userid,@Name,@type,@size,@data)", SQLGetMBoxConnection());
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@Name", file.fileName);
                cmd.Parameters.AddWithValue("@type", file.fileType);
                cmd.Parameters.AddWithValue("@size", file.fileSize);
                cmd.Parameters.AddWithValue("@data", file.filecontent);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Retrieve to display file
        public static SqlDataReader GetFileToDisplay(string username)
        {
            // Get User ID
            SqlDataReader sqlUserID = GetUserInformation(username);
            sqlUserID.Read();
            int userid = int.Parse(sqlUserID["userid"].ToString());

            SqlCommand cmd = new SqlCommand("SELECT * FROM mb_file WHERE userid = @userid", SQLGetMBoxConnection());
            SqlParameter unameParam = new SqlParameter("@userid", SqlDbType.BigInt, 30);
            cmd.Parameters.Add(unameParam);
            cmd.Parameters["@userid"].Value = userid;
            cmd.Prepare();
            return cmd.ExecuteReader();
        }


        // Get User Information from Database
        private static SqlDataReader GetUserInformation(String username)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM mb_auth WHERE username = @uname", SQLGetMBoxConnection());
            SqlParameter unameParam = new SqlParameter("@uname", SqlDbType.VarChar, 30);
            cmd.Parameters.Add(unameParam);
            cmd.Parameters["@uname"].Value = username;
            cmd.Prepare();
            return cmd.ExecuteReader();
        }

        private static SqlConnection SQLGetMBoxConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

    }
    public class MBFolder
    {
        public string folderName { get; set; }
        public string folderuserName { get; set; }
        public int folderencryption { get; set; }
        public byte[] saltfunction { get; set; }
        public string folderPass { get; set; }


        public static SqlDataReader GetFolderToDisplay(string username)
        {
            // Get User ID
            SqlDataReader sqlUserID = GetUserInformation(username);
            sqlUserID.Read();
            int userid = int.Parse(sqlUserID["userid"].ToString());

            SqlCommand cmd = new SqlCommand("SELECT * FROM mb_folder WHERE userid = @userid", SQLGetMBoxConnection());
            SqlParameter unameParam = new SqlParameter("@userid", SqlDbType.BigInt, 30);
            cmd.Parameters.Add(unameParam);
            cmd.Parameters["@userid"].Value = userid;
            cmd.Prepare();

            return cmd.ExecuteReader();
        }
        // Get list of folder names
        public static ArrayList GenerateFolderLocation(String username)
        {
            // Get User ID
            SqlDataReader sqlUserID = GetUserInformation(username);
            sqlUserID.Read();
            int userid = int.Parse(sqlUserID["userid"].ToString());

            SqlCommand cmd = new SqlCommand("SELECT distinct foldername FROM mb_folder WHERE userid=@userid", SQLGetMBoxConnection());
            cmd.Parameters.AddWithValue("@userid", userid);
            SqlDataReader sqldr = cmd.ExecuteReader();
            ArrayList locationList = new ArrayList();
            locationList.Add("==Master Folder==");
            while (sqldr.Read())
            {
                locationList.Add(sqldr["foldername"].ToString());
            }
            locationList.Sort();
            return locationList;
        }

        // Get list of folder names with password
        public static ArrayList GenerateEncryptedFolderLocation(String username)
        {
            // Get User ID
            SqlDataReader sqlUserID = GetUserInformation(username);
            sqlUserID.Read();
            int userid = int.Parse(sqlUserID["userid"].ToString());

            SqlCommand cmd = new SqlCommand("SELECT distinct foldername FROM mb_folder WHERE userid=@userid and folderencryption=1", SQLGetMBoxConnection());
            cmd.Parameters.AddWithValue("@userid", userid);
            SqlDataReader sqldr = cmd.ExecuteReader();
            ArrayList passwordlocationList = new ArrayList();
            passwordlocationList.Add("==Choose a Folder==");
            while (sqldr.Read())
            {
                passwordlocationList.Add(sqldr["foldername"].ToString());
            }
            passwordlocationList.Sort();
            return passwordlocationList;
        }

        public static bool UploadFileToFolder(string foldername,MBFile file)
        {try
            {
                // Get User ID
                SqlDataReader sqlFolderID = GetFolderInformation(foldername);
                sqlFolderID.Read();
                int folderid = int.Parse(sqlFolderID["folderid"].ToString());

                // Get User ID
                SqlDataReader sqlUserID = GetUserInformation(file.fileusername);
                sqlUserID.Read();
                int userid = int.Parse(sqlUserID["userid"].ToString());

                SqlCommand cmd = new SqlCommand("INSERT INTO mb_file(folderid,userid,filename,filetype,filesize,filecontent)values(@folderid,@userid,@Name,@type,@size,@data)", SQLGetMBoxConnection());
                cmd.Parameters.AddWithValue("@folderid", folderid);
                cmd.Parameters.AddWithValue("@userid", userid);
                cmd.Parameters.AddWithValue("@Name", file.fileName);
                cmd.Parameters.AddWithValue("@type", file.fileType);
                cmd.Parameters.AddWithValue("@size", file.fileSize);
                cmd.Parameters.AddWithValue("@data", file.filecontent);
                cmd.ExecuteNonQuery();

                return true;
            }
            catch
            {
                return false;
            }
            
        }

        public static bool CreateNewFolder(MBFolder folder)
        {
            try
            {
                // Get User ID
                SqlDataReader sqldr = GetUserInformation(folder.folderuserName);
                sqldr.Read();
                int userid = int.Parse(sqldr["userid"].ToString());

                // Create Folder

                SqlCommand cmd = new SqlCommand("INSERT INTO mb_folder(userid,foldername,folderencryption) VALUES(@user,@name,@encryption)", SQLGetMBoxConnection());
                cmd.Parameters.AddWithValue("@user", userid);
                cmd.Parameters.AddWithValue("@name", folder.folderName);
                cmd.Parameters.AddWithValue("@encryption", folder.folderencryption);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Storing Hash&Salt Password into database
        public static bool CreateNewFolderWithPassword(MBFolder folder)
        {
            try
            {
                // Get User ID
                SqlDataReader sqldr = GetUserInformation(folder.folderuserName);
                sqldr.Read();
                int userid = int.Parse(sqldr["userid"].ToString());

                // Create Folder

                SqlCommand cmd = new SqlCommand("INSERT INTO mb_folder(userid,foldername,folderencryption,foldersaltfunction,folderpassword) VALUES(@user,@name,@encryption,@salt,@pass)", SQLGetMBoxConnection());
                cmd.Parameters.AddWithValue("@user", userid);
                cmd.Parameters.AddWithValue("@name", folder.folderName);
                cmd.Parameters.AddWithValue("@encryption", folder.folderencryption);
                cmd.Parameters.AddWithValue("@salt", folder.saltfunction);
                cmd.Parameters.AddWithValue("@pass", folder.folderPass);
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Validate Folder Password
        public static bool ValidateFolderPassword(string foldername, string folderpassword)
        {
                SqlDataReader sqlFoldername = GetFolderInformation(foldername);
                if (sqlFoldername.Read())
                {
                    string folderHash= sqlFoldername["folderpassword"].ToString();

                    // Add padding to password to make Base64 Compatible
                    int len = folderpassword.Length % 4;
                    if (len > 0)
                        folderpassword = folderpassword.PadRight(folderpassword.Length + (4 - len), '=');

                    // Convert padded user input to byte array
                    byte[] userInputBytes = Convert.FromBase64String(folderpassword);
                    byte[] saltBytes = Convert.FromBase64String(sqlFoldername["foldersaltfunction"].ToString());
                    byte[] combinedBytes = new byte[userInputBytes.Length + saltBytes.Length];
                    userInputBytes.CopyTo(combinedBytes, 0);
                    saltBytes.CopyTo(combinedBytes, userInputBytes.Length);

                    // Get SHA512 value from user input
                    string userHash;
                    using (SHA512 shaCalc = new SHA512Managed())
                    {
                        userHash = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
                    }

                    if (userHash.Equals(folderHash))
                    {
                        // Empty out strings and sensitive data arrays
                        Array.Clear(userInputBytes, 0, userInputBytes.Length);
                        Array.Clear(combinedBytes, 0, combinedBytes.Length);
                        folderpassword = string.Empty;
                        userHash = string.Empty;
                        // Password correct
                        return true;
                    }
                    else
                    {
                        // Debug information
                        System.Diagnostics.Debug.WriteLine(userHash);
                        System.Diagnostics.Debug.WriteLine(folderHash);
                        // Empty out strings and sensitive data arrays
                        Array.Clear(userInputBytes, 0, userInputBytes.Length);
                        Array.Clear(combinedBytes, 0, combinedBytes.Length);
                        folderpassword = string.Empty;
                        userHash = string.Empty;
                        // Password incorrect
                        return false;
                    }
            }
            else
            {
                return false;
            }

       
        }

        public static bool ChangeFolderPassword(string foldername, string oldfolderpassword, string newfolderpassword)
        {
            if (ValidateFolderPassword(foldername, oldfolderpassword) == true)
            {
                // Generate New salt function
                byte[] newFolderSalt = GenerateSaltFunction();

                // Password Padding
                int len = newfolderpassword.Length % 4;
                if (len > 0)
                    newfolderpassword = newfolderpassword.PadRight(newfolderpassword.Length + (4 - len), '=');

                // Convert new password to byte array
                byte[] newPwBytes = Convert.FromBase64String(newfolderpassword);

                // Join two arrays
                byte[] combinedBytes = new byte[newPwBytes.Length + newFolderSalt.Length];
                newPwBytes.CopyTo(combinedBytes, 0);
                newFolderSalt.CopyTo(combinedBytes, newPwBytes.Length);

                // Convert salt to string
                string newSalt = Convert.ToBase64String(newFolderSalt);

                // New Hash combined arrays
                string folderpassword;
                using (SHA512 shaCalc = new SHA512Managed())
                {
                    folderpassword = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
                }

                // Update database password and salt
                SqlCommand cmd = new SqlCommand(
                    "UPDATE mb_folder SET folderpassword = @newfolderpass , foldersaltfunction = @newSalt WHERE foldername = @foldername",
                    SQLGetMBoxConnection());
                cmd.Parameters.Add(new SqlParameter("@newfolderpass", SqlDbType.VarChar, 88));
                cmd.Parameters.Add(new SqlParameter("@newSalt", SqlDbType.VarChar, 24));
                cmd.Parameters.Add(new SqlParameter("@foldername", SqlDbType.VarChar, 50));
                cmd.Prepare();
                cmd.Parameters["@newfolderpass"].Value = folderpassword;
                cmd.Parameters["@newSalt"].Value = newSalt;
                cmd.Parameters["@foldername"].Value = foldername;
                cmd.ExecuteNonQuery();

                // Clean up all sensitive information
                oldfolderpassword = string.Empty;
                newfolderpassword = string.Empty;
                Array.Clear(combinedBytes, 0, combinedBytes.Length);
                Array.Clear(newPwBytes, 0, newPwBytes.Length);

                return true;
            }else
            {
                return false;
            }

            
        }

        // Generating a SHA 512 password
        public static string GenerateHashPassword(String username, String password, byte[] saltFunction)
        {
            // Add padding to make it 64bit
            int lengthPass = password.Length % 4;
            if (lengthPass > 0)
            {
                password = password.PadRight(password.Length + (4 - lengthPass), '=');
            }
            // Convert padded password to byte array
            byte[] passwordBytes = Convert.FromBase64String(password);
            byte[] passwordSaltBytes = new byte[passwordBytes.Length + saltFunction.Length];
            passwordBytes.CopyTo(passwordSaltBytes, 0);
            saltFunction.CopyTo(passwordSaltBytes, passwordBytes.Length);
            // Convert password to SHA512
            string passwordHash;
            using (SHA512 shaCalc = new SHA512Managed())
            {
                passwordHash = Convert.ToBase64String(shaCalc.ComputeHash(passwordSaltBytes));
            }
            return passwordHash;
        }

        public static byte[] GenerateSaltFunction()
        {
            byte[] newSalt = new byte[16];
            using (RNGCryptoServiceProvider rngcsp = new RNGCryptoServiceProvider())
            {
                rngcsp.GetBytes(newSalt);
            }
            return newSalt;
        }

        // Get User Information from Database
        private static SqlDataReader GetUserInformation(String username)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM mb_auth WHERE username = @uname", SQLGetMBoxConnection());
            SqlParameter unameParam = new SqlParameter("@uname", SqlDbType.VarChar, 30);
            cmd.Parameters.Add(unameParam);
            cmd.Parameters["@uname"].Value = username;
            cmd.Prepare();
            return cmd.ExecuteReader();
        }

        // Get User Information from Database
        private static SqlDataReader GetFolderInformation(String foldername)
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM mb_folder WHERE foldername = @foldername", SQLGetMBoxConnection());
            SqlParameter unameParam = new SqlParameter("@foldername", SqlDbType.VarChar, 30);
            cmd.Parameters.Add(unameParam);
            cmd.Parameters["@foldername"].Value = foldername;
            cmd.Prepare();
            return cmd.ExecuteReader();
        }

        private static SqlConnection SQLGetMBoxConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }


}
