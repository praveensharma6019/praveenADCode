using System;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Sitecore.GyaanGalaxy.Website.Helper;

namespace Sitecore.GyaanGalaxy.Website.Models
{
    public class GyaanGalaxyRegistrationModal
    {

        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidStudentName", ErrorMessageResourceType = typeof(GyaanGalaxyRegistrationModal))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxyRegistrationModal))]
        public string StudentName
        {
            get;
            set;
        }
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(GyaanGalaxyRegistrationModal))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxyRegistrationModal))]
        public string MobileNo
        {
            get;
            set;
        }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(GyaanGalaxyRegistrationModal))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxyRegistrationModal))]
        public string EmailID
        {
            get;
            set;
        }

        public static string InvalidEmailAddress
        {
            get
            {
              
                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Registration-Form/Invalid Email Address", "Please enter a valid email address");
            }
        }

        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Registration-Form/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }
        public static string InvalidStudentName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Registration-Form/Invalid Student Name", "Please enter a valid Name");
            }
        }
        public static string InvalidAge
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Registration-Form/Invalid Age", "Please enter a valid age");
            }
        }
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Registration-Form/Required", "Please enter value for {0}");
            }
        }

        public string reResponse
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxyRegistrationModal))]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceName = "InvalidAge", ErrorMessageResourceType = typeof(GyaanGalaxyRegistrationModal))]
        public string Age
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxyRegistrationModal))]
        public string ClassOrGrade
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxyRegistrationModal))]
        public string HouseAddress
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxyRegistrationModal))]
        public string SchoolName
        {
            get;
            set;
        }
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxyRegistrationModal))]
        public string SchoolAddress
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

        public Guid Id
        {
            get;
            set;
        }
        
    }
}