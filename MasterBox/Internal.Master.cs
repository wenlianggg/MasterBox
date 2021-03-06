﻿using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox
{
    public partial class Internal : System.Web.UI.MasterPage
    {
		void Page_Load(object sender, EventArgs e) {
			LoggedIn.Text = Context.User.Identity.Name;
			User usr = User.GetUser(Context.User.Identity.Name);
			UserFullName.Text = usr.FirstName + " " + usr.LastName;
			UnameDropdown.Text = usr.UserName;
		}

		void Logout_Click(object sender, EventArgs e) {
			FormsAuthentication.SignOut();
			Response.Redirect("~/Auth/signin.aspx");
		}
    }
}