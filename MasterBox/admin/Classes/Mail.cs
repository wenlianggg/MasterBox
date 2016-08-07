using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
//using MailMessage=System.Net.Mail.MailMessage;

namespace MasterBox.Admin {
	public class Mail {

		// Getter and setters (fields)

        string MailSmtpHost { get; set; }
        int MailSmtpPort { get; set; }
        string MailSmtpUsername { get; set; }
        string MailSmtpPassword { get; set; }
        string MailFrom { get; set; }

        //internal string From { get; set; }
		//internal string To { get; set; }
		//internal string Subject { get; set; } 
		//internal string Body { get; set; }

		// Constructors, creates mail object
        /*
		internal Mail(string to, string subject, string body) {
			From = "masterboxnoreply@gmail.com";
			To = to;
			Subject = subject;
			Body = body;
			// Start email server connection or whatnot
		}
        */

        public bool SendEmail(string to, string subject, string body)
        {
            MailMessage mail = new MailMessage(MailFrom, to, subject, body);
            var alternameView = AlternateView.CreateAlternateViewFromString(body, new ContentType("text/html"));
            mail.AlternateViews.Add(alternameView);

            var smtpClient = new SmtpClient(MailSmtpHost, MailSmtpPort);
            smtpClient.Credentials = new NetworkCredential(MailSmtpUsername, MailSmtpPassword);
            try
            {
                smtpClient.Send(mail);
            }
            catch (Exception e)
            {
                //Log errors here
                return false;
            }

            return true;
        }

        /*
        // Methods for mail class
        internal bool Send() {
			// Sends the mail

			// Returns true if sent
			return false;
		}
        */
		// ... any other methods
	}
}