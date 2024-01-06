namespace Sitecore.Feature.Navigation.Controllers
{
    using Sitecore.Abstractions;
    using Sitecore.Feature.Navigation.Repositories;
    using Sitecore.Foundation.Alerts.Extensions;
    using Sitecore.Foundation.Alerts.Models;
    using Sitecore.Foundation.Dictionary.Repositories;
    using Sitecore.Foundation.SitecoreExtensions.Attributes;
    using Sitecore.Mvc.Presentation;
    using System.Web.Mvc;

    public class NavigationController : Controller
    {
        private INavigationRepository navigationRepository { get; }
        private BaseCorePipelineManager PipelineManager { get; }

        public NavigationController(INavigationRepository navigationRepository, BaseCorePipelineManager pipelineManager)
        {
            this.navigationRepository = navigationRepository;
            this.PipelineManager = pipelineManager;
        }


        public ActionResult AboutUSNavigation()
        {
            var items = this.navigationRepository.GetAboutUSNavigation();
            return this.View("AboutUSNavigation", items);
        }

        public ActionResult Breadcrumb()
        {
            var items = this.navigationRepository.GetBreadcrumb();
            return this.View("Breadcrumb", items);
        }

        [HttpGet]
        public ActionResult CustomerCare()
        {
            var items = this.navigationRepository.GetCustomerCare();
            return this.View("CustomerCare", items);
        }

        [HttpPost]
        [ValidateRenderingId]
        public JsonResult GetCustomerCare(string itemId)
        {

            return Json(itemId);
        }


        public ActionResult PrimaryMenu()
        {
            var items = this.navigationRepository.GetPrimaryMenu();
            return this.View("PrimaryMenu", items);
        }

        public ActionResult SecondaryMenu()
        {
            var item = this.navigationRepository.GetSecondaryMenuItem();
            return this.View("SecondaryMenu", item);
        }

        public ActionResult NavigationLinks()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View(items);
        }

        public ActionResult LinkMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("LinkMenu", items);
        }

        public ActionResult LinkMenuMobile()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("LinkMenuMobile", items);
        }

        public ActionResult PrimaryLinkMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("PrimaryLinkMenu", items);
        }

        public ActionResult SecondaryLinkMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("SecondaryLinkMenu", items);
        }

        public ActionResult LinkMenuCarousel()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuCarousel", items);
        }

        public ActionResult LinkMenuExploreServices()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuExploreServices", items);
        }


        public ActionResult LinkMenuBar()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuBar", items);
        }


        public ActionResult LinkMenuCommunity()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuCommunity", items);
        }

        public ActionResult LinkMenuFooterContactUs()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuFooterContactUs", items);
        }

        public ActionResult LinkMenuFooterHelpHand()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuFooterHelpHand", items);
        }

        public ActionResult LinkMenuFooterPrimary()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuFooterPrimary", items);
        }

        public ActionResult LinkMenuFooterSecondary()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuFooterSecondary", items);
        }

        public ActionResult PageNavigationLinkMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 1);
            return this.View(items);
        }

        public ActionResult LinkMenuAllAboutEnergy()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 1);
            return this.View(items);
        }
        public ActionResult PaymentNavigationLinkMenu()
        {
            //if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            //{
            //    return null;
            //}
            //var item = RenderingContext.Current.Rendering.Item;
            //var items = this.navigationRepository.GetLinkMenuItems(item, 0, 1);
            //return this.View(items);

            var items = this.navigationRepository.GetPaymentNavigationLinkMenu();
            return this.View("PaymentNavigationLinkMenu", items);
        }

        public ActionResult PageNavigation3Column()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 1);
            return this.View(items);
        }


        public ActionResult MyAccountNavigation()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 1);
            return this.View(items);
        }


        public ActionResult NewUserNavigation()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 1);
            return this.View(items);
        }


        public ActionResult NewsLetterCollection()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 1);
            return this.View(items);
        }

        public ActionResult Gallery()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                // return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View(items);
        }

        #region Realty 

        public ActionResult BreadcrumbRealty()
        {
            var items = this.navigationRepository.GetBreadcrumb();
            return this.View("BreadcrumbRealty", items);
        }
        public ActionResult BreadcrumbBannerRealty()
        {
            var items = this.navigationRepository.GetBreadcrumb();
            return this.View("BreadcrumbBannerRealty", items);
        }

        public ActionResult PrimaryMenuRealty()
        {
            var items = this.navigationRepository.GetPrimaryMenu();
            return this.View("PrimaryMenuRealty", items);
        }
        public ActionResult LinkMenuRealty()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("LinkMenuRealty", items);
        }

        public ActionResult RealtyNavigationBar()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("RealtyNavigationBar", items);
        }

        public ActionResult PrimaryLinkMenuRealty()
        {
            //if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            //{
            //    return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            //}
            var items = this.navigationRepository.GetRealtyMenuItems();
            return this.View("PrimaryLinkMenuRealty", items);
        }

        public ActionResult LinkMenuFooterSecondaryRealty()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 4);
            return this.View("LinkMenuFooterSecondaryRealty", items);
        }


        public ActionResult SocialLinkMenuFooterRealty()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("SocialLinkMenuFooterRealty", items);
        }



        #endregion

        #region australia

        public ActionResult PrimaryMenuAustralia()
        {
            var items = this.navigationRepository.GetPrimaryMenu();
            return this.View("PrimaryMenuAustralia", items);
        }

        public ActionResult LinkMenuAustralia()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("LinkMenuAustralia", items);
        }

        public ActionResult PrimaryLinkMenuAustralia()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("PrimaryLinkMenuAustralia", items);
        }

        //To get "Our Projects & Businesses" on Australia home Page.....
        public ActionResult LinkMenuOurProjectsBusinesses()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuOurProjectsBusinesses", items);
        }

        public ActionResult LinkMenuFooterAustralia()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("LinkMenuFooterAustralia", items);
        }


        public ActionResult BreadcrumbAustralia()
        {
            var items = this.navigationRepository.GetBreadcrumb();
            return this.View("BreadcrumbAustralia", items);
        }

        public ActionResult LinkMenuResourcesAustralia()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 1);
            return this.View("LinkMenuResourcesAustralia", items);
        }

        public ActionResult LinkMenuFooterLearnMore()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuFooterLearnMore", items);
        }

        public ActionResult LinkMenuFooterSecondaryAustralia()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuFooterSecondaryAustralia", items);
        }

        #region PROJECTS & BUINESSESS


        public ActionResult LinkMenuProjectsAndBuiness()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuProjectsAndBuiness", items);
        }


        #endregion

        #region Sustainability

        public ActionResult LinkMenuLearnMoreAustralia()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuLearnMoreAustralia", items);
        }

        #endregion


        #region Rail
        public ActionResult LinkMenuRailNavigationAustralia()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuRailNavigationAustralia", items);
        }
        #endregion

        #region abbot-port
        public ActionResult SingleLinkNavigationAustralia()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("SingleLinkNavigationAustralia", items);
        }
        #endregion

        #region Media-Center
        public ActionResult MediaCenterResourcesAustralia()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 1);
            return this.View("MediaCenterResourcesAustralia", items);
        }

        public ActionResult LinkMenuFooterLeranMoreProjects()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuFooterLeranMoreProjects", items);
        }

        #endregion Career


        public ActionResult LinkMenuCarrerNavigationAustralia()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 1);
            return this.View("LinkMenuCarrerNavigationAustralia", items);
        }

        #region environemt_reporting
        public ActionResult LinkMenuReportingandApprovals()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuReportingandApprovals", items);
        }
        #endregion

        #endregion

        #region Club


        public ActionResult ClubGurgaonSocialLinkMenuFooter()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("ClubGurgaonSocialLinkMenuFooter", items);
        }

        public ActionResult ClubAhmedabadSocialLinkMenuFooter()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("ClubAhmedabadSocialLinkMenuFooter", items);
        }

        public ActionResult ClubAhmedabadHeaderTopLink()
        {
            //var items = this.navigationRepository.GetPrimaryMenu();
            //return this.View("ClubGurgaonHeaderTopLink", items);
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("ClubAhmedabadHeaderTopLink", items);
        }

        public ActionResult ClubAhmedabadTopMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("ClubAhmedabadTopMenu", items);
        }

        public ActionResult ClubAhmedabadHomeTopMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("ClubAhmedabadHomeTopMenu", items);
        }
        public ActionResult ClubGurgaonHeaderTopLink()
        {
            //var items = this.navigationRepository.GetPrimaryMenu();
            //return this.View("ClubGurgaonHeaderTopLink", items);
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("ClubGurgaonHeaderTopLink", items);
        }

        public ActionResult AffordableHeaderTopLink()
        {
            //var items = this.navigationRepository.GetPrimaryMenu();
            //return this.View("ClubGurgaonHeaderTopLink", items);
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("AffordableHeaderTopLink", items);
        }

        public ActionResult AffordableLinkMenuFooterSecondary()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 4);
            return this.View("AffordableLinkMenuFooterSecondary", items);
        }
        public ActionResult AffordableSocialLinkMenuFooter()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("AffordableSocialLinkMenuFooter", items);
        }


        public ActionResult ClubGurgaonHeaderSocialLink()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("ClubGurgaonHeaderSocialLink", items);
        }


        public ActionResult ClubGurgaonTopMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("ClubGurgaonTopMenu", items);
        }

        public ActionResult ClubGurgaonLinkMenuFooterSecondary()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 4);
            return this.View("ClubGurgaonLinkMenuFooterSecondary", items);
        }

        public ActionResult ClubAhmedabadLinkMenuFooterSecondary()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 4);
            return this.View("ClubAhmedabadLinkMenuFooterSecondary", items);
        }
        #endregion

        #region Adani.com

        public ActionResult BreadcrumbAdani()
        {
            var items = this.navigationRepository.GetBreadcrumb();
            return this.View("BreadcrumbAdani", items);
        }
        //public ActionResult AdaniHeaderTopLink()
        //{
        //    if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
        //    {
        //        return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
        //    }
        //    var item = RenderingContext.Current.Rendering.Item;
        //    var items = this.navigationRepository.GetLinkMenuItems(item);
        //    return this.View("AdaniHeaderTopLink", items);
        //}

        public ActionResult AdaniHeaderTopLink()
        {
            var items = this.navigationRepository.GetPrimaryMenu();
            return this.View("AdaniHeaderTopLink", items);
        }

        public ActionResult AdaniHeaderSocialLink()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("AdaniHeaderSocialLink", items);
        }


        public ActionResult AdaniTopMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("AdaniTopMenu", items);
        }

        public ActionResult AdaniFooterLink()
        {
            var items = this.navigationRepository.GetPrimaryMenu();
            return this.View("AdaniFooterLink", items);
        }

        public ActionResult AdaniLinkMenuFooterSecondary()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 4);
            return this.View("AdaniLinkMenuFooterSecondary", items);
        }


        public ActionResult AdaniSocialLinkMenuFooter()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("AdaniSocialLinkMenuFooter", items);
        }

        public ActionResult AdaniNavigationBar()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("AdaniNavigationBar", items);
        }


        public ActionResult LinkMenuResourcesAdani()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 1);
            return this.View("LinkMenuResourcesAdani", items);
        }
        #endregion

        #region Affordable


        public ActionResult AffordableNavigationBar()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("AffordableNavigationBar", items);
        }

        public ActionResult AffordableBreadcrumb()
        {
            var items = this.navigationRepository.GetBreadcrumb();
            return this.View("AffordableBreadcrumb", items);
        }

        #endregion

        #region Ports

        public ActionResult PortsBannerNewsHome()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("PortsBannerNewsHome", items);
        }
        public ActionResult PortsNewsUpdatesHome()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("PortsNewsUpdatesHome", items);
        }
        public ActionResult PortsNewsroomMediaButtons()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("PortsNewsroomMediaButtons", items);
        }

        public ActionResult PrimaryMenuPorts()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("PrimaryMenuPorts", items);
        }

        public ActionResult PortsPrimaryMenu()
        {
            var items = this.navigationRepository.GetPrimaryMenu();
            return this.View("PortsPrimaryMenu", items);
        }

        public ActionResult PortsPrimaryLinkMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("PortsPrimaryLinkMenu", items);
        }

        public ActionResult PortsFooterBody()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 4);
            return this.View("PortsFooterBody", items);
        }


        public ActionResult PortsFooterSocialLink()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("PortsFooterSocialLink", items);
        }

        public ActionResult PortsCorporatePolicies()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("PortsCorporatePolicies", items);
        }

        public ActionResult PortsCorporateDownloadLink()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("PortsCorporateDownloadLink", items);
        }





        #endregion

        #region CGRF

        public ActionResult CGRFHeaderLink()
        {
            var items = this.navigationRepository.GetPrimaryMenu();
            return this.View("CGRFHeaderLink", items);
        }

        public ActionResult CGRFLeftPenalNavigation()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("CGRFLeftPenalNavigation", items);
        }

        #endregion

        #region AdaniGas

        public ActionResult BreadcrumbAdaniGas()
        {
            var items = this.navigationRepository.GetBreadcrumb();
            return this.View("BreadcrumbAdaniGas", items);
        }

        public ActionResult BreadcrumbWithBannerAdaniGas()
        {
            var items = this.navigationRepository.GetBreadcrumb();
            return this.View("BreadcrumbWithBannerAdaniGas", items);
        }

        public ActionResult BreadcrumbWithBigBannerAdaniGas()
        {
            var items = this.navigationRepository.GetBreadcrumb();
            return this.View("BreadcrumbWithBigBannerAdaniGas", items);
        }

        public ActionResult LinkMenuAdaniGas()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("LinkMenuAdaniGas", items);
        }

        public ActionResult PrimaryMenuAdaniGas()
        {
            var items = this.navigationRepository.GetPrimaryMenu();
            return this.View("PrimaryMenuAdaniGas", items);
        }

        public ActionResult PrimaryLinkMenuAdaniGas()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("PrimaryLinkMenuAdaniGas", items);
        }

        public ActionResult LinkMenuFooterSecondaryAdaniGas()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 4);
            return this.View("LinkMenuFooterSecondaryAdaniGas", items);
        }

        public ActionResult SecondaryLinkMenuAdaniGas()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("SecondaryLinkMenuAdaniGas", items);
        }

        public ActionResult LinkMenuCarouselAdaniGas()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("LinkMenuCarouselAdaniGas", items);
        }
        
        [AllowAnonymous]
        public ActionResult GasQuickLinks()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("GasQuickLinks", items);
        }

        [AllowAnonymous]
        public ActionResult AdaniGasPNGTypeQuickLinks()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item, 0, 2);
            return this.View("AdaniGasPNGTypeQuickLinks", items);
        }
        #endregion

        #region Marathon

        public ActionResult MarathonPrimaryMenu()
        {
            var items = this.navigationRepository.GetPrimaryMenu();
            return this.View("MarathonPrimaryMenu", items);
        }

        public ActionResult MarathonLinkMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("MarathonLinkMenu", items);
        }

        public ActionResult MarathonNavigationLinks()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("MarathonNavigationLinks", items);
        }
        public ActionResult MarathonLinkMenu1()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("MarathonLinkMenu1", items);
        }

        #endregion


        #region
        public ActionResult AdaniSolarHeaderTopLink()
        {
            var items = this.navigationRepository.GetPrimaryMenu();
            return this.View("~/Views/Navigation/AdaniSolar/AdaniSolarHeaderTopLink.cshtml", items);
        }
        public ActionResult AdaniSolarTopMenu()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return Context.PageMode.IsExperienceEditor ? this.InfoMessage(new InfoMessage(DictionaryPhraseRepository.Current.Get("/Navigation/Link Menu/No Items", "This menu has no items."), InfoMessage.MessageType.Warning)) : null;
            }
            var item = RenderingContext.Current.Rendering.Item;
            var items = this.navigationRepository.GetLinkMenuItems(item);
            return this.View("~/Views/Navigation/AdaniSolar/AdaniSolarTopMenu.cshtml", items);
        }
        #endregion

        #region Careers


        public ActionResult CareersPrimaryMenu()
        {
            var items = this.navigationRepository.GetPrimaryMenu();
            return this.View("CareersPrimaryMenu", items);
        }

        #endregion
    }
}