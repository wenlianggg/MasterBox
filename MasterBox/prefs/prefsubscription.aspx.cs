using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.mbox
{
    public partial class PrefSubscription : System.Web.UI.Page
    {
		protected void Page_Load(object sender, EventArgs e) {
            String user = Context.User.Identity.Name;
            User currentUser = Auth.User.GetUser(user);

            // Progress Bar Values //
            double freespace = 5;
			double actualspace = (currentUser.MbrType * 5);
            double addspace = actualspace - 5;
            double spaceused = BytesToMega(MBFile.GetTotalFileStorage(user));
			double percentused = (spaceused / actualspace) * 100;
			double availablespace = actualspace - spaceused;
			var roundedused = Math.Round(percentused, 1);

			if (roundedused <= 40) {
				TotalSpace.InnerText = actualspace + " MB Total";
				Bar.Attributes["style"] = "width:" + roundedused + "%";
				Bar.Attributes["class"] = "progress-bar progress-bar-success progress-bar-striped active";
				Bar.InnerText = Convert.ToString(roundedused) + "%";
				Available.InnerText = availablespace + " MB available";
			}
			else if (roundedused <= 70) {
				TotalSpace.InnerText = actualspace + " MB Total";
				Bar.Attributes["style"] = "width:" + roundedused + "%";
				Bar.Attributes["class"] = "progress-bar progress-bar-warning progress-bar-striped active";
				Bar.InnerText = Convert.ToString(roundedused) + "%";
				Available.InnerText = availablespace + " MB available";
			}
			else {
				TotalSpace.InnerText = actualspace + " MB Total";
				Bar.Attributes["style"] = "width:" + roundedused + "%";
				Bar.Attributes["class"] = "progress-bar progress-bar-danger progress-bar-striped active";
				Bar.InnerText = Convert.ToString(roundedused) + "%";
				Available.InnerText = availablespace + " MB available";
			}



			// Subscription Plans Values //
			FreeSpace.InnerText = "Free Space Given: " + freespace + "MB";
            Additional.InnerText = "Additional Space: " + addspace + "MB";
            SubscriptionStart.InnerText = "Subscription Start Date: " + currentUser.MbrStart.ToString();
            SubscriptionEnd.InnerText = "Subscription Expiry Date: " + currentUser.MbrExpiry.ToString();

		}

		protected void GoToFiles(object sender, EventArgs e) {
			Response.Redirect("~/mbox/FileTransferInterface.aspx");
		}

		protected void GoToPrices(object sender, EventArgs e) {
			Response.Redirect("~/Pricing.aspx");
		}

        protected double BytesToMega(double bytes)
        {
            return (bytes / 1024f) / 1024f;
        }
	}
}