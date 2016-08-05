using MasterBox.fileshare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox
{
    public partial class inv : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string link = Request.QueryString["link"];
            System.Diagnostics.Debug.WriteLine("Link is: " + link);
            LinkShare ls;
            //check link in database
            if (Request.QueryString["link"] == null)
            {
                VerificationLabel.text = "Invalid Link. Redirecting back to home. GTFO " + Auth.User.GetUser(Context.User.Identity.Name);
            }
            else
            {
                ls = new LinkShare(link);

                //check linkmatch
                if (ls.fa == null || ls.dt == null)
                {
                    VerifyLabel.Text = "Invalid Link. Redirecting back to home.";
                    Response.AddHeader("REFRESH", "10; URL=Default.aspx");
                }
                else
                {

                    long userid = 5;
                    //check userid is not null, if null redirect to login

                    /*
                    if (ls.fa.download)
                        DLButton.Enabled = true;
                    if (ls.fa.upload)
                        UPLButton.Enabled = true;
                    if (ls.fa.deleter)
                        DELButton.Enabled = true;
                    */


                    HelloWorldLabel.Text = "Hello, " + GetRandomString(16) + Auth.User.GetUser(Context.User.Identity.Name);
                    string str = ls.Verify(userid);
                    VerificationLabel.Text = str;
                    if (str == "")
                    {
                        Check.Text = "Verified. Redirecting to folders in 10 seconds.";
                        //add entry in FileAccess database
                        ls.CreateFileAccess();

                        //redirect file list


                        //Response.AddHeader("REFRESH", "10;URL=filestore/FileTransferInterface.aspx");
                    }
                    else
                    {
                        CheckLabel.Text = "Failed to verify. Returning to home.";
                        //redirect home
                        //Response.AddHeader("REFRESH", "10;URL=Default.aspx");
                    }
                }
            }
        }
        public static string GetRandomString(int length)
        {
            string charPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            while ((length--) > 0)
                sb.Append(charPool[(int)(rnd.NextDouble() * charPool.Length)]);

            return sb.ToString();
        }
    }
}