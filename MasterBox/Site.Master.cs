using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox {
	public partial class SiteMaster : MasterPage {
		protected void Page_Load(object sender, EventArgs e) {
			IPAddr.Text = "Connected from: " + GetIPAddress();
			if (Context.User.Identity.IsAuthenticated) {
				WelcomeBack.Text = "Welcome Back, " + Context.User.Identity.Name;
				LoggedInUser.Text = "Logged in as: " + Context.User.Identity.Name;
				IPAddr.Text = "Connected from: " + GetIPAddress();
				SignInLink.HRef = "~/mbox/FileTransferInterface.aspx";
			} else {
				WelcomeBack.Text = "Login / Register";
				SignOutLink.Visible = false;
			}
		}

		protected string GetIPAddress() {
			string IPAddr = Context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if (!string.IsNullOrEmpty(IPAddr)) {
				string[] addresses = IPAddr.Split(',');
				if (addresses.Length != 0) {
					return addresses[0];
				}
			}
			return Context.Request.ServerVariables["REMOTE_ADDR"];
		}
	}
}