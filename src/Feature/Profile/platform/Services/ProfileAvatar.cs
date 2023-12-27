using Adani.SuperApp.Airport.Feature.Avatar.Models;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Adani.SuperApp.Airport.Feature.Avatar.Constant;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using Sitecore.Data.Items;
using Sitecore.Layouts;
using Sitecore.Mvc.Presentation;
using Sitecore.Data.Fields;

namespace Adani.SuperApp.Airport.Feature.Avatar.Services
{
    public class ProfileAvatar : IProfileAvatar
    {
        private readonly IAPIResponse avatarResponse;
        private readonly ILogRepository logRepository;
        private readonly IHelper helper;

        public ProfileAvatar(IAPIResponse _faqResponse, ILogRepository _logRepository, ISearchBuilder _searchBuilder, IHelper _helper)
        {
            this.avatarResponse = _faqResponse;
            this.logRepository = _logRepository;
            this.helper = _helper;
        }

        public List<AvatarData> GetAvatarList()
        {
            List<AvatarData> avatarResultData = null;
            try
            {
                avatarResultData = new List<AvatarData>();
                Item avatarFolderItem = Sitecore.Context.Database.GetItem(Constant.Constant.AvatarFolderId);

                if (avatarFolderItem != null)
                {
                    foreach (Item avatar in avatarFolderItem.Children)
                    {
                        if (avatar != null && avatar.TemplateID == Templates.AvatarTemplateId)
                        {
                            AvatarData avatarData = new AvatarData();
                            if (!string.IsNullOrEmpty(avatar.Fields[Constant.Constant.AvatarId].ToString()))
                            {
                                avatarData.avatarId = avatar.Fields[Constant.Constant.AvatarId].ToString();
                            }
                            if (avatar.Fields[Constant.Constant.AvatarInclude]!=null)
                            {
                                avatarData.isAvatarInclude = (avatar.Fields[Constant.Constant.AvatarInclude].Value == "1") ? true : false;
                            }
                            if (avatar.Fields[Constant.Constant.AvatarImage] != null)
                            {
                                avatarData.avatarImagePath = helper.GetImageURL(avatar, Constant.Constant.AvatarImage);
                            }
                            avatarResultData.Add(avatarData);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                logRepository.Error("GetAvatarList Method in ProfileAvatar Class gives error -> " + ex.Message);
            }
            return avatarResultData;
        }
    }
}