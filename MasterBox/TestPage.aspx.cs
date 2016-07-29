using MasterBox.fileshare;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.filestore
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string link = Request.QueryString["link"];
            System.Diagnostics.Debug.WriteLine("Link is: "+link);
            /*
            check link in database
               LinkShare ls = new LinkShare();
               ls = ls.RetrieveLinkShare(link);
            if no match, error, redirect to home
               if (ls == new LinkShare())
               Response.AddHeader("REFRESH", "10;URL=Default.aspx");
            */
            long userid = 5;
            //check userid is not null, if null redirect to login

            DateTime dt = new DateTime(2016, 6, 20);
            LinkShare ls = new LinkShare("1234567812345678", dt, 5, 5, true, false, false);
            HelloWorldLabel.Text = "Hello, " + GetRandomString(16);
            string str = ls.Verify(userid);
            VerifyLabel.Text = str;
            if (str == "")
            {
                Check.Text = "Verified. Redirecting to folders in 10 seconds.";
                //add entry in FileAccess database
                //redirect file list


                Response.AddHeader("REFRESH", "10;URL=filestore/FileTransferInterface.aspx");
            }
            else
            {
                Check.Text = "Failed to verify. Returning to home.";
                //redirect home
                Response.AddHeader("REFRESH", "10;URL=Default.aspx");
            }

            DLButton.Enabled = false;
            UPLButton.Enabled = false;
            DELButton.Enabled = false;

            if (ls.fa.download)
                DLButton.Enabled = true;
            if (ls.fa.upload)
                UPLButton.Enabled = true;
            if (ls.fa.delete)
                DELButton.Enabled = true;
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

        public static LinkShare GenerateLinkShare(long userid, long folderid, bool download, bool upload, bool delete)
        {
            string link = GetRandomString(16);
            FileAccess fa = new FileAccess(userid, folderid, download, upload, delete);

            LinkShare ls = new LinkShare(link, fa);

            return ls;
        }
    }
}