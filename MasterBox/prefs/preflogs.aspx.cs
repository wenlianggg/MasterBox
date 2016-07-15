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

        }
        protected void RefreshAuthTable(object sender, EventArgs e) {
            LogTypeName.Text = "Access Logs";
            LogTable.DataSource = AuthLogger.Instance.GetUserLogs(Auth.User.ConvertToId(User.Identity.Name));
            LogTable.DataBind();
        }

        protected void RefreshFilesTable(object sender, EventArgs e) {
            LogTypeName.Text = "File Logs";
            LogTable.DataSource = FileLogger.Instance.GetUserLogs(Auth.User.ConvertToId(User.Identity.Name));
            LogTable.DataBind();
        }

        protected void RefreshTransactTable(object sender, EventArgs e) {
            LogTypeName.Text = "Transaction Logs";
            LogTable.DataSource = TransactLogger.Instance.GetUserLogs(Auth.User.ConvertToId(User.Identity.Name));
            LogTable.DataBind();
        }
    }
}