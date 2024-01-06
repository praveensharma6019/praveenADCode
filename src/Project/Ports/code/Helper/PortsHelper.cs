using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Sitecore.Data.Items;
using Sitecore.Ports.Website.Models;

namespace Sitecore.Ports.Website.Helper
{
    public class PortsHelper
    {
        Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
        
        public string GetUniqueRegNo()
        {
            int maxSize = 10;
            int minSize = 5;
            char[] chars = new char[62];
            string a;
            a = "1234567890";
            chars = a.ToCharArray();
            int size = maxSize;
            byte[] data = new byte[1];
            RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider();
            crypto.GetNonZeroBytes(data);
            size = maxSize;
            data = new byte[size];
            crypto.GetNonZeroBytes(data);
            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            { result.Append(chars[b % (chars.Length - 1)]); }
            return result.ToString();
        }
    }
}