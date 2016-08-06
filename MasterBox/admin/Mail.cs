using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MasterBox.Admin {
	public class Mail {

		// Getter and setters (fields)

		internal string Sender { get; set; }
		internal string Recipient { get; set; }
		internal string Title { get; set; } 
		internal string Body { get; set; }

		// Constructors, creates mail object

		internal Mail(string recipient, string title, string body) {
			Sender = "jefferyballstester...";
			Recipient = recipient;
			Title = title;
			Body = body;
			// Start email server connection or whatnot
		}

		// Methods for mail class
		internal bool Send() {
			// Sends the mail

			// Returns true if sent
			return false;
		}

		// ... any other methods
	}
}