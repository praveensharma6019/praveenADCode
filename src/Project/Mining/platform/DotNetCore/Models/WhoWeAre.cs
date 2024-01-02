using Project.MiningRenderingHost.Website.Models.Common;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;
using Sitecore.LayoutService.Client.Response;

namespace Project.MiningRenderingHost.Website.Models
{
    public class WhoWeAre : SectionHeadings
    {

        [SitecoreComponentField]
        public LinkData? CtaLink { get; set; }          
        [SitecoreComponentField]
        public List<WhoWeAreDataList>? WhoWeAreCards { get; set; }
    }
    public class WhoWeAreDataList 
    {
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public ImageData? Icon { get; set; }
        [SitecoreComponentField]
        public string? ShortDescription { get; set; }     
       
    }
}
