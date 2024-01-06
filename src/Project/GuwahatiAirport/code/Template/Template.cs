using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;


namespace Sitecore.GuwahatiAirport.Website.Template
{
    public class Template
    {
    }
    public struct MailConfiguration
    {
        public struct AirportSurveyTemplate
        {
            public readonly static ID airportSurveyEmailTemplateID = new ID("{B25DF8BD-558F-4116-AA43-BA097E29B902}");
            public readonly static ID AirportContactUsEmailTemplate = new ID("{6453074C-D8B0-4CC6-B3E8-17783416BB5A}");
            public struct Fields
            {
                public static readonly ID From = new ID("{8605948C-60FB-46B8-8AAA-4C52561B53BC}");
                public static readonly ID Subject = new ID("{0F45DF05-546F-462D-97C0-BA4FB2B02564}");
                public static readonly ID Body = new ID("{1519CCAD-ED26-4F60-82CA-22079AF44D16}");
            }
        }
    }
}