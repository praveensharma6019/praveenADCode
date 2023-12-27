using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class DepartureCarouselContent : IDepartureCarouselContent
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        public DepartureCarouselContent(ILogRepository logRepository, IHelper helper)
        {
            this._logRepository = logRepository;
            this._helper = helper;
        }

       

        
        public HeroContentItem GetCarouselContent(Sitecore.Data.Items.Item item)
        {
            HeroContentItem heroContentItem = new HeroContentItem();
            try
            {
                DepartureCarousel departureCarousel = new DepartureCarousel();

                if (item == null) return new HeroContentItem();
                departureCarousel.Title = item?.Fields[Templates.DepartureCarousel.DepartureCarouselFields.CarouselTitle]?.Value;
                departureCarousel.Src = _helper.GetImageURLbyField(item?.Fields[Templates.DepartureCarousel.DepartureCarouselFields.CarouselImage]);
                departureCarousel.Text = item?.Fields[Templates.DepartureCarousel.DepartureCarouselFields.CarouselDescription]?.Value;
                departureCarousel.Alt = _helper.GetImageAltbyField(item?.Fields[Templates.DepartureCarousel.DepartureCarouselFields.CarouselImage]);
                departureCarousel.BtnUrl = _helper.GetLinkURLbyField(item,item?.Fields[Templates.DepartureCarousel.DepartureCarouselFields.CarouselCTA]);
                departureCarousel.BtnText = _helper.GetLinkTextbyField(item, item?.Fields[Templates.DepartureCarousel.DepartureCarouselFields.CarouselCTA]);
                departureCarousel.DesktopImage = _helper.GetImageURLbyField(item?.Fields[Templates.DeviceSpecificImage.Fields.DesktopImage]);
                departureCarousel.DesktopImageAlt = _helper.GetImageAltbyField(item?.Fields[Templates.DeviceSpecificImage.Fields.DesktopImage]);
                departureCarousel.ThumbnailImage = _helper.GetImageURLbyField(item?.Fields[Templates.DeviceSpecificImage.Fields.ThumbnailImage]);
                departureCarousel.ThumbnailImageAlt = _helper.GetImageAltbyField(item?.Fields[Templates.DeviceSpecificImage.Fields.ThumbnailImage]);
                departureCarousel.MobileImage = _helper.GetImageURLbyField(item?.Fields[Templates.DeviceSpecificImage.Fields.MobileImage]);
                departureCarousel.MobileImageAlt = _helper.GetImageAltbyField(item?.Fields[Templates.DeviceSpecificImage.Fields.MobileImage]);

                heroContentItem.HeroContent = departureCarousel;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return heroContentItem;
        }
    }
}