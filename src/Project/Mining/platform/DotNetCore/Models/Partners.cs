using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models
{
    public class Partners
    {
        [SitecoreComponentField]
        public string? CtaText { get; set; }
        [SitecoreComponentField]
        public string? CtaLink { get; set; }
        [SitecoreComponentField]
        public string? Description { get; set; }
        [SitecoreComponentField]
        public string? SubHeading { get; set; }
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public List<OurPartnersList>? OurPartnersData { get; set; }
    }

    public class OurPartnersList
    {
        [SitecoreComponentField]
        public string? Image { get; set; }
        [SitecoreComponentField]
        public string? MobileImage { get; set; }
        [SitecoreComponentField]
        public string? TabletImage { get; set; }
        [SitecoreComponentField]
        public string? ImageAltText { get; set; }
    }
}
