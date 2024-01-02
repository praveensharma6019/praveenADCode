using System.Collections.Generic;

namespace Adani.SuperApp.Realty.Feature.JSSComponents.Platform.Models
{
    public class HomeFAQ
    {
        public string ViewAllLink { get; set; }
        public string ViewAllLabel { get; set; }
        public string SectionHeading { get; set; }
        public string Target { get; set; }
        public GtmDataModel GtmData { get; set; }
        public List<FAQDataItem> Data { get; set; }
    }
}