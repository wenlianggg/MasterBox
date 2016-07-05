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
			IPAddr.Text = "Connected from: " + GetIPAddress();
			if (Context.User.Identity.IsAuthenticated) {
				User usr = User.GetUser(Context.User.Identity.Name);
				UserFullName.Text = usr.FirstName + " " + usr.LastName;
				UnameDropdown.Text = usr.UserName;
				SignInText.Text = "File Browser";
				SignInLink.HRef = "~/filestore/FileTransferInterface.aspx";
				IPAddr.Text = "Connected from: " + GetIPAddress();
			} else {
				SignInText.Text = "Login / Register";
				UserLogs.Visible = false;
				OTPConf.Visible = false;
				Subscriptions.Visible = false;
				Options.Visible = false;
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