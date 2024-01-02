using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class PackageServicesRepository : IPackageServices
    {
        private readonly ILogRepository _logRepository;
        public PackageServicesRepository(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public PackageServices GetPackageServiceData(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            _logRepository.Info("GetPackageServiceData started");
            PackageServices _service = new PackageServices();
            try
            {
                var renDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (renDatasource != null)
                {
                    _logRepository.Info("PackageServiceData datasource not null");
                    _service.title = !string.IsNullOrEmpty(renDatasource.Fields[Templates.PackageServices.Fields.Title].Value.ToString()) ? renDatasource.Fields[Templates.PackageServices.Fields.Title].Value.ToString() : "";
                    List<PackageServicesList> list = new List<PackageServicesList>();
                    if (((Sitecore.Data.Fields.MultilistField)renDatasource.Fields[Templates.PackageServices.Fields.ServicesList]).Count > 0)
                    {
                        _logRepository.Info("PackageServiceData list population");
                        foreach (Sitecore.Data.Items.Item item in ((Sitecore.Data.Fields.MultilistField)renDatasource.Fields[Templates.PackageServices.Fields.ServicesList]).GetItems())
                        {
                            PackageServicesList serviceItem = new PackageServicesList();
                            serviceItem.addOnServiceId = !string.IsNullOrEmpty(item.Fields[Templates.PackageServicesData.Fields.Value].Value.ToString()) ? item.Fields[Templates.PackageServicesData.Fields.Value].Value.ToString() : "";
                            serviceItem.addOnServiceName = !string.IsNullOrEmpty(item.Fields[Templates.PackageServicesData.Fields.OfferingsName].Value.ToString()) ? item.Fields[Templates.PackageServicesData.Fields.OfferingsName].Value.ToString() : "";
                            serviceItem.addOnServiceDescription = !string.IsNullOrEmpty(item.Fields[Templates.PackageServicesData.Fields.OfferingsDescription].Value.ToString()) ? item.Fields[Templates.PackageServicesData.Fields.OfferingsDescription].Value.ToString() : "";
                            list.Add(serviceItem);
                        }
                        _service.serviceDetails = list;
                        _logRepository.Info("GetPackageServiceData ended");
                    }
                }
                else return null;
            }
            catch(Exception ex)
            {
                _logRepository.Error("GetPackageServiceData throws Exception -> " + ex.Message);
            }
           
            return _service;
        }
    }
}