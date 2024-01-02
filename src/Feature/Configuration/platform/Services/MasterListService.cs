using Adani.SuperApp.Realty.Feature.Configuration.Platform.Models;
using Adani.SuperApp.Realty.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Realty.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Services
{
    public class MasterListService : IMasterListService
    {
        private readonly ILogRepository _logRepository;

        public MasterListService(ILogRepository logRepository)
        {
            this._logRepository = logRepository;
        }
        public MasterListModel GetMasterList(Item dataSource, string queryString)
        {
            MasterListModel masterListModel = new MasterListModel();

            try
            {
                queryString = queryString.ToLower();

                if (queryString.Contains(Constants.country.ToLower()) || queryString.Contains(Constants.all.ToLower()))
                {
                    IEnumerable<Item> ChildItems = dataSource.GetChildren();
                    List<Country> countryListData = new List<Country>();
                    if (ChildItems != null && ChildItems.Count() >= 0)
                    {
                        foreach (Item collapsechild in ChildItems)
                        {

                            countryListData.Add(GetCountryData(collapsechild));
                        }
                        masterListModel.Country = countryListData;
                    }
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));
            }



            return masterListModel;
        }

        public Country GetCountryData(Item item)
        {
            Country country = new Country();
            try
            {
                if (item != null)
                {

                    country.Id = item[Constants.Id];
                    country.CountryName = item[Constants.CountryName];
                    country.DialCode = item[Constants.DialCode];
                    country.ISO3 = item[Constants.ISO3];
                    country.ISO2 = item[Constants.ISO2];
                    country.CurrencyName = item[Constants.CurrencyName];
                    country.CurrencyCode = item[Constants.CurrencyCode];
                    country.UNTERMEnglish = item[Constants.UNTERMEnglish];
                    country.RegionName = item[Constants.RegionName];
                    country.Capital = item[Constants.Capital];
                    country.Continent = item[Constants.Continent];
                    country.TLD = item[Constants.TLD];
                    country.Languages = item[Constants.Languages];
                    country.CountryFlagImage = item.Fields[Constants.FlagImage] != null ? Helper.GetImageURL(item, Constants.FlagImage) : String.Empty;
                    country.ContactNoLength = !string.IsNullOrEmpty(item[Constants.ContactNoLength]) ? item[Constants.ContactNoLength] : "10";
                    CheckboxField checkbox = item.Fields[Constants.IsDeleted];
                    if (checkbox != null)
                        country.IsDeleted = checkbox.Checked;
                }
            }
            catch (Exception ex)
            {
                _logRepository.Error(string.Format("Method Name:{0} \n Error Message: {1}", MethodBase.GetCurrentMethod().Name, ex.Message));

            }
            return country;
        }
    }
}