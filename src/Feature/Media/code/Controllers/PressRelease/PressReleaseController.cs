using Sitecore.Feature.Media.Repositories;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Presentation;
using System.Web.Mvc;

namespace Sitecore.Feature.Media.Controllers.PressRelease
{
    public class PressReleaseController : Controller
    {
        public PressReleaseController(IMediaRepository newsRepository)
        {
            this.Repository = newsRepository;
        }

        private IMediaRepository Repository { get; }

        public ActionResult PressReleaseList()
        {
            var items = this.Repository.Get(RenderingContext.Current.Rendering.Item);
            return this.View("PressReleaseList", items);
        }

        public ActionResult LatestPressRelease()
        {
            //TODO: change to parameter template
            //var count = RenderingContext.Current.Rendering.GetIntegerParameter("count", 10);
            //var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);

            var items = this.Repository.Get(RenderingContext.Current.Rendering.Item);
            return this.View("LatestPressRelease", items);
        }
    }
}