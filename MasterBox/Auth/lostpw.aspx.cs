using MasterBox.Admin;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Auth {
	public partial class PasswordReset : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {
			if (!IsPostBack) {
				Message.Visible = false;
			}
		}

		protected void ValidateSubmission_Click(object sender, EventArgs e) {
			SubmitValidateImg();
		}

		protected void SubmitValidateImg() {
			Stegano steg = UploadFile();
			try { 
				if (steg != null) {
					if (steg.ValidateHash(Auth.User.ConvertToId(UserName.Text))) {
						Message.Visible = true;
						Message.Attributes.Add("class", "alert alert-success");
						Mail mail = new Mail();
						mail.SendEmail("wenlianggg@gmail.com", "test", "test");
						Msg.Text = "Your new password has been sent to your email address.";
					} else {
						Message.Visible = true;
						Message.Attributes.Add("class", "alert alert-warning");
						Msg.Text = "Validation unsuccessful";
					}
				}
			} catch (UserNotFoundException) {
				Message.Visible = true;
				Message.Attributes.Add("class", "alert alert-warning");
				Msg.Text = "Validation unsuccessful";
			}
		}

		protected Stegano UploadFile() {
			try {
				if (UploadValidate() && UploadControl.HasFile) {
					Stegano steg = new Stegano(UploadControl.FileContent, UploadControl.PostedFile.ContentType);
					string filename = Path.GetFileName(UploadControl.FileName);
					string hashvalue = steg.BitmapGetHash();
					return steg;
				}
				return null;
			} catch (Exception ex) {
				Msg.Text = "An error has occured > " + ex.Message;
				return null;
			}
		}

		protected bool UploadValidate() {
			if (UploadControl.HasFile) {
				if (UploadControl.PostedFile.ContentType == "image/jpeg" ||
					UploadControl.PostedFile.ContentType == "image/png" ||
					UploadControl.PostedFile.ContentType == "image/bmp" ||
					UploadControl.PostedFile.ContentType == "image/gif") {
					if (UploadControl.PostedFile.ContentLength < 10240000) {
						return true;
					} else {
						Msg.ForeColor = Color.Red;
						Msg.Text = "Upload status: The file has to be less than 10MB!";
						return false;
					}
				} else {
					Msg.ForeColor = Color.Red;
					Msg.Text = "Upload status: Unsupported file format > " + UploadControl.PostedFile.ContentType;
					return false;
				}
			} else {
				Msg.ForeColor = Color.Red;
				Msg.Text = "Upload status: No file uploaded";
				return false;
			}
		}
	}
}