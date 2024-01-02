using Project.MiningRenderingHost.Website.Models.Common;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models
{
    public class OurAccreditation
    {
        [SitecoreComponentField]
        public LinkData? CTALink { get; set; }
        [SitecoreComponentField]
        public string? Description { get; set; }
        [SitecoreComponentField]
        public string? SubHeading { get; set; }
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public string? Class { get; set; }
        [SitecoreComponentField]
        public List<OurAccreditationlist>? LogoImages { get; set; }

        public class OurAccreditationlist : ImageModel
        {


        }

    }
}
