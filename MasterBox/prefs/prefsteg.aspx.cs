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
			if (!IsPostBack) {
				Msg.Text = "";
				HashMsg.Text = "";
			}
		}

		protected void SubmitFile(object sender, EventArgs e) {
			try {
				if (UploadValidate()) {
					Stegano steg = new Stegano(UploadControl.FileContent, UploadControl.PostedFile.ContentType);
					string filename = Path.GetFileName(UploadControl.FileName);
					string hashvalue = steg.BitmapGetHash();
					ViewState["filename"] = filename;
					HashMsg.Text = hashvalue;
					Msg.ForeColor = Color.Green;
					Msg.Text = "Upload status: File uploaded!";
					HashMsg.Text = hashvalue;
				}
			} catch (Exception ex) {
				Msg.Text = "An error has occured > " + ex.Message;
			}
		}

		protected void DownloadImage(object sender, EventArgs e) {
			if (ViewState["steg"] != null && ViewState["filename"] != null)
				DownloadImage();
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
						Msg.Text = "Upload status: The file has to be less than 1MB!";
						return false;
					}
				} else {
					Msg.Text = "Upload status: Unsupported file format > " + UploadControl.PostedFile.ContentType;
					return false;
				}
			} else {
				Msg.Text = "Upload status: No file uploaded";
				return false;
			}
		}

		protected void DownloadImage() {
			Stegano steg = ViewState["steg"] as Stegano;
			string filename = ViewState["filename"] as string;
			Response.ClearContent();
			Response.ContentType = UploadControl.PostedFile.ContentType;
			Response.AddHeader("Content-Disposition", "attachment;filename=\"" + filename + "\"");
			Response.AddHeader("Content-Length", steg.ImageData.ToArray().Length.ToString());
			Response.BinaryWrite(steg.ImageData.ToArray());
			Response.Flush();
			Response.Close();
		}
	}
}

