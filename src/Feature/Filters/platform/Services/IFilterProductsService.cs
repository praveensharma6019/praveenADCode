using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Realty.Feature.Filters.Platform.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.Filters.Platform.Services
{
    public interface IFilterProductsService
    {
        FilterProductsWidgets GetProductFilters(Sitecore.Mvc.Presentation.Rendering rendering);
        NoresultFounfEnquiryForm NoResultFounfItem(Sitecore.Mvc.Presentation.Rendering rendering);
        CityDescriptionList GetCityDescriptionList(Sitecore.Mvc.Presentation.Rendering rendering);
        SEOHeadingDescription GetHeadingDescriptionList(Sitecore.Mvc.Presentation.Rendering rendering);
        FilterData GetLocationFilterData(Sitecore.Mvc.Presentation.Rendering rendering);

        CommonTextForAboutAdani GetCommonTextForAboutAdani(Sitecore.Mvc.Presentation.Rendering rendering);
        List<Object> GetLocationSerchData(Sitecore.Mvc.Presentation.Rendering rendering);
        List<object> GetPropperyTypesList(Sitecore.Mvc.Presentation.Rendering rendering);
        List<SearchData> GetSearchDataList(Rendering rendering);
    }
}