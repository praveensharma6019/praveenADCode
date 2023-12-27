using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Results;
using Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Models;
using Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Services;
using Adani.SuperApp.Airport.Foundation.DataAPI.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Search.Platform.Services;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Newtonsoft.Json;
using Sitecore.Caching;
using Sitecore.Configuration;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Adani.SuperApp.Airport.Feature.OfferSearch.Platform.Command
{
    public class OfferDataSet
    {
        private IAPIResponse lowestFareResponse;
        LogRepository _logRepository = new LogRepository();
        private readonly IHelper _helper = new Helper();
        private readonly ISearchBuilder searchBuilder = new SearchBuilder();
        IQueryable<SearchResultItem> results;
        int index = 1;
        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            // _logRepository.Info("Offer scheduled task is being run!");
            SetOfferDataC2C();
        }

        private async void SetOfferDataC2C()
        {
            try
            {
                ResponseDataOld responseData = new ResponseDataOld();
                ResultDataOld resultData = new ResultDataOld();
                OfferFilters offerFilters = new OfferFilters();
                OfferFilters_old filter = new OfferFilters_old();
                filter.LOB = "flightbooking";
                filter.AirportCode = String.Empty;

                Services.OfferDataParser offerDataParser = new Services.OfferDataParser(_logRepository, searchBuilder, _helper);

                _logRepository.Info("C2C Offer Data update started .. !!");

                IQueryable<SearchResultItem> results = await searchBuilder.GetOfferSearchResultsAsync(Services.OfferPredicateBuilder_old.GetOfferSetPredicate(filter));

                if (results != null && results.Count() > 0)
                {
                    _logRepository.Info("C2C Offer Search Data object with valid data present.");
                }
                else
                {
                    _logRepository.Info("C2C Offer Search Data object with not a valid data.");
                }
                List<object> finalResult = await offerDataParser.ParseOfferAsync(results, offerFilters);

                if (results != null && results.Count() > 0)
                {
                    int maxOfferDiscount = 0;
                    float lowestFarePrice = 0;

                    OfferMapping offerObject = (OfferMapping)finalResult
                        .OrderByDescending(x => ((OfferMapping)x).OfferDiscountPrice)
                        .FirstOrDefault();

                    if (offerObject != null && offerObject.OfferDiscountPrice > 0)
                    {
                        maxOfferDiscount = offerObject.OfferDiscountPrice;
                    }

                    string contextDB = "web";
                    string currentdb = GetContextIndexValue(contextDB, "domestic-flights");
                    using (var searchContext = ContentSearchManager.GetIndex(currentdb).CreateSearchContext())
                    {
                        if (searchContext != null)
                        {
                            //Find the item from the index using name and template id
                            ID itemTemplateID = new Sitecore.Data.ID(new Guid(Constant.CitytoCItyTemplateID));

                            List<SearchResultItem> searchResult = null;
                            await Task.Run(async () =>
                            {
                                await Task.Delay(10000);
                                searchResult = searchContext.GetQueryable<SearchResultItem>().
                                Where(
                                x => x.TemplateId == itemTemplateID
                                ).ToList();
                            });
                            //x => x.ItemId == ID.Parse("{C959DC6D-DE92-4436-95F7-7264115031DD}") //Delhi-Mumbai
                            //|| x.TemplateId == itemTemplateID
                            //&& x.ItemId == ID.Parse("{BF8CBEFC-0EBD-46BE-B193-34550968E235}") //Goa-Delhi
                            //|| x.TemplateId == itemTemplateID
                            //&& x.ItemId == ID.Parse("{282D78DD-E757-4D1B-B3B9-21705A544D39}") //Ahmedabad-Delhi
                            //|| x.TemplateId == itemTemplateID
                            //&& x.ItemId == ID.Parse("{3AF6004E-D35D-4A6B-BB0A-EBFD4AF954F4}") //Delhi-Ahemadabad
                            //|| x.TemplateId == itemTemplateID
                            //&& x.ItemId == ID.Parse("{742A47DE-EC68-4767-ADF5-3E66B100824F}") //Hyderabad-Delhi
                            //|| x.TemplateId == itemTemplateID
                            //&& x.ItemId == ID.Parse("{0CC642A4-4F36-4920-A3EF-E8A43BF14A8F}") //Delhi-Hyderabad
                            //).ToList();

                            if (searchResult != null)
                            {
                                _logRepository.Info("Retrieved Domestic-Flights data with count: " + searchResult.Count);
                                foreach (SearchResultItem searchItem in searchResult)
                                {
                                    Database masterDB = Factory.GetDatabase("master");
                                    Item reqResultItem = null;
                                    Item resultItem = null;
                                    string pageItem = String.Empty;
                                    SearchResultItem respPageItem = null;

                                    reqResultItem = masterDB.GetItem(searchItem.GetItem().ID);
                                    resultItem = null;

                                    if (reqResultItem != null && !reqResultItem["Title"].Equals(string.Format("{0} to {1} Flights @ ₹{2}", reqResultItem.Fields["DepartureCity"].Value, reqResultItem.Fields["ArrivalCity"].Value, Convert.ToString(lowestFarePrice))))
                                    {
                                        _logRepository.Info("Process started for item: ", reqResultItem.ID);
                                        //await Task.Run(async () =>
                                        //{
                                        //    await Task.Delay(2000);
                                            var departCityCode = (!string.IsNullOrEmpty(reqResultItem.Fields["DepartureCityCode"].Value)) ? reqResultItem.Fields["DepartureCityCode"].Value : String.Empty;
                                            var arrivalCityCode = (!string.IsNullOrEmpty(reqResultItem.Fields["ArrivalCityCode"].Value)) ? reqResultItem.Fields["ArrivalCityCode"].Value : String.Empty;
                                            var originCode = "IN";
                                            var destCode = "IN";
                                            var adt = "1";
                                            //var couponCode = (!string.IsNullOrEmpty(resultItem.Fields["Promotion Code"].Value)) ? resultItem.Fields["Promotion Code"].Value : String.Empty;
                                            var couponCode = (!string.IsNullOrEmpty(offerObject.promotionCode)) ? offerObject.promotionCode : String.Empty;
                                            var today = DateTime.Today;
                                            var deptDate = today.AddDays(1).Date.ToString("dd-MM-yyyy");
                                            var returnDate = today.AddDays(1).Date.ToString("dd-MM-yyyy");
                                        //var returnDate = "14-08-2024";

                                        if (!string.IsNullOrEmpty(departCityCode) && !string.IsNullOrEmpty(arrivalCityCode))
                                        {
                                            _logRepository.Info("Call to get lowest-fair.");

                                            List<FareCalendar> resp = await GetLowestFareFromService(departCityCode, arrivalCityCode, originCode, destCode, adt, deptDate, returnDate);

                                            if (resp != null && resp.Count > 0)
                                            {
                                                _logRepository.Info("Retrieved valid lowest-fair response from service.");
                                                FareCalendar fareCalendar = resp.FirstOrDefault();
                                                if (fareCalendar != null && fareCalendar.prices != null && fareCalendar.prices.Count > 0)
                                                {
                                                    Price price = fareCalendar.prices.OrderBy(x => x.amount).FirstOrDefault();
                                                    if (price != null && price.amount > 0)
                                                    {
                                                        _logRepository.Info("Retrieved lowest-fair object, proceeding to get actual item.");
                                                        if (!string.IsNullOrEmpty(price.from) && !string.IsNullOrEmpty(price.to))
                                                        {
                                                            //string fromCity = await GetAirportCityName(price.from);
                                                            //string toCity = await GetAirportCityName(price.to);
                                                            //pageItem = string.Format("{0}-to-{1}-flights", fromCity.ToLower(), toCity.ToLower());
                                                            //resultItem = masterDB.GetItem(pageItem);
                                                            lowestFarePrice = price.amount;
                                                            var offerPercentValue = Convert.ToInt32(((lowestFarePrice * offerObject.OfferDiscountPercent) / 100));
                                                            maxOfferDiscount = (offerPercentValue > maxOfferDiscount) ? offerPercentValue : maxOfferDiscount;
                                                        }
                                                        else
                                                        {
                                                            _logRepository.Info("FarePrice object returns invalid Airport code.");
                                                        }
                                                    }
                                                    else
                                                    {
                                                        _logRepository.Info("FarePrice amount is not valid amount.");
                                                    }
                                                }
                                                else
                                                {
                                                    _logRepository.Info("FareCalendar object null or no valid price data.");
                                                }

                                                //await Task.Run(async () =>
                                                //{
                                                //    await Task.Delay(300);
                                                //    respPageItem = searchContext.GetQueryable<SearchResultItem>().
                                                //    Where(x => x.TemplateId == itemTemplateID && x.Name == pageItem).
                                                //    FirstOrDefault();
                                                //});

                                                //if (respPageItem != null)
                                                //{
                                                //    resultItem = masterDB.GetItem(respPageItem.GetItem().ID);
                                                //}
                                                resultItem = reqResultItem;
                                                if (lowestFarePrice > 0 && maxOfferDiscount > 0 && resultItem != null)
                                                {
                                                    CacheManager.Enabled = false;
                                                    using (new Sitecore.SecurityModel.SecurityDisabler())
                                                    {
                                                        if (offerObject.title.Contains("₹") && offerObject.title.Contains("%") && offerObject.title.ToLower().Contains("upto"))
                                                        {
                                                            resultItem = resultItem.Versions.AddVersion();
                                                            resultItem.Editing.BeginEdit();

                                                            //{Origin} to {Destination} Flights, Ticket Price @ {Lowest_Price} + {Discount price %} Off Upto Rs. {Maximum capping amount} – Adani One
                                                            resultItem["Title"] = string.Format("{0} to {1} Flights, Ticket Price @ ₹{2} + {3}% Off Upto Rs. {4}  - Adani One", resultItem.Fields["DepartureCity"].Value, resultItem.Fields["ArrivalCity"].Value, Convert.ToString(lowestFarePrice), resultItem.Fields["DiscountPercent"].Value, Convert.ToString(maxOfferDiscount));

                                                            //Book {Origin} to {Destination} Flights, Ticket Price @ {Lowest_Price} + {Discount price %} Off Upto Rs. {Maximum capping amount} – Adani One
                                                            resultItem["MetaTitle"] = string.Format("Book {0} to {1} Flights, Ticket Price @ ₹{2} + {3}% Off Upto Rs. {4}  - Adani One", resultItem.Fields["DepartureCity"].Value, resultItem.Fields["ArrivalCity"].Value, Convert.ToString(lowestFarePrice), resultItem.Fields["DiscountPercent"].Value, Convert.ToString(maxOfferDiscount));

                                                            //{Origin} to {Destination} Flights @ {Lowest_Price} – Get best airfare on flights from {Origin} ({Origin_Airport_Code}) to {Destination} ({Destination_Airport_Code}). Use Coupon Code {Code_Name} and get discount upto {Discount_Price} and earn reward points on {Origin} to {Destination} flight booking with Adani One.
                                                            resultItem["MetaDescription"] = string.Format("{0} to {1} Flights @ {2} – Get best airfare on flights from {3} ({4}) to {5} ({6}). Use Coupon Code {7} and get discount upto {8} and earn reward points on {9} to {10} flight booking with Adani One.",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                Convert.ToString(lowestFarePrice),
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                departCityCode,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                arrivalCityCode,
                                                                couponCode,
                                                                maxOfferDiscount,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            //{Origin} to {Destination} Flights @ {Lowest_Price} – Get best airfare on flights from {Origin} ({Origin_Airport_Code}) to {Destination} ({Destination_Airport_Code}). Use Coupon Code {Code_Name} and get discount upto {Discount_Price} and earn reward points on {Origin} to {Destination} flight booking with Adani One.
                                                            resultItem["Description"] = string.Format("{0} to {1} Flights @ ₹{2} – Get best airfare on flights from {3} ({4}) to {5} ({6}). Use Coupon Code {7} and get discount upto {8} and earn reward points on {9} to {10} flight booking with Adani One.",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                Convert.ToString(lowestFarePrice),
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                departCityCode,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                arrivalCityCode,
                                                                couponCode,
                                                                maxOfferDiscount,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            //{Origin} to {Destination} flights, flights from {Origin} to {Destination}, {Origin} to {Destination} flight booking, {Origin} to {Destination} flight ticket price, {Origin} to {Destination} flight time, {Origin} to {Destination} flight fare, cheap flights from {Origin} to {Destination}, {Origin} to {Destination} flight tickets, {Origin} to {Destination} flight time and price, {Origin} to {Destination} flight distance, {Origin} to {Destination} flight offers, {Origin} to {Destination} cheap airfare
                                                            resultItem["Keywords"] = string.Format("{0} to {1} flights, flights from {2} to {3}, {4} to {5} flight booking, {6} to {7} flight ticket price, {8} to {9} flight time, {10} to {11} flight fare, cheap flights from {12} to {13}, {14} to {15} flight tickets, {16} to {17} flight time and price, {18} to {19} flight distance, {20} to {21} flight offers, {22} to {23} cheap airfare",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            resultItem.Editing.EndEdit();
                                                        }
                                                        else if (offerObject.title.ToLower().Contains("flat") && offerObject.title.Contains("%"))
                                                        {
                                                            resultItem = resultItem.Versions.AddVersion();
                                                            resultItem.Editing.BeginEdit();

                                                            //{Origin} to {Destination} Flights, Ticket Price @ {Lowest_Price} + Flat {Discount price %} Off – Adani One
                                                            resultItem["Title"] = string.Format("{0} to {1} Flights, Ticket Price @ ₹{2} + Flat {3}% Off - Adani One", resultItem.Fields["DepartureCity"].Value, resultItem.Fields["ArrivalCity"].Value, Convert.ToString(lowestFarePrice), resultItem.Fields["DiscountPercent"].Value);

                                                            //Book {Origin} to {Destination} Flights, Ticket Price @ {Lowest_Price} + Flat {Discount price %} Off – Adani One
                                                            resultItem["MetaTitle"] = string.Format("Book {0} to {1} Flights, Ticket Price @ ₹{2} + Flat {3}% Off - Adani One", resultItem.Fields["DepartureCity"].Value, resultItem.Fields["ArrivalCity"].Value, Convert.ToString(lowestFarePrice), resultItem.Fields["DiscountPercent"].Value);

                                                            //{Origin} to {Destination} Flights @ {Lowest_Price} – Get best airfare on flights from {Origin} ({Origin_Airport_Code}) to {Destination} ({Destination_Airport_Code}). Use Coupon Code {Code_Name} and get discount upto {Discount_Price} and earn reward points on {Origin} to {Destination} flight booking with Adani One.
                                                            resultItem["MetaDescription"] = string.Format("{0} to {1} Flights @ {2} – Get best airfare on flights from {3} ({4}) to {5} ({6}). Use Coupon Code {7} and get discount upto {8} and earn reward points on {9} to {10} flight booking with Adani One.",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                Convert.ToString(lowestFarePrice),
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                departCityCode,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                arrivalCityCode,
                                                                couponCode,
                                                                maxOfferDiscount,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            //{Origin} to {Destination} Flights @ {Lowest_Price} – Get best airfare on flights from {Origin} ({Origin_Airport_Code}) to {Destination} ({Destination_Airport_Code}). Use Coupon Code {Code_Name} and get discount upto {Discount_Price} and earn reward points on {Origin} to {Destination} flight booking with Adani One.
                                                            resultItem["Description"] = string.Format("{0} to {1} Flights @ ₹{2} – Get best airfare on flights from {3} ({4}) to {5} ({6}). Use Coupon Code {7} and get discount upto {8} and earn reward points on {9} to {10} flight booking with Adani One.",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                Convert.ToString(lowestFarePrice),
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                departCityCode,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                arrivalCityCode,
                                                                couponCode,
                                                                maxOfferDiscount,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            //{Origin} to {Destination} flights, flights from {Origin} to {Destination}, {Origin} to {Destination} flight booking, {Origin} to {Destination} flight ticket price, {Origin} to {Destination} flight time, {Origin} to {Destination} flight fare, cheap flights from {Origin} to {Destination}, {Origin} to {Destination} flight tickets, {Origin} to {Destination} flight time and price, {Origin} to {Destination} flight distance, {Origin} to {Destination} flight offers, {Origin} to {Destination} cheap airfare
                                                            resultItem["Keywords"] = string.Format("{0} to {1} flights, flights from {2} to {3}, {4} to {5} flight booking, {6} to {7} flight ticket price, {8} to {9} flight time, {10} to {11} flight fare, cheap flights from {12} to {13}, {14} to {15} flight tickets, {16} to {17} flight time and price, {18} to {19} flight distance, {20} to {21} flight offers, {22} to {23} cheap airfare",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            resultItem.Editing.EndEdit();
                                                        }
                                                        else if (offerObject.title.Contains("flat") && offerObject.title.Contains("₹"))
                                                        {
                                                            resultItem = resultItem.Versions.AddVersion();
                                                            //resultItem.Locking.Lock();
                                                            resultItem.Editing.BeginEdit();

                                                            //{Origin} to {Destination} Flights, Ticket Price @ {Lowest_Price} + Flat {Discount_Price} Off – Adani One
                                                            resultItem["Title"] = string.Format("{0} to {1} Flights, Ticket Price @ ₹{2} + Flat ₹{3} Off - Adani One", resultItem.Fields["DepartureCity"].Value, resultItem.Fields["ArrivalCity"].Value, Convert.ToString(lowestFarePrice), Convert.ToString(maxOfferDiscount));

                                                            //Book {Origin} to {Destination} Flights, Ticket Price @ {Lowest_Price} + Flat {Discount_Price} Off – Adani One
                                                            resultItem["MetaTitle"] = string.Format("Book {0} to {1} Flights, Ticket Price @ ₹{2} + Flat ₹{3} Off - Adani One", resultItem.Fields["DepartureCity"].Value, resultItem.Fields["ArrivalCity"].Value, Convert.ToString(lowestFarePrice), Convert.ToString(maxOfferDiscount));

                                                            //{Origin} to {Destination} Flights @ {Lowest_Price} – Get best airfare on flights from {Origin} ({Origin_Airport_Code}) to {Destination} ({Destination_Airport_Code}). Use Coupon Code {Code_Name} and get discount upto {Discount_Price} and earn reward points on {Origin} to {Destination} flight booking with Adani One.
                                                            resultItem["MetaDescription"] = string.Format("{0} to {1} Flights @ {2} – Get best airfare on flights from {3} ({4}) to {5} ({6}). Use Coupon Code {7} and get discount upto {8} and earn reward points on {9} to {10} flight booking with Adani One.",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                Convert.ToString(lowestFarePrice),
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                departCityCode,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                arrivalCityCode,
                                                                couponCode,
                                                                maxOfferDiscount,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            //{Origin} to {Destination} Flights @ {Lowest_Price} – Get best airfare on flights from {Origin} ({Origin_Airport_Code}) to {Destination} ({Destination_Airport_Code}). Use Coupon Code {Code_Name} and get discount upto {Discount_Price} and earn reward points on {Origin} to {Destination} flight booking with Adani One.
                                                            resultItem["Description"] = string.Format("{0} to {1} Flights @ ₹{2} – Get best airfare on flights from {3} ({4}) to {5} ({6}). Use Coupon Code {7} and get discount upto {8} and earn reward points on {9} to {10} flight booking with Adani One.",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                Convert.ToString(lowestFarePrice),
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                departCityCode,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                arrivalCityCode,
                                                                couponCode,
                                                                maxOfferDiscount,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            //{Origin} to {Destination} flights, flights from {Origin} to {Destination}, {Origin} to {Destination} flight booking, {Origin} to {Destination} flight ticket price, {Origin} to {Destination} flight time, {Origin} to {Destination} flight fare, cheap flights from {Origin} to {Destination}, {Origin} to {Destination} flight tickets, {Origin} to {Destination} flight time and price, {Origin} to {Destination} flight distance, {Origin} to {Destination} flight offers, {Origin} to {Destination} cheap airfare
                                                            resultItem["Keywords"] = string.Format("{0} to {1} flights, flights from {2} to {3}, {4} to {5} flight booking, {6} to {7} flight ticket price, {8} to {9} flight time, {10} to {11} flight fare, cheap flights from {12} to {13}, {14} to {15} flight tickets, {16} to {17} flight time and price, {18} to {19} flight distance, {20} to {21} flight offers, {22} to {23} cheap airfare",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            resultItem.Editing.EndEdit();
                                                        }
                                                        else if (offerObject.title.Contains("%") && offerObject.title.ToLower().Contains("upto"))
                                                        {
                                                            resultItem = resultItem.Versions.AddVersion();
                                                            //resultItem.Locking.Lock();
                                                            resultItem.Editing.BeginEdit();

                                                            //{Origin} to {Destination} Flights, Ticket Price @ {Lowest_Price} + {Discount price %} Off – Adani One
                                                            resultItem["Title"] = string.Format("{0} to {1} Flights, Ticket Price @ ₹{2} + Upto {3}% Off - Adani One", resultItem.Fields["DepartureCity"].Value, resultItem.Fields["ArrivalCity"].Value, Convert.ToString(lowestFarePrice), resultItem.Fields["DiscountPercent"].Value);

                                                            //Book {Origin} to {Destination} Flights, Ticket Price @ {Lowest_Price} + {Discount price %} Off – Adani One
                                                            resultItem["MetaTitle"] = string.Format("Book {0} to {1} Flights, Ticket Price @ ₹{2} + Upto {3}% Off - Adani One", resultItem.Fields["DepartureCity"].Value, resultItem.Fields["ArrivalCity"].Value, Convert.ToString(lowestFarePrice), resultItem.Fields["DiscountPercent"].Value);

                                                            //{Origin} to {Destination} Flights @ {Lowest_Price} – Get best airfare on flights from {Origin} ({Origin_Airport_Code}) to {Destination} ({Destination_Airport_Code}). Use Coupon Code {Code_Name} and get discount upto {Discount_Price} and earn reward points on {Origin} to {Destination} flight booking with Adani One.
                                                            resultItem["MetaDescription"] = string.Format("{0} to {1} Flights @ {2} – Get best airfare on flights from {3} ({4}) to {5} ({6}). Use Coupon Code {7} and get discount upto {8} and earn reward points on {9} to {10} flight booking with Adani One.",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                Convert.ToString(lowestFarePrice),
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                departCityCode,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                arrivalCityCode,
                                                                couponCode,
                                                                maxOfferDiscount,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            //{Origin} to {Destination} Flights @ {Lowest_Price} – Get best airfare on flights from {Origin} ({Origin_Airport_Code}) to {Destination} ({Destination_Airport_Code}). Use Coupon Code {Code_Name} and get discount upto {Discount_Price} and earn reward points on {Origin} to {Destination} flight booking with Adani One.
                                                            resultItem["Description"] = string.Format("{0} to {1} Flights @ ₹{2} – Get best airfare on flights from {3} ({4}) to {5} ({6}). Use Coupon Code {7} and get discount upto {8} and earn reward points on {9} to {10} flight booking with Adani One.",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                Convert.ToString(lowestFarePrice),
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                departCityCode,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                arrivalCityCode,
                                                                couponCode,
                                                                maxOfferDiscount,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            //{Origin} to {Destination} flights, flights from {Origin} to {Destination}, {Origin} to {Destination} flight booking, {Origin} to {Destination} flight ticket price, {Origin} to {Destination} flight time, {Origin} to {Destination} flight fare, cheap flights from {Origin} to {Destination}, {Origin} to {Destination} flight tickets, {Origin} to {Destination} flight time and price, {Origin} to {Destination} flight distance, {Origin} to {Destination} flight offers, {Origin} to {Destination} cheap airfare
                                                            resultItem["Keywords"] = string.Format("{0} to {1} flights, flights from {2} to {3}, {4} to {5} flight booking, {6} to {7} flight ticket price, {8} to {9} flight time, {10} to {11} flight fare, cheap flights from {12} to {13}, {14} to {15} flight tickets, {16} to {17} flight time and price, {18} to {19} flight distance, {20} to {21} flight offers, {22} to {23} cheap airfare",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            resultItem.Editing.EndEdit();
                                                        }
                                                        else
                                                        {
                                                            resultItem = resultItem.Versions.AddVersion();
                                                            //resultItem.Locking.Lock();
                                                            resultItem.Editing.BeginEdit();

                                                            //{Origin} to {Destination} Flights, Ticket Price @ {Lowest_Price} – Adani One
                                                            resultItem["Title"] = string.Format("{0} to {1} Flights, Ticket Price @ {2} – Adani One", resultItem.Fields["DepartureCity"].Value, resultItem.Fields["ArrivalCity"].Value, Convert.ToString(lowestFarePrice));

                                                            //Book {Origin} to {Destination} Flights, Ticket Price @ {Lowest_Price} – Adani One
                                                            resultItem["MetaTitle"] = string.Format("Book {0} to {1} Flights, Ticket Price @ {2} – Adani One", resultItem.Fields["DepartureCity"].Value, resultItem.Fields["ArrivalCity"].Value, Convert.ToString(lowestFarePrice));

                                                            //{Origin} to {Destination} Flights @ {Lowest_Price} – Get best airfare on flights from {Origin} ({Origin_Airport_Code}) to {Destination} ({Destination_Airport_Code}). Use Coupon Code {Code_Name} and get discount upto {Discount_Price} and earn reward points on {Origin} to {Destination} flight booking with Adani One.
                                                            resultItem["MetaDescription"] = string.Format("{0} to {1} Flights @ {2} – Get best airfare on flights from {3} ({4}) to {5} ({6}). Use Coupon Code {7} and get discount upto {8} and earn reward points on {9} to {10} flight booking with Adani One.",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                Convert.ToString(lowestFarePrice),
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                departCityCode,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                arrivalCityCode,
                                                                couponCode,
                                                                maxOfferDiscount,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            //{Origin} to {Destination} Flights @ {Lowest_Price} – Get best airfare on flights from {Origin} ({Origin_Airport_Code}) to {Destination} ({Destination_Airport_Code}). Use Coupon Code {Code_Name} and get discount upto {Discount_Price} and earn reward points on {Origin} to {Destination} flight booking with Adani One.
                                                            resultItem["Description"] = string.Format("{0} to {1} Flights @ ₹{2} – Get best airfare on flights from {3} ({4}) to {5} ({6}). Use Coupon Code {7} and get discount upto {8} and earn reward points on {9} to {10} flight booking with Adani One.",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                Convert.ToString(lowestFarePrice),
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                departCityCode,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                arrivalCityCode,
                                                                couponCode,
                                                                maxOfferDiscount,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            //{Origin} to {Destination} flights, flights from {Origin} to {Destination}, {Origin} to {Destination} flight booking, {Origin} to {Destination} flight ticket price, {Origin} to {Destination} flight time, {Origin} to {Destination} flight fare, cheap flights from {Origin} to {Destination}, {Origin} to {Destination} flight tickets, {Origin} to {Destination} flight time and price, {Origin} to {Destination} flight distance, {Origin} to {Destination} flight offers, {Origin} to {Destination} cheap airfare
                                                            resultItem["Keywords"] = string.Format("{0} to {1} flights, flights from {2} to {3}, {4} to {5} flight booking, {6} to {7} flight ticket price, {8} to {9} flight time, {10} to {11} flight fare, cheap flights from {12} to {13}, {14} to {15} flight tickets, {16} to {17} flight time and price, {18} to {19} flight distance, {20} to {21} flight offers, {22} to {23} cheap airfare",
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value,
                                                                resultItem.Fields["DepartureCity"].Value,
                                                                resultItem.Fields["ArrivalCity"].Value);

                                                            resultItem.Editing.EndEdit();
                                                        }

                                                        _logRepository.Info("Process ended for MetaData set with MaxDiscount & LowestFair item: ", resultItem.ID);
                                                    }
                                                }
                                                else if (maxOfferDiscount <= 0 && resultItem != null)
                                                {
                                                    CacheManager.Enabled = false;
                                                    using (new Sitecore.SecurityModel.SecurityDisabler())
                                                    {
                                                        resultItem = resultItem.Versions.AddVersion();
                                                        //resultItem.Locking.Lock();
                                                        resultItem.Editing.BeginEdit();

                                                        //Book {Origin} to {Destination} Flights, Ticket Price @ {Lowest_Price} – Adani One
                                                        resultItem["MetaTitle"] = string.Format("Book {0} to {1} Flights, Ticket Price @ {2} – Adani One", resultItem.Fields["DepartureCity"].Value, resultItem.Fields["ArrivalCity"].Value, Convert.ToString(lowestFarePrice));

                                                        //{Origin} to {Destination} Flights @ {Lowest_Price} – Get best airfare on flights from {Origin} ({Origin_Airport_Code}) to {Destination} ({Destination_Airport_Code}). Book flight tickets with Adani One to avail best deals and promotions for your {Origin} to {Destination} trip!
                                                        resultItem["MetaDescription"] = string.Format("{0} to {1} Flights @ {2} – Get best airfare on flights from {3} ({4}) to {5} ({6}). Book flight tickets with Adani One to avail best deals and promotions for your {7} to {8} trip!",
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            Convert.ToString(lowestFarePrice),
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            departCityCode,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            arrivalCityCode,
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value);


                                                        //{Origin} to {Destination} Flights @ {Lowest_Price} – Get best airfare on flights from {Origin} ({Origin_Airport_Code}) to {Destination} ({Destination_Airport_Code}). Book flight tickets with Adani One to avail best deals and promotions for your {Origin} to {Destination} trip!
                                                        resultItem["Description"] = string.Format("{0} to {1} Flights @ {2} – Get best airfare on flights from {3} ({4}) to {5} ({6}). Book flight tickets with Adani One to avail best deals and promotions for your {7} to {8} trip!",
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            Convert.ToString(lowestFarePrice),
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            departCityCode,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            arrivalCityCode,
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value);

                                                        //{Origin} to {Destination} flights, flights from {Origin} to {Destination}, {Origin} to {Destination} flight booking, {Origin} to {Destination} flight ticket price, {Origin} to {Destination} flight time, {Origin} to {Destination} flight fare, cheap flights from {Origin} to {Destination}, {Origin} to {Destination} flight tickets, {Origin} to {Destination} flight time and price, {Origin} to {Destination} flight distance, {Origin} to {Destination} flight offers, {Origin} to {Destination} cheap airfare
                                                        resultItem["Keywords"] = string.Format("{0} to {1} flights, flights from {2} to {3}, {4} to {5} flight booking, {6} to {7} flight ticket price, {8} to {9} flight time, {10} to {11} flight fare, cheap flights from {12} to {13}, {14} to {15} flight tickets, {16} to {17} flight time and price, {18} to {19} flight distance, {20} to {21} flight offers, {22} to {23} cheap airfare",
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value,
                                                            resultItem.Fields["DepartureCity"].Value,
                                                            resultItem.Fields["ArrivalCity"].Value);

                                                        //{Origin} to {Destination} Flights @ {Lowest_Fare}
                                                        resultItem["Title"] = string.Format("{0} to {1} Flights @ {2}", resultItem.Fields["DepartureCity"].Value, resultItem.Fields["ArrivalCity"].Value, Convert.ToString(lowestFarePrice));

                                                        resultItem.Editing.EndEdit();
                                                        _logRepository.Info("Process ended for MetaData set with MaxDiscount item: ", resultItem.ID);
                                                    }
                                                }

                                                //PublishOffers(resultItem);
                                            }
                                            else
                                            {
                                                _logRepository.Info("LowestFare API response empty.");
                                            }
                                            //});
                                        }
                                        else
                                        {
                                            _logRepository.Info("Arrival or Departure CityCode Empty.");
                                        }
                                    }
                                    else
                                    {
                                        _logRepository.Info("Either item already updated with data or page Item not valid.");
                                    }
                                }
                            }
                            else
                            {
                                _logRepository.Info("Search result with Domestic Flights index yields no Data.");
                            }
                        }
                    }
                }

                _logRepository.Info("C2C Offer Data update completed .. !!");
            }
            catch (Exception ex)
            {
                _logRepository.Error(ex.Message);
            }
        }

        private async Task<List<FareCalendar>> GetLowestFareFromService(string frmCode, string toCode, string originCode, string destCode, string adt, string deptDate, string returnDate)
        {
            _logRepository.Info(String.Format("Inside GetLowestFairFromService method for route {0}-to-{1}", frmCode, toCode));
            LowestFairAPIResponse lowestFairAPIResponse = null;
            List<FareCalendar> fareCalendars = null;
            try
            {
                Dictionary<string, string> headers = new Dictionary<string, string>();
                //headers.Add("accept", "text/plain");
                headers.Add("accept", "application/json");
                headers.Add("traceId", Sitecore.Configuration.Settings.GetSetting("TraceID"));
                headers.Add("channelId", Sitecore.Configuration.Settings.GetSetting("ChannelID"));
                headers.Add("agentId", Sitecore.Configuration.Settings.GetSetting("AgentID"));

                Dictionary<string, string> apiParms = new Dictionary<string, string>();
                apiParms.Add("from", frmCode);
                apiParms.Add("to", toCode);
                apiParms.Add("originccode", originCode);
                apiParms.Add("destccode", destCode);
                apiParms.Add("adt", adt);
                apiParms.Add("deptdate", deptDate);
                apiParms.Add("returndate", returnDate);

                lowestFareResponse = new APIResponse();
                _logRepository.Info("Calling actual API GetDelayedAPIResponse.");
                string response = string.Empty;
                
                response = await lowestFareResponse.GetDelayedAPIResponse("GET", Sitecore.Configuration.Settings.GetSetting("LowestFairSeviceAPI"), headers, apiParms, "");
                
                if (!string.IsNullOrEmpty(response))
                {
                    lowestFairAPIResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<LowestFairAPIResponse>(response);

                    if (lowestFairAPIResponse.data != null && lowestFairAPIResponse.data.fareCalendars.Any())
                    {
                        fareCalendars = lowestFairAPIResponse.data.fareCalendars;
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error("GetLowestFairFromService Method in OfferDataSet command gives error for other filters -> " + ex.Message);
            }
            return fareCalendars;
        }

        private string GetContextIndexValue(string contextDB, string flighttype)
        {
            if (contextDB == "master")
            {
                if (flighttype == "domestic-flights")
                {
                    return Constant.citytocity_domestic_masterindex;
                }
                else if (flighttype == "international-flights")
                {
                    return Constant.citytocity_international_masterindex;
                }
            }
            else if (contextDB == "web")
            {
                if (flighttype == "domestic-flights")
                {
                    return Constant.citytocity_domestic_webindex;
                }
                else if (flighttype == "international-flights")
                {
                    return Constant.citytocity_international_webindex;
                }
            }
            return Constant.citytocity_domestic_masterindex;
        }

        private DateTime getSitecoreDate(string v)
        {
            return Sitecore.DateUtil.IsoDateToDateTime(v);
        }

        private Sitecore.Data.Database GetContextDatabase()
        {
            return Sitecore.Context.ContentDatabase;
        }
        private Sitecore.Data.Items.Item GetExistingItemBasedOnLanguage(string ItemPath)
        {
            return GetContextDatabase().GetItem(ItemPath, Sitecore.Context.Language);
        }
        private string getDisplayText(string type, string value)
        {
            string displaytext = string.Empty;
            switch (type)
            {
                case "PERCENTAGE":
                    displaytext = "Flat " + value + "% OFF";
                    break;
                case "'ABSOLUTE'":
                    displaytext = "Flat " + value + " OFF";
                    break;


            }
            return displaytext;
        }

        private async void PublishOffers(Sitecore.Data.Items.Item item)
        {
            _logRepository.Info("Offer Publish Started for item: " + item.ID.ToString());
            try
            {
                Sitecore.Data.Database master = Sitecore.Configuration.Factory.GetDatabase("master");
                _logRepository.Info("Gets the master/Source database ");
                Sitecore.Data.Database web = Sitecore.Configuration.Factory.GetDatabase("web");
                _logRepository.Info("Gets the Web/Target database ");
                Sitecore.Publishing.PublishOptions publishOptions = new Sitecore.Publishing.PublishOptions(master,
                                        web,
                                        Sitecore.Publishing.PublishMode.SingleItem,
                                        item.Language,
                                        System.DateTime.Now);
                _logRepository.Info("Publish started -> " + System.DateTime.Now.ToString());
                Sitecore.Publishing.Publisher publisher = new Sitecore.Publishing.Publisher(publishOptions);
                _logRepository.Info("Set publish Options -> ");
                publisher.Options.RootItem = item;
                _logRepository.Info("Set Root Item ");
                publisher.Options.Deep = true;

                await Task.Run(() => publisher.Publish());
                _logRepository.Info("Item Published with ID: " + item.ID.ToString());
            }
            catch (Exception ex)
            {

                _logRepository.Error(" Offers Published Failed due to -> " + ex.Message);
            }

        }

        private async Task<string> GetAirportCityName(string airportCode)
        {
            string cityName = string.Empty;
            try
            {
                await Task.Run(async () =>
                {
                    await Task.Delay(300);
                    IQueryable<SearchResultItem> airportCityItems = searchBuilder.GetSearchResults(Services.AirportPredicateBuilder.GetAirportCityPredicate());

                    if (airportCityItems != null)
                    {
                        var cityItems = airportCityItems.Where(x => x.TemplateId == Constant.AirportsCityTemplateId).ToList();

                        if (cityItems != null)
                        {
                            var cityObject = cityItems.FirstOrDefault(x => (x.Fields.ContainsKey(AirportsConstant.CityCode) && x.Fields[key: AirportsConstant.CityCode].ToString() == airportCode));
                            if (cityObject != null)
                            {
                                cityName = (!string.IsNullOrEmpty(cityObject.Fields[key: AirportsConstant.CityName].ToString())) ?
                                                        cityObject.Fields[key: AirportsConstant.CityName].ToString() :
                                                            string.Empty;
                            }
                        }
                    }
                });
            }
            catch (Exception ex)
            {

                _logRepository.Error(" GetAirportCityName Failed due to -> " + ex.Message);
            }
            return cityName;
        }
    }
}