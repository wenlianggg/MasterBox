using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Admin {
	public partial class IPBlocking : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			BlockDataGrid.DataSource = BlockListTable;
			BlockDataGrid.DataBind();
			BlockDataGrid.HeaderRow.TableSection = TableRowSection.TableHeader;

			if (!IsPostBack) {
				Message.Visible = false;
			}
		}

		protected void IPSubmitBtn_Click(object sender, EventArgs e) {
			string ipaddress = IPAddrTxt.Text;
			string reason = IPReasonTxt.Text;
			TimeSpan duration;
			bool isDurationValid = TimeSpan.TryParse(IPDurationTxt.Text, out duration);
			if (isDurationValid) {
				if (isValidIp(ipaddress)) {
					// All validations passed
					IPBlock.Instance.Create(ipaddress, duration, reason);
					Msg.Text = "Blocked " + ipaddress + " for " + duration + ". Reason: " + reason;
				} else {
					Message.Visible = true;
					Message.Attributes["class"] = "alert alert-warning";
					Msg.Text = "IP Address input invalid";
				}
			} else {
				Message.Visible = true;
				Message.Attributes["class"] = "alert alert-warning";
				Msg.Text = "Invalid time input";
			}
		}

		protected void USubmitBtn_Click(object sender, EventArgs e) {
			string username = UUserTxt.Text;
			string reason = UReasonTxt.Text;
			TimeSpan duration;
			bool isDurationValid = TimeSpan.TryParse(UDurationTxt.Text, out duration);
			if (isDurationValid) {
				if (Auth.User.Exists(username)) {
					// All validations passed
					int uid = Auth.User.ConvertToId(username);
					IPBlock.Instance.Create(uid, duration, reason);
					Message.Visible = true;
					Message.Attributes["class"] = "alert alert-success";
					Msg.Text = "Blocked " + username + " for " + duration + ". Reason: " + reason;
				} else {
					Message.Visible = true;
					Message.Attributes["class"] = "alert alert-warning";
					Msg.Text = "User does not exist";
				}
			} else {
				Message.Visible = true;
				Message.Attributes["class"] = "alert alert-warning";
				Msg.Text = "Invalid time input";
			}
		}



		protected void IPUSubmitBtn_Click(object sender, EventArgs e) {
			string username = IPUUserTxt.Text;
			string ipaddress = IPUIPTxt.Text;
			string reason = IPUReasonTxt.Text;
			TimeSpan duration;
			bool isDurationValid = TimeSpan.TryParse(IPUDurationTxt.Text, out duration);
			if (isDurationValid) {
				if (isValidIp(ipaddress)) {
					if (Auth.User.Exists(username)) {
						// All validations passed
						int uid = Auth.User.ConvertToId(username);
						IPBlock.Instance.Create(uid, ipaddress, duration, reason);
						Msg.Text = "Blocked " + username + " on " + ipaddress + " for " + duration + ". Reason: " + reason;
					} else {
						Message.Visible = true;
						Message.Attributes["class"] = "alert alert-warning";
						Msg.Text = "User does not exist";
					}
				} else {
					Message.Visible = true;
					Message.Attributes["class"] = "alert alert-warning";
					Msg.Text = "IP Address input invalid";
				}
			} else {
				Message.Visible = true;
				Message.Attributes["class"] = "alert alert-warning";
				Msg.Text = "Invalid time input";
			}
		}

		protected bool isValidIp(string ip) {
			IPAddress ipobj;
			return IPAddress.TryParse(ip, out ipobj);
		}

		private DataTable BlockListTable {
			get {
				DataTable dt = new DataTable();
				dt.Columns.Add(new DataColumn("Index", typeof(int)));
				dt.Columns.Add(new DataColumn("Username", typeof(string)));
				dt.Columns.Add(new DataColumn("IP Address", typeof(string)));
				dt.Columns.Add(new DataColumn("Expiry", typeof(DateTime)));
				dt.Columns.Add(new DataColumn("Reason", typeof(string)));
				using (DataAccess da = new DataAccess()) {
					int ctr = 0;
					foreach (IPBlockEntry ipbe in IPBlock.Instance.bList) {
						dt.Rows.Add(++ctr,
									Auth.User.ConvertToUserName(ipbe.UserID), 
									ipbe.IPAddress, 
									ipbe.Expiry, 
									ipbe.Reason);
					}
				}
				return dt;
			}
		}
	}
}