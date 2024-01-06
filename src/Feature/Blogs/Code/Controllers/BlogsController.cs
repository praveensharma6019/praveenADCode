namespace Sitecore.Feature.Blogs.Controllers
{
    using System.Web.Mvc;
    using Sitecore.Feature.Blogs.Repositories;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Mvc.Presentation;
    using System.Linq;
    using System.IO;

    public class BlogsController : Controller
    {
        public BlogsController(IBlogsRepository newsRepository)
        {
            this.Repository = newsRepository;
        }

        private IBlogsRepository Repository { get; }

        public ActionResult BlogList()
        {
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);
            return this.View("BlogList", items);
        }

        public ActionResult BlogDisplay()
        {
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            var items = this.Repository.GetCurrentItem(RenderingContext.Current.Rendering.Item);
            return this.View("BlogDisplay", items);
        }
        public ActionResult FeaturedBlogs()
        {
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            var items = this.Repository.GetFeaturedBlog(RenderingContext.Current.Rendering.Item);
            return this.View("FeaturedBlogs", items);
        }

        public ActionResult ClubAhmedabadEvents()
        {
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            var items = this.Repository.GetFeaturedBlog(RenderingContext.Current.Rendering.Item);
            return this.View("ClubAhmedabadEvents", items);
        }

        #region Adani.com ImpactStory

        public ActionResult AdaniStoryList()
        {
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);
            return this.View("AdaniStoryList", items);
        }

        public ActionResult AdaniStoryDisplay()
        {
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            var items = this.Repository.GetCurrentItem(RenderingContext.Current.Rendering.Item);
            return this.View("AdaniStoryDisplay", items);
        }

        public ActionResult AdaniFeaturedStory()
        {
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            var items = this.Repository.GetFeaturedstory(RenderingContext.Current.Rendering.Item);
            return this.View("AdaniFeaturedStory", items);
        }
        #endregion

        #region Affordable

        public ActionResult AffordableBlogDisplay()
        {
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            var items = this.Repository.GetCurrentItem(RenderingContext.Current.Rendering.Item);
            return this.View("AffordableBlogDisplay", items);
        }
        #endregion

        #region Ports
        public ActionResult PortsBlogList()
        {
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);
            return this.View("PortsBlogList", items);
        }
        public ActionResult PortsMediaNewsList()
        {
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);
            return this.View("PortsMediaNewsList", items);
        }

        public ActionResult PortsBlogMainPageList()
        {
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);
            return this.View("PortsBlogMainPageList", items);
        }
        public ActionResult PortsSustainabilityContent()
        {
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);
            return this.View("PortsSustainabilityContent", items);
        }
        public ActionResult PortsSustainabilityInnerContent()
        {
            var count = RenderingContext.Current.Rendering.DataSource;
            var items = Context.Database.GetItem(count);
            string pagename = Path.GetFileName(Request.Url.AbsolutePath);
            var filterItem = items.Children.Where(i => !items.DisplayName.Contains(pagename));
            return this.View("PortsSustainabilityInnerContent", filterItem);
        }


        #endregion
        
        #region AdaniGas
       
        public ActionResult AdaniGasSustainabilityContent()
        {
            var count = RenderingContext.Current.Rendering.GetIntegerParameter("count");
            var items = this.Repository.GetLatest(RenderingContext.Current.Rendering.Item, count);
            return this.View("AdaniGasSustainabilityContent", items);
        }
        public ActionResult AdaniGasSustainabilityInnerContent()
        {
            var count = RenderingContext.Current.Rendering.DataSource;
            var items = Context.Database.GetItem(count);
            string pagename = Path.GetFileName(Request.Url.AbsolutePath);
            var filterItem = items.Children.Where(i => !items.DisplayName.Contains(pagename));
            return this.View("AdaniGasSustainabilityContent", filterItem);
        }


        #endregion
    }
}