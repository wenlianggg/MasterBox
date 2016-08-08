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
											DateTime regStamp, byte[] aesKeyEnc,
											string aesIV, bool isAdmin
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
				"aesIV = @aesIV, " +
				"isAdmin = @isAdmin " +
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
			cmd.Parameters.Add(new SqlParameter("@isAdmin", SqlDbType.Bit, 0));
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
			cmd.Parameters["@isAdmin"].Value = isAdmin;
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
            SqlCommand cmd = new SqlCommand("UPDATE mb_users SET steghash = @steghash WHERE userid = @userid", sqlConn);
            cmd.Parameters.Add(new SqlParameter("@steghash", SqlDbType.VarChar, 88));
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int, 8));
            cmd.Prepare();
			if (hash != null && hash.Length == 88)
				cmd.Parameters["@steghash"].Value = hash;
			else
				cmd.Parameters["@steghash"].Value = DBNull.Value;
			cmd.Parameters["@userid"].Value = userid;

            return cmd.ExecuteNonQuery();

        }

        internal int SqlAddCoupon(string couponvalue, int days)
        {
            SqlCommand cmd = new SqlCommand("INSERT INTO mb_coupon (couponcode, freedays, stat) VALUES (@couponcode, @freedays, 0)", sqlConn);
            cmd.Parameters.Add(new SqlParameter("@couponcode", SqlDbType.VarChar, 16));
            cmd.Parameters.Add(new SqlParameter("@freedays", SqlDbType.Int, 2));
            cmd.Prepare();

            cmd.Parameters["@couponcode"].Value = couponvalue;
            cmd.Parameters["@freedays"].Value = days;

            return cmd.ExecuteNonQuery();
        }

        internal int SqlDeleteCoupon(string couponvalue)
        {
            SqlCommand cmd = new SqlCommand("DELETE FROM mb_coupon WHERE couponcode = @couponcode", sqlConn);
            cmd.Parameters.Add(new SqlParameter("@couponcode", SqlDbType.VarChar, 16));
            cmd.Prepare();

            cmd.Parameters["@couponcode"].Value = couponvalue;

            return cmd.ExecuteNonQuery();
        }

        internal bool SqlCheckCoupon(string couponcode)
        {
            // Check for such coupon
            SqlCommand cmd = new SqlCommand("SELECT COUNT(*) FROM mb_coupon WHERE couponcode = @couponcode", sqlConn);
            cmd.Parameters.Add(new SqlParameter("@couponcode", SqlDbType.VarChar, 16));
            cmd.Parameters["@couponcode"].Value = couponcode;
            cmd.Prepare();
            int couponcount = (int)cmd.ExecuteScalar();

            if (couponcount > 0)
            {
                SqlCommand stat = new SqlCommand("SELECT stat FROM mb_coupon WHERE couponcode = @couponcode", sqlConn);
                stat.Parameters.Add(new SqlParameter("@couponcode", SqlDbType.VarChar, 16));
                stat.Parameters["@couponcode"].Value = couponcode;
                stat.Prepare();

                int status = 0;

                SqlDataReader sqldr = stat.ExecuteReader();
                while(sqldr.Read())
                {
                    status = Convert.ToInt32(sqldr["stat"].ToString());
                }
                      

                if (status != 1) {
                    SqlCommand upd = new SqlCommand("UPDATE mb_coupon SET stat = 1 WHERE couponcode = @couponcode", sqlConn);
                    upd.Parameters.Add(new SqlParameter("@couponcode", SqlDbType.VarChar, 16));
                    upd.Parameters["@couponcode"].Value = couponcode;
                    upd.ExecuteNonQuery();
                    return true;
                }else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }



        internal int SqlUpdateSentCoupon(string couponcode)
        {
            SqlCommand upd = new SqlCommand("UPDATE mb_coupon SET sent = 1 WHERE couponcode = @couponcode", sqlConn);
            upd.Parameters.Add(new SqlParameter("@couponcode", SqlDbType.VarChar, 16));
            upd.Parameters["@couponcode"].Value = couponcode;
            return upd.ExecuteNonQuery();
        }

        internal int SqlUpdateVericode(string username, string vericode) {
            // Update database values
            SqlCommand cmd = new SqlCommand(
                "UPDATE mb_users SET vericode = @vericode WHERE username = @username", sqlConn);
            cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 30));
            cmd.Parameters.Add(new SqlParameter("@vericode", SqlDbType.VarChar, 32));
            cmd.Prepare();
            cmd.Parameters["@username"].Value = username;
            cmd.Parameters["@vericode"].Value = vericode;
            return cmd.ExecuteNonQuery();
        }

        /*
		 *  STORED FUNCTIONS FOR DATA RETRIEVAL
		 */

        internal string SqlGetVerificationCode(string username) {
            SqlCommand cmd = new SqlCommand(
                    "SELECT vericode FROM mb_users WHERE username = @uname",
                    sqlConn);
            cmd.Parameters.Add(new SqlParameter("@uname", SqlDbType.VarChar, 30));
            cmd.Prepare();
            cmd.Parameters["@uname"].Value = username;
            SqlDataReader sqldr = cmd.ExecuteReader();
            if (sqldr.Read()) {
                return sqldr[0].ToString();
            } else {
                return null;
            }
        }

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

		internal SqlDataReader SqlGetServerLogs() {
			SqlCommand cmd = new SqlCommand(
				"SELECT logid, userid, logdesc, logip, loglevel, logtime FROM mb_logs ORDER BY logtime DESC",
				sqlConn);
			cmd.Prepare();
			return cmd.ExecuteReader();
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

		internal string SqlGetUserName(int uid) {
			SqlCommand cmd = new SqlCommand(
				"SELECT DISTINCT username FROM mb_users WHERE userid = @uid", sqlConn);
			cmd.Parameters.Add(new SqlParameter("@uid", SqlDbType.Int, 0));
			cmd.Prepare();
			cmd.Parameters["@uid"].Value = uid;
			SqlDataReader sqldr = cmd.ExecuteReader();
			if (sqldr.Read()) {
				return (string)sqldr[0];
			} else {
				return "No user";
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

        internal DataTable SqlGetAllCoupons()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM mb_coupon", sqlConn);
            cmd.Prepare();
            DataTable data = new DataTable();
            data.Load(cmd.ExecuteReader());
            data.Columns["couponcode"].ColumnName = "Coupon Code";
            data.Columns["freedays"].ColumnName = "Days Given";
            data.Columns["stat"].ColumnName = "Redeemed?";
            data.Columns["sent"].ColumnName = "Sent?";
            return data;
        }

        internal SqlDataReader SqlGetCouponReader()
        {
            SqlCommand cmd = new SqlCommand("SELECT * FROM mb_coupon", sqlConn);
            cmd.Prepare();
            SqlDataReader sldr = cmd.ExecuteReader();
            return sldr;
        }

        internal string SqlGetRandomUsername()
        {
            SqlCommand cmd = new SqlCommand("SELECT TOP 1 username FROM mb_users ORDER BY NEWID()", sqlConn);
            cmd.Prepare();
            SqlDataReader sqldr = cmd.ExecuteReader();

            if (sqldr.Read())
            {
                return sqldr["username"].ToString();
            }else
            {
                return "NaN";
            }
        }
        
        internal DataTable SqlGetUnredeemedCpn()
        {
            SqlCommand cmd = new SqlCommand("SELECT couponcode FROM mb_coupon WHERE sent= 0 and stat = 0", sqlConn);
            cmd.Prepare();

            DataTable dt = new DataTable();
            dt.Load(cmd.ExecuteReader());

            return dt;
        }

        internal DataTable SqlGetUserSubscriptions()
        {
            SqlCommand cmd = new SqlCommand("SELECT username, mbrType, mbrStart, mbrExpiry FROM mb_users", sqlConn);
            cmd.Prepare();
            DataTable data = new DataTable();
            data.Load(cmd.ExecuteReader());
            data.Columns["username"].ColumnName = "Username";
            data.Columns["mbrType"].ColumnName = "Member Type";
            data.Columns["mbrStart"].ColumnName = "Subscription Start";
            data.Columns["mbrExpiry"].ColumnName = "Subscription Expiry";
            return data;
        }

        internal DateTime SqlGetUserMbrStart(string username)
        {
            SqlCommand cmd = new SqlCommand("SELECT mbrStart from mb_users WHERE username = @username", sqlConn);
            cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 50));
            cmd.Parameters["@username"].Value = username;
            cmd.Prepare();

            SqlDataReader sqldr = cmd.ExecuteReader();

            if (sqldr.Read())
            {
                return (DateTime)sqldr["mbrStart"];
            }
            else
            {
                DateTime dt = DateTime.Now;
                return dt;
            }
        }

        internal DateTime SqlGetUserMbrExpiry(string username)
        {
            SqlCommand cmd = new SqlCommand("SELECT mbrExpiry FROM mb_users WHERE username = @username", sqlConn);
            cmd.Parameters.Add(new SqlParameter("@username", SqlDbType.VarChar, 50));
            cmd.Parameters["@username"].Value = username;
            cmd.Prepare();

            SqlDataReader sqldr = cmd.ExecuteReader();

            if (sqldr.Read())
            {
                return (DateTime)sqldr["mbrExpiry"];
            }
            else
            {
                DateTime dt = DateTime.Now;
                return dt;
            }
        }

        internal int SqlGetCouponDays(string couponcode)
        {
            SqlCommand cmd = new SqlCommand("SELECT freedays FROM mb_coupon WHERE couponcode = @couponcode", sqlConn);
            cmd.Parameters.Add(new SqlParameter("@couponcode", SqlDbType.VarChar, 16));
            cmd.Parameters["@couponcode"].Value = couponcode;
            cmd.Prepare();

            SqlDataReader sqldr = cmd.ExecuteReader();

            if (sqldr.Read())
            {
                return Convert.ToInt32(sqldr["freedays"].ToString());
            }
            else
            {
                return 0;
            }
        }
    }
}