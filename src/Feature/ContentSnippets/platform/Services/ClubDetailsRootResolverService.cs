using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public class ClubDetailsRootResolverService : IClubDetailsRootResolverService
    {

        private readonly ILogRepository _logRepository;
        public ClubDetailsRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        #region ClubHeroBanner
        public ClubHeroBannerData GetClubHeroBannerDataList(Rendering rendering)
        {
            ClubHeroBannerData clubHeroBannerDataList = new ClubHeroBannerData();
            try
            {
                clubHeroBannerDataList.herobanner = GetClubHeroBannerDataItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return clubHeroBannerDataList;
        }

        public List<Object> GetClubHeroBannerDataItem(Rendering rendering)
        {
            List<Object> getclubHeroBannerDataList = new List<Object>();
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
                ClubHeroBannerDataItem clubHeroBannerItem;
                if (datasource.TemplateID == Templates.ClubHeroBannerDataFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {
                    List<Item> children = datasource.Children.Where(x => x.TemplateID == Templates.ClubHeroBanner.TemplateID).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Sitecore.Data.Items.Item item in children)
                        {
                            clubHeroBannerItem = new ClubHeroBannerDataItem();

                            clubHeroBannerItem.BannerImgSrc = Helper.GetImageSource(item, Templates.Image.FieldsName.Image) != null ?
                                       Helper.GetImageSource(item, Templates.Image.FieldsName.Image) : "";

                            clubHeroBannerItem.BannerImgAlt = Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                      Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["Alt"].Value : "";

                            clubHeroBannerItem.BannerImgTitle = Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                      Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["TItle"].Value : "";


                            clubHeroBannerItem.ThumbImgSrc = Helper.GetImageSource(item, Templates.IThumbnail.FieldsName.Thumbnail) != null ?
                                       Helper.GetImageSource(item, Templates.IThumbnail.FieldsName.Thumbnail) : "";

                            clubHeroBannerItem.ThumbImgAlt = Helper.GetImageDetails(item, Templates.IThumbnail.FieldsName.Thumbnail) != null ?
                                      Helper.GetImageDetails(item, Templates.IThumbnail.FieldsName.Thumbnail).Fields["Alt"].Value : "";

                            clubHeroBannerItem.Heading = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                            clubHeroBannerItem.City = item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value != null ? item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value : "";

                            clubHeroBannerItem.Location = item.Fields[Templates.ISummary.FieldsName.Summary].Value != null ? item.Fields[Templates.ISummary.FieldsName.Summary].Value : "";

                            clubHeroBannerItem.Link = Helper.GetLinkURL(item, Templates.ILink.FieldsName.Link) != null ?
                                      Helper.GetLinkURL(item, Templates.ILink.FieldsName.Link) : "";

                            clubHeroBannerItem.target = Helper.GetLinkURLTargetSpace(item, Templates.ILink.FieldsName.Link) != null ?
                                      Helper.GetLinkURLTargetSpace(item, Templates.ILink.FieldsName.Link) : "";

                            clubHeroBannerItem.LinkText = Helper.GetLinkTextbyField(item, item.Fields[Templates.ILink.FieldsName.Link]) != null ?
                                      Helper.GetLinkTextbyField(item, item.Fields[Templates.ILink.FieldsName.Link]) : "";
                            clubHeroBannerItem.isVideo = item.Fields[Templates.HeroBanner.Fields.isVideoFieldName].Value != null && item.Fields[Templates.HeroBanner.Fields.isVideoFieldName].Value.ToString() == "1" ? "true" :
                                        item.Fields[Templates.HeroBanner.Fields.isVideoFieldName].Value != null && item.Fields[Templates.HeroBanner.Fields.isVideoFieldName].Value.ToString() == "0" ? "false" : "false";
                            clubHeroBannerItem.videoposter = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Templates.HeroBanner.Fields.videoposterFieldName) != null ?
                                        Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Templates.HeroBanner.Fields.videoposterFieldName) : "";
                            clubHeroBannerItem.videoMp4 = !string.IsNullOrEmpty(item.Fields[Templates.HeroBanner.Fields.VideoMp4FieldName].Value) ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetMedialLinkURL(item, item.Fields[Templates.HeroBanner.Fields.VideoMp4FieldName]) : "";
                            clubHeroBannerItem.videoOgg = !string.IsNullOrEmpty(item.Fields[Templates.HeroBanner.Fields.videoOggFieldName].Value) ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetMedialLinkURL(item, item.Fields[Templates.HeroBanner.Fields.videoOggFieldName]) : "";
                            clubHeroBannerItem.videoposterMobile = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Templates.HeroBanner.Fields.videoposterMobileFieldName) != null ?
                                        Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Templates.HeroBanner.Fields.videoposterMobileFieldName) : "";
                            clubHeroBannerItem.videoMp4Mobile = !string.IsNullOrEmpty(item.Fields[Templates.HeroBanner.Fields.videoMp4MobileFieldName].Value) ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetMedialLinkURL(item, item.Fields[Templates.HeroBanner.Fields.videoMp4MobileFieldName]) : "";
                            clubHeroBannerItem.videoOggMobile = !string.IsNullOrEmpty(item.Fields[Templates.HeroBanner.Fields.videoOggMobileFieldName].Value) ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetMedialLinkURL(item, item.Fields[Templates.HeroBanner.Fields.videoOggMobileFieldName]) : "";
                            clubHeroBannerItem.SEOName = item.Fields[Templates.HeroBanner.Fields.SEOName].Value != null ? item.Fields[Templates.HeroBanner.Fields.SEOName].Value : "";
                            clubHeroBannerItem.SEODescription = item.Fields[Templates.HeroBanner.Fields.SEODescription].Value != null ? item.Fields[Templates.HeroBanner.Fields.SEODescription].Value : "";
                            clubHeroBannerItem.UploadDate = item.Fields[Templates.HeroBanner.Fields.UploadDate].Value != null ? item.Fields[Templates.HeroBanner.Fields.UploadDate].Value : "";

                            getclubHeroBannerDataList.Add(clubHeroBannerItem);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(" AboutUsDataService AboutUsDataList gives -> " + ex.Message);
            }

            return getclubHeroBannerDataList;
        }

        #endregion

        #region AboutAdaniSocialClub

        public AboutAdaniSocialClubData GetAboutAdaniSocialClubDataList(Rendering rendering)
        {
            AboutAdaniSocialClubData aboutAdaniSocialClubDataList = new AboutAdaniSocialClubData();
            try
            {
                aboutAdaniSocialClubDataList.AboutAdaniSocialClub = GetAboutAdaniSocialClubDataItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return aboutAdaniSocialClubDataList;
        }

        public AboutAdaniSocialClubDataItem GetAboutAdaniSocialClubDataItem(Rendering rendering)
        {
            AboutAdaniSocialClubDataItem aboutAdaniSocialClubDataItem = new AboutAdaniSocialClubDataItem();

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

                if (datasource.TemplateID == Templates.AboutAdaniSocialClub.TemplateID)
                {
                    Item item = datasource;
                    if (item != null)
                    {
                        aboutAdaniSocialClubDataItem.Heading = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                        aboutAdaniSocialClubDataItem.About = item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value != null ? item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value : "";

                        aboutAdaniSocialClubDataItem.ReadMore = item.Fields[Templates.ISummary.FieldsName.Summary].Value != null ? item.Fields[Templates.ISummary.FieldsName.Summary].Value : "";
                    }
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return aboutAdaniSocialClubDataItem;
        }

        #endregion

        #region ClubHighLights

        public ClubHighLightsData GetClubHighLightsDataList(Rendering rendering)
        {
            ClubHighLightsData clubHighLightsData = new ClubHighLightsData();
            try
            {
                clubHighLightsData.title = GetClubHighLightsDataItem(rendering).Item1;
                clubHighLightsData.data = GetClubHighLightsDataItem(rendering).Item2;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return clubHighLightsData;
        }

        public Tuple<string, List<Object>> GetClubHighLightsDataItem(Rendering rendering)
        {
            List<Object> clubHighLightsDataItemList = new List<Object>();
            string Title = string.Empty;
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
                ClubHighlightsDataItem clubHighlightsDataItem;

                if (datasource.TemplateID == Templates.ClubHighLightsFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {
                    Title = datasource.Fields[Templates.ITitle.FieldsName.Title].Value != null ? datasource.Fields[Templates.ITitle.FieldsName.Title].Value : "";
                    List<Item> children = datasource.Children.Where(x => x.TemplateID == Templates.ClubHighLights.TemplateID).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Item item in children)
                        {
                            clubHighlightsDataItem = new ClubHighlightsDataItem();
                            clubHighlightsDataItem.ImgSrc = Helper.GetImageSource(item, Templates.Image.FieldsName.Image) != null ?
                                        Helper.GetImageSource(item, Templates.Image.FieldsName.Image) : "";

                            clubHighlightsDataItem.ImgAlt = Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                      Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["Alt"].Value : "";

                            clubHighlightsDataItem.ImgTitle = Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                      Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["TItle"].Value : "";

                            clubHighlightsDataItem.Heading = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                            clubHighlightsDataItem.Description = item.Fields[Templates.IDescription.FieldsName.Description].Value != null ? item.Fields[Templates.IDescription.FieldsName.Description].Value : "";

                            clubHighlightsDataItem.Discount = item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value != null ? item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value : "";

                            clubHighLightsDataItemList.Add(clubHighlightsDataItem);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return Tuple.Create(Title, clubHighLightsDataItemList);
        }

        #endregion

        #region AboutClub

        public AboutClubData GetAboutClubDataList(Rendering rendering)
        {
            AboutClubData aboutClubDataList = new AboutClubData();
            try
            {
                aboutClubDataList.data = GetAboutClubDataItem(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return aboutClubDataList;
        }

        public AboutClubDataItem GetAboutClubDataItem(Rendering rendering)
        {
            AboutClubDataItem aboutClubDataItem = new AboutClubDataItem();
            var commonItem = Sitecore.Context.Database.GetItem(Templates.commondataitem);
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
                if (datasource.TemplateID == Templates.AboutClub.TemplateID)
                {
                    Item item = datasource;
                    if (item != null)
                    {
                        aboutClubDataItem.Heading = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";
                        aboutClubDataItem.clubLifeImageSrc = Helper.GetImageSource(item, Templates.Image.FieldsName.Image) != null ? Helper.GetImageSource(item, Templates.Image.FieldsName.Image) : "";
                        aboutClubDataItem.Description = item.Fields[Templates.IDescription.FieldsName.Description].Value != null ? item.Fields[Templates.IDescription.FieldsName.Description].Value : "";
                        aboutClubDataItem.readmore = commonItem.Fields[Templates.readmoreID].Value != null ? commonItem.Fields[Templates.readmoreID].Value : "";
                        aboutClubDataItem.readless = commonItem.Fields[Templates.readlessID].Value != null ? commonItem.Fields[Templates.readlessID].Value : "";

                    }
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return aboutClubDataItem;
        }

        #endregion
    }
}
