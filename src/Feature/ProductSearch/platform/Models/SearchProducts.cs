using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models
{
    public class SearchProducts
    {
        public string skuCode { get; set; }
        public string productName { get; set; }
        public string productSEOName { get; set; }
        public string brand { get; set; }
        public string brandTitle { get; set; }
        public string category { get; set; }
        public string categoryTitle { get; set; }
        public string subCategory { get; set; }
        public string subCategoryTitle { get; set; }
        public string materialGroup { get; set; }
        public string materialGroupTitle { get; set; }
    }

    public class SearchResponse
    {
        public SearchData data { get; set; }

        public SearchResponse()
        {
            data = new SearchData();
        }
    }

    public class SearchResponseAPP
    {
        public bool status { get; set; }
        public SearchData data { get; set; }
        public string error { get; set; }
        public string warning { get; set; }

        public SearchResponseAPP()
        {
            status = false;
            data = new SearchData();
        }
    }

    public class SearchData
    {
        public List<SearchProducts> result { get; set; }
        public List<BrandSearch> brands { get; set; }
        public List<PopularSearch> popularSearch { get; set; }
        public List<BrandSearch> popularBrands { get; set; }
        public SearchData()
        {
            result = new List<SearchProducts>();
            brands = new List<BrandSearch>();
            popularSearch = new List<PopularSearch>();
            popularBrands = new List<BrandSearch>();
        }
    }

    public class PopularSearch
    {
        public string title { get; set; }
        public string materialGroup { get; set; }
        public List<string> category { get; set; }
        public List<string> subCategory { get; set; }
        public List<string> brand { get; set; }
        public string skuCode { get; set; }
        public string productSEOName { get; set; }
    }

    public class BrandSearch
    {
        public string brandName { get; set; }
        public string brandCode { get; set; }
        public string materialGroup { get; set; }
    }

    public class SearchResponseData
    {
        public SearchResultData data { get; set; }

        public SearchResponseData()
        {
            data = new SearchResultData();
        }
    }

    public class SearchResultData
    {
        public IEnumerable<ProductSearchData> result { get; set; }
        public List<filterData> filterResult { get; set; }
        public string airportInfo { get; set; }
    }

    public class SearchResultDataAPP
    {
        public bool status { get; set; }
        public SearchDataAPP data { get; set; }
        public string error { get; set; }
        public string warning { get; set; }
        public SearchResultDataAPP()
        {
            status = false;
            data = new SearchDataAPP();
        }
    }
    public class SearchDataAPP
    {
        public int count { get; set; }
        public IEnumerable<ProductSearchData> result { get; set; }
        public List<filterData> filterResult { get; set; }
        public string airportInfo { get; set; }
        public SearchDataAPP()
        {
            count = 0;
            filterResult = new List<filterData>();
        }
    }

    public class filterData
    {
        public string title { get; set; }
        public List<CodeTitle> filters { get; set; }
        public filterData()
        {
            title = string.Empty;
            filters = new List<CodeTitle>();
        }

    }
    public class CodeTitle
    {
        public string title { get; set; }
        public string code { get; set; }
        public string materialGroup { get; set; }
        public string imageSrc { get; set; }
        public CodeTitle()
        {
            title = string.Empty;
            code = string.Empty;
            materialGroup = string.Empty;
            imageSrc = string.Empty;
        }

    }
}