using Application.IService.Utility;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Application.Service.Utility
{
    public class EncryptService : IEncryptService
    {
        public static byte[] INIT_VEC = new byte[16]
        {
            (byte) 75,
            (byte) 115,
            (byte) 204,
            (byte) 15,
            (byte) 111,
            (byte) 70,
            (byte) 156,
            (byte) 234,
            (byte) 168,
            (byte) 19,
            (byte) 46,
            (byte) 53,
            (byte) 194,
            (byte) 205,
            (byte) 249,
            (byte) 5
        };

        public static byte[] BYTES_KEY = new byte[24]
        {
            (byte) 23,
            (byte) 24,
            (byte) 1,
            (byte) 2,
            (byte) 3,
            (byte) 4,
            (byte) 5,
            (byte) 6,
            (byte) 15,
            (byte) 16,
            (byte) 17,
            (byte) 18,
            (byte) 19,
            (byte) 20,
            (byte) 21,
            (byte) 22,
            (byte) 7,
            (byte) 8,
            (byte) 9,
            (byte) 10,
            (byte) 11,
            (byte) 12,
            (byte) 13,
            (byte) 14
        };

        public static string Encrypt(string strData)
        {
            return EncryptService.Encrypt(strData, (byte[])null);
        }

        public static string Encrypt(string strData, byte[] key)
        {
            return Convert.ToBase64String(EncryptService.Encrypt(new UTF8Encoding().GetBytes(strData), key));
        }

        public static byte[] Encrypt(byte[] bytesData)
        {
            return EncryptService.Encrypt(bytesData, (byte[])null);
        }

        public static byte[] Encrypt(byte[] bytesData, byte[] key)
        {
            MemoryStream memoryStream = (MemoryStream)null;
            ICryptoTransform transform = (ICryptoTransform)null;
            CryptoStream cryptoStream = (CryptoStream)null;
            byte[] numArray = (byte[])null;
            try
            {
                memoryStream = new MemoryStream();
                transform = EncryptService.GetCryptoServiceProvider(key);
                cryptoStream = new CryptoStream((Stream)memoryStream, transform, CryptoStreamMode.Write);
                cryptoStream.Write(bytesData, 0, bytesData.Length);
                cryptoStream.FlushFinalBlock();
                numArray = memoryStream.ToArray();
            }
            finally
            {
                cryptoStream?.Close();
                transform?.Dispose();
                memoryStream?.Close();
            }

            return numArray;
        }

        private static ICryptoTransform GetCryptoServiceProvider(byte[] key)
        {
            RijndaelManaged rijndaelManaged = new RijndaelManaged();
            rijndaelManaged.Mode = CipherMode.CBC;
            if (key == null)
                rijndaelManaged.Key = EncryptService.BYTES_KEY;
            else
                rijndaelManaged.Key = key;
            rijndaelManaged.IV = EncryptService.INIT_VEC;
            return rijndaelManaged.CreateEncryptor();
        }

        public static string UseEncrypt(string encryptTarget)
        {
            string result = "";

            try
            {
                if (encryptTarget.Length == 0)
                {
                    throw (new Exception("Enter the string to be encrypted."));
                }
                result = EncryptService.Encrypt(encryptTarget);
            }
            catch (Exception ex)
            {
                throw (ex);
            }
            return result;
        }

    }
}
