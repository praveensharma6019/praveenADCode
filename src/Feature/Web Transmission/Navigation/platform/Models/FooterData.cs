using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Models
{
    public class FooterData
    {
        public List<FooterDetails> footerDetails { get; set; }
    }

    public class FooterDetails
    {
        public List<SeoContents> seoContents { get; set; }
        public List<MainNavigations> MainNavigations { get; set; }
        public List<Payments> Payments { get; set; }
        public List<SocialLinks> SocialLinks { get; set; }
        public List<Download> Download { get; set; }
        public List<copyright> CopyRight { get; set; }
        public List<BottomNav> BottomNav { get; set; }
        public List<QuickLinks> QuickLinks { get; set; }
        public List<contactus> Contactus { get; set; }
    }

    public class SeoContents
    {        
        public string Heading { get; set; }
        public List<LinkTitlelist> items { get; set; }
    }
   
    public class MainNavigations
    {      
        public string Heading { get; set; }
        public List<LinkTitlelist> items { get; set; }
    }    
    public class Payments
    {
        public string Heading { get; set; }
        public List<ImageLink> items { get; set; }
    }
    public class SocialLinks
    {
        public string Heading { get; set; }
        public List<LinkURlIcon> items { get; set; }

    }
    public class Download
    {
        public string Heading { get; set; }
        public List<ImageLink> items { get; set; }
    }

    public class copyright
    {
        public string Heading { get; set; }
        public List<LinkTitlelist> items { get; set; }
    }

    public class contactus
    {
        public string Heading { get; set; }
        public List<LinkTitlelist> items { get; set; }
    }

    public class BottomNav
    {
        public string Heading { get; set; }
        public List<BottomNavFields> items { get; set; }
    }

    public class LinkTitlelist
    {
        public string LinkTitle { get; set; }
        public string LinkUrl { get; set; }
    }

    public class ImageLink
    {
        public string Image { get; set; }
        public string Link { get; set; }
    }

    public class LinkURlIcon
    {
        public string LinkTitle { get; set; }
        public string LinkUrl { get; set; }

        public string itemicon { get; set; }

    }

    public class BottomNavFields
    {
        public string Title { get; set; }
        public string ActiveImage { get; set; }
        public string ImagePath { get; set; }
        public string Link { get; set; }
    }
    public class QuickLinks
    {
        public string LinkTitle { get; set; }
        public string Description { get; set; }
        public string LinkUrl { get; set; }
        public string ItemIcon { get; set; }
    }
}