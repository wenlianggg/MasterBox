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

		}
	}
}