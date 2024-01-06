using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.BAU.Transmission.Foundation.Theming.Platform.Models;

namespace Adani.BAU.Transmission.Feature.Navigation.Platform.Models
{
    public class DFHeaderlwidgets
    {
        public WidgetItem widget { get; set; }
        //  public List<MaterialGroup> widgetItems { get; set; }
    }
    /// <summary>
    /// Class to get the Material Groups
    /// </summary>
    public class MaterialGroup
    {

        public string title { get; set; }
        public string code { get; set; }
        public string linkUrl { get; set; }
        public string imageSrc { get; set; }
        public string cdnPath { get; set; }
        public int sortorder { get; set; }
        public bool restricted { get; set; }
        public string materialGroup { get; set; }
        public List<Category> children { get; set; }
        public string age { get; set; }
        
    }

    /// <summary>
    /// Class to get the category 
    /// </summary>
    public class Category
    {
        public string title { get; set; }
        public string code { get; set; }
        public string linkUrl { get; set; }
        public List<string> imageSrc { get; set; }
        public string cdnPath { get; set; }
        public string category { get; set; }
        public string brand { get; set; }
        public bool restricted { get; set; }
        public List<Brand> children { get; set; }

        //public List<SubCategory> SubCategories { get; set; }

    }

    /// <summary>
    /// Class to get the SubCategory
    /// </summary>
    public class SubCategory
    {
        public string title { get; set; }
        public string code { get; set; }
        public string linkUrl { get; set; }
        public string imageSrc { get; set; }
        public string cdnPath { get; set; }

        public List<Brand> brands { get; set; }
    }

    /// <summary>
    /// Class to get the Brands
    /// </summary>
    public class Brand
    {
        public string title { get; set; }
        public string code { get; set; }
        public string imageSrc { get; set; }
        public string cdnPath { get; set; }
        public string description { get; set; }
        public string brand { get; set; }
        public bool restricted { get; set; }
    }
}