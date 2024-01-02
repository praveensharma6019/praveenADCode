using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public class AboutAdaniRealtyRootResolverService : IAboutAdaniRealtyRootResolverService
    {
        private readonly ILogRepository _logRepository;
        public AboutAdaniRealtyRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }

        public AboutAdaniRealtyData GetAboutAdaniRealtyDataList(Rendering rendering)
        {
            AboutAdaniRealtyData aboutAdaniRealtyDataList = new AboutAdaniRealtyData();
            try
            {

                aboutAdaniRealtyDataList.desc = GetAboutAdaniRealtyData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" AboutAdaniRealityDataRootResolverService GetAboutAdaniDataList gives -> " + ex.Message);
            }

            return aboutAdaniRealtyDataList;
        }

        public List<Object> GetAboutAdaniRealtyData(Rendering rendering)
        {
            List<Object> aboutAdaniRealtyDataList = new List<Object>();
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
                AboutAdaniRealtyDataItem aboutAdaniRealtyDataItem;
                if (datasource.TemplateID == Templates.AboutAdaniRealtyFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {
                    List<Item> children = datasource.Children.Where(x => x.TemplateID == Templates.AboutAdaniRealty.TemplateID).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Sitecore.Data.Items.Item item in children)
                        {
                            aboutAdaniRealtyDataItem = new AboutAdaniRealtyDataItem();

                            aboutAdaniRealtyDataItem.aboutAdaniRealtyText = item.Fields[Templates.IDescription.FieldsName.Description].Value != null ? item.Fields[Templates.IDescription.FieldsName.Description].Value : "";

                            aboutAdaniRealtyDataList.Add(aboutAdaniRealtyDataItem);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(" AboutAdaniRealtyService AboutAdaniRealtyDataList gives -> " + ex.Message);
            }

            return aboutAdaniRealtyDataList;
        }


    }
}