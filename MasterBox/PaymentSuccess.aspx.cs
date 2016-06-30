using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PayPal.Api;

namespace MasterBox
{
    public partial class PaymentSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.UrlReferrer != null)
            {
                string previousPageUrl = Request.UrlReferrer.AbsoluteUri;
                string previousPageName = System.IO.Path.GetFileName(Request.UrlReferrer.AbsolutePath);
                System.Diagnostics.Debug.WriteLine(previousPageUrl);
            }

            Response.AddHeader("REFRESH", "3;URL=Default.aspx");
        }
    }
}