using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;

namespace Sitecore.LucknowAirport.Website.Model
{
    [Serializable]
    public class TenderCreateModel
    {
        [Display(Name = "Advertise Date")]
        [Required]
        public string Adv_Date
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
        public string Closing_Date
        {
            get;
            set;
        }

        public string ContentType
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

        public byte[] DocData
        {
            get;
            set;
        }

        public string DocumentPath
        {
            get;
            set;
        }

        public string EnvelopUser1email
        {
            get;
            set;
        }

        public List<EnvelopName> EnvelopUser1EnvelopNameCheckboxs
        {
            get;
            set;
        }

        public string EnvelopUser1Mobile
        {
            get;
            set;
        }

        public string EnvelopUser1Name
        {
            get;
            set;
        }

        public string EnvelopUser2email
        {
            get;
            set;
        }

        public List<EnvelopName> EnvelopUser2EnvelopNameCheckboxs
        {
            get;
            set;
        }

        public string EnvelopUser2Mobile
        {
            get;
            set;
        }

        public string EnvelopUser2Name
        {
            get;
            set;
        }

        public string EnvelopUser3email
        {
            get;
            set;
        }

        public List<EnvelopName> EnvelopUser3EnvelopNameCheckboxs
        {
            get;
            set;
        }

        public string EnvelopUser3Mobile
        {
            get;
            set;
        }

        public string EnvelopUser3Name
        {
            get;
            set;
        }

        public string EnvelopUser4email
        {
            get;
            set;
        }

        public List<EnvelopName> EnvelopUser4EnvelopNameCheckboxs
        {
            get;
            set;
        }

        public string EnvelopUser4Mobile
        {
            get;
            set;
        }

        public string EnvelopUser4Name
        {
            get;
            set;
        }

        public string EnvelopUser5email
        {
            get;
            set;
        }

        public List<EnvelopName> EnvelopUser5EnvelopNameCheckboxs
        {
            get;
            set;
        }

        public string EnvelopUser5Mobile
        {
            get;
            set;
        }

        public string EnvelopUser5Name
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

        [Display(Name = "Browse Tender Files")]
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

        public bool IsPQDoc
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

        public bool PQApprovalRequired { get; set; } = true;

        [Display(Name = "Browse PQ Files")]
        [Required(ErrorMessage = "Please select file.")]
        public HttpPostedFileBase[] PQFiles
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

        [Required(ErrorMessage = "- Select Tender Type -")]
        public string TenderType
        {
            get;
            set;
        }

        public TenderCreateModel()
        {
        }
    }
}