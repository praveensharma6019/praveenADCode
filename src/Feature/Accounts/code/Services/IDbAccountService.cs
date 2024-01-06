using Sitecore.Feature.Accounts.Models;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sitecore.Feature.Accounts.Services
{
    public interface IDbAccountService
    {
        void RegisterUser(RegistrationInfo registrationInfo);
        bool IsAccountNumberExist(string AccountNumber);
        string GetAccountNumberbyUserName(string username);

        string GetPrimaryAccountNumberbyUserName(string username);

        bool isPrimaryAccountNumberbyUserName(string username);

        void UpdateEmailByUserName(EditProfile model);

        bool CheckAccountInDbPresent(string AcntNo, string UserId);
        void DeleteAndInertBasedOnPrimary(string AcntNo, User UserId);
        #region RegisteredAcccount
        bool isRegisteredAccountPresent(string AcntNo, string MeterNo);
        void NonRegisteredAccountInDb(string AcntNo, string MeterNo);

        void RegisteredAccountInDb(string AcntNo, string MeterNo);
        #endregion

        #region ForgorPassword
        bool isUserEmailAccountPresentForgotPassword(string UserName, string AcntNo, string Email);
        #endregion
        List<UserProfile> GetAccountListbyusername(string username);
        UserProfile GetAccountDetailbyId(string id = null);
        #region Switch Account
        void SwitchAccountAsPrimary(string id = null);
        #endregion
        #region de Registered Account
        bool DeregisterAccount(string id = null);
        #endregion

        #region de Registered Account Revamp
        bool DeregisterAccountRevamp(string id = null);
        #endregion
    }
}
