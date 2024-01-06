using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTransAlipurdar.Website.Models
{
    public class ContactDetail
    {
        public string Heading { get; set; }
        public string HTMLText { get; set; }
        public List<CityDropDown> CityDropDown { get; set; }
    }
    public class CityDropDown
    {
        public string Heading { get; set; }
        public List<CityDropDownItem> CityDropDownItem { get; set; }
    }
    public class CityDropDownItem
    {
        public string Heading { get; set; }
        public string Image { get; set; }
        public string ImageAltText { get; set; }
    }
}