using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MasterBox.Auth {
	public class User {
		public static Dictionary<string, User> users = new Dictionary<string, User>();

		private MBProvider _mbp;
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



		public User(string username, string ptPass) {
			_mbp = MBProvider.Instance;
			if (_mbp.ValidateUser(username, ptPass)) {

				SqlDataReader sqldr = _mbp.SQLGetUserByUN(username);
				_userid = (Int64) sqldr["userid"];
				_userName = sqldr["username"].ToString();
				_userName = sqldr["userName"].ToString();
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
		
		public static User createNewUser() {
			return new User("shell", "shell");
		}

		public long UserId {
			get { return _userid; }
		}

		public string UserName {
			get { return _userName; }
		}

		public string FirstName {
			get { return _firstName; }
			set {
				updateValue("fName", value, SqlDbType.VarChar, 100);
				_firstName = getValue("fName").ToString();
			}
		}

		public string LastName {
			get { return _lastName; }
			set {
				updateValue("lName", value, SqlDbType.VarChar, 100);
				_lastName = getValue("lName").ToString();
			}
		}

		public DateTime BirthDate {
			get { return _birthDate; }
			set {
				updateValue("birthDate", value, SqlDbType.Date, 0);
				_birthDate = (DateTime)getValue("birthDate");
			}
		}

		public string EmailAddress {
			get { return _email; }
			set {
				updateValue("email", value, SqlDbType.VarChar, 100);
				_email = getValue("email").ToString();
			}
		}

		public bool IsVerified {
			get { return _isVerified; }
			set {
				updateValue("verified", value, SqlDbType.Bit, 0);
				_isVerified = (bool)getValue("verified");
			}
		}

		public int MemberType {
			get { return _memberType; }
			set {
				updateValue("mbrType", value, SqlDbType.Int, 0);
				_memberType = (int)getValue("mbrType");
			}
		}

		public DateTime MbrStartDate {
			get { return _mbrStartDate; }
			set {
				updateValue("mbrStartDate", value, SqlDbType.DateTime2, 7);
				_mbrStartDate = (DateTime)getValue("mbrStartDate");
			}
		}

		public DateTime MbrExpireDate {
			get { return _mbrExpireDate; }
			set {
				updateValue("mbrExpireDate", value, SqlDbType.DateTime2, 7);
				_mbrExpireDate = (DateTime)getValue("mbrExpireDate");
			}
		}

		public DateTime RegDateTime {
			get { return _regDateTime; }
			set {
				updateValue("regTime", value, SqlDbType.DateTime2, 7);
				_regDateTime = (DateTime)getValue("regTime");
			}
		}

		private bool updateValue(string fieldName, object fieldValue, SqlDbType sdb, int length) {
			SqlCommand cmd = new SqlCommand(
				"UPDATE mb_users SET " + fieldName + " = @fieldValue WHERE userid = @uid",
				SQLGetMBoxConnection()
				);

			cmd.Parameters.Add(new SqlParameter("@fieldValue", sdb, length));
			cmd.Parameters.Add(new SqlParameter("@uid", SqlDbType.BigInt, 0));
			cmd.Prepare();

			cmd.Parameters["@fieldValue"].Value = fieldValue;
			cmd.Parameters["@uid"].Value = (Int64) _userid;
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

		private static SqlConnection SQLGetMBoxConnection() {
			SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
			sqlConnection.Open();
			return sqlConnection;
		}

		public DateTime stringToDateTime(string dateString) {
			IFormatProvider culture = new System.Globalization.CultureInfo("en-SG", true);
			DateTime dateTime  = DateTime.Parse(dateString, culture, System.Globalization.DateTimeStyles.AssumeLocal);
			return dateTime;
		}

	}
}