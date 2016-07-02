using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

namespace MasterBox.Auth {
	public class User : MembershipUser {

		private static Dictionary<int, User> UserList;

		private int _userid; // NOT NULLABLE IN DB
		private string _username; // NOT NULLABLE IN DB
		private DateTime _lastlogin;
		private string _fName;
		private string _lName;
		private string _email; // NOT NULLABLE IN DB
		private bool _verified;
		private DateTime _dob;
		private int _mbrType;
		private DateTime _mbrStart;
		private DateTime _mbrExpiry;
		private DateTime _regStamp;
		private string _aesIV;

		public static User GetUser(int userid) {
			if (UserList == null) {
				UserList = new Dictionary<int, User>();
			}
			User target;
			if (UserList.TryGetValue(userid, out target)) {
				return target;
			} else {
				target = new User(userid);
				UserList.Add(target.UserId, target);
				return GetUser(userid);
			}
		}

		public static User GetUser(string username) {
			return GetUser(User.ConvertToId(username));
		}

		public static User CreateUser(string username, string password, string firstname, string lastname,
								DateTime birthdate, string email, bool isVerified) {
			User target;
			if (!Exists(username)) {
				target = new User(username, password, firstname, lastname, birthdate, email, isVerified);
				UserList.Add(target.UserId, target);
			} else {
				throw new UserAlreadyExistsException();
			}
			return target;
		}

		private User(int userid) {
			// Existing user data retrieval
			SqlDataReader sqldr = MBProvider.Instance.SqlGetUser(userid);
			if (sqldr.Read()) {
				_userid = (int)sqldr["userid"];
				_username = sqldr["username"].ToString();
				_fName = sqldr["fName"].ToString();
				_lName = sqldr["lName"].ToString();
				_dob = (DateTime)sqldr["dob"];
				_email = sqldr["email"].ToString();
				_verified = (bool)sqldr["verified"];
				_mbrType = (int)sqldr["mbrType"];
				_mbrStart = (DateTime)sqldr["mbrStart"];
				_mbrExpiry = (DateTime)sqldr["mbrExpiry"];
				_regStamp = (DateTime)sqldr["regStamp"];
				_aesIV = sqldr["aesIV"].ToString();
			}
		}



		private User(string username, string password, string firstname, string lastname,
						DateTime birthdate, string email, bool isVerified) {
			int _userId = MBProvider.Instance.CreateUser(username, password);
			if (_userId == 0)
				throw new UserNotFoundException();
			// Registration for new sign ups
			_username = username;
			_fName = firstname;
			_lName = lastname;
			_dob = birthdate;
			_email = email;
			_verified = isVerified;
			_mbrType = 0;
			_mbrStart = DateTime.Now;
			_mbrExpiry = DateTime.Today.AddYears(100);
			_mbrStart = DateTime.Now;
			_aesIV = UserCrypto.GenerateEntropy(16);
			UpdateDB();
		}

		public bool RefreshFields() {
			try {
				SqlDataReader sqldr = MBProvider.Instance.SqlGetUser(_userid);
				_username = sqldr["username"].ToString();
				_fName = sqldr["fName"].ToString();
				_lName = sqldr["lName"].ToString();
				_dob = (DateTime)sqldr["dob"];
				_email = sqldr["email"].ToString();
				_verified = (bool)sqldr["verified"];
				_mbrType = (int)sqldr["mbrType"];
				_mbrStart = (DateTime)sqldr["mbrStart"];
				_mbrExpiry = (DateTime)sqldr["mbrExpiry"];
				_regStamp = (DateTime)sqldr["regStamp"];
				_aesIV = sqldr["aesIV"].ToString();
				return true;
			} catch {
				return false;
			}
		}

		public int UpdateDB() {
			// Does not set the ID, this method saves database connections
			SqlCommand cmd;
			if (Exist) {
				cmd = new SqlCommand(
					"UPDATE mb_users SET " +
					"fName = @fName," +
					"lName = @lName," +
					"dob = @dob," +
					"email = @email," +
					"verified = @verified," +
					"mbrType = @mbrType," +
					"mbrStart = @mbrStart," +
					"mbrExpiry = @mbrExpiry," +
					"regStamp = @regStamp " +
					"aesIV = @aesIV " +
					"WHERE userid = @userid;",
					SqlGetConn()
				);

				cmd.Parameters.Add(new SqlParameter("@fName", SqlDbType.VarChar, 100));
				cmd.Parameters.Add(new SqlParameter("@lName", SqlDbType.VarChar, 100));
				cmd.Parameters.Add(new SqlParameter("@dob", SqlDbType.Date, 0));
				cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 100));
				cmd.Parameters.Add(new SqlParameter("@verified", SqlDbType.Bit, 0));
				cmd.Parameters.Add(new SqlParameter("@mbrType", SqlDbType.Int, 0));
				cmd.Parameters.Add(new SqlParameter("@mbrStart", SqlDbType.DateTime2, 7));
				cmd.Parameters.Add(new SqlParameter("@mbrExpiry", SqlDbType.DateTime2, 7));
				cmd.Parameters.Add(new SqlParameter("@regStamp", SqlDbType.DateTime2, 7));
				cmd.Parameters.Add(new SqlParameter("@aesIV", SqlDbType.VarChar, 20));
				cmd.Parameters.Add(new SqlParameter("@uid", SqlDbType.Int, 8));
				cmd.Prepare();

				cmd.Parameters["@fName"].Value = _fName;
				cmd.Parameters["@lName"].Value = _lName;
				cmd.Parameters["@dob"].Value = _dob;
				cmd.Parameters["@email"].Value = _email;
				cmd.Parameters["@verified"].Value = _verified;
				cmd.Parameters["@mbrType"].Value = _mbrType;
				cmd.Parameters["@mbrStart"].Value = _mbrStart;
				cmd.Parameters["@mbrExpiry"].Value = _mbrExpiry;
				cmd.Parameters["@regStamp"].Value = _regStamp;
				cmd.Parameters["@aesIV"].Value = _aesIV;
				cmd.Parameters["@uid"].Value = _userid;
				return cmd.ExecuteNonQuery();
			} else {
				return 0;
			}
		}


		public static DateTime StringToDateTime(string dateString) {
			IFormatProvider culture = new System.Globalization.CultureInfo("en-SG", true);
			DateTime dateTime  = DateTime.Parse(dateString, culture, System.Globalization.DateTimeStyles.AssumeLocal);
			return dateTime;
		}



		// ACCESSORS AND MUTATORS
		// ======================

		public static int ConvertToId(string username) {
			SqlCommand cmd = new SqlCommand(
				"SELECT DISTINCT userid, username FROM mb_users WHERE username = @un",
				SqlGetConn());
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

		private bool UpdateValue(string fieldName, object fieldValue, SqlDbType sdb, int length) {
			SqlCommand cmd = new SqlCommand(
				"UPDATE mb_users SET " + fieldName + " = @fieldValue WHERE userid = @uid",
				SqlGetConn()
				);
			cmd.Parameters.Add(new SqlParameter("@fieldValue", sdb, length));
			cmd.Parameters.Add(new SqlParameter("@uid", SqlDbType.Int, 0));
			cmd.Prepare();

			cmd.Parameters["@fieldValue"].Value = fieldValue;
			cmd.Parameters["@uid"].Value = (Int64)_userid;
			if (cmd.ExecuteNonQuery() == 1) {
				return true;
			} else {
				throw new DatabaseUpdateFailureException("Updating value " + fieldValue + " failed.");
			}
		}

		private object GetValue(string fieldName) {
			SqlCommand cmd = new SqlCommand(
				"SELECT " + fieldName + " FROM mb_users WHERE userid = @uid",
				SqlGetConn()
				);

			cmd.Parameters.Add(new SqlParameter("@uid", SqlDbType.Int, 0));
			cmd.Prepare();
			cmd.Parameters["@uid"].Value = (Int64)_userid;
			return cmd.ExecuteReader()[fieldName];
		}

		public static bool Exists(int userid) {
			try {
				SqlDataReader sqldr = MBProvider.Instance.SqlGetUser(userid);
				if (sqldr.Read()) {
					return true;
				}
			} catch (UserNotFoundException) {
				return false;
			}
			return false;
		}

		public static bool Exists(string username) {
			try {
				SqlDataReader sqldr = MBProvider.Instance.SqlGetUser(username);
				if (sqldr.Read()) {
					return true;
				}
			} catch (UserNotFoundException) {
				return false;
			}
			return false;
		}

		public bool Exist {
			get { return Exists(_userid); }
		}

		public int UserId {
			get { return _userid; }
		}

		public override string UserName {
			get { RefreshFields(); return _username; }
		}

		public DateTime LastLogin {
			get { RefreshFields(); return _lastlogin; }
		}

		public void Login() {
			RefreshFields();
			_lastlogin = DateTime.Now;
			UpdateDB();
		}

		public string FirstName {
			get { RefreshFields(); return _fName; }
			set { UpdateValue("fName", value, SqlDbType.VarChar, 100); RefreshFields(); }
		}

		public string LastName {
			get { RefreshFields(); return _lName; }
			set { UpdateValue("lName", value, SqlDbType.VarChar, 100); RefreshFields(); }
		}

		public DateTime Birthdate {
			get { RefreshFields(); return _dob; }
			set { UpdateValue("dob", value, SqlDbType.Date, 0); RefreshFields(); }
		}

		public override string Email {
			get { RefreshFields(); return _email; }
			set { UpdateValue("email", value, SqlDbType.VarChar, 100); RefreshFields(); }
		}

		public bool IsVerified {
			get { RefreshFields(); return _verified; }
			set { UpdateValue("verified", value, SqlDbType.Bit, 0); RefreshFields(); }
		}

		public int MbrType {
			get { RefreshFields(); return _mbrType; }
			set { UpdateValue("mbrType", value, SqlDbType.Int, 0); RefreshFields(); }
		}

		public DateTime MbrStart {
			get { RefreshFields(); return _mbrStart; }
			set { UpdateValue("mbrStart", value, SqlDbType.DateTime2, 7); RefreshFields(); }
		}

		public DateTime MbrExpiry {
			get { RefreshFields(); return _mbrExpiry; }
			set { UpdateValue("mbrExpiry", value, SqlDbType.DateTime2, 7); RefreshFields(); }
		}

		public DateTime RegStamp {
			get { RefreshFields(); return _regStamp; }
			set { UpdateValue("regStamp", value, SqlDbType.DateTime2, 7); RefreshFields(); }
		}

		public string AesIv {
			get { RefreshFields(); return _aesIV; }
			set { UpdateValue("aesIV", value, SqlDbType.VarChar, 128); RefreshFields(); }
		}

		private static SqlConnection SqlGetConn() {
			SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
			sqlConnection.Open();
			return sqlConnection;
		}

	}

}