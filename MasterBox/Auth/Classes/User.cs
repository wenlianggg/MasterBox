using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Security;

/// Author: Goh Wen Liang (154473G) 

namespace MasterBox.Auth {

	/// <summary>
	/// MasterBox user entity class, interacts with Data Access Layer for
	/// manipulation of user information.
	/// </summary>
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
		private string _aesKey;
		private string _aesIV;
		private bool _isAdmin;

		internal static User GetUser(int userid) {
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

		internal static User GetUser(string username, [CallerMemberName]string memberName = "") {
			return GetUser(ConvertToId(username, memberName));
		}

		internal static User CreateUser(string username, string password, string firstname, string lastname, string email, bool isVerified) {
			User target;
			if (Exists(username)) {
				throw new UserAlreadyExistsException();
			} else {
				target = new User(username, password, firstname, lastname, email, isVerified);
				if (UserList == null) {
					UserList = new Dictionary<int, User>();
				}
				UserList.Add(target.UserId, target);
			}
			return target;
		}

		private User(int userid) {
            // Existing user data retrieval (trusted execution)
            using (DataAccess da = new DataAccess()) {
                _aesIV = da.SqlGetUserIV(userid);
                _userid = userid;
                RefreshFields();
            }
		}



		private User(string username, string password, string firstname, string lastname, string email, bool isVerified) {
			int _userId = MBProvider.Instance.CreateUser(username, password);
			if (_userId == 0)
				throw new UserNotFoundException();
			// Registration for new sign ups (trusted execution)
			_username = username;
			_fName = firstname;
			_lName = lastname;
			_email = email;
			_verified = false;
			_mbrType = 1;
			_mbrStart = DateTime.Now;
			_mbrExpiry = DateTime.Today.AddYears(100);
            _aesKey = UserCrypto.GenerateEntropy(32);
			_aesIV = UserCrypto.GenerateEntropy(16);
			_isAdmin = false;
			UpdateDB();
			RefreshFields();
		}

		internal bool RefreshFields() {
			try {
                using (DataAccess da = new DataAccess())
                using (UserCrypto uc = new UserCrypto(_aesIV)) {
                    using (SqlDataReader sqldr = da.SqlGetUser(_userid))
                        if (sqldr.Read()) {
                            _username = sqldr["username"] as string;
                            if (sqldr["fNameEnc"] != DBNull.Value)
                                _fName = uc.Decrypt((byte[])sqldr["fNameEnc"]);
                            if (sqldr["lNameEnc"] != DBNull.Value)
                                _lName = uc.Decrypt((byte[])sqldr["lNameEnc"]);
                            if (sqldr["emailEnc"] != DBNull.Value)
                                _email = uc.Decrypt((byte[])sqldr["emailEnc"]);
                            _dob = (DateTime)sqldr["dob"];
                            _verified = (bool)sqldr["verified"];
                            _mbrType = (int)sqldr["mbrType"];
                            _mbrStart = (DateTime)sqldr["mbrStart"];
                            _mbrExpiry = (DateTime)sqldr["mbrExpiry"];
                            _regStamp = (DateTime)sqldr["regStamp"];
                            if (sqldr["aesKey"] != DBNull.Value)
                                _aesKey = uc.Decrypt((byte[])sqldr["aesKey"]);
                            _aesIV = sqldr["aesIV"].ToString();
							_isAdmin = (bool) sqldr["isAdmin"];
                        }
                }
				return true;
			} catch {
				return false;
			}
		}

		internal int UpdateDB() {
            // Does not set the ID, this method saves database connections
            using (DataAccess da = new DataAccess())
            using (UserCrypto uc = new UserCrypto(_aesIV)) {
                byte[] fNameEnc = uc.Encrypt(_fName);
                byte[] lNameEnc = uc.Encrypt(_lName);
                byte[] emailEnc = uc.Encrypt(_email);
                byte[] aesKeyEnc = uc.Encrypt(_aesKey);
                // Call using Named Parameters to avoid confusion
                return da.SqlUpdateAllUserValues(userid: _userid, username: _username,
                    fNameEnc: fNameEnc, lNameEnc: lNameEnc, dob: _dob, emailEnc: emailEnc,
                    verified: _verified, mbrType: _mbrType, mbrStart: _mbrStart, mbrExpiry: _mbrExpiry,
                    regStamp: _regStamp, aesKeyEnc: aesKeyEnc, aesIV: _aesIV, isAdmin: _isAdmin);
            }
		}


		internal static DateTime StringToDateTime(string dateString) {
			IFormatProvider culture = new System.Globalization.CultureInfo("en-SG", true);
			DateTime dateTime  = DateTime.Parse(dateString, culture, System.Globalization.DateTimeStyles.AssumeLocal);
			return dateTime;
		}



		// ACCESSORS AND MUTATORS
		// ======================

		protected internal static int ConvertToId(string username, [CallerMemberName]string memberName = "") {
			using (DataAccess da = new DataAccess("ConvertToId from " + memberName)) {
				return da.SqlGetUserId(username);
			}
		}

		protected internal bool UpdateValue(string fieldName, object fieldValue, SqlDbType sdb, int length) {
            using (DataAccess da = new DataAccess())
                return da.SqlUpdateUserValue(_userid, fieldName, fieldValue, sdb, length);
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

	
		protected internal void Login() {
			RefreshFields();
			_lastlogin = DateTime.Now;
			UpdateDB();
		}

		protected internal int UserId {
			get { return _userid; }
		}

		protected internal string FirstName {
			get { RefreshFields(); return _fName; }
			set {
				using (UserCrypto uc = new UserCrypto(_aesIV))
					UpdateValue("fNameEnc", uc.Encrypt(value), SqlDbType.VarBinary, 200);
				RefreshFields();
			}
		}

		protected internal string LastName {
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

		protected internal DateTime LastLogin {
			get { RefreshFields(); return _lastlogin; }
			set {
				UpdateValue("lastlogin", value, SqlDbType.DateTime, 7);
				RefreshFields();
			}
		}

		protected internal DateTime Birthdate {
			get { RefreshFields(); return _dob; }
			set { UpdateValue("dob", value, SqlDbType.Date, 0); RefreshFields(); }
		}

		protected internal bool IsVerified {
			get { RefreshFields(); return _verified; }
			set { UpdateValue("verified", value, SqlDbType.Bit, 0); RefreshFields(); }
		}

		protected internal int MbrType {
			get { RefreshFields(); return _mbrType; }
			set { UpdateValue("mbrType", value, SqlDbType.Int, 0); RefreshFields(); }
		}

		protected internal DateTime MbrStart {
			get { RefreshFields(); return _mbrStart; }
			set { UpdateValue("mbrStart", value, SqlDbType.DateTime2, 7); RefreshFields(); }
		}

		protected internal DateTime MbrExpiry {
			get { RefreshFields(); return _mbrExpiry; }
			set { UpdateValue("mbrExpiry", value, SqlDbType.DateTime2, 7); RefreshFields(); }
		}

		protected internal DateTime RegStamp {
			get { RefreshFields(); return _regStamp; }
		}

		protected internal string AesKey {
			get { RefreshFields(); return _aesKey; }
			set {
				using (UserCrypto uc = new UserCrypto(_aesIV))
					UpdateValue("aesKey", uc.Encrypt(value), SqlDbType.VarBinary, 100);
				RefreshFields(); }
		}

		protected internal string AesIV {
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

		protected internal bool IsAdmin {
			get {
				RefreshFields();
				return _isAdmin;
			}
			set {
				UpdateValue("isAdmin", value, SqlDbType.Bit, 0);
				RefreshFields();
			}
		}


	}

}