using Sitecore.Data.Items;

namespace Sitecore.Feature.PageContent.Models
{
    public class QuickAccessFunc
    {
        public Item Item { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public int Level { get; set; }
        public QuickAccessFuncItems Children { get; set; }
        public string Target { get; set; }
        public bool ShowChildren { get; set; }
    }
}