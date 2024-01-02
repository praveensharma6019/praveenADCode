using Adani.SuperApp.Airport.Feature.C2CCreate.Platform.Models;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Newtonsoft.Json;
using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Layouts;
using Sitecore.SecurityModel;
using Sitecore.Tasks;
using System;
using System.IO;

namespace Adani.SuperApp.Airport.Feature.C2CCreate.Platform.Commands
{
    public class C2CPageCreate
    {
        private ILogRepository logRepository=new LogRepository();

        public void Execute(Item[] items, CommandItem commandItem, ScheduleItem scheduleItem)
        {
            try
            {
                string applicationPath = AppDomain.CurrentDomain.BaseDirectory;
                DirectoryInfo d = new DirectoryInfo(applicationPath + "\\App_Data\\files\\JSONData\\" + commandItem.DisplayName + "-" + DateTime.Today.Date.ToString("yyyy-dd-MM"));
                if (d.Exists)
                {
                    logRepository.Info("Total number of files processing -" + d.GetFiles("*.json").Length);
                    FileInfo[] files = d.GetFiles("*.json");
                    foreach (FileInfo file in files)
                    {
                        logRepository.Info("City2City Page creation command started..");
                        using (StreamReader r = new StreamReader(file.FullName))
                        {
                            string json = r.ReadToEnd();
                            C2CPageModel c2cPageModel = JsonConvert.DeserializeObject<C2CPageModel>(json);
                            if (c2cPageModel != null && !string.IsNullOrEmpty(c2cPageModel.PageTitle))
                            {
                                Database masterDB = Factory.GetDatabase("master");
                                string c2cPageName = c2cPageModel.PageTitle.Trim().Replace(' ', '-').ToLower();
                                ID branchId = masterDB.GetItem(Constants.Constants.BranchTemplateID).ID;
                                string commandName = commandItem.DisplayName;
                                Item parentItem = ((commandName == "Domestic") ? masterDB.GetItem(Constants.Constants.DomesticFlightsItem) : masterDB.GetItem(Constants.Constants.InternationalFlightsItem));
                                string parentPath = parentItem.Paths.FullPath;
                                parentPath = parentPath + "/" + commandItem.DisplayName + "-" + DateTime.Today.Date.ToString("yyyy-dd-MM");
                                Item parentPathItem = null;

                                if (masterDB.GetItem(parentPath) == null)
                                    parentPathItem = ItemManager.AddFromTemplate(commandItem.DisplayName + "-" + DateTime.Today.Date.ToString("yyyy-dd-MM"), masterDB.GetItem(Constants.Constants.FolderTemplateID).ID, parentItem);
                                else
                                    parentPathItem = masterDB.GetItem(parentPath);

                                Sitecore.Collections.ChildList itemList = parentPathItem.GetChildren();
                                bool flag = true;
                                foreach (Item currItem in itemList)
                                {
                                    if (currItem.Fields["ArrivalCity"].Value == c2cPageModel.DestinationCityObject.DCityName && currItem.Fields["DepartureCity"].Value == c2cPageModel.SourceCityObject.SCityName)
                                    {
                                        flag = false;
                                    }
                                }

                                if (flag)
                                {
                                    logRepository.Info($"City2City Page creation command started for {commandName} Flights..");
                                    Item c2cPageItem = ItemManager.AddFromTemplate(c2cPageName, branchId, parentPathItem);
                                    if (c2cPageItem != null)
                                    {
                                        // C2CPage processing start

                                        c2cPageItem.Editing.BeginEdit();

                                        c2cPageItem.Fields["__Display Name"].Value = c2cPageModel.PageTitle;
                                        c2cPageItem.Fields["Title"].Value = c2cPageModel.PageTitle;
                                        c2cPageItem.Fields["BannerTitle"].Value = c2cPageModel.PageTitle;
                                        c2cPageItem.Fields["BannerDescription"].Value = c2cPageModel.PageDescription;
                                        c2cPageItem.Fields["ArrivalCity"].Value = c2cPageModel.DestinationCityObject.DCityName;
                                        c2cPageItem.Fields["DepartureCity"].Value = c2cPageModel.SourceCityObject.SCityName;
                                        c2cPageItem.Fields["ResultsListTitle"].Value = "Flights from " + c2cPageModel.SourceCityObject.SCityName + " to " + c2cPageModel.DestinationCityObject.DCityName;

                                        //Meta section
                                        c2cPageItem.Fields["MetaTitle"].Value = "Book " + c2cPageModel.SourceCityObject.SCityName + " to " + c2cPageModel.DestinationCityObject.DCityName + " Flight Tickets at Lowest Price - Adani One";
                                        const string cityCode = "citycode";
                                        c2cPageItem.Fields["MetaDescription"].Value = "Looking for " + c2cPageModel.SourceCityObject.SCityName + " to " + c2cPageModel.DestinationCityObject.DCityName + " flights at low fare? Book your tickets with Adani One and get the best flight deals, cashback and discounts on " + c2cPageModel.SourceCityObject.SCityName + " (" + cityCode + ") to " + c2cPageModel.DestinationCityObject.DCityName + " (" + cityCode + ") flights.";
                                        c2cPageItem.Fields["Description"].Value = "Looking for " + c2cPageModel.SourceCityObject.SCityName + " to " + c2cPageModel.DestinationCityObject.DCityName + " flights at low fare? Book your tickets with Adani One and get the best flight deals, cashback and discounts on " + c2cPageModel.SourceCityObject.SCityName + " (" + cityCode + ") to " + c2cPageModel.DestinationCityObject.DCityName + " (" + cityCode + ") flights.";

                                        c2cPageItem.Fields["Canonical"].Value = String.Format("<link text=\"\" anchor=\"\" linktype=\"internal\" class=\"\" title=\"\" target=\"\" querystring=\"\" id=\"{0}\"/>", c2cPageItem.ID);
                                        c2cPageItem.Fields["OG-Title"].Value = c2cPageModel.PageTitle;
                                        c2cPageItem.Fields["OG-Description"].Value = c2cPageModel.PageDescription;

                                        //Image section
                                        var dImagepath = "/sitecore/media library/Project/CityToCity/Cities/" + c2cPageModel.DestinationCityObject.DCityName + "/" + c2cPageModel.DestinationCityObject.DCityName.ToLower();
                                        try
                                        {
                                            Item thumbnailImage = masterDB.GetItem(dImagepath);
                                            MediaItem mediaItem = new Sitecore.Data.Items.MediaItem(thumbnailImage);
                                            c2cPageItem.Fields["ArrivalCityImage"].Value = string.Format("<image mediaid=\"{0}\" alt=\"{1}\" />", mediaItem.ID, c2cPageModel.DestinationCityObject.DCityName);
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Image couldn't find with path : {dImagepath}");
                                        }

                                        var sImagepath = "/sitecore/media library/Project/CityToCity/Cities/" + c2cPageModel.SourceCityObject.SCityName + "/" + c2cPageModel.SourceCityObject.SCityName.ToLower();
                                        try
                                        {
                                            Item thumbnailImage = masterDB.GetItem(sImagepath);
                                            MediaItem mediaItem = new Sitecore.Data.Items.MediaItem(thumbnailImage);
                                            c2cPageItem.Fields["DepartureCityImage"].Value = string.Format("<image mediaid=\"{0}\" alt=\"{1}\" />", mediaItem.ID, c2cPageModel.SourceCityObject.SCityName);
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Image couldn't find with path : {sImagepath}");
                                        }

                                        //City Address Section

                                        try
                                        {
                                            c2cPageItem.Fields["AirportInfoTitle"].Value = c2cPageName;

                                            foreach (Item cityAddress in masterDB.GetItem(Constants.Constants.CityAddressFolderPath).Children)
                                            {
                                                if (cityAddress != null && cityAddress.Fields["CityName"].Value.ToLower() == c2cPageModel.DestinationCityObject.DCityName.Trim().ToLower())
                                                {
                                                    c2cPageItem.Fields["ArrivalCityAdddress"].Value = cityAddress.Fields["AirportAddress"].Value;
                                                }
                                                else if (cityAddress != null && cityAddress.Fields["CityName"].Value.ToLower() == c2cPageModel.SourceCityObject.SCityName.Trim().ToLower())
                                                {
                                                    c2cPageItem.Fields["DepartureCityAdddress"].Value = cityAddress.Fields["AirportAddress"].Value;
                                                }
                                            }

                                            c2cPageItem.Editing.EndEdit();
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Airport Address Section");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //Tabs Section
                                            string tabsPath = parentPath + "/" + c2cPageName + "/Tabs";
                                            Item tabsItem = masterDB.GetItem(tabsPath);
                                            Item serviceTabTemplateID = masterDB.GetItem(Constants.Constants.ServiceTabsTemplateID);
                                            if (tabsItem != null && serviceTabTemplateID != null)
                                            {
                                                string dTabsPath = tabsPath + "/" + c2cPageModel.DestinationCityObject.DCityName;
                                                Item aCityItem = ItemManager.AddFromTemplate(c2cPageModel.DestinationCityObject.DCityName, serviceTabTemplateID.ID, tabsItem);
                                                if (aCityItem != null)
                                                {
                                                    aCityItem.Editing.BeginEdit();
                                                    aCityItem.Fields["Title"].Value = c2cPageModel.DestinationCityObject.DCityName;
                                                    aCityItem.Fields["__Sortorder"].Value = "0";
                                                }

                                                string sTabsPath = tabsPath + "/" + c2cPageModel.SourceCityObject.SCityName;
                                                Item sCityItem = ItemManager.AddFromTemplate(c2cPageModel.SourceCityObject.SCityName, serviceTabTemplateID.ID, tabsItem);
                                                if (aCityItem != null)
                                                {
                                                    aCityItem.Editing.BeginEdit();
                                                    aCityItem.Fields["Title"].Value = c2cPageModel.SourceCityObject.SCityName;
                                                    aCityItem.Fields["__Sortorder"].Value = "1";
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Tabs Section");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //Update Airport Information
                                            string dAirportInformationPath = c2cPageItem.Paths.FullPath + "/LocalDS/Airport Information/AirportInformation";
                                            Item dAirportInformationItem = masterDB.GetItem(dAirportInformationPath);
                                            if (dAirportInformationItem != null)
                                            {
                                                dAirportInformationItem.Editing.BeginEdit();
                                                Item airportListItem = masterDB.GetItem(Constants.Constants.AirportsListPath);

                                                if (airportListItem != null)
                                                {
                                                    foreach (Item airportItem in airportListItem.Children)
                                                    {
                                                        if (airportItem.Name.Trim().ToLower() == c2cPageModel.SourceCityObject.SCityName.Trim().ToLower() || airportItem.Name.Trim().ToLower() == c2cPageModel.SourceCityObject.SCityName.Trim().ToLower())
                                                        {
                                                            dAirportInformationItem.Fields["SelectAirports"].Value += airportItem.ID.ToString() + "|";
                                                        }
                                                    }
                                                }
                                                dAirportInformationItem.Editing.EndEdit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Airport Information Update Section");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //Update Destination City AboutAirport Section
                                            string dAboutAirport = c2cPageItem.Paths.FullPath + "/LocalDS/ArrivalCityInformation/AboutAirport";
                                            Item dAboutAirportItem = masterDB.GetItem(dAboutAirport);

                                            if (dAboutAirportItem != null)
                                            {
                                                dAboutAirportItem.Editing.BeginEdit();
                                                dAboutAirportItem.Fields["Title"].Value = "About " + c2cPageModel.DestinationCityObject.DCityName + " Airport";
                                                dAboutAirportItem.Fields["Description"].Value = c2cPageModel.DestinationCityObject.DAirportDescription;
                                                string imagepath = "/sitecore/media library/Project/CityToCity/Cities/" + c2cPageModel.DestinationCityObject.DCityName + "/" + c2cPageModel.DestinationCityObject.DCityName.Trim().ToLower() + "-airport";
                                                try
                                                {
                                                    Item thumbnailImage = masterDB.GetItem(imagepath);
                                                    MediaItem mediaItem = new Sitecore.Data.Items.MediaItem(thumbnailImage);
                                                    dAboutAirportItem.Fields["Image"].Value = String.Format("<image mediaid='{0}' alt='{1}' />", mediaItem.ID, c2cPageModel.DestinationCityObject.DCityName);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                dAboutAirportItem.Editing.EndEdit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Destination City About Airport Section");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //Update Source City AboutAirport Section
                                            string sAboutAirport = c2cPageItem.Paths.FullPath + "/LocalDS/DepartureCityInformation/AboutAirport";
                                            Item sAboutAirportItem = masterDB.GetItem(sAboutAirport);

                                            if (sAboutAirportItem != null)
                                            {
                                                sAboutAirportItem.Editing.BeginEdit();
                                                sAboutAirportItem.Fields["Title"].Value = "About " + c2cPageModel.SourceCityObject.SCityName + " Airport";
                                                sAboutAirportItem.Fields["Description"].Value = c2cPageModel.SourceCityObject.SAirportDescription;
                                                string imagepath = "/sitecore/media library/Project/CityToCity/Cities/" + c2cPageModel.SourceCityObject.SCityName + "/" + c2cPageModel.SourceCityObject.SCityName.Trim().ToLower() + "-airport";
                                                try
                                                {
                                                    Item thumbnailImage = masterDB.GetItem(imagepath);
                                                    MediaItem mediaItem = new Sitecore.Data.Items.MediaItem(thumbnailImage);
                                                    sAboutAirportItem.Fields["Image"].Value = String.Format("<image mediaid='{0}' alt='{1}' />", mediaItem.ID, c2cPageModel.SourceCityObject.SCityName);
                                                }
                                                catch (Exception ex)
                                                {
                                                }
                                                sAboutAirportItem.Editing.EndEdit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Source City About Airport Section");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //Update Destination AboutCity Section
                                            string dAboutCity = c2cPageItem.Paths.FullPath + "/LocalDS/ArrivalCityInformation/AboutCity";
                                            Item dAboutCityItem = masterDB.GetItem(dAboutCity);

                                            if (dAboutCityItem != null)
                                            {
                                                dAboutCityItem.Editing.BeginEdit();
                                                dAboutCityItem.Fields["Text"].Value = "<h5>About " + c2cPageModel.DestinationCityObject.DCityName + "</h5><p>" + c2cPageModel.DestinationCityObject.DCityDescription + "</p>";
                                                dAboutCityItem.Editing.EndEdit();
                                            }

                                            //Update Source AboutCity Section
                                            string sAboutCity = c2cPageItem.Paths.FullPath + "/LocalDS/ArrivalCityInformation/AboutCity";
                                            Item sAboutCityItem = masterDB.GetItem(sAboutCity);

                                            if (sAboutCityItem != null)
                                            {
                                                sAboutCityItem.Editing.BeginEdit();
                                                sAboutCityItem.Fields["Text"].Value = "<h5>About " + c2cPageModel.SourceCityObject.SCityName + "</h5><p>" + c2cPageModel.SourceCityObject.SCityDescription + "</p>";
                                                sAboutCityItem.Editing.EndEdit();
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Source/Destination About City Section");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //Update Banner Section
                                            string bannerPath = c2cPageItem.Paths.FullPath + "/LocalDS/CityToCityBanner/Banner";
                                            Item bannerItem = masterDB.GetItem(bannerPath);
                                            string bannerImage = "Hero Image";

                                            if (bannerItem != null)
                                            {
                                                bannerItem.Editing.BeginEdit();
                                                bannerItem.Fields["Title"].Value = c2cPageModel.PageTitle;

                                                var bannerImagepath = "/sitecore/media library/Project/CityToCity/Banner Images/" + bannerImage;
                                                try
                                                {
                                                    Item thumbnailImage = masterDB.GetItem(bannerImagepath);
                                                    MediaItem mediaItem = new Sitecore.Data.Items.MediaItem(thumbnailImage);
                                                    bannerItem.Fields["Image"].Value = string.Format("<image mediaid='{0}' alt='{1}' />", mediaItem.ID, c2cPageModel.PageTitle);
                                                }
                                                catch (Exception ex)
                                                {
                                                    logRepository.Error($"Issue banner image.");
                                                    logRepository.Error(ex.Message);
                                                }
                                                bannerItem.Editing.EndEdit();
                                            }
                                        }
                                        catch (Exception ex)
                                        { }

                                        try
                                        {
                                            //Plcaes To Visit Section - Start

                                            if (c2cPageModel.DestinationCityObject.DPlacesToVisitList != null && c2cPageModel.DestinationCityObject.DPlacesToVisitList.Count > 2)
                                            {
                                                //string sCarousalPath = "master:/sitecore/content/AirportHome/domestic-flights/" + c2cPageName + "/LocalDS/DCCrousals";
                                                string dCarousalPath = "master:/sitecore/content/AirportHome/domestic-flights/" + c2cPageName + "/LocalDS/ACCrousals";
                                                //string sPlacesToVisitCarousalPath = parentPath + "/" + c2cPageName + "/LocalDS/DepartureCityInformation/PlaceToVisitCarousal";
                                                string dPlacesToVisitCarousalPath = parentPath + "/" + c2cPageName + "/LocalDS/ArrivalCityInformation/PlaceToVisitCarousal";

                                                foreach (PlacesToVisit placeToVisit in c2cPageModel.DestinationCityObject.DPlacesToVisitList)
                                                {
                                                    string imagepath = "/sitecore/media library/Project/CityToCity/Carousal/" + c2cPageModel.DestinationCityObject.DCityName + '/' + c2cPageModel.DestinationCityObject.DCityName.ToLower() + "-" + placeToVisit.PlaceName.Trim().Replace(' ', '-').TrimEnd(':').ToLower();
                                                    try
                                                    {
                                                        Item thumbnailImage = masterDB.GetItem(imagepath);

                                                        if (thumbnailImage != null)
                                                        {
                                                            string aCarousalitemPath = dCarousalPath + "/" + placeToVisit.PlaceName.Trim();
                                                            Item aCarousalItem = masterDB.GetItem(aCarousalitemPath);
                                                            if (aCarousalItem == null)
                                                            {
                                                                Item aCarousalNewItem = ItemManager.AddFromTemplate(placeToVisit.PlaceName.Trim(), ID.Parse(Constants.Constants.PlacesToVisitCardTemplateID), masterDB.GetItem(dCarousalPath));
                                                                if (aCarousalNewItem != null)
                                                                {
                                                                    aCarousalNewItem.Editing.BeginEdit();
                                                                    aCarousalNewItem.Fields["Title"].Value = placeToVisit.PlaceName;
                                                                    aCarousalNewItem.Fields["Description"].Value = placeToVisit.PlaceDescription;
                                                                    string carousalImagepath = "/sitecore/media library/Project/CityToCity/Carousal/" + c2cPageModel.DestinationCityObject.DCityName + '/' + c2cPageModel.DestinationCityObject.DCityName.ToLower() + "-" + placeToVisit.PlaceName.Trim().Replace(' ', '-').TrimEnd(':').ToLower();
                                                                    try
                                                                    {
                                                                        Item carousalImageItem = masterDB.GetItem(carousalImagepath);
                                                                        MediaItem mediaItem = new Sitecore.Data.Items.MediaItem(carousalImageItem);
                                                                        aCarousalNewItem.Fields["Image"].Value = string.Format("<image mediaid='{0}' alt='{1}' />", mediaItem.ID, placeToVisit.PlaceName);
                                                                    }
                                                                    catch
                                                                    {
                                                                    }
                                                                    aCarousalNewItem.Editing.EndEdit();
                                                                }
                                                            }
                                                            else if (aCarousalItem != null)
                                                            {
                                                                aCarousalItem.Editing.BeginEdit();
                                                                aCarousalItem.Fields["Title"].Value = placeToVisit.PlaceName;
                                                                aCarousalItem.Fields["Description"].Value = placeToVisit.PlaceDescription;
                                                                aCarousalItem.Editing.EndEdit();
                                                            }
                                                        }
                                                        else
                                                        {
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                            }

                                            if (c2cPageModel.SourceCityObject.SPlacesToVisitList != null && c2cPageModel.SourceCityObject.SPlacesToVisitList.Count > 2)
                                            {
                                                string sCarousalPath = "master:/sitecore/content/AirportHome/domestic-flights/" + c2cPageName + "/LocalDS/DCCrousals";
                                                //string dCarousalPath = "master:/sitecore/content/AirportHome/domestic-flights/" + c2cPageName + "/LocalDS/ACCrousals";
                                                string sPlacesToVisitCarousalPath = parentPath + "/" + c2cPageName + "/LocalDS/DepartureCityInformation/PlaceToVisitCarousal";
                                                //string dPlacesToVisitCarousalPath = parentPath + "/" + c2cPageName + "/LocalDS/ArrivalCityInformation/PlaceToVisitCarousal";

                                                foreach (PlacesToVisit placeToVisit in c2cPageModel.SourceCityObject.SPlacesToVisitList)
                                                {
                                                    string imagepath = "/sitecore/media library/Project/CityToCity/Carousal/" + c2cPageModel.SourceCityObject.SCityName + '/' + c2cPageModel.SourceCityObject.SCityName.ToLower() + "-" + placeToVisit.PlaceName.Trim().Replace(' ', '-').TrimEnd(':').ToLower();
                                                    try
                                                    {
                                                        Item thumbnailImage = masterDB.GetItem(imagepath);

                                                        if (thumbnailImage != null)
                                                        {
                                                            string sCarousalitemPath = sCarousalPath + "/" + placeToVisit.PlaceName.Trim();
                                                            Item sCarousalItem = masterDB.GetItem(sCarousalitemPath);
                                                            if (sCarousalItem == null)
                                                            {
                                                                Item sCarousalNewItem = ItemManager.AddFromTemplate(placeToVisit.PlaceName.Trim(), ID.Parse(Constants.Constants.PlacesToVisitCardTemplateID), masterDB.GetItem(sCarousalPath));
                                                                if (sCarousalNewItem != null)
                                                                {
                                                                    sCarousalNewItem.Editing.BeginEdit();
                                                                    sCarousalNewItem.Fields["Title"].Value = placeToVisit.PlaceName;
                                                                    sCarousalNewItem.Fields["Description"].Value = placeToVisit.PlaceDescription;
                                                                    string carousalImagepath = "/sitecore/media library/Project/CityToCity/Carousal/" + c2cPageModel.DestinationCityObject.DCityName + '/' + c2cPageModel.DestinationCityObject.DCityName.ToLower() + "-" + placeToVisit.PlaceName.Trim().Replace(' ', '-').TrimEnd(':').ToLower();
                                                                    try
                                                                    {
                                                                        Item carousalImageItem = masterDB.GetItem(carousalImagepath);
                                                                        MediaItem mediaItem = new Sitecore.Data.Items.MediaItem(carousalImageItem);
                                                                        sCarousalNewItem.Fields["Image"].Value = string.Format("<image mediaid='{0}' alt='{1}' />", mediaItem.ID, placeToVisit.PlaceName);
                                                                    }
                                                                    catch
                                                                    {
                                                                    }
                                                                    sCarousalNewItem.Editing.EndEdit();
                                                                }
                                                            }
                                                            else if (sCarousalItem != null)
                                                            {
                                                                sCarousalItem.Editing.BeginEdit();
                                                                sCarousalItem.Fields["Title"].Value = placeToVisit.PlaceName;
                                                                sCarousalItem.Fields["Description"].Value = placeToVisit.PlaceDescription;
                                                                sCarousalItem.Editing.EndEdit();
                                                            }
                                                        }
                                                        else
                                                        {
                                                        }
                                                    }
                                                    catch (Exception ex)
                                                    {

                                                    }
                                                }
                                            }

                                            //Plcaes To Visit Section - End--------------xxxxxxx---------------xxxxxxxxxx--------
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within PlacesToVisit Section");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //FAQ Section Start

                                            string faqPath = c2cPageItem.Paths.FullPath + "/LocalDS/FAQ";
                                            if (c2cPageModel.Faqs != null && c2cPageModel.Faqs.Count > 0)
                                            {
                                                foreach (Faqs faq in c2cPageModel.Faqs)
                                                {
                                                    Item faqItem = masterDB.GetItem(faqPath + faq.Question);

                                                    if (faqItem == null)
                                                    {
                                                        Item newFAQItem = ItemManager.AddFromTemplate(faq.Question.Trim(), masterDB.GetItem(Constants.Constants.FAQTemplateID.ToString()).ID, masterDB.GetItem(faqPath));

                                                        if (newFAQItem != null)
                                                        {
                                                            newFAQItem.Editing.BeginEdit();
                                                            newFAQItem.Fields["AccordionTitle"].Value = faq.Question.Trim();
                                                            newFAQItem.Fields["AccordionDescription"].Value = faq.Answer.Trim();
                                                            newFAQItem.Editing.EndEdit();
                                                        }

                                                    }
                                                    else if (faqItem != null)
                                                    {
                                                        faqItem.Editing.BeginEdit();
                                                        faqItem.Fields["AccordionTitle"].Value = faq.Question.Trim();
                                                        faqItem.Fields["AccordionDescription"].Value = faq.Answer.Trim();
                                                        faqItem.Editing.EndEdit();
                                                    }
                                                }
                                            }

                                            //FAQ Section End------------xxxxxxxxxxxxx-------------xxxxxxxxxxxxxxx----------
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within FAQ Section");
                                            logRepository.Error(ex.Message);
                                        }

                                        #region Add Renderings
                                        try
                                        {
                                            //Source City Carousal
                                            string sCarousalListPath = c2cPageItem.Paths.FullPath + "/LocalDS/DCCrousals";
                                            ChildList sAllItem = masterDB.GetItem(sCarousalListPath) != null ? masterDB.GetItem(sCarousalListPath).Children : null;

                                            if (sAllItem != null && sAllItem.Count > 0)
                                            {
                                                Item sPlacesToVisitCarousal = masterDB.GetItem(c2cPageItem.Paths.FullPath + "/LocalDS/DepartureCityInformation/PlaceToVisitCarousal");

                                                if (sPlacesToVisitCarousal != null)
                                                {
                                                    sPlacesToVisitCarousal.Editing.BeginEdit();
                                                    sPlacesToVisitCarousal.Fields["Title"].Value = "Best Places to Visit in " + c2cPageModel.SourceCityObject.SCityName;
                                                    //sPlacesToVisitCarousal.Fields["Description"].Value = 

                                                    foreach (Item cItem in sAllItem)
                                                    {
                                                        sPlacesToVisitCarousal.Fields["Select Card"].Value += cItem.ID.ToString() + "|";
                                                    }
                                                    sPlacesToVisitCarousal.Editing.EndEdit();
                                                }
                                            }

                                            //Destination City Carousal
                                            string dCarousalListPath = c2cPageItem.Paths.FullPath + "/LocalDS/ACCrousals";
                                            ChildList dAllItem = masterDB.GetItem(dCarousalListPath) != null ? masterDB.GetItem(dCarousalListPath).Children : null;

                                            if (dAllItem != null && dAllItem.Count > 0)
                                            {
                                                Item dPlacesToVisitCarousal = masterDB.GetItem(c2cPageItem.Paths.FullPath + "/LocalDS/ArrivalCityInformation/PlaceToVisitCarousal");

                                                if (dPlacesToVisitCarousal != null)
                                                {
                                                    dPlacesToVisitCarousal.Editing.BeginEdit();
                                                    dPlacesToVisitCarousal.Fields["Title"].Value = "Best Places to Visit in " + c2cPageModel.SourceCityObject.SCityName;
                                                    //sPlacesToVisitCarousal.Fields["Description"].Value = 

                                                    foreach (Item cItem in sAllItem)
                                                    {
                                                        dPlacesToVisitCarousal.Fields["Select Card"].Value += cItem.ID.ToString() + "|";
                                                    }
                                                    dPlacesToVisitCarousal.Editing.EndEdit();
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - Carousal");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //Top Domestic and International Flights section
                                            string flightRoutePath = string.Empty;
                                            Item flightRouteItem = null;
                                            ChildList flightRouteList = null;
                                            if (commandName == "Domestic")
                                            {
                                                flightRoutePath = c2cPageItem.Paths.FullPath + "/LocalDS/TopDomesticFlightRoutes/TopDomesticFlightRoutes";
                                                flightRouteItem = masterDB.GetItem(flightRoutePath);
                                                if (flightRouteItem != null)
                                                {
                                                    flightRouteItem.Editing.BeginEdit();
                                                    flightRouteItem.Fields["TopDomesticFlightsTitle"].Value = "Top Domestic Flights Routes";

                                                    flightRouteList = masterDB.GetItem("/sitecore/content/AirportHome/Datasource/Adani/StaticPages/CityToCity/PopularFlightsList")?.Children;
                                                    foreach (Item dItem in flightRouteList)
                                                    {
                                                        flightRouteItem.Fields["SelectRoutes"].Value += dItem.ID.ToString() + "|";
                                                    }
                                                    flightRouteItem.Editing.EndEdit();
                                                }
                                            }
                                            else if (commandName == "International")
                                            {
                                                flightRoutePath = c2cPageItem.Paths.FullPath + "/LocalDS/TopInternationalFlightRoutes/TopInternationalFlightRoutes";
                                                flightRouteItem = masterDB.GetItem(flightRoutePath);
                                                if (flightRouteItem != null)
                                                {
                                                    flightRouteItem.Editing.BeginEdit();
                                                    flightRouteItem.Fields["TopDomesticFlightsTitle"].Value = "Top International Flights Routes";

                                                    flightRouteList = masterDB.GetItem("/sitecore/content/AirportHome/Datasource/Adani/StaticPages/CityToCity/InternationalPopularFlightsList")?.Children;
                                                    foreach (Item iItem in flightRouteList)
                                                    {
                                                        flightRouteItem.Fields["SelectRoutes"].Value += iItem.ID.ToString() + "|";
                                                    }
                                                    flightRouteItem.Editing.EndEdit();
                                                }
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - Top DOmestic/International Flights");
                                            logRepository.Error(ex.Message);
                                        }

                                        string dsPath = string.Empty;
                                        Item dsItem = null;
                                        string placeholderName = string.Empty;
                                        int posCnt = 1;
                                        ID renderingID = null;
                                        string parameters = string.Empty;

                                        try
                                        {
                                            //Rendering - Tabs
                                            dsPath = c2cPageItem.Paths.FullPath + "/Tabs";
                                            dsItem = masterDB.GetItem(dsPath);
                                            placeholderName = "tabs";
                                            renderingID = new ID(Constants.Constants.TabsRenderingID);
                                            AddRendering(c2cPageItem, renderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty);
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - Tabs");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //CityToCity Banner
                                            dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/CityToCityBanner/Banner";
                                            dsItem = masterDB.GetItem(dsPath);
                                            placeholderName = "banner";
                                            renderingID = new ID(Constants.Constants.BannerRenderingID);
                                            AddRendering(c2cPageItem, renderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty);
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - Banner");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //TitleDescriptionWithImageRIghtAligned
                                            parameters = string.Format("{0} = {1}", "className", "rounded videoSec");
                                            dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/ArrivalCityInformation/AboutAirport";
                                            dsItem = masterDB.GetItem(dsPath);
                                            placeholderName = "arrivalcity";
                                            renderingID = new ID(Constants.Constants.TitleDescriptionWithImageRIghtAlignedID);
                                            AddRendering(c2cPageItem, renderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty, parameters);
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - TitleDescriptionWithImageRIghtAligned");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //TextComponent - AboutCity
                                            dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/ArrivalCityInformation/AboutCity";
                                            dsItem = masterDB.GetItem(dsPath);
                                            placeholderName = "baggage";
                                            renderingID = new ID(Constants.Constants.TextComponentID);
                                            AddRendering(c2cPageItem, renderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty);
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - AboutCity");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //PlacesToVisit
                                            dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/ArrivalCityInformation/PlaceToVisitCarousal";
                                            dsItem = masterDB.GetItem(dsPath);
                                            placeholderName = "baggage";
                                            renderingID = new ID(Constants.Constants.PlacesToVisitID);
                                            AddRendering(c2cPageItem, renderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty);
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - PlaceToVisit");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //TitleDescriptionWithImageRIghtAligned
                                            parameters = string.Format("{0} = {1}", "className", "rounded videoSec");
                                            dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/DepartureCityInformation/AboutAirport";
                                            dsItem = masterDB.GetItem(dsPath);
                                            placeholderName = "departurecity";
                                            renderingID = new ID(Constants.Constants.TitleDescriptionWithImageRIghtAlignedID);
                                            AddRendering(c2cPageItem, renderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty, parameters);
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - TitleDescriptionWithImageRIghtAligned");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //TextComponent - AboutCity
                                            dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/DepartureCityInformation/AboutCity";
                                            dsItem = masterDB.GetItem(dsPath);
                                            placeholderName = "departurecity";
                                            renderingID = new ID(Constants.Constants.TextComponentID);
                                            AddRendering(c2cPageItem, renderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty);
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - AboutCity");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //PlacesToVisit
                                            dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/DepartureCityInformation/PlaceToVisitCarousal";
                                            dsItem = masterDB.GetItem(dsPath);
                                            placeholderName = "departurecity";
                                            renderingID = new ID(Constants.Constants.PlacesToVisitID);
                                            AddRendering(c2cPageItem, renderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty);
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - PlaceToVisit");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //add TopDomesticFlight / TopInternational Rendring
                                            if (commandName == "International")
                                            {
                                                dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/TopInternationalFlightRoutes/TopInternationalFlightRoutes";
                                                dsItem = masterDB.GetItem(dsPath);
                                                placeholderName = "topdomestic";
                                                renderingID = new ID(Constants.Constants.InternationalFlightID);
                                                AddRendering(c2cPageItem, renderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty);
                                            }
                                            else if (commandName == "Domestic")
                                            {
                                                dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/TopDomesticFlightRoutes/TopDomesticFlightRoutes";
                                                dsItem = masterDB.GetItem(dsPath);
                                                placeholderName = "topdomestic";
                                                renderingID = new ID(Constants.Constants.DomesticFlightID);
                                                AddRendering(c2cPageItem, renderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - TopDomesticFlight / TopInternational Rendring");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //FAQ
                                            dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/FAQ";
                                            dsItem = masterDB.GetItem(dsPath);
                                            placeholderName = "faq";
                                            renderingID = new ID(Constants.Constants.FAQID);
                                            AddRendering(c2cPageItem, renderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty);
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - FAQ");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //add TopDomesticFlight / TopInternational Mobile Rendring
                                            if (commandName == "International")
                                            {
                                                dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/TopInternationalFlightRoutes/TopInternationalFlightRoutes";
                                                dsItem = masterDB.GetItem(dsPath);
                                                placeholderName = "faq";
                                                ID irenderingID = new ID(Constants.Constants.DomesticFlightMobileID);
                                                AddRendering(c2cPageItem, irenderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty);
                                            }
                                            else if (commandName == "Domestic")
                                            {
                                                dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/TopDomesticFlightRoutes/TopDomesticFlightRoutes";
                                                dsItem = masterDB.GetItem(dsPath);
                                                placeholderName = "faq";
                                                ID drenderingID = new ID(Constants.Constants.DomesticFlightMobileID);
                                                AddRendering(c2cPageItem, drenderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty);
                                            }
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - TopDomesticFlight / TopInternational Mobile Rendring");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //AirportInformation Mobile
                                            dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/Airport Information/AirportInformation";
                                            dsItem = masterDB.GetItem(dsPath);
                                            placeholderName = "faq";
                                            renderingID = new ID(Constants.Constants.AirportInfoMobileID);
                                            AddRendering(c2cPageItem, renderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty);
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - AirportInformation Mobile");
                                            logRepository.Error(ex.Message);
                                        }

                                        try
                                        {
                                            //AirportInformation
                                            dsPath = c2cPageItem.Paths.FullPath + "/LocalDS/Airport Information/AirportInformation";
                                            dsItem = masterDB.GetItem(dsPath);
                                            placeholderName = "airportinfo";
                                            renderingID = new ID(Constants.Constants.AirportInfoID);
                                            AddRendering(c2cPageItem, renderingID, posCnt++, placeholderName, dsItem != null ? dsItem.ID.ToString() : string.Empty);
                                        }
                                        catch (Exception ex)
                                        {
                                            logRepository.Error($"Issue within Add Rendering - AirportInformation");
                                            logRepository.Error(ex.Message);
                                        }

                                        #endregion

                                        // C2CPage processing end
                                    }
                                }
                            }
                            else
                            {
                                //logRepository.Info("City2City PageName couldnot be blank, Item did not created !!");
                            }
                        }
                    }
                }
                else
                {
                    logRepository.Error("Folder with data does not exist !!");
                }

            }
            catch (Exception ex)
            {
                logRepository.Error(ex.Message);
            }
        }

        public void AddRendering(Item item, ID renderingId, int renderingPosition, string placeholder, string datasource = "", string parameters = "")
        {
            if (item != null)
            {
                Database masterDB = Factory.GetDatabase("master");

                /// Get the layout definitions and the device definition
                LayoutField layoutField = new LayoutField(item.Fields[Sitecore.FieldIDs.FinalLayoutField]);
                LayoutDefinition layoutDefinition = LayoutDefinition.Parse(layoutField.Value);

                /// /sitecore/layout/Devices/Default
                string defaultDeviceId = "{FE5D7FDF-89C0-4D99-9AA3-B5FBD009C9F3}";

                DeviceDefinition deviceDefinition = layoutDefinition.GetDevice(defaultDeviceId);
                DeviceItem deviceItem = new DeviceItem(masterDB.GetItem(defaultDeviceId));
                if (deviceDefinition != null && deviceItem != null)
                {

                    /// Create a RenderingDefinition and add the reference of sublayout or rendering
                    RenderingDefinition renderingDefinition = new RenderingDefinition();
                    renderingDefinition.ItemID = renderingId.ToString();

                    /// Set placeholder where the rendering should be displayed
                    renderingDefinition.Placeholder = placeholder;

                    /// Set the datasource of sublayout, if any
                    renderingDefinition.Datasource = datasource;

                    /// Get all added renderings
                    RenderingReference[] renderings = item.Visualization.GetRenderings(deviceItem, true);

                    if (renderingPosition < 0 || renderings == null || renderings.Length <= 0 || renderingPosition >= renderings.Length)
                    {
                        /// Add rendering to end of list
                        deviceDefinition.AddRendering(renderingDefinition);
                    }
                    else
                    {
                        /// Add rendering at specified index
                        deviceDefinition.Insert(renderingPosition, renderingDefinition);
                    }

                    /// Save the layout changes
                    using (new SecurityDisabler())
                    {
                        item.Editing.BeginEdit();
                        layoutField.Value = layoutDefinition.ToXml();
                        item.Editing.EndEdit();
                    }
                }
            }
        }
    }
}
