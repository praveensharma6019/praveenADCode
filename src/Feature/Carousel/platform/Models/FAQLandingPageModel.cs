using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class Category
    {
        public string categoryTitle { get; set; }

        public List<Faq> faqs { get; set; }

        public string seeAllLink { get; set; }

        public Category()
        {
            faqs = new List<Faq>();
        }
    }

    public class Faq
    {
        public string question { get; set; }

        public string redirectionLink { get; set; }
    }
}