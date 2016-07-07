using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

/// Author: Goh Wen Liang (154473G) 

namespace MasterBox.Auth {
	internal class DataAccess : IDisposable {
		private static string connString = ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString;
		private SqlConnection sqlConn;

		internal DataAccess() { // DataAccess constructor
			sqlConn = new SqlConnection(connString);
			sqlConn.Open();
		}

		~ DataAccess() { // DataAccess destructor
			Dispose();
		}

		public void Dispose() {
			Dispose(true);

		}

		protected virtual void Dispose(bool disposing) {
			if (disposing) {
				if (sqlConn != null)
					sqlConn.Dispose();
			}
		}

		/*
		 *  STORED FUNCTIONS FOR UPDATING DATABASE
		 */
		
		internal int SqlInsertLogEntry(int userid, string ipaddress, string description, Logger.LogLevel loglevel) {
			// Update database values
			SqlCommand cmd = new SqlCommand(
				"INSERT INTO mb_logs (userid, logip, logdesc, loglevel) VALUES (@userid, @logip, @logdesc, @loglevel)", sqlConn);
			cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int, 0));
			cmd.Parameters.Add(new SqlParameter("@logip", SqlDbType.VarChar, 45));
			cmd.Parameters.Add(new SqlParameter("@logdesc", SqlDbType.VarChar, 255));
			cmd.Parameters.Add(new SqlParameter("@loglevel", SqlDbType.Int, 0));
			cmd.Prepare();

			cmd.Parameters["@userid"].Value = userid;
			cmd.Parameters["@logip"].Value = ipaddress;
			cmd.Parameters["@logdesc"].Value = description;
			cmd.Parameters["@loglevel"].Value = (int)loglevel;
			return cmd.ExecuteNonQuery();
		}

		internal int SqlUpdateHashSalt(string username, string hash, string salt, bool useDefaultDA = true) {
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

		internal bool SqlUpdateUserValue(int userid, string fieldName, object fieldValue, SqlDbType sdb, int length) {
			SqlCommand cmd = new SqlCommand(
				"UPDATE mb_users SET " + fieldName + " = @fieldValue WHERE userid = @uid", sqlConn);
			cmd.Parameters.Add(new SqlParameter("@fieldValue", sdb, length));
			cmd.Parameters.Add(new SqlParameter("@uid", SqlDbType.Int, 0));
			cmd.Prepare();

			cmd.Parameters["@fieldValue"].Value = fieldValue;
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

		/*
		 *  STORED FUNCTIONS FOR DATA RETRIEVAL
		 */

		internal SqlDataReader SqlGetAuth(string username) {
			SqlCommand cmd = new SqlCommand(
				"SELECT userid, username, hash, salt, totpsecret, totpbackup FROM mb_users WHERE username = @uname",
				sqlConn);
			cmd.Parameters.Add(new SqlParameter("@uname", SqlDbType.VarChar, 30));
			cmd.Prepare();
			cmd.Parameters["@uname"].Value = username;
			return cmd.ExecuteReader();
		}

		internal DataTable SqlGetLogs(int userid) {
			SqlCommand cmd = new SqlCommand(
				"SELECT userid, logdesc, logip, loglevel, logtime FROM mb_logs ORDER BY logtime DESC",
				sqlConn);
			cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.Int, 0));
			cmd.Prepare();
			cmd.Parameters["@userid"].Value = userid;
			DataTable data = new DataTable();
			data.Load(cmd.ExecuteReader());
			data.Columns["userid"].ColumnName = "User Name";
			data.Columns["logdesc"].ColumnName = "Log Description";
			data.Columns["logip"].ColumnName = "Logged IP";
			data.Columns["loglevel"].ColumnName = "Log Level";
			data.Columns["logtime"].ColumnName = "Time";
			for (int i = 0; i < data.Rows.Count; i++) {
				data.Rows[i][3] = ConvertToLogLevel(int.Parse(data.Rows[i][3].ToString()));
			}
			return data;
		}

		private string ConvertToLogLevel(int loglevel) {
			switch(loglevel) {
				case 0:
					return "Normal";
				case 1:
					return "Security";
				case 2:
					return "Changed";
				case 3:
					return "Error";
				case 4:
					return "Critical Error";
				default:
					return "-";
			}
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

	}
}