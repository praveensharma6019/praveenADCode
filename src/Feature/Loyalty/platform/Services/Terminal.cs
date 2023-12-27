using System;
using System.Collections.Generic;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Sitecore.Data.Items;
using Adani.SuperApp.Airport.Feature.Loyalty.Platform.Models;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.Loyalty.Platform.Services
{
    public class Terminal : ITerminal
    {
        private readonly ILogRepository _logRepository;
        private readonly Foundation.Theming.Platform.Services.IWidgetService _widgetservice;
        public Terminal(ILogRepository logRepository, IWidgetService widgetservice)
        {
            this._logRepository = logRepository;
            this._widgetservice = widgetservice;
        }
       
        WidgetModel ITerminal.GetTerminal(Rendering rendering, string Location)
        {
            WidgetModel widgetModel = new WidgetModel();
            try
            {
                Item widget = rendering.Parameters[Templates.ServicesListCollection.RenderingParamField] != null ? Sitecore.Context.Database.GetItem(rendering.Parameters[Templates.ServicesListCollection.RenderingParamField]) : null;
                widgetModel.widget = widget != null ? widgetModel.widget = _widgetservice.GetWidgetItem(widget) : new Foundation.Theming.Platform.Models.WidgetItem();
                widgetModel.widget.widgetItems = GetTerminalData(rendering , Location);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" ITerminal.GetTerminal gives -> " + ex.Message);
            }
            return widgetModel;
        }

        private List<object> GetTerminalData(Rendering rendering, string Location)
        {
            List<object> terminalModels = new List<object>();
            TerminalModel terminalModel;
            //Get the datasource for the item
            var datasourceItem = RenderingContext.Current.Rendering.Item;
            // Null Check for datasource
            if (datasourceItem != null)
            {
                var terminal = datasourceItem.GetChildren().Where(p => p.Fields["Title"].Value == Location).FirstOrDefault();
                terminalModel = new TerminalModel();
                terminalModel.title = terminal.Fields["Title"].Value;
                terminalModel.airportName= terminal.Fields["Airport Name"].Value;
                terminalModel.list = terminal.GetChildren().Select(x => x.Fields["Title"].Value).ToList();
                terminalModels.Add(terminalModel);
            }
            else
            {
                _logRepository.Error(" GetTerminalData data source is empty");
            }
            return terminalModels;
        }
    }
}