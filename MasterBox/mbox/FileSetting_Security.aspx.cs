﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.mbox
{
    public partial class FileSetting_Security : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FolderPasswordOption.DataSource = MBFolder.GenerateEncryptedFolderLocation(Context.User.Identity.Name);
                FolderPasswordOption.DataBind();
            }
        }

        protected void ChangeFolderPassword_Click(object sender, EventArgs e)
        {
            string foldername = FolderPasswordOption.SelectedValue;
            string oldpassword = CurrentPassword.Text;
            string newpassword = NewPassValid.Text;

            if (MBFolder.ChangeFolderPassword(foldername, oldpassword,newpassword))
            {
                testing.Text = "can";
                CurrentPassword.Text = "";
                NewPassValid.Text="";
            }
            else
            {
                testing.Text = "cannot";
            }
            
        }
    }
}