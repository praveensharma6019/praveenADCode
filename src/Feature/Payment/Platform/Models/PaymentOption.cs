using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.Payment.Platform.Models
{
    public class PaymentOptions
    {
        public string ChoosePaymentHeading { get; set; }

        public string PayText { get; set; }

        public string Hostname { get; set; }

        public List<PaymentType> PaymentTypeList { get; set; }

        public CardDetail CardDetail { get; set; }

        public List<PromoCard> PromoCards { get; set; }

        public SecurityCard SecurityCardDetail { get; set; }

        public PromoCard ApprovePayments { get; set; }

        public string SafeIconSmall { get; set; }

        public string SafeIconBig { get; set; }

        public string SafeText { get; set; }

        public string UPIValidError { get; set; }

        public PromoCard OfferText { get; set; }
        public bool isReward { get; set; }
        
        public string downTimeText { get; set; }

        public string fluctuateTimeText { get; set; }
    }
}