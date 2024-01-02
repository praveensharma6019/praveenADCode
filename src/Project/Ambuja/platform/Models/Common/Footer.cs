using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AmbujaCement.Website.Models.Common
{
    public class Footer
    {
        public FooterData FooterData { get; set; }
        public StickyNavData StickyNavData { get; set; }
    }

    public class FooterData
    {
        public ImageModel BackgroundImage { get; set; }
        public BottomLink bottomLinks { get; set; }
        public List<MainNavigation> MainNavigations { get; set; }
        public string BuCopyright { get; set; }
        public List<SocialLink> SocialLinks { get; set; }
        public Logo Logo { get; set; }
        public List<CopyRight> CopyRight { get; set; }
    }

    public class StickyNavData
    {
        
        public string defaultActiveTab { get; set; }
        public List<Tab> Tabs { get; set; }
        public NavData NavData { get; set; }
        public List<Product> Products { get; set; }
    }
  

    public class ItemModel : LinkModel
    {
      
        public GtmDataModel GtmData { get; set; }
        public string Itemicon { get; set; }
        public string IconName { get; set; }
        public string Label { get; set; }
    }

    public class Link : LinkModel
    {
        public string Description { get; set; }
        public string IconName { get; set; }
        public GtmDataModel GtmData { get; set; }
    }

    public class Logo : ImageModel
    {
        public string ImageTitle { get; set; }
    }

    public class MainNavigation
    {
        public string Heading { get; set; }
        public List<ItemModel> Items { get; set; }
    }

    public class NavData
    {
        public string Heading { get; set; }
        public string Description { get; set; }
        public string HeadingIcon { get; set; }
        public OtherData OtherData { get; set; }
    }

    public class OtherData
    {
        public string Heading { get; set; }
        public List<ItemModel> Items { get; set; }
    }

    public class Product
    {
        public string ProductLabel { get; set; }
        public string ImageSource { get; set; }
        public string ImageAlt { get; set; }
        public string ActiveId { get; set; }
        public List<SubProduct> SubProduct { get; set; }
    }


    public class SocialLink
    {
        public string Heading { get; set; }
        public List<ItemModel> Items { get; set; }
    }



    public class SubProduct : LinkModel
    {
       
        public GtmDataModel GtmData { get; set; }
    }

    public class Tab : LinkModel
    {
        public string Label { get; set; }
        public string ActiveId { get; set; }
        public string Icon { get; set; }
        public bool IsOffcanvas { get; set; }
        public GtmDataModel GtmData { get; set; }
    }

    public class LinkModel
    {
        public string Link { get; set; }
        public string LinkText { get; set; }
        public string LinkTarget { get; set; }
    }
    public class CopyRight
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public List<ItemModel> Items { get; set; }
    }
    public class BottomLink
    {
        public List<BottomLinkItems> links { get; set; }
    }
    public class BottomLinkItems
    {
        public string LinkText { get; set; }
        public string Link { get; set; }
        public string LinkTarget { get; set; }
        public string Description { get; set; }
        public string IconName { get; set; }
        public GtmDataModel GtmData { get; set; }
    }

}