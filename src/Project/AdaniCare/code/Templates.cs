namespace Sitecore.AdaniCare.Website
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct EmailTemplates
        {
            public static readonly ID ClaimOfferEmail = new ID("{A686F2DE-2326-4D1E-8252-C95EA69E25D9}");
            public static readonly ID ClaimOfferEmailToConsumer = new ID("{2AEADAB6-4560-4448-92D3-E79F6CC3747D}");

            public struct Fields
            {
                public static readonly ID From = new ID("{8605948C-60FB-46B8-8AAA-4C52561B53BC}");
                public static readonly ID Subject = new ID("{0F45DF05-546F-462D-97C0-BA4FB2B02564}");
                public static readonly ID Body = new ID("{1519CCAD-ED26-4F60-82CA-22079AF44D16}");
            }
        }

        public struct Pages
        {
            public static readonly ID LoginPage = new ID("{7C5DFD3E-E383-4A54-9CF5-E61C376C6E6B}");
            public static readonly ID ThankYouPage = new ID("{0FD2F871-CE3D-4628-83C3-B3FDAE3D08A2}");
            public static readonly ID OffersPage = new ID("{EAFE156C-2EB0-4A97-B2DA-C3255345025E}");
            public static readonly ID OfferDetailsPage = new ID("{6A8A171E-3C1B-41C9-BDCF-D6CFD6B34650}");
        }

        public struct Offers
        {
            public static readonly ID ID = new ID("{1A6ECA23-9F10-4B3D-B226-C60B5D6F5E39}");
            public struct Fields
            {
                public static readonly ID Company = new ID("{9D77D57B-496F-439F-9438-D857E63274A3}");
                public static readonly ID Heading = new ID("{CC7118BE-B167-477F-B41A-50BB5A988A9E}");
                public static readonly ID ShortDescription = new ID("{10A0815B-21A1-4435-8D66-F18B22114E4C}");
                public static readonly ID LongDescription = new ID("{9F70DE88-DA6B-4768-9427-50D967F511B8}");
                public static readonly ID Image = new ID("{4574713E-A148-4F1E-90D4-A7A8A1C52296}");
            }
        }

        public struct SingleText
        {
            public static readonly ID ID = new ID("{3742F700-308E-4A97-B524-4274E2ACCBB1}");

            public struct Fields
            {
                public static readonly ID Text = new ID("{E4059B29-8F8D-4F8B-B265-4F9917EFC4B6}");
                public static readonly ID Image = new ID("{89651F3D-7AF2-4471-A3B8-2385F977A988}");
            }
        }

        public struct _HasPageContent
        {
            public static readonly ID ID = new ID("{AF74A00B-8CA7-4C9A-A5C1-156A68590EE2}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{C30A013F-3CC8-4961-9837-1C483277084A}");
                public static readonly ID Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
                public static readonly ID Body = new ID("{D74F396D-5C5E-4916-BD0A-BFD58B6B1967}");
                public static readonly ID Image = new ID("{9492E0BB-9DF9-46E7-8188-EC795C4ADE44}");
            }
        }

        public struct HasMediaImage
        {
            public static readonly ID ID = new ID("{FAE0C913-1600-4EBA-95A9-4D6FD7407E25}");

            public struct Fields
            {
                public static readonly ID Image = new ID("{9F51DEAD-AD6E-41C2-9759-7BE17EB474A4}");
            }
        }

        public struct HasMedia
        {
            public static readonly ID ID = new ID("{A44E450E-BA3F-4FAF-9C53-C63241CC34EB}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{63DDA48B-B0CB-45A7-9A1B-B26DDB41009B}");
                public static readonly ID Description = new ID("{302C9F8D-F703-4F76-A4AB-73D222648232}");
                public static readonly ID Thumbnail = new ID("{4FF62B0A-D73B-4436-BEA2-023154F2FFC4}");
                public static readonly ID Link = new ID("{EF85F9C6-11B7-40C2-8173-336D79D70E13}");
                // public static readonly ID SubTitle = new ID("{1F2A968D-040D-44C2-9CF0-949F702A6199}");
                public static readonly ID SubTitle = new ID("{A1F260C7-8EB4-4E87-A654-DC4DFD309F06}");


            }
        }
    }
}