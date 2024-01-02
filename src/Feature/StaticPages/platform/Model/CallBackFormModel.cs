
namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model
{
    public class CallBackFormModel
    {
        public CallBackFormLabels Labels { get; set; }

        public CallBackFormInput Input { get; set; }

        public CallBackFormModel() 
        {
            Input = new CallBackFormInput();        
        }

    }

    public class CallBackFormLabels
    {
        public string FormTitle { get; set; }

        public string Description { get; set; }

        public string BannerImage { get; set; }

        public string FullNameLabel { get; set; }

        public string ContactNumberLabel { get; set; }

        public string EmailAddressLabel { get; set; }

        public string OrganizationLabel { get; set; }

        public string ButtonText { get; set; }

        public string CommercialEmailTo { get; set; }

        public string AirlinePartnershipEmailTo { get; set; }

        public string SuccessMessage { get; set; }

        public string FailureMessage { get; set; }
    }
}
