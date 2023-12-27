using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Media.Platform
{
    public class Templates
    {
        public static class ServicesListCollection
        {
            public static readonly ID ServiceItemTemplateID = new ID("{0B2191D6-78AE-4742-AD47-ADC20306DE48}");
            public static string RenderingParamField = "Widget";
            public static string UniqueId = "UniqueID";
            public static string ServiceTitle = "ServiceTitle";
            public static string TagName = "name";
            public static string TextColor = "textColor";
            public static string BackgroundColor = "backgroundColor";
            public static string ServiceUrl = "ServiceUrl";
            public static string Image = "ServiceImage";
            public static string MobileImage = "MobileImage";
            public static string AutoId = "AutoId";
            public static string IsAgePopup = "IsAgePopup";
            public static readonly ID IsAirportSelectNeeded = new ID("{8B0FC08B-BDB2-40EF-B504-5743A75AAD00}");
        }
        public static class CustomContentCollection
        {
            public static readonly ID TitleWithRichTextTemplateID = new ID("{0A4DB799-BFB9-4BFD-86B8-B6230D78E252}");
        }
    }
}