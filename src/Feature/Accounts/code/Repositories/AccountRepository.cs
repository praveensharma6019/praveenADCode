namespace Sitecore.Feature.Accounts.Repositories
{
    using System;
    using System.Web.Security;
    using Sitecore.Diagnostics;
    using Sitecore.Feature.Accounts.Models;
    using Sitecore.Feature.Accounts.Services;
    using Sitecore.Foundation.Accounts.Pipelines;
    using Sitecore.Foundation.DependencyInjection;
    using Sitecore.Pipelines;
    using Sitecore.Security.Accounts;
    using Sitecore.Security.Authentication;
    using Sitecore.Configuration;
    using Sitecore.Data.Items;
    using Sitecore.Shell.Framework;
    using System.Globalization;
    using Sitecore.ContentSearch;
    using Sitecore.ContentSearch.SearchTypes;
    using System.Linq;
    using Sitecore.Data;
    using System.Linq.Expressions;
    using Sitecore.ContentSearch.Linq.Utilities;
    using System.Collections.Generic;
    using System.Data.SqlClient;
    using System.Data;
    using Sitecore.Foundation.Dictionary.Repositories;
    using Newtonsoft.Json;

    [Service(typeof(IAccountRepository))]
    public class AccountRepository : IAccountRepository
    {
        public IAccountTrackerService AccountTrackerService { get; }
        private readonly PipelineService pipelineService;
        private IDbAccountService _dbAccountService { get; }

        PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext();
        public AccountRepository(PipelineService pipelineService, IAccountTrackerService accountTrackerService, IDbAccountService dbAccountService)
        {
            this.AccountTrackerService = accountTrackerService;
            this.pipelineService = pipelineService;
            this._dbAccountService = dbAccountService;
        }

        public bool Exists(string userName)
        {
            var fullName = Context.Domain.GetFullName(userName);
            return User.Exists(fullName);
        }

        public bool IsValidUser(string userName, string password)
        {
            var UserName = string.Empty;
            var domain = Context.Domain;
            if (domain != null)
            {
                UserName = domain.GetFullName(userName);
            }
            var user = AuthenticationManager.Login(UserName, password);
            return user;
        }

        public User GetUser(string userName, string password)
        {
            var UserName = string.Empty;
            var domain = Context.Domain;
            if (domain != null)
            {
                UserName = domain.GetFullName(userName);
            }
            if (!System.Web.Security.Membership.ValidateUser(UserName, password))
                return null;
            if (User.Exists(UserName))
                return User.FromName(UserName, true);
            return null;
        }

        public User GetUserbyLoginName(string userName)
        {
            var UserName = string.Empty;
            var domain = Context.Domain;
            if (domain != null)
            {
                UserName = domain.GetFullName(userName);
            }
            if (User.Exists(UserName))
                return User.FromName(UserName, true);
            return null;
        }

        public User Login(string userName, string password)
        {
            User user = null;
            try
            {
                var accountName = string.Empty;
                var domain = Context.Domain;
                if (domain != null)
                {
                    accountName = domain.GetFullName(userName);
                }

                var result = AuthenticationManager.Login(accountName, password);
                if (!result)
                {
                    AccountTrackerService.TrackLoginFailed(accountName);
                    return null;
                }

                user = AuthenticationManager.GetActiveUser();

                this.pipelineService.RunLoggedIn(user);

            }
            catch
            {

            }
            return user;
        }

        public User LoginAPI(string userName, string password)
        {
            User user = null;
            try
            {
                string accountName = string.Empty;
                if (userName != null)
                {
                    accountName = "electricity\\" + userName;
                }

                var result = AuthenticationManager.Login(accountName, password);
                if (!result)
                {
                    AccountTrackerService.TrackLoginFailed(accountName);
                    return null;
                }

                user = AuthenticationManager.GetActiveUser();

            }
            catch
            {

            }
            return user;
        }

        public void Logout()
        {
            var user = AuthenticationManager.GetActiveUser();
            AuthenticationManager.Logout();
            if (user != null)
                this.pipelineService.RunLoggedOut(user);
        }

        public string RestorePassword(string userName)
        {
            var domainName = Context.Domain.GetFullName(userName);
            var user = Membership.GetUser(domainName);
            if (user == null)
                throw new ArgumentException($"Could not reset password for user '{userName}'", nameof(userName));
            return user.ResetPassword();
        }

        public string RegisterUser(RegistrationInfo registrationInfo, string profileId)
        {
            Assert.ArgumentNotNullOrEmpty(registrationInfo.Email, nameof(registrationInfo.Email));
            Assert.ArgumentNotNullOrEmpty(registrationInfo.Password, nameof(registrationInfo.Password));

            //TODO: Add domain name electricity
            string errorMsg = string.Empty;
            var fullName = Sitecore.Context.Site.Domain.GetFullName(registrationInfo.LoginName);
            Assert.IsNotNullOrEmpty(fullName, "Can't retrieve full userName");

            var user = User.Create(fullName, registrationInfo.Password);
            try
            {
                user.Profile.Email = registrationInfo.Email;
                if (!string.IsNullOrEmpty(profileId))
                {
                    user.Profile.ProfileItemId = profileId;
                }
                user.Profile.Save();
                //Verfiy Account Number and Meter number valid through API

                //TODO: Function for create account for user (Meter No and Account No). 
                //Create item (Path:- core: \content\home\Accounts) in core database by this template id "{38FCD543-B37B-46EE-B867-40A000148DDB}".
                // Item accountitem = CreateAccountItem(registrationInfo.AccountNo, registrationInfo.MeterNo); // Note : Rajdeep(remove)

                //TODO: set custom property. fieldName and Value
                try
                {
                    user.Profile.SetCustomProperty("FirstName", registrationInfo.FirstName);
                    user.Profile.SetCustomProperty("LastName", registrationInfo.LastName);
                    user.Profile.SetCustomProperty("Gender", registrationInfo.Gender);
                    user.Profile.SetCustomProperty("Landline Number", !string.IsNullOrEmpty(registrationInfo.LandlineNumber) ? registrationInfo.LandlineNumber : string.Empty);
                    user.Profile.SetCustomProperty("Mobile Number", registrationInfo.MobileNumber);
                    user.Profile.SetCustomProperty("Date of birth", Sitecore.DateUtil.ToIsoDate(DateTime.ParseExact(registrationInfo.DateofBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)));
                    user.Profile.SetCustomProperty("Secret Question", registrationInfo.SecretQuestion);
                    user.Profile.SetCustomProperty("Answer", registrationInfo.Answer);
                    //user.Profile.SetCustomProperty("Primary Account No", accountitem.ID.ToString()); // Note : Rajdeep(remove) 
                    user.Profile.SetCustomProperty("E Bill", (Convert.ToInt32(registrationInfo.EBill).ToString()));
                    user.Profile.SetCustomProperty("SMS update", (Convert.ToInt32(registrationInfo.RecieveNotification).ToString()));
                    //user.Profile.SetCustomProperty("Multiple Account", accountitem.ID.ToString()); // Note : Rajdeep(remove) 

                    user.Profile.Save();


                    //insert in customeDB
                    _dbAccountService.RegisterUser(registrationInfo);

                    this.pipelineService.RunRegistered(user);
                    this.Login(registrationInfo.Email, registrationInfo.Password);
                }
                catch (Exception ex)
                {
                    errorMsg = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/technical issue", "There is technical issue. Please try after sometime.");
                    user.Delete();
                    Log.Error("Error RegisterUser in Profile Error -  ", ex.Message);
                    AccountTrackerService.TrackRegistrationFailed(registrationInfo.Email);
                }

            }
            catch (Exception ex)
            {
                errorMsg = DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/technical issue", "There is technical issue. Please try after sometime.");
                user.Delete();
                Log.Error("Error RegisterUser -  ", ex.Message);
                AccountTrackerService.TrackRegistrationFailed(registrationInfo.Email);

                //delete from user when error occur.
            }
            return errorMsg;
        }


        public Item CreateAccountItem(string accountNumber, string meterNumber)
        {
            Item newItem = null;

            Expression<Func<SearchResultItem, bool>> predicateQuery = PredicateBuilder.True<SearchResultItem>();
            predicateQuery = predicateQuery.Or(p => p.TemplateId == new Data.ID("{38FCD543-B37B-46EE-B867-40A000148DDB}"));

            ISearchIndex index = ContentSearchManager.GetIndex("sitecore_core_index");
            using (IProviderSearchContext context = index.CreateSearchContext())
            {
                var searchResult = context.GetQueryable<SearchResultItem>().Where(predicateQuery).Where(x => x.Language == Sitecore.Globalization.Language.Current.Name && x.Name == accountNumber + "-" + meterNumber);
                if (searchResult != null && searchResult.ToList().Count() > 0)
                {
                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {
                        newItem = searchResult.ToList().FirstOrDefault().GetItem();
                    }
                }
                else
                {

                    using (new Sitecore.SecurityModel.SecurityDisabler())
                    {

                        Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase(Settings.ProfileItemDatabase);
                        // Get the template for which you need to create item
                        TemplateItem template = master.GetItem("/sitecore/templates/Feature/Accounts/AccountList");

                        // Get the place in the site tree where the new item must be inserted
                        Item parentItem = master.GetItem("/sitecore/content/Home/Account List");

                        // Add the item to the site tree
                        newItem = parentItem.Add(accountNumber + "-" + meterNumber, template);

                        // Set the new item in editing mode
                        // Fields can only be updated when in editing mode
                        // (It's like the begin transaction on a database)
                        newItem.Editing.BeginEdit();
                        try
                        {
                            // Assign values to the fields of the new item
                            newItem.Fields["Account Number"].Value = accountNumber;
                            newItem.Fields["Meter Number"].Value = meterNumber;

                            // End editing will write the new values back to the Sitecore
                            // database (It's like commit transaction of a database)
                            newItem.Editing.EndEdit();

                            //Refresh sitecore core index adter creating new item
                            ContentSearchManager.GetIndex("sitecore_core_index").Refresh((SitecoreIndexableItem)newItem);
                        }
                        catch (System.Exception ex)
                        {
                            // Log the message on any failure to sitecore log
                            Sitecore.Diagnostics.Log.Error("Could not update item " + newItem.Paths.FullPath + ": " + ex.Message, this);

                            // Cancel the edit (not really needed, as Sitecore automatically aborts
                            // the transaction on exceptions, but it wont hurt your code)
                            newItem.Editing.CancelEdit();
                        }

                    }
                    // Get the master database
                }
            }
            return newItem;
        }


        #region MyRegion
        public ID GetAccountItemId(string accountNumber)
        {
            using (IProviderSearchContext context = ContentSearchManager.GetIndex("sitecore_core_index").CreateSearchContext())
            {
                var predicateQuery = PredicateBuilder.True<SearchResultItem>();
                predicateQuery = predicateQuery.And(p => p.TemplateId == new Data.ID("{38FCD543-B37B-46EE-B867-40A000148DDB}")); //Sitecore.Feature.Accounts.Templates.AccountNumber.ItemId.ToString())
                predicateQuery = predicateQuery.And(p => p["account_number_t"] == accountNumber || p["account_number_s"] == accountNumber);

                var result = context.GetQueryable<SearchResultItem>().Where(predicateQuery).Where(x => x.Language == Sitecore.Globalization.Language.Current.Name);
                if (result != null && result.ToList().Count() > 0)
                {
                    return result.FirstOrDefault().ItemId;
                }
            }
            return null;
        }
        #endregion
        public void RemoveAccountNumberPermenantly(string itemId)
        {
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                Sitecore.Data.Database master = Sitecore.Data.Database.GetDatabase(Settings.ProfileItemDatabase);
                Item AccountItem = master.GetItem(new ID(itemId));
                if (AccountItem != null)
                {
                    AccountItem.Delete();
                    var indexableItem = (SitecoreIndexableItem)AccountItem;
                    ContentSearchManager.GetIndex("sitecore_core_index").Delete(indexableItem.UniqueId);
                }
            }
        }

        public List<User> GetUserbyItemId(string itemId)
        {
            var UserList = Sitecore.Security.Accounts.UserManager.GetUsers().Where(x => !string.IsNullOrEmpty(x.Profile.GetCustomProperty("Primary Account No")) && x.Profile.GetCustomProperty("Primary Account No").Equals(itemId.ToString()) && x.GetDomainName().ToLower() == Constants.DomainDetail.CurrentDomainName).ToList();
            return UserList;
        }

        public User GetUserbyEmail_Account(string Email, string Account)
        {
            // var UserList = Sitecore.Security.Accounts.UserManager.GetUsers().Where(x => x.Profile.Email == Email).ToList();

            var UserList = System.Web.Security.Membership.FindUsersByEmail(Email);
            if (UserList != null && UserList.Count > 0)
            {
                var usernames = UserList.Cast<MembershipUser>().Select(x => x.UserName).ToList();
                bool isvalid = false;
                foreach (var item in usernames)
                {
                    var userDetail = User.FromName(item, true);
                    bool isPresentInDb = _dbAccountService.isUserEmailAccountPresentForgotPassword(userDetail.Profile.UserName, Account, Email);
                    if (isPresentInDb)
                    {
                        isvalid = true;
                    }
                    else
                    {
                        var MultipleAccountItemId = userDetail.Profile.GetCustomProperty("Multiple Account");
                        if (!string.IsNullOrEmpty(MultipleAccountItemId))
                        {
                            List<string> MultipleAccountList = new List<string>();
                            MultipleAccountList = MultipleAccountItemId.Split('|').ToList();
                            foreach (var itemacc in MultipleAccountList)
                            {
                                using (new Sitecore.SecurityModel.SecurityDisabler())
                                {
                                    Sitecore.Data.Database core = Sitecore.Data.Database.GetDatabase(Settings.ProfileItemDatabase);
                                    var accountNumber = core.GetItem(new ID(itemacc)) != null ? core.GetItem(new ID(itemacc)).Fields["Account Number"].Value : "";
                                    if (!string.IsNullOrEmpty(accountNumber) && accountNumber.Equals(Account.Trim()))
                                    {
                                        isvalid = true;
                                        break;
                                    }
                                }
                            }
                        }
                    }
                    if (isvalid)
                    {
                        return userDetail;
                    }

                }
                return null;
            }
            else
            {
                return null;
            }



        }


        public User GetUserByUserID(string userID)
        {
            DataTable dt = GetUserDetailsByUserID(userID);
            if (dt != null && dt.Rows.Count > 0)
            {
                string userName = dt.Rows[0]["UserName"].ToString();
                return User.FromName(userName, true);
            }
            return null;
        }

        public DataTable GetUserDetailsByUserID(string userID)
        {
            SqlConnection conn = null;

            try
            {
                string coreConnectString = Sitecore.Configuration.Settings.GetConnectionString("core").ToString();
                // create and open a connection object
                conn = new SqlConnection(coreConnectString);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand("[dbo].[aspnet_Membership_GetUserByUserId]", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which
                // will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@UserId", userID));
                cmd.Parameters.Add(new SqlParameter("@CurrentTimeUtc", System.DateTime.UtcNow));
                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                return dt;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }

        public static List<T> ConvertToList<T>(DataTable dt)
        {
            var columnNames = dt.Columns.Cast<DataColumn>().Select(c => c.ColumnName.ToLower()).ToList();
            var properties = typeof(T).GetProperties();
            return dt.AsEnumerable().Select(row =>
            {
                var objT = Activator.CreateInstance<T>();
                foreach (var pro in properties)
                {
                    if (columnNames.Contains(pro.Name.ToLower()))
                    {
                        try
                        {
                            pro.SetValue(objT, row[pro.Name]);
                        }
                        catch { }
                    }
                }
                return objT;
            }).ToList();
        }

        public UserStatisticsdata GetUserStatistics(string date)
        {
            SqlConnection conn = null;
            UserStatisticsdata userdata = new UserStatisticsdata();
            try
            {
                Sitecore.Diagnostics.Log.Info("GetUserStatistics Method Called", this);
                string coreConnectString = Sitecore.Configuration.Settings.GetConnectionString("core").ToString();
                // create and open a connection object
                conn = new SqlConnection(coreConnectString);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand("[dbo].[GetUserStatistics]", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which
                // will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@RequestDate", date));

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                var listdata = ConvertToList<UserStatistics>(dt);
                // List<UserStatistics> list = dt.AsEnumerable().ToList<UserStatistics>();

                //string JSONresult = JsonConvert.SerializeObject(dt);
                if (listdata != null && listdata.Count > 0)
                {
                    userdata.message = "Record fetch successfully";
                    userdata.userdata = listdata;
                }
                else
                {
                    userdata.message = "No Record found";
                    userdata.userdata = listdata;
                }
                return userdata;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetUserStatistics:" + ex.Message, this);
                userdata.message = ex.Message;
                userdata.userdata = new List<UserStatistics>();
                return userdata;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }


        public UserCAList GetUserCAList(string UserName)
        {
            SqlConnection conn = null;
            UserCAList userdata = new UserCAList();
            try
            {
                Sitecore.Diagnostics.Log.Info("GetUserCAList Method Called", this);
                string coreConnectString = Sitecore.Configuration.Settings.GetConnectionString("core").ToString();
                // create and open a connection object
                conn = new SqlConnection(coreConnectString);
                conn.Open();

                // 1. create a command object identifying
                // the stored procedure
                SqlCommand cmd = new SqlCommand("[dbo].[GetUserCAList]", conn);

                // 2. set the command object so it knows
                // to execute a stored procedure
                cmd.CommandType = CommandType.StoredProcedure;

                // 3. add parameter to command, which
                // will be passed to the stored procedure
                cmd.Parameters.Add(new SqlParameter("@UserName", "electricity\\" + UserName));

                DataTable dt = new DataTable();
                dt.Load(cmd.ExecuteReader());

                var listdata = ConvertToList<UserCADetails>(dt);
                // List<UserStatistics> list = dt.AsEnumerable().ToList<UserStatistics>();

                //string JSONresult = JsonConvert.SerializeObject(dt);
                if (listdata != null && listdata.Count > 0)
                {
                    userdata.message = "Record fetch successfully";
                    userdata.userdata = listdata;
                    
                }
                else
                {
                    userdata.message = "No Record found";
                    userdata.userdata = listdata;
                }
                return userdata;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetUserStatistics:" + ex.Message, this);
                userdata.message = ex.Message;
                userdata.userdata = new List<UserCADetails>();
                return userdata;
            }
            finally
            {
                if (conn != null)
                {
                    conn.Close();
                }
            }
        }
    }
}