using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Feature.Master.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.Services
{
    public class DomainListService : IDomainListService
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetService;

        public DomainListService(ILogRepository logRepository, IWidgetService widgetService)
        {
            _logRepository = logRepository;
            _widgetService = widgetService;
        }

        public WidgetModel GetDomainData(Rendering rendering)
        {
            WidgetModel widgetList = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.RenderingParamField]);
                widgetList.widget = widget != null ? _widgetService.GetWidgetItem(widget) : new WidgetItem();
                widgetList.widget.widgetItems = GetDomainDataList(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetDomainData in DomainListService gives -> " + ex.Message);
            }
            return widgetList;
        }

        public List<object> GetDomainDataList(Rendering rendering)
        {
            List<object> domainsList = new List<object>();
            try
            {
                Item dataSource = rendering.Item;
                DomainListModel domains = null;
                if (dataSource.Children != null && dataSource.Children.Count > 0)
                {
                    foreach (Item domain in dataSource.GetChildren())
                    {
                        domains = new DomainListModel();
                        domains.label = domain.Fields[Templates.Label] != null ? domain.Fields[Templates.Label].Value : String.Empty;
                        domains.value = domain.Fields[Templates.Value] != null ? domain.Fields[Templates.Value].Value : String.Empty;
                        domainsList.Add(domains);
                    }
                }
            }

            catch (Exception ex)
            {
                _logRepository.Error("GetDomainData in DomainListService gives " + ex.Message);
            }
            return domainsList;
        }
    }
}
