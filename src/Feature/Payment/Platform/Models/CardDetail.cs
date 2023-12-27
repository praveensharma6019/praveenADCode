namespace Adani.SuperApp.Airport.Feature.Payment.Platform.Models
{
    public class CardDetail
    {
        public string CardNumberLabel { get; set; }

        public string ValidThruLabel { get; set; }

        public string CvvLabel { get; set; }

        public string NameOnCardLabel { get; set; }

        public string SecureCardLabel { get; set; }

        public string CardImageSmall { get; set; }

        public string CardImageBig { get; set; }

        public string RequiredCardNumberErrMsg { get; set; }

        public string IncorrectCardNumberErrMsg { get; set; }

        public string RequiredValidThruErrMsg { get; set; }

        public string IncorrectValidThruErrMsg { get; set; }

        public string IncorrectNameCardErrMsg { get; set; }

        public string ExpireIconSmall { get; set; }
        public string ExpireIconBig { get; set; }

        public string InfoIconSmall { get; set; }

        public string InfoIconBig { get; set; }

        public string RequiredCvvErrMsg { get; set; }

        public string IncorrectCvvErrMsg { get; set; }

        public string ViewOtherBankLabel { get; set; }
    }
}