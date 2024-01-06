using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniHousing.Website.Models
{
    public class AdaniHousingBranchListModel
    {
        public int SNo { get; set; }
        public string Business { get; set; }
        public string SubsidiaryCompany { get; set; }
        public string TypeOfAsset { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Site { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string Address { get; set; }
        public List<SelectListItem> StateList { get; set; }
    }
    public class AdaniHousingGetBranchDetails
    {
        AdaniHousingFormsDataContext dbContext = new AdaniHousingFormsDataContext();
        public List<string> GetStates()
        {
            var StateList = dbContext.AdaniCapitalBranchLists.Select(x => x.State).Distinct().OrderBy(x => x).ToList();
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
            var CityList = dbContext.AdaniCapitalBranchLists.Where(t =>t.State == State).Select(x => x.City).Distinct().OrderBy(x => x).ToList();
            if (CityList != null)
            {
                return CityList;
            }
            else
            {
                return null;
            }
        }
        public AdaniHousingBranchListModel GetBranchLocation(string State, string City)
        {
            var Location = dbContext.AdaniCapitalBranchLists.Where(t => t.State == State && t.City == City).Select(i => new AdaniHousingBranchListModel { Latitude = i.Latitude, Longitude = i.Longitude, Address = i.Address }).FirstOrDefault();
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