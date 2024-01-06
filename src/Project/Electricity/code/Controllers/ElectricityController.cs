using Rotativa;
using Sitecore.Diagnostics;
using Sitecore.Electricity.Website.Model;
using Sitecore.Electricity.Website.Services;
using Sitecore.Electricity.Website.Utility;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Sitecore.Data;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using Sitecore.Tasks;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace Sitecore.Electricity.Website.Controllers
{
    public class ElectricityController : Controller
    {
        [HttpGet]
        // GET: Survey
        public ActionResult Survey()
        {
            Survey objSurvey = new Survey();
            var Questions = Sitecore.Mvc.Presentation.RenderingContext.Current.Rendering.DataSource;
            var parent = Sitecore.Context.Database.GetItem(Questions).Children;

            foreach (var item in parent.ToList())
            {
                TypeofAppliance objTypeofAppliance = new TypeofAppliance();
                objTypeofAppliance.Name = item["Title"];
                foreach (var appliance in item.GetChildren().InnerChildren.ToList())
                {
                    Appliance app = new Appliance();
                    app.Name = appliance.Name;
                    foreach (var ques in appliance.GetChildren().InnerChildren.ToList())
                    {
                        Questions que = new Questions();
                        que.Question = ques["Title"];
                        foreach (var opt in ques.GetChildren().ToList())
                        {
                            Sitecore.Electricity.Website.Model.Options Objopts = new Options();
                            Objopts.Option = opt["Title"];
                            Objopts.Response = opt["Response"];
                            Objopts.Checked = false;
                            que.Option.Add(Objopts);
                        }
                        app.QuestionsList.Add(que);
                    }
                    objTypeofAppliance.ApplianceList.Add(app);
                }
                objSurvey.TypeofApplianceList.Add(objTypeofAppliance);
            }
            TempData["Postmodel"] = objSurvey;
            return View(objSurvey);
        }

        [System.Web.Http.HttpPost]
        public ActionResult Survey([Bind(Exclude = "TypeofApplianceList")]Survey model, FormCollection Frm)
        {
            if (!this.ModelState.IsValid)
            {
                if (TempData["Postmodel"] != null)
                {
                    TempData.Keep("Postmodel");
                    return View((Survey)TempData["Postmodel"]);
                }
            }
            model.Frm = SeralizedFormCollection(Frm);
            TempData["FrmKeys"] = model.Frm;
            System.Diagnostics.Debug.WriteLine("Tempdata" + " " + model.Frm);
            TempData.Keep();
            return RedirectToAction("SurveyPDF", "Electricity");
        }

        private string SeralizedFormCollection(FormCollection Frm)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            //Serialize
            string serialized;
            using (MemoryStream stream = new MemoryStream())
            {
                // pass FormCollection to constructor of new NameValueCollection
                // that way we kind of convert it to NameValueCollection which is serializable
                // of course we lost any FormCollection-specific details (if there were any)
                formatter.Serialize(stream, new NameValueCollection(Frm));
                serialized = System.Convert.ToBase64String(stream.ToArray());
            };

            return serialized;
        }

        private FormCollection SeralizedFormCollection(string SeralizedData)
        {
            FormCollection data = null;
            BinaryFormatter formatter = new BinaryFormatter();
            using (MemoryStream stream = new MemoryStream(System.Convert.FromBase64String(SeralizedData)))
            {
                // deserialize as NameValueCollection then create new 
                // FormCollection from that
                data = new FormCollection((NameValueCollection)formatter.Deserialize(stream));
            }

            return data;
        }

        public ActionResult SurveyPDF()
        {
            var formcoll = SeralizedFormCollection(TempData["FrmKeys"].ToString());
            var reqData = Request.Form["survey"];
            SurveyResult objSurvey = new SurveyResult();
            objSurvey.Name = formcoll["Name"];
            objSurvey.CANumber = formcoll["CANumber"];
            objSurvey.Email = formcoll["Email"];
            objSurvey.MobileNo = formcoll["MobileNo"];
            for (var i = 0; i < System.Convert.ToInt32(formcoll["TypeofApplianceList"]); i++)
            {
                SubSurveyResult ObjsubSurvey = new SubSurveyResult();
                ObjsubSurvey.Appliancetype = formcoll["TypeofApplianceList[" + i.ToString() + "].Name"];
                var Appliancelist = formcoll["ApplianceList"].Split(',')[i].ToString() == null ? formcoll["ApplianceList"] : formcoll["ApplianceList"].Split(',')[i];
                for (var j = 0; j < System.Convert.ToInt32(Appliancelist.ToString()); j++)
                {
                    ApplianceList ObjApplianceList = new ApplianceList();
                    ObjApplianceList.ApplianceName = formcoll["TypeofApplianceList[" + i.ToString() + "].ApplianceList[" + j.ToString() + "].Name"];

                    var Questionlist = formcoll["QuestionCount"].Split(',')[i].ToString() == null ? formcoll["QuestionCount"] : formcoll["QuestionCount"].Split(',')[i];
                    for (var k = 0; k < System.Convert.ToInt32(Questionlist); k++)
                    {
                        var Optionlist = formcoll["QuestionOptionCount"].Split(',')[i].ToString() == null ? formcoll["QuestionOptionCount"] : formcoll["QuestionOptionCount"].Split(',')[i];
                        for (var l = 0; l < System.Convert.ToInt32(Optionlist); l++)
                        {
                            if (formcoll["TypeofApplianceList[" + i + "].ApplianceList[" + j + "].QuestionsList[" + k + "].Option[" + l + "].Option"] == "true")
                            {
                                AnswerdQuestionResponse objAns = new AnswerdQuestionResponse();
                                objAns.Response = formcoll["TypeofApplianceList[" + i + "].ApplianceList[" + j + "].QuestionsList[" + k + "].Option[" + l + "].Response"];
                                ObjApplianceList.AnswerdQuestionResponse.Add(objAns);

                            }
                        }
                    }
                    ObjsubSurvey.ApplianceList.Add(ObjApplianceList);
                }
                objSurvey.SubSurveyResult.Add(ObjsubSurvey);
            }

            return new ViewAsPdf("SurveyPDF", objSurvey);
        }

        //Tender Listing for all opened/closed/Corrigendum
        //For Normal User
        public ActionResult TenderListing()
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                List<TenderList> tenderdata = dbcontext.TenderLists.ToList();
                List<TenderDetails> ObjTender = new List<TenderDetails>();
                foreach (var data in tenderdata)
                {
                    TenderDetails ObjTd = new TenderDetails();
                    ObjTd.Id = data.Id;
                    ObjTd.NITPRNo = data.NITNo;
                    ObjTd.Business = data.Business;
                    ObjTd.Description = data.Description;
                    ObjTd.Adv_Date = data.Adv_Date;
                    ObjTd.Bid_Submision_ClosingDate = data.Closing_Date.ToString();
                    ObjTd.Estimated_Cost = !string.IsNullOrEmpty(data.Estimated_Cost) ? data.Estimated_Cost : "-";
                    ObjTd.Cost_of_EMD = !string.IsNullOrEmpty(data.Cost_of_EMD) ? data.Cost_of_EMD : "-";
                    ObjTd.Location = data.Location;
                    ObjTd.OnHold = data.OnHold.GetValueOrDefault();
                    ObjTd.isCorrigendumPresent = OpenTenderDocPAth(data.Id, data.NITNo);
                    ObjTd.CreatedDate = data.Created_Date;
                    ObjTd.ModifiedDate = data.Modified_Date;
                    ObjTd.Status = data.Staus;
                    ObjTd.ClosingDate = data.Closing_Date;
                    ObjTender.Add(ObjTd);
                }
                List<Corrigendum> Corr = dbcontext.Corrigendums.Where(x => x.Status == true).ToList();
                List<CorrigendumDetails> Status = new List<CorrigendumDetails>();
                foreach (var item in Corr)
                {
                    CorrigendumDetails details = new CorrigendumDetails();
                    details.Title = item.Title;
                    details.Date = (DateTime)item.Date;
                    details.NITPRNo = NITRNoFunction(item.Id);
                    details.TenderDocument = TenderDocFunction(item.Id);
                    Status.Add(details);
                }

                TenderStatus objtendersdata = new TenderStatus();
                objtendersdata.OpenTender = ObjTender;
                objtendersdata.CorrigendumTender = Status;

                return View(objtendersdata);
            }
        }

        public bool OpenTenderDocPAth(Guid tenderid, string NITPRNo)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {

                bool IsCorrigendumPresent = dbcontext.CorrigendumTenderMappings.Any(x => x.TenderId == tenderid && x.Corrigendum.Status == true);
                if (IsCorrigendumPresent)
                {
                    return true;
                }
            }
            return false;
        }
        public ActionResult TenderCorrigendumList()
        {

            Guid TenderId;
            try
            {
                TenderId = new Guid(Request.QueryString["id"]);
            }
            catch
            {
                TenderId = Guid.Empty;
            }

            if (TenderId == null)
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderDetails);
                return this.Redirect(item.Url());
            }
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                //var CorrigendumIdList = dbcontext.CorrigendumTenderMappings.Where(x => x.TenderId == TenderId).Select(x => x.Corrigendum).ToList();
                var TenderDetail = dbcontext.TenderLists.Where(x => x.Id == TenderId).FirstOrDefault();
                ViewBag.TenderDetail = TenderDetail.NITNo + " - " + TenderDetail.Business;

                List<Corrigendum> Corr = dbcontext.CorrigendumTenderMappings.Where(x => x.TenderId == TenderId && x.Corrigendum.Status == true).Select(x => x.Corrigendum).ToList();
                List<CorrigendumDetails> objCorrigendumDetails = new List<CorrigendumDetails>();
                foreach (var item in Corr)
                {
                    CorrigendumDetails details = new CorrigendumDetails();
                    details.Title = item.Title;
                    details.Date = (DateTime)item.Date;
                    details.TenderDocument = TenderDocFunction(item.Id);
                    objCorrigendumDetails.Add(details);
                }
                return View(objCorrigendumDetails);
            }
        }
        public List<TenderDetails> NITRNoFunction(Guid corrigenudmID)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                List<TenderDetails> list = new List<TenderDetails>();
                var data = dbcontext.CorrigendumTenderMappings.Where(x => x.CorrigendumId == corrigenudmID).Select(x => x.TenderId).ToList();
                foreach (var item in data)
                {
                    var tendersNITList = dbcontext.TenderLists.Where(x => x.Id == item).Select(s => new TenderDetails() { NITPRNo = s.NITNo }).ToList();
                    list.AddRange(tendersNITList);
                }
                return list;
            }
        }
        public List<TenderDetails> TenderDocFunction(Guid corrigenudmID)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                List<TenderDetails> list = new List<TenderDetails>();
                var TenderDocList = dbcontext.CorrigendumDocuments.Where(x => x.CorrigendumId == corrigenudmID).Select(x => new TenderDetails() { DocumentPath = x.DocumentPath, FileName = x.FileName }).ToList();
                list.AddRange(TenderDocList);
                return list;
            }
        }

        //Tender Detail after Login - Based on User and Tender
        public ActionResult TenderLogin()
        {
            return View(new Login());
        }
        //Method: Tender Login
        //used for Login Tender User
        [HttpPost]
        public ActionResult TenderLogin(Login model)
        {
            try
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderDetails);
                var adminTenderListing = Context.Database.GetItem(Templates.Tender.AdminTenderListing);
                if (!ModelState.IsValid)
                {
                    return this.View(model);
                }
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    var registerUser = dbcontext.Registrations.Where(x => x.UserId == model.LoginName && x.Password == model.Password && x.status == true).FirstOrDefault();
                    if (registerUser != null && registerUser.UserType != "SuperAdmin" && registerUser.UserType != "FinancialAdmin")
                    {
                        DateTime tenderClosingDate = dbcontext.TenderLists.Where(t => t.Id == registerUser.TenderId).FirstOrDefault().Closing_Date.Value;
                        if (tenderClosingDate < DateTime.Now && registerUser.UserType.ToLower() == "visitor")
                        {
                            ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Login tender Error", "This Tender is closed, you won't be able to login now."));
                            return this.View(model);
                        }
                    }
                    if (registerUser != null)
                    {
                        TenderUserSession.TenderUserSessionContext = new TenderLoginModel
                        {
                            userId = model.LoginName,
                            TenderId = registerUser.TenderId.ToString(),
                            UserType = registerUser.UserType.ToString()
                        };
                    }
                    else
                    {
                        ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Login User Error", "User and password is not valid. Please enter valid credential."));
                        return this.View(model);
                    }

                    if (TenderUserSession.TenderUserSessionContext.UserType == "SuperAdmin" || TenderUserSession.TenderUserSessionContext.UserType == "FinancialAdmin" || TenderUserSession.TenderUserSessionContext.UserType == "EnvelopeAdmin")
                    {
                        var url = adminTenderListing.Url();
                        return this.Redirect(url);
                    }
                    else
                    {
                        var url = item.Url();
                        return this.Redirect(url);
                    }
                }
            }
            catch (System.Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at TenderLogin Post:" + ex.Message, this);
                ModelState.AddModelError(nameof(model.Password), DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Login Technical Error", "There is technical problem. Please try after sometime."));
                return this.View(model);
            }
        }

        private bool CheckExtension(HttpPostedFileBase postedFile)
        {
            if (!string.Equals(postedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var postedFileExtension = Path.GetExtension(postedFile.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                 && !string.Equals(postedFileExtension, ".pdf", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (postedFile.ContentLength > (1024 * 1024))
            {
                return false;
            }

            return true;

        }

        static byte[] CompressByImageAlg(int jpegQuality, byte[] data)
        {
            using (MemoryStream inputStream = new MemoryStream(data))
            {
                using (System.Drawing.Image image = System.Drawing.Image.FromStream(inputStream))
                {

                    System.Drawing.Imaging.ImageCodecInfo jpegEncoder = ImageCodecInfo.GetImageDecoders()
                        .First(c => c.FormatID == ImageFormat.Jpeg.Guid);
                    var encoderParameters = new EncoderParameters(1);
                    encoderParameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, jpegQuality);
                    //  byte[] outputBytes = null;
                    using (MemoryStream outputStream = new MemoryStream())
                    {
                        image.Save(outputStream, jpegEncoder, encoderParameters);
                        return outputStream.ToArray();
                    }
                }
            }
        }


        //Tender Detail after Login - Based on User and Tender
        public ActionResult TenderDetails()
        {
            Tender Tender = new Tender();
            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }

                if (Session["Message"] != null)
                {
                    ViewBag.Message = Session["Message"].ToString();
                    ViewBag.MessageBoxTitle = Session["MessageBoxTitle"].ToString();
                    ViewBag.MessageBoxButtonText = Session["MessageBoxButtonText"];

                    Session["Message"] = null;
                    Session["MessageBoxTitle"] = null;
                    Session["MessageBoxButtonText"] = null;
                }

                if (TenderUserSession.TenderUserSessionContext != null && !string.IsNullOrEmpty(TenderUserSession.TenderUserSessionContext.TenderId))
                {
                    string tenderId = TenderUserSession.TenderUserSessionContext.TenderId;
                    string UserId = TenderUserSession.TenderUserSessionContext.userId;

                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        var tenderdetails = dbcontext.TenderLists.Where(x => x.Id == new System.Guid(tenderId)).FirstOrDefault();
                        var userdocument = dbcontext.UserTenderDocuments.Where(x => x.UserId == UserId).ToList();
                        var userDetail = dbcontext.Registrations.Where(x => x.UserId == UserId).FirstOrDefault();
                        var tenderFeeDetails = dbcontext.TenderUserFeeEMDDetails.Where(x => x.UserId == UserId).FirstOrDefault();

                        Tender.UserName = userDetail.Name;
                        Tender.Email = userDetail.Email;
                        Tender.CompanyName = userDetail.CompanyName;
                        Tender.Mobile = userDetail.Mobile;
                        Tender.NITNo = tenderdetails.NITNo;
                        Tender.TenderName = tenderdetails.Description;
                        Tender.TenderId = tenderId;
                        Tender.TenderIdDB = userDetail.TenderId;
                        Tender.AdvDate = tenderdetails.Adv_Date;
                        Tender.ClosingDate = tenderdetails.Closing_Date;
                        Tender.tenderdocument = tenderdetails.TenderDocuments.ToList();
                        Tender.userUpladdocument = userdocument;
                        Tender.DocumentStatus = userdocument != null && userdocument.Count > 0 ? "Uploaded" : "Pending";
                        Tender.Business = userDetail.Business;
                        Tender.Category = userDetail.Category;

                        if (tenderFeeDetails != null)
                        {
                            Tender.CustomerCode = tenderFeeDetails.CustomerCode;
                            Tender.VendorCode = tenderFeeDetails.VendorCode;
                            Tender.PANNumber = tenderFeeDetails.PANNumber;
                            Tender.IsPANUploaded = string.IsNullOrEmpty(tenderFeeDetails.PANDocumentData.ToString()) ? false : true;
                            Tender.GSTNumber = tenderFeeDetails.GSTNumber;
                            Tender.IsGSTUploaded = string.IsNullOrEmpty(tenderFeeDetails.GSTDocumentData.ToString()) ? false : true;
                            Tender.IsTenderFeePaid = true;
                            Tender.TenderFeeBankRef = tenderFeeDetails.TenderFeeBankReferenceNo;
                            Tender.TenderFeeBankBranch = tenderFeeDetails.TenderFeeBankBranch;
                            Tender.TenderFeeModeOfPayment = tenderFeeDetails.TenderFeeModeOfPayment;
                            if (tenderFeeDetails.TenderFeeDateOfPayment != null)
                            {
                                Tender.TenderFeeDateOfPayment = tenderFeeDetails.TenderFeeDateOfPayment.GetValueOrDefault().ToString("dd/MM/yyyy");
                            }
                            Tender.TenderFeeAmount = System.Convert.ToDecimal(tenderFeeDetails.TenderFeeAmount);
                            Tender.IsTenderFeeRefUploaded = !string.IsNullOrEmpty(tenderFeeDetails.TenderFeeDocumentData.ToString()) && (tenderFeeDetails.TenderFeeDocumentData.Length > 0) ? true : false;

                            Tender.EMDBankRef = tenderFeeDetails.EMDBankReferenceNo;
                            Tender.EMDBankBranch = tenderFeeDetails.EMDBankBranch;
                            Tender.EMDModeOfPayment = tenderFeeDetails.EMDModeOfPayment;
                            Tender.EMDDateOfPayment = tenderFeeDetails.EMDDateOfPayment.GetValueOrDefault().ToString("dd/MM/yyyy");
                            Tender.EMDAmount = System.Convert.ToDecimal(tenderFeeDetails.EMDFeeAmount);
                            Tender.IsEMDRefUploaded = !string.IsNullOrEmpty(tenderFeeDetails.EMDDocumentData.ToString()) && (tenderFeeDetails.EMDDocumentData.Length > 0) ? true : false;
                        }
                    }
                }
                else
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
            }
            catch (System.Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at TenderDetails Get:" + ex.Message, this);
                ViewBag.errormsg = "There is There is technical problem. Please try after sometime.";
            }

            return View(Tender);
        }
        //Method: tender detail
        //used for Bidder User
        [HttpPost]
        public ActionResult TenderDetails(Tender Model, string tenderFeeandEMD = null, string tenderFeeandEMDUpdate = null, string uploadDoc = null, string changePassword = null)
        {
            if (Session["TenderUserLogin"] == null)
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            if (TenderUserSession.TenderUserSessionContext != null && !string.IsNullOrEmpty(TenderUserSession.TenderUserSessionContext.TenderId))
            {
                ViewBag.MessageBoxTitle = "";
                ViewBag.MessageBoxButtonText = "";
                try
                {
                    bool envelope1DocUploaded = false;
                    bool envelope2DocUploaded = false;
                    bool envelope3DocUploaded = false;
                    if (!string.IsNullOrEmpty(tenderFeeandEMD))
                    {
                        try
                        {
                            if (Model.PANDocument != null && Model.PANDocument.ContentLength > 0 &&
                                Model.GSTDocument != null && Model.GSTDocument.ContentLength > 0 &&

         Model.EMDBankRefDocument != null && Model.EMDBankRefDocument.ContentLength > 0)
                            {
                                if (!CheckExtension(Model.PANDocument))
                                {
                                    ModelState.AddModelError(nameof(Model.PANDocument), DictionaryPhraseRepository.Current.Get("/Accounts/Tender fee/File not valid", "File is not valid."));
                                    return View(GetTenderDetails());
                                }
                                if (!CheckExtension(Model.GSTDocument))
                                {
                                    ModelState.AddModelError(nameof(Model.GSTDocument), DictionaryPhraseRepository.Current.Get("/Accounts/Tender fee/File not valid", "File is not valid."));
                                    return View(GetTenderDetails());
                                }
                                if (Model.TenderFeeBankRefDocument != null)
                                {
                                    if (!CheckExtension(Model.TenderFeeBankRefDocument))
                                    {
                                        ModelState.AddModelError(nameof(Model.TenderFeeBankRefDocument), DictionaryPhraseRepository.Current.Get("/Accounts/Tender fee/File not valid", "File is not valid."));
                                        return View(GetTenderDetails());
                                    }
                                }

                                if (!CheckExtension(Model.EMDBankRefDocument))
                                {
                                    ModelState.AddModelError(nameof(Model.EMDBankRefDocument), DictionaryPhraseRepository.Current.Get("/Accounts/Tender fee/File not valid", "File is not valid."));
                                    return View(GetTenderDetails());
                                }
                                DateTime? TenderFeeDateOfPayment = null;

                                if (Model.TenderFeeDateOfPayment != null)
                                {
                                    TenderFeeDateOfPayment = (DateTime.ParseExact(Model.TenderFeeDateOfPayment, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                                }
                                DateTime EMDDateOfPayment = (DateTime.ParseExact(Model.EMDDateOfPayment, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                                using (TenderDataContext dbcontext = new TenderDataContext())
                                {
                                    //Customer code check 
                                    string customerCode = "";
                                    string vendorCode = "";

                                    TenderBidderCustomerCodeMaster customerWithSameDetails = dbcontext.TenderBidderCustomerCodeMasters.Where(c => c.PANNo.Trim() == Model.PANNumber.Trim() && c.GSTNo == Model.GSTNumber).FirstOrDefault();
                                    if (customerWithSameDetails == null)
                                    {
                                        customerWithSameDetails = dbcontext.TenderBidderCustomerCodeMasters.Where(c => c.GSTNo == Model.GSTNumber).FirstOrDefault();
                                        if (customerWithSameDetails == null)
                                        {
                                            customerWithSameDetails = dbcontext.TenderBidderCustomerCodeMasters.Where(c => c.PANNo.Trim() == Model.PANNumber.Trim()).FirstOrDefault();
                                        }
                                    }
                                    if (customerWithSameDetails != null)
                                    {
                                        customerCode = customerWithSameDetails.CustomerCode;
                                        vendorCode = customerWithSameDetails.VenderCode;
                                    }
                                    if (!dbcontext.TenderBidderCustomerCodeMasters.Any(c => c.PANNo.Trim() == Model.PANNumber.Trim() && c.GSTNo == Model.GSTNumber))
                                    {
                                        //Insert new record in customer code master - update from admin
                                        var userDetail = dbcontext.Registrations.Where(x => x.UserId == TenderUserSession.TenderUserSessionContext.userId).FirstOrDefault();
                                        if (userDetail != null)
                                        {
                                            dbcontext.TenderBidderCustomerCodeMasters.InsertOnSubmit(new TenderBidderCustomerCodeMaster
                                            {
                                                Name = userDetail.Name,
                                                Address = userDetail.CompanyName,
                                                PANNo = Model.PANNumber,
                                                GSTNo = Model.GSTNumber,
                                                CreatedDate = DateTime.Now,
                                                CreatedBy = TenderUserSession.TenderUserSessionContext.userId,
                                                VenderCode = vendorCode,
                                                CustomerCode = customerCode
                                            });
                                        }
                                    }
                                    string TenderFeeBankBranch = String.Empty;
                                    if (Model.TenderFeeBankBranch != null)
                                    {
                                        TenderFeeBankBranch = Model.TenderFeeBankBranch;
                                    }
                                    string TenderFeeBankReferenceNo = String.Empty;
                                    if (Model.TenderFeeBankRef != null)
                                    {
                                        TenderFeeBankReferenceNo = Model.TenderFeeBankRef;
                                    }
                                    string TenderFeeModeOfPayment = String.Empty;
                                    if (Model.TenderFeeModeOfPayment != null)
                                    {
                                        TenderFeeModeOfPayment = Model.TenderFeeModeOfPayment;
                                    }
                                    Byte[] TenderFeeDocumentData = null;
                                    string TenderFeeDocumentContentType = null;
                                    string TenderFeeDocumentName = null;
                                    if (Model.TenderFeeBankRefDocument != null)
                                    {
                                        TenderFeeDocumentData = new BinaryReader(Model.TenderFeeBankRefDocument.InputStream).ReadBytes((Int32)Model.TenderFeeBankRefDocument.InputStream.Length);
                                        TenderFeeDocumentContentType = Model.TenderFeeBankRefDocument.ContentType;
                                        TenderFeeDocumentName = Model.TenderFeeBankRefDocument.FileName;
                                    }

                                    if (Model.TenderIdDB == null)
                                    {
                                        ViewBag.Message += "Please try again";
                                        ViewBag.MessageBoxTitle = "Message";
                                        ViewBag.MessageBoxButtonText = "Ok";
                                        return View(GetTenderDetails());
                                    }
                                    //add tender fee details 
                                    TenderUserFeeEMDDetail objTenderUserFeeEMDDetail = new TenderUserFeeEMDDetail
                                    {
                                        CreatedDate = DateTime.Now,
                                        CustomerCode = customerCode,
                                        VendorCode = vendorCode,
                                        EMDBankBranch = Model.EMDBankBranch,
                                        EMDBankReferenceNo = Model.EMDBankRef,
                                        EMDDateOfPayment = EMDDateOfPayment,
                                        EMDFeeAmount = Model.EMDAmount.ToString(),
                                        EMDModeOfPayment = Model.EMDModeOfPayment,
                                        GSTNumber = Model.GSTNumber,
                                        IsFeePaid = true,
                                        PANNumber = Model.PANNumber,
                                        TenderFeeAmount = Model.TenderFeeAmount.ToString(),
                                        TenderFeeBankBranch = TenderFeeBankBranch,
                                        TenderFeeBankReferenceNo = TenderFeeBankReferenceNo,
                                        TenderFeeDateOfPayment = TenderFeeDateOfPayment,
                                        TenderFeeModeOfPayment = TenderFeeModeOfPayment,
                                        TenderId = Model.TenderIdDB,
                                        UserId = TenderUserSession.TenderUserSessionContext.userId,
                                        PANDocumentData = new BinaryReader(Model.PANDocument.InputStream).ReadBytes((Int32)Model.PANDocument.InputStream.Length),
                                        PANDocumentContentType = Model.PANDocument.ContentType,
                                        PANDocumentName = Model.PANDocument.FileName,
                                        GSTDocumentData = new BinaryReader(Model.GSTDocument.InputStream).ReadBytes((Int32)Model.GSTDocument.InputStream.Length),
                                        GSTDocumentContentType = Model.GSTDocument.ContentType,
                                        GSTDocumentName = Model.GSTDocument.FileName,
                                        TenderFeeDocumentData = TenderFeeDocumentData,
                                        TenderFeeDocumentContentType = TenderFeeDocumentContentType,
                                        TenderFeeDocumentName = TenderFeeDocumentName,
                                        EMDDocumentData = new BinaryReader(Model.EMDBankRefDocument.InputStream).ReadBytes((Int32)Model.EMDBankRefDocument.InputStream.Length),
                                        EMDDocumentContentType = Model.EMDBankRefDocument.ContentType,
                                        EMDDocumentName = Model.EMDBankRefDocument.FileName,
                                    };

                                    dbcontext.TenderUserFeeEMDDetails.InsertOnSubmit(objTenderUserFeeEMDDetail);
                                    dbcontext.SubmitChanges();
                                }

                                ViewBag.Message += "Tender Fee and EMD details are saved successfully!";
                                ViewBag.MessageBoxTitle = "Thank You";
                                ViewBag.MessageBoxButtonText = "Ok";
                                return View(GetTenderDetails());
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Error("Tender fee save " + e.Message, this);
                            ViewBag.Message = "Please upload any file in envelope prior to submit!";
                            ViewBag.MessageBoxTitle = "Message";
                            ViewBag.MessageBoxButtonText = "Ok";
                            return View(GetTenderDetails());
                        }
                    }
                    if (!string.IsNullOrEmpty(tenderFeeandEMDUpdate))
                    {
                        try
                        {
                            using (TenderDataContext dbcontext = new TenderDataContext())
                            {
                                //update tender fee details 
                                string tenderId = TenderUserSession.TenderUserSessionContext.TenderId;
                                string UserId = TenderUserSession.TenderUserSessionContext.userId;
                                TenderUserFeeEMDDetail objTenderUserFeeEMDDetail = dbcontext.TenderUserFeeEMDDetails.Where(t => t.UserId == UserId && t.TenderId.ToString() == tenderId).FirstOrDefault();

                                if (objTenderUserFeeEMDDetail != null)
                                {
                                    if (Model.PANDocument != null && Model.PANDocument.ContentLength > 0)
                                    {
                                        if (!CheckExtension(Model.PANDocument))
                                        {
                                            ModelState.AddModelError(nameof(Model.PANDocument), DictionaryPhraseRepository.Current.Get("/Accounts/Tender fee/File not valid", "File is not valid."));
                                            return View(GetTenderDetails());
                                        }
                                        objTenderUserFeeEMDDetail.PANDocumentData = CompressByImageAlg(10, new BinaryReader(Model.PANDocument.InputStream).ReadBytes((Int32)Model.PANDocument.InputStream.Length));
                                        objTenderUserFeeEMDDetail.PANDocumentContentType = Model.PANDocument.ContentType;
                                        objTenderUserFeeEMDDetail.PANDocumentName = Model.PANDocument.FileName;
                                    }
                                    if (Model.GSTDocument != null && Model.GSTDocument.ContentLength > 0)
                                    {
                                        if (!CheckExtension(Model.GSTDocument))
                                        {
                                            ModelState.AddModelError(nameof(Model.GSTDocument), DictionaryPhraseRepository.Current.Get("/Accounts/Tender fee/File not valid", "File is not valid."));
                                            return View(GetTenderDetails());
                                        }
                                        objTenderUserFeeEMDDetail.GSTDocumentData = CompressByImageAlg(10, new BinaryReader(Model.GSTDocument.InputStream).ReadBytes((Int32)Model.GSTDocument.InputStream.Length));
                                        objTenderUserFeeEMDDetail.GSTDocumentContentType = Model.GSTDocument.ContentType;
                                        objTenderUserFeeEMDDetail.GSTDocumentName = Model.GSTDocument.FileName;
                                    }
                                    if (Model.TenderFeeBankRefDocument != null && Model.TenderFeeBankRefDocument.ContentLength > 0)
                                    {
                                        if (!CheckExtension(Model.TenderFeeBankRefDocument))
                                        {
                                            ModelState.AddModelError(nameof(Model.TenderFeeBankRefDocument), DictionaryPhraseRepository.Current.Get("/Accounts/Tender fee/File not valid", "File is not valid."));
                                            return View(GetTenderDetails());
                                        }

                                        objTenderUserFeeEMDDetail.TenderFeeDocumentData =
                                             CompressByImageAlg(10, new BinaryReader(Model.TenderFeeBankRefDocument.InputStream).ReadBytes((Int32)Model.TenderFeeBankRefDocument.InputStream.Length));
                                        objTenderUserFeeEMDDetail.TenderFeeDocumentContentType = Model.TenderFeeBankRefDocument.ContentType;
                                        objTenderUserFeeEMDDetail.TenderFeeDocumentName = Model.TenderFeeBankRefDocument.FileName;
                                    }
                                    if (Model.EMDBankRefDocument != null && Model.EMDBankRefDocument.ContentLength > 0)
                                    {
                                        if (!CheckExtension(Model.EMDBankRefDocument))
                                        {
                                            ModelState.AddModelError(nameof(Model.EMDBankRefDocument), DictionaryPhraseRepository.Current.Get("/Accounts/Tender fee/File not valid", "File is not valid."));
                                            return View(GetTenderDetails());
                                        }
                                        objTenderUserFeeEMDDetail.EMDDocumentData = CompressByImageAlg(10, new BinaryReader(Model.EMDBankRefDocument.InputStream).ReadBytes((Int32)Model.EMDBankRefDocument.InputStream.Length));
                                        objTenderUserFeeEMDDetail.EMDDocumentContentType = Model.EMDBankRefDocument.ContentType;
                                        objTenderUserFeeEMDDetail.EMDDocumentName = Model.EMDBankRefDocument.FileName;
                                    }
                                    DateTime? TenderFeeDateOfPayment = null;

                                    if (Model.TenderFeeDateOfPayment != null)
                                    {
                                        TenderFeeDateOfPayment = DateTime.ParseExact(Model.TenderFeeDateOfPayment, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                                    }

                                    DateTime EMDDateOfPayment = (DateTime.ParseExact(Model.EMDDateOfPayment, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                                    objTenderUserFeeEMDDetail.ModifiedDate = DateTime.Now;
                                    objTenderUserFeeEMDDetail.EMDBankBranch = Model.EMDBankBranch;
                                    objTenderUserFeeEMDDetail.EMDBankReferenceNo = Model.EMDBankRef;
                                    objTenderUserFeeEMDDetail.EMDDateOfPayment = EMDDateOfPayment;
                                    objTenderUserFeeEMDDetail.EMDFeeAmount = Model.EMDAmount.ToString();
                                    objTenderUserFeeEMDDetail.EMDModeOfPayment = Model.EMDModeOfPayment;
                                    objTenderUserFeeEMDDetail.GSTNumber = Model.GSTNumber;
                                    objTenderUserFeeEMDDetail.PANNumber = Model.PANNumber;
                                    objTenderUserFeeEMDDetail.TenderFeeAmount = Model.TenderFeeAmount.ToString();
                                    objTenderUserFeeEMDDetail.TenderFeeBankBranch = Model.TenderFeeBankBranch;
                                    objTenderUserFeeEMDDetail.TenderFeeBankReferenceNo = Model.TenderFeeBankRef;
                                    objTenderUserFeeEMDDetail.TenderFeeDateOfPayment = TenderFeeDateOfPayment;
                                    objTenderUserFeeEMDDetail.TenderFeeModeOfPayment = Model.TenderFeeModeOfPayment;
                                    string customerCode = "";
                                    string vendorCode = "";
                                    TenderBidderCustomerCodeMaster customerWithSameDetails = dbcontext.TenderBidderCustomerCodeMasters.Where(c => c.PANNo.Trim() == Model.PANNumber.Trim() && c.GSTNo == Model.GSTNumber).FirstOrDefault();
                                    if (customerWithSameDetails == null)
                                    {
                                        customerWithSameDetails = dbcontext.TenderBidderCustomerCodeMasters.Where(c => c.GSTNo == Model.GSTNumber).FirstOrDefault();
                                        if (customerWithSameDetails == null)
                                        {
                                            customerWithSameDetails = dbcontext.TenderBidderCustomerCodeMasters.Where(c => c.PANNo.Trim() == Model.PANNumber.Trim()).FirstOrDefault();
                                        }
                                    }
                                    if (customerWithSameDetails != null && !string.IsNullOrEmpty(customerWithSameDetails.CustomerCode))
                                    {
                                        customerCode = customerWithSameDetails.CustomerCode;
                                    }
                                    if (customerWithSameDetails != null && !string.IsNullOrEmpty(customerWithSameDetails.VenderCode))
                                    {
                                        vendorCode = customerWithSameDetails.VenderCode;
                                    }
                                    objTenderUserFeeEMDDetail.CustomerCode = customerCode;
                                    objTenderUserFeeEMDDetail.VendorCode = vendorCode;
                                    dbcontext.SubmitChanges();
                                    ViewBag.Message += "Tender Fee and EMD details are updated successfully!";
                                    ViewBag.MessageBoxTitle = "Thank You";
                                    ViewBag.MessageBoxButtonText = "Ok";
                                    return View(GetTenderDetails());
                                }
                                else
                                {
                                    ViewBag.Message += "Tender Fee and EMD details record not found!";
                                    ViewBag.MessageBoxTitle = "Message";
                                    ViewBag.MessageBoxButtonText = "Ok";
                                    return View(GetTenderDetails());
                                }
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Error("Tender fee save " + e.Message, this);
                            ViewBag.Message = "Tender Fee and EMD details are not updated!";
                            ViewBag.MessageBoxTitle = "Message";
                            ViewBag.MessageBoxButtonText = "Ok";
                            return View(GetTenderDetails());
                        }
                    }
                    if (!string.IsNullOrEmpty(uploadDoc))
                    {
                        using (TenderDataContext dbcontext = new TenderDataContext())
                        {
                            if (!dbcontext.TenderUserFeeEMDDetails.Where(u => u.UserId == TenderUserSession.TenderUserSessionContext.userId && u.IsFeePaid == true).Any())
                            {
                                ViewBag.Message += "Please provide Tender Fee and EMD details prior to upload documents!";
                                ViewBag.MessageBoxTitle = "Thank You";
                                ViewBag.MessageBoxButtonText = "Ok";
                                return View(GetTenderDetails());
                            }
                        }
                        //to do: check if fees details updated or not
                        if (Model != null)
                        {
                            bool isupload = false;
                            string fileExistError = string.Empty;
                            if (Model.Envelope11 != null || Model.Envelope12 != null || Model.Envelope13 != null || Model.Envelope14 != null || Model.Envelope15 != null
                                || Model.Envelope21 != null || Model.Envelope22 != null || Model.Envelope23 != null || Model.Envelope24 != null || Model.Envelope25 != null || Model.Envelope26 != null || Model.Envelope27 != null || Model.Envelope28 != null || Model.Envelope3 != null)
                            {
                                if (Model.Envelope11 != null || Model.Envelope12 != null || Model.Envelope13 != null || Model.Envelope14 != null || Model.Envelope15 != null)
                                {
                                    envelope1DocUploaded = true;
                                }
                                if (Model.Envelope21 != null || Model.Envelope22 != null || Model.Envelope23 != null || Model.Envelope24 != null || Model.Envelope25 != null || Model.Envelope26 != null || Model.Envelope27 != null || Model.Envelope28 != null)
                                {
                                    envelope2DocUploaded = true;
                                }
                                if (Model.Envelope3 != null)
                                {
                                    envelope3DocUploaded = true;
                                }
                                if (envelope3DocUploaded && (!envelope2DocUploaded || !envelope1DocUploaded))
                                {
                                    string result = IsUploadingInSequesnce("3", TenderUserSession.TenderUserSessionContext.userId, envelope1DocUploaded, envelope2DocUploaded);
                                    if (result != "success")
                                    {
                                        ViewBag.Message = result;
                                        ViewBag.MessageBoxTitle = "Message";
                                        ViewBag.MessageBoxButtonText = "Ok";
                                        ModelState.Clear();
                                        return View(GetTenderDetails());
                                    }
                                }
                                if (envelope2DocUploaded && !envelope1DocUploaded)
                                {
                                    string result = IsUploadingInSequesnce("2", TenderUserSession.TenderUserSessionContext.userId, envelope1DocUploaded, envelope2DocUploaded);
                                    if (result != "success")
                                    {
                                        ViewBag.Message = result;
                                        ViewBag.MessageBoxTitle = "Message";
                                        ViewBag.MessageBoxButtonText = "Ok";
                                        ModelState.Clear();
                                        return View(GetTenderDetails());
                                    }
                                }

                                if (Model.Business == "1")
                                {
                                    if (Model.Category == "1")
                                    {
                                        if (Model.Envelope21 == null || Model.Envelope22 == null || Model.Envelope25 == null || Model.Envelope26 == null || Model.Envelope27 == null)
                                        {
                                            ViewBag.Message += "Please upload all required documents in Envelope 2";
                                            ViewBag.MessageBoxTitle = "Thank You";
                                            ViewBag.MessageBoxButtonText = "Ok";
                                            ModelState.Clear();
                                            return View(GetTenderDetails());
                                        }
                                    }
                                    if (Model.Category == "2")
                                    {
                                        if (Model.Envelope21 == null || Model.Envelope22 == null || Model.Envelope26 == null || Model.Envelope27 == null)
                                        {
                                            ViewBag.Message += "Please upload all required documents in Envelope 2";
                                            ViewBag.MessageBoxTitle = "Thank You";
                                            ViewBag.MessageBoxButtonText = "Ok";
                                            ModelState.Clear();
                                            return View(GetTenderDetails());
                                        }
                                    }
                                    if (Model.Category == "3")
                                    {
                                        if (Model.Envelope21 == null || Model.Envelope22 == null || Model.Envelope25 == null || Model.Envelope26 == null || Model.Envelope27 == null)
                                        {
                                            ViewBag.Message += "Please upload all required documents in Envelope 2";
                                            ViewBag.MessageBoxTitle = "Thank You";
                                            ViewBag.MessageBoxButtonText = "Ok";
                                            ModelState.Clear();
                                            return View(GetTenderDetails());
                                        }
                                    }
                                }
                                else if (Model.Business == "2")
                                {
                                    if (Model.Category == "1")
                                    {
                                        if (Model.Envelope21 == null || Model.Envelope22 == null || Model.Envelope23 == null || Model.Envelope24 == null || Model.Envelope25 == null || Model.Envelope26 == null || Model.Envelope27 == null)
                                        {
                                            ViewBag.Message += "Please upload all required documents in Envelope 2";
                                            ViewBag.MessageBoxTitle = "Thank You";
                                            ViewBag.MessageBoxButtonText = "Ok";
                                            ModelState.Clear();
                                            return View(GetTenderDetails());
                                        }
                                    }
                                    if (Model.Category == "2")
                                    {
                                        if (Model.Envelope21 == null || Model.Envelope22 == null || Model.Envelope26 == null || Model.Envelope27 == null)
                                        {
                                            ViewBag.Message += "Please upload all required documents in Envelope 2";
                                            ViewBag.MessageBoxTitle = "Thank You";
                                            ViewBag.MessageBoxButtonText = "Ok";
                                            ModelState.Clear();
                                            return View(GetTenderDetails());
                                        }
                                    }
                                    if (Model.Category == "3")
                                    {
                                        if (Model.Envelope21 == null || Model.Envelope22 == null || Model.Envelope23 == null || Model.Envelope24 == null || Model.Envelope25 == null || Model.Envelope26 == null || Model.Envelope27 == null)
                                        {
                                            ViewBag.Message += "Please upload all required documents in Envelope 2";
                                            ViewBag.MessageBoxTitle = "Thank You";
                                            ViewBag.MessageBoxButtonText = "Ok";
                                            ModelState.Clear();
                                            return View(GetTenderDetails());
                                        }
                                    }
                                }
                                else if (Model.Business == "3")
                                {
                                    if (Model.Category == "1")
                                    {
                                        if (Model.Envelope21 == null || Model.Envelope22 == null || Model.Envelope23 == null || Model.Envelope24 == null || Model.Envelope25 == null || Model.Envelope26 == null || Model.Envelope27 == null)
                                        {
                                            ViewBag.Message += "Please upload all required documents in Envelope 2";
                                            ViewBag.MessageBoxTitle = "Thank You";
                                            ViewBag.MessageBoxButtonText = "Ok";
                                            ModelState.Clear();
                                            return View(GetTenderDetails());
                                        }
                                    }
                                    if (Model.Category == "2")
                                    {
                                        if (Model.Envelope21 == null || Model.Envelope22 == null || Model.Envelope26 == null || Model.Envelope27 == null)
                                        {
                                            ViewBag.Message += "Please upload all required documents in Envelope 2";
                                            ViewBag.MessageBoxTitle = "Thank You";
                                            ViewBag.MessageBoxButtonText = "Ok";
                                            ModelState.Clear();
                                            return View(GetTenderDetails());
                                        }
                                    }
                                    if (Model.Category == "3")
                                    {
                                        if (Model.Envelope21 == null || Model.Envelope22 == null || Model.Envelope23 == null || Model.Envelope24 == null || Model.Envelope25 == null || Model.Envelope26 == null || Model.Envelope27 == null)
                                        {
                                            ViewBag.Message += "Please upload all required documents in Envelope 2";
                                            ViewBag.MessageBoxTitle = "Thank You";
                                            ViewBag.MessageBoxButtonText = "Ok";
                                            ModelState.Clear();
                                            return View(GetTenderDetails());
                                        }
                                    }
                                }

                                if (Model.Envelope11 != null && Model.Envelope11.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope11, "1", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope1DocUploaded = true;
                                }
                                if (Model.Envelope12 != null && Model.Envelope12.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope12, "1", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope1DocUploaded = true;
                                }


                                if (Model.Envelope13 != null && Model.Envelope13.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope13, "1", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope1DocUploaded = true;
                                }
                                if (Model.Envelope14 != null && Model.Envelope14.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope14, "1", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope1DocUploaded = true;
                                }
                                if (Model.Envelope15 != null && Model.Envelope15.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope15, "1", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope1DocUploaded = true;
                                }

                                if (envelope2DocUploaded == true)
                                {
                                    //delete existing docs 
                                    var isDeleted = DeleteExistingEnvDocs("2", TenderUserSession.TenderUserSessionContext.userId);
                                }

                                if (Model.Envelope21 != null && Model.Envelope21.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope21, "2", TenderUserSession.TenderUserSessionContext.userId,"1");
                                    isupload = true;
                                    envelope2DocUploaded = true;
                                }
                                if (Model.Envelope22 != null && Model.Envelope22.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope22, "2", TenderUserSession.TenderUserSessionContext.userId,"2");
                                    isupload = true;
                                    envelope2DocUploaded = true;
                                }
                                if (Model.Envelope23 != null && Model.Envelope23.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope23, "2", TenderUserSession.TenderUserSessionContext.userId,"3");
                                    isupload = true;
                                    envelope2DocUploaded = true;
                                }
                                if (Model.Envelope24 != null && Model.Envelope24.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope24, "2", TenderUserSession.TenderUserSessionContext.userId,"4");
                                    isupload = true;
                                    envelope2DocUploaded = true;
                                }
                                if (Model.Envelope25 != null && Model.Envelope25.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope25, "2", TenderUserSession.TenderUserSessionContext.userId,"5");
                                    isupload = true;
                                    envelope2DocUploaded = true;
                                }
                                if (Model.Envelope26 != null && Model.Envelope26.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope26, "2", TenderUserSession.TenderUserSessionContext.userId,"6");
                                    isupload = true;
                                    envelope2DocUploaded = true;
                                }
                                if (Model.Envelope27 != null && Model.Envelope27.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope27, "2", TenderUserSession.TenderUserSessionContext.userId,"7");
                                    isupload = true;
                                    envelope2DocUploaded = true;
                                }
                                if (Model.Envelope28 != null && Model.Envelope28.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope28, "2", TenderUserSession.TenderUserSessionContext.userId,"8");
                                    isupload = true;
                                    envelope2DocUploaded = true;
                                }
                                //if (Model.Envelope29 != null && Model.Envelope29.ContentLength > 0)
                                //{
                                //    fileExistError += saveUserTenderFile(Model.Envelope29, "2", TenderUserSession.TenderUserSessionContext.userId);
                                //    isupload = true;
                                //    envelope2DocUploaded = true;
                                //}
                                //if (Model.Envelope30 != null && Model.Envelope30.ContentLength > 0)
                                //{
                                //    fileExistError += saveUserTenderFile(Model.Envelope30, "2", TenderUserSession.TenderUserSessionContext.userId);
                                //    isupload = true;
                                //    envelope2DocUploaded = true;
                                //}

                                if (envelope3DocUploaded == true)
                                {
                                    //delete existing docs 
                                    var isDeleted = DeleteExistingEnvDocs("3", TenderUserSession.TenderUserSessionContext.userId);
                                }


                                if (Model.Envelope3 != null && Model.Envelope3.ContentLength > 0)
                                {
                                    fileExistError += saveUserTenderFile(Model.Envelope3, "3", TenderUserSession.TenderUserSessionContext.userId);
                                    isupload = true;
                                    envelope3DocUploaded = true;
                                }
                            }
                            else
                            {
                                ViewBag.Message = "Please upload any file in envelope prior to submit!";
                                ViewBag.MessageBoxTitle = "Message";
                                ViewBag.MessageBoxButtonText = "Ok";
                                ModelState.Clear();
                                return View(GetTenderDetails());
                            }


                            if (isupload)
                            {
                                if (!string.IsNullOrEmpty(fileExistError))
                                {
                                    ViewBag.Message += fileExistError + " With the Same Name Already Present in Database. Other Files are Uploaded";
                                    ViewBag.MessageBoxTitle = "Message";
                                    ViewBag.MessageBoxButtonText = "Ok";
                                }
                                else
                                {
                                    if (envelope3DocUploaded == true)
                                    {
                                        
                                        ViewBag.Message += "Envelope - 1, 2 & 3 Documents uploaded & submitted Successfully";
                                        ViewBag.MessageBoxTitle = "Thank You";
                                        ViewBag.MessageBoxButtonText = "Ok";
                                    }
                                    else if (envelope2DocUploaded == true)
                                    {
                                        //Update registration table with Business and Category for Envelope II
                                        AddBusinessCategory(Model.Business, Model.Category);

                                        ViewBag.Message += "Documents uploaded successfully in Envelope 1 & 2. Please upload files in Envelope - 3 and Submit";
                                        ViewBag.MessageBoxTitle = "Message";
                                        ViewBag.MessageBoxButtonText = "Continue";
                                    }
                                    else if (envelope1DocUploaded == true)
                                    {
                                        ViewBag.Message += "Documents uploaded successfully in Envelope - 1. Please upload files in Envelope - 2 to proceed further.";
                                        ViewBag.MessageBoxTitle = "Message";
                                        ViewBag.MessageBoxButtonText = "Continue";
                                    }
                                    else
                                    {
                                        ViewBag.Message += "Envelope - 1, 2 & 3 Documents uploaded & submitted Successfully";
                                        ViewBag.MessageBoxTitle = "Thank You";
                                        ViewBag.MessageBoxButtonText = "Ok";
                                    }
                                }
                                Session["Message"] = ViewBag.Message; ;
                                Session["MessageBoxTitle"] = ViewBag.MessageBoxTitle;
                                Session["MessageBoxButtonText"] = ViewBag.MessageBoxButtonText;

                                string redirectURL = Sitecore.Context.Database.GetItem(Templates.Tender.TenderDetails).Url();
                                return this.RedirectPermanent(redirectURL);
                            }
                            else
                            {
                                ViewBag.Message = "There is problem for uploading file. Please try again.";
                                ViewBag.MessageBoxTitle = "Message";
                                ViewBag.MessageBoxButtonText = "Ok";
                            }

                        }
                    }
                    if (!string.IsNullOrEmpty(changePassword))
                    {
                        if (!ModelState.IsValid)
                        {
                            return View(GetTenderDetails());
                        }
                        if (string.IsNullOrEmpty(Model.OldPassword))
                        {
                            ModelState.AddModelError(nameof(Model.OldPassword), DictionaryPhraseRepository.Current.Get("/Accounts/Tender/Required old password", "Please enter old password"));
                            return View(GetTenderDetails());
                        }
                        if (string.IsNullOrEmpty(Model.Password))
                        {
                            ModelState.AddModelError(nameof(Model.Password), DictionaryPhraseRepository.Current.Get("/Accounts/Tender/Required new password", "Please enter New password"));
                            return View(GetTenderDetails());
                        }
                        using (TenderDataContext dbcontext = new TenderDataContext())
                        {
                            var user = dbcontext.Registrations.Where(x => x.UserId == TenderUserSession.TenderUserSessionContext.userId && x.Password == Model.OldPassword).FirstOrDefault();
                            if (user != null)
                            {
                                user.Password = Model.Password;
                                user.Modified_Date = System.DateTime.Now;
                                user.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                                dbcontext.SubmitChanges();
                                ViewBag.Message = "Your password is updated Successfully.";
                                return View(GetTenderDetails());
                            }
                            else
                            {
                                ModelState.AddModelError(nameof(Model.OldPassword), DictionaryPhraseRepository.Current.Get("/Accounts/Tender/Invalid old password", "Invalid Old Password"));
                                return View(GetTenderDetails());
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at TenderDetails Post:" + ex.Message, this);
                }

                return View(GetTenderDetails());
            }
            else
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
        }

        #region Bidder Tender Document Delete
        //Method: Delete Documents for Bidder
        //Used for Delete document of tender
        public ActionResult TenderDetailDocumentDelete(Guid id)
        {

            ElectricityRepository ElecRepo = new ElectricityRepository();
            ElecRepo.DeleteUserTenderDocument(id);
            var item = Context.Database.GetItem(Templates.Tender.TenderDetails);
            ViewBag.Message = "Request successful!";
            return Redirect(item.Url());
        }
        #endregion


        [HttpPost] //Tender Logout
        public ActionResult Logout()
        {
            var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
            if (TenderUserSession.TenderUserSessionContext != null && !string.IsNullOrEmpty(TenderUserSession.TenderUserSessionContext.userId))
            {
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    var result = (from user in dbcontext.Registrations where user.UserId == TenderUserSession.TenderUserSessionContext.userId select user).Single();
                    result.Modified_Date = System.DateTime.Now;
                    result.ModifiedBy = TenderUserSession.TenderUserSessionContext.userId;
                    dbcontext.SubmitChanges();
                }
            }
            this.Session["TenderUserLogin"] = null;
            TenderUserSession.TenderUserSessionContext = null;
            return this.Redirect(item.Url());
        }

        private string IsUploadingInSequesnce(string envelopeType, string userid, bool isUploaded1, bool isUploaded2)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                if (envelopeType == "2")
                {
                    if (!dbcontext.UserTenderDocuments.Any(x => x.UserId == userid && x.EnvelopeType == "1"))
                    {
                        return DictionaryPhraseRepository.Current.Get("/Accounts/Tender/Upload in Envelope II before I", "Please upload document in Envelope I prior to uploading any document in Envelope II.");
                    }
                    else
                    {
                        return "success";
                    }
                }
                else if (envelopeType == "3")
                {
                    if ((!dbcontext.UserTenderDocuments.Any(x => x.UserId == userid && x.EnvelopeType == "1") && !isUploaded1) || (!dbcontext.UserTenderDocuments.Any(x => x.UserId == userid && x.EnvelopeType == "2") && !isUploaded2))
                    {
                        return DictionaryPhraseRepository.Current.Get("/Accounts/Tender/Upload in Envelope III before II", "Please upload document in Envelope I and II prior to uploading any document in Envelope III.");
                    }
                    else
                    {
                        return "success";
                    }
                }

            }
            return "";
        }

        public string saveUserTenderFile(HttpPostedFileBase file, string envelopeType, string userid, string doctype = null)
        {
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                var fileExt = Path.GetExtension(file.FileName);

                var userFile = Path.GetFileName(file.FileName);

                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    if (envelopeType != "2" && dbcontext.UserTenderDocuments.Any(x => x.UserId == userid && x.EnvelopeType == envelopeType && x.FileName == fileName))
                    {
                        string returnError = userFile + " , ";
                        return returnError;
                    }
                    else
                    {
                        string filenamewithtimestamp = fileName.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;
                        var filepath = "/Tender/Uploadedfile/" + filenamewithtimestamp;

                        var path = Path.Combine(Server.MapPath("~/Tender/Uploadedfile"), filenamewithtimestamp);
                        file.SaveAs(path);

                        if (envelopeType == "3")
                        {
                            var userdocument = dbcontext.UserTenderDocuments.Where(x => x.UserId == userid && x.EnvelopeType == envelopeType && x.DocType == doctype).FirstOrDefault();
                            if (userdocument != null)
                            {
                                userdocument.FileName = fileName;
                                userdocument.DocumentPath = filepath;
                                userdocument.Modified_Date = System.DateTime.Now;
                                userdocument.ModifiedBy = userid;
                                userdocument.DocType = doctype;
                                dbcontext.SubmitChanges();
                            }
                            else
                            {
                                UserTenderDocument dbdoc = new UserTenderDocument();
                                dbdoc.Id = Guid.NewGuid();
                                dbdoc.UserId = userid;
                                dbdoc.FileName = fileName;
                                dbdoc.EnvelopeType = envelopeType;
                                dbdoc.DocumentPath = filepath;
                                dbdoc.DocType = doctype;
                                dbdoc.Created_Date = System.DateTime.Now;
                                dbdoc.CreatedBy = userid;
                                dbcontext.UserTenderDocuments.InsertOnSubmit(dbdoc);
                                dbcontext.SubmitChanges();
                            }
                        }
                        else
                        {
                            UserTenderDocument dbdoc = new UserTenderDocument();
                            dbdoc.Id = Guid.NewGuid();
                            dbdoc.UserId = userid;
                            dbdoc.FileName = fileName;
                            dbdoc.DocType = doctype;
                            dbdoc.EnvelopeType = envelopeType;
                            dbdoc.DocumentPath = filepath;
                            dbdoc.Created_Date = System.DateTime.Now;
                            dbdoc.CreatedBy = userid;
                            dbcontext.UserTenderDocuments.InsertOnSubmit(dbdoc);
                            dbcontext.SubmitChanges();
                        }

                    }

                }
            }
            return string.Empty;
        }

        public bool DeleteExistingEnvDocs(string envelopeType, string userid)
        {
            try
            {
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    if (dbcontext.UserTenderDocuments.Any(x => x.UserId == userid && x.EnvelopeType == envelopeType))
                    {
                        List<UserTenderDocument> docsToDelete = dbcontext.UserTenderDocuments.Where(x => x.UserId == userid && x.EnvelopeType == envelopeType).ToList();
                        foreach (var d in docsToDelete)
                        {
                            dbcontext.UserTenderDocuments.DeleteOnSubmit(d);
                        }
                        dbcontext.SubmitChanges();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                Log.Error("Error at DeleteExistingEnvDocs " + e.Message, this);
                return false;
            }
        }


        public Tender GetTenderDetails()
        {
            Tender tender = new Tender();
            try
            {
                string tenderId = TenderUserSession.TenderUserSessionContext.TenderId;
                string UserId = TenderUserSession.TenderUserSessionContext.userId;
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    var tenderdetails = dbcontext.TenderLists.Where(x => x.Id == new System.Guid(tenderId)).FirstOrDefault();
                    var userdocument = dbcontext.UserTenderDocuments.Where(x => x.UserId == UserId).ToList();
                    var userDetail = dbcontext.Registrations.Where(x => x.UserId == UserId).FirstOrDefault();
                    var tenderFeeDetails = dbcontext.TenderUserFeeEMDDetails.Where(x => x.UserId == UserId).FirstOrDefault();

                    tender.UserName = userDetail.Name;
                    tender.Email = userDetail.Email;
                    tender.CompanyName = userDetail.CompanyName;
                    tender.Mobile = userDetail.Mobile;
                    //tender.UserName = tenderdetails.Registrations.Where(x => x.TenderId == new System.Guid(tenderId) && x.UserId == UserId).FirstOrDefault().Name;
                    tender.NITNo = tenderdetails.NITNo;
                    tender.TenderName = tenderdetails.Description;
                    tender.TenderId = tenderId;
                    tender.AdvDate = tenderdetails.Adv_Date;
                    tender.ClosingDate = tenderdetails.Closing_Date;
                    tender.tenderdocument = tenderdetails.TenderDocuments.ToList();
                    tender.userUpladdocument = userdocument;
                    tender.DocumentStatus = userdocument != null && userdocument.Count > 0 ? "Uploaded" : "Pending";
                    tender.Business = userDetail.Business;
                    tender.Category = userDetail.Category;

                    if (tenderFeeDetails != null)
                    {
                        tender.CustomerCode = tenderFeeDetails.CustomerCode;
                        tender.VendorCode = tenderFeeDetails.VendorCode;
                        tender.PANNumber = tenderFeeDetails.PANNumber;
                        tender.IsPANUploaded = string.IsNullOrEmpty(tenderFeeDetails.PANDocumentData.ToString()) ? false : true;

                        tender.GSTNumber = tenderFeeDetails.GSTNumber;
                        tender.IsGSTUploaded = string.IsNullOrEmpty(tenderFeeDetails.GSTDocumentData.ToString()) ? false : true;
                        tender.IsTenderFeePaid = true;
                        tender.TenderFeeBankRef = tenderFeeDetails.TenderFeeBankReferenceNo;
                        tender.TenderFeeBankBranch = tenderFeeDetails.TenderFeeBankBranch;
                        tender.TenderFeeModeOfPayment = tenderFeeDetails.TenderFeeModeOfPayment;
                        tender.TenderFeeAmount = System.Convert.ToDecimal(tenderFeeDetails.TenderFeeAmount);


                        if (tenderFeeDetails.TenderFeeDateOfPayment != null)
                        {
                            tender.TenderFeeDateOfPayment = tenderFeeDetails.TenderFeeDateOfPayment.GetValueOrDefault().ToString("dd/MM/yyyy");
                        }

                        tender.IsTenderFeeRefUploaded = !string.IsNullOrEmpty(tenderFeeDetails.TenderFeeDocumentData.ToString()) && (tenderFeeDetails.TenderFeeDocumentData.Length > 0) ? true : false;

                        tender.EMDBankRef = tenderFeeDetails.EMDBankReferenceNo;
                        tender.EMDBankBranch = tenderFeeDetails.EMDBankBranch;
                        tender.EMDModeOfPayment = tenderFeeDetails.EMDModeOfPayment;
                        tender.EMDDateOfPayment = tenderFeeDetails.EMDDateOfPayment.GetValueOrDefault().ToString("dd/MM/yyyy");
                        tender.EMDAmount = System.Convert.ToDecimal(tenderFeeDetails.EMDFeeAmount);
                        tender.IsEMDRefUploaded = string.IsNullOrEmpty(tenderFeeDetails.EMDDocumentData.ToString()) ? false : true;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetTenderDetails Get:" + ex.Message, this);
            }
            return tender;
        }

        public FileResult DownloadUserFile(string tenderid, string userid, string type)
        {
            try
            {
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    var fileRecordToDownload = dbcontext.TenderUserFeeEMDDetails.Where(i => i.UserId == userid && i.TenderId.ToString() == tenderid).FirstOrDefault();
                    if (fileRecordToDownload != null)
                    {
                        if (type == "pan")
                            return File(fileRecordToDownload.PANDocumentData.ToArray(), fileRecordToDownload.PANDocumentContentType, fileRecordToDownload.PANDocumentName);
                        else if (type == "gst")
                            return File(fileRecordToDownload.GSTDocumentData.ToArray(), fileRecordToDownload.GSTDocumentContentType, fileRecordToDownload.GSTDocumentName);
                        else if (type == "tenderfee")
                            return File(fileRecordToDownload.TenderFeeDocumentData.ToArray(), fileRecordToDownload.TenderFeeDocumentContentType, fileRecordToDownload.TenderFeeDocumentName);
                        else if (type == "emd")
                            return File(fileRecordToDownload.EMDDocumentData.ToArray(), fileRecordToDownload.EMDDocumentContentType, fileRecordToDownload.EMDDocumentName);
                        else
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at DownloadFile:" + ex.Message, this);
            }
            return null;
        }

        public FileResult DownloadFile(string id)
        {
            try
            {
                if (TenderUserSession.TenderUserSessionContext != null)
                {
                    string tenderId = TenderUserSession.TenderUserSessionContext.TenderId;
                    string UserId = TenderUserSession.TenderUserSessionContext.userId;
                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        var fileRecordToDownload = dbcontext.TenderUserFeeEMDDetails.Where(i => i.UserId == UserId && i.TenderId.ToString() == tenderId).FirstOrDefault();

                        if (id == "pan")
                            return File(fileRecordToDownload.PANDocumentData.ToArray(), fileRecordToDownload.PANDocumentContentType, fileRecordToDownload.PANDocumentName);
                        else if (id == "gst")
                            return File(fileRecordToDownload.GSTDocumentData.ToArray(), fileRecordToDownload.GSTDocumentContentType, fileRecordToDownload.GSTDocumentName);
                        else if (id == "tenderfee")
                            return File(fileRecordToDownload.TenderFeeDocumentData.ToArray(), fileRecordToDownload.TenderFeeDocumentContentType, fileRecordToDownload.TenderFeeDocumentName);
                        else if (id == "emd")
                            return File(fileRecordToDownload.EMDDocumentData.ToArray(), fileRecordToDownload.EMDDocumentContentType, fileRecordToDownload.EMDDocumentName);
                        else
                            return null;
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at DownloadFile:" + ex.Message, this);
            }
            return null;
        }

        public bool AddBusinessCategory(string business, string category)
        {
            try
            {
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    Registration details = dbcontext.Registrations.Where(u => u.UserId == TenderUserSession.TenderUserSessionContext.userId).FirstOrDefault();
                    details.Business = business;
                    details.Category = category;
                    dbcontext.SubmitChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error("Update registration error " + e.Message, this);
                return false;
            }
        }

        //Method: User Registration
        public ActionResult UserRegistration(UserDetails model)
        {
            var listPage = Sitecore.Context.Database.GetItem(Template.Tender.TenderList);
            if (ModelState.IsValid)
            {
                UtilityFunction ut = new UtilityFunction();
                try
                {
                    var userid = ut.GenerateRandomUserId();
                    var password = ut.GenerateRandomPassword();
                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        Registration ObjReg = new Registration();
                        Guid obj = Guid.NewGuid();
                        ObjReg.Id = obj;
                        ObjReg.UserId = userid;
                        ObjReg.Password = password;
                        ObjReg.Name = model.Name;
                        ObjReg.CompanyName = model.Company;
                        ObjReg.Fax_No = model.FaxNo;
                        ObjReg.TenderId = model.TenderID;
                        ObjReg.Email = model.Email;
                        ObjReg.Mobile = model.MobileNo;
                        ObjReg.Name = model.Name;
                        ObjReg.UserType = "Visitor";
                        ObjReg.status = true;
                        ObjReg.Created_Date = System.DateTime.Now;
                        ObjReg.CreatedBy = model.Email;
                        dbcontext.Registrations.InsertOnSubmit(ObjReg);
                        dbcontext.SubmitChanges();
                    }

                    string RedirectUrl = string.Empty;
                    var tenderLoginitem = Sitecore.Context.Database.GetItem(Template.Tender.TenderLogin);
                    if (tenderLoginitem != null)
                    {
                        string baseurl = tenderLoginitem.Url();
                        RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                    }
                    try
                    {
                        if (!string.IsNullOrEmpty(RedirectUrl))
                        {
                            using (TenderDataContext dbcontext = new TenderDataContext())
                            {
                                var tender = dbcontext.TenderLists.Where(x => x.Id == model.TenderID).FirstOrDefault();
                                //Note : Send Mail to User 
                                TenderService Tn = new TenderService();
                                Tn.SendPasswordResetLink(model.Email, RedirectUrl, userid, password, tender.NITNo, tender.Description);
                                TempData["Success"] = "You have successfully registered.Please check you mail box to continue.";
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        Log.Error($"Email Sending Error at User Registration:" + e.Message, e, this);
                        TempData["Success"] = "You have successfully registered. Some issue in Email sending!";
                    }

                }
                catch (MembershipCreateUserException ex)
                {
                    Log.Error($"Can't create user with", ex, this);
                    TempData["Error"] = "Please try Again later.";
                    this.ModelState.AddModelError("Not able to create User", ex.Message);
                }
            }
            else
            {
                TempData["Error"] = "Please provide valid information for registration and try again!";
            }
            return Redirect(listPage.Url());
        }

        public ActionResult GetTenderDocument(Guid Id)
        {
            try
            {
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    List<TenderDocument> TD = dbcontext.TenderDocuments.Where(x => x.TenderId == Id).ToList();
                    return PartialView("GetTenderDocument", TD);
                }
            }
            catch (MembershipCreateUserException ex)
            {
                Log.Error($"Can't create user with", ex, this);
                this.ModelState.AddModelError("GetTenderDocument", ex.Message);
            }
            return RedirectToAction("TenderListing");

        }

        #region TenderAdmin
        //Method: for Tender listing 
        //taking all Data from TenderList table
        public ActionResult AdminTenderListing()
        {
            List<TenderDetails> ObjTender = new List<TenderDetails>();
            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
                string UserId = TenderUserSession.TenderUserSessionContext.userId;
                string UserType = TenderUserSession.TenderUserSessionContext.UserType;
                ViewBag.UserType = UserType;
                List<TenderList> tenderdata = new List<TenderList>();
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    TenderList objtender = new TenderList();
                    if (UserType == "SuperAdmin" || UserType == "FinancialAdmin")
                    {
                        tenderdata = dbcontext.TenderLists.OrderByDescending(x => x.Closing_Date).ToList();
                    }
                    else // Envelope Admin
                    {
                        tenderdata = (from tdruser in dbcontext.UserTenderMappings join tnderlist in dbcontext.TenderLists on tdruser.TenderId equals tnderlist.Id orderby tnderlist.Closing_Date descending where tdruser.UserId == UserId select tnderlist).ToList();
                    }

                    foreach (var data in tenderdata)
                    {
                        TenderDetails ObjTd = new TenderDetails();
                        ObjTd.Id = data.Id;
                        ObjTd.NITPRNo = data.NITNo;
                        ObjTd.Business = data.Business;
                        ObjTd.Description = data.Description;
                        ObjTd.Cost_of_EMD = data.Cost_of_EMD;
                        ObjTd.Estimated_Cost = data.Estimated_Cost;
                        ObjTd.Adv_Date = data.Adv_Date;
                        ObjTd.Bid_Submision_ClosingDate = data.Closing_Date.ToString();
                        ObjTd.CreatedDate = data.Created_Date;
                        ObjTd.ModifiedDate = data.Modified_Date;
                        ObjTd.Status = data.Staus;
                        ObjTd.ClosingDate = data.Closing_Date;
                        ObjTd.Tenderdocs = data.TenderDocuments.Where(x => x.TenderId == data.Id).ToList();
                        ObjTender.Add(ObjTd);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderListing Get:" + ex.Message, this);
            }
            return View(ObjTender);
        }

        //Method: Admin Tender Listing method based on seach using Dropdown
        //Return List based on Selected Dowpdown Status from admin tender screen
        //For Super Admin
        [HttpPost]
        public ActionResult AdminTenderListing(string Status)
        {
            List<TenderDetails> ObjTender = new List<TenderDetails>();
            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
                string UserId = TenderUserSession.TenderUserSessionContext.userId;
                string UserType = TenderUserSession.TenderUserSessionContext.UserType;
                ViewBag.UserType = UserType;
                List<TenderList> tenderdata = new List<TenderList>();
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    TenderList objtender = new TenderList();
                    if (UserType == "SuperAdmin")
                    {
                        tenderdata = dbcontext.TenderLists.OrderByDescending(x => x.Created_Date).ToList();
                    }
                    else // Envelope Admin
                    {
                        tenderdata = (from tdruser in dbcontext.UserTenderMappings join tnderlist in dbcontext.TenderLists on tdruser.TenderId equals tnderlist.Id where tdruser.UserId == UserId select tnderlist).ToList();
                    }

                    foreach (var data in tenderdata)
                    {
                        TenderDetails ObjTd = new TenderDetails();
                        ObjTd.Id = data.Id;
                        ObjTd.NITPRNo = data.NITNo;
                        ObjTd.Business = data.Business;
                        ObjTd.Description = data.Description;
                        ObjTd.Adv_Date = data.Adv_Date;
                        ObjTd.Bid_Submision_ClosingDate = data.Closing_Date.ToString();
                        ObjTd.Cost_of_EMD = data.Cost_of_EMD;
                        ObjTd.Estimated_Cost = data.Estimated_Cost;
                        ObjTd.CreatedDate = data.Created_Date;
                        ObjTd.ModifiedDate = data.Modified_Date;
                        ObjTd.Status = data.Staus.ToLower();
                        ObjTd.ClosingDate = data.Closing_Date;
                        ObjTender.Add(ObjTd);
                    }
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderListing Get:" + ex.Message, this);
            }
            var selectedData = ObjTender;
            if (Status != "All")
            {
                selectedData = ObjTender.Where(x => x.Status == Status.ToLower()).ToList();
            }

            return View(selectedData);
        }
        //Method: User Listing For Tender
        //Return List of user on selecteed Tender
        public ActionResult AdminTenderUserListing()
        {
            var datalist = new List<TenderBidderDetails>();
            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    Guid tenderId = new Guid(Request.QueryString["id"].ToString());
                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        var TenderNITNo = dbcontext.TenderLists.Where(x => x.Id == tenderId).FirstOrDefault();
                        if (TenderNITNo != null)
                        {
                            datalist = dbcontext.Registrations.Where(x => x.TenderId == tenderId && x.status == true && x.UserType.ToLower() == "visitor").Select(s => new TenderBidderDetails
                            {
                                UserName = s.Name,
                                CompanyName = s.CompanyName.Replace(",", "-"),
                                Email = s.Email,
                                Mobile = s.Mobile,
                                UserId = s.UserId
                            }).ToList();

                            foreach (var user in datalist)
                            {
                                user.TenderId = TenderNITNo.Id.ToString();
                                user.TenderName = TenderNITNo.Description.Replace(",", "-");
                                user.TenderNITNo = TenderNITNo.NITNo;
                                var userDocs = dbcontext.UserTenderDocuments.Where(x => x.UserId == user.UserId).ToList();
                                if (userDocs.Any(x => x.EnvelopeType == "1") && userDocs.Any(x => x.EnvelopeType == "2") && userDocs.Any(x => x.EnvelopeType == "3"))
                                {
                                    user.HasDocumentUploaded = "Yes";
                                    user.DateOfUpload = userDocs.Max(u => u.Created_Date);
                                }
                                else
                                    user.HasDocumentUploaded = "No";

                                var userFeeEMDDetails = dbcontext.TenderUserFeeEMDDetails.Where(u => u.UserId == user.UserId && u.TenderId == tenderId).FirstOrDefault();
                                if (userFeeEMDDetails != null)
                                {
                                    user.IsTenderFeePaid = "Yes";
                                    user.CustomerCode = userFeeEMDDetails.CustomerCode;
                                    user.VendorCode = userFeeEMDDetails.VendorCode;
                                    user.PANNumber = userFeeEMDDetails.PANNumber;
                                    user.GSTNumber = userFeeEMDDetails.GSTNumber;
                                    user.TenderFeeBankRef = userFeeEMDDetails.TenderFeeBankReferenceNo;
                                    user.TenderFeeBankBranch = userFeeEMDDetails.TenderFeeBankBranch;
                                    user.TenderFeeModeOfPayment = userFeeEMDDetails.TenderFeeModeOfPayment;
                                    user.TenderFeeDateOfPayment = userFeeEMDDetails.TenderFeeDateOfPayment.GetValueOrDefault().ToString("dd/MM/yyyy");
                                    user.TenderFeeAmount = userFeeEMDDetails.TenderFeeAmount;
                                    user.EMDBankRef = userFeeEMDDetails.EMDBankReferenceNo;
                                    user.EMDBankBranch = userFeeEMDDetails.EMDBankBranch;
                                    user.EMDModeOfPayment = userFeeEMDDetails.EMDModeOfPayment;
                                    user.EMDDateOfPayment = userFeeEMDDetails.EMDDateOfPayment.GetValueOrDefault().ToString("dd/MM/yyyy");
                                    user.EMDAmount = userFeeEMDDetails.EMDFeeAmount;
                                }
                                else
                                {
                                    user.IsTenderFeePaid = "No";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderUserListing Get:" + ex.Message, this);
            }
            return View(datalist);
        }
        //Method: Admin Tender User Export Data
        //Used for Export Tender Corrigendum Datainto CSV
        public ActionResult AdminExportUserData()
        {
            var ReturnURL = Context.Database.GetItem(Templates.Tender.AdminUserTenderListing);
            var datalist = new List<TenderBidderDetails>();

            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
                if (!string.IsNullOrEmpty(Request.QueryString["id"]))
                {
                    Guid tenderId = new Guid(Request.QueryString["id"]);
                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        var TenderNITNo = dbcontext.TenderLists.Where(x => x.Id == tenderId).FirstOrDefault();
                        if (TenderNITNo != null)
                        {
                            datalist = dbcontext.Registrations.Where(x => x.TenderId == tenderId && x.status == true && x.UserType.ToLower() == "visitor").Select(s => new TenderBidderDetails
                            {
                                UserName = s.Name,
                                CompanyName = s.CompanyName.Replace(",", "-"),
                                Email = s.Email,
                                Mobile = s.Mobile,
                                UserId = s.UserId
                            }).ToList();

                            foreach (var user in datalist)
                            {
                                user.TenderId = TenderNITNo.Id.ToString();
                                user.TenderName = TenderNITNo.Description.Replace(",", "-");
                                user.TenderNITNo = TenderNITNo.NITNo;
                                var userDocs = dbcontext.UserTenderDocuments.Where(x => x.UserId == user.UserId).ToList();
                                if (userDocs.Any(x => x.EnvelopeType == "1") && userDocs.Any(x => x.EnvelopeType == "2") && userDocs.Any(x => x.EnvelopeType == "3"))
                                {
                                    user.HasDocumentUploaded = "Yes";
                                    user.DateOfUpload = userDocs.Max(u => u.Created_Date);
                                }
                                else
                                    user.HasDocumentUploaded = "No";

                                var userFeeEMDDetails = dbcontext.TenderUserFeeEMDDetails.Where(u => u.UserId == user.UserId && u.TenderId == tenderId).FirstOrDefault();
                                if (userFeeEMDDetails != null)
                                {
                                    user.IsTenderFeePaid = "Yes";
                                    user.CustomerCode = userFeeEMDDetails.CustomerCode;
                                    user.VendorCode = userFeeEMDDetails.VendorCode;
                                    user.PANNumber = userFeeEMDDetails.PANNumber;
                                    user.GSTNumber = userFeeEMDDetails.GSTNumber;
                                    user.TenderFeeBankRef = userFeeEMDDetails.TenderFeeBankReferenceNo;
                                    user.TenderFeeBankBranch = userFeeEMDDetails.TenderFeeBankBranch;
                                    user.TenderFeeModeOfPayment = userFeeEMDDetails.TenderFeeModeOfPayment;
                                    user.TenderFeeDateOfPayment = userFeeEMDDetails.TenderFeeDateOfPayment.GetValueOrDefault().ToString("dd/MM/yyyy");
                                    user.TenderFeeAmount = userFeeEMDDetails.TenderFeeAmount;
                                    user.EMDBankRef = userFeeEMDDetails.EMDBankReferenceNo;
                                    user.EMDBankBranch = userFeeEMDDetails.EMDBankBranch;
                                    user.EMDModeOfPayment = userFeeEMDDetails.EMDModeOfPayment;
                                    user.EMDDateOfPayment = userFeeEMDDetails.EMDDateOfPayment.GetValueOrDefault().ToString("dd/MM/yyyy");
                                    user.EMDAmount = userFeeEMDDetails.EMDFeeAmount;
                                }
                                else
                                {
                                    user.IsTenderFeePaid = "No";
                                }
                            }
                            DataTable table = ToDataTable(datalist);
                            DatatableToCSV(table, TenderNITNo.NITNo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderUserListing Get:" + ex.Message, this);
            }
            return Redirect(ReturnURL.Url());
        }

        public ActionResult AdminExportUserDataFeeDetails(string tenderid, string userid)
        {
            var ReturnURL = Context.Database.GetItem(Templates.Tender.AdminUserTenderListing);
            var datalist = new List<TenderBidderDetails>();

            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }

                if (!string.IsNullOrEmpty(tenderid) && !string.IsNullOrEmpty(userid))
                {
                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        var TenderNITNo = dbcontext.TenderLists.Where(x => x.Id.ToString() == tenderid).FirstOrDefault();
                        if (TenderNITNo != null)
                        {
                            datalist = dbcontext.Registrations.Where(x => x.TenderId.ToString() == tenderid && x.status == true && x.UserType.ToLower() == "visitor" && x.UserId == userid).Select(s => new TenderBidderDetails
                            {
                                UserName = s.Name,
                                CompanyName = s.CompanyName.Replace(",", "-"),
                                Email = s.Email,
                                Mobile = s.Mobile,
                                UserId = s.UserId
                            }).ToList();

                            foreach (var user in datalist)
                            {
                                user.TenderId = TenderNITNo.Id.ToString();
                                user.TenderName = TenderNITNo.Description.Replace(",", "-");
                                user.TenderNITNo = TenderNITNo.NITNo;
                                var userDocs = dbcontext.UserTenderDocuments.Where(x => x.UserId == user.UserId).ToList();
                                if (userDocs.Any(x => x.EnvelopeType == "1") && userDocs.Any(x => x.EnvelopeType == "2") && userDocs.Any(x => x.EnvelopeType == "3"))
                                {
                                    user.HasDocumentUploaded = "Yes";
                                    user.DateOfUpload = userDocs.Max(u => u.Created_Date);
                                }
                                else
                                    user.HasDocumentUploaded = "No";

                                var userFeeEMDDetails = dbcontext.TenderUserFeeEMDDetails.Where(u => u.UserId == user.UserId && u.TenderId.ToString() == tenderid).FirstOrDefault();
                                if (userFeeEMDDetails != null)
                                {
                                    user.IsTenderFeePaid = "Yes";
                                    user.CustomerCode = userFeeEMDDetails.CustomerCode;
                                    user.VendorCode = userFeeEMDDetails.VendorCode;
                                    user.PANNumber = userFeeEMDDetails.PANNumber;
                                    user.GSTNumber = userFeeEMDDetails.GSTNumber;
                                    user.TenderFeeBankRef = userFeeEMDDetails.TenderFeeBankReferenceNo;
                                    user.TenderFeeBankBranch = userFeeEMDDetails.TenderFeeBankBranch;
                                    user.TenderFeeModeOfPayment = userFeeEMDDetails.TenderFeeModeOfPayment;
                                    user.TenderFeeDateOfPayment = userFeeEMDDetails.TenderFeeDateOfPayment.GetValueOrDefault().ToString("dd/MM/yyyy");
                                    user.TenderFeeAmount = userFeeEMDDetails.TenderFeeAmount;
                                    user.EMDBankRef = userFeeEMDDetails.EMDBankReferenceNo;
                                    user.EMDBankBranch = userFeeEMDDetails.EMDBankBranch;
                                    user.EMDModeOfPayment = userFeeEMDDetails.EMDModeOfPayment;
                                    user.EMDDateOfPayment = userFeeEMDDetails.EMDDateOfPayment.GetValueOrDefault().ToString("dd/MM/yyyy");
                                    user.EMDAmount = userFeeEMDDetails.EMDFeeAmount;
                                }
                                else
                                {
                                    user.IsTenderFeePaid = "No";
                                }
                            }
                            DataTable table = ToDataTable(datalist);
                            DatatableToCSV(table, TenderNITNo.NITNo);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderUserListing Get:" + ex.Message, this);
            }
            return Redirect(ReturnURL.Url());
        }

        //Method: Admin Tender Details
        //used for Tender Details for Super Admin User
        public ActionResult AdminTenderDetails()
        {
            Tender tender = new Tender();
            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }

                if (!string.IsNullOrEmpty(Request.QueryString["id"]) && !string.IsNullOrEmpty(Request.QueryString["uid"]))
                {
                    string UserId = TenderUserSession.TenderUserSessionContext.userId;
                    string UserType = TenderUserSession.TenderUserSessionContext.UserType;

                    Guid tenderId = new Guid(Request.QueryString["id"].ToString());
                    string tenderuserid = Request.QueryString["uid"].ToString();

                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        var tenderdetails = dbcontext.TenderLists.Where(x => x.Id == tenderId).FirstOrDefault();
                        var userdocument = dbcontext.UserTenderDocuments.Where(x => x.UserId == tenderuserid).ToList();
                        var userDetail = dbcontext.Registrations.Where(x => x.UserId == UserId).FirstOrDefault();
                        var tenderfeeDetails = dbcontext.TenderUserFeeEMDDetails.Where(x => x.UserId == tenderuserid && x.TenderId == tenderId).FirstOrDefault();

                        tender.UserName = tenderdetails.Registrations.Where(x => x.TenderId == tenderId && x.UserId == tenderuserid).FirstOrDefault().Name;
                        tender.Email = tenderdetails.Registrations.Where(x => x.TenderId == tenderId && x.UserId == tenderuserid).FirstOrDefault().Email;
                        tender.CompanyName = tenderdetails.Registrations.Where(x => x.TenderId == tenderId && x.UserId == tenderuserid).FirstOrDefault().CompanyName;
                        tender.Mobile = tenderdetails.Registrations.Where(x => x.TenderId == tenderId && x.UserId == tenderuserid).FirstOrDefault().Mobile;
                        tender.NITNo = tenderdetails.NITNo;
                        tender.TenderName = tenderdetails.Description;
                        tender.TenderId = Request.QueryString["id"].ToString();
                        tender.userUpladdocument = userdocument;
                        tender.AdvDate = tenderdetails.Adv_Date;
                        tender.ClosingDate = tenderdetails.Closing_Date;
                        tender.DocumentStatus = userdocument != null && userdocument.Count > 0 ? "Uploaded" : "Pending";
                        ViewBag.Envelopetype = UserType == "SuperAdmin" || UserType == "FinancialAdmin" ? "0" : dbcontext.UserTenderMappings.Where(x => x.UserId == UserId && x.TenderId == tenderId).FirstOrDefault().Envelope;


                        tender.Business = tenderdetails.Registrations.Where(x => x.TenderId == tenderId && x.UserId == tenderuserid).FirstOrDefault().Business;
                        tender.Category = tenderdetails.Registrations.Where(x => x.TenderId == tenderId && x.UserId == tenderuserid).FirstOrDefault().Category;

                        var loggedinuser = dbcontext.Registrations.Where(r => r.UserId == UserId).FirstOrDefault();
                        if (!string.IsNullOrEmpty(loggedinuser.BuyerType) && loggedinuser.BuyerType.ToLower().Contains("buyer"))
                        {
                            tender.IsBuyer = true;
                        }
                        if (UserType == "SuperAdmin")
                        {
                            tender.IsBuyer = true;
                        }
                        if (UserType == "FinancialAdmin")
                        {
                            tender.IsFinancialAdmin = true;
                        }

                        if (tenderfeeDetails != null)
                        {
                            tender.IsTenderFeePaid = true;
                            tender.CustomerCode = tenderfeeDetails.CustomerCode;
                            tender.VendorCode = tenderfeeDetails.VendorCode;
                            tender.PANNumber = tenderfeeDetails.PANNumber;
                            tender.IsPANUploaded = string.IsNullOrEmpty(tenderfeeDetails.PANDocumentData.ToString()) ? false : true;
                            tender.GSTNumber = tenderfeeDetails.GSTNumber;
                            tender.IsGSTUploaded = string.IsNullOrEmpty(tenderfeeDetails.GSTDocumentData.ToString()) ? false : true;
                            tender.IsTenderFeePaid = true;
                            tender.TenderFeeBankRef = tenderfeeDetails.TenderFeeBankReferenceNo;
                            tender.TenderFeeBankBranch = tenderfeeDetails.TenderFeeBankBranch;
                            tender.TenderFeeModeOfPayment = tenderfeeDetails.TenderFeeModeOfPayment;
                            if (tenderfeeDetails.TenderFeeDateOfPayment != null)
                            {
                                tender.TenderFeeDateOfPayment = tenderfeeDetails.TenderFeeDateOfPayment.GetValueOrDefault().ToString("dd/MM/yyyy");
                            }

                            tender.IsTenderFeeRefUploaded = !string.IsNullOrEmpty(tenderfeeDetails.TenderFeeDocumentData.ToString()) && (tenderfeeDetails.TenderFeeDocumentData.Length > 0) ? true : false;
                            tender.TenderFeeAmount = System.Convert.ToDecimal(tenderfeeDetails.TenderFeeAmount);

                            tender.EMDBankRef = tenderfeeDetails.EMDBankReferenceNo;
                            tender.EMDBankBranch = tenderfeeDetails.EMDBankBranch;
                            tender.EMDModeOfPayment = tenderfeeDetails.EMDModeOfPayment;
                            tender.EMDDateOfPayment = tenderfeeDetails.EMDDateOfPayment.GetValueOrDefault().ToString("dd/MM/yyyy");
                            tender.EMDAmount = System.Convert.ToDecimal(tenderfeeDetails.EMDFeeAmount);
                            tender.IsEMDRefUploaded = string.IsNullOrEmpty(tenderfeeDetails.EMDDocumentData.ToString()) ? false : true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderUserListing Get:" + ex.Message, this);
            }
            return View(tender);
        }

        [HttpPost]
        public ActionResult AdminTenderDetails(Tender tender, string updateCustomerCode = null, string updateVendorCode = null)
        {
            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
                Guid tenderId = new Guid(Request["tenderid"].ToString());
                string tenderuserid = Request["userid"].ToString();

                if (!string.IsNullOrEmpty(updateCustomerCode))
                {
                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        var tenderfeeDetails = dbcontext.TenderUserFeeEMDDetails.Where(x => x.UserId == tenderuserid && x.TenderId == tenderId).FirstOrDefault();
                        if (tenderfeeDetails != null)
                        {
                            tenderfeeDetails.CustomerCode = tender.CustomerCode;

                            var userDetail = dbcontext.TenderBidderCustomerCodeMasters.Where(x => x.PANNo == tenderfeeDetails.PANNumber && x.GSTNo == tenderfeeDetails.GSTNumber).FirstOrDefault();
                            if (userDetail != null)
                            {
                                userDetail.CustomerCode = tender.CustomerCode;
                                //userDetail.VenderCode = tender.VendorCode;
                            }
                            dbcontext.SubmitChanges();
                        }
                    }
                }
                if (!string.IsNullOrEmpty(updateVendorCode))
                {
                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        var tenderfeeDetails = dbcontext.TenderUserFeeEMDDetails.Where(x => x.UserId == tenderuserid && x.TenderId == tenderId).FirstOrDefault();
                        if (tenderfeeDetails != null)
                        {
                            tenderfeeDetails.VendorCode = tender.VendorCode;

                            var userDetail = dbcontext.TenderBidderCustomerCodeMasters.Where(x => x.PANNo == tenderfeeDetails.PANNumber && x.GSTNo == tenderfeeDetails.GSTNumber).FirstOrDefault();
                            if (userDetail != null)
                            {
                                //userDetail.CustomerCode = tender.CustomerCode;
                                userDetail.VenderCode = tender.VendorCode;
                            }
                            dbcontext.SubmitChanges();
                        }
                    }
                }

                string redirectURL = Sitecore.Context.Database.GetItem(Templates.Tender.AdminUserTenderListing).Url();
                redirectURL = string.Format(redirectURL + "{0}", "?id=" + tenderId + "");
                return this.RedirectPermanent(redirectURL);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderDetails Post:" + ex.Message, this);
            }

            return View(tender);

        }

        //Method Get: Admin Tender Create
        //used for Create Tender
        public ActionResult AdminTenderCreate()
        {
            if (Session["TenderUserLogin"] == null || TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin")
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            //TenderCreateModel model = new TenderCreateModel();
            //model.Adv_Date = DateTime.Now.ToString("dd/MM/yyyy");
            //model.Closing_Date = DateTime.Now.ToString("dd/MM/yyyy");

            return View(new TenderCreateModel());
        }
        //Method Post: Admin Tender Create
        //used for Create Admin Tender 
        [HttpPost]
        public ActionResult AdminTenderCreate(TenderCreateModel obj)
        {
            try
            {
                if (Session["TenderUserLogin"] == null || TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin")
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }

                if (ModelState.IsValid)
                {
                    DateTime dateTime;
                    if (!DateTime.TryParseExact(obj.Adv_Date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out dateTime))
                    {
                        ModelState.AddModelError("Adv_Date", DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Date Invalid formate", "Please Input Date in dd-MM-yyyy HH:mm:ss format"));
                        return View(obj);
                    }
                    else if (!DateTime.TryParseExact(obj.Closing_Date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out dateTime))
                    {
                        ModelState.AddModelError("Closing_Date", DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Date Invalid formate", "Please Input Date in dd-MM-yyyy HH:mm:ss format"));
                        return View(obj);
                    }
                    else
                    {
                        //Insert Tender details
                        ElectricityRepository ElecRepo = new ElectricityRepository();
                        var TempId = ElecRepo.InsertTenderList(obj);

                        //Insert Tender Documents
                        foreach (HttpPostedFileBase file in obj.Files)
                        {
                            if (file != null)
                            {
                                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                                var fileExt = Path.GetExtension(file.FileName);
                                string filenamewithtimestamp = fileName.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;
                                string path = Server.MapPath("~/Tender/Uploadedfile/");
                                var filepath = "/Tender/Uploadedfile/" + filenamewithtimestamp;
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                file.SaveAs(Path.Combine(path + filenamewithtimestamp));
                                obj.Id = TempId;
                                obj.FileName = fileName;
                                obj.DocumentPath = filepath;
                                ElecRepo.InsertDocumentList(obj);
                            }
                        }

                        //Create Buyer and send email
                        EnvelopRepository EnvelopeRepo = new EnvelopRepository();
                        EnvelopUserDetails EnvelopUserDetails = new EnvelopUserDetails
                        {
                            Company = "Adani",
                            Email = obj.BuyerEmailId,
                            Name = obj.BuyerName,
                            SelectTenderId = TempId.ToString(),
                            BuyerType = "Buyer",
                            IsBuyer = true,
                            EnvelopNameCheckboxs = new List<EnvelopName> {
                            new EnvelopName { Name="1",Value="1",IsChecked=true},
                            new EnvelopName { Name="2",Value="2",IsChecked=true},
                        }
                        };
                        var registrationObj = EnvelopeRepo.InsertRegistrationEnveleope(EnvelopUserDetails);
                        var userId = registrationObj.UserId;
                        var userPassword = registrationObj.Password;
                        string RedirectUrl = string.Empty;
                        var tenderLoginitem = Sitecore.Context.Database.GetItem(Template.Tender.TenderLogin);
                        if (tenderLoginitem != null)
                        {
                            string baseurl = tenderLoginitem.Url();
                            RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                        }
                        using (TenderDataContext dbcontext = new TenderDataContext())
                        {
                            var tender = dbcontext.TenderLists.Where(x => x.Id == TempId).FirstOrDefault();
                            if (!string.IsNullOrEmpty(RedirectUrl))
                            {
                                //Note : Send Mail to buyer 
                                TenderService Tn = new TenderService();
                                Tn.SendEnvelopUserEmail(obj.BuyerEmailId, RedirectUrl, userId, userPassword, tender.NITNo, tender.Description);
                            }
                        }

                        //Create Lead Buyer and send email
                        EnvelopUserDetails = new EnvelopUserDetails
                        {
                            Company = "Adani",
                            Email = obj.LeadBuyerEmailId,
                            Name = obj.LeadBuyerName,
                            SelectTenderId = TempId.ToString(),
                            BuyerType = "LeadBuyer",
                            IsBuyer = true,
                            EnvelopNameCheckboxs = new List<EnvelopName> {
                            new EnvelopName { Name="3",Value="3",IsChecked=true}
                        }
                        };
                        registrationObj = EnvelopeRepo.InsertRegistrationEnveleope(EnvelopUserDetails);
                        userId = registrationObj.UserId;
                        userPassword = registrationObj.Password;
                        RedirectUrl = string.Empty;
                        if (tenderLoginitem != null)
                        {
                            string baseurl = tenderLoginitem.Url();
                            RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                        }
                        using (TenderDataContext dbcontext = new TenderDataContext())
                        {
                            var tender = dbcontext.TenderLists.Where(x => x.Id == TempId).FirstOrDefault();
                            if (!string.IsNullOrEmpty(RedirectUrl))
                            {
                                //Note : Send Mail to buyer 
                                TenderService Tn = new TenderService();
                                Tn.SendEnvelopUserEmail(obj.BuyerEmailId, RedirectUrl, userId, userPassword, tender.NITNo, tender.Description);
                            }
                        }

                        //Create User Buyer and send email
                        EnvelopUserDetails = new EnvelopUserDetails
                        {
                            Company = "Adani",
                            Email = obj.UserEmailId,
                            Name = obj.UserName,
                            SelectTenderId = TempId.ToString(),
                            BuyerType = "User",
                            IsBuyer = true,
                            EnvelopNameCheckboxs = new List<EnvelopName> {
                            new EnvelopName { Name="1",Value="1",IsChecked=true},
                            new EnvelopName { Name="2",Value="2",IsChecked=true},
                        }
                        };
                        registrationObj = EnvelopeRepo.InsertRegistrationEnveleope(EnvelopUserDetails);

                        ViewBag.SuccessMsg = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Tender Successful Message", "Tender Uploaded Successfully");
                        ModelState.Clear();
                        return View();
                    }
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                Log.Error("Error at AdminTenderCreate Get:" + ex.Message, this);
                ViewBag.ErrorMsg = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/AdminTender Exception", "Error in uploading Please try again");
                return View(obj);
            }
        }
        //Method Get: Admin Tender Update
        //Used for Update Details of Tender
        public ActionResult AdminTenderUpdate()
        {
            if (Session["TenderUserLogin"] == null || TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin")
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }

            string tenderid = Request.QueryString["id"];
            if (string.IsNullOrEmpty(tenderid))
            {
                var item = Context.Database.GetItem(Templates.Tender.AdminTenderListing);
                return this.Redirect(item.Url());
            }

            ElectricityRepository ElecRepo = new ElectricityRepository();
            TempData["id"] = tenderid;
            var model = ElecRepo.GetEditTenderList().FirstOrDefault(tid => tid.Id == new Guid(tenderid));
            return View(model);

        }
        //Method Post: Admin Tender Update
        //used for Update Tender
        [HttpPost]
        public ActionResult AdminTenderUpdate(TenderEditModel obj, string Inactivate_tender = null, string Activate_tender = null, string OnHold_tender = null, string UnHold_tender = null)
        {
            ElectricityRepository ElecRepo = new ElectricityRepository();
            Guid tenderid = obj.Id;
            var data = ElecRepo.GetEditTenderList().FirstOrDefault(tid => tid.Id == tenderid);
            obj.TenderDocuments = data.TenderDocuments;

            try
            {
                var listingitem = Sitecore.Context.Database.GetItem(Templates.Tender.AdminTenderListing);
                DateTime dateTime;
                if (!string.IsNullOrEmpty(OnHold_tender))
                {
                    Log.Info("Hold tender with id:" + obj.Id + " NITNo:" + obj.NITNo + " Location:" + obj.Location, this);
                    ElecRepo.OnHoldTender(obj.Id);
                    Log.Info("Hold tender successful with id:" + obj.Id + " NITNo:" + obj.NITNo + " Location:" + obj.Location, this);
                    return Redirect(listingitem.Url());
                }
                else if (!string.IsNullOrEmpty(UnHold_tender))
                {
                    Log.Info("Un Hold tender with id:" + obj.Id + " NITNo:" + obj.NITNo + " Location:" + obj.Location, this);
                    ElecRepo.UnHoldTender(obj.Id);
                    Log.Info("Un Hold tender successful with id:" + obj.Id + " NITNo:" + obj.NITNo + " Location:" + obj.Location, this);
                    return Redirect(listingitem.Url());
                }
                if (!string.IsNullOrEmpty(Inactivate_tender))
                {
                    Log.Info("Inactivate_tender tender with id:" + obj.Id + " NITNo:" + obj.NITNo + " Location:" + obj.Location, this);
                    ElecRepo.InactivateTender(obj.Id);
                    Log.Info("Inactivate_tender tender successful with id:" + obj.Id + " NITNo:" + obj.NITNo + " Location:" + obj.Location, this);
                    return Redirect(listingitem.Url());
                }
                else if (!string.IsNullOrEmpty(Activate_tender))
                {
                    Log.Info("Activate_tender tender with id:" + obj.Id + " NITNo:" + obj.NITNo + " Location:" + obj.Location, this);
                    ElecRepo.ActivateTender(obj.Id);
                    Log.Info("Activate_tender tender successful with id:" + obj.Id + " NITNo:" + obj.NITNo + " Location:" + obj.Location, this);
                    return Redirect(listingitem.Url());
                }
                else if (ModelState.IsValid)
                {
                    //string avd_date = obj.Adv_Date.ToString("dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                    //string close_date = obj.Closing_Date.ToString("dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);

                    if (!DateTime.TryParseExact(obj.Adv_Date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        ModelState.AddModelError("Adv_Date", DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Date Invalid formate", "Please Input Date in dd-MM-yyyy HH:mm:ss format"));
                        return View(obj);
                    }
                    else if (!DateTime.TryParseExact(obj.Closing_Date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        ModelState.AddModelError("Closing_Date", DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Date Invalid formate", "Please Input Date in dd-MM-yyyy HH:mm:ss format"));
                        return View(obj);
                    }
                    else
                    {
                        ElecRepo.UpdateTenderList(obj);

                        using (TenderDataContext dbcontext = new TenderDataContext())
                        {
                            if (obj.Status.ToLower() == "close")
                            {
                                //send email to user - buyer type user
                                var userDetails = dbcontext.Registrations.Where(r => r.TenderId == obj.Id && r.IsBuyer == true && r.BuyerType.ToLower() == "user").FirstOrDefault();

                                var userId = userDetails.UserId;
                                var userPassword = userDetails.Password;
                                var RedirectUrl = string.Empty;
                                var tenderLoginitem = Sitecore.Context.Database.GetItem(Template.Tender.TenderLogin);
                                if (tenderLoginitem != null)
                                {
                                    string baseurl = tenderLoginitem.Url();
                                    RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                                }
                                var tender = dbcontext.TenderLists.Where(x => x.Id == obj.Id).FirstOrDefault();
                                if (!string.IsNullOrEmpty(RedirectUrl))
                                {
                                    //Note : Send Mail to buyer 
                                    TenderService Tn = new TenderService();
                                    Tn.SendEnvelopUserEmail(tender.UserEmailId, RedirectUrl, userId, userPassword, tender.NITNo, tender.Description);
                                }
                            }
                        }
                        return Redirect(listingitem.Url());
                    }
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                Log.Error("Error at AdminTenderUpdate Get:" + ex.Message, this);
                ViewBag.EditError = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/TenderUpdate Exception", "Error in uploading Please try again");
                return View(obj);
            }
        }

        //Admin Tender Deletefile from super admin role
        public ActionResult DeleteFile(Guid id, string DocumentPath)
        {
            var updateitem = Sitecore.Context.Database.GetItem(Templates.Tender.AdminTenderUpdate);
            try
            {
                ElectricityRepository ElecRepo = new ElectricityRepository();
                ElecRepo.DeleteTenderDocument(id, DocumentPath);

            }
            catch (Exception ex)
            {
                Log.Error("Error at DeleteFile Get:" + ex.Message, this);
            }
            return Redirect(updateitem.Url() + "?id=" + TempData["id"]);
        }


        public ActionResult AdminTenderListwithdownload()
        {
            List<TenderDetails> ObjTender = new List<TenderDetails>();
            try
            {
                if (Session["TenderUserLogin"] == null)
                {
                    var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                    return this.Redirect(item.Url());
                }
                string UserId = TenderUserSession.TenderUserSessionContext.userId;
                string UserType = TenderUserSession.TenderUserSessionContext.UserType;
                ViewBag.UserType = UserType;
                List<TenderList> tenderdata = new List<TenderList>();
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    TenderList objtender = new TenderList();
                    tenderdata = dbcontext.TenderLists.OrderByDescending(x => x.Closing_Date).ToList();

                    foreach (var data in tenderdata)
                    {
                        TenderDetails ObjTd = new TenderDetails();
                        ObjTd.Id = data.Id;
                        ObjTd.NITPRNo = data.NITNo;
                        ObjTd.Business = data.Business;
                        ObjTd.Description = data.Description;
                        ObjTd.Cost_of_EMD = data.Cost_of_EMD;
                        ObjTd.Estimated_Cost = data.Estimated_Cost;
                        ObjTd.Adv_Date = data.Adv_Date;
                        ObjTd.Bid_Submision_ClosingDate = data.Closing_Date.ToString();
                        ObjTd.CreatedDate = data.Created_Date;
                        ObjTd.ModifiedDate = data.Modified_Date;
                        ObjTd.Status = data.Staus;
                        ObjTd.ClosingDate = data.Closing_Date;
                        ObjTd.Tenderdocs = data.TenderDocuments.Where(x => x.TenderId == data.Id).ToList();
                        ObjTender.Add(ObjTd);
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at AdminTenderListing Get:" + ex.Message, this);
            }
            return View(ObjTender);
        }


        #endregion

        #region Envelop-User
        //Method Get: Admin Envelope Create
        //Used for Get Admin Envelope
        public ActionResult AdminEnvelopeCreate()
        {
            if (Session["TenderUserLogin"] == null || TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin")
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            EnvelopRepository EnvelopeRepo = new EnvelopRepository();
            ModelState.Clear();
            EnvelopUserDetails model = new EnvelopUserDetails();
            model.TenderList = EnvelopeRepo.GetOpenTenderListfordropdown();

            var envelopeType = Context.Database.GetItem(Templates.Datasource.EnvelopeType);
            var objenvlopeList = envelopeType.GetChildren().ToList().Select(x => new EnvelopName()
            {
                Name = x.Fields["Text"].Value,
                Value = x.Fields["Value"].Value,
                IsChecked = false
            }).ToList();
            model.EnvelopNameCheckboxs = objenvlopeList;

            return View(model);
        }

        //Method Post: Admin Envelope Create
        //Used for Create admin Envelope
        [HttpPost]
        public ActionResult AdminEnvelopeCreate(EnvelopUserDetails obj)
        {
            if (Session["TenderUserLogin"] == null || TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin")
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }

            EnvelopRepository EnvelopeRepo = new EnvelopRepository();
            EnvelopUserDetails ReturnObj = new EnvelopUserDetails();
            ReturnObj.TenderList = EnvelopeRepo.GetOpenTenderListfordropdown();
            //List<EnvelopName> objenvlopeList = new List<EnvelopName>();
            //objenvlopeList.Add(new EnvelopName() { Name = "Envelope 1", Value = "1", IsChecked = false });
            //objenvlopeList.Add(new EnvelopName() { Name = "Envelope 2", Value = "2", IsChecked = false });
            //objenvlopeList.Add(new EnvelopName() { Name = "Envelope 3", Value = "3", IsChecked = false });

            var envelopeType = Context.Database.GetItem(Templates.Datasource.EnvelopeType);
            var objenvlopeList = envelopeType.GetChildren().ToList().Select(x => new EnvelopName()
            {
                Name = x.Fields["Text"].Value,
                Value = x.Fields["Value"].Value,
                IsChecked = false
            }).ToList();

            ReturnObj.EnvelopNameCheckboxs = objenvlopeList;
            try
            {
                if (ModelState.IsValid)
                {
                    var SelectedEnvelope = obj.EnvelopNameCheckboxs.Where(x => x.IsChecked == true).ToList();
                    if (obj.SelectTenderId == null)
                    {
                        ViewBag.SelectTenderId = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Selection Validation msg", "Please Select Tender From List");
                        return View(ReturnObj);
                    }
                    if (SelectedEnvelope.Count <= 0)
                    {
                        ViewBag.SelectedEnvelope = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Envelop selection msg", "Please Select Minimum 1 Envelope");
                        return View(ReturnObj);
                    }
                    var registrationObj = EnvelopeRepo.InsertRegistrationEnveleope(obj);
                    var userId = registrationObj.UserId;
                    var userPassword = registrationObj.Password;


                    string RedirectUrl = string.Empty;
                    var tenderLoginitem = Sitecore.Context.Database.GetItem(Template.Tender.TenderLogin);
                    if (tenderLoginitem != null)
                    {
                        string baseurl = tenderLoginitem.Url();
                        RedirectUrl = HttpContext.Request.Url.Scheme + "://" + HttpContext.Request.Url.Host + baseurl;
                    }

                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        var tender = dbcontext.TenderLists.Where(x => x.Id == new Guid(obj.SelectTenderId)).FirstOrDefault();
                        if (!string.IsNullOrEmpty(RedirectUrl))
                        {
                            //Note : Send Mail to User 
                            TenderService Tn = new TenderService();
                            Tn.SendEnvelopUserEmail(obj.Email, RedirectUrl, userId, userPassword, tender.NITNo, tender.Description);
                        }
                    }
                    ViewBag.SuccessMsg = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/EnvelopUser Success Msg", "User created successfully and an email has been sent with details.");
                    ModelState.Clear();
                    return View(ReturnObj);
                }
                else
                {
                    return View(ReturnObj);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Error at AdminEnvelopCreate Post:" + ex.Message, this);
                ViewBag.ErrorMsg = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/EnvelopUser Exception Msg", "Error in Insert Please try again");
                return View(ReturnObj);
            }

        }

        //Method: Admin Envelope List
        //Used For Listing Envelope List
        public ActionResult AdminEnvelopeListing()
        {
            if (Session["TenderUserLogin"] == null || TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin")
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            EnvelopRepository EnvelopeRepo = new EnvelopRepository();
            return View(EnvelopeRepo.ListRegisterdEnvelope());

        }
        //Method: Delete Envelope User
        //Used for Disable Envelope User from DB
        public ActionResult DisableEnvelopeUser(string id)
        {
            EnvelopRepository EnvelopeRepo = new EnvelopRepository();
            var envelopelisting = Sitecore.Context.Database.GetItem(Templates.Tender.EnvelopeUserListing);
            try
            {
                EnvelopeRepo.DisableUser(id);
            }
            catch (Exception ex)
            {
                Log.Error("Error at DisableEnvelopeUser Post:" + ex.Message, this);
                ViewBag.DisableUserError = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/DisableEnvelopeUser Exception Msg", "There is Error in Disable Users");
                return View();
            }
            return Redirect(envelopelisting.Url());
        }


        #endregion


        #region Corrigendum Tender
        //Method: Corrigendum List
        //Used for Listing corrigendum
        public ActionResult CorrigendumListing()
        {
            if (Session["TenderUserLogin"] == null || TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin")
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            CorrigendumRepository CorriRepo = new CorrigendumRepository();
            //var model = CorriRepo.GetCorrigendumList().Where(x => x.Status == true).ToList();
            var model = CorriRepo.GetCorrigendumList().ToList();
            return View(model);

        }

        //Method Get: Corrigendum Create
        //Used for Create Corrigendum
        public ActionResult CorrigendumCreate()
        {
            if (Session["TenderUserLogin"] == null || TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin")
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            CorrigendumRepository CorriRepo = new CorrigendumRepository();
            ModelState.Clear();
            CorrigendumModel model = new CorrigendumModel();
            model.CheckBoxes = CorriRepo.GetTenderList().Where(x => x.Status != null && x.Status.ToLower() == "open").ToList();
            return View(model);
        }

        //Method Post: Corrigendum Create
        //Used for Create corrigendum
        [HttpPost]
        public ActionResult CorrigendumCreate(CorrigendumModel obj)
        {
            try
            {
                CorrigendumRepository CorriRepo = new CorrigendumRepository();
                if (ModelState.IsValid)
                {
                    var selectedRecords = obj.CheckBoxes.Where(x => x.IsChecked == true).ToList();
                    if (selectedRecords.Count <= 0)
                    {
                        ViewBag.selectedRecordNull = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Tender Selection Validation", "Please Select Tender for Corrigendum List");
                        return View(obj);
                    }

                    //string Date = System.Convert.ToString(obj.Date);
                    //string date = obj.Date.ToString("dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                    DateTime dateTime;
                    if (!DateTime.TryParseExact(obj.Date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        ModelState.AddModelError("Date", DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Date Invalid formate", "Please Input Date in dd-MM-yyyy HH:mm:ss format"));
                        return View(obj);
                    }
                    else
                    {
                        Guid corri_id = CorriRepo.InsertCorrigendum(obj);
                        foreach (HttpPostedFileBase file in obj.Files)
                        {
                            if (file != null)
                            {
                                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                                var fileExt = Path.GetExtension(file.FileName);
                                string filenamewithtimestamp = fileName.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;
                                string path = Server.MapPath("~/Tender/Uploadedfile/Corrigendum/");
                                var filepath = "/Tender/Uploadedfile/Corrigendum/" + filenamewithtimestamp;
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                file.SaveAs(Path.Combine(path + filenamewithtimestamp));
                                obj.Id = corri_id;
                                obj.FileName = fileName;
                                obj.DocumentPath = filepath;
                                CorriRepo.InsertCorrigendumDocument(obj);
                            }
                        }
                        Log.Info("CorrigendumCreate created succesfully", this);
                        foreach (var ls in selectedRecords)
                        {
                            var TenderId = ls.Id;
                            CorriRepo.InsertCorrigendumTenderMapping(TenderId, corri_id);
                            //send email
                            try
                            {
                                Log.Info("CorrigendumCreate created succesfully: send email to bidder started", this);
                                using (TenderDataContext dbcontext = new TenderDataContext())
                                {
                                    var tender = dbcontext.TenderLists.Where(x => x.Id == TenderId).FirstOrDefault();
                                    var tenderBidders = dbcontext.Registrations.Where(x => x.TenderId == TenderId && x.UserType.ToLower() == "visitor").Select(s => s.Email).Distinct().ToList();

                                    Log.Info("CorrigendumCreate created succesfully: send email total distinct bidders-" + tenderBidders.Count, this);
                                    foreach (var bidder in tenderBidders)
                                    {
                                        TenderService Tn = new TenderService();

                                        var date = DateTime.ParseExact(obj.Date, "dd-MM-yyyy HH:mm:ss", new System.Globalization.CultureInfo("en-US"));
                                        Tn.SendCorrigendumCreateEmail(bidder, tender.NITNo, tender.Description, obj.Title, date.ToString("dd-MM-yyyy"));
                                        Log.Info("CorrigendumCreate created succesfully: mail sent to " + bidder, this);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                Log.Error("Error at CorrigendumCreate Send email:" + e.Message, this);
                            }
                        }
                    }
                    ViewBag.SuccessMsg = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Corrigendum Success Msg", "Corrigendum Uploaded Successfully");
                    ModelState.Clear();
                    CorrigendumModel model = new CorrigendumModel();
                    model.CheckBoxes = CorriRepo.GetTenderList().Where(x => x.Status.ToLower() == "open").ToList();
                    return View(model);
                }
                else
                {
                    return View(obj);
                }

            }
            catch (Exception ex)
            {
                Log.Error("Error at CorrigendumCreate Post:" + ex.Message, this);
                ViewBag.ErrorMsg = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Corrigendum exception msg", "Error in Insert Please try again");
                return View(obj);
            }
        }

        //Method Get:Corrigendum Update
        //Used for Corrigendum update
        public ActionResult CorrigenudumUpdate()
        {
            if (Session["TenderUserLogin"] == null || TenderUserSession.TenderUserSessionContext.UserType != "SuperAdmin")
            {
                var item = Context.Database.GetItem(Templates.Tender.TenderLogin);
                return this.Redirect(item.Url());
            }
            CorrigendumRepository CorriRepo = new CorrigendumRepository();
            TempData["id"] = Request.QueryString["id"];
            string corrigendumId = Request.QueryString["id"];
            if (string.IsNullOrEmpty(corrigendumId))
            {
                var item = Context.Database.GetItem(Templates.Tender.CorrigendumTenderListing);
                return this.Redirect(item.Url());
            }
            var model = CorriRepo.GetEditCorrigendumList(new Guid(corrigendumId));
            return View(model);
        }

        //Method Post: Corrigendum Update
        //Used for update corrigendum Details Update
        [HttpPost]
        public ActionResult CorrigenudumUpdate(CorrigedumEditModel obj, string Inactivate_Corrigendum = null, string Activate_Corrigendum = null)
        {
            CorrigendumRepository CorriRepo = new CorrigendumRepository();
            Guid CorriId = obj.Id;
            var data = CorriRepo.GetEditCorrigendumList(CorriId);
            obj.CorrigendumDocument = data.CorrigendumDocument;
            try
            {
                var listingitem = Sitecore.Context.Database.GetItem(Templates.Tender.CorrigendumTenderListing);
                DateTime dateTime;
                if (!string.IsNullOrEmpty(Inactivate_Corrigendum))
                {
                    Log.Info("InactivateCorrigendum with id:" + obj.Id + " Title:" + obj.Title, this);
                    CorriRepo.InactivateCorrigendum(obj.Id);
                    Log.Info("InactivateCorrigendum successful with id:" + obj.Id + " Title:" + obj.Title, this);
                    return Redirect(listingitem.Url());
                }
                else if (!string.IsNullOrEmpty(Activate_Corrigendum))
                {
                    Log.Info("Activate_Corrigendum with id:" + obj.Id + " Title:" + obj.Title, this);
                    CorriRepo.ActivateCorrigendum(obj.Id);
                    Log.Info("Activate_Corrigendum successful with id:" + obj.Id + " Title:" + obj.Title, this);
                    return Redirect(listingitem.Url());
                }
                else if (ModelState.IsValid)
                {

                    var selectedRecords = obj.TenderList.Where(x => x.IsChecked == true).ToList();
                    //string date = obj.Date.Value.ToString("dd-MM-yyyy hh:mm:ss", CultureInfo.InvariantCulture);
                    if (!DateTime.TryParseExact(obj.Date, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out dateTime))
                    {
                        ModelState.AddModelError("Date", DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Date Invalid formate", "Please Input Date in dd-MM-yyyy HH:mm:ss format"));
                        return View(obj);
                    }
                    else if (selectedRecords.Count <= 0)
                    {
                        ViewBag.selectedRecordNull = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Tender Selection Validation", "Please Select Tender List");
                        return View(obj);
                    }
                    else
                    {

                        //Update Corrigendum 
                        CorriRepo.UpdateCorrrigendum(obj);

                        foreach (HttpPostedFileBase file in obj.Files)
                        {
                            if (file != null)
                            {
                                var fileName = Path.GetFileNameWithoutExtension(file.FileName);
                                var fileExt = Path.GetExtension(file.FileName);
                                string filenamewithtimestamp = fileName.ToString() + "_" + DateTime.Now.ToString("yyyyMMddHHmmss") + fileExt;
                                string path = Server.MapPath("~/Tender/Uploadedfile/Corrigendum/");
                                var filepath = "/Tender/Uploadedfile/Corrigendum/" + filenamewithtimestamp;
                                if (!Directory.Exists(path))
                                {
                                    Directory.CreateDirectory(path);
                                }
                                file.SaveAs(Path.Combine(path + filenamewithtimestamp));
                                obj.Id = obj.Id;
                                obj.FileName = fileName;
                                obj.DocumentPath = filepath;
                                CorriRepo.UpdateCorrigendumDocument(obj);
                            }
                        }
                        //Update CorrigendumTenderMapping
                        CorriRepo.UpdateCorrigendumMapping(obj, selectedRecords);
                        return Redirect(listingitem.Url());
                    }
                }
                return View(obj);
            }
            catch (Exception ex)
            {
                Log.Error("Error at CorrigenudumUpdate Post:" + ex.Message, this);
                ViewBag.EditError = DictionaryPhraseRepository.Current.Get("/Electricity/Tender/Corrigendum exception msg", "Error in Insert Please try again");
                return View(obj);
            }
        }

        //Delete corrigendum uploaded file
        public ActionResult DeleteCorrigendumFile(Guid id, string DocumentPath)
        {
            var updateitem = Sitecore.Context.Database.GetItem(Templates.Tender.CorrigendumTenderUpdate);
            try
            {
                CorrigendumRepository CorriRepo = new CorrigendumRepository();
                CorriRepo.DeleteCorrigendumDocument(id, DocumentPath);

            }
            catch (Exception ex)
            {
                Log.Error("Error at DeleteCorrigendumFile Post:" + ex.Message, this);
                //ViewBag.FileDeleteError = "There is Error in File upload";
                //Guid Edit_Id = (Guid)TempData["id"];
                //return RedirectToAction("EditCorrigendum", "Electricity", new { @id = Edit_Id });
            }
            //  Guid EditId = (Guid)TempData["id"];
            //return RedirectToAction("EditCorrigendum", "Electricity", new { @id = EditId });
            return Redirect(updateitem.Url() + "?id=" + TempData["id"]);
        }
        #endregion


        #region Download Functions
        //Method: To data table
        //Used for Convert List to Datatable
        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        //Method: DatatableToCSV
        //Used for data table to csv and download CSV
        public void DatatableToCSV(DataTable dtDataTable, string TenderNITNo)
        {
            #region Old Method
            //StreamWriter sw = new StreamWriter(strFilePath, false);
            ////headers  
            //for (int i = 0; i < dtDataTable.Columns.Count; i++)
            //{
            //    sw.Write(dtDataTable.Columns[i]);
            //    if (i < dtDataTable.Columns.Count - 1)
            //    {
            //        sw.Write(",");
            //    }
            //}
            //sw.Write(sw.NewLine);
            //foreach (DataRow dr in dtDataTable.Rows)
            //{
            //    for (int i = 0; i < dtDataTable.Columns.Count; i++)
            //    {
            //        if (!System.Convert.IsDBNull(dr[i]))
            //        {
            //            string value = dr[i].ToString();
            //            if (value.Contains(','))
            //            {
            //                value = String.Format("\"{0}\"", value);
            //                sw.Write(value);
            //            }
            //            else
            //            {
            //                sw.Write(dr[i].ToString());
            //            }
            //        }
            //        if (i < dtDataTable.Columns.Count - 1)
            //        {
            //            sw.Write(",");
            //        }
            //    }
            //    sw.Write(sw.NewLine);
            //}
            //sw.Close();
            //DownLoad(strFilePath);
            #endregion

            #region New Method
            string fileName = "Tender_User_" + TenderNITNo;
            Stopwatch stw = new Stopwatch();
            stw.Start();
            StringBuilder sb = new StringBuilder();
            //Column headers  
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sb.Append(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(Environment.NewLine);
            //Column Values
            foreach (DataRow dr in dtDataTable.Rows)
            {
                sb.AppendLine(string.Join(",", dr.ItemArray));
            }

            stw.Stop();

            //Download File
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=" + fileName + ".csv");
            Response.Charset = "";
            Response.ContentType = "application/csv";
            Response.Output.Write(sb);
            Response.Flush();
            Response.End();


            #endregion
        }

        #endregion
        public void SendEmailOnTenderClose(Sitecore.Data.Items.Item[] items, CommandItem commandItem, ScheduleItem scheduleItem)
        {
            try
            {
                Log.Info("Scheduler started", this);
                TenderService Tn = new TenderService();
                Tn.SendEmailOnTenderClose_Scheduler();
                Log.Info("Scheduler end", this);
            }
            catch (Exception e)
            {
                Log.Error("Scheduler Exception:" + e.InnerException.Message, this);
            }
        }

        public void SendReminderForTenderClose(Sitecore.Data.Items.Item[] items, CommandItem commandItem, ScheduleItem scheduleItem)
        {
            try
            {
                Log.Info("Scheduler started", this);
                TenderService Tn = new TenderService();
                Tn.SendReminderForTenderClose_Scheduler();
                Log.Info("Scheduler end", this);
            }
            catch (Exception e)
            {
                Log.Error("Scheduler Exception:" + e.InnerException.Message, this);
            }
        }
    }
}

