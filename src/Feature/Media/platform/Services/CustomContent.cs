using Adani.SuperApp.Airport.Feature.Media.Platform.Models;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Adani.SuperApp.Airport.Foundation.Theming.Platform.Services;
using Sitecore.Mvc.Presentation;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using System;


namespace Adani.SuperApp.Airport.Feature.Media.Platform.Services
{
    public class CustomContent : ICustomContent
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;

        public CustomContent (ILogRepository logRepository, IHelper helper)
        {
            this._logRepository = logRepository;
            this._helper = helper;
        }
        public CustomContentList GetCustomContentData(Rendering rendering, string queryString, string cityqueryString)
        {
            CustomContentList customContentList = new CustomContentList();           
            customContentList.contentItems = new List<CustomContentItem>();
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? RenderingContext.Current.Rendering.Item
                : null;
                // Null Check for datasource

                if (datasource == null && datasource.Children.Count() == 0)
                {
                    throw new NullReferenceException("Adani.SuperApp.Airport.Feature.Media.Platform.Services.GetServiceListData => Rendering Datasource is Empty");
                }

                if (!string.IsNullOrEmpty(cityqueryString))
                {
                    var cityDS = datasource.Children.Where(x => x.Name.ToLower() == cityqueryString).FirstOrDefault();
                    if(cityDS != null && cityDS.Children.Count > 0)
                    {
                        List<Item> citychildList = cityDS.Children.Where(x => x.TemplateID == Templates.CustomContentCollection.TitleWithRichTextTemplateID).ToList();
                        if(citychildList != null && citychildList.Any())
                        {
                            foreach (var child in citychildList)
                            {
                                CustomContentItem contentItem = new CustomContentItem();
                                contentItem.name = child.Name.ToLower();
                                contentItem.title = !string.IsNullOrEmpty(child.Fields["Title"].Value.ToString()) ? child.Fields["Title"].Value.ToString() : "";
                                contentItem.richText = !string.IsNullOrEmpty(child.Fields["RichText"].Value.ToString()) ? child.Fields["RichText"].Value.ToString() : "";
                                customContentList.contentItems.Add(contentItem);
                            }
                        }                        
                    }
                }
                else
                {
                    List<Item> childList = datasource.Children.Where(x => x.TemplateID == Templates.CustomContentCollection.TitleWithRichTextTemplateID).ToList();
                    if(childList != null && childList.Any())
                    {
                        foreach (var child in childList)
                        {
                            CustomContentItem contentItem = new CustomContentItem();
                            contentItem.name = child.Name.ToLower();
                            contentItem.title = !string.IsNullOrEmpty(child.Fields["Title"].Value.ToString()) ? child.Fields["Title"].Value.ToString() : "";
                            contentItem.richText = !string.IsNullOrEmpty(child.Fields["RichText"].Value.ToString()) ? child.Fields["RichText"].Value.ToString() : "";
                            customContentList.contentItems.Add(contentItem);
                        }
                    }
                }
            }
                
            catch (Exception ex)
            {

                _logRepository.Error(" Adani.SuperApp.Airport.Feature.Media.Platform.CustomContent GetCustomContentData gives -> " + ex.Message); 
            }


            return customContentList;
        }
    }
}