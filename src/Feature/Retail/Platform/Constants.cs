using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Retail.Platform
{
    public class Constants
    {
        public static string RenderingParamField = "Widget";
        public static readonly ID LocationType = new ID("{45402DAB-F225-4E74-B345-1871F886DD45}");
        public static readonly ID TerminalStoreType = new ID("{80ACC468-9959-4DA6-B0D6-337FD93F2646}");
        public static readonly ID TerminalType = new ID("{64CBF6D0-85FF-436D-8B6F-A90F08A49E0C}");
        public static readonly string Arrival = "{CDE1A7DB-1514-4E38-9960-7E9F0F0F6C19}";
        public static readonly string Departure = "{9474A35A-7413-459C-990A-0CF2D5589EED}";
        public static readonly string Both = "{AE1665D6-05BE-435D-BEE7-27294BF71E5D}";
        public static readonly string Ahmedabad = "{4BE84251-6470-4B7E-A94D-4FADCF0C1B1A}";
        public static readonly string Guwahati = "{A51C1C4A-9FD1-40C9-9165-2675E0BAAFC9}";
        public static readonly string Jaipur = "{A7FD228B-2791-4367-85F2-2D38963FA0E6}";
        public static readonly string Lucknow = "{A49B41EE-E4FA-4040-B535-6E9BF11BCC11}";
        public static readonly string Mangaluru = "{80235B66-A6D6-4C72-ADC8-22CF0299C506}";
        public static readonly string Mumbai = "{F744B88B-E8A8-47F8-8FA1-B17EEFDB815B}";
        public static readonly string Thiruvananthapuram = "{24EC2188-67CB-4168-8296-439845571F8D}";
        public static readonly string adaniOne = "{9034816F-E9CD-4888-800A-049EB877A5A6}";
        public static readonly string Terminal1 = "{E4E49D44-51EF-4F9D-AB42-E99D5816D98A}";
        public static readonly string Terminal2 = "{7CACFCF2-0825-4E59-AD49-DDCC41C81E61}";
        public static readonly ID BrandsDetailsTemplateID = new ID("{3A58D7D3-FE50-45E0-BA05-B155D7F65F7E}");
        public static readonly ID OutletDetailsTemplateID = new ID("{51C4F838-6ADA-45F5-AED3-FEE50D76E5EF}");
        public static readonly string ApplicableOutlets = ("Applicable Outlet");
        public static readonly string ApplicableCategories = ("Product Category");
        public static readonly Sitecore.Data.ID OutletCode = new ID("{8707B16F-A5CD-4E6B-BF26-B76FF9C09245}");
        public static readonly ID OutletLocationType = new ID("{D54228C4-6716-472B-9E0A-D222CE46562B}");
        public static readonly ID OutletTerminalStoreType = new ID("{B7D758EC-FACA-4B20-91DA-3CBA4F2AECE1}");
        public static readonly ID OutletTerminalType = new ID("{DC1CCA11-892C-4998-964E-47A1B9D16F14}");
        public static string Title = "Title";
        public static string ImageSrc = "ImageSrc";
        public static string CTALink = "CTA Link";
        public static string Description = "Description";
        public static string Storecode = "Storecode";
        public static string UniqueId = "UniqueId";
        public static string ThumbnailImage = "ThumbnailImage";
        public static string MobileImage = "MobileImage";
        public static string Image = "Image";
        public static string CTA = "CTA";
        public static string SizeTitle = "SizeTitle";
        public static string Sizes = "Sizes";
        public static string SizeChartTitle = "SizeChartTitle";
    }
    public enum Airport_Location
    {
        AMD,
        GAU,
        JAI,
        LKO,
        TRV,
        IXE,
        BOM,
        ADLONE,

    }

    public class PopularCategoryConstants
    {
        public static readonly ID Title = new ID("{864F13BE-B240-46FB-8BDE-8D125A24E06A}");
        public static readonly ID PopularCategoryTemplateID = new ID("{4851CC96-7B56-473F-A853-7B9E1C36A990}");
        public static readonly Sitecore.Data.ID StoreOutlet = new ID("{261CDD22-3104-4BA6-98D7-B25BE9A97054}");
        public static readonly ID LocationType = new ID("{9C15EAEF-341F-4BC5-A3B2-E14F23AEBAB3}");
        public static readonly ID TerminalStoreType = new ID("{3EFAE179-C92A-4A97-BFF8-A43075206CA1}");
        public static readonly ID TerminalType = new ID("{EDBBC7B8-7DC1-4754-BCE4-B5B4BE5295B7}");
        public static readonly string ApplicableOutlets = ("Applicable Outlet");

    }

    public class Bankoffers
    {
        public static readonly ID LocationType = new ID("{28CBE990-76A2-40B9-8149-1F048528D3E4}");
        public static readonly ID TerminalStoreType = new ID("{B2E658C5-121D-4D30-B4DA-D15B321961E5}");
        public static readonly ID TerminalType = new ID("{2D0D7109-387B-4C52-AE2C-C496CDD5596C}");
        public static readonly ID FNBBankOfferTemplateID = new ID("{19EE3D34-41DC-4D9C-ABE2-4CFF9DADE5CD}");
        public static readonly Sitecore.Data.ID Title = new ID("{64C14552-6309-40B1-8C26-AE6342D4416D}");
        public static readonly Sitecore.Data.ID Icon = new ID("{01D1E398-BDC1-424E-A6DF-7E9E965A35DC}");
        public static readonly Sitecore.Data.ID Code = new ID("{852DD9EE-15CF-4EB8-A4B9-EB1D0CC8E228}");
        public static readonly Sitecore.Data.ID DisplayID = new ID("{569D410B-237A-4C51-B7EC-886C8C53F60C}");
        public static readonly string ApplicableOutlets = ("Applicable Outlet");
        public static readonly Sitecore.Data.ID OutletCode = new ID("{612A3370-DED0-4BD0-9460-6EBD1A354912}");

    }

}