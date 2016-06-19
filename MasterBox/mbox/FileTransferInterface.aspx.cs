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
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
        protected void Page_Load(object sender, EventArgs e)
        {

            // fill up file data on the display
            if (!IsPostBack)
            {
                FillData();
            }
        }
        private void FillData()
        {

            DataTable dt = new DataTable();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString))
            {
                con.Open();
                SqlCommand cmd = new SqlCommand("SELECT filename FROM mb_testfolder", con);
                cmd.Prepare();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
            if (dt.Rows.Count > 0)
            {
                FileTableView.DataSource = dt;
                FileTableView.DataBind();
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
                SqlCommand cmd = new SqlCommand("SELECT * FROM mb_testfolder where fileindex=@ID", con);
                cmd.Parameters.Add("@ID", SqlDbType.Int).Value = id;
                cmd.Prepare();
                SqlDataReader reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
            string name = dt.Rows[0]["filename"].ToString();
            byte[] documentBytes = (byte[])dt.Rows[0]["filesize"];
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
                try
                {
                    // Get File Name
                    string filename = Path.GetFileName(FileUpload.FileName);
                    Stream strm = FileUpload.PostedFile.InputStream;
                    // Get File size 
                    BinaryReader br = new BinaryReader(strm);
                    Byte[] filesize = br.ReadBytes((int)strm.Length);
                    // Get File Type
                    string filetype = FileUpload.PostedFile.ContentType;

                    SqlCommand cmd = new SqlCommand();

                    // Upload to database
                    // Tempo for the id, must manual key in
                    cmd.CommandText = "INSERT INTO mb_testfolder(filename,filetype,filesize)values(@Name,@Type,@data)";
                    cmd.Parameters.AddWithValue("@Name", filename);
                    cmd.Parameters.AddWithValue("@Type", filetype);
                    cmd.Parameters.AddWithValue("@data", filesize);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();

                    // Update text to show status
                    UploadStatus.ForeColor = System.Drawing.Color.Green;
                    UploadStatus.Text = "Success";
                    // Update the file table
                    FillData();
                }
                catch
                {
                    UploadStatus.ForeColor = System.Drawing.Color.Red;
                    UploadStatus.Text = "Upload was not succesful, please try again.";
                }

            }
        }

        private static Folder mbfile = new Folder();
        protected void CreateNewFolder_Click(object sender, EventArgs e)
        {
            bool folderCreation;
            if (encryptionOption.Text == "yes")
            {
                Folder folder = new Folder();
                folder.folderName = FolderName.Text;
                folder.userName = Context.User.Identity.Name;
                folder.saltfunction = mbfile.GenerateSaltFunction();
                folder.folderencryption = 1;
                folder.folderPass = mbfile.GenerateHashPassword(Context.User.Identity.Name, encryptionPass.Text, folder.saltfunction);
                folderCreation = mbfile.CreateNewFolderWithPassword(folder);
            }
            else
            {
                Folder folder = new Folder();
                folder.folderName = FolderName.Text;
                folder.userName = Context.User.Identity.Name;
                folder.folderencryption = 0;
                folder.saltfunction = null;
                folder.folderPass = null;
                folderCreation = mbfile.CreateNewFolder(folder);

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
            FolderName.Text = "";
            encryptionOption.SelectedValue = "yes";
            encryptionPass.Text = "";
            encryptionPassCfm.Text = "";
        }



    }
}