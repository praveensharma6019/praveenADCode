using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.LucknowAirport.Website.Model
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
                return DictionaryPhraseRepository.Current.Get("/LucknowAirport/Tender/Customer ID", "Customer ID");
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
        public string reResponse { get; set; }

        public static string PasswordCaption
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/LucknowAirport/Tender/Password", "Password");
            }
        }

        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/LucknowAirport/Tender/Required", "Please enter a value for {0}");
            }
        }

        public Login()
        {
        }
    }
}