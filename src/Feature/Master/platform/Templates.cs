using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Adani.SuperApp.Airport.Feature.Master.Platform
{
    public static class Templates
    {
        public static class NameValueCollection
        {
            public const string NameValueFolder = "{979C4694-EF8D-4D7F-9017-9E3E9903359B}";
            public const string nameValue = "{407FC9F0-CC99-475B-8F51-541094377FBB}";
            public const string citymaster_template = "{5AAA133C-3A1D-4AF1-A07C-A3BCD9820E46}";
            public static class Fields
            {
                public static readonly ID NavigationTitle = new ID("{32CFF90D-4FDF-4402-A364-21199E88753D}");
            }
        }

        public static class CityStateMaster
        {
            
            public const string citymaster_template = "{5AAA133C-3A1D-4AF1-A07C-A3BCD9820E46}";
           
        }
        public static class AirportListCollection
        {
            public static readonly ID KeywordsFolderTemplateID = new ID("{8F4F2A26-49C5-4E2F-9AFB-A4652C015BA2}");
            public static readonly ID KeywordTemplateID = new ID("{64E55D8A-5A56-40EB-9672-4A7F7384D343}");
        }

        public static readonly string Value = "Value";
        public static readonly string Label = "Title";
        public static readonly string RenderingParamField = "Widget";
    }
}