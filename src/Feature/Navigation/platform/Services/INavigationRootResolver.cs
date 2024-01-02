using Adani.SuperApp.Realty.Feature.Navigation.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System.Collections.Generic;
//using System.Collections.Generic;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.Services
{
    public interface INavigationRootResolver
    {
        Item GetNavigationRoot(Item contextItem);
        List<Item> GetPropertyDetails(Item contextItem);
        List<Item> GetTestimonialList(Item contextItem);

        Item AboutAdaniHouse(Item contextItem);
        List<LocatinAboutAdani> LocationAboutAdaniHouse(Rendering rendering);
        List<LocatinAboutAdani> SeoAboutAdaniHouse(Rendering rendering);

        MobileMenu MobilemenuList(Rendering rendering);
        HeaderMenuList GetHeaderMenuList(Rendering rendering);
        Headerairportlist GetAirportList(Rendering rendering);
        topNavigationModel GetTopnavigationList(Rendering rendering);
        EnquireForm GetEnquiryItem(Rendering rendering);
        NewHeaderMenuList GetprimaryHeaderMenusList(Rendering rendering);
        List<BreadCrumbModel> GetBreadCrumbData(Rendering rendering);
        List<BreadCrumbModel> GetSeoBreadCrumbData(Rendering rendering);
        SEOData GetSEOdataCOntentResolver(Rendering rendering);
        otherProjects GetOtherProjectdata(Rendering rendering);
    }
}
