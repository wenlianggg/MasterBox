using MasterBox.Auth;
using MasterBox.mbox;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Admin
{
    public partial class FileMgmt : System.Web.UI.Page
    {
        DataTable dtUser;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                GetUsersTable();
            }
        }

        protected void GetUsersTable(string username = null)
        {
            dtUser = new DataTable();
            SqlDataReader reader = MBFile.GetUsersToDisplay(username);
            dtUser.Load(reader);

            userstable.DataSource = dtUser;
            userstable.DataBind();
        }

        protected void SearchBtn_Click(object sender, EventArgs e)
        {
            string username = searchTxt.Text;
            GetUsersTable(username);
        }

        protected void UsersLinkBtn_Command(object sender, CommandEventArgs e)
        {
            string command = e.CommandArgument.ToString();


        }
    }
}