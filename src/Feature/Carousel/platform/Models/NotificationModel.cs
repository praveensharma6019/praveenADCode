using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class NotificationModel
    {
        public string type { get; set; }
        public string description { get; set; }
        public bool moreButton { get; set; }
        public string ctaUrl { get; set; }
        public string iconcolor { get; set; }
        public string txtcolor { get; set; }
        public string bgcolor { get; set; }
        public bool mwebenable { get; set; }
        public bool desktopenable { get; set; }

    }
}