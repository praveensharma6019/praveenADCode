using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.Payment.Platform.Models
{
    public class PaymentType
    {   public string TypeFilter { get; set; }
        public Amount ActiveRule { get; set; }
        public string Type { get; set; }
        public string IconSmall { get; set; }
        public string IconBig { get; set; }
        public string Name { get; set; }
        public List<Card> Options { get; set; }
    }

    public class Amount
    {
        public string amountlimit { get; set; }
        public string title { get; set; }
        public string message { get; set; }
        public string minAmount { get; set; }
        public string minAmountMessage { get; set; }

    }
}