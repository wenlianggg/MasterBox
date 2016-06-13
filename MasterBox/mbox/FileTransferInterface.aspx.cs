using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.SqlClient;
using System.IO;
namespace MasterBox
{
    public partial class FileTransferInterface : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
           
        }

        protected void NewUploadFile_Click(object sender, EventArgs e)
        {
            
            if (FileUpload.HasFile)
            {
                try
                {
                    string filename = Path.GetFileName(FileUpload.FileName);
                    Stream strm = FileUpload.PostedFile.InputStream;
                    BinaryReader br = new BinaryReader(strm);
                    Byte[] filesize = br.ReadBytes((int)strm.Length);
                    string filetype = FileUpload.PostedFile.ContentType;
                    SqlConnection con = new SqlConnection("Data source=.;Initial Catalog=uploadFiles;Integrated Security=true");
                    SqlCommand cmd = new SqlCommand();
                    cmd.CommandText = "insert into tablename(Name,type,data)values(@Name,@Type,@data)";
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