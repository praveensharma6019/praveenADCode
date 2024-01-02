using Project.MiningRenderingHost.Website.Models.Common;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models.ContactUs
{
    public class ContactUsPageBanner : ImageModel
    {
        [SitecoreComponentField]
        public  string? Heading { get; set; }
        [SitecoreComponentField]
        public  List<ContactUsBannerData>? BannerItems { get; set; }
    }
    public class ContactUsBannerData : ImageModel
    {
        [SitecoreComponentField]
        public  LinkData? Link { get; set; }
        [SitecoreComponentField]
        public  GtmDataModel? GtmData { get; set; }
    }
}
