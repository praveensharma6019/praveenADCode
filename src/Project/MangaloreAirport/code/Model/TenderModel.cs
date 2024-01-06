using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;

namespace Sitecore.MangaloreAirport.Website.Model
{
    [Serializable]
    public class TenderModel
    {
        [Display(Name = "Advertise Date")]
        [Required]
        public DateTime Adv_Date
        {
            get;
            set;
        }

        [Required]
        public string Business
        {
            get;
            set;
        }

        [Display(Name = "Close Date")]
        [Required]
        public DateTime Closing_Date
        {
            get;
            set;
        }

        public string Cost_of_EMD
        {
            get;
            set;
        }

        [Required]
        public string Description
        {
            get;
            set;
        }

        public string DocumentPath
        {
            get;
            set;
        }

        public string Estimated_Cost
        {
            get;
            set;
        }

        public string FileName
        {
            get;
            set;
        }

        [Display(Name = "Browse File")]
        [Required(ErrorMessage = "Please select file.")]
        public HttpPostedFileBase[] Files
        {
            get;
            set;
        }

        [Required]
        public Guid Id
        {
            get;
            set;
        }

        public string Location
        {
            get;
            set;
        }

        [Display(Name = "NIT No")]
        [Required]
        public string NITNo
        {
            get;
            set;
        }

        [Required(ErrorMessage = "- Select Status -")]
        public string Status
        {
            get;
            set;
        }

        [Required(ErrorMessage = "Please enter support email address.")]
        public string SupportEmailAddress
        {
            get;
            set;
        }

        public TenderModel()
        {
        }
    }
}