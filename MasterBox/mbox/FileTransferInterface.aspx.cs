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
        protected void testButton_Click(object sender,EventArgs e)
        {
            Label1.Text = "Success";
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
                    SqlConnection con = new SqlConnection("Data Source=mbox-mssql.c2apojdl5mfi.ap-southeast-1.rds.amazonaws.com;Initial Catalog=MasterBox;Persist Security Info=True;User ID=masterboxadmin;Password=N0tadatabase!");
                    SqlCommand cmd = new SqlCommand();
                    
                    cmd.CommandText = "INSERT into dbo.mb_testfolder(filename,filetype,filesize)values(@Name,@Type,@data)";
                    cmd.Parameters.AddWithValue("@Name", filename);
                    cmd.Parameters.AddWithValue("@Type", filetype);
                    cmd.Parameters.AddWithValue("@data", filesize);
                    cmd.Connection = con;
                    con.Open();
                    if (con.State != ConnectionState.Open)
                    {
                        System.Windows.Forms.MessageBox.Show("Test");
                    }
                    cmd.ExecuteNonQuery();
                    con.Close();
                    Label1.ForeColor = System.Drawing.Color.Green;
                    Label1.Text = "Success";
                }
                catch
                {
                    Label1.ForeColor = System.Drawing.Color.Red;
                    Label1.Text = "Fail";
                }
              
            }
            else
            {
                Label1.ForeColor = System.Drawing.Color.Red;
                Label1.Text = "Please Select a file";
            }
        }
    }
}