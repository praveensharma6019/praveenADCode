namespace Sitecore.Feature.Accounts.Models
{
    using System;
    using System.Linq;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;
    using Sitecore.Feature.Accounts.Attributes;
    using Sitecore.Foundation.Dictionary.Repositories;
    using System.Web;

    [Serializable]
    public class SolarNewApplicationModel
    {
        public SolarNewApplicationModel()
        {
            VendorsListSelectItem = new List<SelectListItem>();
            VendorsList = new List<SolarApplicationVendorList>();
            VoltageCategoryList = new List<SelectListItem> {
                new SelectListItem{ Value="230", Text="230 V"},
                new SelectListItem{ Value="400", Text="400 V"},
                new SelectListItem{ Value="11", Text="11 KV"},
                new SelectListItem{ Value="22", Text="22 KV"},
                new SelectListItem{ Value="33", Text="33 KV"},
                new SelectListItem{ Value="66", Text="66 KV"},
                new SelectListItem{ Value="132", Text="132 KV"},
                new SelectListItem{ Value="220", Text="220 KV"},
                new SelectListItem{ Value="400", Text="400 KV"},
                new SelectListItem{ Value="NA", Text="NA"}
            };
        }

        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(SolarNewApplicationModel))]
        public string AccountNo { get; set; }
        public string MeterNo { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public string RateCategory { get; set; }
        public string EmailAddress { get; set; }
        public string MobileNo { get; set; }

        public List<SelectListItem> VoltageCategoryList { get; set; }
        public string SelectedVoltageCategory { get; set; }

        public string ProposedACCategory { get; set; }
        public string LECNumber { get; set; }
        public string IsSolarInstalled { get; set; }
        public string SolarOwnershipType { get; set; }
        public string IsObligatedEntity { get; set; }
        public string NetMeterOrBilling { get; set; }
        public string InstallationCost { get; set; }
        public bool IsSubsidizedCategory { get; set; }
        public string ApplicationCategory { get; set; }
        public string RooftopSolarType { get; set; }
        public string AadharNumber { get; set; }

        public List<SolarDocumentCheckList> DocumentsList { get; set; }

        public List<SolarApplicationVendorList> VendorsList { get; set; }

        public List<SelectListItem> VendorsListSelectItem { get; set; }
        public string SelectedVendor { get; set; }

        public decimal AmountToBePaid { get; set; }
        public string AmountToBePaidDetail { get; set; }
        public string OrderNumber { get; set; }
        public bool IsApplicationSaved { get; set; }
        

        public static string Required => DictionaryPhraseRepository.Current.Get("/SolarApplication/Register", "Please enter {0}");
    }

    public class SolarDocumentCheckList
    {
        public bool IsSubsidized { get; set; }
        public Guid DocId { get; set; }
        public int DocSerialNumber { get; set; }
        public string DocName { get; set; }
        public HttpPostedFileBase DocFile { get; set; }
    }
}