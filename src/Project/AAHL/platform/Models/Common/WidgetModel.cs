using Glass.Mapper.Sc.Configuration.Attributes;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Project.AAHL.Website.Models.Common
{

    [SitecoreType(TemplateId = "{A7FE2C78-BDE0-4180-A165-F4ED61D8108C}", AutoMap = true)]
    public class WidgetModel
    {
        [SitecoreField]
        public virtual string WidgetType { get; set; }
        [SitecoreField]
        public virtual string WidgetSubType { get; set; }
        [SitecoreField]
        public virtual string Title { get; set; }
        [SitecoreField]
        public virtual string SubTitle { get; set; }
    }
}