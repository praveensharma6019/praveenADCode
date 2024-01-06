using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.BelvedereClubGurgaon.Website.Models
{
    [Serializable]
    public class LoginModel
    {
        public string userId { get; set; }
        public string TenderId { get; set; }
        public string UserType { get; set; }
        public string leadCity { get; set; }
    }

    [Serializable]
    public class Login
    {

        [Display(Name = nameof(LoginNameCaption), ResourceType = typeof(Login))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Login))]
        public string LoginName { get; set; }

        [Display(Name = nameof(PasswordCaption), ResourceType = typeof(Login))]
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(Login))]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public static string LoginNameCaption => DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Customer ID", "Customer ID");
        public static string PasswordCaption => DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Password", "Password");
        public static string Required => DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Required", "Please enter a value for {0}");
    }
}