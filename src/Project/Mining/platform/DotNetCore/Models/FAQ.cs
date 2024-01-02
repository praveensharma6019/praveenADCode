using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
namespace Project.MiningRenderingHost.Website.Models
{
    public class FAQ
    {
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public string? CTAText { get; set; }
        [SitecoreComponentField]
        public string? CTALink { get; set; }
        [SitecoreComponentField]
        public List<FAQList>? FaqData { get; set; }          
    }

    public class FAQList
    {
        [SitecoreComponentField]
        public string? Question { get; set; }
        [SitecoreComponentField]
        public string? Answer { get; set; }
    }
}
