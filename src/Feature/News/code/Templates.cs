namespace Sitecore.Feature.News
{
    using Sitecore.Data;

    public struct Templates
    {
        public struct NewsArticle
        {
            public static readonly ID ID = new ID("{2F75C8AF-35FC-4A88-B585-7595203F442C}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{BD9ECD4A-C0B0-4233-A3CD-D995519AC87B}");
                public const string Title_FieldName = "NewsTitle";

                public static readonly ID Image = new ID("{3437EAAC-6EE8-460B-A33D-DA1F714B5A93}");

                public static readonly ID Date = new ID("{C464D2D7-3382-428A-BCDF-0963C60BA0E3}");

                public static readonly ID Summary = new ID("{9D08271A-1672-44DD-B7EF-0A6EC34FCBA7}");
                public const string Summary_FieldName = "NewsSummary";

                public static readonly ID Body = new ID("{801612C7-5E98-4E3C-80D2-A34D0EEBCBDA}");
                public const string Body_FieldName = "NewsBody";

                public static readonly ID Publication = new ID("{D54D1D05-D24E-456B-9019-D0E78BA10FB4}");
                public const string publication = "Publication";

                public static readonly ID Link = new ID("{F2DF70EF-1979-4831-9CFA-A6F152979FFA}");
                public const string link = "Link";

                public static readonly ID ViewDownloadLink = new ID("{E8419A33-3754-428E-9503-FA67D76BD933}");
                public const string viewDownloadLink = "ViewDownloadLink";

            }
        }

        public struct NewsFolder
        {
            public static readonly ID ID = new ID("{74889B26-061C-4D6A-8CDB-422665FC34EC}");
        }

        public struct NavigationTitle
        {
            public static readonly ID Title = new ID("{1B483E91-D8C4-4D19-BA03-462074B55936}");
            public static readonly ID ImageLink = new ID("{719E9180-72AB-4592-BE8B-B24A3F28554C}");
        }
        public struct _PageContent
        {
            public static readonly ID Image = new ID("{9492E0BB-9DF9-46E7-8188-EC795C4ADE44}");
        }
    }
}
