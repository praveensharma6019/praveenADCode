using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform
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

        public static readonly string Arrival = "{55202CC8-8E55-4518-8B22-096612F3B986}";
        public static readonly string Both = "{AE1665D6-05BE-435D-BEE7-27294BF71E5D}";
        public static readonly string Departure = "{C7418428-03A8-43EE-9C98-4846B6190696}";

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

        public static readonly Sitecore.Data.ID Question = new ID("{15B33278-D089-4294-B41F-0C2AABB06F32}");
        public static readonly Sitecore.Data.ID Answer = new ID("{E74A69B2-41BC-42C7-9BE4-5B401EF6B656}");
        public static readonly ID FAQTemplateID = new ID("{2CDB3FA6-F31A-40FB-91C8-772D7505687E}");

        public static readonly string RenderingParamField = "Widget";
        public static readonly string RowTitle = "Title";
        public static readonly string Column = "Points";
        public static readonly ID TableDataTemplateID = new ID("{95D40DCF-32DE-4B82-9AF2-6C5020761BE6}");
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
}