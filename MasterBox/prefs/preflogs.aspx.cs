using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Drawing;
using MasterBox.Auth;

namespace MasterBox.Prefs {
    public partial class AccessLog : Page {

        protected void Page_Load(object sender, EventArgs e) {

        }
        protected void RefreshAuthTable(object sender, EventArgs e) {
            LogTypeName.Text = "Access Logs";
            LogTable.DataSource = AuthLogger.Instance.GetUserLogs(Auth.User.ConvertToId(User.Identity.Name));
            LogTable.DataBind();
        }

        protected void RefreshFilesTable(object sender, EventArgs e) {
            LogTypeName.Text = "File Logs";
            LogTable.DataSource = FileLogger.Instance.GetUserLogs(Auth.User.ConvertToId(User.Identity.Name));
            LogTable.DataBind();
        }

        protected void RefreshTransactTable(object sender, EventArgs e) {
            LogTypeName.Text = "Transaction Logs";
            LogTable.DataSource = TransactLogger.Instance.GetUserLogs(Auth.User.ConvertToId(User.Identity.Name));
            LogTable.DataBind();
        }


        protected void LogsRowDataBound(object sender, GridViewRowEventArgs e) {
            if (e.Row.RowType == DataControlRowType.DataRow) {
                string severity = e.Row.Cells[2].Text;
                foreach (TableCell cell in e.Row.Cells) {
                    switch(severity) {
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