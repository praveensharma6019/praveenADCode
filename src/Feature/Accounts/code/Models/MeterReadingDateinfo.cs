using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.Feature.Accounts.Models
{
    public class MeterReadingDateinfo : ProfileBasicInfo
    {
        private string _schedulemeterreadingdate;
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(MeterReadingDateinfo))]
        public string Monthval { get; set; }
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

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please select value for month");
    }
}