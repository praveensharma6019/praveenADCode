using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Configuration.Platform.Models
{
    public class MasterListModel
    {
        public List<Country> Country { get; set; }
    }
    public class Country
    {
        public string CountryName { get; set; }

        public string DialCode { get; set; }
        public string ISO3 { get; set; }

        public string ISO2 { get; set; }
        public string CurrencyName { get; set; }

        public string CurrencyCode { get; set; }
        public string UNTERMEnglish { get; set; }

        public string RegionName { get; set; }

        public string Capital { get; set; }

        public string Continent { get; set; }
        public string TLD { get; set; }
        public string Languages { get; set; }

        public bool IsDeleted { get; set; }

        public string Id { get; set; }

        public string CountryFlagImage { get; set; }
        public string ContactNoLength { get; set; } 
    }
}
