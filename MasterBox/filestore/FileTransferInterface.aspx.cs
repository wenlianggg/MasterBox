using System;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using MasterBox.mbox;
using System.Web.UI;

namespace MasterBox
{
    public partial class FileTransferInterface : System.Web.UI.Page
    {
        DataTable dtFile;
        DataTable dtFolder;
        DataTable dtFolderFile;

        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {
                // Fill up file and folder data on the display
                FillDataFile();
                FillDataFolder();

                // Fill up folder location for upload
                UploadLocation.DataSource = MBFolder.GenerateFolderLocation(Context.User.Identity.Name);
                UploadLocation.DataBind();
            }

        }
        private void FillDataFile()
        {
            dtFile = new DataTable();
            SqlDataReader reader = MBFile.GetFileToDisplay(Context.User.Identity.Name);
            dtFile.Load(reader);

            FileTableView.DataSource = dtFile;
            FileTableView.DataBind();

        }

        private void FillDataFolder()
        {
            dtFolder = new DataTable();
            SqlDataReader reader = MBFolder.GetFolderToDisplay(Context.User.Identity.Name);
            dtFolder.Load(reader);


            FolderTableView.DataSource = dtFolder;
            FolderTableView.DataBind();

        }

        private void FillDataSharedFolder()
        {
            dtFolder = new DataTable();
            SqlDataReader reader = MBFolder.GetSharedFolderToDisplay(Context.User.Identity.Name);
            dtFolder.Load(reader);


            SharedFolderTableView.DataSource = dtFolder;
            SharedFolderTableView.DataBind();

        }

        private void FillFileDataFolder(string foldername, long folderid)
        {
            FolderHeader.Text = foldername;

            dtFolderFile = new DataTable();
            SqlDataReader reader = MBFile.GetFileFromFolderToDisplay(Context.User.Identity.Name, folderid);
            dtFolderFile.Load(reader);

            GridView1.DataSource = dtFolderFile;
            GridView1.DataBind();

        }


        // Download Files from Master Folder     
        private void DownloadFileContent(string username, long fileid, long folderid = 0)
        {
            MBFile mbf = MBFile.RetrieveFile(username, fileid);
            Response.ClearContent();
            Response.ContentType = mbf.fileType;
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + mbf.fileName + "\"");
            Response.AddHeader("Content-Length", mbf.fileSize.ToString());

            Response.BinaryWrite(mbf.filecontent);
            Response.Flush();
            Response.Close();
        }

        // Download Files from Folder
        private void DownloadFolderFile(string username, long id, long folderid)
        {
            MBFile mbf = MBFolder.RetrieveFolderFile(username, id, folderid);
            Response.ClearContent();
            Response.ContentType = mbf.fileType;
            Response.AddHeader("Content-Disposition", "attachment;filename=\"" + mbf.fileName + "\"");
            Response.AddHeader("Content-Length", mbf.fileSize.ToString());

            Response.BinaryWrite(mbf.filecontent);
            Response.Flush();
            Response.Close();
        }

        // Upload a new file
        protected void NewUploadFile_Click(object sender, EventArgs e)
        {
            if (FileUpload.HasFile)
            {
                if (UploadLocation.SelectedValue == "==Master Folder==")
                {
                    MBFile file = new MBFile();
                    file.fileusername = Context.User.Identity.Name;
                    file.fileName = Path.GetFileName(FileUpload.FileName);
                    file.fileType = FileUpload.PostedFile.ContentType;
                  //This is for testing for chart // file.filetimestamp= DateTime.Now;

                    Stream strm = FileUpload.PostedFile.InputStream;
                    BinaryReader br = new BinaryReader(strm);
                    file.filecontent = br.ReadBytes((int)strm.Length);
                    file.fileSize = FileUpload.PostedFile.ContentLength;
                    if (MBFile.FilenameCheck(Context.User.Identity.Name, Path.GetFileName(FileUpload.FileName)))
                    {
                        bool uploadStatus = MBFile.UploadNewFile(file);
                        if (uploadStatus == true)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Upload Status", "<script language='javascript'>alert('" + "Upload Success" + "')</script>");
                            // Update the file table
                            FillDataFile();
                        }
                        // File cannot be uploaded
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Upload Status", "<script language='javascript'>alert('" + "Upload Fail, you may have exceeded your storage limit!" + "')</script>");
                        }
                    }
                    else
                    {
                        // Same file name                
                        TxtBoxCurrentFileName.Text = Path.GetFileName(FileUpload.FileName);
                        TxtBoxFileNameCheck.Text = Path.GetFileName(FileUpload.FileName);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "filenameModal", "showPopupFileName();", true);


                    }

                }
                else
                {

                    string foldername = UploadLocation.SelectedValue;
                    MBFile file = new MBFile();
                    file.fileusername = Context.User.Identity.Name;
                    file.fileName = Path.GetFileName(FileUpload.FileName);
                    file.fileType = FileUpload.PostedFile.ContentType;
                    Stream strm = FileUpload.PostedFile.InputStream;
                    BinaryReader br = new BinaryReader(strm);
                    file.filecontent = br.ReadBytes((int)strm.Length);
                    file.fileSize = FileUpload.PostedFile.ContentLength;
                    if (MBFile.FilenameCheck(file.fileusername, file.fileName, foldername))
                    {
                        bool uploadfiletofolderstatus = MBFolder.UploadFileToFolder(file, foldername);
                        if (uploadfiletofolderstatus == true)
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Upload Status", "<script language='javascript'>alert('" + "Upload Success" + "')</script>");

                        }
                        else
                        {
                            Page.ClientScript.RegisterStartupScript(Page.GetType(), "Upload Status", "<script language='javascript'>alert('" + "Upload Fail, you may have exceeded your storage limit!" + "')</script>");
                        }
                    }
                    else
                    {


                    }

                }
            }


        }


        // Creating a folder
        protected void CreateNewFolder_Click(object sender, EventArgs e)
        {

            bool folderCreation;
            bool foldernamecheck = MBFolder.CheckFolderName(FolderName.Text, Context.User.Identity.Name);
            if (MBFolder.CheckFolderName(FolderName.Text, Context.User.Identity.Name))
            {
                System.Diagnostics.Debug.WriteLine("Password:" + encryptionPass.Text);
                MBFolder folder = new MBFolder();
                folder.folderName = FolderName.Text;
                folder.folderusername = Context.User.Identity.Name;
                folderCreation = folder.CreateNewFolder(folder, encryptionPass.Text);
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Upload Status", "<script language='javascript'>alert('" + "Folder Created" + "')</script>");
                FillDataFolder();
            }
            else
            {
                // Pop up box to ask the person to change
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Upload Status", "<script language='javascript'>alert('" + "Folder name exist" + "')</script>");

            }

        }

        // Check file name
        protected void BtnUploadFile_Click(object sender, EventArgs e)
        {
            MBFile checkfile = MBFile.RetrieveFile(Context.User.Identity.Name, TxtBoxCurrentFileName.Text);
            System.Diagnostics.Debug.WriteLine("File name: " + checkfile);
            string value = RdBtnFileName.SelectedValue;
            if (value == "change")
            {
                checkfile.fileName = TxtBoxFileNameCheck.Text;
                MBFile.UploadNewFile(checkfile);
                FillDataFile();
            }
            else
            {
                MBFile.OverrideFile(checkfile);
                FillDataFile();
            }
        }

        // File Options
        protected void File_Command(object sender, CommandEventArgs e)
        {
            string command = e.CommandName;
            MBFile file;

            switch (command)
            {

                case "ShowPopup":
                    long fileid = Convert.ToInt64(e.CommandArgument.ToString());
                    file = MBFile.RetrieveFile(Context.User.Identity.Name, fileid);
                    LblFileID.Text = fileid.ToString();
                    LblFileName.Text = file.fileName;
                    LblFileType.Text = file.fileType;
                    LblFileSize.Text = file.fileSize.ToString();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "fileModal", "showPopupFile();", true);
                    break;

                case "Download":
                    System.Diagnostics.Debug.WriteLine("Downloading");
                    DownloadFileContent(Context.User.Identity.Name, Convert.ToInt64(LblFileID.Text));
                    break;

                case "Delete":
                    System.Diagnostics.Debug.WriteLine("Deleting");
                    MBFile.DeleteFile(Context.User.Identity.Name, Convert.ToInt64(LblFileID.Text));
                    FillDataFile();
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Delete Status", "<script language='javascript'>alert('" + "File has been deleted!" + "')</script>");
                    break;

            }
        }

        protected void FileFolder_Command(object sender, CommandEventArgs e)
        {
            string command = e.CommandName;
            long folderid = Convert.ToInt64(LblFolderID.Text);

            MBFile file;
            MBFolder folder = MBFolder.GetFolder(Context.User.Identity.Name, folderid);
            switch (command)
            {

                case "OpenFolderFile":
                    long fileid = Convert.ToInt64(e.CommandArgument.ToString());
                    System.Diagnostics.Debug.WriteLine("File ID: " + fileid);
                    file = MBFolder.RetrieveFolderFile(Context.User.Identity.Name, fileid, folderid);
                    LblFolderFileId.Text = fileid.ToString();
                    LblFolderFileName.Text = file.fileName;
                    LblFolderFileType.Text = file.fileType;
                    LblFolderFileSize.Text = file.fileSize.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "folderfileModal", "showPopupFolderFile();", true);
                    break;
                case "DownloadFolderFile":
                    DownloadFolderFile(Context.User.Identity.Name, Convert.ToInt64(LblFolderFileId.Text), folder.folderid);
                    break;
                case "DeleteFolderFile":
                    MBFile.DeleteFile(Context.User.Identity.Name, Convert.ToInt64(LblFolderFileId.Text));
                    FillFileDataFolder(folder.folderName, folder.folderid);
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Delete Status", "<script language='javascript'>alert('" + "File has been deleted!" + "')</script>");
                    break;
            }
        }

        protected void BtnFolderWithoutPass_Command(object sender, CommandEventArgs e)
        {
            string command = e.CommandName;
            string foldername = LblFolderName.Text;
            long folderid = Convert.ToInt64(LblFolderID.Text);

            switch (command)
            {
                case "OpenFolder":
                    FillFileDataFolder(foldername, folderid);
                    break;
                case "DeleteFolder":
                    System.Diagnostics.Debug.WriteLine("Delete folder");

                    break;
            }
        }
        protected void BtnFolderWithPass_Command(object sender, CommandEventArgs e)
        {
            string command = e.CommandName;
            MBFolder folder = MBFolder.GetFolder(Context.User.Identity.Name, LblFolderNamePass.Text);
            string folderchkingpassword = TxtBoxPassword.Text;
            switch (command)
            {
                case "OpenFolder":
                    if (folder.ValidateFolderPassword(folder, folderchkingpassword))
                    {
                        // Update data table
                        FillFileDataFolder(folder.folderName, folder.folderid);
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Password Status", "<script language='javascript'>alert('" + "Wrong Password, try again!" + "')</script>");

                    }
                    break;
                case "DeleteFolder":
                    if (folder.ValidateFolderPassword(folder, folderchkingpassword))
                    {
                        //delete folder
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Password Status", "<script language='javascript'>alert('" + "Wrong Password, try again!" + "')</script>");

                    }
                    break;
            }
        }


        protected void FolderLinkButton_Command(object sender, CommandEventArgs e)
        {

            LinkButton lnk = (LinkButton)sender;
            bool pass = Convert.ToBoolean(lnk.Attributes["FolderEncryption"]);

            string foldername = lnk.Text;
            long folderid = Convert.ToInt64(e.CommandArgument.ToString());
            MBFolder folder = MBFolder.GetFolder(Context.User.Identity.Name, folderid);
            if (pass)
            {
                LblFolderNamePass.Text = folder.folderName;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "folderPasswordModal", "showPopupPassword();", true);

            }
            else
            {
                LblFolderName.Text = folder.folderName;
                LblFolderID.Text = folderid.ToString();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "folderModal", "showPopupFolder();", true);
            }
        }



        protected void BtnDeleteFolderWithPassw_Click(object sender, EventArgs e)
        {
            MBFolder folder = MBFolder.GetFolder(Context.User.Identity.Name, LblFolderNamePass.Text);
            MBFolder.DeleteFolder(folder.folderid);
        }

    }
}