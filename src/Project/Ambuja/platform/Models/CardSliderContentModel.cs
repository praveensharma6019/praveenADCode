using System.Collections.Generic;

namespace Project.AmbujaCement.Website.Models
{
    public class CardSliderContentModel<T> : AmbujaCement.Website.Models.CardTypeContentModel
    {
        public string SectionID { get; set; }
        public string SubHeading { get; set; }
        public List<T> Gallery { get; set; }
        public string ImageSource { get; set; }
        public string ImageSourceMobile { get; set; }
        public string ImageSourceTablet { get; set; }
        public string ImageAlt { get; set; }
        public string ImageTitle { get; set; }
    }

}