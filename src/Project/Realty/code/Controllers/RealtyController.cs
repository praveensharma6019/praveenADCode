using Newtonsoft.Json.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Realty.Website.Model;
using Sitecore.Realty.Website.Services;
using Sitecore.Resources.Media;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Web.Mvc;
using static Sitecore.Realty.Website.Model.PropertyCollection;
using System.Text.RegularExpressions;
using System.Web;

namespace Sitecore.Realty.Website.Controllers
{
    public class RealtyController : Controller
    {
        private static readonly Regex alphaNumber = new Regex("^[a-z A-Z0-9]*$");
        private static readonly Regex emailRegex = new Regex("^[a-zA-Z0-9+_.-]+@[a-zA-Z0-9.-]+$");
        private static readonly Regex NumRegex = new Regex("^[0-9]*$");

        public RealtyController(IPaymentServices paymentService)
        {
            this.PaymentService = paymentService;
        }
        private IPaymentServices PaymentService { get; }
        RealtyRepository realtyRepo = new RealtyRepository();
        private bool IsValueValid = true;

        // GET: Realty
        public ActionResult Index()
        {
            return View();
        }
        public bool IsReCaptchValid(string reResponse)
        {
            bool flag = false;
            string str = reResponse;
            string str1 = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "6Lcql9QZAAAAAOSIsZ-9gNRiZaVPrDmSrbKSe3y2");
            string str2 = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", str1, str);
            using (WebResponse response = ((HttpWebRequest)WebRequest.Create(str2)).GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    flag = (JObject.Parse(streamReader.ReadToEnd()).Value<bool>("success") ? true : false);
                }
            }
            return flag;
        }
        #region RentalForm
        Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
        public ActionResult RentalForm()
        {
            RentalFormModel m = new RentalFormModel();

            return View(m);
        }
        [HttpPost]
        public ActionResult RentalForm(RentalFormModel model, string SubmitRentalForm)
        {

            bool flag = false;

            try
            {
                flag = this.IsReCaptchValid(model.reResponse);
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Error(string.Concat("Failed to validate auto script in AdaniRealty Rental Form : ", exception.ToString()), this);
            }
            if (flag == true)
            {
                Log.Info("Insert AdaniRealty Rental Form Captcha Validated", this);
                try
                {
                    if (!string.IsNullOrEmpty(SubmitRentalForm))
                    {
                        if (ModelState.IsValid)
                        {
                            model.Location = "\"" + model.Location + "\"";
                            model.FormName = "\"" + model.FormName + "\"";
                            model.PageInfo = "\"" + model.PageInfo + "\"";
                            model.Remarks = "\"" + model.Remarks + "\"";
                            model.ReferenceFrom = "\"" + model.ReferenceFrom + "\"";
                            if (model.selectedApartmentType != null && model.selectedApartmentType.Count() > 0)
                            {
                                model.ApartmentTypeList = string.Join(" | ", model.selectedApartmentType);
                            }
                            if (model.selectedApartmentSize != null && model.selectedApartmentSize.Count() > 0)
                            {
                                model.ApartmentSizeList = string.Join(" | ", model.selectedApartmentSize);
                            }
                            RealtyDataContext rdb = new RealtyDataContext();
                            AdaniRealtyRentalForm rentalForm = new AdaniRealtyRentalForm()
                            {
                                Id = Guid.NewGuid(),
                                Name = model.Name,
                                ContactNo = model.ContactNo,
                                EmailId = model.EmailId,
                                Occupation = model.Occupation,
                                Location = model.Location,
                                SelectedApartmentTypes = model.ApartmentTypeList,

                                SelectedApartmentSizes = model.ApartmentSizeList,
                                Remarks = model.Remarks,
                                Referencefrom = model.ReferenceFrom,
                                Budget = model.Budget,
                                BrokerName = model.BrokerName,
                                BrokerContactNo = model.BrokerContactNo,
                                SubmittedOn = new DateTime?(DateTime.Now),
                                FormName = model.FormName ?? "Adani Realty Rental Form",
                                PageInfo = model.PageInfo
                            };
                            foreach (var size in model.selectedApartmentSize)
                            {
                                if (size.Trim().ToUpper() == "1 BHK")
                                {
                                    rentalForm._1_BHK = size.Trim();
                                }
                                if (size.Trim().ToUpper() == "2 BHK")
                                {
                                    rentalForm._2_BHK = size.Trim();
                                }
                                if (size.Trim().ToUpper() == "2.5 BHK")
                                {
                                    rentalForm._2_5_BHK = size.Trim();
                                }
                                if (size.Trim().ToUpper() == "3 BHK")
                                {
                                    rentalForm._3_BHK = size.Trim();
                                }
                                if (size.Trim().ToUpper() == "3.5 BHK")
                                {
                                    rentalForm._3_5_BHK = size.Trim();
                                }
                                if (size.Trim().ToUpper() == "4 BHK")
                                {
                                    rentalForm._4_BHK = size.Trim();
                                }
                                if (size.Trim().ToUpper() == "4 BHK PENT HOUSE")
                                {
                                    rentalForm._4_BHK_PENT_HOUSE = size.Trim();
                                }
                                if (size.Trim().ToUpper() == "4 BHK VILLA")
                                {
                                    rentalForm._4_BHK_VILLA = size.Trim();
                                }
                                if (size.Trim().ToUpper() == "5 BHK PENT HOUSE")
                                {
                                    rentalForm._5_BHK_PENT_HOUSE = size.Trim();
                                }
                                if (size.Trim().ToUpper() == "5 BHK VILLA")
                                {
                                    rentalForm._5_BHK_VILLA = size.Trim();
                                }
                                if (size.Trim().ToUpper() == "6 BHK VILLA")
                                {
                                    rentalForm._6_BHK_VILLA = size.Trim();
                                }
                            }
                            foreach (var type in model.selectedApartmentType)
                            {
                                if (type.Trim().ToUpper() == "BASIC FITTINGS")
                                {
                                    rentalForm.BASIC_FITTINGS = type.Trim();
                                }
                                if (type.Trim().ToUpper() == "FULLY FURNISHED")
                                {
                                    rentalForm.FULLY_FURNISHED = type.Trim();
                                }
                                if (type.Trim().ToUpper() == "SEMI FURNISHED")
                                {
                                    rentalForm.SEMI_FURNISHED = type.Trim();
                                }
                            }
                            rdb.AdaniRealtyRentalForms.InsertOnSubmit(rentalForm);
                            rdb.SubmitChanges();
                            Log.Info("Form Adani Realty Rental Form data saved into db successfully: ", this);
                            string mailFrom = DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/mailFrom", "info@adani.com");
                            string mailTo = DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/mailTo", "info@adani.com");
                            string mailSubject = DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/mailSubject", "Realty: Rental Form Enquiry");
                            string mailMessage = DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/mailMessage", "Thankyou for Enquiry");
                            string mailMessageAdmin = DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/AdminMailMessage", "test");
                            mailMessage.Replace("$name", model.Name);
                            mailMessageAdmin.Replace("$contactNo", model.ContactNo);
                            mailMessageAdmin.Replace("$emailId", model.EmailId);
                            mailMessageAdmin.Replace("$occupation", model.Occupation);
                            mailMessageAdmin.Replace("$location", model.Location);
                            mailMessageAdmin.Replace("$apartmentSize", model.ApartmentSizeList);
                            mailMessageAdmin.Replace("$apartmentType", model.ApartmentTypeList);
                            mailMessageAdmin.Replace("$budget", model.Budget);
                            mailMessageAdmin.Replace("$brokerName", model.BrokerName);
                            mailMessageAdmin.Replace("$brokerContactNo", model.BrokerContactNo);
                            mailMessageAdmin.Replace("$pageInfo", model.PageInfo);
                            try
                            {
                                sendEmail(model.EmailId, mailSubject, mailMessage, mailFrom);
                                sendEmail(mailTo, mailSubject, mailMessageAdmin, mailFrom);
                            }
                            catch (Exception exception3)
                            {
                                Exception exception2 = exception3;
                                Log.Error(string.Concat("Error at sending mail Adani Realty rental Form: ", exception2.Message), this);
                            }
                            return this.Redirect(model.ResponseURL);
                        }
                        else
                        {
                            Session["Key"] = true;
                            return base.View(model);
                        }
                    }
                }
                catch (Exception exception3)
                {
                    Exception exception2 = exception3;
                    Log.Error(string.Concat("Error at Saving Adani Realty rental Form: ", exception2.Message), this);
                    model.ReturnViewMessage = "Something has been wrong, Please try again later";
                    return View(model);
                }
            }
            else
            {
                base.ModelState.AddModelError("reResponse", DictionaryPhraseRepository.Current.Get("/Controller/RentalForm/Captcha Error", "Captcha is Invalid, Please try again"));
                return View(model);
            }
            ((dynamic)base.ViewBag).Message = "Something has been wrong, Please try again later";
            return View(model);
        }
        #endregion


        [HttpGet]
        public ActionResult BookNow()
        {
            if (string.IsNullOrEmpty(RenderingContext.Current.Rendering.DataSource))
            {
                return null;
            }
            var PropertyInformation = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.DataSource;
            var parent = Sitecore.Context.Database.GetItem(PropertyInformation).Children;
            PropertyCollection PropertyCoolection = new PropertyCollection();

            foreach (var item in parent.InnerChildren)
            {
                Property PropertyInfo = new Property();

                PropertyInfo.PropertyName = item["Property Name"];
                Sitecore.Data.Fields.ImageField imgField = ((Sitecore.Data.Fields.ImageField)item.Fields["Property Logo"]);
                string url = imgField.MediaItem != null ? Sitecore.Resources.Media.MediaManager.GetMediaUrl(imgField.MediaItem) : string.Empty;
                PropertyInfo.PropertyLogo = url;
                PropertyInfo.PropertyAddress = item["Property Address"];
                PropertyInfo.PropertyConfiguration = item["Property Configuration"];
                PropertyInfo.PropertyPossession = item["Property Possession"];
                PropertyInfo.PropertyCode = item["PropertyCode"];
                foreach (var items in item.Children.ToList())
                {
                    PropertyType PropertyTypess = new PropertyType();

                    PropertyTypess.CarpetArea = items["Carpet Area"];
                    PropertyTypess.PropertySelection = items["Property Type"];
                    PropertyTypess.BookingPrice = items["Starting Price"];
                    PropertyTypess.BookingAmount = items["Booking Amount"];


                    PropertyInfo.PropertyTypes.Add(PropertyTypess);

                }

                PropertyCoolection.PropertyCollectionLists.Add(PropertyInfo);

            }
            Session["Message"] = null;
            return View(PropertyCoolection);

        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult BookNow(PropertyCollection model, string MakePayment)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(HttpUtility.HtmlEncode(model.PropertyCode));
                model.PropertyCode = sb.ToString();
                #region BookingAmount
                if (!string.IsNullOrEmpty(MakePayment))
                {

                    model.PaymentGateway = DictionaryPhraseRepository.Current.Get("/Realty/BookNow/GatewayName", "BillDesk");
                    model.PropertyName = RealtyHelper.GetPropertyName(model.PropertyCode);
                    model.PaymentAmount = string.IsNullOrEmpty(RealtyHelper.GetPropertyBookingAmount(model.PropertyCode, model.PropertyType)) ? "51000" : RealtyHelper.GetPropertyBookingAmount(model.PropertyCode, model.PropertyType);
                    model.ParentProject = RealtyHelper.GetParentProject(model.PropertyCode);
                    Session["Message"] = "notValid";
                    if (!this.ModelState.IsValid)
                    {
                        model.PropertyCollectionLists = RealtyHelper.GetPropertyList(model.PropertyCode).PropertyCollectionLists;
                        return this.View(model);
                    }
                    if (string.IsNullOrEmpty(model.FirstName.ToString()) || model.PaymentAmount.ToString().Any(char.IsLetter))
                    {
                        this.ModelState.AddModelError("FirstName", DictionaryPhraseRepository.Current.Get("/Realty/BookNow/Invalid FirstName", "Please enter First Name"));
                        IsValueValid = false;
                    }
                    if (string.IsNullOrEmpty(model.LastName.ToString()) || model.PaymentAmount.ToString().Any(char.IsLetter))
                    {
                        this.ModelState.AddModelError("LastName", DictionaryPhraseRepository.Current.Get("/Realty/BookNow/Invalid lastname", "Please enter Last Name"));
                        IsValueValid = false;
                    }
                    if (string.IsNullOrEmpty(model.LastName.ToString()) || model.PaymentAmount.ToString().Any(char.IsLetter))
                    {
                        this.ModelState.AddModelError("LastName", DictionaryPhraseRepository.Current.Get("/Realty/BookNow/InvalidAmount", "Please enter Last Name"));
                        IsValueValid = false;
                    }
                    if (!IsValueValid)
                    {
                        model.PropertyCollectionLists = RealtyHelper.GetPropertyList(model.PropertyCode).PropertyCollectionLists;
                        ViewBag.Message = "";
                        return View(model);
                    }
                    if (string.IsNullOrEmpty(model.PaymentAmount.ToString()) || model.PaymentAmount.ToString().Any(char.IsLetter))
                    {
                        this.ModelState.AddModelError("PaymentAmount", DictionaryPhraseRepository.Current.Get("/Realty/BookNow/InvalidAmount", "Invalid amount payable value."));
                        return this.View(model);
                    }
                    else if (System.Convert.ToDecimal(model.PaymentAmount) < 0)
                    {
                        this.ModelState.AddModelError("PaymentAmount", DictionaryPhraseRepository.Current.Get("/Realty/BookNow/AmountNegativeValidation", "You have no amount payable value."));
                        return this.View(model);
                    }
                    else if (System.Convert.ToDecimal(model.PaymentAmount) == 0 && System.Convert.ToDecimal(model.PaymentAmount) <= 0)
                    {
                        this.ModelState.AddModelError("PaymentAmount", DictionaryPhraseRepository.Current.Get("/Realty/BookNow/AdvanceAmountValidation", "You have no amount payable value. Please enter proper advance amount."));
                        return this.View(model);
                    }
                    else
                    {
                        model.PaymentAmount = System.Convert.ToDecimal(model.PaymentAmount.ToString().Trim()) == 0 ? System.Convert.ToDecimal(model.PaymentAmount.ToString().Trim()).ToString("f2") : System.Convert.ToDecimal(model.PaymentAmount.ToString().Trim()).ToString("f2");
                    }
                    this.PaymentService.StorePaymentRequest(model);
                    string RequestHTML = this.PaymentService.BillDeskTransactionRequestAPIRequestPost(model);
                    return Content(RequestHTML);
                }
                #endregion
                return this.View(model);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at Realty BookNow:" + ex.Message, this);
                return this.View(model);

            }
        }

        [HttpGet]
        public ActionResult BookNowPropertyTypes(string PropName)
        {

            PropertyTypeDetails properties = new PropertyTypeDetails();
            try
            {
                Item propList = Sitecore.Context.Database.GetItem("{8CFDD7DA-3B9E-4B81-BC89-684C8F6521BF}");
                if (propList != null)
                {
                    if (propList.HasChildren)
                    {
                        properties.PropertyCode = PropName;
                        foreach (Item names in propList.GetChildren())
                        {
                            if (names["PropertyCode"] == PropName)
                            {
                                var details = names.GetChildren();
                                foreach (Item types in names.GetChildren())
                                {
                                    PropertyTypeSelect property = new PropertyTypeSelect();
                                    property.PropertySelection = types["Property Type"];
                                    properties.PropertiesTypeList.Add(property);
                                }
                                if (details.Count > 0)
                                {
                                    properties.CarpetArea = details[0]["Carpet Area"];
                                    properties.BookingPrice = details[0]["Starting Price"];
                                    properties.BookingAmount = details[0]["Booking Amount"];

                                }
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Log.Error("Error", e.Message);
            }
            return Json(properties, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult BookNowPropertyTypesList(string PropName, string PropertyType)
        {

            PropertyTypeDetails properties = new PropertyTypeDetails();
            try
            {
                Item propList = Sitecore.Context.Database.GetItem("{8CFDD7DA-3B9E-4B81-BC89-684C8F6521BF}");
                if (propList != null)
                {
                    if (propList.HasChildren)
                    {
                        properties.PropertyCode = PropName;
                        foreach (Item names in propList.GetChildren())
                        {
                            if (names["PropertyCode"] == PropName)
                            {
                                var details = names.GetChildren();
                                foreach (Item types in names.GetChildren())
                                {
                                    if (types["Property Type"] == PropertyType)
                                    {
                                        properties.CarpetArea = types["Carpet Area"];
                                        properties.BookingPrice = types["Starting Price"];
                                        properties.BookingAmount = types["Booking Amount"];
                                        break;
                                    }
                                }
                                break;
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                Log.Error("Error", e.Message);
            }
            return Json(properties, JsonRequestBehavior.AllowGet);
        }

        public ActionResult BillDeskCallBack()
        {
            var FailureUrl = Context.Database.GetItem(Templates.Pages.PaymentFailure).Url();
            var SuccessUrl = Context.Database.GetItem(Templates.Pages.PaymentSuccess).Url();

            try
            {
                #region Variable Initialization
                string checksum = string.Empty;
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

                string ChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_ChecksumKey].Value;
                string merchantId = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Merchant_ID].Value;

                //string VDSChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_ChecksumKey].Value;
                //string VDSMerchantId = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_Merchant_ID].Value;

                #endregion

                //BillDesk Response
                string responsemsg = Request.Form["msg"];
                Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Callback Message - " + responsemsg, this);

                if (responsemsg != null)
                {
                    var responselist = responsemsg.Split('|');
                    var billDeskchecksum = responselist.Last().Trim();
                    var responsemsgdata = string.Join("|", responselist.Take(responselist.Count() - 1).ToArray());//responselist.Take(responselist.Count() - 1).ToArray().Join(;

                    var checksumresponse = this.PaymentService.GetHMACSHA256(responsemsgdata, ChecksumKey);

                    if (checksumresponse.Equals(billDeskchecksum)) // Compare Checksum
                    {
                        if (Constants.BillDeskResponse.SuccessCode.Equals(responselist[14].ToString()))
                        {
                            var modelviewpay = new PropertyCollection()
                            {
                                TransactionId = responselist[2].ToString(),
                                PaymentStatus = Constants.PaymentResponse.Success,
                                Responsecode = responselist[14].ToString(),
                                msg = Constants.PaymentResponse.Success,
                                PaymentRef = responselist[3].ToString(),
                                OrderId = responselist[1].ToString(),
                                PaymentAmount = responselist[4].ToString(),
                                CurrencyType = responselist[8].ToString(),
                                ResponseMessage = responsemsg,
                                PaymentMode = responselist[9].ToString(),
                                TransactionDate = responselist[13].ToString(),
                                FullName = responselist[17].ToString(),
                                ParentProject = responselist[22].ToString(),
                                EmailAddress = responselist[19].ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Realty/Payment Type/Bill Paid", "Booking Amount Pay"),
                                PaymentGateway = DictionaryPhraseRepository.Current.Get("/Realty/Payment Type/GatewayName", "BillDesk")
                            };

                            if (string.IsNullOrEmpty(modelviewpay.PaymentAmount) || modelviewpay.PaymentAmount == "0" || string.IsNullOrEmpty(modelviewpay.TransactionId) || modelviewpay.TransactionId.ToUpper() == "NA")
                            {
                                TempData["PaymentResponse"] = modelviewpay;
                                return this.Redirect(FailureUrl);
                            }
                            using (RealtyBookNowFormDataContext dbcontext = new RealtyBookNowFormDataContext())
                            {
                                RealtyBookNowForm ctx = dbcontext.RealtyBookNowForms.Where(x => x.FirstName + " " + x.LastName == modelviewpay.FullName && x.OrderId == modelviewpay.OrderId).FirstOrDefault();
                                if (ctx != null)
                                {
                                    if (ctx.PaymentAmount == modelviewpay.PaymentAmount)
                                    {
                                        this.PaymentService.StorePaymentResponse(modelviewpay);
                                        TempData["PaymentResponse"] = modelviewpay;
                                        Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response - ", responsemsg);
                                        return this.Redirect(SuccessUrl); ;
                                    }
                                    else
                                    {
                                        TempData["PaymentResponse"] = modelviewpay;
                                        return this.Redirect(FailureUrl);
                                    }
                                }
                            }
                            //PI Service Integration                            
                            return this.Redirect(FailureUrl);
                        }
                        else
                        {
                            //error response
                            var modelviewpay = new PropertyCollection()
                            {
                                TransactionId = responselist[2].ToString(),
                                PaymentStatus = Constants.PaymentResponse.Failure,
                                Responsecode = responselist[14].ToString(), // ErrorStatus
                                msg = responselist[24].ToString(), //DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                                PaymentRef = responselist[3].ToString(),
                                OrderId = responselist[1].ToString(),
                                CurrencyType = responselist[8].ToString(),
                                PaymentAmount = responselist[4].ToString(),
                                ResponseMessage = responsemsg,
                                FullName = responselist[17].ToString(),
                                EmailAddress = responselist[19].ToString(),
                                PaymentMode = responselist[9].ToString(),
                                ParentProject = responselist[22].ToString(),
                                TransactionDate = responselist[13].ToString()
                            };
                            this.PaymentService.StorePaymentResponse(modelviewpay);

                            TempData["PaymentResponse"] = modelviewpay;
                            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response Failure - ", responsemsg);
                            return this.Redirect(FailureUrl);
                        }
                    }
                    else
                    {
                        //Checksum Mismatch
                        var modelviewpay = new PropertyCollection()
                        {
                            TransactionId = responselist[2].ToString(),
                            PaymentStatus = Constants.PaymentResponse.Failure,
                            Responsecode = responselist[14].ToString(), // ErrorStatus
                            msg = DictionaryPhraseRepository.Current.Get("/Realty/BookNow/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                            PaymentRef = responselist[3].ToString(),
                            OrderId = responselist[1].ToString(),
                            CurrencyType = responselist[8].ToString(),
                            PaymentAmount = responselist[4].ToString(),
                            ResponseMessage = responsemsg,
                            FullName = responselist[17].ToString(),
                            PaymentMode = responselist[9].ToString(),
                            EmailAddress = responselist[19].ToString(),
                            ParentProject = responselist[22].ToString(),
                            TransactionDate = responselist[13].ToString()
                        };

                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        TempData["PaymentResponse"] = modelviewpay;
                        Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response checksum mismatch - " + responsemsg, this);
                        return this.Redirect(FailureUrl);
                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBack Response NULL error - " + responsemsg, this);
                    return this.Redirect(FailureUrl);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at BillDeskCallBack - :" + ex.Message, this);
                return this.Redirect(FailureUrl);
            }
        }

        public ActionResult PaymentSuccess()
        {
            var FailureUrl = Context.Database.GetItem(Templates.Pages.PaymentFailure).Url();
            var model = new PropertyCollection();
            if (TempData["PaymentResponse"] != null)
            {
                model = (PropertyCollection)TempData["PaymentResponse"];
                if (string.IsNullOrEmpty(model.PaymentAmount) || model.PaymentAmount == "0" || string.IsNullOrEmpty(model.TransactionId) || model.TransactionId.ToUpper() == "NA" || model.PaymentStatus != Constants.PaymentResponse.Success)
                {
                    return this.Redirect(FailureUrl);
                }
                SendMailforBookNow(model);
            }
            Log.Info("Payment Success Response", this);
            return this.View(model);
        }
        public ActionResult PaymentFailure()
        {
            var model = new PropertyCollection();
            if (TempData["PaymentResponse"] != null)
            {
                model = (PropertyCollection)TempData["PaymentResponse"];
                SendMailforBookNow(model);
            }
            Sitecore.Diagnostics.Log.Info("Payment Failure Response", this);
            return this.View(model);
        }

        public void BillDeskCallBackS2S()
        {
            var FailureUrl = Context.Database.GetItem(Templates.Pages.PaymentFailure).Url();
            var SuccessUrl = Context.Database.GetItem(Templates.Pages.PaymentSuccess).Url();

            try
            {
                #region Variable Initialization
                string checksum = string.Empty;
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.PaymentConfiguration.ID.ToString()));

                string ChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_ChecksumKey].Value;
                string merchantId = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskFields.BDSK_Merchant_ID].Value;

                //string VDSChecksumKey = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_ChecksumKey].Value;
                //string VDSMerchantId = itemInfo.Fields[Templates.PaymentConfiguration.BillDeskVDSFields.BDVDS_Merchant_ID].Value;

                #endregion

                //BillDesk Response
                string responsemsg = Request.Form["msg"];
                Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Callback Message - " + responsemsg, this);

                if (responsemsg != null)
                {
                    var responselist = responsemsg.Split('|');
                    var billDeskchecksum = responselist.Last().Trim();
                    var responsemsgdata = string.Join("|", responselist.Take(responselist.Count() - 1).ToArray());//responselist.Take(responselist.Count() - 1).ToArray().Join(;

                    var checksumresponse = this.PaymentService.GetHMACSHA256(responsemsgdata, ChecksumKey);

                    if (checksumresponse.Equals(billDeskchecksum)) // Compare Checksum
                    {
                        if (Constants.BillDeskResponse.SuccessCode.Equals(responselist[14].ToString()))
                        {
                            var modelviewpay = new PropertyCollection()
                            {
                                TransactionId = responselist[2].ToString(),
                                PaymentStatus = Constants.PaymentResponse.Success,
                                Responsecode = responselist[14].ToString(),
                                msg = Constants.PaymentResponse.Success,
                                PaymentRef = responselist[3].ToString(),
                                OrderId = responselist[1].ToString(),
                                PaymentAmount = responselist[4].ToString(),
                                ResponseMessage = responsemsg,
                                PaymentMode = responselist[9].ToString(),
                                TransactionDate = responselist[13].ToString(),
                                FullName = responselist[17].ToString(),
                                EmailAddress = responselist[19].ToString(),
                                PaymentType = DictionaryPhraseRepository.Current.Get("/Realty/Payment Type/Bill Paid", "Booking Amount Pay"),
                                PaymentGateway = DictionaryPhraseRepository.Current.Get("/Realty/Payment Type/GatewayName", "BillDesk")
                            };
                            this.PaymentService.StorePaymentResponse(modelviewpay);

                            //PI Service Integration


                            //TempData["PaymentResponse"] = modelviewpay;

                            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Response - ", responsemsg);
                        }
                        else
                        {
                            //error response
                            var modelviewpay = new PropertyCollection()
                            {
                                TransactionId = responselist[2].ToString(),
                                PaymentStatus = Constants.PaymentResponse.Failure,
                                Responsecode = responselist[14].ToString(), // ErrorStatus
                                msg = responselist[24].ToString(), //DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                                PaymentRef = responselist[3].ToString(),
                                OrderId = responselist[1].ToString(),
                                PaymentAmount = responselist[4].ToString(),
                                ResponseMessage = responsemsg,
                                FullName = responselist[17].ToString(),
                                EmailAddress = responselist[19].ToString(),
                                PaymentMode = responselist[9].ToString(),
                                TransactionDate = responselist[13].ToString()
                            };
                            this.PaymentService.StorePaymentResponse(modelviewpay);
                            Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Response Failure - ", responsemsg);
                        }
                    }
                    else
                    {
                        //Checksum Mismatch
                        var modelviewpay = new PropertyCollection()
                        {
                            TransactionId = responselist[2].ToString(),
                            PaymentStatus = Constants.PaymentResponse.Failure,
                            Responsecode = responselist[14].ToString(), // ErrorStatus
                            msg = DictionaryPhraseRepository.Current.Get("/Realty/BookNow/Technical Problem Msg", "Technically difficulty in the system. Please contact customer care for more details."),
                            PaymentRef = responselist[3].ToString(),
                            OrderId = responselist[1].ToString(),
                            PaymentAmount = responselist[4].ToString(),
                            ResponseMessage = responsemsg,
                            FullName = responselist[17].ToString(),
                            PaymentMode = responselist[9].ToString(),
                            EmailAddress = responselist[19].ToString(),
                            TransactionDate = responselist[13].ToString()
                        };

                        this.PaymentService.StorePaymentResponse(modelviewpay);
                        Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Response checksum mismatch - " + responsemsg, this);
                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Info("Payment Gateway- BillDeskCallBackS2S Response NULL error - " + responsemsg, this);
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at BillDeskCallBack - :" + ex.Message, this);
            }
        }
        public void SendMailforBookNow(PropertyCollection model)
        {
            Item mailconfig = Context.Database.GetItem(Templates.MailConfiguration.MailConfigurationItemID);
            Log.Info("Payment Success mail sending to client", this);
            string CustomerFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_MailFrom].Value;
            string CustomerTo = model.EmailAddress;
            string CustomerSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SubjectName].Value;
            string CustomerMailBody = string.Empty;

            string OfficialsFrom = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_MailFrom].Value;
            string OfficialsTo = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_RecipientMail].Value;
            string OfficialsMailBody = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_Message].Value;
            string OfficialsSubject = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Officials_SubjectName].Value;
            model.PaymentAmount = model.PaymentAmount.TrimStart(new Char[] { '0' });

            if (model.PaymentStatus == Constants.PaymentResponse.Success)
            {
                CustomerMailBody = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_SuccessMessage].Value;
            }
            else
            {
                CustomerMailBody = mailconfig.Fields[Templates.MailConfiguration.MailConfigurationFields.Customer_FailureMessage].Value;
                CustomerMailBody = CustomerMailBody.Replace("$remark", model.msg);
            }
            using (RealtyBookNowFormDataContext dbcontext = new RealtyBookNowFormDataContext())
            {
                RealtyBookNowForm ctx = dbcontext.RealtyBookNowForms.Where(x => x.FirstName + " " + x.LastName == model.FullName && x.OrderId == model.OrderId).FirstOrDefault();
                //customer mail body
                CustomerMailBody = CustomerMailBody.Replace("$name", model.FullName);
                CustomerMailBody = CustomerMailBody.Replace("$paymentvalue", model.PaymentAmount);
                CustomerMailBody = CustomerMailBody.Replace("$status", model.PaymentStatus);
                CustomerMailBody = CustomerMailBody.Replace("$propertyname", ctx.PropertyName);
                CustomerMailBody = CustomerMailBody.Replace("$propertytype", ctx.PropertyType);
                CustomerMailBody = CustomerMailBody.Replace("$transactionid", model.TransactionId);
                CustomerMailBody = CustomerMailBody.Replace("$orderid", model.OrderId);
                //officials mail body
                OfficialsMailBody = OfficialsMailBody.Replace("$name", model.FullName);
                OfficialsMailBody = OfficialsMailBody.Replace("$paymentvalue", model.PaymentAmount);
                OfficialsMailBody = OfficialsMailBody.Replace("$status", model.PaymentStatus);
                OfficialsMailBody = OfficialsMailBody.Replace("$propertyname", ctx.PropertyName);
                OfficialsMailBody = OfficialsMailBody.Replace("$propertytype", ctx.PropertyType);
                OfficialsMailBody = OfficialsMailBody.Replace("$transactionid", model.TransactionId);
                OfficialsMailBody = OfficialsMailBody.Replace("$orderid", model.OrderId);
                OfficialsMailBody = OfficialsMailBody.Replace("$remark", model.msg);
            }
            var mailSendingCust = sendEmail(CustomerTo, CustomerSubject, CustomerMailBody, CustomerFrom);
            if (mailSendingCust == true)
            {
                Log.Info("Sending mail to customer is Successfull", this);
            }
            else
            {
                Log.Info("Sending mail to customer is Failed", this);
            }
            var mailSendingOfc = sendEmail(OfficialsTo, OfficialsSubject, OfficialsMailBody, OfficialsFrom);
            if (mailSendingOfc == true)
            {
                Log.Info("Sending mail to Officials is Successfull", this);
            }
            else
            {
                Log.Info("Sending mail to Officials is Failed", this);
            }
        }
        public bool sendEmail(string to, string subject, string body, string from)
        {
            bool status = false;
            try
            {
                var mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(to);
                mail.Subject = subject;
                mail.Body = body;
                mail.IsBodyHtml = true;
                mail.From = new MailAddress(from);
                MainUtil.SendMail(mail);
                status = true;
                return status;
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex.Message, "sendEmail - ");
                Log.Error(ex.Message, "sendEmail - ");
                Log.Error(ex.InnerException.ToString(), "sendEmail - ");
                return status;
            }
        }

        [HttpGet]
        public ActionResult Search(RealtyFilter param = null)
        {

            IQueryable<Data.Items.Item> result;

            var qry = param.qry;
            var proj = param.project;
            var type = param.type;
            var status = param.status;
            var minarea = param.minarea;
            var maxarea = param.maxarea;
            var minbudget = param.minbudget;
            var maxbudget = param.maxbudget;
            int defaultval, defqueryval;
            var properties = Sitecore.Context.Site.GetStartItem();

            #region Filter by City
            result = properties.Axes.GetDescendants().Where(x => x.TemplateID == Templates.ResidentialProjects.ID || x.TemplateID == Templates.CommercialProperty.ID).AsQueryable();
            IQueryable<Item> results = null;
            if (!string.IsNullOrEmpty(qry))
            {
                List<SelectListItem> lstLocation = new List<SelectListItem>();
                var locations = Sitecore.Context.Database.GetItem(Templates.RealtyLocations.RootItem);
                string locationid = string.Empty;
                foreach (var item in locations.Axes.GetDescendants().ToList())
                {
                    lstLocation.Add(new SelectListItem() { Text = item.Fields[Templates.SingleText.Fields.Text].Value, Value = item.ID.ToString() });
                }

                var locationfilter = lstLocation.Where(x => x.Text.ToLower().Contains(qry.ToLower())).Any();
                if (locationfilter)
                {
                    locationid = lstLocation.Where(x => x.Text.ToLower().Contains(qry.ToLower())).FirstOrDefault().Value;
                }

                results = result.Where(x => x.Fields[Templates.ResidentialProjects.Fields.Location].Value.Contains(locationid) && (!string.IsNullOrEmpty(locationid))
                || (!string.IsNullOrEmpty(locationid) && x.Fields[Templates.CommercialProperty.Fields.Location].Value.Contains(locationid))).AsQueryable();

            }
            #endregion

            #region Filter by Project Name  
            if (!string.IsNullOrEmpty(qry))
            {
                List<SelectListItem> lstLocation = new List<SelectListItem>();
                var projects = Sitecore.Context.Database.GetItem(Templates.RealtyProject.RootItem);
                string locationid = string.Empty;
                foreach (var item in projects.Axes.GetDescendants().ToList())
                {
                    lstLocation.Add(new SelectListItem() { Text = item.Fields[Templates.SingleText.Fields.Text].Value, Value = item.ID.ToString() });
                }

                var locationfilter = lstLocation.Where(x => x.Text.ToLower().Contains(qry.ToLower())).Any();
                if (locationfilter)
                {
                    locationid = lstLocation.Where(x => x.Text.ToLower().Contains(qry.ToLower())).FirstOrDefault().Value;
                }
                var results2 = result.Where(x => x.Fields[Templates._HasPageContent.Fields.Title].Value.ToLower().Contains(qry.ToLower())).AsQueryable();
                if (results.Any() && results2.Any())
                {
                    results.Concat(results2);
                    result = results;
                }
                else if (!results.Any() && results2.Any())
                {
                    result = results2;
                }
                else
                {
                    result = results;
                }

            }
            #endregion


            #region Filter by Property Type
            if (!string.IsNullOrEmpty(type))
            {
                result = FilterRecordbyPropertytype(type, result);

            }
            #endregion

            #region Filter by Status
            if (!string.IsNullOrEmpty(status))
            {

                result = result.Where(x => x.Fields[Templates.ResidentialProjects.Fields.ProjectStatus].Value.Contains(status)
                    || x.Fields[Templates.CommercialProperty.Fields.ProjectStatus].Value.Contains(status)
                        ).AsQueryable();
            }
            #endregion

            #region Filter by Area Property
            if (!string.IsNullOrEmpty(minarea))
            {

                int minar = int.TryParse(minarea, out defaultval) ? defaultval : 0;

                result = result.Where(x => (!string.IsNullOrEmpty(x.Fields[Templates.ResidentialProjects.Fields.Area].Value)
                            && (int.TryParse(x.Fields[Templates.ResidentialProjects.Fields.Area].Value, out defqueryval) ? defqueryval : 0) >= minar)
                        || (!string.IsNullOrEmpty(x.Fields[Templates.CommercialProperty.Fields.Area].Value)
                            && (int.TryParse(x.Fields[Templates.CommercialProperty.Fields.Area].Value, out defqueryval) ? defqueryval : 0) >= minar)
                   ).AsQueryable();
            }
            if (!string.IsNullOrEmpty(maxarea))
            {

                int maxar = int.TryParse(maxarea, out defaultval) ? defaultval : 0;

                result = result.Where(x => (!string.IsNullOrEmpty(x.Fields[Templates.ResidentialProjects.Fields.Area].Value)
                        && (int.TryParse(x.Fields[Templates.ResidentialProjects.Fields.Area].Value, out defqueryval) ? defqueryval : 0) <= maxar)
                     || (!string.IsNullOrEmpty(x.Fields[Templates.CommercialProperty.Fields.Area].Value)
                        && (int.TryParse(x.Fields[Templates.CommercialProperty.Fields.Area].Value, out defqueryval) ? defqueryval : 0) <= maxar)
                     ).AsQueryable();
            }
            #endregion

            #region Filter by Budget Property
            if (!string.IsNullOrEmpty(minbudget))
            {

                int minbdt = int.TryParse(minbudget, out defaultval) ? defaultval : 0;

                result = result.Where(x => (!string.IsNullOrEmpty(x.Fields[Templates.ResidentialProjects.Fields.Price].Value)
                     && (int.TryParse(x.Fields[Templates.ResidentialProjects.Fields.Price].Value, out defqueryval) ? defqueryval : 0) >= minbdt)
                     || (!string.IsNullOrEmpty(x.Fields[Templates.CommercialProperty.Fields.Price].Value)
                     && (int.TryParse(x.Fields[Templates.CommercialProperty.Fields.Price].Value, out defqueryval) ? defqueryval : 0) >= minbdt)
                     ).AsQueryable();

            }
            if (!string.IsNullOrEmpty(maxbudget))
            {

                int maxnbdt = int.TryParse(maxbudget, out defaultval) ? defaultval : 0;

                result = result.Where(x => (!string.IsNullOrEmpty(x.Fields[Templates.ResidentialProjects.Fields.Price].Value)
                   && (int.TryParse(x.Fields[Templates.ResidentialProjects.Fields.Price].Value, out defqueryval) ? defqueryval : 0) <= maxnbdt)
                   || (!string.IsNullOrEmpty(x.Fields[Templates.CommercialProperty.Fields.Price].Value)
                   && (int.TryParse(x.Fields[Templates.CommercialProperty.Fields.Price].Value, out defqueryval) ? defqueryval : 0) <= maxnbdt)
                   ).AsQueryable();
            }
            #endregion

            return View("Search", result);
        }

        protected IQueryable<Sitecore.Data.Items.Item> FilterRecordbyPropertytype(string type, IQueryable<Sitecore.Data.Items.Item> result)
        {

            var resi = Sitecore.Context.Database.GetItem(Templates.RealtyPropertyTypeText.Residential);
            var comm = Sitecore.Context.Database.GetItem(Templates.RealtyPropertyTypeText.Commercial);
            if (type == resi.Fields[Templates.SingleText.Fields.Text].Value)
            {
                result = result.Where(x => x.TemplateID == Templates.ResidentialProjects.ID).AsQueryable();
            }
            if (type == comm.Fields[Templates.SingleText.Fields.Text].Value)
            {
                result = result.Where(x => x.TemplateID == Templates.CommercialProperty.ID).AsQueryable();
            }
            return result;
        }

        [HttpGet]
        public ActionResult RealtyFilter(RealtyFilter param = null)
        {
            var RealtyType = Sitecore.Context.Database.GetItem(Templates.RealtyType.GlobalRootFolderID);
            var RealtyProjectStatus = Sitecore.Context.Database.GetItem(Templates.RealtyProjectStatus.GlobalRootFolderID);
            var RealtyArea = Sitecore.Context.Database.GetItem(Templates.RealtyArea.GlobalRootFolderID);
            var RealtyBudget = Sitecore.Context.Database.GetItem(Templates.RealtyBudget.GlobalRootFolderID);

            if (param == null)
                param = new RealtyFilter();

            #region Property List Binding
            var propList = new List<SelectListItem>();
            RealtyType.Children.ToList().ForEach(x =>
            {
                propList.Add(new SelectListItem { Text = x.Fields[Templates.RealtyType.Fields.Text].Value, Value = x.Fields[Templates.RealtyType.Fields.Text].Value });
            });
            param.PropertyTypeList = propList;
            #endregion

            #region Status List Binding
            var statusList = new List<SelectListItem>();
            RealtyProjectStatus.Children.ToList().ForEach(x =>
            {
                statusList.Add(new SelectListItem { Text = x.Fields[Templates.RealtyProjectStatus.Fields.Text].Value, Value = x.ID.ToString() });
            });
            param.ProjectStatusList = statusList;
            #endregion

            #region Min /Max Area Binding
            var maximumarealist = new List<SelectListItem>();
            var minimumarealist = new List<SelectListItem>();
            RealtyArea.Children.ToList().ForEach(x =>
            {
                maximumarealist.Add(new SelectListItem { Text = x.Fields[Templates.RealtyArea.Fields.Text].Value, Value = x.Fields[Templates.RealtyArea.Fields.Value].Value });
                minimumarealist.Add(new SelectListItem { Text = x.Fields[Templates.RealtyArea.Fields.Text].Value, Value = x.Fields[Templates.RealtyArea.Fields.Value].Value });
            });
            param.MaximumAreaList = maximumarealist;
            param.MinimumAreaList = maximumarealist;
            #endregion

            #region Min/Max Budget Binding
            var maximumbudgetlist = new List<SelectListItem>();
            var minimumbudgetlist = new List<SelectListItem>();
            RealtyBudget.Children.ToList().ForEach(x =>
            {
                maximumbudgetlist.Add(new SelectListItem { Text = x.Fields[Templates.RealtyBudget.Fields.Text].Value, Value = x.Fields[Templates.RealtyBudget.Fields.Value].Value });
                minimumbudgetlist.Add(new SelectListItem { Text = x.Fields[Templates.RealtyBudget.Fields.Text].Value, Value = x.Fields[Templates.RealtyBudget.Fields.Value].Value });
            });
            param.MaximumBudgetList = maximumbudgetlist;
            param.MinimumBudgetList = minimumbudgetlist;
            #endregion

            return View("RealtyFilter", param);
        }


        [HttpGet]
        public ActionResult Enquiry()
        {
            return View("~/Views/Realty/Sublayouts/EnquiryNowForm.cshtml");
        }

        [HttpPost]
        public ActionResult Enquiry(EnquiryModel model)
        {
            EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0" };

            try
            {

                #region Delete Available otp from database for given mobile number

                //realtyRepo.DeleteOldOtp(model.mobile);
                #endregion
                model.OtpPurpose = PurposeOfOtp.Enquiry;
                #region Generate New Otp for given mobile number and save to database
                string generatedotp = realtyRepo.StoreGeneratedOtp(model, System.Convert.ToInt32(ConfigurationManager.AppSettings["OtpLifeinMinutes"]), System.Convert.ToInt32(ConfigurationManager.AppSettings["MaxOtpAttempts"]));
                #endregion
                if (generatedotp == "0")
                {
                    result = new { status = "2"};
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                //ConfigurationSettings.AppSettings["OtpLifeinMinutes"]
                #region Api call to send SMS of OTP
                try
                {
                    //  var apiurl = string.Format("https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=adtrotpi&subEnterpriseid=adtrotpi&pusheid=adtrotpi&pushepwd=adtrotpi22&msisdn={0}&sender=ADANIR&msgtext=Dear%20customer,%20please%20enter%20the%20verification%20code%20{1}%20to%20submit%20your%20web%20enquiry.", model.mobile, generatedotp);

                    //  var apiurl = string.Format("https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to={0}&msg=Hi%20{1},%20{2}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&msg_type=TEXT&userid=2000150775&auth_scheme=plain&password=VwAUWeZq&v=1.1&format=text", model.mobile, model.first_name, generatedotp);

                    var apiurl = string.Format(" https://bulksms.analyticsmantra.com/sendsms/sendsms.php?username=ADANITRAN&password=tech321&type=TEXT&sender=ADRLTY&mobile={0}&message=Hi%20%20{1}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&PEID=1601100000000013196&HeaderId=1605001594131761200&templateId=1607100000000117984", model.mobile, generatedotp);



                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Error("OTP Api call success. https://enterprise.smsgupshup.com/", this);
                    }
                    else
                    {
                        Log.Error("OTP Api call failed. https://enterprise.smsgupshup.com/", this);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"{0}", ex, this);
                }
                #endregion

                #region Return Response with Mobile Number and Generated otp
                result = new { status = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);

                //return new JsonResult() { Data = new { MobileNumber = model.Mobile,otp= generatedotp, message = "" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpGet]
        public ActionResult westernheight_Enquiry()
        {
            return View("~/Views/Realty/Sublayouts/westernheight_EnquireNow.cshtml");
        }

        [HttpPost]
        public ActionResult westernheight_Enquiry(EnquiryModel model)
        {
            EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0" };

            try
            {

                #region Delete Available otp from database for given mobile number

                //realtyRepo.DeleteOldOtp(model.mobile);
                #endregion
                model.OtpPurpose = PurposeOfOtp.Enquiry;
                #region Generate New Otp for given mobile number and save to database
                string generatedotp = realtyRepo.StoreGeneratedOtp(model, System.Convert.ToInt32(ConfigurationManager.AppSettings["OtpLifeinMinutes"]), System.Convert.ToInt32(ConfigurationManager.AppSettings["MaxOtpAttempts"]));
                #endregion
                if (generatedotp == "0")
                {
                    result = new { status = "2" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                #region Api call to send SMS of OTP
                try
                {
                    // var apiurl = string.Format("https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=adtrotpi&subEnterpriseid=adtrotpi&pusheid=adtrotpi&pushepwd=adtrotpi22&msisdn={0}&sender=ADANIR&msgtext=Dear%20customer,%20please%20enter%20the%20verification%20code%20{1}%20to%20submit%20your%20web%20enquiry.", model.mobile, generatedotp);
                    // var apiurl = string.Format("https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to={0}&msg=Hi%20{1},%20{2}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&msg_type=TEXT&userid=2000150775&auth_scheme=plain&password=VwAUWeZq&v=1.1&format=text", model.mobile, model.first_name, generatedotp);

                    var apiurl = string.Format(" https://bulksms.analyticsmantra.com/sendsms/sendsms.php?username=ADANITRAN&password=tech321&type=TEXT&sender=ADRLTY&mobile={0}&message=Hi%20%20{1}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&PEID=1601100000000013196&HeaderId=1605001594131761200&templateId=1607100000000117984", model.mobile, generatedotp);

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Error("OTP Api call success. https://enterprise.smsgupshup.com/", this);
                    }
                    else
                    {
                        Log.Error("OTP Api call failed. https://enterprise.smsgupshup.com/", this);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"{0}", ex, this);
                }
                #endregion

                #region Return Response with Mobile Number and Generated otp
                result = new { status = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);

                //return new JsonResult() { Data = new { MobileNumber = model.Mobile,otp= generatedotp, message = "" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public ActionResult Codegreen_Enquiry()
        {
            return View("~/Views/Realty/Sublayouts/codegreen_EnquireNow.cshtml");
        }

        [HttpPost]
        public ActionResult Codegreen_Enquiry(EnquiryModel model)
        {
            EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0" };

            try
            {

                #region Delete Available otp from database for given mobile number

                //realtyRepo.DeleteOldOtp(model.mobile);
                #endregion
                model.OtpPurpose = PurposeOfOtp.Enquiry;
                #region Generate New Otp for given mobile number and save to database
                string generatedotp = realtyRepo.StoreGeneratedOtp(model, System.Convert.ToInt32(ConfigurationManager.AppSettings["OtpLifeinMinutes"]), System.Convert.ToInt32(ConfigurationManager.AppSettings["MaxOtpAttempts"]));
                #endregion
                if (generatedotp == "0")
                {
                    result = new { status = "2" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                #region Api call to send SMS of OTP
                try
                {
                    // var apiurl = string.Format("https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=adtrotpi&subEnterpriseid=adtrotpi&pusheid=adtrotpi&pushepwd=adtrotpi22&msisdn={0}&sender=ADANIR&msgtext=Dear%20customer,%20please%20enter%20the%20verification%20code%20{1}%20to%20submit%20your%20web%20enquiry.", model.mobile, generatedotp);

                    //var apiurl = string.Format("https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to={0}&msg=Hi%20{1},%20{2}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&msg_type=TEXT&userid=2000150775&auth_scheme=plain&password=VwAUWeZq&v=1.1&format=text", model.mobile, model.first_name, generatedotp);

                    var apiurl = string.Format(" https://bulksms.analyticsmantra.com/sendsms/sendsms.php?username=ADANITRAN&password=tech321&type=TEXT&sender=ADRLTY&mobile={0}&message=Hi%20%20{1}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&PEID=1601100000000013196&HeaderId=1605001594131761200&templateId=1607100000000117984", model.mobile, generatedotp);


                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Error("OTP Api call success. https://enterprise.smsgupshup.com/", this);
                    }
                    else
                    {
                        Log.Error("OTP Api call failed. https://enterprise.smsgupshup.com/", this);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"{0}", ex, this);
                }
                #endregion

                #region Return Response with Mobile Number and Generated otp
                result = new { status = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);

                //return new JsonResult() { Data = new { MobileNumber = model.Mobile,otp= generatedotp, message = "" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpGet]
        public ActionResult Contact()
        {
            return View("~/Views/Realty/RealtyContactForm.cshtml");
        }

        [HttpPost]

        public ActionResult Contact(EnquiryModel model)
        {
            EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0", mobilenumber = string.Empty, otp = string.Empty, message = "" };

            try
            {

                #region Delete Available otp from database for given mobile number

                //realtyRepo.DeleteOldOtp(model.mobile);
                #endregion
                model.OtpPurpose = PurposeOfOtp.Enquiry;
                #region Generate New Otp for given mobile number and save to database
                string generatedotp = realtyRepo.StoreGeneratedOtp(model, System.Convert.ToInt32(ConfigurationManager.AppSettings["OtpLifeinMinutes"]), System.Convert.ToInt32(ConfigurationManager.AppSettings["MaxOtpAttempts"]));
                #endregion
                if(generatedotp=="0")
                {
                    result = new { status = "2",mobilenumber = model.mobile, otp = generatedotp, message = "" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                #region Api call to send SMS of OTP
                try
                {
                    //var apiurl = string.Format("https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=adtrotpi&subEnterpriseid=adtrotpi&pusheid=adtrotpi&pushepwd=adtrotpi22&msisdn={0}&sender=ADANIR&msgtext=Dear%20customer,%20please%20enter%20the%20verification%20code%20{1}%20to%20submit%20your%20web%20enquiry.", model.mobile, generatedotp);

                    //var apiurl = string.Format("https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to={0}&msg=Hi%20{1},%20{2}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&msg_type=TEXT&userid=2000150775&auth_scheme=plain&password=VwAUWeZq&v=1.1&format=text", model.mobile, model.first_name, generatedotp);

                    var apiurl = string.Format(" https://bulksms.analyticsmantra.com/sendsms/sendsms.php?username=ADANITRAN&password=tech321&type=TEXT&sender=ADRLTY&mobile={0}&message=Hi%20%20{1}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&PEID=1601100000000013196&HeaderId=1605001594131761200&templateId=1607100000000117984", model.mobile, generatedotp);


                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"{0}", ex, this);
                }
                #endregion

                #region Return Response with Mobile Number and Generated otp
                result = new { status = "1", mobilenumber = model.mobile, otp = generatedotp, message = "" };
                return Json(result, JsonRequestBehavior.AllowGet);

                //return new JsonResult() { Data = new { MobileNumber = model.Mobile,otp= generatedotp, message = "" }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult VerifyOTP(EnquiryModel model)
        {
            var result = new { status = "0" };
          
                #region Verify OTP
                string generatedOTP = realtyRepo.GetOTP(model.mobile,PurposeOfOtp.Enquiry);
                if (string.Equals(generatedOTP, model.OTP))
                {
                    result = new { status = "1" };
                }
            
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public bool VerifyEnquiryOTP(EnquiryModel model)
        {

            #region Verify OTP
            string generatedOTP = realtyRepo.GetOTP(model.mobile, PurposeOfOtp.Enquiry);
            if (string.Equals(generatedOTP, model.OTP))
            {
                return true;
            }

            #endregion
            return false;
        }

        [HttpGet]
        public ActionResult GetCityAndProjectSuggestion()
        {
            var LocationName = Sitecore.Context.Database.GetItem(Templates.RealtyLocations.RootItem);
            var projectsName = Sitecore.Context.Database.GetItem(Templates.RealtyProject.RootItem);

            var Locationresult = new List<string>();
            var Projectresult = new List<string>();
            foreach (var item in LocationName.Children.ToList())
            {
                Locationresult.Add(item.Fields[Templates.RealtyLocations.Fields.Text].Value);
            }
            foreach (var item in projectsName.Children.ToList())
            {
                Projectresult.Add(item.Fields[Templates.RealtyLocations.Fields.Text].Value);
            }
            return Json(new { location = Locationresult, projectname = Projectresult }, JsonRequestBehavior.AllowGet); ;
        }

        [HttpPost]
        public ActionResult InsertDetailMoveInShantigram(EnquiryModel m)
        {
            var result = new { status = "1" };
            try
            {
                RealtyDataContext rdb = new RealtyDataContext();
                ConsolidateFormData r = new ConsolidateFormData();

                r.FirstName = m.first_name;
                r.LastName = m.last_name;
                r.Mobile = m.mobile;
                r.Email = m.email;
                r.Country = m.country_code;
                r.State = m.state_code;
                r.PropertyType = m.Projects_Interested__c;
                r.PropertyLocation = m.PropertyLocation;
                r.FormType = m.FormType;
                r.PageInfo = m.PageInfo;
                r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                r.UTMSource = m.UTMSource;
                r.City = m.city;

                #region Insert to DB
                rdb.ConsolidateFormDatas.InsertOnSubmit(r);

                rdb.SubmitChanges();
                Sitecore.Diagnostics.Log.Info("InsertDetailMoveInShantigram - Data added to DB Start first_name:" + m.first_name, this);
                Sitecore.Diagnostics.Log.Info("InsertDetailMoveInShantigram - Data added to DB last_name:" + m.last_name, this);
                Sitecore.Diagnostics.Log.Info("InsertDetailMoveInShantigram - Data added to DB Mobile: " + m.mobile, this);
                Sitecore.Diagnostics.Log.Info("InsertDetailMoveInShantigram - Data added to DB email: " + m.email, this);
                Sitecore.Diagnostics.Log.Info("InsertDetailMoveInShantigram - Data added to DB Projects_Interested__c: " + m.Projects_Interested__c, this);
                Sitecore.Diagnostics.Log.Info("InsertDetailMoveInShantigram - Data added to DB MasterProjectID: " + m.PropertyCode, this);
                Sitecore.Diagnostics.Log.Info("InsertDetailMoveInShantigram - Data added to DB RecordType: " + m.RecordType, this);
                Sitecore.Diagnostics.Log.Info("InsertDetailMoveInShantigram - Data added to DB PageInfo: " + m.PageInfo, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                //Get project details
                Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                LeadObject obj = new LeadObject
                {
                    Firstname = m.first_name,
                    FormType = m.FormType,
                    PageInfo = m.PageInfo,
                    LastName = m.last_name,
                    Email = m.email,
                    Mobile = m.mobile,
                    Country = m.country_code,
                    Remarks = m.Remarks,
                    Project = m.PropertyLocation,
                    Saletype = m.sale_type,
                    Projectintrested = m.Projects_Interested__c,
                    MasterProjectID = m.PropertyCode,
                    AssignmentCity = m.city,
                    LeadSource = m.LeadSource,
                    UtmSource = m.UTMSource,
                    RecordType = m.RecordType,
                    Ads = m.AdvertisementId
                };
                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                var leadResult = SalesForceWrapperObj.GenerateLead(obj);

                result = new { status = "1" };
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult Insertcontactdetail(EnquiryModel m)
        {
            var result = new { status = "1" };
            try
            {
                if (this.VerifyEnquiryOTP(m))
                {

                    RealtyDataContext rdb = new RealtyDataContext();
                    ConsolidateFormData r = new ConsolidateFormData();

                    r.FirstName = m.first_name;
                    r.LastName = m.last_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.Budget = string.IsNullOrEmpty(m.Budget) ? 0 : decimal.Parse(m.Budget);
                    r.Country = m.country_code;
                    r.State = m.state_code;
                    r.PropertyType = m.Projects_Interested__c;
                    r.PropertyLocation = m.PropertyLocation;
                    r.sale_type = m.sale_type;
                    r.Comment = m.Remarks;
                    r.FormType = m.FormType;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    r.City = m.city;
                    r.Lead_Sub_Source = m.UTMPlacement;

                    #region Insert to DB
                    rdb.ConsolidateFormDatas.InsertOnSubmit(r);

                    rdb.SubmitChanges();
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.first_name, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB last_name:" + m.last_name, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Projects_Interested__c: " + m.Projects_Interested__c, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB MasterProjectID: " + m.PropertyCode, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB RecordType: " + m.RecordType, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject obj = new LeadObject
                    {
                        Firstname = m.first_name,
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        LastName = m.last_name,
                        Email = m.email,
                        Mobile = m.mobile,
                        Budget = m.Budget,
                        State = m.state_code,
                        Country = m.country_code,
                        Remarks = m.Remarks,
                        Project = m.PropertyLocation,
                        Saletype = m.sale_type,
                        Projectintrested = m.Projects_Interested__c,
                        MasterProjectID = m.PropertyCode,
                        AssignmentCity = m.city,
                        //ProjectId = m.PropertyCode,
                        UtmSource = m.UTMSource,
                        RecordType = m.RecordType,
                        LeadSource = m.LeadSource,
                        UtmPlacement = m.UTMPlacement,
                        Ads = m.AdvertisementId
                    };

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    result = new { status = "1" };
                }
                else {
                    result = new { status = "2" };
                }
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult UserLogin(Login model)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("inside userlogin post", this);
                var item = Context.Database.GetItem(Templates.LeadGeneration.LeadCreation);
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }
                Sitecore.Diagnostics.Log.Info("inside userlogin post 1", this);
                using (RealtyDataContext dbcontext = new RealtyDataContext())
                {
                    Sitecore.Diagnostics.Log.Info("inside userlogin post 2", this);
                    var registerUser = dbcontext.Registrations.Where(x => x.UserId == model.LoginName && x.Password == model.Password && x.UserType.ToLower() == "realty" && x.status == true).FirstOrDefault();
                    if (registerUser == null)
                    {
                        ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/Realty/UserLogin/Login User Error", "User and password is not valid. Please enter valid credential."));
                        return this.View(model);
                    }
                    Sitecore.Diagnostics.Log.Info("inside userlogin post 3", this);
                    UserSession.UserSessionContext = new LoginModel
                    {
                        userId = registerUser.UserId,
                        leadCity = registerUser.City,
                        UserType = registerUser.UserType
                    };
                    Sitecore.Diagnostics.Log.Info("inside userlogin post 4", this);
                    var url = item.Url();
                    return this.Redirect(url);
                }
            }
            catch (System.Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at UserLoginLogin Post:" + ex.Message, this);
                ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/Realty/UserLogin/Login Technical Error", "There is technical problem. Please try after sometime."));
                return this.View(model);
            }
        }

        [HttpPost]
        public ActionResult Logout()
        {
            var item = Context.Database.GetItem(Templates.LeadGeneration.LogOut);
            if (UserSession.UserSessionContext != null && !string.IsNullOrEmpty(UserSession.UserSessionContext.userId))
            {
                using (RealtyDataContext dbcontext = new RealtyDataContext())
                {
                    var result = (from user in dbcontext.Registrations where user.UserId == UserSession.UserSessionContext.userId select user).Single();
                    result.Modified_Date = System.DateTime.Now;
                    result.ModifiedBy = UserSession.UserSessionContext.userId;
                    dbcontext.SubmitChanges();
                }
            }
            this.Session["UserLogin"] = null;
            UserSession.UserSessionContext = null;
            return this.Redirect(item.Url());
        }

        [HttpGet]
        public ActionResult LeadGeneration()
        {
            Sitecore.Diagnostics.Log.Info("inside LeadGeneration get", this);
            if (UserSession.UserSessionContext == null || string.IsNullOrEmpty(UserSession.UserSessionContext.userId))
            {
                this.Session["UserLogin"] = null;
                UserSession.UserSessionContext = null;
                var item = Context.Database.GetItem(Templates.LeadGeneration.LogOut);
                return this.Redirect(item.Url());
            }
            LeadGenerationModel m = new LeadGenerationModel();
            m.LeadRecordType = new List<SelectListItem>();
            m.LeadRecordType.Add(new SelectListItem { Text = "Residential", Value = "Residential" });
            m.LeadRecordType.Add(new SelectListItem { Text = "Commercial", Value = "Commercial" });

            m.AssignmentCity = new List<SelectListItem>();
            m.AssignmentCity.Add(new SelectListItem { Text = "Ahmedabad", Value = "Ahmedabad" });
            m.AssignmentCity.Add(new SelectListItem { Text = "Gurgaon", Value = "Gurgaon" });
            m.AssignmentCity.Add(new SelectListItem { Text = "Mumbai", Value = "Mumbai" });
            m.AssignmentCity.Add(new SelectListItem { Text = "Pune", Value = "Pune" });
            m.SelectedAssignmentCity = UserSession.UserSessionContext.leadCity;

            m.LeadSource = new List<SelectListItem>();
            m.LeadSource.Add(new SelectListItem { Text = "Direct Walk-in", Value = "Direct Walk-in" });
            m.LeadSource.Add(new SelectListItem { Text = "Employee ", Value = "Employee" });
            m.LeadSource.Add(new SelectListItem { Text = "Employee Referral", Value = "Employee Referral" });
            m.LeadSource.Add(new SelectListItem { Text = "Channel Partner Lead", Value = "Channel Partner Lead" });

            m.LeadSource.Add(new SelectListItem
            {
                Text = "Existing Customer reference",
                Value = "Existing Customer reference",
            });
            m.LeadSource.Add(new SelectListItem
            {
                Text = "Digital(Email & Website)",
                Value = "Digital(Email & Website)",
            });
            m.LeadSource.Add(new SelectListItem
            {
                Text = "Word of Mouth",
                Value = "Word of Mouth",
            });
            m.LeadSource.Add(new SelectListItem
            {
                Text = "Advertisements(News paper & Insert)",
                Value = "Advertisements(News paper & Insert)",
            });
            m.LeadSource.Add(new SelectListItem
            {
                Text = "Business Development",
                Value = "Business Development",
            });
            m.LeadSource.Add(new SelectListItem
            {
                Text = "Secondary Data",
                Value = "Secondary Data",
            });
            m.LeadSource.Add(new SelectListItem
            {
                Text = "Events & Exhibition)",
                Value = "Events & Exhibition)",
            });
            m.LeadSource.Add(new SelectListItem
            {
                Text = "Social(Facebook, Linkedin Etc)",
                Value = "Word of Mouth",
            });
            m.LeadSource.Add(new SelectListItem
            {
                Text = "SMS",
                Value = "SMS",
            });
            m.LeadSource.Add(new SelectListItem
            {
                Text = "Outbound Calls",
                Value = "Outbound Calls",
            });
            m.LeadSource.Add(new SelectListItem
            {
                Text = "Web to Lead",
                Value = "Web to Lead",
            });

            m.LeadStatus = new List<SelectListItem>();
            m.LeadStatus.Add(new SelectListItem { Text = "Open", Value = "Open" });
            m.LeadStatus.Add(new SelectListItem { Text = "Contacted", Value = "Contacted" });
            m.LeadStatus.Add(new SelectListItem { Text = "Contacted – No Response", Value = "Contacted – No Response" });
            m.LeadStatus.Add(new SelectListItem { Text = "Closed Lost", Value = "Closed Lost" });
            m.LeadStatus.Add(new SelectListItem { Text = "Qualified", Value = "Qualified" });
            m.LeadStatus.Add(new SelectListItem { Text = "Re-Open", Value = "Re-Open" });

            m.LeadSubStatus = new List<SelectListItem>();
            m.LeadSubStatus.Add(new SelectListItem { Text = "Not Contactable", Value = "Not Contactable" });
            m.LeadSubStatus.Add(new SelectListItem { Text = "Follow up", Value = "Follow up" });
            m.LeadSubStatus.Add(new SelectListItem { Text = "Follow up – Not Contactable", Value = "Follow up – Not Contactable" });
            m.LeadSubStatus.Add(new SelectListItem { Text = "Visit Confirmed", Value = "Visit Confirmed" });
            m.LeadSubStatus.Add(new SelectListItem { Text = "Visit proposed", Value = "Visit proposed" });
            m.LeadSubStatus.Add(new SelectListItem { Text = "Visit Done", Value = "Visit Done" });
            m.LeadSubStatus.Add(new SelectListItem { Text = "Meeting Fixed", Value = "Meeting Fixed" });
            m.LeadSubStatus.Add(new SelectListItem { Text = "Meeting Done", Value = "Meeting Done" });

            m.LeadGender = new List<SelectListItem>();
            m.LeadGender.Add(new SelectListItem { Text = "Male", Value = "Male" });
            m.LeadGender.Add(new SelectListItem { Text = "Female", Value = "Female" });

            m.ReasonforPurchase = new List<SelectListItem>();
            m.ReasonforPurchase.Add(new SelectListItem { Text = "Self-Use", Value = "Self-Use" });
            m.ReasonforPurchase.Add(new SelectListItem { Text = "Investment", Value = "Investment" });

            m.PropertyStage = new List<SelectListItem>();
            m.PropertyStage.Add(new SelectListItem { Text = "Ready to Move in", Value = "Ready to Move in" });
            m.PropertyStage.Add(new SelectListItem { Text = "Under Construction", Value = "Under Construction" });

            m.ProjectList = new List<SelectListItem>();
            m.ProjectList.Add(new SelectListItem { Text = "Aangan", Value = "Aangan" });
            m.ProjectList.Add(new SelectListItem { Text = "Elysium", Value = "Elysium" });
            m.ProjectList.Add(new SelectListItem { Text = "La Marina", Value = "La Marina" });
            m.ProjectList.Add(new SelectListItem { Text = "Oyster Grande", Value = "Oyster Grande" });
            m.ProjectList.Add(new SelectListItem { Text = "The Meadows", Value = "The Meadows" });
            m.ProjectList.Add(new SelectListItem { Text = "Western Heights", Value = "Western Heights" });
            m.ProjectList.Add(new SelectListItem { Text = "Water Lily", Value = "Water Lily" });
            m.ProjectList.Add(new SelectListItem { Text = "The North Park", Value = "The North Park" });
            m.ProjectList.Add(new SelectListItem { Text = "Monte South", Value = "Monte South" });
            m.ProjectList.Add(new SelectListItem { Text = "Samsara", Value = "Samsara" });
            m.ProjectList.Add(new SelectListItem { Text = "Gurgaon - Samsara Vilasa", Value = "Gurgaon - Samsara Vilasa" });
            m.ProjectList.Add(new SelectListItem { Text = "Pratham", Value = "Pratham" });
            m.ProjectList.Add(new SelectListItem { Text = "Aangan - Shantigram", Value = "Aangan - Shantigram" });
            m.ProjectList.Add(new SelectListItem { Text = "Code Name Greens", Value = "Code Name Greens" });

            m.CountryList = new List<SelectListItem>();
            m.CountryList.Add(new SelectListItem { Text = "Australia", Value = "Australia" });
            m.CountryList.Add(new SelectListItem { Text = "Canada", Value = "Canada" });
            m.CountryList.Add(new SelectListItem { Text = "Brazil", Value = "Brazil" });
            m.CountryList.Add(new SelectListItem { Text = "China", Value = "China" });
            m.CountryList.Add(new SelectListItem { Text = "Germany", Value = "Germany" });
            m.CountryList.Add(new SelectListItem { Text = "United Kingdom", Value = "United Kingdom" });
            m.CountryList.Add(new SelectListItem { Text = "Ireland", Value = "Ireland" });
            m.CountryList.Add(new SelectListItem { Text = "India", Value = "India" });
            m.CountryList.Add(new SelectListItem { Text = "Italy", Value = "Italy" });
            m.CountryList.Add(new SelectListItem { Text = "Mexico", Value = "Mexico" });
            m.CountryList.Add(new SelectListItem { Text = "United States", Value = "United States" });

            return View(m);
        }


        // pletinum realty
        [HttpPost]
        public ActionResult PlatinumRealtySendOtp(EnquiryModel model)
        {
            //EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0" };
            PlatinumRealtyDataContext rdb = new PlatinumRealtyDataContext();
            platinumRealtyOtpHistory r = new platinumRealtyOtpHistory();
            try
            {




                var currDate = DateTime.Now;
                var beforeTime = currDate.AddMinutes(-30);
                var data = rdb.platinumRealtyOtpHistories.Where(x => x.MobileNumber == model.mobile && x.date >= beforeTime && x.date <= currDate).Count();
                if (data >= 3)
                {
                    result = new { status = "503" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                #region Delete Available otp from database for given mobile number

                //realtyRepo.DeleteOldOtp(model.mobile);
                #endregion
                model.OtpPurpose = PurposeOfOtp.Enquiry;
                #region Generate New Otp for given mobile number and save to database
                string generatedotp = realtyRepo.StoreGeneratedOtp(model, System.Convert.ToInt32(ConfigurationManager.AppSettings["OtpLifeinMinutes"]), System.Convert.ToInt32(ConfigurationManager.AppSettings["MaxOtpAttempts"]));
                #endregion
                if (generatedotp == "0")
                {
                    result = new { status = "2" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                #region Api call to send SMS of OTP
                try
                {
                    //var apiurl = string.Format("https://enterprise.smsgupshup.com/GatewayAPI/rest?method=SendMessage&send_to={0}&msg=Hi,%20{1}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&msg_type=TEXT&userid=2000150775&auth_scheme=plain&password=VwAUWeZq&v=1.1&format=text", model.mobile,generatedotp);
                    var apiurl = string.Format(" https://bulksms.analyticsmantra.com/sendsms/sendsms.php?username=ADANITRAN&password=tech321&type=TEXT&sender=ADRLTY&mobile={0}&message=Hi%20%20{1}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&PEID=1601100000000013196&HeaderId=1605001594131761200&templateId=1607100000000117984", model.mobile, generatedotp);




                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Error("OTP Api call success. https://enterprise.smsgupshup.com/", this);
                        result = new { status = "1" };
                        r.MobileNumber = model.mobile;
                        r.otp = generatedotp;
                        r.status = false;
                        r.count = 0;
                        r.date = DateTime.Now;
                        rdb.platinumRealtyOtpHistories.InsertOnSubmit(r);

                        rdb.SubmitChanges();
                    }
                    else
                    {
                        Log.Error("OTP Api call failed. https://enterprise.smsgupshup.com/", this);
                        result = new { status = "0" };
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"{0}", ex, this);
                }
                #endregion

                #region Return Response with Mobile Number and Generated otp
                //result = new { status = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);


                #endregion
            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }
        [HttpPost]
        public ActionResult InsertPlatinumRealtycontactdetail(PlatinumRealtyModel m)
        {
            var result = new { status = "0" };
            PlatinumRealtyDataContext rdb = new PlatinumRealtyDataContext();

            try
            {

                if (string.IsNullOrEmpty(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!alphaNumber.IsMatch(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (!emailRegex.IsMatch(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!NumRegex.IsMatch(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                //if (string.IsNullOrEmpty(m.OTP))
                //{
                //    result = new { status = "406" };
                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}

                //string generatedOTP = realtyRepo.GetOTP(m.mobile);
                //if (!string.Equals(generatedOTP, m.OTP))
                //{



                //    var data = rdb.platinumRealtyOtpHistories.Where(x => x.MobileNumber == m.mobile && x.otp == generatedOTP && x.status==false).FirstOrDefault();
                //    if (data != null)
                //    {
                //        var otpcount = data.count;
                //        if (otpcount >= 3)
                //        {
                //            result = new { status = "103" };
                //        }
                //        else
                //        {
                //            result = new { status = "102" };
                //        }

                //        otpcount = otpcount + 1;
                //        data.count = otpcount;


                //        rdb.SubmitChanges();
                //    }






                //    else {

                //        result = new { status = "108" };
                //    }

                //}









                else
                {


                    platinumRealtyEnquireFormData r = new platinumRealtyEnquireFormData();
                    //var data = rdb.platinumRealtyOtpHistories.Where(x => x.MobileNumber == m.mobile && x.otp == generatedOTP).FirstOrDefault();
                    //data.status = true;

                    r.FullName = m.full_name;
                    //r.LastName = m.last_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.Budget = string.IsNullOrEmpty(m.Budget) ? 0 : decimal.Parse(m.Budget);
                    r.Country = m.country_code;
                    r.State = m.state_code;
                    r.PropertyType = m.Projects_Interested__c;
                    r.PropertyLocation = m.PropertyLocation;
                    r.sale_type = m.sale_type;
                    r.Comment = m.Remarks;
                    r.FormType = m.FormType;
                    r.PageUrl = m.PageInfo;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    //r.City = m.city;
                    //r.Lead_Sub_Source = m.UTMPlacement;

                    #region Insert to DB

                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);

                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);



                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject obj = new LeadObject
                    {
                        Firstname = m.full_name,
                        LastName = "-",
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        Email = m.email,
                        Mobile = m.mobile,
                        Project = "Oyster Grande",
                        MasterProjectID = m.PropertyCode,
                        UtmSource = m.UTMSource,
                        AssignmentCity = m.PropertyLocation,
                        RecordType = m.RecordType,
                        LeadSource = "Digital",

                        Ads = m.AdvertisementId
                    };

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    r.Comment = leadResult;
                    rdb.platinumRealtyEnquireFormDatas.InsertOnSubmit(r);

                    rdb.SubmitChanges();
                    result = new { status = "101" };
                }
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        [HttpPost]
        //[ValidateAntiForgeryToken]
        public ActionResult LeadGeneration(LeadGenerationModel m)
        {
            try
            {
                bool isModelValid = true;

                DateTime scheduledFollowUpDate = new DateTime();
                if (!string.IsNullOrEmpty(m.ScheduledFollowUp))
                {
                    if (!DateTime.TryParseExact(m.ScheduledFollowUp, "dd/MM/yyyy HH:mm", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out scheduledFollowUpDate))
                    {
                        this.ModelState.AddModelError(nameof(m.ScheduledFollowUp), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/ValidDateofBirth", "Please enter valid date in dd/mm/yyyy format."));
                        isModelValid = false;
                    }
                }

                if (!ModelState.IsValid || !isModelValid)
                {
                    m.LeadRecordType = new List<SelectListItem>();
                    m.LeadRecordType.Add(new SelectListItem { Text = "Residential", Value = "Residential" });
                    m.LeadRecordType.Add(new SelectListItem { Text = "Commercial", Value = "Commercial" });

                    m.AssignmentCity = new List<SelectListItem>();
                    m.AssignmentCity.Add(new SelectListItem { Text = "Ahmedabad", Value = "Ahmedabad" });
                    m.AssignmentCity.Add(new SelectListItem { Text = "Gurgaon", Value = "Gurgaon" });
                    m.AssignmentCity.Add(new SelectListItem { Text = "Mumbai", Value = "Mumbai" });
                    m.AssignmentCity.Add(new SelectListItem { Text = "Pune", Value = "Pune" });

                    m.LeadSource = new List<SelectListItem>();
                    m.LeadSource.Add(new SelectListItem { Text = "Direct Walk-in", Value = "Direct Walk-in" });
                    m.LeadSource.Add(new SelectListItem { Text = "Employee ", Value = "Employee" });
                    m.LeadSource.Add(new SelectListItem { Text = "Employee Referral", Value = "Employee Referral" });
                    m.LeadSource.Add(new SelectListItem { Text = "Channel Partner Lead", Value = "Channel Partner Lead" });

                    m.LeadSource.Add(new SelectListItem
                    {
                        Text = "Existing Customer reference",
                        Value = "Existing Customer reference",
                    });
                    m.LeadSource.Add(new SelectListItem
                    {
                        Text = "Digital(Email & Website)",
                        Value = "Digital(Email & Website)",
                    });
                    m.LeadSource.Add(new SelectListItem
                    {
                        Text = "Word of Mouth",
                        Value = "Word of Mouth",
                    });
                    m.LeadSource.Add(new SelectListItem
                    {
                        Text = "Advertisements(News paper & Insert)",
                        Value = "Advertisements(News paper & Insert)",
                    });
                    m.LeadSource.Add(new SelectListItem
                    {
                        Text = "Business Development",
                        Value = "Business Development",
                    });
                    m.LeadSource.Add(new SelectListItem
                    {
                        Text = "Secondary Data",
                        Value = "Secondary Data",
                    });
                    m.LeadSource.Add(new SelectListItem
                    {
                        Text = "Events & Exhibition)",
                        Value = "Events & Exhibition)",
                    });
                    m.LeadSource.Add(new SelectListItem
                    {
                        Text = "Social(Facebook, Linkedin Etc)",
                        Value = "Word of Mouth",
                    });
                    m.LeadSource.Add(new SelectListItem
                    {
                        Text = "SMS",
                        Value = "SMS",
                    });
                    m.LeadSource.Add(new SelectListItem
                    {
                        Text = "Outbound Calls",
                        Value = "Outbound Calls",
                    });
                    m.LeadSource.Add(new SelectListItem
                    {
                        Text = "Web to Lead",
                        Value = "Web to Lead",
                    });

                    m.LeadStatus = new List<SelectListItem>();
                    m.LeadStatus.Add(new SelectListItem { Text = "Open", Value = "Open" });
                    m.LeadStatus.Add(new SelectListItem { Text = "Contacted", Value = "Contacted" });
                    m.LeadStatus.Add(new SelectListItem { Text = "Contacted – No Response", Value = "Contacted – No Response" });
                    m.LeadStatus.Add(new SelectListItem { Text = "Closed Lost", Value = "Closed Lost" });
                    m.LeadStatus.Add(new SelectListItem { Text = "Qualified", Value = "Qualified" });
                    m.LeadStatus.Add(new SelectListItem { Text = "Re-Open", Value = "Re-Open" });

                    m.LeadSubStatus = new List<SelectListItem>();
                    m.LeadSubStatus.Add(new SelectListItem { Text = "Not Contactable", Value = "Not Contactable" });
                    m.LeadSubStatus.Add(new SelectListItem { Text = "Follow up", Value = "Follow up" });
                    m.LeadSubStatus.Add(new SelectListItem { Text = "Follow up – Not Contactable", Value = "Follow up – Not Contactable" });
                    m.LeadSubStatus.Add(new SelectListItem { Text = "Visit Confirmed", Value = "Visit Confirmed" });
                    m.LeadSubStatus.Add(new SelectListItem { Text = "Visit proposed", Value = "Visit proposed" });
                    m.LeadSubStatus.Add(new SelectListItem { Text = "Visit Done", Value = "Visit Done" });
                    m.LeadSubStatus.Add(new SelectListItem { Text = "Meeting Fixed", Value = "Meeting Fixed" });
                    m.LeadSubStatus.Add(new SelectListItem { Text = "Meeting Done", Value = "Meeting Done" });

                    m.LeadGender = new List<SelectListItem>();
                    m.LeadGender.Add(new SelectListItem { Text = "Male", Value = "Male" });
                    m.LeadGender.Add(new SelectListItem { Text = "Female", Value = "Female" });

                    m.ReasonforPurchase = new List<SelectListItem>();
                    m.ReasonforPurchase.Add(new SelectListItem { Text = "Self-Use", Value = "Self-Use" });
                    m.ReasonforPurchase.Add(new SelectListItem { Text = "Investment", Value = "Investment" });

                    m.PropertyStage = new List<SelectListItem>();
                    m.PropertyStage.Add(new SelectListItem { Text = "Ready to Move in", Value = "Ready to Move in" });
                    m.PropertyStage.Add(new SelectListItem { Text = "Under Construction", Value = "Under Construction" });

                    m.ProjectList = new List<SelectListItem>();
                    m.ProjectList.Add(new SelectListItem { Text = "Aangan", Value = "Aangan" });
                    m.ProjectList.Add(new SelectListItem { Text = "Elysium", Value = "Elysium" });
                    m.ProjectList.Add(new SelectListItem { Text = "La Marina", Value = "La Marina" });
                    m.ProjectList.Add(new SelectListItem { Text = "Oyster Grande", Value = "Oyster Grande" });
                    m.ProjectList.Add(new SelectListItem { Text = "The Meadows", Value = "The Meadows" });
                    m.ProjectList.Add(new SelectListItem { Text = "Western Heights", Value = "Western Heights" });
                    m.ProjectList.Add(new SelectListItem { Text = "Water Lily", Value = "Water Lily" });
                    m.ProjectList.Add(new SelectListItem { Text = "The North Park", Value = "The North Park" });
                    m.ProjectList.Add(new SelectListItem { Text = "Monte South", Value = "Monte South" });
                    m.ProjectList.Add(new SelectListItem { Text = "Samsara", Value = "Samsara" });
                    m.ProjectList.Add(new SelectListItem { Text = "Gurgaon - Samsara Vilasa", Value = "Gurgaon - Samsara Vilasa" });
                    m.ProjectList.Add(new SelectListItem { Text = "Pratham", Value = "Pratham" });
                    m.ProjectList.Add(new SelectListItem { Text = "Aangan - Shantigram", Value = "Aangan - Shantigram" });
                    m.ProjectList.Add(new SelectListItem { Text = "Code Name Greens", Value = "Code Name Greens" });

                    m.CountryList = new List<SelectListItem>();
                    m.CountryList.Add(new SelectListItem { Text = "Australia", Value = "Australia" });
                    m.CountryList.Add(new SelectListItem { Text = "Canada", Value = "Canada" });
                    m.CountryList.Add(new SelectListItem { Text = "Brazil", Value = "Brazil" });
                    m.CountryList.Add(new SelectListItem { Text = "China", Value = "China" });
                    m.CountryList.Add(new SelectListItem { Text = "Germany", Value = "Germany" });
                    m.CountryList.Add(new SelectListItem { Text = "United Kingdom", Value = "United Kingdom" });
                    m.CountryList.Add(new SelectListItem { Text = "Ireland", Value = "Ireland" });
                    m.CountryList.Add(new SelectListItem { Text = "India", Value = "India" });
                    m.CountryList.Add(new SelectListItem { Text = "Italy", Value = "Italy" });
                    m.CountryList.Add(new SelectListItem { Text = "Mexico", Value = "Mexico" });
                    m.CountryList.Add(new SelectListItem { Text = "United States", Value = "United States" });

                    return this.View(m);
                }

                RealtyDataContext rdb = new RealtyDataContext();
                ConsolidateFormData r = new ConsolidateFormData();

                r.FirstName = m.FirstName;
                r.LastName = m.LastName;
                r.Mobile = m.Mobile;
                r.Email = m.Email;
                if (string.IsNullOrEmpty(m.Budget))
                    r.Budget = null;
                else
                r.Budget = decimal.Parse(m.Budget);
                r.Country = m.CountryCode;
                r.State = m.StateCode;
                r.City = m.City;
                r.Lead_City = m.SelectedAssignmentCity;
                r.Lead_Configuration = m.Configuration;
                r.Lead_Gender = m.SelectedGender;
                r.Lead_InboundCall = m.InboundCall;
                r.Lead_Lost_Reasons = m.LeadLostReasons;
                r.Lead_PreSalesAgent = m.PreSalesAgent;
                r.Lead_Profession = m.Profession;
                r.Lead_Property_Stage = m.SelectedPropertyStage;
                r.Lead_ReasonForPurchase = m.SelectedReasonforPurchase;
                r.Lead_Source = m.SelectedLeadSource;
                r.Lead_Status = m.SelectedLeadStatus;
                r.Lead_Sub_Source = m.LeadSubSource;
                r.Lead_Sub_Status = m.SelectedLeadSubStatus;
                r.Comment = m.Remarks;
                r.PropertyType = m.SelectedLeadRecordType;
                r.PropertyLocation = m.SelectedProjectName;
                r.EntrySource = "Lead Creation page - By Admin";
                r.FormSubmitOn = DateTime.Now;
                r.FormSubmitBy = UserSession.UserSessionContext.userId;
                if (string.IsNullOrEmpty(m.ScheduledFollowUp))
                    r.ScheduledFollowUp = null;
                else
                    r.ScheduledFollowUp = scheduledFollowUpDate;

                //r.sale_type = m.sale_type;
                r.FormType = "Lead Creation page - By Admin";
                r.PageInfo = "lead-creation";


                rdb.ConsolidateFormDatas.InsertOnSubmit(r);

                rdb.SubmitChanges();
                Log.Info("Lead created successfully:" + DateTime.Now, this);
                var item = Context.Database.GetItem(Templates.LeadGeneration.ThankYou);
                var url = item.Url();
                return this.Redirect(url);
            }
            catch (Exception ex)
            {
                Log.Error("Error at LeadGeneration POST:" + ex.Message, this);
                var item = Context.Database.GetItem(Templates.LeadGeneration.LogOut);
                return this.Redirect(item.Url());
            }

        }

        #region API's

        [HttpGet]
        public ActionResult PropertyTypes()
        {
            if (IsRequestAuthenticated())
            {
                List<PropertyTypes> lstofprop = new List<PropertyTypes>();
                lstofprop.Add(new PropertyTypes() { PropertyName = "Residencial" });
                lstofprop.Add(new PropertyTypes() { PropertyName = "Commercial" });
                lstofprop.Add(new PropertyTypes() { PropertyName = "Club" });

                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "data retrieved successfully.", Result = lstofprop }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            else
            {
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid", Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }


        [HttpPost]
        public ActionResult Insertcontact(string name, string contactnumber, string propertyLocation)
        {
            try
            {
                if (IsRequestAuthenticated())
                {
                    RealtyDataContext rdb = new RealtyDataContext();
                    ConsolidateFormData r = new ConsolidateFormData();

                    r.FirstName = name;
                    r.LastName = string.Empty;
                    r.Mobile = contactnumber;
                    r.Email = string.Empty;
                    r.Budget = 0;
                    r.Country = string.Empty;
                    r.State = string.Empty;
                    r.PropertyType = string.Empty;
                    r.PropertyLocation = propertyLocation;
                    r.sale_type = string.Empty;
                    r.Comment = string.Empty;
                    r.FormType = "chatbot";
                    r.PageInfo = "api";
                    r.FormSubmitOn = DateTime.Now;
                    r.UTMSource = string.Empty;

                    #region Insert to DB
                    rdb.ConsolidateFormDatas.InsertOnSubmit(r);
                    rdb.SubmitChanges();
                    #endregion 
                }
                else
                {
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid", Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, IsSuccess = false, ex.Message, Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
            return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "data inserted successfully.", Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public ActionResult PropertyList(string type)
        {
            try
            {
                IQueryable<Data.Items.Item> result;
                if (IsRequestAuthenticated())
                {
                    //var properties1 = Sitecore.Context.Site.GetStartItem();

                    //Sitecore.Sites.SiteContext site = Sitecore.Context.Site;
                    //if (site == null)
                    //{
                    //    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, Message = "Context site is null", IsSuccess = false, Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    //}
                    ////string startItem = site.StartItem;
                    //string startPath = site.StartPath;

                    Sitecore.Data.Database db = Sitecore.Data.Database.GetDatabase("web");
                    Sitecore.Data.Items.Item properties = db.GetItem("/sitecore/content/Realty/Home");
                    if (properties == null)
                    {
                        return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, Message = "No properties found", IsSuccess = false, Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }

                    List<PropertyInfo> _PropertyInfoList = new List<PropertyInfo>();
                    List<City> _CityList = new List<City>();
                    List<PropertyDetail> _PropertyItemlList = new List<PropertyDetail>();

                    ID location = null;
                    if (string.Equals(type.ToLower(), "residential"))
                    {
                        result = properties.Axes.GetDescendants().Where(x => x.TemplateID == Templates.ResidentialProjects.ID).AsQueryable();
                        location = Templates.ResidentialProjects.Fields.Location;
                    }
                    else if (string.Equals(type.ToLower(), "commercial"))
                    {
                        result = properties.Axes.GetDescendants().Where(x => x.TemplateID == Templates.CommercialProperty.ID).AsQueryable();
                        location = Templates.CommercialProperty.Fields.Location;
                    }
                    else
                    {
                        return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, Message = "Please etner proper type.", IsSuccess = false, Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }

                    if (result != null && result.Any())
                    {
                        if (!string.IsNullOrEmpty(type))
                        {
                            result = FilterRecordbyPropertytype(type, result);
                        }
                        PropertyInfo _propertyInfoModel = new PropertyInfo();
                        _propertyInfoModel.PropertyType = type;



                        foreach (var item in result.ToList())
                        {
                            //bool IsnNewCity = false;
                            var itemlocation = item.GetMultiListValueItems(location).FirstOrDefault();
                            string _nameofcity = itemlocation.Fields[Templates.SingleText.Fields.Text].Value;
                            City _City = _CityList.FirstOrDefault(x => x.Nameofcity == _nameofcity);

                            string _PropertyName = item.Fields[Templates._HasPageContent.Fields.Title].Value;
                            //_PropertyItemlList.Add(new PropertyDetail() { PropertyName = _PropertyName, Id = item.ID.ToString() });
                            _PropertyItemlList.Add(new PropertyDetail() { PropertyName = _PropertyName, cityname = _nameofcity, Id = item.ID.ToString() });
                            //_PropertyItemlList = _PropertyItemlList.Where(x => x.cityname == _nameofcity).ToList();
                            //if (_City == null)
                            //{
                            //    IsnNewCity = true;
                            //}

                            //if (IsnNewCity)
                            //{
                            //    _CityList.Add(new City() { Nameofcity = _nameofcity, propertyitemslist = _PropertyItemlList });
                            //}
                            //_propertyInfoModel.citylist = _CityList;
                        }
                        var city = _PropertyItemlList.Select(x => x.cityname).Distinct().ToList();
                        foreach (var item in city)
                        {
                            var propertydata = _PropertyItemlList.Where(x => x.cityname == item).ToList();
                            _CityList.Add(new City() { Nameofcity = item, propertyitemslist = propertydata });
                        }
                        _propertyInfoModel.citylist = _CityList;
                        _PropertyInfoList.Add(_propertyInfoModel);
                    }

                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "data retrieved successfully.", Result = _PropertyInfoList }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid", Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            catch (Exception ex)
            {

                Sitecore.Diagnostics.Log.Error("Error at stats:" + ex.Message, this);
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, IsSuccess = false, ex.Message, Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }

        }
        [HttpGet]
        public ActionResult ProjectStatus(string projectId)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("ProjectStatus Method Called", this);
                if (IsRequestAuthenticated())
                {
                    Sitecore.Data.Database contextdata = Sitecore.Data.Database.GetDatabase("web");

                    // Sitecore.Data.Items.Item parentItem = master.GetItem("/sitecore/content/Home/Account List");
                    var projectdetails = contextdata.GetItem(projectId);
                    if (projectdetails != null)
                    {
                        var projectstatus = projectdetails.Fields["Project Status"].Value;
                        if (!string.IsNullOrEmpty(projectstatus))
                        {
                            var resultdata = contextdata.GetItem(projectstatus).Fields["Text"].Value;
                            return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "Success", Result = resultdata }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                        else
                        {
                            var projectstatus1 = "";
                            return new JsonResult() { Data = new { StatusCode = HttpStatusCode.NotFound, IsSuccess = true, Message = "Success", Result = projectstatus1 }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                        }
                    }
                    else
                    {
                        var projectstatus = "No record found.";
                        return new JsonResult() { Data = new { StatusCode = HttpStatusCode.NotFound, IsSuccess = true, Message = "Failure", Result = projectstatus }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }

                }
                else
                {
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid", Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at stats:" + ex.Message, this);
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, IsSuccess = false, Message = ex.Message, Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
        [HttpGet]
        public ActionResult ProjectDetails(string projectId)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("ProjectDetails Method Called", this);
                if (IsRequestAuthenticated())
                {
                    Sitecore.Data.Database contextdata = Sitecore.Data.Database.GetDatabase("web");
                    PropertryData objmodal = new PropertryData();
                    // Sitecore.Data.Items.Item parentItem = master.GetItem("/sitecore/content/Home/Account List");
                    var projectdetails = contextdata.GetItem(projectId);


                    if (projectdetails != null && projectdetails.TemplateName == "Residential")
                    {

                        objmodal.Title = projectdetails.Fields["Title"].Value;
                        //Overview
                        string path = projectdetails.Paths.FullPath + "/_Local/Overview";
                        var overviewdetails = contextdata.GetItem(path);
                        var overdesc = overviewdetails != null ? overviewdetails.Fields["Description"].Value : string.Empty;
                        objmodal.Overview = overdesc;

                        //Location
                        string location = projectdetails.Paths.FullPath + "/_Local/Location";
                        var locationdetails = contextdata.GetItem(location);
                        var locationmap = locationdetails != null ? locationdetails.Fields["Location"].Value : string.Empty;
                        objmodal.GoogleMap = locationmap;

                        //Brochure
                        string Brochure = projectdetails.Paths.FullPath + "/_Local/Residential Block Tiles/Brochure";
                        var Brochureetails = contextdata.GetItem(Brochure);
                        //ID LinkURL = new ID("{FF693EDE-C4F7-4FCE-AD3E-AD943C3FDD42}");
                        //var lnkurl = Brochureetails.LinkFieldUrl(LinkURL).ToString();
                        var Brochurelink = Brochureetails != null ? string.Format("https://stage.adanirealty.com/{0}", Brochureetails.LinkFieldUrl(new ID("{FF693EDE-C4F7-4FCE-AD3E-AD943C3FDD42}"))) : string.Empty;

                        objmodal.Brochure = Brochurelink;
                        //Features
                        string Features = projectdetails.Paths.FullPath + "/_Local/Featiures";
                        var FeaturesList = contextdata.GetItem(Features);
                        List<propertyText> featurelist = new List<propertyText>();
                        if (FeaturesList != null && FeaturesList.HasChildren)
                        {
                            foreach (var item in FeaturesList.Children.InnerChildren)
                            {
                                featurelist.Add(new propertyText { text = item.Fields[Templates._HasMedia.Fields.Title].Value });
                            }
                        }

                        objmodal.Features = featurelist;

                        // var Brochureetails = contextdata.GetItem(Brochure);

                        //ReraCertification
                        string ReraCertification = projectdetails.Paths.FullPath + "/_Local/Rera Certificate";
                        var ReraCertificationlist = contextdata.GetItem(ReraCertification);
                        List<propertyText> reralist = new List<propertyText>();
                        if (ReraCertificationlist != null && ReraCertificationlist.HasChildren)
                        {
                            foreach (var item in ReraCertificationlist.Children.InnerChildren)
                            {
                                var LinkUrl = item.LinkFieldUrl(Templates.LinkDetail.Fields.LinkUrl);
                                if (!string.IsNullOrEmpty(LinkUrl))
                                {
                                    reralist.Add(new propertyText { text = !string.IsNullOrEmpty(LinkUrl) ? string.Format("https://stage.adanirealty.com/{0}", LinkUrl) : string.Empty });
                                }
                            }
                        }

                        objmodal.ReraCertification = reralist;

                        //Images gallery
                        string Imagesgallery = projectdetails.Paths.FullPath + "/_Local/Gallery";
                        var Imagesgalleryitem = contextdata.GetItem(Imagesgallery);
                        //var field = Imagesgalleryitem.IsDerived(Templates.HasMediaImage.Fields.Image) ? Templates._HasMedia.Fields.Thumbnail : Templates.HasMediaImage.Fields.Image;
                        //var style = Imagesgalleryitem.ImageUrl(field, new MediaUrlOptions());

                        List<propertyText> Imagesgallerylist = new List<propertyText>();
                        if (Imagesgalleryitem != null && Imagesgalleryitem.HasChildren)
                        {
                            MediaUrlOptions muo = new MediaUrlOptions();
                            muo.AlwaysIncludeServerUrl = true;
                            foreach (var item1 in Imagesgalleryitem.Children.InnerChildren)
                            {
                                var field = item1.IsDerived(Templates.HasMediaImage.Fields.Image) ? Templates._HasMedia.Fields.Thumbnail : Templates.HasMediaImage.Fields.Image;
                                var style = item1.ImageUrl(field, new MediaUrlOptions());
                                var LinkUrl = item1.ImageUrl(Templates.HasMediaImage.Fields.Image, muo);
                                if (!string.IsNullOrEmpty(LinkUrl))
                                {
                                    Imagesgallerylist.Add(new propertyText { text = LinkUrl });
                                }
                            }
                        }

                        objmodal.Images = Imagesgallerylist;

                        return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "Success", Result = objmodal }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    else if (projectdetails != null && projectdetails.TemplateName == "Commercial")
                    {
                        objmodal.Title = projectdetails.Fields["Title"].Value;
                        //Overview
                        string path = projectdetails.Paths.FullPath + "/_Local/Overview";
                        var overviewdetails = contextdata.GetItem(path);
                        var overdesc = overviewdetails != null ? overviewdetails.Fields["Description"].Value : string.Empty;
                        objmodal.Overview = overdesc;

                        //Location
                        string location = projectdetails.Paths.FullPath + "/_Local/Location";
                        var locationdetails = contextdata.GetItem(location);
                        var locationmap = locationdetails != null ? locationdetails.Fields["Location"].Value : string.Empty;
                        objmodal.GoogleMap = locationmap;

                        //Brochure
                        //string Brochure = projectdetails.Paths.FullPath + "/_Local/Residential Block Tiles/Brochure";
                        //var Brochureetails = contextdata.GetItem(Brochure);
                        //var Brochurelink = Brochureetails != null ? string.Format("https://stage.adanirealty.com/{0}", Brochureetails.LinkFieldUrl(new ID("{FF693EDE-C4F7-4FCE-AD3E-AD943C3FDD42}"))) : string.Empty;

                        objmodal.Brochure = string.Empty;
                        //Features
                        string Features = projectdetails.Paths.FullPath + "/_Local/Overview Features";
                        var FeaturesList = contextdata.GetItem(Features);
                        List<propertyText> featurelist = new List<propertyText>();
                        if (FeaturesList != null && FeaturesList.HasChildren)
                        {
                            foreach (var item in FeaturesList.Children.InnerChildren)
                            {
                                featurelist.Add(new propertyText { text = item.Fields[Templates._HasMedia.Fields.Title].Value });
                            }
                        }

                        objmodal.Features = featurelist;

                        // var Brochureetails = contextdata.GetItem(Brochure);

                        //Amenities

                        //Features
                        string amenities = projectdetails.Paths.FullPath + "/_Local/Amenities";
                        var amenitiesList = contextdata.GetItem(amenities);
                        var listamenities = amenitiesList.GetMultiListValueItems(Templates.CommercialMediaSelector.Fields.MediaSelector).ToArray();
                        List<ameinitesdata> ameinitesdatalist = new List<ameinitesdata>();
                        if (listamenities != null)
                        {
                            MediaUrlOptions muo = new MediaUrlOptions();
                            muo.AlwaysIncludeServerUrl = true;
                            foreach (var item in listamenities)
                            {
                                var LinkUrl = item.ImageUrl(new ID("{71FF6029-CAD2-4128-ABBC-7342C118B79A}"), muo);
                                if (!string.IsNullOrEmpty(LinkUrl))
                                {
                                    ameinitesdatalist.Add(new ameinitesdata { text = item.Fields["Overlay Text"].Value, imageURL = LinkUrl });
                                }
                            }
                        }

                        objmodal.Amenities = ameinitesdatalist;


                        //ReraCertification
                        objmodal.ReraCertification = null;

                        //Images gallery
                        string Imagesgallery = projectdetails.Paths.FullPath + "/_Local/Gallery";
                        var Imagesgalleryitem = contextdata.GetItem(Imagesgallery);
                        //var field = Imagesgalleryitem.IsDerived(Templates.HasMediaImage.Fields.Image) ? Templates._HasMedia.Fields.Thumbnail : Templates.HasMediaImage.Fields.Image;
                        //var style = Imagesgalleryitem.ImageUrl(field, new MediaUrlOptions());

                        List<propertyText> Imagesgallerylist = new List<propertyText>();
                        if (Imagesgalleryitem != null && Imagesgalleryitem.HasChildren)
                        {
                            MediaUrlOptions muo = new MediaUrlOptions();
                            muo.AlwaysIncludeServerUrl = true;
                            foreach (var item1 in Imagesgalleryitem.Children.InnerChildren)
                            {
                                //var field = item1.IsDerived(Templates.HasMediaImage.Fields.Image) ? Templates._HasMedia.Fields.Thumbnail : Templates.HasMediaImage.Fields.Image;
                                var LinkUrl = item1.ImageUrl(Templates.HasMediaImage.Fields.Image, muo);
                                if (!string.IsNullOrEmpty(LinkUrl))
                                {
                                    Imagesgallerylist.Add(new propertyText { text = LinkUrl });
                                }
                            }
                        }

                        objmodal.Images = Imagesgallerylist;

                        return new JsonResult() { Data = new { StatusCode = HttpStatusCode.OK, IsSuccess = true, Message = "Success", Result = objmodal }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                    else
                    {
                        var projectstatus = "No record found.";
                        return new JsonResult() { Data = new { StatusCode = HttpStatusCode.NotFound, IsSuccess = true, Message = "Failure", Result = projectstatus }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }

                }
                else
                {
                    return new JsonResult() { Data = new { StatusCode = HttpStatusCode.Unauthorized, IsSuccess = false, Message = "Credential is not valid", Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at stats:" + ex.Message, this);
                return new JsonResult() { Data = new { StatusCode = HttpStatusCode.InternalServerError, IsSuccess = false, Message = ex.Message, Result = string.Empty }, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }

        private bool IsRequestAuthenticated()
        {
            string authHeader = HttpContext.Request.Headers["Authorization"];
            Encoding encoding = Encoding.GetEncoding("iso-8859-1");
            if (string.IsNullOrEmpty(authHeader))
            {
                return false;
            }
            string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
            string usernamePassword = encoding.GetString(System.Convert.FromBase64String(encodedUsernamePassword));
            int seperatorIndex = usernamePassword.IndexOf(':');
            string username = usernamePassword.Substring(0, seperatorIndex);
            string password = usernamePassword.Substring(seperatorIndex + 1);

            string apiUserName = ConfigurationManager.AppSettings["apiRealtyUserName"];
            string apiPassword = ConfigurationManager.AppSettings["apiPassword"];

            if (username == apiUserName && password == apiPassword)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        [HttpPost]
        public ActionResult OysterGrandeEnquiryNow(OGEnquireNowModel m)
        {
            var result = new { status = "0" };
            PlatinumRealtyDataContext rdb = new PlatinumRealtyDataContext();

            try
            {

                if (string.IsNullOrEmpty(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!alphaNumber.IsMatch(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (!emailRegex.IsMatch(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!NumRegex.IsMatch(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                //if (string.IsNullOrEmpty(m.OTP))
                //{
                //    result = new { status = "406" };
                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}

                //string generatedOTP = realtyRepo.GetOTP(m.mobile);
                //if (!string.Equals(generatedOTP, m.OTP))
                //{



                //    var data = rdb.platinumRealtyOtpHistories.Where(x => x.MobileNumber == m.mobile && x.otp == generatedOTP && x.status==false).FirstOrDefault();
                //    if (data != null)
                //    {
                //        var otpcount = data.count;
                //        if (otpcount >= 3)
                //        {
                //            result = new { status = "103" };
                //        }
                //        else
                //        {
                //            result = new { status = "102" };
                //        }

                //        otpcount = otpcount + 1;
                //        data.count = otpcount;


                //        rdb.SubmitChanges();
                //    }

                //    else {

                //        result = new { status = "108" };
                //    }

                //}
                else
                {


                    OysterGrandePenthouseEnquireNow r = new OysterGrandePenthouseEnquireNow();
                    //var data = rdb.platinumRealtyOtpHistories.Where(x => x.MobileNumber == m.mobile && x.otp == generatedOTP).FirstOrDefault();
                    //data.status = true;

                    r.FullName = m.full_name;
                    //r.LastName = m.last_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.Budget = string.IsNullOrEmpty(m.Budget) ? 0 : decimal.Parse(m.Budget);
                    r.Country = m.country_code;
                    r.State = m.state_code;
                    r.PropertyType = m.Projects_Interested__c;
                    r.PropertyLocation = m.PropertyLocation;
                    r.sale_type = m.sale_type;
                    r.Comment = m.Remarks;
                    r.FormType = m.FormType;
                    r.PageUrl = m.PageUrl;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    //r.City = m.city;
                    //r.Lead_Sub_Source = m.UTMPlacement;

                    #region Insert to DB

                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);

                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);



                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject obj = new LeadObject
                    {
                        Firstname = m.full_name,
                        LastName = "-",
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        Email = m.email,
                        Mobile = m.mobile,
                        Project = "Oyster Grande",
                        MasterProjectID = m.PropertyCode,
                        UtmSource = m.UTMSource,
                        AssignmentCity = m.PropertyLocation,
                        RecordType = m.RecordType,
                        LeadSource = "Digital",
                        Ads = m.AdvertisementId
                    };

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    r.Comment = leadResult;
                    rdb.OysterGrandePenthouseEnquireNows.InsertOnSubmit(r);

                    rdb.SubmitChanges();
                    result = new { status = "101" };
                }
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult SamsaraVilasaEnquiryNow(SamsaraVilasaEnquireNowModel m)
        {
            var result = new { status = "0" };
            PlatinumRealtyDataContext rdb = new PlatinumRealtyDataContext();

            try
            {

                if (string.IsNullOrEmpty(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!alphaNumber.IsMatch(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (!emailRegex.IsMatch(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!NumRegex.IsMatch(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                //if (string.IsNullOrEmpty(m.OTP))
                //{
                //    result = new { status = "406" };
                //    return Json(result, JsonRequestBehavior.AllowGet);
                //}

                //string generatedOTP = realtyRepo.GetOTP(m.mobile);
                //if (!string.Equals(generatedOTP, m.OTP))
                //{



                //    var data = rdb.platinumRealtyOtpHistories.Where(x => x.MobileNumber == m.mobile && x.otp == generatedOTP && x.status==false).FirstOrDefault();
                //    if (data != null)
                //    {
                //        var otpcount = data.count;
                //        if (otpcount >= 3)
                //        {
                //            result = new { status = "103" };
                //        }
                //        else
                //        {
                //            result = new { status = "102" };
                //        }

                //        otpcount = otpcount + 1;
                //        data.count = otpcount;


                //        rdb.SubmitChanges();
                //    }

                //    else {

                //        result = new { status = "108" };
                //    }

                //}
                else
                {


                    SamsaraVilasaEnquireNow r = new SamsaraVilasaEnquireNow();
                    //var data = rdb.platinumRealtyOtpHistories.Where(x => x.MobileNumber == m.mobile && x.otp == generatedOTP).FirstOrDefault();
                    //data.status = true;

                    r.FullName = m.full_name;
                    //r.LastName = m.last_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.Budget = string.IsNullOrEmpty(m.Budget) ? 0 : decimal.Parse(m.Budget);
                    r.Country = m.country_code;
                    r.State = m.state_code;
                    r.PropertyType = m.Projects_Interested__c;
                    r.PropertyLocation = m.PropertyLocation;
                    r.sale_type = m.sale_type;
                    r.Comment = m.Remarks;
                    r.FormType = m.FormType;
                    r.PageUrl = m.PageUrl;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    //r.City = m.city;
                    //r.Lead_Sub_Source = m.UTMPlacement;

                    #region Insert to DB

                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);

                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);



                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject obj = new LeadObject
                    {
                        Firstname = m.full_name,
                        LastName = "-",
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        Email = m.email,
                        Mobile = m.mobile,
                        Project = "Samsara Vilasa",
                        MasterProjectID = m.PropertyCode,
                        UtmSource = m.UTMSource,
                        AssignmentCity = m.PropertyLocation,
                        RecordType = m.RecordType,
                        LeadSource = "Digital",
                        Ads = m.AdvertisementId
                    };

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    r.Comment = leadResult;
                    rdb.SamsaraVilasaEnquireNows.InsertOnSubmit(r);

                    rdb.SubmitChanges();
                    result = new { status = "101" };
                }
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult JMPRealtySendOtp(EnquiryModel model)
        {
            //EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0" };
            PlatinumRealtyDataContext rdb = new PlatinumRealtyDataContext();
            JMPRealtyOtpHistory r = new JMPRealtyOtpHistory();
            try
            {




                var currDate = DateTime.Now;
                var beforeTime = currDate.AddMinutes(-30);
                var data = rdb.JMPRealtyOtpHistories.Where(x => x.MobileNumber == model.mobile && x.date >= beforeTime && x.date <= currDate).Count();
                if (data >= 3)
                {
                    result = new { status = "503" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                #region Delete Available otp from database for given mobile number

                //realtyRepo.DeleteOldOtp(model.mobile);
                #endregion
                model.OtpPurpose = PurposeOfOtp.Enquiry;
                #region Generate New Otp for given mobile number and save to database
                string generatedotp = realtyRepo.StoreGeneratedOtp(model, System.Convert.ToInt32(ConfigurationManager.AppSettings["OtpLifeinMinutes"]), System.Convert.ToInt32(ConfigurationManager.AppSettings["MaxOtpAttempts"]));
                #endregion
                if (generatedotp == "0")
                {
                    result = new { status = "2" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                #region Api call to send SMS of OTP
                try
                {

                    var apiurl = string.Format(" https://bulksms.analyticsmantra.com/sendsms/sendsms.php?username=ADANITRAN&password=tech321&type=TEXT&sender=ADRLTY&mobile={0}&message=Hi%20%20{1}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&PEID=1601100000000013196&HeaderId=1605001594131761200&templateId=1607100000000117984", model.mobile, generatedotp);

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Error("OTP Api call success. https://enterprise.smsgupshup.com/", this);
                        result = new { status = "1" };
                        r.MobileNumber = model.mobile;
                        r.otp = generatedotp;
                        r.status = false;
                        r.count = 0;
                        r.date = DateTime.Now;
                        rdb.JMPRealtyOtpHistories.InsertOnSubmit(r);
 

                        rdb.SubmitChanges();
                    }
                    else
                    {
                        Log.Error("OTP Api call failed. https://enterprise.smsgupshup.com/", this);
                        result = new { status = "0" };
                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"{0}", ex, this);
                }
                #endregion

                #region Return Response with Mobile Number and Generated otp
                //result = new { status = "1" };
                return Json(result, JsonRequestBehavior.AllowGet);


                #endregion
            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
                return Json(result, JsonRequestBehavior.AllowGet);
            }

        }


        [HttpPost]
        public ActionResult JMPEnquiryNow(JMPEnquireNowModel m)
        {
            var result = new { status = "0" };
            PlatinumRealtyDataContext rdb = new PlatinumRealtyDataContext();

            try
            {

                if (string.IsNullOrEmpty(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!alphaNumber.IsMatch(m.full_name))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (!emailRegex.IsMatch(m.email))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!NumRegex.IsMatch(m.mobile))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                if (string.IsNullOrEmpty(m.OTP))
                {
                    result = new { status = "406" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

                string generatedOTP = realtyRepo.GetOTP(m.mobile, PurposeOfOtp.Enquiry);
                if (!string.Equals(generatedOTP, m.OTP))
                {



                    var data = rdb.JMPRealtyOtpHistories.Where(x => x.MobileNumber == m.mobile && x.otp == generatedOTP && x.status==false).FirstOrDefault();
                    if (data != null)
                   {
                       var otpcount = data.count;
                       if (otpcount >= 3)
                       {
                           result = new { status = "103" };
                         }
                        else
                         {
                             result = new { status = "102" };
                         }

                         otpcount = otpcount + 1;
                          data.count = otpcount;


                         rdb.SubmitChanges();
                     }

                    else {

                         result = new { status = "108" };
                     }

                 }
                else
                {


                    JMPEnquireNow r = new JMPEnquireNow();
                    //var data = rdb.platinumRealtyOtpHistories.Where(x => x.MobileNumber == m.mobile && x.otp == generatedOTP).FirstOrDefault();
                    //data.status = true;

                    r.FullName = m.full_name;
                    //r.LastName = m.last_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.Budget = string.IsNullOrEmpty(m.Budget) ? 0 : decimal.Parse(m.Budget);
                    r.Country = m.country_code;
                    r.State = m.state_code;
                    r.PropertyType = m.Projects_Interested__c;
                    r.PropertyLocation = m.PropertyLocation;
                    r.sale_type = m.sale_type;
                    r.Comment = m.Remarks;
                    r.FormType = m.FormType;
                    r.PageUrl = m.PageUrl;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    //r.City = m.city;
                    //r.Lead_Sub_Source = m.UTMPlacement;

                    #region Insert to DB

                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);

                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);



                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject obj = new LeadObject
                    {
                        Firstname = m.full_name,
                        LastName = "-",
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        Email = m.email,
                        Mobile = m.mobile,
                        Project = m.project_type,
                        Budget = "",
                        Country=m.country_code,
                        State = m.state_code,
                        AssignmentCity=m.city,
                        Remarks="",
                        MasterProjectID = m.PropertyCode,
                        UtmSource = m.UTMSource,
                        RecordType = m.RecordType,
                        LeadSource = "Digital",
                        Ads = m.AdvertisementId,
                        PostToSalesforce= "true",
                        Saletype="Outright",
                        Projectintrested="RESIDENTIAL",
                        UtmPlacement=""
                    };

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    r.Comment = leadResult;
                    rdb.JMPEnquireNows.InsertOnSubmit(r);

                    rdb.SubmitChanges();
                    result = new { status = "101" };
                }
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AsterEnquiryNow(AsterEnquireModel m)
        {
            Log.Error("Aster Enquiry Now Start", "");
            var result = new { status = "0" };
            RealtyDataContext rdb = new RealtyDataContext();
            PlatinumRealtyDataContext rdb1 = new PlatinumRealtyDataContext();
            String[] FormType = { "Contact us", "Enquire Now"};
            try
            {
                if (!Regex.IsMatch(m.full_name, (@"^([A-Za-z ]){2,100}$")))
                {
                    result = new { status = "401" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(m.email, (@"^([0-9a-zA-Z]([-\.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[com]{2,9})$")))
                {
                    result = new { status = "403" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(m.mobile, (@"^\d{10}$")))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!FormType.Contains(m.FormType))
                {
                    result = new { status = "406" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(m.PageInfo, (@"^[A-Za-z ]+$")))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                if (!Regex.IsMatch(m.UTMSource, (@"^[A-Za-z _]+$")))
                {
                    result = new { status = "405" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {


                    AsterEnquireNow r = new AsterEnquireNow();
                    //var data = rdb.platinumRealtyOtpHistories.Where(x => x.MobileNumber == m.mobile && x.otp == generatedOTP).FirstOrDefault();
                    //data.status = true;

                    r.FullName = m.full_name;
                    //r.LastName = m.last_name;
                    r.Mobile = m.mobile;
                    r.Email = m.email;
                    r.Budget = string.IsNullOrEmpty(m.Budget) ? 0 : decimal.Parse(m.Budget);
                    r.Country = m.country_code;
                    r.State = m.state_code;
                    r.PropertyType = m.Projects_Interested__c;
                    r.PropertyLocation = m.PropertyLocation;
                    r.sale_type = m.sale_type;
                    r.Comment = m.Remarks;
                    r.FormType = m.FormType;
                    r.PageUrl = m.PageUrl;
                    r.PageInfo = m.PageInfo;
                    r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
                    r.UTMSource = m.UTMSource;
                    //r.City = m.city;
                    //r.Lead_Sub_Source = m.UTMPlacement;

                    #region Insert to DB

                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.full_name, this);

                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);



                    Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);

                    Log.Error("Aster Enquiry Now Lead Genration Start", "");
                    SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                    //Get project details
                    Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                    LeadObject obj = new LeadObject
                    {
                        Firstname = m.full_name,
                        LastName = "-",
                        FormType = m.FormType,
                        PageInfo = m.PageInfo,
                        Email = m.email,
                        Mobile = m.mobile,
                        Project = "Aster",
                        MasterProjectID = m.PropertyCode,
                        UtmSource = m.UTMSource,
                        AssignmentCity = m.PropertyLocation,
                        RecordType = m.RecordType,
                        LeadSource = "Digital",
                        Ads = m.AdvertisementId
                    };

                    Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                    var leadResult = SalesForceWrapperObj.GenerateLead(obj);
                    if (leadResult == null)
                    {
                        result = new { status = "407" };
                        return Json(result, JsonRequestBehavior.AllowGet);
                    }
                    r.Comment = leadResult;
                    Log.Error("Aster Enquiry Now Lead Genration Result"+ leadResult ,"");
                    rdb.AsterEnquireNows.InsertOnSubmit(r);

                    rdb.SubmitChanges();
                    result = new { status = "101" };
                }
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        #endregion

    }
}