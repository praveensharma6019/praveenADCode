namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;
    using System.Web;

    [Serializable]
    public class ITDeclarationsModel
    {
        public ITDeclarationsModel()
        {
            int currentFYYear = DateTime.Now.Year;
            if (DateTime.Now.Month <= 3)
            {
                currentFYYear = DateTime.Now.Year - 1;
            };

            FY_3 = "FY " + (currentFYYear - 3).ToString() + "-" + ((currentFYYear - 2) - 2000).ToString();
            FY_2 = "FY " + (currentFYYear - 2).ToString() + "-" + ((currentFYYear - 1) - 2000).ToString();
            FY_1 = "FY " + (currentFYYear - 1).ToString() + "-" + ((currentFYYear) - 2000).ToString();
            FY = "FY " + (currentFYYear).ToString() + "-" + ((currentFYYear + 1) - 2000).ToString();
        }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ITDeclarationsModel))]
        [RegularExpression(@"^[0-9]{8,10}$", ErrorMessageResourceName = nameof(InvalidAccountNumber), ErrorMessageResourceType = typeof(ITDeclarationsModel))]
        public string CANumber { get; set; }
        public string Source { get; set; }

        public string DeclarationType { get; set; }
        public bool PANNotLinked { get; set; }
        public string Captcha { get; set; }

        public bool IsvalidatAccount { get; set; }

        public string MobileNumber { get; set; }
        //public string ConsumerName { get; set; }

        public bool IsOTPSend { get; set; }
        public string OTPNumber { get; set; }

        public bool IsvalidatOTP { get; set; }

        public string PANNumber { get; set; }

        [RegularExpression(@"^[0-9]{12}$", ErrorMessageResourceName = nameof(InvalidAadharNumber), ErrorMessageResourceType = typeof(ITDeclarationsModel))]
        public string AadharNumber { get; set; }

        public string AgreeOption { get; set; }

        public string FY { get; set; }
        public string FY_1 { get; set; }

        public string FY_2 { get; set; }
        [RegularExpression(@"^[0-9]{15}$", ErrorMessageResourceName = nameof(InvalidAcknowledgementNumber), ErrorMessageResourceType = typeof(ITDeclarationsModel))]
        public string FY_2AcknowledgementNumber { get; set; }
        public string FY_2DateOfFilingReturn { get; set; }

        public string FY_3 { get; set; }
        [RegularExpression(@"^[0-9]{15}$", ErrorMessageResourceName = nameof(InvalidAcknowledgementNumber), ErrorMessageResourceType = typeof(ITDeclarationsModel))]
        public string FY_3AcknowledgementNumber { get; set; }
        public string FY_3DateOfFilingReturn { get; set; }

        public string Bill_Amount { get; set; }

       // [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ITDeclarationsModel))]
        [RegularExpression(@"^[0-9]*(?:\.[0-9]*)?$", ErrorMessage = "Only Numbers allowed")]
        public string Amount_considered_for_TDS { get; set; }
        //public decimal PAYMENT_AMT { get; set; }
        //public string TDS_RATE { get; set; }
        public decimal TDS_Deducted { get; set; }
        public string Net_amount_post_TDS_deduction { get; set; }
        public string POSTING_DATE { get; set; }

        public bool IsSuccess { get; set; }
        public string SuccessMessage { get; set; }

        public string ReSendMessage { get; set; }

        public bool AlreadySubmittedforCurrentFY { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value.");
        public static string InvalidAccountNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Account", "Please enter a valid Account Number.");
        public static string InvalidAadharNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid InvalidAadharNumber", "Please enter a valid 12 digit Aadhar Number.");
        public static string InvalidAcknowledgementNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid InvalidAadharNumber", "Please enter a valid Acknowledgement Number.");

    }
}