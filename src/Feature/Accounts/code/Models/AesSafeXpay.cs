using com.paygate.ag.common.utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class AesSafeXpay
    {
        public string encrypt(string plainText, string MerchantEncryptionKey)
        {
            string va = string.Empty;
            string encryptText = PayGateCryptoUtils.encrypt(plainText, MerchantEncryptionKey);
            return encryptText;
        }

        public string decrypt(string encryptText, string MerchantEncryptionKey)
        {
            string va = string.Empty;
            string decryptText = PayGateCryptoUtils.decrypt(encryptText, MerchantEncryptionKey);
            return decryptText;
        }
        public string ComputeSha256Hash(string rawData)
        {
            // Create a SHA256   
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - returns byte array  
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(rawData));

                // Convert byte array to a string   
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }
    }
}