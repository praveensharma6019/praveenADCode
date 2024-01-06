using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Ports.Website.Models;
using Sitecore.Ports.Website.Services;
using System;
using RestSharp;
using System.Collections.Generic;
using System.Linq;
using Sitecore;
using Sitecore.Mvc;
using System.Data.Linq.SqlClient;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Sitecore.Foundation.SitecoreExtensions.Attributes;
using System.Data;
using Sitecore.Ports.Website.AnchorageAMCTPLServiceReference;
using Sitecore.Ports.Website.AnchorageAICTPLServiceReference;
using Sitecore.Ports.Website.DharmaBerthServiceReference;
using Sitecore.Ports.Website.DharmaExpectedServiceReference;
using Sitecore.Ports.Website.DharmaSailedServiceReference;
using Sitecore.Ports.Website.ExpectedACMTPLServiceReference;
using Sitecore.Ports.Website.ExpectedAICTPLServiceReference;
using Sitecore.Ports.Website.AnchorageAMCTServiceReference;
using Sitecore.Ports.Website.AnchorageBreakBulkServiceReference;
using Sitecore.Ports.Website.AnchorageBULKServiceReference;
using Sitecore.Ports.Website.AnchorageBunkerServiceReference;
using Sitecore.Ports.Website.AnchorageHMELServiceReference;
using Sitecore.Ports.Website.AnchorageIOCLServiceReference;
using Sitecore.Ports.Website.AnchorageLiquidVesselsServiceReference;
using Sitecore.Ports.Website.AnchorageMICTServiceReference;
using Sitecore.Ports.Website.AnchorageOTHERServiceReference;
using Sitecore.Ports.Website.AnchorageSTSServiceReference;
using Sitecore.Ports.Website.AnchorageWestBasinServiceReference;
using Sitecore.Ports.Website.DahejAnchorageServiceReference;
using Sitecore.Ports.Website.DahejBerthServiceReference;
using Sitecore.Ports.Website.DahejExpectedServiceReference;
using Sitecore.Ports.Website.DahejSailedServiceReference;
using Sitecore.Ports.Website.ExpectedAMCTServiceReference;
using Sitecore.Ports.Website.ExpectedBreakBilkServiceReference;
using Sitecore.Ports.Website.ExpectedBulkServiceReference;
using Sitecore.Ports.Website.ExpectedBunkerServiceReference;
using Sitecore.Ports.Website.ExpectedHMELServiceReference;
using Sitecore.Ports.Website.ExpectedIOCLServiceReference;
using Sitecore.Ports.Website.ExpectedLiquidServiceReference;
using Sitecore.Ports.Website.ExpectedMICTServiceReference;
using Sitecore.Ports.Website.ExpectedSTSServiceReference;
using Sitecore.Ports.Website.ExpectedWestBasinServiceReference;
using Sitecore.Data.Items;
using System.Text;
using Sitecore.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Sitecore.Ports.Website.PortBerthACMTPLServiceReference;
using Sitecore.Ports.Website.PortBerthAICTPLServiceReference;
using Sitecore.Ports.Website.PortsBerthShipAMCTTServiceReference;
using Sitecore.Ports.Website.PortsBerthShipAtMICTServiceReference;
using Sitecore.Ports.Website.PortsBerthShipSPMSTSServiceReference;
using Sitecore.Ports.Website.MundraContainerTrackingServiceReference;
using Sitecore.Mvc.Presentation;
using Sitecore.Ports.Website;
using Sitecore.Ports.Website.SailedACMTPLServiceReference;
using Sitecore.Ports.Website.SailedAICTPLServiceReference;
using Sitecore.Ports.Website.SailedAMCTServiceReference;
using Sitecore.Ports.Website.SailedBreakBulkServiceReference;
using Sitecore.Ports.Website.SailedBulkServiceReference;
using Sitecore.Ports.Website.SailedBunkerServiceReference;
using Sitecore.Ports.Website.SailedHMELServiceReference;
using Sitecore.Ports.Website.SailedIOCLServiceReference;
using Sitecore.Ports.Website.SailedLiquidServiceReference;
using Sitecore.Ports.Website.SailedMICTServiceReference;
using Sitecore.Ports.Website.SailedSTSServiceReference;
using Sitecore.Ports.Website.SailedWestBasinServiceReference;
using System.Net.Mime;
using System.Globalization;
using Sitecore.Ports.Website.MundraCT4ContainerTrackingServiceReference;
using Sitecore.Ports.Website.EnnoreNewContainerTrackingServiceReference;
using Sitecore.Ports.Website.MundraCT2ContainerTrackingServiceReference;
using System.Text.RegularExpressions;
using System.Net;
using System.IO;
using Sitecore.Ports.Website.Helper;
using Sitecore.Exceptions;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Ports.Website.Provider;
using static Sitecore.Configuration.State;
using Sitecore.Mvc.Common;
using System.Runtime.InteropServices;
using Sitecore.Ports.Website.Attribute;
using Sitecore.Configuration;
using Sitecore.Ports.Website.Providers;

namespace Sitecore.Ports.Website.Controllers
{
    public class PortsController : Controller
    {
        Recaptchav2Provider captchavalidate = new Recaptchav2Provider();
        private PortsRepository portsrepo = new PortsRepository();
        readonly BlobAPIService blobAPIService = new BlobAPIService();
        public string EncryptionKey = DictionaryPhraseRepository.Current.Get("/GSM/EncryptionKey", "Tl;jld@456763909QPwOeiRuTy873XY7");
        public string EncryptionIV = DictionaryPhraseRepository.Current.Get("/GSM/EncryptionIV", "CEIVRAJWquG8iiMw");


        public PortsController()
        {
        }

        public ActionResult AnchorageACMTPLVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_anchorage_ct4SoapClient()).get_anchorage_ct4_data();
                Log.Info("API calling from AnchorageACMTPLVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from AnchorageACMTPLVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult AnchorageAICTPLVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_anchorage_southbasinSoapClient()).get_anchorage_southbasin_data();
                Log.Info("API calling from AnchorageAICTPLVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from AnchorageAICTPLVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult AnchorageAMCTVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_anchorage_ct2SoapClient()).get_anchorage_ct2_data();
                Log.Info("API calling from AnchorageAMCTVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from AnchorageAMCTVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult AnchorageBreakBulkVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_anchorage_drypcSoapClient()).get_anchorage_drypc_data();
                Log.Info("API calling from AnchorageBreakBulkVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from AnchorageBreakBulkVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult AnchorageBULKVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_anchorage_drybcSoapClient()).get_anchorage_drybc_data();
                Log.Info("API calling from AnchorageBULKVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from AnchorageBULKVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult AnchorageBunkerVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_anchorage_bunkerSoapClient()).get_anchorage_bunker_data();
                Log.Info("API calling from AnchorageBunkerVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from AnchorageBunkerVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult AnchorageHMELVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_anchorage_spm_hmelSoapClient()).get_anchorage_spm_hmel_data();
                Log.Info("API calling from AnchorageHMELVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from AnchorageHMELVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult AnchorageIOCLVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_anchorage_spm_ioclSoapClient()).get_anchorage_spm_iocl_data();
                Log.Info("API calling from AnchorageIOCLVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from AnchorageIOCLVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult AnchorageLiquidVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_anchorage_liquidSoapClient()).get_anchorage_liquid_data();
                Log.Info("API calling from AnchorageLiquidVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from AnchorageLiquidVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult AnchorageMICTVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_anchorage_mictSoapClient()).get_anchorage_mict_data();
                Log.Info("API calling from AnchorageMICTVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from AnchorageMICTVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult AnchorageOTHERVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_anchorage_otherSoapClient()).get_anchorage_other_data();
                Log.Info("API calling from AnchorageOTHERVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from AnchorageOTHERVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult AnchorageSTSVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_anchorage_stsSoapClient()).get_anchorage_sts_data();
                Log.Info("API calling from AnchorageSTSVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from AnchorageSTSVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult AnchorageWestBasinVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_anchorage_westbasinSoapClient()).get_anchorage_westbasin_data();
                Log.Info("API calling from AnchorageWestBasinVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from AnchorageWestBasinVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult ContainerTrackingResult()
        {
            ActionResult actionResult;
            if (base.TempData["dataset"] == null)
            {
                actionResult = base.View();
            }
            else
            {
                DataSet ds = new DataSet();
                ds = (DataSet)base.TempData["dataset"];
                actionResult = base.View(ds);
            }
            return actionResult;
        }

        [HttpPost]
        public ActionResult CreateOTP(PortsContactModel model)
        {
            ActionResult actionResult;
            EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0" };
            try
            {
                this.portsrepo.DeleteOldOtp(model.Mobile);
                string generatedotp = this.portsrepo.StoreGeneratedOtp(model);
                try
                {
                    string apiurl = string.Format("https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=adtrotpi&subEnterpriseid=adtrotpi&pusheid=adtrotpi&pushepwd=adtrotpi22&msisdn={0}&sender=ADANIR&msgtext=Dear%20customer,%20please%20enter%20the%20verification%20code%20{1}%20to%20submit%20your%20web%20enquiry.", model.Mobile, generatedotp);
                    HttpClient client = new HttpClient()
                    {
                        BaseAddress = new Uri(apiurl)
                    };
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    if (!client.GetAsync(apiurl).Result.IsSuccessStatusCode)
                    {
                        Log.Error("OTP Api call failed. https://otp2.maccesssmspush.com", this);
                    }
                    else
                    {
                        Log.Error("OTP Api call success. https://otp2.maccesssmspush.com", this);
                    }
                }
                catch (Exception exception)
                {
                    Exception ex = exception;
                    Log.Error(string.Format("{0}", 0), ex, this);
                }
                result = new { status = "1" };
                actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                Log.Error(string.Format("{0}", 0), ex, this);
                actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
            }
            return actionResult;
        }

        public ActionResult DahejAnchorageVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_anchorageSoapClient()).get_vessel_at_anchorage_data();
                Log.Info("API calling from DahejAnchorageVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from DahejAnchorageVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult DahejBerthVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_ships_at_mptSoapClient()).get_ships_at_mpt_data();
                Log.Info("API calling from DahejBerthVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from DahejBerthVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult DahejExpectedVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_ships_expectedSoapClient()).get_ships_expected_data();
                Log.Info("API calling from DahejExpectedVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from DahejExpectedVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult DahejSailedVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_sailing_timeSoapClient()).get_sailing_time_data();
                Log.Info("API calling from DahejSailedVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from DahejSailedVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult DharmaAnchorageVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_anchorageSoapClient()).get_vessel_at_anchorage_data();
                Log.Info("API calling from DharmaAnchorageVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from DharmaAnchorageVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult DharmaBerthVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_berthSoapClient()).get_vessel_at_berth_data();
                Log.Info("API calling from DharmaBerthVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from DharmaBerthVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult DharmaExpectedVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_expectedSoapClient()).get_vessel_at_expected_data();
                Log.Info("API calling from DharmaExpectedVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from DharmaExpectedVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult DharmaSailedVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_sailedSoapClient()).get_vessel_at_sailed_data();
                Log.Info("API calling from DharmaExpectedVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from DharmaExpectedVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult ExpectedACMTPLVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_expected_ct4SoapClient()).get_expected_ct4_data();
                Log.Info("API calling from ExpectedACMTPLVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from ExpectedACMTPLVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult ExpectedAICTPLVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_expected_southbasinSoapClient()).get_expected_southbasin_data();
                Log.Info("API calling from ExpectedAICTPLVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from ExpectedAICTPLVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult ExpectedAMCTVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_expected_ct2SoapClient()).get_expected_ct2_data();
                Log.Info("API calling from ExpectedAMCTVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from ExpectedAMCTVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult ExpectedBreakBulkVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_expected_drypcSoapClient()).get_expected_drypc_data();
                Log.Info("API calling from ExpectedBreakBulkVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from ExpectedBreakBulkVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult ExpectedBULKVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_expected_drybcSoapClient()).get_expected_drybc_data();
                Log.Info("API calling from ExpectedBULKVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from ExpectedBULKVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult ExpectedBunkerVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_expected_bunkerSoapClient()).get_expected_bunker_data();
                Log.Info("API calling from ExpectedBunkerVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from ExpectedBunkerVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult ExpectedHMELVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_expected_spm_hmelSoapClient()).get_expected_spm_hmel_data();
                Log.Info("API calling from ExpectedHMELVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from ExpectedHMELVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult ExpectedIOCLVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_expected_spm_ioclSoapClient()).get_expected_spm_iocl_data();
                Log.Info("API calling from ExpectedIOCLVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from ExpectedIOCLVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult ExpectedLiquidVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_expected_liquidSoapClient()).get_expected_liquid_data();
                Log.Info("API calling from ExpectedLiquidVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from ExpectedLiquidVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult ExpectedMICTVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_expected_mictSoapClient()).get_expected_mict_data();
                Log.Info("API calling from ExpectedMICTVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from ExpectedMICTVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult ExpectedSTSVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_expected_stsSoapClient()).get_expected_sts_data();
                ds = (new get_expected_stsSoapClient()).get_expected_sts_data();
                Log.Info("API calling from ExpectedSTSVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from ExpectedSTSVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult ExpectedWestBasinVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_expected_westbasinSoapClient()).get_expected_westbasin_data();
                Log.Info("API calling from ExpectedWestBasinVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from ExpectedWestBasinVessels", e.Message), this);
            }
            return base.View(ds);
        }

        [HttpPost]
        [ValidateRenderingId]
        public JsonResult GetContactAddress(string itemId)
        {
            Item Location = Context.Database.GetItem(ID.Parse(itemId));
            string body = Location.Fields["Body"].Value;
            body = string.Concat(body, "Summary:", Location.Fields["Summary"].Value);
            return base.Json(body);
        }

        public DataSet getResultAsync(string regionCode, string ctrNo)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            try
            {
                string username = "websiteuser";
                string password = "3e9s1t@pr0d";
                string url = "https://entosbext.adani.com/SeachContainerProject/PS/SearchContainerPS";
                string responseData = string.Empty;
                byte[] authenticationBytes = Encoding.ASCII.GetBytes(string.Concat(username, ":", password));
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                    HttpContent content = new StringContent(string.Concat(new string[] { "{\"regionCode\": ", regionCode, ",\"requestBody\": {\"ctrNo\": \"", ctrNo, "\"}}" }));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    Task<HttpResponseMessage> res = client.PostAsync(url, content);
                    res.Result.EnsureSuccessStatusCode();
                    responseData = res.Result.Content.ReadAsStringAsync().Result;
                    dynamic obj = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(responseData));
                    JObject joResponse = (JObject)JObject.Parse(obj);
                    if (joResponse["data"].Type.ToString() == "Null")
                    {
                        dataTable.TableName = "Error";
                        dataSet.Tables.Add(dataTable);
                    }
                    else
                    {
                        JArray array = (JArray)joResponse["data"];
                        dataTable.Columns.Add(new DataColumn("Container No", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Base Sts", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Location", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Line", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("ISO Code", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("F/e Ind ", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Category", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Haz_Class", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Ref_Temp", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Weight", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("POD", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Entry Mode", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Entry DTTM", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Origin Code", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Exit Mode", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Exit DTTM", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Destination Code", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("VIA (D)", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Vessel Name (D)", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("VIA (L)", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Vessel Name (L)", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Vessel_Name", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Scan Status", typeof(string)));
                        DataRow newRow = dataTable.NewRow();
                        newRow["Container No"] = ((JArray)joResponse["data"])[0]["ctrNo"];
                        newRow["Base Sts"] = ((JArray)joResponse["data"])[0]["baseSts"];
                        newRow["Location"] = ((JArray)joResponse["data"])[0]["curLocInd"];
                        newRow["Line"] = ((JArray)joResponse["data"])[0]["lineCd"];
                        newRow["ISO Code"] = ((JArray)joResponse["data"])[0]["isoCd"];
                        newRow["F/e Ind "] = ((JArray)joResponse["data"])[0]["feInd"];
                        newRow["Category"] = ((JArray)joResponse["data"])[0]["ctrCatCd"];
                        newRow["Haz_Class"] = ((JArray)joResponse["data"])[0]["imcocd"];
                        newRow["Ref_Temp"] = ((JArray)joResponse["data"])[0]["actTemp"];
                        newRow["Weight"] = ((JArray)joResponse["data"])[0]["ctrWt"];
                        newRow["POD"] = ((JArray)joResponse["data"])[0]["pod"];
                        newRow["Entry Mode"] = ((JArray)joResponse["data"])[0]["entryMode"];
                        newRow["Entry DTTM"] = ((JArray)joResponse["data"])[0]["entryDttm"];
                        newRow["Origin Code"] = ((JArray)joResponse["data"])[0]["depotOriginCd"];
                        newRow["Exit Mode"] = ((JArray)joResponse["data"])[0]["exitMode"];
                        newRow["Exit DTTM"] = ((JArray)joResponse["data"])[0]["exitDttm"];
                        newRow["Destination Code"] = ((JArray)joResponse["data"])[0]["depotDestCd"];
                        newRow["VIA (D)"] = ((JArray)joResponse["data"])[0]["dschViaNo"];
                        newRow["Vessel Name (D)"] = ((JArray)joResponse["data"])[0]["dschVslNm"];
                        newRow["VIA (L)"] = ((JArray)joResponse["data"])[0]["ldViaNo"];
                        newRow["Vessel Name (L)"] = ((JArray)joResponse["data"])[0]["ldVslNm"];
                        newRow["Vessel_Name"] = ((JArray)joResponse["data"])[0]["vslNm"];
                        newRow["Scan Status"] = ((JArray)joResponse["data"])[0]["scanFlg"];
                        dataTable.Rows.Add(newRow);
                        dataSet.Tables.Add(dataTable);
                    }
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Error in API calling from getResultAsync", ex.Message), this);
                dataTable.TableName = "Error";
                dataSet.Tables.Add(dataTable);
            }
            return dataSet;
        }

        public DataSet GetContainerMundraCT4Result(string regionCode, string ctrNo)
        {
            DataSet dataSet = new DataSet();
            DataTable dataTable = new DataTable();
            try
            {
                string username = "websiteuser";
                string password = "3e9s1t@pr0d"; // websitepass;// "3e9s1t@pr0d";
                string url = "https://entosbext.adani.com/SeachContainerProject/PS/SearchContainerPS";
                string responseData = string.Empty;
                byte[] authenticationBytes = Encoding.ASCII.GetBytes(string.Concat(username, ":", password));
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", System.Convert.ToBase64String(authenticationBytes));
                    HttpContent content = new StringContent(string.Concat(new string[] { "{\"regionCode\": ", regionCode, ",\"requestBody\": {\"ctrNo\": \"", ctrNo, "\"}}" }));
                    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    Task<HttpResponseMessage> res = client.PostAsync(url, content);
                    res.Result.EnsureSuccessStatusCode();
                    responseData = res.Result.Content.ReadAsStringAsync().Result;
                    dynamic obj = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(responseData));
                    JObject joResponse = (JObject)JObject.Parse(obj);
                    if (joResponse["data"].Type.ToString() == "Null")
                    {
                        dataTable.TableName = "Error";
                        dataSet.Tables.Add(dataTable);
                    }
                    else
                    {
                        JArray array = (JArray)joResponse["data"];
                        dataTable.Columns.Add(new DataColumn("Container No", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Base Sts", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Location", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Line", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("ISO Code", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("F/e Ind ", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Category", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Haz_Class", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Ref_Temp", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Weight", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("POD", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Entry Mode", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Entry DTTM", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Origin Code", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Exit Mode", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Exit DTTM", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Destination Code", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("VIA (D)", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Vessel Name (D)", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("VIA (L)", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Vessel Name (L)", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Vessel_Name", typeof(string)));
                        dataTable.Columns.Add(new DataColumn("Scan Status", typeof(string)));
                        DataRow newRow = dataTable.NewRow();
                        newRow["Container No"] = ((JArray)joResponse["data"])[0]["ctrNo"];
                        newRow["Base Sts"] = ((JArray)joResponse["data"])[0]["baseSts"];
                        newRow["Location"] = ((JArray)joResponse["data"])[0]["curLocInd"];
                        newRow["Line"] = ((JArray)joResponse["data"])[0]["lineCd"];
                        newRow["ISO Code"] = ((JArray)joResponse["data"])[0]["isoCd"];
                        newRow["F/e Ind "] = ((JArray)joResponse["data"])[0]["feInd"];
                        newRow["Category"] = ((JArray)joResponse["data"])[0]["ctrCatCd"];
                        newRow["Haz_Class"] = ((JArray)joResponse["data"])[0]["imcocd"];
                        newRow["Ref_Temp"] = ((JArray)joResponse["data"])[0]["actTemp"];
                        newRow["Weight"] = ((JArray)joResponse["data"])[0]["ctrWt"];
                        newRow["POD"] = ((JArray)joResponse["data"])[0]["pod"];
                        newRow["Entry Mode"] = ((JArray)joResponse["data"])[0]["entryMode"];
                        newRow["Entry DTTM"] = ((JArray)joResponse["data"])[0]["entryDttm"];
                        newRow["Origin Code"] = ((JArray)joResponse["data"])[0]["depotOriginCd"];
                        newRow["Exit Mode"] = ((JArray)joResponse["data"])[0]["exitMode"];
                        newRow["Exit DTTM"] = ((JArray)joResponse["data"])[0]["exitDttm"];
                        newRow["Destination Code"] = ((JArray)joResponse["data"])[0]["depotDestCd"];
                        newRow["VIA (D)"] = ((JArray)joResponse["data"])[0]["dschViaNo"];
                        newRow["Vessel Name (D)"] = ((JArray)joResponse["data"])[0]["dschVslNm"];
                        newRow["VIA (L)"] = ((JArray)joResponse["data"])[0]["ldViaNo"];
                        newRow["Vessel Name (L)"] = ((JArray)joResponse["data"])[0]["ldVslNm"];
                        newRow["Vessel_Name"] = ((JArray)joResponse["data"])[0]["vslNm"];
                        newRow["Scan Status"] = ((JArray)joResponse["data"])[0]["scanFlg"];
                        dataTable.Rows.Add(newRow);
                        dataSet.Tables.Add(dataTable);
                    }
                }
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Error(string.Concat("Error in API calling from GetContainerMundraCT4Result", ex.Message), this);
                dataTable.TableName = "Error";
                dataSet.Tables.Add(dataTable);
            }
            return dataSet;
        }


        public ActionResult HaziraAnchorageVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new HaziraAnchorageServiceReference.get_vessel_at_anchorageSoapClient()).get_vessel_at_anchorage_data();
                Log.Info("API calling from HaziraAnchorageVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from HaziraAnchorageVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult HaziraBerthVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new HaziraBerthServiceReference.get_ships_at_mptSoapClient()).get_ships_at_mpt_data();
                Log.Info("API calling from HaziraBerthVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from HaziraBerthVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult HaziraExpectedVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new HaziraExpectedServiceReference.get_ships_expectedSoapClient()).get_ships_expected_data();
                Log.Info("API calling from HaziraExpectedVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from HaziraExpectedVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult HaziraSailedVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new HaziraSailedServiceReference.get_sailing_timeSoapClient()).get_sailing_time_data();
                Log.Info("API calling from HaziraSailedVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from HaziraSailedVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult Index()
        {
            return base.View();
        }

        [HttpPost]
        public ActionResult InsertContactdetail(PortsContactModel m)
        {
            Log.Error("Insertcontactdetail ", "Start");
            string getEmailTo = "";
            string getMessage = "";
            string getEmailSubject = "";
            var result = new { status = "1" };
            bool Validated = false;
            try
            {
                //  Recaptchav2Provider recaptchav2 = new Recaptchav2Provider();
                // Validated = captchavalidate.IsReCaptchValid(m.reResponse);
                Recaptchav3Provider recaptchav2 = new Recaptchav3Provider();
                Validated = recaptchav2.IsReCaptchValid(m.reResponse);
            }
            catch (Exception ex)
            {
                Exception exception = ex;
                Log.Info(string.Concat("ContactUsForm Failed to validate auto script : ", ex.ToString()), this);
                result = new { status = "0" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            try
            {
                PortsFormDataDataContext rdb = new PortsFormDataDataContext();
                PortsFormData r = new PortsFormData()
                {
                    Name = m.Name,
                    Email = m.Email,
                    MessageType = m.MessageType,
                    Mobile = m.Mobile,
                    Message = m.Message,
                    FormType = m.FormType,
                    PageInfo = m.PageInfo,
                    FormSubmitOn = new DateTime?(m.FormSubmitOn)
                };
                rdb.PortsFormDatas.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                result = new { status = "0" };
                Console.WriteLine(ex);
            }
            try
            {
                Item msgTpye = Context.Database.GetItem("{5AC563B9-1E93-47FA-B383-2EA0266336C9}");
                IEnumerable<Item> getfilteredItem =
                    from x in msgTpye.Children
                    where x.Fields["SubjectName"].Value.ToLower() == m.MessageType.ToLower()
                    select x;
                foreach (Item itemData in getfilteredItem.ToList<Item>())
                {
                    getEmailTo = itemData.Fields["Email"].Value;
                    getMessage = itemData.Fields["Body"].Value;
                    getEmailSubject = itemData.Fields["EmailSubject"].Value;
                }
                getMessage = string.Concat("Hello ", m.Name, ",<br><br>", getMessage);
                string from = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailFrom", "");
                if (this.sendEmail(m.Email, getEmailSubject, getMessage, from))
                {
                    Log.Error("Email Sent to specific Message Subject type- ", "");
                }
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                result = new { status = "0" };
                Log.Error("Failed to get email from subject", ex.ToString());
            }
            try
            {
                string from = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailFrom", "");
                string emailText = DictionaryPhraseRepository.Current.Get("/ContactUs/EmailText", "");
                string message = "";
                message = string.Concat("Hello<br><br>", emailText, "<br><br>Name: ", m.Name);
                if (this.sendEmail(getEmailTo, getEmailSubject, string.Concat(new string[] { message, "<br>Email: ", m.Email, "<br>Subject of Message: ", m.MessageType, "<br>Message: ", m.Message, "<br><br>Thanks" }), from))
                {
                    Log.Error("Email Sent- ", "");
                }
            }
            catch (Exception exception2)
            {
                Exception ex = exception2;
                result = new { status = "1" };
                Log.Error("Failed to sent Email", ex.ToString());
            }
            return base.Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertLogisticsQuotedetail(PortsLogisticsQuoteModel m)
        {
            Log.Error("InsertLogisticsQuotedetail ", "Start");
            var result = new { status = "1" };
            try
            {
                PortsQuoteDataDataContext rdb = new PortsQuoteDataDataContext();
                PortsLogisticsQuoteDetail r = new PortsLogisticsQuoteDetail()
                {
                    Name = m.Name,
                    Organization = m.Organization,
                    Email = m.Email,
                    ContactNumber = m.ContactNumber,
                    Service = m.Service,
                    Details = m.Details,
                    FormType = m.FormType,
                    PageInfo = m.PageInfo,
                    FormSubmitOn = new DateTime?(m.FormSubmitOn)
                };
                rdb.PortsLogisticsQuoteDetails.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                result = new { status = "0" };
                Console.WriteLine(ex);
            }
            try
            {
                string emailText = DictionaryPhraseRepository.Current.Get("/LogisticsRequestQuote/EmailText", "");
                string message = "";
                message = string.Concat("Hello<br><br>", emailText, "<br><br>");
                message = string.Concat(new string[] { message, "<br>Requestor Name: ", m.Name, "<br>Organization Name: ", m.Organization, "<br>Email: ", m.Email, "<br>Contact Number: ", m.ContactNumber, "<br>Service: ", m.Service, "<br>Details: ", m.Details, "<br><br>Thanks" });
                string to = DictionaryPhraseRepository.Current.Get("/LogisticsRequestQuote/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/LogisticsRequestQuote/EmailFrom", "");
                if (this.sendEmail(to, DictionaryPhraseRepository.Current.Get("/LogisticsRequestQuote/EmailSubject", ""), message, from))
                {
                    Log.Error("Email Sent- ", "");
                }
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                result = new { status = "1" };
                Log.Error("Failed to sent Email", ex.ToString());
            }
            return base.Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertQuotedetail(PortsQuoteModel m)
        {
            Log.Error("InsertQuotedetail ", "Start");
            var result = new { status = "1" };
            try
            {
                PortsQuoteDataDataContext rdb = new PortsQuoteDataDataContext();
                PortsQuoteDetail r = new PortsQuoteDetail()
                {
                    Name = m.Name,
                    Company = m.Company,
                    Email = m.Email,
                    DeliveryType = m.DeliveryType,
                    Commodity = m.Commodity,
                    CommodityType = m.CommodityType,
                    ExpectedQuantity = m.ExpectedQuantity,
                    ExpectedDate = m.ExpectedDate,
                    Message = m.Message,
                    FormType = m.FormType,
                    PageInfo = m.PageInfo,
                    FormSubmitOn = new DateTime?(m.FormSubmitOn)
                };
                rdb.PortsQuoteDetails.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                result = new { status = "0" };
                Console.WriteLine(ex);
            }
            try
            {
                string emailText = DictionaryPhraseRepository.Current.Get("/RequestQuote/EmailText", "");
                string message = "";
                message = string.Concat("Hello<br><br>", emailText, "<br><br>");
                message = string.Concat(new string[] { message, "<br>Name: ", m.Name, "<br>Company: ", m.Company, "<br>Email: ", m.Email, "<br>Type: ", m.DeliveryType, "<br>Commodity: ", m.Commodity, "<br>Commodity Type: ", m.CommodityType, "<br>Expected Quantity: ", m.ExpectedQuantity, "<br>Expected Vessel Arrival Date: ", m.ExpectedDate, "<br>Message: ", m.Message, "<br><br>Thanks" });
                string to = DictionaryPhraseRepository.Current.Get("/RequestQuote/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/RequestQuote/EmailFrom", "");
                if (this.sendEmail(to, DictionaryPhraseRepository.Current.Get("/RequestQuote/EmailSubject", ""), message, from))
                {
                    Log.Error("Email Sent- ", "");
                }
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                result = new { status = "1" };
                Log.Error("Failed to sent Email", ex.ToString());
            }
            return base.Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult KattupalliAnchorageVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_anchorageSoapClient()).get_vessel_at_anchorage_data();
                Log.Info("API calling from KattupalliAnchorageVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from KattupalliAnchorageVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult KattupalliBerthVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_berthSoapClient()).get_vessel_at_berth_data();
                Log.Info("API calling from KattupalliBerthVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from KattupalliBerthVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult KattupalliExpectedVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_expectedSoapClient()).get_vessel_at_expected_data();
                Log.Info("API calling from KattupalliExpectedVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from KattupalliExpectedVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult KattupalliSailedVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_sailedSoapClient()).get_vessel_at_sailed_data();
                Log.Info("API calling from KattupalliSailedVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from KattupalliSailedVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult MurmugaoAnchorageVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_anchorageSoapClient()).get_vessel_at_anchorage_data();
                Log.Info("API calling from MurmugaoBerthVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from MurmugaoBerthVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult MurmugaoBerthVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_berthSoapClient()).get_vessel_at_berth_data();
                Log.Info("API calling from MurmugaoBerthVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from MurmugaoBerthVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult MurmugaoExpectedVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_expectedSoapClient()).get_vessel_at_expected_data();
                Log.Info("API calling from MurmugaoExpectedVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from MurmugaoExpectedVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult MurmugaoSailedVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_sailedSoapClient()).get_vessel_at_sailed_data();
                Log.Info("API calling from MurmugaoSailedVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from MurmugaoSailedVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult PortAcountRegisteration()
        {
            return base.View("/Views/Ports/Sublayouts/PortAcountRegisterations.cshtml");
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        public ActionResult PortAcountRegisteration(AccountRegistrationForm m)
        {
            AccountRegistrationForm a1 = new AccountRegistrationForm();
            bool validationStatus = false;
            var result = new { status = "", ErrorMessage = "" };
            //    var result1 = new { status = "1" };
            Log.Info("Insert Account Registration details of AdaniPorts ", "Start");
            string getEmailTo = "";
            string getMessage = "";
            string getEmailSubject = "";
            var userID = m.GenerateId();
            // Recaptchav3Provider recaptchav2 = new Recaptchav3Provider();
            // validationStatus = captchavalidate.IsReCaptchValidV3(m.reResponse);
            Recaptchav3Provider recaptchav2 = new Recaptchav3Provider();
            validationStatus = recaptchav2.IsReCaptchValid(m.reResponse);
            if (validationStatus == false)
            {
                Log.Error("Captcha verification failed " + validationStatus, this);
                result = new { status = "1", ErrorMessage = "Invalid Captcha" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var errorResponse = new ErrorResponse();
            validationStatus = ValidationProvider.Validate(m, out errorResponse);
            if (!validationStatus)
            {
                return Json(errorResponse, JsonRequestBehavior.AllowGet);
            }

            if (validationStatus)
            {
                try
                {
                    AccountRegistrationFormDataContext rdb1 = new AccountRegistrationFormDataContext();
                    PortsAccountRegistrationForm r = new PortsAccountRegistrationForm()
                    {
                        UserRegisterationID = userID,
                        TypeofCustomers = m.TypeofCustomers,
                        PortType = m.PortType,
                        ShippingAgentDetails = m.ShippingAgentDetails,
                        NameofPrincipal = m.NameofPrincipal,
                        NameofCompany = m.NameofCompany,
                        SCMTRCode = m.SCMTRCode,
                        SCMTRID = m.SCMTRID,
                        CompanyRegistrationDate = DateTime.Parse(m.CompanyRegistrationDate.ToString(), CultureInfo.GetCultureInfo("en-gb")),
                        PANNumber = m.PANNumber,
                        TANNumber = m.TANNumber,
                        GSTNumber = m.GSTNumber,
                        RegisteredOfficeAddress = m.RegisteredOfficeAddress,
                        RegisteredOfficeState = m.RegisteredOfficeState,
                        RegisteredOfficeCountry = m.RegisteredOfficeCountry,
                        RegisteredOfficeContactNumber = m.RegisteredOfficeContactNumber,
                        RegisteredOfficeEmail = m.RegisteredOfficeEmail,
                        LocalOfficeAddress = m.LocalOfficeAddress,
                        LocalOfficeState = m.LocalOfficeState,
                        LocalOfficeCountry = m.LocalOfficeCountry,
                        LocalOfficeContactNumber = m.LocalOfficeContactNumber,
                        LocalOfficeEmail = m.LocalOfficeEmail,
                        BillingAddress = m.BillingAddress,
                        BillingState = m.BillingState,
                        BillingCountry = m.BillingCountry,
                        BillingContactNumber = m.BillingContactNumber,
                        BillingEmail = m.BillingEmail,
                        OperationsName = m.OperationsName,
                        OperationsDesignation = m.OperationsDesignation,
                        OperationDirectNo = m.OperationDirectNo,
                        OperationMobileNumber = m.OperationMobileNumber,
                        OperationEmailID = m.OperationEmailID,
                        OperationsName2 = m.OperationsName2,
                        OperationsDesignation2 = m.OperationsDesignation2,
                        OperationDirectNo2 = m.OperationDirectNo2,
                        OperationMobileNumber2 = m.OperationMobileNumber2,
                        OperationEmailID2 = m.OperationEmailID2,
                        OperationsName3 = m.OperationsName3,
                        OperationsDesignation3 = m.OperationsDesignation3,
                        OperationDirectNo3 = m.OperationDirectNo3,
                        OperationMobileNumber3 = m.OperationMobileNumber3,
                        OperationEmailID3 = m.OperationEmailID3,
                        FinanceName = m.FinanceName,
                        FinanceDesignation = m.FinanceDesignation,
                        FinanceDirectNo = m.FinanceDirectNo,
                        FinanceMobileNumber = m.FinanceMobileNumber,
                        FinanceEmailID = m.FinanceEmailID,
                        FinanceName2 = m.FinanceName2,
                        FinanceDesignation2 = m.FinanceDesignation2,
                        FinanceDirectNo2 = m.FinanceDirectNo2,
                        FinanceMobileNumber2 = m.FinanceMobileNumber2,
                        FinanceEmailID2 = m.FinanceEmailID2,
                        FinanceName3 = m.FinanceName3,
                        FinanceDesignation3 = m.FinanceDesignation3,
                        FinanceDirectNo3 = m.FinanceDirectNo3,
                        FinanceMobileNumber3 = m.FinanceMobileNumber3,
                        FinanceEmailID3 = m.FinanceEmailID3,
                        MICTRegisterCode = m.MICTRegisterCode,
                        CT2RegisterCode = m.CT2RegisterCode,
                        CT3RegisterCode = m.CT3RegisterCode,
                        CT4RegisterCode = m.CT4RegisterCode,
                        T2RegisterCode = m.T2RegisterCode,
                        Rupees = m.Rupees,
                        RupeesInWords = m.RupeesInWords,
                        Name = m.Name,
                        Designation = m.Designation,
                        SubmitOnDate = DateTime.Parse(DateTime.Now.ToString()),
                        CommunicationEmail1 = m.CommunicationEmail1 != "undefined" ? m.CommunicationEmail1 : null,
                        CommunicationEmail2 = m.CommunicationEmail2 != "undefined" ? m.CommunicationEmail2 : null,
                        CommunicationEmail3 = m.CommunicationEmail3 != "undefined" ? m.CommunicationEmail3 : null,
                        FormType = "port-AccountRegistration",
                        PageInfo = "/portAccountRegistration"

                    };

                    var AgreementWithPrincipal = Request.Files["AgreementWithPrincipal"];
                    var AcknowledgementGSTRegistration = Request.Files["AcknowledgementGSTRegistration"];
                    var CancelledCheque = Request.Files["CancelledCheque"];
                    var CertificationOfIncorporation = Request.Files["CertificationOfIncorporation"];
                    var CustomDPDPermission = Request.Files["CustomDPDPermission"];
                    var DrivingLicense = Request.Files["DrivingLicense"];
                    var IECodeCertificate = Request.Files["IECodeCertificate"];
                    var MemorandumArticleAssociation = Request.Files["MemorandumArticleAssociation"];
                    var MunicipalLicence = Request.Files["MunicipalLicence"];
                    var PanCard = Request.Files["PanCard"];
                    var SCMTRProof = Request.Files["SCMTRProof"];
                    var AEOLicence = Request.Files["AEOLicence"];
                    var PowerofAttorney = Request.Files["PowerofAttorney"];

                    string[] contenttypeExtenstion = new string[] { "application/pdf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/msword", "image/jpeg" };

                    if (!string.IsNullOrEmpty(m.TypeofCustomers))
                    {
                        if (CertificationOfIncorporation != null)
                        {
                            var CertificationOfIncorporationmime = CertificationOfIncorporation.ContentType;
                            if (!contenttypeExtenstion.Contains(CertificationOfIncorporationmime.ToLower()))
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CertificationOfIncorporationError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string filecontent = GetFileBinaryData(CertificationOfIncorporation);

                            if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CertificationOfIncorporationError", ErrorMessage = "Please upload only .pdf, .doc, .docx , .jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string fileName = CertificationOfIncorporation.FileName;
                            CertificationOfIncorporation.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                            r.CertificationOfIncorporation = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                        }
                        if (CertificationOfIncorporation == null)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CertificationOfIncorporationError", ErrorMessage = "This Field is Required" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        if (PanCard != null)
                        {
                            var PanCardmime = AgreementWithPrincipal.ContentType;
                            if (!contenttypeExtenstion.Contains(PanCardmime.ToLower()))
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "PanCardError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string filecontent = GetFileBinaryData(PanCard);

                            if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "PanCardError", ErrorMessage = "Please upload only .pdf, .doc, .docx, .jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string fileName = PanCard.FileName;
                            PanCard.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                            r.PanCard = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                        }
                        if (PanCard == null)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "PanCardError", ErrorMessage = "This Field is Required" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        if (AcknowledgementGSTRegistration != null)
                        {
                            var AcknowledgementGSTRegistrationdmime = AcknowledgementGSTRegistration.ContentType;
                            if (!contenttypeExtenstion.Contains(AcknowledgementGSTRegistrationdmime.ToLower()))
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "AcknowledgementGSTRegistrationError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string filecontent = GetFileBinaryData(AcknowledgementGSTRegistration);

                            if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "AcknowledgementGSTRegistrationError", ErrorMessage = "Please upload only .pdf, .doc, .docx, .jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string fileName = AcknowledgementGSTRegistration.FileName;
                            AcknowledgementGSTRegistration.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                            r.AcknowledgementGSTRegistration = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                        }
                        if (AcknowledgementGSTRegistration == null)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "AcknowledgementGSTRegistrationError", ErrorMessage = "This Field is Required" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (m.TypeofCustomers == "Vessel-Agent")
                    {
                        if (MemorandumArticleAssociation != null)
                        {
                            var MemorandumArticleAssociationmime = MemorandumArticleAssociation.ContentType;
                            if (!contenttypeExtenstion.Contains(MemorandumArticleAssociationmime.ToLower()))
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "MemorandumArticleAssociationError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string filecontent = GetFileBinaryData(MemorandumArticleAssociation);

                            if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "MemorandumArticleAssociationError", ErrorMessage = "Please upload only .pdf, .doc, .docx, .jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string fileName = MemorandumArticleAssociation.FileName;
                            MemorandumArticleAssociation.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                            r.MemorandumArticleAssociation = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                        }
                        if (MemorandumArticleAssociation == null)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "MemorandumArticleAssociationError", ErrorMessage = "This Field is Required" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        if (PowerofAttorney != null)
                        {
                            var PowerofAttorneymime = PowerofAttorney.ContentType;
                            if (!contenttypeExtenstion.Contains(PowerofAttorneymime.ToLower()))
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "PowerofAttorneyError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string filecontent = GetFileBinaryData(PowerofAttorney);

                            if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "PowerofAttorneyError", ErrorMessage = "Please upload only .pdf, .doc, .docx , .jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string fileName = PowerofAttorney.FileName;
                            PowerofAttorney.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                            r.PowerofAttorney = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                        }
                        if (PowerofAttorney == null)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "PowerofAttorneyError", ErrorMessage = "This Field is Required" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                    }
                    else if (m.TypeofCustomers == "Shipping-Line" || m.TypeofCustomers == "NVOCC")
                    {
                        if (MemorandumArticleAssociation != null)
                        {
                            var MemorandumArticleAssociationmime = MemorandumArticleAssociation.ContentType;
                            if (!contenttypeExtenstion.Contains(MemorandumArticleAssociationmime.ToLower()))
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "MemorandumArticleAssociationError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string filecontent = GetFileBinaryData(MemorandumArticleAssociation);

                            if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "MemorandumArticleAssociationError", ErrorMessage = "Please upload only .pdf, .doc, .docx, .jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string fileName = MemorandumArticleAssociation.FileName;
                            MemorandumArticleAssociation.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                            r.MemorandumArticleAssociation = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                        }
                        if (MemorandumArticleAssociation == null)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "MemorandumArticleAssociationError", ErrorMessage = "This Field is Required" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        if (AgreementWithPrincipal != null)
                        {
                            var AgreementWithPrincipalmime = AgreementWithPrincipal.ContentType;
                            if (!contenttypeExtenstion.Contains(AgreementWithPrincipalmime.ToLower()))
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "AgreementWithPrincipalError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string filecontent = GetFileBinaryData(AgreementWithPrincipal);

                            if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "AgreementWithPrincipalError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string fileName = AgreementWithPrincipal.FileName;
                            AgreementWithPrincipal.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                            r.AgreementWithPrincipal = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                        }
                        if (AgreementWithPrincipal == null)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "AgreementWithPrincipalError", ErrorMessage = "This Field is Required" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }

                    }
                    else
                    {
                        if (IECodeCertificate != null)
                        {
                            var IECodeCertificatemime = IECodeCertificate.ContentType;
                            if (!contenttypeExtenstion.Contains(IECodeCertificatemime.ToLower()))
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "IECodeCertificateError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string filecontent = GetFileBinaryData(IECodeCertificate);

                            if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "IECodeCertificateError", ErrorMessage = "Please upload only .pdf, .doc, .docx Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string fileName = IECodeCertificate.FileName;
                            IECodeCertificate.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                            r.IECodeCertificate = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                        }
                        if (IECodeCertificate == null)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "IECodeCertificateError", ErrorMessage = "This Field is Required" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        if (CancelledCheque != null)
                        {
                            var CancelledChequemime = CancelledCheque.ContentType;
                            if (!contenttypeExtenstion.Contains(CancelledChequemime.ToLower()))
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CancelledChequeError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string filecontent = GetFileBinaryData(CancelledCheque);

                            if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                            {
                                errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CancelledChequeError", ErrorMessage = "Please upload only .pdf, .doc, .docx , .jpeg , .jpg Files" };
                                return Json(errorResponse, JsonRequestBehavior.AllowGet);
                            }
                            string fileName = CancelledCheque.FileName;
                            CancelledCheque.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                            r.CancelledCheque = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                        }
                        if (CancelledCheque == null)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CancelledChequeError", ErrorMessage = "This Field is Required" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                    }

                    if (DrivingLicense != null)
                    {
                        var DrivingLicensemime = DrivingLicense.ContentType;
                        if (!contenttypeExtenstion.Contains(DrivingLicensemime.ToLower()))
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "DrivingLicenseError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        string filecontent = GetFileBinaryData(DrivingLicense);

                        if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "DrivingLicenseError", ErrorMessage = "Please upload only .pdf, .doc, .docx , .jpeg , .jpg Files" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        string fileName = DrivingLicense.FileName;
                        DrivingLicense.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                        r.DrivingLicense = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                    }
                    if (DrivingLicense == null)
                    {
                        errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "DrivingLicenseError", ErrorMessage = "This Field is Required" };
                        return Json(errorResponse, JsonRequestBehavior.AllowGet);
                    }
                    if (MunicipalLicence != null)
                    {
                        var MunicipalLicencemime = MunicipalLicence.ContentType;
                        if (!contenttypeExtenstion.Contains(MunicipalLicencemime.ToLower()))
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "MunicipalLicenceError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        string filecontent = GetFileBinaryData(MunicipalLicence);

                        if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "MunicipalLicenceError", ErrorMessage = "Please upload only .pdf, .doc, .docx , .jpeg , .jpg Files" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        string fileName = MunicipalLicence.FileName;
                        MunicipalLicence.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                        r.MunicipalLicence = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                    }
                    if (AEOLicence != null)
                    {
                        var AEOLicencemime = AEOLicence.ContentType;
                        if (!contenttypeExtenstion.Contains(AEOLicencemime.ToLower()))
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "AEOLicenceError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        string filecontent = GetFileBinaryData(AEOLicence);

                        if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "AEOLicenceError", ErrorMessage = "Please upload only .pdf, .doc, .docx , .jpeg , .jpg Files" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        string fileName = AEOLicence.FileName;
                        AEOLicence.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                        r.AEOLicence = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                    }
                    if (CustomDPDPermission != null)
                    {
                        var CustomDPDPermissionmime = CustomDPDPermission.ContentType;
                        if (!contenttypeExtenstion.Contains(CustomDPDPermissionmime.ToLower()))
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CustomDPDPermissionError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        string filecontent = GetFileBinaryData(CustomDPDPermission);

                        if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "CustomDPDPermissionError", ErrorMessage = "Please upload only .pdf, .doc, .docx, .jpeg , .jpg Files" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        string fileName = CustomDPDPermission.FileName;
                        CustomDPDPermission.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                        r.CustomDPDPermission = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                    }
                    if (m.TypeofCustomers != "CHA")
                    {
                        if (SCMTRProof == null)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "SCMTRProofError", ErrorMessage = "This Field is Required" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                    }
                    if (SCMTRProof != null)
                    {
                        var SCMTRProofmime = SCMTRProof.ContentType;
                        if (!contenttypeExtenstion.Contains(SCMTRProofmime.ToLower()))
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "SCMTRProofError", ErrorMessage = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg Files" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        string filecontent = GetFileBinaryData(SCMTRProof);

                        if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                        {
                            errorResponse = new ErrorResponse { IsErrorResonse = false, FieldName = "SCMTRProofError", ErrorMessage = "Please upload only .pdf, .doc, .docx , .jpeg , .jpg Files" };
                            return Json(errorResponse, JsonRequestBehavior.AllowGet);
                        }
                        string fileName = SCMTRProof.FileName;
                        SCMTRProof.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName })));
                        r.SCMTRProof = string.Concat(new object[] { "/PortsAccountForms/Upload/", userID, "-", fileName });
                    }
                    rdb1.PortsAccountRegistrationForms.InsertOnSubmit(r);
                    rdb1.SubmitChanges();
                    Log.Info("Insert Account Registration details of AdaniPorts ", "End");
                    try
                    {
                        Item getContentItem = Context.Database.GetItem("{5246B436-3930-4C99-9E1E-7F10B6CF2F58}");
                        getEmailTo = getContentItem.Fields["EmailTo"].Value;
                        getMessage = getContentItem.Fields["Body"].Value;
                        getEmailSubject = getContentItem.Fields["EmailSubject"].Value;
                        getMessage = string.Concat("Hello ", m.Name, ",<br><br>", "Your Registeration ID is: ", userID, "<br><br>", getMessage);
                        string from = DictionaryPhraseRepository.Current.Get("/AccountRegistration/EmailFrom", "");
                        if (this.sendEmail(m.CommunicationEmail1, getEmailSubject, getMessage, from))
                        {
                            Log.Info("Email Sent to specific Message Subject type- ", "");
                        }
                        if (!string.IsNullOrEmpty(m.CommunicationEmail2))
                        {
                            if (this.sendEmail(m.CommunicationEmail2, getEmailSubject, getMessage, from))
                            {
                                Log.Info("Email2 Sent to specific Message Subject type- ", "");
                            }
                        }
                        if (!string.IsNullOrEmpty(m.CommunicationEmail3))
                        {
                            if (this.sendEmail(m.CommunicationEmail3, getEmailSubject, getMessage, from))
                            {
                                Log.Info("Email3 Sent to specific Message Subject type- ", "");
                            }
                        }
                    }
                    catch (Exception exception1)
                    {
                        Exception ex = exception1;
                        result = new { status = "0", ErrorMessage = exception1.Message };
                        Log.Error("Failed to get email from subject", ex.ToString());
                    }
                    try
                    {
                        Item getContentItem = Context.Database.GetItem("{F9A84BE6-DAC7-4CBD-B44C-A999A807FAD6}");
                        string getEmailTo2 = getContentItem.Fields["EmailTo"].Value;
                        string getMessage2 = getContentItem.Fields["Body"].Value;
                        string getEmailSubject2 = getContentItem.Fields["EmailSubject"].Value;
                        string from = DictionaryPhraseRepository.Current.Get("/AccountRegistration/EmailFrom", "");
                        if (this.sendEmail(getEmailTo2, getEmailSubject2, string.Concat(new string[] { getMessage2, "<br>ID: ", userID, "<br>Name: ", m.Name, "<br>Email-Id: ", m.CommunicationEmail1, "<br>Type of Customer: ", m.TypeofCustomers, "<br>Port: ", m.PortType, "<br>Shipping Agent Details: ", m.ShippingAgentDetails, "<br>Name of Principal: ", m.NameofPrincipal, "<br>Name of Company: ", m.NameofCompany, "<br>Comapny Reg Date: ", m.CompanyRegistrationDate.ToString(), "<br>PAN: ", m.PANNumber, "<br>TAN: ", m.TANNumber, "<br>GST: ", m.GSTNumber, "<br>Registered Office Address: ", m.RegisteredOfficeAddress, m.RegisteredOfficeState, m.RegisteredOfficeCountry, "<br>Registered Office Contact No: ", m.RegisteredOfficeContactNumber, "<br>Registered Office MailId: ", m.RegisteredOfficeEmail, "<br>Local Office Details: ", m.LocalOfficeAddress, m.LocalOfficeState, m.LocalOfficeCountry, "<br>", m.LocalOfficeContactNumber, "<br>", m.LocalOfficeEmail, "<br>Billing Details: ", m.BillingAddress, m.BillingState, m.BillingCountry, "<br>", m.BillingContactNumber, "<br>", m.BillingEmail, "<br>Security Deposit Details: ", m.Rupees, m.RupeesInWords, "<br><br>Thanks" }), from))
                        {
                            Log.Info("Email Sent- ", "");
                        }
                    }
                    catch (Exception exception2)
                    {
                        Exception ex = exception2;
                        result = new { status = "0", ErrorMessage = exception2.Message };
                        Log.Error("Failed to sent Email", ex.ToString());
                    }
                    return Json(new { IsInfoSave = true }, JsonRequestBehavior.AllowGet);
                }

                catch (Exception exception)
                {
                    Log.Error(exception.Message, "Form Submit-");
                    Log.Error("Failed to Submit Form", exception.ToString());
                    return Json(new { IsInfoSave = false, errormessage = exception.Message }, JsonRequestBehavior.AllowGet);

                }


            }
            return this.Redirect("/AccountRegister-thankyou");
        }

        private static string GetFileBinaryData(HttpPostedFileBase AgreementWithPrincipal)
        {
            byte[] bytecontent = null;
            string filecontent = null;
            byte[] bytes = null;

            using (BinaryReader br = new BinaryReader(AgreementWithPrincipal.InputStream))
            {
                bytes = br.ReadBytes(AgreementWithPrincipal.ContentLength);
                bytecontent = bytes;
                filecontent = System.Convert.ToBase64String(bytes);
            }

            return filecontent;
        }

        public static Dictionary<string, string> GetErrors(ModelStateDictionary modelState)
        {
            var dicObject = new Dictionary<string, string>();
            var result = from ms in modelState
                         where ms.Value.Errors.Any()
                         let fieldKey = ms.Key
                         let errors = ms.Value.Errors
                         from error in errors
                         select new KeyValuePair<string, string>(fieldKey, error.ErrorMessage);

            foreach (var item in result)
            {
                dicObject.Add(item.Key, item.Value);
            }

            return dicObject;
        }
        public ActionResult PortsBerthShipAtACMTPL()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_berth_ct4SoapClient()).get_berth_ct4_data();
                Log.Info("API calling from PortsBerthShipAtACMTPL Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from PortsBerthShipAtACMTPL", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult PortsBerthShipAtAICTPL()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_ships_at_southbasinSoapClient()).get_ships_at_southbasin_data();
                Log.Info("API calling from PortsBerthShipAtAICTPL Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from PortsBerthShipAtAICTPL", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult PortsBerthShipAtAMCTT()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_ships_at_amctSoapClient()).get_ships_at_amct_data();
                Log.Info("API calling from PortsBerthShipAtAMCTT Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from PortsBerthShipAtAMCTT", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult PortsBerthShipAtAtMICT()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_ships_at_mictSoapClient()).get_ships_at_mict_data();
                Log.Info("API calling from PortsBerthShipAtAtMICT Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from PortsBerthShipAtAtMICT", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult PortsBerthShipAtAtSPMSTS()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_ships_at_spm_stsSoapClient()).get_ships_at_spm_sts_data();
                Log.Info("API calling from PortsBerthShipAtAtSPMSTS Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from PortsBerthShipAtAtSPMSTS", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult PortsBerthShipAtMPT()
        {
            DataSet ds = new DataSet();
            try
            {
                Log.Info("API calling from PortsBerthShipAtMPT", this);
                ds = (new PortsBerthGetShipAtMPTServiceReference.get_ships_at_mptSoapClient()).get_ships_at_mpt_data();
                Log.Info("API calling from PortsBerthShipAtMPT Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from PortsBerthShipAtMPT", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult PortsBerthShipAtWestBasin()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new PortsBerthGetShipAtMPTServiceReference.get_ships_at_mptSoapClient()).get_ships_at_mpt_data();
                Log.Info("API calling from PortsBerthShipAtWestBasin Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from PortsBerthShipAtWestBasin", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult PortsContainerTrackingForm()
        {
            return base.View();
        }

        [HttpPost]
        public ActionResult PortsContainerTrackingForm(string TrackingportType, string TrackingcontainerNo)
        {
            ActionResult actionResult;
            if (TrackingportType != "--Select Port--")
            {
                DataSet ds = new DataSet();
                if (TrackingportType == "Mundra CT2")
                {
                    ds = this.getResultAsync("3", TrackingcontainerNo);
                }
                if (TrackingportType == "Mundra CT4")
                {
                    // ds = (new MundraCT4ContainerTrackingServiceReference.get_container_tracking_ct4SoapClient()).get_container_tracking_ct4_data(TrackingcontainerNo);
                    ds = this.GetContainerMundraCT4Result("6", TrackingcontainerNo);
                }
                if (TrackingportType == "Mundra T2")
                {
                    ds = this.getResultAsync("2", TrackingcontainerNo);
                }
                if (TrackingportType == "Mundra CT3")
                {
                    ds = this.getResultAsync("1", TrackingcontainerNo);
                }
                if (TrackingportType == "Kattupalli")
                {
                    ds = this.getResultAsync("5", TrackingcontainerNo);
                }

                else if (TrackingportType == "Hazira")
                {
                    ds = this.getResultAsync("4", TrackingcontainerNo);
                }
                else if (TrackingportType == "Ennore")
                {
                    ds = this.getResultAsync("7", TrackingcontainerNo);
                    // ds = (new EnnoreNewContainerTrackingServiceReference.get_container_trackingSoapClient()).get_container_tracking_data(TrackingcontainerNo);
                }
                base.TempData["dataset"] = ds;
                actionResult = this.Redirect("/ContainerTrackingResult");
            }
            else
            {
                actionResult = base.View();
            }
            return actionResult;
        }

        //Ports Logistic container tracking  by Divyansh Start        
        [HttpGet]
        public ActionResult PortsLogisticContainerTrackingForm(string ContainerIDs)
        {
            var ServiceURLBucket = DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/ServiceURLBucket", "yo40xxh6c5.execute-api");
            var AWSRegion = DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/AWSRegion", "ap-south-1");
            var AWSAccessKey = DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/AWSAccessKey", "AKIA2PTTRY3BL7G2JQGG");
            var AWSSecretKey = DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/AWSSecretKey", "N4LkxVZaQkEwvfygnSWh4MqgyO5+/Tt/wO4/trT2");
            var AWSObjectKey = DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/AWSObjectKey", "beta/tyc-get?cntr_nos=");
            var AWSQueryParameter = DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/AWSQueryParameter", "cntr_nos=");
            var AWSServiceMethod = DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/AWSServiceMethod", "GET");
            var AWSServiceName = DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/AWSServiceName", "execute-api");
            string ErrorResponse = "{\"Error\":\"" + DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/ErrorResponse", "Something has been wrong. Please try again later") + "\"";
            String regex = "^[a-zA-Z0-9,]*$";
            if (!string.IsNullOrEmpty(ContainerIDs))
            {
                var ValidateState = Regex.Match(ContainerIDs, regex);
                if (ValidateState.Success)
                {
                    PortsLogisticsResult results = new PortsLogisticsResult();
                    try
                    {

                        AWSObjectKey = AWSObjectKey + HttpUtility.UrlEncode(ContainerIDs).ToUpper();
                        AWSQueryParameter = AWSQueryParameter + HttpUtility.UrlEncode(ContainerIDs).ToUpper();
                        var regionUrlPart = string.Empty;
                        if (!string.IsNullOrEmpty(AWSRegion))
                        {
                            if (!AWSRegion.Equals("us-east-1", StringComparison.OrdinalIgnoreCase))
                                regionUrlPart = string.Format("{0}", AWSRegion);
                        }

                        //var endpointUri = string.Format("https://{0}.{1}.amazonaws.com/{2}",
                        //                                   ServiceURLBucket,
                        //                                   regionUrlPart,
                        //                                   AWSObjectKey);

                        var LogisticURL = "track-your-container-37noc59p.an.gateway.dev/all-tyc";
                        var LogisticKey = "AIzaSyB9rZl9wCN0PlJxkpuUJjQlWneI4bt7mfA";
                        var endpointUri = string.Format("https://{0}?key={1}&cntr={2}",
                                                        LogisticURL,
                                                        LogisticKey,
                                                        ContainerIDs);

                        var uri = new Uri(endpointUri);

                        // for a simple GET, we have no body so supply the precomputed 'empty' hash
                        var headers = new Dictionary<string, string>();
                        //{
                        //    {AWS4SignerBase.X_Amz_Content_SHA256, AWS4SignerBase.EMPTY_BODY_SHA256},
                        //    {"content-type", "text/plain"}
                        //};

                        var signer = new AWS4SignerForAuthorizationHeader
                        {
                            EndpointUri = uri,
                            HttpMethod = AWSServiceMethod,
                            Service = AWSServiceName,
                            Region = AWSRegion
                        };

                        var authorization = signer.ComputeSignature(headers,
                                                                    AWSQueryParameter,   // no query parameters
                                                                    AWS4SignerBase.EMPTY_BODY_SHA256,
                                                                    AWSAccessKey,
                                                                    AWSSecretKey);

                        // place the computed signature into a formatted 'Authorization' header 
                        // and call S3
                        headers.Add("Authorization", authorization);

                        try
                        {
                            var request = ConstructWebRequest(uri, AWSServiceMethod, headers);
                            request.UserAgent = "Chrome/90.0.4430.93";
                            using (var response = (HttpWebResponse)request.GetResponse())
                            {
                                if (response.StatusCode == HttpStatusCode.OK)
                                {
                                    Log.Info("\n-- AWS HTTP call succeeded in Adani Ports Container Logistics", this);
                                    var responseBody = ReadResponseBody(response).Replace("None", "\"\"");
                                    if (!string.IsNullOrEmpty(responseBody))
                                    {

                                        Log.Info("\n-- AWS HTTP call Response body in Adani Ports Container Logistics: " + responseBody, this);
                                        dynamic OBJ = JsonConvert.DeserializeObject<Object>(responseBody);
                                        if (OBJ != null)
                                        {
                                            var TIDCount = OBJ.Count;
                                            if (TIDCount > 0)
                                            {
                                                for (int i = 0; i < TIDCount; i++)
                                                {
                                                    PortsLogisticContainerTracking tracking = new PortsLogisticContainerTracking();
                                                    tracking.cont_no = OBJ[i]["cont_no"];
                                                    tracking.cont_size = OBJ[i]["cont_size"];
                                                    tracking.train_no_vehicle_no = OBJ[i]["train_no_vehicle_no"];
                                                    tracking.origin = OBJ[i]["origin"];
                                                    tracking.destination = OBJ[i]["destination"];
                                                    tracking.current_status = OBJ[i]["current_status"];
                                                    tracking.movement_type = OBJ[i]["movement_type"];
                                                    tracking.last_updated_time = OBJ[i]["last_updated_time"];
                                                    tracking.current_location = OBJ[i]["current_location"];
                                                    tracking.landmark = OBJ[i]["landmark"];
                                                    tracking.address = OBJ[i]["address"];
                                                    tracking.city = OBJ[i]["city"];
                                                    //if (tracking.current_location != null && tracking.current_location.ToUpper() !="GDL" && tracking.current_status.ToUpper() != "DELIVERED" && tracking.current_location.ToUpper() != "NOT AVAILABLE")
                                                    //{
                                                    //    var loc = tracking.current_location.Split(',');
                                                    //    for (int j = 0; j < loc.Length; j++)
                                                    //    {
                                                    //        var values = loc[j].Split(':');
                                                    //        if(values[0].Trim().ToUpper() == "LONG")
                                                    //        {
                                                    //            tracking.Longitude = values[1].Trim();
                                                    //        }
                                                    //        else
                                                    //        {
                                                    //            tracking.Latitude = values[1].Trim();
                                                    //        }
                                                    //    }
                                                    //}
                                                    results.LogsticsResultList.Add(tracking);
                                                }
                                            }
                                        }
                                        ViewBag.ErrorMessage = "";
                                        return View("~/Views/Ports/PortsLogisticsTrackingResult.cshtml", results);
                                    }
                                }
                                else
                                {
                                    Log.Error("\n-- AWS HTTP call failed in Adani Ports Container Logistics, status code: " + response.StatusCode, this);
                                    ErrorResponse = "{\"Error\":\"" + DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/AWSErrorResponse", "Trcking Failed. Please try again after sometime.") + "\"";
                                    return View("~/Views/Ports/PortsLogisticsTrackingResult.cshtml", results);
                                }

                            }
                        }
                        catch (WebException ex)
                        {
                            using (var response = ex.Response as HttpWebResponse)
                            {
                                if (response != null)
                                {
                                    var errorMsg = ReadResponseBody(response);
                                    Log.Error("\n-- HTTP call failed with exception in Adani Ports Container Logistics:" + errorMsg + ", status code :" + response.StatusCode, this);
                                    ViewBag.ErrorMessage = DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/ErrorResponse", "Something has been wrong. Please try again later object" + errorMsg);
                                    return View("~/Views/Ports/PortsLogisticsTrackingResult.cshtml");
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("\n-- HTTP call failed with exception in Adani Ports Container Logistics:" + ex.Message, this);
                        ViewBag.ErrorMessage = DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/ErrorResponse", "Something has been wrong. Please try again later");
                        return View("~/Views/Ports/PortsLogisticsTrackingResult.cshtml", results);
                    }
                }
                else
                {
                    ViewBag.ErrorMessage = DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/InvalidTrackingID", "Please enter valid tracking id");
                    return View("~/Views/Ports/PortsLogisticsTrackingResult.cshtml");
                }
            }
            else
            {
                ViewBag.ErrorMessage = DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/InvalidTrackingID", "Please enter valid tracking id");
                return View("~/Views/Ports/PortsLogisticsTrackingResult.cshtml");
            }
            ViewBag.ErrorMessage = DictionaryPhraseRepository.Current.Get("/PortsLogisticContainerTracking/InvalidTrackingID", "Please enter valid tracking id");
            return View("~/Views/Ports/PortsLogisticsTrackingResult.cshtml");
        }
        public static HttpWebRequest ConstructWebRequest(Uri endpointUri,
                                                         string httpMethod,
                                                         IDictionary<string, string> headers)
        {
            var request = (HttpWebRequest)WebRequest.Create(endpointUri);
            request.Method = httpMethod;

            foreach (var header in headers.Keys)
            {
                // not all headers can be set via the dictionary
                if (header.Equals("host", StringComparison.OrdinalIgnoreCase))
                    request.Host = headers[header];
                else if (header.Equals("content-length", StringComparison.OrdinalIgnoreCase))
                    request.ContentLength = long.Parse(headers[header]);
                else if (header.Equals("content-type", StringComparison.OrdinalIgnoreCase))
                    request.ContentType = headers[header];
                else
                    request.Headers.Add(header, headers[header]);
            }

            return request;
        }
        public static string ReadResponseBody(HttpWebResponse response)
        {
            if (response == null)
                throw new ArgumentNullException("response", "Value cannot be null");

            // Then, open up a reader to the response and read the contents to a string
            // and return that to the caller.
            string responseBody = string.Empty;
            using (var responseStream = response.GetResponseStream())
            {
                if (responseStream != null)
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        responseBody = reader.ReadToEnd();
                    }
                }
            }
            return responseBody;
        }
        //Ports Logistic container tracking  by Divyansh End


        [HttpGet]
        public ActionResult PortsCorporateAnnouncementList(PortsAnnouncementFilter filter = null)
        {
            IQueryable<Item> result;
            string segment = filter.Segment;
            string category = filter.Category;
            string date = filter.Date;
            string toDate = filter.ToDate;
            string datasource = RenderingContext.Current.Rendering.DataSource;
            Item AllItem = Context.Database.GetItem(datasource);
            result = (string.IsNullOrEmpty(segment) ? (
                from x in AllItem.Children
                where x.Fields["IsVisible"].Value == "1"
                orderby x.Fields["Date"].Value descending
                select x).AsQueryable<Item>() : (
                from x in AllItem.Children
                where (!(x.Fields["IsVisible"].Value == "1") || !x.Fields["Segment"].Value.Contains(segment) || !x.Fields["Category"].Value.Contains(category) || !(DateTime.Parse(x.Fields["Date"].Value) >= DateTime.Parse(date)) ? false : DateTime.Parse(x.Fields["Date"].Value) <= DateTime.Parse(toDate))
                orderby x.Fields["Date"].Value descending
                select x).AsQueryable<Item>());
            return base.View("PortsCorporateAnnouncementList", result);
        }

        public ActionResult PortsCorporateAnnouncementList()
        {
            string datasource = RenderingContext.Current.Rendering.DataSource;
            IQueryable<Item> result = (
                from x in Context.Database.GetItem(datasource).Children
                where x.Fields["IsVisible"].Value == "1"
                orderby x.Fields["Date"].Value descending
                select x).AsQueryable<Item>();
            return base.View("PortsCorporateAnnouncementList", result);
        }

        public ActionResult SailedACMTPLVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_sailed_ct4SoapClient()).get_sailed_ct4_data();
                Log.Info("API calling from SailedACMTPLVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from SailedACMTPLVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult SailedAICTPLVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_sailed_southbasinSoapClient()).get_sailed_southbasin_data();
                Log.Info("API calling from SailedAICTPLVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from SailedAICTPLVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult SailedAMCTVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_sailed_ct2SoapClient()).get_sailed_ct2_data();
                Log.Info("API calling from SailedAMCTVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from SailedAMCTVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult SailedBreakBulkVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_sailed_drypcSoapClient()).get_sailed_drypc_data();
                Log.Info("API calling from SailedBreakBulkVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from SailedBreakBulkVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult SailedBulkVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_sailed_drybcSoapClient()).get_sailed_drybc_data();
                Log.Info("API calling from SailedBulkVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from SailedBulkVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult SailedBunkerVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_sailed_bunkerSoapClient()).get_sailed_bunker_data();
                Log.Info("API calling from SailedBunkerVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from SailedBunkerVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult SailedHMELVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_sailed_spm_hmelSoapClient()).get_sailed_spm_hmel_data();
                Log.Info("API calling from SailedHMELVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from SailedHMELVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult SailedIOCLVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_sailed_spm_ioclSoapClient()).get_sailed_spm_iocl_data();
                Log.Info("API calling from SailedIOCLVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from SailedIOCLVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult SailedLiquidVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_sailed_liquidSoapClient()).get_sailed_liquid_data();
                Log.Info("API calling from SailedLiquidVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from SailedLiquidVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult SailedMICTVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_sailed_mictSoapClient()).get_sailed_mict_data();
                Log.Info("API calling from SailedMICTVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from SailedMICTVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult SailedSTSVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_sailed_stsSoapClient()).get_sailed_sts_data();
                Log.Info("API calling from SailedSTSVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from SailedSTSVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult SailedWestBasinVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_sailed_westbasinSoapClient()).get_sailed_westbasin_data();
                Log.Info("API calling from SailedWestBasinVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from SailedWestBasinVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public bool sendEmail(string to, string subject, string body, string from)
        {
            bool flag;
            bool status = false;
            try
            {
                MailMessage mail = new MailMessage()
                {
                    From = new MailAddress(from)
                };
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                ContentType ct = new ContentType("application/pdf");
                mail.From = new MailAddress(from);
                MainUtil.SendMail(mail);
                status = true;
                flag = status;
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Console.WriteLine(ex.Message, "sendEmail - ");
                Log.Error(ex.Message, "sendEmail - ");
                Log.Error(ex.InnerException.ToString(), "sendEmail - ");
                flag = status;
            }
            return flag;
        }

        public ActionResult TunaAnchorageVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_anchorageSoapClient()).get_vessel_at_anchorage_data();
                Log.Info("API calling from TunaAnchorageVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from TunaAnchorageVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult TunaBerthVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_berthSoapClient()).get_vessel_at_berth_data();
                Log.Info("API calling from TunaBerthVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from TunaBerthVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult TunaExpectedVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_expectedSoapClient()).get_vessel_at_expected_data();
                Log.Info("API calling from TunaExpectedVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from TunaExpectedVessels", e.Message), this);
            }
            return base.View(ds);
        }

        public ActionResult TunaSailedVessels()
        {
            DataSet ds = new DataSet();
            try
            {
                ds = (new get_vessel_at_sailedSoapClient()).get_vessel_at_sailed_data();
                Log.Info("API calling from TunaSailedVessels Successful", this);
            }
            catch (Exception exception)
            {
                Exception e = exception;
                Log.Error(string.Concat("Error in API calling from TunaSailedVessels", e.Message), this);
            }
            return base.View(ds);
        }

        [HttpPost]
        public ActionResult VerifyOTP(PortsContactModel model)
        {
            var result = new { status = "0" };
            if (string.Equals(this.portsrepo.GetOTP(model.Mobile), model.OTP))
            {
                result = new { status = "1" };
            }
            return base.Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult PaymentAdviseForm()
        {
            return base.View("/Views/Ports/Sublayouts/PaymentAdviseForm.cshtml");
        }


        [HttpPost]
        public ActionResult PaymentAdviseForm(PaymentAdviseForm m, string SubmitApplications, FormCollection fc)
        {
            Log.Info("Insert DefenceVendorEnrollmentForm", "Start");
            bool validationStatus = true;
            try
            {
                validationStatus = this.IsReCaptchValid(m.reResponse);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Log.Info(string.Concat("Failed to validate auto script : ", ex.ToString()), "Failed");
            }

            if (m.reResponse == null || m.reResponse == "")
            {
                ModelState.AddModelError(nameof(m.reResponse), DictionaryPhraseRepository.Current.Get("/Ports/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                return View("/Views/Ports/Sublayouts/PaymentAdviseForm.cshtml", m);
            }

            //if (!validationStatus)
            //{
            //  ModelState.AddModelError(nameof(m.reResponse), DictionaryPhraseRepository.Current.Get("/Ports/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
            //    return View("/Views/Ports/Sublayouts/PaymentAdviseForm.cshtml", m);
            //}
            //else
            //{
            Log.Info("Insert PortsPaymentForm Captcha Validated", "Start");
            Helper.PortsHelper helper = new Helper.PortsHelper();
            byte[] bytes;
            try
            {
                if (!string.IsNullOrEmpty(SubmitApplications))
                {




                    m.Registrationno = helper.GetUniqueRegNo();
                    for (int i = 1; i <= 10; i++)
                    {
                        m.Id = Guid.NewGuid();
                        var CustomerCode = m.CustomerCode;
                        if (fc["InvoiceNumber" + i] == null || fc["InvoiceNumber" + i] == "")
                        {
                            // m.InvoiceDate1 = DateTime.Now;
                            break;
                        }
                        DateTime date = DateTime.Now;
                        if (fc["InvoiceNumber" + i] != "1/1/0001 12:00:00 AM")
                        {
                            date = DateTime.Parse(fc["InvoiceDate" + i], (IFormatProvider)CultureInfo.GetCultureInfo("en-gb"));
                        }

                        var InvoiceAmount = fc["InvoiceAmount" + i];
                        var TDSAmount = fc["TDSAmount" + i];
                        var netPayment = Decimal.Parse(InvoiceAmount) - Decimal.Parse(TDSAmount);
                        PortsPaymentAdviseFormDataContext ppafdc = new PortsPaymentAdviseFormDataContext();
                        PortsPaymentAdviseForm ppaf = new PortsPaymentAdviseForm()
                        {

                            Id = m.Id,
                            RegistrationNumber = m.Registrationno,
                            BankDetail = m.BankDetail,
                            CompanyCode = m.CompanyCode,
                            CustomerCode = m.CustomerCode,
                            CustomerName = m.CustomerName,
                            NetAmountPaid = m.NetAmountPaid,
                            InvoiceDate = date,
                            // InvoiceDate = DateTime.Parse(fc["InvoiceDate" + i].ToString(), CultureInfo.GetCultureInfo("en-gb")),
                            // InvoiceDate= fc["InvoiceDate" + i],
                            SubmitOnDate = DateTime.Now,
                            GSTIN = m.GSTIN,
                            InvoiceAmount = fc["InvoiceAmount" + i],
                            InvoiceNumber = fc["InvoiceNumber" + i],
                            NetPayment = netPayment.ToString(),
                            Remarks = fc["Remarks" + i],
                            TDSAmount = fc["TDSAmount" + i],
                            RemittanceDate = m.RemittanceDate,
                            UTR = m.UTR
                        };
                        if (m.customFile != null)
                        {
                            HttpPostedFileBase customFile = m.customFile;
                            string fileName = customFile.FileName;
                            customFile.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", m.CustomerCode, "-", fileName })));
                            ppaf.BrowseFileUrl = string.Concat(new object[] { "https://www.adaniports.com/PortsAccountForms/Upload/", m.CustomerCode, "-", fileName });
                        }
                        ppafdc.PortsPaymentAdviseForms.InsertOnSubmit(ppaf);
                        ppafdc.SubmitChanges();
                        Log.Info("Insert Account Registration details of AdaniPorts ", "End");


                    }
                    if (fc["InvoiceNumber1"] == null || fc["InvoiceNumber1"] == "")
                    {
                        if (m.customFile != null)
                        {
                            PortsPaymentAdviseFormDataContext ppafdc = new PortsPaymentAdviseFormDataContext();
                            PortsPaymentAdviseForm ppaf = new PortsPaymentAdviseForm()
                            {

                                Id = m.Id,
                                RegistrationNumber = m.Registrationno,
                                BankDetail = m.BankDetail,
                                CompanyCode = m.CompanyCode,
                                CustomerCode = m.CustomerCode,
                                CustomerName = m.CustomerName,
                                NetAmountPaid = m.NetAmountPaid,
                                //  InvoiceDate = "1/1/0001 12:00:00 AM",
                                // InvoiceDate = DateTime.Parse(fc["InvoiceDate" + i].ToString(), CultureInfo.GetCultureInfo("en-gb")),
                                // InvoiceDate= fc["InvoiceDate" + i],
                                SubmitOnDate = DateTime.Now,
                                GSTIN = m.GSTIN,
                                InvoiceAmount = "",
                                InvoiceNumber = "",
                                NetPayment = "",
                                Remarks = "",
                                TDSAmount = "",
                                RemittanceDate = m.RemittanceDate,
                                UTR = m.UTR
                            };
                            HttpPostedFileBase customFile = m.customFile;
                            string fileName = customFile.FileName;
                            customFile.SaveAs(base.Server.MapPath(string.Concat(new object[] { "/PortsAccountForms/Upload/", m.CustomerCode, "-", fileName })));
                            ppaf.BrowseFileUrl = string.Concat(new object[] { "https://www.adaniports.com/PortsAccountForms/Upload/", m.CustomerCode, "-", fileName });
                            ppafdc.PortsPaymentAdviseForms.InsertOnSubmit(ppaf);
                            ppafdc.SubmitChanges();
                            Log.Info("Insert Account Registration details of AdaniPorts ", "End");
                        }
                    }
                    //rdb1.PortsPaymentAdviseForms.InsertOnSubmit(r);
                    // rdb1.SubmitChanges();
                    // Log.Info("Insert Account Registration details of AdaniPorts ", "End");
                }
                return Redirect("/thankyou");
                // return View("/Views/Ports/Sublayouts/PaymentAdviseForm.cshtml", m);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at Saving Form Ports Vendor: " + ex.Message, this);
                ViewBag.Message = "Something has been wrong, Please try again later";
                return View("/Views/Ports/Sublayouts/PaymentAdviseForm.cshtml", m);
            }
            // }
        }

        public bool IsReCaptchValid(string reResponse)
        {
            var result = false;
            // var captchaResponse = Request.Form["g-recaptcha-response"];
            var captchaResponse = reResponse;
            string secretKey = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "");
            //var secretKey = ConfigurationManager.AppSettings["SecretKey"];
            // var secretKey = "6LdkC64UAAAAAJiii15Up9xETgsLuPQnQ1BKZft8";
            var apiUrl = "https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}";
            var requestUri = string.Format(apiUrl, secretKey, captchaResponse);
            var request = (HttpWebRequest)WebRequest.Create(requestUri);

            using (WebResponse response = request.GetResponse())
            {
                using (StreamReader stream = new StreamReader(response.GetResponseStream()))
                {
                    JObject jResponse = JObject.Parse(stream.ReadToEnd());
                    var isSuccess = jResponse.Value<bool>("success");
                    result = (isSuccess) ? true : false;
                }
            }
            return result;
        }

        [HttpPost]
        public ActionResult CheckCustomerDetails(String CustomerCode)
        {
            var result = new { status = "0", GSTIN = "", CustomerName = "" };
            PaymentAdviseForm emp = new PaymentAdviseForm();
            try
            {
                // Log.Error("CheckAffiliateCode ", "Start");

                if (!string.IsNullOrEmpty(CustomerCode))
                {
                    try
                    {
                        using (PortsPaymentAdviseFormDataContext objcontext = new
                            PortsPaymentAdviseFormDataContext())

                        {
                            if (objcontext.PortsCustomerDetailChecks.Where(x => x.CustomerCode.ToString() == CustomerCode.ToString()).Any())
                            {
                                emp = objcontext.PortsCustomerDetailChecks.Where(x => x.CustomerCode.ToString() == CustomerCode.ToString()).Select(x => new PaymentAdviseForm()
                                {
                                    CustomerName = x.CustomerName.ToString(),
                                    GSTIN = x.GSTIN.ToString(),
                                }).FirstOrDefault();
                                string GSTIN = emp.GSTIN;
                                string CustomerName = emp.CustomerName;

                                //var GSTIN = (objcontext.PortsCustomerDetailChecks.Where(x => x.CustomerCode.ToString() == CustomerCode.ToString()).Select(x => x.GSTIN)).ToString();
                                //var CustomerName = objcontext.PortsCustomerDetailChecks.Where(x => x.CustomerCode.ToString() == CustomerCode.ToString()).Select(x => x.CustomerName);

                                result = new { status = "1", GSTIN = GSTIN, CustomerName = CustomerName };
                                return Json(result, JsonRequestBehavior.AllowGet);
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        Log.Error("", ex.Message);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("", ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult DownloadSample(String CustomerCode)
        {
            var result = new { status = "0", GSTIN = "", CustomerName = "" };
            try
            {
                String file = base.Server.MapPath("~/sitecore/admin/Ports/SampleFile/Form-Records.csv");
                FileInfo fileInfo = new FileInfo(file);
                if (!fileInfo.Exists)
                {
                    base.Response.Write("This file does not exist.");
                }
                else
                {
                    base.Response.Clear();
                    base.Response.AddHeader("Content-Disposition", "attachment; filename=Form-Records.csv");
                    base.Response.AddHeader("Content-Length", fileInfo.Length.ToString());
                    base.Response.ContentType = "application/octet-stream";
                    base.Response.WriteFile(fileInfo.FullName);
                    base.Response.End();
                    return File(new System.Text.UTF8Encoding().GetBytes(file), "text/csv", "SampleFile");

                }

            }
            catch (Exception ex)
            {
                Log.Error("", ex.Message);
            }
            return Json(result, JsonRequestBehavior.AllowGet);

        }
        [HttpGet]
        public ActionResult PortsITDeclaration()
        {
            return base.View("/Views/Ports/Sublayouts/PortsITDeclaration.cshtml", new PortsIncomeTaxReturnModal());
        }
        [HttpPost]
        public ActionResult PortsITDeclaration(PortsIncomeTaxReturnModal m, string SubmitApplication)
        {

            ActionResult actionResult;
            var result = new { status = "1" };

            Log.Info("Insert Form", "Start");
            bool flag = true;
            byte[] bytes;
            try
            {
                flag = this.IsReCaptchValid(m.reResponse);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Info(string.Concat("Failed to validate auto script : ", exception.ToString()), "Failed");
            }
            if (flag == true)

            {

                Log.Info("Insert Form Captcha Validated", "Start");
                PortsIncomeTaxReturnHelper helper = new PortsIncomeTaxReturnHelper();
                try
                {
                    if (!string.IsNullOrEmpty(SubmitApplication))
                    {
                        if (ModelState.IsValid)

                        {
                            PortsIncomeTaxDataContext rdb = new PortsIncomeTaxDataContext();
                            m.Id = Guid.NewGuid();
                            PortsIncomeTaxReturn CapitalCareerForm = new PortsIncomeTaxReturn()
                            {
                                Id = m.Id,
                                VendorName = m.VendorName,
                                VendorId = m.VendorNumber,
                                AadharCardNumber = m.AadharCardNumber,
                                PanCardNumber = m.PanNo,
                                LinkedPanWithAdhar = m.IsPanLinkedWithAadhar,
                                FilledItReturns = m.IsFilledItReturnFilling,
                                FormName = m.FormType,
                                FormSubmitOn = new DateTime?(DateTime.Now)

                            };
                            if (m.PanWithAadharAttachment != null && m.PanWithAadharAttachment.ContentLength > 0)
                            {
                                HttpPostedFileBase UploadedResumeLinks = m.PanWithAadharAttachment;
                                string UploadedResumeLink = UploadedResumeLinks.FileName;
                                UploadedResumeLinks.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/Ports/AadharPanLink/", m.VendorName, "-", m.VendorNumber, "-", UploadedResumeLink })));
                                CapitalCareerForm.AadharPanAttachment = string.Concat(new string[] { "/Ports/AadharPanLink/", m.VendorName, "-", m.VendorNumber, "-", UploadedResumeLink });


                            }
                            if (m.ITReturnFillingAttachment != null && m.ITReturnFillingAttachment.ContentLength > 0)
                            {
                                HttpPostedFileBase UploadedResumeLinks = m.ITReturnFillingAttachment;
                                string UploadedResumeLink = UploadedResumeLinks.FileName;
                                UploadedResumeLinks.SaveAs(base.Server.MapPath(string.Concat(new string[] { "/Ports/ItReturnFilling/", m.VendorName, "-", m.VendorNumber, "-", UploadedResumeLink })));
                                CapitalCareerForm.ProofOfItReturn = string.Concat(new string[] { "/Ports/ItReturnFilling/", m.VendorName, "-", m.VendorNumber, "-", UploadedResumeLink });


                            }
                            rdb.PortsIncomeTaxReturns.InsertOnSubmit(CapitalCareerForm);
                            rdb.SubmitChanges();
                            Log.Info("Form data saved into db successfully: ", this);
                            try
                            {


                                HttpPostedFileBase PanWithAadharAttachments = m.PanWithAadharAttachment;
                                HttpPostedFileBase ITReturnFillingAttachment = m.ITReturnFillingAttachment;
                                string fileName = PanWithAadharAttachments.FileName;
                                string fileName2 = ITReturnFillingAttachment.FileName;
                                string str5 = string.Concat(new string[] { "<a href=\"https://adanistaging-cd.azurewebsites.net/Ports/AadharPanLink/", m.VendorName, "-", m.VendorNumber, "-", fileName, "\"", " Download>Click to Download </a>" });
                                string str6 = string.Concat(new string[] { "<a href=\"https://adanistaging-cd.azurewebsites.net/Ports/ItReturnFilling/", m.VendorName, "-", m.VendorNumber, "-", fileName, "\"", " Download>Click to Download </a>" });
                                string emailText = DictionaryPhraseRepository.Current.Get("/PortsForm/EmailText", "");
                                string message = "";
                                string getEmailTo = DictionaryPhraseRepository.Current.Get("/PortsForm/EmailTo", "");
                                message = string.Concat("Hello<br><br>", emailText, "<br><br>");
                                message = string.Concat(new string[] { message, "<br>Name: ", m.VendorName, "<br>Email-Id: ", m.VendorNumber, "<br>  <br> AadharPanLink: ", str5, "<br>  <br> AadharPanLink: ", str6, "<br><br> Thanks" });
                                string from = DictionaryPhraseRepository.Current.Get("/PortsForm/EmailFrom", "");
                                if (this.sendEmail(getEmailTo, DictionaryPhraseRepository.Current.Get("/PortsForm/EmailSubject", ""), message, from))
                                {
                                    Log.Error("Email Sent- ", "");
                                }
                            }


                            catch (Exception exception3)
                            {
                                Exception ex = exception3;
                                result = new { status = "1" };
                                Log.Error("Failed to sent Email", ex.ToString());
                            }
                            return Redirect("/thankyou");

                        }
                        else
                        {
                            actionResult = base.View("/Views/Ports/Sublayouts/PortsITDeclaration.cshtml", m);
                            return actionResult;
                        }
                    }
                }

                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Form Registration Form: ", exception2.Message), this);
                    ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
                    actionResult = base.View("/Views/Ports/Sublayouts/PortsITDeclaration.cshtml", m);
                    return actionResult;
                }

            }
            else
            {

                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/Ports/Controller Messages/Captcha Error", "Captcha is Invalid, Please try again"));
                actionResult = base.View("/Views/Ports/Sublayouts/PortsITDeclaration.cshtml", m);
                return actionResult;
            }
             ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
            actionResult = base.View("/Views/Ports/Sublayouts/PortsITDeclaration.cshtml", m);
            return actionResult;
        }



        #region Ports GMS
        //........PortsGms Code Start From here........//
        [HttpGet]
        public ActionResult PortsGmsRegistration()
        {
            if (Request.QueryString["gid"] != null)
            {
                Session["InCompleteGrievance"] = Request.QueryString["gid"].ToString();
            }
            else
            {
                Session["InCompleteGrievance"] = null;
            }
            return View();
        }


        [HttpPost]
        public ActionResult PortsGmsRegistration(PortsGmsRegistration pm)
        {
            string msg = "";
            string url = "";
            bool Validated = false;

            try
            {
                Recaptchav3Provider recaptchav2 = new Recaptchav3Provider();
                Validated = recaptchav2.IsReCaptchValid(pm.RegistrationGcaptcha);

            }
            catch (Exception ex)
            {
                msg = "Somthing went wrong please try again.";
                Session["ErrorMsg"] = msg;
                Console.WriteLine(ex);
            }

            if (Validated == true)
            {
                if (!string.IsNullOrEmpty(pm.Address) && (!Regex.IsMatch(pm.Address, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                {
                    msg = "Please Enter Valid Mobile Number.";
                    Session["SuccessMsg"] = msg;
                    url = "/GrievanceRegistration";
                }
                else if (!string.IsNullOrEmpty(pm.Fax_No) && (!Regex.IsMatch(pm.Fax_No, (@"^[ A-Za-z0-9]*$"))))
                {
                    msg = "Please Enter Valid Fax Number.";
                    Session["SuccessMsg"] = msg;
                    url = "/GrievanceRegistration";

                }
                else if (!ModelState.IsValid)
                {
                    msg = "Somthing went wrong please try again after sometime.";
                    Session["ErrorMsg"] = msg;
                    return View();
                }
                else
                {
                    try
                    {
                        PortsGMSDataContext rdb = new PortsGMSDataContext();
                        PortsGms_Registration pgr = new PortsGms_Registration();
                        PortsGms_Registration_Attachment pgra = new PortsGms_Registration_Attachment();
                        if (rdb.PortsGms_Registrations.Any(x => x.Mobile == GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Mobile, EncryptionIV) || x.Email == GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Email, EncryptionIV)))
                        {

                            msg = "User Alrady Registrated";
                            Session["ErrorMsg"] = msg;
                            url = "/GrievanceRegistration";
                        }
                        else
                        {
                            var Mobile = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Mobile && x.OTP == pm.Mobile_Verified && x.OTPType == "registration").FirstOrDefault();
                            var Email = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Email && x.OTP == pm.Email_Verified && x.OTPType == "registration").FirstOrDefault();
                            if (Mobile != null && Email != null)
                            {
                                if (Mobile.Status == true && Email.Status == true)
                                {
                                    pm.Id = Guid.NewGuid();
                                    pgr.Id = pm.Id;
                                    pgr.Name = pm.first_name + ' ' + pm.middle_name + ' ' + pm.last_name;
                                    pgr.Email = GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Email, EncryptionIV);
                                    pgr.Mobile = GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Mobile, EncryptionIV);
                                    pgr.Phone = pm.Phone;
                                    pgr.DOB = pm.DOB;
                                    pgr.Fax_No = pm.Fax_No;
                                    pgr.Qustion = pm.Qustion;
                                    pgr.Answer = pm.Answer;
                                    pgr.Company_Name = pm.Company_Name;
                                    pgr.Address = pm.Address;
                                    pgr.State = pm.State;
                                    pgr.Country = pm.Country;
                                    pgr.Pin_Code = pm.Pin_Code;
                                    pgr.User_Type = PortsGMSTemplates.UserType.Stakeholder;
                                    pgr.Status = true;
                                    pgr.Department = pm.Department != null ? pm.Department : null;
                                    pgr.Location = pm.Location != null ? pm.Location : null;

                                    pgr.Created_Date = System.DateTime.Now;
                                    pgr.Modified_Date = System.DateTime.Now;

                                    #region Insert to DB
                                    rdb.PortsGms_Registrations.InsertOnSubmit(pgr);
                                    rdb.SubmitChanges();


                                    var AgreementWithPrincipal = pm.Attachment[0];
                                    string[] contenttypeExtenstion = new string[] { "application/pdf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/msword", "image/jpeg" };

                                    if (AgreementWithPrincipal != null)
                                    {
                                        var CertificationOfIncorporationmime = AgreementWithPrincipal.ContentType;
                                        string filecontent = GetFileBinaryData(AgreementWithPrincipal);

                                        if (!contenttypeExtenstion.Contains(CertificationOfIncorporationmime.ToLower()))
                                        {
                                            msg = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg File in Attachment";
                                            Session["SuccessMsg"] = msg;
                                            url = "/GrievanceRegistration";
                                        }
                                        else if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                                        {
                                            msg = "Please upload only .pdf, .doc, .docx , .jpeg , .jpg File in Attachment";
                                            Session["SuccessMsg"] = msg;
                                            url = "/GrievanceRegistration";
                                        }
                                        else
                                        {
                                            if (pm.InCompleteGrievance != null)
                                            {
                                                url = "/Grievance?gid=" + pm.InCompleteGrievance;
                                            }
                                            else
                                            {
                                                url = "/Grievance";
                                            }
                                            saveUserAttachmentFile(pm.Attachment, pm.Id);

                                            SendEmailGMSStackHolder(pm.Email, pgr.Name, new Guid(), PortsGMSTemplates.GMSFlags.Registration);
                                            msg = "User Registration Successfully Done, Please Login.";
                                            Session["SuccessMsg"] = msg;
                                        }


                                    }
                                }
                                else
                                {
                                    if (Mobile.Status == false)
                                    {
                                        msg = "Please Verify Your Mobile Number";
                                        Session["ErrorMsg"] = msg;
                                        url = "/GrievanceRegistration";
                                    }
                                    if (Email.Status == false)
                                    {
                                        msg = "Please Verify Your Email";
                                        Session["ErrorMsg"] = msg;
                                        url = "/GrievanceRegistration";
                                    }
                                }
                            }
                        }
                        //  Session["validate"] = Request.Cookies["SIDCC"].Value;
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        msg = "Somthing went wrong please try again after sometime.";
                        Session["ErrorMsg"] = msg;
                        url = "/GrievanceRegistration";
                        Console.WriteLine(ex);
                    }
                }
            }
            else
            {
                msg = "Captcha Validation Failed!!!";
                Session["ErrorMsg"] = msg;
                url = "/GrievanceRegistration";
                Console.WriteLine("Captcha Validation Failed!!!!");
            }


            return Redirect(url);

        }


        [HttpGet]
        public ActionResult PortsGmsCorporateRegistration()
        {

            PortsGMSDataContext rdb = new PortsGMSDataContext();
            PortsGms_Registration pgr = new PortsGms_Registration();
            PortsGms_Registration userData = new PortsGms_Registration();

            if (Session["PortsGmsAdminUser"] != null)
            {
                Guid RegistrationId = new Guid(Session["PortsGmsAdminUser"].ToString());
                userData = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
            }
            if (Session["PortsGmsLevel1User"] != null)
            {
                Guid RegistrationId = new Guid(Session["PortsGmsLevel1User"].ToString());
                userData = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
            }
            if (Session["PortsGmsLevel2User"] != null)
            {
                Guid RegistrationId = new Guid(Session["PortsGmsLevel2User"].ToString());
                userData = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
            }
            if (Session["PortsGmsLevel2User"] != null)
            {
                Guid RegistrationId = new Guid(Session["PortsGmsLevel2User"].ToString());
                userData = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
            }

            if (userData != null)
            {
                PortsGmsCorporateRegistration CorporateUser = new PortsGmsCorporateRegistration();

                CorporateUser.Name = userData.Name;
                CorporateUser.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, userData.Email, EncryptionIV);
                CorporateUser.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, userData.Mobile, EncryptionIV);
                CorporateUser.Phone = userData.Phone;
                CorporateUser.Fax_No = userData.Fax_No;
                CorporateUser.DOB = userData.DOB;
                CorporateUser.Company_Name = userData.Company_Name;
                CorporateUser.Qustion = userData.Qustion;
                CorporateUser.Answer = userData.Answer;
                CorporateUser.Address = userData.Address;
                CorporateUser.State = userData.State;
                CorporateUser.Country = userData.Country;
                CorporateUser.Pin_Code = userData.Pin_Code;
                CorporateUser.Id = userData.Id;
                return this.View(CorporateUser);
            }

            return this.View();
        }

        [HttpPost]
        public ActionResult PortsGmsCorporateRegistration(PortsGmsCorporateRegistration pm)
        {
            string msg = "";
            string url = "";
            var LoginPage = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLogin);
            var CorporateRegistrationPage = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceCorprateUserRegistration);
            if (ModelState.IsValid)
            {
                try
                {
                    PortsGMSDataContext rdb = new PortsGMSDataContext();
                    PortsGms_Registration pgr = new PortsGms_Registration();
                    PortsGms_Registration_Attachment pgra = new PortsGms_Registration_Attachment();




                    if (Session["PortsGmsAdminUser"] != null || Session["PortsGmsLevel1User"] != null || Session["PortsGmsLevel2User"] != null || Session["PortsGmsLevel3User"] != null)
                    {

                        //var Mobile = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Mobile && x.OTP == pm.Mobile_Verified && x.OTPType == "registration").FirstOrDefault();
                        //var Email = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Email && x.OTP == pm.Email_Verified && x.OTPType == "registration").FirstOrDefault();
                        //if (Mobile.Status == true && Email.Status == true)
                        //{
                        pgr = rdb.PortsGms_Registrations.Where(x => x.Id == pm.Id).FirstOrDefault();
                        //pm.Id = Guid.NewGuid();
                        //pgr.Id = pm.Id;
                        pgr.Name = pm.first_name + ' ' + pm.middle_name + ' ' + pm.last_name;
                        //pgr.Email = pm.Email;
                        //pgr.Mobile = pm.Mobile;
                        pgr.Phone = pm.Phone;
                        pgr.DOB = pm.DOB;
                        pgr.Fax_No = pm.Fax_No;
                        //pgr.Department = pm.Department;
                        pgr.Qustion = pm.Qustion;
                        pgr.Answer = pm.Answer;
                        pgr.Company_Name = pm.Company_Name;
                        pgr.Address = pm.Address;
                        pgr.State = pm.State;
                        pgr.Country = pm.Country;
                        pgr.Pin_Code = pm.Pin_Code;
                        //pgr.User_Type = PortsGMSTemplates.UserType.Stakeholder;
                        pgr.Corporate_User = true;
                        pgr.Status = true;
                        // pgr.Department = pm.Department != null ? pm.Department : null;
                        //pgr.Location = pm.Location != null ? pm.Location : null;

                        //pgr.Created_Date = System.DateTime.Now; ;
                        pgr.Modified_Date = System.DateTime.Now; ;

                        #region Insert to DB
                        //rdb.PortsGms_Registrations.InsertOnSubmit(pgr);
                        rdb.SubmitChanges();
                        #endregion
                        saveUserAttachmentFile(pm.Attachment, pm.Id);

                        SendEmailGMSStackHolder(pm.Email, pgr.Name, new Guid(), PortsGMSTemplates.GMSFlags.Registration);

                        msg = "User Registration Successfully Done, Please Login.";

                        Session["SuccessMsg"] = msg;
                        url = LoginPage.Url();
                        // }
                        // else
                        /*{
                            if (Mobile.Status == false)
                            {
                                msg = "Please Verify Your Mobile Number";
                                Session["ErrorMsg"] = msg;
                                url = "/GrievanceRegistration";

                            }
                            if (Email.Status == false)
                            {
                                msg = "Please Verify Your Email";
                                Session["ErrorMsg"] = msg;
                                url = "/GrievanceRegistration";
                            }*/
                        // }
                    }
                    else
                    {
                        if (rdb.PortsGms_Registrations.Any(x => x.Mobile == GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Mobile, EncryptionIV) || x.Email == GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Email, EncryptionIV)))
                        {

                            msg = "User Alrady Registrated";
                            Session["ErrorMsg"] = msg;
                            url = CorporateRegistrationPage.Url();
                        }
                        else
                        {
                            var Mobile = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Mobile && x.OTP == pm.Mobile_Verified && x.OTPType == "registration").FirstOrDefault();
                            var Email = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Email && x.OTP == pm.Email_Verified && x.OTPType == "registration").FirstOrDefault();
                            if (Mobile != null && Email != null)
                            {
                                if (Mobile.Status == true && Email.Status == true)
                                {
                                    pm.Id = Guid.NewGuid();
                                    pgr.Id = pm.Id;
                                    pgr.Name = pm.first_name + ' ' + pm.middle_name + ' ' + pm.last_name;
                                    pgr.Email = GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Email, EncryptionIV);
                                    pgr.Mobile = GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Mobile, EncryptionIV);
                                    pgr.Phone = pm.Phone;
                                    pgr.DOB = pm.DOB;
                                    pgr.Fax_No = pm.Fax_No;
                                    pgr.Department = pm.Department;
                                    pgr.Qustion = pm.Qustion;
                                    pgr.Answer = pm.Answer;
                                    pgr.Company_Name = pm.Company_Name;
                                    pgr.Address = pm.Address;
                                    pgr.State = pm.State;
                                    pgr.Country = pm.Country;
                                    pgr.Pin_Code = pm.Pin_Code;
                                    pgr.User_Type = PortsGMSTemplates.UserType.Stakeholder;
                                    pgr.Status = true;
                                    pgr.Department = pm.Department != null ? pm.Department : null;
                                    pgr.Location = pm.Location != null ? pm.Location : null;
                                    pgr.Corporate_User = true;
                                    pgr.Created_Date = System.DateTime.Now; ;
                                    pgr.Modified_Date = System.DateTime.Now; ;

                                    #region Insert to DB
                                    rdb.PortsGms_Registrations.InsertOnSubmit(pgr);
                                    rdb.SubmitChanges();
                                    #endregion
                                    saveUserAttachmentFile(pm.Attachment, pm.Id);

                                    SendEmailGMSStackHolder(pm.Email, pgr.Name, new Guid(), PortsGMSTemplates.GMSFlags.Registration);

                                    msg = "User Registration Successfully Done, Please Login.";

                                    Session["SuccessMsg"] = msg;
                                    url = LoginPage.Url();
                                }
                                else
                                {
                                    if (Mobile.Status == false)
                                    {
                                        msg = "Please Verify Your Mobile Number";
                                        Session["ErrorMsg"] = msg;
                                        url = CorporateRegistrationPage.Url();

                                    }
                                    if (Email.Status == false)
                                    {
                                        msg = "Please Verify Your Email";
                                        Session["ErrorMsg"] = msg;
                                        url = CorporateRegistrationPage.Url();
                                    }
                                }
                            }
                        }
                        //  Session["validate"] = Request.Cookies["SIDCC"].Value;
                    }

                }
                catch (Exception ex)
                {
                    msg = "Somthing went wrong please try again after sometime.";
                    Session["ErrorMsg"] = msg;
                    url = CorporateRegistrationPage.Url();
                    Console.WriteLine(ex);
                }
                return Redirect(url);
            }
            else
            {
                return this.View(pm);
            }

        }

        //public string saveUserAttachmentFile(HttpPostedFileBase[] files, Guid Id)
        //{
        //    foreach (HttpPostedFileBase file in files)
        //    {
        //        if (file != null && file.ContentLength > 0)
        //        {
        //            // extract only the filename
        //            //var fileName = Path.GetFileNameWithoutExtension(file.FileName);
        //            var fileExt = Path.GetExtension(file.FileName);

        //            var userFile = Path.GetFileName(file.FileName);

        //            byte[] bytes;
        //            if (fileExt == ".jpg" || fileExt == "jpeg")
        //            {
        //                ExifReader ExifReaderobj = new ExifReader();
        //                bytes = ExifReaderobj.SetUpMetadataOnImage(file.InputStream, file.FileName);
        //                //using (BinaryReader br = new BinaryReader(file.InputStream))
        //                //{
        //                //    bytes = br.ReadBytes(file.ContentLength);
        //                //}
        //            }
        //            else
        //            {



        //                using (BinaryReader br = new BinaryReader(file.InputStream))
        //                {
        //                    bytes = br.ReadBytes(file.ContentLength);
        //                }
        //            }

        //            PortsGMSDataContext rdb = new PortsGMSDataContext();

        //            PortsGms_Registration_Attachment pgra = new PortsGms_Registration_Attachment();
        //            pgra.Id = Guid.NewGuid();
        //            pgra.Gms_Registration_Id = Id;
        //            pgra.FileName = file.FileName;
        //            pgra.ContentType = file.ContentType;
        //            pgra.DocData = bytes;

        //            pgra.Created_Date = System.DateTime.Now;

        //            rdb.PortsGms_Registration_Attachments.InsertOnSubmit(pgra);
        //            rdb.SubmitChanges();
        //        }
        //    }
        //    return string.Empty;
        //}

        public string saveUserAttachmentFile(HttpPostedFileBase[] files, Guid Id)
        {
            foreach (HttpPostedFileBase file in files)
            {
                if (file != null)
                {
                    // extract only the filename
                    //var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                    var fileExt = Path.GetExtension(file.FileName);

                    var userFile = Path.GetFileName(file.FileName);

                    byte[] bytes;
                    if (fileExt == ".jpg" || fileExt == "jpeg")
                    {
                        ExifReader ExifReaderobj = new ExifReader();
                        bytes = ExifReaderobj.SetUpMetadataOnImage(file.InputStream, file.FileName);
                        //using (BinaryReader br = new BinaryReader(file.InputStream))
                        //{
                        //    bytes = br.ReadBytes(file.ContentLength);
                        //}
                    }
                    else
                    {



                        using (BinaryReader br = new BinaryReader(file.InputStream))
                        {
                            bytes = br.ReadBytes(file.ContentLength);
                        }
                    }

                    PortsGMSDataContext rdb = new PortsGMSDataContext();

                    PortsGms_Registration_Attachment pgra = new PortsGms_Registration_Attachment();
                    pgra.Id = Guid.NewGuid();
                    pgra.Gms_Registration_Id = Id;
                    pgra.FileName = file.FileName;
                    pgra.ContentType = file.ContentType;
                    pgra.DocData = bytes;

                    pgra.Created_Date = System.DateTime.Now;

                    rdb.PortsGms_Registration_Attachments.InsertOnSubmit(pgra);
                    rdb.SubmitChanges();
                }
            }
            return string.Empty;
        }
        public ActionResult PortsGMSGenerateOTP(PortsGMSOTP model)
        {
            ActionResult actionResult;

            var result = new { status = "0" };
            string msg = "";
            bool Validated = false;
            actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
            try
            {
                Recaptchav3Provider recaptchav2 = new Recaptchav3Provider();
                Validated = recaptchav2.IsReCaptchValid(model.Gcptchares);
            }
            catch (Exception ex)
            {
                msg = "Somthing went wrong please try again.";
                Session["ErrorMsg"] = msg;
                Console.WriteLine(ex);
            }

            if (Validated == true)
            {
                try
                {
                    PortsGMSDataContext rdb = new PortsGMSDataContext();
                    var obj = rdb.PortsGms_Registrations.Where(x => x.Mobile == model.OTPFor || x.Email == model.OTPFor).FirstOrDefault();
                    string userName = "Customer";
                    if (obj != null)
                    {
                        userName = obj.Name;
                    }
                    Log.Info($"Ports Get Mobile number from  model: {model.OTPFor}", model.OTPFor);
                    string generatedotp = this.portsrepo.PortsGMSStoreGeneratedOtp(model);
                    if (generatedotp != null)
                    {
                        Log.Info($"Ports otp generated: {generatedotp}", generatedotp);
                        var energycalculator = DictionaryPhraseRepository.Current.Get("/More Services For You/Energy Calculator/Energy Calculator", "Energy Calculator");
                        string Message = "Hi " + userName + ", Please use this OTP for verification:" + generatedotp + " Thanks, APSEZ GMS TEAM";
                        Log.Info($"Ports message generated: {Message}", Message);
                        if (model.IsMobile)
                        {
                            try
                            {
                                string apiurl = string.Format("https://apps.vibgyortel.in/client/api/sendmessage?apikey=532d9e0ffc47eba6&mobiles={0}&sms={1}&senderid=APSEZL&dlt-entity-id=1101738230000039545&dlt-template-id=1707167583658958772", model.OTPFor, Message);
                                HttpClient client = new HttpClient()
                                {
                                    BaseAddress = new Uri(apiurl)
                                };
                                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                                if (!client.GetAsync(apiurl).Result.IsSuccessStatusCode)
                                {
                                    Log.Error("OTP Api call failed. https://otp2.maccesssmspush.com", this);
                                }
                                else
                                {
                                    Log.Error("OTP Api call success. https://otp2.maccesssmspush.com", this);
                                }
                            }
                            catch (Exception exception)
                            {
                                Exception ex = exception;
                                Log.Error(string.Format("{0}", 0), ex, this);
                            }
                        }
                        else
                        {
                            SendEmailGMSOTP(model.OTPFor, userName, model.OTPType, generatedotp);
                        }

                        result = new { status = "1" };
                        actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
                    }

                }
                catch (Exception exception1)
                {
                    Exception ex = exception1;
                    Log.Error(string.Format("{0}", 0), ex, this);
                    actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                msg = "Captcha Validation Failed!!!";
                Session["ErrorMsg"] = msg;
                Console.WriteLine("Captcha Validation Failed!!!!");
            }
            return actionResult;
        }

        public ActionResult PortsGMSVerifyOTP(PortsGMSOTP model)
        {
            ActionResult actionResult;
            PortsGMSDataContext pgd = new PortsGMSDataContext();
            var result = new { status = "0" };
            try
            {

                if (!string.IsNullOrEmpty(model.OTPFor))
                {
                    var data = pgd.PortsGMSOTPHistories.Where(x => x.OTPFor == model.OTPFor && x.OTP == model.OTP && x.OTPType == model.OTPType).FirstOrDefault();
                    result = new { status = data.OTP };
                    if (string.Equals(data.OTP, model.OTP))
                    {
                        result = new { status = "1" };
                        data.Status = true;
                        pgd.SubmitChanges();


                    }
                }



                actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exception1)
            {
                Exception ex = exception1;
                Log.Error(string.Format("{0}", 0), ex, this);
                actionResult = base.Json(result, JsonRequestBehavior.AllowGet);
            }
            return actionResult;
        }

        [HttpGet]
        public ActionResult PortsGmsLogin()
        {
            Session.Abandon();
            Response.Cookies.Add(new System.Web.HttpCookie("ASP.NET_SessionId", ""));
            if (Request.QueryString["gid"] != null)
            {
                Session["InCompleteGrievance"] = Request.QueryString["gid"].ToString();
            }
            else
            {
                Session["InCompleteGrievance"] = null;
            }
            return View();

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult PortsGmsLogin(PortsGMSLoginModel pm)
        {
            string msg = "";
            string direct = "/Grievance/Booking";
            bool Validated = false;

            try
            {
                Recaptchav3Provider recaptchav2 = new Recaptchav3Provider();
                Validated = recaptchav2.IsReCaptchValid(pm.Gcptchares);
            }
            catch (Exception ex)
            {
                msg = "Somthing went wrong please try again.";
                Session["ErrorMsg"] = msg;
                Console.WriteLine(ex);
            }
            if (Validated == true)
            {
                try
                {
                    PortsGMSDataContext rdb = new PortsGMSDataContext();
                    PortsGms_Registration pgr = new PortsGms_Registration();

                    if (rdb.PortsGms_Registrations.Any(x => x.Mobile == GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Mobile, EncryptionIV) || x.Email == GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Email, EncryptionIV)))
                    {
                        if (pm.Mobile != null)
                        {
                            var Mobile = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Mobile && x.OTPType == "login").OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                            if ((DateTime.Now - Mobile.CreatedDate.Value).TotalMinutes < 10 && Mobile.Status == false && Mobile.OTP == pm.OTP)
                            {
                                direct = "/Grievance/Booking";
                                Mobile.Status = true;
                                rdb.SubmitChanges();
                                var registerUser = rdb.PortsGms_Registrations.Where(x => x.Mobile == GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Mobile, EncryptionIV) && x.Status == true).FirstOrDefault();
                                /*PortsGMSUserSession.PortsGMSUserSessionContext = new PortsGMSLoginModel
                                {
                                    Mobile = pm.Mobile,
                                    RegistrationID = registerUser.Id,
                                    UserType = registerUser.User_Type.ToString()


                                };*/

                                if (registerUser.User_Type == PortsGMSTemplates.UserType.Stakeholder)
                                {
                                    var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceStakHolderDashbord);
                                    direct = page.Url();
                                    Session["PortsGmsUser"] = registerUser.Id.ToString();
                                    if (pm.InCompleteGrievance != null)
                                    {
                                        var IncompleteGrievance = rdb.PortsGMSInCompleteGrievances.Where(x => x.Id == new Guid(pm.InCompleteGrievance)).FirstOrDefault();
                                        var GrievanceSubmissionPage = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceBookingOnBehalfReview);
                                        direct = GrievanceSubmissionPage.Url() + "?id=" + IncompleteGrievance.GirevanceId + "&icd=" + pm.InCompleteGrievance;
                                    }
                                }
                                else
                                if (registerUser.User_Type == PortsGMSTemplates.UserType.Admin)
                                {
                                    var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceAdminDashbord);
                                    direct = page.Url();
                                    Session["PortsGmsAdminUser"] = registerUser.Id.ToString();
                                }
                                else if (registerUser.User_Type == PortsGMSTemplates.UserType.Level0)
                                {
                                    var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel0Dashbord);
                                    direct = page.Url();
                                    Session["PortsGmsLevel0User"] = registerUser.Id.ToString();
                                }
                                else
                                 if (registerUser.User_Type == PortsGMSTemplates.UserType.Level1)
                                {
                                    var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel1Dashbord);
                                    direct = page.Url();
                                    Session["PortsGmsLevel1User"] = registerUser.Id.ToString();
                                }
                                else
                                 if (registerUser.User_Type == PortsGMSTemplates.UserType.Level2)
                                {
                                    var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel2Dashbord);
                                    direct = page.Url();
                                    Session["PortsGmsLevel2User"] = registerUser.Id.ToString();
                                }
                                else
                                 if (registerUser.User_Type == PortsGMSTemplates.UserType.Level3)
                                {
                                    var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel3Dashbord);
                                    direct = page.Url();
                                    Session["PortsGmsLevel3User"] = registerUser.Id.ToString();
                                }
                                PortsGMSLoginHistory entity = new PortsGMSLoginHistory()
                                {
                                    Id = Guid.NewGuid(),
                                    LoginId = pm.Mobile,
                                    Create_Date = DateTime.Now.Date,
                                    Status = true

                                };
                                rdb.PortsGMSLoginHistories.InsertOnSubmit(entity);
                                rdb.SubmitChanges();
                            }
                            else
                            {
                                direct = "/Grievance";
                                msg = "Mobile Verification Failed Please Try Again";
                                Session["ErrorMsg"] = msg;
                                PortsGMSLoginHistory entity = new PortsGMSLoginHistory()
                                {
                                    Id = Guid.NewGuid(),
                                    LoginId = pm.Mobile,
                                    Create_Date = DateTime.Now.Date,
                                    Status = false

                                };
                                rdb.PortsGMSLoginHistories.InsertOnSubmit(entity);
                                rdb.SubmitChanges();

                            }
                        }
                        if (pm.Email != null)
                        {
                            var Email = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Email && x.OTPType == "login").OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                            if ((DateTime.Now - Email.CreatedDate.Value).TotalMinutes < 10 && Email.Status == false && Email.OTP == pm.OTP)
                            {

                                Email.Status = true;
                                rdb.SubmitChanges();
                                var registerUser = rdb.PortsGms_Registrations.Where(x => x.Email == GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Email, EncryptionIV) && x.Status == true).FirstOrDefault();
                                /*PortsGMSUserSession.PortsGMSUserSessionContext = new PortsGMSLoginModel
                                {
                                    Email = pm.Email,
                                    RegistrationID = registerUser.Id,
                                    UserType = registerUser.User_Type.ToString()

                                };
                                Session["PortsGmsUser"] = PortsGMSUserSession.PortsGMSUserSessionContext;*/
                                if (registerUser.User_Type == PortsGMSTemplates.UserType.Stakeholder)
                                {
                                    var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceStakHolderDashbord);
                                    direct = page.Url();
                                    Session["PortsGmsUser"] = registerUser.Id.ToString();

                                    if (pm.InCompleteGrievance != null)
                                    {
                                        var IncompleteGrievance = rdb.PortsGMSInCompleteGrievances.Where(x => x.Id == new Guid(pm.InCompleteGrievance)).FirstOrDefault();
                                        var GrievanceSubmissionPage = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceBookingOnBehalfReview);
                                        direct = GrievanceSubmissionPage.Url() + "?id=" + IncompleteGrievance.GirevanceId + "&icd=" + pm.InCompleteGrievance;
                                    }
                                }
                                else
                                if (registerUser.User_Type == PortsGMSTemplates.UserType.Admin)
                                {
                                    var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceAdminDashbord);
                                    direct = page.Url();
                                    Session["PortsGmsAdminUser"] = registerUser.Id.ToString();
                                }
                                else if (registerUser.User_Type == PortsGMSTemplates.UserType.Level0)
                                {
                                    var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel0Dashbord);
                                    direct = page.Url();
                                    Session["PortsGmsLevel0User"] = registerUser.Id.ToString();
                                }
                                else
                                if (registerUser.User_Type == PortsGMSTemplates.UserType.Level1)
                                {
                                    var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel1Dashbord);
                                    direct = page.Url();
                                    Session["PortsGmsLevel1User"] = registerUser.Id.ToString();
                                }
                                else
                                if (registerUser.User_Type == PortsGMSTemplates.UserType.Level2)
                                {
                                    var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel2Dashbord);
                                    direct = page.Url();
                                    Session["PortsGmsLevel2User"] = registerUser.Id.ToString();
                                }
                                else
                                if (registerUser.User_Type == PortsGMSTemplates.UserType.Level3)
                                {
                                    var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel3Dashbord);
                                    direct = page.Url();
                                    Session["PortsGmsLevel3User"] = registerUser.Id.ToString();
                                }

                                PortsGMSLoginHistory entity = new PortsGMSLoginHistory()
                                {
                                    Id = Guid.NewGuid(),
                                    LoginId = pm.Email,
                                    Create_Date = DateTime.Now.Date,
                                    Status = true

                                };
                                rdb.PortsGMSLoginHistories.InsertOnSubmit(entity);
                                rdb.SubmitChanges();
                            }
                            else
                            {
                                direct = "/Grievance";
                                msg = "Email Verification Failed Please Try Again";
                                Session["ErrorMsg"] = msg;
                                PortsGMSLoginHistory entity = new PortsGMSLoginHistory()
                                {
                                    Id = Guid.NewGuid(),
                                    LoginId = pm.Email,
                                    Create_Date = DateTime.Now.Date,
                                    Status = false

                                };
                                rdb.PortsGMSLoginHistories.InsertOnSubmit(entity);
                                rdb.SubmitChanges();
                            }
                        }
                    }
                    else
                    {
                        msg = "User not regitered";
                        Session["ErrorMsg"] = msg;
                        var loginhistories = rdb.PortsGMSLoginHistories.Where(x => x.LoginId == (pm.Mobile != null ? pm.Mobile : pm.Email) && x.Create_Date.GetValueOrDefault().Date == DateTime.Now.Date && x.Status == false).Count();
                        if (loginhistories <= 2)
                        {
                            PortsGMSLoginHistory entity = new PortsGMSLoginHistory()
                            {
                                Id = Guid.NewGuid(),
                                LoginId = (pm.Mobile != null ? pm.Mobile : pm.Email),
                                Create_Date = DateTime.Now.Date,
                                Status = false

                            };
                            rdb.PortsGMSLoginHistories.InsertOnSubmit(entity);
                            rdb.SubmitChanges();
                        }
                        else
                        {

                            msg = "You have exceeded the Login Limit for Today";
                            Session["ErrorMsg"] = msg;
                        }
                        msg = "Email and Mobile Number is not valid. Please enter valid credential.";
                        Session["ErrorMsg"] = msg;
                    }

                }
                catch (Exception ex)
                {
                    msg = "Somthing went wrong please try again.";
                    Session["ErrorMsg"] = msg;
                    Console.WriteLine(ex);
                }
            }
            else
            {
                msg = "Captcha Validation Failed!!!";
                Session["ErrorMsg"] = msg;
                Console.WriteLine("Captcha Validation Failed!!!!");
            }
            return Redirect(direct);
        }

        [HttpGet]
        public ActionResult PortsGmsGrievanceBooking()
        {
            if (Session["PortsGMSUser"] == null)
            {
                return Redirect("/Grievance");
            }

            Guid RegistrationID = new Guid(Session["PortsGMSUser"].ToString());
            PortsGMSDataContext rdb = new PortsGMSDataContext();
            var StakeHolder = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationID).FirstOrDefault();
            PortsGMSGrievanceBookingModel pm = new PortsGMSGrievanceBookingModel();
            pm.Nature = StakeHolder.Answer;
            return this.View(pm);
        }

        [HttpPost]
        public ActionResult PortsGMSGrievanceBooking(PortsGMSGrievanceBookingModel pm)
        {

            string msg = "";
            try
            {
                if (Session["PortsGMSUser"] != null)
                {

                    Guid RegistrationID = new Guid(Session["PortsGMSUser"].ToString());

                    PortsGMSDataContext rdb = new PortsGMSDataContext();
                    PortsGMSGrievanceBooking pgr = new PortsGMSGrievanceBooking();
                    PortsGms_Grievance_Booking_Attachment pgra = new PortsGms_Grievance_Booking_Attachment();

                    var StakeHolder = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationID).FirstOrDefault();


                    pm.Id = Guid.NewGuid();
                    pgr.Id = pm.Id;
                    pgr.RegistrationId = RegistrationID;
                    pgr.Nature = pm.Nature;
                    pgr.Company = pm.Company;
                    pgr.Location = pm.Location;
                    pgr.Subject = pm.Subject;
                    pgr.Brief = pm.Brief;
                    pgr.WhoImpacted = pm.WhoImpacted;
                    pgr.UserType = "Level 0";
                    pgr.AssignedLevel = "0";
                    pgr.AssignedState = "Assigned";

                    pgr.Created_Date = System.DateTime.Now; ;
                    pgr.Modified_Date = System.DateTime.Now; ;
                    pgr.Status = pm.SaveAsDraft == "1" ? PortsGMSTemplates.GMSFlags.BookingDraft : PortsGMSTemplates.GMSFlags.BookingOpen;
                    #region Insert to DB
                    rdb.PortsGMSGrievanceBookings.InsertOnSubmit(pgr);
                    rdb.SubmitChanges();

                    saveGrievanceBookingAttachmentFile(pm.File, pm.Id);
                    msg = "Grievance Booking Save as draft";
                    Session["SuccessMsg"] = msg;
                    SendEmailGMSStackHolder(GSM_EncryptDecrypt.DecryptString(EncryptionKey, StakeHolder.Email, EncryptionIV), StakeHolder.Name, pgr.Id, PortsGMSTemplates.GMSFlags.Booking);
                    if (pm.SaveAsDraft != "1")
                    {
                        //PortsGMSAssignGrievanceBooking(pgr.Id, pgr.Nature);
                        PortsGMSAssignGrievanceBookingToLevelZero(pgr.Id);
                        msg = "Grievance Booking Successfully Done";
                        Session["SuccessMsg"] = msg;
                    }

                    //  Session["validate"] = Request.Cookies["SIDCC"].Value;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                msg = "Somthing went wrong please try again after some time";
                Session["ErrorMsg"] = msg;
                Console.WriteLine(ex);
            }
            var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceBooking);

            return Redirect(page.Url());
        }
        //Assign  Grievance To Level 0
        public void PortsGMSAssignGrievanceBookingToLevelZero(Guid Id = new Guid())
        {

            PortsGMSDataContext rdb = new PortsGMSDataContext();
            PortsGMSGrievanceBooking bookings = new PortsGMSGrievanceBooking();
            PortsGMSBookingAssignment bookingAssignment = new PortsGMSBookingAssignment();
            PortsGms_Registration Level0User = new PortsGms_Registration();

            Level0User = rdb.PortsGms_Registrations.Where(x => x.User_Type.Equals("level0")).FirstOrDefault();

            if (Level0User != null)
            {
                var Grievance = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == Id).FirstOrDefault();
                Grievance.UserType = "Level 0";
                Grievance.AssignedLevel = "0";
                Grievance.AssignedState = "Assigned";
                bookingAssignment.Id = Guid.NewGuid();
                bookingAssignment.GrievanceBookingId = Id;
                bookingAssignment.RegistrationId = Level0User.Id;
                bookingAssignment.LevelInfo = Level0User.User_Type;
                bookingAssignment.Response = PortsGMSTemplates.GMSFlags.RePendings;
                bookingAssignment.Status = PortsGMSTemplates.GrievanceStatus.Level0;
                bookingAssignment.Department = Level0User.Department;
                bookingAssignment.UserType = "Level 0";
                bookingAssignment.AssignedLevel = "0";
                bookingAssignment.AssignedState = "Assigned";
                bookingAssignment.CreatedDate = System.DateTime.Now; ;
                bookingAssignment.ModifiedDate = System.DateTime.Now; ;
                rdb.PortsGMSBookingAssignments.InsertOnSubmit(bookingAssignment);
                rdb.SubmitChanges();
            }
        }
        public void PortsGMSAssignGrievanceBooking(Guid Id = new Guid(), string nature = null)
        {

            var Department = Context.Database.GetItem(PortsGMSTemplates.MasterData.GrievanceNature);
            PortsGMSDataContext rdb = new PortsGMSDataContext();
            PortsGMSBookingAssignment bookingAssignment = new PortsGMSBookingAssignment();
            PortsGms_Registration Level1User = new PortsGms_Registration();
            //var Level1User = "";
            foreach (var element in Department.GetChildren().ToList())
            {
                if (element.Fields["Key"].Value == nature)
                {

                    // PortsGms_Registration user = new PortsGms_Registration();

                    Level1User = rdb.PortsGms_Registrations.Where(x => x.Department == element.Fields["Value"].Value).FirstOrDefault();
                    break;

                }


            }

            if (Level1User != null)
            {
                bookingAssignment.Id = Guid.NewGuid();
                bookingAssignment.GrievanceBookingId = Id;
                bookingAssignment.RegistrationId = Level1User.Id;
                bookingAssignment.LevelInfo = Level1User.User_Type;
                bookingAssignment.Response = PortsGMSTemplates.GMSFlags.RePendings;
                bookingAssignment.Status = PortsGMSTemplates.GrievanceStatus.Level1;
                bookingAssignment.Department = Level1User.Department;
                bookingAssignment.CreatedDate = System.DateTime.Now; ;
                bookingAssignment.ModifiedDate = System.DateTime.Now; ;
                rdb.PortsGMSBookingAssignments.InsertOnSubmit(bookingAssignment);
                rdb.SubmitChanges();

            }


        }

        [HttpGet]
        public ActionResult PortsGmsGrievanceBookingOnBehalf()
        {
            if (Session["PortsGMSLevel0User"] == null)
            {
                return Redirect("/Grievance");
            }
            PortsGMSDataContext rdb = new PortsGMSDataContext();

            var RegisterdUser = rdb.PortsGms_Registrations.Where(x => x.User_Type == PortsGMSTemplates.UserType.Stakeholder).ToList();

            PortsGMSGrievanceBookingOnBeHalfModel StakHolderObj = new PortsGMSGrievanceBookingOnBeHalfModel();

            foreach (var item in RegisterdUser)
            {
                PortsGms_Registration ObjUsers = new PortsGms_Registration();
                ObjUsers.Id = item.Id;
                ObjUsers.Name = item.Name;

                StakHolderObj.StakHolders.Add(ObjUsers);
            }

            return this.View(StakHolderObj);
        }
        [HttpPost]
        public ActionResult PortsGmsGrievanceBookingOnBehalf(PortsGMSGrievanceBookingOnBeHalfModel pm)
        {
            string msg = "";
            try
            {
                if (Session["PortsGMSLevel0User"] != null)
                {
                    Guid RegistrationID = new Guid(Session["PortsGMSLevel0User"].ToString());
                    PortsGMSDataContext rdb = new PortsGMSDataContext();
                    PortsGMSGrievanceBooking pgr = new PortsGMSGrievanceBooking();
                    PortsGms_Grievance_Booking_Attachment pgra = new PortsGms_Grievance_Booking_Attachment();
                    PortsGMSGrievanceBookingOnBehalf bof = new PortsGMSGrievanceBookingOnBehalf();
                    var Level0User = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationID).FirstOrDefault();

                    pm.Id = Guid.NewGuid();
                    pgr.Id = pm.Id;
                    pgr.RegistrationId = RegistrationID;
                    pgr.Nature = pm.Nature;
                    pgr.Company = pm.Company;
                    pgr.Location = pm.Location;
                    pgr.Subject = pm.Subject;
                    pgr.Brief = pm.Brief;
                    pgr.WhoImpacted = pm.WhoImpacted;

                    pgr.Created_Date = System.DateTime.Now; ;
                    pgr.Modified_Date = System.DateTime.Now; ;
                    pgr.Status = PortsGMSTemplates.GMSFlags.BookingOnBehalf;
                    bof.Id = Guid.NewGuid();
                    bof.Name = pm.Name;
                    bof.Email = pm.Email;
                    bof.Mobile = pm.Mobile;
                    bof.OnBehalf = pm.OnBehalf;
                    bof.CreatedBy = RegistrationID;
                    bof.Created_Date = System.DateTime.Now;
                    bof.Modified_Date = System.DateTime.Now;
                    bof.BookingId = pgr.Id;
                    #region Insert to DB
                    rdb.PortsGMSGrievanceBookings.InsertOnSubmit(pgr);
                    rdb.PortsGMSGrievanceBookingOnBehalfs.InsertOnSubmit(bof);
                    rdb.SubmitChanges();

                    saveGrievanceBookingAttachmentFile(pm.File, pm.Id);
                    msg = "Grievance Booking Successfully Done On Behalf Of Stakholder";
                    Session["SuccessMsg"] = msg;
                    SendEmailGMSStackHolder(pm.Email, pm.Name, pgr.Id, null, PortsGMSTemplates.GMSFlags.BookingOnBehalf);

                    //PortsGMSAssignGrievanceBooking(pgr.Id, pgr.Nature);

                    //  Session["validate"] = Request.Cookies["SIDCC"].Value;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                msg = "Somthing went wrong please try again after some time";
                Session["ErrorMsg"] = msg;
                Console.WriteLine(ex);
            }


            return this.View(pm);
        }

        [HttpGet]
        public ActionResult PortsGmsGrievanceBookingOnBehalfReview()
        {
            Guid GrievanceId = new Guid(Request.QueryString["id"].ToString());
            PortsGMSDataContext rdb = new PortsGMSDataContext();

            var GrievanceOnBehalf = rdb.PortsGMSGrievanceBookingOnBehalfs.Where(x => x.BookingId == GrievanceId).FirstOrDefault();
            var Grievance = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId).FirstOrDefault();

            PortsGMSGrievanceBookingOnBeHalfModel StakHolderObj = new PortsGMSGrievanceBookingOnBeHalfModel();



            return this.View(StakHolderObj);
        }

        [HttpGet]
        public ActionResult PortsGmsGrievanceBookingOnBehalfStackholderSubmission()
        {

            if (Request.QueryString["id"] != null)
            {
                Guid GrievanceId = new Guid(Request.QueryString["id"].ToString());

                if (Request.QueryString["icd"] != null)
                {
                    Session["IncompleteGrievanceIcd"] = Request.QueryString["icd"].ToString();
                }
                else
                {
                    Session["IncompleteGrievanceIcd"] = null;
                }

                PortsGMSDataContext rdb = new PortsGMSDataContext();

                var GrievanceOnBehalf = rdb.PortsGMSGrievanceBookingOnBehalfs.Where(x => x.BookingId == GrievanceId).FirstOrDefault();
                if (GrievanceOnBehalf != null)
                {
                    //var url = HttpContext.Request.UrlReferrer.ToString();
                    PortsGMSInCompleteGrievance pgr = new PortsGMSInCompleteGrievance();
                    pgr.Id = Guid.NewGuid();
                    pgr.Email = GrievanceOnBehalf.Email;
                    pgr.Mobile = GrievanceOnBehalf.Mobile;
                    pgr.GirevanceId = GrievanceOnBehalf.BookingId;
                    pgr.Status = false;
                    pgr.Created = System.DateTime.Now;
                    pgr.Modified = System.DateTime.Now;

                    rdb.PortsGMSInCompleteGrievances.InsertOnSubmit(pgr);
                    rdb.SubmitChanges();


                    var stakeHolder = rdb.PortsGms_Registrations.Where(x => x.Email == GSM_EncryptDecrypt.EncryptString(EncryptionKey, GrievanceOnBehalf.Email, EncryptionIV)).FirstOrDefault();
                    if (stakeHolder != null)
                    {

                        if (Session["PortsGmsUser"] == null)
                        {

                            var Login = Sitecore.Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLogin);
                            return Redirect(Login.Url() + "?gid=" + pgr.Id);
                        }
                    }
                    else
                    {

                        var Registration = Sitecore.Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceRegistration);
                        return Redirect(Registration.Url() + "?gid=" + pgr.Id);

                    }
                    var Grievance = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId).FirstOrDefault();

                    PortsGMSGrievanceBookingModel StakHolderObj = new PortsGMSGrievanceBookingModel();
                    StakHolderObj.Company = Grievance.Company;
                    StakHolderObj.Location = Grievance.Location;
                    //StakHolderObj.Nature = Grievance.Nature;
                    StakHolderObj.Subject = Grievance.Subject;
                    StakHolderObj.WhoImpacted = Grievance.WhoImpacted;
                    StakHolderObj.Brief = Grievance.Brief;
                    StakHolderObj.Id = Grievance.Id;

                    Guid RegistrationID = new Guid(Session["PortsGMSUser"].ToString());

                    var StakeHolders = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationID).FirstOrDefault();
                    PortsGMSGrievanceBookingModel pm = new PortsGMSGrievanceBookingModel();
                    StakHolderObj.Nature = StakeHolders.Answer;


                    return this.View(StakHolderObj);
                }
                else
                {

                    Session["ErrorMsg"] = "Invalid Grievance";

                    PortsGMSGrievanceBookingModel StakHolderObj = new PortsGMSGrievanceBookingModel();
                    return this.View(StakHolderObj);
                }
            }
            else
            {
                Session["ErrorMsg"] = "Invalid Grievance";

                PortsGMSGrievanceBookingModel StakHolderObj = new PortsGMSGrievanceBookingModel();
                return this.View(StakHolderObj);
            }
        }

        [HttpPost]
        public ActionResult PortsGmsGrievanceBookingOnBehalfStackholderSubmission(PortsGMSGrievanceBookingModel pm)
        {
            string msg = "";
            try
            {
                if (Session["PortsGMSUser"] != null)
                {

                    Guid RegistrationID = new Guid(Session["PortsGMSUser"].ToString());

                    PortsGMSDataContext rdb = new PortsGMSDataContext();
                    PortsGMSGrievanceBooking pgr = new PortsGMSGrievanceBooking();
                    PortsGms_Grievance_Booking_Attachment pgra = new PortsGms_Grievance_Booking_Attachment();

                    var IncompleteGrievance = rdb.PortsGMSInCompleteGrievances.Where(x => x.Id == new Guid(pm.Icd)).FirstOrDefault();

                    var StakeHolder = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationID).FirstOrDefault();
                    pgr = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == pm.Id).FirstOrDefault();

                    IncompleteGrievance.Status = true;

                    pgr.RegistrationId = RegistrationID;
                    pgr.Nature = pm.Nature;
                    pgr.Company = pm.Company;
                    pgr.Location = pm.Location;
                    pgr.Subject = pm.Subject;
                    pgr.Brief = pm.Brief;
                    pgr.WhoImpacted = pm.WhoImpacted;
                    pgr.Modified_Date = System.DateTime.Now; ;
                    pgr.Status = pm.SaveAsDraft == "1" ? PortsGMSTemplates.GMSFlags.BookingDraft : PortsGMSTemplates.GMSFlags.BookingOpen;
                    #region Insert to DB
                    //rdb.PortsGMSGrievanceBookings.InsertOnSubmit(pgr);
                    rdb.SubmitChanges();

                    saveGrievanceBookingAttachmentFile(pm.File, pm.Id);
                    msg = "Grievance Booking Save as draft";
                    Session["SuccessMsg"] = msg;
                    SendEmailGMSStackHolder(GSM_EncryptDecrypt.DecryptString(EncryptionKey, StakeHolder.Email, EncryptionIV), StakeHolder.Name, pgr.Id, PortsGMSTemplates.GMSFlags.Booking);
                    if (pm.SaveAsDraft != "1")
                    {
                        PortsGMSAssignGrievanceBooking(pgr.Id, pgr.Nature);
                        msg = "Grievance Booking Successfully Done";
                        Session["SuccessMsg"] = msg;
                    }

                    //  Session["validate"] = Request.Cookies["SIDCC"].Value;
                    #endregion
                }
            }
            catch (Exception ex)
            {
                msg = "Somthing went wrong please try again after some time";
                Session["ErrorMsg"] = msg;
                Console.WriteLine(ex);
            }
            var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceBooking);

            return Redirect(page.Url());
        }

        public string saveGrievanceBookingAttachmentFile(HttpPostedFileBase file, Guid Id)
        {
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                //var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExt = Path.GetExtension(file.FileName);

                var userFile = Path.GetFileName(file.FileName);

                byte[] bytes;
                if (fileExt == ".jpg" || fileExt == "jpeg")
                {
                    ExifReader ExifReaderobj = new ExifReader();
                    bytes = ExifReaderobj.SetUpMetadataOnImage(file.InputStream, file.FileName);
                    //using (BinaryReader br = new BinaryReader(file.InputStream))
                    //{
                    //    bytes = br.ReadBytes(file.ContentLength);
                    //}
                }
                else
                {



                    using (BinaryReader br = new BinaryReader(file.InputStream))
                    {
                        bytes = br.ReadBytes(file.ContentLength);
                    }
                }

                PortsGMSDataContext rdb = new PortsGMSDataContext();

                PortsGms_Grievance_Booking_Attachment pgra = new PortsGms_Grievance_Booking_Attachment();
                pgra.Id = Guid.NewGuid();
                pgra.Grievance_booking_Id = Id;
                pgra.FileName = file.FileName;
                pgra.ContentType = file.ContentType;
                pgra.DocData = bytes;

                pgra.Created_Date = System.DateTime.Now;

                rdb.PortsGms_Grievance_Booking_Attachments.InsertOnSubmit(pgra);
                rdb.SubmitChanges();
            }
            return string.Empty;
        }


        [HttpGet]
        public ActionResult PortsGmsUpdateUsers()
        {
            if (Session["PortsGMSAdminUser"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid UserId = new Guid();
            PortsGMSUpdateUsers ObjUsers = new PortsGMSUpdateUsers();
            if (Request.QueryString["user"] != null)
            {
                UserId = new Guid(Request.QueryString["user"].ToString());
                PortsGMSDataContext rdb = new PortsGMSDataContext();


                ObjUsers.GMSRegisterdUsers = rdb.PortsGms_Registrations.Where(x => x.Id == UserId).FirstOrDefault();
                return View(ObjUsers);
            }
            else
            {
                ObjUsers.GMSRegisterdUsers = null;
                return View(ObjUsers);
            }


        }


        [HttpPost]
        public ActionResult PortsGmsUpdateUsers(PortsGMSUpdateUsers pm)
        {
            PortsGMSDataContext rdb = new PortsGMSDataContext();
            PortsGMSUpdateUsers ObjUsers = new PortsGMSUpdateUsers();
            var updateUser = rdb.PortsGms_Registrations.Where(x => x.Id == pm.Id).FirstOrDefault();
            ObjUsers.GMSRegisterdUsers = updateUser;
            //if (ModelState.IsValid)
            //{
            string msg = "";

            try
            {

                PortsGms_Registration pgr = new PortsGms_Registration();
                PortsGms_Registration_Attachment pgra = new PortsGms_Registration_Attachment();




                updateUser.Status = pm.Status;



                updateUser.Modified_Date = System.DateTime.Now; ;

                #region Insert to DB

                rdb.SubmitChanges();

                ObjUsers.GMSRegisterdUsers = updateUser;

                return View(ObjUsers);

                //SendEmailGMSAddUser(pgr.Email, pgr.Name, pgr.User_Type);

                msg = "User Updated Successfully";

                Session["SuccessMsg"] = msg;




                //  Session["validate"] = Request.Cookies["SIDCC"].Value;
                #endregion
            }
            catch (Exception ex)
            {
                msg = "Somthing went wrong please try again after sometime.";
                Session["ErrorMsg"] = msg;

                Console.WriteLine(ex);
            }

            /*}
            else
            {
                

                return View(ObjUsers);
            }*/

            return View(ObjUsers);
        }

        [HttpGet]
        public ActionResult PortsGmsAddUsers()
        {
            if (Session["PortsGMSAdminUser"] == null)
            {
                return Redirect("/Grievance");
            }


            return View();
        }
        [HttpPost]
        public ActionResult PortsGmsAddUsers(PortsGMSAddUsers pm)
        {
            string msg = "";
            string url = "";
            bool Validated = false;

            try
            {
                Recaptchav3Provider recaptchav2 = new Recaptchav3Provider();
                Validated = recaptchav2.IsReCaptchValid(pm.AddUsersGcaptcha);
            }
            catch (Exception ex)
            {
                msg = "Somthing went wrong please try again.";
                Session["ErrorMsg"] = msg;
                Console.WriteLine(ex);
            }

            if (Validated == true)
            {
                if (ModelState.IsValid)
                {

                    try
                    {
                        PortsGMSDataContext rdb = new PortsGMSDataContext();
                        PortsGms_Registration pgr = new PortsGms_Registration();
                        PortsGms_Registration_Attachment pgra = new PortsGms_Registration_Attachment();
                        var ad = PortsGMSTemplates.UserType.Admin.ToString();
                        if (rdb.PortsGms_Registrations.Any(x => x.Mobile == GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Mobile, EncryptionIV) || x.Email == GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Email, EncryptionIV)) == true)
                        {

                            msg = "User Alrady Registrated";
                            Session["ErrorMsg"] = msg;
                            // url = "/GrievanceRegistration";
                        }
                        else
                        {
                            Log.Info("PortsGmsAddUsers model - " + pm, this);
                            var manageUser = Sitecore.Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceManageUser);
                            pm.Id = Guid.NewGuid();
                            pgr.Id = pm.Id;
                            pgr.Name = pm.Title + ' ' + pm.first_name + ' ' + pm.middle_name + ' ' + pm.last_name;
                            pgr.Email = GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Email, EncryptionIV);
                            pgr.Mobile = GSM_EncryptDecrypt.EncryptString(EncryptionKey, pm.Mobile, EncryptionIV);
                            pgr.User_Type = pm.User_Type;
                            pgr.BusinessGroup = pm.BusinessGroup;
                            pgr.Team = pm.Team;
                            pgr.HO = pm.HO;
                            pgr.SiteHead = pm.SiteHead;
                            pgr.Status = pm.Status;

                            pgr.Department = pm.Department != null ? pm.Department : null;
                            pgr.Location = pm.Location != null ? pm.Location : null;

                            pgr.Created_Date = System.DateTime.Now; ;
                            pgr.Modified_Date = System.DateTime.Now; ;

                            #region Insert to DB
                            rdb.PortsGms_Registrations.InsertOnSubmit(pgr);
                            rdb.SubmitChanges();

                            SendEmailGMSAddUser(pm.Email, pgr.Name, pgr.User_Type);
                            msg = "User Added Successfully";
                            Session["SuccessMsg"] = msg;
                            return Redirect(manageUser.Url());

                        }
                        //  Session["validate"] = Request.Cookies["SIDCC"].Value;
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        Log.Error("PortsGmsAddUsers exception occur - " + ex, this);
                        msg = "Somthing went wrong please try again after sometime.";
                        Session["ErrorMsg"] = msg;
                        Console.WriteLine(ex);
                    }

                }
                else
                {
                    Log.Error("PortsGmsAddUsers Validation Fail - " + pm, this);
                    return this.View(pm);
                }
            }
            else
            {
                msg = "Captcha Validation Failed!!!";
                Session["ErrorMsg"] = msg;
                Console.WriteLine("Captcha Validation Failed!!!!");
            }

            return this.View(pm);
        }

        [HttpGet]
        public ActionResult PortsGMSAdminDashbord()
        {
            if (Session["PortsGMSAdminUser"] == null)
            {
                return Redirect("/Grievance");
            }

            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {

                var lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                             join assign in rdb.PortsGMSBookingAssignments on bookings.Id equals assign.GrievanceBookingId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Status, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, registration.Name, registration.Email, registration.Mobile, assign.LevelInfo, assign.Response }).OrderByDescending(p => p.Created_Date).ToList();

                PortsGmsAdminDashbord ObjBooking = new PortsGmsAdminDashbord();
                foreach (var item in lists)
                {
                    PortsGmsAdminDashbord ObjUsers = new PortsGmsAdminDashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.Location = item.Location;
                    ObjUsers.Nature = item.Nature;
                    ObjUsers.Subject = item.Subject;
                    ObjUsers.Company = item.Company;
                    ObjUsers.WhoImpacted = item.WhoImpacted;
                    ObjUsers.Status = item.Status;
                    ObjUsers.Brief = item.Brief;
                    ObjUsers.Created_Date = item.Created_Date;
                    ObjUsers.Name = item.Name;
                    ObjUsers.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Email, EncryptionIV);
                    ObjUsers.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Mobile, EncryptionIV);
                    ObjUsers.LevelInfo = item.LevelInfo;
                    ObjUsers.Response = item.Response;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }


                return View(ObjBooking);
            }

            // return View();
        }

        [HttpPost]
        public ActionResult PortsGMSAdminDashbord(PortsGmsAdminDashbord pm, string submit)
        {
            if (Session["PortsGMSAdminUser"] == null)
            {
                return Redirect("/Grievance");
            }
            DateTime FromDate = pm.FromDate != null ? DateTime.Parse(pm.FromDate.ToString()).Date : System.DateTime.Now;
            DateTime ToDate = pm.ToDate != null ? DateTime.Parse(pm.ToDate.ToString()).Date : System.DateTime.Now;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {

                //var lists = (from registration in rdb.PortsGms_Registrations
                //             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                //             join assign in rdb.PortsGMSBookingAssignments.Where(x => x.Status == pm.Status && x.CreatedDate >= FromDate && x.CreatedDate <= ToDate) on bookings.Id equals assign.GrievanceBookingId
                //             select new { bookings.Id, bookings.Location, bookings.Status, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, registration.Name, registration.Email, registration.Mobile, assign.LevelInfo, assign.Response }).ToList();
                var adminLists = (from registration in rdb.PortsGms_Registrations
                                  join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                                  join assign in rdb.PortsGMSBookingAssignments on bookings.Id equals assign.GrievanceBookingId
                                  select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Status, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, registration.Name, registration.Email, registration.Mobile, assign.LevelInfo, assign.Response }).Where(x => x.Status == pm.Status && x.Created_Date >= FromDate && x.Created_Date <= ToDate).OrderByDescending(p => p.Created_Date).ToList();


                PortsGmsAdminDashbord ObjBooking = new PortsGmsAdminDashbord();
                if (submit == "Reset")
                {
                    ObjBooking.FromDate = null;
                    ObjBooking.ToDate = null;
                    ObjBooking.Status = null;

                    adminLists = (from registration in rdb.PortsGms_Registrations
                                  join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                                  join assign in rdb.PortsGMSBookingAssignments on bookings.Id equals assign.GrievanceBookingId
                                  select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Status, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, registration.Name, registration.Email, registration.Mobile, assign.LevelInfo, assign.Response }).OrderByDescending(p => p.Created_Date).ToList();

                }
                else
                {
                    ObjBooking.FromDate = pm.FromDate;
                    ObjBooking.ToDate = pm.ToDate;
                    ObjBooking.Status = pm.Status;
                }
                foreach (var item in adminLists)
                {
                    PortsGmsAdminDashbord ObjUsers = new PortsGmsAdminDashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.Location = item.Location;
                    ObjUsers.Nature = item.Nature;
                    ObjUsers.Subject = item.Subject;
                    ObjUsers.Company = item.Company;
                    ObjUsers.WhoImpacted = item.WhoImpacted;
                    ObjUsers.Status = item.Status;
                    ObjUsers.Brief = item.Brief;
                    ObjUsers.Created_Date = item.Created_Date;
                    ObjUsers.Name = item.Name;
                    ObjUsers.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Email, EncryptionIV);
                    ObjUsers.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Mobile, EncryptionIV); ;
                    ObjUsers.LevelInfo = item.LevelInfo;
                    ObjUsers.Response = item.Response;

                    ObjBooking.AllBookings.Add(ObjUsers);
                }


                return View(ObjBooking);
            }

            // return View();
        }

        [HttpGet]
        public ActionResult PortsGMSAdminGrivanceView()
        {
            if (Session["PortsGMSAdminUser"] == null)
            {
                return Redirect("/Grievance");
            }

            Guid GrievanceId = new Guid(Request.QueryString["id"].ToString());
            PortsGMSDataContext rdb = new PortsGMSDataContext();
            PortsGmsAdminDashbord ObjBooking = new PortsGmsAdminDashbord();
            var Grievance = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId).FirstOrDefault();
            var StakHolder = rdb.PortsGms_Registrations.Where(x => x.Id == Grievance.RegistrationId).FirstOrDefault();
            var AssignedHistroy = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == GrievanceId).ToList();
            if (StakHolder != null)
            {
                ObjBooking.Name = StakHolder.Name;
                ObjBooking.DOB = StakHolder.DOB;
                ObjBooking.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, StakHolder.Email, EncryptionIV);
                ObjBooking.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, StakHolder.Mobile, EncryptionIV);
                ObjBooking.Address = StakHolder.Address;
            }
            if (Grievance != null)
            {
                ObjBooking.Id = Grievance.Id;
                ObjBooking.Location = Grievance.Location;
                ObjBooking.Subject = Grievance.Subject;
                ObjBooking.Brief = Grievance.Brief;
                ObjBooking.WhoImpacted = Grievance.WhoImpacted;
                ObjBooking.Company = Grievance.Company;
            }
            foreach (var item in AssignedHistroy)
            {
                var level1User = rdb.PortsGms_Registrations.Where(x => x.Id == item.RegistrationId && x.User_Type == PortsGMSTemplates.UserType.Level1).FirstOrDefault();
                var level2User = rdb.PortsGms_Registrations.Where(x => x.Id == item.RegistrationId && x.User_Type == PortsGMSTemplates.UserType.Level2).FirstOrDefault();
                var level3User = rdb.PortsGms_Registrations.Where(x => x.Id == item.RegistrationId && x.User_Type == PortsGMSTemplates.UserType.Level3).FirstOrDefault();
                ObjBooking.Level1UserName = level1User != null ? level1User.Name : "Not assign yet";
                ObjBooking.Level2UserName = level2User != null ? level2User.Name : "Not assign yet";
                ObjBooking.Level3UserName = level3User != null ? level3User.Name : "Not assign yet";
                ObjBooking.Department = item.Department;
            }

            return this.View(ObjBooking);
        }

        [HttpGet]
        public ActionResult PortsGMSStakHolderGrivanceView()
        {
            if (Session["PortsGMSUser"] == null)
            {
                return Redirect("/Grievance");
            }

            Guid GrievanceId = new Guid(Request.QueryString["id"].ToString());

            PortsGMSDataContext rdb = new PortsGMSDataContext();
            PortsGmsStakHolderDashbord ObjBooking = new PortsGmsStakHolderDashbord();
            var Grievance = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId).FirstOrDefault();
            var StakHolder = rdb.PortsGms_Registrations.Where(x => x.Id == Grievance.RegistrationId).FirstOrDefault();
            var AssignedHistroy = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == GrievanceId).ToList();

            int submitToStakeholderRecordCount = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId && x.UserType == PortsGMSTemplates.UserType.Stakeholder && x.AssignedLevel == "stakeholder" && x.Status == PortsGMSTemplates.GMSFlags.Response).Count();
            ViewBag.SubmitToStakeholderRecordCount = submitToStakeholderRecordCount;
            if (ViewBag.SubmitToStakeholderRecordCount > 1)
            {
                var level0reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level0").FirstOrDefault();
                var level1reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level1").FirstOrDefault();
                var level2reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level2").FirstOrDefault();
                var level3reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level3").FirstOrDefault();
                ObjBooking.Level0Comment = level0reply.Comment;
                ObjBooking.Level1Comment = level1reply.Comment;
                ObjBooking.Level2Comment = level2reply.Comment;
                ObjBooking.Level3Comment = level3reply.Comment;
                ObjBooking.Level0UserName = level0reply.Username;
                ObjBooking.Level1UserName = level1reply.Username;
                ObjBooking.Level2UserName = level2reply.Username;
                ObjBooking.Level3UserName = level3reply.Username;
            }
            if (StakHolder != null)
            {
                ObjBooking.Name = StakHolder.Name;
                ObjBooking.DOB = StakHolder.DOB;
                ObjBooking.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, StakHolder.Email, EncryptionIV);
                ObjBooking.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, StakHolder.Mobile, EncryptionIV);
                ObjBooking.Gender = StakHolder.Gender;
                ObjBooking.Address = StakHolder.Address;
            }
            if (Grievance != null)
            {
                ObjBooking.Id = Grievance.Id;
                ObjBooking.Location = Grievance.Location;
                ObjBooking.Subject = Grievance.Subject;
                ObjBooking.Brief = Grievance.Brief;
                ObjBooking.WhoImpacted = Grievance.WhoImpacted;
                ObjBooking.Company = Grievance.Company;
                ObjBooking.Status = Grievance.Status;
                ObjBooking.FinalComment = Grievance.FinalComment;
                ObjBooking.StakeholderRemarks = Grievance.StakeholderRemarks;
                ObjBooking.Assignedlevel = Grievance.AssignedLevel;
                ObjBooking.UserType = Grievance.UserType;
            }
            //foreach (var item in AssignedHistroy)
            //{
            //    var level1User = rdb.PortsGms_Registrations.Where(x => x.Id == item.RegistrationId && x.User_Type == PortsGMSTemplates.UserType.Level1).FirstOrDefault();
            //    var level2User = rdb.PortsGms_Registrations.Where(x => x.Id == item.RegistrationId && x.User_Type == PortsGMSTemplates.UserType.Level2).FirstOrDefault();
            //    var level3User = rdb.PortsGms_Registrations.Where(x => x.Id == item.RegistrationId && x.User_Type == PortsGMSTemplates.UserType.Level3).FirstOrDefault();
            //    ObjBooking.Level1UserName = level1User != null ? level1User.Name : "Not assign yet";
            //    ObjBooking.Level2UserName = level2User != null ? level2User.Name : "Not assign yet";
            //    ObjBooking.Level3UserName = level3User != null ? level3User.Name : "Not assign yet";
            //    ObjBooking.Department = item.Department;
            //}
            var StakeholderFinalComment = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level0" && x.UserType == "stakeholder").OrderBy(x => x.CreatedDate).ToList();
            if (StakeholderFinalComment != null && StakeholderFinalComment.Count > 0)
            {
                foreach (var item in StakeholderFinalComment)
                {
                    StakeholderModel obj = new StakeholderModel();
                    obj.FinalComment = item.Comment;
                    obj.Created_Date = item.CreatedDate;
                    var username = rdb.PortsGms_Registrations.Where(x => x.Id == item.RegistrationId).Select(x => x.Name).FirstOrDefault();
                    obj.UserName = username;
                    ObjBooking.StakeholderFinalComment.Add(obj);
                }
            }
            return this.View(ObjBooking);
        }

        [HttpPost]
        public ActionResult PortsGMSStakHolderGrivanceView(PortsGmsStakHolderDashbord ObjBooking, bool chkResponse = true)
        {
            string msg = "";
            if (Session["PortsGMSUser"] == null)
            {
                ////return Redirect("/Grievance");
                var item = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLogin);
                return Json(item.Url());
            }

            Guid GrievanceId = ObjBooking.Id;
            PortsGMSDataContext rdb = new PortsGMSDataContext();
            PortsGMSBookingAssignment bookingAssignment = new PortsGMSBookingAssignment();
            var Grievance = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId).FirstOrDefault();
            var StakHolder = rdb.PortsGms_Registrations.Where(x => x.Id == Grievance.RegistrationId).FirstOrDefault();
            if (chkResponse == true)
            {
                Grievance.ChkResponse = true;
                Grievance.Status = PortsGMSTemplates.GMSFlags.BookingClosed;
            }
            else
            {
                Grievance.UserType = "Level 0";
                Grievance.AssignedLevel = "0";
                Grievance.ChkResponse = false;
                Grievance.Status = PortsGMSTemplates.GMSFlags.BookingReOpen;
            }
            Grievance.StakeholderRemarks = ObjBooking.StakeholderRemarks;
            Grievance.Modified_Date = DateTime.Now;
            rdb.SubmitChanges();
            if (chkResponse == false)
            {
                bookingAssignment.Id = Guid.NewGuid();
                bookingAssignment.GrievanceBookingId = GrievanceId;
                bookingAssignment.RegistrationId = Grievance.RegistrationId;
                bookingAssignment.LevelInfo = "level0";
                bookingAssignment.UserType = "Level 0";
                bookingAssignment.Response = PortsGMSTemplates.GMSFlags.RePendings;
                bookingAssignment.Status = PortsGMSTemplates.GrievanceStatus.Reopen;
                bookingAssignment.AssignedLevel = "0";
                bookingAssignment.AssignedState = "Assigned";
                bookingAssignment.CreatedDate = System.DateTime.Now;
                rdb.PortsGMSBookingAssignments.InsertOnSubmit(bookingAssignment);
                rdb.SubmitChanges();
            }
            msg = "Grievance is successfully reviewed from StackHolder";
            Session["SuccessMsgStake"] = msg;
            var item1 = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceStakHolderDashbord);
            return Json(item1.Url());
        }

        [HttpGet]
        public ActionResult PortsGMSStakHolderDashbord()
        {
            if (Session["PortsGMSUser"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSUser"].ToString());
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {




                //BookingData = rdb.PortsGMSGrievanceBookings.ToList();
                var lists = (from bookings in rdb.PortsGMSGrievanceBookings
                             join registration in rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId) on bookings.RegistrationId equals registration.Id
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.Status, registration.Name, registration.Email, registration.Mobile }).OrderByDescending(p => p.Created_Date).ToList();

                PortsGmsStakHolderDashbord ObjBooking = new PortsGmsStakHolderDashbord();
                foreach (var item in lists)
                {
                    PortsGmsStakHolderDashbord ObjUsers = new PortsGmsStakHolderDashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.Location = item.Location;
                    ObjUsers.Nature = item.Nature;
                    ObjUsers.Subject = item.Subject;
                    ObjUsers.Company = item.Company;
                    ObjUsers.WhoImpacted = item.WhoImpacted;
                    ObjUsers.Brief = item.Brief;
                    ObjUsers.Created_Date = item.Created_Date;
                    ObjUsers.Name = item.Name;
                    ObjUsers.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Email, EncryptionIV);
                    ObjUsers.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Mobile, EncryptionIV);
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }




                return View(ObjBooking);
            }

            // return View();
        }

        [HttpPost]
        public ActionResult PortsGMSStakHolderDashbord(PortsGmsStakHolderDashbord pm, string submit)
        {
            if (Session["PortsGMSUser"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSUser"].ToString());
            DateTime FromDate = pm.FromDate != null ? DateTime.Parse(pm.FromDate.ToString()).Date : System.DateTime.Now;
            DateTime ToDate = pm.ToDate != null ? DateTime.Parse(pm.ToDate.ToString()).Date : System.DateTime.Now;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {

                //BookingData = rdb.PortsGMSGrievanceBookings.ToList();
                var lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings.Where(x => x.RegistrationId == RegistrationId && x.Created_Date >= FromDate && x.Created_Date <= ToDate) on registration.Id equals bookings.RegistrationId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.Status, registration.Name, registration.Email, registration.Mobile }).OrderByDescending(p => p.Created_Date).ToList();
                PortsGmsStakHolderDashbord ObjBooking = new PortsGmsStakHolderDashbord();
                if (pm.BookingStatus != null)
                {
                    lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings.Where(x => x.RegistrationId == RegistrationId && x.Created_Date >= FromDate && x.Created_Date <= ToDate && x.Status == pm.BookingStatus) on registration.Id equals bookings.RegistrationId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.Status, registration.Name, registration.Email, registration.Mobile }).OrderByDescending(p => p.Created_Date).ToList();
                }
                if (pm.BookingStatus != null && submit == "Reset")
                {
                    lists = (from bookings in rdb.PortsGMSGrievanceBookings
                             join registration in rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId) on bookings.RegistrationId equals registration.Id
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.Status, registration.Name, registration.Email, registration.Mobile }).OrderByDescending(p => p.Created_Date).ToList();

                    ObjBooking.FromDate = null;
                    ObjBooking.ToDate = null;
                    ObjBooking.Status = null;
                    foreach (var item in lists)
                    {
                        PortsGmsStakHolderDashbord ObjUsers = new PortsGmsStakHolderDashbord();
                        ObjUsers.Id = item.Id;
                        ObjUsers.Location = item.Location;
                        ObjUsers.Nature = item.Nature;
                        ObjUsers.Subject = item.Subject;
                        ObjUsers.Company = item.Company;
                        ObjUsers.WhoImpacted = item.WhoImpacted;
                        ObjUsers.Brief = item.Brief;
                        ObjUsers.Created_Date = item.Created_Date;
                        ObjUsers.Name = item.Name;
                        ObjUsers.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Email, EncryptionIV);
                        ObjUsers.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Mobile, EncryptionIV);
                        ObjUsers.Status = item.Status;
                        ObjBooking.AllBookings.Add(ObjUsers);
                    }
                }
                else
                {
                    ObjBooking.FromDate = pm.FromDate;
                    ObjBooking.ToDate = pm.ToDate;
                    ObjBooking.BookingStatus = pm.BookingStatus;

                    foreach (var item in lists)
                    {
                        PortsGmsStakHolderDashbord ObjUsers = new PortsGmsStakHolderDashbord();
                        ObjUsers.Id = item.Id;
                        ObjUsers.Location = item.Location;
                        ObjUsers.Nature = item.Nature;
                        ObjUsers.Subject = item.Subject;
                        ObjUsers.Company = item.Company;
                        ObjUsers.WhoImpacted = item.WhoImpacted;
                        ObjUsers.Brief = item.Brief;
                        ObjUsers.Created_Date = item.Created_Date;
                        ObjUsers.Name = item.Name;
                        ObjUsers.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Email, EncryptionIV);
                        ObjUsers.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Mobile, EncryptionIV);
                        ObjUsers.Status = item.Status;
                        ObjBooking.AllBookings.Add(ObjUsers);
                    }
                }
                return View(ObjBooking);
            }
        }
        [HttpGet]
        public ActionResult PortsGMSLevel0Dashbord()
        {
            if (Session["PortsGMSLevel0User"] == null)
            {
                //return Redirect("/Grievance");
            }
            if (Session["PortsGMSLevel0User"] != null)
            {
                Guid RegistrationId = new Guid(Session["PortsGMSLevel0User"].ToString());
            }
            //new Guid("0A77F427-845E-459A-BE9C-4B8FD4E0814A"); 
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                var lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.Status, registration.Name, registration.Email, registration.Mobile }).Where(x => (x.UserType == "Level 0" && x.Status == PortsGMSTemplates.GMSFlags.BookingOpen) || x.Status == PortsGMSTemplates.GMSFlags.BookingClosed || x.Status == PortsGMSTemplates.GMSFlags.BookingOnBehalf || (x.Status == PortsGMSTemplates.GMSFlags.BookingReOpen && x.UserType == "Level 0") || x.Status == PortsGMSTemplates.GMSFlags.Response).OrderByDescending(x => x.Created_Date).ToList();
                //var lists = (from registration in rdb.PortsGms_Registrations
                //             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                //             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.Status, registration.Name, registration.Email, registration.Mobile }).Where(x => ( x.Status == PortsGMSTemplates.GMSFlags.BookingOpen) || x.Status == PortsGMSTemplates.GMSFlags.BookingClosed || x.Status == PortsGMSTemplates.GMSFlags.BookingOnBehalf || (x.Status == PortsGMSTemplates.GMSFlags.BookingReOpen) || x.Status == PortsGMSTemplates.GMSFlags.Response).OrderByDescending(x => x.Created_Date).ToList();



                PortsGmsLevel0Dashbord ObjBooking = new PortsGmsLevel0Dashbord();
                foreach (var item in lists)
                {
                    PortsGmsLevel0Dashbord ObjUsers = new PortsGmsLevel0Dashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.Location = item.Location;
                    ObjUsers.Nature = item.Nature;
                    ObjUsers.Subject = item.Subject;
                    ObjUsers.Company = item.Company;
                    ObjUsers.WhoImpacted = item.WhoImpacted;
                    ObjUsers.Brief = item.Brief;
                    ObjUsers.Created_Date = item.Created_Date;
                    ObjUsers.Name = item.Name;
                    ObjUsers.Email = item.Email;
                    ObjUsers.Mobile = item.Mobile;
                    ObjUsers.UserType = item.UserType;
                    ObjUsers.LevelInfo = "";
                    ObjUsers.Response = "";
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }
                return View(ObjBooking);
            }



            // return View();
        }



        [HttpPost]
        public ActionResult PortsGMSLevel0Dashbord(PortsGmsLevel0Dashbord pm, string submit)
        {
            if (Session["PortsGMSLevel0User"] == null)
            {
                return Redirect("/Grievance");
            }
            if (Session["PortsGMSLevel0User"] != null)
            {
                Guid RegistrationId = new Guid(Session["PortsGMSLevel0User"].ToString());
            }
            DateTime FromDate = pm.FromDate != null ? DateTime.Parse(pm.FromDate.ToString()).Date : System.DateTime.Now;
            DateTime ToDate = pm.ToDate != null ? DateTime.Parse(pm.ToDate.ToString()).Date : System.DateTime.Now;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                //BookingData = rdb.PortsGMSGrievanceBookings.ToList();
                //var lists = (from registration in rdb.PortsGms_Registrations
                //             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                //             join assign in rdb.PortsGMSBookingAssignments.Where(x => x.RegistrationId == RegistrationId && x.CreatedDate >= FromDate && x.CreatedDate <= ToDate) on bookings.Id equals assign.GrievanceBookingId
                //             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, registration.Name, registration.Email, registration.Mobile, assign.LevelInfo, assign.Response, registration.Gender }).ToList();



                var lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.Status, registration.Name, registration.Email, registration.Mobile, }).Where(x => (x.UserType == "Level 0" && x.Status == PortsGMSTemplates.GMSFlags.BookingOpen) || x.Status == PortsGMSTemplates.GMSFlags.BookingClosed || x.Status == PortsGMSTemplates.GMSFlags.BookingOnBehalf || (x.Status == PortsGMSTemplates.GMSFlags.BookingReOpen && x.UserType == "Level 0") || x.Status == PortsGMSTemplates.GMSFlags.Response && x.Created_Date.GetValueOrDefault().Date >= FromDate.Date && x.Created_Date.GetValueOrDefault().Date <= ToDate.Date && x.Status == pm.Status).OrderByDescending(p => p.Created_Date).ToList();
                if (pm.Status != null && submit != "Reset")
                {
                    if (pm.Status == PortsGMSTemplates.GMSFlags.BookingOpen)
                    {
                        lists = (from registration in rdb.PortsGms_Registrations
                                 join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                                 select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.Status, registration.Name, registration.Email, registration.Mobile, }).Where(x => x.UserType == "Level 0" && x.Status == PortsGMSTemplates.GMSFlags.BookingOpen && x.Created_Date.GetValueOrDefault().Date >= FromDate.Date && x.Created_Date.GetValueOrDefault().Date <= ToDate.Date && x.Status == pm.Status).OrderByDescending(p => p.Created_Date).ToList();
                    }
                    else if (pm.Status == PortsGMSTemplates.GMSFlags.BookingReOpen)
                    {
                        lists = (from registration in rdb.PortsGms_Registrations
                                 join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                                 select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.Status, registration.Name, registration.Email, registration.Mobile, }).Where(x => x.UserType == "Level 0" && x.Status == PortsGMSTemplates.GMSFlags.BookingReOpen && x.Created_Date.GetValueOrDefault().Date >= FromDate.Date && x.Created_Date.GetValueOrDefault().Date <= ToDate.Date && x.Status == pm.Status).OrderByDescending(p => p.Created_Date).ToList();
                    }
                    else if (pm.Status == PortsGMSTemplates.GMSFlags.BookingClosed)
                    {
                        lists = (from registration in rdb.PortsGms_Registrations
                                 join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                                 select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.Status, registration.Name, registration.Email, registration.Mobile, }).Where(x => x.Status == PortsGMSTemplates.GMSFlags.BookingClosed && x.Created_Date.GetValueOrDefault().Date >= FromDate.Date && x.Created_Date.GetValueOrDefault().Date <= ToDate.Date && x.Status == pm.Status).OrderByDescending(p => p.Created_Date).ToList();
                    }
                    else if (pm.Status == PortsGMSTemplates.GMSFlags.BookingOnBehalf)
                    {
                        lists = (from registration in rdb.PortsGms_Registrations
                                 join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                                 select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.Status, registration.Name, registration.Email, registration.Mobile, }).Where(x => x.Status == PortsGMSTemplates.GMSFlags.BookingOnBehalf && x.Created_Date.GetValueOrDefault().Date >= FromDate.Date && x.Created_Date.GetValueOrDefault().Date <= ToDate.Date && x.Status == pm.Status).OrderByDescending(p => p.Created_Date).ToList();
                    }
                    else if (pm.Status == PortsGMSTemplates.GMSFlags.Response)
                    {
                        lists = (from registration in rdb.PortsGms_Registrations
                                 join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                                 select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.Status, registration.Name, registration.Email, registration.Mobile, }).Where(x => x.Status == PortsGMSTemplates.GMSFlags.Response && x.Created_Date.GetValueOrDefault().Date >= FromDate.Date && x.Created_Date.GetValueOrDefault().Date <= ToDate.Date && x.Status == pm.Status).OrderByDescending(p => p.Created_Date).ToList();
                    }
                    //else
                    //{
                    //    lists = (from registration in rdb.PortsGms_Registrations
                    //             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                    //             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.Status, registration.Name, registration.Email, registration.Mobile, }).Where(x => x.Status == PortsGMSTemplates.GMSFlags.BookingClosed || x.Status == PortsGMSTemplates.GMSFlags.BookingOnBehalf || x.Status == PortsGMSTemplates.GMSFlags.Response && x.Created_Date >= FromDate && x.Created_Date <= ToDate && x.Status == pm.Status).ToList();
                    //}

                }
                PortsGmsLevel0Dashbord ObjBooking = new PortsGmsLevel0Dashbord();
                if (submit == "Reset")
                {
                    ObjBooking.FromDate = null;
                    ObjBooking.ToDate = null;
                    ObjBooking.Status = null;
                }
                else
                {
                    ObjBooking.FromDate = pm.FromDate;
                    ObjBooking.ToDate = pm.ToDate;
                    ObjBooking.Status = pm.Status;
                }


                foreach (var item in lists)
                {
                    PortsGmsLevel0Dashbord ObjUsers = new PortsGmsLevel0Dashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.Location = item.Location;
                    ObjUsers.Nature = item.Nature;
                    ObjUsers.Subject = item.Subject;
                    ObjUsers.Company = item.Company;
                    ObjUsers.WhoImpacted = item.WhoImpacted;
                    ObjUsers.Brief = item.Brief;
                    ObjUsers.Created_Date = item.Created_Date;
                    ObjUsers.Name = item.Name;
                    ObjUsers.Email = item.Email;
                    ObjUsers.Mobile = item.Mobile;
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }
                return this.View(ObjBooking);
            }
            // return View();
        }



        [HttpGet]
        public ActionResult PortsGMSLevel0DashbordNew()
        {
            if (Session["PortsGMSLevel0User"] == null)
            {
                return Redirect("/Grievance");
            }
            if (Session["PortsGMSLevel0User"] != null)
            {
                Guid RegistrationId = new Guid(Session["PortsGMSLevel0User"].ToString());
            }
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                var lists = rdb.PortsGMSGrievanceBookings.ToList();
                PortsGmsLevel0Dashbord ObjBooking = new PortsGmsLevel0Dashbord();
                ObjBooking.TotalCount = lists.Count();
                foreach (var item in lists)
                {
                    PortsGmsLevel0Dashbord ObjUsers = new PortsGmsLevel0Dashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.Location = item.Location;
                    ObjUsers.Nature = item.Nature;
                    ObjUsers.Subject = item.Subject;
                    ObjUsers.Company = item.Company;
                    ObjUsers.WhoImpacted = item.WhoImpacted;
                    ObjUsers.Brief = item.Brief;
                    ObjUsers.Created_Date = item.Created_Date;
                    //ObjUsers.Name = item.Name;
                    //ObjUsers.Email = item.Email;
                    //ObjUsers.Mobile = item.Mobile;
                    ObjUsers.UserType = item.UserType;
                    ObjUsers.LevelInfo = "";
                    ObjUsers.Response = "";
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }
                return View(ObjBooking);
            }
        }

        [HttpPost]
        public ActionResult PortsGMSLevel0DashbordNew(PortsGmsLevel0Dashbord pm)
        {
            if (Session["PortsGMSLevel0User"] == null)
            {
                return Redirect("/Grievance");
            }
            if (Session["PortsGMSLevel0User"] != null)
            {
                Guid RegistrationId = new Guid(Session["PortsGMSLevel0User"].ToString());
            }

            int TotalCount = 0;
            int OpenCount = 0;
            int CloseCount = 0;
            int ResponseCount = 0;
            int OnBehalfCount = 0;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                var lists = rdb.PortsGMSGrievanceBookings.AsQueryable(); ;
                if (pm.Status != null)
                {
                    if (pm.Status == PortsGMSTemplates.GMSFlags.BookingOpen)
                    {
                        lists = rdb.PortsGMSGrievanceBookings.Where(x => x.Status == PortsGMSTemplates.GMSFlags.BookingOpen);
                    }
                    else if (pm.Status == PortsGMSTemplates.GMSFlags.BookingReOpen)
                    {
                        lists = rdb.PortsGMSGrievanceBookings.Where(x => x.Status == PortsGMSTemplates.GMSFlags.BookingReOpen);
                    }
                    else if (pm.Status == PortsGMSTemplates.GMSFlags.BookingClosed)
                    {
                        lists = rdb.PortsGMSGrievanceBookings.Where(x => x.Status == PortsGMSTemplates.GMSFlags.BookingClosed);
                    }
                    else if (pm.Status == PortsGMSTemplates.GMSFlags.BookingOnBehalf)
                    {
                        lists = rdb.PortsGMSGrievanceBookings.Where(x => x.Status == PortsGMSTemplates.GMSFlags.BookingOnBehalf);
                    }
                    else if (pm.Status == PortsGMSTemplates.GMSFlags.Response)
                    {
                        lists = rdb.PortsGMSGrievanceBookings.Where(x => x.Status == PortsGMSTemplates.GMSFlags.Response);
                    }
                }

                if (pm.BusinessGroup != null)
                {
                    lists = lists.Where(x => x.BuisnessGroup == pm.BusinessGroup);
                }
                if (pm.SiteHead != null)
                {
                    lists = lists.Where(x => x.SiteHead == pm.SiteHead);
                }
                if (pm.HO != null)
                {
                    lists = lists.Where(x => x.HOType == pm.HO);
                }
                if (pm.PointMan != null)
                {
                    lists = lists.Where(x => x.PointMan == pm.PointMan);
                }
                var TotalList = lists.ToList();
                TotalCount = TotalList.Count();
                OpenCount = TotalList.Where(x => x.Status == PortsGMSTemplates.GMSFlags.BookingOpen).Count();
                CloseCount = TotalList.Where(x => x.Status == PortsGMSTemplates.GMSFlags.BookingClosed).Count();
                ResponseCount = TotalList.Where(x => x.Status == PortsGMSTemplates.GMSFlags.Response).Count();
                OnBehalfCount = TotalList.Where(x => x.Status == PortsGMSTemplates.GMSFlags.BookingOnBehalf).Count();
                PortsGmsLevel0Dashbord ObjBooking = new PortsGmsLevel0Dashbord();
                ObjBooking.Status = pm.Status;
                ObjBooking.TotalCount = TotalCount;
                ObjBooking.OpenCount = OpenCount;
                ObjBooking.CloseCount = CloseCount;
                ObjBooking.ResponseCount = ResponseCount;
                ObjBooking.OnBehalfCount = OnBehalfCount;
                foreach (var item in lists)
                {
                    PortsGmsLevel0Dashbord ObjUsers = new PortsGmsLevel0Dashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.Location = item.Location;
                    ObjUsers.Nature = item.Nature;
                    ObjUsers.Subject = item.Subject;
                    ObjUsers.Company = item.Company;
                    ObjUsers.WhoImpacted = item.WhoImpacted;
                    ObjUsers.Brief = item.Brief;
                    ObjUsers.Created_Date = item.Created_Date;
                    //ObjUsers.Name = item.Name;
                    //ObjUsers.Email = item.Email;
                    //  ObjUsers.Mobile = item.Mobile;
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }
                return this.View(ObjBooking);
            }
        }

        [HttpGet]
        public ActionResult PortsGMSLevel0Reply()
        {
            if (Session["PortsGMSLevel0User"] == null)
            {
                return Redirect("/Grievance");
            }

            Guid RegistrationId = new Guid(Session["PortsGMSLevel0User"].ToString());
            Guid GrievanceId = new Guid(Request.QueryString["id"].ToString());
            PortsGMSDataContext rdb = new PortsGMSDataContext();
            PortsGmsLevel0Dashbord ObjBooking = new PortsGmsLevel0Dashbord();
            var Grievance = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId).FirstOrDefault();
            var StakHolder = rdb.PortsGms_Registrations.Where(x => x.Id == Grievance.RegistrationId).FirstOrDefault();

            if (Grievance.Status == PortsGMSTemplates.GMSFlags.BookingOnBehalf)
            {
                var OnbehalfStakHolder = rdb.PortsGMSGrievanceBookingOnBehalfs.Where(x => x.BookingId == GrievanceId).FirstOrDefault();
            }
            var Level0User = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
            // var level0reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level0" && x.AssignedLevel == "0" && x.Response == "Completed").OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            var level0reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level0" && x.UserType == "Level 0" && x.Response == "Completed").OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            var level1reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level1" && x.Response == "Completed").OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            var level2reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level2" && x.Response == "Completed").OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            var level3reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level3" && x.Response == "Completed").OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            int submitToStakeholderRecordCount = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId && x.UserType == PortsGMSTemplates.UserType.Stakeholder && x.AssignedLevel == "stakeholder" && x.Status == PortsGMSTemplates.GMSFlags.Response).Count();
            ViewBag.SubmitToStakeholderRecordCount = submitToStakeholderRecordCount;
            //var Level0Response = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level0" && x.AssignedLevel == "0").OrderByDescending(x => x.CreatedDate).FirstOrDefault();

            var Level0Response = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level0").OrderByDescending(x => x.CreatedDate).FirstOrDefault();

            //var AssignedLevel0Response = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.AssignedLevel == "0" && x.Status == PortsGMSTemplates.GMSFlags.BookingOpen).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            var AssignedLevel0Response = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.AssignedLevel == "0" && x.Status == null).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (Grievance.Status == PortsGMSTemplates.GMSFlags.BookingOnBehalf)
            {
                var OnbehalfStakHolder = rdb.PortsGMSGrievanceBookingOnBehalfs.Where(x => x.BookingId == GrievanceId).FirstOrDefault();
                ObjBooking.Name = OnbehalfStakHolder.Name;
                ObjBooking.Email = OnbehalfStakHolder.Email;
                ObjBooking.Mobile = OnbehalfStakHolder.Mobile;
            }
            else
            {
                if (StakHolder != null)
                {
                    ObjBooking.Name = StakHolder.Name;
                    ObjBooking.DOB = StakHolder.DOB;
                    ObjBooking.Gender = StakHolder.Gender;
                    //ObjBooking.Email = StakHolder.Email;
                    //ObjBooking.Mobile = StakHolder.Mobile;
                    ObjBooking.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, StakHolder.Email, EncryptionIV);
                    ObjBooking.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, StakHolder.Mobile, EncryptionIV);
                }

            }
            if (StakHolder != null)
            {
                ObjBooking.Address = StakHolder.Address;
            }
            if (Grievance != null)
            {
                ObjBooking.Id = Grievance.Id;
                ObjBooking.Location = Grievance.Location;
                ObjBooking.Subject = Grievance.Subject;
                ObjBooking.Brief = Grievance.Brief;
                ObjBooking.WhoImpacted = Grievance.WhoImpacted;
                ObjBooking.Company = Grievance.Company;
                ObjBooking.Status = Grievance.Status;
                ObjBooking.UserType = Grievance.UserType;
                ObjBooking.Remarks = Grievance.StakeholderRemarks;
                ViewBag.AssignedLevel = Grievance.AssignedLevel;
            }
            if (Level0User != null)
            {
                ObjBooking.Department = Level0User.Department;
                ObjBooking.Level0UserName = Level0User.Name;

            }


            if (level0reply != null && level1reply != null && level2reply != null && level3reply != null)
            {
                ObjBooking.Level0UserName = level0reply.Username;
                ObjBooking.Level1UserName = level1reply.Username;
                ObjBooking.Level2UserName = level2reply.Username;
                ObjBooking.Level3UserName = level3reply.Username;
                ObjBooking.Level0Comment = level0reply.Comment;
                ObjBooking.Level1Comment = level1reply.Comment;
                ObjBooking.Level2Comment = level2reply.Comment;
                ObjBooking.Level3Comment = level3reply.Comment;
                ObjBooking.BusinessGroup = level0reply.BusinessGroup;
                ObjBooking.SiteHead = level1reply.SiteHead;
                ObjBooking.HO = level1reply.HOType;
                ObjBooking.PointMan = level2reply.PointMan;
                ObjBooking.FinalComment = Grievance.FinalComment;
                ObjBooking.Remarks = Grievance.StakeholderRemarks;
            }
            //response
            if (ObjBooking.Status == PortsGMSTemplates.GMSFlags.Response && ViewBag.SubmitToStakeholderRecordCount > 0 &&
                Level0Response != null && level1reply != null && level2reply != null && level3reply != null)
            {
                ObjBooking.Level0UserName = Level0Response.Username;
                ObjBooking.Level1UserName = level1reply.Username;
                ObjBooking.Level2UserName = level2reply.Username;
                ObjBooking.Level3UserName = level3reply.Username;
                ObjBooking.Level0Comment = Level0Response.Comment;
                ObjBooking.Level1Comment = level1reply.Comment;
                ObjBooking.Level2Comment = level2reply.Comment;
                ObjBooking.Level3Comment = level3reply.Comment;
                ObjBooking.BusinessGroup = Level0Response.BusinessGroup;
                ObjBooking.SiteHead = level1reply.SiteHead;
                ObjBooking.HO = level1reply.HOType;
                ObjBooking.PointMan = level2reply.PointMan;
                ObjBooking.FinalComment = Grievance.FinalComment;
                ObjBooking.Remarks = Grievance.StakeholderRemarks;
            }


            if (Grievance != null && ObjBooking.Status != null)
            {
                if (ObjBooking.Status == PortsGMSTemplates.GMSFlags.BookingOpen && (Grievance.AssignedLevel == "1" || Grievance.AssignedLevel == "2" || Grievance.AssignedLevel == "3"))
                {
                    ObjBooking.Level0UserName = AssignedLevel0Response.Username;
                    ObjBooking.Level0Comment = AssignedLevel0Response.Comment;
                }
            }


            if (ObjBooking.Status == PortsGMSTemplates.GMSFlags.BookingOpen && Grievance.AssignedLevel == "Re-assigned request 0")
            {
                ObjBooking.Level0UserName = level0reply.Username;
                ObjBooking.Level1UserName = level1reply.Username;
                ObjBooking.Level2UserName = level2reply.Username;
                ObjBooking.Level3UserName = level3reply.Username;
                ObjBooking.Level0Comment = level0reply.Comment;
                ObjBooking.Level1Comment = level1reply.Comment;
                ObjBooking.Level2Comment = level2reply.Comment;
                ObjBooking.Level3Comment = level3reply.Comment;
                ObjBooking.BusinessGroup = level0reply.BusinessGroup;
                ObjBooking.SiteHead = level1reply.SiteHead;
                ObjBooking.HO = level1reply.HOType;
                ObjBooking.PointMan = level2reply.PointMan;



            }
            ViewBag.AssignedState = Grievance.AssignedState;
            //var ReopenLevel0Records = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.AssignedState == "Assigned" && x.LevelInfo == "level0").OrderBy(x => x.CreatedDate).ToList();
            //if (ReopenLevel0Records != null && ReopenLevel0Records.Count > 0)
            //{
            //    foreach (var item in ReopenLevel0Records)
            //    {
            //        Level0RecordsModel obj = new Level0RecordsModel();
            //        obj.Comment = item.Comment;
            //        obj.BusinessGroup = item.BusinessGroup;
            //        obj.Created_Date = item.CreatedDate;
            //        obj.UserName = level0reply.Username;
            //        ObjBooking.Level0Records.Add(obj);
            //    }
            //}
            var StakeholderFinalComment = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level0" && x.UserType == "stakeholder").OrderBy(x => x.CreatedDate).ToList();
            if (StakeholderFinalComment != null && StakeholderFinalComment.Count > 0)
            {
                foreach (var item in StakeholderFinalComment)
                {
                    StakeholderModel obj = new StakeholderModel();
                    obj.FinalComment = item.Comment;
                    obj.Created_Date = item.CreatedDate;
                    var username = rdb.PortsGms_Registrations.Where(x => x.Id == item.RegistrationId).Select(x => x.Name).FirstOrDefault();
                    obj.UserName = username;
                    ObjBooking.StakeholderFinalComment.Add(obj);
                }
            }
            return this.View(ObjBooking);
        }



        [HttpPost]
        public ActionResult PortsGMSLevel0Reply(PortsGmsLevel0Dashbord pm)
        {
            if (Session["PortsGMSLevel0User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel0User"].ToString());
            Guid GrievanceId = pm.Id;
            PortsGMSGrievanceBooking grievencebooking = new PortsGMSGrievanceBooking();
            PortsGMSBookingAssignment bookingAssignment = new PortsGMSBookingAssignment();
            PortsGms_Registration Level1User = new PortsGms_Registration();
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                string msg = "";
                try
                {



                    var Username = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).Select(x => x.Name).FirstOrDefault();
                    // update the grivence booking record
                    var GrievanceDetails = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId).FirstOrDefault();
                    GrievanceDetails.UserType = "Level 1";
                    GrievanceDetails.AssignedLevel = "1";
                    GrievanceDetails.AssignedState = "Assigned";
                    GrievanceDetails.BuisnessGroup = pm.BusinessGroup;
                    GrievanceDetails.Modified_Date = System.DateTime.Now;
                    rdb.SubmitChanges();



                    //update the  existing booking assignment record
                    var existingBookingAssignment = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == GrievanceId && x.Response == PortsGMSTemplates.GMSFlags.RePendings).FirstOrDefault();
                    if (existingBookingAssignment != null)
                    {
                        existingBookingAssignment.Comment = pm.Comment;
                        existingBookingAssignment.BusinessGroup = pm.BusinessGroup;
                        existingBookingAssignment.Response = PortsGMSTemplates.GMSFlags.Completed;
                        existingBookingAssignment.ModifiedDate = System.DateTime.Now;
                        existingBookingAssignment.Username = Username;
                        existingBookingAssignment.LevelInfo = "level0";
                        existingBookingAssignment.UserType = "Level 0";
                        bookingAssignment.Status = PortsGMSTemplates.GrievanceStatus.Level0;
                        rdb.SubmitChanges();
                    }


                    // creating new booking assignment for 
                    bookingAssignment.Id = Guid.NewGuid();
                    bookingAssignment.GrievanceBookingId = GrievanceId;
                    bookingAssignment.RegistrationId = existingBookingAssignment.RegistrationId;
                    bookingAssignment.LevelInfo = "level1";
                    bookingAssignment.UserType = "Level 1";
                    bookingAssignment.Response = PortsGMSTemplates.GMSFlags.RePendings;
                    bookingAssignment.Status = PortsGMSTemplates.GrievanceStatus.Level1;
                    bookingAssignment.Department = existingBookingAssignment.Department;
                    bookingAssignment.AssignedLevel = "1";
                    bookingAssignment.AssignedState = "Assigned";



                    bookingAssignment.CreatedDate = System.DateTime.Now;
                    rdb.PortsGMSBookingAssignments.InsertOnSubmit(bookingAssignment);
                    rdb.SubmitChanges();
                    msg = "Grievance is successfully assigned to Level 1";
                    Session["SuccessMsg0"] = msg;
                }
                catch (Exception ex)
                {
                    msg = "Somthing went wrong please try again after some time";
                    Session["ErrorMsg0"] = msg;
                    Console.WriteLine(ex);
                }
            }
            var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel0Dashbord);



            return Redirect(page.Url());
        }



        [HttpPost]
        public JsonResult PortsGMSLevel0SubmitToStackholder(PortsGmsLevel0Dashbord pm)
        {
            if (Session["PortsGMSLevel0User"] == null)
            {
                var item = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLogin);
                return Json(item.Url());
            }

            Guid RegistrationId = new Guid(Session["PortsGMSLevel0User"].ToString());
            Guid GrievanceId = pm.Id;
            PortsGMSGrievanceBooking grievencebooking = new PortsGMSGrievanceBooking();
            PortsGMSBookingAssignment bookingAssignment = new PortsGMSBookingAssignment();
            PortsGms_Registration Level1User = new PortsGms_Registration();
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                string msg = "";
                var Username = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).Select(x => x.Name).FirstOrDefault();
                // update the grivence booking record
                var GrievanceDetails = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId && x.UserType == "Level 0").FirstOrDefault();
                GrievanceDetails.UserType = PortsGMSTemplates.UserType.Stakeholder;
                GrievanceDetails.AssignedLevel = "Stakeholder";
                GrievanceDetails.Status = PortsGMSTemplates.GMSFlags.Response;
                GrievanceDetails.AssignedState = "Responsed";
                GrievanceDetails.FinalComment = pm.FinalComment;
                rdb.SubmitChanges();



                //update the last existing booking assignment record of level 0



                if (GrievanceDetails.AssignedLevel == "Stakeholder")
                {
                    var Level0Record = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == GrievanceId && x.AssignedLevel == "0" && x.UserType == "Level 0" && x.Response == PortsGMSTemplates.GMSFlags.RePendings).FirstOrDefault();
                    if (Level0Record != null)
                    {
                        Level0Record.Comment = pm.FinalComment;
                        Level0Record.Response = PortsGMSTemplates.GMSFlags.Completed;
                        Level0Record.ModifiedDate = System.DateTime.Now;
                        Level0Record.UserType = PortsGMSTemplates.UserType.Stakeholder;
                        // Level0Record.AssignedLevel = "Stakeholder";
                        Level0Record.AssignedState = "Responsed";
                        Level0Record.Username = Username;
                        rdb.SubmitChanges();
                        msg = "Grievance is successfully submitted to Stakeholder";
                        Session["SuccessMsg0"] = msg;
                    }



                    var lastlevel0Finalrecord = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == GrievanceId && x.AssignedLevel == "Re-assigned 0" && x.UserType == "Level 0" && x.Response == PortsGMSTemplates.GMSFlags.RePendings).FirstOrDefault();
                    if (lastlevel0Finalrecord != null)
                    {
                        lastlevel0Finalrecord.Comment = pm.FinalComment;
                        lastlevel0Finalrecord.Response = PortsGMSTemplates.GMSFlags.Completed;
                        lastlevel0Finalrecord.ModifiedDate = System.DateTime.Now;
                        lastlevel0Finalrecord.UserType = PortsGMSTemplates.UserType.Stakeholder;
                        //lastlevel0Finalrecord.AssignedLevel = "Stakeholder";
                        lastlevel0Finalrecord.AssignedState = "Responsed";
                        lastlevel0Finalrecord.Username = Username;
                        rdb.SubmitChanges();
                        msg = "Grievance is successfully submitted to Stakeholder";
                        Session["SuccessMsg0"] = msg;
                    }
                }
            }
            var item1 = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel0Dashbord);
            return Json(item1.Url());
        }


        [HttpGet]
        public ActionResult PortsGMSLevel1Dashbord()
        {
            if (Session["PortsGMSLevel1User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel1User"].ToString());
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                var level1UserDetails = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
                var lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.BuisnessGroup, bookings.Status, registration.Name, registration.Email, registration.Mobile }).Where(x => x.UserType == "Level 1" && x.BuisnessGroup == level1UserDetails.BusinessGroup).OrderByDescending(x => x.Created_Date).ToList();

                PortsGmsLevel1Dashbord ObjBooking = new PortsGmsLevel1Dashbord();
                foreach (var item in lists)
                {
                    PortsGmsLevel1Dashbord ObjUsers = new PortsGmsLevel1Dashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.Location = item.Location;
                    ObjUsers.Nature = item.Nature;
                    ObjUsers.Subject = item.Subject;
                    ObjUsers.Company = item.Company;
                    ObjUsers.WhoImpacted = item.WhoImpacted;
                    ObjUsers.Brief = item.Brief;
                    ObjUsers.Created_Date = item.Created_Date;
                    ObjUsers.Name = item.Name;
                    ObjUsers.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Email, EncryptionIV);
                    ObjUsers.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Mobile, EncryptionIV);
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }
                return View(ObjBooking);
            }

            // return View();
        }

        [HttpPost]
        public ActionResult PortsGMSLevel1Dashbord(PortsGmsLevel1Dashbord pm, string submit)
        {
            if (Session["PortsGMSLevel1User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel1User"].ToString());
            DateTime FromDate = pm.FromDate != null ? DateTime.Parse(pm.FromDate.ToString()).Date : System.DateTime.Now;
            DateTime ToDate = pm.ToDate != null ? DateTime.Parse(pm.ToDate.ToString()).Date : System.DateTime.Now;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                //BookingData = rdb.PortsGMSGrievanceBookings.ToList();
                //var lists = (from registration in rdb.PortsGms_Registrations
                //             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                //             join assign in rdb.PortsGMSBookingAssignments.Where(x => x.RegistrationId == RegistrationId && x.CreatedDate >= FromDate && x.CreatedDate <= ToDate) on bookings.Id equals assign.GrievanceBookingId
                //             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, registration.Name, registration.Email, registration.Mobile, assign.LevelInfo, assign.Response }).ToList();

                var level1UserDetails = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
                var lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.BuisnessGroup, bookings.Status, registration.Name, registration.Email, registration.Mobile }).Where(x => x.UserType == "Level 1" && x.BuisnessGroup == level1UserDetails.BusinessGroup && x.Created_Date.GetValueOrDefault().Date >= FromDate.Date && x.Created_Date.GetValueOrDefault().Date <= ToDate.Date).OrderByDescending(p => p.Created_Date).ToList();

                PortsGmsLevel1Dashbord ObjBooking = new PortsGmsLevel1Dashbord();
                if (submit == "Reset")
                {
                    lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.BuisnessGroup, bookings.Status, registration.Name, registration.Email, registration.Mobile }).Where(x => x.UserType == "Level 1" && x.BuisnessGroup == level1UserDetails.BusinessGroup).OrderByDescending(x => x.Created_Date).OrderByDescending(p => p.Created_Date).ToList();
                    ObjBooking.FromDate = null;
                    ObjBooking.ToDate = null;
                    ObjBooking.Status = null;
                }
                else
                {
                    ObjBooking.FromDate = pm.FromDate;
                    ObjBooking.ToDate = pm.ToDate;
                    ObjBooking.Status = pm.Status;
                }

                foreach (var item in lists)
                {
                    PortsGmsLevel1Dashbord ObjUsers = new PortsGmsLevel1Dashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.Location = item.Location;
                    ObjUsers.Nature = item.Nature;
                    ObjUsers.Subject = item.Subject;
                    ObjUsers.Company = item.Company;
                    ObjUsers.WhoImpacted = item.WhoImpacted;
                    ObjUsers.Brief = item.Brief;
                    ObjUsers.Created_Date = item.Created_Date;
                    ObjUsers.Name = item.Name;
                    ObjUsers.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Email, EncryptionIV);
                    ObjUsers.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Mobile, EncryptionIV);
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }
                return this.View(ObjBooking);
            }

            // return View();
        }


        [HttpGet]
        public ActionResult PortsGMSLevel1Reply()
        {
            if (Session["PortsGMSLevel1User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel1User"].ToString());
            Guid GrievanceId = new Guid(Request.QueryString["id"].ToString());
            PortsGMSDataContext rdb = new PortsGMSDataContext();
            PortsGmsLevel1Dashbord ObjBooking = new PortsGmsLevel1Dashbord();
            var Grievance = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId).FirstOrDefault();
            var level0reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level0" && x.AssignedLevel == "0" && x.Response == PortsGMSTemplates.GMSFlags.Completed).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            var StakHolder = rdb.PortsGms_Registrations.Where(x => x.Id == Grievance.RegistrationId).FirstOrDefault();
            var Level1User = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
            if (StakHolder != null)
            {
                ObjBooking.Name = StakHolder.Name;
                ObjBooking.DOB = StakHolder.DOB;
                ObjBooking.Gender = StakHolder.Gender;
                ObjBooking.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, StakHolder.Email, EncryptionIV);
                ObjBooking.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, StakHolder.Mobile, EncryptionIV);
                ObjBooking.Address = StakHolder.Address;
            }
            if (Grievance != null)
            {
                ObjBooking.Id = Grievance.Id;
                ObjBooking.Location = Grievance.Location;
                ObjBooking.Department = Level1User.Department;
                ObjBooking.Subject = Grievance.Subject;
                ObjBooking.Brief = Grievance.Brief;
                ObjBooking.WhoImpacted = Grievance.WhoImpacted;
                ObjBooking.Company = Grievance.Company;
                ObjBooking.Status = Grievance.Status;
            }
            if (Level1User != null)
            {
                ObjBooking.Level1UserName = Level1User.Name;
            }
            if (level0reply != null)
            {
                ObjBooking.Level0UserName = level0reply.Username;
                ObjBooking.Level0Comment = level0reply.Comment;
                ObjBooking.BusinessGroup = level0reply.BusinessGroup;
            }
            //    var ReopenLevel0Records = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.AssignedState == "Assigned" && x.LevelInfo == "level0").OrderBy(x=>x.CreatedDate).ToList();
            //if (ReopenLevel0Records != null && ReopenLevel0Records.Count > 0)
            //{
            //    foreach (var item in ReopenLevel0Records)
            //    {
            //        Level0RecordsModel obj = new Level0RecordsModel();
            //        obj.Comment = item.Comment;
            //        obj.BusinessGroup = item.BusinessGroup;
            //        obj.Created_Date = item.CreatedDate;
            //        obj.UserName = level0reply.Username;
            //        ObjBooking.Level0Records.Add(obj);
            //    }
            //}
            return this.View(ObjBooking);
        }

        [HttpPost]
        public ActionResult PortsGMSLevel1Reply(PortsGmsLevel1Dashbord pm)
        {
            if (Session["PortsGMSLevel1User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel1User"].ToString());
            Guid GrievanceId = pm.Id;
            PortsGMSGrievanceBooking grievencebooking = new PortsGMSGrievanceBooking();
            PortsGMSBookingAssignment bookingAssignment = new PortsGMSBookingAssignment();
            PortsGms_Registration Level1User = new PortsGms_Registration();
            string msg = "";
            try
            {
                using (PortsGMSDataContext rdb = new PortsGMSDataContext())
                {

                    var Username = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).Select(x => x.Name).FirstOrDefault();
                    // update the grivence booking record
                    var GrievanceDetails = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId).FirstOrDefault();
                    GrievanceDetails.UserType = "Level 2";
                    GrievanceDetails.AssignedLevel = "2";
                    GrievanceDetails.SiteHead = pm.SiteHead;
                    GrievanceDetails.HOType = pm.HO;
                    GrievanceDetails.Modified_Date = System.DateTime.Now;
                    rdb.SubmitChanges();

                    //update the  existing booking assignment record
                    var level0Records = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == GrievanceId && x.LevelInfo == "level0" && x.Response == PortsGMSTemplates.GMSFlags.Completed).FirstOrDefault();
                    var existingBookingAssignment = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == GrievanceId && x.LevelInfo == "level1" && x.Response == PortsGMSTemplates.GMSFlags.RePendings).FirstOrDefault();
                    if (existingBookingAssignment != null)
                    {
                        existingBookingAssignment.Comment = pm.Comment;
                        existingBookingAssignment.BusinessGroup = level0Records.BusinessGroup;
                        existingBookingAssignment.SiteHead = pm.SiteHead;
                        existingBookingAssignment.HOType = pm.HO;
                        existingBookingAssignment.Response = PortsGMSTemplates.GMSFlags.Completed;
                        existingBookingAssignment.ModifiedDate = System.DateTime.Now;
                        existingBookingAssignment.Username = Username;
                        rdb.SubmitChanges();
                    }


                    // creating new booking assignment for 
                    bookingAssignment.Id = Guid.NewGuid();
                    bookingAssignment.GrievanceBookingId = GrievanceId;
                    bookingAssignment.RegistrationId = existingBookingAssignment.RegistrationId;
                    bookingAssignment.LevelInfo = "level2";
                    bookingAssignment.UserType = "Level 2";
                    bookingAssignment.Response = PortsGMSTemplates.GMSFlags.RePendings;
                    bookingAssignment.Status = PortsGMSTemplates.GrievanceStatus.Level2;
                    bookingAssignment.Department = existingBookingAssignment.Department;
                    bookingAssignment.AssignedLevel = "2";
                    bookingAssignment.AssignedState = "Assigned";

                    bookingAssignment.CreatedDate = System.DateTime.Now;
                    rdb.PortsGMSBookingAssignments.InsertOnSubmit(bookingAssignment);
                    rdb.SubmitChanges();
                    msg = "Grievance is successfully assigned to Level 2";
                    Session["SuccessMsg1"] = msg;

                }
            }
            catch (Exception ex)
            {
                msg = "Somthing went wrong please try again after some time";
                Session["ErrorMsg1"] = msg;
                Console.WriteLine(ex);
            }
            var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel1Dashbord);

            return Redirect(page.Url());
        }

        [HttpGet]
        public ActionResult PortsGMSLevel2Reply()
        {
            if (Session["PortsGMSLevel2User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel2User"].ToString());
            Guid GrievanceId = new Guid(Request.QueryString["id"].ToString());
            PortsGMSDataContext rdb = new PortsGMSDataContext();
            PortsGmsLevel2Dashbord ObjBooking = new PortsGmsLevel2Dashbord();

            var Grievance = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId).FirstOrDefault();
            var StakHolder = rdb.PortsGms_Registrations.Where(x => x.Id == Grievance.RegistrationId).FirstOrDefault();
            var Level2User = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
            var Assigned = rdb.PortsGMSBookingAssignments.Where(x => x.RegistrationId == RegistrationId).FirstOrDefault();
            var level0reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level0" && x.AssignedLevel == "0" && x.Response == PortsGMSTemplates.GMSFlags.Completed).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            var level1reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level1" && x.Response == PortsGMSTemplates.GMSFlags.Completed).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (StakHolder != null)
            {
                ObjBooking.Name = StakHolder.Name;
                ObjBooking.DOB = StakHolder.DOB;
                ObjBooking.Gender = StakHolder.Gender;
                ObjBooking.Email = StakHolder.Email;
                ObjBooking.Mobile = StakHolder.Mobile;
                ObjBooking.Address = StakHolder.Address;
            }
            if (StakHolder != null)
            {
                ObjBooking.Id = Grievance.Id;
                ObjBooking.Location = Grievance.Location;
                ObjBooking.Department = Level2User.Department;
                ObjBooking.Subject = Grievance.Subject;
                ObjBooking.Brief = Grievance.Brief;
                ObjBooking.WhoImpacted = Grievance.WhoImpacted;
                ObjBooking.Company = Grievance.Company;
            }
            if (level0reply != null)
            {
                ObjBooking.Level0UserName = level0reply.Username;
                ObjBooking.Level0Comment = level0reply.Comment;
                ObjBooking.BusinessGroup = level0reply.BusinessGroup;
            }
            if (level1reply != null)
            {
                ObjBooking.Level1UserName = level1reply.Username;
                ObjBooking.Level1Comment = level1reply.Comment;
                ObjBooking.SiteHead = level1reply.SiteHead;
                ObjBooking.HO = level1reply.HOType;
            }
            ObjBooking.Level2UserName = Level2User.Name != null ? Level2User.Name : null;
            ObjBooking.Assigned = Assigned != null ? true : false;


            return this.View(ObjBooking);
        }

        [HttpPost]
        public ActionResult PortsGMSLevel2Reply(PortsGmsLevel2Dashbord pm)
        {
            if (Session["PortsGMSLevel2User"] == null)
            {
                //return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel2User"].ToString());
            Guid GrievanceId = pm.Id;
            PortsGMSGrievanceBooking grievencebooking = new PortsGMSGrievanceBooking();
            PortsGMSBookingAssignment bookingAssignment = new PortsGMSBookingAssignment();
            PortsGms_Registration Level1User = new PortsGms_Registration();
            string msg = "";
            try
            {
                using (PortsGMSDataContext rdb = new PortsGMSDataContext())
                {
                    var Username = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).Select(x => x.Name).FirstOrDefault();
                    // update the grivence booking record
                    var GrievanceDetails = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId).FirstOrDefault();
                    GrievanceDetails.UserType = "Level 3";
                    GrievanceDetails.AssignedLevel = "3";
                    GrievanceDetails.PointMan = pm.PointMan;
                    GrievanceDetails.Modified_Date = System.DateTime.Now;
                    rdb.SubmitChanges();

                    //update the  existing booking assignment record
                    var level1Records = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == GrievanceId && x.LevelInfo == "level1" && x.Response == PortsGMSTemplates.GMSFlags.Completed).FirstOrDefault();
                    var existingBookingAssignment = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == GrievanceId && x.LevelInfo == "level2" && x.Response == PortsGMSTemplates.GMSFlags.RePendings).FirstOrDefault();
                    if (existingBookingAssignment != null)
                    {
                        existingBookingAssignment.Comment = pm.Comment;
                        existingBookingAssignment.BusinessGroup = level1Records.BusinessGroup;
                        existingBookingAssignment.SiteHead = level1Records.SiteHead;
                        existingBookingAssignment.HOType = level1Records.HOType;
                        existingBookingAssignment.PointMan = pm.PointMan;
                        existingBookingAssignment.Response = PortsGMSTemplates.GMSFlags.Completed;
                        existingBookingAssignment.ModifiedDate = System.DateTime.Now;
                        existingBookingAssignment.Username = Username;
                        rdb.SubmitChanges();
                    }
                    // creating new booking assignment for 
                    bookingAssignment.Id = Guid.NewGuid();
                    bookingAssignment.GrievanceBookingId = GrievanceId;
                    bookingAssignment.RegistrationId = existingBookingAssignment.RegistrationId;
                    bookingAssignment.LevelInfo = "level3";
                    bookingAssignment.UserType = "Level 3";
                    bookingAssignment.Response = PortsGMSTemplates.GMSFlags.RePendings;
                    bookingAssignment.Status = PortsGMSTemplates.GrievanceStatus.Level3;
                    bookingAssignment.Department = existingBookingAssignment.Department;
                    bookingAssignment.AssignedLevel = "3";
                    bookingAssignment.AssignedState = "Assigned";
                    bookingAssignment.CreatedDate = System.DateTime.Now;
                    rdb.PortsGMSBookingAssignments.InsertOnSubmit(bookingAssignment);
                    rdb.SubmitChanges();
                    msg = "Grievance is successfully assigned to Level 3";
                    Session["SuccessMsg2"] = msg;

                }
            }
            catch (Exception ex)
            {
                msg = "Somthing went wrong please try again after some time";
                Session["ErrorMsg2"] = msg;
                Console.WriteLine(ex);
            }
            var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel2Dashbord);

            return Redirect(page.Url());
        }


        [HttpGet]
        public ActionResult PortsGMSLevel3Reply()
        {
            if (Session["PortsGMSLevel3User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel3User"].ToString());
            Guid GrievanceId = new Guid(Request.QueryString["id"].ToString());
            PortsGMSDataContext rdb = new PortsGMSDataContext();
            PortsGmsLevel3Dashbord ObjBooking = new PortsGmsLevel3Dashbord();
            var Grievance = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId).FirstOrDefault();
            var StakHolder = rdb.PortsGms_Registrations.Where(x => x.Id == Grievance.RegistrationId).FirstOrDefault();
            var Level3User = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
            var Assigned = rdb.PortsGMSBookingAssignments.Where(x => x.RegistrationId == RegistrationId).FirstOrDefault();
            var level0reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level0" && x.AssignedLevel == "0" && x.Response == PortsGMSTemplates.GMSFlags.Completed).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            var level1reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level1" && x.Response == PortsGMSTemplates.GMSFlags.Completed).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            var level2reply = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == Grievance.Id && x.LevelInfo == "level2" && x.Response == PortsGMSTemplates.GMSFlags.Completed).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            if (StakHolder != null)
            {
                ObjBooking.Name = StakHolder.Name;
                ObjBooking.DOB = StakHolder.DOB;
                ObjBooking.Gender = StakHolder.Gender;
                ObjBooking.Email = StakHolder.Email;
                ObjBooking.Mobile = StakHolder.Mobile;
                ObjBooking.Address = StakHolder.Address;
            }
            if (StakHolder != null)
            {
                ObjBooking.Id = Grievance.Id;
                ObjBooking.Location = Grievance.Location;
                ObjBooking.Department = Level3User.Department;
                ObjBooking.Subject = Grievance.Subject;
                ObjBooking.Brief = Grievance.Brief;
                ObjBooking.WhoImpacted = Grievance.WhoImpacted;
                ObjBooking.Company = Grievance.Company;
                ViewBag.AssignedLevel = Grievance.AssignedLevel;
            }
            if (level0reply != null)
            {
                ObjBooking.Level0UserName = level0reply.Username;
                ObjBooking.Level0Comment = level0reply.Comment;
                ObjBooking.BusinessGroup = level0reply.BusinessGroup;
            }
            if (level1reply != null)
            {
                ObjBooking.Level1UserName = level1reply.Username;
                ObjBooking.Level1Comment = level1reply.Comment;
                ObjBooking.SiteHead = level1reply.SiteHead;
                ObjBooking.HO = level1reply.HOType;
            }
            if (level2reply != null)
            {
                ObjBooking.Level2UserName = level2reply.Username;
                ObjBooking.Level2Comment = level2reply.Comment;
                ObjBooking.PointMan = level2reply.PointMan;
            }

            ObjBooking.Level3UserName = Level3User.Name != null ? Level3User.Name : null;
            ObjBooking.Assigned = Assigned != null ? true : false;

            return this.View(ObjBooking);
        }

        [HttpPost]
        public ActionResult PortsGMSLevel3Reply(PortsGmsLevel3Dashbord pm)
        {
            if (Session["PortsGMSLevel3User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel3User"].ToString());
            Guid GrievanceId = pm.Id;
            PortsGMSGrievanceBooking grievencebooking = new PortsGMSGrievanceBooking();
            PortsGMSBookingAssignment bookingAssignment = new PortsGMSBookingAssignment();
            PortsGms_Registration Level3User = new PortsGms_Registration();
            string msg = "";
            try
            {
                using (PortsGMSDataContext rdb = new PortsGMSDataContext())
                {
                    var Username = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).Select(x => x.Name).FirstOrDefault();
                    // update the grivence booking record
                    var GrievanceDetails = rdb.PortsGMSGrievanceBookings.Where(x => x.Id == GrievanceId).FirstOrDefault();
                    GrievanceDetails.UserType = "Level 0";
                    GrievanceDetails.AssignedLevel = "Re-assigned 0";
                    GrievanceDetails.Modified_Date = System.DateTime.Now;
                    rdb.SubmitChanges();

                    //update the  existing booking assignment record
                    var level2Records = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == GrievanceId && x.LevelInfo == "level2" && x.Response == PortsGMSTemplates.GMSFlags.Completed).FirstOrDefault();
                    var existingBookingAssignment = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == GrievanceId && x.LevelInfo == "level3" && x.Response == PortsGMSTemplates.GMSFlags.RePendings).FirstOrDefault();
                    if (existingBookingAssignment != null)
                    {
                        existingBookingAssignment.Comment = pm.Comment;
                        existingBookingAssignment.BusinessGroup = level2Records.BusinessGroup;
                        existingBookingAssignment.SiteHead = level2Records.SiteHead;
                        existingBookingAssignment.HOType = level2Records.HOType;
                        existingBookingAssignment.PointMan = level2Records.PointMan;
                        existingBookingAssignment.Response = PortsGMSTemplates.GMSFlags.Completed;
                        existingBookingAssignment.ModifiedDate = System.DateTime.Now;
                        existingBookingAssignment.Username = Username;
                        rdb.SubmitChanges();
                    }

                    // creating new booking assignment for 
                    bookingAssignment.Id = Guid.NewGuid();
                    bookingAssignment.GrievanceBookingId = GrievanceId;
                    bookingAssignment.RegistrationId = existingBookingAssignment.RegistrationId;
                    bookingAssignment.LevelInfo = "level0";
                    bookingAssignment.UserType = "Level 0";
                    bookingAssignment.Response = PortsGMSTemplates.GMSFlags.RePendings;
                    bookingAssignment.Status = PortsGMSTemplates.GrievanceStatus.ReassignLevel0;
                    bookingAssignment.Department = existingBookingAssignment.Department;
                    bookingAssignment.BusinessGroup = level2Records.BusinessGroup;
                    bookingAssignment.SiteHead = existingBookingAssignment.SiteHead;
                    bookingAssignment.PointMan = existingBookingAssignment.PointMan;
                    bookingAssignment.HOType = existingBookingAssignment.HOType;
                    bookingAssignment.AssignedLevel = "Re-assigned 0";
                    bookingAssignment.AssignedState = "Assigned";

                    bookingAssignment.CreatedDate = System.DateTime.Now;
                    rdb.PortsGMSBookingAssignments.InsertOnSubmit(bookingAssignment);
                    rdb.SubmitChanges();
                    msg = "Grievance is successfully re-assigned to Level 0";
                    Session["SuccessMsg3"] = msg;

                }
            }
            catch (Exception ex)
            {
                msg = "Somthing went wrong please try again after some time";
                Session["ErrorMsg3"] = msg;
                Console.WriteLine(ex);
            }
            var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel3Dashbord);

            return Redirect(page.Url());
        }


        [HttpGet]
        public ActionResult PortsGMSLevel2Dashbord()
        {
            if (Session["PortsGMSLevel2User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel2User"].ToString());

            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                var level2UserDetails = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
                var lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.BuisnessGroup, bookings.SiteHead, bookings.HOType, bookings.Status, registration.Name, registration.Email, registration.Mobile }).Where(x => x.UserType == "Level 2" && x.BuisnessGroup == level2UserDetails.BusinessGroup && x.SiteHead == level2UserDetails.SiteHead && x.HOType == level2UserDetails.HO).OrderByDescending(x => x.Created_Date).ToList();


                PortsGmsLevel2Dashbord ObjBooking = new PortsGmsLevel2Dashbord();
                foreach (var item in lists)
                {
                    PortsGmsLevel2Dashbord ObjUsers = new PortsGmsLevel2Dashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.Location = item.Location;
                    ObjUsers.Nature = item.Nature;
                    ObjUsers.Subject = item.Subject;
                    ObjUsers.Company = item.Company;
                    ObjUsers.WhoImpacted = item.WhoImpacted;
                    ObjUsers.Brief = item.Brief;
                    ObjUsers.Created_Date = item.Created_Date;
                    ObjUsers.Name = item.Name;
                    ObjUsers.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Email, EncryptionIV);
                    ObjUsers.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Mobile, EncryptionIV);
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }
                return View(ObjBooking);
            }

            // return View();
        }
        [HttpPost]
        public ActionResult PortsGMSLevel2Dashbord(PortsGmsLevel2Dashbord pm, string submit)
        {
            if (Session["PortsGMSLevel2User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel2User"].ToString());
            DateTime FromDate = pm.FromDate != null ? DateTime.Parse(pm.FromDate.ToString()).Date : System.DateTime.Now;
            DateTime ToDate = pm.ToDate != null ? DateTime.Parse(pm.ToDate.ToString()).Date : System.DateTime.Now;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {

                var RegistratedUser = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();


                //BookingData = rdb.PortsGMSGrievanceBookings.ToList();
                var level2UserDetails = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
                var lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.BuisnessGroup, bookings.SiteHead, bookings.HOType, bookings.Status, registration.Name, registration.Email, registration.Mobile }).Where(x => x.UserType == "Level 2" && x.BuisnessGroup == level2UserDetails.BusinessGroup && x.SiteHead == level2UserDetails.SiteHead && x.HOType == level2UserDetails.HO && x.Created_Date.GetValueOrDefault().Date >= FromDate.Date && x.Created_Date.GetValueOrDefault().Date <= ToDate.Date).OrderByDescending(p => p.Created_Date).ToList();

                if (pm.Status != null && pm.Status == PortsGMSTemplates.GrievanceStatus.Level1 && submit != "Reset")
                {
                    (from registration in rdb.PortsGms_Registrations
                     join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                     select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.BuisnessGroup, bookings.SiteHead, bookings.HOType, bookings.Status, registration.Name, registration.Email, registration.Mobile }).Where(x => x.UserType == "Level 2" && x.BuisnessGroup == level2UserDetails.BusinessGroup && x.SiteHead == level2UserDetails.SiteHead && x.HOType == level2UserDetails.HO && x.Created_Date.GetValueOrDefault().Date >= FromDate.Date && x.Created_Date.GetValueOrDefault().Date <= ToDate.Date && x.Status == pm.Status).OrderByDescending(p => p.Created_Date).ToList();
                }



                PortsGmsLevel2Dashbord ObjBooking = new PortsGmsLevel2Dashbord();
                if (submit == "Reset")
                {
                    lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.BuisnessGroup, bookings.SiteHead, bookings.HOType, bookings.Status, registration.Name, registration.Email, registration.Mobile }).Where(x => x.UserType == "Level 2" && x.BuisnessGroup == level2UserDetails.BusinessGroup && x.SiteHead == level2UserDetails.SiteHead && x.HOType == level2UserDetails.HO).OrderByDescending(x => x.Created_Date).OrderByDescending(p => p.Created_Date).ToList();

                    ObjBooking.FromDate = null;
                    ObjBooking.ToDate = null;
                    ObjBooking.Status = null;
                }
                else
                {
                    ObjBooking.FromDate = pm.FromDate;
                    ObjBooking.ToDate = pm.ToDate;
                    ObjBooking.Status = pm.Status;
                }
                foreach (var item in lists)
                {
                    PortsGmsLevel2Dashbord ObjUsers = new PortsGmsLevel2Dashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.Location = item.Location;
                    ObjUsers.Nature = item.Nature;
                    ObjUsers.Subject = item.Subject;
                    ObjUsers.Company = item.Company;
                    ObjUsers.WhoImpacted = item.WhoImpacted;
                    ObjUsers.Brief = item.Brief;
                    ObjUsers.Created_Date = item.Created_Date;
                    ObjUsers.Name = item.Name;
                    ObjUsers.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Email, EncryptionIV);
                    ObjUsers.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Mobile, EncryptionIV);
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }
                return this.View(ObjBooking);
            }



        }

        [HttpPost]
        public ActionResult PortsGMSLevel3Dashbord(PortsGmsLevel3Dashbord pm, string submit)
        {
            if (Session["PortsGMSLevel3User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel3User"].ToString());
            DateTime FromDate = pm.FromDate != null ? DateTime.Parse(pm.FromDate.ToString()).Date : System.DateTime.Now;
            DateTime ToDate = pm.ToDate != null ? DateTime.Parse(pm.ToDate.ToString()).Date : System.DateTime.Now;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {

                var RegistratedUser = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();


                //BookingData = rdb.PortsGMSGrievanceBookings.ToList();
                //var lists = (from registration in rdb.PortsGms_Registrations
                //             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                //             join assign in rdb.PortsGMSBookingAssignments.Where(x => x.RegistrationId == RegistrationId && x.CreatedDate >= FromDate && x.CreatedDate <= ToDate) on bookings.Id equals assign.GrievanceBookingId
                //             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, registration.Name, registration.Email, registration.Mobile, assign.LevelInfo, assign.Response }).ToList();
                var level3UserDetails = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
                var lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.BuisnessGroup, bookings.SiteHead, bookings.HOType, bookings.PointMan, bookings.Status, registration.Name, registration.Email, registration.Mobile }).Where(x => x.UserType == "Level 3" && x.BuisnessGroup == level3UserDetails.BusinessGroup && x.SiteHead == level3UserDetails.SiteHead && x.HOType == level3UserDetails.HO && x.PointMan == level3UserDetails.Team && x.Created_Date.GetValueOrDefault().Date >= FromDate.Date && x.Created_Date.GetValueOrDefault().Date <= ToDate.Date).OrderByDescending(p => p.Created_Date).ToList();

                ////if (pm.Status == PortsGMSTemplates.GrievanceStatus.Level1)
                ////{
                ////    lists = (from registration in rdb.PortsGms_Registrations
                ////             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                ////             join assign in rdb.PortsGMSBookingAssignments.Where(x => x.LevelInfo == PortsGMSTemplates.UserType.Level1 && x.Department == RegistratedUser.Department && x.CreatedDate >= FromDate && x.CreatedDate <= ToDate) on bookings.Id equals assign.GrievanceBookingId
                ////             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, registration.Name, registration.Email, registration.Mobile, assign.LevelInfo, assign.Response }).ToList();
                ////}
                ////else if (pm.Status == PortsGMSTemplates.GrievanceStatus.Level2)
                ////{
                ////    lists = (from registration in rdb.PortsGms_Registrations
                ////             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                ////             join assign in rdb.PortsGMSBookingAssignments.Where(x => x.LevelInfo == PortsGMSTemplates.UserType.Level2 && x.Department == RegistratedUser.Department && x.CreatedDate >= FromDate && x.CreatedDate <= ToDate) on bookings.Id equals assign.GrievanceBookingId
                ////             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, registration.Name, registration.Email, registration.Mobile, assign.LevelInfo, assign.Response }).ToList();
                ////}

                if (pm.Status != null && submit != "Reset")
                {
                    (from registration in rdb.PortsGms_Registrations
                     join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                     select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.BuisnessGroup, bookings.SiteHead, bookings.HOType, bookings.PointMan, bookings.Status, registration.Name, registration.Email, registration.Mobile }).Where(x => x.UserType == "Level 3" && x.BuisnessGroup == level3UserDetails.BusinessGroup && x.SiteHead == level3UserDetails.SiteHead && x.HOType == level3UserDetails.HO && x.PointMan == level3UserDetails.Team && x.Created_Date.GetValueOrDefault().Date >= FromDate.Date && x.Created_Date.GetValueOrDefault().Date <= ToDate.Date && x.Status == pm.Status).OrderByDescending(p => p.Created_Date).ToList();
                }


                PortsGmsLevel3Dashbord ObjBooking = new PortsGmsLevel3Dashbord();
                if (submit == "Reset")
                {
                    lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.BuisnessGroup, bookings.SiteHead, bookings.HOType, bookings.PointMan, bookings.Status, registration.Name, registration.Email, registration.Mobile }).Where(x => x.UserType == "Level 3" && x.BuisnessGroup == level3UserDetails.BusinessGroup && x.SiteHead == level3UserDetails.SiteHead && x.HOType == level3UserDetails.HO && x.PointMan == level3UserDetails.Team).OrderByDescending(x => x.Created_Date).OrderByDescending(p => p.Created_Date).ToList();

                    ObjBooking.FromDate = null;
                    ObjBooking.ToDate = null;
                    ObjBooking.Status = null;
                }
                else
                {
                    ObjBooking.FromDate = pm.FromDate;
                    ObjBooking.ToDate = pm.ToDate;
                    ObjBooking.Status = pm.Status;
                }
                foreach (var item in lists)
                {
                    PortsGmsLevel3Dashbord ObjUsers = new PortsGmsLevel3Dashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.Location = item.Location;
                    ObjUsers.Nature = item.Nature;
                    ObjUsers.Subject = item.Subject;
                    ObjUsers.Company = item.Company;
                    ObjUsers.WhoImpacted = item.WhoImpacted;
                    ObjUsers.Brief = item.Brief;
                    ObjUsers.Created_Date = item.Created_Date;
                    ObjUsers.Name = item.Name;
                    ObjUsers.Email = item.Email;
                    ObjUsers.Mobile = item.Mobile;
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }
                return this.View(ObjBooking);
            }
        }

        [HttpGet]
        public ActionResult PortsGMSLevel3Dashbord()
        {
            if (Session["PortsGMSLevel3User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel3User"].ToString());
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                var level3UserDetails = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
                var lists = (from registration in rdb.PortsGms_Registrations
                             join bookings in rdb.PortsGMSGrievanceBookings on registration.Id equals bookings.RegistrationId
                             select new { bookings.Id, bookings.Location, bookings.Nature, bookings.Subject, bookings.Company, bookings.WhoImpacted, bookings.Brief, bookings.Created_Date, bookings.UserType, bookings.AssignedLevel, bookings.BuisnessGroup, bookings.SiteHead, bookings.HOType, bookings.PointMan, bookings.Status, registration.Name, registration.Email, registration.Mobile }).Where(x => x.UserType == "Level 3" && x.BuisnessGroup == level3UserDetails.BusinessGroup && x.SiteHead == level3UserDetails.SiteHead && x.HOType == level3UserDetails.HO && x.PointMan == level3UserDetails.Team).OrderByDescending(x => x.Created_Date).ToList();

                PortsGmsLevel3Dashbord ObjBooking = new PortsGmsLevel3Dashbord();
                foreach (var item in lists)
                {
                    PortsGmsLevel3Dashbord ObjUsers = new PortsGmsLevel3Dashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.Location = item.Location;
                    ObjUsers.Nature = item.Nature;
                    ObjUsers.Subject = item.Subject;
                    ObjUsers.Company = item.Company;
                    ObjUsers.WhoImpacted = item.WhoImpacted;
                    ObjUsers.Brief = item.Brief;
                    ObjUsers.Created_Date = item.Created_Date;
                    ObjUsers.Name = item.Name;
                    ObjUsers.Email = item.Email;
                    ObjUsers.Mobile = item.Mobile;
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }

                return View(ObjBooking);
            }

            // return View();
        }

        [HttpGet] //GMS Logout
        public ActionResult PortsGMSLogout()
        {
            if (Session["PortsGMSAdminUser"] != null)
            {
                Session["PortsGMSAdminUser"] = null;
            }
            if (Session["PortsGMSUser"] != null)
            {
                Session["PortsGMSUser"] = null;
            }

            return this.Redirect("/Grievance");
        }

        private void SendEmailGMSStackHolder(string Email = null, string Name = null, Guid Id = new Guid(), string Level1UserName = null, string Type = null)
        {
            MailMessage mail = null;
            var getEmailTo = Email;
            string link = null;
            try
            {
                var settingsItem = Context.Database.GetItem("{781F5BC1-91FC-43C1-898B-A414C87E4257}");

                if (Type == "Booking")
                {
                    settingsItem = Context.Database.GetItem("{740BC9CA-B269-41A8-BBAF-15FCCDC3ADD4}");
                }
                if (Type == "BookingOnBehalf")
                {
                    settingsItem = Context.Database.GetItem("{DEDB7D1B-A3C9-4F32-92DD-97B1B733533B}");
                    var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceBookingOnBehalfReview);
                    link = "<a href='" + page.Url() + "?" + @Id + "'>Click Here</a>";
                }

                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Subject];

                try
                {
                    string to = getEmailTo;
                    string from = fromMail.Value;
                    string emailSubject = subject.Value;
                    string message = body.Value.Replace("#Name#", Name).Replace("#User#", Level1UserName).Replace("#ClickHere#", link);
                    EmailServicesProvider emailServicesProvider = new EmailServicesProvider();
                    if (emailServicesProvider.sendEmail(to, emailSubject, message, from))
                    {
                        Log.Info("Email Sent- ", "");
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
                }
            }
            catch (Exception ex)
            {
                // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }
        }
        private void SendEmailGMSLevel1(string Email = null, string Name = null, Guid Id = new Guid(), string Type = null)
        {
            MailMessage mail = null;
            var getEmailTo = Email;
            try
            {
                var settingsItem = Context.Database.GetItem("{30B37390-9AF3-4BD8-8144-E94162DAD89F}");


                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Subject];

                try
                {
                    string to = getEmailTo;
                    string from = fromMail.Value;
                    string emailSubject = subject.Value;
                    string message = body.Value.Replace("#Name#", Name).Replace("#ID#", Name);
                    EmailServicesProvider emailServicesProvider = new EmailServicesProvider();
                    if (emailServicesProvider.sendEmail(to, emailSubject, message, from))
                    {
                        Log.Info("Email Sent- ", "");
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
                }
            }
            catch (Exception ex)
            {
                // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }

        }
        private void SendEmailGMSLevel2(string Email = null, string Name = null, Guid Id = new Guid(), string Type = null)
        {
            MailMessage mail = null;
            var getEmailTo = Email;
            try
            {
                var settingsItem = Context.Database.GetItem("{9D2959D7-28FE-4742-82C6-D86575CC0D72}");

                if (Type == "Booking")
                {
                    settingsItem = Context.Database.GetItem("{781F5BC1-91FC-43C1-898B-A414C87E4257}");
                }
                if (Type == "BookingOnBehalf")
                {
                    settingsItem = Context.Database.GetItem("{781F5BC1-91FC-43C1-898B-A414C87E4257}");
                }

                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Subject];

                try
                {
                    string to = getEmailTo;
                    string from = fromMail.Value;
                    string emailSubject = subject.Value;
                    string message = body.Value.Replace("#Name#", Name).Replace("#ID#", Name);
                    EmailServicesProvider emailServicesProvider = new EmailServicesProvider();
                    if (emailServicesProvider.sendEmail(to, emailSubject, message, from))
                    {
                        Log.Info("Email Sent- ", "");
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
                }
            }
            catch (Exception ex)
            {
                // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }
        }
        private void SendEmailGMSLevel3(string Email = null, string Name = null, Guid Id = new Guid(), string Type = null)
        {
            MailMessage mail = null;
            var getEmailTo = Email;
            try
            {
                var settingsItem = Context.Database.GetItem("{3E10CBC6-51C4-4740-9BC6-6EBB4257429F}");

                if (Type == "Booking")
                {
                    settingsItem = Context.Database.GetItem("{781F5BC1-91FC-43C1-898B-A414C87E4257}");
                }
                if (Type == "BookingOnBehalf")
                {
                    settingsItem = Context.Database.GetItem("{781F5BC1-91FC-43C1-898B-A414C87E4257}");
                }

                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Subject];

                try
                {
                    string to = getEmailTo;
                    string from = fromMail.Value;
                    string emailSubject = subject.Value;
                    string message = body.Value.Replace("#Name#", Name).Replace("#ID#", Name);
                    EmailServicesProvider emailServicesProvider = new EmailServicesProvider();
                    if (emailServicesProvider.sendEmail(to, emailSubject, message, from))
                    {
                        Log.Info("Email Sent- ", "");
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
                }
            }
            catch (Exception ex)
            {
                // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }

        }
        private void SendEmailGMSSustainabilityCellUser(string Email = null, string Name = null, Guid Id = new Guid(), string Type = null)
        {
            MailMessage mail = null;
            var getEmailTo = Email;
            try
            {
                var settingsItem = Context.Database.GetItem("{781F5BC1-91FC-43C1-898B-A414C87E4257}");

                if (Type == "Booking")
                {
                    settingsItem = Context.Database.GetItem("{781F5BC1-91FC-43C1-898B-A414C87E4257}");
                }
                if (Type == "BookingOnBehalf")
                {
                    settingsItem = Context.Database.GetItem("{781F5BC1-91FC-43C1-898B-A414C87E4257}");
                }

                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Subject];


                try
                {
                    string to = getEmailTo;
                    string from = fromMail.Value;
                    string emailSubject = subject.Value;
                    string message = body.Value.Replace("#Name#", Name);
                    EmailServicesProvider emailServicesProvider = new EmailServicesProvider();
                    if (emailServicesProvider.sendEmail(to, emailSubject, message, from))
                    {
                        Log.Info("Email Sent- ", "");
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
                }
            }
            catch (Exception ex)
            {
                // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }
        }

        private void SendEmailGMSOTP(string Email = null, string Name = null, string Type = null, string OTP = null)
        {
            Log.Info("SendEmailGMSOTP start Email:" + Email + "Name" + Name + "Type" + Type + "Otp" + OTP, this);

            //*
            ///================Email code New =========================

            var settingsItem = Context.Database.GetItem("{5FA38428-DEC0-40A7-BF4D-90B975842433}");

            var mailTemplateItem = settingsItem;
            string body = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Body].Value;
            string subject = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Subject].Value;

            body = body.Replace("#Name#", Name);
            body = body.Replace("#OTP#", OTP);
            body = body.Replace("#TYPE", Type);
            string from = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.From].Value;
            try
            {
                Log.Info("SendEmailGMSOTP try start Email:" + Email + "Name" + Name + "Type" + Type + "Otp" + OTP, this);

                string to = Email;
                string emailSubject = subject;
                string message = body;
                EmailServicesProvider emailServicesProvider = new EmailServicesProvider();
                if (emailServicesProvider.sendEmail(to, emailSubject, message, from))
                {
                    Log.Info("Email Sent- ", "");
                }
                Log.Info("SendEmailGMSOTP try end Email:" + Email + "Name" + Name + "Type" + Type + "Otp" + OTP, this);
            }
            catch (Exception exception)
            {
                Exception ex = exception;
                Console.WriteLine(ex.Message, "sendOTPEmail - ");
                Log.Error(ex.Message, "sendOTPEmail - ");
                Log.Error(ex.InnerException.ToString(), "sendOTPEmail - ");
                Log.Error("SendEmailGMSOTP exception occur" + exception, this);
            }
        }



        private void SendEmailGMSAddUser(string Email = null, string Name = null, string userType = null)
        {
            MailMessage mail = null;
            var getEmailTo = Email;
            try
            {
                var settingsItem = Context.Database.GetItem("{8E5A5158-0873-463C-B383-F73D7436DDED}");


                var mailTemplateItem = settingsItem;
                var fromMail = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.From];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }

                var body = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Body];
                var subject = mailTemplateItem.Fields[PortsGMSTemplates.MailTemplate.Fields.Subject];


                try
                {
                    string to = getEmailTo;
                    string from = fromMail.Value;
                    string emailSubject = subject.Value;
                    string message = body.Value.Replace("#user#", Name);
                    EmailServicesProvider emailServicesProvider = new EmailServicesProvider();
                    if (emailServicesProvider.sendEmail(to, emailSubject, message, from))
                    {
                        Log.Info("Email Sent- ", "");
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
                }
            }
            catch (Exception ex)
            {
                // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }

        }


        //[HttpPost]
        //public static List<PortsGMSGrievanceBookingModel> GetCountriesName(string Location = null)
        //{
        //    List<PortsGMSGrievanceBookingModel> lst = new List<PortsGMSGrievanceBookingModel>();
        //    var item = Sitecore.Context.Database.GetItem("{6C7091FF-F6B4-4937-AA04-21E7E51ABD5D}");
        //    foreach (var element in item.GetChildren().ToList())
        //    {
        //        var childCount = element.GetChildren().Count();
        //        if (childCount != 0)
        //        {

        //        }
        //        else
        //        {

        //        }
        //    }
        //    lst.Add(new PortsGMSGrievanceBookingModel() { CountryId = 1, CountryName = "India" });
        //    lst.Add(new CountryList() { CountryId = 2, CountryName = "Nepal" });
        //    lst.Add(new CountryList() { CountryId = 3, CountryName = "America" });
        //    return lst;

        //}



        // vessel Hazira Port : Vessel Schedule
        //Stage Url- https://muosbstgext.adani.com/VesselDtlsBerthProject/PS/VesselDtlsBerthPS
        //ID - websiteuser Password - Web51t@5t9(sr
        public ActionResult VesselHaziraBerthList()
        {
            VesselModel VesselModel = new VesselModel();
            //  var url = "https://entosbext.adani.com/VesselDtlsBerthProject/PS/VesselDtlsBerthPS";

            var url = Settings.GetSetting("VesselDtlsBerthProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""103"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""3"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselHaziraBerth", vesselsvar);
        }
        public ActionResult VesselHaziraAnchorList()
        {
            VesselModel VesselModel = new VesselModel();
            //stage Url- https://muosbstgext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS
            //var url = "https://entosbext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS";

            var url = Settings.GetSetting("VesselDtlsAnchorageProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""103"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""3"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselHaziraAnchor", vesselsvar);
        }
        public ActionResult VesselHaziraExpectedList()
        {
            //stage Url - https://muosbstgext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS
            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS";

            var url = Settings.GetSetting("VesselDtlsExpectedProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);


            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""103"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""3"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselHaziraExpected", vesselsvar);
        }
        public ActionResult VesselHaziraSailedList()
        {
            //stage Url - https://muosbstgext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS
            VesselModel VesselModel = new VesselModel();
            // var url = "https://entosbext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS";
            var url = Settings.GetSetting("VesselDtlsSailed24Project:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");
            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""103"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""3"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselHaziraSailed", vesselsvar);
        }


        // Vessel Dahej Port : Vessel Schedule
        //stage url - https://muosbstgext.adani.com/VesselDtlsBerthProject/PS/VesselDtlsBerthPS
        public ActionResult VesselDahejBerthList()
        {

            VesselModel VesselModel = new VesselModel();
            //stage
            //   var url = "https://muosbstgext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsBerthPS";
            // var url = "https://entosbext.adani.com/VesselDtlsBerthProject/PS/VesselDtlsBerthPS";

            var url = Settings.GetSetting("VesselDtlsBerthProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");
            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""102"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""2"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselDahejBerth", vesselsvar);
        }

        public ActionResult VesselDahejAnchorList()
        {
            //stage url - https://muosbstgext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS
            VesselModel VesselModel = new VesselModel();
            // var url = "https://entosbext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS";
            // var url = "https://muosbstgext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS";
            var url = Settings.GetSetting("VesselDtlsAnchorageProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""102"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""2"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselDahejAnchor", vesselsvar);
        }

        public ActionResult VesselDahejExpectedList()
        {
            //stage Url - https://muosbstgext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS
            VesselModel VesselModel = new VesselModel();

            // var url = "https://entosbext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS";
            // var url = "https://muosbstgext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS";

            var url = Settings.GetSetting("VesselDtlsExpectedProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""102"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""2"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselDahejExpected", vesselsvar);
        }
        public ActionResult VesselDahejSailedList()
        {
            //stage Url - https://muosbstgext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS
            VesselModel VesselModel = new VesselModel();
            //  var url = "https://entosbext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS";
            // var url = "https://muosbstgext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS";
            var url = Settings.GetSetting("VesselDtlsSailed24Project:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");
            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""102"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""2"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselDahejSailed", vesselsvar);
        }


        // Vessel Kattupalli Port : vessel Schedule
        //stage Url - https://muosbstgext.adani.com/VesselDtlsBerthProject/PS/VesselDtlsBerthPS
        public ActionResult VesselKattupalliBerthList()
        {
            VesselModel VesselModel = new VesselModel();
            //  var url = "https://entosbext.adani.com/VesselDtlsBerthProject/PS/VesselDtlsBerthPS";

            var url = Settings.GetSetting("VesselDtlsBerthProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");
            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""108"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""8"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselKattupalliBerth", vesselsvar);
        }

        public ActionResult VesselKattupalliAnchorList()
        {
            //Stage url - "https://muosbstgext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS"
            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS";

            var url = Settings.GetSetting("VesselDtlsAnchorageProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");
            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""108"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""8"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselKattupalliAnchor", vesselsvar);
        }

        public ActionResult VesselKattupalliExpectedList()
        {
            //stage url - https://muosbstgext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS
            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS";

            var url = Settings.GetSetting("VesselDtlsExpectedProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");
            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""108"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""8"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselKattupalliExpected", vesselsvar);
        }
        public ActionResult VesselKattupalliSailedList()
        {
            //stage url - https://muosbstgext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS
            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            var url = Settings.GetSetting("VesselDtlsSailed24Project:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""108"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""8"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselKattupalliSailed", vesselsvar);
        }

        //vessel Tuna Terminal  : vessel Scehdule
        public ActionResult VesselTunaBerthList()
        {

            VesselModel VesselModel = new VesselModel();

            // var url = "https://entosbext.adani.com/VesselDtlsBerthProject/PS/VesselDtlsBerthPS";
            var url = Settings.GetSetting("VesselDtlsBerthProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");
            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""106"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""6"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselTunaBerth", vesselsvar);
        }
        public ActionResult VesselTunaAnchorList()
        {

            VesselModel VesselModel = new VesselModel();
            // var url = "https://entosbext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS";
            var url = Settings.GetSetting("VesselDtlsAnchorageProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""106"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""6"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselTunaAnchor", vesselsvar);
        }

        public ActionResult VesselTunaExpectedList()
        {

            VesselModel VesselModel = new VesselModel();
            //   var url = "https://entosbext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS";
            var url = Settings.GetSetting("VesselDtlsExpectedProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");
            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""106"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""6"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselTunaExpected", vesselsvar);
        }
        public ActionResult VesselTunaSailedList()
        {

            VesselModel VesselModel = new VesselModel();
            //   var url = "https://entosbext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS";
            var url = Settings.GetSetting("VesselDtlsSailed24Project:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""106"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""6"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselTunaSailed", vesselsvar);
        }


        //vessel Dhamra Port : Vessel Schedule

        public ActionResult VesselDhamraBerthList()
        {

            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsBerthProject/PS/VesselDtlsBerthPS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            var url = Settings.GetSetting("VesselDtlsBerthProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""107"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""7"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselDhamraBerth", vesselsvar);
        }

        public ActionResult VesselDhamraAnchorList()
        {

            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            var url = Settings.GetSetting("VesselDtlsAnchorageProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""107"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""7"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselDhamraAnchor", vesselsvar);
        }

        public ActionResult VesselDhamraExpectedList()
        {

            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");

            var url = Settings.GetSetting("VesselDtlsExpectedProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""107"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""7"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselDhamraExpected", vesselsvar);
        }
        public ActionResult VesselDhamraSailedList()
        {
            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            var url = Settings.GetSetting("VesselDtlsSailed24Project:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""107"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""7"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselDhamraSailed", vesselsvar);
        }

        //vessel Vizag Terminal : Vessel Schedule 
        public ActionResult VesselVizagBerthList()
        {

            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsBerthPS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0dVesselDtlsBerthProject
            var url = Settings.GetSetting("VesselDtlsBerthProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""105"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""5"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselVizagBerth", vesselsvar);
        }

        public ActionResult VesselVizagAnchorList()
        {

            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            var url = Settings.GetSetting("VesselDtlsAnchorageProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""105"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""5"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselVizagAnchor", vesselsvar);
        }

        public ActionResult VesselVizagExpectedList()
        {

            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            var url = Settings.GetSetting("VesselDtlsExpectedProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""105"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""5"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselVizagExpected", vesselsvar);
        }
        public ActionResult VesselVizagSailedList()
        {

            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            var url = Settings.GetSetting("VesselDtlsSailed24Project:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""105"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""5"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselVizagSailed", vesselsvar);
        }

        //vessel Mormugao Terminal  : Vessel Schedule 
        public ActionResult VesselMurmugaoBerthList()
        {
            //stage url - https://muosbstgext.adani.com/VesselDtlsBerthProject/PS/VesselDtlsBerthPS
            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsBerthProject/PS/VesselDtlsBerthPS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            var url = Settings.GetSetting("VesselDtlsBerthProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""104"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""4"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselMurmugaoBerth", vesselsvar);
        }

        public ActionResult VesselMurmugaoAnchorList()
        {
            //stage Url - https://muosbstgext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS
            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            var url = Settings.GetSetting("VesselDtlsAnchorageProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""104"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""4"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselMurmugaoAnchor", vesselsvar);
        }

        public ActionResult VesselMurmugaoExpectedList()
        {
            //stage url - https://muosbstgext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS
            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            var url = Settings.GetSetting("VesselDtlsExpectedProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""104"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""4"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselMurmugaoExpected", vesselsvar);
        }
        public ActionResult VesselMurmugaoSailedList()
        {
            //stage url - https://muosbstgext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS
            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");

            var url = Settings.GetSetting("VesselDtlsSailed24Project:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");

            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""104"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""4"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselMurmugaoSailed", vesselsvar);
        }

        // vessel Krishnapatnam Port : vessel schedule
        public ActionResult VesselKrishnapatnamBerthList()
        {
            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsBerthProject/PS/VesselDtlsBerthPS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            var url = Settings.GetSetting("VesselDtlsBerthProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""109"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""9"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselKrishnapatnamBerth", vesselsvar);
        }

        public ActionResult VesselKrishnapatnamAnchorList()
        {
            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            var url = Settings.GetSetting("VesselDtlsAnchorageProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");



            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""109"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""9"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselKrishnapatnamAnchor", vesselsvar);
        }

        public ActionResult VesselKrishnapatnamExpectedList()
        {
            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            //var url = "https://entosbext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS";

            var url = Settings.GetSetting("VesselDtlsExpectedProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");




            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""109"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""9"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselKrishnapatnamExpected", vesselsvar);
        }
        public ActionResult VesselKrishnapatnamSailedList()
        {
            VesselModel VesselModel = new VesselModel();
            //var url = "https://entosbext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     "3e9s1t@pr0d");
            var url = Settings.GetSetting("VesselDtlsSailed24Project:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");



            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""109"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""9"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselKrishnapatnamSailed", vesselsvar);
        }


        // Vessel Mundra Port : vessel Schedule
        public ActionResult VesselMundraBerthList()
        {
            VesselModel VesselModel = new VesselModel();
            //var url = "https://muosbstgext.adani.com/VesselDtlsBerthProject/PS/VesselDtlsBerthPS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     websitepass);
            var url = Settings.GetSetting("VesselDtlsBerthProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""101"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""1"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselMundraBerth", vesselsvar);

        }
        public ActionResult VesselMundraAnchorList()
        {
            VesselModel VesselModel = new VesselModel();
            //var url = "https://muosbstgext.adani.com/VesselDtlsAnchorageProject/PS/VesselDtlsAnchoragePS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     websitepass);
            var url = Settings.GetSetting("VesselDtlsAnchorageProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""101"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""1"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselMundraAnchor", vesselsvar);
        }

        public ActionResult VesselMundraExpectedList()
        {
            VesselModel VesselModel = new VesselModel();
            //var url = "https://muosbstgext.adani.com/VesselDtlsExpectedProject/PS/VesselDtlsExpectedPS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     websitepass);
            var url = Settings.GetSetting("VesselDtlsExpectedProject:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""101"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""1"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselMundraExpected", vesselsvar);
        }
        public ActionResult VesselMundraSailedList()
        {
            VesselModel VesselModel = new VesselModel();
            //var url = "https://muosbstgext.adani.com/VesselDtlsSailed24Project/PS/VesselDtlsSailed24PS";

            //byte[] credentialBuffer = new UTF8Encoding().GetBytes(
            //     websiteuser + ":" +
            //     websitepass);
            var url = Settings.GetSetting("VesselDtlsSailed24Project:ApiPath");
            var websiteuser = Settings.GetSetting("UserName:ApiPath");
            var websitepass = Settings.GetSetting("UserPassword:ApiPath");


            var httpRequest = (HttpWebRequest)WebRequest.Create(url);
            httpRequest.Method = "POST";

            httpRequest.Accept = "application/json";
            httpRequest.ContentType = "application/json";
            //httpRequest.Headers["Authentication"]="Basic" +Convert.ToBase64String(credentialBuffer);
            httpRequest.UseDefaultCredentials = true;
            httpRequest.PreAuthenticate = true;
            httpRequest.Credentials = new NetworkCredential(websiteuser, websitepass);

            var postData = @"{
                 ""serviceCode"" : ""1"",
                 ""regionCode"" : ""101"",
                 ""requestBody"" : {
                  ""terminalNo"" : ""1"",
                  ""consumer"" : ""ADANI"",
                  ""SBUcode"" : """"
                 }
                }";

            //using (var streamWriter = new StreamWriter(httpRequest.GetRequestStream()))
            //{
            //    streamWriter.Write(data);
            //}
            var data = Encoding.ASCII.GetBytes(postData);

            using (var stream = httpRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            HttpWebResponse response = (HttpWebResponse)httpRequest.GetResponse();
            String ver = response.ProtocolVersion.ToString();
            StreamReader reader = new StreamReader(response.GetResponseStream());
            string str = reader.ReadLine();

            var result = JsonConvert.DeserializeObject<DataClass>(str);
            List<VesselModel> vesselsvar = new List<VesselModel>();

            foreach (var item in result.data.ToList())
            {
                vesselsvar = item.vessels;
            }

            return View("VesselMundraSailed", vesselsvar);
        }

        #endregion
    }

}