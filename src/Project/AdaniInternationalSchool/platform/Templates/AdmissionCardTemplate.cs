using Sitecore.Data;

namespace Project.AdaniInternationalSchool.Website.Templates
{
    public static class AdmissionCardTemplate
    {
        public static readonly ID TemplateID = new ID("{6070FD32-4142-4649-8B62-5679EA556FFF}");

        public static class Fields
        {
            public static readonly ID theme = new ID("{53B247D9-9B28-4A6E-BBDC-966A3FFBA81D}");
            public static readonly ID textFirst = new ID("{BF3F61B4-A6C9-4241-B8FE-3FCC616E49BB}");
            public static readonly ID cardType = new ID("{C4CD6FA4-19E5-4369-AB45-367E751B80C4}");
            public static readonly ID mediaType = new ID("{BDB77CE8-7ED8-46A3-B664-B3546EDB2B41}");
        }
    }
}
