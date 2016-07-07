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

        protected void PayPalBtn_Click(object sender, ImageClickEventArgs e)
        {
            ImageButton buttonclicked = (ImageButton)sender;
            int storageOpted;
            if (Int32.TryParse(buttonclicked.Attributes["ItemSize"], out storageOpted)) {
                string business = "VY34CAC6JZ6LU";
                string itemName = storageOpted + " MB";
                double itemAmount = 20.00 * storageOpted;
                string currencyCode = "SGD";
                string itemId = buttonclicked.Attributes["ItemID"];

                StringBuilder ppHref = new StringBuilder();

                ppHref.Append("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick");
                ppHref.Append("&business=" + business);
                ppHref.Append("&item_name=" + itemName);
                ppHref.Append("&amount=" + itemAmount.ToString("#.00"));
                ppHref.Append("&currency_code=" + currencyCode);
                ppHref.Append("&item_id=" + itemId);

                Response.Redirect(ppHref.ToString(), true);
            }
        }

       
    }
}