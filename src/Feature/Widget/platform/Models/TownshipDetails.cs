using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Widget.Platform.Models
{
    public class Shantigram
    {
        public string src { get; set; }
        public string alt { get; set; }
        public string title { get; set; }
        public string heading { get; set; }
        public string subheading { get; set; }
        public string thumbImg { get; set; }
        public string class1 { get; set; }
        public string class2 { get; set; }
    }

    public class InfoTabLink
    {
        public string link { get; set; }
        public string linktitle { get; set; }
        public string title { get; set; }
        public string titlelink { get; set; }
    }

    public class InfoTabs
    {
        public List<InfoTabLink> data { get; set; }
    }

    public class InfoTabsData
    {
        public InfoTabs infoTabs { get; set; }
    }


    public class ShantigramData
    {
        public Shantigram shantigram { get; set; }
    }
}