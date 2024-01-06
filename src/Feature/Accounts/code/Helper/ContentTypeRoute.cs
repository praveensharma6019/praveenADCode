using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;

namespace Sitecore.Feature.Accounts.Helper
{
    public class ContentTypeRoute : RouteFactoryAttribute
    {
        public ContentTypeRoute(string template, string contentType)
            : base(template)
        {
            ContentType = contentType;
        }

        public override IDictionary<string, object> Constraints
        {
            get
            {
                var constraints = new HttpRouteValueDictionary();
                constraints.Add("Content-Type", new ContentTypeConstraint(ContentType));
                return constraints;
            }
        }

        public string ContentType { get; private set; }
    }

    internal class ContentTypeConstraint : IHttpRouteConstraint
    {
        public ContentTypeConstraint(string allowedMediaType)
        {
            AllowedMediaType = allowedMediaType;
        }

        public string AllowedMediaType { get; private set; }

        public bool Match(HttpRequestMessage request, IHttpRoute route, string parameterName, IDictionary<string, object> values, HttpRouteDirection routeDirection)
        {
            if (routeDirection == HttpRouteDirection.UriResolution)
                return (GetMediaHeader(request) == AllowedMediaType);
            else
                return true;
        }

        private string GetMediaHeader(HttpRequestMessage request)
        {
            IEnumerable<string> headerValues;
            if (request.Content.Headers.TryGetValues("Content-Type", out headerValues) && headerValues.Count() == 1)
                return headerValues.First();
            else
                return "application/x-www-form-urlencoded";
        }
    }
}