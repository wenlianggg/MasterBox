using MasterBox.Auth;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace MasterBox.fileshare
{
    public class FileAccess
    {
        public long userid { get; set; }
        public long folderid { get; set; }
        public bool download { get; set; }
        public bool upload { get; set; }
        public bool deleter { get; set; }

        public FileAccess(long userid, long folderid, bool download, bool upload, bool deleter)
        {
            this.userid = userid;
            this.folderid = folderid;
            this.download = download;
            this.upload = upload;
            this.deleter = deleter;
        }

        private static SqlConnection SQLGetMBoxConnection()
        {
            SqlConnection sqlConnection = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
            sqlConnection.Open();
            return sqlConnection;
        }

        public static ArrayList GetSelfAccess(string username)
        {
            ArrayList accessList = new ArrayList();
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM mb_fileaccess WHERE folderid in (SELECT folderid FROM fileaccess WHERE userid = @userid) GROUP BY folderid HAVING COUNT(folderid) = 1", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = user.UserId;

            SqlDataReader accessReader = cmd.ExecuteReader();

            while (accessReader.HasRows)
            {
                while (accessReader.Read())
                {
                    long userid = (long)accessReader["userid"];
                    long folderid = (long)accessReader["folderid"];
                    bool download = (bool)accessReader["download"];
                    bool upload = (bool)accessReader["upload"];
                    bool deleter = (bool)accessReader["deleter"];
                    accessList.Add(new FileAccess(userid, folderid, download, upload, deleter));
                }
                accessReader.NextResult();
            }

            return accessList;
        }

        public static ArrayList GetSharedAccess(string username)
        {
            ArrayList accessList = new ArrayList();
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand(
                "SELECT * FROM mb_fileaccess WHERE folderid in (SELECT folderid FROM fileaccess WHERE userid = @userid) GROUP BY folderid HAVING COUNT(folderid) > 1", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@userid"].Value = user.UserId;

            SqlDataReader accessReader = cmd.ExecuteReader();

            while (accessReader.HasRows)
            {
                while (accessReader.Read())
                {
                    long userid = (long)accessReader["userid"];
                    long folderid = (long)accessReader["folderid"];
                    bool download = (bool)accessReader["download"];
                    bool upload = (bool)accessReader["upload"];
                    bool deleter = (bool)accessReader["deleter"];
                    accessList.Add(new FileAccess(userid, folderid, download, upload, deleter));
                }
                accessReader.NextResult();
            }
            
            return accessList;
        }

        public void RemoveAccess(string username, long userid, long folderid)
        {
            User user = User.GetUser(username);

            SqlCommand cmd = new SqlCommand(
                "DELETE * FROM mb_fileaccess WHERE folderid = @folderid AND userid = @userid", SQLGetMBoxConnection());
            cmd.Parameters.Add(new SqlParameter("@folderid", SqlDbType.BigInt, 8));
            cmd.Parameters.Add(new SqlParameter("@userid", SqlDbType.BigInt, 8));
            cmd.Prepare();
            cmd.Parameters["@folderid"].Value = folderid;
            cmd.Parameters["@userid"].Value = userid;
            cmd.ExecuteReader();
        }
    }
}