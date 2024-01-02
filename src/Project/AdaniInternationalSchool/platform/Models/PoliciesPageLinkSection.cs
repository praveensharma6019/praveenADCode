using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{

    public class PoliciesPageLinkSection : BaseContentModel
    {
        public List<LinkSectionLinkItemModel> Links { get; set; } = new List<LinkSectionLinkItemModel>();
    }
}