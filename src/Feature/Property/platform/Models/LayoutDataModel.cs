using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class LayoutDataModel
    {
        public LayoutData layoutData { get; set; }
    }

    public class LayoutData
    {
        public string projectTitle { get; set; }
        public string projectConfiguration { get; set; }
        public string projectPossesionInfo { get; set; }
        public string planAVisitLabel { get; set; }
        public string bookNowLabel { get; set; }
        public string enquireNowLabel { get; set; }
    }

}