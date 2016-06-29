using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Auth {
	public partial class SignUp : Page {
		protected void Page_Load(object sender, EventArgs e) {

		}

		protected void processRegistration(object sender, EventArgs e) {
			string EncodedResponse = Request.Form["g-Recaptcha-Response"];
			if (reCAPTCHA.Validate(EncodedResponse) == "True" ? true : false) {
				Msg.Text = "CAPTCHA is valid";
				Page.Validate();
				if (Page.IsValid) {
					} else {
						Msg.Text = "An error has occured while registering.";
					}
			} else {
				Msg.Text = "CAPTCHA is invalid";
			}
		}
	}
}