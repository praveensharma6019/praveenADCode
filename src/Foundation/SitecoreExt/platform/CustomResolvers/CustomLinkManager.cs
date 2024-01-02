using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.IO;
using Sitecore.Links;
using Sitecore.Links.UrlBuilders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.CustomResolvers
{
    
    public class CustomLinkManager: ItemUrlBuilder
    {
        public CustomLinkManager(DefaultItemUrlBuilderOptions defaultOptions) : base(defaultOptions)
        { }
        public override string Build(Item item, ItemUrlBuilderOptions options)
        {
            //Only update the url's for the template items
            if (item.TemplateID == ID.Parse(Constants.Constant.CitytoCItyTemplateID))
            {
                //forming the url
                //item.Name=chicken
                return FileUtil.MakePath(item.Name, string.Empty).Replace(" ", "-");
            }
            return base.Build(item, options);
        }

    }
}