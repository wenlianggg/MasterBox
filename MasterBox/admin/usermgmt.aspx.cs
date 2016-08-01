using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Admin {
	public partial class UserMgmt : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
            if (!IsPostBack) {
                userstable.DataSource = GetUsersTable();
            }
		}

        protected DataTable GetUsersTable() {
            DataTable dt = new DataTable();
            dt.Columns.Add("ID");
            dt.Columns.Add("Username");
            dt.Columns.Add("Registered");
            using (DataAccess da = new DataAccess()) {
                foreach (int i in da.SqlGetAllUserIds()) {
                    User usr = Auth.User.GetUser(i);
                    dt.Rows.Add(usr.UserId, usr.UserName, usr.RegStamp);
                }
            }
            return dt;
        }
	}
}