namespace Sitecore.AGELPortal.Website.Services
{
    using Sitecore.Abstractions;
    using Sitecore.AGELPortal.Website.Models;
    using Sitecore.Data;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Dictionary.Repositories;
    using Sitecore.Pipelines.GetSignInUrlInfo;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    [Service(typeof(IFedAuthLoginButtonRepository))]
    public class FedAuthLoginButtonRepository : IFedAuthLoginButtonRepository
    {
        public FedAuthLoginButtonRepository(BaseCorePipelineManager pipelineManager)
        {
            this.PipelineManager = pipelineManager;
            //this.AccountsSettingsService = accountsSettingsService;
        }

        public BaseCorePipelineManager PipelineManager { get; }
        //public IAccountsSettingsService AccountsSettingsService { get; }

        public IEnumerable<FedAuthLoginButton> GetAll()
        {
            //var returnUrl = this.AccountsSettingsService.GetPageLinkOrDefault(Context.Item, Templates.AccountsSettings.Fields.AfterLoginPage);
            var args = new GetSignInUrlInfoArgs(Context.Site.Name, "/agelportal/home/");
            GetSignInUrlInfoPipeline.Run(this.PipelineManager, args);
            if (args.Result == null)
            {
                throw new InvalidOperationException("Could not retrieve federated authentication logins");
            }
            return args.Result.Select(CreateFedAuthLoginButton).ToArray();
        }

        public string  GetAllAAD()
        {
            //Sitecore.Pipelines.CorePipeline.Run("AzureADIdentityProviderProcessor", new Sitecore.Pipelines.PipelineArgs());
            //var returnUrl = this.AccountsSettingsService.GetPageLinkOrDefault(Context.Item, Templates.AccountsSettings.Fields.AfterLoginPage);
            var args = new GetSignInUrlInfoArgs(Context.Site.Name, "/agelportal/home/login");
            GetSignInUrlInfoPipeline.Run(this.PipelineManager, args);

            if (args.Result == null)
            {
                throw new InvalidOperationException("Could not retrieve federated authentication logins");
            }
            if(args.Result.Count > 0 && args.Result.Where(x => x.IdentityProvider == "azureAD").Count() > 0)
            {
                return args.Result.Where(x => x.IdentityProvider == "azureAD").FirstOrDefault().Href;
            }
            else
            {
                return "failed";
            }
           
        }

        private static FedAuthLoginButton CreateFedAuthLoginButton(SignInUrlInfo signInInfo)
        {
            var caption = DictionaryPhraseRepository.Current.Get($"/Accounts/Sign in providers/{signInInfo.IdentityProvider}", $"Sign in with {signInInfo.Caption}");
            string iconClass = null;
            switch (signInInfo.IdentityProvider.ToLower())
            {
                case "facebook":
                    iconClass = "fa fa-facebook";
                    break;
                case "google":
                    iconClass = "fa fa-google";
                    break;
                case "linkedin":
                    iconClass = "fa fa-linkedin";
                    break;
                case "twitter":
                    iconClass = "fa fa-twitter";
                    break;
                default:
                    iconClass = "fa fa-cloud";
                    break;
            }

            return new FedAuthLoginButton
            {
                Provider = signInInfo.IdentityProvider,
                IconClass = iconClass,
                Href = signInInfo.Href,
                Caption = caption,
            };
        }
    }
}