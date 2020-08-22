using System;
using System.Security.Cryptography;
using System.Text;

namespace CryptographyHelperUtil.Helpers
{
    public class CryptographyHelper
    {
        public static string ConnectionStringPassword = "password";

        public static byte[] Encrypt(byte[] clearData)
        {
            if (string.IsNullOrEmpty(ConnectionStringPassword))
            {
                return clearData;
            }
            var utf8 = new UTF8Encoding();
            var clearText = utf8.GetString(clearData);
            var encryptedData = Encrypt(clearText);
            var result = utf8.GetBytes(encryptedData);
            return result;
        }

        public static byte[] Decrypt(byte[] cipherData)
        {
            if (string.IsNullOrEmpty(ConnectionStringPassword))
            {
                return cipherData;
            }
            var utf8 = new UTF8Encoding();
            var encryptedText = utf8.GetString(cipherData);
            var decryptedText = Decrypt(encryptedText);
            var result = utf8.GetBytes(decryptedText);
            return result;
        }

        public static string Encrypt(string clearText)
        {
            if (string.IsNullOrEmpty(ConnectionStringPassword))
            {
                return clearText;
            }
            byte[] result;
            var utf8 = new UTF8Encoding();
            var hashProvider = new MD5CryptoServiceProvider();

            var password = ConnectionStringPassword; //TODO: read from certificate file

            byte[] tdesKey = hashProvider.ComputeHash(utf8.GetBytes(password));
            var tdesAlgorithm = GetTripleDesCryptoServiceProvider(tdesKey);
            var dataToEncrypt = utf8.GetBytes(clearText);
            try
            {
                var encryptor = tdesAlgorithm.CreateEncryptor();
                result = encryptor.TransformFinalBlock(dataToEncrypt, 0, dataToEncrypt.Length);
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }
            string base64String = Convert.ToBase64String(result);
            return base64String;
        }

        public static string Decrypt(string cipherText)
        {
            if (string.IsNullOrEmpty(ConnectionStringPassword))
            {
                return cipherText;
            }
            var result = new byte[] { };
            var utf8 = new UTF8Encoding();
            var hashProvider = new MD5CryptoServiceProvider();

            var password = ConnectionStringPassword; //TODO: read from certificate file

            var tdesKey = hashProvider.ComputeHash(utf8.GetBytes(password));
            var tdesAlgorithm = GetTripleDesCryptoServiceProvider(tdesKey);

            try
            {
                var dataToDecrypt = Convert.FromBase64String(cipherText);
                var decryptor = tdesAlgorithm.CreateDecryptor();
                result = decryptor.TransformFinalBlock(dataToDecrypt, 0, dataToDecrypt.Length);
            }
            catch
            {
                //Wrong password
            }
            finally
            {
                tdesAlgorithm.Clear();
                hashProvider.Clear();
            }
            string utf8String = utf8.GetString(result);
            return utf8String;
        }

        private static TripleDESCryptoServiceProvider GetTripleDesCryptoServiceProvider(byte[] tdesKey)
        {
            var tdesAlgorithm = new TripleDESCryptoServiceProvider
            {
                Key = tdesKey,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            return tdesAlgorithm;
        }
    }

}
