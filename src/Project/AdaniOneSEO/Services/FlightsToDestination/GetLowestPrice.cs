using Glass.Mapper.Sc;
using Project.AdaniOneSEO.Website.Helpers;
using Project.AdaniOneSEO.Website.Models;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination;
using Project.AdaniOneSEO.Website.Models.FlightsToDestination.Banner;
using Sitecore.Data.Items;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Project.AdaniOneSEO.Website.Services.FlightsToDestination
{
    public class GetLowestPrice : IGetLowestPrice
    {
        public CheapestFlightCalenderModel GetLowestPriceData(Rendering rendering)
        {
            CheapestFlightCalenderModel cheapestFlightCalenderModel = new CheapestFlightCalenderModel();            
          
            try
            {
                var datasource = Utils.GetRenderingDatasource(rendering);
                if (datasource == null) return null;

               List<CheapestFlightData> getLowestPrice = new List<CheapestFlightData>();

                foreach (Item item in datasource.GetChildren())
                {
                    List<CheapestFlightData> getLowestPriceModel = new List<CheapestFlightData>();
                    List<Item> itemList = new List<Item>();
                    foreach (Item item1 in item.GetChildren())
                    {
                        var chkValue = item1.Fields["lowestfair"].Value;
                        if (chkValue == "1")
                        {
                            itemList.Add(item1);
                        }
                    }

                    var lowestPriceItem = itemList.OrderByDescending(x => x["price"]).ToList();


                    foreach (var item2 in lowestPriceItem)
                    {
                        CheapestFlightData cheapestFlightData = new CheapestFlightData();
                        cheapestFlightData.Price = Convert.ToDecimal(item2.Fields["price"].Value);


                        var dateTimeField = item.Fields["Date"].Value;

                        if (!string.IsNullOrEmpty(dateTimeField))
                        {
                            string dateTimeString = dateTimeField;
                            DateTime dateTimeStruct = Sitecore.DateUtil.IsoDateToDateTime(dateTimeString);
                            cheapestFlightData.Date = String.Format("{0:d MMM yyyy}", dateTimeStruct);
                        }
                        getLowestPriceModel.Add(cheapestFlightData);
                    }
                    getLowestPrice.AddRange(getLowestPriceModel);
                    cheapestFlightCalenderModel.FlightCalender = getLowestPrice;
                }

                
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, this);
            }

            return cheapestFlightCalenderModel;
        }
    }
}