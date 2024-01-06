using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;

namespace Sitecore.MangaloreAirport.Website.Model
{
   

    [Serializable]
    public class Survey
    {
        public Survey()
        {
            this.TypeofApplianceList = new List<TypeofAppliance>();
        }

        [Display(Name = "FirstNameCaption", ResourceType = typeof(Survey)), Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Survey))]
        public string Name { get; set; }

        [Display(Name = "CANumberCaption", ResourceType = typeof(Survey)), Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Survey)), RegularExpression("^[0-9]{9,9}$", ErrorMessageResourceName = "InvalidCANumber", ErrorMessageResourceType = typeof(Survey))]
        public string CANumber { get; set; }

        [Display(Name = "EmailCaption", ResourceType = typeof(Survey)), Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Survey)), EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(Survey)), DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Display(Name = "MobileNumberCaption", ResourceType = typeof(Survey)), Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Survey)), RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(Survey))]
        public string MobileNo { get; set; }

        public List<TypeofAppliance> TypeofApplianceList { get; set; }

        public string Frm { get; set; }

        public static string FirstNameCaption =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Register/FirstName", "First Name");

        public static string EmailCaption =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Register/Email", "E-mail id");

        public static string Required =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Register/Required", "Please enter a value for {0}");

        public static string InvalidEmailAddress =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Email Address", "Please enter a valid email address");

        public static string InvalidCANumber =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid CA Number", "Please enter a valid CA Number");

        public static string CANumberCaption =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Register/CANumber", "CA Number");

        public static string MobileNumberCaption =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Register/MobileNumber", "Mobile No.");

        public static string InvalidMobile =>
            DictionaryPhraseRepository.Current.Get("/Accounts/Register/Invalid Mobile", "Please enter a valid Mobile Number");
    }
}

