using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class SchoolInfoModel
    {
        public SchoolInfoModel()
        {
            Social = new List<LinkItemModel>();
            Contact = new List<FooterContactModel>();
        }

        public string SchoolLogo { get; set; }
        public string Alt { get; set; }
        public GtmDataModel GtmData { get; set; }
        public string PageType { get; set; }
        public List<LinkItemModel> Social { get; set; }
        public List<FooterContactModel> Contact { get; set; }
    }
}