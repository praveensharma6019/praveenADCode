namespace Sitecore.Feature.News.Controllers
{
    using System.Web.Mvc;
    using Sitecore.Feature.News.Repositories;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Mvc.Presentation;

    public class NewsController : Controller
    {
        public NewsController(INewsRepository newsRepository)
        {
            this.Repository = newsRepository;
        }

        private INewsRepository Repository { get; }

        public ActionResult NewsList()
        {
            var items = RenderingContext.Current.Rendering.Item.Axes.GetDescendants();
            //var count = RenderingContext.Current.Rendering.GetIntegerParameter("count", 4);
            //var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);
            //var items = this.Repository.Get(RenderingContext.Current.Rendering.Item);
            return this.View("NewsList", items);
        }

        public ActionResult LatestNews()
        {
            //TODO: change to parameter template
            //var count = RenderingContext.Current.Rendering.GetIntegerParameter("count", 5);
            //var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);

            var items = RenderingContext.Current.Rendering.Item.Axes.GetDescendants();
            return this.View("LatestNews", items);
        }

        #region australia

        public ActionResult NewsListAustralia()
        {
            var item = RenderingContext.Current.Rendering.Item.Axes.GetDescendants();
            //var count = RenderingContext.Current.Rendering.GetIntegerParameter("count", 3);
            //var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);
            return this.View("NewsListAustralia", item);
        }


        public ActionResult LatestNewsAustralia()
        {
            var items = RenderingContext.Current.Rendering.Item.Axes.GetDescendants();
            //var count = RenderingContext.Current.Rendering.GetIntegerParameter("count", 8);
            //var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);
            return this.View("LatestNewsAustralia", items);
        }


        public ActionResult GetAllNewsAustralia()
        {
            //TODO: change to parameter template
            var items = RenderingContext.Current.Rendering.Item.Axes.GetDescendants();
            return this.View("GetAllNewsAustralia", items);
        }
        #endregion


        #region Realty

        public ActionResult LatestNewsRealty()
        {
            var items = RenderingContext.Current.Rendering.Item.Axes.GetDescendants();
            //var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            //var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);
            return this.View("LatestNewsRealty", items);
        }

        #endregion


        #region Adani.com

        public ActionResult LatestNewsAdanicom()
        {
            var items = RenderingContext.Current.Rendering.Item.Axes.GetDescendants();
            return this.View("LatestNewsAdanicom", items);
        }

        #endregion
    }
}