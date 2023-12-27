namespace Adani.SuperApp.Airport.Feature.Payment.Platform.Models
{
    public class PromoCard
    {
        #region Private Variables

        private string _buttonText;
        private string _note;

        #endregion

        public string Heading { get; set; }
        public string Description { get; set; }
        public string ImageSmall { get; set; }
        public string ImageLarge { get; set; }
        public string Note
        {
            get
            { return _note ?? string.Empty; }
            set
            { _note = value; }
        }
        public string ButtonText
        {
            get { return _buttonText ?? string.Empty; }

            set
            {
                _buttonText = value;
            }
        }

        public string BtnLink { get; set; }
    }
}