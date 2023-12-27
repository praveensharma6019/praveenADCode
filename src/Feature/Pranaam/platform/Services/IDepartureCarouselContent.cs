using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.Pranaam.Models;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public interface IDepartureCarouselContent
    {
        HeroContentItem GetCarouselContent(Sitecore.Data.Items.Item datasource);
    }
}