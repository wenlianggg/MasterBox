using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Diagnostics;
using MasterBox.Auth;

namespace MasterBox.Auth {
	public partial class SignIn : Page { 
		protected void Page_Load(object sender, EventArgs e) {
			if (User.Identity.IsAuthenticated) {
				Response.Redirect("~/Default.aspx");
			}
		}
		protected void logonClick(object sender, EventArgs e) {
			MBProvider mbp = MBProvider.Instance;
			try {
				if (mbp.ValidateUser(UserName.Text, UserPass.Text)) {
					System.Diagnostics.Debug.WriteLine("correk");
					Session["Username"] = mbp.GetCorrectCasingUN(UserName.Text);
					Session["IsPasswordAuthorized"] = true;
					Session["StayLoggedIn"] = Persist.Checked;
					Response.Redirect("~/Auth/otpverify.aspx");
				}
			} catch (UserNotFoundException) {
				Msg.Text = "Invalid credentials. Please try again, please check your username casing!";
			}
		}

		protected void registrationStart(object sender, EventArgs e) {
			Debug.WriteLine(ResolveUrl("~/auth/signup") + "?username=" + UserName.Text);
			Response.Redirect("~/auth/signup.aspx" + "?username=" + UserName.Text);
		}
	}

}