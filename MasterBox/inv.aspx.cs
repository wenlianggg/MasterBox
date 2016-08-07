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
            
            string link = Request.QueryString["link"];
            System.Diagnostics.Debug.WriteLine("Link is: "+link);
            LinkShare ls;
            
            if (Request.QueryString["link"]==null)
            {
                VerifyLabel.Text = "Invalid Link. Redirecting back to home. ";
                Response.AddHeader("REFRESH", "10; URL=Default.aspx");
            }
            else
            {
                ls = new LinkShare(link);
                
                if (ls.fa==null||ls.dt==null)
                {
                    VerifyLabel.Text = "Invalid Link. Redirecting back to home.";
                    Response.AddHeader("REFRESH", "10; URL=Default.aspx");
                }
                else
                {

                    long userid = Auth.User.GetUser(Context.User.Identity.Name).UserId;
                    
                    string str = ls.Verify(userid);
                    VerifyLabel.Text = str;
                    if (str == "")
                    {
                        Check.Text = "Verified. Redirecting to folders in 10 seconds.";
                        ls.CreateFileAccess();

                        Response.AddHeader("REFRESH", "10;URL=filestore/FileTransferInterface.aspx");
                    }
                    else
                    {
                        Check.Text = "Failed to verify. Returning to home.";
                        Response.AddHeader("REFRESH", "10;URL=Default.aspx");
                    }
                }
            }
        }

    }
}