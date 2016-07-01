using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

namespace MasterBox.Auth {
	public class User : MembershipUser {

		private long _userid;
		private string _userName;
		private string _firstName;
		private string _lastName;
		private DateTime _birthDate;
		private string _email;
		private bool _isVerified;
		private int _memberType;
		private DateTime _mbrStartDate;
		private DateTime _mbrExpireDate;
		private DateTime _regDateTime;

		public User(string username) : this(MBProvider.Instance.UsernameToId(username)) {
		}

		public User(long userid) {
			SqlDataReader sqldr = MBProvider.Instance.SQLGetUser(userid);
			if (sqldr.Read()) {
				_userid = (Int64)sqldr["userid"];
				_userName = sqldr["username"].ToString();
				_firstName = sqldr["fName"].ToString();
				_lastName = sqldr["lName"].ToString();
				_birthDate = (DateTime)sqldr["birthdate"];
				_email = sqldr["email"].ToString();
				_isVerified = (bool)sqldr["verified"];
				_memberType = (int)sqldr["mbrType"];
				_mbrStartDate = (DateTime)sqldr["mbrStartDate"];
				_mbrExpireDate = (DateTime)sqldr["mbrExpireDate"];
				_regDateTime = (DateTime)sqldr["regTime"];
			}
		}

		public User(string username, string firstname, string lastname,
			DateTime birthdate, string email, bool isVerified) {
			// New user;
			_userName = username;
			_firstName = firstname;
			_lastName = lastname;
			_birthDate = birthdate;
			_email = email;
			_isVerified = isVerified;
			_memberType = 0;
			_mbrStartDate = DateTime.Now;
			_mbrExpireDate = DateTime.Today.AddYears(100);
			_regDateTime = DateTime.Now;
			DbUpdateAllFields();
		}

		public bool RefreshAllFields() {
			try {
				SqlDataReader sqldr = MBProvider.Instance.SQLGetUser(_userid);
				_userName = sqldr["username"].ToString();
				_firstName = sqldr["fName"].ToString();
				_lastName = sqldr["lName"].ToString();
				_birthDate = (DateTime)sqldr["birthdate"];
				_email = sqldr["email"].ToString();
				_isVerified = (bool)sqldr["verified"];
				_memberType = (int)sqldr["mbrType"];
				_mbrStartDate = (DateTime)sqldr["mbrStartDate"];
				_mbrExpireDate = (DateTime)sqldr["mbrExpireDate"];
				_regDateTime = (DateTime)sqldr["regTime"];
				return true;
			} catch {
				return false;
			}
		}

		public int DbUpdateAllFields() {
			// Does not set the ID, this method saves database connections
			SqlCommand cmd;
			if (Exist)
				cmd = new SqlCommand(
					"UPDATE mb_users SET " +
					"fName = @fName," +
					"lName = @lName," +
					"birthDate = @birthDate," +
					"email = @email," +
					"verified = @verified," +
					"mbrType = @mbrType," +
					"mbrStartDate = @mbrStartDate," +
					"mbrExpireDate = @mbrExpireDate," +
					"regTime = @regTime " +
					"WHERE userid = @uid;",
					SQLGetMBoxConnection()
				);
			else
				cmd = new SqlCommand(
					"INSERT INTO mb_users " +
					"(fName, lName, birthDate, email, verified, mbrType, mbrStartDate, mbrExpireDate, regTime) " +
					"VALUES " +
					"(@fName, @lName, @birthDate, @email, @verified, @mbrType, @mbrStartDate, @mbrExpireDate, @regTime)",
					SQLGetMBoxConnection()
				);
			cmd.Parameters.Add(new SqlParameter("@fName", SqlDbType.VarChar, 100));
			cmd.Parameters.Add(new SqlParameter("@lName", SqlDbType.VarChar, 100));
			cmd.Parameters.Add(new SqlParameter("@birthDate", SqlDbType.Date, 0));
			cmd.Parameters.Add(new SqlParameter("@email", SqlDbType.VarChar, 100));
			cmd.Parameters.Add(new SqlParameter("@verified", SqlDbType.Bit, 0));
			cmd.Parameters.Add(new SqlParameter("@mbrType", SqlDbType.BigInt, 0));
			cmd.Parameters.Add(new SqlParameter("@mbrStartDate", SqlDbType.DateTime2, 7));
			cmd.Parameters.Add(new SqlParameter("@mbrExpireDate", SqlDbType.DateTime2, 7));
			cmd.Parameters.Add(new SqlParameter("@regTime", SqlDbType.DateTime2, 7));
			if (Exist)
				cmd.Parameters.Add(new SqlParameter("@uid", SqlDbType.BigInt, 8));
			cmd.Prepare();

			cmd.Parameters["@fName"].Value = _firstName;
			cmd.Parameters["@lName"].Value = _lastName;
			cmd.Parameters["@birthDate"].Value = _birthDate;
			cmd.Parameters["@email"].Value = _email;
			cmd.Parameters["@verified"].Value = _isVerified;
			cmd.Parameters["@mbrType"].Value = _memberType;
			cmd.Parameters["@mbrStartDate"].Value = _mbrStartDate;
			cmd.Parameters["@mbrExpireDate"].Value = _mbrExpireDate;
			cmd.Parameters["@regTime"].Value = _regDateTime;
			if (Exist)
				cmd.Parameters["@uid"].Value = _userid;
			return cmd.ExecuteNonQuery();
		}


		public static DateTime StringToDateTime(string dateString) {
			IFormatProvider culture = new System.Globalization.CultureInfo("en-SG", true);
			DateTime dateTime  = DateTime.Parse(dateString, culture, System.Globalization.DateTimeStyles.AssumeLocal);
			return dateTime;
		}



		// ACCESSORS AND MUTATORS
		// ======================

		private bool updateValue(string fieldName, object fieldValue, SqlDbType sdb, int length) {
			SqlCommand cmd = new SqlCommand(
				"UPDATE mb_users SET " + fieldName + " = @fieldValue WHERE userid = @uid",
				SQLGetMBoxConnection()
				);
			cmd.Parameters.Add(new SqlParameter("@fieldValue", sdb, length));
			cmd.Parameters.Add(new SqlParameter("@uid", SqlDbType.BigInt, 0));
			cmd.Prepare();

			cmd.Parameters["@fieldValue"].Value = fieldValue;
			cmd.Parameters["@uid"].Value = (Int64)_userid;
			if (cmd.ExecuteNonQuery() == 1) {
				return true;
			} else {
				throw new DatabaseUpdateFailureException("Updating value " + fieldValue + " failed.");
			}
		}

		private object getValue(string fieldName) {
			SqlCommand cmd = new SqlCommand(
				"SELECT " + fieldName + " FROM mb_users WHERE userid = @uid",
				SQLGetMBoxConnection()
				);

			cmd.Parameters.Add(new SqlParameter("@uid", SqlDbType.BigInt, 0));
			cmd.Prepare();
			cmd.Parameters["@uid"].Value = (Int64)_userid;
			return cmd.ExecuteReader()[fieldName];
		}

		public static bool UserExists(string username) {
			try {
				SqlDataReader sqldr = MBProvider.Instance.SQLGetUser(username);
				if (sqldr.Read()) {
					return true;
				}
			}
			catch (UserNotFoundException) {
				return false;
			}
			return false;
		}

		public bool Exist {
			get {
				return UserExists(UserName);
			}
		}

		public long UserId {
			get {
				return _userid;
			}
		}

		public override string UserName {
			get {
				RefreshAllFields();
				return _userName;
			}
		}

		public string FirstName {
			get {
				RefreshAllFields();
				return _firstName;
			}
			set {
				updateValue("fName", value, SqlDbType.VarChar, 100);
				RefreshAllFields();
			}
		}

		public string LastName {
			get {
				RefreshAllFields();
				return _lastName;
			}
			set {
				updateValue("lName", value, SqlDbType.VarChar, 100);
				RefreshAllFields();
			}
		}

		public DateTime BirthDate {
			get {
				RefreshAllFields();
				return _birthDate;
			}
			set {
				updateValue("birthDate", value, SqlDbType.Date, 0);
				RefreshAllFields();
			}
		}

		public string EmailAddress {
			get {
				RefreshAllFields();
				return _email;
			}
			set {
				updateValue("email", value, SqlDbType.VarChar, 100);
				RefreshAllFields();
			}
		}

		public bool IsVerified {
			get {
				RefreshAllFields();
				return _isVerified;
			}
			set {
				updateValue("verified", value, SqlDbType.Bit, 0);
				RefreshAllFields();
			}
		}

		public int MemberType {
			get {
				RefreshAllFields();
				return _memberType;
			}
			set {
				updateValue("mbrType", value, SqlDbType.BigInt, 0);
				RefreshAllFields();
			}
		}

		public DateTime MbrStartDate {
			get {
				RefreshAllFields();
				return _mbrStartDate;
			}
			set {
				updateValue("mbrStartDate", value, SqlDbType.DateTime2, 7);
				RefreshAllFields();
			}
		}

		public DateTime MbrExpireDate {
			get {
				RefreshAllFields();
				return _mbrExpireDate;
			}
			set {
				updateValue("mbrExpireDate", value, SqlDbType.DateTime2, 7);
				RefreshAllFields();
			}
		}

		public DateTime RegDateTime {
			get {
				RefreshAllFields();
				return _regDateTime;
			}
			set {
				updateValue("regTime", value, SqlDbType.DateTime2, 7);
				RefreshAllFields();
			}
		}

		private static SqlConnection SQLGetMBoxConnection() {
			SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
			sqlConnection.Open();
			return sqlConnection;
		}

	}

}