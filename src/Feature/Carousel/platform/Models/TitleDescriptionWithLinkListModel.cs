using Sitecore.Publishing.Pipelines.PublishItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class TitleDescriptionWithLinkListModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public string AutoId { get; set; }
        public string UniqueId { get; set; }
        public string MobileImage { get; set; }
        public string linkTarget { get; set; }
        public bool isAgePopup { get; set; }
        public GTMTags tags { get; set; }
    }
}