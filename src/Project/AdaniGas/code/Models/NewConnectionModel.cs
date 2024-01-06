using System.ComponentModel.DataAnnotations;

using Sitecore.Foundation.Dictionary.Repositories;
using System.Web.Mvc;
using System.Collections.Generic;
using System;

namespace Sitecore.Feature.Accounts.Models
{
    public class NewConnectionModel
    {

        public string Id { get; set; }
        public string EnquiryNo { get; set; }
        public string CampaignID { get; set; }
        public string ReferenceSource { get; set; }
        public string Partner_Type { get; set; }
        public string Name { get; set; }
        public string MobileNo { get; set; }
        public string ConnectionTypeCode { get; set; }
        public string CityCode { get; set; }
        public string CityName { get; set; }
        public string AreaCode { get; set; }
        public string AreaName { get; set; }
        public string TypeOfHouse { get; set; }
        public string CApartmentComplex { get; set; }
        public string OtherApartmentOrSociety { get; set; }
        public string TypeOfCustomer { get; set; }
        public string OtherTypeOfCustomer { get; set; }
        public string Application { get; set; }
        public string OtherApplication { get; set; }
        public string CurrentFuelUsing { get; set; }
        public string OtherFuelUsing { get; set; }
        public string TypeOfIndustry { get; set; }
        public string OtherTypeOfIndustry { get; set; }
        public string MonthlyConsumption { get; set; }
        public string HouseNo_BuildingNo { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string Pincode { get; set; }
        public string FormURL { get; set; }
        public Nullable<DateTime> SubmitOn { get; set; }
        public string SubmittedBy { get; set; }

    }

}