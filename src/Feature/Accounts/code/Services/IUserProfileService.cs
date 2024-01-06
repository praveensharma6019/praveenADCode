namespace Sitecore.Feature.Accounts.Services
{
    using System.Collections.Generic;
    using System.Web;
    using System.Web.Mvc;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Feature.Accounts.Models;
    using Sitecore.Security;
    using Sitecore.Security.Accounts;

    public interface IUserProfileService
    {
        string GetUserDefaultProfileId();
        EditProfile GetEmptyProfile();
        EditProfile GetProfile(User user);

        string GetAccountNumber(User user);
        void SaveProfile(UserProfile userProfile, EditProfile model);

        void SaveEmailAlerts(UserProfile userProfile, EditProfile model);

        void SaveSMSAlerts(UserProfile userProfile, EditProfile model);

        void SavePaperlessBillingFlag(UserProfile userProfile, EditProfile model);

        IEnumerable<string> GetInterests();
        string GetAccountNumberfromItem(string accountItemId);
        Item GetAccountItem(string accountItemId);
        string GetLoginName();

        string GetPageURL(ID itemId);
        string HostURL();
        Item InsertAttachmentURL(byte[] ProfilePicture,string FileName);
    }
}