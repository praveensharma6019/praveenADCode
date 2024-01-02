using System.IO;
using System.Web;

namespace Sitecore.Feature.SEO.Pipelines
{
    public class SitemapXmlHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context != null)
            {
                if (string.Compare(context.Request.Url.PathAndQuery, "/sitemap.xml", true) == 0)
                {
                    HttpResponse response = context.Response;
                    response.Clear();
                    response.ContentType = "application/xml";

                    string currentHost = context.Request.Url.Host;
                    string currentHostSitemapFileName = string.Format("sitemap-{0}.xml", currentHost);
                    string hostMatchFileName = context.Server.MapPath("sitemaps/" + currentHostSitemapFileName);
                    string defaultFileName = context.Server.MapPath("sitemap.xml");

                    if (File.Exists(hostMatchFileName))
                    {
                        // found a host match between the request's host and a file on the hard drive

                        response.Write(File.ReadAllText(hostMatchFileName));
                    }
                    else if (File.Exists(defaultFileName))
                    {
                        // found /robots.txt at the root.

                        response.Write(File.ReadAllText(defaultFileName));
                    }
                    else
                    {
                        response.Write("Not Found");
                    }

                    response.Flush();
                    response.End();
                }
            }
        }

        public bool IsReusable
        {
            get { return false; }
        }
    }
}