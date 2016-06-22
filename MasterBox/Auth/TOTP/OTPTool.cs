using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
using System.ComponentModel;
using System.Timers;

namespace MasterBox.Auth.TOTP {
	public class OTPTool {
		private int _secondsToGo;
		private string _identity;
		private byte[] _secret;
		private Int64 _timestamp;
		private byte[] _hmac;
		private int _offset;
		private int _OTPNow;
		private int[] _OTPRange = new int[5];


		public int SecondsToGo {
			get {
				return _secondsToGo;
			}
			private set {
				_secondsToGo = value;
				if (SecondsToGo == 30) CalculateCurrentOTP();
			}
		}


		public string Identity {
			get {
				return _identity;
			}
			set {
				_identity = value;
				CalculateCurrentOTP();
			}
		}


		public string SecretBase32 {
			get {
				return Base32.ToString(Secret);
				}
			set {
				try { 
				Secret = Base32.ToBytes(value);
					} catch {};
				}
		}


		public byte[] Secret {
			get { return _secret; }
			set {
				_secret = value;
				CalculateCurrentOTP();
			}
		}

		public string QRCodeUrl {
			get { return GetQRCodeUrl(); }
		}


		public Int64 Timestamp {
			get { return _timestamp; }
			private set {
				_timestamp = value;
			}
		}


		public byte[] Hmac {
			get { return _hmac;}
			private set { _hmac = value; }
		}


		public byte[] HmacPart1 {
			get { return _hmac.Take(Offset).ToArray(); }
		}

		public byte[] HmacPart2 {
			get { return _hmac.Skip(Offset).Take(4).ToArray(); }
		}

		public byte[] HmacPart3 {
			get { return _hmac.Skip(Offset + 4).ToArray();}
		}


		public int Offset {
			get { return _offset; }
			private set { _offset = value; }
		}


		public int OneTimePassword {
			get {
				CalculateCurrentOTP();
				return _OTPNow;
			}
		}

		public int[] OneTimePasswordRange {
			get {
				LenientCalculateOTP();
				return _OTPRange;
			}
		}

		public string generateSecret() {
			using (RNGCryptoServiceProvider cryptrng = new RNGCryptoServiceProvider()) {
				cryptrng.GetBytes(Secret);
			}
			return SecretBase32;
		}

		private string GetQRCodeUrl() {
			return String.Format("https://www.google.com/chart?chs=200x200&chld=M|0&cht=qr&chl=otpauth://totp/{0}%3Fsecret%3D{1}",
								Identity,
								SecretBase32);
		}


		// ONE MINUTE BEFORE AND ONE MINUTE AFTER
		private void LenientCalculateOTP() {
			// https://tools.ietf.org/html/rfc4226
			Array.Clear(_OTPRange, 0, _OTPRange.Length);
			Timestamp = Convert.ToInt64(GetUnixTimestamp() / 30) - 2;
			for (int i = 0; i < 5; i++) {
				var data = BitConverter.GetBytes(Timestamp + i).Reverse().ToArray();
				Hmac = new HMACSHA1(Secret).ComputeHash(data);
				Offset = Hmac.Last() & 0x0F;
				_OTPRange[i] = (
					((Hmac[Offset + 0] & 0x7f) << 24) |
					((Hmac[Offset + 1] & 0xff) << 16) |
					((Hmac[Offset + 2] & 0xff) << 8) |
					(Hmac[Offset + 3] & 0xff)
					) % 1000000;
			}
		}

		private void CalculateCurrentOTP() {
			// https://tools.ietf.org/html/rfc4226
			Timestamp = Convert.ToInt64(GetUnixTimestamp() / 30);
			var data = BitConverter.GetBytes(Timestamp).Reverse().ToArray();
			Hmac = new HMACSHA1(Secret).ComputeHash(data);
			Offset = Hmac.Last() & 0x0F;
			_OTPNow = (
				((Hmac[Offset + 0] & 0x7f) << 24) |
				((Hmac[Offset + 1] & 0xff) << 16) |
				((Hmac[Offset + 2] & 0xff) << 8) |
				(Hmac[Offset + 3] & 0xff)
				) % 1000000;
		}

		private static Int64 GetUnixTimestamp() {
			return Convert.ToInt64(Math.Round((DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0)).TotalSeconds));
		}
	}
}