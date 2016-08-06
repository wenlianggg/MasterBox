using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Auth {
	public partial class SignUp : Page {
		protected void Page_Load(object sender, EventArgs e) {
			if (User.Identity.IsAuthenticated) {
				Response.Redirect("~/Default.aspx");
			}
			if (Request.QueryString["username"] != null && !Page.IsPostBack) {
				UserName.Text = Request.QueryString["username"];
			}
		}

		protected void processRegistration(object sender, EventArgs e) {
			Msg.Text = string.Empty;
			string EncodedResponse = Request.Form["g-Recaptcha-Response"];
			if (reCAPTCHA.Validate(EncodedResponse)) {
				Page.Validate();
				if (Page.IsValid) {
					try {
						User newuser = Auth.User.CreateUser(UserName.Text, UserPass.Text,
															FirstName.Text, LastName.Text,
															UserEmail.Text, false);
						ConfirmSent.Visible = true;
						EmailAddrSent.Text = newuser.Email;
						RegFields.Visible = false;
					} catch (UserAlreadyExistsException) {
						Msg.Text = "User already exists";
						return;
					}
				} else {
					Msg.Text = "An error has occured while registering.";
				}
			} else {
				Msg.Text = "CAPTCHA is invalid.";
			}
		}
	}
}