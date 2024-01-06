using System.IO;
using System.Web;

namespace Sitecore.Feature.SEO.Pipelines
{
    public class RobotsTextHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            if (context != null)
            {
                if (string.Compare(context.Request.Url.PathAndQuery, "/robots.txt", true) == 0)
                {
                    HttpResponse response = context.Response;
                    response.Clear();
                    response.ContentType = "text/plain";

                    string currentHost = context.Request.Url.Host;
                    string currentHostRobotsFileName = string.Format("robots-{0}.txt", currentHost);
                    string hostMatchFileName = context.Server.MapPath("robots/" + currentHostRobotsFileName);
                    string defaultFileName = context.Server.MapPath("robots.txt");

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
                        response.Write("User-agent: *\nDisallow: /");
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