using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Sitecore.Owin.Authentication.Identity;
using Sitecore.Owin.Authentication.Services;
using Sitecore.SecurityModel.Cryptography;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SitecoreAzureADIntegrationApp.Pipelines.AzureAd
{
    public class ExternalUserBuilder : DefaultExternalUserBuilder
    {
        public ExternalUserBuilder(IHashEncryption hashEncryption) : base(hashEncryption) { }

        protected override string CreateUniqueUserName(UserManager<ApplicationUser> userManager, ExternalLoginInfo externalLoginInfo)
        {
            Sitecore.Diagnostics.Log.Warn("CreateUnique UserName", this);
            try
            {
                string emailAddress = string.Empty;
                if (externalLoginInfo != null && externalLoginInfo.ExternalIdentity != null && externalLoginInfo.ExternalIdentity.Claims != null)
                {
                    foreach (var claim in externalLoginInfo.ExternalIdentity.Claims)
                    {
                        Sitecore.Diagnostics.Log.Info("claim.Type =" + claim.Type + "claim.Value = " + claim.Value, this);
                        if (claim.Type.ToLower() == "preferred_username" && !string.IsNullOrEmpty(claim.Value))
                        {
                            emailAddress = claim.Value;

                        }
                    }

                    if (!string.IsNullOrWhiteSpace(emailAddress))
                    {
                        Sitecore.Diagnostics.Log.Warn("Email address sso" + emailAddress, this);
                        return "sitecore\\" + emailAddress;
                    }
                    else
                    {
                        Sitecore.Diagnostics.Log.Warn("empty Email sso", this);
                        Sitecore.Diagnostics.Log.Warn("default username sso " + externalLoginInfo.DefaultUserName, this);

                        var validUserName = externalLoginInfo.DefaultUserName.Replace(",", "");
                        return "sitecore\\" + validUserName.Replace(" ", "");

                    }
                }
                Sitecore.Diagnostics.Log.Error("null UserInfo when creating UserName", this);


            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("EXternaluserbuilder exception" + ex.ToString(), this);

            }
            return "nullUserInfo";
        }
    }
}