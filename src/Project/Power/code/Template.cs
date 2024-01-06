using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Power.Website
{
    public class Template
    {
        public struct Tender
        {
            public static readonly ID TenderRegistration = new ID("{12705B01-75A4-4C50-8BC9-5563393B25D0}");
            public static readonly ID TenderLogin = new ID("{FB10F927-F4B1-4A7A-B42D-BF89E5BA0183}");
            public static readonly ID TenderList = new ID("{C197452E-83C9-45A6-BDB3-5442CA0866AE}");
        }

        public struct MailTemplate
        {
            public static readonly ID ID = new ID("{597784C2-0787-4DD6-9CEB-DA403E3FC92F}");
            public static readonly ID EnvelopeUser = new ID("{010F07CB-956D-4AF8-AA06-8907637C880B}");
            public struct Fields
            {
                public static readonly ID From = new ID("{8605948C-60FB-46B8-8AAA-4C52561B53BC}");
                public static readonly ID Subject = new ID("{0F45DF05-546F-462D-97C0-BA4FB2B02564}");
                public static readonly ID Body = new ID("{1519CCAD-ED26-4F60-82CA-22079AF44D16}");
            }
        }

        public struct Article
        {
            public static readonly ID ID = new ID("{D479B0E8-52CF-41D0-8230-35433143AEB6}");
            public struct Fields
            {
                public static readonly ID DateCreated = new ID("{BD94BE86-2217-4A5E-810A-DF84183137C9}");
            }
        }
    }
}