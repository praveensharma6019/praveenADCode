using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class PackagesRepository : IPackages
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;
        private readonly IHelper _helper;
        public PackagesRepository(ILogRepository logRepository, IHelper helper, IWidgetService widgetService)
        {

            this._logRepository = logRepository;
            this._helper = helper;
            this._widgetservice = widgetService;
        }

        public Packageswidgets GetPackagesData(Rendering rendering)
        {
            Packageswidgets packagesWidgits = new Packageswidgets();
            try
            {
                Sitecore.Data.Items.Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;

                if (widget != null)
                {
                   
                    packagesWidgits.widget = _widgetservice.GetWidgetItem(widget);
                }
                else
                {
                    packagesWidgits.widget = new Foundation.Theming.Platform.Models.WidgetItem();
                }
                packagesWidgits.widget.widgetItems = GetPackageData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error("GetPackagesData throws Exception -> " + ex.Message);
            }
            return packagesWidgits;
        }


        private List<Object> GetPackageData(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            _logRepository.Info("GetPackageData started");
            List<Object> packageList = new List<Object>();
            try
            {
                PranaamPackage _pckg = new PranaamPackage();
                Packages _obj = new Packages();
                var renDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (renDatasource != null)
                {
                    _logRepository.Info("GetPackageData datsource not null");
                    _obj.title = !string.IsNullOrEmpty(renDatasource.Fields[Templates.PranaamPackageDatasource.Fields.Title].Value.ToString()) ? renDatasource.Fields[Templates.PranaamPackageDatasource.Fields.Title].Value.ToString() : "";

                    PackageCard _card = new PackageCard();
                    List<PackageItems> PackageList = new List<PackageItems>();

                    _logRepository.Info("GetPackageData packages data population");
                    foreach (Sitecore.Data.Items.Item item in ((Sitecore.Data.Fields.MultilistField)renDatasource.Fields[Templates.PranaamPackageDatasource.Fields.SelectPackages]).GetItems())
                    {
                        PackageItems cardItem = new PackageItems();
                        cardItem.PackageId = !string.IsNullOrEmpty(item.Fields[Templates.PranaamPackages.Fields.Id].Value.ToString()) ? item.Fields[Templates.PranaamPackages.Fields.Id].Value.ToString() : "";
                        cardItem.PackageTitle = !string.IsNullOrEmpty(item.Fields[Templates.PranaamPackages.Fields.Title].Value.ToString()) ? item.Fields[Templates.PranaamPackages.Fields.Title].Value.ToString() : "";
                        cardItem.IsRecommended = _helper.GetCheckboxOption(item.Fields[Templates.PranaamPackages.Fields.IsRecommended]);
                        cardItem.alt = _helper.GetImageAlt(item, Templates.PranaamPackages.Fields.StanderedImage.ToString());
                        cardItem.src = _helper.GetImageURL(item, Templates.PranaamPackages.Fields.StanderedImage.ToString());
                        cardItem.cardDesc = !string.IsNullOrEmpty(item.Fields[Templates.PranaamPackages.Fields.Description].Value.ToString()) ? item.Fields[Templates.PranaamPackages.Fields.Description].Value.ToString() : "";
                        cardItem.finalPrice = !string.IsNullOrEmpty(item.Fields[Templates.PranaamPackages.Fields.NewPrice].Value.ToString()) ? item.Fields[Templates.PranaamPackages.Fields.NewPrice].Value.ToString() : "";
                        cardItem.btnText = _helper.GetLinkText(item, Templates.PranaamPackages.Fields.CTA.ToString());
                        cardItem.btnUrl = _helper.GetLinkURL(item, Templates.PranaamPackages.Fields.CTA.ToString());
                        cardItem.btnVariant = _helper.GetDropLinkValue(item.Fields[Templates.PranaamPackages.Fields.ButtonVariant]);
                        cardItem.mobileImage = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                        cardItem.mobileImageAlt = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.MobileImage.ToString());
                        cardItem.webImage = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                        cardItem.webImageAlt = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.DesktopImage.ToString());
                        cardItem.thumbnailImage = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                        cardItem.thumbnailImageAlt = _helper.GetImageURL(item, Templates.DeviceSpecificImage.Fields.ThumbnailImage.ToString());
                        PackageList.Add(cardItem);
                    }
                    _card.items = PackageList;
                    _obj.cards = _card;
                    _pckg.pranaamPackage = _obj;
                    packageList.Add(_pckg);
                    _logRepository.Info("GetPackageData Ended");
                }
                else return null;
            }
            catch(Exception ex)
            {
                _logRepository.Error("GetPackageData throws Exception -> " + ex.Message);
            }
            
            return packageList;
        }

        public AppPackage GetPackageData(Sitecore.Mvc.Presentation.Rendering rendering, string isapp)
        {
            _logRepository.Info("GetPackageData(App) Ended");
            AppPackage _obj = new AppPackage();
            
            try
            {
                List<Sitecore.Data.Items.Item> service_Obj = new List<Sitecore.Data.Items.Item>();
                var renDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (renDatasource != null)
                {
                    _logRepository.Info("GetPackageData(App) datasource not null");
                    Sitecore.Data.Items.Item servicesList_Obj = Sitecore.Context.Database.GetItem(Templates.PackageServiceItem.PackageServiceItemID);
                    if (servicesList_Obj.Children.Count > 0)
                    {
                        _logRepository.Info("GetPackageData(App) services data population");
                        foreach (Sitecore.Data.Items.Item services in servicesList_Obj.Children)
                        {
                            service_Obj.Add(services);
                        }
                    }
                    if (((Sitecore.Data.Fields.MultilistField)renDatasource.Fields[Templates.PranaamPackageDatasource.Fields.SelectPackages]).Count > 0)
                    {
                        _logRepository.Info("GetPackageData(App) packages data population");
                        List<AppPackageDetails> appList = new List<AppPackageDetails>();
                        foreach (Sitecore.Data.Items.Item item in ((Sitecore.Data.Fields.MultilistField)renDatasource.Fields[Templates.PranaamPackageDatasource.Fields.SelectPackages]).GetItems())
                        {
                            AppPackageDetails appPackage = new AppPackageDetails();
                            appPackage.PackageName = !string.IsNullOrEmpty(item.Fields[Templates.PranaamPackages.Fields.Title].Value.ToString()) ? item.Fields[Templates.PranaamPackages.Fields.Title].Value.ToString() : "";
                            appPackage.packageId = !string.IsNullOrEmpty(item.Fields[Templates.PranaamPackages.Fields.Id].Value.ToString()) ? item.Fields[Templates.PranaamPackages.Fields.Id].Value.ToString() : "";
                            appPackage.FinalPrice = !string.IsNullOrEmpty(item.Fields[Templates.PranaamPackages.Fields.NewPrice].Value.ToString()) ? item.Fields[Templates.PranaamPackages.Fields.NewPrice].Value.ToString() : "";
                            appPackage.IsRecommended = _helper.GetCheckboxOption(item.Fields[Templates.PranaamPackages.Fields.IsRecommended]);
                            if (((Sitecore.Data.Fields.MultilistField)item.Fields[Templates.PranaamPackages.Fields.ServicesList]).Count > 0)
                            {
                                Sitecore.Data.Items.Item[] listOfServices = ((Sitecore.Data.Fields.MultilistField)item.Fields[Templates.PranaamPackages.Fields.ServicesList]).GetItems();
                                List<NameValue> list = new List<NameValue>();
                                List<Sitecore.Data.Items.Item> getServiceList = new List<Sitecore.Data.Items.Item>();
                                getServiceList = ((Sitecore.Data.Fields.MultilistField)item.Fields[Templates.PranaamPackages.Fields.ServicesList]).GetItems().ToList();
                                foreach (Sitecore.Data.Items.Item serviceItem in service_Obj)
                                {
                                    if (getServiceList.Any(x => x.ID.ToString() == serviceItem.ID.ToString()))
                                    {
                                        NameValue nm = new NameValue();
                                        nm.Name = !string.IsNullOrEmpty(serviceItem.Fields[Templates.PackageServicesData.Fields.OfferingsName].Value.ToString()) ? serviceItem.Fields[Templates.PackageServicesData.Fields.OfferingsName]?.Value.ToString() : "";
                                        nm.Value = true;
                                        list?.Add(nm);
                                    }
                                    else
                                    {
                                        NameValue nm = new NameValue();
                                        nm.Name = !string.IsNullOrEmpty(serviceItem.Fields[Templates.PackageServicesData.Fields.OfferingsName].Value.ToString()) ? serviceItem.Fields[Templates.PackageServicesData.Fields.OfferingsName].Value.ToString() : "";
                                        nm.Value = false;
                                        list?.Add(nm);
                                    }
                                }
                                appPackage.ServicesList = list;
                            }
                            appList.Add(appPackage);
                        }
                        _obj.data = appList;
                        _logRepository.Info("GetPackageData(App) Ended");
                    }
                }
                else return null;
            }
            catch(Exception ex)
            {
                _logRepository.Error("GetPackageData throws Exception -> " + ex.Message);
            }
            
            return _obj;
        }
    }
}