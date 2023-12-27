using Adani.SuperApp.Airport.Feature.MetaData.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Feature.MetaData.Platform.Services;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Sites;
using System;
using System.Collections.Generic;
using System.Linq;


namespace Adani.SuperApp.Airport.Feature.MetaData.Platform.Services
{
    public class PageMetaData : IPageMetaData

    {
        private readonly ILogRepository logging;
        private readonly IHelper helper;

        public PageMetaData(ILogRepository _logging, IHelper _helper)
        {
            this.logging = _logging;
            this.helper = _helper;

        }

        public Metadata GetMetadata(Item datasource)
        {
            Metadata metadata = new Metadata();

            if (datasource != null)
            {
                 
                
                metadata.MetaTitle = datasource.Fields[Templates.fields.MetaTitle].ToString();
                metadata.MetaDescription = datasource.Fields[Templates.fields.MetaDescription].ToString();
                metadata.Keywords = datasource.Fields[Templates.fields.Keywords].ToString();
                metadata.Canonical = helper.LinkUrl(datasource.Fields[Templates.fields.Canonical]);
                metadata.Viewport = datasource.Fields[Templates.fields.Viewport].ToString();
                metadata.Robots = datasource.Fields[Templates.fields.Robots].ToString();
                metadata.OG_Title = datasource.Fields[Templates.fields.OG_Title].ToString();
                metadata.OG_Image = datasource.Fields[Templates.fields.OG_Image].ToString();
                metadata.OG_Description = datasource.Fields[Templates.fields.OG_Description].ToString();

            }
            return metadata;

        }
       
    }
}