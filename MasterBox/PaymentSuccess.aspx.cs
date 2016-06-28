using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

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
            }

            Response.AddHeader("REFRESH", "3;URL=Default.aspx");
        }
    }
}