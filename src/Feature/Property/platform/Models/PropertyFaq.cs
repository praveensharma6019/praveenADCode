using System.Collections.Generic;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class Faq
    {
        public string title { get; set; }
        public string body { get; set; }
    }

    public class FaqData
    {
        public string id { get; set; }
        public string heading { get; set; }
        public string seeAll { get; set; }
        public string faqLink { get; set; }
        public List<Faq> faqs { get; set; }
    }

    public class PropertyFaq
    {
        public FaqData faqData { get; set; }
    }

}