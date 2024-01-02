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
    public class AboutCityRootResolverService : IAboutCityRootResolverService
    {
        
        private readonly ILogRepository _logRepository;
        public AboutCityRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public AboutCityData GetAboutCityDataList(Rendering rendering)
        {
            AboutCityData aboutCityData = new AboutCityData();
            try
            {

                aboutCityData.aboutCity = GetAboutCityData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return aboutCityData;
        }

        public AboutCityDataItem GetAboutCityData(Rendering rendering)
        {
            AboutCityDataItem aboutCitydataItem = new AboutCityDataItem();
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
                if (datasource.TemplateID == Templates.AboutCity.TemplateID)
                {
                    Item item = datasource;
                    aboutCitydataItem.Heading = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                    aboutCitydataItem.About = item.Fields[Templates.IDescription.FieldsName.Description].Value != null ? item.Fields[Templates.IDescription.FieldsName.Description].Value : "";

                    aboutCitydataItem.ReadMore = item.Fields[Templates.ISummary.FieldsName.Summary].Value != null ? item.Fields[Templates.ISummary.FieldsName.Summary].Value : "";                  
                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return aboutCitydataItem;
        }

    }
}