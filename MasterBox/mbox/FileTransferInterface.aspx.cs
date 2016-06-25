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
using System.Collections;

namespace MasterBox
{
    public partial class FileTransferInterface : System.Web.UI.Page
    {
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {
            // Fill up file data on the display
            if (!IsPostBack)
            {

                FillData();
                FillDataFolder();
                // Fill up folder location for upload
                UploadLocation.DataSource = MBFolder.GenerateFolderLocation(Context.User.Identity.Name);
                UploadLocation.DataBind();
            }

        }
        private void FillData()
        {
            DataTable dtFile = new DataTable();
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
            DataTable dtFile = new DataTable();
            SqlDataReader reader = MBFolder.GetFolderToDisplay(Context.User.Identity.Name);
            dtFile.Load(reader);

            if (dtFile.Rows.Count > 0)
            {
                FolderTableView.DataSource = dtFile;
                FolderTableView.DataBind();
            }
        }

        // Trying to do download button
        protected void DownloadFile(object sender, EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)lnk.NamingContainer;
            // this is for the auto method...however got error atm 
            // May not use this method cause its by ID.
            // May use another attribute for this.
            // string stringID = FileTableView.DataKeys[gr.RowIndex].Value.ToString();
            // int id = int.Parse(stringID);
            Download(3);
        }


        private void Download(int id)
        {
            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT * FROM mb_file where fileid=@ID", con);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                cmd.Prepare();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
            string name = dt.Rows[0]["filename"].ToString();
            byte[] documentBytes = (byte[])dt.Rows[0]["filecontent"];
            Response.ClearContent();
            Response.ContentType = "application/octestream";
            Response.AddHeader("Content-Disposition", string.Format("attachment;filename=(0)", name));
            Response.AddHeader("Content-Length", documentBytes.Length.ToString());

            Response.BinaryWrite(documentBytes);
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
                            FillData();
                        }
                    }
                    catch
                    {
                        Label1.ForeColor = System.Drawing.Color.Red;
                        Label1.Text = "Upload was not succesful, please try again.";
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
                        bool uploadfiletofolderstatus = MBFolder.UploadFileToFolder(foldername, file);
                        if (uploadfiletofolderstatus == true)
                        {
                            FillData();
                        }
                    }
                    catch
                    {
                        Label1.ForeColor = System.Drawing.Color.Red;
                        Label1.Text = "Upload was not succesful, please try again.";
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
                folder.saltfunction = MBFolder.GenerateSaltFunction();
                folder.folderencryption = 1;
                folder.folderPass = MBFolder.GenerateHashPassword(encryptionPass.Text, folder.saltfunction);
                folderCreation = MBFolder.CreateNewFolderWithPassword(folder);
            }
            else
            {
                MBFolder folder = new MBFolder();
                folder.folderName = FolderName.Text;
                folder.folderuserName = Context.User.Identity.Name;
                folder.folderencryption = 0;
                folder.saltfunction = null;
                folder.folderPass = null;
                folderCreation = MBFolder.CreateNewFolder(folder);

            }
            if (folderCreation == true)
            {
                Label1.Text = "Successs";
            }
            else
            {
                Label1.Text = "fail";
            }
            // Reset the form fields
            Response.Redirect(Request.RawUrl);
            /*
            FolderName.Text = "";
            encryptionOption.SelectedValue = "yes";
            encryptionPass.Text = "";
            encryptionPassCfm.Text = "";
            */
        }
    }
}