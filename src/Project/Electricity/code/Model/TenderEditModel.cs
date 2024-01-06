using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
namespace Sitecore.Electricity.Website.Model
{
    [Serializable]
    public class TenderEditModel
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
        public string Adv_Date { get; set; }
        [Required]
        [Display(Name = "Close Date")]
        public string Closing_Date { get; set; }
        public string Location { get; set; }
        public string Estimated_Cost { get; set; }
        public string Cost_of_EMD { get; set; }
        [Required(ErrorMessage = "- Select Status -")]
        public string Status { get; set; }
        public bool OnHold { get; set; }
        public List<TenderDocument> TenderDocuments { get; set; }
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] Files { get; set; }

    }
}