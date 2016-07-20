using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace MasterBox.fileshare
{
    public class LinkShare
    {
        public string link { get; set; }
        public DateTime dt { get; set; }
        public FileAccess fa { get; set; }
        
        public LinkShare()
        {

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
        public LinkShare(string link, DateTime dt, long userid, long folderid, bool download, bool upload, bool delete)
        {
            this.link = link;
            this.dt = dt;
            fa = new FileAccess(userid, folderid, download, upload, delete);
        }

        private static SqlConnection SQLGetMBoxConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        public LinkShare RetrieveLinkShare(string link)
        {
            LinkShare ls = new LinkShare();

            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM linkshare WHERE link = @link", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@link", SqlDbType.VarChar, 16));
            cmd.Prepare();
            cmd.Parameters["@link"].Value = link;
            SqlDataReader reader = cmd.ExecuteReader();

            if (reader.Read())
            {
                DateTime dt = (DateTime)reader["dt"]; ;
                long userid = (long)reader["userid"];
                long folderid = (long)reader["folderid"];
                bool download = (bool)reader["download"];
                bool upload = (bool)reader["upload"];
                bool delete = (bool)reader["delete"];
                ls = new LinkShare(link, dt, userid, folderid, download, upload, delete);
            }

            return ls;
        }

        public void UploadLinkShare()
        {
            SqlCommand cmd = new SqlCommand(
                "INSERT INTO linkshare(link, dt, userid, folderid, download, upload, delete) VALUES(@link, NOW(), @userid, @folderid, @download, @upload, @delete", SQLGetMBoxConnection());
            cmd.Parameters.AddWithValue("@link", link);
            cmd.Parameters.AddWithValue("@userid", fa.userid);
            cmd.Parameters.AddWithValue("@folderid", fa.folderid);
            cmd.Parameters.AddWithValue("@download", fa.download);
            cmd.Parameters.AddWithValue("@upload", fa.upload);
            cmd.Parameters.AddWithValue("@delete", fa.delete);

            SqlDataReader reader = cmd.ExecuteReader();
        }

        public string Verify(long userid)
        {
            if (CheckUser(userid))
            {
                if (CheckDate())
                {
                    return "";
                }
                else return "Invitation link has timed out.";
            }
            else return "You are not the intended recipient of this invitation link.";
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

            System.Diagnostics.Debug.WriteLine(difference);

            if (difference.TotalDays < 1)
            {
                return true;
            }
            return false;
        }
    }
}