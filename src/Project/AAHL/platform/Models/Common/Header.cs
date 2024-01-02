using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Common
{
    public class Header
    {
        public List<HeaderDetail> HeaderDetails { get; set; }
    }

    public class HeaderDetail
    {
        public List<BusinessesMenu> BusinessesMenu { get; set; }
        public List<TopNavigation> TopNavigation { get; set; }
        public Brand Brand { get; set; }
        public List<PrimaryHeaderMenu> PrimaryHeaderMenus { get; set; }
        public List<SearchData> Search { get; set; }
    }

    public class BusinessesMenu
    {
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public string Target { get; set; }
        public List<ItemModel> Items { get; set; }
    }

    public class TopNavigation
    {
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public string Target { get; set; }
        public object Items { get; set; }
    }

    public class Brand
    {
        public string ColoredLogo { get; set; }
        public string DefaultLogo { get; set; }
        public string ColoredLogoIconClass { get; set; }
        public string DefaultLogoIconClass { get; set; }
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public string Target { get; set; }
    }
    public class PrimaryHeaderMenu
    {
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public string Target { get; set; }
        public List<primaryHeaderMenusItemModel> Items { get; set; }
        public Messages Messages { get; set; }
        public Card Card { get; set; }
    }

    public class SearchData
    {
        public string Title { get; set; }
        public string EmptyMsg { get; set; }
        public List<SearchItemModel> Items { get; set; }
    }


    public class Card
    {
        public string ImagePath { get; set; }
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public string Target { get; set; }
        public string IconClass { get; set; }
        public string Bg { get; set; }
    }

    public class ItemModel
    {
        public string Heading { get; set; }
        public string SubHeading { get; set; }
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public string Target { get; set; }
    }

    public class LinkItemModel
    {
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public string Target { get; set; }
    }

    public class SearchItemModel
    {
        public bool Popular { get; set; }
        public string LinkText { get; set; }
        public string LinkUrl { get; set; }
        public string Target { get; set; }
    }
    public class primaryHeaderMenusItemModel
    {
        public string Heading { get; set; }

        public List<ItemModel> Items { get; set; }     
    }

    public class Messages
    {
        public string Description { get; set; }
        public string Title { get; set; }
        public string SubTitle { get; set; }
    }

   
   

  
    


}