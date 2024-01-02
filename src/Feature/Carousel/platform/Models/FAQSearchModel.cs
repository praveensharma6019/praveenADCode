using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class FAQSearchModel
    {
        public string title { get; set; }

        public string description { get; set; }

        public string searchText { get; set; }

        public string itemNotFoundText { get; set; }
    }
}