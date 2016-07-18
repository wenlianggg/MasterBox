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

/// Author: Goh Wen Liang (154473G) 

namespace MasterBox.Auth {
	
	/// <summary>
	/// UserCrypto is used to encrypt and decrypt a user field with a given initialization vector. 
	/// </summary>
	public class UserCrypto : IDisposable, IEquatable<UserCrypto> {

		/// <summary> Global decryption key value </summary>
		private byte[] _globalKey;
		/// <summary> Per-user initialization vector </summary>
		private byte[] _initVector;

		/// <summary> Creates a UserCrypto instance. </summary>
		/// <param name="initVector"> Per-user initialization vector </param>
		internal UserCrypto(string initVector) {
			_globalKey = Encoding.UTF8.GetBytes(ConfigurationManager.AppSettings["GlobalUserCryptKey"]);
			_initVector = Encoding.UTF8.GetBytes(initVector);
		}

		~ UserCrypto() {
			Dispose();
		}

		public void Dispose() {
			Dispose(true);
		}

		protected virtual void Dispose(bool disposing) {
			_globalKey = null;
		}

		public bool Equals(UserCrypto uc) {
			if (_initVector.Equals(uc._initVector))
				return true;
			else
				return false;
		}

		public byte[] Encrypt(string plainText) {
			// Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
			// so that the same Salt and IV values can be used when decrypting.  
			var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
			using (AesManaged aes = new AesManaged()) {
				aes.BlockSize = 128;
				aes.KeySize = 256;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;
				using (var encryptor = aes.CreateEncryptor(_globalKey, _initVector))
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

		public string Decrypt(byte[] cipherBytes) {
			using (AesManaged aes = new AesManaged()) {
				aes.BlockSize = 128;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;
				using (var decryptor = aes.CreateDecryptor(_globalKey, _initVector))
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

        public string CalculateHash(string plaintext, byte[] salt = null) {
            // Obtain base variables
            byte[] ptBytes = Encoding.UTF8.GetBytes(plaintext);
            byte[] combinedBytes;
            string newHash;

            // Calculate length of combinedbytes
            if (salt == null) {
                combinedBytes = new byte[ptBytes.Length];
                ptBytes.CopyTo(combinedBytes, 0);
            } else {
                combinedBytes = new byte[ptBytes.Length + salt.Length];
                ptBytes.CopyTo(combinedBytes, 0);
                salt.CopyTo(combinedBytes, ptBytes.Length);
            }

			using (SHA512 shaCalc = new SHA512Managed()) {
				newHash = Convert.ToBase64String(shaCalc.ComputeHash(combinedBytes));
			}

			return newHash;

        }

		//
		// Utilities for encryption and decryption
		//
		public static string GenerateEntropy(int length) {
			const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			StringBuilder res = new StringBuilder();
			using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) {
				byte[] uintBuffer = new byte[sizeof(uint)];
				while (length -- > 0) {
					rng.GetBytes(uintBuffer);
					uint num = BitConverter.ToUInt32(uintBuffer, 0);
					res.Append(valid[(int)(num % (uint)valid.Length)]);
				}
			}
			return res.ToString();
		}


	}
}