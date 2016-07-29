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
                PayPalBtn30MB.Visible = true;
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
                PayPalBtn30MB.Visible = false;
                PayPalBtn10MB.Visible = false;
                PayPalBtn15MB.Visible = false;
                PayPalBtn20MB.Visible = false;
            }
        }

        protected void PayPalBtn10MB_Command(object sender, CommandEventArgs e)
        {
            ImageButton buttonclicked = (ImageButton)sender;
            businesslbl.Text = "VY34CAC6JZ6LU";
            itemNamelbl.Text = 100 + " MB";
            itemAmountlbl.Text = "20";
            currencyCodelbl.Text = "SGD";
            itemIdlbl.Text = buttonclicked.Attributes["ItemID"];

            switch (e.CommandName)
            {
                case "PopUpModal":
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "showPopup();", true);
                    break;
                case "PayForMember":
                    int storageOpted;
                    if (Int32.TryParse(buttonclicked.Attributes["ItemSize"], out storageOpted))
                    {
                        string business = businesslbl.Text;
                        string itemName = itemNamelbl.Text;
                        double itemAmount = 20;
                        string currencyCode = currencyCodelbl.Text;
                        string itemId = itemIdlbl.Text;

                        StringBuilder ppHref = new StringBuilder();

                        ppHref.Append("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick");
                        ppHref.Append("&business=" + business);
                        ppHref.Append("&item_name=" + itemName);
                        ppHref.Append("&amount=" + itemAmount.ToString("#.00"));
                        ppHref.Append("&currency_code=" + currencyCode);
                        ppHref.Append("&item_id=" + itemId);

                        // Response.Redirect(ppHref.ToString(), true);
                    }
                    break;

            }
        }
    }
 }