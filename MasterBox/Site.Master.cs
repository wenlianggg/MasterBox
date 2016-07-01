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
				UnameDropdown.Text = Context.User.Identity.Name;
				SignInText.Text = "File Browser";
				SignInLink.HRef = "~/mbox/FileTransferInterface.aspx";
				IPAddr.Text = "Connected from: " + GetIPAddress();
			} else {
				SignInText.Text = "Login / Register";
				UserLogs.Visible = false;
				OTPConf.Visible = false;
				Subscriptions.Visible = false;
				Options.Visible = false;
				UserSettings.Visible = false;
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