using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.Data;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using System.Linq;
using Sitecore.Data.Items;
using Sitecore.Collections;
using Sitecore.Data;
using System.Net;
using System.IO;
using Sitecore.Resources.Media;
using Sitecore.Configuration;
using Sitecore.SecurityModel;
using Sitecore.Links;
using Sitecore;
using System.Text.RegularExpressions;
using Sitecore.Electricity.Website.Model;
using System.Linq.Expressions;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using System.Text;
using System.Web.Security;
using Sitecore.Security.Domains;

namespace Sitecore.Electricity.Website.Services
{
    public class TransferUserdatatoDB
    {
        
        public void Execute(Sitecore.Data.Items.Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {

            Sitecore.Diagnostics.Log.Info("My TransferUserdatatoDB scheduled task is being run!", this);
            StringBuilder sb = new StringBuilder();
            string savefilepath = System.Web.Hosting.HostingEnvironment.MapPath("~/sitecore/admin/MeterReading/temp/Import/");
            try
            {

                sb.AppendLine("Importdata Started - " + System.DateTime.Now.ToString());
                File.AppendAllText(savefilepath + "log.txt", sb.ToString());
                sb.Clear();

                // var userlist = Sitecore.Security.Accounts.UserManager.GetUsers().ToList();
                var userlists = Domain.GetDomain("electricity").GetUsers();//.OrderByDescending(x=>x.Profile.LastUpdatedDate);
                
                sb.AppendLine("User Count - " + Domain.GetDomain("electricity").GetUserCount());
                File.AppendAllText(savefilepath + "log.txt", sb.ToString());
                sb.Clear();

                if (userlists != null)
                {
                    foreach (var userDetail in userlists)
                    {
                        var primaryAccountItemId = userDetail.Profile.GetCustomProperty("Primary Account No");
                        var multipleAccountItemId = userDetail.Profile.GetCustomProperty("Multiple Account");
                        if (!string.IsNullOrEmpty(primaryAccountItemId))
                        {
                            try
                            {
                                var accountitem = GetAccountNumberfromItem(primaryAccountItemId);
                                if (accountitem != null)
                                {
                                    string accountNumber = accountitem.Fields["Account Number"].Value;
                                    string meterNumber = accountitem.Fields["Meter Number"].Value;
                                    if (!string.IsNullOrEmpty(accountNumber) && !isAccountExist(accountNumber, meterNumber, userDetail.Profile.UserName))
                                    {
                                        using (TenderDataContext dbcontext = new TenderDataContext())
                                        {
                                            dbcontext.CommandTimeout = 0;
                                            UserProfile objdata = new UserProfile();
                                            Guid newid = Guid.NewGuid();
                                            objdata.Id = newid;
                                            objdata.AccountNumber = accountNumber;
                                            objdata.MeterNumber = meterNumber;
                                            objdata.UserName = userDetail.Profile.UserName;
                                            objdata.Email = userDetail.Profile.Email;
                                            objdata.Mobile = userDetail.Profile.GetCustomProperty("Mobile Number");
                                            objdata.IsPrimary = true;
                                            objdata.status = true;
                                            objdata.CreatedBy = "Admin";
                                            objdata.Created_Date = System.DateTime.Now;
                                            dbcontext.UserProfiles.InsertOnSubmit(objdata);
                                            dbcontext.SubmitChanges();
                                            
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex.Message, ex, this);
                                sb.AppendLine("Inner Exception  - User - " + userDetail.Profile.UserName + " Error - " + ex.Message);
                                File.AppendAllText(savefilepath + "log.txt", sb.ToString());
                                sb.Clear();
                            }
                        }
                        //Multiple Account 
                        if (!string.IsNullOrEmpty(multipleAccountItemId) && !multipleAccountItemId.Equals(primaryAccountItemId))
                        {
                            List<string> MultipleAccountList = new List<string>();
                            if (multipleAccountItemId.Contains('|'))
                            {
                                MultipleAccountList = multipleAccountItemId.Split('|').ToList();
                            }
                            MultipleAccountList.Remove(primaryAccountItemId);
                            try
                            {
                                if (MultipleAccountList != null && MultipleAccountList.Any())
                                {
                                    foreach (var multipleaccount in MultipleAccountList)
                                    {
                                        var accountitem = GetAccountNumberfromItem(multipleaccount);
                                        if (accountitem != null)
                                        {
                                            string accountNumber = accountitem.Fields["Account Number"].Value;
                                            string meterNumber = accountitem.Fields["Meter Number"].Value;
                                            if (!string.IsNullOrEmpty(accountNumber) && !isAccountExist(accountNumber, meterNumber, userDetail.Profile.UserName))
                                            {
                                                using (TenderDataContext dbcontext = new TenderDataContext())
                                                {
                                                    dbcontext.CommandTimeout = 0;
                                                    UserProfile objdata = new UserProfile();
                                                    
                                                    Guid newid = Guid.NewGuid();
                                                    objdata.Id = newid;
                                                    objdata.AccountNumber = accountNumber;
                                                    objdata.MeterNumber = meterNumber;
                                                    objdata.UserName = userDetail.Profile.UserName;
                                                    objdata.Email = userDetail.Profile.Email;
                                                    objdata.Mobile = userDetail.Profile.GetCustomProperty("Mobile Number");
                                                    objdata.IsPrimary = false;
                                                    objdata.status = true;
                                                    objdata.CreatedBy = "Admin";
                                                    objdata.Created_Date = System.DateTime.Now;
                                                    dbcontext.UserProfiles.InsertOnSubmit(objdata);
                                                    dbcontext.SubmitChanges();
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Log.Error(ex.Message, ex, this);
                                sb.AppendLine("Inner Exception for Multiple Account  - User - " + userDetail.Profile.UserName + " Error - " + ex.Message);
                                File.AppendAllText(savefilepath + "log.txt", sb.ToString());
                                sb.Clear();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, this);
                sb.AppendLine("Main Exception  - " + ex.Message);
                File.AppendAllText(savefilepath + "log.txt", sb.ToString());
                sb.Clear();
            }
        }

        public Item GetAccountNumberfromItem(string accountItemId)
        {
            Item accountNumberItem = null;
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                // Get the core database
                Sitecore.Data.Database core = Sitecore.Data.Database.GetDatabase(Settings.ProfileItemDatabase);
                accountNumberItem = core.GetItem(new ID(accountItemId)) != null ? core.GetItem(new ID(accountItemId)) : null;
            }
            return accountNumberItem;
        }

        public bool isAccountExist(string accountNumber, string meterNumber, string userName)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                dbcontext.CommandTimeout = 0;
                if (dbcontext.UserProfiles.Any(x => x.AccountNumber == accountNumber && x.MeterNumber == meterNumber && x.UserName == userName))
                {
                    return true;
                }
            }
            return false;
        }


        public bool Exists(string userName)
        {
            var fullname = "electricity" + "\\" + userName;
            return Sitecore.Security.Accounts.User.Exists(fullname);
        }


        public User GetUser(string userName, string password)
        {
            var UserName = string.Empty;
            UserName = "electricity" + "\\" + userName;
            if (!System.Web.Security.Membership.ValidateUser(UserName, password))
                return null;
            if (Sitecore.Security.Accounts.User.Exists(UserName))
                return Sitecore.Security.Accounts.User.FromName(UserName, true);
            return null;
        }
    }
}