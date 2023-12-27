using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model
{
    public class ContactUsForm
    {
        public string FormTitle { get; set; }

        public string FormSubtitle { get; set; }

        public string BannerTitle { get; set; }
       
        public string BannerImage { get; set; }   
        
        public string MobileBannerImage { get; set; }

        public string ReachOutText { get; set; }

        public string SendQueryText { get; set; }       

        public string FirstNameLabel { get; set; }

        public string FirstNameRequired { get; set; }

        public string FirstNameIncorrect { get; set; }

        public string LastNameLabel { get; set; }

        public string LastNameRequired { get; set; }

        public string LastNameIncorrect { get; set; }

        public string MobileNoLabel { get; set; }

        public string MobileNoRequired { get; set; }

        public string MobileNoIncorrect { get; set; }

        public string EmailIdLabel { get; set; }

        public string EmailIdRequired { get; set; }

        public string EmailIdIncorrect { get; set; }

        public string SelectAirportLabel { get; set; }

        public string AirportRequired { get; set; }

        public string FlightNumberLabel { get; set; }

        public string FlightDateLabel { get; set; }

        public string IssueTypeLabel { get; set; }

        public List<Airports> AirportDetails { get; set; }

        public List<IssueType> IssueTypeList { get; set; }

        public string IssueTypeRequired { get; set; }

        public string HelpTextLabel { get; set; }

        public string HelpTextRequired { get; set; }

        public string HelpTextMaxCharacter { get; set; }

        public string HelpTextMaxCharacterMsg { get; set; }

        public string WritePlacholderLabel { get; set; }
        
        public string TermsLabel { get; set; }

        public string TermsRequired { get; set; }

        public string SubmitButtonLabel { get; set; }    

        
    }

    public class IssueType
    {
        public string IssueText { get; set; }

        public string IssueValue { get; set; }

        public string EmailSubmissionMsg { get; set; }

        public string SMSSubmissionMsg { get; set; }

        public string EmailFulfillmentMsg { get; set; }

        public string SMSFulfillmentMsg { get; set; }

        public string PopUpSuccessMsg { get; set; }

        public string PopUpReOpenMsg { get; set; }
        public string PopUpTitle { get; set; }

    }
}