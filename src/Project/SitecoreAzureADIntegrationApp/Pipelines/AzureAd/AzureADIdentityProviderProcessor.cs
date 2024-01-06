using Microsoft.IdentityModel.Logging;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.OpenIdConnect;
using Owin;
using Sitecore.Configuration;
using Sitecore.Owin.Authentication.Configuration;
using Sitecore.Owin.Authentication.Pipelines.IdentityProviders;
using Sitecore.Owin.Authentication.Services;
using System.Globalization;
using System.Net;

namespace SitecoreAzureADIntegrationApp.Pipelines.AzureAd
{
    public class AzureAdIdentityProviderProcessor : IdentityProvidersProcessor
	{
	    private readonly string _clientId = Settings.GetSetting("ClientId");
	    private readonly string _tenant = Settings.GetSetting("Tenant");
        private readonly string _aadInstance = Settings.GetSetting("AADInstance");
	    private readonly string _redirectUri = Settings.GetSetting("RedirectURI");
        private readonly string _postLogoutUri = Settings.GetSetting("PostLogoutRedirectURI");

        public AzureAdIdentityProviderProcessor(FederatedAuthenticationConfiguration federatedAuthenticationConfiguration) : 
			base(federatedAuthenticationConfiguration)
		{
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            IdentityModelEventSource.ShowPII = true;
        }

        protected override string IdentityProviderName => "AzureAD";

        protected override void ProcessCore(IdentityProvidersArgs args)
		{

            var identityProvider = GetIdentityProvider();
            var authenticationType = GetAuthenticationType();

            args.App.UseOpenIdConnectAuthentication(new OpenIdConnectAuthenticationOptions
            {
                TokenValidationParameters = new System.IdentityModel.Tokens.TokenValidationParameters() 
                { 
                    ValidateAudience = true
                },
                Caption = identityProvider.Caption,
                AuthenticationType = authenticationType,
                AuthenticationMode = AuthenticationMode.Passive,
                ClientId = _clientId,
                Authority = string.Format(CultureInfo.InvariantCulture, _aadInstance, _tenant),
			    RedirectUri = _redirectUri,
			    PostLogoutRedirectUri = _postLogoutUri,
                Notifications = new OpenIdConnectAuthenticationNotifications
			    {
			        SecurityTokenValidated = async context =>
			        {
                        // Get the Ident object from Ticket
                        var identity = context.AuthenticationTicket.Identity;

                        // Use Sitecore Claim Transformation Service to generate additional claims like role or admin
                        foreach (var claimTransformationService in identityProvider.Transformations)
                        {
                            claimTransformationService.Transform(identity,
                                new TransformationContext(FederatedAuthenticationConfiguration, identityProvider));
                        }

                        // Create new Auth Ticket
                        context.AuthenticationTicket = new AuthenticationTicket(identity, context.AuthenticationTicket.Properties);

                        //Returns blank task
                        return;
                    }
                },
                MetadataAddress = "https://login.microsoftonline.com/04c72f56-1848-46a2-8167-8e5d36510cbc/v2.0/.well-known/openid-configuration"
            });;
		}

	}
}
