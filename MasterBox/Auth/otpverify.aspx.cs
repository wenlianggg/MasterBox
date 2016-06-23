using System;
using System.Collections.Generic;
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
			if ((bool) Session["IsPasswordAuthorized"]) {
				username = (string) Session["Username"];
				loginpersist = (bool) Session["StayLoggedIn"];
				UID.Text = "Logging in as: " + username;
				Persist.Text = "Stay logged in: " + loginpersist;
			} else {
				Response.Redirect("~/Auth/signout.aspx");
			}
        }

        protected void ConfirmOTP(object sender, EventArgs e)  {
			MBProvider mbp = MBProvider.Instance;
			if (mbp.ValidateTOTP(username, OTPValue.Text)) {
				FormsAuthentication.RedirectFromLoginPage(username, loginpersist);
			} else {
				Msg.Text = "Incorrect OTP entered!";
			}
		}

		protected void CancelOTP(object sender, EventArgs e)
        {
			FormsAuthentication.RedirectFromLoginPage(username, loginpersist);
		}

    }
}