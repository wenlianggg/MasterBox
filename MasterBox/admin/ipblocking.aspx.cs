using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Admin {
	public partial class IPBlocking : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {

		}

		protected void USubmitBtn_Click(object sender, EventArgs e) {
			string username = UUserTxt.Text;
			TimeSpan duration;
			bool isDurationValid = TimeSpan.TryParse(UDurationTxt.Text, out duration);
		}

		protected void IPSubmitBtn_Click(object sender, EventArgs e) {
			string ipaddress = IPAddrTxt.Text;
			TimeSpan duration;
			bool isDurationValid = TimeSpan.TryParse(UDurationTxt.Text, out duration);
		
		}

		protected void IPUSubmitBtn_Click(object sender, EventArgs e) {
			string username = UUserTxt.Text;
			string ipaddress = IPUIPTxt.Text;
			TimeSpan duration;
			bool isDurationValid = TimeSpan.TryParse(UDurationTxt.Text, out duration);
		}
	}
}