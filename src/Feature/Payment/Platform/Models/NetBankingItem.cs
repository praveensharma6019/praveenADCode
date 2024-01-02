namespace Adani.SuperApp.Airport.Feature.Payment.Platform.Models
{
    public class NetBankingItem
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public bool IsActive { get; set; }

        public string SmallIcon { get; set; }

        public string LargeIcon { get; set; }

        public bool IsShowInPage { get; set; }
    }
}