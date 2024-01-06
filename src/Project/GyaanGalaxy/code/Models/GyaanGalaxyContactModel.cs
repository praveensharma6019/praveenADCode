using System;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;


namespace Sitecore.GyaanGalaxy.Website.Models
{
    public class GyaanGalaxyContactModel
    {
        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidName", ErrorMessageResourceType = typeof(GyaanGalaxyContactModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxyContactModel))]
        public string Name
        {
            get;
            set;
        }
        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(GyaanGalaxyContactModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxyContactModel))]
        public string MobileNo
        {
            get;
            set;
        }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(GyaanGalaxyContactModel))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(GyaanGalaxyContactModel))]
        public string EmailID
        {
            get;
            set;
        }

        public static string InvalidEmailAddress
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/GyaanGalaxy/Contact-Form/Invalid Email Address", "Please enter a valid email address");
            }
        }

        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Contact-Form/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }
        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Contact-Form/Invalid Name", "Please enter a valid Name");
            }
        }
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("GyaanGalaxy/Contact-Form/Required", "Please enter value for {0}");
            }
        }

        public string reResponse
        {
            get;
            set;
        }
        public string Message
        {
            get;
            set;
        }
        public string MessageType
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
        public string RegistrationNo
        {
            get;
            set;
        }

       
    }
}