using System;
using System.Collections.Generic;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using ClosedXML.Excel;
using System.Data;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using DocumentFormat.OpenXml.Office.CustomUI;
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
using Sitecore.ElectricityNew.Website.Model;
using System.Linq.Expressions;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using System.Text;
using System.Globalization;

namespace Sitecore.ElectricityNew.Website.Services
{
    public class UserUpload
    {
        public void Execute(Sitecore.Data.Items.Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            Sitecore.Diagnostics.Log.Info("My UserUpload scheduled task is being run!", this);
            try
            {
                StringBuilder sb = new StringBuilder();
                string savefilepathlog = System.Web.Hosting.HostingEnvironment.MapPath("~/sitecore/admin/MeterReading/temp/Import/");
                sb.AppendLine("User Upload Execute - " + System.DateTime.Now.ToString());
                File.AppendAllText(savefilepathlog + "log.txt", sb.ToString());
                sb.Clear();

                string savefilepath = System.Web.Hosting.HostingEnvironment.MapPath("~/sitecore/admin/MeterReading/temp/Import/Blank_Meter_Number_data.xlsx");
                using (XLWorkbook workBook = new XLWorkbook(savefilepath))
                {
                    IXLWorksheet workSheet = workBook.Worksheet(1);

                    //Create a new DataTable.
                    DataTable dt = new DataTable();

                    //Loop through the Worksheet rows.
                    bool firstRow = true;
                    foreach (IXLRow row in workSheet.Rows())
                    {
                        if (!row.IsEmpty())
                        {
                            if (firstRow)
                            {
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dt.Columns.Add(cell.Value.ToString());
                                }
                                firstRow = false;
                            }
                            else
                            {
                                //Add rows to DataTable.
                                dt.Rows.Add();
                                int i = 0;
                                foreach (IXLCell cell in row.Cells())
                                {
                                    dt.Rows[dt.Rows.Count - 1][i] = cell.Value.ToString();
                                    i++;
                                }
                            }
                        }
                    }
                    if (dt != null && dt.Rows.Count > 0)
                    {
                        Importdata(dt);
                    }
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex, this);
            }
        }


        private void Importdata(DataTable dt)
        {
            StringBuilder sb = new StringBuilder();
            string savefilepath = System.Web.Hosting.HostingEnvironment.MapPath("~/sitecore/admin/MeterReading/temp/Import/");
            sb.AppendLine("User Upload Started - " + System.DateTime.Now.ToString());
            File.AppendAllText(savefilepath + "log.txt", sb.ToString());
            sb.Clear();
            
            int cnt = 0;
            try
            {
                string profileId = "{14523428-879D-491C-9DF0-1417E82A4311}";

                foreach (DataRow row in dt.Rows)
                {
                    cnt++;
                    if (row["LoginName"].ToString().Contains(","))
                    {
                        sb.AppendLine(cnt + "Comma in User - " + row["LoginName"].ToString() + " - " + System.DateTime.Now.ToString() + "");
                        File.AppendAllText(savefilepath + "log.txt", sb.ToString());
                        sb.Clear();
                    }
                    if (!string.IsNullOrEmpty(row["LoginName"].ToString()) && !row["LoginName"].ToString().Contains(",") && !Exists(row["LoginName"].ToString()) && !string.IsNullOrEmpty(row["Password"].ToString()))
                    {
                        if (!string.IsNullOrEmpty(row["Account No"].ToString()))
                        {
                            var fullName = "electricity" + "\\" + row["LoginName"].ToString();
                            var user = Sitecore.Security.Accounts.User.Create(fullName, row["Password"].ToString().Trim());
                            user.Profile.Email = row["Email"].ToString();
                            if (!string.IsNullOrEmpty(profileId))
                            {
                                user.Profile.ProfileItemId = profileId;
                            }

                            string multiplerecordlist = string.Empty;
                            string primaryAcc = string.Empty;
                            try
                            {
                                var existingitemid = GetAccountItemId(row["Account No"].ToString().Trim());

                                if (!string.IsNullOrEmpty(existingitemid))
                                {
                                    multiplerecordlist = existingitemid;
                                    primaryAcc = existingitemid;
                                }
                                else
                                {
                                    string meternumber = string.Empty;
                                    if (!string.IsNullOrEmpty(row["Meter No"].ToString()))
                                    {
                                        meternumber = Regex.Replace(row["Meter No"].ToString().Trim(), @"[^0-9a-zA-Z]+", "_");
                                    }
                                    else
                                    {
                                        meternumber = "0000000";
                                    }
                                    
                                    Sitecore.Data.Items.Item accountitem = CreateAccountItem(row["Account No"].ToString().Trim(), meternumber);

                                    multiplerecordlist = accountitem.ID.ToString();
                                    primaryAcc = accountitem.ID.ToString();
                                }

                                if (!string.IsNullOrEmpty(row["SUBCA"].ToString()))
                                {
                                    var multiplerecord = row["SUBCA"].ToString().Split(',');
                                    if (multiplerecord.Length > 0)
                                    {
                                        foreach (var item in multiplerecord)
                                        {
                                            Sitecore.Data.Items.Item subaccountitem = CreateAccountItem(item.Trim(), "0000000");
                                            multiplerecordlist = multiplerecordlist + "|" + subaccountitem.ID.ToString();
                                        }
                                    }
                                }

                                user.Profile.SetCustomProperty("FirstName", row["FirstName"].ToString().Trim());
                                user.Profile.SetCustomProperty("LastName", row["LastName"].ToString().Trim());
                                user.Profile.SetCustomProperty("Gender", row["Gender"].ToString().Trim());
                                user.Profile.SetCustomProperty("Landline Number", row["Landline Number"].ToString().Trim());
                                user.Profile.SetCustomProperty("Mobile Number", row["Mobile Number"].ToString().Trim());
                                user.Profile.SetCustomProperty("Date of birth", !string.IsNullOrEmpty(row["Date of birth"].ToString()) ? Sitecore.DateUtil.ToIsoDate(DateTime.ParseExact(row["Date of birth"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture)) : "");
                                user.Profile.SetCustomProperty("Secret Question", row["Secret Question"].ToString().Trim());
                                user.Profile.SetCustomProperty("Answer", row["Answer"].ToString().Trim());
                                user.Profile.SetCustomProperty("Primary Account No", primaryAcc);
                                user.Profile.SetCustomProperty("E Bill", row["E Bill"].ToString().Trim() == "1" || row["E Bill"].ToString().Trim().ToLower() == "on" ? (System.Convert.ToInt32(System.Convert.ToBoolean("True")).ToString()) : (System.Convert.ToInt32(System.Convert.ToBoolean("False")).ToString()));
                                user.Profile.SetCustomProperty("SMS update", row["SMS update"].ToString().Trim() == "1" ? (System.Convert.ToInt32(System.Convert.ToBoolean("True")).ToString()) : (System.Convert.ToInt32(System.Convert.ToBoolean("False")).ToString()));
                                user.Profile.SetCustomProperty("PaperlessBilling", row["PaperlessBilling"].ToString().Trim().ToUpper() == "Y" ? (System.Convert.ToInt32(System.Convert.ToBoolean("True")).ToString()) : (System.Convert.ToInt32(System.Convert.ToBoolean("False")).ToString()));
                                user.Profile.SetCustomProperty("Multiple Account", multiplerecordlist);
                                user.Profile.Save();
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error RegisterUser -  "+ ex.Message, this);
                                user.Delete();
                                //accountitem.Delete();
                                //var indexableItem = (SitecoreIndexableItem)accountitem;
                                // ContentSearchManager.GetIndex("sitecore_core_index").Delete(indexableItem.UniqueId);
                                sb.AppendLine("Error for Login - " + System.DateTime.Now.ToString() + " - " + row["LoginName"].ToString() + " and CA - " + row["Account No"].ToString() + " " + ex.Message);
                                File.AppendAllText(savefilepath + "log.txt", sb.ToString());
                                sb.Clear();
                            }
                        }
                    }
                    else
                    {
                        Log.Info("User Upload - LoginName exists - " + row["LoginName"].ToString() + "", this);
                        if (!string.IsNullOrEmpty(row["Account No"].ToString()))
                        {
                            var userDetail = GetUser(row["LoginName"].ToString(), row["Password"].ToString().Trim());
                            if (userDetail == null)
                            {
                                sb.AppendLine("UserDetail getting null - " + System.DateTime.Now.ToString());
                                File.AppendAllText(savefilepath + "log.txt", sb.ToString());
                                sb.Clear();
                            }

                            else
                            {

                                string multiplerecordlist = string.Empty;
                                string primaryAcc = string.Empty;
                                try
                                {
                                    var existingitemid = GetAccountItemId(row["Account No"].ToString().Trim());

                                    if (!string.IsNullOrEmpty(existingitemid))
                                    {
                                        multiplerecordlist = existingitemid;
                                        primaryAcc = existingitemid;
                                    }
                                    else
                                    {
                                        string meternumber = string.Empty;
                                        if (!string.IsNullOrEmpty(row["Meter No"].ToString()))
                                        {
                                            meternumber = Regex.Replace(row["Meter No"].ToString().Trim(), @"[^0-9a-zA-Z]+", "_");
                                        }
                                        else
                                        {
                                            meternumber = "0000000";
                                        }

                                        Sitecore.Data.Items.Item accountitem = CreateAccountItem(row["Account No"].ToString().Trim(), meternumber);

                                        multiplerecordlist = accountitem.ID.ToString();
                                        primaryAcc = accountitem.ID.ToString();
                                    }

                                    if (!string.IsNullOrEmpty(row["SUBCA"].ToString()))
                                    {
                                        var multiplerecord = row["SUBCA"].ToString().Split(',');
                                        if (multiplerecord.Length > 0)
                                        {
                                            foreach (var subcaitem in multiplerecord)
                                            {
                                                Sitecore.Data.Items.Item subaccountitem = CreateAccountItem(subcaitem.Trim(), "0000000");

                                                multiplerecordlist = multiplerecordlist + "|" + subaccountitem.ID.ToString();
                                            }
                                        }
                                    }
                                    userDetail.Profile.SetCustomProperty("FirstName", row["FirstName"].ToString().Trim());
                                    userDetail.Profile.SetCustomProperty("LastName", row["LastName"].ToString().Trim());
                                    userDetail.Profile.SetCustomProperty("Gender", row["Gender"].ToString().Trim());
                                    userDetail.Profile.SetCustomProperty("Landline Number", row["Landline Number"].ToString().Trim());
                                    userDetail.Profile.SetCustomProperty("Mobile Number", row["Mobile Number"].ToString().Trim());
                                    userDetail.Profile.SetCustomProperty("Date of birth", !string.IsNullOrEmpty(row["Date of birth"].ToString())? Sitecore.DateUtil.ToIsoDate(DateTime.ParseExact(row["Date of birth"].ToString(), "dd/MM/yyyy", CultureInfo.InvariantCulture)):"");
                                    userDetail.Profile.SetCustomProperty("Secret Question", row["Secret Question"].ToString().Trim());
                                    userDetail.Profile.SetCustomProperty("Answer", row["Answer"].ToString().Trim());
                                    userDetail.Profile.SetCustomProperty("Primary Account No", primaryAcc);
                                    userDetail.Profile.SetCustomProperty("E Bill", row["E Bill"].ToString().Trim() == "1" || row["E Bill"].ToString().Trim().ToLower() == "on" ? (System.Convert.ToInt32(System.Convert.ToBoolean("True")).ToString()) : (System.Convert.ToInt32(System.Convert.ToBoolean("False")).ToString()));
                                    userDetail.Profile.SetCustomProperty("SMS update", row["SMS update"].ToString().Trim() == "1" ? (System.Convert.ToInt32(System.Convert.ToBoolean("True")).ToString()) : (System.Convert.ToInt32(System.Convert.ToBoolean("False")).ToString()));
                                    userDetail.Profile.SetCustomProperty("PaperlessBilling", row["PaperlessBilling"].ToString().Trim().ToUpper() == "Y" ? (System.Convert.ToInt32(System.Convert.ToBoolean("True")).ToString()) : (System.Convert.ToInt32(System.Convert.ToBoolean("False")).ToString()));
                                    userDetail.Profile.SetCustomProperty("Multiple Account", multiplerecordlist);
                                    userDetail.Profile.Save();

                                }
                                catch (Exception ex)
                                {
                                    Log.Error("Error RegisterUser User Upload Page-  " + ex.Message, this);
                                    sb.AppendLine("Exception Existing User   - " + ex.Message + row["LoginName"].ToString() + " - " + multiplerecordlist + " - " + System.DateTime.Now.ToString());
                                    File.AppendAllText(savefilepath + "log.txt", sb.ToString());
                                    sb.Clear();
                                }
                            }
                        }
                    }

                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error Description : " + cnt + " - " + ex.Message, this);
                sb.AppendLine("User Upload  Error - " + cnt + " - " + System.DateTime.Now.ToString() + " -  " + ex.Message);
                File.AppendAllText(savefilepath + "log.txt", sb.ToString());
                sb.Clear();
            }

            sb.AppendLine("User Upload Ended - " + System.DateTime.Now.ToString());
            File.AppendAllText(savefilepath + "log.txt", sb.ToString());
            sb.Clear();
        }

        public bool Exists(string userName)
        {
            var fullname = "electricity" + "\\" + userName;
            //  var fullName = Sitecore.Context.Domain.GetFullName(userName);
            return Sitecore.Security.Accounts.User.Exists(fullname);
        }

        public string GetAccountItemId(string accountNumber)
        {
            using (IProviderSearchContext context = ContentSearchManager.GetIndex("sitecore_core_index").CreateSearchContext())
            {
                var predicateQuery = PredicateBuilder.True<SearchResultItem>();
                predicateQuery = predicateQuery.And(p => p.TemplateId == new Data.ID("{38FCD543-B37B-46EE-B867-40A000148DDB}")); //Sitecore.Feature.Accounts.Templates.AccountNumber.ItemId.ToString())
                predicateQuery = predicateQuery.And(p => p["account_number_t"] == accountNumber || p["account_number_s"] == accountNumber);

                var result = context.GetQueryable<SearchResultItem>().Where(predicateQuery).Where(x => x.Language == Sitecore.Globalization.Language.Current.Name);
                if (result != null && result.ToList().Count() > 0)
                {
                    //return true;
                    return result.FirstOrDefault().ItemId.ToString();
                }
            }
            return string.Empty;
        }

        public Sitecore.Data.Items.Item CreateAccountItem(string accountNumber, string meterNumber)
        {
            Sitecore.Data.Items.Item newItem = null;

            Expression<Func<SearchResultItem, bool>> predicateQuery = PredicateBuilder.True<SearchResultItem>();
            predicateQuery = predicateQuery.And(p => p.TemplateId == new Data.ID("{38FCD543-B37B-46EE-B867-40A000148DDB}"));
            predicateQuery = predicateQuery.And(p => p["account_number_t"] == accountNumber || p["account_number_s"] == accountNumber);

            ISearchIndex index = ContentSearchManager.GetIndex("sitecore_core_index");
            using (IProviderSearchContext context = index.CreateSearchContext())
            {
                var searchResult = context.GetQueryable<SearchResultItem>().Where(predicateQuery).Where(x => x.Language == Sitecore.Globalization.Language.Current.Name && x.Name.Equals(accountNumber + "-" + meterNumber));
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

                        TemplateItem template = master.GetItem("/sitecore/templates/Feature/Accounts/AccountList");

                        Sitecore.Data.Items.Item parentItem = master.GetItem("/sitecore/content/Home/Account List");

                        newItem = parentItem.Add(accountNumber + "-" + meterNumber, template);

                        newItem.Editing.BeginEdit();
                        try
                        {
                            // Assign values to the fields of the new item
                            newItem.Fields["Account Number"].Value = accountNumber;
                            newItem.Fields["Meter Number"].Value = meterNumber;
                            newItem.Editing.EndEdit();

                            //Refresh sitecore core index adter creating new item
                            ContentSearchManager.GetIndex("sitecore_core_index").Refresh((SitecoreIndexableItem)newItem);
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + newItem.Paths.FullPath + ": " + ex.Message, this);
                            newItem.Editing.CancelEdit();
                        }

                    }
                    // Get the master database
                }
            }
            return newItem;
        }

        public User GetUser(string userName, string password)
        {
            var UserName = string.Empty;
            UserName = "electricity" + "\\" + userName;
            if (Sitecore.Security.Accounts.User.Exists(UserName))
                return Sitecore.Security.Accounts.User.FromName(UserName, true);
            return null;
        }
    }
}