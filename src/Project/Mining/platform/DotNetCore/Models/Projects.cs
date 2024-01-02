using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models
{
    public class Projects
    {
        [SitecoreComponentField]
        public string? SubHeading { get; set; }
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public List<OurProjectsList>? OurProjectsdata { get; set; }
        
    }

    public class OurProjectsList
    {
        [SitecoreComponentField]
        public string? Description { get; set; }
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public string? SubHeading { get; set; }
        [SitecoreComponentField]
        public string? Image { get; set; }
        [SitecoreComponentField]
        public string? MobileImage { get; set; }
        [SitecoreComponentField]
        public string? TabletImage { get; set; }
        [SitecoreComponentField]
        public string? ImageAltText { get; set; }
        [SitecoreComponentField]
        public string? CTAText { get; set; }
        [SitecoreComponentField]
        public string? CTAText2 { get; set; }
        [SitecoreComponentField]
        public string? CTALink { get; set; }
        [SitecoreComponentField]
        public string? CTALink2 { get; set; }
    }
}
