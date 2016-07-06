using System;
using System.Web.Security;

namespace MasterBox.Auth {
	public partial class otpverify : System.Web.UI.Page {
		User usr;
		bool loginpersist;

        protected void Page_Load(object sender, EventArgs e)  {
			if (Session["IsPasswordAuthorized"] != null) {
				if ((bool) Session["IsPasswordAuthorized"]) {
					usr = (User) Session["UserEntity"];
					loginpersist = (bool) Session["StayLoggedIn"];
					UID.Text = "Logging in as: " + usr.UserName;
				} else {
					Response.Redirect("~/Auth/signout.aspx");
				}
			} else {
				Response.Redirect("~/Auth/signout.aspx");
			}
		}

        protected void ConfirmOTP(object sender, EventArgs e)  {
			if (MBProvider.Instance.ValidateTOTP(usr.UserName, OTPValue.Text)) {
				Session.Abandon();
				MBProvider.Instance.LoginSuccess(usr, (bool) Session["StayLoggedIn"]);
			} else {
				Msg.Text = "Incorrect OTP entered!";
			}
		}

		protected void CancelOTP(object sender, EventArgs e) {
			MBProvider.Instance.LoginSuccess(usr, (bool)Session["StayLoggedIn"]);
		}

    }
}