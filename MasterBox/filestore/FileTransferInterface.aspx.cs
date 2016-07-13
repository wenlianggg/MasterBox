using System;
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
        DataTable dtDelete;

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
                
                DeleteLocation.DataSource=MBFolder.GenerateFolderLocation(Context.User.Identity.Name);
                DeleteLocation.DataBind();

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
        protected void OpenFolder(object sender, EventArgs e)
        {
            // Get Folder id
            LinkButton lnk = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)lnk.NamingContainer;
            int folderid = Int32.Parse(lnk.Attributes["FolderID"]);
            string foldername = lnk.Text;
            FolderHeader.Text = foldername;
            FillFileDataFolder(folderid);


        }

        private void FillFileDataFolder(int folderid)
        {
            dtFolderFile = new DataTable();
            SqlDataReader reader = MBFile.GetFileFromFolderToDisplay(Context.User.Identity.Name,folderid);
            dtFolderFile.Load(reader);

            if (dtFolderFile.Rows.Count > 0)
            {
                GridView1.DataSource = dtFolderFile;
                GridView1.DataBind();
            }
        }

        // Download Files from Master Folder
        protected void DownloadFile(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)lnk.NamingContainer;
            DownloadFile(Context.User.Identity.Name, Int32.Parse(lnk.Attributes["FileID"]));
        }
        private void DownloadFile(string username, int id)
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
                            // Update the file table
                            FillDataFile();
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
                            FillDataFile();
                        }

                }
            }


        }

        // Creating a folder
        protected void CreateNewFolder_Click(object sender, EventArgs e)
        {



            bool folderCreation;
            //bool foldernamecheck = MBFolder.CheckFolderName(FolderName.Text, Context.User.Identity.Name);
            //if (MBFolder.CheckFolderName(FolderName.Text, Context.User.Identity.Name))
            //{
                System.Diagnostics.Debug.WriteLine("Password:" + encryptionPass.Text);
                    MBFolder folder = new MBFolder();
                    folder.folderName = FolderName.Text;
                    folder.folderusername = Context.User.Identity.Name;
                    folderCreation = folder.CreateNewFolder(folder, encryptionPass.Text); 
                         
            //}
            //else
            //{
                // Pop up box
                
            //}
            // Reset the form fields
            Response.Redirect(Request.RawUrl);

        }

        protected void Delete_Click(object sender, EventArgs e)
        {

        }

        protected void DeleteLocation_SelectedIndexChanged(object sender, EventArgs e)
        {
            // To test out if i get the value
            e.ToString();

            Testing.Text = "Working Status";

            dtDelete = new DataTable();
            SqlDataReader reader = MBFile.GetFileToDisplay(Context.User.Identity.Name);
            dtDelete.Load(reader);

            if (dtDelete.Rows.Count > 0)
            {
                GridView2.DataSource = dtFile;
                GridView2.DataBind();
            }
        }
    }
}