namespace Sitecore.Feature.Accounts.Services
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Web.Mvc;
    using Sitecore.Data;
    using Sitecore.Data.Items;
    using Sitecore.Feature.Accounts.Models;
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
        private readonly IProfileSettingsService profileSettingsService;
        private readonly IUserProfileProvider userProfileProvider;
        private readonly IUpdateContactFacetsService updateContactFacetsService;
        private readonly IAccountTrackerService accountTrackerService;
        private readonly IDbAccountService _dbAccountService;
        public UserProfileService(IProfileSettingsService profileSettingsService, IUserProfileProvider userProfileProvider, IUpdateContactFacetsService updateContactFacetsService, IAccountTrackerService accountTrackerService, IDbAccountService dbAccountService)
        {
            this.profileSettingsService = profileSettingsService;
            this.userProfileProvider = userProfileProvider;
            this.updateContactFacetsService = updateContactFacetsService;
            this.accountTrackerService = accountTrackerService;
            this._dbAccountService = dbAccountService;
        }

        public virtual string GetUserDefaultProfileId()
        {
            return this.profileSettingsService.GetUserDefaultProfile()?.ID?.ToString();
        }

        public virtual EditProfile GetEmptyProfile()
        {
            return new EditProfile
            {
                InterestTypes = this.profileSettingsService.GetInterests()
            };
        }

        public virtual string GetAccountNumber(User user)
        {
            this.SetProfileIfEmpty(user);
            string UserName = !string.IsNullOrEmpty(user.Profile.UserName.Split('\\').LastOrDefault()) ? user.Profile.UserName.Split('\\').LastOrDefault() : user.Profile.UserName;
            var properties = this.userProfileProvider.GetCustomProperties(user.Profile);
            var masterAccount = properties.ContainsKey(Constants.UserProfile.Fields.PrimaryAccountNo) ? properties[Constants.UserProfile.Fields.PrimaryAccountNo] : "";
            return GetAccountNumberfromItem(masterAccount.ToString());
        }

        public virtual EditProfile GetProfile(User user)
        {
            this.SetProfileIfEmpty(user);
            string UserName = !string.IsNullOrEmpty(user.Profile.UserName.Split('\\').LastOrDefault()) ? user.Profile.UserName.Split('\\').LastOrDefault() : user.Profile.UserName;
            var properties = this.userProfileProvider.GetCustomProperties(user.Profile);
            var masterAccount = properties.ContainsKey(Constants.UserProfile.Fields.PrimaryAccountNo) ? properties[Constants.UserProfile.Fields.PrimaryAccountNo] : "";


            var model = new EditProfile()
            {
                LoginName = UserName,
                AccountNumber = GetAccountNumberfromItem(masterAccount),
                Email = user.Profile.Email,
                FirstName = properties.ContainsKey(Constants.UserProfile.Fields.FirstName) ? properties[Constants.UserProfile.Fields.FirstName] : "",
                LastName = properties.ContainsKey(Constants.UserProfile.Fields.LastName) ? properties[Constants.UserProfile.Fields.LastName] : "",
                MobileNumber = properties.ContainsKey(Constants.UserProfile.Fields.MobileNo) ? properties[Constants.UserProfile.Fields.MobileNo] : "",
                LandlineNumber = properties.ContainsKey(Constants.UserProfile.Fields.LandLineNo) ? properties[Constants.UserProfile.Fields.LandLineNo] : "",
                DateofBirth = properties.ContainsKey(Constants.UserProfile.Fields.Birthday) ? Sitecore.DateUtil.IsoDateToDateTime(properties[Constants.UserProfile.Fields.Birthday]).ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) : "",
                EBill = properties.ContainsKey(Constants.UserProfile.Fields.Ebill) ? (properties[Constants.UserProfile.Fields.Ebill] == "1" ? true : false) : false,
                SMSUpdate = properties.ContainsKey(Constants.UserProfile.Fields.SMSUpdate) ? (properties[Constants.UserProfile.Fields.SMSUpdate] == "1" ? true : false) : false,
                PaperlessBilling = properties.ContainsKey(Constants.UserProfile.Fields.PaperlessBilling) ? (properties[Constants.UserProfile.Fields.PaperlessBilling] == "1" ? true : false) : false,
            };

            return model;
        }

        public virtual void SaveEmailAlerts(UserProfile userProfile, EditProfile model)
        {
            var properties = new Dictionary<string, string>
            {                
                [Constants.UserProfile.Fields.Ebill] = (Convert.ToInt16(model.EBill).ToString())
            };           
            this.userProfileProvider.SetCustomProfile(userProfile, properties);            
            accountTrackerService.TrackEditProfile(userProfile);
        }

        public virtual void SaveSMSAlerts(UserProfile userProfile, EditProfile model)
        {
            var properties = new Dictionary<string, string>
            {
                [Constants.UserProfile.Fields.SMSUpdate] = (Convert.ToInt16(model.SMSUpdate).ToString())
            };
            this.userProfileProvider.SetCustomProfile(userProfile, properties);
            accountTrackerService.TrackEditProfile(userProfile);
        }

        public virtual void SavePaperlessBillingFlag(UserProfile userProfile, EditProfile model)
        {
            var properties = new Dictionary<string, string>
            {
                [Constants.UserProfile.Fields.PaperlessBilling] = (Convert.ToInt16(model.PaperlessBilling).ToString())
            };
            this.userProfileProvider.SetCustomProfile(userProfile, properties);
            Context.User.Profile.Save();
            accountTrackerService.TrackEditProfile(userProfile);
        }

        

        public virtual void SaveProfile(UserProfile userProfile, EditProfile model)
        {
            //DateTime.ParseExact(model.DateofBirth, "MM/dd/yyyy", CultureInfo.InvariantCulture)
            var properties = new Dictionary<string, string>
            {
                [Constants.UserProfile.Fields.MobileNo] = model.MobileNumber,
                [Constants.UserProfile.Fields.LandLineNo] = model.LandlineNumber,
                [Constants.UserProfile.Fields.Birthday] = Sitecore.DateUtil.ToIsoDate(DateTime.ParseExact(model.DateofBirth, "dd/MM/yyyy", CultureInfo.InvariantCulture)),
                [Constants.UserProfile.Fields.Ebill] = (Convert.ToInt16(model.EBill).ToString())
            };
            userProfile.Email = model.Email;
            this.userProfileProvider.SetCustomProfile(userProfile, properties);
            try
            {
                _dbAccountService.UpdateEmailByUserName(model);
            }
            catch (Exception ex)
            {
                Log.Error("Error Update Profile in database -  ", ex.Message);
            }
            this.updateContactFacetsService.UpdateContactFacets(userProfile);
            accountTrackerService.TrackEditProfile(userProfile);
        }

        public IEnumerable<string> GetInterests()
        {
            return this.profileSettingsService.GetInterests();
        }
        private void SetProfileIfEmpty(User user)
        {
            if (Context.User.Profile.ProfileItemId != null)
                return;

            user.Profile.ProfileItemId = this.GetUserDefaultProfileId();
            user.Profile.Save();
        }


        public string GetAccountNumberfromItem(string accountItemId)
        {
            string accountNumber = String.Empty;
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                // Get the core database

                accountNumber = _dbAccountService.GetAccountNumberbyUserName(Context.User.Profile.UserName);

                if (string.IsNullOrEmpty(accountNumber))
                {
                    Sitecore.Data.Database core = Sitecore.Data.Database.GetDatabase(Settings.ProfileItemDatabase);
                    accountNumber = core.GetItem(new ID(accountItemId)) != null ? core.GetItem(new ID(accountItemId)).Fields["Account Number"].Value : "";
                }
               
            }
            return accountNumber;
        }

        public Item GetAccountItem(string accountItemId)
        {
            Item  accountItem = null;
            using (new Sitecore.SecurityModel.SecurityDisabler())
            {
                // Get the core database
                if (!string.IsNullOrEmpty(accountItemId))
                {
                    Sitecore.Data.Database core = Sitecore.Data.Database.GetDatabase(Settings.ProfileItemDatabase);
                    accountItem = core.GetItem(new ID(accountItemId)) != null ? core.GetItem(new ID(accountItemId)) : null;
                }

            }
            return accountItem;
        }
        public string GetLoginName()
        {
            var profile = Context.User.Profile;
            string loginName = !string.IsNullOrEmpty(profile.UserName.Split('\\').LastOrDefault()) ? profile.UserName.Split('\\').LastOrDefault() : profile.UserName;
            return loginName;
        }

        public string HostURL()
        {
            return Sitecore.Globals.ServerUrl;
        }

        public string GetPageURL(ID itemId)
        {
            var ItemId = Context.Database.GetItem(itemId);
            return ItemId.Url();
        }

        public Item InsertAttachmentURL(byte[] attachment,string FileName)
        {
            string mediaUrl = string.Empty;
            try
            {
                var fileName = "/sitecore/media library/Project/Electricity/Mailbox"+"/"+"UploadItem_"+DateTime.UtcNow.ToBinary();
                //var fileName = "/sitecore/media library/Project/Electricity/Mailbox/";
                Sitecore.Resources.Media.MediaCreatorOptions options = new Sitecore.Resources.Media.MediaCreatorOptions();
                options.FileBased = false;
                options.IncludeExtensionInItemName = true;
                options.Versioned = false;
                options.Destination = fileName;
                options.Database = Sitecore.Context.Database;

                var creator = new MediaCreator();
                var fileStream = new MemoryStream(attachment);

                //Create a new item
                using (new SecurityDisabler())
                {

                    var attachmentItem = creator.CreateFromStream(fileStream, fileName, options);
                    attachmentItem.Editing.BeginEdit();
                    string Extension = Path.GetExtension(FileName).Replace(".", string.Empty);
                    attachmentItem.Fields["Extension"].Value = Extension;
                    attachmentItem.Editing.EndEdit();
                    return attachmentItem;

                }

                //byte[] data;
                //using (Stream inputStream = attachment.InputStream)
                //{
                //    MemoryStream memoryStream = inputStream as MemoryStream;
                //    if (memoryStream == null)
                //    {
                //        memoryStream = new MemoryStream();
                //        inputStream.CopyTo(memoryStream);
                //    }
                //    data = memoryStream.ToArray();

                //    #region OLD Method
                //    //using (new SecurityDisabler())
                //    //{
                //    //    Item item = Sitecore.Resources.Media.MediaManager.Creator.CreateFromStream(memoryStream, fileName, options);


                //    //    //var mediaUrl = Sitecore.Resources.Media.MediaManager.GetMediaUrl(item);
                //    //    return item.ID;
                //    //} 
                //    #endregion

                //    var creator = new MediaCreator();
                //    var fileStream = new MemoryStream(data);

                //    //Create a new item
                //    using (new SecurityDisabler())
                //    {

                //        var attachmentItem = creator.CreateFromStream(fileStream, fileName, options);
                //        attachmentItem.Editing.BeginEdit();
                //        string Extension = Path.GetExtension(attachment.FileName).Replace(".",string.Empty);
                //        attachmentItem.Fields["Extension"].Value = Extension;
                //        attachmentItem.Editing.EndEdit();
                //        return attachmentItem.ID; 

                //    }
               // }

            }
            catch (Exception ex)
            {
                Log.Error("Method - InsertAttachmentURL - ", ex.Message);
                return null;
            }
        }

    }
}