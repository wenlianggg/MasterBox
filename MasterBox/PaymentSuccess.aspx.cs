using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using PayPal.Api;
using System.Collections;
using System.Collections.Specialized;

namespace MasterBox
{
    public partial class PaymentSuccess : System.Web.UI.Page
    {
        private static List<string> posts = new List<string>();
        private static List<NameValueCollection> forms = new List<NameValueCollection>();

        protected void Page_Load(object sender, EventArgs e)
        {
            String[] keys = Request.Form.AllKeys;
            for(int i=0; i < keys.Length; i++)
            {
                posts.Add(keys[i] + ": " + Request.Form[keys[i]] + "<br>");
            }
            foreach (var post in posts)
            {
                Response.Write(post);
            }

            if (Request.UrlReferrer != null)
            {
                string previousPageUrl = Request.UrlReferrer.AbsoluteUri;
                string previousPageName = System.IO.Path.GetFileName(Request.UrlReferrer.AbsolutePath);
                System.Diagnostics.Debug.WriteLine(previousPageUrl);
                GetNewWebhook();
            }

            //Response.AddHeader("REFRESH", "3;URL=Default.aspx");
        }

        public static string GetNewWebhookUrl()
        {
            return "https://masterboxsite.azurewebsites.net/PaymentSuccess" + Guid.NewGuid().ToString();
        }

        /// <summary>
        /// Helper method for creating a new webhook object to be used by the webhook sample pages.
        /// </summary>
        /// <returns>A new Webhook object.</returns>
        public static Webhook GetNewWebhook()
        {
            return new Webhook
            {
                url = GetNewWebhookUrl(),
                event_types = new List<WebhookEventType>
                {
                    new WebhookEventType
                    {
                        name = "PAYMENT.SALE.COMPLETED"
                    },
                    new WebhookEventType
                    {
                        name = "PAYMENT.SALE.DENIED"
                    }
                }

            };
        }
    }
}