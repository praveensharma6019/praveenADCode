using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniCapital.Website.Models
{
    public class AdaniCapitalCollectionCentreListModel
    {
        public int Id { get; set; }
        public string OfficeType { get; set; }
        public string MerchantOrShop { get; set; }
        public string ContactNo { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Site { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public List<SelectListItem> StateList { get; set; }
    }
    public class AdaniCapitalGetCCDetails
    {
        AdaniCapitalFormsDataContext dbContext = new AdaniCapitalFormsDataContext();
        public List<string> GetStates()
        {
            var StateList = dbContext.AdaniCapitalHousingCollectionCentres.Select(x => x.State).Distinct().OrderBy(x => x).ToList();
            if (StateList != null)
            {
                return StateList;
            }
            else
            {
                return null;
            }
        }
        public List<string> GetCities(string State)
        {
            var CityList = dbContext.AdaniCapitalHousingCollectionCentres.Where(t => t.State.ToUpper() == State.ToUpper()).Select(x => x.City).Distinct().OrderBy(x => x).ToList();
            if (CityList != null)
            {
                return CityList;
            }
            else
            {
                return null;
            }
        }
        public List<AdaniCapitalCollectionCentreListModel> GetCentreLocation(string State, string City)
        {
            var Location = dbContext.AdaniCapitalHousingCollectionCentres.Where(t => t.State.ToUpper() == State.ToUpper() && t.City.ToUpper() == City.ToUpper()).Select(i => new AdaniCapitalCollectionCentreListModel { OfficeType = i.OfficeType, Latitude = i.Latitude, Longitude = i.Longitude, Address = i.Address, MerchantOrShop= i.MerchantOrShop,ContactNo=i.ContactNo }).Distinct().ToList();
            if (Location != null)
            {
                return Location;
            }
            else
            {
                return null;
            }
        }
    }
}