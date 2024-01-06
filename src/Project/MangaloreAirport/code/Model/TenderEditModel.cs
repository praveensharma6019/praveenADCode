using Sitecore.MangaloreAirport.Website;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;

namespace Sitecore.MangaloreAirport.Website.Model
{
    [Serializable]
    public class TenderEditModel
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

        public string Estimated_Cost
        {
            get;
            set;
        }

        [Display(Name = "Browse Tender File")]
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

        public List<TenderDocument> PQDocuments
        {
            get;
            set;
        }

        [Display(Name = "Browse PQ File")]
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

        public List<TenderDocument> TenderDocuments
        {
            get;
            set;
        }

        public TenderEditModel()
        {
        }
    }
}