using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox {
	public partial class ChangePw : System.Web.UI.Page {
		private static MBProvider mbprovider = new MBProvider();

		protected void Page_Load(object sender, EventArgs e) {
			if (Context.User.Identity.IsAuthenticated) {
				LoggedInUsername.Text = Context.User.Identity.Name;
			} else {
				Response.Redirect("SignIn.aspx");
			}
		}

		protected void ChangePassClick(object sender, EventArgs e) {
			// TODO: Check if the passwords match
			if (mbprovider.ChangePassword(Context.User.Identity.Name, OldUserPass.Text, NewUserPass.Text)) {
				Msg.Text = "Successfully changed password";
			} else {
				Msg.Text = "Password unchanged due to validation error.";
			}
		}
	}
}