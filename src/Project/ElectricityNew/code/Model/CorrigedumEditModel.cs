using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Sitecore.ElectricityNew.Website.Model
{
    [Serializable]
    public class CorrigedumEditModel
    {
        public Guid Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Date { get; set; }
        [Required]
        public bool? Status { get; set; }

        public string FileName { get; set; }
        public string DocumentPath { get; set; }
        public List<Check> TenderList { get; set; }
        public List<CorrigendumDocument> CorrigendumDocument { get; set; }
        public List<CorrigendumTenderMapping> CorrigendumTenderMapping { get; set; }
        [Display(Name = "Browse File")]
        public HttpPostedFileBase[] Files { get; set; }


    }
}