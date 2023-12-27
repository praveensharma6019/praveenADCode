using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model
{
    public class SubmitViewModel
    {
        public SubmitFormItem SubmitFormItems { get; set; }

        public FeedbackForm FeedbackFormData { get; set; }

        public FeedbackFormResponse FeedbackFormResult { get; set; }
    }
    
    public class SubmitFormItem
    {
        public string SelectAirportLabel { get; set; }

        public string IssueFacedLabel { get; set; }

        public string FullNameLabel { get; set; }

        public string MobileNoLabel { get; set; }

        public string EmailIDLabel { get; set; }

        public string HelpTextLabel { get; set; }

        public string WritePlacholderLabel { get; set; }

        public string AgreeLabel { get; set; }

        public string SubmitButtonLabel { get; set; }

        public string ModalLabel { get; set; }

        public string CloseLabel { get; set; }

        public List<string> IssueList { get; set; }

        public List<Airports> AirportList { get; set; }
    }
}