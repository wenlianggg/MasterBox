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
            if (Context.User.Identity.IsAuthenticated)
            {
                PayPalBtn5MB.Visible = true;
                PayPalBtn10MB.Visible = true;
                PayPalBtn15MB.Visible = true;
                PayPalBtn20MB.Visible = true;
                LoginLink.Visible = false;
                LoginLink2.Visible = false;
                LoginLink3.Visible = false;
                ThisLogin.Visible = false;
            }
            else
            {
                LoginLink.Visible = true;
                LoginLink2.Visible = true;
                LoginLink3.Visible = true;
                ThisLogin.Visible = true;
                PayPalBtn5MB.Visible = false;
                PayPalBtn10MB.Visible = false;
                PayPalBtn15MB.Visible = false;
                PayPalBtn20MB.Visible = false;
            }
        }

        protected void PayPalBtn5MB_Click(object sender, ImageClickEventArgs e)
        {
            StringBuilder ppHref = new StringBuilder();

            ppHref.Append("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=RRFX7PSVFP3UQ");

            Response.Redirect(ppHref.ToString(), true);
        }

        protected void PayPalBtn10MB_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void PayPalBtn15MB_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void PayPalBtn20MB_Click(object sender, ImageClickEventArgs e)
        {

        }
    }
}