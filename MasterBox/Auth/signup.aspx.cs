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
			if (Request.QueryString["username"] != null) {
				UserName.Text = Request.QueryString["username"];
			}
		}

		protected void processRegistration(object sender, EventArgs e) {
			Msg.Text = string.Empty;
			string EncodedResponse = Request.Form["g-Recaptcha-Response"];
			if (reCAPTCHA.Validate(EncodedResponse)) {
				Msg.Text = "CAPTCHA is valid";
				Page.Validate();
				if (Page.IsValid) {
					User newuser = new User(UserName.Text,
											FirstName.Text,
											LastName.Text,
											DateTime.Now,
											UserEmail.Text,
											false);
					Msg.Text = newuser.UserId.ToString();
				} else {
					Msg.Text = "An error has occured while registering.";
				}
			} else {
				Msg.Text = "CAPTCHA is invalid.";
			}
		}
	}
}