using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Adani.SuperApp.Realty.Foundation;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using static Adani.SuperApp.Realty.Feature.Career.Platform.Templates;

namespace Adani.SuperApp.Realty.Feature.Career.Platform.Services
{
    public class AboutCareerServices : IAboutCareerServices
    {
        private readonly ILogRepository _logRepository;

        public AboutCareerServices(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
       
        public AboutCareerData GetAboutCareer(Rendering rendering)
        {
            AboutCareerData aboutCareerData = new AboutCareerData();
            try
            {

                aboutCareerData.aboutCareer = GetStaticText(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return aboutCareerData;
        }

        public AboutCareerDataItem GetStaticText(Rendering rendering)
        {
            AboutCareerDataItem aboutCareerDataItem = new AboutCareerDataItem();
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
                if (datasource.TemplateID == Templates.AboutCareer.TemplateID)
                {
                    Item item = datasource;
                    aboutCareerDataItem.Heading = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                    aboutCareerDataItem.About = item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value != null ? item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value : "";

                    aboutCareerDataItem.ReadMore = item.Fields[Templates.IDescription.FieldsName.Description].Value != null ? item.Fields[Templates.IDescription.FieldsName.Description].Value : "";

                    aboutCareerDataItem.Terms = item.Fields[Templates.IBody.FieldsName.Body].Value != null ? item.Fields[Templates.IBody.FieldsName.Body].Value : "";

                    aboutCareerDataItem.DetailLink = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, Templates.ILink.FieldsName.Link) != null ?
                                      Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, Templates.ILink.FieldsName.Link) : "";

                    aboutCareerDataItem.DetailLinkText = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkTextbyField(item, item.Fields[Templates.ILink.FieldsName.Link]) != null ?
                                      Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkTextbyField(item, item.Fields[Templates.ILink.FieldsName.Link]) : "";

                    aboutCareerDataItem.ExtrCharges = item.Fields[Templates.ISummary.FieldsName.Summary].Value != null ? item.Fields[Templates.ISummary.FieldsName.Summary].Value : "";

                }

            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return aboutCareerDataItem;
        }

    }
}