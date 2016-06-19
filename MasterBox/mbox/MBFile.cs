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
        public string fileName { get; set; }
        public string fileType { get; set; }
        public byte fileSize { get; set; }

    }
    public class Folder : MBFile
    {
        public string folderName { get; set; }
        public string userName { get; set; }
        public int folderencryption { get; set; }
        public byte[] saltfunction { get; set; }
        public string folderPass { get; set; }

        public void GenerateFolderLocation(String username) {
            SqlCommand cmd = new SqlCommand("SELECT DISTINT foldername FROM mb_folder WHERE userid=?");
            SqlDataReader sqldr = cmd.ExecuteReader();
            ArrayList locationList = new ArrayList();
            while (sqldr.Read())
            {
                locationList.Add(sqldr["foldername"].ToString());
            }

        }

        public bool CreateNewFolder(Folder folder)
        {
            // Get User ID
            SqlDataReader sqldr = GetUserInformation(folder.userName);
            sqldr.Read();
            int userid = int.Parse(sqldr["userid"].ToString());

            // Create Folder
            try
            {
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
            // Get User ID
            SqlDataReader sqldr = GetUserInformation(folder.userName);
            sqldr.Read();
            int userid = int.Parse(sqldr["userid"].ToString());
            
           // Create Folder
            try
            {
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
        public string GenerateHashPassword(String username, String password,byte[] saltFunction)
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

        // Get User value from Database
        private SqlDataReader GetUserInformation(String username)
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
