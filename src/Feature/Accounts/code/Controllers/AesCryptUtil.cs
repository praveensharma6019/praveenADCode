using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ccavutil
{
    public class AesCryptUtil
    {
        private byte[] data;

        private byte[] AesIV;

        public AesCryptUtil(string Key)
        {
            this.data = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(Key));
            this.AesIV = new byte[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12, 13, 14, 15 };
        }

        public string decrypt(string strToDecrypt)
        {
            string str;
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                str = DecryptStringFromBytes(strToDecrypt, this.data, this.AesIV);
            }
            return str;
        }

        private static string DecryptStringFromBytes(string encryptedText, byte[] Key, byte[] IV)
        {
            int length = encryptedText.Length;
            byte[] num = new byte[length / 2];
            for (int i = 0; i < length; i += 2)
            {
                num[i / 2] = Convert.ToByte(encryptedText.Substring(i, 2), 16);
            }
            if (num == null || (int)num.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (Key == null || (int)Key.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }
            if (IV == null || (int)IV.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }
            string end = null;
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Key = Key;
                rijndaelManaged.IV = IV;
                ICryptoTransform cryptoTransform = rijndaelManaged.CreateDecryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                using (MemoryStream memoryStream = new MemoryStream(num))
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Read))
                    {
                        using (StreamReader streamReader = new StreamReader(cryptoStream))
                        {
                            end = streamReader.ReadToEnd();
                        }
                    }
                }
            }
            return end;
        }

        public string encrypt(string strToEncrypt)
        {
            string str = "";
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                byte[] bytes = AesCryptUtil.EncryptStringToBytes(strToEncrypt, this.data, this.AesIV);
                StringBuilder stringBuilder = new StringBuilder();
                byte[] numArray = bytes;
                for (int i = 0; i < (int)numArray.Length; i++)
                {
                    stringBuilder.AppendFormat("{0:x2}", numArray[i]);
                }
                str = stringBuilder.ToString();
            }
            return str;
        }

        private static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            byte[] array;
            if (plainText == null || plainText.Length <= 0)
            {
                throw new ArgumentNullException("plainText");
            }
            if (Key == null || (int)Key.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }
            if (IV == null || (int)IV.Length <= 0)
            {
                throw new ArgumentNullException("Key");
            }
            using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
            {
                rijndaelManaged.Key = Key;
                rijndaelManaged.IV = IV;
                ICryptoTransform cryptoTransform = rijndaelManaged.CreateEncryptor(rijndaelManaged.Key, rijndaelManaged.IV);
                using (MemoryStream memoryStream = new MemoryStream())
                {
                    using (CryptoStream cryptoStream = new CryptoStream(memoryStream, cryptoTransform, CryptoStreamMode.Write))
                    {
                        using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                        {
                            streamWriter.Write(plainText);
                        }
                        array = memoryStream.ToArray();
                    }
                }
            }
            return array;
        }
    }
}