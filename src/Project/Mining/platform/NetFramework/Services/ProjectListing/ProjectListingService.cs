using Glass.Mapper.Sc;
using Project.Mining.Website.Helpers;
using Project.Mining.Website.Models;
using Project.Mining.Website.Models.ProjectListing;
using Sitecore.Mvc.Presentation;
using System;


namespace Project.Mining.Website.Services.ProjectListing
{
    public class ProjectListingService : IProjectListingService
    {
        private readonly ISitecoreService _sitecoreService;
        public ProjectListingService(ISitecoreService sitecoreService)
        {
            _sitecoreService = sitecoreService;
        }
        public ProjectListingModel GetProjectListing(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<ProjectListingModel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public RequestAcallModel GetRequestAcall(Rendering rendering)
        {
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                var ModelData = _sitecoreService.GetItem<RequestAcallModel>(datasource);
                return ModelData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}