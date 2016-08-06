using MasterBox.Auth;
using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;
using BlowFishCS;

namespace MasterBox.mbox
{
    public class MBFolder
    {
        public long folderid { get; set; }
        public string folderName { get; set; }
        public string folderusername { get; set; }
        public bool folderencryptionstatus { get; set; }
        public string folderpassword { get; set; }
        public string foldersalt { get; set; }
        public string folderBlowFishKey { get; set; }
        public string folderBlowFishIV { get; set; }
        public DateTime foldertimestamp { get; set; }
        
        // Get Folder Information to display
        public static SqlDataReader GetFolderToDisplay(string username)
        {
            // Get User ID
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand("SELECT * FROM mb_folder WHERE userid = @userid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = user.UserId;

            return cmd.ExecuteReader();
        }

        public static SqlDataReader GetSharedFolderToDisplay(string username)
        {
            // Get User ID
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand("SELECT * FROM mb_folder WHERE folderid = (SELECT folderid FROM mb_fileaccess WHERE userid = @userid)", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = user.UserId;

            return cmd.ExecuteReader();
        }

        // Get list of all folder names
        public static ArrayList GenerateFolderLocation(String username)
        {
            // Get User ID
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand("SELECT distinct foldername FROM mb_folder WHERE userid=@userid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = user.UserId;
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

        // Get list of folder names with passwords
        public static ArrayList GenerateEncryptedFolderLocation(String username)
        {
            // Get User ID
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand("SELECT distinct foldername FROM mb_folder WHERE userid=@userid and folderencryption=1", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = user.UserId;
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

        // Get list of folder names without password
        public static ArrayList GenerateUnencryptedFolderLocation(String username)
        {
            // Get User ID
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand("SELECT distinct foldername FROM mb_folder WHERE userid=@userid and folderencryption=0", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = user.UserId;
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

        // Check Folder name
        public static bool CheckFolderName(string foldername,string username)
        {
            //Get UserID
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand("SELECT foldername FROM mb_folder WHERE userid=@userid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = user.UserId;

            SqlDataReader sqldr = cmd.ExecuteReader();
            while (sqldr.Read())
            {
                if(foldername == sqldr["foldername"].ToString())
                {
                    return false;
                }
            }
            return true;
           
        }



        // Download File From Folder
        public static MBFile RetrieveFolderFile(string username, long fileid, long folderid)
        {
            // Get User ID
            User user = User.GetUser(username);

            // Get File Data
            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM mb_file WHERE userid = @userid AND fileid = @fileid AND folderid=@folderid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Parameters.Add(new SqlParameter("@fileid", SqlDbType.BigInt, 8));
            cmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt, 8));
            cmd.Prepare();

            cmd.Parameters["@userid"].Value = user.UserId;
            cmd.Parameters["@fileid"].Value = fileid;
            cmd.Parameters["@folderid"].Value = folderid;

            // Get Folder Key and IV
            MBFolder folder = MBFolder.GetFolder(username, folderid);
            string key = folder.folderBlowFishKey;
            string iv = folder.folderBlowFishIV;

            // File Retrieval
            SqlDataReader sqldr = cmd.ExecuteReader();
            MBFile mbf = new MBFile();
            if (sqldr.Read())
            {
                mbf.filecontent = DecryptionBlowfishFileFolder((byte[])sqldr["filecontent"], key, iv);
                mbf.fileName = sqldr["filename"].ToString();
                mbf.fileSize = (int)sqldr["filesize"];
                mbf.fileType = sqldr["filetype"].ToString();
                mbf.filetimestamp = Convert.ToDateTime(sqldr["filetimestamp"]);
            }
            if (mbf.fileSize == 0)
                return null;
            return mbf;
        }

        // When password is created,changed,deleted need to re encrypt
        public static void ReEncryptFiles(MBFolder oldfolder, MBFolder newFolder)
        {
            // Get User ID
            User user = User.GetUser(oldfolder.folderusername);

            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM mb_file WHERE userid = @userid AND folderid = @folderid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt, 8));
            cmd.Prepare();

            cmd.Parameters["@userid"].Value = user.UserId;
            cmd.Parameters["@folderid"].Value = oldfolder.folderid;


            // File Retrieval
            SqlDataReader sqldr = cmd.ExecuteReader();
            MBFile mbf;
            while(sqldr.Read())
            {
                mbf = new MBFile();
                mbf.fileName = sqldr["filename"].ToString();
                mbf.filecontent = MBFolder.DecryptionBlowfishFileFolder((byte[])sqldr["filecontent"], oldfolder.folderBlowFishKey, oldfolder.folderBlowFishIV);
                mbf.filecontent = MBFolder.EncryptionBlowfishFileFolder((byte[])sqldr["filecontent"], newFolder.folderBlowFishKey, newFolder.folderBlowFishIV);

                //Update
                SqlCommand fileUpdate = new SqlCommand(
                           "UPDATE mb_file SET filecontent=@filecontent WHERE userid= @userid and folderid=@folderid and filename=@filename",
                           SQLGetMBoxConnection());
                fileUpdate.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
                fileUpdate.Parameters.Add(new SqlParameter("@folderid",SqlDbType.BigInt,8));
                fileUpdate.Parameters.Add(new SqlParameter("@filename", SqlDbType.NVarChar, -1));
                fileUpdate.Parameters.Add(new SqlParameter("@filecontent", SqlDbType.VarBinary, -1));
                fileUpdate.Prepare();

                fileUpdate.Parameters["@userid"].Value = user.UserId;
                fileUpdate.Parameters["@folderid"].Value = oldfolder.folderid;
                fileUpdate.Parameters["@filename"].Value = mbf.fileName;
                fileUpdate.Parameters["@filecontent"].Value = mbf.filecontent;

                fileUpdate.ExecuteNonQuery();
            }
        }

        // Files in folder Blowfish448 Encryption
        private static byte[] EncryptionBlowfishFileFolder(byte[] filecontent, string key, string iv)
        {
            // Declare Blowfish
            BlowFish b = new BlowFish(key);
            // Get IV 
            byte[] blowiv = Convert.FromBase64String(iv);
            b.IV = blowiv;
            // Encrypt using CBC
            byte[] encryptedFile = b.Encrypt_CBC(filecontent);
            // Debug
            string ct = Convert.ToBase64String(encryptedFile);
            System.Diagnostics.Debug.WriteLine("Cipher Text: " + ct);

            return encryptedFile;
        }

        // Files in folder Blowfish448 Decryption
        public static byte[] DecryptionBlowfishFileFolder(byte[] filecontent, string key, string iv)
        {
            // Declare Blowfish
            BlowFish b = new BlowFish(key);
            // Get IV 
            byte[] blowiv = Convert.FromBase64String(iv);
            b.IV = blowiv;
            // Encrypt using CBC
            byte[] decryptedfile = b.Decrypt_CBC(filecontent);
            // Debug
            string pt = Convert.ToBase64String(decryptedfile);
            System.Diagnostics.Debug.WriteLine("Plain Text: " + pt);

            return decryptedfile;
        }

        // Generate BlowFish Key
        private string RandomPasswordGeneration(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()_-=~`";

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

        // Generate a salt
        private byte[] GenerateSaltFunction()
        {
            byte[] newSalt = new byte[16];
            using (RNGCryptoServiceProvider rngcsp = new RNGCryptoServiceProvider())
            {
                rngcsp.GetBytes(newSalt);
            }
            return newSalt;
        }

        public static bool UploadFileToFolder(MBFile file, string foldername)
        {
            try
            {
                // Get Folder ID
                MBFolder folder = MBFolder.GetFolder(file.fileusername, foldername);
                System.Diagnostics.Debug.WriteLine("Name: " + folder.folderName);
                System.Diagnostics.Debug.WriteLine("Key: " + folder.folderBlowFishKey);
                System.Diagnostics.Debug.WriteLine("IV: " + folder.folderBlowFishIV);
                // Get User ID
                User user = User.GetUser(file.fileusername);
                int userid = (int)user.UserId;

                // Encryption With Blowfish if there is a password
                string key = folder.folderBlowFishKey;
                string iv = folder.folderBlowFishIV;
                file.filecontent = EncryptionBlowfishFileFolder(file.filecontent, key, iv);

                // Insert Database into database
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO mb_file(folderid,userid,filename,filetype,filesize,filecontent,filetimestamp) "
                    + "values(@folderid,@userid,@name,@type,@size,@data,@timestamp)", SQLGetMBoxConnection());
                cmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt, 8));
                cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
                cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 50));
                cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.NVarChar, -1));
                cmd.Parameters.Add(new SqlParameter("@size", SqlDbType.Int, 4));
                cmd.Parameters.Add(new SqlParameter("@data", SqlDbType.VarBinary, -1));
                cmd.Parameters.Add(new SqlParameter("@timestamp", SqlDbType.DateTime2, 7));

                cmd.Prepare();
                cmd.Parameters["@folderid"].Value = folder.folderid;
                cmd.Parameters["@userid"].Value = userid;
                cmd.Parameters["@name"].Value = file.fileName;
                cmd.Parameters["@type"].Value = file.fileType;
                cmd.Parameters["@size"].Value = file.fileSize;
                cmd.Parameters["@data"].Value = file.filecontent;
                cmd.Parameters["@timestamp"].Value = file.filetimestamp;
                cmd.ExecuteNonQuery();               
                return true;
            }
            catch
            {
                return false;
            }
          
        }

        public static bool OverwriteFileToFolder(MBFile file, string foldername)
        {
            try
            {
                // Get Folder ID
                MBFolder folder = MBFolder.GetFolder(file.fileusername, foldername);
                System.Diagnostics.Debug.WriteLine("Name: " + folder.folderName);
                System.Diagnostics.Debug.WriteLine("Key: " + folder.folderBlowFishKey);
                System.Diagnostics.Debug.WriteLine("IV: " + folder.folderBlowFishIV);
                // Get User ID
                User user = User.GetUser(file.fileusername);
                int userid = (int)user.UserId;

                // Encryption With Blowfish if there is a password
                string key = folder.folderBlowFishKey;
                string iv = folder.folderBlowFishIV;
                file.filecontent = EncryptionBlowfishFileFolder(file.filecontent, key, iv);

                // Insert Database into database
                SqlCommand cmd = new SqlCommand(
                        "UPDATE mb_file SET filesize=@filesize,filetype=@filetype,filecontent=@filecontent,filetimestamp=@filetimestamp WHERE filename=@filename AND userid=@userid AND folderid=@folderid", SQLGetMBoxConnection());
                cmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt, 8));
                cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
                cmd.Parameters.Add(new SqlParameter("@filename", SqlDbType.VarChar, 50));
                cmd.Parameters.Add(new SqlParameter("@filetype", SqlDbType.NVarChar, -1));
                cmd.Parameters.Add(new SqlParameter("@filesize", SqlDbType.Int, 4));
                cmd.Parameters.Add(new SqlParameter("@filecontent", SqlDbType.VarBinary, -1));
                cmd.Parameters.Add(new SqlParameter("@filetimestamp", SqlDbType.DateTime2, 7));
                cmd.Prepare();

                cmd.Parameters["@folderid"].Value = folder.folderid;
                cmd.Parameters["@userid"].Value = user.UserId;
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

        public bool CreateNewFolder(MBFolder folder, string folderpassword)
        {
            try
            {
                // Get User ID
                User user = User.GetUser(folder.folderusername);
                long userid = user.UserId;

                //Generate New Salt
                byte[] newSalt = GenerateSaltFunction();
                string saltstring = Convert.ToBase64String(newSalt);

                // Generate Password Based on condition
                byte[] folderpassbyte;
                if (folderpassword != "")
                {
                    // Convert Password to byte array
                    folderpassbyte = Encoding.UTF8.GetBytes(folderpassword);

                    // Set Personal Password status
                    folder.folderencryptionstatus = true;
                }
                else
                {
                    folderpassbyte = Encoding.UTF8.GetBytes(RandomPasswordGeneration(16));
                    // Set Personal Password status
                    folder.folderencryptionstatus = false;
                }
                // Debug
                System.Diagnostics.Debug.WriteLine("Folder Password: " + Convert.ToBase64String(folderpassbyte));

                // Join Pass and salt together
                byte[] saltpassbyte = new byte[folderpassbyte.Length + newSalt.Length];
                folderpassbyte.CopyTo(saltpassbyte, 0);
                newSalt.CopyTo(saltpassbyte, folderpassbyte.Length);

                // Get key and iv for blowfish encryption
                Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(folderpassbyte, newSalt, 16);
                byte[] blowkey = key.GetBytes(56); // 32bytes is 256bits
                byte[] blowiv = key.GetBytes(8); // 8bytes is 64bits

                //Debug purpose only
                System.Diagnostics.Debug.WriteLine("Blowfish Key Length: " + blowkey.Length);
                System.Diagnostics.Debug.WriteLine("Blowfish IV Length: " + blowiv.Length);

                // Convert Key and IV to String
                string blowkeystring = Convert.ToBase64String(blowkey);
                string blowivstring = Convert.ToBase64String(blowiv);

                //Debug
                System.Diagnostics.Debug.WriteLine("Blowfish key: " + blowkeystring);
                System.Diagnostics.Debug.WriteLine("Blowfish IV: " + blowivstring);

                // Get Password Hash
                string passhash;
                using (SHA512 shaCalc = new SHA512Managed())
                {
                    passhash = Convert.ToBase64String(shaCalc.ComputeHash(saltpassbyte));
                }
                System.Diagnostics.Debug.WriteLine("Hash: " + passhash);

                // Create Folder
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO mb_folder(userid,foldername,folderencryption,foldersaltfunction,folderpassword,folderkey,folderiv,foldertimestamp) "
                    + "VALUES(@user,@name,@encryptionstatus,@salt,@pass,@key,@iv,@timestamp)", SQLGetMBoxConnection());
                cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.BigInt, 8));
                cmd.Parameters.Add(new SqlParameter("@name", SqlDbType.VarChar, 50));
                cmd.Parameters.Add(new SqlParameter("@encryptionstatus", SqlDbType.Bit, 1));
                cmd.Parameters.Add(new SqlParameter("@salt", SqlDbType.VarChar, 24));
                cmd.Parameters.Add(new SqlParameter("@pass", SqlDbType.VarChar, 88));
                cmd.Parameters.Add(new SqlParameter("@key", SqlDbType.VarChar, 64));
                cmd.Parameters.Add(new SqlParameter("@iv", SqlDbType.VarChar, 32));
                cmd.Parameters.Add(new SqlParameter("@timestamp",SqlDbType.DateTime2,7));
                cmd.Prepare();
                cmd.Parameters["@user"].Value = userid;
                cmd.Parameters["@name"].Value = folder.folderName;
                cmd.Parameters["@encryptionstatus"].Value = folder.folderencryptionstatus;
                cmd.Parameters["@salt"].Value = saltstring;
                cmd.Parameters["@pass"].Value = passhash;
                cmd.Parameters["@key"].Value = blowkeystring;
                cmd.Parameters["@iv"].Value = blowivstring;
                cmd.Parameters["@timestamp"].Value = folder.foldertimestamp;

                cmd.ExecuteNonQuery();

                // Log the entry for folder creation
                FileLogger.Instance.FolderCreated(user.UserId, folder.folderName);

                return true;
            }
            catch
            {
                return false;
            }
        }

        // Delete Folder
        public static void DeleteFolder(long folderid)
        {
            System.Diagnostics.Debug.WriteLine("What is this? "+folderid);
            SqlCommand filecmd = new SqlCommand(
                  "DELETE FROM mb_file WHERE folderid=@folderid", SQLGetMBoxConnection());
            filecmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt, 8));
            filecmd.Prepare();
            filecmd.Parameters["@folderid"].Value = folderid;
            filecmd.ExecuteNonQuery();

            SqlCommand foldercmd = new SqlCommand(
                   "DELETE FROM mb_folder WHERE folderid=@folderid", SQLGetMBoxConnection());
            foldercmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt, 8));
            foldercmd.Prepare();

            foldercmd.Parameters["@folderid"].Value = folderid;
            foldercmd.ExecuteNonQuery();


        }


        // Validate Folder Password
        public bool ValidateFolderPassword(MBFolder folder, string folderpassword)
        {
            MBFolder checkFolder = MBFolder.GetFolder(folder.folderusername, folder.folderName);

            // Database Folder Password and salt
            string folderHash = checkFolder.folderpassword;
            byte[] saltBytes = Convert.FromBase64String(checkFolder.foldersalt);

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

        // Folder Password Settings
        public bool FolderPasswordSettings(MBFolder folder, string folderpassword,bool personalEncryption)
        {
            
            MBFolder oldFolder = folder;
            MBFolder newFolder = new MBFolder();
            newFolder.folderName = oldFolder.folderName;

            // Get UserID
            User user = User.GetUser(folder.folderusername);
            long userid = user.UserId;

            // Generate New salt function
            byte[] newFolderSalt = GenerateSaltFunction();
            newFolder.foldersalt= Convert.ToBase64String(newFolderSalt);

            byte[] newPwBytes;
            if (folderpassword!="")
            {
                // Convert new password to byte array
                 newPwBytes = Encoding.UTF8.GetBytes(folderpassword);
            }else
            {
                // Make a password for them
                newPwBytes = Encoding.UTF8.GetBytes(RandomPasswordGeneration(16));
            }

            // Join two arrays
            byte[] combinedBytes = new byte[newPwBytes.Length + newFolderSalt.Length];
            newPwBytes.CopyTo(combinedBytes, 0);
            newFolderSalt.CopyTo(combinedBytes, newPwBytes.Length);

            // New Hash combined arrays
            using (SHA512 shaCalc = new SHA512Managed())
            {
                newFolder.folderpassword = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
            }

            // Get key and iv for blowfish encryption
            Rfc2898DeriveBytes key = new Rfc2898DeriveBytes(newPwBytes, newFolderSalt, 16);
            byte[] blowkey = key.GetBytes(56); // 32bytes is 256bits
            byte[] blowiv = key.GetBytes(8); // 8bytes is 64bits

            // Convert Key and IV to String
            newFolder.folderBlowFishKey = Convert.ToBase64String(blowkey);
            newFolder.folderBlowFishIV = Convert.ToBase64String(blowiv);

            // Update all Files with new encryption in this folder
            MBFolder.ReEncryptFiles(oldFolder, newFolder);

            try
            {
                // Create Folder entry in database
                SqlCommand cmd = new SqlCommand(
                           "UPDATE mb_folder SET folderpassword = @newfolderpass , foldersaltfunction = @newSalt, folderkey=@folderkey, folderiv=@folderiv,folderencryption=@folderencryption WHERE foldername = @foldername and userid= @userid",
                           SQLGetMBoxConnection());
                cmd.Parameters.Add(new SqlParameter("@newfolderpass", SqlDbType.VarChar, 88));
                cmd.Parameters.Add(new SqlParameter("@newSalt", SqlDbType.VarChar, 24));
                cmd.Parameters.Add(new SqlParameter("@folderkey", SqlDbType.VarChar, 64));
                cmd.Parameters.Add(new SqlParameter("@folderiv", SqlDbType.VarChar, 32));
                cmd.Parameters.Add(new SqlParameter("@foldername", SqlDbType.VarChar, 50));
                cmd.Parameters.Add(new SqlParameter("@folderencryption", SqlDbType.Bit,1));
                cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
                cmd.Prepare();

                cmd.Parameters["@newfolderpass"].Value = newFolder.folderpassword;
                cmd.Parameters["@newSalt"].Value = newFolder.foldersalt;
                cmd.Parameters["@folderkey"].Value = newFolder.folderBlowFishKey;
                cmd.Parameters["@folderiv"].Value = newFolder.folderBlowFishIV;
                cmd.Parameters["@foldername"].Value = newFolder.folderName;
                cmd.Parameters["@folderencryption"].Value = personalEncryption;
                cmd.Parameters["@userid"].Value = userid;
                cmd.ExecuteNonQuery();

                return true;
            }
            catch
            {
                return false;
            }

        }
       
        // Get SQL Connection
        private static SqlConnection SQLGetMBoxConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        // Getter and setter
        public static MBFolder GetFolder(string username, string foldername)
        {
            MBFolder folder = new MBFolder();
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM mb_folder WHERE foldername = @foldername and userid= @userid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@foldername", SqlDbType.VarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@foldername"].Value = foldername;
            cmd.Parameters["@userid"].Value = user.UserId;

            SqlDataReader folderReader = cmd.ExecuteReader();
            if (folderReader.Read())
            {           
                folder.folderid = (long)folderReader["folderid"];
                folder.folderName = folderReader["foldername"].ToString();
                folder.folderusername = username;
                folder.folderBlowFishKey = folderReader["folderkey"].ToString();
                folder.folderBlowFishIV = folderReader["folderiv"].ToString();
                folder.folderpassword = folderReader["folderpassword"].ToString();
                folder.foldersalt = folderReader["foldersaltfunction"].ToString();
                folder.folderencryptionstatus = (bool)folderReader["folderencryption"];
                folder.foldertimestamp = Convert.ToDateTime(folderReader["foldertimestamp"]);
            }
            return folder;
        }

        public static MBFolder GetFolder(string username, long folderid)
        {
            MBFolder folder = new MBFolder();
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM mb_folder WHERE folderid = @folderid and userid= @user", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.VarChar, 50));
            cmd.Parameters.Add(new SqlParameter("@user", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@folderid"].Value = folderid;
            cmd.Parameters["@user"].Value = user.UserId;

            SqlDataReader folderReader = cmd.ExecuteReader();
            if (folderReader.Read())
            {
                folder.folderid = Convert.ToInt64(folderReader["folderid"].ToString());
                folder.folderName = folderReader["foldername"].ToString();
                folder.folderusername = username;
                folder.folderpassword = folderReader["folderpassword"].ToString();
                folder.foldersalt = folderReader["foldersaltfunction"].ToString();
                folder.folderBlowFishKey = folderReader["folderkey"].ToString();
                folder.folderBlowFishIV = folderReader["folderiv"].ToString();
                folder.folderencryptionstatus = (bool)folderReader["folderencryption"];
                folder.foldertimestamp = Convert.ToDateTime(folderReader["foldertimestamp"]);

            }
            return folder;
        }

    }
}
