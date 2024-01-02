using Sitecore.Data;

namespace Project.AdaniInternationalSchool.Website.Templates
{
    public static class HomeCurriculumTemplate
    {

        public static readonly ID TemplateID = new ID("{3A5A0A5D-9582-4CFD-AA7D-C50594E3F0CB}");

        public static class Fields
        {

            public static readonly ID LearningGalleryList = new ID("{B829BEB9-601C-4060-9F42-7B8E36B5254C}");
            public static readonly ID academicDetailsList = new ID("{54BA644E-721C-43C4-8720-0499913CF957}");
            public static readonly ID FeaturesList = new ID("{94CD6499-5162-4D4D-8B6D-877EA77B2EF8}");
        }

        public static class HomeCurriculumFeature
        {
            public static readonly ID TemplateID = new ID("{26FC8C4A-072B-4934-831A-9BD2E8AF890A}");
            public static class Fields
            {
                public static readonly ID theme = new ID("{698042BF-593A-4CCC-BCAA-5DE2C2DFCF37}");
                public static readonly ID description = new ID("{00AD51D6-BB40-4B5D-B20F-C180F1DCF486}");
            }
        }
    }
}
