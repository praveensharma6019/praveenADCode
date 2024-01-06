using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Foundation.Dictionary.Repositories;
using System;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class RegisterForEBillAlert
    {
        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegisterForEBillAlert))]
        public string CustomerID { get; set; }

        //[Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(RegisterForEBillAlert))]
        public string CustomerName { get; set; }

        public bool RegisterForSoftCopy { get; set; }

        public bool RegisterForSoftAndHardCopy { get; set; }

        public string EInvoice_Type { get; set; }
        public string Email_id { get; set; }
        public string Msg_Flag { get; set; }
        public string Message { get; set; }
        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
    }
}