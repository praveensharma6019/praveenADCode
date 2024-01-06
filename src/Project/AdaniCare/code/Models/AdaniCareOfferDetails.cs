using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniCare.Website.Models
{
    [Serializable]
    public class AdaniCareOfferDetails
    {
        public AdaniCareOfferDetails()
        {
        }
        public int Id { get; set; }
        public string AccountNumber { get; set; }
        public string reResponse { get; set; }
        public string MobileNumber { get; set; }
        public string OfferName { get; set; }
        public string OfferDesc { get; set; }
        public string OfferId { get; set; }
        public string OfferCode { get; set; }
        public string OfferCompany { get; set; }
        public string OfferLink { get; set; }
        public string ConsumerName { get; set; }
        public DateTime ClaimedDate { get; set; }

        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = "InvalidEmailAddress", ErrorMessageResourceType = typeof(AdaniCareContactUsForm))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniCareContactUsForm))]
        public string ClaimEmailAddress { get; set; }

        [RegularExpression("^[0-9]{10,10}$", ErrorMessageResourceName = "InvalidMobile", ErrorMessageResourceType = typeof(AdaniCareContactUsForm))]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(AdaniCareContactUsForm))]
        public string ClaimMobileNumber { get; set; }

        public static string InvalidEmailAddress
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("/AdaniCare/Contact-Form/Invalid Email Address", "Please enter a valid email address");
            }
        }

        public static string InvalidMobile
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("AdaniCare/Contact-Form/Invalid Mobile", "Please enter a valid Mobile Number");
            }
        }
        public static string Required
        {
            get
            {
                return DictionaryPhraseRepository.Current.Get("AdaniCare/Contact-Form/Required", "Please enter value");
            }
        }
    }
}