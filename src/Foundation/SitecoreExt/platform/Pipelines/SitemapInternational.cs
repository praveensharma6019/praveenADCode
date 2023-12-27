
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using Sitecore.Pipelines.HttpRequest;
using static Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.Models.UrlSet;


#region Main Code
namespace Adani.SuperApp.Airport.Foundation.SitecoreExtension.Platform.Pipelines
{ 
    public class SitemapInternational : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            //This check will verify if the physical path of the request exists or not.
            if (!System.IO.File.Exists(args.HttpContext.Request.PhysicalPath) &&
                !System.IO.Directory.Exists(args.HttpContext.Request.PhysicalPath))
            {
                Assert.ArgumentNotNull(args, "args");
                //Check if the request is of sitemap.xml then only allow the request to serve sitemap.xml
                if (args.Url == null || !args.Url.FilePath.ToLower().Contains("-sitemap.xml")) return;
                try
                {
                    // Homepage of the Website.
                    // Start path will give homepage including Multisite.
                    Item homepage= Context.Database.GetItem("/sitecore/content/AirportHome/domestic-flights");

                    if (args.Url.FilePath.Contains("domestic-flights-sitemap.xml"))
                    {
                          homepage = Context.Database.GetItem("/sitecore/content/AirportHome/domestic-flights");
                    }
                    else if(args.Url.FilePath.Contains("international-flights-sitemap.xml")) {

                         homepage = Context.Database.GetItem("/sitecore/content/AirportHome/international-flights");
                    }
                    else if (args.Url.FilePath.Contains("domestic-airlines-sitemap.xml"))
                    {

                        homepage = Context.Database.GetItem("/sitecore/content/AirportHome/domestic-airlines");
                    }
                    else if (args.Url.FilePath.Contains("international-airlines-sitemap.xml"))
                    {

                        homepage = Context.Database.GetItem("/sitecore/content/AirportHome/international-airlines");
                    }


                    var ser = new XmlSerializer(typeof(Urlset));

                    var urlSet = new Urlset();

                    //Create node of Homepage in Sitemap.
                    var tmpurlset = new List<Url>();
                    var config = AppendLanguage();

                    if (IncludeItemFromSitemap(homepage))
                    {
                        tmpurlset.Add(new Url
                        {
                            Loc = GetAbsoluteLink(LinkManager.GetItemUrl(homepage, new UrlOptions() { LanguageEmbedding = config == 2 ? LanguageEmbedding.Always : (config == 1 ? LanguageEmbedding.AsNeeded : LanguageEmbedding.Never) })),
                            Lastmod = homepage.Statistics.Updated.ToString("yyyy-MM-dd hh:mm:ss"),
                            ChangeFrequency = (!string.IsNullOrEmpty(homepage.Fields["changefrequency"].ToString()) ? homepage.Fields["changefrequency"].ToString() : string.Empty),
                            Priority = (!string.IsNullOrEmpty(homepage.Fields["priority"].ToString()) ? string.Join(".", homepage.Fields["priority"].ToString().ToCharArray()) : string.Empty),
                          
                        });
                    }
                        
                    // Get all decendants of Homepage to create full Sitemap.
                    var childrens = homepage.Axes.GetDescendants();
                    //Remove the items whose templateid is in exclude list
                    var finalcollection = childrens.Where(x => IncludeItemFromSitemap(x)).ToList();

                    tmpurlset.AddRange(finalcollection.Select(childItem => new Url
                    {
                        Loc = GetAbsoluteLink(LinkManager.GetItemUrl(childItem, new UrlOptions() { LanguageEmbedding = (config == 2 ? LanguageEmbedding.Always : (config == 1 ? LanguageEmbedding.AsNeeded : LanguageEmbedding.Never)) })),
                        Lastmod = childItem.Statistics.Updated.ToString("yyyy-MM-dd hh:mm:ss"),
                        ChangeFrequency = (!string.IsNullOrEmpty(childItem.Fields["changefrequency"].ToString()) ? childItem.Fields["changefrequency"].ToString() : string.Empty),
                        Priority = (!string.IsNullOrEmpty(childItem.Fields["priority"].ToString()) ? string.Join(".", childItem.Fields["priority"].ToString().ToCharArray()) : string.Empty),
                       
                    }));

                    // Populate created collection to right object
                    urlSet.Url = tmpurlset;

                    //Write XML Response for Sitemap.
                    var response = HttpContext.Current.Response;
                    response.AddHeader("Content-Type", "text/xml");
                    ser.Serialize(response.OutputStream, urlSet);
                    HttpContext.Current.Response.Flush(); // Sends all currently buffered output to the client.
                    HttpContext.Current.Response.SuppressContent = true;  // Gets or sets a value indicating whether to send HTTP content to the client.
                    HttpContext.Current.ApplicationInstance.CompleteRequest(); // Causes ASP.NET to bypass all events and filtering in the HTTP pipeline chain of execution and directly execute the EndRequest event
                    //Response Ends Here
                }
                catch (Exception ex)
                {
                    Log.Error("Error - Sitemap.xml.", ex, this);
                }
            }
        }

        ///

        /// Crete Absolute url as per the site
        /// 

        ///
        ///
        private static string GetAbsoluteLink(string relativeUrl)
        {
            return HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Host + relativeUrl;
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
        private static bool IncludeItemFromSitemap(Item objItem)
        {
            //Check if the item is having any version
            if (objItem.Versions.Count > 0)
            {
                var excludeItems = Sitecore.Configuration.Settings.GetSetting("IncludedTemplateItem");
                var collection = excludeItems.Split(',').ToList();
                return collection.Contains(objItem.TemplateID.ToString());

            }
            return false;
        }
    }
}
#endregion