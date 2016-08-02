using MasterBox.Auth;
using System;

namespace MasterBox.Prefs {
    public partial class FileSetting_Profile : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
			User CurrentUser = Auth.User.GetUser(Context.User.Identity.Name);
			ViewState["CurrUserId"] = CurrentUser.UserId;
			usernameTxt.Text = CurrentUser.UserName;
			usernameTxt.Enabled = false;
			emailTxt.Text = CurrentUser.Email;
			FNameTxt.Text = CurrentUser.FirstName;
			LNameTxt.Text = CurrentUser.LastName;
			DobTxt.Text = CurrentUser.Birthdate.ToShortDateString();
        }

		// TODO: Input sanitization
		protected void ProfileChange_Click(object sender, EventArgs e) {
			User CurrentUser = Auth.User.GetUser(Context.User.Identity.Name);
			try {
				if ((int)ViewState["CurrUserId"] == CurrentUser.UserId) {
					if (!emailTxt.Text.Equals(CurrentUser.Email))
						CurrentUser.Email = emailTxt.Text;
					if (!FNameTxt.Text.Equals(CurrentUser.FirstName))
						CurrentUser.FirstName = FNameTxt.Text;
					if (!LNameTxt.Text.Equals(CurrentUser.LastName))
						CurrentUser.LastName = LNameTxt.Text;
					if (!DobTxt.Text.Equals(CurrentUser.Birthdate.ToShortDateString()))
						CurrentUser.Birthdate = DateTime.Parse(DobTxt.Text);
				}
			} catch (FormatException) {
				Msg.Text = "Invalid date format input";
			} catch (Exception) {
				Msg.Text = "An error has occured";
			}
		}
	}
}