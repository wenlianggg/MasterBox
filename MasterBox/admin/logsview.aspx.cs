using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Admin {
	public partial class LogsView : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			DataTable dt = Logger.GetAllLogs();
			LogsGridView.DataSource = dt;
			LogsGridView.DataBind();
			LogsGridView.HeaderRow.TableSection = TableRowSection.TableHeader;
		}

		protected void LogsGridView_DataBound(object sender, GridViewRowEventArgs e) {
			if (e.Row.RowType == DataControlRowType.DataRow) {
				string severity = e.Row.Cells[4].Text;
				foreach (TableCell cell in e.Row.Cells) {
					switch (severity) {
						case "Normal":
							cell.BackColor = Color.Azure;
							break;
						case "Access":
							cell.BackColor = Color.LightYellow;
							break;
						case "Change":
							cell.BackColor = Color.Plum;
							break;
						case "ClientError":
							cell.BackColor = Color.DodgerBlue;
							break;
						case "ServerError":
							cell.BackColor = Color.SteelBlue;
							break;
						default:
							cell.BackColor = Color.LavenderBlush;
							break;
					}
				}
			}
		}
	}
}