using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Configuration;
using MasterBox.mbox;

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

        // Trying to do download button
        protected void DownloadFile(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)lnk.NamingContainer;
			Download(Context.User.Identity.Name, Int32.Parse(lnk.Attributes["FileID"]));
        }


        private void Download(string username, int id)
        {
			MBFile mbf = MBFile.RetrieveFile(username, id);
			Response.ClearContent();
			Response.ContentType = mbf.fileType;
			Response.AddHeader("Content-Disposition", "attachment;filename=\"" + mbf.fileName + "\"");
			Response.AddHeader("Content-Length", mbf.fileSize.ToString());

			Response.BinaryWrite(mbf.filecontent);
			Response.Flush();
			Response.Close();
		}
        protected void NewUploadFile_Click(object sender, EventArgs e)
        {
            if (FileUpload.HasFile)
            {
                if (UploadLocation.SelectedValue == "==Master Folder==")
                {
                    try
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
                            // Update the file table
                            FillDataFile();
                        }
                    }
                    catch
                    {
                      
                    }
                }
                else
                {
                    try
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
                            FillDataFile();
                        }
                    }
                    catch
                    {
                       
                    }
                }
            }


        }

        protected void CreateNewFolder_Click(object sender, EventArgs e)
        {
            bool folderCreation;
            if (encryptionOption.Text == "yes")
            {
                MBFolder folder = new MBFolder();
                folder.folderName = FolderName.Text;
                folder.folderuserName = Context.User.Identity.Name;
                folder.folderencryption = 1;
                folderCreation = folder.CreateNewFolderWithPassword(folder,encryptionPass.Text);
            }
            else
            {
                MBFolder folder = new MBFolder();
                folder.folderName = FolderName.Text;
                folder.folderuserName = Context.User.Identity.Name;
                folder.folderencryption = 0;
                folderCreation = folder.CreateNewFolder(folder);

            }
            if (folderCreation == true)
            {
                
            }
            else
            {
                
            }
            // Reset the form fields
            Response.Redirect(Request.RawUrl);
        }

        protected void OpenFolder(object sender, EventArgs e)
        {
            // Get Folder id
            LinkButton lnk = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)lnk.NamingContainer;
            int folderid=Int32.Parse(lnk.Attributes["FolderID"]);
            
                dtFolderFile = new DataTable();
                SqlDataReader reader = MBFile.GetFileFromFolderToDisplay(Context.User.Identity.Name, folderid);
                dtFile.Load(reader);

                if (dtFolderFile.Rows.Count > 0)
                {
                    Folder_FileTableView.DataSource = dtFolderFile;
                    Folder_FileTableView.DataBind();
                }
            
        }
        
    }
}