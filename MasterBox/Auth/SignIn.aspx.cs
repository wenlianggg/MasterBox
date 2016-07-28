
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Diagnostics;

namespace MasterBox.Auth {
	public partial class SignIn : Page { 
		protected void Page_Load(object sender, EventArgs e) {
			if (User.Identity.IsAuthenticated) {
				Response.Redirect("~/Default.aspx");
			}
		}
		protected void logonClick(object sender, EventArgs e) {
			if (IPBlock.Instance.CheckUser(UserName.Text) != null) {
				Msg.Text = IPBlock.Instance.CheckUser(UserName.Text);
				return;
			}
			MBProvider mbp = MBProvider.Instance;
			try {
				if (mbp.ValidateUser(UserName.Text, UserPass.Text)) {
					User usr = Auth.User.GetUser(UserName.Text);
					Session["UserEntity"] = usr;
					Session["IsPasswordAuthorized"] = true;
					Session["StayLoggedIn"] = Persist.Checked;
					if (MBProvider.Instance.IsTotpEnabled(usr.UserName)) {
						Response.Redirect("~/Auth/otpverify.aspx", false);
					} else {
						MBProvider.Instance.LoginSuccess(usr, Persist.Checked);
					}
				} else {
					Msg.Text = "Invalid credentials, please try again!";
				}
			} catch (UserNotFoundException) {
				Msg.Text = "Invalid credentials, please try again!";
			} catch (InvalidTOTPLength) {
				Msg.Text = "Error while loading OTP info, contact us.";
			}
		}

		protected void Registration_Start(object sender, EventArgs e) {
			if (Auth.User.Exists(UserName.Text)) {
				Msg.Text = "User already exists!";
			} else {
				Debug.WriteLine(ResolveUrl("~/auth/signup") + "?username=" + UserName.Text);
				Response.Redirect("~/auth/signup.aspx" + "?username=" + UserName.Text);
			}
		}
	}

}