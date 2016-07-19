using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Prefs {
    public partial class Prefs : System.Web.UI.MasterPage {
        protected void Page_Load(object sender, EventArgs e) {
			switch (Path.GetFileName(Request.Url.ToString())) {
				case "prefgeneral":
					prefgeneral.Attributes.Add("class", "currentpage");
					break;
				case "prefprofile":
					prefprofile.Attributes.Add("class", "currentpage");
					break;
				case "prefpassword":
					prefpassword.Attributes.Add("class", "currentpage");
					break;
				case "prefsecurity":
					prefsecurity.Attributes.Add("class", "currentpage");
					break;
				case "prefotpsetup":
					prefotpsetup.Attributes.Add("class", "currentpage");
					break;
				case "prefsubscription":
					prefsubscription.Attributes.Add("class", "currentpage");
					break;
				case "preflogs":
					preflogs.Attributes.Add("class", "currentpage");
					break;
				default:
					break;
			}

		}
    }
}