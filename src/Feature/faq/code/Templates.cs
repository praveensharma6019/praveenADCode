namespace Sitecore.Feature.FAQ
{
    using Sitecore.Data;

    public class Templates
    {
        public struct Faq
        {
            public static readonly ID ID = new ID("{9544F0B3-FD5E-4301-9DDE-9E73D2C3F7BA}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{862B8CA0-B926-4913-82AF-D778EF7F29E6}");
                public static readonly ID Question = new ID("{9588B6D5-3E6A-4C16-BD37-98DA6F1DDE52}");
                public static readonly ID Answer = new ID("{57F39C75-51F0-4888-903E-724DFDCC8A38}");
            }
        }

        public struct FaqDetails
        {
            public static readonly ID ID = new ID("{8BEB561E-9A10-44F3-9611-4AB72558F609}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{923C70AA-537E-4853-AFED-717D3EED42C8}");
                public static readonly ID FAQList = new ID("{22D7817F-9D0D-476D-A898-B0F859DCD5F9}");
                public static readonly ID ImagePlus = new ID("{651210C8-DDB3-475C-A595-B1150A782FC0}");
                public static readonly ID ImageMinus = new ID("{D4537CF1-03CE-46DB-A4E5-DF0BC9D41181}");
                public static readonly ID Class = new ID("{9B3E15DE-B993-41AC-B4A9-A1963421C1BF}");
                public static readonly ID SubTitle = new ID("{6EE79ED3-5A3D-4BC7-9289-6CCD4D95D8D7}");

            }
        }

        public struct FaqGroup
        {
            public static readonly ID ID = new ID("{3AF7DB6C-A602-4ABC-8D63-19E2D2C6726B}");

            public struct Fields
            {
                public static readonly ID GroupMember = new ID("{631DA648-E2A5-4E3B-9733-C9C066C41EAE}");
            }
        }
    }
}