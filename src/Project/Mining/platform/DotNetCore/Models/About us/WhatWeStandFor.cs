using Project.MiningRenderingHost.Website.Models.Common;
using Sitecore.AspNet.RenderingEngine.Binding.Attributes;

namespace Project.MiningRenderingHost.Website.Models.About_us
{
    public class WhatWeStandFor
    {
        [SitecoreComponentField]
        public string? Heading { get; set; }
        [SitecoreComponentField]
        public List<CardsList>? Cards { get; set; }
    }
    public class CardsList : ImageModel
    {
        [SitecoreComponentField]
        public string? Heading { get; set; }        
        [SitecoreComponentField]
        public string? Description { get; set; }
    }
}
