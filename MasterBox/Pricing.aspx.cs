using MasterBox.Auth;
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

        protected void PayPalBtn_Command(object sender, CommandEventArgs e)
        {
            switch (e.CommandName)
            {
                case "PopUpModal":
                    int storageOpted;
                    ImageButton buttonclicked = (ImageButton)sender;
                    if (Int32.TryParse(buttonclicked.Attributes["ItemSize"], out storageOpted))
                    {
                        string business = "VY34CAC6JZ6LU";
                        string itemName = Convert.ToString(storageOpted);
                        int itemAmount = 20 * storageOpted;
                        string itemId = buttonclicked.Attributes["ItemID"];

                        businesslbl.Text = business;
                        itemNamelbl.Text = itemName + " MB";
                        itemAmountlbl.Text = Convert.ToString(itemAmount);
                        itemIdlbl.Text = itemId;
                    }
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "showPopup();", true);
                    break;
                case "PayForMember":
                    User usr = Auth.User.GetUser(Context.User.Identity.Name);
                    if (MBProvider.Instance.ValidateTOTP(usr.UserName, OTPValue.Text).Equals(true))
                    {
                        StringBuilder ppHref = new StringBuilder();
                        int itemAmount = Convert.ToInt32(itemAmountlbl.Text);
                        string currencyCode = "SGD";

                        //Build Link
                        ppHref.Append("https://www.sandbox.paypal.com/cgi-bin/webscr?cmd=_xclick");
                        ppHref.Append("&business=" + businesslbl.Text);
                        ppHref.Append("&item_name=" + itemNamelbl.Text);
                        ppHref.Append("&amount=" + itemAmount.ToString("#.00"));
                        ppHref.Append("&currency_code=" + currencyCode);
                        ppHref.Append("&item_id=" + itemIdlbl.Text);

                        //Redirect user to payment 
                        Response.Redirect(ppHref.ToString(), true);

                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Upload Status", "<script language='javascript'>alert('" + "OTP Entered is Incorrect!" + "')</script>");
                        Msg.Text = "Entered OTP is incorrect!";
                    }
                    break;
            }
        }

    }
}