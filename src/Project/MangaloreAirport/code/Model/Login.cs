using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.MangaloreAirport.Website.Model
{
    [Serializable]
    public class Login
    {
        [Display(Name = "LoginNameCaption", ResourceType = typeof(Login))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Login))]
        public string LoginName
        {
            get;
            set;
        }

        public static string LoginNameCaption
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/MangaloreAirport/Tender/Customer ID", "Customer ID");
            }
        }

        [DataType(DataType.Password)]
        [Display(Name = "PasswordCaption", ResourceType = typeof(Login))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Login))]
        public string Password
        {
            get;
            set;
        }

        public static string PasswordCaption
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/MangaloreAirport/Tender/Password", "Password");
            }
        }

        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/MangaloreAirport/Tender/Required", "Please enter a value for {0}");
            }
        }
        public string reResponse
        {
            get;
            set;
        }

        public Login()
        {
        }
    }
}