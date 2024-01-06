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
    public class SDEMIProcess
    {
        public SDEMIProcess()
        {
            int todayDateMonth = DateTime.Now.Month;
            if (todayDateMonth > 2 && todayDateMonth < 10)
            {
                NumberOfInstalmentsSelectList = new List<ListItem> {
                    new ListItem{ Value="6", Text="6"},
                    new ListItem{ Value="5", Text="5"},
                     new ListItem{ Value="4", Text="4"},
                      new ListItem{ Value="3", Text="3"},
                      new ListItem{ Value="2", Text="2"}
                };
            }
            else if (todayDateMonth == 10)
            {
                NumberOfInstalmentsSelectList = new List<ListItem> {
                    new ListItem{ Value="5", Text="5"},
                     new ListItem{ Value="4", Text="4"},
                      new ListItem{ Value="3", Text="3"},
                      new ListItem{ Value="2", Text="2"}
                };
            }
            else if (todayDateMonth == 11)
            {
                NumberOfInstalmentsSelectList = new List<ListItem> {
                     new ListItem{ Value="4", Text="4"},
                      new ListItem{ Value="3", Text="3"},
                      new ListItem{ Value="2", Text="2"}

                };
            }
            else if (todayDateMonth == 12)
            {
                NumberOfInstalmentsSelectList = new List<ListItem> {
                      new ListItem{ Value="3", Text="3"},
                      new ListItem{ Value="2", Text="2"},
                };
            }
            else if (todayDateMonth == 1)
            {
                NumberOfInstalmentsSelectList = new List<ListItem> {
                    new ListItem{ Value="2", Text="2"}
                };
            }
            if (NumberOfInstalmentsSelectList != null && NumberOfInstalmentsSelectList.Count > 0)
            {
                SelectedNumberOfInstalments = NumberOfInstalmentsSelectList.First().Value;
            }
        }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(PANUpdateModel))]
        [RegularExpression(@"^[0-9]{8,10}$", ErrorMessageResourceName = nameof(InvalidAccountNumber), ErrorMessageResourceType = typeof(PANUpdateModel))]
        public string CANumber { get; set; }
        public string Source { get; set; }

        public string Captcha { get; set; }

        public bool IsvalidatAccount { get; set; }

        public string MobileNumber { get; set; }
        public string ConsumerName { get; set; }

        public bool IsOTPSend { get; set; }
        public string OTPNumber { get; set; }

        public bool IsvalidatOTP { get; set; }

        public bool ProceedWithEMI { get; set; }
        public decimal SecurityDepositAmount { get; set; }

        public List<ListItem> NumberOfInstalmentsSelectList { get; set; }
        public string SelectedNumberOfInstalments { get; set; }

        public string Result { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value.");
        public static string InvalidAccountNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Account", "Please enter a valid Account Number.");
    }

    [Serializable]
    public class ListItem
    {
        public string Text { get; set; }
        public string Value { get; set; }
    }
}