
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Models
{
    public class FAQModel
    {
        public string title { get; set; }

        public List<FAQCard> list { get; set; }

        // Field added for the loyalty Module
        public string faqCtaText { get; set; }
        public string faqCtaURL { get; set; }
        public String faqHTML { get; set; }
    }

    public class FAQCard
    {
        public string title { get; set; }

        public string body { get; set; }
    }
}