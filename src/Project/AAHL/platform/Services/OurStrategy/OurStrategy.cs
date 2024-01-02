using Glass.Mapper.Sc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.OurBelief;
using Project.AAHL.Website.Models.OurStrategy;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Services.OurStrategy
{
    public class OurStrategy: IOurStrategy
    {
        private readonly ISitecoreService _sitecoreService;

        public OurStrategy(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public CardDetailWithImagesModel GetCardDetailWithImages(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<CardDetailWithImagesModel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       
    }
}