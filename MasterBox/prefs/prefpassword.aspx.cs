using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Prefs {
	public partial class ChangePw : System.Web.UI.Page {

		protected void Page_Load(object sender, EventArgs e) {
			if (Context.User.Identity.IsAuthenticated) {
				LoggedInUsername.Text = Context.User.Identity.Name;
			} else {
				Response.Redirect("SignIn.aspx");
			}
		}

		protected void ChangePassClick(object sender, EventArgs e) {
			if (NewUserPass.Text.Equals(NewUserPassCfm.Text)) {
				try {
					if (MBProvider.Instance.ChangePassword(Context.User.Identity.Name, OldUserPass.Text, NewUserPass.Text)) {
						Msg.ForeColor = System.Drawing.Color.LimeGreen;
						Msg.Text = "Successfully changed password";
					} else {
						Msg.ForeColor = System.Drawing.Color.Red;
						Msg.Text = "Password unchanged due to validation error.";
					}
				} catch (UserNotFoundException) {
					Msg.ForeColor = System.Drawing.Color.Red;
					Msg.Text = "An unexpected error has occured: USER_NOT_FOUND";
				}
			} else {
				Msg.ForeColor = System.Drawing.Color.Red;
				Msg.Text = "Password fields do not match";
			}
		}
	}
}