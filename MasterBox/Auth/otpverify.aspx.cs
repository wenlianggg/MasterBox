using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Auth {
    public partial class otpverify : System.Web.UI.Page {
		string username;
		bool loginpersist;

        protected void Page_Load(object sender, EventArgs e)  {
			if (Session["IsPasswordAuthorized"] != null) {
				if ((bool)Session["IsPasswordAuthorized"]) {
					username = (string)Session["Username"];
					loginpersist = (bool)Session["StayLoggedIn"];
					UID.Text = "Logging in as: " + username;
				} else {
					Response.Redirect("~/Auth/signout.aspx");
				}
			} else {
				Response.Redirect("~/Auth/signout.aspx");
			}
		}

        protected void ConfirmOTP(object sender, EventArgs e)  {
			if (MBProvider.Instance.ValidateTOTP(username, OTPValue.Text)) {
				Session.Abandon();				
				FormsAuthentication.RedirectFromLoginPage(username, loginpersist);
			} else {
				Msg.Text = "Incorrect OTP entered!";
			}
		}

		protected void CancelOTP(object sender, EventArgs e) {
			FormsAuthentication.RedirectFromLoginPage(username, loginpersist);
		}

    }
}