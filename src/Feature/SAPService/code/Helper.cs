using System;
using System.Globalization;
using Newtonsoft.Json;

namespace SapPiService
{
    public static class Helper
    {
        private const int AccountNumberLength = 12;
        public static string FormatAccountNumber(string accountNumber)
        {
            return accountNumber.PadLeft(AccountNumberLength, '0');
        }

        public static string ConvertDateToString(DateTime date)
        {
            return date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
        }

        public static void LogObject(object obj)
        {

#if DEBUG
            Console.WriteLine(JsonConvert.SerializeObject(obj)); 
#else

#endif

        }

        public class BillPaymentItem
        {
            public string AccountNumber { get; set; }
            public string TransactionId { get; set; }
            public DateTime TransactionTime { get; set; }
            public decimal PaymentAmount { get; set; }
            public string PaymentMode { get; set; }
            public string PaymentType { get; set; }
            public string PaymentGateway { get; set; }
            public bool IsSuccess { get; set; }
            public string Message { get; set; }
        }
    }
}