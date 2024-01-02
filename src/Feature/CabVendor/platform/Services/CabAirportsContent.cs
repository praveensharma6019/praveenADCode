using Adani.SuperApp.Airport.Feature.CabVendor.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.CabVendor.Platform.Services
{
    public class CabAirportsContent : ICabAirportsContent
    {
        private readonly ILogRepository logging;
        private readonly IHelper helper;

        public CabAirportsContent(ILogRepository _logging, IHelper _helper)
        {
            this.logging = _logging;
            this.helper = _helper;
        }
        public AirportList GetCabAirports(Item datasource)
        {
            return GetAirportwithCode(datasource);
        }

        private AirportList GetAirportwithCode(Item datasource)
        {

            //AirportList airportList = null;


            AirportList airportList = new AirportList();

            try
            {
                IEnumerable<Item> listItems = null;

                listItems = datasource.Children.Where(x => x.TemplateID == VendorConstant.CityTemplateID).ToList();

                List<AirportItem> airportItems = new List<AirportItem>();
                if (listItems != null && listItems.Count() > 0)
                {

                    foreach (Sitecore.Data.Items.Item item in listItems)
                    {
                        if (item != null && item.Children != null)
                        {
                            IEnumerable<Item> AirportItems =
                                item.Children.Where(x => x.TemplateID.Guid == VendorConstant.AirportTemplateID).ToList();


                            if (AirportItems != null && AirportItems.Count() > 0)
                            {
                                foreach (Item airport in AirportItems)
                                {

                                    if (airport != null)
                                    {
                                        IEnumerable<Item> terminalItemsList = airport.Children.Where(x => x.TemplateID.Guid == VendorConstant.TerminalTemplateID).ToList();

                                        AirportItem airportDetails = new AirportItem();
                                        //AirportItem airportDetails = new AirportItem();
                                        ContactDetailsItems contactDetailsItems = new ContactDetailsItems();
                                        airportDetails.AirportName = airport.Fields["AirportName"]?.Value;
                                        airportDetails.AirportCode = airport.Fields["AirportCode"]?.Value;
                                        airportDetails.AirportID = airport.Fields["AirportID"]?.Value;
                                        airportDetails.Details = airport.Fields["Details"]?.Value;
                                        airportDetails.City = item.Fields["CityName"]?.Value;
                                        airportDetails.AirportImage = helper.GetImageURL(airport, "Image");
                                        airportDetails.AirportAddress = airport.Fields["AirportAddress"]?.Value;
                                        airportDetails.AirportThumbnailImage = helper.GetImageURL(airport, "ImageThumbnail");
                                        airportDetails.ColorCode = airport.Fields["ColorCode"]?.Value;
                                        airportDetails.AirportIcon = helper.GetImageURL(airport, "AirportIcon");
                                        airportDetails.TerminalList = airport.Fields["Terminal"]?.Value;
                                        airportDetails.AirportPrefixName = airport.Fields["AirportPrefixName"]?.Value;
                                        airportDetails.IsTheme = helper.GetCheckboxOption(airport.Fields["IsTheme"]);
                                        airportDetails.IsCabEnabled = helper.GetCheckboxOption(airport.Fields["IsCabEnabled"]);
                                        airportDetails.IsNonAdaniAirport = helper.GetCheckboxOption(airport.Fields["IsNonAdaniAirport"]);
                                        airportDetails.PostFix = airport.Fields["PostFix"]?.Value;
                                        airportDetails.AppBarBackgroundColor = airport.Fields["AppBarBackgroundColor"]?.Value;
                                        airportDetails.AppBarSubtitleColor = airport.Fields["AppBarSubtitleColor"]?.Value;
                                        airportDetails.AppBarTitleColor = airport.Fields["AppBarTitleColor"]?.Value;
                                        airportDetails.StickyWidgetBackgroundColor = airport.Fields["StickyWidgetBackgroundColor"]?.Value;
                                        airportDetails.StickyWidgetTextColor = airport.Fields["StickyWidgetTextColor"]?.Value;
                                        airportDetails.StickyWidgetIconColor = airport.Fields["StickyWidgetIconColor"]?.Value;
                                        airportDetails.lightStatusBar = helper.GetCheckboxOption(airport.Fields["LightStatusBar"]);
                                        airportDetails.latitude = airport.Fields["Latitude"]?.Value;
                                        airportDetails.longitude = airport.Fields["Longitude"]?.Value;
                                        airportDetails.placeID = airport.Fields["PlaceID"]?.Value;
                                        airportDetails.PranaamServiceAvailable = helper.GetCheckboxOption(airport.Fields["PranaamServiceAvailable"]);
                                        airportDetails.PorterServiceAvailable = helper.GetCheckboxOption(airport.Fields["PorterServiceAvailable"]);
                                        airportDetails.MetaTitle = airport.Fields["MetaTitle"]?.Value;
                                        airportDetails.MetaDescription = airport.Fields["MetaDescription"]?.Value;
                                        airportDetails.MetaKeywords = airport.Fields["MetaKeywords"]?.Value;
                                        airportDetails.MetaImage = !string.IsNullOrEmpty(helper.GetImageURL(airport, "MetaImage")) ? helper.GetImageURL(airport, "MetaImage") : string.Empty;
                                        airportDetails.MetaUrl = !string.IsNullOrEmpty(helper.GetLinkURL(airport, "MetaUrl")) ? helper.GetLinkURL(airport, "MetaUrl") : string.Empty;

                                        airportDetails.dutyFreeAgeLimit = string.IsNullOrEmpty(airport.Fields["DutyFreeAgeLimit"].ToString()) ? 0 : Convert.ToInt32(airport.Fields["DutyFreeAgeLimit"].ToString());
                                        List<Terminals> terminalsListItems = new List<Terminals>();
                                        Item phone = helper.GetDropLinkItem(airport.Fields["Phone"]);
                                        if (phone != null)
                                        {
                                            ContactDetails phoneItem = new ContactDetails();
                                            phoneItem.name = phone.Fields["Name"].Value;
                                            phoneItem.title = phone.Fields["Title"].Value;
                                            phoneItem.richText = phone.Fields["RichText"].Value;
                                            contactDetailsItems.phone = phoneItem;

                                        }
                                        Item email = helper.GetDropLinkItem(airport.Fields["Email"]);
                                        if (email != null)
                                        {
                                            ContactDetails emailItem = new ContactDetails();
                                            emailItem.name = email.Fields["Name"].Value;
                                            emailItem.title = email.Fields["Title"].Value;
                                            emailItem.richText = email.Fields["RichText"].Value;
                                            contactDetailsItems.email = emailItem;

                                        }
                                        airportDetails.contactDetails = contactDetailsItems;
                                        if (terminalItemsList != null && terminalItemsList.Count() > 0)
                                        {
                                            foreach (Item terminal in terminalItemsList)
                                            {
                                                Terminals terminals = new Terminals();
                                                terminals.TerminalName = terminal.Fields["TerminalName"]?.Value;
                                                terminals.TerminalAddress = terminal.Fields["TerminalAddress"]?.Value;
                                                terminals.TerminalCode = terminal.Fields["TerminalCode"]?.Value;
                                                terminals.PranaamServiceAvailable = helper.GetCheckboxOption(terminal.Fields["PranaamServiceAvailable"]);
                                                terminals.Latitude = terminal.Fields["Latitude"]?.Value;
                                                terminals.Longitude = terminal.Fields["Longitude"]?.Value;
                                                terminals.DutyFreeAvailable = helper.GetCheckboxOption(terminal.Fields["DutyFreeAvailable"]);
                                                terminals.placeID = terminal.Fields["PlaceID"]?.Value;
                                                terminals.IsCabEnabled = helper.GetCheckboxOption(terminal.Fields["IsAvailableForCab"]);

                                                IEnumerable<Item> gateItemsList = terminal.Children.Where(x => x.TemplateID.Guid == VendorConstant.TerminalTemplateID).ToList();
                                                foreach (Item gateItem in gateItemsList)
                                                {
                                                    Gate gate = new Gate();
                                                    gate.gateType = gateItem.Fields["TerminalCode"]?.Value;
                                                    gate.gate = gateItem.Fields["TerminalName"]?.Value;
                                                    gate.latitude = gateItem.Fields["Latitude"]?.Value;
                                                    gate.longitude = gateItem.Fields["Longitude"]?.Value;
                                                    gate.pickupAddress = gateItem.Fields["TerminalAddress"]?.Value;
                                                    gate.fnbAvailable = helper.GetCheckboxOption(gateItem.Fields["fnbAvailable"]);
                                                    gate.retailAvailable = helper.GetCheckboxOption(gateItem.Fields["retailAvailable"]);
                                                    gate.dutyFreeAvailable = helper.GetCheckboxOption(gateItem.Fields["DutyFreeAvailable"]);
                                                    gate.placeID = gateItem.Fields["PlaceID"]?.Value;
                                                    gate.IsCabEnabled = helper.GetCheckboxOption(gateItem.Fields["IsAvailableForCab"]);
                                                    terminals.gates.Add(gate);
                                                }

                                                List<string> terminalkeywordsList = new List<string>();
                                                IEnumerable<Item> terminalkeywordstItems = terminal.Children.Where(x => x.TemplateID == VendorConstant.KeywordsFolderTemplateID).ToList();

                                                if (terminalkeywordsList != null && terminalkeywordstItems.Count() > 0)
                                                {
                                                    foreach (Item keyWords in terminalkeywordstItems)
                                                    {
                                                        IEnumerable<Item> keywordtItem = keyWords.Children.Where(x => x.TemplateID == VendorConstant.KeywordTemplateID).ToList();
                                                        if (keywordtItem != null && keywordtItem.Count() > 0)
                                                        {
                                                            terminalkeywordsList.AddRange(from Item keyword in keywordtItem
                                                                                  let keywords = keyword.Fields["keyword"]?.Value
                                                                                  select keywords);
                                                        }
                                                    }
                                                }
                                                terminals.Keywords = terminalkeywordsList;

                                                terminalsListItems.Add(terminals);
                                            }
                                        }
                                        airportDetails.TerminalsList = terminalsListItems;

                                        List<string> keywordsList = new List<string>();
                                        IEnumerable<Item> keywordstItems = item.Children.Where(x => x.TemplateID == VendorConstant.KeywordsFolderTemplateID).ToList();

                                        if (keywordstItems != null && keywordstItems.Count() > 0)
                                        {
                                            foreach (Item keyWords in keywordstItems)
                                            {
                                                IEnumerable<Item> keywordtItem = keyWords.Children.Where(x => x.TemplateID == VendorConstant.KeywordTemplateID).ToList();
                                                if (keywordtItem != null && keywordtItem.Count() > 0)
                                                {
                                                    keywordsList.AddRange(from Item keyword in keywordtItem
                                                                          let keywords = keyword.Fields["keyword"]?.Value
                                                                          select keywords);
                                                }
                                            }
                                        }
                                        airportDetails.Keywords = keywordsList;

                                        airportItems.Add(airportDetails);
                                        // airportList.ListOfAirports.Add(airportDetails);
                                    }

                                    //return airportList;
                                }
                                airportList.ListOfAirports = airportItems;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                this.logging.Error("CityAirportsContent - GetAirports()" + ex.Message);
            }


            return airportList;
        }

    }
}