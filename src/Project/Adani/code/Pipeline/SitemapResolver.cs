using Sitecore.Adani.Website.Models;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Links;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Sites;
using Sitecore.Web;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Web;
using System.Xml.Serialization;

namespace Sitecore.Adani.Website.Pipeline
{

    public class SitemapResolver : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            //This check will verify if the physical path of the request exists or not.
            if (!System.IO.File.Exists(args.HttpContext.Request.PhysicalPath) &&
                !System.IO.Directory.Exists(args.HttpContext.Request.PhysicalPath))
            {
                Assert.ArgumentNotNull(args, "args");
                //Check if the request is of sitemap.xml then only allow the request to serve sitemap.xml
                if (args.Url == null || !args.Url.FilePath.ToLower().EndsWith("sitemap.xml")) return;
                if (args.StartPath == null || !args.StartPath.ToLower().EndsWith("adani/home")) return;
                try
                {
                    var homepage = Context.Database.GetItem(args.StartPath);
                    string hostname = string.Empty;
                    List<string> excludeitemname = new List<string>();

                    var excludeItemsandchildren = Sitecore.Configuration.Settings.GetSetting("ExcludeSitecoreItemsAndItsChildrenByItemInSitemap");
                    var excludeitemcollection = excludeItemsandchildren.Split(',').ToList();

                    if (excludeitemcollection.Any())
                    {
                        foreach (var item in excludeitemcollection)
                        {
                            var itempath = Context.Database.GetItem(ID.Parse(item));
                            if (itempath != null)
                            {
                                excludeitemname.Add(itempath.Name.ToLower());
                            }
                        }
                    }


                    // Homepage of the Website.
                    // Start path will give homepage including Multisite.

                    if (homepage != null)
                    {
                        var sitename = SiteContextFactory.Sites
                          .Where(s => !string.IsNullOrWhiteSpace(s.RootPath) && homepage.Paths.Path.StartsWith(s.RootPath, StringComparison.OrdinalIgnoreCase))
                          .OrderByDescending(s => s.RootPath.Length)
                          .FirstOrDefault();

                        if (sitename != null)
                        {
                            hostname = sitename.HostName;
                        }
                    }
                    var excludeItemsandchildrenbytemplatesid = Sitecore.Configuration.Settings.GetSetting("ExcludeSitecoreItemsAndItsChildrenByTemplatesInSitemap");
                    var excludeitemtemplatecollection = excludeItemsandchildren.Split(',').ToList();

                    if (excludeitemtemplatecollection.Any())
                    {
                        foreach (var objtemplate in excludeitemtemplatecollection)
                        {
                            var commanfoldernames = homepage.Axes.GetDescendants().Where(c => c.TemplateID == ID.Parse(objtemplate)).ToList();
                            if(commanfoldernames.Count() > 0)
                            {
                                foreach (var item in commanfoldernames)
                                {
                                    excludeitemname.Add(item.Name.ToLower());
                                }
                            }
                        }
                    }
                    var ser = new XmlSerializer(typeof(Urlset));
                    var urlSet = new Urlset();
                    //Create node of Homepage in Sitemap.
                    var tmpurlset = new List<SitemapUrl>();
                    var config = AppendLanguage();

                    if (!ExcludeItemFromSitemap(homepage, excludeitemname))
                    {
                        tmpurlset.Add(new SitemapUrl
                        {
                            Name = "Home",
                            Loc = GetAbsoluteLink(LinkManager.GetItemUrl(homepage, new UrlOptions() { LanguageEmbedding = (config == 2 ? LanguageEmbedding.Always : (config == 1 ? LanguageEmbedding.AsNeeded : LanguageEmbedding.Never)) }), hostname),
                            Lastmod = homepage.Statistics.Updated.ToString("yyyy-MM-dd hh:mm:ss")
                        });
                    }

                    var excludeItems = Sitecore.Configuration.Settings.GetSetting("ExcludeSitecoreItemsByTemplatesInSitemap");
                    // Get all decendants of Homepage to create full Sitemap.
                    var childrens = GetDescendantsByTemplate(homepage, excludeItems);
                    //Remove the items whose templateid is in exclude list
                    var finalcollection = childrens.Where(x => !ExcludeItemFromSitemap(x, excludeitemname)).ToList();

                    tmpurlset.AddRange(finalcollection.Select(childItem => new SitemapUrl
                    {
                        Name = childItem.Name,
                        Loc = GetAbsoluteLink(LinkManager.GetItemUrl(childItem, new UrlOptions() { LanguageEmbedding = (config == 2 ? LanguageEmbedding.Always : (config == 1 ? LanguageEmbedding.AsNeeded : LanguageEmbedding.Never)) }), hostname),
                        Lastmod = childItem.Statistics.Updated.ToString("yyyy-MM-dd hh:mm:ss")
                    }));

                    // Populate created collection to right object
                    urlSet.Url = tmpurlset;

                    //Write XML Response for Sitemap.
                    var response = HttpContext.Current.Response;
                    response.AddHeader("Content-Type", "text/xml");
                    ser.Serialize(response.OutputStream, urlSet);
                    HttpContext.Current.Response.End();
                    //Response Ends Here

                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
                    Log.Error("Error - Sitemap.xml.", ex, this);
                }
            }
        }


        public static IEnumerable<Item> GetDescendantsByTemplate(Item rootItem, string templateIds)
        {
            var collection = templateIds.Split(',').ToList();


            return from i in rootItem.Axes.GetDescendants()
                   where !collection.Contains(i.TemplateID.ToString())
                   //&& excludeitemname.Any(c => i.Paths.Path.ToLower().Split(',').Any(j => j == c))
                   select i;
        }

        ///

        /// Crete Absolute url as per the site
        /// 

        ///
        ///
        private static string GetAbsoluteLink(string relativeUrl, string hostname)
        {
            relativeUrl = System.Convert.ToString(relativeUrl).Replace("  ", "-").Replace(" ", "-").ToLower();
            if (!string.IsNullOrEmpty(hostname))
            {
                return HttpContext.Current.Request.Url.Scheme + "://" + hostname + relativeUrl;
            }
            else
            {
                return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + relativeUrl;
            }

        }

        ///

        /// Append language or not in URL to return language specific sitemap.xml
        /// 

        ///
        private static int AppendLanguage()
        {
            return string.IsNullOrEmpty(Sitecore.Configuration.Settings.GetSetting("LanguageEmbedForSitemap")) ? 0 : System.Convert.ToInt32((Sitecore.Configuration.Settings.GetSetting("LanguageEmbedForSitemap")));
        }

        ///

        /// This method will get a list of excluding template ids and will check if the passed item is in
        /// 

        ///
        ///
        private static bool ExcludeItemFromSitemap(Item objItem, List<string> excludeitemname)
        {
            //Check if the item is having any version
            if (objItem.Versions.Count > 0)
            {
                //ExcludeSitecoreItemsAndItsChildren
                var excludeItems = Sitecore.Configuration.Settings.GetSetting("ExcludeSitecoreItemsByTemplatesInSitemap");

                var itempathcollection = objItem.Paths.FullPath.ToLower().Split('/');

                if (excludeItems.Any() && itempathcollection.Any())
                {
                    var a = itempathcollection.Where(i => excludeitemname.Contains(i)).ToList();
                    if (a.Any())
                    {
                        return true;
                    }
                    else
                    {
                        var collection = excludeItems.Split(',').ToList();
                        return collection.Contains(objItem.TemplateID.ToString());
                    }
                }
                else
                {
                    var collection = excludeItems.Split(',').ToList();
                    return collection.Contains(objItem.TemplateID.ToString());
                }




            }
            return true;
        }
    }

}