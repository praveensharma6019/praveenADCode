using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Sitecore.Foundation.Dictionary.Repositories;

namespace Sitecore.Electricity.Website.Model
{
    [Serializable]
    public class TenderModel
    {
        [Required]
        public Guid Id { get; set; }
        [Required]
        [Display(Name = "NIT No")]
        public string NITNo { get; set; }
        [Required]
        public string Business { get; set; }
        [Required]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Advertise Date")]

        public DateTime Adv_Date { get; set; }
        [Required]
        [Display(Name = "Close Date")]

        public DateTime Closing_Date { get; set; }

        public string Location { get; set; }
        public string Estimated_Cost { get; set; }
        public string Cost_of_EMD { get; set; }
        [Required(ErrorMessage = "- Select Status -")]

        public string Status { get; set; }
        public string FileName { get; set; }

        public string DocumentPath { get; set; }

        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] Files { get; set; }
    }



    [Serializable]
    public class TenderCreateModel
    {
        [Required]
        public Guid Id { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(TenderCreateModel))]
        [Display(Name = "NIT No")]
        public string NITNo { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(TenderCreateModel))]
        public string Business { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(TenderCreateModel))]
        public string Description { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(TenderCreateModel))]
        [Display(Name = "Advertise Date")]
        public string Adv_Date { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(TenderCreateModel))]
        [Display(Name = "Close Date")]
        public string Closing_Date { get; set; }

        public string Location { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(TenderCreateModel))]
        public string Estimated_Cost { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(TenderCreateModel))]
        public string Cost_of_EMD { get; set; }

        //[Required(ErrorMessage = "- Select Status -")]
        public string Status { get; set; }
        public string FileName { get; set; }

        public string DocumentPath { get; set; }

        [Required(ErrorMessage = "Please select file.")]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] Files { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(TenderCreateModel))]
        [Display(Name = "Buyer Name")]
        public string BuyerName { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(TenderCreateModel))]
        [Display(Name = "Buyer EmailId")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(TenderCreateModel))]
        public string BuyerEmailId { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(TenderCreateModel))]
        [Display(Name = "Lead Buyer Name")]
        public string LeadBuyerName { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(TenderCreateModel))]
        [Display(Name = "Lead Buyer EmailId")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(TenderCreateModel))]
        public string LeadBuyerEmailId { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(TenderCreateModel))]
        [Display(Name = "User Name")]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(TenderCreateModel))]
        [Display(Name = "User Email Id")]
        [DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessageResourceName = nameof(InvalidEmailAddress), ErrorMessageResourceType = typeof(TenderCreateModel))]
        public string UserEmailId { get; set; }

        public static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/PAN Registration/Required", "Please enter a value.");
        public static string InvalidEmailAddress => DictionaryPhraseRepository.Current.Get("/Accounts/PAN Registration/Invalid Email Address", "Please enter a valid email address");
    }
}