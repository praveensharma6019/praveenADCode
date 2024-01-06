using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTransAlipurdar.Website.Models
{
    public class Header
    {
        public string Image { get; set; }
        public string ImageAltText { get; set; }
        public string Link { get; set; }
        public List<Navigation> Navigation { get; set; }
    }
}