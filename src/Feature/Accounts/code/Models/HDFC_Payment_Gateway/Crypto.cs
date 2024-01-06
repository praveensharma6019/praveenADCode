using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sitecore.Feature.Accounts.Models.HDFC_Payment_Gateway
{
    public class Crypto
    {
        //private static object hmacsha256;

        public static string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            SHA256 sha256Hash = SHA256.Create();

            // ComputeHash - returns byte array  
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

            // Convert byte array to a string   
            var builder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                builder.Append(bytes[i].ToString("x2"));
            }
            return builder.ToString();
        }

        /*
        public static string GetSHA256(string text)
        {
            UTF8Encoding encoder = new UTF8Encoding();
            SHA256Managed sha256hasher = new SHA256Managed();
            byte[] hashedDataBytes = sha256hasher.ComputeHash(encoder.GetBytes(text));
            StringBuilder output = new StringBuilder("");
            for (int i = 0; i < hashedDataBytes.Length; i++)
            {
                output.Append(hashedDataBytes[i].ToString("x2"));
            }
            return output.ToString();
        }
        */

        public static string ComputeHmacSha256Hash(string message, string secret)
        {
            secret = string.IsNullOrEmpty(secret) ? "" : secret;
            var encoding = new ASCIIEncoding();
            var keyByte = encoding.GetBytes(secret);
            var messageBytes = encoding.GetBytes(message);

            var hmacsha256 = new HMACSHA256(keyByte);
            var hashmessage = hmacsha256.ComputeHash(messageBytes);
            return System.Convert.ToBase64String(hashmessage);
        }
    }
}