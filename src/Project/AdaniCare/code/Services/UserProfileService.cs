namespace Sitecore.AdaniCare.Website
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Foundation.Dictionary.Repositories;
    using Sitecore.Security;
    using Sitecore.Security.Accounts;
    using Sitecore.SecurityModel;
    using Sitecore.Configuration;
    using Sitecore.Foundation.SitecoreExtensions.Extensions;
    using Sitecore.Links;
    using System.IO;
    using Sitecore.Resources.Media;
    using Sitecore.Diagnostics;

    [Service(typeof(IUserProfileService))]
    public class UserProfileService : IUserProfileService
    {
        public string GetPageURL(ID itemId)
        {
            var ItemId = Context.Database.GetItem(itemId);
            return ItemId.Url();
        }
    }
}