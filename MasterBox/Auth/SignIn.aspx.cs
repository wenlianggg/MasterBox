
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using System.Diagnostics;

namespace MasterBox.Auth {
	public partial class SignIn : Page { 
		protected void Page_Load(object sender, EventArgs e) {
            try {
                if (User.Identity.IsAuthenticated) {
                    Response.Redirect("~/Default.aspx");
                } else if (!string.IsNullOrEmpty(Request.QueryString["vericode"])) {
                    string vericode = Request.QueryString["vericode"];
                    string username = Request.QueryString["username"];
                    bool valresult = MBProvider.Instance.ValidateVericode(username, vericode);
                    User currentuser = Auth.User.GetUser(username);
                    if (currentuser.IsVerified == false && valresult == true) {
                        currentuser.IsVerified = true;
                        Msg.ForeColor = System.Drawing.Color.Green;
                        Msg.Text = "Your email has been verified successfully.";
                    } else if (currentuser.IsVerified == true) {
                        Msg.Text = "Your email has already been verified";
                    } else if (valresult == false) {
                        Msg.Text = "Your validation code is not valid";
                    }
                }
            } catch (Exception) {
                Msg.Text = "An unknown error has occured";
            }
		}
		protected void logonClick(object sender, EventArgs e) {
			if (IPBlock.Instance.CheckUser(UserName.Text) != null) {
				Msg.Text = IPBlock.Instance.CheckUser(UserName.Text);
				return;
			}
			MBProvider mbp = MBProvider.Instance;
			try {
                if (mbp.ValidateUser(UserName.Text, UserPass.Text)) {
                    User usr = Auth.User.GetUser(UserName.Text);
                    Session["UserEntity"] = usr;
                    Session["IsPasswordAuthorized"] = true;
                    Session["StayLoggedIn"] = Persist.Checked;
                    if (!usr.IsVerified) {
                        Msg.Text = "Your email address has not been verified!"; 
                    } else if (MBProvider.Instance.IsTotpEnabled(usr.UserName)) {
                        if (RequestedUrl != null) {
                            // Keep requested url
                            Response.Redirect("~/Auth/otpverify.aspx?ReturnUrl=" + RequestedUrl, false);
                        } else {
                            Response.Redirect("~/Auth/otpverify.aspx", false);
                        }
                    } else {
                        MBProvider.Instance.LoginSuccess(usr, Persist.Checked);
                    }
				} else {
					Msg.Text = "Invalid credentials, please try again!";
				}
			} catch (UserNotFoundException) {
				Msg.Text = "Invalid credentials, please try again!";
			} catch (InvalidTOTPLength) {
				Msg.Text = "Error while loading OTP info, contact us.";
			} catch (Exception) {
                Msg.Text = "Unknown error has occured, please contact us";
            }
		}

		protected void Registration_Start(object sender, EventArgs e) {
			if (Auth.User.Exists(UserName.Text)) {
				Msg.Text = "User already exists!";
			} else {
				Debug.WriteLine(ResolveUrl("~/auth/signup") + "?username=" + UserName.Text);
				Response.Redirect("~/auth/signup.aspx" + "?username=" + UserName.Text);
			}
		}

		private string RequestedUrl {
			get {
				return Request.QueryString["ReturnUrl"];
			}
		}
	}

}