﻿using MasterBox.Auth;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace MasterBox.Prefs {
	public partial class Steg : Page {
		protected void Page_Load(object sender, EventArgs e) {
			if (HasImageKey()) {
				HasExisting.Visible = true;
                System.Diagnostics.Debug.WriteLine(ViewState["Steg"] == null);
				if (ViewState["Steg"] != null) {
					DownloadFromSteg.Visible = true;
				} else {
					DownloadFromSteg.Visible = false;
				}
			} else {
				HasExisting.Visible = false;
			}
		}

		protected void UploadDoSteg(object sender, EventArgs e) {
            try {
                UploadFile();
            } catch (Exception e1) {
                Msg.Text = e1.Message;
            }
			if (ViewState["Steg"] != null) {
				Stegano steg = ViewState["Steg"] as Stegano;
				HashMsg1.Text = "Raw hash: " + steg.BitmapGetHash();
				string resulthash = steg.JumblePixels();
				HashMsg2.Text = "New hash: " + resulthash;
			}
		}

		protected void DownloadImage(object sender, EventArgs e) {
			DownloadImage();
		}

		protected void UploadDoStegSetKey(object sender, EventArgs e) {
			UploadFile();
			if (ViewState["Steg"] != null) {
				Stegano steg = ViewState["Steg"] as Stegano;
				HashMsg1.Text = "Raw hash: " + steg.BitmapGetHash();
				string resulthash = steg.JumblePixels();
				HashMsg2.Text = "New hash: " + resulthash;
				steg.SetForUser(Auth.User.ConvertToId(Context.User.Identity.Name));
			}
		}

		protected void SubmitValidateImg(object sender, EventArgs e) {
			UploadFile();
			if (ViewState["Steg"] != null) {
				Stegano steg = ViewState["Steg"] as Stegano;
				if (steg.ValidateHash(Auth.User.ConvertToId(Context.User.Identity.Name))) {
					Msg.ForeColor = Color.Green;
					Msg.Text = "Validation successful";
					HashMsg1.Text = "Hash: " + steg.BitmapGetHash();
				} else {
					Msg.ForeColor = Color.Red;
					Msg.Text = "Validation unsuccessful";
					HashMsg1.Text = "Hash: " + steg.BitmapGetHash();
				}
			}
		}

		protected void DisableImageKey(object sender, EventArgs e) {
			using (DataAccess da = new DataAccess()) {
				da.SqlSetImageHash(Auth.User.ConvertToId(Context.User.Identity.Name), null);
			}
		}

		protected bool HasImageKey() {
			using (DataAccess da = new DataAccess()) {
				return (da.SqlGetImageHash(Auth.User.ConvertToId(Context.User.Identity.Name)) != null);
			}
		}

		protected void UploadFile() {
			try {
				if (UploadValidate() && UploadControl.HasFile) {
					Stegano steg = new Stegano(UploadControl.FileContent, UploadControl.PostedFile.ContentType);
                    System.Diagnostics.Debug.WriteLine(steg == null);
                    string filename = Path.GetFileName(UploadControl.FileName);
					string hashvalue = steg.BitmapGetHash();
					ViewState["filename"] = filename;
					ViewState["Steg"] = steg;
                    if (ViewState["Steg"] == null) {
                        throw new Exception("Something went wrong, please try again/another image");
                    }
				}
			} catch (Exception ex) {
				Msg.Text = "An error has occured > " + ex.Message;
			}
		}


		protected bool UploadValidate() {
			if (UploadControl.HasFile) {
				if (UploadControl.PostedFile.ContentType == "image/jpeg" ||
					UploadControl.PostedFile.ContentType == "image/png" ||
					UploadControl.PostedFile.ContentType == "image/bmp" ||
					UploadControl.PostedFile.ContentType == "image/gif") {
					if (UploadControl.PostedFile.ContentLength < 10240000 &&
                        UploadControl.PostedFile.ContentLength > 512000) {
						return true;
					} else {
						Msg.ForeColor = Color.Red;
						Msg.Text = "Upload status: The file has to be less than 10MB and larger than 500KB!";
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

		protected void DownloadImage() {
			Stegano steg = ViewState["Steg"] as Stegano;
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

