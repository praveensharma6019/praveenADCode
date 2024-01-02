using Adani.SuperApp.Airport.Feature.Master.Platform.Models;
using Adani.SuperApp.Airport.Feature.Master.Platform.Constant;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;

namespace Adani.SuperApp.Airport.Feature.Master.Platform.Services
{
    public class CountryMasterService : ICountryMasterService
    {
        private readonly IHelper _helper;

        public CountryMasterService(IHelper helper)
        {

            this._helper = helper;
        }
        public List<CountryMasterModel> GetCountryMasterData(Item dataSource)
        {
            List<CountryMasterModel> countryListData = new List<CountryMasterModel>();
            List<Item> countries = dataSource.GetChildren().ToList();
            foreach(Item country in countries)
            {
                if(country != null)
                    countryListData.Add(GetCountryData(country));
            }
            return countryListData;
        }

        public CountryMasterModel GetCountryData(Item item)
        {
            CountryMasterModel country = new CountryMasterModel();
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
            country.CountryFlagImage = item.Fields[Constants.FlagImage]!=null ? _helper.GetImageURL(item, Constants.FlagImage) : String.Empty;
            CheckboxField checkbox = item.Fields[Constants.IsDeleted];
            if(checkbox != null)
                country.IsDeleted = checkbox.Checked;
            return country;
        }
    }
}