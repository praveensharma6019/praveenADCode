namespace Adani.SuperApp.Airport.Feature.Payment.Platform.Models
{
    public class CardImageIcon
    {
        public string CardIconSmall { get; set; }

        public string CardIconBig { get; set; }

        public string RegexForCardNumber { get; set; }

        public string CVVLength { get; set; }

        public bool IsNameMandatory { get; set; }
    }
}