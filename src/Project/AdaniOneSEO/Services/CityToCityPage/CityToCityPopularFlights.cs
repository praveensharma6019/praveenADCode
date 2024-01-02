using Microsoft.AspNet.OData.Query;
using Project.AdaniOneSEO.Website.Models;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination;
using Project.AdaniOneSEO.Website.Services.FlightsToDestination;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Web;

namespace Project.AdaniOneSEO.Website.Services.CityToCityPage
{
    public class CityToCityPopularFlights : ICityToCityPopularFlights
    {
        string UAT = "https://sitecorecdaks.uat.adanione.com";
        string Production = "https://sitecorecdaks.adanione.com";
        string Dev = "https://sitecorecddevaks.adanione.cloud";
        string domainName = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority);
        string itemId = "";
        string Page = "";
        public PopularFlightsModel GetPopularFlightsNew(Rendering rendering)
        {
            PopularFlightsModel popularFlightsDataModel = new PopularFlightsModel();
            List<PopularFlightsList> popularFlightsList = new List<PopularFlightsList>();

            try
            {
                Uri CurrentURL = new Uri(HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + HttpContext.Current.Request.RawUrl);

                string itemsPath = HttpUtility.ParseQueryString(CurrentURL.Query).Get("item");
                string itemPath = "";
                if (!string.IsNullOrEmpty(itemsPath))
                {
                    if (ID.TryParse(itemsPath, out ID itemId))
                    {
                        Item item = Sitecore.Context.Database.GetItem(itemId);

                        if (item != null)
                        {
                            itemPath = item.Paths.FullPath;
                            string[] itemPathSegment = itemPath.Split('/');
                            string Flight = itemPathSegment[itemPathSegment.Length - 1];
                            if (itemPath != null && itemPath.Contains("domestic-flights"))
                            {
                                Page = "domestic-flights";
                            }
                            else if (itemPath.Contains("international-flights"))
                            {
                                Page = "international-flights";
                            }
                            else
                            { Page = itemPathSegment[itemPathSegment.Length - 2]; }

                            if (!string.IsNullOrEmpty(Flight))
                            {
                                string[] parts = Flight.Split('-');
                                if (parts.Length >= 2)
                                {
                                    string City1 = parts[0]; // "city1"
                                    string City2 = parts[2]; // "city2"

                                    if (!string.IsNullOrEmpty(City1) && !string.IsNullOrEmpty(City2) && !string.IsNullOrEmpty(Page))
                                    {
                                        //Popular Flights to City 1
                                        PopularFlightsList popularFlightsToList = GetPopularFlights(City1, "to", Page);
                                        popularFlightsList.Add(popularFlightsToList);

                                        //Popular Flights from City 1
                                        PopularFlightsList popularFlightsFromList = GetPopularFlights(City1, "from", Page);
                                        popularFlightsList.Add(popularFlightsFromList);

                                        //Popular Flights to City 2
                                        PopularFlightsList popularFlightsToList2 = GetPopularFlights(City2, "to", Page);
                                        popularFlightsList.Add(popularFlightsToList2);

                                        //Popular Flights from City 2
                                        PopularFlightsList popularFlightsFromList2 = GetPopularFlights(City2, "from", Page);
                                        popularFlightsList.Add(popularFlightsFromList2);

                                        //Top Domestic Routes
                                        PopularFlightsList topDomesticRoute = GetTopDomesticRoutes(Page);
                                        popularFlightsList.Add(topDomesticRoute);
                                    }
                                }
                            }
                        }
                    }
                    
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            popularFlightsDataModel.PopularFlights = popularFlightsList;
            return popularFlightsDataModel;
        }

        public PopularFlightsList GetPopularFlights(string CityName, string key, string PageName)
        {
            PopularFlightsList popularFlightsListObj = new PopularFlightsList();

            try
            {                
                popularFlightsListObj.Heading = "Popular Flights " + key + " " + CityName;

                List<PopularFlightsItemsList> popularFlightsItems = new List<PopularFlightsItemsList>();

                int count = 1;
                string[] popularFlights = new string[] { "Mumbai", "Delhi", "Kolkata", "Bangalore", "Chennai", "Goa", "Ahmedabad", "Hyderabad", "Pune", "Trivandrum", "Guwahati", "Jaipur", "Lucknow", "Visakhapatnam", "Patna" };
                foreach (string c in popularFlights)
                {
                    if (count <= 10)
                    {
                        if (CityName.ToLower() == c.ToLower())
                        {
                            continue;
                        }
                        else
                        {
                            count++;

                            PopularFlightsItemsListComponent Link = new PopularFlightsItemsListComponent();
                            Link.Target = "";
                            string Text = "{0} {1} {2} Flights";
                            string LinkText = string.Empty;
                            if (key.ToLower() == "to")
                            {
                                LinkText = string.Format(Text, c, "To", CityName);
                            }
                            if (key.ToLower() == "from")
                            {
                                LinkText = string.Format(Text, CityName, "To", c);
                            }
                            Link.Text = LinkText;
                            string flightName = LinkText.ToLower().Replace(" ", "-");
                            if (domainName == Dev)
                            {
                                domainName = "https://www.adanione.com";
                                Link.Url = domainName + "/" + PageName + "/" + flightName;
                            }
                            else if (domainName == UAT)
                            {
                                domainName = "https://www.adanione.com";
                                Link.Url = domainName + "/" + PageName + "/" + flightName;
                            }
                            else if (domainName == Production)
                            {
                                domainName = "https://www.adanione.com";
                                Link.Url = domainName + "/" + PageName + "/" + flightName;
                            }
                            else
                            {
                                domainName = "https://www.adanione.com";
                                Link.Url = domainName + "/" + PageName + "/" + flightName;
                            }
                            //Link.Url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/" + PageName + "/" + flightName;

                            PopularFlightsItemsList popularFlightsItemsList = new PopularFlightsItemsList();
                            popularFlightsItemsList.Link = Link;
                            popularFlightsItems.Add(popularFlightsItemsList);
                        }
                    }
                }

                popularFlightsListObj.PopularFlightsItems = popularFlightsItems;

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return popularFlightsListObj;
        }

        public PopularFlightsList GetTopDomesticRoutes(string PageName)
        {
            PopularFlightsList popularFlightsListObj = new PopularFlightsList();

            try
            {
                popularFlightsListObj.Heading = "Top Domestic Flight Routes";

                List<PopularFlightsItemsList> popularFlightsItems = new List<PopularFlightsItemsList>();
                
                string[] popularFlights = new string[] { "Delhi To Goa Flights", "Mumbai to Delhi Flights", "Mumbai to Bangalore Flights", "Chennai to Mumbai Flights", "Delhi to Mumbai Flights", "Mumbai to Ahmedabad Flights", "Pune to Delhi Flights", "Delhi to Srinagar Flights", "Patna to Delhi Flights", "Kolkata to Bangalore Flights" };
                foreach (string c in popularFlights)
                {
                    PopularFlightsItemsListComponent Link = new PopularFlightsItemsListComponent();
                    Link.Target = "";
                    Link.Text = c;
                    string flightName = c.ToLower().Replace(" ", "-");
                    if (domainName == Dev)
                    {
                        domainName = "https://www.adanione.com";
                        Link.Url = domainName + "/" + PageName + "/" + flightName;
                    }
                    else if (domainName == UAT)
                    {
                        domainName = "https://www.adanione.com";
                        Link.Url = domainName + "/" + PageName + "/" + flightName;
                    }
                    else if (domainName == Production)
                    {
                        domainName = "https://www.adanione.com";
                        Link.Url = domainName + "/" + PageName + "/" + flightName;
                    }
                    else
                    {
                        domainName = "https://www.adanione.com";
                        Link.Url = domainName + "/" + PageName + "/" + flightName;
                    }
                    //Link.Url = HttpContext.Current.Request.Url.Scheme + "://" + HttpContext.Current.Request.Url.Authority + "/" + PageName + "/" + flightName;

                    PopularFlightsItemsList popularFlightsItemsList = new PopularFlightsItemsList();
                    popularFlightsItemsList.Link = Link;
                    popularFlightsItems.Add(popularFlightsItemsList);
                }
                popularFlightsListObj.PopularFlightsItems = popularFlightsItems;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return popularFlightsListObj;
        }
    }
}