using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Sitecore.JaipurAirport.Website.Template
{
    public class Template
    {
    }
    public struct MailConfiguration
    {
        public struct AirportSurveyTemplate
        {
            public readonly static ID airportSurveyEmailTemplateID = new ID("{31586848-04FC-4180-9EC1-837CBE77CB4E}");
            public readonly static ID AirportContactUsEmailTemplate = new ID("{703EF0DD-4A18-4C5C-801F-8E9C90B8FE87}");

            public struct Fields
            {
                public static readonly ID From = new ID("{8605948C-60FB-46B8-8AAA-4C52561B53BC}");
                public static readonly ID Subject = new ID("{0F45DF05-546F-462D-97C0-BA4FB2B02564}");
                public static readonly ID Body = new ID("{1519CCAD-ED26-4F60-82CA-22079AF44D16}");
            }
        }
    }
}