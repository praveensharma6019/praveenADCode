using Adani.BAU.Transmission.Feature.Media.Platform.Models;
using Sitecore.Data.Items;
using System.Collections.Generic;
using System.Linq;
using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Adani.BAU.Transmission.Foundation.Theming.Platform.Services;
using Sitecore.Mvc.Presentation;
using Adani.BAU.Transmission.Foundation.Logging.Platform.Repositories;
using System;

namespace Adani.BAU.Transmission.Feature.Media.Platform.Services
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
        public CustomContentList GetCustomContentData(Rendering rendering, string queryString)
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
                    throw new NullReferenceException("Adani.BAU.Transmission.Feature.Media.Platform.Services.GetServiceListData => Rendering Datasource is Empty");
                }
                List<Item> childList = datasource.Children.Where(x => x.TemplateID == Templates.CustomContentCollection.TitleWithRichTextTemplateID).ToList();

                foreach(var child in childList)
                {
                    CustomContentItem contentItem = new CustomContentItem();
                    contentItem.name = child.Name.ToLower();
                    contentItem.title = !string.IsNullOrEmpty(child.Fields["Title"].Value.ToString()) ? child.Fields["Title"].Value.ToString() : "";
                    contentItem.richText = !string.IsNullOrEmpty(child.Fields["RichText"].Value.ToString()) ? child.Fields["RichText"].Value.ToString() : "";
                    customContentList.contentItems.Add(contentItem);
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(" Adani.BAU.Transmission.Feature.Media.Platform.CustomContent GetCustomContentData gives -> " + ex.Message); 
            }


            return customContentList;
        }
    }
}