namespace Sitecore.Feature.Accounts.Attributes
{
    using System.Web.Mvc;
    using Microsoft.Extensions.DependencyInjection;
    using Sitecore.Analytics.Tracking;
    using Sitecore.DependencyInjection;
    using Sitecore.Feature.Accounts.Models;
    using Sitecore.Feature.Accounts.Repositories;
    using Sitecore.Feature.Accounts.Services;
    using Sitecore.Web.Authentication;

    public class RedirectUnauthenticatedAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        private readonly IGetRedirectUrlService getRedirectUrlService;
        private readonly IAccountRepository AccountRepository;
        private readonly IDbAccountService _dbAccountService;
        public RedirectUnauthenticatedAttribute() : this(ServiceLocator.ServiceProvider.GetService<IGetRedirectUrlService>()
            , ServiceLocator.ServiceProvider.GetService<IAccountRepository>(), ServiceLocator.ServiceProvider.GetService<IDbAccountService>()
            )
        {

        }

        public RedirectUnauthenticatedAttribute(IGetRedirectUrlService getRedirectUrlService
            , IAccountRepository accountRepository, IDbAccountService dbAccountService
            )
        {
            this.getRedirectUrlService = getRedirectUrlService;
            this.AccountRepository = accountRepository;
            this._dbAccountService = dbAccountService;
        }

        public void OnAuthorization(AuthorizationContext context)
        {
            if (Context.User != null && !_dbAccountService.isPrimaryAccountNumberbyUserName(Context.User.Profile.UserName))
            {
                this.AccountRepository.Logout();
            }
            else if (Context.User != null && Context.User.IsAuthenticated && Context.User.GetDomainName().ToLower() != "sitecore")
            {
                RegistrationRepository webAPIServices = new RegistrationRepository();
                if (webAPIServices.IsUserLoggedIn() && webAPIServices.ValidateCurrentSession())
                {
                    return;
                }
            }

            //logout
            this.AccountRepository.Logout();
            SessionHelper.UserSession.AEMLComplaintUserSessionContext = null;
            SessionHelper.UserSession.UserSessionContext = null;

            var link = this.getRedirectUrlService.GetRedirectUrl(AuthenticationStatus.Unauthenticated, context.HttpContext.Request.RawUrl);
            context.Result = new RedirectResult(link);

        }
    }
}