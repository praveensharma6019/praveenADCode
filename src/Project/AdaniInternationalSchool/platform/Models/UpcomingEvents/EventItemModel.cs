using System;
using System.Linq;
using System.Web;

namespace Project.AdaniInternationalSchool.Website.Models.UpcomingEvents
{
    public class EventItemModel : BaseImageContentModel
    {
        public string Date { get; set; }
        public string Url { get; set; }
        public GtmDataModel GtmData { get; set; }
    }


}