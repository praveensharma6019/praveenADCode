using Sitecore.Diagnostics;
using Sitecore.Feature.Accounts;
using Sitecore.Feature.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Services
{
    public class RateusServices
    {
       
        public bool RateusInsert(RatusRevamp rateusRevampModel)
        {
            try
            {
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    RatusRevamp rateusObj = new RatusRevamp()
                    {
                        CANumber = rateusRevampModel.CANumber,
                        CategoryName = rateusRevampModel.CategoryName,
                        Rating = rateusRevampModel.Rating,
                        AppreciationNote = rateusRevampModel.AppreciationNote,
                        CreatedDate = DateTime.Now
                    };
                    dbcontext.RatusRevamps.InsertOnSubmit(rateusObj);
                    dbcontext.SubmitChanges();
                    return true;
                }
            }
            
            catch (Exception e)
            {
                Log.Error("InsertRateUsRecord error in db save: " + e.Message, this);
            }
            return false;
        }
    }
}