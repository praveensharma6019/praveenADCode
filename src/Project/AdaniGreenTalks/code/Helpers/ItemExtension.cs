using Sitecore.AdaniGreenTalks.Website.Models;
using Sitecore.Data;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniGreenTalks.Website.Helpers
{
    public static class ItemExtension
    {
        public static IList<Options> GetOptionsList(ID itemid)
        {
            IList<Options> options = new List<Options>();
            Item item = Sitecore.Context.Database.Items.GetItem(itemid);
            var nameValueListString = item[SitecoreConstant.NameValueCollection];
            NameValueCollection nameValueList = Sitecore.Web.WebUtil.ParseUrlParameters(nameValueListString);
            
            if (nameValueList != null)
            {
                var items = nameValueList.AllKeys.SelectMany(nameValueList.GetValues, (k, v) => new { key = k, value = v });
                foreach (var NVitem in items)
                {
                    Options option = new Options
                    {
                        Value = NVitem.key,
                        Text = NVitem.value
                    };
                    options.Add(option);
                }
            }
            return options;
        }
    }
}