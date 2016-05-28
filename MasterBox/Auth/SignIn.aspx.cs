﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace MasterBox {
	public partial class SignIn : System.Web.UI.Page {

		protected void Page_Load(object sender, EventArgs e) {
			if (User.Identity.IsAuthenticated)
			{
				Response.Redirect("~/Auth/Default.aspx");
			}
		}
		protected void Logon_Click(object sender, EventArgs e) {
			if ((UserEmail.Text == "Dominic") && (UserPass.Text == "123") ||
				(UserEmail.Text == "James") && (UserPass.Text == "123") ||
				(UserEmail.Text == "Roy") && (UserPass.Text == "123") ||
				(UserEmail.Text == "Wilfred") && (UserPass.Text == "123") ||
				(UserEmail.Text == "Wuggle") && (UserPass.Text == "123")){
				FormsAuthentication.RedirectFromLoginPage(UserEmail.Text, Persist.Checked);
			} else {
				Msg.Text = "Invalid credentials. Please try again.";
			}
		}

	}
}