using System;

namespace Adani.BAU.AdaniUpdates.Project.Models
{
    public class CardItemModel
    {
        public string LinkUrl { get; set; }
        public string LinkTarget { get; set; }
        public string ImageSrc { get; set; }
        public string ImageAlt { get; set; }
        public DateTime Date { get; set; }
        public string DateString { get; set; }
        public string Title { get; set; }
        public string Summary { get; set; }
        public bool IsVideo { get; set; }
    }
}