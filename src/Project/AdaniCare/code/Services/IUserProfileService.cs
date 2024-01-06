namespace Sitecore.AdaniCare.Website
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Security;
    using Sitecore.Security.Accounts;

    public interface IUserProfileService
    {
        string GetPageURL(ID itemId);
    }
}