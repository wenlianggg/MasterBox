using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using MasterBox.Auth;
using System.Text;
using System.Configuration;
using System.Web.Configuration;

namespace MasterBox.Auth {
	public partial class DEBUG : System.Web.UI.Page {

		string provider = "RsaProtectedConfigurationProvider";
		string section = "connectionStrings";

		protected void Page_Load(object sender, EventArgs e) {
			// User usr = Auth.User.GetUser(5);
			// Response.Write(usr.UserId + "<br>");
			// Response.Write(usr.UserName + "<br>");
			// Response.Write(usr.FirstName + "<br>");
			// Response.Write(usr.LastName + "<br>");
			// Response.Write(usr.Email + "<br>");
			// Response.Write(usr.AesKey + "<br>");
			// Response.Write(usr.AesIV + "<br>");
		}

		protected void btnEncrypt_Click(object sender, EventArgs e) {
			try {
				Configuration confg = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
				ConfigurationSection confStrSect = confg.GetSection(section);
				if (confStrSect != null) {
					confStrSect.SectionInformation.ProtectSection(provider);
					confg.Save();
				}
				// the encrypted section is automatically decrypted!!
				Response.Write("Configuration Section " + "<b>" +
					WebConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString + "</b>" + " is automatically decrypted");
			} catch (Exception) {

			}
		}

		protected void btnDecrypt_Click(object sender, EventArgs e) {
			try {
				Configuration confg = WebConfigurationManager.OpenWebConfiguration(Request.ApplicationPath);
				ConfigurationSection confStrSect = confg.GetSection(section);
				if (confStrSect != null && confStrSect.SectionInformation.IsProtected) {
					confStrSect.SectionInformation.UnprotectSection();
					confg.Save();
				}

			} catch (Exception) {

			}
		}
	}
}