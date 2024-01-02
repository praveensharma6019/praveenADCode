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
    public class AchievementsRootResolverService : IAchievementsRootResolverService
    {
        private readonly ILogRepository _logRepository;
        public AchievementsRootResolverService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }

        public AchievementsData GetAchievementsList(Rendering rendering)
        {
            AchievementsData achievementsList = new AchievementsData();
            try
            {

                achievementsList.data = GetAchievements(rendering);
            }
            catch (Exception ex)
            {

                _logRepository.Error(" AchievementsRootResolverService GetachievementsList gives -> " + ex.Message);
            }


            return achievementsList;
        }

        public List<Object> GetAchievements(Rendering rendering)
        {
            List<Object> achievementsList = new List<Object>();
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
                AchievementsDataItem achievementsdata;
                if (datasource.TemplateID == Templates.AchievementsFolder.TemplateID && datasource.Children.ToList().Count > 0)
                {
                    List<Item> children  = datasource.Children.Where(x => x.TemplateID == Templates.Achievement.TemplateID).ToList();
                    if(children!=null && children.Count>0)
                    {
                        foreach (Sitecore.Data.Items.Item item in children)
                        {
                            achievementsdata = new AchievementsDataItem();
                            achievementsdata.iconsrc = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Templates.Image.ThumbImageName) != null ?
                                         Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Templates.Image.ThumbImageName) : "";


                            achievementsdata.icon = item.Fields[Templates.ITitle.FieldsName.Title].Value != null ? item.Fields[Templates.ITitle.FieldsName.Title].Value : "";

                            achievementsdata.count = item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value != null ? item.Fields[Templates.ISubTitle.FieldsName.SubTitle].Value : "";

                            achievementsdata.Descriptions = item.Fields[Templates.IDescription.FieldsName.Description].Value != null ? item.Fields[Templates.IDescription.FieldsName.Description].Value : "";

                            achievementsdata.Start = item.Fields[Templates.Achievement.Fields.FieldsName.Start].Value != null ? item.Fields[Templates.Achievement.Fields.FieldsName.Start].Value : "";

                            achievementsdata.Delay = item.Fields[Templates.Achievement.Fields.FieldsName.Delay].Value != null ? item.Fields[Templates.Achievement.Fields.FieldsName.Delay].Value : "";

                            achievementsdata.imageSrc = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Templates.Image.FieldsName.Image) != null ?
                                         Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageSource(item, Templates.Image.FieldsName.Image) : "";

                            achievementsdata.imgAlt = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                         Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["Alt"].Value : "";

                            achievementsdata.imgTitle = Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Templates.Image.FieldsName.Image) != null ?
                                         Foundation.SitecoreHelper.Platform.Helper.Helper.GetImageDetails(item, Templates.Image.FieldsName.Image).Fields["TItle"].Value : "";

                            achievementsList.Add(achievementsdata);
                        }
                    }                  
                }
                
            }
            catch (Exception ex)
            {

                _logRepository.Error(" AchievementsService AchievementsList gives -> " + ex.Message);
            }

            return achievementsList;
        }
    }
}