using Adani.SuperApp.Airport.Feature.Carousel.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;

namespace Adani.SuperApp.Airport.Feature.Carousel.Platform.Services
{
    public class HeroSliderAppService : IHeroSliderAppService
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        private readonly IWidgetService _widgetservice;

        public HeroSliderAppService(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }


        /// <summary>
        /// Implementation to get HeroSliderData
        /// </summary>
        /// <param name="datasource"></param>
        /// <returns></returns>
        public HeroCarouselwidgets GetHeroSliderData(Rendering rendering)
        {
            HeroCarouselwidgets heroCarouselWidgits = new HeroCarouselwidgets();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constant.RenderingParamField]);
                if (widget != null)
                {
                    // WidgetService widgetService = new WidgetService();
                    heroCarouselWidgits.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    heroCarouselWidgits.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                heroCarouselWidgits.widget.widgetItems = GetHeroSliderdata(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroSliderAppService GetHeroSliderData gives -> " + ex.Message);
            }


            return heroCarouselWidgits;
        }

        private List<Object> GetHeroSliderdata(Rendering rendering)
        {
            List<Object> heroCarouselList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = RenderingContext.Current.Rendering.Item;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }

                HeroCarousel heroCarousel;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    heroCarousel = new HeroCarousel();
                    heroCarousel.isAirportSelectNeeded = _helper.GetCheckboxOption(item.Fields[Constant.isAirportSelectNeeded]);
                    heroCarousel.title = item[Constant.Title];
                    heroCarousel.imageSrc = _helper.GetImageURL(item, Constant.Image);
                    heroCarousel.description = item[Constant.Description];
                    heroCarousel.ctaLink = _helper.GetLinkURL(item, Constant.Link);
                    //These fields are not available in HeroSlider
                    heroCarousel.deepLink = null;
                    heroCarousel.subTitle = string.Empty;

                    heroCarouselList.Add(heroCarousel);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroSliderAppService GetHeroSliderdata gives -> " + ex.Message);
            }

            return heroCarouselList;
        }
    }
}