using Adani.SuperApp.Airport.Feature.Pranaam.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.Pranaam.Services
{
    public class AddOnServiceTabRepository : IAddOnServiceTab
    {
        private readonly ILogRepository _logRepository;       
        private readonly IHelper _helper;
        public AddOnServiceTabRepository(ILogRepository logRepository,  IHelper helper)
        {           
            this._logRepository = logRepository;
            this._helper = helper;
        }
        public AddOnServiceTab GetServiceTab(Sitecore.Mvc.Presentation.Rendering rendering)
        {
            _logRepository.Info("GetServiceTab Started");
            AddOnServiceTab _obj = new AddOnServiceTab();
            try
            {
                var feedbackDatasource = RenderingContext.Current.Rendering.Item != null ? RenderingContext.Current.Rendering.Item : null;
                if (feedbackDatasource != null)
                {
                    _logRepository.Info("ServiceTab Datasource is not null");
                    _obj.title = !string.IsNullOrEmpty(feedbackDatasource.Fields[Templates.AddOnServiceTab.Fields.Title].Value.ToString()) ? feedbackDatasource.Fields[Templates.AddOnServiceTab.Fields.Title].Value.ToString() : "";
                    if (((Sitecore.Data.Fields.ImageField)feedbackDatasource.Fields[Templates.AddOnServiceTab.Fields.Banner]) != null)
                    {
                        _logRepository.Info("ServiceTab Banner Data population");
                        HeroContent hero = new HeroContent();
                        hero.src = _helper.GetImageURL(feedbackDatasource, Templates.AddOnServiceTab.Fields.Banner.ToString());
                        hero.alt = _helper.GetImageAlt(feedbackDatasource, Templates.AddOnServiceTab.Fields.Banner.ToString());
                        hero.text = !string.IsNullOrEmpty(feedbackDatasource.Fields[Templates.AddOnServiceTab.Fields.Text].Value.ToString()) ? feedbackDatasource.Fields[Templates.AddOnServiceTab.Fields.Text].Value.ToString() : "";
                        _obj.heroContent = hero;
                    }
                    List<Dictionary<string, TabData>> tabContentList = new List<Dictionary<string, TabData>>();

                    var listOfCategory = ((Sitecore.Data.Fields.MultilistField)feedbackDatasource.Fields[Templates.AddOnServiceTab.Fields.Tabs]).GetItems();
                    if (((Sitecore.Data.Fields.MultilistField)feedbackDatasource.Fields[Templates.AddOnServiceTab.Fields.Tabs]).Count > 0)
                    {
                        _logRepository.Info("ServiceTab tabs data population");
                        foreach (Sitecore.Data.Items.Item category in listOfCategory)
                        {
                            List<Service> serviceList = new List<Service>();
                            if (((Sitecore.Data.Fields.MultilistField)category.Fields[Templates.AddOnServiceTabCategory.Fields.ListOfServices]).Count > 0)
                            {
                                _logRepository.Info("ServiceTab services data population");
                                var listOfServices = ((Sitecore.Data.Fields.MultilistField)category.Fields[Templates.AddOnServiceTabCategory.Fields.ListOfServices]).GetItems();
                                foreach (Sitecore.Data.Items.Item services in listOfServices)
                                {
                                    Service service = new Service();
                                    service.Src = _helper.GetImageURL(services, Templates.AddOnService.Fields.ServiceImage.ToString());
                                    service.Alt = _helper.GetImageAlt(services, Templates.AddOnService.Fields.ServiceImage.ToString());
                                    service.InitialPrice = !string.IsNullOrEmpty(services.Fields[Templates.AddOnService.Fields.OldPrice].Value.ToString()) ? services.Fields[Templates.AddOnService.Fields.OldPrice].Value.ToString() : "";
                                    service.FinalPrice = !string.IsNullOrEmpty(services.Fields[Templates.AddOnService.Fields.NewPrice].Value.ToString()) ? services.Fields[Templates.AddOnService.Fields.NewPrice].Value.ToString() : "";
                                    service.Title = !string.IsNullOrEmpty(services.Fields[Templates.AddOnService.Fields.Title].Value.ToString()) ? services.Fields[Templates.AddOnService.Fields.Title].Value.ToString() : "";
                                    service.Description = !string.IsNullOrEmpty(services.Fields[Templates.AddOnService.Fields.Description].Value.ToString()) ? services.Fields[Templates.AddOnService.Fields.Description].Value.ToString() : "";
                                    serviceList.Add(service);
                                }
                            }
                            tabContentList.Add(new Dictionary<string, TabData> {
                         {
                                category.Name.ToString(),
                                new TabData
                                {
                                    title = !string.IsNullOrEmpty(category.Fields[Templates.AddOnServiceTabCategory.Fields.CategoryName].Value.ToString()) ? category.Fields[Templates.AddOnServiceTabCategory.Fields.CategoryName].Value.ToString() : "",
                                    data = serviceList
                                 }
                            }
                        });
                        }
                        _obj.tabContent = tabContentList;
                    }
                }
                else return null;
                _logRepository.Info("GetServiceTab Ended");
            }
            catch(Exception ex)
            {
                _logRepository.Error("GetServiceTab throws Exception -> " + ex.Message);
            }
            return _obj;
        }
    }
}