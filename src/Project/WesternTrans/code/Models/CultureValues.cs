using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTrans.Website.Models
{
    public class CultureValues
    {    
        public List<CultureValuesData> Data { get; set; }
    }
    public class CultureValuesData
    {
        public string Heading { get; set; }
        public List<CultureValuesDataItems> CultureValuesItems { get; set; }
    }

    public class CultureValuesDataItems
    {
        public string Image { get; set; }
        public string ImageAltText { get; set; }
        public string Heading { get; set; }
        public string SubHeading { get; set; }
    }
}