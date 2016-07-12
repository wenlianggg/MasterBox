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

        protected void Page_Load(object sender, EventArgs e) {
            if (!User.Identity.IsAuthenticated) {
                Response.Redirect("~/Default.aspx");
            }
        }
        protected void RefreshAuthTable(object sender, EventArgs e) {
            FileLogs.Visible = false;
            AuthLogs.Visible = true;
            AuthLogsTable.DataSource = AuthLogger.Instance.GetUserLogs(Auth.User.ConvertToId(User.Identity.Name));
            AuthLogsTable.DataBind();
        }

        protected void RefreshFilesTable(object sender, EventArgs e) {
            AuthLogs.Visible = false;
            FileLogs.Visible = true;
            FileLogsTable.DataSource = FileLogger.Instance.GetUserLogs(Auth.User.ConvertToId(User.Identity.Name));
            FileLogsTable.DataBind();
        }
    }
}