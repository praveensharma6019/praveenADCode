using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{

    public class MandatoryPublicDisclosure: BaseContentModel
    {
        public List<MandatoryPublicDisclosureLinkItem> Links { get; set; }
    }
}