using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Realty.Feature.Carousel.Platform.Models;
using Sitecore.Data.Items;
using Adani.SuperApp.Realty.Feature.Widget.Platform.Services;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using static Adani.SuperApp.Realty.Feature.Carousel.Platform.Templates;
using static Adani.SuperApp.Realty.Feature.Carousel.Platform.Models.ProppertiesList;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using System.Globalization;
using Sitecore.Data;

namespace Adani.SuperApp.Realty.Feature.Carousel.Platform.Services
{
    public class HeroCarouselService : IHeroCarouselService
    {
        private readonly ILogRepository _logRepository;
        public HeroCarouselService(ILogRepository logRepository)
        {

            this._logRepository = logRepository;
        }
        /// <summary>
        /// Implementation to get the header data
        /// </summary>
        /// <param name="datasource"></param>
        /// <returns></returns>
        //public HeroCarouselwidgets GetHeroCarouseldata(Rendering rendering)
        //{
        //    HeroCarouselwidgets heroCarouselWidgits = new HeroCarouselwidgets();
        //    try
        //    {
        //        Item widget = null;

        //        IDictionary<string, string> paramDictionary = rendering.Parameters.ToDictionary(pair => pair.Key, pair => pair.Value);
        //        foreach (string key in paramDictionary.Keys.ToList())
        //        {
        //            if (Sitecore.Data.ID.TryParse(paramDictionary[key], out var itemId))
        //            {
        //                widget = rendering.RenderingItem.Database.GetItem(itemId);
        //            }
        //        }
        //        if (widget != null)
        //        {
        //            WidgetService widgetService = new WidgetService();
        //            //heroCarouselWidgits.widget = widgetService.GetWidgetItem(widget);
        //        }
        //        else
        //        {
        //            //  heroCarouselWidgits.widget = new Feature.Widget.Platform.Models.WidgetItem();
        //        }
        //        //  heroCarouselWidgits.widget.widgetItems = GetCarouseldata(rendering);
        //    }
        //    catch (Exception ex)
        //    {

        //        _logRepository.Error(" HeroCarouselService GetHeroCarouseldata gives -> " + ex.Message);
        //    }


        //    return heroCarouselWidgits;
        //}
        public static DateTime ConvertDateFormat(string datetimeString)
        {
            string format = "yyyyMMdd'T'HHmmss'Z'";

            DateTime utcdate = DateTime.ParseExact(datetimeString, format, CultureInfo.InvariantCulture);
            return TimeZoneInfo.ConvertTimeFromUtc(utcdate, TimeZoneInfo.FindSystemTimeZoneById("India Standard Time"));
        }
        public static string ConvertDateFormatTostring(DateTime date)
        {
            return date.ToString("MMM dd yyyy");
        }
        public List<mediaCoverage> GetMediaCoverage(Rendering rendering)
        {
            List<mediaCoverage> listofmediacourage = new List<mediaCoverage>();

            var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
            if (datasource == null)
            {
                throw new NullReferenceException();
            }
            try
            {
                var recordwithoutDate = datasource.Children.Where(x => x.Fields[mediaCoverageTemp.Fields.date].Value == "").ToList();
                var recordwithDate = datasource.Children.Where(x => x.Fields[mediaCoverageTemp.Fields.date].Value != "").OrderByDescending(x => ConvertDateFormat(x.Fields[mediaCoverageTemp.Fields.date].Value)).ToList();
                recordwithDate.AddRange(recordwithoutDate);
                foreach (Item mediaItem in recordwithDate)
                {
                    List<imageData> listofImageData = new List<imageData>();
                    mediaCoverage mediaCoverage = new mediaCoverage();

                    if (mediaItem.TemplateID == mediaCoverageTemp.TemplateID)
                    {
                        mediaCoverage.posterSrc = Helper.GetImageSource(mediaItem, mediaCoverageTemp.Fields.posterSrc.ToString()) != null ?
                                              Helper.GetImageSource(mediaItem, mediaCoverageTemp.Fields.posterSrc.ToString()) : "";
                        mediaCoverage.posterAlt = Helper.GetImageDetails(mediaItem, mediaCoverageTemp.Fields.posterSrc.ToString()) != null ?
                                                Helper.GetImageDetails(mediaItem, mediaCoverageTemp.Fields.posterSrc.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                        mediaCoverage.posterTitle = Helper.GetImageDetails(mediaItem, mediaCoverageTemp.Fields.posterSrc.ToString()) != null ?
                                                Helper.GetImageDetails(mediaItem, mediaCoverageTemp.Fields.posterSrc.ToString()).Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                        mediaCoverage.title = !string.IsNullOrEmpty(mediaItem.Fields[mediaCoverageTemp.Fields.title].Value.ToString()) ? mediaItem.Fields[mediaCoverageTemp.Fields.title].Value.ToString() : "";
                        mediaCoverage.date = mediaItem.Fields[mediaCoverageTemp.Fields.date] != null && mediaItem.Fields[mediaCoverageTemp.Fields.date].Value != "" ? ConvertDateFormatTostring(ConvertDateFormat(mediaItem.Fields[mediaCoverageTemp.Fields.date].Value)) : "";
                        mediaCoverage.link = mediaItem.Fields[mediaCoverageTemp.Fields.link] != null ?
                                    Helper.GetLinkURLbyField(mediaItem, mediaItem.Fields[mediaCoverageTemp.Fields.link]) : "";
                        mediaCoverage.linkTitle = !string.IsNullOrEmpty(mediaItem.Fields[mediaCoverageTemp.Fields.linkTitle].Value.ToString()) ? mediaItem.Fields[mediaCoverageTemp.Fields.linkTitle].Value.ToString() : "";
                        mediaCoverage.modalTitle = !string.IsNullOrEmpty(mediaItem.Fields[mediaCoverageTemp.Fields.modalTitle].Value.ToString()) ? mediaItem.Fields[mediaCoverageTemp.Fields.modalTitle].Value.ToString() : ""; 
                        mediaCoverage.pdfSrc = Helper.GetPropLinkURLbyField(mediaItem, mediaItem.Fields[mediaCoverageTemp.Fields.pdfSrc]);

                    }
                    listofmediacourage.Add(mediaCoverage);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroCarouselService mediaCoverage gives -> " + ex.Message);
            }
            return listofmediacourage;
        }

        public ClubLandingModel GetClubLanding(Rendering rendering)
        {
            ClubLandingModel clubLandingModel = new ClubLandingModel();
            try
            {

                clubLandingModel.clubLanding = ClublandingItem(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroCarouselService GetClubLanding gives -> " + ex.Message);
            }


            return clubLandingModel;
        }
        private List<clubLanding> ClublandingItem(Rendering rendering)
        {
            List<clubLanding> listofclubLanding = new List<clubLanding>();
            var commonItem = Sitecore.Context.Database.GetItem(Templates.commonItem);
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                var clubProperty = datasource.GetMultiListValueItem(ClubSelection.Fields.ItemID);
                if (clubProperty != null)
                {
                    foreach (var clubItem in clubProperty.ToList())
                    {
                        List<ClubCarosuel> clubCarosuelsItem = new List<ClubCarosuel>();
                        clubLanding clubLandingItem = new clubLanding();
                        clubLandingItem.clubLink = Helper.GetLinkURL(clubItem, ClubSelection.Fields.LinkfieldName) != null ?
                                Helper.GetLinkURL(clubItem, ClubSelection.Fields.LinkfieldName) : "";
                        clubLandingItem.clubLogo = Helper.GetImageSource(clubItem, ClubSelection.Fields.ImageFieldName) != null ?
                                Helper.GetImageSource(clubItem, ClubSelection.Fields.ImageFieldName) : "";
                        clubLandingItem.clubLogoAlt = Helper.GetImageDetails(clubItem, ClubSelection.Fields.ImageFieldName) != null ?
                                        Helper.GetImageDetails(clubItem, ClubSelection.Fields.ImageFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                        clubLandingItem.clubName = !string.IsNullOrEmpty(clubItem.Fields[ClubSelection.Fields.TitleID].Value.ToString()) ? clubItem.Fields[ClubSelection.Fields.TitleID].Value.ToString() : "";
                        clubLandingItem.clubAddress = !string.IsNullOrEmpty(clubItem.Fields[ClubSelection.Fields.LocationID].Value.ToString()) ? clubItem.Fields[ClubSelection.Fields.LocationID].Value.ToString() : "";
                        clubLandingItem.clubAbout = !string.IsNullOrEmpty(clubItem.Fields[ClubSelection.Fields.SummaryID].Value.ToString()) ? clubItem.Fields[ClubSelection.Fields.SummaryID].Value.ToString() : "";
                        clubLandingItem.artisticDisclaimer = !string.IsNullOrEmpty(clubItem.Fields[ClubSelection.Fields.BodyID].Value.ToString()) ? clubItem.Fields[ClubSelection.Fields.BodyID].Value.ToString() : "";
                        clubLandingItem.readmore = !string.IsNullOrEmpty(commonItem.Fields[ClubSelection.Fields.readmoreID].Value.ToString()) ? commonItem.Fields[ClubSelection.Fields.readmoreID].Value.ToString() : "";
                        clubLandingItem.readless = !string.IsNullOrEmpty(commonItem.Fields[ClubSelection.Fields.readlessID].Value.ToString()) ? commonItem.Fields[ClubSelection.Fields.readlessID].Value.ToString() : "";
                        var carosuelImage = clubItem.GetMultiListValueItem(ClubSelection.Fields.MediaLibraryID);
                        foreach (var images in carosuelImage)
                        {
                            ClubCarosuel clubCarosuel = new ClubCarosuel();
                            clubCarosuel.clubImage = Helper.GetImageSource(images, ClubSelection.Fields.thumbFieldName) != null ?
                                Helper.GetImageSource(images, ClubSelection.Fields.thumbFieldName) : "";
                            clubCarosuel.clubAlt = Helper.GetImageDetails(images, ClubSelection.Fields.thumbFieldName) != null ?
                                        Helper.GetImageDetails(images, ClubSelection.Fields.thumbFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                            clubCarosuel.type = !string.IsNullOrEmpty(images.Fields[ClubSelection.Fields.imageTypeID].Value.ToString()) ? images.Fields[ClubSelection.Fields.imageTypeID].Value.ToString() : "";
                            clubCarosuel.imgtype = !string.IsNullOrEmpty(images.Fields[ClubSelection.BaseFields.TitleField].Value.ToString()) ? images.Fields[ClubSelection.BaseFields.TitleField].Value.ToString() : "";
                            clubCarosuelsItem.Add(clubCarosuel);
                        }
                        clubLandingItem.data = clubCarosuelsItem;
                        listofclubLanding.Add(clubLandingItem);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroCarouselService ClublandingItem gives -> " + ex.Message);
            }

            return listofclubLanding;
        }
        private List<Object> GetCarouseldata(Rendering rendering)
        {
            List<Object> heroCarouselList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }

                HeroCarousel heroCarousel;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    heroCarousel = new HeroCarousel();
                    heroCarousel.title = !string.IsNullOrEmpty(item.Fields[Constant.Title].Value.ToString()) ? item.Fields[Constant.Title].Value.ToString() : "";
                    heroCarousel.subTitle = !string.IsNullOrEmpty(item.Fields[Constant.SubTitle].Value.ToString()) ? item.Fields[Constant.SubTitle].Value.ToString() : "";
                    heroCarousel.imageSrc = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageURL(item, Constant.StanderedImage);
                    heroCarousel.description = !string.IsNullOrEmpty(item.Fields[Constant.Description].Value.ToString()) ? item.Fields[Constant.Description].Value.ToString() : "";
                    heroCarousel.ctaLink = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, Constant.CTA);
                    heroCarousel.deepLink = !string.IsNullOrEmpty(item.Fields[Constant.deepLink].Value.ToString()) ? item.Fields[Constant.deepLink].Value.ToString() : "";

                    heroCarouselList.Add(heroCarousel);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroCarouselService GetCarouseldata gives -> " + ex.Message);
            }

            return heroCarouselList;
        }
        public CarosuelList GetCarousuelList(Rendering rendering)
        {
            CarosuelList carosuelList = new CarosuelList();
            try
            {

                carosuelList.data = CarousuelList(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroCarouselService GetCarousuelList gives -> " + ex.Message);
            }


            return carosuelList;
        }

        public List<Object> CarousuelList(Rendering rendering)
        {
            List<Object> CarosuelList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }

                RealityCarousel carousel;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    carousel = new RealityCarousel();
                    carousel.isVideo = item.Fields[HeroBanner.Fields.isVideoFieldName].Value != null && item.Fields[HeroBanner.Fields.isVideoFieldName].Value.ToString() == "1" ? "true" :
                                        item.Fields[HeroBanner.Fields.isVideoFieldName].Value != null && item.Fields[HeroBanner.Fields.isVideoFieldName].Value.ToString() == "0" ? "false" : "false";
                    carousel.logo = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, HeroBanner.Fields.LogoFieldName) != null ?
                                        Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, HeroBanner.Fields.LogoFieldName) : "";
                    carousel.logoalt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, HeroBanner.Fields.LogoFieldName) != null ?
                                         Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, HeroBanner.Fields.LogoFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    carousel.logotitle = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, HeroBanner.Fields.LogoFieldName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, HeroBanner.Fields.LogoFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                    carousel.heading = !string.IsNullOrEmpty(item.Fields[HeroBanner.Fields.headingFieldName].Value.ToString()) ? item.Fields[HeroBanner.Fields.headingFieldName].Value.ToString() : "";
                    carousel.subheading = !string.IsNullOrEmpty(item.Fields[HeroBanner.Fields.subheadingFieldName].Value.ToString()) ? item.Fields[HeroBanner.Fields.subheadingFieldName].Value.ToString() : "";
                    carousel.link = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, HeroBanner.Fields.LinkFieldName) != null ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, HeroBanner.Fields.LinkFieldName) : "";
                    carousel.linktarget = Helper.GetLinkURLTargetSpace(item, HeroBanner.Fields.LinkFieldName) != null ?
                                      Helper.GetLinkURLTargetSpace(item, HeroBanner.Fields.LinkFieldName) : "";
                    carousel.linktitle = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkTextbyField(item, item.Fields[HeroBanner.Fields.LinkFieldName]) != null ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkTextbyField(item, item.Fields[HeroBanner.Fields.LinkFieldName]) : "";
                    carousel.rerano = !string.IsNullOrEmpty(item.Fields[HeroBanner.Fields.reranoFieldName].Value.ToString()) ? item.Fields[HeroBanner.Fields.reranoFieldName].Value.ToString() : "";
                    carousel.propertyType = !string.IsNullOrEmpty(item.Fields[HeroBanner.Fields.propertyTypeId].Value.ToString()) ? item.Fields[HeroBanner.Fields.propertyTypeId].Value.ToString() : "";
                    carousel.propertyName = !string.IsNullOrEmpty(item.Fields[HeroBanner.Fields.propertyNameId].Value.ToString()) ? item.Fields[HeroBanner.Fields.propertyNameId].Value.ToString() : "";
                    carousel.imgtype = !string.IsNullOrEmpty(item.Fields[HeroBanner.Fields.imgtypeFieldName].Value.ToString()) ? item.Fields[HeroBanner.Fields.imgtypeFieldName].Value.ToString() : "";
                    carousel.thumb = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, HeroBanner.Fields.thumbFieldName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, HeroBanner.Fields.thumbFieldName) : "";
                    carousel.srcMobile = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, HeroBanner.Fields.MobileImageFieldName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, HeroBanner.Fields.MobileImageFieldName) : "";
                    carousel.thumbalt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, HeroBanner.Fields.thumbFieldName) != null ?
                                         Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, HeroBanner.Fields.thumbFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    carousel.thumbtitle = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, HeroBanner.Fields.thumbFieldName) != null ?
                                         Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, HeroBanner.Fields.thumbFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    carousel.videoposter = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, HeroBanner.Fields.videoposterFieldName) != null ?
                                        Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, HeroBanner.Fields.videoposterFieldName) : "";
                    carousel.videoMp4 = !string.IsNullOrEmpty(item.Fields[HeroBanner.Fields.VideoMp4FieldName].Value) ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetMedialLinkURL(item, item.Fields[HeroBanner.Fields.VideoMp4FieldName]) : "";
                    carousel.videoOgg = !string.IsNullOrEmpty(item.Fields[HeroBanner.Fields.videoOggFieldName].Value) ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetMedialLinkURL(item, item.Fields[HeroBanner.Fields.videoOggFieldName]) : "";
                    carousel.videoposterMobile = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, HeroBanner.Fields.videoposterMobileFieldName) != null ?
                                        Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, HeroBanner.Fields.videoposterMobileFieldName) : "";
                    carousel.videoMp4Mobile = !string.IsNullOrEmpty(item.Fields[HeroBanner.Fields.videoMp4MobileFieldName].Value) ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetMedialLinkURL(item, item.Fields[HeroBanner.Fields.videoMp4MobileFieldName]) : "";
                    carousel.videoOggMobile = !string.IsNullOrEmpty(item.Fields[HeroBanner.Fields.videoOggMobileFieldName].Value) ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetMedialLinkURL(item, item.Fields[HeroBanner.Fields.videoOggMobileFieldName]) : "";
                    carousel.SEOName = item.Fields[HeroBanner.Fields.SEOName].Value != null ? item.Fields[HeroBanner.Fields.SEOName].Value : "";
                    carousel.SEODescription = item.Fields[HeroBanner.Fields.SEODescription].Value != null ? item.Fields[HeroBanner.Fields.SEODescription].Value : "";
                    carousel.UploadDate = item.Fields[HeroBanner.Fields.UploadDate].Value != null ? item.Fields[HeroBanner.Fields.UploadDate].Value : "";

                    CarosuelList.Add(carousel);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroCarouselService CarousuelList gives -> " + ex.Message);
            }

            return CarosuelList;
        }
        public List<Banner> GetLocationBanner(Rendering rendering)
        {
            List<Banner> mainBannerobj = new List<Banner>();
            string urlParam = HttpUtility.ParseQueryString(HttpContext.Current.Request.Url.Query).Get("Location");
            urlParam = !string.IsNullOrEmpty(urlParam) ? urlParam.Replace("property-in-", "") : "";

            string strSitedomain = string.Empty;
            var CItem = Sitecore.Context.Database.GetItem(commonItem);
            strSitedomain = CItem != null ? CItem.Fields["Site Domain"].Value : string.Empty;
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                if (urlParam.ToLower() != "")
                {
                    foreach (Sitecore.Data.Items.Item item in datasource.Children)
                    {
                        if (urlParam.ToLower() == item.Name.ToLower())
                        {
                            Banner banner = new Banner();
                            List<Object> Bannerobj = new List<Object>();
                            banner.src = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, BannerTemplate.Fields.thumbFieldName) != null ?
                                                Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, BannerTemplate.Fields.thumbFieldName) : "";
                            banner.alt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName) != null ?
                                                 Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                            banner.thumb = Helper.GetImageSource(item, BannerTemplate.Fields.logoFieldName) != null ?
                                               Helper.GetImageSource(item, BannerTemplate.Fields.logoFieldName) : "";
                            banner.title = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName) != null ?
                                      Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                            banner.headerdesc = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.headingFieldName].Value.ToString()) ? item.Fields[BannerTemplate.Fields.headingFieldName].Value.ToString() : "";
                            banner.buttontext = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.BTNTextFieldName].Value.ToString()) ? item.Fields[BannerTemplate.Fields.BTNTextFieldName].Value.ToString() : "";
                            banner.class1 = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.class1FieldName].Value.ToString()) ? item.Fields[BannerTemplate.Fields.class1FieldName].Value.ToString() : "";
                            banner.class2 = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.class2FieldName].Value.ToString()) ? item.Fields[BannerTemplate.Fields.class2FieldName].Value.ToString() : "";
                            var emailLink = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, BannerTemplate.Fields.LinkFieldName) != null ?
                                    Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, BannerTemplate.Fields.LinkFieldName) : "";
                            banner.emailLink = emailLink.Contains(strSitedomain) ? emailLink.Replace(strSitedomain, "") : emailLink;
                            mainBannerobj.Add(banner);
                        }
                    }
                }
                else
                {
                    foreach (Sitecore.Data.Items.Item item in datasource.Children)
                    {
                        Banner banner = new Banner();
                        List<Object> Bannerobj = new List<Object>();
                        banner.src = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, BannerTemplate.Fields.thumbFieldName) != null ?
                                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, BannerTemplate.Fields.thumbFieldName) : "";
                        banner.alt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName) != null ?
                                             Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                        banner.thumb = Helper.GetImageSource(item, BannerTemplate.Fields.logoFieldName) != null ?
                                           Helper.GetImageSource(item, BannerTemplate.Fields.logoFieldName) : "";
                        banner.title = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName) != null ?
                                  Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                        banner.headerdesc = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.headingFieldName].Value.ToString()) ? item.Fields[BannerTemplate.Fields.headingFieldName].Value.ToString() : "";
                        banner.buttontext = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.BTNTextFieldName].Value.ToString()) ? item.Fields[BannerTemplate.Fields.BTNTextFieldName].Value.ToString() : "";
                        banner.class1 = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.class1FieldName].Value.ToString()) ? item.Fields[BannerTemplate.Fields.class1FieldName].Value.ToString() : "";
                        banner.class2 = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.class2FieldName].Value.ToString()) ? item.Fields[BannerTemplate.Fields.class2FieldName].Value.ToString() : "";
                        var emailLink = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, BannerTemplate.Fields.LinkFieldName) != null ?
                                Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, BannerTemplate.Fields.LinkFieldName) : "";
                        banner.emailLink = emailLink.Contains(strSitedomain) ? emailLink.Replace(strSitedomain, "") : emailLink;
                        mainBannerobj.Add(banner);
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroCarouselService GetLocationBanner gives -> " + ex.Message);
            }

            return mainBannerobj;
        }
        public Banner GetCommunicationBanner(Rendering rendering)
        {
            Banner mainBannerobj = new Banner();
            try
            {
                var currentItem = Sitecore.Context.Item;
                mainBannerobj.src = Helper.GetImageSource(currentItem, Communicationcornertemplate.Fields.ImageFieldName) != null ?
                                          Helper.GetImageSource(currentItem, Communicationcornertemplate.Fields.ImageFieldName) : ""; ;
                mainBannerobj.alt = Helper.GetImageDetails(currentItem, Communicationcornertemplate.Fields.ImageFieldName) != null ?
                                         Helper.GetImageDetails(currentItem, Communicationcornertemplate.Fields.ImageFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                mainBannerobj.title = Helper.GetImageDetails(currentItem, Communicationcornertemplate.Fields.ImageFieldName) != null ?
                             Helper.GetImageDetails(currentItem, Communicationcornertemplate.Fields.ImageFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                mainBannerobj.headerdesc = !string.IsNullOrEmpty(currentItem.Fields[Communicationcornertemplate.Fields.TitleID].Value.ToString()) ? currentItem.Fields[Communicationcornertemplate.Fields.TitleID].Value.ToString() : "";
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroCarouselService GetCommunicationBanner gives -> " + ex.Message);
            }

            return mainBannerobj;
        }
        public List<Banner> GetBannerComponent(Rendering rendering)
        {
            string strSitedomain = string.Empty;
            var CItem = Sitecore.Context.Database.GetItem(commonItem);
            strSitedomain = CItem != null ? CItem.Fields["Site Domain"].Value : string.Empty;
            List<Banner> mainBannerobj = new List<Banner>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }

                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    Banner banner = new Banner();
                    List<Object> Bannerobj = new List<Object>();
                    banner.src = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, BannerTemplate.Fields.thumbFieldName) != null ?
                                        Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, BannerTemplate.Fields.thumbFieldName) : "";
                    banner.srcMobile = Helper.GetImageSource(item, BannerTemplate.Fields.MobileImage) != null ?
                                       Helper.GetImageSource(item, BannerTemplate.Fields.MobileImage) : "";
                    banner.alt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName) != null ?
                                         Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    banner.thumb = Helper.GetImageSource(item, BannerTemplate.Fields.logoFieldName) != null ?
                                       Helper.GetImageSource(item, BannerTemplate.Fields.logoFieldName) : "";
                    banner.title = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName) != null ?
                              Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                    banner.headerdesc = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.headingFieldName].Value.ToString()) ? item.Fields[BannerTemplate.Fields.headingFieldName].Value.ToString() : "";
                    banner.buttontext = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.BTNTextFieldName].Value.ToString()) ? item.Fields[BannerTemplate.Fields.BTNTextFieldName].Value.ToString() : "";
                    banner.class1 = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.class1FieldName].Value.ToString()) ? item.Fields[BannerTemplate.Fields.class1FieldName].Value.ToString() : "";
                    banner.class2 = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.class2FieldName].Value.ToString()) ? item.Fields[BannerTemplate.Fields.class2FieldName].Value.ToString() : "";
                    var emailLink = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, BannerTemplate.Fields.LinkFieldName) != null ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, BannerTemplate.Fields.LinkFieldName) : "";
                    banner.emailLink = emailLink.Contains(strSitedomain) ? emailLink.Replace(strSitedomain, "") : emailLink;
                    mainBannerobj.Add(banner);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroCarouselService GetBannerComponent gives -> " + ex.Message);
            }

            return mainBannerobj;
        }
        public List<AboutUsBanner> GetAboutUsBannerComponent(Rendering rendering)
        {
            List<AboutUsBanner> mainBannerobj = new List<AboutUsBanner>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }

                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    AboutUsBanner banner = new AboutUsBanner();
                    List<Object> Bannerobj = new List<Object>();
                    banner.leftImg = Helper.GetImageSource(item, BannerTemplate.Fields.thumbFieldName) != null ?
                                        Helper.GetImageSource(item, BannerTemplate.Fields.thumbFieldName) : "";
                    banner.imgAlt = Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName) != null ?
                                         Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    banner.imgTitle = Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName) != null ?
                              Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName).Fields[ImageFeilds.Fields.TitleFieldName].Value : "";
                    banner.rightImg = Helper.GetImageSource(item, BannerTemplate.Fields.logoFieldName) != null ?
                                       Helper.GetImageSource(item, BannerTemplate.Fields.logoFieldName) : "";
                    banner.heading = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.headingFieldName].Value.ToString()) ? item.Fields[BannerTemplate.Fields.headingFieldName].Value.ToString() : "";
                    banner.subHeading = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.SubheaadingID].Value.ToString()) ? item.Fields[BannerTemplate.Fields.SubheaadingID].Value.ToString() : "";
                    banner.description = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.TitleID].Value.ToString()) ? item.Fields[BannerTemplate.Fields.TitleID].Value.ToString() : "";
                    mainBannerobj.Add(banner);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetAboutUsBannerComponent gives -> " + ex.Message);
            }

            return mainBannerobj;
        }

        public BrandIconList GetBrandIconList(Rendering rendering)
        {
            BrandIconList brandIconList = new BrandIconList();
            try
            {

                brandIconList.data = GetBrandIconItem(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" HeroCarouselService GetBrandIconList gives -> " + ex.Message);
            }


            return brandIconList;
        }
        public List<Object> GetBrandIconItem(Rendering rendering)
        {
            List<Object> brandIconList = new List<Object>();
            List<Item> listofitem = new List<Item>();
            try
            {
                Item contextItem1 = Sitecore.Context.Database.GetItem(BrandICons.CommercialItemID);
                listofitem.AddRange(contextItem1.Axes.GetDescendants().Where(x => (x.TemplateID.ToString() == BrandICons.CommercialTemplateID.ToString())));
                Item contextItem2 = Sitecore.Context.Database.GetItem(BrandICons.ResidentialItemID);
                listofitem.AddRange(contextItem2.Axes.GetDescendants().Where(x => (x.TemplateID.ToString() == BrandICons.ResidentialTemplateID.ToString())));


                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }

                List<Item> dropOutChild = listofitem.Distinct().ToList();

                BranIconItem carousel;
                foreach (Sitecore.Data.Items.Item item in datasource.Children)
                {
                    var locCount = dropOutChild.Where(x => x.Fields[BrandICons.Fields.LocationFieldName].Value.ToLower().Contains(item.Name.ToLower())).ToList().Count();
                    if (locCount != 0)
                    {
                        var cityCount = dropOutChild.GroupBy(x => x.Fields[BrandICons.Fields.LocationFieldName].Value).ToList();
                        carousel = new BranIconItem();
                        carousel.src = Helper.GetImageSource(item, BrandICons.Fields.thumbFieldName) != null ?
                                            Helper.GetImageSource(item, BrandICons.Fields.thumbFieldName) : "";
                        carousel.mobileimage = Helper.GetImageSource(item, BrandICons.Fields.mobileimageFieldName) != null ?
                                            Helper.GetImageSource(item, BrandICons.Fields.mobileimageFieldName) : "";
                        carousel.imgAlt = Helper.GetImageDetails(item, BrandICons.Fields.thumbFieldName) != null ?
                                        Helper.GetImageDetails(item, BrandICons.Fields.thumbFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                        carousel.location = item.Name;
                        carousel.longitude = !string.IsNullOrEmpty(item.Fields[BrandICons.Fields.LongitudeID].Value.ToString()) ? item.Fields[BrandICons.Fields.LongitudeID].Value.ToString() : "";
                        carousel.latitude = !string.IsNullOrEmpty(item.Fields[BrandICons.Fields.LatitudeID].Value.ToString()) ? item.Fields[BrandICons.Fields.LatitudeID].Value.ToString() : "";
                        //var locCount = dropOutChild.Where(x => x.Fields[BrandICons.Fields.LocationFieldName].Value.ToLower().Contains(item.Name.ToLower())).ToList().Count();
                        if (locCount > 1)
                        {
                            carousel.projectcount = locCount + " Projects";
                        }
                        else
                        {
                            carousel.projectcount = locCount + " Project";
                        }

                        carousel.link = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, BrandICons.Fields.LinkFieldName) != null ?
                                Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, BrandICons.Fields.LinkFieldName) : "";
                        brandIconList.Add(carousel);
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetBrandIconItem gives -> " + ex.Message);
            }
            return brandIconList;
        }
        public ProjectFoundStatus GetProjectLocation(Rendering rendering, string location = null)
        {
            ProjectFoundStatus projectFoundStatus = new ProjectFoundStatus();
            List<Item> listOfProperty = new List<Item>();
            var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                                    ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                                    : null;
            var propertyType = datasource.TemplateID.ToString() == PropertiesLIst.TemplateID.ToString() ? datasource.GetMultiListValueItem(PropertiesLIst.PropertiesID) : null;
            var commonItem = Sitecore.Context.Database.GetItem(Templates.commonItem);

            if (propertyType != null)
            {
                foreach (var propertyTypeItem in propertyType)
                {
                    var x = propertyTypeItem.Axes.GetDescendants().Where(y => y.TemplateID == PropertiesLIst.ResidentialID || y.TemplateID == PropertiesLIst.CommercialID);
                    listOfProperty.AddRange(x);
                }
            }
            try
            {
                if (!string.IsNullOrEmpty(location))
                {
                    foreach (Item item in listOfProperty)
                    {
                        var cityValue = item.Fields[PropertiesLIst.Fields.LocationID].Value;
                        if (cityValue.ToLower().Contains(location.ToString().ToLower()))
                        {
                            projectFoundStatus.locationHeading = !string.IsNullOrEmpty(item.Parent.Fields[PropertiesLIst.Fields.StaticTextLabelID].Value) ? item.Parent.Fields[PropertiesLIst.Fields.StaticTextLabelID].Value : "";
                        }
                    }
                    projectFoundStatus.projectsfound = commonItem?.Fields[PropertiesLIst.Fields.Projectsfoundtext].Value;
                    projectFoundStatus.projectfound = commonItem?.Fields[PropertiesLIst.Fields.Projectfoundtext].Value;
                    //projectFoundStatus.city = location;
                    projectFoundStatus.city = char.ToUpper(location[0]) + location.Substring(1);
                    if (string.IsNullOrEmpty(projectFoundStatus.city))
                    {
                        projectFoundStatus.projectsfound = commonItem?.Fields[PropertiesLIst.Fields.projectsFound].Value; ; 
                    }
                }
                else if (datasource.TemplateID == SEOpage.BaseTemplateID)
                {
                    projectFoundStatus.locationHeading = commonItem?.Fields[PropertiesLIst.Fields.Locationpagebreadcrumb].Value;
                    projectFoundStatus.projectsfound = commonItem?.Fields[PropertiesLIst.Fields.Projectsfoundtext].Value;
                    projectFoundStatus.projectfound = commonItem?.Fields[PropertiesLIst.Fields.Projectfoundtext].Value;
                    projectFoundStatus.city = !string.IsNullOrEmpty(datasource.Fields[PropertiesLIst.Fields.CityName].Value.ToString()) ? datasource.Fields[PropertiesLIst.Fields.CityName].Value.ToString() : ""; ;
                    if (string.IsNullOrEmpty(projectFoundStatus.city))
                    {
                        projectFoundStatus.projectsfound = commonItem?.Fields[PropertiesLIst.Fields.projectsFound].Value; ;
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetProjectLocation gives -> " + ex.Message);
            }
            return projectFoundStatus;
        }
        public List<Object> GetEnquiryFormProperty(Rendering rendering)
        {
            List<object> returnData = new List<object>();
            List<Item> listOfProperty = new List<Item>();
            ProppertiesList proppertiesList = new ProppertiesList();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                                    ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                                    : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                var propertyType = datasource.TemplateID.ToString() == PropertiesLIst.TemplateID.ToString() ? datasource.GetMultiListValueItem(PropertiesLIst.PropertiesID) : null;

                if (propertyType != null)
                {
                    foreach (var propertyTypeItem in propertyType)
                    {
                        var x = propertyTypeItem.Axes.GetDescendants().Where(y => y.TemplateID == PropertiesLIst.ResidentialID || y.TemplateID == PropertiesLIst.CommercialID || y.TemplateID == PropertiesLIst.ClubID);
                        listOfProperty.AddRange(x);
                    }
                }
                else
                {
                    throw new NullReferenceException();
                }
                foreach (Item item in listOfProperty)
                {
                    var status = item.Fields[PropertiesLIst.Fields.isProjectCompleted] != null ? Helper.GetCheckBoxSelection(item.Fields[PropertiesLIst.Fields.isProjectCompleted]) : false;
                    if (!status)
                    {
                        proppertiesList.data = Properties(rendering, item);
                        returnData.Add(new ProppertiesList() { carouselImages = proppertiesList.carouselImages, data = proppertiesList.data });
                    }
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetEnquiryFormProperty gives -> " + ex.Message);
            }
            return returnData;
        }
        public List<Object> GetSeoPropertyList(Rendering rendering)
        {
            List<object> returnData = new List<object>();
            List<Item> listOfProperty = new List<Item>();
            ProppertiesList proppertiesList = new ProppertiesList();
            List<Item> data = new List<Item>();
            List<Item> filtereddata = new List<Item>();
            try
            {

                var datasource = !string.IsNullOrEmpty(rendering.DataSource) ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource) : null;
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                var propertyType = datasource.TemplateID.ToString() == PropertiesLIst.TemplateID.ToString() ? datasource.GetMultiListValueItem(PropertiesLIst.PropertiesID) : null;
                if (propertyType != null)
                {
                    foreach (var propertyTypeItem in propertyType)
                    {
                        var x = propertyTypeItem.Axes.GetDescendants().Where(y => y.TemplateID == PropertiesLIst.ResidentialID || y.TemplateID == PropertiesLIst.CommercialID);
                        listOfProperty.AddRange(x);
                    }
                }
                else
                {
                    throw new NullReferenceException();
                }

                var SeoLocation = Sitecore.Context.Item.Fields["CityName"].Value; //SEO Page
                if (string.IsNullOrEmpty(SeoLocation))
                {
                    filtereddata.AddRange(listOfProperty);
                }
                if (!string.IsNullOrEmpty(SeoLocation))
                {
                    foreach (Item item in listOfProperty)
                    {
                        var locationItem = Helper.GetSelectedItemFromDroplistFieldValue(item, PropertiesLIst.Fields.Location); //All Property
                        if (SeoLocation == locationItem)
                        {
                            data.Add(item);
                        }
                    }
                    filtereddata.Clear();
                }
                filtereddata.AddRange(data);
                data.Clear();

                var SeoProjectType = Helper.GetSelectedItemFromDroplistFieldValue(Sitecore.Context.Item, PropertiesLIst.Fields.SEOProjectType); //SEO Page
                if (SeoProjectType != null)
                {
                    foreach (Item item in filtereddata)
                    {
                        var projectType = Helper.GetSelectedItemFromDroplistFieldValue(item, PropertiesLIst.Fields.ProjectType); //All Property
                        if (SeoProjectType == projectType)
                        {
                            data.Add(item);
                        }
                    }
                    filtereddata.Clear();
                }
                filtereddata.AddRange(data);
                data.Clear();

                var area = Helper.GetSelectedItemFromDroplistFieldValue(Sitecore.Context.Item, PropertiesLIst.Fields.Area); //SEO Page
                if (!string.IsNullOrEmpty(area))
                {
                    foreach (var item in filtereddata)
                    {
                        var subcity = Helper.GetSelectedItemFromDroplistFieldValue(item, PropertiesLIst.Fields.Subcity); //All Property
                        if (area == subcity)
                        {
                            data.Add(item);
                        }
                    }
                    filtereddata.Clear();
                    filtereddata.AddRange(data);
                }
                data.Clear();

                var SeoFlats = Helper.GetSelectedItemFromDroplistFieldValue(Sitecore.Context.Item, PropertiesLIst.Fields.Flats); ; //SEO Page
                if (!string.IsNullOrEmpty(SeoFlats))
                {
                    List<string> FlatItem = new List<string>();
                    foreach (var item in filtereddata)
                    {
                        var Flat = item.GetMultiListValueItem(PropertiesLIst.Fields.PropertyType);
                        foreach (var type in Flat)
                        {
                            var x = !string.IsNullOrEmpty(type.DisplayName) ? type.DisplayName : type.Name;
                            FlatItem.Add(x);
                        }
                        if (FlatItem.Contains(SeoFlats))
                        {
                            data.Add(item);
                        }
                        FlatItem.Clear();

                    }
                    filtereddata.Clear();
                    filtereddata.AddRange(data);
                }
                data.Clear();


                var SeoprojectStatus = Helper.GetSelectedItemFromDroplistFieldValue(Sitecore.Context.Item, PropertiesLIst.Fields.SEOProjectStatus); //SEO Page
                if (!string.IsNullOrEmpty(SeoprojectStatus))
                {
                    foreach (var item in filtereddata)
                    {
                        var projectstatus = Helper.GetSelectedItemFromDroplistFieldValue(item, PropertiesLIst.Fields.ProjectStatus); //All Property
                        if (SeoprojectStatus.ToLower() == projectstatus.ToLower())
                        {
                            data.Add(item);
                        }
                    }
                    filtereddata.Clear();
                    filtereddata.AddRange(data);
                }
                data.Clear();



                var SeotypeItem = Helper.GetSelectedItemFromDroplistFieldValue(Sitecore.Context.Item, PropertiesLIst.Fields.ConfugurationType); //SEO Page
                if (!string.IsNullOrEmpty(SeotypeItem))
                {
                    List<string> FlatItem = new List<string>();
                    foreach (var item in filtereddata)
                    {
                        var Propertytype = item.GetMultiListValueItem(PropertiesLIst.Fields.typeID);
                        foreach (var type in Propertytype)
                        {
                            var x = !string.IsNullOrEmpty(type.DisplayName) ? type.DisplayName : type.Name;
                            FlatItem.Add(x);
                        }
                        if (FlatItem.Contains(SeotypeItem))
                        {
                            data.Add(item);
                        }
                        FlatItem.Clear();
                    }
                    filtereddata.Clear();
                    filtereddata.AddRange(data);
                }
                data.Clear();



                foreach (Item item in filtereddata)
                {
                    proppertiesList.carouselImages = GetPropertyItem(rendering, item);
                    proppertiesList.data = Properties(rendering, item);
                    returnData.Add(new ProppertiesList() { carouselImages = proppertiesList.carouselImages, data = proppertiesList.data });
                }

                return returnData;
            }
            catch (Exception)
            {

                throw new NullReferenceException();
            }
        }
        public List<Object> GetPropertyList(Rendering rendering, string location = null, string type = null, string status = null)
        {
            List<object> returnData = new List<object>();
            List<Item> listOfProperty = new List<Item>();
            ProppertiesList proppertiesList = new ProppertiesList();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                                    ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                                    : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                var propertyType = datasource.TemplateID.ToString() == PropertiesLIst.TemplateID.ToString() ? datasource.GetMultiListValueItem(PropertiesLIst.PropertiesID) : null;

                if (propertyType != null)
                {
                    foreach (var propertyTypeItem in propertyType)
                    {
                        var x = propertyTypeItem.Axes.GetDescendants().Where(y => y.TemplateID == PropertiesLIst.ResidentialID || y.TemplateID == PropertiesLIst.CommercialID);
                        listOfProperty.AddRange(x);
                    }
                }
                else
                {
                    throw new NullReferenceException();
                }

                if (string.IsNullOrEmpty(location) && string.IsNullOrEmpty(type) && string.IsNullOrEmpty(status))
                {
                    foreach (Item item in listOfProperty)
                    {
                        proppertiesList.carouselImages = GetPropertyItem(rendering, item);
                        proppertiesList.data = Properties(rendering, item);
                        returnData.Add(new ProppertiesList() { carouselImages = proppertiesList.carouselImages, data = proppertiesList.data });
                    }
                }
                else
                {
                    List<Item> newListOfProperty = new List<Item>();
                    List<Item> propertyTypeList = new List<Item>();
                    if (listOfProperty.Count() > 0)
                    {
                        List<Item> a = listOfProperty.Where(x => x.Fields[PropertiesLIst.Fields.PropertyRankingonSite].Value != "0" && x.Fields[PropertiesLIst.Fields.PropertyRankingonSite].Value != "").OrderBy(x => Convert.ToInt32(x.Fields[PropertiesLIst.Fields.PropertyRankingonSite].Value)).ToList();
                        List<Item> b = listOfProperty.Where(x => x.Fields[PropertiesLIst.Fields.PropertyRankingonSite].Value == "").ToList();
                        if (a.Count() != 0)
                        {
                            newListOfProperty.AddRange(a);
                        }
                        if (b.Count() != 0)
                        {
                            newListOfProperty.AddRange(b);
                        }
                        var residentialPropertyList = newListOfProperty.Where(x => x.TemplateID == PropertiesLIst.ResidentialID).ToList();
                        var commercialPropertyList = newListOfProperty.Where(x => x.TemplateID == PropertiesLIst.CommercialID).ToList();
                        if (residentialPropertyList.Count > 0)
                        {
                            propertyTypeList.AddRange(residentialPropertyList);
                        }
                        if (commercialPropertyList.Count > 0)
                        {
                            propertyTypeList.AddRange(commercialPropertyList);
                        }
                        if (!string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(status) && !string.IsNullOrEmpty(type))
                        {
                            foreach (Item item in propertyTypeList)
                            {
                                var cityValue = item.Parent.Name.ToLower();
                                // var statuscheck = Helper.GetSelectedItemFromDroplistField(item, PropertiesLIst.Fields.ProjectStatus).ToLower();
                                var statuscheck = Helper.GetSelectedItemFromDroplistFieldValue(item, PropertiesLIst.Fields.ProjectStatus);
                                //var statusValue = item.Fields[PropertiesLIst.Fields.ProjectStatus].Value;
                                var propertyTypeValue = item.Fields[PropertiesLIst.Fields.PropertyTypeID].Value;
                                var ConfigurationValue = item.Fields[PropertiesLIst.Fields.typeID].Value;
                                var myList = ConfigurationValue.Split('|').ToList();

                                if (!string.IsNullOrEmpty(cityValue) && !string.IsNullOrEmpty(statuscheck) && !string.IsNullOrEmpty(item.TemplateName) && statuscheck.ToLower().Replace(" ", "_").Contains(status.ToLower()) && item.TemplateName.ToLower().Contains(type.ToLower()) && cityValue.ToLower() == location.ToLower())
                                {
                                    proppertiesList.carouselImages = GetPropertyItem(rendering, item);
                                    proppertiesList.data = Properties(rendering, item);
                                    returnData.Add(new ProppertiesList() { carouselImages = proppertiesList.carouselImages, data = proppertiesList.data });
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(status))
                        {
                            foreach (Item item in propertyTypeList)
                            {
                                var cityValue = item.Fields[PropertiesLIst.Fields.LocationID].Value;
                                var statuscheck = Helper.GetSelectedItemFromDroplistFieldValue(item, PropertiesLIst.Fields.ProjectStatus);
                                //var statusValue = item.Fields[PropertiesLIst.Fields.ProjectStatus].Value;
                                if (!string.IsNullOrEmpty(cityValue) && !string.IsNullOrEmpty(statuscheck) && cityValue.ToLower().Contains(location.ToLower()) && statuscheck.ToLower().Replace(" ", "_").Contains(status.ToLower()))
                                {
                                    proppertiesList.carouselImages = GetPropertyItem(rendering, item);
                                    proppertiesList.data = Properties(rendering, item);
                                    returnData.Add(new ProppertiesList() { carouselImages = proppertiesList.carouselImages, data = proppertiesList.data });
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(location) && !string.IsNullOrEmpty(type))
                        {
                            foreach (Item item in propertyTypeList)
                            {
                                var propertyTypeValue = item.Fields[PropertiesLIst.Fields.PropertyTypeID].Value;
                                var cityValue = item.Fields[PropertiesLIst.Fields.LocationID].Value;
                                if (cityValue.ToLower().Contains(location.ToLower()) && item.TemplateName.ToLower().Contains(type.ToLower()))
                                {
                                    proppertiesList.carouselImages = GetPropertyItem(rendering, item);
                                    proppertiesList.data = Properties(rendering, item);
                                    returnData.Add(new ProppertiesList() { carouselImages = proppertiesList.carouselImages, data = proppertiesList.data });
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(type) && !string.IsNullOrEmpty(status))
                        {
                            foreach (Item item in propertyTypeList)
                            {

                                //var statusValue = item.Fields[PropertiesLIst.Fields.ProjectStatus].Value;
                                //    var statuscheck = Helper.GetSelectedItemFromDroplistField(item, PropertiesLIst.Fields.ProjectStatus).ToLower();
                                var statuscheck = Helper.GetSelectedItemFromDroplistFieldValue(item, PropertiesLIst.Fields.ProjectStatus);
                                var propertyTypeValue = item.Fields[PropertiesLIst.Fields.PropertyTypeID].Value;
                                if (!string.IsNullOrEmpty(statuscheck) && statuscheck.ToLower().Replace(" ", "_").Contains(status.ToLower()) && item.TemplateName.ToLower().Contains(type.ToLower()))
                                {
                                    proppertiesList.carouselImages = GetPropertyItem(rendering, item);
                                    proppertiesList.data = Properties(rendering, item);
                                    returnData.Add(new ProppertiesList() { carouselImages = proppertiesList.carouselImages, data = proppertiesList.data });
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(location))
                        {
                            foreach (Item item in propertyTypeList)
                            {
                                var cityValue = item.Parent.Name.ToLower();
                                if (cityValue.ToLower() == (location.ToString().ToLower()))
                                {
                                    proppertiesList.carouselImages = GetPropertyItem(rendering, item);
                                    proppertiesList.data = Properties(rendering, item);
                                    returnData.Add(new ProppertiesList() { carouselImages = proppertiesList.carouselImages, data = proppertiesList.data });
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(type))
                        {
                            foreach (Item item in propertyTypeList)
                            {
                                var propertyTypeValue = item.Fields[PropertiesLIst.Fields.PropertyTypeID].Value;
                                if (item.TemplateName.ToLower().Contains(type.ToLower()))
                                {
                                    proppertiesList.carouselImages = GetPropertyItem(rendering, item);
                                    proppertiesList.data = Properties(rendering, item);
                                    returnData.Add(new ProppertiesList() { carouselImages = proppertiesList.carouselImages, data = proppertiesList.data });
                                }
                            }
                        }
                        else if (!string.IsNullOrEmpty(status))
                        {
                            foreach (Item item in propertyTypeList)
                            {
                                //  var statuscheck = Helper.GetSelectedItemFromDroplistField(item, PropertiesLIst.Fields.ProjectStatus).ToLower();
                                var statuscheck = Helper.GetSelectedItemFromDroplistFieldValue(item, PropertiesLIst.Fields.ProjectStatus);
                                //var statusValue = item.Fields[PropertiesLIst.Fields.ProjectStatus].Value;
                                if (!string.IsNullOrEmpty(statuscheck) && statuscheck.ToLower().Replace(" ", "_").Contains(status.ToLower()))
                                {
                                    proppertiesList.carouselImages = GetPropertyItem(rendering, item);
                                    proppertiesList.data = Properties(rendering, item);
                                    returnData.Add(new ProppertiesList() { carouselImages = proppertiesList.carouselImages, data = proppertiesList.data });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetPropertyList gives -> " + ex.Message);
            }
            return returnData;
        }
        public Shantigramhighlights GetShantigramhighlights(Rendering rendering)
        {
            Shantigramhighlights shantigramhighlights = new Shantigramhighlights();
            List<Cards> HighlightCards = new List<Cards>();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                                      ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                                      : null;

                var highlights = datasource.GetMultiListValueItem(Highlights.cards);
                foreach (var highlight in highlights)
                {
                    Cards card = new Cards();
                    card.frontImage = Helper.GetImageSource(highlight, Highlights.card.frontImage.ToString()) != null ?
                                        Helper.GetImageSource(highlight, Highlights.card.frontImage.ToString()) : "";
                    card.backImage = Helper.GetImageSource(highlight, Highlights.card.backImage.ToString()) != null ?
                                        Helper.GetImageSource(highlight, Highlights.card.backImage.ToString()) : "";
                    card.imageAlt = Helper.GetImageDetails(highlight, Highlights.card.frontImage.ToString()) != null ?
                                       Helper.GetImageDetails(highlight, Highlights.card.frontImage.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    card.title = !string.IsNullOrEmpty(highlight.Fields[Highlights.card.title].Value.ToString()) ? highlight.Fields[Highlights.card.title].Value.ToString() : "";
                    card.cardLink = Helper.GetPropLinkURLbyField(highlight, highlight.Fields[Highlights.card.cardLink]) != null ?
                                        Helper.GetPropLinkURLbyField(highlight, highlight.Fields[Highlights.card.cardLink]) : "";
                    card.target = Helper.GetLinkURLTargetSpace(highlight, Highlights.card.cardLink.ToString()) != null ?
                                        Helper.GetLinkURLTargetSpace(highlight, Highlights.card.cardLink.ToString()) : "";
                    card.backImageAlt = Helper.GetImageDetails(highlight, Highlights.card.backImage.ToString()) != null ?
                                       Helper.GetImageDetails(highlight, Highlights.card.backImage.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    HighlightCards.Add(card);
                }
                shantigramhighlights.cards = HighlightCards;
                shantigramhighlights.disclaimer = !string.IsNullOrEmpty(datasource.Fields[Highlights.disclaimer].Value.ToString()) ? datasource.Fields[Highlights.disclaimer].Value.ToString() : "";
                shantigramhighlights.heading = !string.IsNullOrEmpty(datasource.Fields[Highlights.heading].Value.ToString()) ? datasource.Fields[Highlights.heading].Value.ToString() : "";
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetShantigramhighlights gives -> " + ex.Message);
            }
            return shantigramhighlights;
        }
        public List<Object> GetTownshipPropertyList(Rendering rendering, string location = null)
        {
            List<object> returnData = new List<object>();
            List<Item> listOfProperty = new List<Item>();
            ProppertiesList proppertiesList = new ProppertiesList();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                                    ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                                    : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                var propertyType = datasource.GetMultiListValueItem(PropertiesLIst.PropertiesID);

                if (propertyType != null)
                {
                    listOfProperty.AddRange(propertyType);
                }
                else
                {
                    throw new NullReferenceException();
                }
                if (string.IsNullOrEmpty(location))
                {
                    List<Item> newListOfProperty = new List<Item>();
                    List<Item> propertyTypeList = new List<Item>();

                    List<Item> a = listOfProperty.Where(x => x.Fields[PropertiesLIst.Fields.PropertyRankingonSite].Value != "0" && x.Fields[PropertiesLIst.Fields.PropertyRankingonSite].Value != "").OrderBy(x => Convert.ToInt32(x.Fields[PropertiesLIst.Fields.PropertyRankingonSite].Value)).ToList();
                    List<Item> b = listOfProperty.Where(x => x.Fields[PropertiesLIst.Fields.PropertyRankingonSite].Value == "").ToList();
                    if (a.Count() != 0)
                    {
                        newListOfProperty.AddRange(a);
                    }
                    if (b.Count() != 0)
                    {
                        newListOfProperty.AddRange(b);
                    }
                    var residentialPropertyList = newListOfProperty.Where(x => x.TemplateID == PropertiesLIst.ResidentialID).ToList();
                    var commercialPropertyList = newListOfProperty.Where(x => x.TemplateID == PropertiesLIst.CommercialID).ToList();
                    if (residentialPropertyList.Count > 0)
                    {
                        propertyTypeList.AddRange(residentialPropertyList);
                    }
                    if (commercialPropertyList.Count > 0)
                    {
                        propertyTypeList.AddRange(commercialPropertyList);
                    }
                    foreach (Item item in propertyTypeList)
                    {
                        proppertiesList.carouselImages = GetPropertyItem(rendering, item);
                        proppertiesList.data = Properties(rendering, item);
                        returnData.Add(new ProppertiesList() { carouselImages = proppertiesList.carouselImages, data = proppertiesList.data });
                    }
                }
                else
                {
                    if (!string.IsNullOrEmpty(location))
                    {
                        foreach (Item item in listOfProperty)
                        {
                            var cityValue = item.Fields[PropertiesLIst.Fields.LocationID].Value;
                            if (cityValue.ToLower() == location.ToLower())
                            {
                                proppertiesList.carouselImages = GetPropertyItem(rendering, item, location);
                                proppertiesList.data = Properties(rendering, item, location);
                                returnData.Add(new ProppertiesList() { carouselImages = proppertiesList.carouselImages, data = proppertiesList.data });
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetTownshipPropertyList gives -> " + ex.Message);
            }
            return returnData;
        }
        public List<Object> GetPropertyItem(Rendering rendering, Item item, string location = null, string type = null, string status = null)
        {
            List<Object> proppertiesList = new List<Object>();
            try
            {
                List<carouselImagesItem> carouselImagesList = new List<carouselImagesItem>();
                var multiListForcarousel = item.GetMultiListValueItem(PropertiesLIst.MediaSelectionID);
                foreach (var media in multiListForcarousel)
                {
                    carouselImagesItem carouselImagesItem = new carouselImagesItem();
                    carouselImagesItem.src = Helper.GetImageSource(media, PropertiesLIst.Fields.thumbID.ToString()) != null ?
                                    Helper.GetImageSource(media, PropertiesLIst.Fields.thumbID.ToString()) : "";
                    carouselImagesItem.alt = Helper.GetImageDetails(media, PropertiesLIst.Fields.thumbID.ToString()) != null ?
                                   Helper.GetImageDetails(media, PropertiesLIst.Fields.thumbID.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    carouselImagesItem.porjectLogo = Helper.GetImageSource(item, PropertiesLIst.Fields.PropertyLogoFieldName) != null ?
                                    Helper.GetImageSource(item, PropertiesLIst.Fields.PropertyLogoFieldName) : "";
                    carouselImagesItem.porjectLogoAlt = Helper.GetImageDetails(item, PropertiesLIst.Fields.PropertyLogoFieldName) != null ?
                                    Helper.GetImageDetails(item, PropertiesLIst.Fields.PropertyLogoFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                    carouselImagesItem.imageDesc = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.imgtypeID].Value.ToString()) ? media.Fields[PropertiesLIst.Fields.imgtypeID].Value.ToString() : "";
                    carouselImagesList.Add(carouselImagesItem);
                }
                proppertiesList.Add(new propertyDetails() { Items = carouselImagesList });
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetPropertyItem gives -> " + ex.Message);
            }
            return proppertiesList;
        }
        public PropertiesItemdata Properties(Rendering rendering, Item item, string location = null, string type = null, string status = null)
        {
            PropertiesItemdata proppertiesItemdata = new PropertiesItemdata();
            try
            {
                List<string> listOfProperty = new List<string>();
                List<string> listofConfiguration = new List<string>();
                List<Object> proppertiesItemdataItem = new List<Object>();
                proppertiesItemdata.rera = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.ReraID].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.ReraID].Value.ToString() : "";
                proppertiesItemdata.PropertyID = item.ID.ToString();
                proppertiesItemdata.link = Helper.GetLinkURLbyField(item, item.Fields[PropertiesLIst.Fields.PropertyLinkID]) != null ?
                                    Helper.GetLinkURLbyField(item, item.Fields[PropertiesLIst.Fields.PropertyLinkID]) : "";
                proppertiesItemdata.linktarget = Helper.GetLinkURLTargetSpace(item, PropertiesLIst.Fields.PropertyLinkID.ToString()) != null ?
                                    Helper.GetLinkURLTargetSpace(item, PropertiesLIst.Fields.PropertyLinkID.ToString()) : "";
                proppertiesItemdata.PropertyLocation = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.LocationID].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.LocationID].Value.ToString() : "";
                proppertiesItemdata.propertyStatus = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.SiteStatus].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.SiteStatus].Value.ToString() : "";
                proppertiesItemdata.propertyStatusID = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.ProjectStatus].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.ProjectStatus].Value.ToString() : "";
                proppertiesItemdata.projectName = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.TitleID].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.TitleID].Value.ToString() : "";
                proppertiesItemdata.projectSpec = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.projectSpec].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.projectSpec].Value.ToString() : "";
                proppertiesItemdata.areaLabel = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.areaLabel].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.areaLabel].Value.ToString() : "";
                proppertiesItemdata.priceLabel = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.priceLabel].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.priceLabel].Value.ToString() : "";
                proppertiesItemdata.areaDetail = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.areaDetail].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.areaDetail].Value.ToString() : "";
                proppertiesItemdata.priceDetail = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.priceDetail].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.priceDetail].Value.ToString() : "";
                proppertiesItemdata.onwards = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.onwards].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.onwards].Value.ToString() : "";
                proppertiesItemdata.condition = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.condition].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.condition].Value.ToString() : "";
                proppertiesItemdata.propertyPricefilter = item.Fields[PropertiesLIst.Fields.PropertyPriceFilterID].Value.ToString() == "NA" ? 0 : item.Fields[PropertiesLIst.Fields.PropertyPriceFilterID].Value.ToString() == "Not to be highlighted" ? 0 : !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.PropertyPriceFilterID].Value.ToString()) ? Convert.ToInt32(item.Fields[PropertiesLIst.Fields.PropertyPriceFilterID].Value) : 0;
                proppertiesItemdata.propertyPrice = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.PropertyPriceID].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.PropertyPriceID].Value.ToString() : "";
                proppertiesItemdata.propertyConfiguration = item.Fields[PropertiesLIst.Fields.typeID].Value.Split('|').ToList();
                proppertiesItemdata.propertyType = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.PropertyTypeID].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.PropertyTypeID].Value.ToString() : "";
                var data = item.Fields[PropertiesLIst.Fields.typeID].Value;
                var configurationType = item.GetMultiListValueItem(PropertiesLIst.Fields.typeID);
                foreach (var configTypeItem in configurationType)
                {
                    var y = !string.IsNullOrEmpty(configTypeItem.DisplayName) ? configTypeItem.DisplayName : configTypeItem.Name;
                    var x = configTypeItem.Name;
                    listOfProperty.Add(x);
                    listofConfiguration.Add(y);
                }
                proppertiesItemdata.propertyConfiguration = listofConfiguration;
                proppertiesItemdata.propertyConfigurationFilter = listOfProperty;
                proppertiesItemdata.propertyStatusFilter = proppertiesItemdata.propertyStatus.Replace(" ", "");
                proppertiesItemdata.propertyTypeFilter = item.Parent.Parent.TemplateID == PropertiesLIst.CommercialLandingID ? Constant.CommercialItemName : item.Parent.Parent.TemplateID == PropertiesLIst.ResidentialItemID ? Constant.ResidentialItemName : item.Parent.Parent.TemplateID == PropertiesLIst.ClubLandingID ? Constant.ClubItemName : "";
                proppertiesItemdata.township = !string.IsNullOrEmpty(item.Fields[PropertiesLIst.Fields.townshipID].Value.ToString()) ? item.Fields[PropertiesLIst.Fields.townshipID].Value.ToString() : "";


                proppertiesItemdataItem.Add(proppertiesItemdata);
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService Properties gives -> " + ex.Message);
            }
            return proppertiesItemdata;
        }

        public OfficeData GetOfficeData(Rendering rendering)
        {
            OfficeData officeData = new OfficeData();
            try
            {
                var Item = !string.IsNullOrEmpty(rendering.DataSource)
                                      ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                                      : null;
                if (Item.TemplateID == OfficeListTemp.BaseTemplateID)
                {
                    officeData.heading = !string.IsNullOrEmpty(Item.Fields[OfficeListTemp.BaseFields.TitleField].Value.ToString()) ? Item.Fields[OfficeListTemp.BaseFields.TitleField].Value.ToString() : "";
                    officeData.data = GetOfficeItem(rendering, Item);
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetOfficeData gives -> " + ex.Message);
            }
            return officeData;
        }
        public List<OfficeItem> GetOfficeItem(Rendering rendering, Item Item)
        {
            List<OfficeItem> officeList = new List<OfficeItem>();
            try
            {
                if (Item.HasChildren)
                {
                    var listOfOffice = Item.Children.Where(x => x.TemplateID == OfficeListTemp.ItemtemplateID).ToList();
                    if (listOfOffice.Count > 0)
                    {
                        foreach (var childItem in listOfOffice)
                        {
                            OfficeItem officeItem = new OfficeItem();
                            officeItem.src = Helper.GetImageSource(childItem, OfficeListTemp.Fields.imagesFieldName) != null ?
                                        Helper.GetImageSource(childItem, OfficeListTemp.Fields.imagesFieldName) : "";
                            officeItem.title = !string.IsNullOrEmpty(childItem.Fields[OfficeListTemp.Fields.TitleField].Value.ToString()) ? childItem.Fields[OfficeListTemp.Fields.TitleField].Value.ToString() : "";
                            officeItem.address = !string.IsNullOrEmpty(childItem.Fields[OfficeListTemp.Fields.AddressField].Value.ToString()) ? childItem.Fields[OfficeListTemp.Fields.AddressField].Value.ToString() : "";
                            officeList.Add(officeItem);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetOfficeItem gives -> " + ex.Message);
            }
            return officeList;
        }
        public ContactUsPageData GetContactUsPageData(Rendering rendering)
        {
            ContactUsPageData pageData = new ContactUsPageData();
            try
            {
                var Item = !string.IsNullOrEmpty(rendering.DataSource)
                                      ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                                      : null;
                if (Item.TemplateID == ContactUsPageDataTemp.BaseTemplateID)
                {
                    pageData.PageData = !string.IsNullOrEmpty(Item.Fields[ContactUsPageDataTemp.Fields.SummaryField].Value.ToString()) ? Item.Fields[ContactUsPageDataTemp.Fields.SummaryField].Value.ToString() : "";
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetOfficeData gives -> " + ex.Message);
            }
            return pageData;
        }
        public ResidentialProject GetResidentialBannerList(Rendering rendering)
        {
            ResidentialProject carouselImages = new ResidentialProject();
            var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                                     ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                                     : null;
            if (datasource == null)
            {
                throw new NullReferenceException();
            }
            try
            {
                var item = datasource.Children.FirstOrDefault();
                carouselImages.src = Helper.GetImageSource(item, BannerTemplate.Fields.thumbFieldName) != null ?
                                Helper.GetImageSource(item, BannerTemplate.Fields.thumbFieldName) : "";
                carouselImages.srcMobile = Helper.GetImageSource(item, BannerTemplate.Fields.MobileImage) != null ?
                                Helper.GetImageSource(item, BannerTemplate.Fields.MobileImage) : "";
                carouselImages.heading = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.headingID].Value.ToString()) ? item.Fields[BannerTemplate.Fields.headingID].Value.ToString() : "";
                carouselImages.alt = Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName) != null ?
                                        Helper.GetImageDetails(item, BannerTemplate.Fields.thumbFieldName).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                carouselImages.imgtitle = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.imgtypeID].Value.ToString()) ? item.Fields[BannerTemplate.Fields.imgtypeID].Value.ToString() : "";
                carouselImages.description = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.SubheaadingID].Value.ToString()) ? item.Fields[BannerTemplate.Fields.SubheaadingID].Value.ToString() : "";
                carouselImages.descriptionReadmore = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.readmoreID].Value.ToString()) ? item.Fields[BannerTemplate.Fields.readmoreID].Value.ToString() : "";
                carouselImages.readmore = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.CTATitleID].Value.ToString()) ? item.Fields[BannerTemplate.Fields.CTATitleID].Value.ToString() : "";
                carouselImages.readless = !string.IsNullOrEmpty(item.Fields[BannerTemplate.Fields.BTNtextID].Value.ToString()) ? item.Fields[BannerTemplate.Fields.BTNtextID].Value.ToString() : "";
            }
            catch (Exception ex)
            {
                _logRepository.Error(" HeroCarouselService GetResidentialList gives -> " + ex.Message);
            }
            return carouselImages;
        }
        public List<amenities> GetAmenities(Rendering rendering)
        {
            List<amenities> shantigramAmenities = new List<amenities>();
            var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                                     ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                                     : null;
            if (datasource == null)
            {
                throw new NullReferenceException("datasource is null");
            }
            var amenities = datasource.Children.ToList();
            foreach (var item in amenities)
            {
               amenities items = new amenities();
                items.mainHeading = !string.IsNullOrEmpty(item.Fields[tempAmenities.mainHeading].Value.ToString()) ? item.Fields[tempAmenities.mainHeading].Value.ToString() : "";
                items.id = !string.IsNullOrEmpty(item.Fields[tempAmenities.id].Value.ToString()) ? item.Fields[tempAmenities.id].Value.ToString() : "";
                items.dataList = getdatalist(item);
                shantigramAmenities.Add(items);
            }
            return shantigramAmenities;
        }

        public List<dataList> getdatalist(Item item)
        {
            List<dataList> shantigramDatalist = new List<dataList>();
            var datalists = item.GetMultiListValueItem(tempAmenities.dataLists);
            if (datalists == null)
            {
                throw new NullReferenceException("datalists is null");
            }
            foreach (var aminitiesitem in datalists)
            {
                dataList dataList = new dataList();
                dataList.sectionHeading = !string.IsNullOrEmpty(aminitiesitem.Fields[tempAmenities.dataList.sectionHeading].Value.ToString()) ? aminitiesitem.Fields[tempAmenities.dataList.sectionHeading].Value.ToString() : "";
                dataList.subHeading = !string.IsNullOrEmpty(aminitiesitem.Fields[tempAmenities.dataList.subHeading].Value.ToString()) ? aminitiesitem.Fields[tempAmenities.dataList.subHeading].Value.ToString() : "";
                dataList.description = !string.IsNullOrEmpty(aminitiesitem.Fields[tempAmenities.dataList.description].Value.ToString()) ? aminitiesitem.Fields[tempAmenities.dataList.description].Value.ToString() : "";
                dataList.imageSource = Helper.GetImageSource(aminitiesitem, tempAmenities.dataList.imageSource.ToString()) != null ?
                                              Helper.GetImageSource(aminitiesitem, tempAmenities.dataList.imageSource.ToString()) : "";
                dataList.imageSourceMobile = Helper.GetImageSource(aminitiesitem, tempAmenities.dataList.imageSourceMobile.ToString()) != null ?
                                              Helper.GetImageSource(aminitiesitem, tempAmenities.dataList.imageSourceMobile.ToString()) : "";
                dataList.imageSourceTablet = Helper.GetImageSource(aminitiesitem, tempAmenities.dataList.imageSourceTablet.ToString()) != null ?
                                              Helper.GetImageSource(aminitiesitem, tempAmenities.dataList.imageSourceTablet.ToString()) : "";
                dataList.imageAlt = Helper.GetImageDetails(aminitiesitem, tempAmenities.dataList.imageSource.ToString()) != null ?
                                                Helper.GetImageDetails(aminitiesitem, tempAmenities.dataList.imageSource.ToString()).Fields[ImageFeilds.Fields.AltFieldName].Value : "";
                shantigramDatalist.Add(dataList);

            }
            return shantigramDatalist;
        }

        //public List<object> GetPropertyList(Rendering rendering, string location, string type, string status)
        //{
        //    throw new NotImplementedException();
        //}
    }
}