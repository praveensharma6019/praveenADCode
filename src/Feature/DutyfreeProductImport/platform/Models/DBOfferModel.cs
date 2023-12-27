using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Adani.SuperApp.Airport.Feature.DutyFreeProductImport.Platform.Models
{
    public class DBOfferModel
    {
        // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);
        public class DBOfferDataModel
        {
            public int skuId { get; set; }
            public string promotionCode { get; set; }
            public int buyQuantity { get; set; }
            public string operatorType { get; set; }
            public string promotionType { get; set; }
            public string offerType { get; set; }
            public double offer { get; set; }
            public string offerDisplayText { get; set; }
            public double offerPrice { get; set; }
            public double discountPrice { get; set; }
            public DateTime effectiveFrom { get; set; }
            public DateTime effectiveTo { get; set; }
            public int id { get; set; }
            public string createdBy { get; set; }
            public DateTime createdOn { get; set; }
            public string modifiedBy { get; set; }
            public DateTime modifiedOn { get; set; }

        }

        public class DBOfferDataModelResponse
        {
            public List<DBOfferDataModel> data { get; set; }
            public bool status { get; set; }
            public object message { get; set; }
            public object code { get; set; }
            public object warning { get; set; }
            public object error { get; set; }
        }


    }
}