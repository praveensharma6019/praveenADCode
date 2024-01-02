using Project.AAHL.Website.Helpers;
using Project.AAHL.Website.Models;
using Project.AAHL.Website.Templates;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Project.AAHL.Website.Services.Common
{
    public class Footer : IFooter
    {
        public FooterModel GetFooter(Rendering rendering)
        {
            FooterModel footerModelData = new FooterModel();

            FooterDetailsList footerDetailsListData = new FooterDetailsList();
            List<FooterDetailsList> footerDetailsList = new List<FooterDetailsList>();
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

                Item NavigationContext = Sitecore.Context.Database.GetItem(BaseTemplate.DataSourceID.FooterMainNavigationSourceID);
                Item SocialLinksContext = Sitecore.Context.Database.GetItem(BaseTemplate.DataSourceID.SocialIconSourceID);
                Item CopyrightContext = Sitecore.Context.Database.GetItem(BaseTemplate.DataSourceID.CopyRightSourceID);

                #region Main Navigation
                if(NavigationContext != null && NavigationContext.HasChildren)
                {                   
                    List<MainNavigationsList> mainNavigationlist = new List<MainNavigationsList>();
                    foreach (Item item in NavigationContext.Children)
                    {
                        MainNavigationsList mainNavigationlistObj = new MainNavigationsList();

                        mainNavigationlistObj.Heading = Utils.GetValue(item, BaseTemplate.Fields.Heading);
                        List<MainNavigationsItemList> mainNavigationsItemLists = new List<MainNavigationsItemList>();

                        foreach (Item child in item.Children)
                        {
                            var subitemdata = new MainNavigationsItemList
                            {
                                LinkTitle = Utils.GetValue(child, BaseTemplate.Fields.CTAText),
                                LinkUrl = Utils.GetLinkURL(child.Fields[BaseTemplate.Fields.CTALink]),
                                Target = Utils.GetValue(child, BaseTemplate.Fields.Target)
                            };
                            mainNavigationsItemLists.Add(subitemdata);
                        }

                        mainNavigationlistObj.Items = mainNavigationsItemLists;
                        mainNavigationlist.Add(mainNavigationlistObj);
                    }
                    footerDetailsListData.MainNavigations = mainNavigationlist;
                   
                }                
                #endregion
                #region Social Items
                if (SocialLinksContext != null && SocialLinksContext.HasChildren)
                {
                    SocialLinksList socialLinksListObj = new SocialLinksList();
                    List<SocialLinksList> socialLinksList = new List<SocialLinksList>();

                    foreach (Item item in SocialLinksContext.Children)
                    {
                        socialLinksListObj.Heading = Utils.GetValue(item, BaseTemplate.Fields.Heading);
                        List<SocialLinksItemList> socialLinkItemLists = new List<SocialLinksItemList>();
                        foreach (Item child in item.Children)
                        {
                            var subitemdata = new SocialLinksItemList
                            {
                                LinkTitle = Utils.GetValue(child, BaseTemplate.Fields.CTAText),
                                LinkUrl = Utils.GetLinkURL(child.Fields[BaseTemplate.Fields.CTALink]),
                                Target = Utils.GetValue(child, BaseTemplate.Fields.Target),
                                Itemicon = Utils.GetValue(child, BaseTemplate.Fields.IconImage)
                            };
                            socialLinkItemLists.Add(subitemdata);
                        }
                        socialLinksListObj.Items = socialLinkItemLists;
                        socialLinksList.Add(socialLinksListObj);
                    }
                    footerDetailsListData.SocialLinks = socialLinksList;
                    //footerDetailsList.Add(footerDetailsListData);
                }                
                #endregion
                #region Copyright
                if (CopyrightContext != null && CopyrightContext.HasChildren)
                {
                    MainNavigationsList mainNavigationlistObj1 = new MainNavigationsList();
                    List<MainNavigationsList> mainNavigationlist1 = new List<MainNavigationsList>();
                    foreach (Item item in CopyrightContext.Children)
                    {
                        mainNavigationlistObj1.Heading = Utils.GetValue(item, BaseTemplate.Fields.Heading);
                        List<MainNavigationsItemList> mainNavigationsItemLists1 = new List<MainNavigationsItemList>();

                        foreach (Item child in item.Children)
                        {
                            var subitemdata = new MainNavigationsItemList
                            {
                                LinkTitle = Utils.GetValue(child, BaseTemplate.Fields.CTAText),
                                LinkUrl = Utils.GetLinkURL(child.Fields[BaseTemplate.Fields.CTALink]),
                                Target = Utils.GetValue(child, BaseTemplate.Fields.Target)
                            };
                            mainNavigationsItemLists1.Add(subitemdata);
                        }
                        mainNavigationlistObj1.Items = mainNavigationsItemLists1;
                        mainNavigationlist1.Add(mainNavigationlistObj1);
                    }
                    footerDetailsListData.CopyRight = mainNavigationlist1;
                    
                }
                #endregion

                //Assigning All list to Main List
                footerDetailsList.Add(footerDetailsListData);
                footerModelData.FooterDetails = footerDetailsList;
            }

            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }
            return footerModelData;
        }
    }
}