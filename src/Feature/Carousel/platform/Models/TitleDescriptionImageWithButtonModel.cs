using Sitecore.Publishing.Pipelines.PublishItem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class TitleDescriptionImageWithButtonModel
    {
        public string title { get; set; }
        public string subTitle { get; set; }
        public string desktopImage { get; set; }
        public string mobileImage { get; set; }
        public string buttonText { get; set; }
        public string description { get; set; }
        public bool isActive { get; set; }
        public PopupDetails popupdetails { get; set; }
        public List<Domains> domainsList { get; set; }

        public class PopupDetails
        {
            public string title { get; set; }
            public string subTitle { get; set; }
            public string image { get; set; }
            public string buttonText { get; set; }
        }

        public class Domains
        {
            public string label { get; set; }
            public string value { get; set; }
        }

        public TitleDescriptionImageWithButtonModel()
        {
            popupdetails = new PopupDetails();
            domainsList = new List<Domains>();
        }
    }
}