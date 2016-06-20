using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Auth
{
    public partial class oneTimePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void ConfirmOTP(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

        protected void CancelOTP(object sender, EventArgs e)
        {
            Response.Redirect("~/Default.aspx");
        }

    }
}