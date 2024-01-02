using Adani.EV.Project.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class LegalNavbar
    {
        public string id { get; set; }
        public string title { get; set; }
        public string type { get; set; }
        public string imageSrc { get; set; }
        public List<LegalNavbarwidgetItems> legalNavbarwidgetItems { get; set; }
    }

    public class LegalNavbarwidgetItems
    {
        public string id { get; set; }
        public string title { get; set; }
    }
}


