using System;
using System.Linq;
using System.Text;

namespace Sitecore.MangaloreAirport.Website.Utility
{
   

    public class UtilityFunction
    {
        public static string Decryptdata(string encryptpwd)
        {
            System.Text.Decoder decoder = new UTF8Encoding().GetDecoder();
            byte[] bytes = System.Convert.FromBase64String(encryptpwd);
            char[] chars = new char[decoder.GetCharCount(bytes, 0, bytes.Length)];
            decoder.GetChars(bytes, 0, bytes.Length, chars, 0);
            return new string(chars);
        }

        public static string Encryptdata(string password)
        {
            byte[] buffer = new byte[password.Length];
            return System.Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }

        public string GenerateRandomPassword()
        {
            Random random = new Random();
            return Encryptdata(new string((from s in Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10) select s[random.Next(s.Length)]).ToArray<char>()));
        }

        public string GenerateRandomUserId()
        {
            Random random = new Random();
            return new string((from s in Enumerable.Repeat<string>("0123456789", 10) select s[random.Next(s.Length)]).ToArray<char>());
        }
    }
}

