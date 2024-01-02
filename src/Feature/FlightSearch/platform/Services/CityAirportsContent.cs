using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Sites;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services
{
    public class CityAirportsContent : ICityAirportsContent
    {

        private readonly ILogRepository logging;
        private readonly IHelper helper;

        public CityAirportsContent(ILogRepository _logging, IHelper _helper)
        {
            this.logging = _logging;
            this.helper = _helper;
        }

        /// <summary>
        /// Implementation to get the header data
        /// </summary>
        /// <param name="datasource"></param>
        /// <returns></returns>
        public CityAirportsData GetCityAirportsData(Item datasource, string isdomestic)
        {
            CityAirportsData listCityAirportsData = new CityAirportsData();
            List<CityDetails> listCityAirports = new List<CityDetails>();
            IEnumerable<Item> listItems = null;
            if (isdomestic.ToLower() == "in")
            {
                listItems = datasource.Children.Where(x => x.TemplateID == Templates.CityListCollection.CityTemplateID && x[Constants.Constants.CountryCode] == isdomestic.ToUpper()).ToList();
            }
            else
            {
                listItems = datasource.Children.Where(x => x.TemplateID == Templates.CityListCollection.CityTemplateID).ToList();
            }
            try
            {
                if (listItems != null && listItems.Count() > 0)
                {
                    foreach (Item item in listItems)
                    {

                        CityDetails cityDetails = new CityDetails
                        {


                            CountryCode = item.Fields["CountryCode"]?.Value,
                            CountryName = item.Fields["CountryName"]?.Value,
                            IsDomestic = Convert.ToString(item.Fields["CountryCode"].Value) == "IN" ? true : false,
                            CityCode = item.Fields["CityCode"]?.Value,
                            CityName = item.Fields["CityName"]?.Value,
                            About = item.Fields["About"]?.Value,
                            CityImage = helper.GetImageURL(item, "Image"),
                            BestTimetoVisitTitle = item.Fields["BestTimetoVisitTitle"]?.Value,
                            BestTimetoVisitDetails = item.Fields["BestTimetoVisitDetails"]?.Value
                        };


                        List<string> keywordsList = new List<string>();
                        IEnumerable<Item> keywordstItems = item.Children.Where(x => x.TemplateID == Templates.CityListCollection.KeywordsFolderTemplateID).ToList();

                        if (keywordstItems != null && keywordstItems.Count() > 0)
                        {
                            foreach (Item keyWords in keywordstItems)
                            {
                                IEnumerable<Item> keywordtItem = keyWords.Children.Where(x => x.TemplateID == Templates.CityListCollection.KeywordTemplateID).ToList();
                                if (keywordtItem != null && keywordtItem.Count() > 0)
                                {
                                    keywordsList.AddRange(from Item keyword in keywordtItem
                                                          let keywords = keyword.Fields["keyword"]?.Value
                                                          select keywords);
                                }
                            }
                        }
                        cityDetails.Keywords = keywordsList;

                        cityDetails.ListAirports = GetAirports(item);
                        listCityAirports.Add(cityDetails);
                    }


                }
                listCityAirportsData.ListCity = listCityAirports;
            }
            catch (Exception ex)
            {

                this.logging.Error("CityAirport - GetCityAirportsData()" + ex.Message);
            }

            return listCityAirportsData;
        }


        public AirportList GetCityAirports(Item datasource)
        {


            return GetAirportwithCode(datasource);
        }

        private AirportDetails GetAirports(Item item, bool IsAirportList = false)
        {
            AirportDetails airPortsList = new AirportDetails();
            AirportDetails airportDetails = null;
            try
            {
                if (item != null && item.Children != null)
                {
                    IEnumerable<Item> AirportItems =
                        item.Children.Where(x => x.TemplateID == Templates.CityListCollection.AirportTemplateID).ToList();

                    if (AirportItems != null && AirportItems.Count() > 0)
                    {
                        foreach (Item airport in AirportItems)
                        {

                            if (IsAirportList && airport.Fields["IsAdaniAirport"]?.Value == "1")
                            {
                                airportDetails = new AirportDetails()
                                {
                                    AirportName = airport.Fields["AirportName"]?.Value,
                                    AirportCode = airport.Fields["AirportCode"]?.Value,
                                    AirportID = airport.Fields["AirportID"]?.Value,
                                    Details = airport.Fields["Details"]?.Value,
                                    City = airport.Fields["City"]?.Value,
                                    AirportImage = helper.GetImageURL(airport, "Image"),
                                    AirportAddress = airport.Fields["AirportAddress"]?.Value
                                };
                            }


                            if (!IsAirportList)
                            {
                                airportDetails = new AirportDetails()
                                {
                                    AirportName = airport.Fields["AirportName"]?.Value,
                                    AirportCode = airport.Fields["AirportCode"]?.Value,
                                    AirportID = airport.Fields["AirportID"]?.Value,
                                    Details = airport.Fields["Details"]?.Value,
                                    City = airport.Fields["City"]?.Value,
                                    AirportImage = helper.GetImageURL(airport, "Image"),
                                    HealthandSaftyMeasure = airport.Fields["HealthandSaftyMeasure"]?.Value,
                                    BaggageInformation = airport.Fields["BaggageInformation"]?.Value,
                                    AirportAddress = airport.Fields["AirportAddress"]?.Value
                                };

                                List<TerminalDetails> terminalList = new List<TerminalDetails>();
                                IEnumerable<Item> AirportTerminalItems = airport.Children.Where(x => x.TemplateID == Templates.CityListCollection.TerminalTemplateID).ToList();
                                if (AirportTerminalItems != null && AirportTerminalItems.Count() > 0)
                                {
                                    foreach (Item terminals in AirportTerminalItems)
                                    {
                                        TerminalDetails terminalDetails = new TerminalDetails
                                        {
                                            TerminalName = terminals.Fields["TerminalName"]?.Value,
                                            TerminalAddress = terminals.Fields["TerminalAddress"]?.Value
                                        };
                                        CheckboxField checkboxField = terminals.Fields["PranaamServiceAvailable"];
                                        if (checkboxField != null && checkboxField.Checked)
                                        {
                                            terminalDetails.PranaamServiceAvailable = true;
                                        }
                                        terminalList.Add(terminalDetails);
                                    }
                                }
                                airportDetails.TerminalList = terminalList;
                            }

                            airPortsList = airportDetails;
                        }
                    }
                }
            }
            catch (Exception ex)
            {

                this.logging.Error("CityAirportsContent - GetAirports()" + ex.Message);
            }


            return airPortsList;
        }

        private AirportList GetAirportwithCode(Item datasource)
        {
            
            //AirportList airportList = null;


            AirportList airportList = new AirportList();

            try
            {
                IEnumerable<Item> listItems = null;

                listItems = datasource.Children.Where(x => x.TemplateID == Templates.CityListCollection.CityTemplateID).ToList();

                List<AirportItem> airportItems = new List<AirportItem>();
                if (listItems != null && listItems.Count() > 0)
                {

                    foreach (Sitecore.Data.Items.Item item in listItems)
                    {
                        if (item != null && item.Children != null)
                        {
                            IEnumerable<Item> AirportItems =
                                item.Children.Where(x => x.TemplateID == Templates.CityListCollection.AirportTemplateID && x[Constants.Constants.IsAdaniAirport] == "1").ToList();

                            
                            if (AirportItems != null && AirportItems.Count() > 0)
                            {
                                foreach (Item airport in AirportItems)
                                {

                                    if (airport != null)
                                    {
                                        IEnumerable<Item> terminalItemsList = airport.Children.Where(x => x.TemplateID == Templates.CityListCollection.TerminalTemplateID).ToList();

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
                                        airportDetails.PostFix = airport.Fields["PostFix"]?.Value;
                                        airportDetails.AppBarBackgroundColor = airport.Fields["AppBarBackgroundColor"]?.Value;
                                        airportDetails.AppBarSubtitleColor = airport.Fields["AppBarSubtitleColor"]?.Value;
                                        airportDetails.AppBarTitleColor = airport.Fields["AppBarTitleColor"]?.Value;
                                        airportDetails.StickyWidgetBackgroundColor = airport.Fields["StickyWidgetBackgroundColor"]?.Value;
                                        airportDetails.StickyWidgetTextColor = airport.Fields["StickyWidgetTextColor"]?.Value;
                                        airportDetails.StickyWidgetIconColor = airport.Fields["StickyWidgetIconColor"]?.Value;
                                        airportDetails.lightStatusBar = helper.GetCheckboxOption(airport.Fields["LightStatusBar"]);
                                        airportDetails.IsNonAdaniAirport = helper.GetCheckboxOption(airport.Fields[Constants.Constants.IsNonAdaniAirport]);
                                        airportDetails.PranaamServiceAvailable = helper.GetCheckboxOption(airport.Fields["PranaamServiceAvailable"]);
                                        airportDetails.PorterServiceAvailable = helper.GetCheckboxOption(airport.Fields["PorterServiceAvailable"]);
                                        airportDetails.facilityNumber = airport.Fields["CarParkingFacilityNumber"]?.Value;
                                        airportDetails.CarParkingNumber = airport.Fields["CarParkingNumber"]?.Value;
                                        airportDetails.isCarParkEnabled = helper.GetCheckboxOption(airport.Fields["isCarParkEnabled"]);

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
                                            foreach (var terminal in terminalItemsList)
                                            {
                                                Terminals terminals = new Terminals();
                                                terminals.TerminalName = terminal.Fields["TerminalName"]?.Value;
                                                terminals.TerminalAddress = terminal.Fields["TerminalAddress"]?.Value;
                                                terminals.TerminalCode = terminal.Fields["TerminalCode"]?.Value;
                                                terminals.PranaamServiceAvailable = helper.GetCheckboxOption(terminal.Fields["PranaamServiceAvailable"]);
                                                terminals.Latitude = terminal.Fields["Latitude"]?.Value;
                                                terminals.Longitude = terminal.Fields["Longitude"]?.Value;
                                                terminals.DutyFreeAvailable = helper.GetCheckboxOption(terminal.Fields["DutyFreeAvailable"]);
                                                airportDetails.facilityNumber = terminal.Fields["CarParkingFacilityNumber"]?.Value;
                                                airportDetails.CarParkingNumber = terminal.Fields["CarParkingNumber"]?.Value;
                                                airportDetails.isCarParkEnabled = helper.GetCheckboxOption(airport.Fields["isCarParkEnabled"]);

                                                IEnumerable<Item> gateItemsList = terminal.Children.Where(x => x.TemplateID == Templates.CityListCollection.TerminalTemplateID).ToList();
                                                foreach (var gateItem in gateItemsList)
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
                                                    gate.international = helper.GetCheckboxOption(gateItem.Fields["isInternational"]);
                                                    gate.domestic = helper.GetCheckboxOption(gateItem.Fields["isDomestic"]);
                                                    terminals.gates.Add(gate);
                                                }
                                                terminalsListItems.Add(terminals);
                                            }
                                        }
                                        airportDetails.TerminalsList = terminalsListItems;

                                        List<string> keywordsList = new List<string>();
                                        IEnumerable<Item> keywordstItems = item.Children.Where(x => x.TemplateID == Templates.CityListCollection.KeywordsFolderTemplateID).ToList();

                                        if (keywordstItems != null && keywordstItems.Count() > 0)
                                        {
                                            foreach (Item keyWords in keywordstItems)
                                            {
                                                IEnumerable<Item> keywordtItem = keyWords.Children.Where(x => x.TemplateID == Templates.CityListCollection.KeywordTemplateID).ToList();
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

        /// <summary>
        /// method for mobile app service
        /// </summary>
        /// <param name="datasource"></param>
        /// <param name="airportType"></param>
        /// <returns></returns>
        public AppAirportsData GetAppAirportsData(Item datasource, string airportType, string isdomestic, string airportcode)
        {            
           
            AppAirportsData appAirportdata = new AppAirportsData();
            List<AirportDetailsApp> listappAirports = new List<AirportDetailsApp>();

            try
            {
                IEnumerable<Item> listItems = null;
                if (isdomestic.ToLower() == "in")
                {
                    listItems = datasource.Children.Where(x => x.TemplateID == Templates.CityListCollection.CityTemplateID && x[Constants.Constants.CountryCode] == isdomestic.ToUpper()).ToList();
                }
                else
                {
                    listItems = datasource.Children.Where(x => x.TemplateID == Templates.CityListCollection.CityTemplateID).ToList();
                   //listItems = listItems.OrderBy(x => x.Fields[Constants.Constants.Priority]).ToList();
                }

                if (listItems != null && listItems.Count() > 0)
                {

                    foreach (Sitecore.Data.Items.Item parent in listItems)
                    {                       
                       
                        IEnumerable<Item> airportlistItems = null;
                        if (airportcode != null && airportcode.Trim() != "")
                        {
                            String[] airportCode = airportcode.ToUpper().Split(',');
                            airportlistItems = parent.Children.Where(x => x.TemplateID == Templates.CityListCollection.AirportTemplateID && airportCode.Contains(x.Fields[Constants.Constants.AirportCode].Value.ToUpper())).ToList();
                        }
                        else
                        {
                            airportlistItems = parent.Children.Where(x => x.TemplateID == Templates.CityListCollection.AirportTemplateID).ToList();
                        }
                        if (airportlistItems != null && airportlistItems.Count() > 0)
                        {
                            AirportDetailsApp listCityAirportsData = new AirportDetailsApp
                            {
                                CountryCode = parent.Fields["CountryCode"]?.Value,
                                CountryName = parent.Fields["CountryName"]?.Value,
                                IsDomestic = Convert.ToString(parent.Fields["CountryCode"].Value) == "IN" ? true : false,
                                CityCode = parent.Fields["CityCode"]?.Value,
                                CityName = parent.Fields["CityName"]?.Value,
                                Priority= parent.Fields["Priority"]?.Value
                            };
                            CheckboxField checkboxField = parent.Fields["isPopular"];
                           
                            foreach (Item item in airportlistItems)
                            {
                                if (checkboxField != null && checkboxField.Checked)
                                {
                                    listCityAirportsData.isPopular = true;
                                }
                                listCityAirportsData.AirportCode = item.Fields["AirportCode"].Value;
                                listCityAirportsData.AirportName = item.Fields["AirportName"].Value;
                                listCityAirportsData.AirportType = item.Fields["Airport Type"].Value;
                                listCityAirportsData.AirportID = item.Fields["AirportID"].Value;
                                CheckboxField chkIsParnaam = item.Fields["IsPranaam"];
                                if (chkIsParnaam != null && chkIsParnaam.Checked)
                                {
                                    listCityAirportsData.IsPranaam = true;
                                }
                                CheckboxField chkIsParnaamMaster = item.Fields["IsPranaamMasterAirport"];
                                if (chkIsParnaamMaster != null && chkIsParnaamMaster.Checked)
                                {
                                    listCityAirportsData.IsPranaamMasterAirport = true;
                                }
                                CheckboxField chkIsMasterAirport = item.Fields["IsMasterAirport"];
                                if (chkIsMasterAirport != null && chkIsMasterAirport.Checked)
                                {
                                    listCityAirportsData.IsMasterAirport = true;
                                }
                            }
                           
                            List<string> keywordsList = new List<string>();
                            IEnumerable<Item> keywordstItems = parent.Children.Where(x => x.TemplateID == Templates.CityListCollection.KeywordsFolderTemplateID).ToList();

                            if (keywordstItems != null && keywordstItems.Count() > 0)
                            {
                                foreach (Sitecore.Data.Items.Item keyWords in keywordstItems)
                                {
                                    IEnumerable<Item> keywordtItem = keyWords.Children.Where(x => x.TemplateID == Templates.CityListCollection.KeywordTemplateID).ToList();
                                    if (keywordtItem != null && keywordtItem.Count() > 0)
                                    {
                                        keywordsList.AddRange(from Item keyword in keywordtItem
                                                              let keywords = keyword.Fields["keyword"]?.Value
                                                              select keywords);
                                    }
                                    listCityAirportsData.Keywords = keywordsList;
                                }
                            }
                            listappAirports.Add(listCityAirportsData);
                        }
                    }
                    listappAirports = listappAirports.OrderBy(x => Int32.TryParse(x.Priority, out var i) ? i : Int32.MinValue).ToList();                    

            }
                if (!string.IsNullOrEmpty(airportType))
                {
                    listappAirports = listappAirports.Where(x => x.AirportType.Equals(airportType, StringComparison.OrdinalIgnoreCase)).ToList();
                    
                }
                appAirportdata.ListAirportApp = listappAirports;
                //}
            }
            catch (Exception ex)
            {

                this.logging.Error("CityAirportsContent - GetAirportsData()" + ex.Message);
            }


            return appAirportdata;
        }




    }
}