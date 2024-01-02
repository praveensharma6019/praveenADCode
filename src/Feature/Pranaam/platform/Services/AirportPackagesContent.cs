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
    public class AirportPackagesContent : IAirportPackagesContent
    {
        private readonly ILogRepository _logRepository;       
        private readonly IHelper _helper;
        public AirportPackagesContent(ILogRepository logRepository, IHelper helper)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            
        }
        public Terminal GetDotNetPackages(Item item, string airportcode)
        {
            Terminal _termObj = new Terminal();
            try
            {
                Item cityitem = item.Children.Where(x => x.TemplateID == Templates.AirportId.CityTemplateId && x.Fields["CityCode"].Value == airportcode).FirstOrDefault();
                var terminalList = cityitem.Children.Where(x => x.TemplateID == Templates.AirportId.AirportTemplateId);
                
                if(terminalList != null)
                {
                    List<PranaamPackageDetail> _pckg = new List<PranaamPackageDetail>();
                    List<Terminals> _terminalsObj = new List<Terminals>();
                    foreach (Item pckgitem in terminalList)
                    {                        
                        var terminalsList = pckgitem.Children;
                        foreach(Item term in terminalsList)
                        {
                            Terminals _terminalObj = new Terminals();
                            _terminalObj.TerminalName = !string.IsNullOrEmpty(term.Fields[Templates.AirportTerminalDetails.Fields.TerminalName].Value) ? term.Fields[Templates.AirportTerminalDetails.Fields.TerminalName].Value : string.Empty;
                            MultilistField cityPackage = term?.Fields[Templates.AirportTerminalDetails.Fields.PranaamPackages];

                            PackageDetailsList dotNetPackageService = new PackageDetailsList();
                            dotNetPackageService.PackageDetail = new List<PranaamPackageDetail>();
                            if (cityPackage != null && cityPackage.GetItems().Length > 0)
                            {
                                foreach (var _obj in cityPackage.GetItems())
                                {
                                    PranaamPackageDetail _pckgDetails = new PranaamPackageDetail();
                                    _pckgDetails.AirportMasterId = airportcode;
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
                                    dotNetPackageService.PackageDetail.Add(_pckgDetails);
                                }
                                _terminalObj.PackageDetails = dotNetPackageService;
                            }
                            _terminalsObj.Add(_terminalObj);                           
                        }
                    }
                    _termObj.ListOfTerminals = _terminalsObj;
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }
            return _termObj;
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
                        addOn.AddOnMobileImage = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
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