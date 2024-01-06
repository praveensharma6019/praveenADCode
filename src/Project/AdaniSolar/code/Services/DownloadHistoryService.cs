using Sitecore.AdaniSolar.Website.Models;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace Sitecore.AdaniSolar.Website.Services
{
    public class DownloadHistoryService
    {

        public bool SaveData(DownloadHistoryModel model)
        {
            using (AdaniSolarDownloadHistoryDataContext dbcontext = new AdaniSolarDownloadHistoryDataContext())
            {
                AdaniSolarDownloadHistory ObjReg = new AdaniSolarDownloadHistory();
                   ObjReg.UserName = model.UserName;

               
                ObjReg.Invoice_Number = model.Invoice_Number;
                if (model.Invoice_Date.ToShortDateString() == "1/1/0001")
                {
                    ObjReg.Invoice_Date = null;
                }
                else
                {
                    ObjReg.Invoice_Date = model.Invoice_Date;
                }
                ObjReg.Pallet_Id = model.Pallet_Id;
                ObjReg.Module_Serial_Number = model.Module_Serial_Number;
                ObjReg.Warranty_valid_till = model.Warranty_valid_till;
                ObjReg.Currentdate = DateTime.Now;//model.Currentdate;
                ObjReg.CurrentTime = DateTime.Now;
               
                dbcontext.AdaniSolarDownloadHistories.InsertOnSubmit(ObjReg);
                dbcontext.SubmitChanges();
                return true;
            }
            return false;
        }
    }
}