using MasterBox.Auth;
using MasterBox.mbox;
using System;
using System.Data;
using System.Data.SqlClient;
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
            int command = Convert.ToInt32(e.CommandArgument.ToString());
            System.Diagnostics.Debug.WriteLine("What is this? " + command);

            Auth.User user = Auth.User.GetUser(command);
            LblUserId.Text = user.UserId.ToString();
            LblUserName.Text = user.UserName;
            LblUserEmail.Text = user.Email;
            FolderNameOption.DataSource = MBFolder.GenerateEncryptedFolderLocation(user.UserName);
            FolderNameOption.DataBind();

            InformationPanel.Style.Add("display", "block");
        }

        protected void FolderNameOption_SelectedIndexChanged(object sender, EventArgs e)
        {
            Auth.User user = Auth.User.GetUser(LblUserName.Text);
            MBFolder folder = MBFolder.GetFolder(Context.User.Identity.Name, FolderNameOption.SelectedValue.ToString());
            LblFileinFolderNum.Text = MBFolder.CountFileNumInFolder(Context.User.Identity.Name, folder.folderid).ToString();
            LblFolderDate.Text = folder.foldertimestamp.ToString();
        }

        protected void LnkBtnResetPass_Click(object sender, EventArgs e)
        {
            if (FolderNameOption.SelectedValue.ToString() != "==Choose a Folder==")
            {
                string resetpass = MBFolder.RandomPasswordGeneration(16);
                MBFolder folder = MBFolder.GetFolder(Context.User.Identity.Name, FolderNameOption.SelectedValue.ToString());
                if (folder.FolderPasswordSettings(folder, resetpass, true))
                {
                    Auth.User user = Auth.User.GetUser(LblUserName.Text);
                    LblResetPass.Text = resetpass;
                    ToEmailTxtBox.Text = LblUserEmail.Text;
                    SubjectTxtBox.Text = "Folder Password Reset";
                    string message =
                         "Dear " + user.FirstName + " " + user.LastName + ","
                         + "We have reset the password for the folder you have specified. Password has been changed to:"
                         + "Yours Sincerely, MasterBox";
                    MessageTxtBox.Text = message.Replace("\r\n", "<br/>");
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "emailModal", "showEmailPopup();", true);
                }
                else
                {
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Folder alert", "<script language='javascript'>alert('" + "Password failure!" + "')</script>");
                }
            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Folder alert", "<script language='javascript'>alert('" + "Please choose a folder" + "')</script>");
            }
        }

        protected void BtnSendEmail_Click(object sender, EventArgs e)
        {
            Mail mail = new Mail();
            if (mail.SendEmail(ToEmailTxtBox.Text, SubjectTxtBox.Text, MessageTxtBox.Text))
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Email alert", "<script language='javascript'>alert('" + "Email Sent!" + "')</script>");

            }
            else
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Email alert", "<script language='javascript'>alert('" + "Fail to send email!" + "')</script>");
            }
        }
    }
}