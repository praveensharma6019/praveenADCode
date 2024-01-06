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
    public class GreenPowerOptInModel
    {
        public GreenPowerOptInModel()
        {
            PercentageSelectList = new List<ListItem> {
                    new ListItem{ Value="100", Text="100"},
                    new ListItem{ Value="90", Text="90"},
                     new ListItem{ Value="80", Text="80"},
                      new ListItem{ Value="70", Text="70"},
                      new ListItem{ Value="60", Text="60"},
                      new ListItem{ Value="50", Text="50"},
                      new ListItem{ Value="40", Text="40"},
                      new ListItem{ Value="30", Text="30"},
                      new ListItem{ Value="20", Text="20"},
                      new ListItem{ Value="10", Text="10"}
                };
            SelectedPercentage = "100";
        }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(PANUpdateModel))]
        [RegularExpression(@"^[0-9]{8,10}$", ErrorMessageResourceName = nameof(InvalidAccountNumber), ErrorMessageResourceType = typeof(PANUpdateModel))]
        public string CANumber { get; set; }

        public string MeterNumber { get; set; }

        public List<ListItem> PercentageSelectList { get; set; }
        public string SelectedPercentage { get; set; }

        public string Source { get; set; }

        public string Captcha { get; set; }

        public bool IsvalidatAccount { get; set; }
        public bool IsProcessingDone { get; set; }
        public bool IsUpdateMobile { get; set; }

        public string MobileNumber { get; set; }
        public string EmailId { get; set; }
        public string ConsumerName { get; set; }

        public bool IsOTPSend { get; set; }
        public string OTPNumber { get; set; }

        public bool IsvalidatOTP { get; set; }
        public bool IsAccepted { get; set; }
        public bool IsSubmit { get; set; }
        public bool IsIPledge { get; set; }
        public string ImageSrc { get; set; }
        public string imageLink { get; set; }

        public string FacebookId { get; set; }
        public string TwitterId { get; set; }

        public string ActivateBillingPeriod { get; set; }

        public bool ProceedWithEMI { get; set; }
        public decimal SecurityDepositAmount { get; set; }

        public List<ListItem> NumberOfInstalmentsSelectList { get; set; }
        public string SelectedNumberOfInstalments { get; set; }

        public string Result { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value.");
        public static string InvalidAccountNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Account", "Please enter a valid Account Number.");
    }

}