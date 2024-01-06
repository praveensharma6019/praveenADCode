using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.AdaniCapital.Website.Models;
using static Sitecore.AdaniCapital.Website.Models.AdaniCapitalCareerFormModel;

namespace Sitecore.AdaniCapital.Website.Helper
{
    public class AdaniCapitalCareerForm
    {
        private Database db = Factory.GetDatabase("web");

        public AdaniCapitalCareerForm()
        {

        }
        

       
        public string GetUniqueRegNo()
        {
            int num = 10;
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