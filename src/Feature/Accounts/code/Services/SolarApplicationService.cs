using SapPiService.Domain;
using Sitecore.Diagnostics;
using Sitecore.Feature.Accounts;
using Sitecore.Feature.Accounts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Models
{
    public class SolarApplicationService
    {
        private const int AccountNumberLength = 12;

        public static string FormatAccountNumber(string accountNumber)
        {
            return accountNumber.PadLeft(AccountNumberLength, '0');
        }

        public List<SolarApplicationDocumentMaster> GetDocuments()
        {
            List<SolarApplicationDocumentMaster> result = new List<SolarApplicationDocumentMaster>();
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                result = dataContext.SolarApplicationDocumentMasters.OrderBy(d => d.DocumentSerialNumber).ToList();
            }
            return result;
        }

        public List<SolarApplicationVoltageMaster> GetVoltageDetails()
        {
            List<SolarApplicationVoltageMaster> result = new List<SolarApplicationVoltageMaster>();
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                result = dataContext.SolarApplicationVoltageMasters.OrderBy(d => d.Id).ToList();
            }
            return result;
        }


        public List<SolarApplicationVendorList> GetVendors()
        {
            List<SolarApplicationVendorList> result = new List<SolarApplicationVendorList>();
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                result = dataContext.SolarApplicationVendorLists.OrderBy(d => d.NameOfVendorAgency).ToList();
            }
            return result;
        }

        public List<SolarApplicationDetail> GetExistingApplications(string accountNumber)
        {
            List<SolarApplicationDetail> result = new List<SolarApplicationDetail>();
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                result = dataContext.SolarApplicationDetails.Where(a=>a.AccountNumber==accountNumber).OrderByDescending(d => d.CreatedDate).ToList();
            }
            return result;
        }

        //public SolarApplicationDetail SaveSolarApplicationDetails(SolarNewApplicationModel model)
        //{
        //    SolarApplicationDetail objNewSolarApplication;
        //    List<SolarApplicationVendorList> result = new List<SolarApplicationVendorList>();
        //    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
        //    {
        //        objNewSolarApplication = new SolarApplicationDetail
        //        {
        //            Id = new Guid(),
        //            AccountNumber = model.AccountNo,
        //            Address = model.Address,
        //            ConsumerName = model.Name,
        //            EmailAddress = model.EmailAddress,
        //            InstallationCost = model.InstallationCost,
        //            IsNetMeterOpted = model.NetMeterOrBilling == "Meter" ? true : false,
        //            IsObligatedEntity = model.IsObligatedEntity == "Yes" ? true : false,
        //            IsOwnershipLeased = model.SolarOwnershipType == "Leased" ? true : false,
        //            IsRooftopSolarInstalled = model.IsSolarInstalled == "Yes" ? true : false,
        //            LECNumber = model.LECNumber,
        //            //MeterNumber=model.MeterNumber,
        //            MobileNumber = model.MobileNo,
        //            ProposedACcapacity = model.ProposedACCategory,
        //            RateCategory = model.RateCategory,
        //            VenderNameSeleted = model.SelectedVendor,
        //            //VenderSelected=model.SelectedVendor,
        //            VoltageCategorySelected = model.SelectedVoltageCategory
        //            //date to be added
        //        };
        //    }
        //    return objNewSolarApplication;
        //}

        //public bool SaveSolarApplicationDocuments(SolarApplicationDetail application, HttpPostedFileBase file, byte[] bytes, SolarApplicationDocumentMaster doc)
        //{
        //    SolarApplicationDocumentDetail objSolarApplicationDocumentDetail;

        //    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
        //    {
        //        objSolarApplicationDocumentDetail = new SolarApplicationDocumentDetail
        //        {
        //            AccountNumber=application.AccountNumber,
        //            CreatedBy=application.AccountNumber,
        //            CreatedDate=DateTime.Now,
        //            DocumentData=bytes,
        //            DocumentDataContenttype=file.ContentType,
        //            DocumentId=doc.Id,
        //            DocumentSerialNumber=doc.DocumentSerialNumber,
        //            Id=new Guid(),
        //            DocumentName=file.FileName,
        //            IsSentToSAP=false,
        //            SolarApplicationId=application.Id
        //        };
        //    }

        //    return true;
        //}
    }
}