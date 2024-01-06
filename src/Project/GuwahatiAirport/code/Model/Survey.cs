using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.GuwahatiAirport.Website.Model
{
    [Serializable]
    public class Survey
    {
        [Display(Name = "CANumberCaption", ResourceType = typeof(Survey))]
        [RegularExpression("^[0-9]{9,9}$", ErrorMessageResourceName = "InvalidCANumber", ErrorMessageResourceType = typeof(Survey))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Survey))]
        public string CANumber
        {
            get;
            set;
        }

        public static string CANumberCaption
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Register/CANumber", "CA Number");
            }
        }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "EmailCaption", ResourceType = typeof(Survey))]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(Survey))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Survey))]
        public string Email
        {
            get;
            set;
        }

        public static string EmailCaption
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Register/Email", "E-mail id");
            }
        }

        public static string FirstNameCaption
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Register/FirstName", "First Name");
            }
        }

        public string Frm
        {
            get;
            set;
        }

        public static string InvalidCANumber
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid CA Number", "Please enter a valid CA Number");
            }
        }

        public static string InvalidEmailAddress
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");
            }
        }

        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }

        [Display(Name = "MobileNumberCaption", ResourceType = typeof(Survey))]
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(Survey))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Survey))]
        public string MobileNo
        {
            get;
            set;
        }

        public static string MobileNumberCaption
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Register/MobileNumber", "Mobile No.");
            }
        }

        [Display(Name = "FirstNameCaption", ResourceType = typeof(Survey))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Survey))]
        public string Name
        {
            get;
            set;
        }

        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");
            }
        }

        public List<TypeofAppliance> TypeofApplianceList
        {
            get;
            set;
        }

        public Survey()
        {
            this.TypeofApplianceList = new List<TypeofAppliance>();
        }
    }
}