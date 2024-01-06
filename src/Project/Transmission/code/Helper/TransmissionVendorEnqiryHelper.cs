using Sitecore.Configuration;
using Sitecore.Data;
using System;
using System.Security.Cryptography;
using System.Text;

namespace Sitecore.Transmission.Website.Helper
{
    public class TransmissionVendorEnqiryHelper
    {
        private Database db = Factory.GetDatabase("web");

        public TransmissionVendorEnqiryHelper()
        {
        }

        public string GetUniqueRegNo()
        {
            int num = 5;
            char[] charArray = new char[62];
            charArray = "1234567890".ToCharArray();
            int num1 = num;
            byte[] numArray = new byte[1];
            RNGCryptoServiceProvider rNGCryptoServiceProvider = new RNGCryptoServiceProvider();
            rNGCryptoServiceProvider.GetNonZeroBytes(numArray);
            num1 = num;
            numArray = new byte[num1];
            rNGCryptoServiceProvider.GetNonZeroBytes(numArray);
            StringBuilder stringBuilder = new StringBuilder(num1);
            byte[] numArray1 = numArray;
            for (int i = 0; i < (int)numArray1.Length; i++)
            {
                byte num2 = numArray1[i];
                stringBuilder.Append(charArray[num2 % ((int)charArray.Length - 1)]);
            }
            return stringBuilder.ToString();
        }
    }
}