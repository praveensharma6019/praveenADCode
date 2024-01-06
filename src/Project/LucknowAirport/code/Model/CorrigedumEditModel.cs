using Sitecore.LucknowAirport.Website;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Web;

namespace Sitecore.LucknowAirport.Website.Model
{
    [Serializable]
    public class CorrigedumEditModel
    {
        public string ContentType
        {
            get;
            set;
        }

        public List<LKO_CorrigendumDocument> CorrigendumDocument
        {
            get;
            set;
        }

        public List<LKO_CorrigendumTenderMapping> CorrigendumTenderMapping
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
        public HttpPostedFileBase[] Files
        {
            get;
            set;
        }

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

        [Required]
        public bool? Status
        {
            get;
            set;
        }

        public List<Check> TenderList
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

        public CorrigedumEditModel()
        {
        }
    }
}