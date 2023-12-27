using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Models;
using Sitecore.Data.Items;
using Sitecore.Globalization;

namespace Adani.SuperApp.Airport.Feature.FlightSearch.Platform.Services
{
    public class FlightSearch : IFlightSearch
    {
        /// <summary>
        /// Implementation to get the header data
        /// </summary>
        /// <param name="datasource"></param>
        /// <returns></returns>
        public BookFlight GetBookFlightWidgetData(Item datasource)
        {
            BookFlight bookFlight = new BookFlight();
            List<NameValueCollectionList> nameValueCollection = new List<NameValueCollectionList>();
            //FlightSearchWidget flightSearchWidget = new FlightSearchWidget();
            IEnumerable<Item> kids = datasource.Axes.GetDescendants().Where(x => x.TemplateID.ToString() == Templates.NameValueCollection.NameValueFolder).ToList();
            bookFlight.Title = datasource.Fields["Title"].Value.ToString();
            bookFlight.ID = datasource.ID.ToString();
            foreach (Sitecore.Data.Items.Item item in kids)
            {
                
                NameValueCollectionList nameValueCollectionList = new NameValueCollectionList();
                //List<NameandValue> classvalue = new List<NameandValue>();
                IEnumerable<Item> nameValueList = item.Axes.GetDescendants().Where(x => x.TemplateID.ToString() == Templates.NameValueCollection.nameValue).ToList();
                if (nameValueList != null && nameValueList.Count() > 0)
                {
                    nameValueCollectionList.ListName = item.Fields["Title"].Value.ToString();
                    List<NameandValue> nameandValues = new List<NameandValue>();
                    foreach (Sitecore.Data.Items.Item citem in nameValueList)
                    {
                        NameandValue nameandValueItem = new NameandValue();
                        nameandValueItem.Name = citem.Fields["name"].Value.ToString();
                        nameandValueItem.Value= citem.Fields["value"].Value.ToString();

                        nameandValues.Add(nameandValueItem);
                    }
                    nameValueCollectionList.Collection = nameandValues;
                }
                nameValueCollection.Add(nameValueCollectionList);
            }
            bookFlight.nameValueCollection = nameValueCollection;

            return bookFlight;
        }
    }
}