using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Net.Mail;

namespace MasterBox.fileshare
{
    public class LinkShare
    {
        public string link { get; set; }
        public DateTime dt { get; set; }
        public FileAccess fa { get; set; }
        
        public LinkShare(string link)
        {
            this.link = link;
            RetrieveLinkShare();
        }

        public LinkShare(string link, DateTime dt, FileAccess fa)
        {
            this.link = link;
            this.dt = dt;
            this.fa = fa;
        }

        public LinkShare(string link, FileAccess fa)
        {
            this.link = link;
            this.fa = fa;
        }
        public LinkShare(string link, DateTime dt, long userid, long folderid, bool download, bool upload, bool deleter)
        {
            this.link = link;
            this.dt = dt;
            fa = new FileAccess(userid, folderid, download, upload, deleter);
        }

        private static SqlConnection SQLGetMBoxConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        public void RetrieveLinkShare()
        {
            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM mb_linkshare WHERE link = @link", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@link", SqlDbType.VarChar, 16));
            cmd.Prepare();
            cmd.Parameters["@link"].Value = this.link;
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                dt = (DateTime)reader["dt"]; ;
                long userid = (long)reader["userid"];
                long folderid = (long)reader["folderid"];
                bool download = (bool)reader["download"];
                bool upload = (bool)reader["upload"];
                bool deleter = (bool)reader["deleter"];
                fa = new FileAccess(userid, folderid, download, upload, deleter);
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Link doesn't exist.");
            }
        }

        public void UploadLinkShare()
        {
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO mb_linkshare(link, dt, userid, folderid, download, upload, deleter) VALUES(@link, CURRENT_TIMESTAMP, @userid, @folderid, @download, @upload, @deleter)", SQLGetMBoxConnection());
            cmd.Parameters.AddWithValue("@link", link);
            cmd.Parameters.AddWithValue("@userid", fa.userid);
            cmd.Parameters.AddWithValue("@folderid", fa.folderid);
            cmd.Parameters.AddWithValue("@download", fa.download);
            cmd.Parameters.AddWithValue("@upload", fa.upload);
            cmd.Parameters.AddWithValue("@deleter", fa.deleter);

            SqlDataReader reader = cmd.ExecuteReader();

            System.Diagnostics.Debug.WriteLine("LinkShare created.");
        }

        public void CreateFileAccess()
        {
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO mb_fileaccess(userid, folderid, download, upload, deleter) VALUES(@userid, @folderid, @download, @upload, @deleter)", SQLGetMBoxConnection());
            cmd.Parameters.AddWithValue("@userid", fa.userid);
            cmd.Parameters.AddWithValue("@folderid", fa.folderid);
            cmd.Parameters.AddWithValue("@download", fa.download);
            cmd.Parameters.AddWithValue("@upload", fa.upload);
            cmd.Parameters.AddWithValue("@deleter", fa.deleter);

            SqlDataReader reader = cmd.ExecuteReader();

            System.Diagnostics.Debug.WriteLine("FileAccess created.");
        }

        public string Verify(long userid)
        {
            string str = "";
            if (!CheckUser(userid))
            {
               str += "You are not the intended recipient of this invitation link. ";
            }


            System.Diagnostics.Debug.WriteLine("Run CheckDate;");

            if (!CheckDate())
            {
                str += "Invitation link has timed out.";
            }
            return str;
        }
        public bool CheckUser(long userid)
        {
            if (userid == fa.userid)
            {
                return true;
            }
            return false;
        }

        public bool CheckDate()
        {
            TimeSpan difference = DateTime.Now - dt;

            System.Diagnostics.Debug.WriteLine("TimeSpan difference is: " + difference);

            if (difference.TotalDays < 1)
            {
                return true;
            }
            return false;
        }
    }
}