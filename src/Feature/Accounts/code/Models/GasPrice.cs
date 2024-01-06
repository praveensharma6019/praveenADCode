using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class GasPrice
    {
        public List<SelectListItem> CityList { get; set; }
        public List<SelectListItem> CityListD { get; set; }
        public List<SelectListItem> CityListC { get; set; }
        public List<SelectListItem> CityListI { get; set; }
        public string SelectedCity { get; set; }
        public List<SelectListItem> LocateCNGCityList { get; set; }
        public string LocateCNGCity { get; set; }
        public string PageType { get; set; }
        public List<CityAndPrice> CityWisePrice { get; set; }

        public GasPrice()
        {
            CityWisePrice = new List<CityAndPrice>();
        }
    }

    public class CityWisePrice
    {
        public string City { get; set; }
        public string Product { get; set; }
        public string Eff_date { get; set; }
        public string MMBTU_Rate { get; set; }
    }

    public class PriceObject
    {
        public string Type { get; set; }
        public string Effectice_Date_1 { get; set; }
        public string Price_1 { get; set; }
        public string Desc_1 { get; set; }
        public string Effectice_Date_2 { get; set; }
        public string Price_2 { get; set; }
        public string Desc_2 { get; set; }
        public string Effectice_Date_3 { get; set; }
        public string Price_3 { get; set; }
        public string Desc_3 { get; set; }
    }

    public class CityAndPrice
    {
        public string City { get; set; }
        public PriceObject DomesticCityWisePrice { get; set; }
        public PriceObject CommercialCityWisePrice { get; set; }
        public PriceObject IndustrailCityWisePrice { get; set; }
        public PriceObject CNGCityWisePrice { get; set; }

        public CityAndPrice()
        {
            DomesticCityWisePrice = new PriceObject();
            CommercialCityWisePrice = new PriceObject();
            IndustrailCityWisePrice = new PriceObject();
            CNGCityWisePrice = new PriceObject();
        }

    }
}