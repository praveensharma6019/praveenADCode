namespace Sitecore.Feature.Accounts.Attributes
{
    using System.Web.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.Feature.Accounts.Models;
    using Sitecore.Feature.Accounts.Services;
    using Sitecore.Feature.Accounts.SessionHelper;

    public class RedirectAuthenticatedComplaintPortalAttribute : ActionFilterAttribute
    {
        private readonly IGetRedirectUrlService getRedirectUrlService;

        public RedirectAuthenticatedComplaintPortalAttribute() : this(ServiceLocator.ServiceProvider.GetService<IGetRedirectUrlService>())
        {
        }

        public RedirectAuthenticatedComplaintPortalAttribute(IGetRedirectUrlService getRedirectUrlService)
        {
            this.getRedirectUrlService = getRedirectUrlService;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (UserSession.AEMLComplaintUserSessionContext != null && UserSession.AEMLComplaintUserSessionContext.IsAuthenticated)
            {
                var link = this.getRedirectUrlService.GetRedirectUrl(AuthenticationStatus.AuthenticatedForComplaint);
                filterContext.Result = new RedirectResult(link);
            }

            return;
        }
    }
}