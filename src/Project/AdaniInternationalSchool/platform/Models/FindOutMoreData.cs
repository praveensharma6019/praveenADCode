namespace Project.AdaniInternationalSchool.Website.Models
{
    public class FindOutMoreData: BaseContentModel<FindOutMoreDataModel>
    {
        public string VideoDuration { get; set; }
        public string GtmEvent { get; set; }
        public string GtmCategory { get; set; }
        public string GtmSubCategory { get; set; }
        public string GtmVideoStartEvent { get; set; }
        public string GtmVideoCompletedEvent { get; set; }
        public string GtmVideoProgressEvent { get; set; }
        public string GtmBannerCategory { get; set; }
        public string GtmIndex { get; set; }
        public string GtmEventSub { get; set; }
        public string PageType { get; set; }
    }
}