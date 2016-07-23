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

			if (dtFile.Rows.Count > 0)
            {	
                FileTableView.DataSource = dtFile;
                FileTableView.DataBind();
            }
        }

        private void FillDataFolder()
        {
			dtFolder = new DataTable();
            SqlDataReader reader = MBFolder.GetFolderToDisplay(Context.User.Identity.Name);
            dtFolder.Load(reader);

            if (dtFolder.Rows.Count > 0)
            {
                FolderTableView.DataSource = dtFolder;
                FolderTableView.DataBind();
            }
        }

        private void FillFileDataFolder(string foldername,long folderid)
        {
            FolderHeader.Text = foldername;

            dtFolderFile = new DataTable();
            SqlDataReader reader = MBFile.GetFileFromFolderToDisplay(Context.User.Identity.Name,folderid);
            dtFolderFile.Load(reader);

            if (dtFolderFile.Rows.Count > 0)
            {
                GridView1.DataSource = dtFolderFile;
                GridView1.DataBind();
            }else
            {
                GridView1.DataSource = null;
                GridView1.DataBind();
            }
        }


        // Download Files from Master Folder     
        private void DownloadFileContent(string username, long fileid)
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

        // Download file from folders
        protected void DownloadFolderFile(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)lnk.NamingContainer;
            DownloadFolderFile(Context.User.Identity.Name, Int32.Parse(lnk.Attributes["FileID"]), Int32.Parse(lnk.Attributes["FolderID"]));

        }

        private void DownloadFolderFile(string username, int id,int folderid)
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
                        Stream strm = FileUpload.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(strm);
                        file.filecontent = br.ReadBytes((int)strm.Length);
                        file.fileSize = FileUpload.PostedFile.ContentLength;
                        bool uploadStatus = MBFile.UploadNewFile(file);
                        if (uploadStatus == true)
                        {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(),"Upload Status","<script language='javascript'>alert('"+"Upload Success"+"')</script>");
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
                        string foldername = UploadLocation.SelectedValue;
                        MBFile file = new MBFile();
                        file.fileusername = Context.User.Identity.Name;
                        file.fileName = Path.GetFileName(FileUpload.FileName);
                        file.fileType = FileUpload.PostedFile.ContentType;
                        Stream strm = FileUpload.PostedFile.InputStream;
                        BinaryReader br = new BinaryReader(strm);
                        file.filecontent = br.ReadBytes((int)strm.Length);
                        file.fileSize = FileUpload.PostedFile.ContentLength;
                        bool uploadfiletofolderstatus = MBFolder.UploadFileToFolder(file,foldername);
                        if (uploadfiletofolderstatus == true)
                        {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Upload Status", "<script language='javascript'>alert('" + "Upload Success" + "')</script>");
                        
                    }
                    else
                    {
                        Page.ClientScript.RegisterStartupScript(Page.GetType(), "Upload Status", "<script language='javascript'>alert('" + "Upload Fail, you may have exceeded your storage limit!" + "')</script>");
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

                // Pop up box
                Page.ClientScript.RegisterStartupScript(Page.GetType(), "Upload Status", "<script language='javascript'>alert('" + "Folder name exist" + "')</script>");

            }

        }


        protected void File_Command(object sender, CommandEventArgs e)
        {
			string command = e.CommandName;
            MBFile file;

            switch (command)
            {
                case "ShowPopup":
                    System.Diagnostics.Debug.WriteLine("Running");
                    long fileid = Convert.ToInt64(e.CommandArgument.ToString());
                    System.Diagnostics.Debug.WriteLine("FileID: " + fileid);
                    file = MBFile.RetrieveFile(Context.User.Identity.Name, fileid);
                    LblFileID.Text = fileid.ToString();
                    LblFileName.Text = file.fileName;
                    LblFileType.Text = file.fileType;
                    LblFileSize.Text = file.fileSize.ToString();

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "myModal", "showPopup();", true);
					break;

                case "Delete":
                    System.Diagnostics.Debug.WriteLine("Deleting");
                    MBFile.DeleteFile(Context.User.Identity.Name, Convert.ToInt64(LblFileID.Text));
                   
                    break;

                case "Download":
                    System.Diagnostics.Debug.WriteLine("Downloading");
                    DownloadFileContent(Context.User.Identity.Name, Convert.ToInt64(LblFileID.Text));
                    Page.ClientScript.RegisterStartupScript(Page.GetType(), "Delete Status", "<script language='javascript'>alert('" + "File has been deleted" + "')</script>");
                    break;
            }
        }

        protected void FolderLinkButton_Command(object sender, CommandEventArgs e)
        {

            LinkButton lnk = (LinkButton)sender;
            bool pass= Convert.ToBoolean(lnk.Attributes["FolderEncryption"]);

            string foldername = lnk.Text;
            long folderid = Convert.ToInt64(e.CommandArgument.ToString());
            System.Diagnostics.Debug.WriteLine("Checking: "+pass);
            if (pass)
            {                    
                 MBFolder folder = MBFolder.GetFolder(Context.User.Identity.Name, folderid);
                 LblFolderNamePass.Text = folder.folderName;
                 ScriptManager.RegisterStartupScript(this, this.GetType(), "folderPasswordModal", "showPopupPassword();", true);
             
            }else
            {
                FillFileDataFolder(foldername,folderid);
            }
        }

        protected void BtnCheckPasswordFolder_Click(object sender, EventArgs e)
        {
           MBFolder folder = MBFolder.GetFolder(Context.User.Identity.Name, LblFolderNamePass.Text);         
           string folderchkingpassword=TxtBoxPassword.Text;
            if (folder.ValidateFolderPassword(folder, folderchkingpassword))
            {
                FillFileDataFolder(folder.folderName, folder.folderid);
            }else
            {

            }
        }

        protected void BtnDeleteFolderWithPassw_Click(object sender, EventArgs e)
        {

        }
    }
}