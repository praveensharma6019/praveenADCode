using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace InstamojoAPI
{
    public class PaymentOrderDetailsResponse
    {
        public PaymentOrderDetailsResponse()
        {
            status = "pending";
            currency = "INR";
        }

        public string id { get; set; }
        public string transaction_id { get; set; }
        public string status { get; set; }
        public string currency { get; set; }
        public decimal? amount { get; set; }
        public string name { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string description { get; set; }
        public string redirect_url { get; set; }

        public object payments { get; set; }

        public string created_at { get; set; }
        public string resource_uri { get; set; }
    }
    public class EncryptionDecryption
    {
        public static string Decrypt(string textToDecrypt, IReadOnlyList<byte[]> key)
        {
            var buffer = System.Convert.FromBase64String(textToDecrypt);

            var cipher = new RijndaelManaged
            {
                Mode = CipherMode.CBC,
                Padding = PaddingMode.PKCS7,
                KeySize = 0x80, // 256bit key
                BlockSize = 0x80,
                Key = key[0],
                IV = key[1]
            };

            var transform = cipher.CreateDecryptor();
            string decryptedString;

            using (var encryptedStream = new MemoryStream())
            {
                using (var decryptStream = new CryptoStream(encryptedStream, transform, CryptoStreamMode.Write))
                {
                    decryptStream.Write(buffer, 0, buffer.Length);
                }

                decryptedString = Encoding.UTF8.GetString(encryptedStream.ToArray());
            }

            return decryptedString;

        }

        public static string GenerateRandomOrderId(string UserLocationPath)
        {
            string returnString = string.Empty;
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            returnString = new string(Enumerable.Repeat(chars, 15).Select(s => s[random.Next(s.Length)]).ToArray());

            //Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
            //var Item = dbWeb.GetItem(UserLocationPath + "/" + returnString);
            //if (Item != null)
            //    return GenerateRandomOrderId(UserLocationPath);
            return returnString;

        }

        public static string Encrypt(string textToEncrypt, IReadOnlyList<byte[]> key)
        {
            string encryptedString;
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(textToEncrypt);

                RijndaelManaged cipher = new RijndaelManaged
                {
                    Mode = CipherMode.CBC,
                    Padding = PaddingMode.PKCS7,
                    KeySize = 0x80, // 256bit key
                    BlockSize = 0x80,
                    Key = key[0],
                    IV = key[1]
                };

                using (ICryptoTransform transform = cipher.CreateEncryptor())
                {
                    byte[] bytes = transform.TransformFinalBlock(buffer, 0, buffer.Length);
                    encryptedString = System.Convert.ToBase64String(bytes);
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return encryptedString;
        }

        public class KeyGenerator
        {
            public static string GetUniqueKey(int size)
            {
                char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
                byte[] data = new byte[size];
                using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
                {
                    crypto.GetBytes(data);
                }
                StringBuilder result = new StringBuilder(size);
                foreach (byte b in data)
                {
                    result.Append(chars[b % (chars.Length)]);
                }
                return result.ToString();
            }
        }

        public static byte[][] GetHashKeys(string key)
        {
            // Get SHA256 hash bytes for salt
            var result = new byte[2][];

            var pwdBytes = Encoding.UTF8.GetBytes(key);
            var keyBytes = new byte[0x10];
            SHA256 sha2 = new SHA256CryptoServiceProvider();
            var hashKey = sha2.ComputeHash(pwdBytes);
            var len = hashKey.Length;

            if (len > keyBytes.Length)
            {
                len = keyBytes.Length;
            }

            Array.Copy(hashKey, keyBytes, len); result[0] = keyBytes; // Get IV  bytes

            const string iv = "xxxxyyyyzzzzwwww";
            var sourceArray = Encoding.UTF8.GetBytes(iv);
            var destinationArray = new byte[0x10];
            var length = sourceArray.Length;

            if (length > destinationArray.Length)
            {
                length = sourceArray.Length;
            }

            Array.Copy(sourceArray, destinationArray, length);

            result[1] = sourceArray;

            return result;
        }
    }
}