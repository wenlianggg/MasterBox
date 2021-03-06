﻿using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace MasterBox.mbox
{
    public class MBFile
    {
        public long fildid { get; set; }
        public string fileusername { get; set; }
        public string fileName { get; set; }
        public string fileType { get; set; }
        public int fileSize { get; set; }
        public byte[] filecontent { get; set; }
        public DateTime filetimestamp { get; set; }
        private string filekey { get; set; }
        private string fileiv { get; set; }

        // Upload of file
        public static bool UploadNewFile(MBFile file)
        {
            if (SufficientSpace(file).Equals(true))
            {
                try
                {
                    // Get User ID
                    User user = User.GetUser(file.fileusername);

                    file.filekey = user.AesKey;
                    file.fileiv = user.AesIV;
                    file.filecontent = MBFile.EncryptAES256File(file);

                    // Storing of File
                    SqlCommand cmd = new SqlCommand(
                        "INSERT INTO mb_file(userid,filename,filetype,filesize,filecontent,filetimestamp) "
                        + "values(@user,@name,@type,@size,@data,@timestamp)", SQLGetMBoxConnection());
                    cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.BigInt, 8));
                    cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.NVarChar, -1));
                    cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar, -1));
                    cmd.Parameters.Add(new SqlParameter("@size", SqlDbType.Int, 4));
                    cmd.Parameters.Add(new SqlParameter("@data", SqlDbType.VarBinary, -1));
                    cmd.Parameters.Add(new SqlParameter("@timestamp", SqlDbType.DateTime2, 7));
                    cmd.Prepare();
                    cmd.Parameters["@user"].Value = user.UserId;
                    cmd.Parameters["@name"].Value = file.fileName;
                    cmd.Parameters["@type"].Value = file.fileType;
                    cmd.Parameters["@size"].Value = file.fileSize;
                    cmd.Parameters["@data"].Value = file.filecontent;
                    cmd.Parameters["@timestamp"].Value = file.filetimestamp;
                    cmd.ExecuteNonQuery();

                    // Loggin for file upload
                    FileLogger.Instance.FileUploaded(user.UserId, file.fileName);

                    // Clear all
                    file = null;

                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        // Override File
        public static bool OverwriteFile(MBFile file)
        {
            if (SufficientSpace(file).Equals(true))
            {
                try
                {
                    // Get User ID
                    User user = User.GetUser(file.fileusername);

                    file.filekey = user.AesKey;
                    file.fileiv = user.AesIV;
                    file.filecontent = MBFile.EncryptAES256File(file);

                    SqlCommand cmd = new SqlCommand(
                        "UPDATE mb_file SET filesize=@filesize,filetype=@filetype,filecontent=@filecontent,filetimestamp=@filetimestamp WHERE filename=@filename", SQLGetMBoxConnection());
                    cmd.Parameters.Add(new SqlParameter("@filename", SqlDbType.NVarChar, -1));
                    cmd.Parameters.Add(new SqlParameter("@filetype", SqlDbType.NVarChar, -1));
                    cmd.Parameters.Add(new SqlParameter("@filesize", SqlDbType.Int, 4));
                    cmd.Parameters.Add(new SqlParameter("@filecontent", SqlDbType.VarBinary, -1));
                    cmd.Parameters.Add(new SqlParameter("@filetimestamp", SqlDbType.DateTime2, 7));

                    cmd.Prepare();

                    cmd.Parameters["@filename"].Value = file.fileName;
                    cmd.Parameters["@filetype"].Value = file.fileType;
                    cmd.Parameters["@filesize"].Value = file.fileSize;
                    cmd.Parameters["@filecontent"].Value = file.filecontent;
                    cmd.Parameters["@filetimestamp"].Value = file.filetimestamp;

                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        // Delete File
        public static void DeleteFile(string username, long fileid)
        {
            User user = User.GetUser(username);
            SqlCommand cmd = new SqlCommand(
               "DELETE FROM mb_file WHERE fileid=@fileid and userid=@userid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@fileid", SqlDbType.BigInt, 8));
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();

            cmd.Parameters["@fileid"].Value = fileid;
            cmd.Parameters["@userid"].Value = user.UserId;
            cmd.ExecuteNonQuery();

        }

        // Check file name
        public static bool FilenameCheck(string username, string filename)
        {
            User user = User.GetUser(username);
            SqlCommand cmd = new SqlCommand(
                   "SELECT filename FROM mb_file WHERE userid=@userid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = user.UserId;

            SqlDataReader sqldr = cmd.ExecuteReader();
            while (sqldr.Read())
            {
                if (sqldr["filename"].ToString() == filename)
                {
                    System.Diagnostics.Debug.WriteLine("Same name");
                    return false;
                }

            }
            return true;
        }

        // Check file name in folder
        public static bool FilenameCheck(string username, string filename, string foldername)
        {
            User user = User.GetUser(username);
            MBFolder folder = MBFolder.GetFolder(username, foldername);

            SqlCommand cmd = new SqlCommand(
                   "SELECT filename FROM mb_file WHERE userid=@userid and folderid=@folderid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = user.UserId;
            cmd.Parameters["@folderid"].Value = folder.folderid;
            SqlDataReader sqldr = cmd.ExecuteReader();
            while (sqldr.Read())
            {
                if (sqldr["filename"].ToString() == filename)
                {
                    return false;
                }

            }
            return true;
        }

        // AES256 Encryption for file
        private static byte[] EncryptAES256File(MBFile file)
        {
            // Convert PT to byte
            byte[] plainbyte = file.filecontent;

            System.Diagnostics.Debug.WriteLine("DB Key: " + file.filekey);
            System.Diagnostics.Debug.WriteLine("DB IV: " + file.fileiv);

            AesCryptoServiceProvider aes = new AesCryptoServiceProvider();
            aes.BlockSize = 128;
            aes.KeySize = 256;
            aes.Key = System.Text.ASCIIEncoding.ASCII.GetBytes(file.filekey);
            aes.IV = System.Text.ASCIIEncoding.ASCII.GetBytes(file.fileiv);
            aes.Padding = PaddingMode.PKCS7;
            aes.Mode = CipherMode.CBC;

            ICryptoTransform crypto = aes.CreateEncryptor(aes.Key, aes.IV);
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
        public static byte[] DecryptAES256File(byte[] filecontent, string AESKEY, string AESIV)
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

        // Retrieve to display users
        public static SqlDataReader GetUsersToDisplay(string username=null)
        {
            if (username == null)
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM mb_users", SQLGetMBoxConnection());
                return cmd.ExecuteReader();
            }else
            {
                SqlCommand cmd = new SqlCommand("SELECT * FROM mb_users WHERE username LIKE @username", SQLGetMBoxConnection());
                cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 30));
                cmd.Prepare();
                string searchTerm = string.Format("%{0}%", username);
                cmd.Parameters["@username"].Value = searchTerm;
                return cmd.ExecuteReader();
            }
        }

        // Retrieve to display file
        public static SqlDataReader GetFileToDisplay(string username)
        {
            // Get User ID
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand("SELECT * FROM mb_file WHERE userid = @userid and folderid is null", SQLGetMBoxConnection());
            SqlParameter unameParam = new SqlParameter("@userid", SqlDbType.BigInt, 8);
            cmd.Parameters.Add(unameParam);
            cmd.Parameters["@userid"].Value = user.UserId;

            cmd.Prepare();
            return cmd.ExecuteReader();
        }

        // Retrieve to display file from folder
        public static SqlDataReader GetFileFromFolderToDisplay(string username, long folderid)
        {
            User user = User.GetUser(username);
            System.Diagnostics.Debug.WriteLine("Folder ID: " + folderid);
            System.Diagnostics.Debug.WriteLine("User ID: " + user.UserId);
            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM mb_file WHERE userid = @userid AND folderid=@folderid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt, 8));
            cmd.Prepare();

            cmd.Parameters["@userid"].Value = user.UserId;
            cmd.Parameters["@folderid"].Value = folderid;

            return cmd.ExecuteReader();
        }

        public static SqlDataReader GetFileFromFolderToDisplay(long folderid)
        {
            System.Diagnostics.Debug.WriteLine("Folder ID: " + folderid);
            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM mb_file WHERE folderid=@folderid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            
            cmd.Parameters["@folderid"].Value = folderid;

            return cmd.ExecuteReader();
        }


        // Get all user files
        public static List<MBFile> RetrieveAllUserFiles(string username)
        {
            List<MBFile> filelist = new List<MBFile>();
            // Get User ID
            User user = User.GetUser(username);
            SqlCommand cmd = new SqlCommand(
               "SELECT fileid,filename,filesize,filetimestamp FROM mb_file WHERE userid = @userid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = user.UserId;
            SqlDataReader sqldr = cmd.ExecuteReader();
            MBFile mbf;
            while (sqldr.Read())
            {
                mbf = new MBFile(); 
                mbf.fildid = (long)sqldr["fileid"];
                mbf.fileName = sqldr["filename"].ToString();
                mbf.fileSize = (int)sqldr["filesize"];
                mbf.filetimestamp = Convert.ToDateTime(sqldr["filetimestamp"]);

                /*
                // debug
                System.Diagnostics.Debug.WriteLine("Name: " + mbf.fileName);
                System.Diagnostics.Debug.WriteLine("size: " + mbf.fileSize);
                System.Diagnostics.Debug.WriteLine("time: " + mbf.filetimestamp);
                */
                filelist.Add(mbf);
            }

            return filelist;
        }

        // Get File Information
        public static MBFile RetrieveFile(string username, long fileid)
        {
            // Get User ID
            User user = User.GetUser(username);
            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM mb_file WHERE userid = @userid AND fileid = @fileid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Parameters.Add(new SqlParameter("@fileid", SqlDbType.BigInt, 8));
            cmd.Prepare();

            cmd.Parameters["@userid"].Value = user.UserId;
            cmd.Parameters["@fileid"].Value = fileid;

            // File Retrieval
            SqlDataReader sqldr = cmd.ExecuteReader();
            MBFile mbf = new MBFile();
            if (sqldr.Read())
            {
                mbf.fildid = (long)sqldr["fileid"];
                mbf.filecontent = MBFile.DecryptAES256File((byte[])sqldr["filecontent"], user.AesKey, user.AesIV);
                mbf.fileName = sqldr["filename"].ToString();
                mbf.fileSize = (int)sqldr["filesize"];
                mbf.fileType = sqldr["filetype"].ToString();
                mbf.fileusername = user.UserName;
                mbf.filetimestamp = Convert.ToDateTime(sqldr["filetimestamp"]);
            }
            if (mbf.fileSize == 0)
                return null;
            return mbf;
        }

        public static MBFile RetrieveFile(string username, string filename)
        {
            // Get User ID
            User user = User.GetUser(username);
            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM mb_file WHERE userid = @userid AND filename = @filename AND folderid is NULL", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Parameters.Add(new SqlParameter("@filename", SqlDbType.NVarChar, -1));
            cmd.Prepare();

            cmd.Parameters["@userid"].Value = user.UserId;
            cmd.Parameters["@filename"].Value = filename;

            // File Retrieval
            SqlDataReader sqldr = cmd.ExecuteReader();
            MBFile mbf = new MBFile();
            if (sqldr.Read())
            {
                mbf.fildid = (long)sqldr["fileid"];
                mbf.filecontent = MBFile.DecryptAES256File((byte[])sqldr["filecontent"], user.AesKey, user.AesIV);
                mbf.fileName = sqldr["filename"].ToString();
                mbf.fileSize = (int)sqldr["filesize"];
                mbf.fileType = sqldr["filetype"].ToString();
                mbf.fileusername = user.UserName;
                mbf.filetimestamp = Convert.ToDateTime(sqldr["filetimestamp"]);
            }

            if (mbf.fileSize == 0)
                return null;
            return mbf;
        }

        public static int GetTotalFileStorage(string username)
        {
            int totalFileSize = 0;

            User user = User.GetUser(username);
            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM mb_file WHERE userid = @userid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = user.UserId;

            SqlDataReader sqlDR = cmd.ExecuteReader();
            while (sqlDR.Read())
            {
                int filesize = (int)sqlDR["filesize"];
                totalFileSize = totalFileSize + filesize;
            }
            return totalFileSize;
        }

        // Check for exceeding current storage space
        private static Boolean SufficientSpace(MBFile file)
        {
            // Get current user's Member Type
            User user = User.GetUser(file.fileusername);
            SqlCommand cmd = new SqlCommand("SELECT mbrType FROM mb_users WHERE userid = @userid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Parameters["@userid"].Value = user.UserId;
            cmd.Prepare();

            // Execute Retrieval of mbrType
            SqlDataReader rd = cmd.ExecuteReader();
            rd.Read();
            int mbrType = (int)rd["mbrType"];

            // Values of space max threshold
            int threshold = mbrType * 5;
            double sizeMB = Math.Round(BytesToMega(file.fileSize), 0);

            // File is within threshold
            if (sizeMB < threshold)
            {
                return true;
            }
            // File exceeds threshold
            else
            {
                return false;
            }
        }

        // Bytes to Megabytes
        protected static double BytesToMega(double bytes)
        {
            return (bytes / 1024f) / 1024f;
        }

        private static SqlConnection SQLGetMBoxConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }
    }
}
