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
    public class GetInTouchRootResolverService : IGetInTouchRootResolverService
    {

        private readonly ILogRepository _logRepository;
        public GetInTouchRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public GetInTouchData GetInTouchDataList(Rendering rendering)
        {
            GetInTouchData getInTouchDataList = new GetInTouchData();
            try
            {

                getInTouchDataList.getInTouch = GetInTouchData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return getInTouchDataList;
        }

        public List<Object> GetInTouchData(Rendering rendering)
        {
            List<Object> getInTouchData = new List<Object>();
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
                GetInTouchDataItem getInTouchdata;
                if (datasource.TemplateID == Templates.GetInTouchFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {
                    List<Item> children = datasource.Children.Where(x => x.TemplateID == Templates.GetInTouch.TemplateID).ToList();
                    if (children != null && children.Count > 0)
                    {
                        foreach (Item item in children)
                        {
                            getInTouchdata = new GetInTouchDataItem();

                            getInTouchdata.heading = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                            getInTouchdata.description = item.Fields[Templates.IDescription.FieldsName.Description].Value != null ? item.Fields[Templates.IDescription.FieldsName.Description].Value : "";

                            getInTouchdata.button = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkTextbyField(item, item.Fields[Templates.ILink.FieldsName.Link]) != null ?
                                      Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkTextbyField(item, item.Fields[Templates.ILink.FieldsName.Link]) : "";

                            getInTouchdata.buttonLink = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, Templates.ILink.FieldsName.Link) != null ?
                                      Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, Templates.ILink.FieldsName.Link) : "";

                            getInTouchdata.enquireNowLabel = item.Fields[Templates.IDescription.SubTitle].Value != null ? item.Fields[Templates.IDescription.SubTitle].Value : "";
                            getInTouchData.Add(getInTouchdata);
                        }
                    }
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return getInTouchData;
        }

    }
}