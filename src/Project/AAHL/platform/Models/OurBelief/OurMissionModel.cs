using Glass.Mapper.Sc.Configuration.Attributes;
using Glass.Mapper.Sc.Fields;
using System.Collections.Generic;

namespace Project.AAHL.Website.Models.OurBelief
{
    public class OurMissionModel
    {
        public  string Heading { get; set; }
        public  string Description { get; set; }
        public  List<OurMissionList> Items { get; set; } 
        public  List<ImageListModel> ImageItems { get; set; }
    }
    public class OurMissionList
    {
        public  string Heading { get; set; }
        public  string Description { get; set; }
    }

    public class ImageListModel
    {
        public string ImagePath { get; set; }
        public string MImagePath { get; set; }
        public string TImagePath { get; set; }
        public string Imgalttext { get; set; }
    }
}