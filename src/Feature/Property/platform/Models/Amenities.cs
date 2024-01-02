namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class Amenities
    {
        public string caption { get; set; }

        public string src { get; set; }

        public string ImageText { get; set; }

        public string link { get; set; }

        public string imgAlt { get; set; }
        public string status { get; set; }
        public string srcMobile { get; set; }
    }


    public class AmenetiesData
    {
        public string componentID { get; set; }

        public string heading { get; set; }

        public string disclaimer { get; set; }

        public ProjectAmeneties projectAmeneties { get; set; }
    }

    public class ProjectAmeneties
    {
        public dynamic data { get; set; }
    }

    public class FeaturesData
    {
        public string componentID { get; set; }

        public string heading { get; set; }

        public dynamic features { get; set; }
    }

    public class Featute
    {
        public string caption { get; set; }

        public string src { get; set; }
        public string imgAlt { get; set; }

        public string title { get; set; }

        public string desc { get; set; }
        public string labelTerms { get; set; }
        public string srcMobile { get; set; }
    }

    public class NavbarTabs
    {
        public string about { get; set; }
        public string ameneties { get; set; }
        public string projects { get; set; }
        public string masterLayout { get; set; }
        public string typicalFloorPlan { get; set; }
        public string typicalUnitPlan { get; set; }
        public string exploreTownship { get; set; }
        public string locationMap { get; set; }
        public string video { get; set; }
        public string projectHighlights { get; set; }
    }


}