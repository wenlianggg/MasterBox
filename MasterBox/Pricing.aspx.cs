using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox
{
    public partial class Pricing : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        { 

        }

        protected void PayPalBtn_Click(object sender, ImageClickEventArgs e)
        {
            StringBuilder ppHref = new StringBuilder();

            ppHref.Append("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RRFX7PSVFP3UQ");

            Response.Redirect(ppHref.ToString(), true);
        }
    }
}