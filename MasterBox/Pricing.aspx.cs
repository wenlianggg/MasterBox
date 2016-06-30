using System;
using System.Collections.Generic;
using System.Configuration;
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
            string business = "VY34CAC6JZ6LU";
            string itemName = "5 MegaBytes";
            double itemAmount = 100.00;
            string currencyCode = "SGD";

            StringBuilder ppHref = new StringBuilder();

            ppHref.Append("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick");
            ppHref.Append("&business=" + business);
            ppHref.Append("&item_name=" + itemName);
            ppHref.Append("&amount=" + itemAmount.ToString("#.00"));
            ppHref.Append("&currency_code=" + currencyCode);

            Response.Redirect(ppHref.ToString(), true);
        }

        protected void PayPalBtn10MB_Click(object sender, ImageClickEventArgs e)
        {
            string business = "VY34CAC6JZ6LU";
            string itemName = "10 MegaBytes";
            double itemAmount = 200.00;
            string currencyCode = "SGD";

            StringBuilder ppHref = new StringBuilder();

            ppHref.Append("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick");
            ppHref.Append("&business=" + business);
            ppHref.Append("&item_name=" + itemName);
            ppHref.Append("&amount=" + itemAmount.ToString("#.00"));
            ppHref.Append("&currency_code=" + currencyCode);

            Response.Redirect(ppHref.ToString(), true);
        }

        protected void PayPalBtn15MB_Click(object sender, ImageClickEventArgs e)
        {
            string business = "VY34CAC6JZ6LU";
            string itemName = "15 MegaBytes";
            double itemAmount = 300.00;
            string currencyCode = "SGD";

            StringBuilder ppHref = new StringBuilder();

            ppHref.Append("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick");
            ppHref.Append("&business=" + business);
            ppHref.Append("&item_name=" + itemName);
            ppHref.Append("&amount=" + itemAmount.ToString("#.00"));
            ppHref.Append("&currency_code=" + currencyCode);

            Response.Redirect(ppHref.ToString(), true);
        }

        protected void PayPalBtn20MB_Click(object sender, ImageClickEventArgs e)
        {
            string business = "VY34CAC6JZ6LU";
            string itemName = "20 MegaBytes";
            double itemAmount = 400.00;
            string currencyCode = "SGD";

            StringBuilder ppHref = new StringBuilder();

            ppHref.Append("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick");
            ppHref.Append("&business=" + business);
            ppHref.Append("&item_name=" + itemName);
            ppHref.Append("&amount=" + itemAmount.ToString("#.00"));
            ppHref.Append("&currency_code=" + currencyCode);

            Response.Redirect(ppHref.ToString(), true);
        }
    }
}