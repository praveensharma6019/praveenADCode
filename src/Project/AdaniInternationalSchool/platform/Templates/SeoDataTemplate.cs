using Sitecore.Data;

namespace Project.AdaniInternationalSchool.Website.Templates
{
    public static class SeoDataTemplate
    {
        public static class Fields
        {
            public static readonly ID heading = new ID("{F8DE225A-8948-43AF-AE9B-EEAFEE40756F}");
            public static readonly ID ogTitle = new ID("{85EF9E25-7904-4773-B04C-2C27147A92D3}");
            public static readonly ID robotsTags = new ID("{AE677E03-BF82-4715-937B-F7EDCF3EAA1D}");
            public static readonly ID browserTitle = new ID("{0721BBBA-F6C1-4DEB-AF27-C17B33C180C6}");
            public static readonly ID ogDescription = new ID("{4E691357-EB36-4C2D-BC55-00C866F80A6C}");
            public static readonly ID googleSiteVerification = new ID("{6C59A5B8-4931-404B-87C3-9F9EEFD5E8E0}");
            public static readonly ID msValidate = new ID("{1220CA65-F50F-48EE-89D4-51479B58B33F}");
            public static readonly ID orgSchema = new ID("{212AB9C4-9564-4762-994D-8DB97D934B4B}");

            public class SeoDataOrgSchema
            {
                public static readonly ID addressLocality = new ID("{AA554827-07E7-48F5-9BC2-D675FEEC55C3}");
                public static readonly ID addressRegion = new ID("{D222395C-6257-40DF-AE22-C41C93CA7014}");
                public static readonly ID postalCode = new ID("{4A69CE2B-DA54-41D4-B871-AE732CA404D5}");
                public static readonly ID contactOption = new ID("{D5777636-CEEA-4094-A400-B7DA1962C9D2}");
                public static readonly ID logo = new ID("{B6E2EFCA-7DA1-4007-89F6-541E40EF1800}");
                public static readonly ID sameAs = new ID("{65C7CD09-7EF0-447B-ABDF-3B05C5E2DBEA}");
            }
        }
    }

}
