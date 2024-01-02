using Sitecore.Data;

namespace Project.AmbujaCement.Website.Templates
{
    public static class SeoDataTemplate
    {
        public static class Fields
        {
            public static readonly ID heading = new ID("{F8DE225A-8948-43AF-AE9B-EEAFEE40756F}");
            public static readonly ID ogTitle = new ID("{8B924F7B-1112-44BD-9883-333459E6B6AA}");
            public static readonly ID robotsTags = new ID("{6534ECC3-BE99-4090-921E-0DBDBA726BCA}");
            public static readonly ID robotsIndexTag = new ID("{A8D17677-78D6-4B46-B101-367CA61A32AA}");
            public static readonly ID robotsFollowTag = new ID("{D46A4008-4813-4F77-85CA-94EC42C61854}");
            public static readonly ID browserTitle = new ID("{A857F82A-6D85-4FE6-9B12-20009A7ADF16}");
            public static readonly ID ogDescription = new ID("{4E82BF81-EAE6-4013-B68E-A14AB66FE394}");
            public static readonly ID googleSiteVerification = new ID("{2805AAAF-33BD-4891-AFA0-39E8D59B8A85}");
            public static readonly ID msValidate = new ID("{F8286C83-C3DF-443A-B915-41CE4224B333}");
            public static readonly ID orgSchema = new ID("{4A74C36A-AA4D-4193-B268-294B7C2ECE0F}");

            public class SeoDataOrgSchema
            {
                public static readonly ID addressLocality = new ID("{3B78753B-7DF2-40D2-AE4D-52A4125CF45C}");
                public static readonly ID addressRegion = new ID("{423D1840-F776-4117-9F11-943BF50FF1CD}");
                public static readonly ID postalCode = new ID("{DE33986C-6644-4B26-B6F8-B0F73B615D37}");
                public static readonly ID contactOption = new ID("{ECD714B9-A987-47DE-BDF0-72DD3C34BC8E}");
                public static readonly ID logo = new ID("{7110BF49-54EC-4152-A61A-92A29A8E0A77}");
                public static readonly ID sameAs = new ID("{2EACF7E3-1D03-40D0-BE30-98298197E83E}");
            }
        }
    }

}
