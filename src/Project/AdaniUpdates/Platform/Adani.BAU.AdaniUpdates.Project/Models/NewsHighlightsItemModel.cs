using System;
using System.Web;

namespace Adani.BAU.AdaniUpdates.Project.Models
{
    public class NewsHighlightsItemModel
    {
        public string LinkUrl { get; set; }
        public string LinkTarget { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }

        public string GetFileUrl()
        {
            if (LinkUrl.StartsWith("https://"))
                return LinkUrl;

            return $"https://{ HttpContext.Current.Request.Url.Host}{LinkUrl}";
        }
    }
}