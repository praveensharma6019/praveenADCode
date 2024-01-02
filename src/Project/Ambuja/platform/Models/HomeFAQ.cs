using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models
{
    public class HomeFAQ
    {

        public string SectionHeading { get; set; }
        public string Link { get; set; }
        public string LinkText { get; set; }
        public string linkTarget { get; set; }
        public GtmDataModel GtmData { get; set; }
        public List<FAQDataItem> Data { get; set; }
    }
}