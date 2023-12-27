using Sitecore.Mvc.Presentation;
using System;
using Adani.SuperApp.Airport.Feature.CustomContent.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using System.Linq;
using Sitecore.Data.Items;
using System.Collections.Generic;

namespace Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services
{
    public class CustomContentJSON : ICustomContentJSON
    {
        private readonly ILogRepository _logRepository;
        private readonly IHelper _helper;

        public CustomContentJSON(ILogRepository logRepository, IHelper helper)
        {
            this._logRepository = logRepository;
            this._helper = helper;
        }

        ContentJSONList ICustomContentJSON.GetCustomContentJSONData(Rendering rendering, string storeType, string infoType, string insurance, string zeroCancellation, string status, string fareType)
        {
            ContentJSONList contentJSONList = null;
            try
            {
                //Get the datasource for the item
                var datasource = !string.IsNullOrEmpty(rendering.DataSource)
                ? RenderingContext.Current.Rendering.Item
                : null;
                // Null Check for datasource
                if (datasource == null && datasource.Children.Count() == 0)
                {
                    throw new NullReferenceException("Adani.SuperApp.Airport.Feature.CustomContent.Platform.Services.GetServiceListData => Rendering Datasource is Empty");
                }
                var infoTypeFolder = datasource.Children.Where(x => x.Name.Trim().ToLower() == infoType).FirstOrDefault();
                if (infoTypeFolder != null && infoTypeFolder.Children != null && infoTypeFolder.Children.Count > 0)
                {
                    var statusFolder = infoTypeFolder.Children.Where(x => x.Name.ToLower() == status).FirstOrDefault();
                    if (statusFolder != null && statusFolder.Children != null && statusFolder.Children.Count > 0)
                    {
                        List<Item> childList = statusFolder.Children.Where(x => x.TemplateID == Constants.TitleWithRichtextTemplateID).ToList();
                        if (childList.Count > 0)
                        {
                            contentJSONList = new ContentJSONList();

                            foreach (var child in childList)
                            {
                                ContentJSON contentJSON = new ContentJSON();

                                contentJSON.title = child.Fields.Contains(Constants.Title) ? child.Fields[Constants.Title].Value.ToString() : "";

                                if (child.HasChildren)
                                {
                                    List<Item> lineItems = child.Children.Where(x => x.TemplateID == Constants.NameValueTemplateID).ToList();
                                    List<Item> linewithStoretypeItems = child.Children.Where(x => x.TemplateID == Constants.NameValueStoretypeTemplateID).ToList();

                                    foreach (Item lineItem in lineItems)
                                    {
                                        LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem, insurance, zeroCancellation, fareType);
                                        if (linewithLinks != null)
                                        {
                                            contentJSON.lines.Add(linewithLinks);
                                        }
                                    }

                                    foreach (Item lineItem in linewithStoretypeItems)
                                    {
                                        LinewithLinks linewithLinks = GetLineItems(lineItem, storeType);
                                        if (linewithLinks != null)
                                        {
                                            contentJSON.lines.Add(linewithLinks);
                                        }
                                    }
                                }

                                contentJSONList.contentList.Add(contentJSON);
                            }
                        }
                    }
                    else
                    {
                        List<Item> childList = infoTypeFolder.Children.Where(x => x.TemplateID == Constants.TitleWithRichtextTemplateID).ToList();
                        if (childList.Count > 0)
                        {
                            contentJSONList = new ContentJSONList();

                            foreach (var child in childList)
                            {
                                ContentJSON contentJSON = new ContentJSON();

                                contentJSON.title = child.Fields.Contains(Constants.Title) ? child.Fields[Constants.Title].Value.ToString() : "";

                                if (child.HasChildren)
                                {
                                    List<Item> lineItems = child.Children.Where(x => x.TemplateID == Constants.NameValueTemplateID).ToList();
                                    List<Item> linewithStoretypeItems = child.Children.Where(x => x.TemplateID == Constants.NameValueStoretypeTemplateID).ToList();

                                    foreach (Item lineItem in lineItems)
                                    {
                                        LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem, insurance, zeroCancellation, fareType);
                                        if (linewithLinks != null)
                                        {
                                            contentJSON.lines.Add(linewithLinks);
                                        }
                                    }

                                    foreach (Item lineItem in linewithStoretypeItems)
                                    {
                                        LinewithLinks linewithLinks = GetLineItems(lineItem, storeType);
                                        if (linewithLinks != null)
                                        {
                                            contentJSON.lines.Add(linewithLinks);
                                        }
                                    }
                                }

                                contentJSONList.contentList.Add(contentJSON);
                            }
                        }
                    }
                }
                else
                {
                    List<Item> childList = datasource.Children.Where(x => x.TemplateID == Constants.TitleWithRichtextTemplateID).ToList();
                    if (childList.Count > 0)
                    {
                        contentJSONList = new ContentJSONList();

                        foreach (var child in childList)
                        {
                            ContentJSON contentJSON = new ContentJSON();

                            contentJSON.title = child.Fields.Contains(Constants.Title) ? child.Fields[Constants.Title].Value.ToString() : "";

                            if (child.HasChildren)
                            {
                                List<Item> lineItems = child.Children.Where(x => x.TemplateID == Constants.NameValueTemplateID).ToList();
                                List<Item> linewithStoretypeItems = child.Children.Where(x => x.TemplateID == Constants.NameValueStoretypeTemplateID).ToList();

                                foreach (Item lineItem in lineItems)
                                {
                                    LinewithLinks linewithLinks = GetLineItemsInsurance(lineItem, insurance, zeroCancellation, fareType);
                                    if (linewithLinks != null)
                                    {
                                        contentJSON.lines.Add(linewithLinks);
                                    }
                                }

                                foreach (Item lineItem in linewithStoretypeItems)
                                {
                                    LinewithLinks linewithLinks = GetLineItems(lineItem, storeType);
                                    if (linewithLinks != null)
                                    {
                                        contentJSON.lines.Add(linewithLinks);
                                    }
                                }
                            }

                            contentJSONList.contentList.Add(contentJSON);
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                _logRepository.Error(" Adani.SuperApp.Airport.Feature.CustomContent.Platform.CustomContent GetCustomContentJSONData gives -> " + ex.Message);
            }
            return contentJSONList;
        }


        public LinewithLinks GetLineItemsInsurance(Item item, string insurance, string zeroCancellation, string fareType)
        {
            LinewithLinks linewithLinks = null;
            if (!string.IsNullOrEmpty(item.Fields[Constants.Value].ToString()))
            {
                if ((!string.IsNullOrEmpty(insurance) && insurance == "true") && (!string.IsNullOrEmpty(zeroCancellation) && zeroCancellation == "true"))
                {
                    if (string.IsNullOrEmpty(fareType))
                    {
                        switch (item.Fields[Constants.Name].Value.ToLower())
                        {
                            case "stuf":
                                return null;
                            case "armf":
                                return null;
                            case "secf":
                                return null;
                        }
                        linewithLinks = new LinewithLinks();
                        linewithLinks.line = item.Fields[Constants.Value].ToString();
                        if (item.HasChildren)
                        {
                            List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                            foreach (Item itemLink in lineLinkItems)
                            {
                                LineLinks lineLinks = new LineLinks();
                                lineLinks.link = itemLink.Name.Trim();
                                lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                                lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                                linewithLinks.links.Add(lineLinks);
                            }
                        }
                    }
                    else
                    {
                        if (item.Fields[Constants.Name].Value.ToLower().Contains("stuf") || item.Fields[Constants.Name].Value.ToLower().Contains("armf") || item.Fields[Constants.Name].Value.ToLower().Contains("secf"))
                        {
                            if (item.Fields[Constants.Name].Value.Contains(fareType))
                            {
                                linewithLinks = new LinewithLinks();
                                linewithLinks.line = item.Fields[Constants.Value].ToString();
                                if (item.HasChildren)
                                {
                                    List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                                    foreach (Item itemLink in lineLinkItems)
                                    {
                                        LineLinks lineLinks = new LineLinks();
                                        lineLinks.link = itemLink.Name.Trim();
                                        lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                                        lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                                        linewithLinks.links.Add(lineLinks);
                                    }
                                }
                            }
                        }
                        else
                        {
                            linewithLinks = new LinewithLinks();
                            linewithLinks.line = item.Fields[Constants.Value].ToString();
                            if (item.HasChildren)
                            {
                                List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                                foreach (Item itemLink in lineLinkItems)
                                {
                                    LineLinks lineLinks = new LineLinks();
                                    lineLinks.link = itemLink.Name.Trim();
                                    lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                                    lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                                    linewithLinks.links.Add(lineLinks);
                                }
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(insurance) && insurance == "true" && (string.IsNullOrEmpty(zeroCancellation) || zeroCancellation == "false"))
                {
                    if (string.IsNullOrEmpty(fareType))
                    {
                        switch (item.Fields[Constants.Name].Value.ToLower())
                        {
                            case "stuf":
                                return null;
                            case "armf":
                                return null;
                            case "secf":
                                return null;
                            case "zerocancellation":
                                return null;
                        }
                        linewithLinks = new LinewithLinks();
                        linewithLinks.line = item.Fields[Constants.Value].ToString();
                        if (item.HasChildren)
                        {
                            List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                            foreach (Item itemLink in lineLinkItems)
                            {
                                LineLinks lineLinks = new LineLinks();
                                lineLinks.link = itemLink.Name.Trim();
                                lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                                lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                                linewithLinks.links.Add(lineLinks);
                            }
                        }
                    }
                    else
                    {
                        if (item.Fields[Constants.Name].Value.ToLower() == "stuf" || item.Fields[Constants.Name].Value.ToLower() == "armf" || item.Fields[Constants.Name].Value.ToLower() == "secf")
                        {
                            if (item.Fields[Constants.Name].Value.Contains(fareType))
                            {
                                linewithLinks = new LinewithLinks();
                                linewithLinks.line = item.Fields[Constants.Value].ToString();
                                if (item.HasChildren)
                                {
                                    List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                                    foreach (Item itemLink in lineLinkItems)
                                    {
                                        LineLinks lineLinks = new LineLinks();
                                        lineLinks.link = itemLink.Name.Trim();
                                        lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                                        lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                                        linewithLinks.links.Add(lineLinks);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (item.Fields[Constants.Name].Value.ToLower() == "zerocancellation")
                                return null;
                            linewithLinks = new LinewithLinks();
                            linewithLinks.line = item.Fields[Constants.Value].ToString();
                            if (item.HasChildren)
                            {
                                List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                                foreach (Item itemLink in lineLinkItems)
                                {
                                    LineLinks lineLinks = new LineLinks();
                                    lineLinks.link = itemLink.Name.Trim();
                                    lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                                    lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                                    linewithLinks.links.Add(lineLinks);
                                }
                            }
                        }
                    }
                }
                else if (!string.IsNullOrEmpty(zeroCancellation) && zeroCancellation == "true" && (string.IsNullOrEmpty(insurance) || insurance == "false"))
                {
                    if (string.IsNullOrEmpty(fareType))
                    {
                        switch (item.Fields[Constants.Name].Value.ToLower())
                        {
                            case "stuf":
                                return null;
                            case "armf":
                                return null;
                            case "secf":
                                return null;
                            case "insurance":
                                return null;
                        }
                        linewithLinks = new LinewithLinks();
                        linewithLinks.line = item.Fields[Constants.Value].ToString();
                        if (item.HasChildren)
                        {
                            List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                            foreach (Item itemLink in lineLinkItems)
                            {
                                LineLinks lineLinks = new LineLinks();
                                lineLinks.link = itemLink.Name.Trim();
                                lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                                lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                                linewithLinks.links.Add(lineLinks);
                            }
                        }
                    }
                    else
                    {
                        if (item.Fields[Constants.Name].Value.ToLower() == "stuf" || item.Fields[Constants.Name].Value.ToLower() == "armf" || item.Fields[Constants.Name].Value.ToLower() == "secf")
                        {
                            if (item.Fields[Constants.Name].Value.Contains(fareType))
                            {
                                linewithLinks = new LinewithLinks();
                                linewithLinks.line = item.Fields[Constants.Value].ToString();
                                if (item.HasChildren)
                                {
                                    List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                                    foreach (Item itemLink in lineLinkItems)
                                    {
                                        LineLinks lineLinks = new LineLinks();
                                        lineLinks.link = itemLink.Name.Trim();
                                        lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                                        lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                                        linewithLinks.links.Add(lineLinks);
                                    }
                                }
                            }
                        }
                        else
                        {
                            if (item.Fields[Constants.Name].Value.ToLower() == "insurance")
                                return null;
                            linewithLinks = new LinewithLinks();
                            linewithLinks.line = item.Fields[Constants.Value].ToString();
                            if (item.HasChildren)
                            {
                                List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                                foreach (Item itemLink in lineLinkItems)
                                {
                                    LineLinks lineLinks = new LineLinks();
                                    lineLinks.link = itemLink.Name.Trim();
                                    lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                                    lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                                    linewithLinks.links.Add(lineLinks);
                                }
                            }
                        }
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(fareType))
                    {
                        switch (item.Fields[Constants.Name].Value.ToLower())
                        {
                            case "stuf":
                                return null;
                            case "armf":
                                return null;
                            case "secf":
                                return null;
                            case "insurance":
                                return null;
                            case "zerocancellation":
                                return null;
                        }
                        linewithLinks = new LinewithLinks();
                        linewithLinks.line = item.Fields[Constants.Value].ToString();
                        if (item.HasChildren)
                        {
                            List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                            foreach (Item itemLink in lineLinkItems)
                            {
                                LineLinks lineLinks = new LineLinks();
                                lineLinks.link = itemLink.Name.Trim();
                                lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                                lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                                linewithLinks.links.Add(lineLinks);
                            }
                        }
                    }
                    else
                    {
                        if (item.Fields[Constants.Name].Value.ToLower() == "stuf" || item.Fields[Constants.Name].Value.ToLower() == "armf" || item.Fields[Constants.Name].Value.ToLower() == "secf")
                        {
                            if (item.Fields[Constants.Name].Value.Contains(fareType))
                            {
                                linewithLinks = new LinewithLinks();
                                linewithLinks.line = item.Fields[Constants.Value].ToString();
                                if (item.HasChildren)
                                {
                                    List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                                    foreach (Item itemLink in lineLinkItems)
                                    {
                                        LineLinks lineLinks = new LineLinks();
                                        lineLinks.link = itemLink.Name.Trim();
                                        lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                                        lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                                        linewithLinks.links.Add(lineLinks);
                                    }
                                }
                            }
                        }
                        else
                        {
                            switch (item.Fields[Constants.Name].Value.ToLower())
                            {
                                case "insurance":
                                    return null;
                                case "zerocancellation":
                                    return null;
                            }
                            linewithLinks = new LinewithLinks();
                            linewithLinks.line = item.Fields[Constants.Value].ToString();
                            if (item.HasChildren)
                            {
                                List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                                foreach (Item itemLink in lineLinkItems)
                                {
                                    LineLinks lineLinks = new LineLinks();
                                    lineLinks.link = itemLink.Name.Trim();
                                    lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                                    lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                                    linewithLinks.links.Add(lineLinks);
                                }
                            }
                        }
                    }

                }
            }

            return linewithLinks;
        }

        public LinewithLinks GetLineItems(Item item, string storeType)
        {
            LinewithLinks linewithLinks = null;
            string lineStoreType = string.Empty;
            if (!string.IsNullOrEmpty(item.Fields[Constants.Value].ToString()))
            {
                linewithLinks = new LinewithLinks();
                linewithLinks.line = item.Fields[Constants.Value].ToString();

                lineStoreType = item.Fields.Contains(Constants.StoreType) ? item.Fields[Constants.StoreType].Value.ToString() : string.Empty;

                if (item.HasChildren)
                {
                    List<Item> lineLinkItems = item.Children.Where(x => x.TemplateID == Constants.TitleWithLinkTemplateID).ToList();
                    foreach (Item itemLink in lineLinkItems)
                    {
                        LineLinks lineLinks = new LineLinks();
                        lineLinks.link = itemLink.Name.Trim();
                        lineLinks.linkText = itemLink.Fields.Contains(Constants.LinkText) ? itemLink.Fields[Constants.LinkText].Value.ToString() : "";
                        lineLinks.linkURL = itemLink.Fields.Contains(Constants.LinkURL) ? _helper.LinkUrl(itemLink.Fields[Constants.LinkURL]) : "";
                        linewithLinks.links.Add(lineLinks);
                    }
                }
            }
            if (!string.IsNullOrEmpty(lineStoreType) && !lineStoreType.ToLower().Equals("all"))
            {
                if (!storeType.Equals(lineStoreType.ToLower()))
                {
                    linewithLinks = null;
                }
            }

            return linewithLinks;
        }

    }
}