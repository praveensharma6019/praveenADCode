using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Electricity.Website
{
    public class Template
    {
        public struct Tender
        {
            public static readonly ID TenderRegistration = new ID("{12705B01-75A4-4C50-8BC9-5563393B25D0}");
            public static readonly ID TenderLogin = new ID("{FB10F927-F4B1-4A7A-B42D-BF89E5BA0183}");
            public static readonly ID TenderList = new ID("{AAAEA830-BE82-49E0-848F-688584EC40CB}");
        }

        public struct SwitchToGreen
        {
            public static readonly ID GetSwitchToGreenIn_EmailTemplate_ = new ID("{F1855A12-3EAA-4996-A1D0-9457473BB93A}");
            public static readonly ID GetSwitchToGreenOrg_EmailTemplate_ = new ID("{1B679E35-5BEA-44EE-A1EA-90775DBFFD8C}");
            public static readonly ID GetSwitchToEVIn_EmailTemplate_ = new ID("{10CE2960-E8F3-4A8D-8912-1B8789E13A54}");
            public static readonly ID GetSwitchToEVS_EmailTemplate_ = new ID("{6B327DA3-74C8-4CDD-9287-4278B5569D77}");
            public static readonly ID GetSwitchToEVP_EmailTemplate_ = new ID("{6E912B1B-CD62-49AA-9AD0-27D206A0F850}");
        }

        public struct ITSR
        {
            public static readonly ID BidderFormSubmitEmailToOwners = new ID("{BF9B7F91-2990-4142-8CBC-50562BC37F60}");
            public static readonly ID BidderFormCreateEmailToOwners = new ID("{55EAA7DF-EC97-4A6D-A196-21C18C83FADB}");
            public static readonly ID CreateUserEmail = new ID("{1E9C2322-FADD-4846-954C-A6576F26A32F}");
            
        }

        public struct MailTemplate
        {
            public static readonly ID ID = new ID("{597784C2-0787-4DD6-9CEB-DA403E3FC92F}");
            public static readonly ID EnvelopeUser = new ID("{010F07CB-956D-4AF8-AA06-8907637C880B}");
            public static readonly ID EnvelopeUserOnTenderClose = new ID("{87D0A9A8-6D76-4501-AF04-50EF69716EC3}");
            public static readonly ID EnvelopeUserReminderForTenderClose = new ID("{3858400B-E15F-4251-A8D2-44C10248A1DF}");
            public static readonly ID CorrigendumCreate = new ID("{0AFEF6AF-6A38-4526-95C1-5F4B34CD37EF}");
            
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


        public struct Regulatory
        {
            public static readonly ID ID = new ID("{ECE47E09-E111-442E-AB5A-1DF1C559F803}");
            public struct Fields
            {
                public static readonly ID Title = new ID("{65EB83F9-3400-4569-BEFE-6A29567FE3B3}");
                public static readonly ID Document_Collection = new ID("{BE3010D5-DDC2-4E51-A1C8-F4415A9601CC}");
                public static readonly ID Is_Archive = new ID("{55958F48-7605-4313-809E-E0194BB53240}");
            }
        }
    }
}