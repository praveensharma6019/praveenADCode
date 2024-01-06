namespace Farmpik.Infrastructure.Models
{
    public class FarmpikOTPDataModel
    {
        public string id { get; set; }
        public SmsData smsData { get; set; }
    }
    public class SmsData
    {
        public string recipient { get; set; }
        public string otp { get; set; }
    }
}