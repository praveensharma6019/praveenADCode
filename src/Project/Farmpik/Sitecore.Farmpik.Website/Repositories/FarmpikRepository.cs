using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Farmpik.Website.Models;
using Sitecore.Links;
using System.Collections.Generic;
using System.Net;

namespace Sitecore.Farmpik.Website.Repositories
{
    public class FarmpikRepository : IFarmpikRepository
    {
        public ActivityModel GetRecentActivities(string ItemId)
        {
            var siteItem = Context.Database.Items.GetItem(ItemId);
            var headingValue = siteItem.Fields[Constant.Heading];
            LinkField linkUrl = siteItem.Fields[Constant.ActivitiesViewMore];
            string urlValue = GetGeneralLinkFieldValue(linkUrl);
            var activityModel = new ActivityModel { Heading = headingValue.ToString(), Url = urlValue };
            return activityModel;
        }

        public KnowledgeHubTabsModel GetKnowledgeHubTabsModel()
        {
            var knowledgeHubTabsModel = new KnowledgeHubTabsModel();
            knowledgeHubTabsModel.Payload = new List<KnowledgeHubTabModel>();
            var tabsParentItem = Context.Database.GetItem(KnowledgeHubTabsTemplate.KnowledgeHubTabsFolderItemID);
            if (tabsParentItem != null && tabsParentItem.GetChildren() != null)
            {
                foreach (Item tabItem in tabsParentItem.GetChildren())
                {
                    CheckboxField isActiveTab = tabItem.Fields[KnowledgeHubTabsTemplate.KnowledgeHubTabsIsActiveTabFieldID];
                    if (isActiveTab != null && isActiveTab.Checked)
                    {
                        var knowledgeHubTabModel = new KnowledgeHubTabModel();
                        knowledgeHubTabModel.Title = tabItem.Fields[KnowledgeHubTabsTemplate.KnowledgeHubTabsTitleFieldID].Value;
                        LinkField linkUrl = tabItem.Fields[KnowledgeHubTabsTemplate.KnowledgeHubTabsUrlFieldID];
                        knowledgeHubTabModel.Url = GetGeneralLinkFieldValue(linkUrl);
                        knowledgeHubTabsModel.Payload.Add(knowledgeHubTabModel);
                    }
                }
            }
            if (knowledgeHubTabsModel.Payload.Count > 0)
            {
                knowledgeHubTabsModel.Count = knowledgeHubTabsModel.Payload.Count;
                knowledgeHubTabsModel.Status = true;
                knowledgeHubTabsModel.StatusCode = HttpStatusCode.OK;
                knowledgeHubTabsModel.Message = "Success";
            }
            else {
                knowledgeHubTabsModel.Count = int.Parse("0");
                knowledgeHubTabsModel.Status = false;
                knowledgeHubTabsModel.StatusCode = HttpStatusCode.NotFound;
                knowledgeHubTabsModel.Message = "Failure";
            }

            return knowledgeHubTabsModel;
        }
        private string GetGeneralLinkFieldValue(LinkField linkUrl)
        {
            var options = LinkManager.GetDefaultUrlOptions();
            options.AlwaysIncludeServerUrl = true;
            var urlValue = linkUrl.TargetItem != null ? LinkManager.GetItemUrl(linkUrl.TargetItem, options) : string.Empty;
            return urlValue;
        }
    }
}