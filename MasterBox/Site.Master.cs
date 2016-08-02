using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox {
	public partial class SiteMaster : MasterPage {
		protected void Page_Load(object sender, EventArgs e) {
			IPAddr.Text = "Your IP address (" + GetIP() + ") will be logged for security";
			if (Context.User.Identity.IsAuthenticated) {
				User usr = User.GetUser(Context.User.Identity.Name);
				NavRightLink1.Visible = false;
				NavRightLabel2.Text = "File Browser";
				NavRightLink2.HRef = "~/filestore/FileTransferInterface.aspx";
				UserFullName.Text = usr.FirstName + " " + usr.LastName;
				UnameDropdown.Text = usr.UserName;
			} else {
				NavRightLabel1.Text = "Register  <i class=\"fa fa-user-plus\" aria-hidden=\"true\"></i>";
				NavRightLink1.HRef = "~/Auth/signup.aspx";
				NavRightLabel2.Text = "Login  <i class=\"fa fa-sign-in\" aria-hidden=\"true\"></i>";
				NavRightLink2.HRef = "~/Auth/signin.aspx";
				Options.Visible = false;
			}
		}

		protected string GetIP() {
			HttpRequest thisrequest = HttpContext.Current.Request;
			HttpBrowserCapabilities browser = thisrequest.Browser;
			string ipAddress = thisrequest.ServerVariables["HTTP_X_FORWARDED_FOR"];
			string userIp = "NIL";

			if (!string.IsNullOrEmpty(ipAddress)) {
				string[] addresses = ipAddress.Split(',');
				if (addresses.Length != 0) {
					userIp = addresses[0];
				}
			} else {
				userIp = thisrequest.ServerVariables["REMOTE_ADDR"];
			}

			if (userIp.Equals("::1")) {
				userIp = "127.0.0.1";
			}

			return userIp + " on " + browser.Type;
		}
	}

	public class TimingModule : IHttpModule {
		public void Dispose() {
		}

		public void Init(HttpApplication context) {
			context.BeginRequest += OnBeginRequest;
			context.EndRequest += OnEndRequest;
		}

		void OnBeginRequest(object sender, System.EventArgs e) {
			if (HttpContext.Current.Request.IsLocal
				&& HttpContext.Current.IsDebuggingEnabled) {
				var stopwatch = new Stopwatch();
				HttpContext.Current.Items["Stopwatch"] = stopwatch;
				stopwatch.Start();
			}
		}

		void OnEndRequest(object sender, System.EventArgs e) {
			if (HttpContext.Current.Request.IsLocal
				&& HttpContext.Current.IsDebuggingEnabled) {
				Stopwatch stopwatch =
				  (Stopwatch)HttpContext.Current.Items["Stopwatch"];
				stopwatch.Stop();

				TimeSpan ts = stopwatch.Elapsed;
				string elapsedTime = String.Format("{0}ms", ts.TotalMilliseconds);

				HttpContext.Current.Response.Write("<p>" + elapsedTime + "</p>");
			}
		}
	}
}