using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;

namespace Sitecore.GuwahatiAirport.Website.Model
{
    [Serializable]
    public class CorrigendumModel
    {
        public List<Check> CheckBoxes
        {
            get;
            set;
        }

        public string ContentType
        {
            get;
            set;
        }

        [Required]
        public string Date
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

        public string FileName
        {
            get;
            set;
        }

        [Display(Name = "Browse File")]
        [Required(ErrorMessage = "Select File to upload")]
        public HttpPostedFileBase[] Files
        {
            get;
            set;
        }

        public DateTime FormatedDate
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

        public List<TenderDetails> NITPRNo
        {
            get;
            set;
        }

        [Required]
        public bool? Status
        {
            get;
            set;
        }

        [Required]
        public string Title
        {
            get;
            set;
        }

        public CorrigendumModel()
        {
        }
    }
}