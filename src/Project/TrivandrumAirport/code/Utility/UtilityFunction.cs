using System;
using System.Linq;
using System.Text;

namespace Sitecore.TrivandrumAirport.Website.Utility
{
    public class UtilityFunction
    {
        public UtilityFunction()
        {
        }

        public static string Decryptdata(string encryptpwd)
        {
            string decryptpwd = string.Empty;
            Decoder Decode = (new UTF8Encoding()).GetDecoder();
            byte[] todecode_byte = System.Convert.FromBase64String(encryptpwd);
            int charCount = Decode.GetCharCount(todecode_byte, 0, (int)todecode_byte.Length);
            char[] decoded_char = new char[charCount];
            Decode.GetChars(todecode_byte, 0, (int)todecode_byte.Length, decoded_char, 0);
            return new string(decoded_char);
        }

        public static string Encryptdata(string password)
        {
            string strmsg = string.Empty;
            byte[] encode = new byte[password.Length];
            return System.Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        }

        public string GenerateRandomPassword()
        {
            string returnString = string.Empty;
            Random random = new Random();
            returnString = new string((
                from s in Enumerable.Repeat<string>("ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789", 10)
                select s[random.Next(s.Length)]).ToArray<char>());
            return UtilityFunction.Encryptdata(returnString);
        }

        public string GenerateRandomUserId()
        {
            string returnString = string.Empty;
            Random random = new Random();
            returnString = new string((
                from s in Enumerable.Repeat<string>("0123456789", 10)
                select s[random.Next(s.Length)]).ToArray<char>());
            return returnString;
        }
    }
}