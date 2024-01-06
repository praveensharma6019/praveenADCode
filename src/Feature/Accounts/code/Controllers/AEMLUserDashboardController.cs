using SapPiService.Domain;
using Sitecore.Diagnostics;
using Sitecore.Feature.Accounts.Attributes;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Feature.Accounts.Services;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Controllers
{
    [CookieTemperingRedirectNotFound]
    public class AEMLUserDashboardController : Controller
    {
        public AEMLUserDashboardController(IUserProfileService userProfileService)
        {
            this.UserProfileService = userProfileService;
        }

        private IUserProfileService UserProfileService { get; }
        // GET: UserDashboard
        [RedirectUnauthenticatedCookieTempered]
        public ActionResult UserDashboardLanding()
        {
            return View();
        }

        public JsonResult UserDashboardPowerCutNotification()
        {
            SapPiService.Domain.OutageDetail model = new SapPiService.Domain.OutageDetail();
            try
            {
                model = SapPiService.Services.RequestHandler.GetOutageInformation(SessionHelper.UserSession.UserSessionContext.primaryAccountNumber);

                if (model.CurrentOutageDetails != null && model.CurrentOutageDetails.Count > 1)
                {
                    List<OutageRecord> currList = new List<OutageRecord>();
                    var maxEndTime = model.CurrentOutageDetails.Max(c => c.EndDatetime);
                    foreach (var r in model.CurrentOutageDetails)
                    {
                        if (r.EndDatetime == maxEndTime)
                            currList.Add(r);
                    }

                    if (currList.Count > 1)
                    {
                        var minStartTime = currList.Min(c => c.StartDateTime);
                        List<OutageRecord> currList1 = new List<OutageRecord>();
                        foreach (var r in currList)
                        {
                            if (r.StartDateTime == minStartTime)
                                currList1.Add(r);
                        }

                        if (currList1.Count > 1)
                        {
                            model.CurrentOutageDetails = new List<OutageRecord>();
                            model.CurrentOutageDetails.Add(currList1.Where(c => c.OutageType == "HT").FirstOrDefault());
                        }
                        else
                            model.CurrentOutageDetails = currList1;
                    }
                    else
                        model.CurrentOutageDetails = currList;
                }


                foreach (var r in model.CurrentOutageDetails)
                {
                    switch (r.ActivityType)
                    {
                        case "1": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/UserDashboard/Notifications/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                        case "2": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/UserDashboard/Notifications/OutageInfomation/Disconnected for Safety", "Disconnected for Safety"); break;
                        case "3": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/UserDashboard/Notifications/OutageInfomation/Breakdown", "Breakdown"); break;
                        case "4": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/UserDashboard/Notifications/OutageInfomation/Transmission outage", "Transmission outage"); break;
                        default: r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/UserDashboard/Notifications/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                    }
                }
                foreach (var r in model.FutureOutageDetails)
                {
                    switch (r.ActivityType)
                    {
                        case "1": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/UserDashboard/Notifications/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                        case "2": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/UserDashboard/Notifications/OutageInfomation/Disconnected for Safety", "Disconnected for Safety"); break;
                        case "3": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/UserDashboard/Notifications/OutageInfomation/Breakdown", "Breakdown"); break;
                        case "4": r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/UserDashboard/Notifications/OutageInfomation/Transmission outage", "Transmission outage"); break;
                        default: r.ActivityDescription = DictionaryPhraseRepository.Current.Get("/UserDashboard/Notifications/OutageInfomation/Planned Maintenance", "Planned Maintenance"); break;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at " + MethodBase.GetCurrentMethod().Name + ":" + ex.Message, this);
            }
            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserDashboardSubmitMeterReadingNotification()
        {
            var profile = this.UserProfileService.GetProfile(Context.User);
            SubmitMeterReading meterReading = new SubmitMeterReading();
            meterReading.IsSubmitted = false;
            if (profile.AccountNumber != null)
            {
                try
                {
                    meterReading.Source = "3";
                    meterReading.CANumber = profile.AccountNumber;
                    var Reqoutput = SapPiService.Services.RequestHandler.ReadMeterNumberForSelfMeterReading(meterReading.CANumber, "", meterReading.Source);
                    List<MeterReadingDetail> MeterList = new List<MeterReadingDetail>();
                    List<MeterAttachment> MeterAttachmentList = new List<MeterAttachment>();

                    if (Reqoutput.IT_OUTPUT.Length > 0)
                    {
                        foreach (ZBAPI_DM_GETMETERNUMBERService.ZBAPI_DM_T_GETMETERNUMBER_OUT output in Reqoutput.IT_OUTPUT)
                        {
                            MeterReadingDetail detail = new MeterReadingDetail();
                            detail.MeterNumber = output.METER_NUMBER;
                            detail.SMRD = output.SMRD;
                            string DT = System.DateTime.Now.ToString("dd-MM-yyyy");
                            detail.MeterReadingDate = DateTime.ParseExact(DT, "dd-MM-yyyy", CultureInfo.InvariantCulture);

                            MeterList.Add(detail);
                            MeterAttachment attachment = new MeterAttachment();
                            attachment.Id = Guid.NewGuid();
                            MeterAttachmentList.Add(attachment);

                        }

                    }
                    else
                    {
                        if (Reqoutput.IT_RETURN != null)
                        {
                            if (Reqoutput.IT_RETURN.Length > 0)
                            {
                                meterReading.IsSubmitted = true;
                                meterReading.Result = Reqoutput.IT_RETURN[0].MESSAGE.ToString().Replace(" ", "_"); ;
                            }
                        }
                        return Json(new { data = meterReading }, JsonRequestBehavior.AllowGet);
                    }

                    meterReading.MeterList = MeterList;
                    meterReading.MeterAttachments = MeterAttachmentList;
                }
                catch (Exception ex)
                {
                    meterReading.Result = ex.Message;
                    Log.Error(" my account submit meter reading " + ex.Message, this);
                    return Json(new { data = "0" }, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(new { data = meterReading }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UserDashboardComplaintsNotification()
        {
            ComplaintPortalService objservice = new ComplaintPortalService();
            TrackComplaintModel model = new TrackComplaintModel();

            model.ComplaintList = objservice.GetOpenComplaintsFromSAPByCA(SessionHelper.UserSession.UserSessionContext.primaryAccountNumber);

            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserDashboardCGRFComplaintsNotifications()
        {
            TrackComplaintModel model = new TrackComplaintModel();
            ComplaintPortalService objservice = new ComplaintPortalService();
            model.ComplaintList = objservice.GetCGRFOpenComplaintsByAccountNumber(SessionHelper.UserSession.UserSessionContext.primaryAccountNumber, "7");

            return Json(new { data = model }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult UserDashboardElectricityCharges()
        {
            try
            {
                InvoiceLine LatestBillDetails = SapPiService.Services.RequestHandler.FetchInvoiceHistory(SessionHelper.UserSession.UserSessionContext.primaryAccountNumber).InvoiceLines.FirstOrDefault();
                if (LatestBillDetails != null)
                {
                    var OnlineBillPdfResponse = SapPiService.Services.RequestHandler.FetchOnlineBillPDF(LatestBillDetails.AccountNumber, LatestBillDetails.InvoiceNumber);
                    if (!string.IsNullOrEmpty(OnlineBillPdfResponse.TXT.FirstOrDefault().ZTXT))
                    {
                        var OnlineBillPdf = OnlineBillPdfResponse.TXT.FirstOrDefault().ZTXT.Split('|');
                        if (OnlineBillPdfResponse.FTYP_FLG == "T")
                        {
                            var details = new
                            {
                                EnergyCharge = 17 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[17]) ? OnlineBillPdf[17] : string.Empty, //18
                                FixedDemandCharge = 19 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[19]) ? OnlineBillPdf[19] : string.Empty, //20
                                FueladjustmentCharges = 24 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[24]) ? OnlineBillPdf[24] : string.Empty, //25
                                GovernmentElectricityDuty = 131 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[131]) ? OnlineBillPdf[131] : string.Empty, //132
                                Wheelingcharge = 366 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[366]) ? OnlineBillPdf[366] : string.Empty, //367
                                RegulatoryAssetCharge = 368 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[368]) ? OnlineBillPdf[368] : string.Empty, //369
                                Govtdutyrate = 11 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[11]) ? OnlineBillPdf[11] : string.Empty, //12
                                TaxOnSale = 130 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[130]) ? OnlineBillPdf[130] : string.Empty, //131
                                MahGovtTaxOnSaleRate = 21 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[21]) ? OnlineBillPdf[21] : string.Empty, //22
                                BillPeriodTo = 208 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[208]) ? OnlineBillPdf[208] : string.Empty, //209
                                BillPeriodFrom = 209 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[209]) ? OnlineBillPdf[209] : string.Empty, //210
                                CurrentMonthBillAmount = 180 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[180]) ? OnlineBillPdf[180] : string.Empty, //181
                                UnitConsumed = 125 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[125]) ? OnlineBillPdf[125] : string.Empty //126
                            };
                            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
                        }
                        else if (OnlineBillPdfResponse.FTYP_FLG == "I")
                        {
                            var details = new
                            {
                                EnergyCharge = 200 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[200]) ? OnlineBillPdf[200] : string.Empty, //201
                                FixedDemandCharge = 199 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[199]) ? OnlineBillPdf[199] : string.Empty, //200
                                FueladjustmentCharges = 205 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[205]) ? OnlineBillPdf[205] : string.Empty, //206
                                GovernmentElectricityDuty = 202 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[202]) ? OnlineBillPdf[202] : string.Empty, //203
                                Wheelingcharge = 554 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[554]) ? OnlineBillPdf[554] : string.Empty, //555
                                RegulatoryAssetCharge = 556 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[556]) ? OnlineBillPdf[556] : string.Empty, //557
                                Govtdutyrate = 255 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[255]) ? OnlineBillPdf[255] : string.Empty, //256
                                TaxOnSale = 207 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[207]) ? OnlineBillPdf[207] : string.Empty, //208
                                MahGovtTaxOnSaleRate = 256 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[256]) ? OnlineBillPdf[256] : string.Empty, //257
                                BillPeriodTo = 4 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[4]) ? OnlineBillPdf[4] : string.Empty, //5
                                BillPeriodFrom = 6 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[6]) ? OnlineBillPdf[6] : string.Empty, //7
                                CurrentMonthBillAmount = 219 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[219]) ? OnlineBillPdf[219] : string.Empty, //220
                                UnitConsumed = 173 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[173]) ? OnlineBillPdf[173] : string.Empty //174
                                //PenaltyOnDemandQTY = 199 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[199]) ? OnlineBillPdf[199] : string.Empty,
                                //PowerFactorSurchargeIncentive = 204 <= OnlineBillPdf.Count() && !string.IsNullOrEmpty(OnlineBillPdf[204]) ? OnlineBillPdf[204] : string.Empty
                            };
                            return Json(new { data = details }, JsonRequestBehavior.AllowGet);
                        }
                    }
                }

                return Json(new { data = "0" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Error(" my account submit meter reading " + ex.Message, this);
                return Json(new { data = "0" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}