using System;
using System.Collections.Generic;
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
            if (!IsPostBack)
            {
                string[] months = new string[] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                Random rnd = new Random();
                for (int i = 0; i < 12; i++)
                {
                    DataChart.Series["Date"].Points.AddXY(months[i], rnd.Next(1, 100));
                }
            }
        }

    }
}