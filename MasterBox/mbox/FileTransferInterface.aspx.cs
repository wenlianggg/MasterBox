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

namespace MasterBox
{
    public partial class FileTransferInterface : System.Web.UI.Page
    {
        
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
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString)){
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
            Response.AddHeader("Content-Disposition",string.Format("attachment;filename=(0)",name));
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
                    SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["MBoxCString"].ConnectionString);
                    SqlCommand cmd = new SqlCommand();

                    // Upload to database
                    // Tempo for the id, must manual key in
                    cmd.CommandText = "INSERT INTO mb_testfolder(fileindex,filename,filetype,filesize)values(@Index,@Name,@Type,@data)";
                    cmd.Parameters.AddWithValue("@Index", 3);
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

        protected void CreateNewFolder_Click(object sender, EventArgs e)
        {
            // reset the form fields
            Response.Redirect(Request.Url.AbsoluteUri);
        }

        protected void PasswordValidator_ServerValidate(object source, ServerValidateEventArgs args)
        {
            if (encryptionOption.Text == "no")
            {
                PassValidator.Enabled = false;
            }else
            {
                PassValidator.Enabled = true;
            }
        }

    }
}