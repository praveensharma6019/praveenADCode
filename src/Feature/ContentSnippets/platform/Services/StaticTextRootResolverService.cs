using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public class StaticTextRootResolverService : IStaticTextRootResolverService
    {
        
        private readonly ILogRepository _logRepository;
        public StaticTextRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public StaticTextData GetStaticTextData(Rendering rendering)
        {
            StaticTextData statictextDataData = new StaticTextData();
            try
            {

                statictextDataData.staticText = GetStaticText(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return statictextDataData;
        }

        public StaticTextDataItem GetStaticText(Rendering rendering)
        {
            StaticTextDataItem statictextdataItem = new StaticTextDataItem();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
                : null;
                // Null Check for datasource
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }
                if (datasource.TemplateID == Templates.StaticText.TemplateID)
                {
                    Item item = datasource;
                    statictextdataItem.Rera = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                    statictextdataItem.NewLaunch = item.Fields[Templates.IDescription.FieldsName.Description].Value != null ? item.Fields[Templates.IDescription.FieldsName.Description].Value : "";
                    
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return statictextdataItem;
        }

    }
}