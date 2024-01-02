using System;

namespace Project.AdaniInternationalSchool.Website.Models
{
    public class MainCardItemModel: ImageContentModel
    {
        public string MainDescription { get; set; }
        public string MapSource { get; set; }

        public GtmDataModel GtmData { get; set; }
    }
}