using Glass.Mapper.Sc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models;
using Project.AAHL.Website.Models.Home;
using Project.AAHL.Website.Models.OurStory;
using Project.AAHL.Website.Templates;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using static Project.AAHL.Website.Templates.BaseTemplate;
using static Project.AAHL.Website.Templates.BaseTemplate.TitleTemplate;

namespace Project.AAHL.Website.Services.Common
{
    public class OurStory : IOurStory
    {

        private readonly ISitecoreService _sitecoreService;

        public OurStory(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }


        public Highlights GetHighlights(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<Highlights>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }  
        public OurTimelineCarousel GetOurTimelineCarousel(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<OurTimelineCarousel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        
        public FutureGrowthThroughExcellenace GetFutureGrowthThroughExcellenace(Rendering rendering)
        {

            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<FutureGrowthThroughExcellenace>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

       

    }
}