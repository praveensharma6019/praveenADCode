using Project.MiningRenderingHost.Website.Models.Common;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models.ProjectDetails
{
    public class ProjectDetails : SectionHeadings
    {
        [SitecoreComponentField]
        public LinkData? CTALink { get; set; }
        [SitecoreComponentField]
        public string? CtaClass { get; set; }
        [SitecoreComponentField]
        public List<ProjectsList>? Projects { get; set; }
    }
    public class ProjectsList
    {
        [SitecoreComponentField]
        public LinkData? Link { get; set; }
        [SitecoreComponentField]
        public string? Year { get; set; }
        [SitecoreComponentField]
        public string? ProjectName { get; set; }
        [SitecoreComponentField]
        public string? LocationHeading { get; set; }
        [SitecoreComponentField]
        public string? Location { get; set; }
        [SitecoreComponentField]
        public string? MineralHeading { get; set; }
        [SitecoreComponentField]
        public string? MineralUsed { get; set; }
        [SitecoreComponentField]
        public string? ProjectDetails { get; set; }
        [SitecoreComponentField]
        public string? BgColor { get; set; }
        [SitecoreComponentField]
        public bool? IsSelected { get; set; }
        [SitecoreComponentField]
        public List<ItemDetaillist>? Items { get; set; }

    }
    public class ItemDetaillist
    {
        [SitecoreComponentField]
        public string? Title { get; set; }
        [SitecoreComponentField]
        public string? Description { get; set; }
    }

}
