using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.Electricity.Website.Model
{
    [Serializable]
    public class Check
    {
        public Guid Id { get; set; }

        public string NITNo { get; set; }

        public string Business { get; set; }

        public string Description { get; set; }

        public DateTime Adv_Date { get; set; }

        public DateTime Closing_Date { get; set; }

        public string Location { get; set; }

        public string Status { get; set; }
        public string FileName { get; set; }

        public string DocumentPath { get; set; }

        public HttpPostedFileBase[] Files { get; set; }

        public bool IsChecked { get; set; }
    }
    [Serializable]
    public class CorrigendumModel
    {
        public List<Check> CheckBoxes { get; set; }
        [Required]
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Date { get; set; }
        //[DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd-MM-yyyy}")]
        public DateTime FormatedDate { get; set; }
        [Required]
        public bool? Status { get; set; }
        public string FileName { get; set; }
        public string DocumentPath { get; set; }

        [Required(ErrorMessage = "Select File to upload")]
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] Files { get; set; }

        public List<TenderDetails> NITPRNo { get; set; }

    }
}