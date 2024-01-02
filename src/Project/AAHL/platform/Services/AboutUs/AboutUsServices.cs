using Glass.Mapper.Sc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.AboutUs;
using Project.AAHL.Website.Models.Common;
using Project.AAHL.Website.Templates;
using Sitecore.Mvc.Presentation;
using System;
using Item = Sitecore.Data.Items.Item;

namespace Project.AAHL.Website.Services.AboutUs
{
    public class AboutUsServices : IAboutUsServices
    {
        //readonly Item _contextItem;
        private readonly ISitecoreService _sitecoreService;
        public AboutUsServices(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public BannerModel GetBanner(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var bannerdata = _sitecoreService.GetItem<BannerModel>(datasource);
                return bannerdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public DetailModel GetDetails(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var detaildata = _sitecoreService.GetItem<DetailModel>(datasource);
                return detaildata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public AboutAAHLModel GetaboutAAHL(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var aboutAAHLdata = _sitecoreService.GetItem<AboutAAHLModel>(datasource);
                return aboutAAHLdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public StoryModel GetStory(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var storydata = _sitecoreService.GetItem<StoryModel>(datasource);
                return storydata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public OurLeadershipModel GetOurLeadership(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ourLeadershipdata = _sitecoreService.GetItem<OurLeadershipModel>(datasource);
                return ourLeadershipdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
            
        }
        public OurStrategyModel GetStrategy(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ourStrategydata = _sitecoreService.GetItem<OurStrategyModel>(datasource);
                return ourStrategydata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            

        }
    }
}