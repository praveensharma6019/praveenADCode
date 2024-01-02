using Sitecore.Data;

namespace Project.AmbujaCement.Website.Templates
{
    public static class HomeFAQTemplate
    {

        public static readonly ID TemplateID = new ID("{24560AF2-AB37-45F0-8F66-B1D463B6F505}");

        public static class Fields
        {
            public static readonly ID categoryID = new ID("{20FE01E0-BEF0-419D-8377-1763900871D8}");
            public static readonly ID questionID = new ID("{6D2EE760-1510-4ECF-A12B-9C55A0BC9B12}");
            public static readonly ID question = new ID("{D269E159-4CCF-4EE7-9B9F-80D235FF529F}");
            public static readonly ID answer = new ID("{DEF84C77-B9D2-4525-8B4B-014D4A634CC1}");
            public static readonly ID FaqGalleryList = new ID("{E88EA24C-5DBB-4455-8E21-0295F4BC4CCC}");
        }

        public static class CategoryDropList
        {
            public static readonly ID TemplateID = new ID("{A474498B-BC61-4981-86D9-9EB77B261351}");           
        }

    }
}
