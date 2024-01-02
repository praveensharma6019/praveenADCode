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
    public class AboutUsRootResolverService:IAboutUsRootResolverService
    {

        private readonly ILogRepository _logRepository;
        public AboutUsRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public AboutUsData GetAboutUsDataList(Rendering rendering)
        {
            AboutUsData aboutUsDataList = new AboutUsData();
            try
            {

                aboutUsDataList.desc = GetAboutUsData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" AboutUsDataRootResolverService GetAboutUsDataList gives -> " + ex.Message);
            }

            return aboutUsDataList;
        }

        public List<Object> GetAboutUsData(Rendering rendering)
        {
            List<Object> getAboutUsDataList = new List<Object>();
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
                AboutUsDataItem aboutUsdata;
                if (datasource.TemplateID == Templates.AboutUsFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {
                    List<Item> children = datasource.Children.Where(x => x.TemplateID == Templates.AboutUs.TemplateID).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Sitecore.Data.Items.Item item in children)
                        {
                            aboutUsdata = new AboutUsDataItem();

                            aboutUsdata.aboutUsText = item.Fields[Templates.IDescription.FieldsName.Description].Value != null ? item.Fields[Templates.IDescription.FieldsName.Description].Value : "";

                            getAboutUsDataList.Add(aboutUsdata);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(" AboutUsDataService AboutUsDataList gives -> " + ex.Message);
            }

            return getAboutUsDataList;
        }

    }
}