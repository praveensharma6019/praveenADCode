using Sitecore.Diagnostics;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Foundation.DependencyInjection;
using Sitecore.Security.Accounts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Sitecore.Feature.Accounts.Services
{
    [Service(typeof(IDbAccountService))]
    public class DbAccountService : IDbAccountService
    {
        //PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext();

        public void RegisterUser(RegistrationInfo registrationInfo)
        {
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                dbcontext.CommandTimeout = 0;
                var fullName = Sitecore.Context.Site.Domain.GetFullName(registrationInfo.LoginName);
                UserProfile entity = new UserProfile()
                {
                    Id = Guid.NewGuid(),
                    UserName = fullName,
                    Email = registrationInfo.Email,
                    Mobile = registrationInfo.MobileNumber,
                    AccountNumber = registrationInfo.AccountNo,
                    MeterNumber = registrationInfo.MeterNo,
                    IsPrimary = true,
                    status = true,
                    Created_Date = DateTime.Now,
                    CreatedBy = fullName
                };
                dbcontext.UserProfiles.InsertOnSubmit(entity);
                dbcontext.SubmitChanges();
            }
           
        }
        
        public bool IsAccountNumberExist(string accountNumber)
        {
            bool IsExist = false;
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                if (!string.IsNullOrEmpty(accountNumber))
                {
                    IsExist = dbcontext.UserProfiles.Any(x => x.AccountNumber == accountNumber && x.status == true);
                }
            }
            return IsExist;
        }

        public string GetAccountNumberbyUserName(string username)
        {
            string AccountNumber = string.Empty;
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                var user = dbcontext.UserProfiles.FirstOrDefault(x => x.UserName == username && x.IsPrimary == true);
                AccountNumber = user != null ? user.AccountNumber : string.Empty;
            }
            return AccountNumber;
        }

        public string GetPrimaryAccountNumberbyUserName(string username)
        {
            string AccountNumber = string.Empty;
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                var user = dbcontext.UserProfiles.FirstOrDefault(x => x.UserName == username && x.IsPrimary == true);
                 AccountNumber = user != null ? user.AccountNumber : string.Empty;
            }
            return AccountNumber;
        }

        public bool isPrimaryAccountNumberbyUserName(string username) // Need to check this condition
        {
            bool AccountNumber = false;
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                var user = dbcontext.UserProfiles.FirstOrDefault(x => x.UserName == username && x.IsPrimary == true);
                AccountNumber = user != null && !string.IsNullOrEmpty(user.AccountNumber) ? true : !string.IsNullOrEmpty(Context.User.Profile.GetCustomProperty("Primary Account No")) ? true : false;
               
            }
            return AccountNumber;
        }

        /*Update customDB profile
         * args: Email, MobileNumber
         */
        public void UpdateEmailByUserName(EditProfile model)
        {
            var userName = Context.User.Profile.UserName;
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                dbcontext.CommandTimeout = 0;
                var userProfileData = dbcontext.UserProfiles.Where(x => x.UserName == userName).ToList();
                if (userProfileData != null && userProfileData.Count > 0)
                {
                    foreach (var user in userProfileData)
                    {
                        user.Email = model.Email;
                        user.Mobile = model.MobileNumber;
                        user.ModifiedBy = userName;
                        user.Modified_Date = DateTime.Now;
                    }
                    dbcontext.SubmitChanges();
                }
            }

        }
        public bool CheckAccountInDbPresent(string AcntNo, string UserId)
        {
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                if (dbcontext.UserProfiles.Any(x => x.UserName == UserId && x.AccountNumber == AcntNo))
                {
                    return true;
                }
            }
            return false;
        }
        public void DeleteAndInertBasedOnPrimary(string AcntNo, User UserDetails)
        {
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                var data = dbcontext.UserProfiles.Where(x => x.UserName == UserDetails.Profile.UserName && x.AccountNumber == AcntNo).FirstOrDefault();

                bool accountprimary = data != null && data.IsPrimary == true ? true : false;
                var userProfile = Context.User.Profile;
                UserProfile user = new UserProfile()
                {
                    Id = Guid.NewGuid(),

                    UserName = userProfile.UserName,
                    Email = userProfile.Email,
                    AccountNumber = data.AccountNumber,
                    Mobile = userProfile.GetCustomProperty("Mobile Number"),
                    MeterNumber = data.MeterNumber,
                    IsPrimary = false,
                    status = true,
                    Created_Date = DateTime.Now,
                    CreatedBy = Context.User.Profile.UserName,
                };
                dbcontext.UserProfiles.InsertOnSubmit(user);
                dbcontext.UserProfiles.DeleteOnSubmit(data);
                dbcontext.SubmitChanges();

                if (accountprimary && dbcontext.UserProfiles.Any(x => x.UserName == UserDetails.Profile.UserName))
                {
                    var userdetails = dbcontext.UserProfiles.Where(x => x.UserName == UserDetails.Profile.UserName).FirstOrDefault();
                    userdetails.IsPrimary = true;
                    userdetails.Modified_Date = DateTime.Now;
                    userdetails.ModifiedBy = Context.User.Profile.UserName;
                    dbcontext.SubmitChanges();
                }
                else if (accountprimary && !dbcontext.UserProfiles.Any(x => x.UserName == UserDetails.Profile.UserName))
                {
                    UserDetails.Delete();
                }
            }

        }

        #region NonRegisteredAccountIntoDb
        public void NonRegisteredAccountInDb(string AcntNo, string MeterNo)
        {
            var userDetail = Context.User.Profile;
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                UserProfile user = new UserProfile()
                {
                    Id = Guid.NewGuid(),
                    UserName = userDetail.UserName,
                    Email = userDetail.Email,
                    Mobile = userDetail.GetCustomProperty("Mobile Number"),
                    AccountNumber = AcntNo,
                    MeterNumber = MeterNo,
                    IsPrimary = false,
                    status = true,
                    Created_Date = DateTime.Now,
                    CreatedBy = userDetail.UserName
                };
                dbcontext.UserProfiles.InsertOnSubmit(user);
                dbcontext.SubmitChanges();
            }
        }

        public void RegisteredAccountInDb(string AcntNo, string MeterNo)
        {
            var userDetail = Context.User.Profile;
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                UserProfile user = new UserProfile()
                {
                    Id = Guid.NewGuid(),
                    UserName = userDetail.UserName,
                    Email = userDetail.Email,
                    Mobile = userDetail.GetCustomProperty("Mobile Number"),
                    AccountNumber = AcntNo,
                    MeterNumber = MeterNo,
                    IsPrimary = true,
                    status = true,
                    Created_Date = DateTime.Now,
                    CreatedBy = userDetail.UserName
                };
                dbcontext.UserProfiles.InsertOnSubmit(user);
                dbcontext.SubmitChanges();
            }
        }

        public bool isRegisteredAccountPresent(string AcntNo, string MeterNo)
        {
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                var isPresent = dbcontext.UserProfiles.Any(x => x.UserName == Context.User.Profile.UserName && x.AccountNumber == AcntNo && x.MeterNumber == MeterNo && x.status == true);

                return isPresent;
            }
        }
        #endregion

        #region ChangePassword
        public bool isUserEmailAccountPresentForgotPassword(string UserName, string AcntNo, string Email)
        {
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                dbcontext.CommandTimeout = 0;
                var isPresent = dbcontext.UserProfiles.Any(x => x.UserName == UserName && x.AccountNumber == AcntNo && x.Email == Email && x.status == true);

                return isPresent;
            }
        }
        #endregion
        public List<UserProfile> GetAccountListbyusername(string username)
        {
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                var accountlist = from ac in dbcontext.UserProfiles where ac.UserName == username select ac;
                return accountlist.ToList();
            }
        }

        public UserProfile GetAccountDetailbyId(string id = null)
        {
            Guid resultId;
            if (id != null)
            {
                bool status = Guid.TryParse(id, out resultId);
                if (status)
                {
                    using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                    {
                        var accountdetail = from ac in dbcontext.UserProfiles where ac.Id == resultId select ac;
                        return accountdetail.FirstOrDefault();
                    }
                }
            }
            return null;
        }

        #region Switch Account
        public void SwitchAccountAsPrimary(string id = null)
        {
            Guid resultId;
            var username = Context.User.Profile.UserName;
            using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
            {
                var accountlist = from ac in dbcontext.UserProfiles where ac.UserName == username select ac;
                bool status = Guid.TryParse(id, out resultId);
                if (status)
                {
                    foreach (var item in accountlist.ToList())
                    {
                        if (item.Id == resultId)
                        {
                            item.IsPrimary = true;
                            item.ModifiedBy = username;
                            item.Modified_Date = DateTime.Now;
                        }
                        else
                        {
                            item.IsPrimary = false;
                            item.ModifiedBy = username;
                            item.Modified_Date = DateTime.Now;
                        }
                        dbcontext.SubmitChanges();
                    }
                }
            }

        }
        #endregion

        #region De Registered Account
        public bool DeregisterAccount(string id = null)
        {
            Guid resultId;
            bool status = false;
            bool guidconversion = Guid.TryParse(id, out resultId);
            if (guidconversion)
            {
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    var accountlist = from ac in dbcontext.UserProfiles
                                      where ac.Id == resultId
                                      select ac;
                    if (accountlist != null && accountlist.Any())
                    {
                        var entity = accountlist.FirstOrDefault();
                        dbcontext.UserProfiles.DeleteOnSubmit(entity);
                        dbcontext.SubmitChanges();
                        return status = true;
                    }
                }

            }
            return status;
        }
        #endregion

        #region De Registered Account Revamp
        public bool DeregisterAccountRevamp(string id = null)
        {
            Guid resultId;
            bool status = false;
            bool guidconversion = Guid.TryParse(id, out resultId);
            var username = Context.User.Profile.UserName;
            if (guidconversion)
            {
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    var accountlist = from ac in dbcontext.UserProfiles
                                      where ac.Id == resultId
                                      select ac;

                    var contextAccountlist = from ac in dbcontext.UserProfiles
                                             where ac.Email == Sitecore.Context.User.Profile.Email && ac.UserName == Sitecore.Context.User.Profile.UserName
                                             select ac;

                    if (accountlist != null && accountlist.Any() && contextAccountlist != null && contextAccountlist.Any())
                    {
                        //Deleting the Context account
                        var entity = accountlist.FirstOrDefault();

                        //Making another Account Primary before deleting the Context account
                        var item = contextAccountlist.ToList().FirstOrDefault(p => p.Id != entity.Id);

                        if (item != null)
                        {
                            //Setting another account to primary
                            item.IsPrimary = true;
                            item.ModifiedBy = username;
                            item.Modified_Date = DateTime.Now;

                            //Setting IsPrimary Account to False
                            entity.IsPrimary = false;
                            entity.ModifiedBy = username;
                            entity.Modified_Date = DateTime.Now;

                            if (entity.IsPrimary == false && item.IsPrimary == true)
                            {
                                dbcontext.UserProfiles.DeleteOnSubmit(entity);
                                dbcontext.SubmitChanges();
                                return status = true;
                            }
                        }
                    }
                }
            }
            return status;
        }
        #endregion
    }
}