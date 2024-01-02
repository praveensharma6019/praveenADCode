using System;
using System.Collections.Generic;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class Project
    {
        public string Title { get; set; }

        public string SubTitle { get; set; }

        public string Image { get; set; }

    }
    public class TownshipMasterLayout
    {
        public string image { get; set; }
        public List<TownshipPoints> points { get; set; }
    }
    public class TownshipPoints
    {
        public string left { get; set; }
        public string bottom { get; set; }
        public string title { get; set; }
    }
    public class ExploreTownship
    {
        public string id { get; set; }
        public string src { get; set; }
        public string imgtitle { get; set; }
        public string imgAlt { get; set; }
        public string dataCols { get; set; }
    }
    public class TownshipSidebar
    {
        public Image image { get; set; }

        public string description { get; set; }

        public Studio studio { get; set; }
    }
    public class Image
    {
        public string src { get; set; }
        public string alt { get; set; }
    }
    public class Studio
    {
        public string heading { get; set; }
        public List<Service> services { get; set; }
    }
    public class Service
    {
        public string type { get; set; }
        public int key { get; set; }
    }
    public class PropertyBasicInfo
    {
        public string Location { get; set; }

        public string ProjectStatus { get; set; }

        public string SiteStatus { get; set; }

        public string ProjectArea { get; set; }

        public string PropertyLogo { get; set; }

        public string SimilarProjectTitle { get; set; }

        public List<Project> SimilarProjects { get; set; }

        public string ProjectLayout { get; set; }

        public string MapLink { get; set; }

        public string UnitSize { get; set; }

        public string Possession { get; set; }

        public string Brochure { get; set; }

        public string PropertyType { get; set; }

        public string status { get; set; }

        public string Type { get; set; }

        public List<string> MediaLibrary { get; set; }

        public string PriceLabel { get; set; }

        public string Onwards { get; set; }

        public string AreaLabel { get; set; }

        public string rating { get; set; }
        public string ratingCount { get; set; }
        public string bestRating { get; set; }
        public string ratingName { get; set; }
        public string propertyDescription { get; set; }
        public string propertyImage { get; set; }
        public bool isProjectCompleted { get; set; }
    }

    public class Datum
    {
        public string labelheading { get; set; }
        public string labeldata { get; set; }
    }

    public class Clubthanks
    {
        public string heading { get; set; }
        public string para { get; set; }
        public List<Datum> data { get; set; }
    }

    public class ContactCtaData
    {
        public string getInTouch { get; set; }
        public string heading { get; set; }
        public string desc { get; set; }
        public string button { get; set; }
        public string enquireNow { get; set; }
        public Clubthanks propertyThanks { get; set; }
    }

    public class ContactCtaDataModel
    {
        public ContactCtaData contactCtaData { get; set; }
    }

    public class ProjectName
    {
        public string title { get; set; }
        public string location { get; set; }
        public string projectLink { get; set; }
        public string price { get; set; }
        public string discountLabel { get; set; } = string.Empty;
        public string discount { get; set; } = string.Empty;
        public string priceLabel { get; set; } = string.Empty;
        public string Rs { get; set; } = string.Empty;
        public string priceStartingAt { get; set; } = string.Empty;
        public string sup { get; set; } = string.Empty;
        public string allInclusive { get; set; } = string.Empty;
    }


}