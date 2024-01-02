using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Adani.SuperApp.Airport.Feature.BrandListing.Models;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.BrandListing.Services
{
   public interface IBrandListing
    {
        BrandListingWidget GetBrandListing(Rendering rendering);
    }
}
