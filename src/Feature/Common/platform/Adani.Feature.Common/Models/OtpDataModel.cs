using System;

namespace Adani.Feature.Common.Models
{
    public class OtpDataModel
    {
        public string ID { get; set; }
        public string OTP { get; set; }
        public DateTimeOffset ExpireAt { get; set; }
    }
}
