using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data;


namespace Sitecore.LucknowAirport.Website.Template
{
    public class Template
    {
    }
    public struct MailConfiguration
    {
        public struct AirportSurveyTemplate
        {
            public readonly static ID airportSurveyEmailTemplateID = new ID("{F33F3D3A-60BB-47FA-9DE5-09720642A5BF}");
            public readonly static ID AirportContactUsEmailTemplate = new ID("{4DEA6D84-F1DC-4C57-B859-2092BA10DDA8}");


            public struct Fields
            {
                public static readonly ID From = new ID("{8605948C-60FB-46B8-8AAA-4C52561B53BC}");
                public static readonly ID Subject = new ID("{0F45DF05-546F-462D-97C0-BA4FB2B02564}");
                public static readonly ID Body = new ID("{1519CCAD-ED26-4F60-82CA-22079AF44D16}");
            }
        }
    }
}