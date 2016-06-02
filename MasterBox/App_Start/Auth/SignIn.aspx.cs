using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Diagnostics;

namespace MasterBox {
	public partial class SignIn : System.Web.UI.Page {

		protected void Page_Load(object sender, EventArgs e) {
			if (User.Identity.IsAuthenticated) {
				Response.Redirect("~/Default.aspx");
			}
		}
		protected void logonClick(object sender, EventArgs e) {
			if ((UserName.Text == "Dominic") && (UserPass.Text == "123") ||
				(UserName.Text == "James") && (UserPass.Text == "123") ||
				(UserName.Text == "Roy") && (UserPass.Text == "123") ||
				(UserName.Text == "Wilfred") && (UserPass.Text == "123") ||
				(UserName.Text == "Wuggle") && (UserPass.Text == "123")) {
				FormsAuthentication.RedirectFromLoginPage(UserName.Text, Persist.Checked);
			} else {
				Msg.Text = "Invalid credentials. Please try again.";
			}
		}

		protected void registrationStart(object sender, EventArgs e) {
			Debug.WriteLine(ResolveUrl("~/auth/registration") + "?username=" + UserName.Text);
			Response.Redirect("~/auth/registration.aspx" + "?username=" + UserName.Text);
		}
	}

}