using Glass.Mapper.Sc;
using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models.OurLeadership;
using Sitecore.Mvc.Presentation;
using System;

namespace Project.AAHL.Website.Services.OurLeadership
{
    public class OurLeadersServices : IOurLeadersServices
    {
        //readonly Item _contextItem;
        private readonly ISitecoreService _sitecoreService;
        public OurLeadersServices(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }

        public OurLeadersModel GetOurLeaders(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ourLeadersdata = _sitecoreService.GetItem<OurLeadersModel>(datasource);
                return ourLeadersdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

    }
}