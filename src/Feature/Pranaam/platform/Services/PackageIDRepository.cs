using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class PackageIDRepository : IPackageIDRepository
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;
        public PackageIDRepository(ILogRepository logRepository, IHelper helper)
        {

            this._logRepository = logRepository;
            this._helper = helper;

        }
        public PackageDetailsList GetPackagesDetail(Sitecore.Data.Items.Item datasourceItem, string packageIds)
        {
            PackageDetailsList _pckgIdObj = new PackageDetailsList();
            List<PranaamPackageDetail> packageList = new List<PranaamPackageDetail>();
            string[] ids = new string[] {};
            try
            {
                if (!string.IsNullOrEmpty(packageIds))
                {
                    ids = packageIds.Split(',');
                }
                
                if(ids.Count() > 0)
                {
                    foreach(string packageid in ids)
                    {                        
                        IEnumerable<Item> IdList = datasourceItem.Children.Where(x=>x.Fields[Templates.PranaamPackages.Fields.Id].Value == packageid);
                        
                        if (IdList != null)
                        {                            
                            foreach (var _obj in IdList)
                            {
                                 PranaamPackageDetail _pckgDetails = new PranaamPackageDetail();
                                 _pckgDetails.Name = _obj?.Fields[Templates.PranaamPackages.Fields.Title]?.Value;
                                 _pckgDetails.ShortDesc = _obj?.Fields[Templates.PranaamPackages.Fields.Description]?.Value;
                                 _pckgDetails.PackageId = _obj?.Fields[Templates.PranaamPackages.Fields.Id]?.Value;
                                if (_pckgDetails.Name.ToLower().Contains("elite"))
                                {
                                    _pckgDetails.PackageImage = !string.IsNullOrEmpty(_helper.GetImageURL(_obj, Templates.PranaamPackages.Fields.StanderedImage.ToString())) ? _helper.GetImageURL(_obj, Templates.PranaamPackages.Fields.StanderedImage.ToString()) : Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/-/media/Project/Adani/Pranaam/Packages/Elite_Web.png";
                                }
                                else if (_pckgDetails.Name.ToLower().Contains("platinum"))
                                {
                                    _pckgDetails.PackageImage = !string.IsNullOrEmpty(_helper.GetImageURL(_obj, Templates.PranaamPackages.Fields.StanderedImage.ToString())) ? _helper.GetImageURL(_obj, Templates.PranaamPackages.Fields.StanderedImage.ToString()) : Sitecore.Configuration.Settings.GetSetting("domainUrl").ToString() + "/-/media/Project/Adani/Pranaam/Packages/Platinum_web.png";
                                }
                                else
                                {
                                    _pckgDetails.PackageImage = _helper.GetImageURL(_obj, Templates.PranaamPackages.Fields.StanderedImage.ToString());
                                }                                
                                 _pckgDetails.PackageAddOns = GetPackageAddOnServices(_obj?.Fields[Templates.PranaamPackages.Fields.AddOns]);
                                 _pckgDetails.PackageServices = GetPackageServices(_obj?.Fields[Templates.PranaamPackages.Fields.ServicesList]);
                                 packageList.Add(_pckgDetails);
                            }                                                                        
                        }
                    }                   
                }
                _pckgIdObj.PackageDetail = packageList;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return _pckgIdObj;
        }

        private PackageAddOns GetPackageAddOnServices(Field field)
        {
            PackageAddOns packageAddOn = new PackageAddOns();
            List<PranaamPackageAddOn> addOnServices = new List<PranaamPackageAddOn>();
            try
            {
                MultilistField addonField = field;
                if (addonField != null && addonField.GetItems().Length > 0)
                {
                    foreach (var item in addonField.GetItems())
                    {
                        PranaamPackageAddOn addOn = new PranaamPackageAddOn();
                        addOn.AddOnServiceId = item?.Fields[Templates.AddOnService.Fields.Id]?.Value;
                        addOn.AddOnServiceName = item?.Fields[Templates.AddOnService.Fields.Title]?.Value;
                        addOn.AddOnServiceDescription = item?.Fields[Templates.AddOnService.Fields.Description]?.Value;
                        addOn.AddOnImage = _helper.GetImageURL(item, Templates.AddOnService.Fields.ServiceImage.ToString());
                        addOnServices.Add(addOn);
                    }
                }
                packageAddOn.PackageAddOn = addOnServices;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return packageAddOn;
        }

        private AirportPackageServices GetPackageServices(Field field)
        {
            AirportPackageServices _pckgservice = new AirportPackageServices();
            List<AirportPackageService> services = new List<AirportPackageService>();
            try
            {
                MultilistField addonField = field;
                if (addonField != null && addonField.GetItems().Length > 0)
                {
                    foreach (var item in addonField.GetItems())
                    {
                        AirportPackageService _service = new AirportPackageService();
                        _service.ServiceId = item?.Fields[Templates.PackageServicesData.Fields.Value]?.Value;
                        _service.ServiceName = item?.Fields[Templates.PackageServicesData.Fields.OfferingsName]?.Value;
                        _service.ServiceDescription = item?.Fields[Templates.PackageServicesData.Fields.OfferingsDescription]?.Value;
                        services.Add(_service);
                    }
                }
                _pckgservice.PackageService = services;
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return _pckgservice;
        }
    }
}