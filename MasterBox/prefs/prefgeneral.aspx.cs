using MasterBox.mbox;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Prefs
{
    public partial class FileSetting_General : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            double megabytes = (MBFile.GetTotalFileStorage(Context.User.Identity.Name)/1024f)/ 1024f;
            LblDataTrackerFileSize.Text = "Total file storage used: " + Math.Round(megabytes,3) +" MB";
            LblDataTrackerFileNum.Text = "Total number of files uploaded: " + MBFile.RetrieveAllUserFiles(Context.User.Identity.Name).Count.ToString();
            LblDataTrackerFolderNum.Text = "Total number of folders created: " + ((MBFolder.GenerateFolderLocation(Context.User.Identity.Name).Count)-1).ToString();
            ChartLoad();
        }
        private void ChartLoad()
        {
            List<MBFile> filelist = MBFile.RetrieveAllUserFiles(Context.User.Identity.Name);
            for(int i = 0; i < filelist.Count; i++)
            {
                DataChart.Series["Data"].Points.AddXY(filelist[i].filetimestamp.Date, filelist[i].fileSize);
            }
        }

    }
}