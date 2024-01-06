namespace Sitecore.Feature.Navigation.Models
{
    using Sitecore.Data.Items;

    public class NavigationItem
    {
        public Item Item { get; set; }
        public string Url { get; set; }
        public bool IsActive { get; set; }
        public int Level { get; set; }
        public NavigationItems Children { get; set; }
        public string Target { get; set; }
        public bool ShowChildren { get; set; }
        public bool IsNewTab { get; set; }
        public string Summary { get; set; }
        public string IsMessage { get; set; }
        public bool IsExternalLink { get; set; }
        public string Link { get; set; }

        public string IconCss { get; set; }
        public string BGCSS { get; set; }

        public bool IsVisible { get; set; }
    }
}