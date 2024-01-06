namespace Sitecore.Electricity.Website.Model
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Sitecore.Foundation.Dictionary.Repositories;

    [Serializable]
    public class ITSRFormListing
    {
        public string FormId { get; set; }
        public string Title { get; set; }
        public string TenderNo { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
    }

   
}