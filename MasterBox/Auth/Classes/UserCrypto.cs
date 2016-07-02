using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;

namespace MasterBox.Auth {
	public static class UserCrypto {

		private static byte[] globalKey = Encoding.UTF8.GetBytes("y7T5YqUgwqOuxhy255s7Ad4XrXt6W768");

		public static byte[] Encrypt(string plainText, string iv) {
			// Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
			// so that the same Salt and IV values can be used when decrypting.  
			var ivBytes = Encoding.UTF8.GetBytes(iv);
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			using (AesManaged aes = new AesManaged()) {
				aes.BlockSize = 128;
				aes.KeySize = 256;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;
				using (var encryptor = aes.CreateEncryptor(globalKey, ivBytes))
				using (var memoryStream = new MemoryStream())
				using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write)) {
						cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
						cryptoStream.FlushFinalBlock();
						// Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
						var cipherBytes = memoryStream.ToArray();
						memoryStream.Close();
						cryptoStream.Close();
						return cipherBytes;
				}
			}
		}

		public static string Decrypt(byte[] cipherBytes, string iv) {
			byte[] ivBytes = Encoding.UTF8.GetBytes(iv);
			using (AesManaged aes = new AesManaged()) {
				aes.BlockSize = 128;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;
				using (var decryptor = aes.CreateDecryptor(globalKey, ivBytes))
				using (var memoryStream = new MemoryStream(cipherBytes))
				using (var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read)) {
					var plainTextBytes = new byte[cipherBytes.Length];
					var decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
					memoryStream.Close();
					cryptoStream.Close();
					return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
				}
			}
		}

		//
		// Utilities for encryption and decryption
		//
		public static string GenerateEntropy(int length) {
			var randomBytes = new byte[length]; // 32 Bytes will give us 256 bits.
			using (var rngCsp = new RNGCryptoServiceProvider()) {
				// Fill the array with cryptographically secure random bytes.
				rngCsp.GetBytes(randomBytes);
			}
			return Encoding.UTF8.GetString(randomBytes);
		}

	}
}