using MasterBox.Auth;
using MasterBox.mbox;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Prefs {
    public partial class FileSetting_Profile : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
			User CurrentUser = Auth.User.GetUser(Context.User.Identity.Name);
			username.Text = CurrentUser.UserName;
			email.Text = CurrentUser.Email;
        }
    }
}