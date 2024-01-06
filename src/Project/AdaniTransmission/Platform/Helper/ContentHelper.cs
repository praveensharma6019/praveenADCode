using System;
using System.Web.Mvc;
using Sitecore.Data;

namespace Adani.BAU.Transmission.Project.Platform.Helper
{
    public static class ContentHelper
    {
        public static MvcHtmlString GTMHeader(string itemid)
        {
            var str = new MvcHtmlString(String.Empty);
            try
            {

                var iItemid = ID.Parse(itemid);
                Sitecore.Data.Items.Item siteItem = Sitecore.Context.Database.Items.GetItem(iItemid);
                str = new MvcHtmlString(siteItem["HeaderTagScript"]);
            }
            catch (Exception)
            {
                //Sitecore.Diagnostics.TextLog(string.Format("Navigation Helper: Error Message:{0}, {1}", ex.Message, ex.StackTrace));
            }
            return str;

        }

        public static MvcHtmlString GTMBody(string itemid)
        {
            var str = new MvcHtmlString(String.Empty);
            try
            {

                var iItemid = ID.Parse(itemid);
                Sitecore.Data.Items.Item siteItem = Sitecore.Context.Database.Items.GetItem(iItemid);
                str = new MvcHtmlString(siteItem["BodyTagScript"]);
            }
            catch (Exception)
            {
                //Sitecore.Diagnostics.TextLog(string.Format("Navigation Helper: Error Message:{0}, {1}", ex.Message, ex.StackTrace));
            }
            return str;

        }
    }
}