using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Realty.Feature.Leaders.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Realty.Feature.Leaders.Platform.Services
{
    public class LeadersDataRootResolverService : ILeadersDataRootResolverService
    {
        private readonly ILogRepository _logRepository;
        public LeadersDataRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public LeadersData GetLeadersDataList(Rendering rendering)
        {
            LeadersData leadersDataList = new LeadersData();
            try
            {

                leadersDataList.data = GetLeadersData(rendering).data;
                leadersDataList.Title = GetLeadersData(rendering).Title;
            }
            catch (Exception ex)
            {

                _logRepository.Error(" LeadersDataRootResolverService GetLeadersDataList gives -> " + ex.Message);
            }

            return leadersDataList;
        }

        public LeadersData GetLeadersData(Rendering rendering)
        {
            LeadersData leadersDataObj = new LeadersData();
            List<Object> leadersDataList = new List<Object>();
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
                LeadersDataItem leadersdata;
                if (datasource.TemplateID == Templates.LeadersDataFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {
                    leadersDataObj.Title = datasource.Fields[Templates.LeadersDataFolder.FieldsName.Title].Value != null ? datasource.Fields[Templates.LeadersDataFolder.FieldsName.Title].Value : "";
                    
                    List<Item> children = datasource.Children.Where(x => x.TemplateID == Templates.LeadersData.TemplateID).ToList();
                    
                    if (children != null && children.Count > 0)
                    {
                        foreach (Sitecore.Data.Items.Item item in children)
                        {
                            leadersdata = new LeadersDataItem();

                            leadersdata.quoteText = item.Fields[Templates.LeadersData.Fields.FieldsName.Quote].Value != null ? item.Fields[Templates.LeadersData.Fields.FieldsName.Quote].Value : "";

                            leadersdata.imageSrc = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Templates.Image.FieldsName.Image) != null ?
                                      Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Templates.Image.FieldsName.Image) : "";

                            leadersdata.imgAlt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                      Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["Alt"].Value : "";

                            leadersdata.imgTitle = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                      Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["TItle"].Value : "";

                            leadersdata.Name = item.Fields[Templates.LeadersData.Fields.FieldsName.Name].Value != null ? item.Fields[Templates.LeadersData.Fields.FieldsName.Name].Value : "";

                            leadersdata.Designation = item.Fields[Templates.LeadersData.Fields.FieldsName.Designation].Value != null ? item.Fields[Templates.LeadersData.Fields.FieldsName.Designation].Value : "";                  

                            leadersDataList.Add(leadersdata);
                        }
                        leadersDataObj.data = leadersDataList;
                    }
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(" LeadersDataService LeadersDataList gives -> " + ex.Message);
            }
            
            return leadersDataObj;
        }       

       
    }
}