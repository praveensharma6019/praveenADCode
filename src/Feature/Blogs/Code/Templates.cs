namespace Sitecore.Feature.Blogs
{
    using Sitecore.Data;

    public struct Templates
    {

        public struct BlogsArticle
        {
            public static readonly ID ID = new ID("{3C5B316F-B65A-4CD7-BCD2-DA80BEBE9BAC}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{E4E2F158-32AE-4D86-95A1-624BDB24D8C4}");
                public const string Title_FieldName = "Blog Title";

                public static readonly ID Summary = new ID("{DF1CC120-68E5-45C7-80A7-B4F03DEA973F}");
                public const string Summary_FieldName = "Blog Summary";

                public static readonly ID Body = new ID("{68BB2B92-8499-41FB-BA30-CC5489D57FEB}");
                public const string Body_FieldName = "Blog Body";

                public static readonly ID BlogType = new ID("{47A7AD3F-D2A5-456F-85FE-82708090DDC9}");
                public const string BlogType_FieldName = "Blog Type";

                public static readonly ID Date = new ID("{5278FFB0-2177-4C1C-ADB9-03FF3339B2D8}");
                public const string Date_FieldName = "Blog Date";

                public static readonly ID Author = new ID("{6045A811-05B1-4406-B079-0707B2B9886A}");
                public const string Author_FieldName = "Blog Author";

                public static readonly ID Category = new ID("{A327BB63-6C48-4000-955D-52F9483AA898}");
                public const string Category_FieldName = "Blog Category";

                public static readonly ID Image = new ID("{A686181B-38FA-41AC-87E5-398490D6A6FF}");
                public const string Image_FieldName = "Blog Image";

            }
        }

        public struct HasBannerSelectorclubahmd
        {
            public static readonly ID ID = new ID("{2C35125E-CDD6-4A39-800A-097F64ECC248}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{EA00D5C0-56B8-47C6-BAEA-2ED002F8CC85}");
                public static readonly ID Description = new ID("{E16CABF8-A6C8-49E8-8876-9E6F1A5DFBA6}");
                public static readonly ID Image = new ID("{30ECC4F1-7D3F-4E91-A4C7-B3A70726AADC}");

            }

        }

        public struct HasPageContent
        {
            public static readonly ID ID = new ID("{AF74A00B-8CA7-4C9A-A5C1-156A68590EE2}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{C30A013F-3CC8-4961-9837-1C483277084A}");
                public const string Title_FieldName = "Title";
                public static readonly ID Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
                public const string Summary_FieldName = "Summary";
                public static readonly ID Body = new ID("{D74F396D-5C5E-4916-BD0A-BFD58B6B1967}");
                public const string Body_FieldName = "Body";
                public static readonly ID Image = new ID("{9492E0BB-9DF9-46E7-8188-EC795C4ADE44}");
            }
        }
        public struct LinkMenuItem
        {
            public static readonly ID ID = new ID("{18BAF6B0-E0D6-4CCE-9184-A4849343E7E4}");

            public struct Fields
            {
                public static readonly ID Icon = new ID("{2C24649E-4460-4114-B026-886CFBE1A96D}");
                public static readonly ID DividerBefore = new ID("{4231CD60-47C1-42AD-B838-0A6F8F1C4CFB}");
                public static readonly ID Image = new ID("{ECC76733-D0D9-48D6-B55E-293FE5B13EA6}");
                public static readonly ID ShortDesc = new ID("{92A73427-1B36-4BCF-8F6F-9F72C350C65D}");
                public static readonly ID BGCSS = new ID("{4CB29455-78C6-4998-8A83-73C19AE52474}");
                public static readonly ID LinkToSection = new ID("{2DE66FB6-840D-4708-9D00-CEAF062955FA}");
                public static readonly ID ActiveImage = new ID("{C0947EAD-9FAC-4F79-940F-5E5B33B9930A}");
                public static readonly ID TagLine = new ID("{5D8603E0-F2C0-4958-A0E0-B4378564FD1B}");
                public static readonly ID VideoThumb = new ID("{6E5E40E5-EE0D-449F-85DF-9D077CDBF44C}");
                public static readonly ID Title = new ID("{1B483E91-D8C4-4D19-BA03-462074B55936}");
            }
        }

        public struct SingleText
        {
            public static readonly ID ID = new ID("{5E2D514F-6D17-4D81-9953-46E1448DD642}");

            public struct Fields
            {
                public static readonly ID Text = new ID("{43A2E7CE-799E-4765-81D7-C2BA836146DD}");
                public static readonly ID Image = new ID("{41C95B72-FF29-4E6A-94DB-F7769F672290}");
            }
        }
    }
}