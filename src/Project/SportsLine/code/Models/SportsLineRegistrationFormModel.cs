using System;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.SportsLine.Website.Helper;

namespace Sitecore.SportsLine.Website.Models
{
    public class SportsLineRegistrationFormModel
    {
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidName", ErrorMessageResourceType = typeof(SportsLineRegistrationFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SportsLineRegistrationFormModel))]
        public string Name
        {
            get;
            set;
        }
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidLastName", ErrorMessageResourceType = typeof(SportsLineRegistrationFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SportsLineRegistrationFormModel))]
        public string LastName
        {
            get;
            set;
        }
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(SportsLineRegistrationFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SportsLineRegistrationFormModel))]
        public string MobileNo
        {
            get;
            set;
        }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(SportsLineRegistrationFormModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SportsLineRegistrationFormModel))]
        public string EmailID
        {
            get;
            set;
        }

        public static string InvalidEmailAddress
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/SportsLine/Registration-Form/Invalid Email Address", "Please enter a valid email address");
            }
        }

        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("SportsLine/Registration-Form/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }
        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("SportsLine/Registration-Form/Invalid Name", "Please enter a valid Name");
            }
        }
        public static string InvalidLastName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("SportsLine/Registration-Form/Invalid LastName", "Please enter a valid LastName");
            }
        }
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("SportsLine/Registration-Form/Required", "Please enter value for {0}");
            }
        }

        public string reResponse
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SportsLineRegistrationFormModel))]
        public string Address
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SportsLineRegistrationFormModel))]
        public string Gender
        {
            get;
            set;
        }
       
        public string SportsType
        {
            get;
            set;
        }
       
        public string PageInfo
        {
            get;
            set;
        }
        public DateTime SubmitOnDate
        {
            get;
            set;
        }


        public string SubmittedBy
        {
            get;
            set;
        }
       

        public string FormName
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SportsLineRegistrationFormModel))]
        public DateTime DateofBirth
        {
            get;
            set;
        }

        public Guid Id
        {
            get;
            set;
        }
        public string RegistrationNo
        {
            get;
            set;
        }
        public List<CheckBoxes> SportsTypeList { get; set; }
       
        public SportsLineRegistrationFormModel()
        {
            SportsLineRegistrationFormHelper helper = new SportsLineRegistrationFormHelper();
           SportsTypeList = helper.GetSectorServedList();

        }
        public class CheckBoxes
        {

            public string Text { get; set; }
            public string Value { get; set; }
            public bool Checked { get; set; }
        }
    }
}