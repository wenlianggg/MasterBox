using MasterBox.fileshare;
using System;
using System.Net.Mail;
using System.Text;

namespace MasterBox.filestore
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

            DLButton.Enabled = false;
            UPLButton.Enabled = false;
            DELButton.Enabled = false;

            string link = Request.QueryString["link"];
            System.Diagnostics.Debug.WriteLine("Link is: "+link);
            LinkShare ls;
            //check link in database
            if (Request.QueryString["link"]==null)
            {
                VerifyLabel.Text = "Invalid Link. Redirecting back to home. GTFO ";
            }
            else
            {
                ls = new LinkShare(link);

                //check linkmatch
                if (ls.fa==null||ls.dt==null)
                {
                    VerifyLabel.Text = "Invalid Link. Redirecting back to home.";
                    Response.AddHeader("REFRESH", "10; URL=Default.aspx");
                }
                else
                {

                    long userid = Auth.User.GetUser(Context.User.Identity.Name).UserId;
                    //check userid is not null, if null redirect to login

                    /*
                    if (ls.fa.download)
                        DLButton.Enabled = true;
                    if (ls.fa.upload)
                        UPLButton.Enabled = true;
                    if (ls.fa.deleter)
                        DELButton.Enabled = true;
                    */


                    HelloWorldLabel.Text = "Hello, " + Auth.User.GetUser(Context.User.Identity.Name);
                    string str = ls.Verify(userid);
                    VerifyLabel.Text = str;
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
                        Check.Text = "Failed to verify. Returning to home.";
                        //redirect home
                        //Response.AddHeader("REFRESH", "10;URL=Default.aspx");
                    }
                }
            }
        }

    }
}