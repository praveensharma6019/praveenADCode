using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data.Items;
using System.Web;
using static Adani.SuperApp.Airport.Feature.Forex.Platform.Models.ForexImportantInfoModel;
using Sitecore.Mvc.Presentation;

namespace Adani.SuperApp.Airport.Feature.Forex.Platform.Services
{
    public class ImportantInformation : IImportantInformation
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;

        public ImportantInformation(ILogRepository logRepository, IHelper helper)
        {
            this._logRepository = logRepository;
            this._helper = helper;
        }

        public ForexImportantInfoList GetForexImportantInfo(Rendering rendering)
        {
            ForexImportantInfoList infoList = null;
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? RenderingContext.Current.Rendering.Item
                : null;
                // Null Check for datasource
                if (datasource == null && datasource.Children.Count() == 0)
                {
                    throw new NullReferenceException("Adani.SuperApp.Airport.Feature.Forex.Platform.Services.GetServiceListData => Rendering Datasource is Empty");
                }

                List<Item> childList = datasource.Children.Where(x => x.TemplateID == Constants.TitleWithRichtextTemplateID).ToList();

                if (childList.Count > 0)
                {
                    foreach (var child in childList)
                    {
                        infoList = new ForexImportantInfoList();
                        ForexImportantInfoJSON InfoJSON = new ForexImportantInfoJSON();

                        InfoJSON.title = child.Fields.Contains(Constants.Title) ? child.Fields[Constants.Title].Value.ToString() : "";

                        if (child.HasChildren)
                        {
                            List<Item> lineItems = child.Children.Where(x => x.TemplateID == Constants.ForexImportantInfoTemplateID).ToList();


                            foreach (Item lineItem in lineItems)
                            {
                                LineswithLinks lineswithLinks = GetLineItems(lineItem);
                                if (lineswithLinks != null)
                                {
                                    InfoJSON.lines.Add(lineswithLinks);
                                }
                            }
                        }
                        infoList.InfoList.Add(InfoJSON);
                    }
                }

            }
            catch (Exception ex)
            {

                _logRepository.Error(" Adani.SuperApp.Airport.Feature.CustomContent.Platform.CustomContent GetCustomContentJSONData gives -> " + ex.Message);
            }
            return infoList;

        }
        public LineswithLinks GetLineItems(Item item)
        {
            LineswithLinks lineswithLinks = null;

            if (!string.IsNullOrEmpty(item.Fields[Constants.Value].ToString()))
            {
                lineswithLinks = new LineswithLinks();
                lineswithLinks.line = item.Fields[Constants.Value].ToString();
                lineswithLinks.iconURL = _helper.GetImageURLbyField(item?.Fields[Constants.Icon]);

                if (item.HasChildren)
                {
                    List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                    foreach (Item itemLink in lineLinkItems)
                    {
                        LineLinks lineLinks = new LineLinks();
                        lineLinks.link = itemLink.Name.Trim();
                        lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                        lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                        lineswithLinks.links.Add(lineLinks);
                    }
                }
            }


            return lineswithLinks;
        }
    }
}
