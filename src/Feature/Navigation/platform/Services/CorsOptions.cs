using Sitecore.Pipelines.PreprocessRequest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.Services
{
    public class CorsOptions : PreprocessRequestProcessor
    {
        public override void Process(PreprocessRequestArgs args)
        {
            var request = args.HttpContext.Request;
            var response = args.HttpContext.Response;
            if (request.Headers.AllKeys.Contains("Origin", StringComparer.InvariantCultureIgnoreCase) && (request.RawUrl.Contains("api/sitecore/Header/postSfdcWrapper") || request.RawUrl.Contains("api/sitecore/Header/POSTOTP")))
            {
                if (request.HttpMethod == "OPTIONS")
                {
                    response.AddHeader("X-Fabricated-By-Adani", "Adani Realty");
                    response.AddHeader("Access-Control-Allow-Origin", "*");
                    response.Headers.Remove("Server");
                    response.Flush();
                }
                if (request.HttpMethod == "POST")
                {
                    response.AddHeader("Access-Control-Allow-Origin", "*");
                    response.AddHeader("Access-Control-Allow-Credentials", "true");
                    response.AddHeader("Strict-Transport-Security", "max-age=31536000; includeSubdomains;preload");
                    response.AddHeader("Referrer-Policy", "strict-origin-when-cross-origin");
                    response.Headers.Remove("Server");
                }
            }
        }
    }
}