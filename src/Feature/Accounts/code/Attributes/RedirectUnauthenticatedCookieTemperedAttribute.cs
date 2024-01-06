using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Feature.Accounts.Helper;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Feature.Accounts.Repositories;
using Sitecore.Feature.Accounts.Services;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace Sitecore.Feature.Accounts.Attributes
{
    public class RedirectUnauthenticatedCookieTemperedAttribute : ActionFilterAttribute, IAuthorizationFilter
    {
        private readonly IGetRedirectUrlService getRedirectUrlService;
        private readonly IAccountRepository AccountRepository;
        private readonly IDbAccountService _dbAccountService;
        public RedirectUnauthenticatedCookieTemperedAttribute() : this(ServiceLocator.ServiceProvider.GetService<IGetRedirectUrlService>()
            , ServiceLocator.ServiceProvider.GetService<IAccountRepository>(), ServiceLocator.ServiceProvider.GetService<IDbAccountService>()
            )
        {

        }

        public RedirectUnauthenticatedCookieTemperedAttribute(IGetRedirectUrlService getRedirectUrlService
            , IAccountRepository accountRepository, IDbAccountService dbAccountService
            )
        {
            this.getRedirectUrlService = getRedirectUrlService;
            this.AccountRepository = accountRepository;
            this._dbAccountService = dbAccountService;
        }

        public void OnAuthorization(AuthorizationContext context)
        {
            string contextUserName = Context.User != null && Context.User.GetLocalName() != null ? Context.User.GetLocalName() : string.Empty;

            if (Context.User != null && !_dbAccountService.isPrimaryAccountNumberbyUserName(Context.User.Profile.UserName))
            {
                this.AccountRepository.Logout();
                RegistrationRepository rp1 = new RegistrationRepository();
                rp1.DeleteSessionAfterLogout();
                SessionHelper.UserSession.AEMLComplaintUserSessionContext = null;
                SessionHelper.UserSession.UserSessionContext = null;
                HttpContext.Current.ClearSession();
            }
            else if (Context.User != null && Context.User.IsAuthenticated && Context.User.GetDomainName().ToLower() != "sitecore" && SessionHelper.UserSession.UserSessionContext != null && contextUserName == SessionHelper.UserSession.UserSessionContext.UserName)
            {
                RegistrationRepository webAPIServices = new RegistrationRepository();
                if (webAPIServices.IsUserLoggedIn() && webAPIServices.ValidateCurrentSession())
                {
                    return;
                }
            }

            //logout
            this.AccountRepository.Logout();
            RegistrationRepository rp = new RegistrationRepository();
            rp.DeleteSessionAfterLogout();
            SessionHelper.UserSession.AEMLComplaintUserSessionContext = null;
            SessionHelper.UserSession.UserSessionContext = null;
            HttpContext.Current.ClearSession();
            //HttpContext.Current.Response.Cookies.Add(new HttpCookie("ASP.NET_SessionId", HttpContext.Current.Request.Browser.ToHashString()) { HttpOnly = true, Secure = true });

            var link = this.getRedirectUrlService.GetRedirectUrl(AuthenticationStatus.Unauthenticated, context.HttpContext.Request.RawUrl);
            context.Result = new RedirectResult(link);

        }

        private string GenerateHashKey()
        {
            StringBuilder myStr = new StringBuilder();
            myStr.Append(HttpContext.Current.Request.Browser.Browser);
            myStr.Append(HttpContext.Current.Request.Browser.Platform);
            myStr.Append(HttpContext.Current.Request.Browser.MajorVersion);
            myStr.Append(HttpContext.Current.Request.Browser.MinorVersion);
            //myStr.Append(Request.LogonUserIdentity.User.Value);
            byte[] hashdata = Encoding.UTF8.GetBytes(myStr.ToString());
            return System.Convert.ToBase64String(hashdata);
        }
    }
}