using Sitecore.Pipelines.HttpRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;


namespace Adani.SuperApp.Airport.Foundation.ErrorHandling.Platform.ItemNotFoundHandler
{
    public class SetHttpResponseCode : HttpRequestProcessor
    {
        

        public override void Process(HttpRequestArgs args)
        {
            if (CustomContext.PageNotFound)
            {
                HttpContext.Current.Response.StatusCode = (int)HttpStatusCode.NotFound;
            }
        }
    }
}