using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class SidebarModel
    {
        public string name { get; set; }
        public SideNavbarModel widgetItems { get; set; }=new SideNavbarModel();
        
    }
}