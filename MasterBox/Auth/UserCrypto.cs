﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Web;

namespace MasterBox.Auth {
	public static class UserCrypto {

		private const string globalKey = "OElhTCCrIj1Wax8ITmeQQPZiBhcwBYgz";

		public static byte[] EncryptObject(object value, string iv) {
			

			return new byte[0];
		}

		public static object DecryptObject(byte[] encrypted, string iv) {

			return "";
		}

		//
		// Utilities for encryption and decryption
		//

		private static object BytesToObject(byte[] arrBytes) {
			MemoryStream memStream = new MemoryStream();
			BinaryFormatter binForm = new BinaryFormatter();
			memStream.Write(arrBytes, 0, arrBytes.Length);
			memStream.Seek(0, SeekOrigin.Begin);
			Object obj = (Object)binForm.Deserialize(memStream);

			return obj;
		}

		private static byte[] ObjectToBytes(object obj) {
			if (obj == null)
				return null;

			BinaryFormatter bf = new BinaryFormatter();
			MemoryStream ms = new MemoryStream();
			bf.Serialize(ms, obj);

			return ms.ToArray();
		}

		public static string GenKeyIv(int length) {
			const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			System.Text.StringBuilder res = new System.Text.StringBuilder();
			using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider()) {
				byte[] uintBuffer = new byte[sizeof(uint)];

				while (length-- > 0) {
					rng.GetBytes(uintBuffer);
					uint num = BitConverter.ToUInt32(uintBuffer, 0);
					res.Append(valid[(int)(num % (uint)valid.Length)]);
				}
			}
			System.Diagnostics.Debug.WriteLine(res.ToString());
			return res.ToString();
		}

	}
}