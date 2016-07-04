using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MasterBox.Auth;
using System.Text;

namespace MasterBox.Auth {
	public partial class DEBUG : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			User usr = Auth.User.GetUser(5);
			Response.Write(usr.UserId + "<br>");
			Response.Write(usr.UserName + "<br>");
			Response.Write(usr.FirstName + "<br>");
			Response.Write(usr.LastName + "<br>");
			Response.Write(usr.Email + "<br>");
			Response.Write(usr.AesKey + "<br>");
			Response.Write(usr.AesIV + "<br>");
		}
	}
}