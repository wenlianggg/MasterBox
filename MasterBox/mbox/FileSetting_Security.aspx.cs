using System;
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
                // Generate list of non encrypted folders
                NewFolderPasswordOption.DataSource = MBFolder.GenerateUnencryptedFolderLocation(Context.User.Identity.Name);
                NewFolderPasswordOption.DataBind();

                // Generate list of encrypted folders
                ChangeFolderPasswordOption.DataSource = MBFolder.GenerateEncryptedFolderLocation(Context.User.Identity.Name);
                ChangeFolderPasswordOption.DataBind();
            }
        }

        protected void NewFolderPassword_Click(object sender, EventArgs e)
        {
            MBFolder folder = new MBFolder();
            folder.folderName = NewFolderPasswordOption.SelectedValue;
            folder.folderuserName = Context.User.Identity.Name;
            folder.folderencryption = 1;
            string folderpassword = NewPassword.Text;

            if (folder.NewFolderPassword(folder, folderpassword))
            {

            }else
            {

            }
            
        }

        protected void ChangeFolderPassword_Click(object sender, EventArgs e)
        {
            MBFolder folder = new MBFolder();
            folder.folderName= ChangeFolderPasswordOption.SelectedValue;
            folder.folderuserName = Context.User.Identity.Name;
            string oldpassword = CurrentPassword.Text;
            string newpassword = ChangeNewPassword.Text;
                      
            if (folder.ChangeFolderPassword(folder, oldpassword,newpassword))
            {
                CurrentPassword.Text = "";
                ChangeNewPassword.Text="";
            }
            else
            {

            }
            
            
            
        }
    }
}