using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using System.Net;

/// Author: Goh Wen Liang (154473G) 

namespace MasterBox.Auth {

	public class reCAPTCHA {
		[JsonProperty("success")]
		private string _Success;
		public string Success {
			get {
				return _Success;
			}
			set {
				_Success = value;
			}
		}

		[JsonProperty("error-codes")]
		private List<string> _ErrorCodes;
		public List<string> ErrorCodes {
			get {
				return _ErrorCodes;
			}
			set {
				_ErrorCodes = value;
			}
		}

		public static bool Validate(string EncodedResponse) {
			WebClient wc = new WebClient();
			var googleReply = wc.DownloadString(
								string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}",
								"6Ld6kiETAAAAAJznhW6zGmj9BSgNgxDxkB2sw2Ml",
								EncodedResponse
								));
			var captchaResponse = JsonConvert.DeserializeObject<reCAPTCHA>(googleReply);
			System.Diagnostics.Debug.WriteLine(captchaResponse.Success);
			return captchaResponse.Success.Contains("true");
		}
	}
}