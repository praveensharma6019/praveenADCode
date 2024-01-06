namespace Sitecore.Feature.Accounts.Repositories
{
    using Sitecore.Data.Items;
    using Sitecore.Feature.Accounts.Models;
    using Sitecore.Security.Accounts;
    using Sitecore.Data;
    using System.Collections.Generic;
    using System.Data;

    public interface IAccountRepository
    {
        /// <summary>
        ///   Method method changes thepassword for the user to a random string,
        ///   and returns that string.
        /// </summary>
        /// <param name="userName">Username that should have new password</param>
        /// <returns>New generated password</returns>
        string RestorePassword(string userName);
        string RegisterUser(RegistrationInfo registrationInfo, string profileId);
        bool Exists(string userName);
        void Logout();
        User Login(string userName, string password);

        User LoginAPI(string userName, string password);
        Item CreateAccountItem(string accountNumber, string meterNumber);

        ID GetAccountItemId(string AccountNumber);

        bool IsValidUser(string userName, string password);

        User GetUser(string userName, string password);

        void RemoveAccountNumberPermenantly(string itemId);
        List<User> GetUserbyItemId(string itemId);

        User GetUserbyLoginName(string userName);
        User GetUserbyEmail_Account(string Email, string Account);

        User GetUserByUserID(string userID);

        UserStatisticsdata GetUserStatistics(string date);

        UserCAList GetUserCAList(string UserName);
    }
}