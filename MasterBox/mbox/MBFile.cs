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
        public bool UploadNewFile(MBFile file)
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
    public class Folder
    {
        public string folderName { get; set; }
        public string folderuserName { get; set; }
        public int folderencryption { get; set; }
        public byte[] saltfunction { get; set; }
        public string folderPass { get; set; }

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
            while (sqldr.Read())
            {
                locationList.Add(sqldr["foldername"].ToString());
            }
            return locationList;
        }

        public bool CreateNewFolder(Folder folder)
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
        public bool CreateNewFolderWithPassword(Folder folder)
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

        // Generating a SHA 512 password
        public string GenerateHashPassword(String username, String password, byte[] saltFunction)
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

        public byte[] GenerateSaltFunction()
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

        private static SqlConnection SQLGetMBoxConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }


}
