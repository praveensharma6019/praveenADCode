using Sitecore.Data;


namespace Sitecore.BelvedereClubGurgaon.Website
{
    public class Templates
    {
        public struct HappyCustomer
        {
            public static readonly ID ID = new ID("{8E8A5940-7931-4076-A5C2-CA5A4A62CDE6}");

            public struct Fields
            {
                public static readonly ID Description = new ID("{25029B66-C02D-4709-9AFD-9F1DC0114E86}");
              
                public static readonly ID Name = new ID("{F4F90EAC-20D7-4D0A-A1FE-0404EDBC5EBE}");
                public static readonly ID Image = new ID("{E2604C65-22FB-4946-B44B-F3CFEE33ABD1}");
            }
        }

        public struct HasBannerSelector
        {
            public static readonly ID ID = new ID("{A59A3E2B-7C1E-462A-9756-5948BFE93C9D}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{45BD0DB4-7BDC-4D34-B4B0-975226CE3D91}");
                public static readonly ID Description = new ID("{437C31F9-4270-4644-955C-6111C70C0B02}");
                public static readonly ID Image = new ID("{AC5ECA30-48D4-4153-91FD-B2714B7E4C69}");
            }
        }

        public struct LeadGeneration
        {
            public static readonly ID LogOut = new ID("{416EBFC1-1323-4C4E-B1F5-AAE0A5E5BF83}");
            public static readonly ID LeadCreation = new ID("{878B2855-FBBE-43B9-B083-925811F52C8C}");
            public static readonly ID ThankYou = new ID("{7BCA1238-C88F-41D4-993F-4932D0B23837}");
        }

    }
}