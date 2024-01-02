using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.EV.Project.Models.BaseModel;

namespace Adani.EV.Project.Models
{
    public class HeaderNavBarModel : IId
    {
        public string Id { get; set; }
        public List<HeaderNavBarWidgetItem> widgetItems { get; set; }
    }

    public class HeaderNavBarWidgetItem :IId, IName, ICtaLink
    {   
        public string Id { get; set; }       
        public string CtaLink { get; set; }
        public string Name { get; set; }
    }
}