using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Project.MiningRenderingHost.Website.Models;
using Sitecore.AspNet.RenderingEngine;
using Sitecore.AspNet.RenderingEngine.Filters;
using Sitecore.LayoutService.Client.Exceptions;
using System.Net;

namespace Project.MiningRenderingHost.Website.Controllers
{
    public class MiningController : Controller
    {
        public MiningController() { }

        [UseSitecoreRendering]
        public IActionResult Index(Sitecore.LayoutService.Client.Response.Model.Route route)
        {
            var request = HttpContext.GetSitecoreRenderingContext();
            if (request.Response != null)
            {
                if (request.Response.HasErrors)
                {
                    foreach (var error in request.Response.Errors)
                    {
                        switch (error)
                        {
                            case ItemNotFoundSitecoreLayoutServiceClientException notFound:
                                Response.StatusCode = (int)HttpStatusCode.NotFound;
                                return View("NotFound", request.Response.Content.Sitecore.Context);
                            case InvalidRequestSitecoreLayoutServiceClientException badRequest:
                            case CouldNotContactSitecoreLayoutServiceClientException transportError:
                            case InvalidResponseSitecoreLayoutServiceClientException serverError:
                            default:
                                throw error;
                        }
                    }
                }               
            }
            return View(route);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();

            return View(new ErrorViewModel
            {
                IsInvalidRequest = exceptionHandlerPathFeature?.Error is InvalidRequestSitecoreLayoutServiceClientException
            });
        }

    }
}
