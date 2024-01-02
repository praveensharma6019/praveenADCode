using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class InvestmentGuidelineModel
    {
        public InvestmentGuidelines investmentGuidelines { get; set; }
    }
    public class InvestmentGuidelines
    {
        public string heading { get; set; }
        public string content { get; set; }
        public string readMore { get; set; }
    }
}