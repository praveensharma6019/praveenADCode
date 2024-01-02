using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models
{
    public class CardsData : VideoContentModel
    {
        public CardsData()
        {
            ListItem = new List<CardslistItemStudentTimings>();
        }
        public GtmDataModel GtmData { get; set; }
        public List<CardslistItemStudentTimings> ListItem { get; set; }
    }


    public class CardslistItemStudentTimings:BaseContentModel
    {
        public List<CardsDataitem> Item { get; set; } = new List<CardsDataitem>();
    }

    public class CardsDataitem
    {
        public string Description { get; set; }
        public string Timing { get; set; }
    }

}