using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Models
{
    public class OffersAndDiscountsModel
    {
        public string TabName { get; set; }
        public List <SliderWithImageAndDetail> OfferList { get; set; }
    }
}