using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Diagnostics;

namespace MasterBox.Auth {
	public partial class AccessLog : Page {

		int userid;

		protected void Page_Load(object sender, EventArgs e) {
			if (!User.Identity.IsAuthenticated) {
				Response.Redirect("~/Default.aspx");
			} else {
				if (!IsPostBack)
				using (DataAccess da = new DataAccess()) {
					userid = Auth.User.ConvertToId(User.Identity.Name);
					LogsTable.DataSource = da.SqlGetLogs(userid);
					LogsTable.DataBind();
				}
			}
		}
		protected void RefreshTable(object sender, EventArgs e) {
			using (DataAccess da = new DataAccess()) {
				LogsTable.DataSource = da.SqlGetLogs(userid);
				LogsTable.DataBind();
			}
		}

	}

}