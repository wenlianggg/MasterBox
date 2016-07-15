using System;
using System.IO;
using System.Text;
using System.Net;
using System.Web;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using MasterBox.Auth;

namespace MasterBox
{
    public partial class PaymentSuccess : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // Get current user
            User currUser = Auth.User.GetUser(Context.User.Identity.Name);

            // CUSTOMIZE THIS: This is the seller's Payment Data Transfer authorization token.
            // Replace this with the PDT token in "Website Payment Preferences" under your account.
            string authToken = "Sbx5L1TExNnB1KTDit_UOn4Q0zYqZjzfoCI2udRxvfoQ0OW3A91-KtO9vFO";
            string txToken = Request.QueryString["tx"];
            string query = "cmd=_notify-synch&tx=" + txToken + "&at=" + authToken;

            //Paypal doesnt work with TLS1 anymore
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            //Post back to either sandbox
            string strSandbox = "https://www.sandbox.paypal.com/cgi-bin/webscr";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(strSandbox);

            //Set values for the request back
            req.Method = "POST";
            req.ContentType = "application/x-www-form-urlencoded";
            req.ContentLength = query.Length;


            //Send the request to PayPal and get the response
            StreamWriter streamOut = new StreamWriter(req.GetRequestStream(), System.Text.Encoding.ASCII);
            streamOut.Write(query);
            streamOut.Close();
            StreamReader streamIn = new StreamReader(req.GetResponse().GetResponseStream());
            string strResponse = streamIn.ReadToEnd();
            streamIn.Close();

            Dictionary<string, string> results = new Dictionary<string, string>();
            if (strResponse != "")
            {
                StringReader reader = new StringReader(strResponse);
                string line = reader.ReadLine();

                if (line == "SUCCESS")
                {

                    while ((line = reader.ReadLine()) != null)
                    {
                        results.Add(line.Split('=')[0], line.Split('=')[1]);

                    }

                    // // Execution of updates to user entity // //

                    // Initialization of variables
                    String clearItem = SpecChar.CleanInput(results["item_name"]);
                    ItemName.InnerText = "You successfully paid for: " + clearItem;
                    String storageString = Regex.Match(clearItem, @"\d+").Value;
                    int storageBought = Int32.Parse(storageString);
                    if (currUser.MbrType > 0)
                    {
                        // First time buying storage 
                        if (currUser.MbrType == 1)
                        {
                            currUser.MbrType = storageBought / 5;
                            currUser.MbrStart = DateTime.Now;
                            currUser.MbrStart = DateTime.Now.AddMonths(1);
                        }
                        // Add months to current storage plan
                        else if (currUser.MbrType == (storageBought / 5))
                        {
                            currUser.MbrExpiry = currUser.MbrExpiry.AddMonths(1);
                        }
                        // Upgrade from current plan
                        else if (currUser.MbrType < (storageBought / 5))
                        {
                            int daysleft = DateTime.Compare(currUser.MbrStart, currUser.MbrExpiry);
                            int mbDays = daysleft * (currUser.MbrType * 5);
                            int addDays = mbDays / (storageBought);

                            currUser.MbrType = storageBought / 5;
                            currUser.MbrStart = DateTime.Now;
                            currUser.MbrExpiry = DateTime.Now.AddDays(addDays).AddMonths(1);
                        }
                        // Downgrade from current plan
                        else if (currUser.MbrType > (storageBought / 5))
                        {
                            int daysleft = DateTime.Compare(currUser.MbrStart, currUser.MbrExpiry);
                            int mbDays = daysleft * (currUser.MbrType * 5);
                            int addDays = mbDays / (storageBought);

                            currUser.MbrType = storageBought / 5;
                            currUser.MbrStart = DateTime.Now;
                            currUser.MbrExpiry = DateTime.Now.AddDays(addDays).AddMonths(1);
                        }
                        TransactLogger.Instance.TransactionCompleted(currUser.UserId, storageBought);
                    }
                    // // End execution of updates // //
                }
                else if (line == "FAIL")
                {
                    // Log for manual investigation
                    Response.Write("Unable to retrive transaction detail");
                }
            }
            else
            {
                //unknown error
                Response.Write("ERROR");
            }

            Response.AddHeader("REFRESH", "5;URL=Default.aspx");
        }
    }
}