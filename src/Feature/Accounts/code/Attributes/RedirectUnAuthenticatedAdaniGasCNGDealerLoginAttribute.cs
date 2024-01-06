namespace Sitecore.Feature.Accounts.Attributes
{
    using System.Web.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.Feature.Accounts.Models;
    using Sitecore.Feature.Accounts.Services;

    public class RedirectUnAuthenticatedAdaniGasCNGDealerLoginAttribute : ActionFilterAttribute
    {
        private readonly IGetRedirectUrlService getRedirectUrlService;

        public RedirectUnAuthenticatedAdaniGasCNGDealerLoginAttribute() : this(ServiceLocator.ServiceProvider.GetService<IGetRedirectUrlService>())
        {
        }

        public RedirectUnAuthenticatedAdaniGasCNGDealerLoginAttribute(IGetRedirectUrlService getRedirectUrlService)
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
            if (WebAPIAdaniGas.IsCNGDealerUserLoggedIn())
                return;
            string redirectUrl = this.getRedirectUrlService.GetRedirectUrl(AuthenticationStatus.CNGDealerUnauthenticated);

            filterContext.Result = new RedirectResult(redirectUrl);
        }
    }
}