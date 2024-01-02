using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.EV.Project.Models
{
    public class SideNavbarModel
    {
        public Login Login { get; set; }=new Login();
        public Profile profile { get; set; }=new Profile();
        public List<SideNavbarTargetItem> targetItems { get; set; }=new List<SideNavbarTargetItem>();   
    }

    public class Login
    {
        public string ctaLink { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public string sideDrawerRightIcon { get; set; }
    }

    public class Profile
    {
        public string ctaLink { get; set; }
        public string image { get; set; }
        public string title { get; set; }
        public string sideDrawerRightIcon { get; set; }
    }

    public class SideNavbarTargetItem
    {
        public string title { get; set; }
        public string image { get; set; }
        public string sideDrawerRightIcon { get; set; }
        public List<SideNavbarItem> items { get; set; }=new List<SideNavbarItem>();
    }

    public class SideNavbarItem
    {
        public string title { get; set; }
        public string image { get; set; }
        public string ctaLink { get; set; }
    }
}