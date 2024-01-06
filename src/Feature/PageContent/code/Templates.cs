namespace Sitecore.Feature.PageContent
{
    using Sitecore.Data;

    public struct Templates
    {
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

        public struct ContactUs
        {
            public static readonly ID ID = new ID("{C21FC6A3-4CD9-48C8-B44A-B93230E0A361}");

            public struct Fields
            {
                public static readonly ID Title = new ID("{5173BCF3-68D5-4D05-8313-60FBD5125792}");
                public static readonly ID Description = new ID("{3289F3F8-D95D-4563-95FF-C32ACECDE9C2}");
                public static readonly ID ListingProducts = new ID("{C136F9EB-AC5E-4B17-87F9-49108003CC81}");
            }
        }

        public struct SubBody
        {
            public static readonly ID Body = new ID("{DA6ED874-F918-47E5-AD6A-B5E93B40B0AF}");
            public const string SubBody_FieldName = "Body";
        }


        public struct Navigable
        {
            public static readonly ID ID = new ID("{A1CBA309-D22B-46D5-80F8-2972C185363F}");

            public struct Fields
            {
                public static readonly ID ShowInNavigation = new ID("{5585A30D-B115-4753-93CE-422C3455DEB2}");
                public static readonly ID NavigationTitle = new ID("{1B483E91-D8C4-4D19-BA03-462074B55936}");
                public static readonly ID ShowChildren = new ID("{68016087-AA00-45D6-922A-678475C50D4A}");
                public static readonly ID Summary = new ID("{AC3FD4DB-8266-476D-9635-67814D91E901}");
                public static readonly ID ShowInNewTab = new ID("{10FF37C5-B368-4FAA-89C6-48432D9EBAA0}");
                public static readonly ID Link = new ID("{719E9180-72AB-4592-BE8B-B24A3F28554C}");
                public static readonly ID IsExternalLink = new ID("{40AC25E7-2AFB-4D57-A831-7855C433E1C8}");
                public static readonly ID IsVisible = new ID("{DA2F4A46-CF34-4E5E-A1D2-E1F2E1A221DD}");
            }
        }
    }
}