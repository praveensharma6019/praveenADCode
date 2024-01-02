using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Extensions;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services
{
    public class TableJSON : ITableJSON
    {
        private readonly ILogRepository _logRepository;
        private readonly IWidgetService _widgetservice;

        public TableJSON(ILogRepository logRepository, IWidgetService widgetService)
        {
            this._logRepository = logRepository;
            this._widgetservice = widgetService;
        }

        public WidgetModel GetTableDataList(Rendering rendering)
        {
            WidgetModel TableDataList = new WidgetModel();
            try
            {
                Item widget = Sitecore.Context.Database.GetItem(rendering.Parameters[Constants.RenderingParamField]);
                TableDataList.widget = widget != null ? _widgetservice.GetWidgetItem(widget) : new WidgetItem();
                TableDataList.widget.widgetItems = GetTableData(rendering);
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetTableDataList gives -> " + ex.Message);
            }
            return TableDataList;
        }

        private List<Object> GetTableData(Rendering rendering)
        {
            List<Object> tableDataList = new List<Object>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? RenderingContext.Current.Rendering.Item
                : null;
                // Null Check for datasource
                if (datasource == null && datasource.Children.Count() == 0)
                {
                    throw new NullReferenceException("Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services.GetTableData => Rendering Datasource is Empty");
                }
                TableData data = new TableData();
                TableModel tableModel = null;
                var tableItems = datasource.Children.Where(a => a.TemplateID == Constants.TableDataTemplateID).ToList();
                if (tableItems != null && tableItems.Count > 0)
                {
                    foreach (Item item in tableItems)
                    {
                        tableModel = new TableModel();
                        tableModel.rowTitle = item.Fields[Constants.RowTitle] != null ? item.Fields[Constants.RowTitle].Value.ToString() : "";
                        MultilistField columnDataList = item.Fields[Constants.Column];
                        if (columnDataList != null && columnDataList.Count > 0)
                        {
                            tableModel.column.columnValue = (from Item columnItem in columnDataList.GetItems() select columnItem.Fields[Constants.RowTitle].Value.ToString()).ToList();
                        }
                        data.tableData.Add(tableModel);
                    }
                    tableDataList.Add(data);
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" Adani.SuperApp.Airport.Feature.CustomContent.Platform.CustomContent GetTableData gives -> " + ex.Message);
            }

            return tableDataList;
        }
    }
}