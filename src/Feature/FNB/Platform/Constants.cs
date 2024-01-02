using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.FNB.Platform
{
    public class Constants
    {
        public static readonly ID TitleWithRichtextTemplateID = new ID("{0A4DB799-BFB9-4BFD-86B8-B6230D78E252}");
        public static readonly ID NameValueTemplateID = new ID("{407FC9F0-CC99-475B-8F51-541094377FBB}");
        public static readonly ID NameValueStoretypeTemplateID = new ID("{0FC1E4B0-5D12-4056-B342-DDFC76A0BCD0}");
        public static readonly ID TitleWithLinkTemplateID = new ID("{940150CA-75BC-4D94-9DAB-1963AA1491E7}");

        //For location,storeType and Terminaltype for ExclusiveOutlets FNB
        public static readonly ID LocationType = new ID("{FD918E6B-ECEA-4EEE-92D8-6A075C84047F}");
        public static readonly ID TerminalStoreType = new ID("{CB3CB05E-1A83-4E1E-AE10-CBE7AEE292FA}");

        public static readonly string Ahmedabad = "{4BE84251-6470-4B7E-A94D-4FADCF0C1B1A}";
        public static readonly string Guwahati = "{A51C1C4A-9FD1-40C9-9165-2675E0BAAFC9}";
        public static readonly string Jaipur = "{A7FD228B-2791-4367-85F2-2D38963FA0E6}";
        public static readonly string Lucknow = "{A49B41EE-E4FA-4040-B535-6E9BF11BCC11}";
        public static readonly string Mangaluru = "{80235B66-A6D6-4C72-ADC8-22CF0299C506}";
        public static readonly string Mumbai = "{F744B88B-E8A8-47F8-8FA1-B17EEFDB815B}";
        public static readonly string Thiruvananthapuram = "{24EC2188-67CB-4168-8296-439845571F8D}";
        public static readonly string adaniOne = "{9034816F-E9CD-4888-800A-049EB877A5A6}";

        public static readonly string ArrivalInternational= "{55202CC8-8E55-4518-8B22-096612F3B986}";
        public static readonly string ArrivalDomestic= "{38B9586C-197B-447F-AA8B-3579BBDA5B3A}";
        public static readonly string Both = "{AE1665D6-05BE-435D-BEE7-27294BF71E5D}";
        public static readonly string DepartureInternational = "{C7418428-03A8-43EE-9C98-4846B6190696}";
        public static readonly string DepartureDomestic = "{7723753C-2FD7-4FED-B729-25404238736C}";

        public static readonly ID TerminalType = new ID("{00ECE17B-561A-43D2-865A-5F29EC17896E}");
        public static readonly string Terminal1 = "{BC3D3FA0-132B-4E36-8B69-0D13FCA6054C}";
        public static readonly string Terminal2 = "{C88D56EE-D765-4101-A3C1-DCB555787735}";
      



        public static readonly Sitecore.Data.ID Title = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
        public static readonly Sitecore.Data.ID Value = new ID("{C89D7769-7095-49EA-ABB1-47AD56E32802}");
        public static readonly Sitecore.Data.ID Name = new ID("{7A4ADE39-7CD4-4713-A53A-8F55A7BDD173}");

        public static readonly Sitecore.Data.ID StoreType = new ID("{DED01CA0-D8BE-4193-8AC9-10AF181949DD}");

        public static readonly Sitecore.Data.ID LinkText = new ID("{186367E6-CC9E-42A5-981F-A3EECBD3F263}");
        public static readonly Sitecore.Data.ID LinkURL = new ID("{FF14F53D-FFC6-4E18-A890-87543CE8278D}");

        public static readonly Sitecore.Data.ID CancellationTitle = new ID("{43ED8C8B-7ABC-4F54-BDCF-C5CEDE9E8EBE}");
        public static readonly Sitecore.Data.ID CancellationDescription = new ID("{ED2534C4-B080-48A0-A82D-4CA2EE96A935}");
        public static readonly Sitecore.Data.ID CancellationCTA = new ID("{DF4DE6F4-A385-49E9-B817-DB28E3AFFFDA}");
        public static readonly Sitecore.Data.ID ReasonsTitle = new ID("{A8F96E5C-2B00-4FD2-B7F5-4BC4DC4FDF9B}");
        public static readonly Sitecore.Data.ID ReasonsList = new ID("{9BFFAA36-2582-42A7-A4B6-B80E29AE4A71}");
        public static readonly Sitecore.Data.ID Reason = new ID("{7A7F5B3F-F715-4056-98D1-B542125906C8}");
        public static readonly Sitecore.Data.ID HintText = new ID("{32B9CC41-C9A9-46BC-B0F8-81335DA1CA70}");
        public static readonly Sitecore.Data.ID DescriptionLength = new ID("{74F61047-22EC-44A4-8E6C-64F5EE670C99}");
        public static readonly Sitecore.Data.ID DictionaryValue = new ID("{403524F3-7AA3-46BA-A322-DE4C8627CDDB}");
        public static string RenderingParamField = "Widget";
    }

    public class FnbExclusiveOutlet
    {
        public static readonly ID ExclusiveOutletTemplateID = new ID("{61DCC328-F0A7-428B-9B23-030CB52EE256}");
        public static readonly Sitecore.Data.ID Title = new ID("{BF6FD924-E5E0-43D0-B2E6-20EDB9D7E80E}");
        public static readonly Sitecore.Data.ID ImageSrc = new ID("{6E5A822E-A19A-47B0-B97E-8CB7C3E2E4F0}");
        public static readonly Sitecore.Data.ID StoreCode = new ID("{AB968FBB-C301-4948-861A-0D87100260F0}");
        public static readonly Sitecore.Data.ID ThumbnailImage = new ID("{C05C8FED-4E23-4048-8BAD-1DFEE79E1A0D}");
        public static readonly Sitecore.Data.ID OpeningTime = new ID("{D04F2BB4-0AF0-4AA8-AAAF-7D878360D525}");
        public static readonly Sitecore.Data.ID ClosingTime = new ID("{EA6ECAFA-9B90-4411-BBE9-2E1F3DCA9078}");
        public static readonly Sitecore.Data.ID PreparationTime = new ID("{5F90A57A-C8EB-44C2-B5E1-434DA534826C}");
        public static readonly Sitecore.Data.ID StoreStatus = new ID("{CBA1F36B-99D5-4F54-AF11-651E643522AC}");
        public static readonly Sitecore.Data.ID MobileImage = new ID("{783EF5C7-94DF-4A5D-B044-0EE22C47687F}");
    }

    public class FnbSapApi
    {
        public static readonly Sitecore.Data.ID Title = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
        public static readonly Sitecore.Data.ID CTALink = new ID("{D5034FA0-81D3-47D7-AA7F-965F39C0B6A5}");
    }

    public class OutletstoreStatus
    {
        public static readonly Sitecore.Data.ID Title = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
        public static readonly Sitecore.Data.ID Icon = new ID("{5F7308D0-0C63-4556-84F5-3B97FF2559EF}");
    }
    public class TitleDescription
    {
        public static readonly ID LocationType = new ID("{BEE1A2E7-D6ED-414B-B327-1C843034B6AD}");
        public static readonly ID TerminalStoreType = new ID("{DDDA1DA0-C7A4-4B37-9254-2B69DC46AD2A}");
        public static readonly ID TerminalType = new ID("{03022906-A8EB-4129-87AD-E98F5B84D1B7}");
        public static readonly ID TitleDescriptionTemplateID = new ID("{6D8175FC-B716-45AA-A6A4-9EEB1DCFC154}");
        public static readonly Sitecore.Data.ID Title = new ID("{65292D60-BED0-4207-9D4E-C399DB7F2244}");
        public static readonly Sitecore.Data.ID Description = new ID("{61BA4988-4D1D-4103-BFFB-9B660432AD02}");
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
        public static string CTALink = "CTALink";
        public static string ExpiryDate = "ExpiryDate";
        public static string ErrorMessage = "ErrorMessage";
        public static string IsApply = "IsApply";
        public static string PotentialEarnMessage = "PotentialEarnMessage";

        public static readonly Sitecore.Data.ID OutletCode = new ID("{612A3370-DED0-4BD0-9460-6EBD1A354912}");
        public static readonly Sitecore.Data.ID Information = new ID("{81EF5B70-C113-4AD9-A379-9E99A2E99AB8}");


    }

    public class TermsAndCondition
    {
        public static readonly ID Title = new ID("{0E9B039F-FD7F-48B1-84F5-F54732ADE7A0}");
        public static readonly ID OfferTitle = new ID("{0B1F490D-A068-4FA3-8A86-5CD88BFEF888}");
        public static readonly ID Airport = new ID("{E3A96452-ACB5-4BF9-AA60-988E85B8685F}");
        public static readonly ID TermsAndConditionTemplateID = new ID("{24783392-F1FA-482D-9F85-F1E6D045359D}");
        public static readonly Sitecore.Data.ID Image = new ID("{A6362B42-1BC2-43B6-9910-BEA75DF62EE4}");
        public static readonly Sitecore.Data.ID LicenseText = new ID("{EFD80D36-E967-4344-AB38-8A6A64F127C7}");
        public static readonly Sitecore.Data.ID LicenseCode = new ID("{7F44069F-F380-49D0-8771-8869FC908102}");
        public static readonly Sitecore.Data.ID StoreOutlet = new ID("{40A5FE0E-5367-4A17-B4F6-6B94DCDD92EE}");

    }

    public class Carousels
    {
        public static readonly ID HeroCarouselFolderID = new ID("{CB530AA4-8F52-4BE8-91DB-7483EB529F73}");
        public static readonly ID CarouselDetailsTemplateID = new ID("{6F60D6FF-FA07-45B0-98C8-69E5A0555CC8}");
        public static readonly ID LocationType = new ID("{76046852-4E2A-4EED-B24B-08933414DF14}");
        public static readonly ID TerminalStoreType = new ID("{0FA3CCA2-B836-48AD-9917-7EB3C8B75BEE}");
        public static readonly ID TerminalType = new ID("{63BCEDF4-C7C1-438B-9469-18B6ACE4F4DB}");
        public static string Title = "Title";
        public static string ImageSrc = "ImageSrc";
        public static string CTALink = "CTALink";
        public static string CTALinkText = "CTALinkText";
        public static string Description = "Description";
        public static string OutletID = "OutletID";
        public static string ThumbnailImage = "ThumbnailImage";
        public static string MobileImage = "MobileImage";
        public static string BannerCode = "BannerCode";
        public static string RestaurantName = "RestaurantName";
        public static string SubTitle = "SubTitle";
        public static string BannerCondition = "BannerCondition";
        public static string offerUniqueID = "offerUniqueID";
        public static string UniqueID = "UniqueID";
    }
    public class Offers
    {
        public static readonly ID AlloffersFolderID = new ID("{800C3F08-9528-4AE0-81A7-E5A7A3DDD4CB}");
        public static readonly ID LocationType = new ID("{76046852-4E2A-4EED-B24B-08933414DF14}");
        public static readonly ID TerminalStoreType = new ID("{0FA3CCA2-B836-48AD-9917-7EB3C8B75BEE}");
        public static readonly ID TerminalType = new ID("{63BCEDF4-C7C1-438B-9469-18B6ACE4F4DB}");
        
    } 
    public class BannerData
    {
        public string ButtonText = "ButtonText";
        public string Title = "Title";
        public string ButtonDescription = "ButtonDescription";
        public string Description = "Description";
        public string Imagesrc = "Image";
        public string MobileImage = "MobileImage";
    }
    public  enum Airport_Location
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
    public class Illustrations
    {
        public static readonly string ImageSrc = "ImageSrc";
        public static readonly string MobileImage = "MobileImage";
    }

    public class Cart
    {
        public static readonly ID CartFNBTemplateID = new ID("{6E2CB40E-59F3-420D-B350-539BE8F1ABEA}");
        public static readonly Sitecore.Data.ID Title = new ID("{2BE21CC8-B1D3-4700-A1DE-0ABAAA6B8BA9}");
        public static readonly Sitecore.Data.ID ImageSrc = new ID("{644C93C0-4067-40B4-8E3D-3400E33137B1}");
        public static readonly Sitecore.Data.ID MobileImage = new ID("{EA6FA8AC-62C3-408C-ADCF-B7860E66C48C}");
        public static readonly Sitecore.Data.ID Code = new ID("{F4ACA195-560F-44D4-8A63-41E217D464FD}");
    }

    public class ReasonsCode
    {
        public static readonly string Label = "Label";
        public static readonly string Code = "Code";
    }

    public class FAQData {
        public static readonly ID FAQTemplateID = new ID("{C60EE1C1-AA69-4DBB-9D62-C0B5B0E10B66}");
        public static string Title = "Title";
        public static string CTALink = "CTAUrl";
        public static string CTAText = "CTAText";
        public static string FAQHtml = "FAQHtml";
        public static readonly string Location = ("Location");
        public static readonly string Question = "Question";
        public static readonly string Answer = "Answer";
        public static readonly string FAQList = "FAQList";

    }
}