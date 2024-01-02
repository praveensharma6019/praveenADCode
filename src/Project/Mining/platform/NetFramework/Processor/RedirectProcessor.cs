using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Mvc.Processing.SubmitActions;
using Sitecore.Pipelines.HttpRequest;
using System;
using System.Security.Policy;
using System.Web;

namespace Project.Mining.Website.Processor
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
                        Log.Info("Requested URL Found. Redirected to " + RedirectUrl, "");
                        HttpContext.Current.Response.Status = "301 Moved Permanently";
                        HttpContext.Current.Response.RedirectPermanent(RedirectUrl, false);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Project.Mining.Website.Processor : Process Method Fails due to - " + ex.Message, this);
            }
        }
        public string GetRedirectItem(string RequestedURL)
        {
            try
            {
                Log.Info("GetRedirectItem Method Starts and Resquest URL is : " + RequestedURL, "");
                string WebsiteHostID = Sitecore.Configuration.Settings.GetSetting("WebsiteHost");
                Item WebsiteHostContextItem = Sitecore.Context.Database.GetItem(ID.Parse(WebsiteHostID));
                string WebsiteHost = WebsiteHostContextItem?.Fields["APIEndpoint"].Value;
                string RedirectHost = WebsiteHostContextItem?.Fields["RedirectHost"].Value;
                if (!string.IsNullOrEmpty(WebsiteHost) && !string.IsNullOrEmpty(RedirectHost))
                {
                    bool isUpper = false;
                    string[] hostValues = WebsiteHost.Split('|');
                    foreach (string host in hostValues)
                    {
                        if (RequestedURL.ToLower().Contains(host.ToLower()))
                        {
                            RequestedURL = RequestedURL.Replace(host, RedirectHost);
                        }

                    }
                    if (RequestedURL.ToLower().Contains(RedirectHost))
                    { 
                        foreach (char c in RequestedURL)
                        {
                            if (char.IsUpper(c))
                            {
                                Log.Info("Requested URL has upper case values", "");
                                isUpper = true;
                                break;
                            }
                        }
                        if (isUpper)
                        {
                            return RequestedURL.ToLower();
                        }
                        else
                        {
                            Log.Info("Requested URL not match with Sitecore Website Host value", "");
                            return "Not Found";
                        }
                    }
                   
                   
                }

            }
            catch (Exception ex)
            {
                Log.Error("Project.Mining.Website.Processor : GetRedirectItem Method Fails due to - "+ ex.Message,this);
            }
            Log.Info("Requested URL Not found. ", "");
            return "Not Found";
        }
    }
}