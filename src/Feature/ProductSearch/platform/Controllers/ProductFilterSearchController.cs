using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using System.Net;
using System.Text;
using System.IO;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Controllers
{
    public class ProductFilterSearchController : ApiController
    {
        private readonly IAPIResponse dataAPI;
        private ILogRepository logRepository;
        private readonly IHelper _helper;
        private readonly ISearchItems searchItems;
        public ProductFilterSearchController(ILogRepository _logRepository, IHelper helper, ISearchItems _searchItems, IAPIResponse _offersResponse)
        {
            this.logRepository = _logRepository;
            this._helper = helper;
            this.searchItems = _searchItems;
            this.dataAPI = _offersResponse;
        }



        [HttpPost]
        [Route("api/GetProductFilters")]
        public IHttpActionResult GetProductFilters([FromBody] Filters filter)
        {
            ResponseFilters responseData = new ResponseFilters();
            ResultFilters resultData = new ResultFilters();

            resultData.result = GetFilterSearchResult(ref filter);
            ///ar jsonResult = ParseProduct(results);
            if (resultData.result != null)
            {
                responseData.status = true;
                responseData.data = resultData;
            }
            return Json(responseData);
        }

        private Object GetFilterSearchResult(ref Filters filter)
        {

            SearchProductFilters result = new SearchProductFilters();
            Sitecore.Globalization.Language language = Sitecore.Globalization.Language.Parse(filter.language);
            Sitecore.Data.Database contextDB = Sitecore.Context.Database;
            List<FilterData> allBrands = new List<FilterData>();
            List<string> skuCodes = new List<string>(); // used for get products of Travel Exclusive and Brand and Boutique
            result.selectedFilter = new SelectedFilters();


            string exclusiveProducts = string.Empty;

            if (!string.IsNullOrEmpty(filter.slug))
            {
                result.selectedFilter = SetFilters(ref filter);

                if (result.selectedFilter.category.Length > 0)
                {
                    filter.category = result.selectedFilter.category;
                }

                if (result.selectedFilter.subCategory.Length > 0)
                {
                    filter.subCategory = result.selectedFilter.subCategory;
                }

                if (result.selectedFilter.brand.Length > 0)
                {
                    filter.brand = result.selectedFilter.brand;
                }
            }
            string materialGrpoup = filter.materialGroup.Trim().ToLower();          
           
            bool virtualMatetialGroup = false;
            virtualMatetialGroup = Constant.VirtualMaterialGroup.IndexOf(_helper.SanitizeName(filter.materialGroup.ToLower())) > -1 ? true : false;

            List<FilterData> comboCategories = new List<FilterData>();
            List<APIProduct> APIComboProducts = new List<APIProduct>();
            if (virtualMatetialGroup && (filter.materialGroup.ToLower().IndexOf("combo") > -1))
            {
                APIComboProducts = searchItems.GetAPIComboProducts(filter);
            }

            // get Categories Filters
            try
            {
                Item materialGroupFolder = contextDB.GetItem(Constant.MaterialGroupFolder.ToString(), language);

                if (materialGroupFolder.HasChildren)
                {
                    Item materialGroupItem = materialGroupFolder.Children.ToList().Find(x => x.TemplateID.Guid == Constant.MaterialGroupTemplate
                        && x.Fields[Constant.MaterialGroup_Code].Value.ToLower().Trim().Equals(materialGrpoup));
                    if (materialGroupItem != null)
                    {
                        result.materialGroup = !string.IsNullOrEmpty(materialGroupItem.Fields[Constant.MaterialGroup_Code].Value) ? materialGroupItem.Fields[Constant.MaterialGroup_Code].Value.ToLower() : string.Empty;
                        exclusiveProducts = !string.IsNullOrEmpty(materialGroupItem.Fields[Constant.MaterialGroup_Exclusive].Value) ? materialGroupItem.Fields[Constant.MaterialGroup_Exclusive].Value : string.Empty;
                        //specialMaterialGroup = (result.materialGroup.ToLower().Trim() == Constant.TravelExclusive.ToLower()) ? true : specialMaterialGroup;
                        //specialMaterialGroup = (result.materialGroup.ToLower().Trim() == Constant.BrandBoutique.ToLower()) ? true : specialMaterialGroup;
                       

                        if (!virtualMatetialGroup)
                        {
                            result.selectedFilter.materialGroup = result.materialGroup;
                            result.selectedFilter.materialGroupTitle = !string.IsNullOrEmpty(materialGroupItem.Fields[Constant.MaterialGroup_Title].Value) ? materialGroupItem.Fields[Constant.MaterialGroup_Title].Value : string.Empty;
                            result.selectedFilter.category = filter.category;
                            result.selectedFilter.subCategory = filter.subCategory;
                            result.selectedFilter.brand = filter.brand;
                            result.selectedFilter.offers = filter.offers;
                        }
                        else
                        {
                            result.selectedFilter.materialGroup = result.materialGroup;
                            result.selectedFilter.materialGroupTitle = !string.IsNullOrEmpty(materialGroupItem.Fields[Constant.MaterialGroup_Title].Value) ? materialGroupItem.Fields[Constant.MaterialGroup_Title].Value : string.Empty;
                        }

                        if (virtualMatetialGroup && filter.materialGroup.IndexOf("combo") > -1)
                        {
                           
                            var materialGroupList = materialGroupFolder.Children.ToList().Where(x => x.TemplateID.Guid == Constant.MaterialGroupTemplate
                                                    && Constant.VirtualMaterialGroup.IndexOf(x.Fields[Constant.MaterialGroup_Code].Value.ToLower()) == -1);
                            comboCategories = materialGroupList.Select(a => new FilterData { 
                                title = a.Fields[Constant.MaterialGroup_Title].Value, 
                                filterValue = APIComboProducts.Where(p => _helper.SanitizeName(p.material_group_name).Equals(a.Fields[Constant.MaterialGroup_Code].Value) ).Count().ToString(), 
                                code = a.Fields[Constant.MaterialGroup_Code].Value 
                            }).ToList();
                        }

                        Item bannerItem = materialGroupItem.Children.ToList().FirstOrDefault(x => x.TemplateID.Guid == Constant.BannerTemplate);
                        result.materialGroupBanner = GetBanner(bannerItem, filter.channel.ToLower().Trim());

                        List<CategoryFilters> categories = null;
                        if (!virtualMatetialGroup)
                        {
                            // get Categories, Subcategories and Brands for Liquor, Fashion, Beauty, Electronics, Confectionery and Others                           
                            categories = GetFilterCategories(materialGroupItem, ref allBrands, ref filter, null, null, null);
                        }
                        else
                        {
                            // get Categories, Subcategories and Brands for Travel Exclusive and Brand Boutique
                            categories = GetSpecialFiltersFromProduct(materialGroupFolder, result.materialGroup, ref allBrands, ref filter, ref skuCodes);
                        }
                        categories = categories.OrderBy(c => c.filterCode).ToList();

                        result.categories = categories;

                    }

                }
            }
            catch (Exception ex)
            {

                logRepository.Error("GetFilterSearchResult Method in ProductFilterSearchController gives error for Categories filters -> " + ex.Message);
            }

            // Other filters
            try
            {
                Item folderItem = contextDB.GetItem(Sitecore.Configuration.Settings.GetSetting("FiltersSearchFolder"), language);
                if (folderItem != null && folderItem.HasChildren)
                {
                    foreach (Item filterSearchItem in folderItem.GetChildren())
                    {
                        ProductFilters productFilters = new ProductFilters();
                      
                        productFilters.filterTitle = !string.IsNullOrEmpty(filterSearchItem.Fields[Constant.FiltersSearchName].Value.ToString()) ? filterSearchItem.Fields[Constant.FiltersSearchName].Value.ToString() : "";
                        productFilters.filterCode = !string.IsNullOrEmpty(filterSearchItem.Fields[Constant.FiltersSearchCode].Value.ToString()) ? filterSearchItem.Fields[Constant.FiltersSearchCode].Value.ToString() : "";
                        productFilters.filterValue = !string.IsNullOrEmpty(filterSearchItem.Fields[Constant.FiltersSearchValue].Value.ToString()) ? filterSearchItem.Fields[Constant.FiltersSearchValue].Value.ToString() : "";
                        productFilters.multiselect = false;
                        productFilters.showInPrimery = true;
                        productFilters.filterData = new List<FilterData>();

                        foreach (Item filterItem in filterSearchItem.GetChildren())
                        {
                            FilterData filterData = new FilterData();
                            filterData.title = !string.IsNullOrEmpty(filterItem.Fields[Constant.FiltersSearchName].Value.ToString()) ? filterItem.Fields[Constant.FiltersSearchName].Value.ToString() : "";
                            filterData.code = !string.IsNullOrEmpty(filterItem.Fields[Constant.FiltersSearchCode].Value.ToString()) ? filterItem.Fields[Constant.FiltersSearchCode].Value.ToString() : "";
                            filterData.filterValue = !string.IsNullOrEmpty(filterItem.Fields[Constant.FiltersSearchValue].Value.ToString()) ? filterItem.Fields[Constant.FiltersSearchValue].Value.ToString() : "";
                            if (filter.includeOOS)
                            {
                                filterData.filterSelected = true;
                            }
                            else
                            {
                                filterData.filterSelected = false;

                            }
                            filterData.imageSrc = _helper.GetImageURL(filterItem, Constant.FilterImage);
                           
                            productFilters.filterData.Add(filterData);
                        }
                        switch (productFilters.filterCode)
                        {
                            case "allbrands":
                                productFilters.multiselect = true;
                                productFilters.filterData = (allBrands.GroupBy(b => b.code).Select(group => group.First())).OrderBy(b => b.title).ToList();

                                productFilters.filterData = (filter.restricted) ? productFilters.filterData.Where(b => b.restricted == false).ToList() : productFilters.filterData;

                                result.brands = productFilters;
                                break;
                            case "sort":
                                result.sort = productFilters;
                                break;
                            case "price":
                                result.price = productFilters;
                                break;
                            case "offers":
                                productFilters.multiselect = true;

                                productFilters.filterData = GetOfferFilters(ref filter, ref skuCodes, result.selectedFilter.offers);
                                result.offers = productFilters;
                                break;
                            case "exclusive":
                                productFilters.multiselect = false;
                                if (!virtualMatetialGroup)
                                {
                                    List<APIProduct> APIExclusiveProducts = searchItems.GetAPIExclusiveProducts(filter);
                                    APIExclusiveProducts = APIExclusiveProducts.Where(a => _helper.SanitizeName(a.material_group_name).ToLower().Equals(materialGrpoup)).ToList();
                                    productFilters.filterValue = APIExclusiveProducts.Count() > 0 ? "true" : "false";
                                    productFilters.skuCode = APIExclusiveProducts.Select(a => a.flemingo_sku_code).ToArray();
                                    if (APIExclusiveProducts.Count() > 0)
                                    {
                                        result.exclusive = productFilters;
                                    }
                                    if (APIExclusiveProducts.Count() == 0 && !string.IsNullOrEmpty(exclusiveProducts))
                                    {
                                        productFilters.skuCode = exclusiveProducts.Split('|');
                                        productFilters.filterValue = "true";
                                        result.exclusive = productFilters;
                                    }
                                }
                                break;
                         
                            case "includeOOS":
                                result.includeOOS = productFilters;
                                break;
                        }
                        if (filter.materialGroup.IndexOf("combo") > -1)
                        {
                            productFilters.filterData = comboCategories;
                            result.comboCategories = productFilters;
                        }
                        else
                        {
                            if (!virtualMatetialGroup)
                            {
                                productFilters.multiselect = false;                               
                                productFilters.filterValue = APIComboProducts.Where(p=> _helper.SanitizeName(p.material_group_name).Equals(materialGrpoup)).Count() > 0 ? "true" : "false";
                                result.isCombo = productFilters;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                logRepository.Error("GetFilterSearchResult Method in ProductFilterSearchController gives error for other filters -> " + ex.Message);
            }
            return result;
        }

        private List<CategoryFilters> GetFilterCategories(Item materialGroupItem, ref List<FilterData> allBrands, ref Filters filter, List<string> filterCatergories, List<string> filterSubcategories, List<string> filterBrands)
        {
            List<CategoryFilters> categories = new List<CategoryFilters>();
            foreach (Item categotyItem in materialGroupItem.GetChildren())
            {
                if (categotyItem.TemplateID.Guid == Constant.CategoryTemplate)
                {
                    Item bannerItem = null;
                    bannerItem = categotyItem.Children.ToList().FirstOrDefault(x => x.TemplateID.Guid == Constant.BannerTemplate);
                    CategoryFilters category = new CategoryFilters();
                    category.filterTitle = !string.IsNullOrEmpty(categotyItem.Fields[Constant.FiltersSearchName].Value.ToString()) ? categotyItem.Fields[Constant.FiltersSearchName].Value.ToString() : "";
                    category.filterCode = !string.IsNullOrEmpty(categotyItem.Fields[Constant.FiltersSearchCode].Value.ToString()) ? categotyItem.Fields[Constant.FiltersSearchCode].Value.ToString() : "";
                    category.multiselect = true;
                    category.categoryBanner = GetBanner(bannerItem, filter.channel.ToLower().Trim());
                    Sitecore.Data.Fields.CheckboxField chkShowInFilter = categotyItem.Fields[Constant.CateroryShowInFilter];
                    category.showInPrimery = (chkShowInFilter.Checked) ? true : false;
                    // All Brands available in this Category     
                    category.brands.AddRange(GetBrandFilterData(categotyItem.Fields[Constant.CateroryBrands], filterBrands, ref filter));
                    category.brands = (filter.restricted) ? category.brands.Where(b => b.restricted == false).OrderBy(b => b.title).ToList() : category.brands.OrderBy(b => b.title).ToList();
                    // Add Brands for All Brands Filter
                    foreach (Brand brand in category.brands)
                    {
                        FilterData filterData = new FilterData();
                        filterData.code = brand.code;
                        filterData.title = brand.title;
                        filterData.imageSrc = brand.imageSrc;
                        filterData.restricted = brand.restricted;
                        filterData.disableForAirport = brand.disableForAirport;
                        if (!filterData.disableForAirport)
                        {
                            allBrands.Add(filterData);
                        }

                    }

                    category.disableForAirport = _helper.GetAvaialbilityOnAirport(categotyItem, filter.airportCode, filter.storeType);


                    // all sub categories for this Category
                    //List<CategoryFilterData>
                    category.filterData.AddRange(GetSubCategoryFilterData(categotyItem, filterSubcategories, filterBrands, ref filter));
                    if (!category.disableForAirport)
                    {
                        categories.Add(category);
                    }

                }
            }

            if (filterCatergories != null)
            {
                categories = categories.Where(c => filterCatergories.Contains(c.filterCode.ToLower().Trim())).ToList();
            }

            return categories;
        }

        private List<CategoryFilterData> GetSubCategoryFilterData(Item categotyItem, List<string> filterSubcategories, List<string> filterBrands, ref Filters filter)
        {
            List<CategoryFilterData> subCategories = new List<CategoryFilterData>();
            foreach (Item subCategotyItem in categotyItem.GetChildren())
            {
                if (subCategotyItem.TemplateID.Guid == Constant.SubCategoryTemplate)
                {
                    CategoryFilterData categoryFilterData = new CategoryFilterData();
                    categoryFilterData.title = !string.IsNullOrEmpty(subCategotyItem.Fields[Constant.FiltersSearchName].Value.ToString()) ? subCategotyItem.Fields[Constant.FiltersSearchName].Value.ToString() : "";
                    categoryFilterData.code = !string.IsNullOrEmpty(subCategotyItem.Fields[Constant.FiltersSearchCode].Value.ToString()) ? subCategotyItem.Fields[Constant.FiltersSearchCode].Value.ToString() : "";
                    categoryFilterData.filterSelected = false;
                    categoryFilterData.filterValue = string.Empty;

                    categoryFilterData.brands.AddRange(GetBrandFilterData(subCategotyItem.Fields[Constant.CateroryBrands], filterBrands, ref filter));

                    categoryFilterData.brands.OrderBy(b => b.title);
                    subCategories.Add(categoryFilterData);
                }
            }

            if (filterSubcategories != null)
            {
                subCategories = subCategories.Where(c => filterSubcategories.Contains(c.code.ToLower().Trim())).ToList();
            }
            subCategories = subCategories.OrderBy(o => o.code).ToList();

            return subCategories;
        }

        private List<Brand> GetBrandFilterData(Sitecore.Data.Fields.MultilistField multiselectFieldBrand, List<string> filterBrands, ref Filters filter)
        {
            List<Brand> brandList = new List<Brand>();
            foreach (Item brandItem in multiselectFieldBrand.GetItems())
            {
                Brand brand = new Brand();
                brand.title = !string.IsNullOrEmpty(brandItem.Fields[Constant.Brand_Name].Value.ToString()) ? _helper.ToTitleCase(brandItem.Fields[Constant.Brand_Name].Value.ToString()) : "";
                brand.code = !string.IsNullOrEmpty(brandItem.Fields[Constant.Brand_Code].Value.ToString()) ? brandItem.Fields[Constant.Brand_Code].Value.ToString() : "";
                brand.imageSrc = _helper.GetImageURL(brandItem, Constant.BrandImage);
                brand.restricted = _helper.SanitizeName(brandItem.Fields[Constant.Brand_MaterialGroup].Value).ToLower().Equals(Constant.restrictedGroup) ? true : false;
                brand.disableForAirport = _helper.GetAvaialbilityOnAirport(brandItem, filter.airportCode, filter.storeType);
                brandList.Add(brand);
            }
            if (filterBrands != null)
            {
                brandList = brandList.Where(b => filterBrands.Contains(b.code.ToLower())).ToList();
            }
            return brandList;
        }

        private Banner GetBanner(Item banner, string channel)
        {
            Banner bannerObj = new Banner();
            try
            {
                if (banner != null)
                {
                    bannerObj.title = banner.Fields[Constant.BannerTitle] != null ? banner.Fields[Constant.BannerTitle].Value.ToString() : "";
                    bannerObj.description = banner.Fields[Constant.BannerDescription] != null ? banner.Fields[Constant.BannerDescription].Value.ToString() : "";
                    if (banner.Fields[Constant.BannerCTA] != null)
                    {
                        bannerObj.ctaLink = _helper.LinkUrl(banner.Fields[Constant.BannerCTA]);
                        bannerObj.ctaText = _helper.GetLinkText(banner, Constant.BannerCTA);
                        bannerObj.mimageSrc = _helper.GetImageURL(banner, Constant.MobileImage);
                    }
                    switch (channel)
                    {
                        case "app":
                        case "mobile":
                            if (banner.Fields[Constant.MobileImage] != null)
                            { bannerObj.imageSrc = _helper.GetImageURL(banner, Constant.MobileImage); }
                            break;
                        default:
                            if (banner.Fields[Constant.DesktopImage] != null)
                            { bannerObj.imageSrc = _helper.GetImageURL(banner, Constant.DesktopImage); }
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetBanner Method in ProductFilterSearchController gives error for other filters -> " + ex.Message);
            }
            return bannerObj;
        }

        private SelectedFilters SetFilters(ref Filters filter)
        {
            SelectedFilters selectedFilters = new SelectedFilters();
            filter.materialGroup = "";
            try
            {
                string[] searchString = filter.slug.Split('/');
                string offerFilter = string.Empty;
                offerFilter = searchString.Any(a => a.IndexOf("offer") > -1) ? searchString.Where(a => a.IndexOf("offer") > -1).FirstOrDefault() : string.Empty;
                List<string> CatCodeList = new List<string>();
                List<string> SubcatCodeList = new List<string>();
                List<string> BrandCodeList = new List<string>();
                List<string> codeList = new List<string>();
                if (searchString.Length > 0)
                {
                    List<SearchMaterialGroup> searchMaterialGroups = searchItems.GetSolrMaterialgroups();
                    List<SearchCategory> searchCategories = searchItems.GetSolrCategories();
                    List<SearchSubcategory> searchSubcategories = searchItems.GetSolrSubcategories();
                    List<SearchBrand> searchBrands = searchItems.GetSolrBrands();
                    Filters productFilter = new Filters();
                    foreach (string slug in searchString)
                    {
                        if (!string.IsNullOrEmpty(slug))
                        {
                            SearchMaterialGroup searchMaterialGroup = searchMaterialGroups.FirstOrDefault(x => x.MaterialGroupName.Equals(slug.Trim()) || x.MaterialGroupCode.Equals(slug.Trim().ToLower()));
                            productFilter.materialGroup = searchMaterialGroup != null ? searchMaterialGroup.MaterialGroupCode : string.Empty;
                            selectedFilters.materialGroupTitle = searchMaterialGroup != null ? searchMaterialGroup.MaterialGroupName : selectedFilters.materialGroupTitle;
                            selectedFilters.materialGroup = searchMaterialGroup != null ? searchMaterialGroup.MaterialGroupCode : selectedFilters.materialGroup;
                            filter.materialGroup = selectedFilters.materialGroup;

                            SearchCategory searchCategory = searchCategories.FirstOrDefault(x => x.CategoryName.Equals(slug.Trim()) || x.CategoryCode.Equals(slug.Trim().ToLower()));
                            if (searchCategory != null && CatCodeList.IndexOf(searchCategory.CategoryCode) == -1 && !searchCategory.CategoryCode.ToLower().Equals(Constant.BrandBoutique.ToLower()))
                            {
                                CatCodeList.Add(searchCategory.CategoryCode);
                            }
                            SearchSubcategory searchSubcategory = searchSubcategories.FirstOrDefault(x => x.SubcategoryName.Equals(_helper.SanitizeName(slug.Trim())) || x.SubcategoryCode.Equals(slug.Trim().ToLower()));
                            if (searchSubcategory != null && SubcatCodeList.IndexOf(searchSubcategory.SubcategoryCode) < 0)
                            {
                                SubcatCodeList.Add(searchSubcategory.SubcategoryCode);
                            }
                            SearchBrand searchBrand = searchBrands.FirstOrDefault(x => x.BrandName.Equals(slug.Trim()) || x.BrandCode.Equals(slug.Trim().ToLower()));
                            if (searchBrand != null && BrandCodeList.IndexOf(searchBrand.BrandCode) < 0)
                            {
                                BrandCodeList.Add(searchBrand.BrandCode);
                            }
                        }
                    }

                    productFilter.category = CatCodeList.ToArray();
                    productFilter.subCategory = SubcatCodeList.ToArray();
                    productFilter.brand = BrandCodeList.ToArray();
                    productFilter.filterType = "Product";
                    if (productFilter.materialGroup.Equals(string.Empty) && CatCodeList.Count == 0 && SubcatCodeList.Count == 0 && BrandCodeList.Count == 0)
                    {
                        productFilter.materialGroup = "NoRecords";
                    }
                    else
                    {
                        System.Linq.IQueryable<SearchResultItem> productList = searchItems.GetSolrProducts(productFilter);
                        SearchResultItem product = productList.FirstOrDefault();

                        if (product != null)
                        {
                            productFilter.materialGroup = (product.Fields.ContainsKey(Constant.MaterialGroupCode)) ? product.Fields[key: Constant.MaterialGroupCode].ToString() : string.Empty;
                            if (!selectedFilters.materialGroup.Equals(Constant.BrandBoutique.ToLower()))
                            {
                                if (selectedFilters.materialGroup != productFilter.materialGroup)
                                {
                                    if (string.IsNullOrEmpty(selectedFilters.materialGroup))
                                    {
                                        selectedFilters.materialGroup = productFilter.materialGroup;
                                        selectedFilters.materialGroupTitle = (product.Fields.ContainsKey(Constant.MaterialGroupTitle)) ? product.Fields[key: Constant.MaterialGroupTitle].ToString() : string.Empty;
                                    }
                                    else
                                    {
                                        selectedFilters.materialGroup = string.Empty;
                                        selectedFilters.materialGroupTitle = string.Empty;
                                    }
                                }
                            }
                        }


                    }
                    filter.materialGroup = selectedFilters.materialGroup;
                    selectedFilters.category = CatCodeList.ToArray();
                    selectedFilters.subCategory = SubcatCodeList.ToArray();
                    selectedFilters.brand = BrandCodeList.ToArray();
                }
                if (!string.IsNullOrEmpty(offerFilter))
                {
                    Sitecore.Data.Database contextDB = Sitecore.Context.Database;
                    Item offerFolderItem = contextDB.GetItem(Constant.ExclusiveOffersFolderId.ToString());
                    SelectedFilters selectedFiltersOffer = null;
                    selectedFiltersOffer = searchItems.GetOffersConfiguration(offerFilter.Trim().ToLower(), offerFolderItem);
                    filter.materialGroup = selectedFiltersOffer.materialGroup.ToLower();
                    selectedFilters.offers = selectedFiltersOffer.offers;
                    selectedFilters.skuCode = selectedFiltersOffer.skuCode;
                    selectedFilters.brand = selectedFiltersOffer.brand;
                    selectedFilters.category = selectedFiltersOffer.category;
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("SetFilters Method in ProductFilterSearchController Class gives error  -> " + ex.Message);
            }
            return selectedFilters;
        }

        private List<FilterData> GetOfferFilters(ref Filters filter, ref List<string> skuCodes, string[] selectedOffers)
        {
            List<FilterData> offersFiltered = new List<FilterData>();
            try
            {
                string storeType = filter.storeType.ToLower().Trim();
                string materialgroup = filter.materialGroup.ToLower().Trim();
                string[] category = filter.category;
                string[] subCategory = filter.subCategory;
                string[] brand = filter.brand;
                string[] filterOffers = filter.offers.Select(o => o.ToLower().Trim()).ToArray();

                if (selectedOffers.Length > 0)
                {
                    filterOffers = selectedOffers.Select(o => o.ToLower().Trim()).ToArray();
                }

                APIOffersResponse offersResponse = GetAPIOffers(ref filter);
                List<APIOffer> aPIOffers = offersResponse.data.Where(x => x.operatorType.ToLower().Trim() == storeType).ToList();

                if (skuCodes.Count > 0)
                {
                    List<string> skuCodeObj = skuCodes;
                    aPIOffers = aPIOffers.Where(o => skuCodeObj.Contains(o.skuResponse.skuCode)).ToList();
                }
                else
                {
                    aPIOffers = aPIOffers.Where(o => _helper.SanitizeName(o.skuResponse.materialGroupName).ToLower() == materialgroup).ToList();

                    aPIOffers = category.Length > 0 ? aPIOffers.Where(o => category.Contains(_helper.SanitizeName(o.skuResponse.categoryName.ToLower()))).ToList() : aPIOffers;

                    aPIOffers = subCategory.Length > 0 ? aPIOffers.Where(o => subCategory.Contains(_helper.SanitizeName(o.skuResponse.subCategoryName.ToLower()))).ToList() : aPIOffers;

                    aPIOffers = brand.Length > 0 ? aPIOffers.Where(o => brand.Contains(_helper.SanitizeName(o.skuResponse.brandName.ToLower()))).ToList() : aPIOffers;
                }

                List<APIOffer> freeBeeOffers = aPIOffers.Where(o => (o.offerType.ToLower().Equals("quantity") || o.offerType.ToLower().Equals("skuprice"))).ToList();

                aPIOffers = aPIOffers.Where(o => !(o.offerType.ToLower().Equals("quantity"))).Where(o => !o.offerType.ToLower().Equals("skuprice")).ToList();

                if (freeBeeOffers.Any())
                {
                    string freeOfferText = "offer Freebee";
                    Sitecore.Data.Database contextDB = Sitecore.Context.Database;
                    Item dictionaryItem = contextDB.GetItem(Sitecore.Data.ID.Parse(Constant.FreeOfferText), Sitecore.Globalization.Language.Parse(filter.language));

                    if (dictionaryItem != null)
                    {
                        freeOfferText = dictionaryItem.Fields["Phrase"].Value.ToString();
                    }

                    FilterData filterData = new FilterData();
                    filterData.title = freeOfferText;
                    filterData.code = string.Join("|", freeBeeOffers.GroupBy(o => new { o.offerDisplayText }).Select(o => o.Key.offerDisplayText)); ;
                    filterData.filterSelected = filterOffers.Any(a => filterData.code.ToLower().Contains(a.ToLower()));
                    filterData.filterValue = Enumerable.Sum(freeBeeOffers.GroupBy(o => new { o.offerDisplayText }).Select(o => o.Count())).ToString();
                    offersFiltered.Add(filterData);
                }


                offersFiltered.AddRange(aPIOffers.GroupBy(o => new { o.offerDisplayText })
                                                     .Select(o => new FilterData
                                                     {
                                                         title = o.Key.offerDisplayText,
                                                         code = o.Key.offerDisplayText,
                                                         filterValue = o.Count().ToString(),
                                                         filterSelected = filterOffers.Contains(o.Key.offerDisplayText.ToLower().Trim())
                                                     }).ToList());

            }
            catch (Exception ex)
            {
                logRepository.Error("GetOfferFilters Method in ProductFilterSearchController gives error for other filters -> " + ex.Message);
            }


            return offersFiltered.Distinct().ToList();
        }

        private APIOffersResponse GetAPIOffers(ref Filters filter)
        {
            APIOffersResponse OffersResponse = null;
            try
            {
                Guid guID = Guid.NewGuid();
                string API_URL = Sitecore.Configuration.Settings.GetSetting("dutyfreeOffersAPI");
                if (!(API_URL.IndexOf("GetAllPromotionFilter") > 0))
                {
                    Dictionary<string, string> headers = new Dictionary<string, string>();
                    headers.Add("traceId", guID.ToString());
                    headers.Add("channelId", "Sitecore");
                    Dictionary<string, string> apiParms = new Dictionary<string, string>();
                    apiParms.Add("airportCode", (string.IsNullOrEmpty(filter.airportCode.Trim()) ? "BOM" : filter.airportCode.Trim()));
                    var response = dataAPI.GetAPIResponse("GET", Sitecore.Configuration.Settings.GetSetting("dutyfreeOffersAPI"), headers, apiParms, "");
                    OffersResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<APIOffersResponse>(response);
                }
                else
                {
                    var parameters = Newtonsoft.Json.JsonConvert.SerializeObject(GetPromotionFilters(filter));

                    var req = WebRequest.Create(Sitecore.Configuration.Settings.GetSetting("dutyfreeOffersAPI") + "?language=" + filter.language);
                    req.Method = "POST";
                    req.ContentType = "application/json";

                    byte[] bytes = Encoding.ASCII.GetBytes(parameters);

                    req.ContentLength = bytes.Length;
                    req.Headers.Add("traceId", guID.ToString());
                    req.Headers.Add("channelId", "Sitecore");
                    req.Headers.Add("agentID", "Sitecore");

                    using (var os = req.GetRequestStream())
                    {
                        os.Write(bytes, 0, bytes.Length);

                        os.Close();
                    }

                    var stream = req.GetResponse().GetResponseStream();

                    if (stream != null)
                        using (stream)
                        using (var sr = new StreamReader(stream))
                        {
                            OffersResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<APIOffersResponse>(sr.ReadToEnd().Trim());
                        }
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetAPIOffers Method in ProductFilterSearchController gives error for other filters -> " + ex.Message);
            }
            return OffersResponse;
        }

        private List<CategoryFilters> GetSpecialFiltersFromProduct(Item materialGroupFolder, string materialGroup, ref List<FilterData> allBrands, ref Filters filter, ref List<string> skuCodes)
        {
            List<CategoryFilters> categoryFilters = new List<CategoryFilters>();
            Filters filterObj = new Filters();
            CategoryFilters category = new CategoryFilters();
            category.filterData = new List<CategoryFilterData>(); //subcategory
            category.brands = new List<Brand>();
            category.multiselect = true;
            category.showInPrimery = false;
            filterObj.filterType = "Product";
            filterObj.storeType = filter.storeType;
            filterObj.materialGroup = filter.materialGroup;
            filterObj.restricted = filter.restricted;
            filterObj.airportCode = filter.airportCode;
            List<string> filterMaterialGroups = new List<string>();
            List<string> filterCatergories = new List<string>();
            List<string> filterSubcategories = new List<string>();
            List<string> filterBrands = new List<string>();

            if (filterObj.materialGroup != null)
            {
                filterObj.travelExclusive = filterObj.materialGroup.ToLower().Trim().Equals(Constant.TravelExclusive.ToLower());
                searchItems.UpdateForBrandBoutique(materialGroupFolder, ref filterObj);
            }

            try
            {
                //Get Products from Solr service
                IQueryable<SearchResultItem> productResultsSolar = searchItems.GetSolrProducts(filterObj);
                List<SearchResultItem> productResults = productResultsSolar.ToList();
                skuCodes = productResults.Select(p => p.Name.ToString()).ToList();
                productResults = productResults.Where(p => p.Fields.ContainsKey(Constant.MaterialGroupCode) && p.Fields.ContainsKey(Constant.BrandCode)
                                                           && p.Fields.ContainsKey(Constant.CategoryCode) && p.Fields.ContainsKey(Constant.SubCategoryCode)).ToList();

                if (filterObj.restricted)
                {
                    productResults = productResults.Where(p => !p.Fields[key: Constant.MaterialGroupCode].ToString().ToLower().Equals(Constant.restrictedGroup)).ToList();
                }

                filterMaterialGroups = productResults.Select(p => p.Fields[key: Constant.MaterialGroupCode].ToString().ToLower()).Distinct().ToList();
                filterCatergories = productResults.Select(p => p.Fields[key: Constant.CategoryCode].ToString().ToLower()).Distinct().ToList();
                filterSubcategories = productResults.Select(p => p.Fields[key: Constant.SubCategoryCode].ToString().ToLower()).Distinct().ToList();
                filterBrands = productResults.Select(p => p.Fields[key: Constant.BrandCode].ToString().ToLower()).Distinct().ToList();

                List<CategoryFilters> categoriesFiltered = null;
                foreach (string materialGrpoup in filterMaterialGroups)
                {
                    Item materialGroupItem = materialGroupFolder.Children.ToList().Find(x => x.TemplateID.Guid == Constant.MaterialGroupTemplate
                                             && x.Fields[Constant.MaterialGroup_Code].Value.ToLower().Trim().Equals(materialGrpoup));

                    categoriesFiltered = GetFilterCategories(materialGroupItem, ref allBrands, ref filter, filterCatergories, filterSubcategories, filterBrands);
                    categoryFilters.AddRange(categoriesFiltered);
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetSpecialFiltersFromProduct Method in ProductFilterSearchController gives error for other filters -> " + ex.Message);
            }

            return categoryFilters;
        }

        private OfferFilter GetPromotionFilters(Filters filter)
        {
            OfferFilter offerFilter = new OfferFilter();
            offerFilter.airportCode = filter.airportCode;
            offerFilter.materialGroupName = filter.materialGroup;
            offerFilter.operatorType = filter.storeType;

            return offerFilter;
        }
    }
}