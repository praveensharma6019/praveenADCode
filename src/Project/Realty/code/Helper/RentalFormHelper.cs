using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Realty.Website.Helper
{
    public class RentalFormHelper
    {
        Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
        public IEnumerable<SelectListItem> GetallApartmentSizeList()
        {
            try
            {
                Item ApartmentSizeList = db.GetItem(Templates.RentalFormFields.ApartmentSizeList);
                if (ApartmentSizeList.HasChildren)
                {
                    List<SelectListItem> TDDList = new List<SelectListItem>();
                        {
                            foreach (Item ml in ApartmentSizeList.GetChildren())
                            {
                            TDDList = ApartmentSizeList.GetChildren().ToList().Select(x => new SelectListItem()
                                {
                                    Text = x.Fields["Text"].Value ?? "",
                                    Value = x.Fields["Value"].Value ?? ""
                                }).ToList();
                        }
                        };
                        return TDDList;
                    }
                
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetSegmentServedTypeList: " + ex.Message, this);
            }
            return null;
        }
        public IEnumerable<SelectListItem> GetallApartmentTypeList()
        {
            try
            {
                Item ApartmentTypeList = db.GetItem(Templates.RentalFormFields.ApartmentTypeList);
                if (ApartmentTypeList.HasChildren)
                {
                    List<SelectListItem> TDDList = new List<SelectListItem>();
                    {
                        foreach (Item ml in ApartmentTypeList.GetChildren())
                        {
                            TDDList = ApartmentTypeList.GetChildren().ToList().Select(x => new SelectListItem()
                            {
                                Text = x.Fields["Text"].Value ?? "",
                                Value = x.Fields["Value"].Value ?? ""
                            }).ToList();
                        }
                    };
                    return TDDList;
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetSegmentServedTypeList: " + ex.Message, this);
            }
            return null;
        }
    }
}