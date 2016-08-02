using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;

/// Author: Goh Wen Liang (154473G) 

namespace MasterBox.Auth {
	internal class DataAccess : IDisposable {
		private static string connString = ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString;
        private static long queriescounter = 0;
        private static System.Diagnostics.Stopwatch sw;
		private SqlConnection _sqlConn;

		private SqlConnection sqlConn {
			get {
				if (_sqlConn != null && _sqlConn.State == ConnectionState.Open) {
					_sqlConn.Close();
				}

				_sqlConn = new SqlConnection(connString);
				_sqlConn.Open();
                queriescounter++;
				return _sqlConn;
			}
		}

		~ DataAccess() { // DataAccess destructor
			Dispose();
		}

		public void Dispose() {
			Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing) {
			if (disposing) {
                sw.Stop();
                System.Diagnostics.Debug.Write("...done! (Took " + sw.ElapsedMilliseconds + "ms)");
                if (_sqlConn != null) {
					_sqlConn.Close();
					_sqlConn.Dispose();
				}
			}
		}
        /*
         *  CONSTRUCTOR
         */

        internal DataAccess([CallerMemberName]string memberName = "") {
            // Print out caller class
            sw = System.Diagnostics.Stopwatch.StartNew();
            System.Diagnostics.Debug.Write("\n" + queriescounter + ". DataAccess Entity Created From: " + memberName);
        }

		/*
		 *  STORED FUNCTIONS FOR UPDATING DATABASE
		 */

		internal int SqlInsertBlockEntry(IPBlockEntry ipbe) {
			SqlCommand cmd = new SqlCommand(
				"INSERT INTO mb_ipblock (userid, address, expiry, reason) VALUES (@userid, @address, @expiry, @reason)", sqlConn);
			cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int, 0));
			cmd.Parameters.Add(new SqlParameter("@address", SqlDbType.VarChar, 50));
			cmd.Parameters.Add(new SqlParameter("@expiry", SqlDbType.DateTime2, 7));
			cmd.Parameters.Add(new SqlParameter("@reason", SqlDbType.VarChar, 255));
			cmd.Prepare();

			cmd.Parameters["@userid"].Value = ipbe.UserID;
			if (ipbe.IPAddress != null)
				cmd.Parameters["@address"].Value = ipbe.IPAddress;
			else
				cmd.Parameters["@address"].Value = DBNull.Value;

			if (ipbe.Expiry != null)
				cmd.Parameters["@expiry"].Value = ipbe.Expiry;
			else
				cmd.Parameters["@expiry"].Value = DBNull.Value;

			if (ipbe.Reason != null)
				cmd.Parameters["@reason"].Value = ipbe.Reason;
			else
				cmd.Parameters["@reason"].Value = DBNull.Value;

			return cmd.ExecuteNonQuery();
		}
		
		internal int SqlInsertLogEntry(int userid, string ipaddress, string description, int loglevel, int logtype) {
			// Update database values
			SqlCommand cmd = new SqlCommand(
				"INSERT INTO mb_logs (userid, logip, logdesc, loglevel, logtype) VALUES (@userid, @logip, @logdesc, @loglevel, @logtype)", sqlConn);
			cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int, 0));
			cmd.Parameters.Add(new SqlParameter("@logip", SqlDbType.VarChar, 45));
			cmd.Parameters.Add(new SqlParameter("@logdesc", SqlDbType.VarChar, 255));
			cmd.Parameters.Add(new SqlParameter("@loglevel", SqlDbType.Int, 0));
            cmd.Parameters.Add(new SqlParameter("@logtype", SqlDbType.TinyInt, 0));
            cmd.Prepare();

			cmd.Parameters["@userid"].Value = userid;
			cmd.Parameters["@logip"].Value = ipaddress;
			cmd.Parameters["@logdesc"].Value = description;
			cmd.Parameters["@loglevel"].Value = loglevel;
            cmd.Parameters["@logtype"].Value = logtype;

            return cmd.ExecuteNonQuery();
		}

        internal int SqlUpdateHashSalt(string username, string hash, string salt) {
            if (User.Exists(username)) {
                // Update database values
                SqlCommand cmd = new SqlCommand(
                    "UPDATE mb_users SET hash = @newHash, salt = @newSalt WHERE username = @uname", sqlConn);
                cmd.Parameters.Add(new SqlParameter("@uname", SqlDbType.VarChar, 30));
                cmd.Parameters.Add(new SqlParameter("@newHash", SqlDbType.VarChar, 88));
                cmd.Parameters.Add(new SqlParameter("@newSalt", SqlDbType.VarChar, 24));
                cmd.Prepare();

                cmd.Parameters["@uname"].Value = username;
                cmd.Parameters["@newHash"].Value = hash;
                cmd.Parameters["@newSalt"].Value = salt;
                return cmd.ExecuteNonQuery();
            } else {
                // Update database values
                SqlCommand cmd = new SqlCommand(
                    "INSERT INTO mb_users (username, hash, salt) VALUES (@uname, @newHash, @newSalt)", sqlConn);
                cmd.Parameters.Add(new SqlParameter("@uname", SqlDbType.VarChar, 30));
                cmd.Parameters.Add(new SqlParameter("@newHash", SqlDbType.VarChar, 88));
                cmd.Parameters.Add(new SqlParameter("@newSalt", SqlDbType.VarChar, 24));
                cmd.Prepare();

                cmd.Parameters["@uname"].Value = username;
                cmd.Parameters["@newHash"].Value = hash;
                cmd.Parameters["@newSalt"].Value = salt;
                return cmd.ExecuteNonQuery();
            }
		}

		internal bool SqlUpdateUserValue(int userid, string fieldName, object fieldValue, SqlDbType sdb, int length) {
			SqlCommand cmd = new SqlCommand(
				"UPDATE mb_users SET " + fieldName + " = @fieldValue WHERE userid = @uid", sqlConn);
			cmd.Parameters.Add(new SqlParameter("@fieldValue", sdb, length));
			cmd.Parameters.Add(new SqlParameter("@uid", SqlDbType.Int, 0));
			cmd.Prepare();

			cmd.Parameters["@fieldValue"].Value = (fieldValue != null) ? fieldValue : DBNull.Value;
			cmd.Parameters["@uid"].Value = userid;
			if (cmd.ExecuteNonQuery() == 1) {
				return true;
			} else {
				throw new DatabaseUpdateFailureException("Updating value " + fieldValue + " failed.");
			}
		}

		internal int SqlUpdateAllUserValues(int userid, string username,
											byte[] fNameEnc, byte[] lNameEnc,
											DateTime dob, byte[] emailEnc,
											bool verified, int mbrType,
											DateTime mbrStart, DateTime mbrExpiry,
											DateTime regStamp, byte[] aesKeyEnc, string aesIV
											) {
			if (userid == 0) userid = SqlGetUserId(username);

			SqlCommand cmd = new SqlCommand(
				"UPDATE mb_users SET " +
                "fNameEnc = @fNameEnc," +
				"lNameEnc = @lNameEnc," +
				"dob = @dob," +
				"emailEnc = @emailEnc," +
				"verified = @verified," +
				"mbrType = @mbrType," +
				"mbrStart = @mbrStart," +
				"mbrExpiry = @mbrExpiry," +
				"regStamp = @regStamp, " +
                "aesKey = @aesKey, " + 
				"aesIV = @aesIV " +
				"WHERE userid = @userid;",
				sqlConn);
			cmd.Parameters.Add(new SqlParameter("@fNameEnc", SqlDbType.VarBinary, 200));
			cmd.Parameters.Add(new SqlParameter("@lNameEnc", SqlDbType.VarBinary, 200));
			cmd.Parameters.Add(new SqlParameter("@dob", SqlDbType.Date, 0));
			cmd.Parameters.Add(new SqlParameter("@emailEnc", SqlDbType.VarBinary, 200));
			cmd.Parameters.Add(new SqlParameter("@verified", SqlDbType.Bit, 0));
			cmd.Parameters.Add(new SqlParameter("@mbrType", SqlDbType.Int, 0));
			cmd.Parameters.Add(new SqlParameter("@mbrStart", SqlDbType.DateTime2, 7));
			cmd.Parameters.Add(new SqlParameter("@mbrExpiry", SqlDbType.DateTime2, 7));
			cmd.Parameters.Add(new SqlParameter("@regStamp", SqlDbType.DateTime2, 7));
			cmd.Parameters.Add(new SqlParameter("@aesIV", SqlDbType.VarChar, 40));
			cmd.Parameters.Add(new SqlParameter("@aesKey", SqlDbType.VarBinary, 100));
			cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int, 8));
			cmd.Prepare();

			cmd.Parameters["@fNameEnc"].Value = fNameEnc;
			cmd.Parameters["@lNameEnc"].Value = lNameEnc;
			cmd.Parameters["@dob"].Value = dob;
			cmd.Parameters["@emailEnc"].Value = emailEnc;
			cmd.Parameters["@verified"].Value = verified;
			cmd.Parameters["@mbrType"].Value = mbrType;
			cmd.Parameters["@mbrStart"].Value = mbrStart;
			cmd.Parameters["@mbrExpiry"].Value = mbrExpiry;
			cmd.Parameters["@regStamp"].Value = regStamp;
			cmd.Parameters["@aesKey"].Value = aesKeyEnc;
			cmd.Parameters["@aesIV"].Value = aesIV;
			cmd.Parameters["@userid"].Value = userid;

			return cmd.ExecuteNonQuery();
		}

        internal int SqlUpdateMbrType(int userid, int mbrType)  {
            SqlCommand cmd = new SqlCommand("UPDATE mb_users SET mbrType = @mbrType WHERE userid = @userid", sqlConn);
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int, 8));
            cmd.Parameters.Add(new SqlParameter("@mbrType", SqlDbType.Int, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = userid;
            cmd.Parameters["@mbrType"].Value = mbrType;

            return cmd.ExecuteNonQuery();
        }

        internal int SqlSetImageHash(int userid, string hash) {
            if (hash.Length == 88) {
                SqlCommand cmd = new SqlCommand("UPDATE mb_users SET steghash = @steghash WHERE userid = @userid", sqlConn);
                cmd.Parameters.Add(new SqlParameter("@steghash", SqlDbType.VarChar, 88));
                cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int, 8));
                cmd.Prepare();
                cmd.Parameters["@steghash"].Value = hash;
                cmd.Parameters["@userid"].Value = userid;

                return cmd.ExecuteNonQuery();
            } else {
                return 0;
            }
        }

		/*
		 *  STORED FUNCTIONS FOR DATA RETRIEVAL
		 */

		internal SqlDataReader SqlGetBlockList() {
			SqlCommand cmd = new SqlCommand(
				"SELECT * from mb_ipblock", sqlConn);
			cmd.Prepare();
			return cmd.ExecuteReader();
		}

        internal List<int> SqlGetAllUserIds() {
            SqlCommand cmd = new SqlCommand(
                "SELECT userid FROM mb_users",
                sqlConn);
            cmd.Prepare();
            using (SqlDataReader sqldr = cmd.ExecuteReader()) {
                List<int> idlist = new List<int>();
                while(sqldr.Read()) {
                    idlist.Add((int) sqldr[0]);
                }
                return idlist;
            }
        }

		internal SqlDataReader SqlGetAuth(string username) {
			SqlCommand cmd = new SqlCommand(
				"SELECT userid, username, hash, salt, totpsecret, totpbackup FROM mb_users WHERE username = @uname",
				sqlConn);
			cmd.Parameters.Add(new SqlParameter("@uname", SqlDbType.VarChar, 30));
			cmd.Prepare();
			cmd.Parameters["@uname"].Value = username;
			return cmd.ExecuteReader();
		}

		internal SqlDataReader SqlGetUserLogs(int userid, int type) {
			SqlCommand cmd = new SqlCommand(
				"SELECT logdesc, logip, loglevel, logtime FROM mb_logs WHERE userid = @userid AND logtype = @type ORDER BY logtime DESC",
				sqlConn);
			cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int, 0));
            cmd.Parameters.Add(new SqlParameter("@type", SqlDbType.TinyInt, 0));
            cmd.Prepare();
			cmd.Parameters["@userid"].Value = userid;
            cmd.Parameters["@type"].Value = type;
			return cmd.ExecuteReader();
		}

		internal DataTable SqlGetServerLogs() {
			SqlCommand cmd = new SqlCommand(
				"SELECT logid, userid, logdesc, logip, loglevel, logtime FROM mb_logs ORDER BY logtime DESC",
				sqlConn);
			cmd.Prepare();
			DataTable data = new DataTable();
			data.Load(cmd.ExecuteReader());
			data.Columns["logid"].ColumnName = "ID";
			data.Columns["userid"].ColumnName = "User Name";
			data.Columns["logdesc"].ColumnName = "Log Description";
			data.Columns["logip"].ColumnName = "Logged IP";
			data.Columns["loglevel"].ColumnName = "Severity";
			data.Columns["logtime"].ColumnName = "Time";
			return data;
		}


		internal SqlDataReader SqlGetUser(int userid) {
			if (userid == 0) throw new UserNotFoundException();
			SqlCommand cmd = new SqlCommand(
				"SELECT * FROM mb_users WHERE userid = @uid",
				sqlConn);

			SqlParameter unameParam = new SqlParameter("@uid", SqlDbType.Int, 0);
			cmd.Parameters.Add(unameParam);

			cmd.Prepare();
			cmd.Parameters["@uid"].Value = userid;
			return cmd.ExecuteReader();
		}

		internal SqlDataReader SqlGetUser(string username) {
			return SqlGetUser(User.ConvertToId(username));
		}

		internal int SqlGetUserId(string username) {
			SqlCommand cmd = new SqlCommand(
				"SELECT DISTINCT userid FROM mb_users WHERE username = @un", sqlConn);
			cmd.Parameters.Add(new SqlParameter("@un", SqlDbType.VarChar, 30));
			cmd.Prepare();
			cmd.Parameters["@un"].Value = username;
			SqlDataReader sqldr = cmd.ExecuteReader();
			if (sqldr.Read()) {
				return (int)sqldr[0];
			} else {
				throw new UserNotFoundException(username);
			}
		}

		internal string SqlGetUserIV(int userid) {
			SqlCommand cmd = new SqlCommand(
				"SELECT DISTINCT aesIV FROM mb_users WHERE userid = @uid", sqlConn);
			cmd.Parameters.Add(new SqlParameter("@uid", SqlDbType.Int, 0));
			cmd.Prepare();
			cmd.Parameters["@uid"].Value = userid;
			SqlDataReader sqldr = cmd.ExecuteReader();
			if (sqldr.Read()) {
				return sqldr[0].ToString();
			} else {
				throw new UserNotFoundException();
			}
		}

        internal string SqlGetImageHash(int userid) {
            SqlCommand cmd = new SqlCommand("SELECT steghash FROM mb_users WHERE userid = @userid", sqlConn);
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = userid;
            SqlDataReader sqldr = cmd.ExecuteReader();
            if (sqldr.Read()) {
                return sqldr["steghash"].ToString();
            }
            return null;
        }

    }
}