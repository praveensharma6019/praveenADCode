using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Models
{
    public class ManageEmailsAdaniGas
    {
        public string Message { get; set; }
        public bool IsError { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(ManageEmailsAdaniGas))]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(ManageEmailsAdaniGas))]
        [DataType(DataType.EmailAddress)]
        public string NewEmailAddress { set; get; }
        public List<EmailEntry> EmailsList { set; get; }

        public ManageEmailsAdaniGas()
        {
            IsError = false;
            EmailsList = new List<EmailEntry>();
            Message = string.Empty;
            NewEmailAddress = string.Empty;
        }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");

    }

    public class EmailEntry
    {
        public string CustomerID { get; set; }
        public string EmailId { get; set; }
        public string MessageFlag { get; set; }
        public string Message { get; set; }
    }
}