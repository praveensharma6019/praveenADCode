using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models
{


    public class CollapseItem
    {
        public string LinkText { get; set; }
        public GtmData GtmData { get; set; }
        public string Link { get; set; }
        public string LinkTarget { get; set; }
        public string ItemLeftIcon { get; set; }
        public string ItemSubText { get; set; }
        public bool IsActive { get; set; }
    }

    public class GtmData
    {
        public string Event { get; set; }
        public string Category { get; set; }
        public string SubCategory { get; set; }
        public string Title { get; set; }
        public string Label { get; set; }
        public string PageType { get; set; }
    }

    public class HamburgerMenuDatum
    {
        public string LinkText { get; set; }
        public string ItemSubText { get; set; }
        public string ItemLeftIcon { get; set; }
        public string LinkTarget { get; set; }
        public string Link { get; set; }
        public string HeaderLeftIcon { get; set; }
        public string HeaderRightIcon { get; set; }
        public GtmData GtmData { get; set; }
        public List<HamburgerMenuItem> Items { get; set; }
        public List<HamburgerMenuItem> collapseItems { get; set; }

    }

    public class HeaderModel
    {

        public string HeaderLogo { get; set; }
        public string Logo { get; set; }
        public string MobileLogo { get; set; }
        public string BuLink { get; set; }
        public string LinkTarget { get; set; }
        public string BuLogoAltText { get; set; }
        public bool IsAbsolute { get; set; }
        public string AddOnClass { get; set; }
        public string PageHeading { get; set; }
        public List<NavDatum> NavData { get; set; }
        //   public MenuData MenuData { get; set; }
        public List<HamburgerMenuDatum> HamburgerMenuData { get; set; }
        public List<TopbarList> TopbarList { get; set; }
    }
    public class HamburgerMenuItem
    {
        public string LinkText { get; set; }
        public GtmData GtmData { get; set; }
        public string Link { get; set; }
        public string LinkTarget { get; set; }
        public string ItemLeftIcon { get; set; }
        public string HighlightLabel { get; set; }
        public string ItemRightIcon { get; set; }
        public List<CollapseItem> collapseItems { get; set; }
    }
    public class Item
    {

        public string LinkText { get; set; }
        public GtmData GtmData { get; set; }
        public string Link { get; set; }
        public string LinkTarget { get; set; }
        public List<SubMenu> SubMenu { get; set; }
        //    public List<CollapseItem> CollapseItems { get; set; }
        //public string ItemLeftIcon { get; set; }
        //public string HighlightLabel { get; set; }
        //public string ItemRightIcon { get; set; }

        //public string ItemSubText { get; set; }
        //public bool IsActive { get; set; }
    }

    //public class MenuData
    //{
    //    public List<HamburgerMenuDatum> HamburgerMenuData { get; set; }
    //}

    public class NavDatum
    {

        public string Link { get; set; }
        public string LinkText { get; set; }
        public string LinkTarget { get; set; }
        public bool HeaderCallback { get; set; }
        public List<Item> Items { get; set; }
        public string DefaultImage { get; set; }
        public bool IsActive { get; set; }
        public GtmData GtmData { get; set; }
    }



    public class SubMenu
    {
        public List<SubMenu> subMenu { get; set; }
        public string Link { get; set; }
        public string LinkText { get; set; }
        public string LinkTarget { get; set; }
        public GtmData GtmData { get; set; }
    }

    public class TopbarList
    {
        public string Phone { get; set; }
        public string PhoneLink { get; set; }
        public string PhoneIcon { get; set; }
        public string LinkText { get; set; }
        public string Link { get; set; }
        public string HeaderLeftIcon { get; set; }
        public string HeaderRightIcon { get; set; }
        public GtmData GtmData { get; set; }
        public List<CollapseItem> Items { get; set; }
    }


}