using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Models.Sustainability
{
    public class SustainabilityModel
    {
        public List<SustainabilityItem> Items { get; set; }
    }

    public class SustainabilityItem
    {
        public string Heading { get; set; }
        public bool Isactive { get; set; }
        public SustainabilityItemData SustainabilityData { get; set; }
        public StoriesReports StoriesReports { get; set; }
        public List<Listitem> CardListitem { get; set; }
        public CommonData DetailsData { get; set; }
    }

    public class SustainabilityItemData : ImageListModel
    {
        public string Heading { get; set; }
        public string Description { get; set; }
    }

    public class StoriesReports
    {
        public string Heading { get; set; }
        public List<StoriesReportsitem> StoriesReportsitem { get; set; }
    }
    public class StoriesReportsitem : ImageListModel
    {
        public string LinkUrl { get; set; }
        public string LinkText { get; set; }
        public string Heading { get; set; }
        public string Description { get; set; }
    }

    public class Listitem : ImageListModel
    {
        public string Heading { get; set; }
        public string Description { get; set; }
    }
    public class CommonData
    {
        public string Heading { get; set; }
        public string Description { get; set; }
        public string SubDescription { get; set; }
    }

    public class ImageListModel
    {
        public string ImagePath { get; set; }
        public string MImagePath { get; set; }
        public string TImagePath { get; set; }
        public string Imgalttext { get; set; }
    }
}