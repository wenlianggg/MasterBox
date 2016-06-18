using MasterBox.Auth;
using System;
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
        // Storing Hash&Salt Password into database
        protected void StoreHashPassword(String hashPassword)
        {

        }

        // Generating a SHA 512 password
        public string GenerateHashPassword(String username, String password)
        {
            SqlDataReader sqldr = GetSaltFunction(username);
            // Add padding to make it 64bit
            int lengthPass = password.Length % 4;
            if (lengthPass > 0)
            {
                password = password.PadRight(password.Length + (4 - lengthPass), '=');
            }
            // Convert padded password to byte array
            byte[] passwordBytes = Convert.FromBase64String(password);
            byte[] saltBytes = Convert.FromBase64String(sqldr["salt"].ToString());
            byte[] passwordSaltBytes = new byte[passwordBytes.Length + saltBytes.Length];
            passwordBytes.CopyTo(passwordSaltBytes, 0);
            saltBytes.CopyTo(passwordSaltBytes, passwordBytes.Length);
            // Convert password to SHA512
            string passwordHash;
            using (SHA512 shaCalc = new SHA512Managed())
            {
                passwordHash = Convert.ToBase64String(shaCalc.ComputeHash(passwordSaltBytes));
            }
            return passwordHash;
        }

        // Get Salt value from Database
        private SqlDataReader GetSaltFunction(String username)
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
            SqlCommand cmd = new SqlCommand("SELECT salt FROM mb_auth WHERE username = @uname", sqlConnection);
            SqlParameter unameParam = new SqlParameter("@uname", SqlDbType.VarChar, 30);
            cmd.Parameters.Add(unameParam);
            cmd.Prepare();
            cmd.Parameters["@uname"].Value = username;
            return cmd.ExecuteReader();
        }
    }


}
