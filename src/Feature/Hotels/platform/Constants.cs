using Sitecore;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http.Results;

namespace Adani.SuperApp.Airport.Feature.Hotels.Platform
{
    public class Constants
    {
        public static readonly ID AllPoliciesFolderID = new ID("{6BAB545B-24ED-4495-9B87-BE8D05E29966}");
        public static readonly ID TermsConditionsFolderID = new ID("{0F8A3F1F-252D-4990-8A67-9C23D7385D77}");
        public static readonly ID ImpInformationFolderID = new ID("{8B353ABE-4C74-48E5-BFA0-DC1C82689D38}");
        public static readonly ID PoliciesTemplateID = new ID("{0A4DB799-BFB9-4BFD-86B8-B6230D78E252}");
        public static readonly ID ContactDetailsTemplateID = new ID("{7108515A-C66D-4E6E-9B7E-1D4016058101}");
        public static readonly ID ImportantInfoTemplateID = new ID("{0A4DB799-BFB9-4BFD-86B8-B6230D78E252}");

        public static string RenderingParamField = "Widget";
        public static readonly string AutoId = "AutoId";
        public static readonly string Title = "Title";
        public static readonly string Name = "Name";
        public static readonly string Value = "value";
        public static readonly string Description = "Description";
        public static readonly string SubTitle = "SubTitle";
        public static readonly string AuxillaryText = "AuxillaryText";
        public static readonly string Image = "Image";
        public static readonly string Link = "Link";
        public static readonly string NotHeroCarousel = "NotHeroCarousel";
        public static readonly string UniqueId = "UniqueId";
        public static readonly string UniqueID = "UniqueID";
        public static readonly string ReadMoreText = "ReadMoreText";
        public static readonly string Rating = "Rating";
        public static readonly string IsInternational = "IsInternational";
        public static readonly string City = "City";
        public static readonly string IsoCodeA2 = "IsoCodeA2";
        public static readonly string Country = "Country";
        public static readonly string SlugId = "SlugId";
        public static readonly string SellingCurrency = "SellingCurrency";
        public static readonly string Hotel = "Hotel";
        public static readonly string HotelId = "HotelId";
        public static readonly string CityId = "CityId";
        public static readonly string BtnText = "btnText";
        public static readonly string MobileImage = "MobileImage";
        public static readonly string SellingCountry = "SellingCountry";
    }
}