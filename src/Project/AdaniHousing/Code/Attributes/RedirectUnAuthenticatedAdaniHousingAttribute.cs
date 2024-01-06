namespace Sitecore.AdaniHousing.Website.Attributes
{
    using System.Web.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.AdaniHousing.Website.Models;
    using Sitecore.AdaniHousing.Website.Services;

    public class RedirectUnAuthenticatedAdaniHousingAttribute : ActionFilterAttribute
    {
        private readonly IGetRedirectUrlService getRedirectUrlService;

        public RedirectUnAuthenticatedAdaniHousingAttribute() : this(ServiceLocator.ServiceProvider.GetService<IGetRedirectUrlService>())
        {
        }

        public RedirectUnAuthenticatedAdaniHousingAttribute(IGetRedirectUrlService getRedirectUrlService)
        {
            this.getRedirectUrlService = getRedirectUrlService;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            //string redirectUrl = this.getRedirectUrlService.GetRedirectUrl(AuthenticationStatus.Unauthenticated);

            //var currentVisiterIP = WebAPIAdaniGas.GetIPAddress();
            //if (SessionHelper.UserSession.AdaniGasUserSessionContext != null && SessionHelper.UserSession.AdaniGasUserSessionContext.UserIP != null && currentVisiterIP != null && currentVisiterIP != SessionHelper.UserSession.AdaniGasUserSessionContext.UserIP)
            //{
            //    filterContext.Result = new RedirectResult(redirectUrl);
            //    //SessionHelper.UserSession.AdaniGasUserSessionContext = null;
            //}
            //else
            //{
            //    //string loginType = WebAPIAdaniGas.LoginModuleType(System.Web.HttpContext.Current.Request.Url.AbsolutePath.ToLower());
            //    if (WebAPIAdaniGas.IsUserLoggedIn())
            //        return;
            //    filterContext.Result = new RedirectResult(redirectUrl);
            //}

            //string loginType = WebAPIAdaniGas.LoginModuleType(System.Web.HttpContext.Current.Request.Url.AbsolutePath.ToLower());
            AdaniHousingWebAPIServices webAPIServices = new AdaniHousingWebAPIServices();
            if (webAPIServices.IsUserLoggedIn() && webAPIServices.ValidateCurrentSession())
                return;
            string redirectUrl = this.getRedirectUrlService.GetRedirectUrl(AuthenticationStatus.Unauthenticated);

            filterContext.Result = new RedirectResult(redirectUrl);
        }
    }
}