using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;

/// Author: Goh Wen Liang (154473G) 

namespace MasterBox.Auth {

	/// <summary>
	/// MasterBox user entity class, interacts with Data Access Layer for
	/// manipulation of user information.
	/// </summary>
	public class User : MembershipUser {

		private static Dictionary<int, User> UserList;
		private DataAccess _da = new DataAccess();
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
		private string _aesKey;
		private string _aesIV;

		public static User GetUser(int userid) {
			User target;
			if (UserList == null) {
				UserList = new Dictionary<int, User>();
			}
			if (UserList.ContainsKey(userid)) {
				UserList.TryGetValue(userid, out target);
				if (target != null)
					return target;
				else
					return GetUser(userid);
			} else {
				target = new User(userid);
				if (!UserList.ContainsKey(userid))
					UserList.Add(userid, target);
				return GetUser(userid);
			}
		}

		public static User GetUser(string username) {
			return GetUser(User.ConvertToId(username));
		}

		public static User CreateUser(string username, string password, string firstname, string lastname,
								DateTime birthdate, string email, bool isVerified) {
			User target;
			if (Exists(username)) {
				throw new UserAlreadyExistsException();
			} else {
				target = new User(username, password, firstname, lastname, birthdate, email, isVerified);
				if (UserList == null) {
					UserList = new Dictionary<int, User>();
				}
				UserList.Add(target.UserId, target);
			}
			return target;
		}

		private User(int userid) {
			// Existing user data retrieval (trusted execution)
			if (_da == null)
				_da = new DataAccess();
			_aesIV = _da.SqlGetUserIV(userid);
			_userid = userid;
			RefreshFields();
		}



		private User(string username, string password, string firstname, string lastname,
						DateTime birthdate, string email, bool isVerified) {
			if (_da == null)
				_da = new DataAccess();
			int _userId = MBProvider.Instance.CreateUser(username, password);
			if (_userId == 0)
				throw new UserNotFoundException();
			// Registration for new sign ups (trusted execution)
			_username = username;
			_fName = firstname;
			_lName = lastname;
			_dob = birthdate;
			_email = email;
			_verified = false;
			_mbrType = 0;
			_mbrStart = DateTime.Now;
			_mbrExpiry = DateTime.Today.AddYears(100);
			_mbrStart = DateTime.Now;
            _aesKey = UserCrypto.GenerateEntropy(32);
			_aesIV = UserCrypto.GenerateEntropy(16);
			UpdateDB();
			RefreshFields();
		}

		public bool RefreshFields() {
			try {
				using (UserCrypto uc = new UserCrypto(_aesIV)) {
					using (SqlDataReader sqldr = _da.SqlGetUser(_userid))
					if (sqldr.Read()) {
						_username = sqldr["username"].ToString();
						_fName = uc.Decrypt((byte[]) sqldr["fNameEnc"]);
						_lName = uc.Decrypt((byte[]) sqldr["lNameEnc"]);
						_email = uc.Decrypt((byte[]) sqldr["emailEnc"]);
						_aesKey = uc.Decrypt((byte[]) sqldr["aesKey"]);
						_dob = (DateTime) sqldr["dob"];
						_verified = (bool) sqldr["verified"];
						_mbrType = (int) sqldr["mbrType"];
						_mbrStart = (DateTime) sqldr["mbrStart"];
						_mbrExpiry = (DateTime) sqldr["mbrExpiry"];
						_regStamp = (DateTime) sqldr["regStamp"];
                        _aesKey = uc.Decrypt((byte[]) sqldr["aesKey"]);
						_aesIV = sqldr["aesIV"].ToString();
					}
				}
				return true;
			} catch {
				return false;
			}
		}

		public int UpdateDB() {
			// Does not set the ID, this method saves database connections
			using (UserCrypto uc = new UserCrypto(_aesIV)) {
				byte[] fNameEnc = uc.Encrypt(_fName);
				byte[] lNameEnc = uc.Encrypt(_lName);
				byte[] emailEnc = uc.Encrypt(_email);
				byte[] aesKeyEnc = uc.Encrypt(_aesKey);
				// Call using Named Parameters to avoid confusion
				return _da.SqlUpdateAllUserValues(userid: _userid, username: _username,
					fNameEnc: fNameEnc, lNameEnc: lNameEnc, dob: _dob, emailEnc: emailEnc,
					verified: _verified, mbrType: _mbrType, mbrStart: _mbrStart, mbrExpiry: _mbrExpiry,
					regStamp: _regStamp, aesKeyEnc: aesKeyEnc, aesIV: _aesIV);
			}
		}


		public static DateTime StringToDateTime(string dateString) {
			IFormatProvider culture = new System.Globalization.CultureInfo("en-SG", true);
			DateTime dateTime  = DateTime.Parse(dateString, culture, System.Globalization.DateTimeStyles.AssumeLocal);
			return dateTime;
		}



		// ACCESSORS AND MUTATORS
		// ======================

		protected internal static int ConvertToId(string username) {
			using (DataAccess da = new DataAccess()) {
				return da.SqlGetUserId(username);
			}
		}

		protected internal static string ConvertToUname(string username) {
			using (DataAccess da = new DataAccess()) {
				SqlDataReader sqldr = da.SqlGetUser(username);
				if (sqldr.Read())
					return sqldr["username"].ToString();
				else
					return "";
			}
		}

		protected internal bool UpdateValue(string fieldName, object fieldValue, SqlDbType sdb, int length) {
			return _da.SqlUpdateUserValue(_userid, fieldName, fieldValue, sdb, length);
		}

		protected internal static bool Exists(int userid) {
			try {
				using (DataAccess da = new DataAccess())
				using (SqlDataReader sqldr = da.SqlGetUser(userid))
					if (sqldr.Read())
						return true;
					else
						return false;
			} catch (UserNotFoundException) {
				return false;
			}
		}

		protected internal static bool Exists(string username) {
			try {
				using (DataAccess da = new DataAccess())
				using (SqlDataReader sqldr = da.SqlGetUser(username))
				if (sqldr.Read())
					return true;
			} catch (UserNotFoundException) {
				return false;
			}
			return false;
		}

		protected internal bool Exist {
			get { return Exists(_userid); }
		}

		public override string UserName {
			get { RefreshFields(); return _username; }
		}

		protected internal DateTime LastLogin {
			get { RefreshFields(); return _lastlogin; }
		}

		protected internal void Login() {
			RefreshFields();
			_lastlogin = DateTime.Now;
			UpdateDB();
		}

		public int UserId {
			get { return _userid; }
		}

		public string FirstName {
			get { RefreshFields(); return _fName; }
			set {
				using (UserCrypto uc = new UserCrypto(_aesIV))
					UpdateValue("fNameEnc", uc.Encrypt(value), SqlDbType.VarBinary, 200);
				RefreshFields();
			}
		}

		public string LastName {
			get { RefreshFields(); return _lName; }
			set {
				using (UserCrypto uc = new UserCrypto(_aesIV))
					UpdateValue("lNameEnc", uc.Encrypt(value), SqlDbType.VarBinary, 200);
				RefreshFields(); }
		}

		public override string Email {
			get { RefreshFields(); return _email; }
			set {
				using (UserCrypto uc = new UserCrypto(_aesIV))
					UpdateValue("emailEnc", uc.Encrypt(value), SqlDbType.VarBinary, 200);
				RefreshFields();
			}
		}

		public DateTime Birthdate {
			get { RefreshFields(); return _dob; }
			set { UpdateValue("dob", value, SqlDbType.Date, 0); RefreshFields(); }
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

		public string AesKey {
			get { RefreshFields(); return _aesKey; }
			set {
				using (UserCrypto uc = new UserCrypto(_aesIV))
					UpdateValue("aesKey", uc.Encrypt(value), SqlDbType.VarBinary, 100);
				RefreshFields(); }
		}

		public string AesIV {
			get { RefreshFields(); return _aesIV; }
			set {
				UpdateValue("aesIV", value, SqlDbType.VarChar, 40);
				using (UserCrypto uc = new UserCrypto(value)) {
					UpdateValue("fNameEnc", uc.Encrypt(_fName), SqlDbType.VarBinary, 200);
					UpdateValue("lNameEnc", uc.Encrypt(_lName), SqlDbType.VarBinary, 200);
					UpdateValue("emailEnc", uc.Encrypt(_email), SqlDbType.VarBinary, 200);
					UpdateValue("aesKey", uc.Encrypt(_aesKey), SqlDbType.VarBinary, 100);

				}
				RefreshFields(); }
		}

        public bool IsAdmin {
            get {
                if (_mbrType == -1) return true;
                else return false;
            }
        }
	}

}