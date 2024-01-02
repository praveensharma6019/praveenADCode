using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class LivingTheGoodLifeDataModel
    {
        public LivingTheGoodLifeData livingTheGoodLifeData { get; set; }
    }
  
    public class LivingTheGoodLifeData
    {
        public string heading { get; set; }
        public List<TestimonialDatum> testimonialData { get; set; }
    }

    public class TestimonialDatum
    {
        public string isVideoTestimonial { get; set; }
        public string useravtar { get; set; }
        public string useravtaralt { get; set; }
        public string useravtartitle { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string author { get; set; }
        public string propertylocation { get; set; }
        public string iframesrc { get; set; }
        public string SEOName { get; set; }
        public string SEODescription { get; set; }
        public string UploadDate { get; set; }
    }


}