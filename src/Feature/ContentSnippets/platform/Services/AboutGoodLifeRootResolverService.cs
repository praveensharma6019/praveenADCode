using Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Adani.SuperApp.Realty.Feature.ContentSnippets.Platform.Services
{
    public class AboutGoodLifeRootResolverService : IAboutGoodLifeRootResolverService
    {

        private readonly ILogRepository _logRepository;
        public AboutGoodLifeRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public AboutGoodLifeData GetAboutGoodLife(Rendering rendering)
        {
            AboutGoodLifeData aboutGoodLifeDataList = new AboutGoodLifeData();
            try
            {

                aboutGoodLifeDataList.AboutGoodLife = GetAboutGoodLifeData(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return aboutGoodLifeDataList;
        }

        public AboutGoodLifeDataItem GetAboutGoodLifeData(Rendering rendering)
        {
            string strSitedomain = string.Empty;
            var CItem = Sitecore.Context.Database.GetItem(Templates.commonItem);
            strSitedomain = CItem != null ? CItem.Fields["Site Domain"].Value : string.Empty;

            AboutGoodLifeDataItem aboutGoodLifedataItem = new AboutGoodLifeDataItem();
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
                if (datasource.TemplateID == Templates.AboutGoodLife.TemplateID)
                {
                    Item item = datasource;
                    aboutGoodLifedataItem.Heading = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";
                    
                    aboutGoodLifedataItem.About = item.Fields[Templates.AboutGoodLife.Fields.FieldsName.About].Value != null ? item.Fields[Templates.AboutGoodLife.Fields.FieldsName.About].Value : "";
                    
                    aboutGoodLifedataItem.ReadMore = item.Fields[Templates.AboutGoodLife.Fields.FieldsName.ReadMore].Value != null ? item.Fields[Templates.AboutGoodLife.Fields.FieldsName.ReadMore].Value : "";
                   
                    aboutGoodLifedataItem.Terms = item.Fields[Templates.AboutGoodLife.Fields.FieldsName.Terms].Value != null ? item.Fields[Templates.AboutGoodLife.Fields.FieldsName.Terms].Value : "";
                    
                    
                    var DetailLink = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, Templates.AboutGoodLife.Fields.FieldsName.DetailLink) != null ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkURL(item, Templates.AboutGoodLife.Fields.FieldsName.DetailLink) : "";
                    aboutGoodLifedataItem.DetailLink = DetailLink.Contains(strSitedomain) ? DetailLink.Replace(strSitedomain, "") : DetailLink; 

                    aboutGoodLifedataItem.ExtraCharges = item.Fields[Templates.AboutGoodLife.Fields.FieldsName.ExtraCharges].Value != null ? item.Fields[Templates.AboutGoodLife.Fields.FieldsName.ExtraCharges].Value : "";

                    aboutGoodLifedataItem.DetailLinkText = Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkTextbyField(item, item.Fields[Templates.AboutGoodLife.Fields.FieldsName.DetailLink]) != null ?
                            Foundation.SitecoreHelper.Platform.Helper.Helper.GetLinkTextbyField(item, item.Fields[Templates.AboutGoodLife.Fields.FieldsName.DetailLink]) : "";
                }

            }
            catch (Exception ex)
            {


                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return aboutGoodLifedataItem;
        }
        public AOGBriefModel GetAOGBrief(Rendering rendering)
        {
            AOGBriefModel AOGBriefData = new AOGBriefModel();
            try
            {
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
               ? rendering.RenderingItem?.Database.GetItem(rendering.DataSource)
               : null;
                if (datasource == null)
                {
                    throw new NullReferenceException();
                }

                AOGBriefData.pageHeading = datasource.Fields[Templates.AOGBriefTemp.pageHeading].Value != null ? datasource.Fields[Templates.AOGBriefTemp.pageHeading].Value : "";
                AOGBriefData.pageHeadingInGradiant = datasource.Fields[Templates.AOGBriefTemp.pageHeadingInGradiant].Value != null ? datasource.Fields[Templates.AOGBriefTemp.pageHeadingInGradiant].Value : "";
                AOGBriefData.sup = datasource.Fields[Templates.AOGBriefTemp.sup].Value != null ? datasource.Fields[Templates.AOGBriefTemp.sup].Value : "";
                AOGBriefData.subHeading = datasource.Fields[Templates.AOGBriefTemp.subHeading].Value != null ? datasource.Fields[Templates.AOGBriefTemp.subHeading].Value : "";
                AOGBriefData.description = datasource.Fields[Templates.AOGBriefTemp.description].Value != null ? datasource.Fields[Templates.AOGBriefTemp.description].Value : "";
            }
            catch (Exception ex)
            {

                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }

            return AOGBriefData;
        }

    }
}
