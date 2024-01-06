using Sitecore.Diagnostics;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Sitecore.AdaniConneX.Website.Helpers
{
    public class EncryptionServiceHelper
    {
        public static string EncryptString(string key, string plainText, string iv)
        {
            try
            {
                byte[] array;
                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = Encoding.UTF8.GetBytes(iv);

                    ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                        {
                            using (StreamWriter streamWriter = new StreamWriter((Stream)cryptoStream))
                            {
                                streamWriter.Write(plainText);
                            }

                            array = memoryStream.ToArray();
                        }
                    }
                }

                return System.Convert.ToBase64String(array);
            }
            catch (Exception ex)
            {
                Log.Error("Encryption Error: ", ex.Message);
                return null;
            }
        }

        public static string DecryptString(string key, string cipherText, string iv)
        {
            try
            {
                byte[] buffer = System.Convert.FromBase64String(cipherText);

                using (Aes aes = Aes.Create())
                {
                    aes.Key = Encoding.UTF8.GetBytes(key);
                    aes.IV = Encoding.UTF8.GetBytes(iv);
                    ICryptoTransform decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                    using (MemoryStream memoryStream = new MemoryStream(buffer))
                    {
                        using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                        {
                            using (StreamReader streamReader = new StreamReader((Stream)cryptoStream))
                            {
                                return streamReader.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Decryption Error: ", ex.Message);
                return cipherText;
            }
        }
    }
}