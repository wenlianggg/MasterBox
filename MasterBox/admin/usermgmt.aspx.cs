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
			DataTable dt = GetUsersTable();
			userstable.DataSource = dt;
			userstable.DataBind();
			userstable.HeaderRow.TableSection = TableRowSection.TableHeader;
		}

		protected DataTable GetUsersTable() {
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("ID", typeof(int)));
            dt.Columns.Add(new DataColumn("Username", typeof(string)));
			dt.Columns.Add(new DataColumn("First Name", typeof(string)));
			dt.Columns.Add(new DataColumn("Last Name", typeof(string)));
			dt.Columns.Add(new DataColumn("Email", typeof(string)));
			dt.Columns.Add(new DataColumn("Verified", typeof(bool)));
			dt.Columns.Add(new DataColumn("Registered On", typeof(DateTime)));
			using (DataAccess da = new DataAccess()) {
                foreach (int i in da.SqlGetAllUserIds()) {
                    User usr = Auth.User.GetUser(i);
                    dt.Rows.Add(usr.UserId, usr.UserName, usr.FirstName, usr.LastName, usr.Email, usr.IsVerified, usr.RegStamp);
                }
            }
            return dt;
        }
	}
}