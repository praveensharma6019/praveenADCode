namespace Sitecore.Feature.Accounts.Attributes
{
    using System.Web.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.Feature.Accounts.Models;
    using Sitecore.Feature.Accounts.Services;

    public class RedirectAuthenticatedAttribute : ActionFilterAttribute
    {
        private readonly IGetRedirectUrlService getRedirectUrlService;

        public RedirectAuthenticatedAttribute() : this(ServiceLocator.ServiceProvider.GetService<IGetRedirectUrlService>())
        {
        }

        public RedirectAuthenticatedAttribute(IGetRedirectUrlService getRedirectUrlService)
        {
            this.getRedirectUrlService = getRedirectUrlService;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (!Context.PageMode.IsNormal)
                return;
            if (!Context.User.IsAuthenticated || SessionHelper.UserSession.UserSessionContext == null)
                return;
            var link = this.getRedirectUrlService.GetRedirectUrl(AuthenticationStatus.Authenticated);
            if (SessionHelper.UserSession.UserSessionContext != null && SessionHelper.UserSession.UserSessionContext.userType.ToLower() != UserTypes.Standard.ToLower())
                link = this.getRedirectUrlService.GetRedirectUrl(AuthenticationStatus.PVCAuthenticated);

            filterContext.Result = new RedirectResult(link);
        }
    }
}