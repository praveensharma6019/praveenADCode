using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Web;

namespace Project.AdaniOneSEO.Website.Processors
{
    public class RedirectProcessor : HttpRequestProcessor
    {
        public override void Process(HttpRequestArgs args)
        {
            try
            {
                string BaseURL = args.RequestUrl.Scheme + "://" + args.RequestUrl.Host;
                string RequestedURL = BaseURL + args.RequestUrl.AbsolutePath;
                if (!RequestedURL.Contains("/sitecore/shell") && !RequestedURL.Contains("/sitecore/client") && !RequestedURL.Contains("sc_mode=edit"))
                {                    
                    string RedirectUrl = GetRedirectItem(RequestedURL);
                    if (RedirectUrl != "Not Found")
                    {
                        Log.Info("Requested URL Found.", "");
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.RedirectPermanent(RedirectUrl, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Project.AdaniOneSEO.Website.Processors : Process Method Fails due to - " + ex.Message, this);
            }
        }
        public string GetRedirectItem(string RequestedURL)
        {
            try
            {
                Log.Info("Project.AdaniOneSEO.Website.Processors: GetRedirectItem Method Starts and Resquest URL is : " + RequestedURL, "");

                Item URLRedirectContextItem = null;
                Sitecore.Data.Database webDBContext = Sitecore.Configuration.Factory.GetDatabase("web");
                #region Fecthing values from Config and Sitecore
                string WebsiteHostID = Sitecore.Configuration.Settings.GetSetting("WebsiteHost");
                Item WebsiteHostContextItem = webDBContext.GetItem(ID.Parse(WebsiteHostID));

                string WebsiteHost = WebsiteHostContextItem?.Fields["WebsiteHost"].Value.ToLower();
                string WebsiteHostRequestedDomain = WebsiteHostContextItem?.Fields["WebsiteHostRequestedDomain"].Value.ToLower();
                string RedirectionDataSource = Sitecore.Configuration.Settings.GetSetting("RedirectionDataSource");
                #endregion

                #region Redirection Validations
                if (!string.IsNullOrEmpty(WebsiteHost) && !string.IsNullOrEmpty(RedirectionDataSource))
                {
                    if (!string.IsNullOrEmpty(WebsiteHostRequestedDomain) && RequestedURL.Contains(WebsiteHostRequestedDomain))
                    {
                        RequestedURL = RequestedURL.Replace(WebsiteHostRequestedDomain, WebsiteHost);
                        Log.Info("Project.AdaniOneSEO.Website.Processors: Requested URL contains : " + RequestedURL, "");
                    }
                    if (RequestedURL.Contains(WebsiteHost))
                    {
                        URLRedirectContextItem = webDBContext.GetItem(ID.Parse(RedirectionDataSource));
                        if (URLRedirectContextItem != null && URLRedirectContextItem.HasChildren)
                        {
                            foreach (Item item in URLRedirectContextItem.GetChildren())
                            {
                                string oldURL = item?.Fields["OldURL"].Value;
                                string newURL = item?.Fields["NewURL"].Value;

                                if (!string.IsNullOrEmpty(oldURL) && !string.IsNullOrEmpty(newURL))
                                {
                                    if (oldURL.ToLower() == RequestedURL.ToLower())
                                    {
                                        Log.Info("Project.AdaniOneSEO.Website.Processors: Requested URL found " + oldURL + " and redirected to " + newURL, "");
                                        return newURL.ToLower();
                                    }

                                }
                            }
                        }
                        else
                        {
                            Log.Info("Project.AdaniOneSEO.Website.Processors: URLRedirectContextItem is Null or doesn't contains any child items.", "");
                        }
                    }
                    else
                    {
                        Log.Info("Project.AdaniOneSEO.Website.Processors: Requested URL " + RequestedURL + " does not match with WebsiteHost " + WebsiteHost, "");
                    }
                }
                else
                {
                    Log.Info("Project.AdaniOneSEO.Website.Processors: App setting values WebsiteHost and RedirectionDataSource not found.", "");
                }
                #endregion

            }
            catch (Exception ex)
            {
                Log.Error("Project.AdaniOneSEO.Website.Processors : GetRedirectItem Method Fails due to - "+ ex.Message,this);
            }
            Log.Info("Requested URL Not found. ", "");
            return "Not Found";
        }
    }
}