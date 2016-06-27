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
				_firstName = value;
			}
		}

		public string LastName {
			get { return _lastName; }
			set {
				_lastName = value;
			}
		}

		public DateTime BirthDate {
			get { return _birthDate; }
			set {
				_birthDate = value;
			}
		}

		public string EmailAddress {
			get { return _email; }
			set {
				_email = value;
			}
		}

		public bool IsVerified {
			get { return _isVerified; }
			set {
				_isVerified = value;
			}
		}

		public int MemberType {
			get { return _memberType; }
			set {
				_memberType = value;
			}
		}

		public DateTime MbrStartDate {
			get { return _mbrStartDate; }
			set {
				_mbrStartDate = value;
			}
		}

		public DateTime MbrExpireDate {
			get { return _mbrExpireDate; }
			set {
				_mbrExpireDate = value;
			}
		}

		public DateTime RegDateTime {
			get { return _regDateTime; }
			set {
				_regDateTime = value;
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