using MasterBox.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
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
        private static SqlDataReader GetUserInformation(string username)
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

        public static bool UploadFileToFolder(MBFile file,string foldername)
        {
            try
            {
                // Get User ID
                SqlDataReader sqlFolderID = GetFolderInformation(file.fileusername,foldername);
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

        public bool CreateNewFolder(MBFolder folder)
        {
            try
            {
                // Get User ID
                SqlDataReader sqldr = GetUserInformation(folder.folderuserName);
                sqldr.Read();
                int userid = int.Parse(sqldr["userid"].ToString());

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
            catch
            {
                return false;
            }
        }

        // Storing Hash&Salt Password into database
        public bool CreateNewFolderWithPassword(MBFolder folder, string folderpassword)
        {
            try
            {
                // Get User ID
                SqlDataReader sqldr = GetUserInformation(folder.folderuserName);
                sqldr.Read();
                int userid = int.Parse(sqldr["userid"].ToString());

                //Generate New Salt
                byte[] newSalt = new byte[16];
                using (RNGCryptoServiceProvider rngcsp = new RNGCryptoServiceProvider())
                {
                    rngcsp.GetBytes(newSalt);
                }
                string saltstring = Convert.ToBase64String(newSalt);

                // Convert Password to byte array
                var folderpassbyte= Encoding.UTF8.GetBytes(folderpassword);
                // Join Pass and salt together
                byte[] saltpassbyte = new byte[folderpassbyte.Length + newSalt.Length];
                folderpassbyte.CopyTo(saltpassbyte, 0);
                newSalt.CopyTo(saltpassbyte, folderpassbyte.Length);

                string passhash;
                using (SHA512 shaCalc = new SHA512Managed())
                {
                    passhash = Convert.ToBase64String(shaCalc.ComputeHash(saltpassbyte));
                }

                // Create Folder
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO mb_folder(userid,foldername,folderencryption,foldersaltfunction,folderpassword) " 
                    +"VALUES(@user,@name,@encryption,@salt,@pass)", SQLGetMBoxConnection());
                cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.BigInt, 8));
                cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 50));
                cmd.Parameters.Add(new SqlParameter("@encryption", SqlDbType.Bit,1));
                cmd.Parameters.Add(new SqlParameter("@salt", SqlDbType.VarChar, 24));
                cmd.Parameters.Add(new SqlParameter("@pass", SqlDbType.VarChar,88));
                cmd.Prepare();
                cmd.Parameters["@user"].Value = userid;
                cmd.Parameters["@name"].Value = folder.folderName;
                cmd.Parameters["@encryption"].Value = folder.folderencryption;
                cmd.Parameters["@salt"].Value = saltstring;
                cmd.Parameters["@pass"].Value = passhash;
                cmd.ExecuteNonQuery();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Validate Folder Password
        public bool ValidateFolderPassword(string username,string foldername, string folderpassword)
        {
            SqlDataReader sqlFoldername = GetFolderInformation(username,foldername);
            if (sqlFoldername.Read())
            {
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
                using (SHA512 shaCalc = new SHA512Managed())
                {
                    userHash = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
                }

                string saltstring = Convert.ToBase64String(saltBytes);
                System.Diagnostics.Debug.WriteLine(saltstring);
                System.Diagnostics.Debug.WriteLine(folderHash);
                System.Diagnostics.Debug.WriteLine(folderpassword);
                System.Diagnostics.Debug.WriteLine(userHash);

                // Check if password mathces database
                if (userHash == (folderHash))
                {
                    // Empty out strings and sensitive data arrays
                    Array.Clear(validpassbyte, 0, validpassbyte.Length);
                    Array.Clear(combinedBytes, 0, combinedBytes.Length);
                    folderpassword = string.Empty;
                    userHash = string.Empty;
                    // Password correct
                    System.Diagnostics.Debug.WriteLine("Correct");
                    return true;
                }
                else
                {
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
            else
            {
                return false;
            }


        }

        public bool ChangeFolderPassword(string username,string foldername, string oldfolderpassword, string newfolderpassword)
        {
            if (ValidateFolderPassword(username, folderName, oldfolderpassword))
            {
                // Get User id
                SqlDataReader sqldr = GetUserInformation(username);
                sqldr.Read();
                int userid = int.Parse(sqldr["userid"].ToString());

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
                string folderpassword;
                using (SHA512 shaCalc = new SHA512Managed())
                {
                    folderpassword = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
                }
                System.Diagnostics.Debug.WriteLine(newfolderpassword);
                System.Diagnostics.Debug.WriteLine(newSalt);
                System.Diagnostics.Debug.WriteLine(folderpassword);

                // Update database password and salt
                SqlCommand cmd = new SqlCommand(
                        "UPDATE mb_folder SET folderpassword = @newfolderpass , foldersaltfunction = @newSalt WHERE foldername = @foldername and userid= @userid",
                        SQLGetMBoxConnection());
                cmd.Parameters.Add(new SqlParameter("@newfolderpass", SqlDbType.VarChar, 88));
                cmd.Parameters.Add(new SqlParameter("@newSalt", SqlDbType.VarChar, 24));
                cmd.Parameters.Add(new SqlParameter("@foldername", SqlDbType.VarChar, 50));
                cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
                cmd.Prepare();
                cmd.Parameters["@newfolderpass"].Value = folderpassword;
                cmd.Parameters["@newSalt"].Value = newSalt;
                cmd.Parameters["@foldername"].Value = foldername;
                cmd.Parameters["@userid"].Value = userid;
                cmd.ExecuteNonQuery();

                return true;
            }else
            {
                return false;
            }
        }

        // Generating a SHA 512 password
        public static string GenerateHashPassword(String password, byte[] saltFunction)
        {          
            string saltString= Convert.ToBase64String(saltFunction);

            // Convert password to byte array
            var passwordBytes = Encoding.UTF8.GetBytes(password);

            byte[] passwordSaltBytes = new byte[passwordBytes.Length + saltFunction.Length];
            passwordBytes.CopyTo(passwordSaltBytes, 0);
            saltFunction.CopyTo(passwordSaltBytes, passwordBytes.Length);

            // Convert password to SHA512
            string passwordHash;
            using (SHA512 shaCalc = new SHA512Managed())
            {
                passwordHash = Convert.ToBase64String(shaCalc.ComputeHash(passwordSaltBytes));
            }
            System.Diagnostics.Debug.WriteLine(passwordHash);
            System.Diagnostics.Debug.WriteLine(saltString);
            return passwordHash;
        }

        // Generate a salt
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
        private static SqlDataReader GetFolderInformation(string username,string foldername)
        {
            // Get Userid
            SqlDataReader sqldr = GetUserInformation(username);
            sqldr.Read();
            int userid = int.Parse(sqldr["userid"].ToString());

            SqlCommand cmd = new SqlCommand("SELECT * FROM mb_folder WHERE foldername = @foldername and userid= @user", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@foldername", SqlDbType.VarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@foldername"].Value = foldername;
            cmd.Parameters["@user"].Value = userid;

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
