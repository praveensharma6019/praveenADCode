using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Pipelines.HttpRequest;

namespace Adani.SuperApp.Airport.Feature.MetaData.Platform.Models
{
    public class SeoProcessor : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context == null)
            {
                return;
            }

            string requestUrl = context.Request.Url.ToString();

            if (string.IsNullOrEmpty(requestUrl))
            {
                return;
            }

            if (requestUrl.ToLower().TrimEnd('/').EndsWith("robots.txt"))
            {
                ProcessRobots(context);
            }
            else if (requestUrl.ToLower().TrimEnd('/').EndsWith("sitemap.xml"))
            {
                ProcessSitemap(context);
            }
        }

        private static void ProcessRobots(HttpContext context)
        {
            string robotsTxtContent = string.Empty;
            if (global::Sitecore.Context.Site != null && global::Sitecore.Context.Database != null)
            {
                Item siteconfigNode = GetConfigNode();
                if (siteconfigNode !=null && siteconfigNode.Fields[Templates.HeadDataCollection.RobotsContent] != null)
                {
                    robotsTxtContent = siteconfigNode.Fields[Templates.HeadDataCollection.RobotsContent].Value;
                }
                else
                {
                    robotsTxtContent = string.Empty;
                }
               
            }
            context.Response.ContentType = "text/plain";
            context.Response.Write(robotsTxtContent);
            context.Response.End();
        }

        private static void ProcessSitemap(HttpContext context)
        {
            context.Response.ContentType = "text/xml";
            context.Response.Write("<?xml version=\"1.0\" encoding=\"UTF-8\"?>" + Environment.NewLine);
            if (context.Request.Path == "/sitemap.xml")
            {
                context.Response.Write("<sitemapindex xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\">" + Environment.NewLine);
                context.Response.Write(XmlSitemap.GetCacheXml(null));
                context.Response.Write(Environment.NewLine + "</sitemapindex>");
            }
            else if(context.Request.Path == "/home-sitemap.xml")
            {
                context.Response.Write("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:mobile=\"http://www.google.com/schemas/sitemap-mobile/1.0\">" + Environment.NewLine);
                context.Response.Write(XmlSitemap.GetHomeCacheXml(null));
                context.Response.Write(Environment.NewLine + "</urlset>");
            }
            else
            {
                context.Response.Write("<urlset xmlns=\"http://www.sitemaps.org/schemas/sitemap/0.9\" xmlns:mobile=\"http://www.google.com/schemas/sitemap-mobile/1.0\">" + Environment.NewLine);
                context.Response.Write(XmlSitemap.GetCacheXml(null));
                context.Response.Write(Environment.NewLine + "</urlset>");
            }
            context.Response.End();
        }

        public static Item GetConfigNode()
        {
            Sitecore.Sites.SiteContext site = Sitecore.Context.Site;
            if (site == null) return null;

            Sitecore.Data.Database db = Sitecore.Context.Database;
            if (db == null) return null;
            Item start = db.GetItem(site.StartPath);
            if (start == null) return null;
            
            return start;
        }

        public bool IsReusable
        {
            get { return true; }
        }
    }
}