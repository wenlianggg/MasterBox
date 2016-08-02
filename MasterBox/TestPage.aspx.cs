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
                VerifyLabel.Text = "Invalid Link. Redirecting back to home. GTFO " + Auth.User.GetUser(Context.User.Identity.Name);
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

        public static string GetRandomString(int length)
        {
            string charPool = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz1234567890";
            StringBuilder sb = new StringBuilder();
            Random rnd = new Random();

            while ((length--) > 0)
                sb.Append(charPool[(int)(rnd.NextDouble() * charPool.Length)]);

            return sb.ToString();
        }

        public static LinkShare GenerateLinkShare(string link, long userid, long folderid, bool download, bool upload, bool delete)
        {
            FileAccess fa = new FileAccess(userid, folderid, download, upload, delete);

            LinkShare ls = new LinkShare(link, fa);

            return ls;
        }

        public void sendShare(string email)
        {
            string link = GetRandomString(16);
            GenerateLinkShare(link, 5, 80, true, true, true);

            SmtpClient smtpClient = new SmtpClient("gmail.com", 25);

            smtpClient.Credentials = new System.Net.NetworkCredential("masterboxnoreply@gmail.com", "N0tasmurf!");
            smtpClient.UseDefaultCredentials = true;
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtpClient.EnableSsl = true;
            MailMessage mail = new MailMessage();

            //Setting From , To and CC
            mail.From = new MailAddress("masterboxnoreply@gmail.com", "MasterBox");
            mail.To.Add(new MailAddress(email));
            mail.Body = "Hello,\n\nHere is a link\nwww.masterboxsite.azurewebsites.net/TestPage?link="+link+"\n\nRegards,\nMasterBox";

            smtpClient.Send(mail);
        }
    }
}