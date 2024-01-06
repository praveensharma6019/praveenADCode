using SapPiService.Domain;
using System;
using System.ComponentModel;

namespace SapPiService
{
    public static class EnumHelper
    {
        /*
         * string s = Enum.GetName(typeof(ConsumerType), 0);
                Console.WriteLine(s);

                Console.WriteLine("The values of the Day Enum are:");
                foreach (int i in Enum.GetValues(typeof(ConsumerType)))
                    Console.WriteLine(i);

                Console.WriteLine("The names of the Day Enum are:");
                foreach (string str in Enum.GetNames(typeof(ConsumerType)))
                    Console.WriteLine(str);
         */


        public static string GetDescription<T>(this T enumerationValue) where T : struct
        {
            var type = enumerationValue.GetType();
            if (!type.IsEnum)
            {
                throw new ArgumentException("EnumerationValue must be of Enum type", nameof(enumerationValue));
            }

            //Tries to find a DescriptionAttribute for a potential friendly name
            //for the enum
            var memberInfo = type.GetMember(enumerationValue.ToString());
            if (memberInfo.Length <= 0) return enumerationValue.ToString();
            var attrs = memberInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
            return attrs.Length > 0 ? ((DescriptionAttribute)attrs[0]).Description : enumerationValue.ToString();
        }

        public static BillingLanguage ParseLanguage(string languageName)
        {
            foreach (BillingLanguage value in Enum.GetValues(typeof(BillingLanguage)))
            {
                if (languageName == value.GetDescription()) return value;
            }

            return BillingLanguage.English;
        }
    }
}