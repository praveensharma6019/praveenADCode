using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models
{
    public class FooterModel
    {
        public List<FooterDetailsList> FooterDetails { get; set; }
    }
    public class FooterDetailsList
    {
        public List<MainNavigationsList> MainNavigations { get; set; }     
        public List<SocialLinksList> SocialLinks { get; set; }
        public List<MainNavigationsList> CopyRight { get; set; }
    }
    public class MainNavigationsList
    {
        public string Heading { get; set; }
        public List<MainNavigationsItemList> Items { get; set; }
    }

    public class MainNavigationsItemList
    {
        public string LinkTitle { get; set;}
        public string LinkUrl { get; set; }
        public string Target { get; set; }
    }

    public class SocialLinksList
    {
        public string Heading { get; set; }
        public List<SocialLinksItemList> Items { get; set; }
    }

    public class SocialLinksItemList
    {
        public string LinkTitle { get; set; }
        public string LinkUrl { get; set; }
        public string Itemicon { get; set; }
        public string Target { get; set; }
    }
   
}