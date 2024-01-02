using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models
{
    public class CategoryFilters
    {
        public string filterTitle { get; set; }
        public string filterCode { get; set; }
        public string filterValue { get; set; }
        public bool multiselect { get; set; } = false;
        public bool showInPrimery { get; set; } = false;
        public bool disableForAirport { get; set; }
        public Banner categoryBanner { get; set; }        
        public List<Brand> brands { get; set; }
        public List<CategoryFilterData> filterData { get; set; }

        public CategoryFilters()
        {
            filterData = new List<CategoryFilterData>();
            brands = new List<Brand>();
        }
    }

    public class CategoryFilterData
    {
        public string title { get; set; }
        public string code { get; set; }
        public bool filterSelected { get; set; }
        public string filterValue { get; set; }
        public List<Brand> brands { get; set; }
        public CategoryFilterData()
        {
            brands = new List<Brand>();
        }
    }

    public class Brand
    {
        public string title { get; set; }
        public string code { get; set; }
        public string imageSrc { get; set; }
        public bool restricted { get; set; }
        public bool disableForAirport { get; set; }
     }

    public class ProductFilters
    {
        public string filterTitle { get; set; }
        public string filterCode { get; set; }
        public string filterValue { get; set; }
        public bool multiselect { get; set; } = false;
        public bool showInPrimery { get; set; } = false;
        public string[] skuCode { get; set; }
        public List<FilterData> filterData { get; set; }
        public ProductFilters()
        {
            filterData = new List<FilterData>();
        }
    }

    public class FilterData
    {
        public string title { get; set; }
        public string code { get; set; }
        public string imageSrc { get; set; }
        public bool filterSelected { get; set; } = false;
        public string filterValue { get; set; } = "";
        public bool restricted { get; set; }
        public bool disableForAirport { get; set; }
    }

    public class SearchProductFilters
    {
        public string materialGroup { get; set; }
        public Banner materialGroupBanner { get; set; }
        public SelectedFilters selectedFilter { get; set; }
        public List<CategoryFilters> categories { get; set; }
        public ProductFilters price { get; set; }
        public ProductFilters sort { get; set; }
        public ProductFilters brands { get; set; }
        public ProductFilters offers { get; set; }
        public ProductFilters exclusive { get; set; }
        public ProductFilters includeOOS { get; set; }
        public ProductFilters isCombo { get; set; }
        public ProductFilters comboCategories { get; set; }
        public SearchProductFilters()
        {
            categories = new List<CategoryFilters>();
        }
    }

    public class ResponseFilters
    {
        public bool status { get; set; }
        public ResultFilters data { get; set; }
        public Error error { get; set; }
        public Warning warning { get; set; }
    }
    public class ResultFilters
    {
       // public int count { get; set; }
        public Object result { get; set; }
    }

    public class Banner
    {
        public string title { get; set; } = "";
        public string description { get; set; } = "";
        public string imageSrc { get; set; } = "";
        public string mimageSrc { get; set; } = "";
        public string ctaLink { get; set; } = "";
        public string ctaText { get; set; } = "";
    }

    public class SelectedFilters
    {
        public string materialGroupTitle { get; set; }
        public string materialGroup { get; set; }
        public string[] category { get; set; }
        public string[] subCategory { get; set; }
        public string[] brand { get; set; }
        public string[] skuCode { get; set; }
        public string[] offers { get; set; }

        public SelectedFilters()
        {
            materialGroupTitle = "";
            materialGroup = "";
            category = new List<string>().ToArray();
            subCategory = new List<string>().ToArray();
            brand = new List<string>().ToArray();
            offers = new List<string>().ToArray();
            skuCode = new List<string>().ToArray();
        }
    }
}