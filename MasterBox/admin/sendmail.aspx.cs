using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Net.Mail;

namespace MasterBox.Admin {
	public partial class SendMail : System.Web.UI.Page {
		protected void Page_Load(object sender, EventArgs e) {

		}

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("I AM CLICKED");
            MailMessage Msg = new MailMessage();
            Msg.From = new MailAddress("masterboxnoreply@gmail.com");
            Msg.To.Add(txtToMail.Text);
            Msg.Subject = txtSubject.Text + "  ( From: MasterBox ) ";
            //Msg.Subject = txtSubject.Text;
            Msg.Body = txtMessage.Text;
            Msg.IsBodyHtml = true;

            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            System.Net.NetworkCredential NetworkCred = new System.Net.NetworkCredential();
            NetworkCred.UserName = "masterboxnoreply@gmail.com";
            NetworkCred.Password = "N0tasmurf!";
            smtp.UseDefaultCredentials = true;
            smtp.Credentials = NetworkCred;

            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Send(Msg);
            lblMsg.Text = "Email has been successfully sent";
            //lblMsg.Text = "Email has been successfully sent..!!";
        }
    }
}