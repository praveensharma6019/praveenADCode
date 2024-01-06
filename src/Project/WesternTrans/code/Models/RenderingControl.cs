using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.WesternTrans.Website.Models
{
    public class RenderingControl
    {
        public string DataSource { get; set; }
        public string refItem { get; set; }
        public string ControllerAction { get; set; }
    }
}