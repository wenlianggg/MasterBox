﻿using MasterBox.Auth;
using MasterBox.Auth.TOTP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace MasterBox.Prefs {
    public partial class prefotpsetup : System.Web.UI.Page {
        protected void Page_Load(object sender, EventArgs e) {
            if (MBProvider.Instance.IsTotpEnabled(Context.User.Identity.Name))
                ExistingOTP.Visible = true;
            OTPTool ot = new OTPTool();
            ViewState["TOTPKey"] = ot.generateSecret();
            OTPQrCode.ImageUrl = ot.QRCodeUrl;
            string secret = ot.SecretBase32;
            string spacedsecret = secret.Substring(0, 4) + "&nbsp";
            spacedsecret += secret.Substring(4, 4) + "&nbsp";
            spacedsecret += secret.Substring(8, 4) + "&nbsp";
            spacedsecret += secret.Substring(12, 4);
            GeneratedSecret.Text = spacedsecret;
        }

        protected void VerifyOTP_Button(object sender, EventArgs e) {
            // TODO: Logging OTP and password checks
            int input;
            OTPTool ot = (OTPTool)ViewState["TOTPKey"];
            if (MBProvider.Instance.ValidateUser(Context.User.Identity.Name, CurrPw.Text)) {
                if (int.TryParse(OTPVal.Text, out input)) {
                    if (OTPCheck(input, ot)) {
                        MBProvider.Instance.SetTotpSecret(Context.User.Identity.Name, ot.SecretBase32);
                        Msg.ForeColor = System.Drawing.Color.Green;
                        Msg.Text = "2nd Factor Authentication has been enabled";
                    } else {
                        Msg.Text = "Invalid OTP Entered, please try again.";
                    }
                }
            } else {
                Msg.Text = "Current password is incorrect.";
            }
        }

        protected void DisableTOTP_Button(object sender, EventArgs e) {
            // TODO: Logging OTP and password checks
            if (MBProvider.Instance.ValidateUser(Context.User.Identity.Name, CurrPw.Text)) {
                Msg.Text = "2nd Factor Authentication has been disabled";
            } else {
                Msg.Text = "Current password is incorrect";
            }
        }

        protected bool OTPCheck(int otpentered, OTPTool ot) {
            int[] otprange = ot.OneTimePasswordRange;
            foreach (int i in otprange) {
                if (otpentered == i) 
                    return true;
            }
            return false;
        }


    }
}