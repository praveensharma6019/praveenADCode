using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.ProductSearch.Platform.Services
{
   public interface IBrandListing
    {
        BrandListingWidget GetBrandListing(Rendering rendering, string restricted, string storeType, string airport);
    }
}
