using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox
{
    public partial class Subscriptions : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Progress Bar Values //
            double freespace = 5;
            double actualspace = 1000 + freespace;
            double spaceused = 2;
            double percentused = (spaceused / actualspace) * 100;
            double availablespace = actualspace - spaceused;
            var roundedused = Math.Round(percentused, 1);

            if(roundedused <= 40)
            {
                TotalSpace.InnerText = actualspace + " MB Total";
                Bar.Attributes["style"] = "width:" + roundedused + "%";
                Bar.Attributes["class"] = "progress-bar progress-bar-success progress-bar-striped active";
                Bar.InnerText = Convert.ToString(roundedused) + "%";
                Available.InnerText = availablespace + " MB available";
            }
            else if (roundedused <= 70)
            {
                TotalSpace.InnerText = actualspace + " MB Total";
                Bar.Attributes["style"] = "width:" + roundedused + "%";
                Bar.Attributes["class"] = "progress-bar progress-bar-warning progress-bar-striped active";
                Bar.InnerText = Convert.ToString(roundedused) + "%";
                Available.InnerText = availablespace + " MB available";
            }
            else
            {
                TotalSpace.InnerText = actualspace + " MB Total";
                Bar.Attributes["style"] = "width:" + roundedused + "%";
                Bar.Attributes["class"] = "progress-bar progress-bar-danger progress-bar-striped active";
                Bar.InnerText = Convert.ToString(roundedused) + "%";
                Available.InnerText = availablespace + " MB available";
            }
            
   

            // Subscription Plans Values //
            FreeSpace.InnerText = "Free Space Given: " + freespace + "MB";
        }

        protected void GoToFiles(object sender, EventArgs e)
        {
            Response.Redirect("~/mbox/FileTransferInterface.aspx");
        }

        protected void GoToPrices(object sender, EventArgs e)
        {
            Response.Redirect("~/Pricing.aspx");
        }
    }
}
