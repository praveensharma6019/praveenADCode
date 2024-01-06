using Sitecore.AdaniHousing.Website.Models;
using Sitecore.Foundation.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.AdaniHousing.Website.Services;
using Sitecore.Pipelines.GetSignInUrlInfo;
using Sitecore.Abstractions;

namespace Sitecore.AdaniHousing.Website.Repositories
{
    [Service(typeof(IFedAuthLoginButtonRepository))]
    public class FedAuthLoginButtonRepository:IFedAuthLoginButtonRepository
    {
        public IAccountsSettingsService AccountsSettingsService { get; }
        public BaseCorePipelineManager PipelineManager { get; }
        public IEnumerable<FedAuthLoginButton> GetAll()
        {
            var returnUrl = this.AccountsSettingsService.GetPageLinkOrDefault(Context.Item, Templates.AccountsSettings.Fields.AfterLoginPage);
            var args = new GetSignInUrlInfoArgs(Context.Site.Name, returnUrl);
            GetSignInUrlInfoPipeline.Run(this.PipelineManager, args);
            if (args.Result == null)
            {
                throw new InvalidOperationException("Could not retrieve federated authentication logins");
            }
            return args.Result.Select(CreateFedAuthLoginButton).ToArray();
        }
    }
}