using Glass.Mapper.Sc;
using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination.Banner;
using Project.AdaniOneSEO.Website.Models.VideoGallery;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.AdaniOneSEO.Website.Services.VideoGallery
{
    public class VideoGalleryService : IVideoGalleryService
    {
        //private readonly ISitecoreService _sitecoreService;
        //public VideoGalleryService(ISitecoreService sitecoreService)
        //{
        //    _sitecoreService = sitecoreService;
        //}
        //public VideoGalleryModel GetVideoGalleryModel(Rendering rendering)
        //{
        //    var datasource = Utils.GetRenderingDatasource(rendering);
        //    if (datasource == null) return null;

        //    var videoGallery = _sitecoreService.GetItem<VideoGalleryModel>(datasource);
        //    return videoGallery;
        //}

        public VideoGalleryModelNew GetVideoGalleryModel(Rendering rendering)
        {
            VideoGalleryModelNew VideoGalleryDataModel = null;

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                VideoGalleryDataModel = new VideoGalleryModelNew();
                VideoGalleryDataModel.GalleryTitle = Utils.GetValue(datasource, BaseTemplates.VideoGalleryTemplate.GalleryTitle);
                VideoGalleryDataModel.GalleryBanner = Utils.GetImageURLByFieldId(datasource, BaseTemplates.VideoGalleryTemplate.GalleryBanner);
                VideoGalleryDataModel.GalleryMobileBanner = Utils.GetImageURLByFieldId(datasource, BaseTemplates.VideoGalleryTemplate.GalleryMobileBanner);
                VideoGalleryDataModel.GalleryTabletBanner = Utils.GetImageURLByFieldId(datasource, BaseTemplates.VideoGalleryTemplate.GalleryTabletBanner);
               
                if(datasource.HasChildren)
                {
                    List<VideoDetailsNew> videolist = new List<VideoDetailsNew>();
                    foreach (Item child in datasource.Children)
                    {
                        VideoDetailsNew ItemsListData = new VideoDetailsNew();
                        ItemsListData.VideoTitle = Utils.GetValue(child, BaseTemplates.VideoGalleryTemplate.VideoTitle);
                        ItemsListData.VideoDescription = Utils.GetValue(child, BaseTemplates.VideoGalleryTemplate.VideoDescription);
                        ItemsListData.VideoCategory = Utils.GetValue(child, BaseTemplates.VideoGalleryTemplate.VideoCategory);
                        ItemsListData.VideoSubCategory = Utils.GetValue(child, BaseTemplates.VideoGalleryTemplate.VideoSubCategory);
                        ItemsListData.VideoThumbnail = Utils.GetImageURLByFieldId(child, BaseTemplates.VideoGalleryTemplate.VideoThumbnail);
                        ItemsListData.VideoUrl = Utils.GetLinkURL(child, BaseTemplates.VideoGalleryTemplate.VideoUrl.ToString());
                        ItemsListData.YoutubeVideoLink = Utils.GetLinkURL(child, BaseTemplates.VideoGalleryTemplate.YoutubeVideoLink.ToString());
                        videolist.Add(ItemsListData);
                    }
                    VideoGalleryDataModel.Videos = videolist;
                }
                
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return VideoGalleryDataModel;
        }

        //public VideoDetailsPageModel GetVideoDetailsPageModel(Rendering rendering)
        //{
        //    var datasource = Utils.GetRenderingDatasource(rendering);
        //    if (datasource == null) return null;

        //    var videoDetailsPage = _sitecoreService.GetItem<VideoDetailsPageModel>(datasource);
        //    return videoDetailsPage;
        //}

        public VideoDetailsPageModelNew GetVideoDetailsPageModel(Rendering rendering)
        {
            VideoDetailsPageModelNew VideoDetailDataModel = null;

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                VideoDetailDataModel = new VideoDetailsPageModelNew();
                VideoDetailDataModel.VideoTitle = Utils.GetValue(datasource, BaseTemplates.VideoGalleryTemplate.VideoTitle);
                VideoDetailDataModel.VideoDescription = Utils.GetValue(datasource, BaseTemplates.VideoGalleryTemplate.VideoDescription);
                VideoDetailDataModel.VideoCategory = Utils.GetValue(datasource, BaseTemplates.VideoGalleryTemplate.VideoCategory);
                VideoDetailDataModel.VideoSubCategory = Utils.GetValue(datasource, BaseTemplates.VideoGalleryTemplate.VideoSubCategory);
                VideoDetailDataModel.VideoThumbnail = Utils.GetImageURLByFieldId(datasource, BaseTemplates.VideoGalleryTemplate.VideoThumbnail);
                VideoDetailDataModel.VideoUrl = Utils.GetLinkURL(datasource, BaseTemplates.VideoGalleryTemplate.VideoUrl.ToString());
                VideoDetailDataModel.YoutubeVideoLink = Utils.GetLinkURL(datasource, BaseTemplates.VideoGalleryTemplate.YoutubeVideoLink.ToString());

                var similarvideoMultiListField = Utils.GetMultiListValueItem(datasource, BaseTemplates.VideoGalleryTemplate.SimilarVideosMultilistID);

                if(similarvideoMultiListField != null)
                {
                    List<VideoDetailsNew> videolist = new List<VideoDetailsNew>();

                    foreach (Item child in similarvideoMultiListField.ToList())
                    {
                        VideoDetailsNew ItemsListData = new VideoDetailsNew();
                        ItemsListData.VideoTitle = Utils.GetValue(child, BaseTemplates.VideoGalleryTemplate.VideoTitle);
                        ItemsListData.VideoDescription = Utils.GetValue(child, BaseTemplates.VideoGalleryTemplate.VideoDescription);
                        ItemsListData.VideoCategory = Utils.GetValue(child, BaseTemplates.VideoGalleryTemplate.VideoCategory);
                        ItemsListData.VideoSubCategory = Utils.GetValue(child, BaseTemplates.VideoGalleryTemplate.VideoSubCategory);
                        ItemsListData.VideoThumbnail = Utils.GetImageURLByFieldId(child, BaseTemplates.VideoGalleryTemplate.VideoThumbnail);
                        ItemsListData.VideoUrl = Utils.GetLinkURL(child, BaseTemplates.VideoGalleryTemplate.VideoUrl.ToString());
                        ItemsListData.YoutubeVideoLink = Utils.GetLinkURL(child, BaseTemplates.VideoGalleryTemplate.YoutubeVideoLink.ToString());
                        videolist.Add(ItemsListData);
                    }
                    VideoDetailDataModel.SimilarVideos = videolist;
                }
                      
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return VideoDetailDataModel;
        }
    }
}