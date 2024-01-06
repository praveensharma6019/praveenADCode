using ccavutil;
using System;

namespace CCA.Util
{
    public class CCACrypto
    {
        public CCACrypto()
        {
        }

        private string adler32(long adler, string strPattern)
        {
            long num = (long)0;
            long num1 = (long)65521;
            long num2 = this.andop(adler, (long)65535);
            long num3 = this.andop(this.cdec(this.rightshift(this.cbin(adler), (long)16)), (long)65535);
            for (int i = 0; i < strPattern.Length; i++)
            {
                char[] charArray = strPattern.Substring(i, 1).ToCharArray();
                num2 = (long)((num2 + (long)charArray[0]) % num1);
                num3 = (num3 + num2) % num1;
            }
            long num4 = this.cdec(this.leftshift(this.cbin(num3), (long)16)) + num2;
            return num4.ToString();
        }

        private long andop(long op1, long op2)
        {
            string str = "";
            string str1 = this.cbin(op1);
            string str2 = this.cbin(op2);
            for (int i = 0; i < 32; i++)
            {
                str = string.Concat(str, long.Parse(str1.Substring(i, 1)) & long.Parse(str2.Substring(i, 1)));
            }
            return this.cdec(str);
        }

        private string cbin(long num)
        {
            string str = "";
            do
            {
                str = string.Concat(num % (long)2, str).ToString();
                num = (long)Math.Floor(num / new decimal(2));
            }
            while (num != (long)0);
            long length = (long)(32 - str.Length);
            for (int i = 1; (long)i <= length; i++)
            {
                str = string.Concat("0", str);
            }
            return str;
        }

        private long cdec(string strNum)
        {
            long num = (long)0;
            for (int i = 0; i < strNum.Length; i++)
            {
                num = num + long.Parse(strNum.Substring(i, 1)) * this.power((long)(strNum.Length - (i + 1)));
            }
            return num;
        }

        public string Decrypt(string strToDecrypt, string Key)
        {
            return (new AesCryptUtil(Key)).decrypt(strToDecrypt);
        }

        public string Encrypt(string strToEncrypt, string Key)
        {
            return (new AesCryptUtil(Key)).encrypt(strToEncrypt);
        }

        public string getchecksum(string MerchantId, string OrderId, string Amount, string redirectUrl, string WorkingKey)
        {
            string[] merchantId = new string[] { MerchantId, "|", OrderId, "|", Amount, "|", redirectUrl, "|", WorkingKey };
            string str = string.Concat(merchantId);
            return this.adler32((long)1, str);
        }

        private string leftshift(string str, long num)
        {
            long length = (long)(32 - str.Length);
            for (int i = 1; (long)i <= length; i++)
            {
                str = string.Concat("0", str);
            }
            for (int j = 1; (long)j <= num; j++)
            {
                str = string.Concat(str, "0");
                str = str.Substring(1, str.Length - 1);
            }
            return str;
        }

        private long power(long num)
        {
            long num1 = (long)1;
            for (int i = 1; (long)i <= num; i++)
            {
                num1 *= (long)2;
            }
            return num1;
        }

        private string rightshift(string str, long num)
        {
            for (int i = 1; (long)i <= num; i++)
            {
                str = string.Concat("0", str);
                str = str.Substring(0, str.Length - 1);
            }
            return str;
        }

        public string verifychecksum(string MerchantId, string OrderId, string Amount, string AuthDesc, string WorkingKey, string checksum)
        {
            string str;
            string[] merchantId = new string[] { MerchantId, "|", OrderId, "|", Amount, "|", AuthDesc, "|", WorkingKey };
            string str1 = string.Concat(merchantId);
            str = (string.Compare(this.adler32((long)1, str1), checksum, true) != 0 ? "false" : "true");
            return str;
        }
    }
}