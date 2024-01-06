using Sitecore.Foundation.Dictionary.Repositories;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Adani.Feature.Common.Helpers
{
    public class CustomExtension
    {
        public static string GetAndStoreEncryptedIPInCookies(string clientIP)
        {
            string encryptedClientIPString;

            // Convert the string to bytes
            byte[] clientIPDataBytes = Encoding.UTF8.GetBytes(clientIP);

            // Create an AES encryption provider
            using (Aes aes = Aes.Create())
            {
                // Set a secret key and initialization vector (IV)
                byte[] key = Encoding.UTF8.GetBytes(DictionaryPhraseRepository.Current.Get("AESEncryptDecryptKey/EncryptionKey", "8080808080808080"));
                byte[] iv = Encoding.UTF8.GetBytes(DictionaryPhraseRepository.Current.Get("AESEncryptDecryptKey/iv", "8080808080808080"));

                // Encrypt the data
                using (ICryptoTransform encryptor = aes.CreateEncryptor(key, iv))
                {
                    byte[] encryptedClientIPData = encryptor.TransformFinalBlock(clientIPDataBytes, 0, clientIPDataBytes.Length);

                    // Convert the encrypted data to a Base64-encoded string
                    encryptedClientIPString = System.Convert.ToBase64String(encryptedClientIPData);
                }
            }

            return encryptedClientIPString;
        }

        public static string Decrypt(string encryptedText, string key, string iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = Encoding.UTF8.GetBytes(iv);

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(System.Convert.FromBase64String(encryptedText)))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd();
                        }
                    }
                }
            }
        }
    }
}