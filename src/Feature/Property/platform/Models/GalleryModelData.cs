using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Property.Platform.Models
{
    public class GalleryDatum
    {
        public int id { get; set; }
        public string poster { get; set; }
        public string posterAlt { get; set; }
        public string videomp4 { get; set; }
        public string videoogg { get; set; }
        public string thumbsrc { get; set; }
        public string thumbalt { get; set; }
        public string thumbtitle { get; set; }
        public string label { get; set; }
        public string datatype { get; set; }
        public string tabType { get; set; }
        public string iframeurl { get; set; }
        public string imgtype { get; set; }
        public string imgalt { get; set; }
        public string imgtitle { get; set; }
        public string enquirecomponent { get; set; }
        public string thumbsrcMobile { get; set; }
        public string posterMobile { get; set; }
 

    }

    public class GalleryTabDatum
    {
        public string title { get; set; }
        public string link { get; set; }
        public string tabtypecount { get; set; }
    }

    public class GalleryModalData
    {
        public string title { get; set; }
        public string closelink { get; set; }
        public string sharelink { get; set; }
        public string share { get; set; }
        public VideoCarouselData videoCarouselData { get; set; }
        public ModalShare modalShare { get; set; }
    }

    public class GalleryTabs
    {
        public List<GalleryTabDatum> data { get; set; }
    }

    public class ModalSlidesData
    {
        public List<GalleryDatum> Gallerydata { get; set; }
    }



    public class VideoCarouselData
    {
        public ModalSlidesData ModalSlidesData { get; set; }
        public GalleryTabs GalleryTabs { get; set; }
    }
}