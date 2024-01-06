using Sitecore.Diagnostics;
using Sitecore.Feature.Accounts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.Accounts.Services
{
    public class SubmitYourQueryServices
    {
       
        public bool SubmitYourQueryInsert(SubmitYourQuery objsubmitquery)
        {
            try
            {
                using (PaymentHistoryDataContext dbcontext = new PaymentHistoryDataContext())
                {
                    SubmityourQuery SubmityourQueryObj = new SubmityourQuery()
                    {
                        CategoryName = objsubmitquery.CategoryName,
                        Discription = objsubmitquery.Discription,
                        CompanyName = objsubmitquery.CompanyName,
                        SubCategory = objsubmitquery.SubCategory,
                        Name = objsubmitquery.Name,
                        EmailId = objsubmitquery.EmailId,
                        MobileNo = objsubmitquery.MobileNo,
                        Address = objsubmitquery.Address,
                        Area = objsubmitquery.Area,
                        City = objsubmitquery.City,
                        CreatedDate = DateTime.Now,
                        Createdby= objsubmitquery.CreatedBy
                    };
                    dbcontext.SubmityourQueries.InsertOnSubmit(SubmityourQueryObj);
                    dbcontext.SubmitChanges();
                    return true;
                }
            }
            
            catch (Exception e)
            {
                Log.Error("InsertYourQueryRecord error in db save: " + e.Message, this);
            }
            return false;
        }
    }
}