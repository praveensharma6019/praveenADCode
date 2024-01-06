using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;

namespace Sitecore.TrivandrumAirport.Website.Template
{
    public class Template
    {
    }
    public struct MailConfiguration
    {
        public struct AirportSurveyTemplate
        {
            public readonly static ID airportSurveyEmailTemplateID = new ID("{CDD8F86E-E003-4C41-80A5-141C885D2870}");
            public readonly static ID AirportContactUsEmailTemplate = new ID("{A0332EF5-C99C-4702-BFD8-A8EC7AE0ED42}");

            public struct Fields
            {
                public static readonly ID From = new ID("{8605948C-60FB-46B8-8AAA-4C52561B53BC}");
                public static readonly ID Subject = new ID("{0F45DF05-546F-462D-97C0-BA4FB2B02564}");
                public static readonly ID Body = new ID("{1519CCAD-ED26-4F60-82CA-22079AF44D16}");
            }
        }
    }
}