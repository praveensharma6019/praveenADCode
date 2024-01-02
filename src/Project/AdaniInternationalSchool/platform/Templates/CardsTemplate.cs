using Sitecore.Data;

namespace Project.AdaniInternationalSchool.Website.Templates
{
    public static class CardsTemplate
    {
        public static readonly ID cards = new ID("{8B4AE3FF-9394-4BD3-9E2F-BD8111539079}");
        public static class Fields
        {
            public static readonly ID data = new ID("{BD8FFE0C-E210-4DE8-800A-3F49263C23AE}");


            public class CardsDataSection

            {
                public static readonly ID theme = new ID("{6E7B0C97-D395-4F1A-8EF6-06566FCDD3CE}");
                public static readonly ID textFirst = new ID("{2A51A5CD-E1C1-4F53-B588-D2024C98693F}");
                public static readonly ID cardType = new ID("{E3DB970A-99C0-46E5-A5F2-C29DF4643BDA}");
                public static readonly ID mediaType = new ID("{00AD51D6-BB40-4B5D-B20F-C180F1DCF486}");
                public static readonly ID imageSourceMobile = new ID("{3CA7CCD9-7A40-4620-9820-650B8BDCD873}");
                public static readonly ID imageSourceTablet = new ID("{406DA90F-65DD-41B4-A5C6-E8EFAF7EBFC9}");
                public static readonly ID videoSource = new ID("{E03CE3FA-50B0-42FC-97C2-0297CCF046B7}");
                public static readonly ID videoSourceMobile = new ID("{411963D1-A9CF-49B4-849F-F7CF9F2B1E5D}");
                public static readonly ID videoSourceTablet = new ID("{0912A593-0656-498B-8C56-0FFAA57F32D1}");
                public static readonly ID mapSource = new ID("{321F7539-5AA8-4844-845E-5A1D14F9A254}");
                public static readonly ID listItem = new ID("{7D83D0F2-9C52-47DF-BE16-85B17591CDBE}");
            }

            public class CardDatastudentTimings
            {
                public static readonly ID item = new ID("{6DA40EFA-7538-46CB-92B9-41D53337FC52}");
            }
        }
    }
}
