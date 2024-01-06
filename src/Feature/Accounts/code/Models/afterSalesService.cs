
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Models
{
    [Serializable]
    public class afterSalesServices : PayOnline
    {
        public afterSalesServices()
        {
            afterSalesServicesList = new List<afterSalesServiceRecords>();
            afterSalesServiceSelectedList = new List<SelectListItem>();
        }

        public List<afterSalesServiceRecords> afterSalesServicesList { get; set; }
        public List<SelectListItem> afterSalesServiceSelectedList { get; set; }

        [Required(ErrorMessage = "Please select the required field")]
        public string selectedAfterSalesServiceRequest { get; set; }
        [Required]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Quantity must be a number/Please Enter Value Between Min-Max Value")]
        public string Quantity { get; set; }
        [Required(ErrorMessageResourceName = nameof(Required), ErrorMessageResourceType = typeof(afterSalesServices))]
        public string Comment { get; set; }

        public string TempAmount { get; set; }
        public string Tax { get; set; }
        public string AmountWithTax { get; set; }
        public string ExtraAmount { get; set; }

        public string TempQuantity_Min { get; set; }
        public string TempQuantity_Max { get; set; }
        public string RequestNumber { get; set; }
        public afterSalesServiceRecords AfterSalesServiceRecord { get; set; }

        public new static string Required => DictionaryPhraseRepository.Current.Get("/Accounts/AfterSalesServices/Required", "Please enter a value for {0}");

    }

    [Serializable]
    public class afterSalesServiceRecords
    {
        public string Comp_Cat { get; set; }
        public string SrNo { get; set; }
        public string Comp_Type { get; set; }
        public string Comp_Text { get; set; }
        public string Task_Group { get; set; }
        public string Quantity_Min { get; set; }
        public string Quantity_Max { get; set; }
        public string Partner_Type { get; set; }

        public string MaxLengthpipe { get; set; }

        public string Messageflg { get; set; }
        public string Message { get; set; }

        public string CustomerId { get; set; }
        public string CustomerPassword { get; set; }
        public string CustomerPartnerNo { get; set; }
    }
}
