using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace MasterBox.Prefs {
	public partial class Steg : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {

		}

		protected void SubmitFile(object sender, EventArgs e) {
				try {
					if (UploadControl.HasFile) {
						if (UploadControl.PostedFile.ContentType == "image/jpeg" ||
							UploadControl.PostedFile.ContentType == "image/png" ||
							UploadControl.PostedFile.ContentType == "image/bmp" ||
							UploadControl.PostedFile.ContentType == "image/gif") {
							if (UploadControl.PostedFile.ContentLength < 1024000) {
								string filename = Path.GetFileName(UploadControl.FileName);
								Image img = Image.FromStream(UploadControl.FileContent);
								string hashvalue;
								Stegano.JumblePixels(img, out hashvalue);
								Msg.ForeColor = Color.Green;
								Msg.Text = "Upload status: File uploaded! " + hashvalue;
							} else
								Msg.Text = "Upload status: The file has to be less than 1MB!";
						} else
							Msg.Text = "Upload status: Unsupported file format > " + UploadControl.PostedFile.ContentType;
					}
				} catch (Exception ex) {
				Msg.Text = "An error has occured > " + ex.Message;
				}
			}
		}
	}

