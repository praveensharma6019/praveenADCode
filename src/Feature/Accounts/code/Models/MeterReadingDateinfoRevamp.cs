using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.Feature.Accounts.Models
{
    public class MeterReadingDateinfoRevamp
    {
        private string _schedulemeterreadingdate;
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(MeterReadingDateinfoRevamp))]
        public string Monthval { get; set; }

       // [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(MeterReadingDateinfoRevamp))]
        //[RegularExpression(@"^[0-9]{8,10}$", ErrorMessageResourceName = nameof(InvalidAccountNumber), ErrorMessageResourceType = typeof(MeterReadingDateinfoRevamp))]
        public string CANumber { get; set; }
        public List<SelectListItem> MonthList { get; set; }
        public string ScheduleMeterReadingdate
        {
            get
            {
                return this._schedulemeterreadingdate;
            }
            set
            {
                if (!string.IsNullOrEmpty(value))
                {
                    this._schedulemeterreadingdate = value;
                }
                else
                {
                    this._schedulemeterreadingdate = DictionaryPhraseRepository.Current.Get("/MyAccounts/Meter Reading/No Data Available", "No records exist for selected item");
                }
            }
        }

        public string Captcha { get; set; }
        public bool IsCheckData { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please select value for month");
        public static string InvalidAccountNumber => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Account", "Please enter a valid Account Number.");
    }
}