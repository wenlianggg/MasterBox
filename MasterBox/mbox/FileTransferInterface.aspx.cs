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
        protected void DownloadFile(object sender,EventArgs e)
        {
            LinkButton lnk = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)lnk.NamingContainer;

            int id = int.Parse(FileTableView.DataKeys[gr.RowIndex].Value.ToString());
        }
        private void Download(int id)
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
                    
                    cmd.CommandText = "INSERT INTO mb_testfolder(fileindex,filename,filetype,filesize)values(@Index,@Name,@Type,@data)";
                    cmd.Parameters.AddWithValue("@Index", 1);
                    cmd.Parameters.AddWithValue("@Name", filename);
                    cmd.Parameters.AddWithValue("@Type", filetype);
                    cmd.Parameters.AddWithValue("@data", filesize);
                    cmd.Connection = con;
                    con.Open();
                    cmd.ExecuteNonQuery();
                    con.Close();
                    UploadStatus.ForeColor = System.Drawing.Color.Green;
                    UploadStatus.Text = "Success";
                }
                catch
                {
                    UploadStatus.ForeColor = System.Drawing.Color.Red;
                    UploadStatus.Text = "Fail";
                }
              
            }
            else
            {
                UploadStatus.ForeColor = System.Drawing.Color.Red;
                UploadStatus.Text = "Please Select a file";
            }
        }

    
    }
}