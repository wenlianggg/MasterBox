using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Diagnostics;
using MasterBox.Auth;

namespace MasterBox {
	public partial class SignIn : Page {
		private static MBProvider mbprovider = new MBProvider();

		protected void Page_Load(object sender, EventArgs e) {
			if (User.Identity.IsAuthenticated) {
				Response.Redirect("~/Default.aspx");
			}
		}
		protected void logonClick(object sender, EventArgs e) {
			if (mbprovider.ValidateUser(UserName.Text, UserPass.Text)) {
				FormsAuthentication.RedirectFromLoginPage(UserName.Text, Persist.Checked);
			} else {
				Msg.Text = "Invalid credentials. Please try again.";
			}
		}

		protected void registrationStart(object sender, EventArgs e) {
			Debug.WriteLine(ResolveUrl("~/auth/signup") + "?username=" + UserName.Text);
			Response.Redirect("~/auth/signup.aspx" + "?username=" + UserName.Text);
		}
	}

}