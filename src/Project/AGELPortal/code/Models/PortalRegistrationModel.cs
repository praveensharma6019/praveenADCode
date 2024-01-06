using System;
using System.Linq;
using System.Web;
using Sitecore.Foundation.Dictionary.Repositories;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sitecore.AGELPortal.Website.Models
{
    [Serializable]
    public class PortalRegistrationModel
    {

        [RegularExpression("^[a-zA-Z][a-zA-Z ]*$", ErrorMessageResourceName = "InvalidName", ErrorMessageResourceType = typeof(PortalLoginModel))]
        [Required(ErrorMessage = "Name is Required")]
        public string name
        {
            get;
            set;
        }

        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(PortalLoginModel))]
        [Required(ErrorMessage = "Mobile is Required")]
        public string mobile
        {
            get;
            set;
        }
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(PortalLoginModel))]
        [Required(ErrorMessage = "Email is Required")]
        public string email
        {
            get;
            set;
        }

        public static string InvalidEmailAddress
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/Invalid Email Address", "Please enter a valid email address.");
            }
        }

        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("AGELPortal/Form/Invalid Mobile", "Please enter a valid Mobile Number.");
            }
        }
        public static string InvalidName
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("AGELPortal/Form/Invalid Name", "Please enter a valid Name.");
            }
        }
        public Guid Id
        {
            get;
            set;
        }

        public string UserValidation { get; set; }
        [Required(ErrorMessage = "OTP is Required")]
        public string OTP { get; set; }


        public bool islog { get; set; }

        public string password { get; set; }

        [NotMapped]
        [Compare("password")]
        public string confirm_password { get; set;}
        [NotMapped]
        [Compare("confirm_password")]
       
        public string new_password { get; set; }

        public List<AGELPortalContent> video_contents;
        public List<AGELPortalContent> document_contents;
        public List<AGELPortalContent> created_date;
        public List<AGELPortalUserLoginHistory> User_visitors;
       

        public int usercount;

      
    }
}