using System.Collections.Generic;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class PageDescriptionModel : BaseContentModel
    {
        public PageDescriptionModel()
        {
            Banners = new List<BannerModel>();
        }

        public List<BannerModel> Banners { get; set; }
    }
}