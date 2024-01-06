using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class QAMeterReading
    {
        private string _schedulemeterreadingdate;

        public string Monthval { get; set; }
        public List<SelectListItem> MonthList { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(QAMeterReading))]
        public string AccountNo { get; set; }
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

        public string CycleNumber { get; set; }
        public string Captcha { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
    }
}