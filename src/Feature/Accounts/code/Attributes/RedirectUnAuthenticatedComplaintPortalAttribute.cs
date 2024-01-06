namespace Sitecore.Feature.Accounts.Attributes
{
    using System.Web.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.DependencyInjection;
    using Sitecore.Feature.Accounts.Models;
    using Sitecore.Feature.Accounts.Repositories;
    using Sitecore.Feature.Accounts.Services;
    using Sitecore.Feature.Accounts.SessionHelper;

    public class RedirectUnAuthenticatedComplaintPortalAttribute : ActionFilterAttribute
    {
        private readonly IGetRedirectUrlService getRedirectUrlService;
        private readonly IAccountRepository AccountRepository;

        public RedirectUnAuthenticatedComplaintPortalAttribute() : this(ServiceLocator.ServiceProvider.GetService<IGetRedirectUrlService>(), ServiceLocator.ServiceProvider.GetService<IAccountRepository>())
        {
        }

        public RedirectUnAuthenticatedComplaintPortalAttribute(IGetRedirectUrlService getRedirectUrlService
            , IAccountRepository accountRepository
            )
        {
            this.getRedirectUrlService = getRedirectUrlService;
            this.AccountRepository = accountRepository;
        }

        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            if (UserSession.AEMLComplaintUserSessionContext != null && UserSession.AEMLComplaintUserSessionContext.IsAuthenticated && !UserSession.AEMLComplaintUserSessionContext.IsRegistered)
            {
                return;
            }
            else if(UserSession.AEMLComplaintUserSessionContext == null)
            { 
                var link = this.getRedirectUrlService.GetRedirectUrl(AuthenticationStatus.UnAuthenticatedForComplaint);
                filterContext.Result = new RedirectResult(link);
            }

            if (UserSession.AEMLComplaintUserSessionContext == null || (UserSession.AEMLComplaintUserSessionContext != null && !UserSession.AEMLComplaintUserSessionContext.IsAuthenticated))
            {
                //logout
                this.AccountRepository.Logout();
                SessionHelper.UserSession.AEMLComplaintUserSessionContext = null;
                SessionHelper.UserSession.UserSessionContext = null;

                var link = this.getRedirectUrlService.GetRedirectUrl(AuthenticationStatus.UnAuthenticatedForComplaint);
                filterContext.Result = new RedirectResult(link);
            }
            else
            {
                RegistrationRepository webAPIServices = new RegistrationRepository();
                if (UserSession.AEMLComplaintUserSessionContext.IsAdmin || (webAPIServices.IsUserLoggedIn() && webAPIServices.ValidateCurrentSession()))
                {
                    return;
                }
                else
                {
                    //logout
                    this.AccountRepository.Logout();
                    SessionHelper.UserSession.AEMLComplaintUserSessionContext = null;
                    SessionHelper.UserSession.UserSessionContext = null;

                    string redirectUrl = this.getRedirectUrlService.GetRedirectUrl(AuthenticationStatus.UnAuthenticatedForComplaint);
                    filterContext.Result = new RedirectResult(redirectUrl);
                }
            }
        }
    }
}