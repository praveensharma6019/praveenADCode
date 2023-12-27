using Adani.SuperApp.Airport.Feature.Pranaam.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.LayoutService.Configuration;
using Sitecore.LayoutService.ItemRendering.ContentsResolvers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.LayoutService
{
    public class AddOnServiceContentResolver: RenderingContentsResolver
    {
        private readonly ILogRepository _logRepository;
        private readonly IAddOnServiceContent addOnService;
        public AddOnServiceContentResolver(IAddOnServiceContent addOnService, ILogRepository logRepository)
        {
            this.addOnService = addOnService;
            this._logRepository = logRepository;
        }

        public override object ResolveContents(Rendering rendering, IRenderingConfiguration renderingConfig)
        {
            var datasourceItem = RenderingContext.Current?.Rendering?.Item;
            try
            {
                if (datasourceItem == null)
                {
                    _logRepository.Info(string.Format("Method Name:{0} \n Error Message: datasource null", MethodBase.GetCurrentMethod().Name));
                    return datasourceItem;
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return this.addOnService.GetAddOnService(rendering);
        }
    }
}