using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.AGELPortal.Website.Models;
using Sitecore.AGELPortal.Website.Helpers;
using Sitecore.AGELPortal.Website.Services;
using System.Net.Mail;
using Sitecore.Exceptions;
using Sitecore.Diagnostics;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.OpenIdConnect;
using System.Web.Security;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using Sitecore.Pipelines.GetSignInUrlInfo;
using Sitecore.Web;
using Sitecore.Abstractions;
using System.Threading.Tasks;
using Microsoft.Azure.Management.Media;
using Microsoft.Azure.Management.Media.Models;
using Microsoft.Rest;
using Microsoft.Identity.Client;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using System.Security.Claims;
using Sitecore.Mvc.Extensions;
using System.Globalization;
using System.Net.Http;
using Newtonsoft.Json;
using RestSharp;
using System.Xml.Linq;
using System.ComponentModel;
using ClosedXML.Excel;
using System.Text;
using Sitecore.Sites;
using System.Security.Cryptography;

namespace Sitecore.AGELPortal.Website.Controllers
{

    public class AGELPortalController : Controller
    {

        public AGELPortalController(IFedAuthLoginButtonRepository fedAuthLoginRepository)
        {
            this.FedAuthLoginRepository = fedAuthLoginRepository;

        }

        private IFedAuthLoginButtonRepository FedAuthLoginRepository { get; }
        private AGELPortalRepository realtyRepo = new AGELPortalRepository();
        private AGELPortalDataContext rdb = new AGELPortalDataContext();
        private static readonly Regex emailRegex = new Regex("[a-z0-9][-a-z0-9._]+@([-a-z0-9]+\\.)+[a-z]{2,5}$");
        private static readonly Regex mobileRegex = new Regex("[0-9]{10}$");
        private static readonly Regex UserNameRegex = new Regex("^[a-zA-Z ]+$");
        AzureBlobStorageServices objazureBlobStorageServices = new AzureBlobStorageServices();

        [AGELRedirectUnauthenticated]
        public ActionResult Home()
        {
            var documentCategoryList = (from content in rdb.AGELPortalContents.Where(x => x.contetn_type == "document" && x.status == true).OrderByDescending(y => y.created_date)
                                        join category in rdb.AGElPortalCategories.Where(x => x.status == true) on content.category_id equals category.Id
                                        select category).Distinct().Take(5).ToList();


            var documents = new List<AGELPortalContent>();
            foreach (var item in documentCategoryList)
            {
                var listdocuments = rdb.AGELPortalContents.Where(x => x.contetn_type == "document" && x.status == true && x.category_id == item.Id).OrderByDescending(y => y.created_date).Take(8).ToList();
                documents.AddRange(listdocuments);
            }



            var videoCategoryList = (from content in rdb.AGELPortalContents.Where(x => x.contetn_type == "video" && x.status == true).OrderByDescending(y => y.created_date)
                                     join category in rdb.AGElPortalCategories.Where(x => x.status == true) on content.category_id equals category.Id
                                     select category).Distinct().Take(5).ToList();
            

            var videos = new List<AGELPortalContent>();
            foreach (var item in videoCategoryList)
            {
                var listvideo = rdb.AGELPortalContents.Where(x => x.contetn_type == "video" && x.status == true && x.category_id == item.Id).OrderByDescending(y => y.created_date).Take(8).ToList();
                videos.AddRange(listvideo);
            }



            //var documents = rdb.AGELPortalContents.Where(x => x.contetn_type == "document" && x.status == true && x.document_data != null).OrderByDescending(y => y.created_date).Take(8).ToList();
            // var videos = rdb.AGELPortalContents.Where(x => x.contetn_type == "video" && x.status == true).OrderByDescending(y => y.created_date).Take(8).ToList();
            // var categories = rdb.AGElPortalCategories.Where(x => x.status == true).ToList();

            PortalAddContentModel ContentModel = new PortalAddContentModel();
            if (documents != null)
                ContentModel.documents = documents;
            if (videos != null)
                ContentModel.videos = videos;
                



            //if (categories != null)
            // ContentModel.contentCategories = categories;



            //var videoCategoryList = (from content in rdb.AGELPortalContents.Where(x => x.contetn_type == "video")
            // join category in rdb.AGElPortalCategories.Where(x => x.status == true) on content.category_id equals category.Id
            // select category).Distinct().Take(5).
            // ToList();


            //var documentCategoryList = (from content in rdb.AGELPortalContents.Where(x => x.contetn_type == "document")
            // join category in rdb.AGElPortalCategories.Where(x => x.status == true) on content.category_id equals category.Id
            // select category).Distinct().Take(5).ToList();



            ContentModel.videoCategories = videoCategoryList;
            ContentModel.documentCategories = documentCategoryList;


            return View(ContentModel);



        }


        /// <summary>
        /// Send an OpenID Connect sign-out request.
        /// </summary>
        public void SignOut()
        {
            var context = SiteContext.GetSite("GreenEnergy");
            if (context != null && context.CacheHtml)
            {
                context.Caches?.HtmlCache?.Clear();
            }
            Session.Clear();
            Session.RemoveAll();
            Session.Abandon();
            HttpContext.GetOwinContext().Authentication.SignOut(
                    OpenIdConnectAuthenticationDefaults.AuthenticationType,
                    CookieAuthenticationDefaults.AuthenticationType);

        }

        public static string GetMimeTypeByWindowsRegistry(string fileNameOrExtension)
        {
            string mimeType = "";
            string ext = (fileNameOrExtension.Contains(".")) ? System.IO.Path.GetExtension(fileNameOrExtension).ToLower() : "." + fileNameOrExtension;
            Microsoft.Win32.RegistryKey regKey = Microsoft.Win32.Registry.ClassesRoot.OpenSubKey(ext);
            if (regKey != null && regKey.GetValue("Content Type") != null) mimeType = regKey.GetValue("Content Type").ToString();
            return mimeType;
        }

        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult SendOtp(PortalRegistrationModel model)
        {
            //EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0" };

            AGElPortalOtpHistory r = new AGElPortalOtpHistory();
            try
            {




                var otpFor = model.email != null ? model.email : model.mobile;

                #region Delete Available otp from database for given mobile number

                realtyRepo.DeleteOldOtp(otpFor);
                #endregion
                #region Generate New Otp for given mobile number and save to database
                string generatedotp = realtyRepo.StoreGeneratedOtp(model);
                #endregion
                #region Api call to send SMS of OTP
                try
                {



                    /*var apiurl = string.Format(" https://bulksms.analyticsmantra.com/sendsms/sendsms.php?username=ADANITRAN&password=tech321&type=TEXT&sender=ADRLTY&mobile={0}&message=Hi%20%20{1}%20is%20the%20unique%20code%20to%20acknowledge%20your%20interest%20in%20Adani%20Realty%20projects%20and%20allowing%20our%20team%20to%20connect%20with%20you%20to%20provide%20details%20about%20our%20current%20%26%20upcoming%20Projects.&PEID=1601100000000013196&HeaderId=1605001594131761200&templateId=1607100000000117984", model.mobile, generatedotp);

                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(apiurl).Result;*/
                    //if (response.IsSuccessStatusCode)

                    //SendOtpmobile(model.mobile, model.name, generatedotp);


                    //r.otp_for = model.mobile;
                    //r.otp_for = model.email;
                    //r.otp = generatedotp;
                    //r.status = true;
                    //r.count = 0;
                    //r.date = DateTime.Now;

                    //rdb.AGElPortalOtpHistories.InsertOnSubmit(r);
                    //rdb.SubmitChanges();
                    result = new { status = "1" };
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

        public ActionResult AGELPortalRegistration()
        {
            return base.View(new PortalRegistrationModel());
        }
        //[HttpPost]
        //public ActionResult AGELPortalRegistration(PortalRegistrationModel pm)
        //{
        //    string msg = "";
        //    string url = "";
        //    if (ModelState.IsValid)
        //    {
        //        try
        //        {

        //            string generatedOTP = realtyRepo.GetOTP(pm.email);
        //            if (!string.Equals(generatedOTP, pm.OTP))
        //            {

        //                this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Please Enter correct OTP"));
        //                return this.View(pm);


        //            }
        //            else
        //            {


        //                AGELPortalRegistration pgr = new AGELPortalRegistration();

        //                if (rdb.AGELPortalRegistrations.Any(x => x.mobile == pm.mobile || x.email == pm.email))
        //                {

        //                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "User Alrady Registerd"));
        //                    return this.View(pm);
        //                }
        //                else
        //                {
        //                    //var Mobile = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Mobile && x.OTP == pm.Mobile_Verified && x.OTPType == "registration").FirstOrDefault();
        //                    //var Email = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Email && x.OTP == pm.Email_Verified && x.OTPType == "registration").FirstOrDefault();
        //                    //if (Mobile.Status == true && Email.Status == true)
        //                    //{
        //                    PasswordGenerator generator = new PasswordGenerator();
        //                    pm.Id = Guid.NewGuid();
        //                    pgr.Id = pm.Id;
        //                    pgr.name = pm.name;
        //                    pgr.email = pm.email;
        //                    pgr.mobile = pm.mobile;

        //                    pgr.user_type = "enduser";
        //                    pgr.status = true;


        //                    pgr.created_date = System.DateTime.Now; ;
        //                    pgr.modified = System.DateTime.Now; ;
        //                    pgr.password = generator.RandomPassword(12);
        //                    #region Insert to DB
        //                    rdb.AGELPortalRegistrations.InsertOnSubmit(pgr);
        //                    rdb.SubmitChanges();



        //                    //SendVerificationEmail(pm.email, pgr.name, new Guid(), PortsGMSTemplates.GMSFlags.Registration);

        //                    msg = "User Registration Successfully Done, Please Login.";

        //                    Session["SuccessMsg"] = msg;

        //                    url = "/login";

        //                    // UserWelcomeEmail(pm.email, pm.name, pgr.password);
        //                    // }
        //                    /* else
        //                     {
        //                         if (Mobile.Status == false)
        //                         {
        //                             msg = "Please Verify Your Mobile Number";
        //                             Session["ErrorMsg"] = msg;
        //                             url = "/GrievanceRegistration";

        //                         }
        //                         if (Email.Status == false)
        //                         {
        //                             msg = "Please Verify Your Email";
        //                             Session["ErrorMsg"] = msg;
        //                             url = "/GrievanceRegistration";
        //                         }
        //                     }*/
        //                }

        //                #endregion
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", ex.ToString()));

        //            return this.View(pm);
        //        }
        //        return Redirect(url);
        //    }
        //    else
        //    {
        //        return this.View(pm);
        //    }
        //}
        [AGELRedirectUnauthenticated]

        public static DataTable ConvertToDataTable(List<AGELPortalRegistration> data)
        {
            List<AGELPortalRegistration> list = data.ToList();



            PropertyDescriptorCollection props = null;
            DataTable table = new DataTable();
            if (list != null && list.Count > 0)
            {
                props = TypeDescriptor.GetProperties(list[0]);
                for (int i = 0; i < props.Count; i++)
                {
                    PropertyDescriptor prop = props[i];
                    table.Columns.Add(prop.Name, Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType);
                }
            }
            if (props != null)
            {
                object[] values = new object[props.Count];
                foreach (var item in data)
                {
                    for (int i = 0; i < values.Length; i++)
                    {
                        values[i] = props[i].GetValue(item) ?? DBNull.Value;
                    }
                    table.Rows.Add(values);
                }
            }
            return table;
        }
        [HttpPost]
        public FileResult DownloadExcel(string searchKeyword)
        {
            try
            {
                //searchKeyword = model.
                MemoryStream myStream = null;
                Uri myUri = new Uri(Request.Url.AbsoluteUri);
                var page = System.Convert.ToInt32(HttpUtility.ParseQueryString(myUri.Query).Get("page"));
                var userType = HttpUtility.ParseQueryString(myUri.Query).Get("user_type");
                var siteType = HttpUtility.ParseQueryString(myUri.Query).Get("site");


                PortalAddUserModel User = new PortalAddUserModel();
                if (userType != null && userType != "" && siteType != null && siteType != "" && searchKeyword != null && searchKeyword != "")
                {

                    User.Users = rdb.AGELPortalRegistrations.Where(x => x.user_type == userType && x.user_type != "superadmin" && x.site == siteType && (x.name.Contains(searchKeyword) || x.email.Contains(searchKeyword) || x.mobile.Contains(searchKeyword))).OrderByDescending(x => x.created_date).ToList();

                }
                else if (userType != null && userType != "" && siteType != null && siteType != "")
                {

                    User.Users = rdb.AGELPortalRegistrations.Where(x => x.user_type == userType && x.user_type != "superadmin" && x.site == siteType).OrderByDescending(x => x.created_date).ToList();

                }
                else if (userType != null && userType != "all" && userType != "" && searchKeyword != null && searchKeyword != "")
                {
                    User.Users = rdb.AGELPortalRegistrations.Where(x => x.user_type == userType && x.user_type != "superadmin" && (x.name.Contains(searchKeyword) || x.email.Contains(searchKeyword) || x.mobile.Contains(searchKeyword))).OrderByDescending(x => x.created_date).ToList();

                }
                else if (siteType != null && siteType != "all" && siteType != "" && searchKeyword != null && searchKeyword != "")
                {

                    User.Users = rdb.AGELPortalRegistrations.Where(x => x.site == siteType && x.user_type != "superadmin" && (x.name.Contains(searchKeyword) || x.email.Contains(searchKeyword) || x.mobile.Contains(searchKeyword))).OrderByDescending(x => x.created_date).ToList();

                }
                else if (userType != null && userType != "all" && userType != "")
                {

                    User.Users = rdb.AGELPortalRegistrations.Where(x => x.user_type == userType && x.user_type != "superadmin").OrderByDescending(x => x.created_date).ToList();

                }
                else if (siteType != null && siteType != "all" && siteType != "")
                {
                    User.Users = rdb.AGELPortalRegistrations.Where(x => x.site == siteType && x.user_type != "superadmin").OrderByDescending(x => x.created_date).ToList();


                }
                else if (searchKeyword != null && searchKeyword != "")
                {
                    User.Users = rdb.AGELPortalRegistrations.Where(x => x.name.Contains(searchKeyword) && x.user_type != "superadmin" || x.email.Contains(searchKeyword) || x.mobile.Contains(searchKeyword)).OrderByDescending(x => x.created_date).ToList();
                }
                else
                {
                    User.Users = rdb.AGELPortalRegistrations.Where(x => x.user_type != "superadmin").OrderByDescending(x => x.created_date).ToList();
                }
                DataTable dt = new DataTable();
                dt = ConvertToDataTable(User.Users);
                dt.Columns.Remove("Id");
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dt, "New_NameTrasfer");



                    Response.Clear();
                    Response.Buffer = true;
                    Response.Charset = "";
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                    Response.AddHeader("content-disposition", "attachment;filename=NameTransferApplicationList.xlsx");

                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        wb.SaveAs(MyMemoryStream);
                        MyMemoryStream.WriteTo(Response.OutputStream);
                        Response.Flush();
                        Response.End();
                        myStream = MyMemoryStream;
                    }

                }
                byte[] bytes = myStream.ToArray();
                return File(bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

            }
            catch (Exception ex)
            {
                Log.Error("Excel File Download Error " + ex.Message, this);
                return null;

            }

            // return null;

            //return File(MyMemoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "attachment;filename=NameTransferApplicationList.xsls");
        }

        public ActionResult AGELPortalOtpVerify()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AGELPortalOtpVerify(PortalRegistrationModel pm)
        {
            pm.OTP = "123";
            //string User1 = "";
            string url = "";
            //AGELPortalRegistration rdb = new AGELPortalRegistration();
            if (ModelState.IsValid)
            {
                //added by neeraj yadav IsUserValid() code
                bool allowRegistration = IsUserValid(pm.mobile);
                if (!allowRegistration)
                {
                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "You have entered wrong OTP 3 times. Please try again after 1 hr."));
                    TempData["RegistrationOTPFailed"] = "You have entered wrong OTP 3 times.Please try again after 1 hr.";
                    if (Session["AGELPortalUser"] != null)
                    {
                        Session["AGELPortalUser"] = null;
                    }
                    if (Session["AGELPortalUserViewMode"] != null)
                    {
                        Session["AGELPortalUserViewMode"] = null;
                    }
                    if (Session["AGELPortalUserName"] != null)
                    {
                        Session["AGELPortalUserName"] = null;
                    }
                    SignOut();
                    return this.CustomRedirect("/AGELPortal/Home/login");
                }
                else
                {
                    var User = rdb.AGELPortalRegistrations.Where(x => x.email.Contains(pm.email) || x.mobile.Contains(pm.mobile)).ToList().FirstOrDefault();
                    var User1 = rdb.AGELPortalRegistrations.Where(x => x.email.Contains(pm.email) && x.mobile.Contains(pm.mobile) && x.password == null).ToList().FirstOrDefault();
                    if (User == null && User1 == null)
                    {
                        //this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Enter User from User managment"));
                        //return this.View(pm);
                        url = "/agelportal/home/Registration_Step?token1=" + pm.name + "&token2=" + pm.email + "&token3=" + pm.mobile;
                    }
                    else if (User != null && !string.IsNullOrEmpty(User.password))
                    {
                        TempData["RegistrationOTPFailed"] = "You already registered, please sign in.";
                        return this.CustomRedirect("/AGELPortal/Home/login");

                    }
                    else if (User != null && User1 != null)
                    {
                        if (User1.email.ToLower() == pm.email.ToLower() && User1.mobile == pm.mobile && User1.name.ToLower() != pm.name.ToLower())
                        {
                            //this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Global/Dictionary/Form/User Alerady Register", "Username, Mobile Number or Email ID  is wrong."));
                            //return this.View(pm);
                            url = "/agelportal/home/Registration_Step?token1=" + pm.name + "&token2=" + pm.email + "&token3=" + pm.mobile;
                        }
                        else
                        {
                            bool timediffer = CanSendOTP(pm.mobile);
                            if (!timediffer)
                            {
                                this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/Form/InvalidUser", "Please try again after 1 hour."));
                                return this.View(pm);
                            }
                            else
                            {
                                if (SendOtpForValidation(pm))
                                {
                                    url = "/agelportal/home/Registration_Step?token1=" + pm.name + "&token2=" + pm.email + "&token3=" + pm.mobile;
                                }
                                else
                                {
                                    //this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/Form/InvalidUser", "OTP cannot be sent. Please check details."));
                                    //return this.View(pm);
                                    Session["RegOtpError"] = "OTP cannot be sent. Please check details.";
                                    url = "/agelportal/home/Registration_Step?token1=" + pm.name + "&token2=" + pm.email + "&token3=" + pm.mobile;
                                }
                            }
                        }

                    }
                    else if (User == null && User1 != null)
                    {
                        if (User1.email.ToLower() == pm.email.ToLower() && User1.mobile == pm.mobile && User1.name.ToLower() != pm.name.ToLower())
                        {
                            //this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Global/Dictionary/Form/User Alerady Register", "Username, Mobile Number or Email ID  is wrong."));
                            //return this.View(pm);
                            url = "/agelportal/home/Registration_Step?token1=" + pm.name + "&token2=" + pm.email + "&token3=" + pm.mobile;
                        }
                        else
                        {
                            bool timediffer = CanSendOTP(pm.mobile);
                            if (!timediffer)
                            {
                                this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/Form/InvalidUser", "Please try again after 1 hour."));
                                return this.View(pm);
                            }
                            else
                            {
                                if (SendOtpForValidation(pm))
                                {
                                    url = "/agelportal/home/Registration_Step?token1=" + pm.name + "&token2=" + pm.email + "&token3=" + pm.mobile;
                                }
                                else
                                {
                                    //this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/Form/InvalidUser", "OTP cannot be sent. Please check details."));
                                    //return this.View(pm);
                                    url = "/agelportal/home/Registration_Step?token1=" + pm.name + "&token2=" + pm.email + "&token3=" + pm.mobile;
                                }
                            }
                        }

                    }
                   
                    else
                    {
                        if (User.email.ToLower() != pm.email.ToLower() && User.mobile == pm.mobile)
                        {
                            //this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Global/Dictionary/Form/User Alerady Register", "Username, Mobile Number or Email ID  is wrong."));
                            url = "/agelportal/home/Registration_Step?token1=" + pm.name + "&token2=" + pm.email + "&token3=" + pm.mobile;

                        }
                        if (User.email.ToLower() == pm.email.ToLower() && User.mobile != pm.mobile)
                        {
                            //this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Global/Dictionary/Form/User Alerady Register", "Username, Mobile Number or Email ID  is wrong."));
                            url = "/agelportal/home/Registration_Step?token1=" + pm.name + "&token2=" + pm.email + "&token3=" + pm.mobile;
                        }
                        if (User.email.ToLower() == pm.email.ToLower() && User.mobile == pm.mobile)
                        {
                            //this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Global/Dictionary/Form/User Alerady Register", "User is already registerd please go to forgot password."));
                            url = "/agelportal/home/Registration_Step?token1=" + pm.name + "&token2=" + pm.email + "&token3=" + pm.mobile;

                        }

                        //return this.View(pm);
                        //this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Global/Dictionary/Form/User Alerady Register", "User is already registerd please go to forgot password."));
                        //return this.View(pm);
                    }
                }
            }
            else
            {
                return this.View(pm);
            }
            return CustomRedirect(url);
        }

        [HttpPost]
        public ActionResult AGELPortalRegistration(PortalRegistrationModel pm)
        {
            string msg = "";
            string url = "";
            if (ModelState.IsValid)
            {
                try
                {
                    //digit and 10 number only
                    Regex pattern = new Regex(@"(?<!\d)\d{10}(?!\d)");

                    //digit and 10 number only
                    //Regex pattern = new Regex(@"(?<!\d)\d{10}(?!\d)");

                    //if (!pattern.Match(pm.mobile.Trim()).Success)
                    //{
                    //    Session["InvalidOTP"] = "Invalid Request.";
                    //    this.CustomRedirect("/AGELPortal/Home/login");
                    //    return this.CustomRedirect(url);
                    //}
                    //if (!emailRegex.Match(pm.email.Trim()).Success)
                    //{
                    //    Session["InvalidOTP"] = "Invalid Request.";
                    //    this.CustomRedirect("/AGELPortal/Home/login");
                    //    return this.CustomRedirect(url);
                    //}
                    

                    string generatedOTP = realtyRepo.GetOTP(pm.mobile);
                    //bool timediffer = CanSendOTP(pm.mobile);
                    bool isreachmaxinvalidattempt = CheckIfUserEnterInvalidOTPTime(pm.mobile);
                    if (!isreachmaxinvalidattempt)
                    {
                        //var Userrecord = rdb.AGElPortalOtpHistories.Where(x => x.otp_for == pm.mobile && x.status == true && x.WrongOTPAttempts == 2).FirstOrDefault();
                        //if (Userrecord != null)
                        //{
                        //    var time1 = Userrecord.date.Value.AddMinutes(60);
                        //    var time2 = DateTime.Now;
                        //    var timeLeft = time2 - time1;
                        //    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "You have entered wrong OTP 3 times.Please try again after 1 hr."));
                        //    TempData["RegistrationOTPFailed"] = "You have entered wrong OTP 3 times. Please try again after " + timeLeft.Minutes + "min" + timeLeft.Seconds + "sec";
                        //}

                        this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "You have entered wrong OTP 3 times. Please try again after 1 hr."));
                        TempData["RegistrationOTPFailed"] = "You have entered wrong OTP 3 times.Please try again after 1 hr.";
                        if (Session["AGELPortalUser"] != null)
                        {
                            Session["AGELPortalUser"] = null;
                        }
                        if (Session["AGELPortalUserName"] != null)
                        {
                            Session["AGELPortalUserName"] = null;
                        }
                        if (Session["AGELPortalUserViewMode"] != null)
                        {
                            Session["AGELPortalUserViewMode"] = null;
                        }
                        SignOut();
                        return this.CustomRedirect("/AGELPortal/Home/login");

                        // return this.View(pm);
                    }
                    //else if (!timediffer)
                    //{
                    //    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "You have entered wrong OTP 3 times. Please try again after 1 hr."));
                    //    return this.View(pm);
                    //}
                    else if (generatedOTP == "optexpired")
                    {
                        this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Please OTP is expired, please generate again."));
                        return this.View(pm);
                    }

                    else if (!string.Equals(generatedOTP, pm.OTP))
                    {
                        AddInvalidAttemptOTPHistory(pm.mobile);
                        this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Please enter the correct OTP."));
                        return this.View(pm);
                    }
                    else
                    {


                        AGELPortalRegistration pgr = new AGELPortalRegistration();
                        var user = rdb.AGELPortalRegistrations.Where(x => x.email == pm.email && x.mobile == pm.mobile).FirstOrDefault();


                        if (user == null)
                        {

                            this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Invalid user."));
                            return this.View(pm);
                        }
                        else
                        {

                            if (user.password != null)
                            {

                                this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "User is already registerd."));
                                return this.View(pm);
                            }
                            //else
                            //{
                            //    CreatePassword();
                            //}



                            //var Mobile = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Mobile && x.OTP == pm.Mobile_Verified && x.OTPType == "registration").FirstOrDefault();
                            //var Email = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Email && x.OTP == pm.Email_Verified && x.OTPType == "registration").FirstOrDefault();
                            //if (Mobile.Status == true && Email.Status == true)
                            //{
                            PasswordGenerator generator = new PasswordGenerator();

                            //user.allow_login = true;



                            //user.modified = System.DateTime.Now; ;
                            //user.password = generator.RandomPassword(12);
                            #region Insert to DB
                            //rdb.AGELPortalRegistrations.InsertOnSubmit(pgr);

                            rdb.SubmitChanges();
                            realtyRepo.DeleteOldOtp(user.mobile);

                            msg = "User registered successfully. Please log in.";

                            Session["SuccessMsg"] = msg;

                            url = "/agelportal/home/Create_Password?id=" + user.Id;


                            // UserWelcomeEmail(pm.email, pm.name, pgr.password);

                        }

                        #endregion
                    }


                }
                catch (Exception ex)
                {

                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", ex.ToString()));

                    return this.View(pm);
                }
                return CustomRedirect(url);
            }
            else
            {
                return this.View(pm);
            }
        }

        public bool IsUserValid(string mobile)
        {
            var objOTPHistorylist = rdb.AGElPortalOtpHistories.Where(x => x.otp_for == mobile && x.date.Value > DateTime.Now.AddMinutes(-60) && x.status == true && x.WrongOTPAttempts == 3).OrderByDescending(c => c.date.Value).ToList();

            if (objOTPHistorylist.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }



        public ActionResult CreatePassword(string id)
        {


            var user = rdb.AGELPortalRegistrations.Where(x => x.Id.ToString() == id).FirstOrDefault();

            PortalRegistrationModel pm = new PortalRegistrationModel();
            //pm.Id = user.Id;
            return View(pm);
        }

        [HttpPost]
        public ActionResult CreatePassword(PortalRegistrationModel pm, string id)
        {
            //string msg = "";
            string url = "";

            if (pm.password == pm.confirm_password)
            {

                var user = rdb.AGELPortalRegistrations.Where(x => x.Id.ToString() == id).FirstOrDefault();
                if (user != null)
                {
                    //AGELPortalRegistration rdb = new AGELPortalRegistration();
                    user.password = pm.password;
                    user.status = true;
                    user.modified = System.DateTime.Now;

                    //msg = " Password Created successfully";


                    Session["Accountregister"] = "Account registered successfully.";
                    rdb.SubmitChanges();
                    url = "/agelportal/home/login";

                    //send email for updated password                   
                    SendEmail_UserUpdatedPassword(user.email, user.name, user.password);
                    string sms_content = "Dear " + user.name +",";
                    sms_content = " Your account with AGEL - Safety Portal is active Now. ";
                //    sms_content = " New Password: " + user.password;
                    //if (SendSMSUpdates(user.mobile, sms_content))
                    //{
                    //    Log.Info("sms sent", "");
                    //}
                    //else
                    //{
                    //    Log.Info("sms failed", "");
                    //}
                }
                else
                {
                    url = "/agelportal/home/Registration";
                }
            }
            else
            {
                url = "/agelportal/home/Create_Password?id=" + id;
            }
            return CustomRedirect(url);
        }
        public ActionResult AGELPortalLogin()
        {
            Log.Info("AGELPortalLogin Start" + DateTime.Now, this);
            Session.Abandon();
            //clearing cache
            var context = SiteContext.GetSite("GreenEnergy");
            if (context != null && context.CacheHtml)
            {
                context.Caches?.HtmlCache?.Clear();
            }
            //string Url = "";
            if (TempData["ForceLoginMessage"] != null)
            {
                ViewBag.successmessage = System.Convert.ToString(TempData["ForceLoginMessage"]);
            }
            if (TempData["RegistrationOTPFailed"] != null)
            {
                ViewBag.successmessage = System.Convert.ToString(TempData["RegistrationOTPFailed"]);
            }
            var cookie = new System.Web.HttpCookie("ASP.NET_SessionId", "");
            cookie.Secure = true; //Add this flag
                                  // cookie.HttpOnly = true;
            Response.Cookies.Add(cookie);
            // Response.Cookies.Add(new System.Web.HttpCookie("ASP.NET_SessionId", ""));
            if (ClaimsPrincipal.Current.Claims.ToList().Count() > 10)
            {
                var checkClaims = ClaimsPrincipal.Current.Claims.ToList();
                //   Log.Info("Adani AD checkClaims first" + JsonConvert.SerializeObject(checkClaims),this);
                //foreach (var item in checkClaims)
                //{
                //    Log.Info("checkClaims item" + item.Type, this);
                //    Log.Info("checkClaims value" + item.Value, this);

                //}
                Log.Info("Adani AD checkClaims" + checkClaims, this);
                string useremail = checkClaims != null && checkClaims.Count > 0 ? checkClaims.Where(x => x.Type.Contains("upn")).FirstOrDefault().Value : "";
                Log.Info("Adani AD useremail" + useremail, this);
                // if (useremail == "" && useremail == null)
                if (string.IsNullOrEmpty(useremail))
                {
                    //this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/Your Email is not register";
                    ViewBag.message = "This email ID is not registered.";
                    return View();
                }
                else
                {
                    var userRegistrationCheck = rdb.AGELPortalRegistrations.Where(x => x.email == useremail && x.status == true).FirstOrDefault();
                    //     Log.Info("Adani AD userRegistrationCheck" + userRegistrationCheck.email,this);
                    if (userRegistrationCheck != null)
                    {
                        Log.Info("Adani AD userRegistrationCheckif" + userRegistrationCheck.email, this);

                        //Guid sessionId = Guid.NewGuid();
                        //AGELPortalUserSession agelus = new AGELPortalUserSession();

                        //agelus.id = sessionId;
                        //agelus.email = useremail;
                        //agelus.created_date = DateTime.Now;
                        //#region Insert to DB
                        //rdb.AGELPortalUserSessions.InsertOnSubmit(agelus);
                        //rdb.SubmitChanges();

                        //#endregion

                        //Session["AGELPortalUser"] = "demo@adani.com";
                        //Session["AGELPortalUserType"] = userRegistrationCheck.user_type.ToString();
                        //Session["AGELPortalUser"] = userRegistrationCheck.email.ToString();
                        //Session["AGELPortalUserName"] = userRegistrationCheck.email.ToString();
                        //Session["Date"] = System.DateTime.Now.ToString();



                        //Url = "/Dashboard?sessionId=" + agelus.id;
                        Session["AGELPortalUser"] = userRegistrationCheck.Id.ToString();
                        Session["AGELPortalUser1"] = userRegistrationCheck.Id;
                        Session["AGELPortalUserName"] = userRegistrationCheck.name.ToString();
                        Session["AGELPortalUserType"] = userRegistrationCheck.user_type.ToString();
                        Session["Date"] = System.DateTime.Now.ToString();


                        //if (userRegistrationCheck.user_type != "superadmin")
                        //{
                        Session["AGELPortalUserViewMode"] = "employee";
                        return CustomRedirect("/AGELPortal/Home");
                        //}
                        //else
                        //{
                        //    Session["AGELPortalUserViewMode"] = "admin";
                        //    return CustomRedirect("/AGELPortal/Home/dashboard");
                        //}






                        // return View();
                    }
                    else
                    {
                        Log.Info("Adani AD userRegistrationCheckelse", this);
                        ViewBag.message = "Email is not register in user management";
                        return View();
                    }
                }
            }

            else
            {
                if (TempData["ResetPassword"] != null)
                {
                    ViewBag.successmessage = System.Convert.ToString(TempData["ResetPassword"]);
                }
                ViewBag.message = "";
                return View();
            }

        }

        //[HttpPost]
        //[ValidateAntiForgeryToken()]
        //public ActionResult AGELPortalLogin(PortalLoginModel m)
        //{
        //    AGELPortalUserLoginHistory pm = new AGELPortalUserLoginHistory();
        //    string url = "/AGELPortal/Home/login";
        //    try
        //    {

        //        if (ModelState.IsValid)
        //        {
        //            var User = rdb.AGELPortalRegistrations.Where(x => x.email == m.email && x.status == true).FirstOrDefault();
        //            if (User != null)
        //            {
        //                if (User.password != m.password)
        //                {

        //                    this.ModelState.AddModelError(nameof(m.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Username and password are incorrect."));
        //                    return this.View(m);
        //                }
        //                if (User.user_type == "superadmin")
        //                {
        //                    pm.Id = Guid.NewGuid();
        //                    pm.User_Id = User.Id;
        //                    pm.UserName = User.name.ToString();
        //                    pm.Login_Time = System.DateTime.Now;
        //                    var user_ID = rdb.AGELPortalUserLoginHistories.Where(x => x.User_Id == pm.User_Id).FirstOrDefault();
        //                    if (user_ID == null)
        //                    {
        //                        pm.Id = Guid.NewGuid();
        //                        pm.User_Id = User.Id;
        //                        pm.UserName = User.name.ToString();
        //                        pm.Login_Time = System.DateTime.Now;
        //                        rdb.AGELPortalUserLoginHistories.InsertOnSubmit(pm);
        //                        rdb.SubmitChanges();
        //                    }
        //                    else
        //                    {
        //                        user_ID.Login_Time = System.DateTime.Now;
        //                        rdb.SubmitChanges();
        //                    }
        //                    url = "/AGELPortal/Home/Dashboard";

        //                }
        //                else
        //                {
        //                    pm.Id = Guid.NewGuid();
        //                    pm.User_Id = User.Id;
        //                    pm.UserName = User.name.ToString();
        //                    pm.Login_Time = System.DateTime.Now;
        //                    var user_ID = rdb.AGELPortalUserLoginHistories.Where(x => x.User_Id == pm.User_Id).FirstOrDefault();
        //                    if (user_ID == null)
        //                    {
        //                        pm.Id = Guid.NewGuid();
        //                        pm.User_Id = User.Id;
        //                        pm.UserName = User.name.ToString();
        //                        pm.Login_Time = System.DateTime.Now;
        //                        rdb.AGELPortalUserLoginHistories.InsertOnSubmit(pm);
        //                        rdb.SubmitChanges();
        //                    }
        //                    else
        //                    {
        //                        user_ID.Login_Time = System.DateTime.Now;
        //                        rdb.SubmitChanges();
        //                    }
        //                    url = "/AGELPortal/Home";
        //                }
        //                Session["AGELPortalUser"] = User.Id.ToString();
        //                Session["AGELPortalUser1"] = User.Id;
        //                Session["AGELPortalUserName"] = User.name.ToString();
        //                Session["AGELPortalUserType"] = User.user_type.ToString();
        //                Session["Date"] = System.DateTime.Now.ToString();
        //            }
        //            else
        //            {
        //                this.ModelState.AddModelError(nameof(m.UserValidation), DictionaryPhraseRepository.Current.Get("/Form/InvalidUser", "Username and password are incorrect."));
        //                return this.View(m);
        //            }
        //        }
        //        else
        //        {

        //            return this.View(m);
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //        this.ModelState.AddModelError(nameof(m.UserValidation), DictionaryPhraseRepository.Current.Get("/Form/InvalidUser", ex.ToString()));

        //        return this.View(m);
        //    }
        //    return Redirect(url);
        //}

        [HttpPost]
        [ValidateAntiForgeryToken()]
        public ActionResult AGELPortalLogin(PortalLoginModel m)
        {
            AGELPortalUserLoginHistory pm = new AGELPortalUserLoginHistory();
            string url = "/AGELPortal/Home/login";
            try
            {

                if (ModelState.IsValid)

                {

                    var User = rdb.AGELPortalRegistrations.Where(x => x.email == m.email && x.status == true).FirstOrDefault();
                    if (User != null)
                    {
                        //added by neeraj yadav IsUserValid() code
                        bool allowRegistration = IsUserValid(User.mobile);
                        if (!allowRegistration)
                        {

                            this.ModelState.AddModelError(nameof(m.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "You have entered wrong OTP 3 times. Please try again after 1 hr."));
                            TempData["RegistrationOTPFailed"] = "Your account is disabled for 1 hour as you have attempted too many wrong OTP.";
                            ViewBag.successmessage = System.Convert.ToString(TempData["RegistrationOTPFailed"]);
                            if (Session["AGELPortalUser"] != null)
                            {
                                Session["AGELPortalUser"] = null;
                            }
                            if (Session["AGELPortalUserName"] != null)
                            {
                                Session["AGELPortalUserName"] = null;
                            }
                            if (Session["AGELPortalUserViewMode"] != null)
                            {
                                Session["AGELPortalUserViewMode"] = null;
                            }
                            SignOut();
                            return this.CustomRedirect("/AGELPortal/Home/login");
                        }
                        else
                        {

                            var isallowtologin = false;
                            //if (User.user_type != "superadmin")
                            //{
                            isallowtologin = CheckAllowtoLogin(User.email);
                            bool isreachmaxinvalidattempt = CheckIfUserEnterInvalidOTPTime(User.mobile);

                            //added by neeraj yadav, password decryption
                           // var password = AesOperation.DecryptStringAES(objUL.HDpass);
                            var decryptedPassword = DecryptStringFromBytes(m.password);
                            Log.Info("Decrypted Password" + decryptedPassword, this);
                            if (!string.IsNullOrEmpty(decryptedPassword))
                            {
                                Log.Info("password decrypted successfully", this);
                                m.password = decryptedPassword;
                            }
                            else
                            {
                                Log.Info("password decryption failed", this);
                                this.ModelState.AddModelError(nameof(m.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Something went wrong in password encryption"));
                                return this.View(m);
                            }
                            //password decryption code till here


                            if (!isallowtologin || !isreachmaxinvalidattempt)
                            {
                                if (!isreachmaxinvalidattempt)
                                {
                                    var objLoginHistory = rdb.AGElPortalOtpHistories.Where(x => x.otp_for == User.mobile && x.status == true).OrderByDescending(c => c.date).FirstOrDefault();
                                    if (objLoginHistory != null)
                                    {
                                        objLoginHistory.status = false;
                                        rdb.SubmitChanges();
                                    }
                                }

                                this.ModelState.AddModelError(nameof(m.UserValidation), DictionaryPhraseRepository.Current.Get("/Form/InvalidUser", "Your account is suspended , please try after 1 hour."));
                                return this.View(m);
                            }
                            else if (User.password != m.password)
                            {
                                var user_ID = rdb.AGELPortalUserLoginHistories.Where(x => x.User_Id == User.Id).FirstOrDefault();
                                if (user_ID != null)
                                {
                                    pm.Id = Guid.NewGuid();
                                    pm.User_Id = User.Id;
                                    pm.UserName = User.name.ToString();
                                    pm.Login_Time = System.DateTime.Now;
                                    pm.checkLoginAttempts = true;
                                    rdb.AGELPortalUserLoginHistories.InsertOnSubmit(pm);
                                    rdb.SubmitChanges();
                                }

                                var count = rdb.AGELPortalUserLoginHistories.Where(x => x.User_Id == User.Id && x.checkLoginAttempts == true).Count();
                                var maxattempt = 3 - count;

                                if (maxattempt > 0)
                                {
                                    this.ModelState.AddModelError(nameof(m.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Username and password are incorrect. User can do 3 login attempts max. else account will be block for 1 hour."));
                                    return this.View(m);
                                }
                                else
                                {
                                    this.ModelState.AddModelError(nameof(m.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Username and password are incorrect. your account is disabled. please try after 1 hour."));
                                    return this.View(m);
                                }

                            }
                            else
                            {
                                pm.Id = Guid.NewGuid();
                                pm.User_Id = User.Id;
                                pm.UserName = User.name.ToString();
                                pm.Login_Time = System.DateTime.Now;
                                //var user_ID = rdb.AGELPortalUserLoginHistories.Where(x => x.User_Id == pm.User_Id).FirstOrDefault();
                                var objLoginHistorylist = rdb.AGELPortalUserLoginHistories.Where(x => x.User_Id == User.Id && x.checkLoginAttempts == true).ToList();


                                if (User != null)
                                {
                                    pm.Id = Guid.NewGuid();
                                    pm.User_Id = User.Id;
                                    pm.UserName = User.name.ToString();
                                    pm.Login_Time = System.DateTime.Now;
                                    pm.checkLoginAttempts = false;
                                    rdb.AGELPortalUserLoginHistories.InsertOnSubmit(pm);
                                    rdb.SubmitChanges();
                                }


                                if (objLoginHistorylist.Count() > 0)
                                {
                                    foreach (var item in objLoginHistorylist)
                                    {
                                        item.checkLoginAttempts = false;
                                    }
                                    rdb.SubmitChanges();
                                }


                                url = "/AGELPortal/Home";
                            }
                            Session["AGELPortalUser"] = User.Id.ToString();
                            Session["AGELPortalUser1"] = User.Id;
                            Session["AGELPortalUserName"] = User.name.ToString();
                            Session["AGELPortalUserType"] = User.user_type.ToString();
                            Session["Date"] = System.DateTime.Now.ToString();

                            Session["AGELPortalUserViewMode"] = "employee";
                            return CustomRedirect("/AGELPortal/Home");
                            //if (User.user_type != "superadmin")
                            //{
                            //    Session["AGELPortalUserViewMode"] = "employee";
                            //    return CustomRedirect("/AGELPortal/Home");
                            //}
                            //else
                            //{
                            //    Session["AGELPortalUserViewMode"] = "admin";
                            //    return CustomRedirect("/AGELPortal/Home/dashboard");
                            //}

                        }
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(m.UserValidation), DictionaryPhraseRepository.Current.Get("/Form/InvalidUser", "Username and password are incorrect. User can do 3 login attempts max. else account will be block for 1 hour."));
                        return this.View(m);
                    }
                }
                else
                {

                    return this.View(m);
                }

            }
            catch (Exception ex)
            {

                this.ModelState.AddModelError(nameof(m.UserValidation), DictionaryPhraseRepository.Current.Get("/Form/InvalidUser", ex.ToString()));

                return this.View(m);
            }
            return CustomRedirect(url);
        }

        public string DecryptStringFromBytes(string password)
        {
            var keybytes = Encoding.UTF8.GetBytes("8080808080808080");
            var iv = Encoding.UTF8.GetBytes("8080808080808080");
            var encryptedPassword = System.Convert.FromBase64String(password);
            Log.Info("DecryptStringFromBytes encryptedPassword: " + encryptedPassword, this);
            // Declare the string used to hold the decrypted text.  
            string decryptedPlainText = null;

            try
            {
                // Create an RijndaelManaged object with the specified key and IV.  
                using (var rijAlg = new RijndaelManaged())
                {
                    //Settings  
                    rijAlg.Mode = CipherMode.CBC;
                    rijAlg.Padding = PaddingMode.PKCS7;
                    //rijAlg.FeedbackSize = 128;
                    rijAlg.FeedbackSize = 128 / 8;
                    rijAlg.Key = keybytes;
                    rijAlg.IV = iv;

                    Log.Info("DecryptStringFromBytes rijAlg: " + rijAlg, this);

                    // Create a decryptor to perform the stream transform.  
                    var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
                    Log.Info("DecryptStringFromBytes decryptor: " + decryptor, this);
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(encryptedPassword))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream and place them in a string.  
                                decryptedPlainText = srDecrypt.ReadToEnd();
                            }

                        }
                    }

                }
                Log.Info("DecryptStringFromBytes response" + decryptedPlainText, this);
                return decryptedPlainText;

            }
            catch (Exception e)
            {
                Log.Error("exception occur inside DecryptStringFromBytes" + e, this);
                return decryptedPlainText;
            }

        }

        public bool CheckAllowtoLogin(string emailid)
        {
            bool isallowlogin = false;
            double timedifference = 0;
            var User = rdb.AGELPortalRegistrations.Where(x => x.email == emailid).First();
            // var objLoginHistorylist = rdb.AGELPortalUserLoginHistories.Where(x => x.User_Id == User.Id && x.Login_Time.Value > DateTime.Now.AddMinutes(-60)).OrderByDescending(c => c.Login_Time.Value).Take(3).ToList();
            var objLoginHistorylist = rdb.AGELPortalUserLoginHistories.Where(x => x.User_Id == User.Id && x.Login_Time.Value > DateTime.Now.AddMinutes(-60) && x.checkLoginAttempts == true).OrderByDescending(c => c.Login_Time.Value).Take(3).ToList();

            int checktimediffer = 60;

            if (objLoginHistorylist != null)
            {
                if (objLoginHistorylist.Count() == 3)
                {
                    var objloginhistoryfirst = objLoginHistorylist.OrderByDescending(c => c.Login_Time).First();
                    var objloginhistorylast = objLoginHistorylist.OrderByDescending(c => c.Login_Time).Last();
                    // var lastdatetime = System.Convert.ToDateTime(objloginhistorylast.date.Value);
                    timedifference = System.Math.Round(objloginhistoryfirst.Login_Time.Value.Subtract(objloginhistorylast.Login_Time.Value).TotalMinutes);
                    if (timedifference > checktimediffer)
                    {
                        isallowlogin = true;
                    }
                    else
                    {
                        isallowlogin = false;
                    }
                }
                else if (objLoginHistorylist.Count() < 3)
                {
                    isallowlogin = true;
                }
            }
            else
            {
                isallowlogin = true;
            }

            return isallowlogin;
        }

        public ActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult ForgotPassword(PortalRegistrationModel pm)
        {
            string url = "";
            try
            {
                //added by neeraj yadav IsUserValid() code
                bool allowRegistration = IsUserValid(pm.mobile);
                if (!allowRegistration)
                {
                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "You have entered wrong OTP 3 times. Please try again after 1 hr."));
                    TempData["RegistrationOTPFailed"] = "You have entered wrong OTP 3 times.Please try again after 1 hr.";
                    if (Session["AGELPortalUser"] != null)
                    {
                        Session["AGELPortalUser"] = null;
                    }
                    if (Session["AGELPortalUserName"] != null)
                    {
                        Session["AGELPortalUserName"] = null;
                    }
                    if (Session["AGELPortalUserViewMode"] != null)
                    {
                        Session["AGELPortalUserViewMode"] = null;
                    }

                    SignOut();
                    return this.CustomRedirect("/AGELPortal/Home/login");
                }
                else
                {

                    var User = rdb.AGELPortalRegistrations.Where(x => x.mobile == pm.mobile).FirstOrDefault();
                    var token1 = string.Empty;
                    if (User != null && string.IsNullOrEmpty(User.password))
                    {
                        //this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "User is not registered"));
                        //return this.View(pm);
                        url = "/AGELPortal/Home/Forgot password step?token1=" + token1;
                    }
                    else if (User != null && User.status == true)
                    {
                        //var objLoginHistorylist = rdb.AGElPortalOtpHistories.Where(x => x.otp_for == User.mobile).OrderByDescending(c => c.date).Take(3).ToList();
                        token1 = pm.mobile;
                        //if (objLoginHistorylist.Count() == 3)
                        //{
                        var isallowtologin = CheckAllowtoLogin(User.email);
                        bool isAllowedToSendOtp = CanSendOTP(User.mobile);
                        if (!isallowtologin)
                        {
                            this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Please try again after 1 hour"));
                            //Session["InvalidOTP"] = "Please try again after 1 hour";
                            TempData["RegistrationOTPFailed"] = "Please try again after 1 hour";
                            url = "/AGELPortal/Home/login";
                        }
                        else if (!isAllowedToSendOtp)
                        {
                            this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Please try again after 1 hour"));
                            //Session["InvalidOTP"] = "Please try again after 1 hour";
                            TempData["RegistrationOTPFailed"] = "Please try again after 1 hour";
                            url = "/AGELPortal/Home/login";
                        }
                        else
                        {
                            pm.email = User.email;
                            pm.name = User.name;
                            SendOtpForValidation(pm);
                            url = "/AGELPortal/Home/Forgot password step?token1=" + token1;
                        }


                        //}
                        //else
                        //{
                        //    pm.email = User.email;
                        //    pm.name = User.name;
                        //    SendOtpForValidation(pm);
                        //    url = "/AGELPortal/Home/Forgot password step?token1=" + token1;
                        //}


                    }
                    else if (User != null)
                    {
                        token1 = pm.mobile;
                        this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "OTP Will be send on your registered mobile number"));
                        //Session["InvalidOTP"] = "OTP Will be send on your registered mobile number";
                        url = "/AGELPortal/Home/Forgot password step?token1=" + token1;
                        // return this.View(pm);
                    }
                    else
                    {
                        token1 = "ad3df";
                        this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "OTP Will be send on your registered mobile number"));
                        //Session["InvalidOTP"] = "OTP Will be send on your registered mobile number";
                        url = "/AGELPortal/Home/Forgot password step?token1=" + token1;
                    }

                }
            }
            catch (Exception ex)
            {

                this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", ex.ToString()));

                return this.View(pm);
            }


            return CustomRedirect(url);

        }

        public bool CheckIfUserEnterInvalidOTPTime(string moblieno)
        {
            List<DateTime> lastOtpsSentOn;
            int otpAllowedInBundary1 = 3;

            var objLoginHistory = rdb.AGElPortalOtpHistories.Where(x => x.otp_for == moblieno && x.status == true && x.date.Value > DateTime.Now.AddMinutes(-60)).OrderByDescending(c => c.date).FirstOrDefault();
            if (objLoginHistory != null)
            {
                if (objLoginHistory.WrongOTPAttempts >= otpAllowedInBundary1)
                {
                    return false;
                }
                else
                {
                    return true;
                }

            }
            else
            {
                return true;
            }
        }
        public bool IsUserLocked(string mobile)
        {
            var objLoginHistoryLock = rdb.AGElPortalOtpHistories.Where(x => x.otp_for == mobile && x.date.Value > DateTime.Now.AddMinutes(-60)).OrderByDescending(c => c.date).FirstOrDefault();
            if (objLoginHistoryLock != null && objLoginHistoryLock.IsuserLock)
            {
                return true;
            }
            return false;

        }
        public void AddInvalidAttemptOTPHistory(string moblieno)
        {
            var objInvalidAttemptOTPHistory = rdb.AGElPortalOtpHistories.Where(x => x.otp_for == moblieno && x.status == true).OrderByDescending(c => c.date).FirstOrDefault();
            if (objInvalidAttemptOTPHistory != null)
            {
                objInvalidAttemptOTPHistory.WrongOTPAttempts = objInvalidAttemptOTPHistory.WrongOTPAttempts + 1;
            }

            rdb.SubmitChanges();
        }
        public bool CanSendOTP(string moblieno)
        {
            List<DateTime> lastOtpsSentOn; int boundr1InMinutes = 60; int otpAllowedInBundary1 = 3; int boundr2InMinutes = 60;
            if (IsUserLocked(moblieno))
            {
                return false;
            }

            var objLoginHistorylist = rdb.AGElPortalOtpHistories.Where(x => x.otp_for == moblieno && x.status == true).OrderByDescending(c => c.date).Take(3).ToList();
            if (objLoginHistorylist != null && objLoginHistorylist.Count > 0)
            {


                lastOtpsSentOn = objLoginHistorylist.Select(c => c.date.Value).ToList();

                if (lastOtpsSentOn.Count < otpAllowedInBundary1)
                {
                    return true;
                }
                DateTime firstOtpSentInBoundry = lastOtpsSentOn.OrderByDescending(d => d).Take(otpAllowedInBundary1).Last();
                DateTime lastOtpSentInBoundry = lastOtpsSentOn.OrderByDescending(d => d).First();

                if ((lastOtpSentInBoundry - firstOtpSentInBoundry).TotalMinutes <= boundr1InMinutes)
                {
                    if (firstOtpSentInBoundry.AddMinutes(boundr2InMinutes) >= DateTime.Now)
                    {
                        //realtyRepo.DeleteOldOtp(moblieno);

                        var objLoginHistoryLock = rdb.AGElPortalOtpHistories.Where(x => x.otp_for == moblieno && x.status == true).OrderByDescending(c => c.date).FirstOrDefault();
                        if (objLoginHistoryLock != null)
                        {
                            objLoginHistoryLock.IsuserLock = true;
                            rdb.SubmitChanges();
                        }
                        //var removeexistotp = rdb.AGElPortalOtpHistories.Where(x => x.otp_for == moblieno && x.status == true);
                        foreach (var item in objLoginHistorylist)
                        {
                            item.status = false;

                        }
                        rdb.SubmitChanges();
                        return false;
                    }

                }
                return true;
            }
            else
            {
                return true;
            }
        }

        public ActionResult ForgotPasswordOTP()
        {
          

            //if(TempData["InvalidUser"] != null)
            //{
            //    ViewBag.InvalidUser = System.Convert.ToString(TempData["InvalidUser"]);
            //}
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPasswordOTP(PortalRegistrationModel pm, string token1 = null)
        {
            string url = "";

            pm.mobile = token1;
            try
            {
                Regex pattern = new Regex(@"(?<!\d)\d{10}(?!\d)");

                if(!pattern.Match(token1).Success)
                {
                    Session["InvalidOTP"] = "Invalid Request.";
                    url = "/agelportal/home/Forgot password step?token1=";
                    return this.CustomRedirect(url);
                }

                string generatedOTP = realtyRepo.GetOTP(pm.mobile);
                bool isreachmaxinvalidattempt = CheckIfUserEnterInvalidOTPTime(pm.mobile);
                if (!isreachmaxinvalidattempt)
                {
                    //var User = rdb.AGElPortalOtpHistories.Where(x => x.otp_for == pm.mobile && x.status == true && x.WrongOTPAttempts == 2).FirstOrDefault();
                    //if (User != null)
                    //{
                    //    var time1 = User.date.Value.AddMinutes(60);
                    //    var time2 = DateTime.Now;
                    //    var timeLeft = time2 - time1;
                    //    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "You have entered wrong OTP 3 times. Please try again after 1 hr."));
                    //    TempData["RegistrationOTPFailed"] = "You have entered wrong OTP 3 times.Please try again after " + timeLeft.Minutes + "min" + timeLeft.Seconds + "sec";
                    //}

                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "You have entered wrong OTP 3 times. Please try again after 1 hr."));
                    TempData["RegistrationOTPFailed"] = "You have entered wrong OTP 3 times.Please try again after 1 hr.";
                    if (Session["AGELPortalUser"] != null)
                    {
                        Session["AGELPortalUser"] = null;
                    }
                    if (Session["AGELPortalUserName"] != null)
                    {
                        Session["AGELPortalUserName"] = null;
                    }
                    if (Session["AGELPortalUserViewMode"] != null)
                    {
                        Session["AGELPortalUserViewMode"] = null;
                    }
                    SignOut();
                    return this.CustomRedirect("/AGELPortal/Home/login");

                    // return this.View(pm);
                }
                else if (generatedOTP == "optexpired")
                {
                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "OTP is expired, please generate again."));
                    //return this.View(pm);
                    Session["InvalidOTP"] = "OTP is expired, please generate again.";
                    url = "/agelportal/home/Forgot password step?token1=" + token1;
                }
                else if (!string.Equals(generatedOTP, pm.OTP))
                {

                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Please enter the correct OTP."));
                    Session["InvalidOTP"] = "Please enter the correct OTP.";
                    url = "/agelportal/home/Forgot password step?token1=" + token1;
                    AddInvalidAttemptOTPHistory(pm.mobile);

                }
                else
                {
                    Session["Mobile"] = pm.mobile;
                    realtyRepo.DeleteOldOtp(pm.mobile);
                    url = "/AGELPortal/Home/Reset Password";
                }
            }
            catch (Exception ex)
            {

                this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", ex.ToString()));

                return this.View(pm);
            }


            return CustomRedirect(url);
        }
        public ActionResult ResetPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ResetPassword(PortalRegistrationModel pm)
        {
            //var a = Session["Mobile"];
            pm.mobile = Session["Mobile"].ToString();
            var url = "";
            var usercheck = rdb.AGELPortalRegistrations.Where(x => x.mobile == pm.mobile).FirstOrDefault();
            if (pm.mobile != null)
            {
                if (usercheck != null)
                {
                    if (pm.confirm_password != null && pm.confirm_password != "" && usercheck.password == pm.confirm_password)
                    {
                        this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Old Password and New Password should not same."));
                        return this.View(pm);
                    }
                    else if (pm.confirm_password != null && pm.confirm_password != "" && pm.password == pm.confirm_password)
                    {
                        usercheck.password = pm.password;
                        usercheck.modified = System.DateTime.Now;
                        rdb.SubmitChanges();
                        Session["Mobile"] = null;
                        TempData["ResetPassword"] = "Password Reset Successfully";
                        realtyRepo.DeleteOldOtp(pm.mobile);
                        url = "/AGELPortal/Home/login";
                        //send email for updated password                   
                        SendEmail_UserUpdatedPassword(usercheck.email, usercheck.name, usercheck.password);
                        string sms_content = "Dear " + usercheck.name +",";
                        sms_content = "Your account with AGEL - Safety Portal is active Now.";
                        // sms_content = " New Password: " + usercheck.password;
                        //if (SendSMSUpdates(usercheck.mobile, sms_content))
                        //{
                        //    Log.Info("sms sent", "");
                        //}
                        //else
                        //{
                        //    Log.Info("sms failed", "");
                        //}
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Please enter new password and confirm new password. "));
                        return this.View(pm);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "OTP cannot be sent. Please check details."));
                    return this.View(pm);
                }
            }
            else
            {
                url = "/AGELPortal/Home/Forgot Password";
            }
            return CustomRedirect(url);
        }
        [HttpGet] //GMS Logout
        public ActionResult AGELPortalLogout()
        {
            if (Session["AGELPortalUser"] != null)
            {
                Session["AGELPortalUser"] = null;
            }
            if (Session["AGELPortalUserName"] != null)
            {
                Session["AGELPortalUserName"] = null;
            }
            if (Session["AGELPortalUserViewMode"] != null)
            {
                Session["AGELPortalUserViewMode"] = null;
            }
            SignOut();
            return this.CustomRedirect("/AGELPortal/Home/login");
        }
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalAddUser()
        {
            var usertype = Session["AGELPortalUserType"]?.ToString();

            if (Session["AGELPortalUserType"].ToString() != "superadmin")
            {
                return CustomRedirect("/AGELPortal/Home/login");
            }


            return base.View();
        }
        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalAddUser(PortalAddUserModel pm)
        {
            string msg = "";
            string url = "";
            try
            {

                if (ModelState.IsValid)

                {
                    AGELPortalContent content1 = new AGELPortalContent();
                    AGELPortalRegistration pgr = new AGELPortalRegistration();
                    if (pm.user_type != "superadmin")
                    {
                        if (rdb.AGELPortalRegistrations.Any(x => x.mobile == pm.mobile.Trim() || x.email == pm.email.Trim()))
                        {

                            this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "User is already registerd."));
                            return this.View(pm);
                        }
                        else
                        {
                            //var Mobile = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Mobile && x.OTP == pm.Mobile_Verified && x.OTPType == "registration").FirstOrDefault();
                            //var Email = rdb.PortsGMSOTPHistories.Where(x => x.OTPFor == pm.Email && x.OTP == pm.Email_Verified && x.OTPType == "registration").FirstOrDefault();
                            //if (Mobile.Status == true && Email.Status == true)
                            //{
                            PasswordGenerator generator = new PasswordGenerator();
                            pm.Id = Guid.NewGuid();
                            pgr.Id = pm.Id;
                            pgr.name = pm.name;
                            pgr.email = pm.email;
                            pgr.mobile = pm.mobile;

                            if (pm.user_type == "1")
                            {
                                pm.user_type = "adani";
                            }
                            else if (pm.user_type == "2")
                            {
                                pm.user_type = "vendor";
                            }
                            else
                            {
                                pm.user_type = "adani";
                            }
                            pgr.user_type = pm.user_type;
                            pgr.status = true;
                            pgr.vendor = pm.vendor;
                            pgr.site = pm.site;
                            pgr.created_date = System.DateTime.Now; ;
                            pgr.modified = System.DateTime.Now; ;
                            //pgr.password = generator.RandomPassword(12);
                            #region Insert to DB
                            rdb.AGELPortalRegistrations.InsertOnSubmit(pgr);
                            rdb.SubmitChanges();
                            #endregion

                            //SendVerificationEmail(pm.email, pgr.name, new Guid(), PortsGMSTemplates.GMSFlags.Registration);

                            // msg = "User Registration Successfully Done, Please Login.";

                            Session["addUserSuccess"] = "User added successfully.";
                            url = "/AGELPortal/Home/Dashboard/user_managment";
                            SendEmail_NewUserAdded(pgr.email, pgr.name);
                            string sms_content = "Hi " + pgr.name;
                            sms_content = ". Your account with AGEL - Safety Portal is active now. Please use your credential to login and browse through the available resources:";
                            sms_content = sms_content +" Name: " + pgr.name+",";
                            sms_content = sms_content + " Email: " + pgr.email+",";                     
                            sms_content = sms_content + "Thank You,";
                            //if (SendSMSUpdates(pgr.mobile, sms_content))
                            //{
                            //    Log.Info("sms sent", "");
                            //}
                            //else
                            //{
                            //    Log.Info("sms failed", "");
                            //}


                        }
                    }
                    else
                    {

                        this.ModelState.AddModelError(nameof(pm.user_type), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "User cannot be a super admin"));
                        return this.View(pm);
                    }
                }
                else
                {
                    return this.View(pm);
                }
            }
            catch (Exception ex)
            {

                this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", ex.ToString()));

                return this.View(pm);
            }
            return CustomRedirect(url);
        }
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalEditUser()
        {
            var usertype = Session["AGELPortalUserType"]?.ToString();

            if (Session["AGELPortalUserType"].ToString() != "superadmin")
            {
                return CustomRedirect("/AGELPortal/Home/login");
            }


            Uri myUri = new Uri(Request.Url.AbsoluteUri);

            Guid Id = new Guid(HttpUtility.ParseQueryString(myUri.Query).Get("Id"));

            var user = rdb.AGELPortalRegistrations.Where(x => x.Id == Id).FirstOrDefault();



            PortalAddUserModel UserModel = new PortalAddUserModel();
            UserModel.sites

               = rdb.AGELPortalRegistrations.Where(t => t.site != null).Select(t => new SelectListItem
               {
                   Text = t.site.ToString(),
                   Value = t.site.ToString()

               }).Distinct().ToList();
            UserModel.mobile = user.mobile;
            UserModel.user_type = user.user_type;
            UserModel.vendor = user.vendor;
            UserModel.site = user.site;
            UserModel.email = user.email;
            UserModel.name = user.name;
            UserModel.Id = user.Id;
            UserModel.user = user;
            return base.View(UserModel);
        }

        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalEditUser(PortalAddUserModel pm)
        {
            if (ModelState.IsValid)
            {
                if (pm.user_type != "superadmin")
                {
                    bool Checkuser = false;
                    var user = rdb.AGELPortalRegistrations.Where(x => x.Id == pm.Id).FirstOrDefault();
                    pm.sites = rdb.AGELPortalRegistrations.Where(t => t.site != null).Select(t => new SelectListItem
                    {
                        Text = t.site.ToString(),
                        Value = t.site.ToString()
                    }).Distinct().ToList();

                    if (user != null)
                    {
                        var isEmailAlreadyExists = rdb.AGELPortalRegistrations.Any(x => x.email == pm.email && x.Id != pm.Id);
                        var isMobileAlreadyExists = rdb.AGELPortalRegistrations.Any(x => x.mobile == pm.mobile && x.Id != pm.Id);

                        if(isEmailAlreadyExists)
                        {
                            this.ModelState.AddModelError(nameof(pm.email), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/Invalid email", "This email id already exists for another user."));
                            pm.user_type = user.user_type;
                            return this.View(pm);
                            //ModelState.AddModelError("Mobile", " Mobile No  already exists");
                        }
                       else if (isMobileAlreadyExists)
                        {
                            this.ModelState.AddModelError(nameof(pm.mobile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/Invalid Mobile", "This mobile number already exists for another user."));
                            pm.user_type = user.user_type;
                            return this.View(pm);
                            //ModelState.AddModelError("Mobile", " Mobile No  already exists");
                        }                        
                      else
                        {
                            user.name = pm.name;
                            user.email = pm.email;
                            //user.user_type = pm.user_type;
                            if (pm.user_type == "1")
                            {
                                user.user_type = "adani";
                            }
                            else if (pm.user_type == "2")
                            {
                                user.user_type = "vendor";
                            }
                            else
                            {
                                user.user_type = "adani";
                            }
                            user.mobile = pm.mobile;
                            //user.status = true;
                            user.vendor = pm.vendor;
                            user.site = pm.site;
                            user.modified = System.DateTime.Now;
                            rdb.SubmitChanges();

                            //ModelState.AddModelError("Mobile", " Mobile No  already exists");
                            Session["addUserSuccess"] = "User updated successfully.";
                            return CustomRedirect("/AGELPortal/Home/Dashboard/user_managment");
                            //return this.View(pm);
                        }
                        //else
                        //{
                        //    Session["addUserSuccess"] = "User not updated. Please try again.";
                        //    return this.View(pm);
                        //}
                    }
                    else
                    {
                        return this.View(pm);
                    }
                }
                else
                {
                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "User cannot be a super admin"));
                    return this.View(pm);
                }
            }
            else
            {
                return this.View(pm);
            }
            //return CustomRedirect("/Dashboard/user_managment");
        }
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalCategory()
        {
            var usertype = Session["AGELPortalUserType"]?.ToString();

            if (Session["AGELPortalUserType"].ToString() != "superadmin")
            {
                return CustomRedirect("/AGELPortal/Home/login");
            }
            Uri myUri = new Uri(Request.Url.AbsoluteUri);
            var page = System.Convert.ToInt32(HttpUtility.ParseQueryString(myUri.Query).Get("page"));
            var pageNumber = page > 0 ? page : 1;
            var pageSize = 10;
            PortalCategoryModel cat = new PortalCategoryModel();
            cat.totalRecord = rdb.AGElPortalCategories.Count();
            cat.categories = rdb.AGElPortalCategories.OrderByDescending(x => x.created_date).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return this.View(cat);
        }
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalUsers(string searchKeyword = null)
        {
            var usertype = Session["AGELPortalUserType"]?.ToString();

            if (Session["AGELPortalUserType"].ToString() != "superadmin")
            {
                return CustomRedirect("/AGELPortal/Home/login");
            }
            Uri myUri = new Uri(Request.Url.AbsoluteUri);
            var page = System.Convert.ToInt32(HttpUtility.ParseQueryString(myUri.Query).Get("page"));
            var userType = HttpUtility.ParseQueryString(myUri.Query).Get("user_type");
            var siteType = HttpUtility.ParseQueryString(myUri.Query).Get("site");
            var pageNumber = page > 0 ? page : 1;
            var pageSize = 10;
            PortalAddUserModel User = new PortalAddUserModel();
            //User.totalRecord = rdb.AGELPortalRegistrations.Count();
            if (userType != null && userType != "" && siteType != null && siteType != "" && searchKeyword != null && searchKeyword != "")
            {
                var totalCount = rdb.AGELPortalRegistrations.Where(x => x.user_type == userType && x.site == siteType && (x.name.Contains(searchKeyword) || x.email.Contains(searchKeyword) || x.mobile.Contains(searchKeyword))).ToList().Count();
                User.Users = rdb.AGELPortalRegistrations.Where(x => x.user_type == userType && x.user_type != "superadmin" && x.site == siteType && (x.name.Contains(searchKeyword) || x.email.Contains(searchKeyword) || x.mobile.Contains(searchKeyword))).OrderByDescending(x => x.created_date).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                User.totalRecord = totalCount;
            }
            else if (userType != null && userType != "" && siteType != null && siteType != "")
            {
                var totalCount = rdb.AGELPortalRegistrations.Where(x => x.user_type == userType && x.site == siteType).OrderByDescending(x => x.created_date).ToList().Count();
                User.Users = rdb.AGELPortalRegistrations.Where(x => x.user_type == userType && x.user_type != "superadmin" && x.site == siteType).OrderByDescending(x => x.created_date).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                User.totalRecord = totalCount;
            }
            else if (userType != null && userType != "all" && userType != "" && searchKeyword != null && searchKeyword != "")
            {
                var totalCount = rdb.AGELPortalRegistrations.Where(x => x.user_type == userType && (x.name.Contains(searchKeyword) || x.email.Contains(searchKeyword) || x.mobile.Contains(searchKeyword))).OrderByDescending(x => x.created_date).ToList().Count();
                User.Users = rdb.AGELPortalRegistrations.Where(x => x.user_type == userType && x.user_type != "superadmin" && (x.name.Contains(searchKeyword) || x.email.Contains(searchKeyword) || x.mobile.Contains(searchKeyword))).OrderByDescending(x => x.created_date).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                User.totalRecord = totalCount;
            }
            else if (siteType != null && siteType != "all" && siteType != "" && searchKeyword != null && searchKeyword != "")
            {
                var totalCount = rdb.AGELPortalRegistrations.Where(x => x.site == siteType && (x.name.Contains(searchKeyword) || x.email.Contains(searchKeyword) || x.mobile.Contains(searchKeyword))).OrderByDescending(x => x.created_date).ToList().Count();
                User.Users = rdb.AGELPortalRegistrations.Where(x => x.site == siteType && x.user_type != "superadmin" && (x.name.Contains(searchKeyword) || x.email.Contains(searchKeyword) || x.mobile.Contains(searchKeyword))).OrderByDescending(x => x.created_date).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                User.totalRecord = totalCount;
            }
            else if (userType != null && userType != "all" && userType != "")
            {
                var totalCount = rdb.AGELPortalRegistrations.Where(x => x.user_type == userType).OrderByDescending(x => x.created_date).ToList().Count();
                User.Users = rdb.AGELPortalRegistrations.Where(x => x.user_type == userType && x.user_type != "superadmin").OrderByDescending(x => x.created_date).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                User.totalRecord = totalCount;
            }
            else if (siteType != null && siteType != "all" && siteType != "")
            {
                var totalCount = rdb.AGELPortalRegistrations.Where(x => x.site == siteType).OrderByDescending(x => x.created_date).ToList().Count();
                User.Users = rdb.AGELPortalRegistrations.Where(x => x.site == siteType && x.user_type != "superadmin").OrderByDescending(x => x.created_date).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();

                User.totalRecord = totalCount;
            }
            else if (searchKeyword != null && searchKeyword != "")
            {
                var totalCount = rdb.AGELPortalRegistrations.Where(x => x.name.Contains(searchKeyword) || x.email.Contains(searchKeyword) || x.mobile.Contains(searchKeyword)).OrderByDescending(x => x.created_date).ToList().Count();
                User.Users = rdb.AGELPortalRegistrations.Where(x => x.name.Contains(searchKeyword) && x.user_type != "superadmin" || x.email.Contains(searchKeyword) || x.mobile.Contains(searchKeyword)).OrderByDescending(x => x.created_date).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                User.totalRecord = totalCount;
            }
            else
            {
                User.Users = rdb.AGELPortalRegistrations.Where(x => x.user_type != "superadmin").OrderByDescending(x => x.created_date).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                User.totalRecord = rdb.AGELPortalRegistrations.Where(x => x.user_type != "superadmin").Count();
            }
            return this.View(User);
        }
        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalCategory(PortalCategoryModel m)
        {
            string url = "";
            try
            {
                // return CustomRedirect("www.google.com");

                var page = m.page;
                var pageNumber = page > 0 ? page : 1;
                var pageSize = 10;
                if (m.Id != new Guid())
                {
                    m.mType = "editCat";
                }
                else
                {
                    m.mType = "addCat";
                }
                m.totalRecord = rdb.AGElPortalCategories.Count();
                m.categories = rdb.AGElPortalCategories.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
                if (ModelState.IsValid)
                {
                    if (m.mType != "editCat")
                    {
                        AGElPortalCategory pgr = new AGElPortalCategory();
                        if (rdb.AGElPortalCategories.Any(x => x.name == m.name))
                        {
                            Session["ExistCatagory"] = "Category already exists.";
                            //this.ModelState.AddModelError(nameof(m.validation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Category allready created"));
                            return View(m);
                        }
                        else
                        {
                            m.Id = Guid.NewGuid();
                            pgr.Id = m.Id;
                            pgr.name = m.name;
                            //pgr.status = true;
                            pgr.status = m.status == "1" ? true : false;
                            pgr.created_date = System.DateTime.Now;
                            pgr.modified = System.DateTime.Now;
                            #region Insert to DB
                            rdb.AGElPortalCategories.InsertOnSubmit(pgr);
                            Session["addCatSuccess"] = "Category added successfully.";
                            url = "/AGELPortal/Home/Dashboard/categories";
                            //return this.View(m);
                        }
                    }
                    else
                    {
                        var cat = rdb.AGElPortalCategories.Where(x => x.Id == m.Id).FirstOrDefault();
                        if (cat != null)
                        {
                            cat.name = m.name;
                            cat.status = m.status == "1" ? true : false;
                            //cat.status = m.status;
                            Session["addCatSuccess"] = "Category updated successfully.";
                            //Session["addCatSuccess"] = DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/Categoryupdatedsuccessfully", "Category updated successfully"); 

                        }
                    }
                    rdb.SubmitChanges();
                    url = "/AGELPortal/Home/Dashboard/categories";


                }

                else
                {
                    return this.View(m);
                }

            }
            catch (Exception ex)
            {
                this.ModelState.AddModelError(nameof(m.validation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", ex.ToString()));

                return this.View(m);
                throw;
            }

            return CustomRedirect(url);
        }

        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult GetCategory(PortalCategoryModel m)
        {


            var cat = rdb.AGElPortalCategories.Where(x => x.Id == m.Id).FirstOrDefault();
            if (cat != null)
            {

                var result = new { status = "1", CatStatus = cat.status, Namecat = cat.name };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var result1 = new { status = "0" };
            return Json(result1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalChangeUserStatus(string Id)
        {
            var user = rdb.AGELPortalRegistrations.Where(x => x.Id == new Guid(Id)).FirstOrDefault();
            if (user != null)
            {
                bool status = user.status == true ? false : true;
                user.status = user.status == true ? false : true;
                rdb.SubmitChanges();

                //send email update to user 
                SendEmail_UserAccountStatus(user.email, user.name, status);
                //send email update to user 
                string sms_content = "AGEL Portal  Profile Status- ";
                sms_content += " Name:" + user.name;
                sms_content += " Email:" + user.email;
                sms_content += " Status:" + status;
                //if(SendSMSUpdates(user.mobile, sms_content))
                //{
                //    Log.Info("sms sent", "");
                //}
                //else
                //{
                //    Log.Info("sms failed","");
                //}
                //var Txt = status == true ? "Active" : "Inactive";
                //var Txt = user.status == "Active" ? "Status" : "status2";
                var Txt = user.status == true ? "Active" : "Inactive";

                var result = new { status = "1", txt = Txt };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var result1 = new { status = "0" };
            return Json(result1, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalChangeCategoryStatus(string Id)
        {

            var cat = rdb.AGElPortalCategories.Where(x => x.Id == new Guid(Id)).FirstOrDefault();
            if (cat != null)
            {
                cat.status = cat.status == true ? false : true;
                rdb.SubmitChanges();
                var Txt = cat.status == true ? "Active" : "Inactive";
                var result = new { status = "1", txt = Txt, clas = "status" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var result1 = new { status = "0" };
            return Json(result1, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalChangeContentStatus(string Id)
        {

            var cat = rdb.AGELPortalContents.Where(x => x.Id == new Guid(Id)).FirstOrDefault();
            if (cat != null)
            {
                cat.status = cat.status == true ? false : true;
                rdb.SubmitChanges();
                var Txt = cat.status == true ? "Published" : "Unpublished";
                var result = new { status = "1", txt = Txt };
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            var result1 = new { status = "0" };
            return Json(result1, JsonRequestBehavior.AllowGet);

        }

        // [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalDashboard()
        {
            if (ClaimsPrincipal.Current.Claims.ToList().Count() > 10)
            {
                var checkClaims = ClaimsPrincipal.Current.Claims.ToList();
                Log.Info("Adani AD checkClaims Dashboard" + checkClaims, this);
                string useremail = checkClaims != null && checkClaims.Count > 0 ? checkClaims.Where(x => x.Type.Contains("upn")).FirstOrDefault().Value : "";
                var User = rdb.AGELPortalRegistrations.Where(x => x.email == useremail && x.status == true).FirstOrDefault();
                if (User != null)
                {
                    Session["AGELPortalUser"] = User.Id.ToString();
                    Session["AGELPortalUser1"] = User.Id;
                    Session["AGELPortalUserName"] = User.name.ToString();
                    Session["AGELPortalUserType"] = User.user_type.ToString();
                    Session["Date"] = System.DateTime.Now.ToString();
                    //Log.Info("Adani AD AGELPortalUser Dashboard" + Session["AGELPortalUser"], this);
                    //Log.Info("Adani AD AGELPortalUser1 Dashboard" + Session["AGELPortalUser1"], this);
                    //Log.Info("Adani AD AGELPortalUserName Dashboard" + Session["AGELPortalUserName"], this);
                    //Log.Info("Adani AD AGELPortalUserType Dashboard" + Session["AGELPortalUserType"], this);
                }
            }
            //Session["AGELPortalUser"] = "demo@adani.com";
            var usertype = Session["AGELPortalUserType"]?.ToString();
            if (usertype != null)
            {
                Session["AGELPortalUserViewMode"] = "admin";
                if (Session["AGELPortalUserType"].ToString() != "superadmin")
                {
                    return CustomRedirect("/AGELPortal/Home");
                }
                //else
                //{
                //    return Redirect("/");
                //}
            }
            else
            {
                if (Session["AGELPortalUser"] != null)
                {
                    Session["AGELPortalUser"] = null;
                }
                if (Session["AGELPortalUserName"] != null)
                {
                    Session["AGELPortalUserName"] = null;
                }
                if (Session["AGELPortalUserType"] != null)
                {
                    Session["AGELPortalUserType"] = null;
                }

                SignOut();
                return CustomRedirect("/AGELPortal/Home/login");
                // return Redirect("/AGELPortal/Home");
            }
            return View();
        }
        [AGELRedirectUnauthenticated]
        public ActionResult DashboardVisiter()
        {
            PortalRegistrationModel model1 = new PortalRegistrationModel();

            var usercount = rdb.AGELPortalRegistrations.Where(x => x.Id != null && x.status == true && x.user_type != "superadmin").ToList();
            var countdocument = rdb.AGELPortalContents.Where(x => x.Id != null && x.contetn_type != "video" && x.status == true).ToList();
            var countvideo = rdb.AGELPortalContents.Where(x => x.Id != null && x.contetn_type != "document" && x.status == true).ToList();
            var countday = rdb.AGELPortalUserLoginHistories.Where(x => x.User_Id != null && x.Login_Time >= DateTime.Now.AddDays(-1)).ToList();


            model1.usercount = usercount.Count();
            model1.video_contents = countvideo;
            model1.document_contents = countdocument;
            model1.User_visitors = countday;

            //var url = "/dashboard";
            return View(model1);

        }
        [AGELRedirectUnauthenticated]
        public ActionResult UserViewed()
        {
            PortalLessonlerneningVideo model = new PortalLessonlerneningVideo();
            List<string> userviewdeList = new List<string>();
            List<PortalLessonlerneningVideo> adduserviewdeList = new List<PortalLessonlerneningVideo>();
            var GetListDetails = rdb.AGELPortalLessonLearningVideos.ToList();
            var GetListContent = GetListDetails.Where(x => x.user_id != null && x.content_type != null).ToList();
            var GetList = GetListContent.GroupBy(o => o.user_id).ToList();

            foreach (var item in GetList)
            {

                PortalLessonlerneningVideo obj = new PortalLessonlerneningVideo();
                var userId = item.Select(x => x.user_id).FirstOrDefault();
                var userData = rdb.AGELPortalRegistrations.Where(x => x.Id == userId && x.status == true && x.user_type != "superadmin").FirstOrDefault();
                if (userData != null)
                {
                    obj.user_name = userData.name;
                    obj.User_Site = userData.site;

                    //Check First and Last name then get First and last charachter
                    String[] nameparts = userData.name.Split(' ');
                    var initials = "AB";
                    if (nameparts.Count() > 0 && nameparts.Count() == 1)
                    {
                        initials = (nameparts[0] != null ? nameparts[0].Substring(0, 1).ToUpper() : null);
                    }
                    else if (nameparts.Count() > 1 && nameparts.Count() == 2)
                    {
                        initials = (nameparts[0] != null ? nameparts[0].Substring(0, 1).ToUpper() : null) + (nameparts[1] != null ? nameparts[1].Substring(0, 1).ToUpper() : null);
                    }

                    obj.part_user_name = initials;
                    obj.userId = userId;
                    obj.VideoCount = item.Where(x => x.user_id == userId && x.content_type == "video").Select(x => x.video_id).Distinct().ToList().Count();
                    obj.DocCount = item.Where(x => x.user_id == userId && x.content_type == "document").Select(x => x.video_id).Distinct().ToList().Count();

                    adduserviewdeList.Add(obj);
                }
            }
            model.videos = adduserviewdeList;
            return View(model);
        }
        public ActionResult LearningOverview()
        {
            PortalLessonlerneningVideo model = new PortalLessonlerneningVideo();
            List<string> learningoverviewList = new List<string>();
            List<PortalLessonlerneningVideo> addlearningoverviewdeList = new List<PortalLessonlerneningVideo>();
            var GetListDetails = rdb.AGELPortalLessonLearningVideos.ToList();
            var GetListContent = GetListDetails.Where(x => x.user_id != null && x.content_type != null).ToList();
            var GetList = GetListContent.GroupBy(o => o.video_id).ToList();



            foreach (var item in GetList)
            {
                PortalLessonlerneningVideo obj = new PortalLessonlerneningVideo();
                var videoId = item.Select(x => x.video_id).FirstOrDefault();

                obj.video_name = rdb.AGELPortalLessonLearningVideos.Where(x => x.video_id == videoId).Select(x => x.video_name).FirstOrDefault();
                obj.Content_Type = rdb.AGELPortalLessonLearningVideos.Where(x => x.video_id == videoId).Select(x => x.content_type).FirstOrDefault();
                // obj.userId = item.
                obj.ContentCount = item.Where(x => x.video_id == videoId).Select(x => x.user_id).Distinct().ToList().Count();
                obj.DocVideoImage_Name = rdb.AGELPortalContents.Where(x => x.Id == videoId).Select(x => x.DocVideoImage_Name).FirstOrDefault();
                obj.video_url = rdb.AGELPortalLessonLearningVideos.Where(x => x.video_id == videoId).Select(x => x.video_url).FirstOrDefault();
                obj.userId = item.Select(x => x.user_id).FirstOrDefault();
                obj.video_id = videoId.ToString();


                addlearningoverviewdeList.Add(obj);
            }
            model.videos = addlearningoverviewdeList;
            return View(model);
        }

        public ActionResult ContentOverviewPopup(string userID)
        {
            PortalLessonlerneningVideo model = new PortalLessonlerneningVideo();
            List<string> learningoverviewList = new List<string>();
            List<PortalLessonlerneningVideo> addlearningoverviewdeList = new List<PortalLessonlerneningVideo>();
            var userIDNew = !string.IsNullOrEmpty(userID) ? new Guid(userID) : new Guid();
            var GetListDetails = rdb.AGELPortalLessonLearningVideos.Where(x => x.user_id == userIDNew).ToList();
            var GetListContent = GetListDetails.Where(x => x.user_id != null && x.content_type != null).ToList();
            var GetList = GetListContent.GroupBy(o => o.video_id).ToList();

            var Video_List = rdb.AGELPortalLessonLearningVideos.Where(x => x.user_id == userIDNew && x.content_type == "video").ToList();
            //added by neeraj yadav 
            var Video_List2 = Video_List.Select(y => y.video_id).Distinct().ToList();
            var Doc_List = GetListContent.Where(x => x.user_id == userIDNew && x.content_type == "document").ToList();
            var Doc_List1 = Doc_List.Select(x => x.video_id).Distinct().ToList();
            Video_List2.AddRange(Doc_List1);
            //till here


            foreach (var item in Video_List2)
            {
                PortalLessonlerneningVideo obj = new PortalLessonlerneningVideo();
                var recordContent = rdb.AGELPortalLessonLearningVideos.Where(x => x.video_id == item).FirstOrDefault();
                var videoId = recordContent.video_id;
                var created_date = recordContent.created_date;
                obj.video_id = videoId.ToString();
                obj.video_name = rdb.AGELPortalLessonLearningVideos.Where(x => x.video_id == videoId).Select(x => x.video_name).FirstOrDefault();
                obj.Content_Type = recordContent.content_type;
                obj.created_date = created_date?.ToString("d MMM yyyy");
                obj.DocVideoImage_Name = rdb.AGELPortalContents.Where(x => x.Id == videoId).Select(x => x.DocVideoImage_Name).FirstOrDefault();
                obj.video_url = rdb.AGELPortalLessonLearningVideos.Where(x => x.video_id == videoId).Select(x => x.video_url).FirstOrDefault();

                obj.ContentCount = rdb.AGELPortalLessonLearningVideos.Where(x => x.video_id == item).Select(x => x.user_id).Distinct().ToList().Count();
                addlearningoverviewdeList.Add(obj);
            }
            //foreach (var item in GetList)
            //{
            //    PortalLessonlerneningVideo obj = new PortalLessonlerneningVideo();
            //    var videoId = item.Select(x => x.video_id).FirstOrDefault();
            //    var created_date = item.Max(x => x.created_date);
            //    obj.video_id = videoId.ToString();
            //    obj.video_name = rdb.AGELPortalLessonLearningVideos.Where(x => x.video_id == videoId).Select(x => x.video_name).FirstOrDefault();
            //    obj.Content_Type = item.Select(x => x.content_type).FirstOrDefault().ToString();
            //    //obj.Content_Type = rdb.AGELPortalLessonLearningVideos.Where(x => x.video_id == videoId && x.content_type != null).Select(x => x.content_type).FirstOrDefault();
            //    obj.created_date = created_date?.ToString("d MMM yyyy");
            //    obj.DocVideoImage_Name = rdb.AGELPortalContents.Where(x => x.Id == videoId).Select(x => x.DocVideoImage_Name).FirstOrDefault();
            //    obj.video_url = rdb.AGELPortalLessonLearningVideos.Where(x => x.video_id == videoId).Select(x => x.video_url).FirstOrDefault();

            //    obj.ContentCount = item.Where(x => x.video_id == videoId).Select(x => x.user_id).Distinct().ToList().Count();
            //    addlearningoverviewdeList.Add(obj);
            //}
            model.videos = addlearningoverviewdeList;
            model.videos.Where(x => x.Content_Type != null);
            return View(model);


        }
        public ActionResult UserOverviewPopup(string videoID)
        {
            PortalLessonlerneningVideo model = new PortalLessonlerneningVideo();
            List<string> userviewdeList = new List<string>();
            List<PortalLessonlerneningVideo> adduserviewdeList = new List<PortalLessonlerneningVideo>();
            var GetListDetails = rdb.AGELPortalLessonLearningVideos.ToList();
            var videoIDNew = !string.IsNullOrEmpty(videoID) ? new Guid(videoID) : new Guid();
            var GetListContent = GetListDetails.Where(x => x.user_id != null && x.content_type != null && videoID != null && x.video_id == videoIDNew).ToList();
            var GetList = GetListContent.GroupBy(o => o.user_id).ToList();
            foreach (var item in GetList)
            {
                PortalLessonlerneningVideo obj = new PortalLessonlerneningVideo();
                var userId = item.Select(x => x.user_id).FirstOrDefault();
                var userData = rdb.AGELPortalRegistrations.Where(x => x.Id == userId).FirstOrDefault();
                if (userData != null)
                {
                    var created_date = item.Max(x => x.created_date);
                    obj.user_name = userData.name;
                    obj.User_Site = userData.site;
                    obj.created_date = created_date?.ToString("d MMM  yyyy");

                    //Check First and Last name then get First and last charachter
                    String[] nameparts = userData.name.Split(' ');
                    var initials = "AB";
                    if (nameparts.Count() > 0 && nameparts.Count() == 1)
                    {
                        initials = (nameparts[0] != null ? nameparts[0].Substring(0, 1).ToUpper() : null);
                    }
                    else if (nameparts.Count() > 1 && nameparts.Count() == 2)
                    {
                        initials = (nameparts[0] != null ? nameparts[0].Substring(0, 1).ToUpper() : null) + (nameparts[1] != null ? nameparts[1].Substring(0, 1).ToUpper() : null);
                    }

                    obj.part_user_name = initials;
                    obj.userId = userId;
                    obj.VideoCount = item.Where(x => x.user_id == userId && x.content_type == "video").Select(x => x.video_id).Distinct().ToList().Count();
                    obj.DocCount = item.Where(x => x.user_id == userId && x.content_type == "document").Select(x => x.video_id).Distinct().ToList().Count();
                    adduserviewdeList.Add(obj);
                }
            }
            model.videos = adduserviewdeList;
            return View(model);

        }
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalUserProfile()
        {

            if (Session["AGELPortalUser"] == null)
            {
                return CustomRedirect("/AGELPortal/Home/login");
            }
            var userId = new Guid(Session["AGELPortalUser"]?.ToString());
            var usertype = Session["AGELPortalUserType"]?.ToString();
            var user = rdb.AGELPortalRegistrations.Where(x => x.Id == userId).FirstOrDefault();
            PortalAddUserModel UserData = new PortalAddUserModel();
            UserData.name = user.name;
            UserData.mobile = user.mobile;
            UserData.email = user.email;
            //if (TempData["ChangepassWordSuccess"] != null)
            //{
            //    ViewBag.ChangepassWordSuccess = System.Convert.ToString(TempData["ChangepassWordSuccess"]);
            //}

            return View(UserData);
        }

        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult DeleteCategory(PortalCategoryModel m)
        {


            var cat = rdb.AGElPortalCategories.Where(x => x.Id == m.Id).FirstOrDefault();
            var catCheck = rdb.AGELPortalContents.Where(x => x.category_id == m.Id).ToList();

            if (cat != null && catCheck.Count() == 0)
            {
                rdb.AGElPortalCategories.DeleteOnSubmit(cat);
                rdb.SubmitChanges();
                var result = new { status = "1" };
                Session["addCatSuccess"] = "Catagory deleted succesfully.";
                return Json(result, JsonRequestBehavior.AllowGet);

            }
            else if (cat == null)
            {
                var result = new { status = "0" };
                Session["addCatSuccess"] = "Category does not exist.";
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            else if (catCheck.Count() != 0)
            {
                var result = new { status = "0" };
                Session["addCatSuccess"] = "Category data already exists.";
                return Json(result, JsonRequestBehavior.AllowGet);
            }


            var result1 = new { status = "0" };
            return Json(result1, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteUser(string UserId = "")
        {

            if (!string.IsNullOrEmpty(UserId))
            {
                var userRegistrations = rdb.AGELPortalRegistrations.Where(x => x.Id == new Guid(UserId)).FirstOrDefault();
                var userLessonLearningVideos = rdb.AGELPortalLessonLearningVideos.Where(x => x.user_id == new Guid(UserId)).ToList();
                foreach (var item in userLessonLearningVideos)
                {
                    rdb.AGELPortalLessonLearningVideos.DeleteOnSubmit(item);
                    rdb.SubmitChanges();
                }
                if (userRegistrations != null)
                {
                    rdb.AGELPortalRegistrations.DeleteOnSubmit(userRegistrations);
                    rdb.SubmitChanges();
                    var result = new { status = "1" };
                    Session["addUserSuccess"] = "User deleted succesfully.";


                    return Json(result, JsonRequestBehavior.AllowGet);
                }


            }
            var result1 = new { status = "0" };
            return Json(result1, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult AGELPortalUploadBulkUser(CsvUpload m)
        {
            string msg = "";
            var flag = false;
            var st = "0";
            var result = new { status = st, message = msg };
            try
            {
                HttpPostedFileBase CsvFile = Request.Files[0];
                var lineNumber = 0;
                var error = 0;

                using (StreamReader reader = new StreamReader(CsvFile.InputStream))
                {

                    while (!reader.EndOfStream)
                    {
                        var line = reader.ReadLine();
                        if (lineNumber != 0 && line != "")
                        {
                            var values = line.Split(',');
                            if (emailRegex.IsMatch(values[2]) == false || values[2] == "")
                            {
                                msg += "Email ID is not valid. " + lineNumber + "in CSV file, ";
                                flag = true;
                                error++;

                            }
                            if (mobileRegex.IsMatch(values[1]) == false || values[1] == "" || values[1].Length != 10)
                            {
                                msg += "Mobile number is not valid. " + lineNumber + "in CSV file, ";
                                flag = true;
                                error++;

                            }
                            //^[a-zA-Z ]+$
                            if (string.IsNullOrEmpty(values[0]))
                            {
                                msg += "Name is required." + lineNumber + "in CSV file, ";
                                flag = true;
                                error++;

                            }
                            else if (UserNameRegex.IsMatch(values[0]) == false)
                            {
                                msg += "Numeric and symbols inputs are not allowed in Name." + lineNumber + "in CSV file, ";
                                flag = true;
                                error++;
                            }



                            if (rdb.AGELPortalRegistrations.Any(x => x.mobile == values[1]))
                            {
                                msg += "This mobile number already exists. " + lineNumber + "in CSV file, ";
                                flag = true;
                                error++;
                            }

                            if (rdb.AGELPortalRegistrations.Any(x => x.email == values[2]))
                            {
                                msg += "This email ID already exists. " + lineNumber + "in CSV file, ";
                                flag = true;
                                error++;
                            }
                            var vendor = string.Empty;
                            if (string.IsNullOrEmpty(values[3]))
                            {
                                msg += "user type is required." + lineNumber + "in CSV file, ";
                                flag = true;
                                error++;

                            }
                            else if (System.Convert.ToString(values[3]).Trim().ToLower() != "vendor" && System.Convert.ToString(values[3]).Trim().ToLower() != "adani")
                            {
                                msg += "user type is either vendor or adani." + lineNumber + "in CSV file, ";
                                flag = true;
                                error++;
                            }

                            if (!string.IsNullOrEmpty(values[5]))
                            {
                                if (!string.IsNullOrEmpty(values[3]) && System.Convert.ToString(values[3]).Trim().ToLower() == "vendor" && UserNameRegex.IsMatch(values[5]) == false)
                                {
                                    msg += "Numeric and symbols inputs are not allowed in vendor." + lineNumber + "in CSV file, ";
                                    flag = true;
                                    error++;
                                }
                                if (!string.IsNullOrEmpty(values[3]) && System.Convert.ToString(values[3]).Trim().ToLower() == "vendor")
                                {
                                    vendor = System.Convert.ToString(values[5]).Trim();
                                }
                            }
                            //added by neeraj yadav, to check the entere site is exists or not.
                            if (!string.IsNullOrEmpty(values[4]))
                            {
                                var site = System.Convert.ToString(values[4]).Trim().ToLower();
                                //add AGELPortalCity table in dbml file
                                var city = rdb.AGELPortalCities.Where(x => x.sitename.ToLower() == site).Any();
                                if (!city)
                                {
                                    msg += "only Gurgaon, Mumbai, Ahemdabad are valid inputs for site " + lineNumber + " in CSV file, ";
                                    flag = true;
                                    error++;
                                }
                            }
                            //till here


                            msg += "<br>";
                            if (flag)
                                continue;

                            AGELPortalRegistration pgr = new AGELPortalRegistration();


                            pgr.Id = Guid.NewGuid();
                            pgr.name = values[0];
                            pgr.email = values[2];
                            pgr.mobile = values[1];

                            pgr.user_type = values[3];
                            //pgr.status = false;
                            pgr.status = true;
                            pgr.vendor = vendor;
                            pgr.site = System.Convert.ToString(values[4]).Trim();

                            pgr.created_date = System.DateTime.Now; ;
                            pgr.modified = System.DateTime.Now; ;
                            #region Insert to DB
                            rdb.AGELPortalRegistrations.InsertOnSubmit(pgr);
                            rdb.SubmitChanges();

                            SendEmail_NewUserAdded(pgr.email, pgr.name);

                            string sms_content = "Hi " + pgr.name;
                            sms_content = ". Your account with AGEL - Safety Portal is active now. Please use your credential to login and browse through the available resources.";
                            sms_content = sms_content + " Name: " + pgr.name +",";
                            sms_content = sms_content + " Email: " + pgr.email + ",";
                            sms_content = sms_content + " Thank You,";

                            //if (SendSMSUpdates(pgr.mobile, sms_content))
                            //{
                            //    Log.Info("sms sent", "");
                            //}
                            //else
                            //{
                            //    Log.Info("sms failed", "");
                            //}
                        }
                        #endregion


                        lineNumber++;
                        flag = false;
                    }
                }

                if (error > 0)
                {
                    st = "0";
                    msg += "<b> Users uploaded with error. Please check the errors mentioned.</b>";
                }
                else
                {
                    st = "1";
                    msg += "Users uploaded successfully.";
                }


                result = new { status = st, message = msg };
            }
            catch (Exception ex)
            {
                // result = new { status = "1" };
                Log.Error(ex.ToString(), ex.ToString());
                msg += ex.ToString();
                result = new { status = "0", message = msg };
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalAddContent()
        {
            Uri myUri = new Uri(Request.Url.AbsoluteUri);
            var RequstedId = HttpUtility.ParseQueryString(myUri.Query).Get("Id");
            Guid Id = new Guid();
            if (RequstedId != null)
            {
                Id = new Guid(RequstedId);
            }

            var content = rdb.AGELPortalContents.Where(x => x.Id == Id).FirstOrDefault();
            PortalAddContentModel ContentModel = new PortalAddContentModel();
            if (content != null)
                ContentModel.content = content;



            ContentModel.categories = rdb.AGElPortalCategories.Where(x => x.status == true).Select(t => new SelectListItem
            {
                Text = t.name.ToString(),

                Value = t.Id.ToString()
            }).ToList();


            return base.View(ContentModel);
        }

        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalAddContent(PortalAddContentModel m, string id = null)
        {

            var url = "/add-content";
            string fileBrowsPath = "";

            m.categories = rdb.AGElPortalCategories.Where(x => x.status == true).Select(t => new SelectListItem
            {
                Text = t.name.ToString(),
                Value = t.Id.ToString()
            }).ToList();
            try
            {
                AGELPortalContent dt = new AGELPortalContent();

                var content = rdb.AGELPortalContents.Where(x => x.Id == m.Id).FirstOrDefault();

                if (m.endpoint_url != null)
                {
                    fileBrowsPath = m.endpoint_url;
                }                

                //till here


                if (content != null)
                {
                    Log.Info("Edit Content document", this);

                    byte[] bytes;

                    if (m.agel_file != null && m.agel_file.ContentLength > 0 && m.content_type == "document")
                    {
                        Log.Info("Content type document" , this);
                        //Removing special and unwanted characters from string
                         var documentFileName = GetValidFileName(m.agel_file.FileName);

                        //added by neeraj yadav for extension check
                        //string[] allowedExtenstions1 = new string[] { ".txt", ".doc", ".docx", ".pdf", ".xls", ".xlsx", ".mp4", ".flv", ".wmv", ".avi", ".mp3", ".webm", ".mpg", ".mkv", ".mov" };
                        string[] allowedExtenstions1 = new string[] { ".doc", ".docx", ".pdf", ".xls", ".xlsx"};

                        //string extension1 = Path.GetExtension(m.agel_file.FileName);
                        string extension1 = Path.GetExtension(documentFileName);

                        if (!allowedExtenstions1.Contains(extension1))
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only Word/PDF/Excel/txt/mp4/mp3 format."));
                            return this.View(m);
                        }
                        //till here
                        //added by neeraj for mime check
                        //string str = m.agel_file.FileName;
                        string str = documentFileName;
                        char ch = '.';
                        int occurance = str.Count(f => (f == ch));
                        if (occurance > 1)
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload file with single extension"));
                            return this.View(m);
                        }
                        //till here
                        //added by neeraj for checking content type
                        string contentType = GetMimeTypeByWindowsRegistry(extension1);
                        if (contentType != "")
                        {
                            if (contentType != "text/plain" && contentType != "application/msword" && contentType != "application/pdf" && contentType != "application/vnd.ms-excel" )
                            {
                                //&& contentType != "video/mp4" && contentType != "video/x-flv" && contentType != "video/x-ms-wmv" && contentType != "video/x-msvideo" && contentType != "audio/mpeg" && contentType != "video/webm" && contentType != "video/mpeg" && contentType != "video/x-matroska" && contentType != "video/quicktime"
                                this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only Word/PDF/Excel/txt/mp4/mp3 format."));
                                return this.View(m);
                            }
                        }
                        else
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only jpeg/jpg/png format."));
                            return this.View(m);
                        }
                        //till here
                        int tenMegaBite = 10 * 1024 * 1024;
                        var docSize = m.agel_file.ContentLength;
                        if (docSize > tenMegaBite)
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "document size should be less than 10 MB"));
                            return this.View(m);
                        }
                       // added by neeraj to check file header or signature
                        BinaryReader b = new BinaryReader(m.agel_file.InputStream);
                        byte[] bindata = b.ReadBytes(m.agel_file.ContentLength);
                        string filecontent = System.Convert.ToBase64String(bindata);
                        if (filecontent.StartsWith("JVBER") == false)
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only Word/PDF/Excel/txt/mp4/mp3 format."));
                            return this.View(m);
                        }
                        //till here

                        //Added by neeraj yadav - save document to azureblob storage
                        //string uploadedfile_extension = Path.GetExtension(m.agel_file.FileName);
                        string uploadedfile_extension = Path.GetExtension(documentFileName);
                        //string uploadedfilewith_extension = Path.GetFileNameWithoutExtension(m.agel_file.FileName);
                        string uploadedfilewith_extension = Path.GetFileNameWithoutExtension(documentFileName);
                        //string uploadedfile_Name = m.agel_file.FileName;
                        string uploadedfile_Name = documentFileName;
                        string uploadedfilefileName = uploadedfilewith_extension.Replace(" ", "-") + "-" + DateTime.Now.ToString("yyyyMMddHHmmsss") + uploadedfile_extension;
                        var isUploaded = objazureBlobStorageServices.UploadFileToBlobBtyes(bindata, uploadedfilefileName, contentType);
                        Log.Info("Azure UploadFileToBlob response: " + isUploaded, this);
                        if (isUploaded.Status)
                        {
                            Log.Info("Inside If", this);
                            Log.Info("Azure UploadFileToBlob Response Status: " + isUploaded.Status, this);
                            Log.Info("Azure UploadFileToBlob Response Status Message: " + isUploaded.StatusMsg, this);
                            var isFileDeleted = objazureBlobStorageServices.DeleteFileBlob(Path.GetFileName(content.url));
                                if(isFileDeleted.Status)
                                {
                                    Log.Info("File deleted succesfully ", this);
                                    Log.Info("File deleted succesfully status" + isFileDeleted.Status, this);
                                    Log.Info("File deleted succesfully status message" + isFileDeleted.StatusMsg, this);
                                }
                                else
                                {                              
                                        Log.Info("File deleted failed ", this);
                                        Log.Info("File deleted failed status" + isFileDeleted.Status, this);
                                        Log.Info("File deleted failed status message" + isFileDeleted.StatusMsg, this);
                             
                                }
                            content.url = isUploaded.URL;
                        }
                        else
                        {
                            Log.Info("Inside Else", this);
                            Log.Info("Azure UploadFileToBlob Response Status: " + isUploaded.Status, this);
                            Log.Info("Azure UploadFileToBlob Response Status Message: " + isUploaded.StatusMsg, this);

                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", isUploaded.StatusMsg));
                            return this.View(m);
                        }


                        string urlpath = Server.MapPath(@"~\images\agelportal\pdf\");
                        //string extension = Path.GetExtension(m.agel_file.FileName).Substring(1);
                        string extension = Path.GetExtension(documentFileName).Substring(1);
                        string filename = content.Id + extension;
                        string sav = Path.Combine(urlpath, filename);
                        fileBrowsPath = "/images/agelportal/pdf/" + filename;
                        m.agel_file.SaveAs(sav);

                        if (bindata != null && bindata.Length > 0)
                        {
                            dt.document_data = bindata;
                            //fileBrowsPath = m.agel_file.FileName;
                            fileBrowsPath = documentFileName;
                        }
                        else
                        {
                            using (BinaryReader br = new BinaryReader(m.agel_file.InputStream))
                            {
                                bytes = br.ReadBytes(m.agel_file.ContentLength);
                                content.document_data = bytes;
                                //fileBrowsPath = m.agel_file.FileName;
                                fileBrowsPath = documentFileName;
                            }
                        }
                    }

                    if (m.imagefile != null && m.imagefile.ContentLength > 0)
                    {
                        //added by neeraj yadav for extension check
                        string[] allowedExtenstions = new string[] { ".png", ".jpg", ".jpeg" };
                        string extension1 = Path.GetExtension(m.imagefile.FileName);

                        if (!allowedExtenstions.Contains(extension1))
                        {
                            this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload only PNG/JPG/JPEG file format."));
                            return this.View(m);
                        }
                        //till here
                        //added by neeraj for mime check
                        string str = m.imagefile.FileName;
                        char ch = '.';
                        int occurance = str.Count(f => (f == ch));
                        if (occurance > 1)
                        {
                            this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload file with single extension"));
                            return this.View(m);
                        }
                        //till here
                        //added by neeraj for checking content type
                        string contentType = GetMimeTypeByWindowsRegistry(extension1);
                        if (contentType != "")
                        {
                            if (contentType != "image/png" && contentType != "image/jpeg")
                            {
                                this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only jpeg/jpg/png format."));
                                return this.View(m);
                            }
                        }
                        else
                        {
                            this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only jpeg/jpg/png format."));
                            return this.View(m);
                        }
                        //till here
                        int imagefileSize = 10 * 1024 * 1024;
                        var docSize = m.agel_file.ContentLength;
                        if (docSize > imagefileSize)
                        {
                            this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "document size should be less than 10 MB"));
                            return this.View(m);
                        }
                        //added by neeraj to check file header or signature
                        BinaryReader b = new BinaryReader(m.imagefile.InputStream);
                        byte[] bindata = b.ReadBytes(m.imagefile.ContentLength);
                        string filecontent = System.Convert.ToBase64String(bindata);
                        if (filecontent.StartsWith("iVBOR") == false && filecontent.StartsWith("/9j/4") == false)
                        {
                            this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only png/jpeg format."));
                            return this.View(m);
                        }
                        //till here

                       

                        string urlpath = Server.MapPath(@"~\images\agelportal\upload_images\");
                        string extension = Path.GetExtension(m.imagefile.FileName);
                        string filename = content.Id + extension;
                        // string fileName = m.imagefile.FileName;
                        string sav = Path.Combine(urlpath, filename);
                        content.DocVideoImage_Name = filename;
                        //content.url = filename;
                        rdb.SubmitChanges();
                        m.imagefile.SaveAs(sav);

                    }

                    if (m.agel_file != null && m.agel_file.ContentLength > 0 && m.content_type == "video")
                    {
                        //removing unwanted characters from video file name
                        var videoFileName = GetValidFileName(m.agel_file.FileName);

                        //added by neeraj yadav for extension check
                        string[] allowedExtenstions1 = new string[] { ".mp4", ".flv", ".wmv", ".avi", ".mp3", ".webm", ".mpg", ".mkv", ".mov" };

                        //string extension1 = Path.GetExtension(m.agel_file.FileName);
                        string extension1 = Path.GetExtension(videoFileName);

                        if (!allowedExtenstions1.Contains(extension1))
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload only /webm/mp4/mp3/mov/flv/avi format"));
                            return this.View(m);
                        }
                        //till here
                        //added by neeraj for mime check
                        string str = videoFileName;
                        //string str = m.agel_file.FileName;
                        char ch = '.';
                        int occurance = str.Count(f => (f == ch));
                        if (occurance > 1)
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload file with single extension"));
                            return this.View(m);
                        }
                        //till here
                        //added by neeraj for checking content type
                        string contentType = GetMimeTypeByWindowsRegistry(extension1);
                        if (contentType != "")
                        {
                            if (contentType != "text/plain" && contentType != "application/msword" && contentType != "application/pdf" && contentType != "application/vnd.ms-excel" && contentType != "video/mp4" && contentType != "video/x-flv" && contentType != "video/x-ms-wmv" && contentType != "video/x-msvideo" && contentType != "audio/mpeg" && contentType != "video/webm" && contentType != "video/mpeg" && contentType != "video/x-matroska" && contentType != "video/quicktime")
                            {
                                this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only Word/PDF/Excel/txt/mp4/mp3 format."));
                                return this.View(m);
                            }
                        }
                        else
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only Word/PDF/Excel/txt/mp4/mp3 format."));
                            return this.View(m);
                        }
                        //till here
                        int videoSizeMegaBite = 125 * 1024 * 1024;
                        var docSize = m.agel_file.ContentLength;
                        if (docSize > videoSizeMegaBite)
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "video size should be less than 125 MB"));
                            return this.View(m);
                        }
                        string videopath = RunAsync(m).Result;
                        fileBrowsPath = videopath;
                        content.url = fileBrowsPath;
                    }
                    content.category_id = new Guid(m.category_id);
                    content.contetn_type = m.content_type;
                    content.title = m.title;
                    content.DocVideoCnt = m.size;
                    content.discription = m.discription;
                    content.modify_date = DateTime.Now;
                    content.status = m.status == "1" ? true : false;
                 
                    Session["add_content"] = "Updated succesfully.";
                    rdb.SubmitChanges();
                }

                else
                {
                    byte[] bytes;
                    dt.Id = Guid.NewGuid();
                    Log.Info("Client call 6" + m.category_id, "");
                    dt.category_id = new Guid(m.category_id);
                    dt.contetn_type = m.content_type;
                    dt.title = m.title;
                    dt.discription = m.discription;
                    dt.DocVideoCnt = m.size;
                    dt.created_date = System.DateTime.Now;
                    dt.modify_date = System.DateTime.Now;
                    dt.status = m.status == "1" ? true : false;
                   
                    

                    ////Added by neeraj yadav, check whether new content is already exists in the database or not
                    //var DuplicateContent = rdb.AGELPortalContents.Where(x => x.discription.ToLower() == m.discription.ToLower() && x.title.ToLower() == m.title.ToLower()).FirstOrDefault();

                    //var DuplicateContentCategory = rdb.AGElPortalCategories.Where(x => x.Id == DuplicateContent.category_id).FirstOrDefault();

                    //if (contentcategory.name == DuplicateContentCategory.name)
                    //{

                    //    this.ModelState.AddModelError(nameof(m.title), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "This title is already taken. Please try another one."));
                    //    this.ModelState.AddModelError(nameof(m.discription), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "This description is already taken. Please try another one."));
                    //    return this.View(m);
                    //}
                    ////till here

                    if (m.agel_file != null && m.agel_file.ContentLength > 0 && m.content_type == "document")
                    {
                       
                        var documentFileName = GetValidFileName(m.agel_file.FileName);

                        Log.Info("Inside Add content document",this);
                        string[] allowedExtenstions1 = new string[] { ".txt", ".doc", ".docx", ".pdf", ".xls", ".xlsx", ".mp4", ".flv", ".wmv", ".avi", ".mp3", ".webm", ".mpg", ".mkv", ".mov" };

                        string extension1 = Path.GetExtension(documentFileName);

                        if (!allowedExtenstions1.Contains(extension1))
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only Word/PDF/Excel/txt/mp4/mp3 format."));
                            return this.View(m);
                        }

                        //added by neeraj to check file header or signature
                        BinaryReader b = new BinaryReader(m.agel_file.InputStream);
                        byte[] bindata = b.ReadBytes(m.agel_file.ContentLength);
                        string filecontent = System.Convert.ToBase64String(bindata);
                        if (filecontent.StartsWith("JVBER") == false)
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only Word/PDF/Excel/txt/mp4/mp3 format."));
                            return this.View(m);
                        }
                        ////till here

                        //added by neeraj for content length
                        int tenMegaBite = 10 * 1024 * 1024;
                        var docSize = m.agel_file.ContentLength;
                        if (docSize > tenMegaBite)
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "document size should be less than 10 MB"));
                            return this.View(m);
                        }
                        //till here

                        //added by neeraj for mime check
                        string str = documentFileName;
                        char ch = '.';
                        int occurance = str.Count(f => (f == ch));
                        if (occurance > 1)
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload file with single extension"));
                            return this.View(m);
                        }
                        //till here                        
                        //added by neeraj for checking content type
                        string contentType = GetMimeTypeByWindowsRegistry(extension1);
                        if (contentType != "")
                        {
                            if (contentType != "text/plain" && contentType != "application/msword" && contentType != "application/pdf" && contentType != "application/vnd.ms-excel" && contentType != "video/mp4" && contentType != "video/x-flv" && contentType != "video/x-ms-wmv" && contentType != "video/x-msvideo" && contentType != "audio/mpeg" && contentType != "video/webm" && contentType != "video/mpeg" && contentType != "video/x-matroska" && contentType != "video/quicktime")
                            {
                                this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only Word/PDF/Excel/txt/mp4/mp3 format."));
                                return this.View(m);
                            }
                        }
                        else
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only Word/PDF/Excel/txt/mp4/mp3 format."));
                            return this.View(m);
                        }
                        //till here
                        //using (BinaryReader br = new BinaryReader(m.agel_file.InputStream))
                        //{
                        //    bytes = br.ReadBytes(m.agel_file.ContentLength);
                        //    dt.document_data = bytes;
                        //    fileBrowsPath = m.agel_file.FileName;

                        //}

                        //added by mangesh late
                        if (bindata != null && bindata.Length > 0)
                        {
                            dt.document_data = bindata;
                            fileBrowsPath = documentFileName;
                        }
                        else
                        {
                            using (BinaryReader br = new BinaryReader(m.agel_file.InputStream))
                            {
                                bytes = br.ReadBytes(m.agel_file.ContentLength);
                                dt.document_data = bytes;
                                bindata = bytes;
                                fileBrowsPath = documentFileName;
                            }
                        }
                        string uploadedfile_extension = Path.GetExtension(documentFileName);
                        string uploadedfilewith_extension = Path.GetFileNameWithoutExtension(documentFileName);
                        //string uploadedfile_Name = m.agel_file.FileName;
                        string uploadedfile_Name = uploadedfilewith_extension.Replace(" ", "-") + "-" + DateTime.Now.ToString("yyyyMMddHHmmsss") + uploadedfile_extension;
                        //Added by neeraj yadav - save document to azureblob storage
                        var isUploaded = objazureBlobStorageServices.UploadFileToBlobBtyes(bindata, uploadedfile_Name,contentType);
                        Log.Info("Azure UploadFileToBlob response: " + isUploaded, this);
                        if (isUploaded.Status)
                        {
                            Log.Info("Inside If", this);
                            Log.Info("Azure UploadFileToBlob Response Status: " + isUploaded.Status, this);
                            Log.Info("Azure UploadFileToBlob Response Status Message: " + isUploaded.StatusMsg, this);
                            dt.url = isUploaded.URL;
                        }
                        else
                        {
                            Log.Info("Inside Else", this);
                            Log.Info("Azure UploadFileToBlob Response Status: " + isUploaded.Status, this);
                            Log.Info("Azure UploadFileToBlob Response Status Message: " + isUploaded.StatusMsg, this);

                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", isUploaded.StatusMsg));
                            return this.View(m);
                        }

                        if (m.imagefile != null && m.imagefile.ContentLength > 0)
                        {
                            int tenMegaBite1 = 10 * 1024 * 1024;
                            double docSize1 = m.imagefile.ContentLength;
                            if (docSize1 > tenMegaBite1)
                            {
                                this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "image size should be less than 10 MB"));
                                return this.View(m);
                            }

                            string[] allowedExtenstions = new string[] { ".png", ".jpg", ".jpeg" };
                            string extension = Path.GetExtension(m.imagefile.FileName);

                            if (!allowedExtenstions.Contains(extension))
                            {
                                this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload only PNG/JPG/JPEG file format."));
                                return this.View(m);
                            }


                            //added by neeraj for mime check
                            string str1 = m.imagefile.FileName;
                            char ch1 = '.';
                            int occurance1 = str1.Count(f => (f == ch1));
                            if (occurance1 > 1)
                            {
                                this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload file with single extension"));
                                return this.View(m);
                            }
                            //till here
                            //added by neeraj for checking content type
                            string contentType1 = GetMimeTypeByWindowsRegistry(extension);
                            if (contentType1 != "")
                            {
                                if (contentType1 != "image/png" && contentType1 != "image/jpeg")
                                {
                                    this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only png/jpg/jpeg format."));
                                    return this.View(m);
                                }
                            }
                            else
                            {
                                this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only png/jpg/jpeg format."));
                                return this.View(m);
                            }
                            //till here

                            //added by neeraj to check file header or signature
                            BinaryReader b1 = new BinaryReader(m.imagefile.InputStream);
                            byte[] bindata1 = b1.ReadBytes(m.imagefile.ContentLength);
                            string filecontent1 = System.Convert.ToBase64String(bindata1);
                            if (filecontent1.StartsWith("iVBOR") == false && filecontent1.StartsWith("/9j/4") == false)
                            {
                                this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only png/jpeg format."));
                                return this.View(m);
                            }
                            //till here

                            string urlpath = Server.MapPath(@"~\images\agelportal\upload_images\");
                            string FileName = Path.GetFileNameWithoutExtension(m.imagefile.FileName);
                            string extensionFile = Path.GetExtension(m.imagefile.FileName);

                            string fileName = m.imagefile.FileName;
                            fileName = fileName.Trim() + extensionFile;
                            string sav = Path.Combine(urlpath, fileName);
                            if (!System.IO.File.Exists(sav))
                            {
                                dt.DocVideoImage_Name = fileName;
                               // dt.url = fileName;
                                m.content = dt;
                                rdb.AGELPortalContents.InsertOnSubmit(dt);
                                rdb.SubmitChanges();

                                m.imagefile.SaveAs(sav);
                                m.imagepath = "~/images/agelportal/upload_images/" + fileName;

                                Session["add_content"] = "File added succesfully.";

                            }
                            else
                            {
                                m.content = dt;
                                this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "This file already exits."));
                                return this.View(m);

                            }

                        }

                        else
                        {
                           

                            string urlpath = Server.MapPath(@"~\images\agelportal\upload_images\");
                            string FileName = "document";
                            string extensionFile = ".jpg";
                            string fileName = "document.jpg";                            
                            fileName = fileName.Trim() + extensionFile;

                            dt.DocVideoImage_Name = "document.jpg";
                            //dt.url = fileName;
                            m.content = dt;
                            rdb.AGELPortalContents.InsertOnSubmit(dt);
                            rdb.SubmitChanges();
                            Session["add_content"] = "File added succesfully.";
                        }
                    }

                    if (m.agel_file != null && m.agel_file.ContentLength > 0 && m.content_type == "video")
                    {
                        //removing unwanted characters from video file name
                        var videoFileName = GetValidFileName(m.agel_file.FileName);

                        string[] allowedExtenstions1 = new string[] { ".mp4", ".flv", ".wmv", ".avi", ".mp3", ".webm", ".mpg", ".mkv", ".mov" };

                        string extension1 = Path.GetExtension(videoFileName);

                        if (!allowedExtenstions1.Contains(extension1))
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload only /webm/mp4/mp3/mov/flv/avi format"));
                            return this.View(m);
                        }
                        //added by neeraj for mime check
                        string str1 = videoFileName;
                        char ch1 = '.';
                        int occurance1 = str1.Count(f => (f == ch1));
                        if (occurance1 > 1)
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload file with single extension"));
                            return this.View(m);
                        }
                        //till here
                        //added by neeraj for checking content type
                        //string contentType = GetMimeTypeByWindowsRegistry(extension1);
                        //if (contentType != "")
                        //{
                        //    if (contentType != "text/plain" && contentType != "application/msword" && contentType != "application/pdf" && contentType != "application/vnd.ms-excel" && contentType != "video/mp4" && contentType != "video/x-flv" && contentType != "video/x-ms-wmv" && contentType != "video/x-msvideo" && contentType != "audio/mpeg" && contentType != "video/webm" && contentType != "video/mpeg" && contentType != "video/x-matroska" && contentType != "video/quicktime")
                        //    {
                        //        this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only Word/PDF/Excel/txt/mp4/mp3 format."));
                        //        return this.View(m);
                        //    }
                        //}
                        //else
                        //{
                        //    this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only Word/PDF/Excel/txt/mp4/mp3 format."));
                        //    return this.View(m);
                        //}
                        //till here
                        int videoSizeMegaBite = 125 * 1024 * 1024;
                        var docSize = m.agel_file.ContentLength;
                        if (docSize > videoSizeMegaBite)
                        {
                            this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "video size should be less than 125 MB"));
                            return this.View(m);
                        }
                        //BinaryReader b1 = new BinaryReader(m.agel_file.InputStream);
                        //byte[] bindata1 = b1.ReadBytes(m.agel_file.ContentLength);
                        //string filecontent1 = System.Convert.ToBase64String(bindata1);
                        //if (filecontent1.StartsWith("AAAA") == false)
                        //{
                        //    this.ModelState.AddModelError(nameof(m.agel_file), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only mp4 format."));
                        //    return this.View(m);
                        //}
                        //till here
                        string videopath = RunAsync(m).Result;
                        fileBrowsPath = videopath;
                        dt.url = fileBrowsPath;
                        Log.Info("Client call 4" + fileBrowsPath, "");

                        if (m.imagefile != null && m.imagefile.ContentLength > 0)
                        {
                            string[] allowedExtenstions = new string[] { ".png", ".jpg", ".jpeg" };
                            string extension2 = Path.GetExtension(m.imagefile.FileName);

                            if (!allowedExtenstions.Contains(extension2))
                            {
                                this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload only PNG/JPG/JPEG file format."));
                                return this.View(m);
                            }
                            //added by neeraj for mime check
                            string str = m.imagefile.FileName;
                            char ch = '.';
                            int occurance = str.Count(f => (f == ch));
                            if (occurance > 1)
                            {
                                this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload file with single extension"));
                                return this.View(m);
                            }
                            //till here
                            //added by neeraj for checking content type
                            string contentType1 = GetMimeTypeByWindowsRegistry(extension2);
                            if (contentType1 != "")
                            {
                                if (contentType1 != "image/png" && contentType1 != "image/jpeg")
                                {
                                    this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only png/jpg/jpeg format."));
                                    return this.View(m);
                                }
                            }
                            else
                            {
                                this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only png/jpg/jpeg format."));
                                return this.View(m);
                            }
                            //till here
                            int imageSize = 10 * 1024 * 1024;
                            var docSize1 = m.agel_file.ContentLength;
                            if (docSize1 > imageSize)
                            {
                                this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "image size should be less than 10 MB"));
                                return this.View(m);
                            }
                            //added by neeraj to check file header or signature
                            BinaryReader b = new BinaryReader(m.imagefile.InputStream);
                            byte[] bindata = b.ReadBytes(m.imagefile.ContentLength);
                            string filecontent = System.Convert.ToBase64String(bindata);
                            if (filecontent.StartsWith("iVBOR") == false && filecontent.StartsWith("/9j/4") == false)
                            {
                                this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "File type is invalid. Please upload in only png/jpeg format."));
                                return this.View(m);
                            }
                            //till here
                            string urlpath = Server.MapPath(@"~\images\agelportal\upload_images\");
                            string extension = Path.GetExtension(m.imagefile.FileName);
                            string fileName = m.imagefile.FileName;
                            //fileName = System.DateTime.Now.ToString("yymmssfff") + fileName;
                            fileName = fileName.Trim();

                            string sav = Path.Combine(urlpath, fileName);
                            if (!System.IO.File.Exists(sav))
                            {
                                dt.DocVideoImage_Name = fileName;
                              //  dt.url = fileBrowsPath;
                                m.content = dt;
                                rdb.AGELPortalContents.InsertOnSubmit(dt);
                                rdb.SubmitChanges();
                                m.imagefile.SaveAs(sav);
                                Session["add_content"] = "File added succesfully.";
                                url = "/AGELPortal/Home/dashboard/content-managements";

                            }
                            else
                            {
                                m.content = dt;
                                this.ModelState.AddModelError(nameof(m.imagefile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "This file already exits."));
                                return this.View(m);
                            }

                        }
                        else
                        {
                            dt.DocVideoImage_Name = "video.jpg";
                            //dt.url = "video.jpg";
                           
                            m.content = dt;
                            rdb.AGELPortalContents.InsertOnSubmit(dt);
                            rdb.SubmitChanges();
                            Session["add_content"] = "File added succesfully.";
                        }
                    }

                }
                url = "/AGELPortal/Home/dashboard/content-managements";
            }
            catch (Exception ex)
            {
                Log.Error("VideoUploadIssue = " + ex.ToString(), ex.ToString());
                Session["add_content"] = ex.ToString();
                return this.View(m);
            }
            return CustomRedirect(url);
        }

        //added by Neeraj Yadav, 30 August
        //To validate filename
        public static string GetValidFileName(string fileName)
        {
            // remove any invalid character from the filename.   
            
            string ret = Regex.Replace(fileName.Trim(), "[^A-Za-z0-9_. ]+", "");
            return ret.Replace(" ", "-");
        }



        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalContentManagements()
        {
            var usertype = Session["AGELPortalUserType"]?.ToString();

            if (Session["AGELPortalUserType"].ToString() != "superadmin")
            {
                return CustomRedirect("/AGELPortal/Home/login");
            }
            Uri myUri = new Uri(Request.Url.AbsoluteUri);
            var page = System.Convert.ToInt32(HttpUtility.ParseQueryString(myUri.Query).Get("page"));
            var pageNumber = page > 0 ? page : 1;
            var pageSize = 10;
            PortalAddContentModel contetnModel = new PortalAddContentModel();
            contetnModel.totalRecord = rdb.AGELPortalContents.Count();
            contetnModel.contents = rdb.AGELPortalContents.OrderByDescending(c => c.created_date).Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
            return this.View(contetnModel);
        }

        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult DeleteContent(PortalAddContentModel m)
        {
            var cat = rdb.AGELPortalContents.Where(x => x.Id == m.Id).FirstOrDefault();
            var LearnCat = rdb.AGELPortalLessonLearningVideos.Where(x => x.video_id == m.Id).ToList();

            if (LearnCat != null)
            {
                foreach (var itemDelete in LearnCat)
                {
                    rdb.AGELPortalLessonLearningVideos.DeleteOnSubmit(itemDelete);
                    rdb.SubmitChanges();
                }

            }
            if (cat != null)
            {
                //Added by neeraj yadav - delete document from azureblob storage               
                
                    var isFileDeleted = objazureBlobStorageServices.DeleteFileBlob(Path.GetFileName(cat.url));
                    if (isFileDeleted.Status)
                    {
                        Log.Info("File deleted succesfully ", this);
                        Log.Info("File deleted succesfully status" + isFileDeleted.Status, this);
                        Log.Info("File deleted succesfully status message" + isFileDeleted.StatusMsg, this);
                    }
                    else
                    {
                        Log.Info("File deleted failed ", this);
                        Log.Info("File deleted failed status" + isFileDeleted.Status, this);
                        Log.Info("File deleted failed status message" + isFileDeleted.StatusMsg, this);

                    }
                   
                

                rdb.AGELPortalContents.DeleteOnSubmit(cat);
                rdb.SubmitChanges();
                var result = new { status = "1" };
                Session["add_content"] = "Content deleted succesfully.";
                //return Json(result, JsonRequestBehavior.AllowGet);
                return Json(new { Status = "success", Message = "Succesfully updated" });


            }

            var result1 = new { status = "0" };


            return Json(result1, JsonRequestBehavior.AllowGet);
        }
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalUserChangePassword()
        {
            if (Session["AGELPortalUser"] == null)
            {
                return CustomRedirect("/AGELPortal/Home/login");
            }


            return View();
        }

        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalUserChangePassword(PortalLoginModel m)
        {

            var userId = new Guid(Session["AGELPortalUser"]?.ToString());
            var usertype = Session["AGELPortalUserType"]?.ToString();

            if (m.password != m.reset_password)
            {
                this.ModelState.AddModelError(nameof(m.reset_password), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Password does not match."));
                return this.View(m);
            }
            if (m.reset_password == "" || m.reset_password == null)
            {
                this.ModelState.AddModelError(nameof(m.reset_password), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Password is required."));
                return this.View(m);
            }

            var user = rdb.AGELPortalRegistrations.Where(x => x.Id == userId && x.user_type != "superadmin").FirstOrDefault();

            if (user != null)
            {
                user.password = m.reset_password;
                rdb.SubmitChanges();
                Session["changePassword"] = "Password changed successfully.";

                //send email for updated password                   
                SendEmail_UserUpdatedPassword(user.email, user.name, user.password);
                string sms_content = "Hi " + user.name;
                sms_content = ". Your Password is updated successfully.";
                sms_content = " New Password: " + user.password;
                //if (SendSMSUpdates(user.mobile, sms_content))
                //{
                //    Log.Info("sms sent", "");
                //}
                //else
                //{
                //    Log.Info("sms failed", "");
                //}
            }
            else
            {
                this.ModelState.AddModelError(nameof(m.reset_password), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Something went wrong."));
                return this.View(m);
            }

            return CustomRedirect("/AGELPortal/Home/login");
        }
        private void SendOtpEmail(string Email = null, string Name = null, string OTP = null, Guid Id = new Guid())
        {
            MailMessage mail = null;
            var getEmailTo = Email;
            try
            {
                var settingsItem = Context.Database.GetItem("{6D5F82A8-64AA-462B-A0E7-D6E279C635FC}");
                var mailTemplateItem = settingsItem;
                //From field
                var fromMail = mailTemplateItem.Fields["{8605948C-60FB-46B8-8AAA-4C52561B53BC}"];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }
                //body field
                var body = mailTemplateItem.Fields["{1519CCAD-ED26-4F60-82CA-22079AF44D16}"];
                // subject field
                var subject = mailTemplateItem.Fields["{0F45DF05-546F-462D-97C0-BA4FB2B02564}"];
                mail = new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
            }
            catch (Exception ex)
            {
                // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }

            try
            {
                mail.To.Add(getEmailTo);
                mail.Body = mail.Body.Replace("#Name#", Name);
                mail.Body = mail.Body.Replace("#OTP#", OTP);

                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
            }
        }
        private void UserWelcomeEmail(string Email = null, string Name = null, string password = null, Guid Id = new Guid())
        {
            MailMessage mail = null;
            var getEmailTo = Email;

            try
            {
                var settingsItem = Context.Database.GetItem("{051EDBB4-9B13-4318-A15F-7D5F3FCD0B66}");



                var mailTemplateItem = settingsItem;
                //From field
                var fromMail = mailTemplateItem.Fields["{8605948C-60FB-46B8-8AAA-4C52561B53BC}"];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }
                //body field
                var body = mailTemplateItem.Fields["{1519CCAD-ED26-4F60-82CA-22079AF44D16}"];
                // subject field
                var subject = mailTemplateItem.Fields["{0F45DF05-546F-462D-97C0-BA4FB2B02564}"];

                mail = new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
            }
            catch (Exception ex)
            {
                // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }

            try
            {
                mail.To.Add(getEmailTo);
                mail.Body = mail.Body.Replace("#Name#", Name);
                mail.Body = mail.Body.Replace("#password#", password);

                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
            }
        }

        private void SendEmail_UserUpdatedPassword(string Email = null, string Name = null, string password = null, Guid Id = new Guid())
        {
            MailMessage mail = null;
            var getEmailTo = Email;

            try
            {
                var settingsItem = Context.Database.GetItem("{38C7DAE5-2565-4A5E-8A3E-1CB88B89EA48}");
                var mailTemplateItem = settingsItem;
                //From field
                var fromMail = mailTemplateItem.Fields["{8605948C-60FB-46B8-8AAA-4C52561B53BC}"];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }
                //body field
                var body = mailTemplateItem.Fields["{1519CCAD-ED26-4F60-82CA-22079AF44D16}"];
                // subject field
                var subject = mailTemplateItem.Fields["{0F45DF05-546F-462D-97C0-BA4FB2B02564}"];

                mail = new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
            }
            catch (Exception ex)
            {
                // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }

            try
            {
                mail.To.Add(getEmailTo);
                mail.Body = mail.Body.Replace("#Name#", Name);
                mail.Body = mail.Body.Replace("#password#", password);

                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
            }
        }


        private void SendEmail_NewUserAdded(string Email = null, string Name = null, Guid Id = new Guid())
        {
            MailMessage mail = null;
            var getEmailTo = Email;
            try
            {
                var settingsItem = Context.Database.GetItem("{C4D055CB-1DE7-4098-80D1-7EFF3FE9AB02}");
                var mailTemplateItem = settingsItem;
                //From field
                var fromMail = mailTemplateItem.Fields["{8605948C-60FB-46B8-8AAA-4C52561B53BC}"];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }
                //body field
                var body = mailTemplateItem.Fields["{1519CCAD-ED26-4F60-82CA-22079AF44D16}"];
                // subject field
                var subject = mailTemplateItem.Fields["{0F45DF05-546F-462D-97C0-BA4FB2B02564}"];

                mail = new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
            }
            catch (Exception ex)
            {
                // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }

            try
            {
                mail.To.Add(getEmailTo);
                mail.Body = mail.Body.Replace("#Name#", Name);
                mail.Body = mail.Body.Replace("#Email#", Email);


                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
            }
        }


        private void SendEmail_UserAccountStatus(string Email = null, string Name = null, bool status = true, Guid Id = new Guid())
        {
            MailMessage mail = null;
            var getEmailTo = Email;
            var profileStatus = "";
            if (status)
            {
                profileStatus = "Activated";
            }
            else
            {
                profileStatus = "Deactivated";
            }

            try
            {
                var settingsItem = Context.Database.GetItem("{78A5E46B-20D3-4B00-BB42-0C6B0A84A139}");
                var mailTemplateItem = settingsItem;
                //From field
                var fromMail = mailTemplateItem.Fields["{8605948C-60FB-46B8-8AAA-4C52561B53BC}"];

                if (string.IsNullOrEmpty(fromMail.Value))
                {
                    throw new InvalidValueException("'From' field in mail template should be set");
                }
                //body field
                var body = mailTemplateItem.Fields["{1519CCAD-ED26-4F60-82CA-22079AF44D16}"];
                // subject field
                var subject = mailTemplateItem.Fields["{0F45DF05-546F-462D-97C0-BA4FB2B02564}"];

                mail = new MailMessage
                {
                    From = new MailAddress(fromMail.Value),
                    Body = body.Value,
                    Subject = subject.Value,
                    IsBodyHtml = true
                };
            }
            catch (Exception ex)
            {
                // result = new { status = "1" };
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }

            try
            {
                mail.To.Add(getEmailTo);
                mail.Body = mail.Body.Replace("#Name#", Name);
                mail.Body = mail.Body.Replace("#Email#", Email);
                mail.Body = mail.Body.Replace("#status#", profileStatus);


                MainUtil.SendMail(mail);
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + getEmailTo + " - Error - " + ex.Message + "", ex, this);
            }
        }
        //public dynamic SendOtpForValidation(PortalRegistrationModel model)
        //{
        //    Sitecore.Diagnostics.Log.Info(String.Format("1. SendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}", model.name, model.email, model.OTP), this);
        //    Log.Info("1. SendOtpForValidation:" + model.mobile, this);
        //    var result = false;

        //    AGElPortalOtpHistory OtpHistoryObj = new AGElPortalOtpHistory();
        //    try
        //    {
        //        Log.Info("2. SendOtpForValidation:" + model.mobile, this);
        //        var otpFor = model.email != null ? model.email : model.mobile;

        //        #region Delete Available otp from database for given mobile number

        //        //realtyRepo.DeleteOldOtp(otpFor);
        //        #endregion
        //        Sitecore.Diagnostics.Log.Info(String.Format("2. delete sendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}", model.name, model.email, model.OTP), this);
        //        #region Generate New Otp for given mobile number and save to database
        //        string generatedotp = realtyRepo.StoreGeneratedOtp(model);
        //        Log.Info("3. SendOtpForValidation generatedotp:" + generatedotp, this);
        //        #endregion
        //        #region Api call to send SMS of OTP
        //        try
        //        {
        //            MessageList message1 = new MessageList
        //            {
        //                key = "OTP",
        //                value = generatedotp
        //            };
        //            MessageList message2 = new MessageList
        //            {
        //                key = "PassengerName",
        //                value = model.name
        //            };

        //            List<MessageList> messageList = new List<MessageList>();
        //            messageList.Add(message1);
        //            messageList.Add(message2);


        //            OTPRequest request1 = new OTPRequest
        //            {
        //                mobileNo = model.mobile,
        //                messageList = messageList
        //            };
        //            Log.Info("4. SendOtpForValidation request start:", this);

        //            var serializedObj = JsonConvert.SerializeObject(request1);
        //            Sitecore.Diagnostics.Log.Info(String.Format("3. serializedobject SendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, serializedObj:{3}", model.name, model.email, model.OTP, System.Convert.ToString(serializedObj)), this);
        //            var client = new RestClient("https://dev2.adanidigitallabs.com/SMSDispatcher/Api/sms/send");
        //            client.Timeout = -1;
        //            var request = new RestRequest(Method.POST);
        //            request.AddHeader("ApiKey", "apikey12345");
        //            request.AddHeader("Content-Type", "application/json");
        //            request.AddParameter("application/json", serializedObj, ParameterType.RequestBody);
        //            IRestResponse response = client.Execute(request);
        //            Log.Info("4. SendOtpForValidation response:" + response.Content, this);
        //            if (response.IsSuccessful && response.Content != null)
        //            {
        //                Sitecore.Diagnostics.Log.Info(String.Format("4. response SendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, serializedObj:{3}", model.name, model.email, model.OTP, System.Convert.ToString(serializedObj)), this);

        //                OTPDeserialize jsonObject = JsonConvert.DeserializeObject<OTPDeserialize>(response.Content);

        //                Sitecore.Diagnostics.Log.Info(String.Format("5. jsonObject SendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, jsonObject:{3}", model.name, model.email, model.OTP, System.Convert.ToString(jsonObject)), this);

        //                OTPData propertyData = new OTPData();
        //                //SendOtpEmail(model.email, model.name, propertyData.otp, model.Id);
        //                propertyData = jsonObject.data;
        //                Log.Info($" Adanidigitallabs Api_sms_send response:  '{response.Content}'.", "");
        //                //OtpHistoryObj.otp_for = model.mobile;
        //                //OtpHistoryObj.otp = generatedotp;
        //                //OtpHistoryObj.status = propertyData.isSendOtp;
        //                //OtpHistoryObj.date = DateTime.Now;
        //                //rdb.AGElPortalOtpHistories.InsertOnSubmit(OtpHistoryObj);
        //                //rdb.SubmitChanges();
        //                result = true;
        //            }
        //            else
        //            {
        //                string responseCode = "response " + response.ResponseStatus;
        //                Log.Info($" Adanidigitallabs Api_sms_send responseCode: '{response.ResponseStatus}'.", "");
        //                result = false;
        //            }
        //        }
        //        catch (Exception ex)
        //        {
        //            Log.Error($"{0}", ex, this);
        //            Log.Error("SendOtpForValidation Log Error Exception:" + ex.Message, this);
        //            result = false;
        //        }
        //        #endregion

        //        #region Return Response with Mobile Number and Generated otp
        //        //result = new { status = "1" };
        //        return result;



        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"{0}", ex, this);
        //        return result;
        //    }

        //}

        //added by neeraj yadav, merging new sms api changes
        public dynamic SendOtpForValidation(PortalRegistrationModel model)
        {
            Sitecore.Diagnostics.Log.Info(String.Format("1. SendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}", model.name, model.email, model.OTP), this);
            Log.Info("1. SendOtpForValidation:" + model.mobile, this);
            var result = false;
            StringBuilder strresult = new StringBuilder();

            AGElPortalOtpHistory OtpHistoryObj = new AGElPortalOtpHistory();
            try
            {
                Log.Info("2. SendOtpForValidation:" + model.mobile, this);
                var otpFor = model.email != null ? model.email : model.mobile;

                #region Delete Available otp from database for given mobile number

                //realtyRepo.DeleteOldOtp(otpFor);
                #endregion
                Sitecore.Diagnostics.Log.Info(String.Format("2. delete sendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}", model.name, model.email, model.OTP), this);
                #region Generate New Otp for given mobile number and save to database
                string generatedotp = realtyRepo.StoreGeneratedOtp(model);
                Log.Info("3. SendOtpForValidation generatedotp:" + generatedotp, this);
                #endregion
                #region Api call to send SMS of OTP
                try
                {
                    MessageList message1 = new MessageList
                    {
                        key = "OTP",
                        value = generatedotp
                    };
                    //MessageList message2 = new MessageList
                    //{
                    //    key = "PassengerName",
                    //    value = model.name
                    //};

                    List<MessageList> messageList = new List<MessageList>();
                    messageList.Add(message1);
                    // messageList.Add(message2);


                    OTPRequest request1 = new OTPRequest
                    {
                        mobileNo = model.mobile,
                        data = messageList
                    };
                    Log.Info("4. SendOtpForValidation request start:", this);

                    var serializedObj = JsonConvert.SerializeObject(request1);
                    strresult.Append("serializedObj:" + serializedObj);
                    Sitecore.Diagnostics.Log.Info(String.Format("3. serializedobject SendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, serializedObj:{3}", model.name, model.email, model.OTP, System.Convert.ToString(serializedObj)), this);

                    var smsapiurl = System.Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AGELSMSAPIURL"]);
                    var smsapikey = System.Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AGELSMSAPIKEY"]);
                    var smsapivalue = "degf@2457@!$#(&&$%^$2342";
                    string rawData = string.Format("{0} ~ {1}", smsapivalue, model.mobile);
                    //calling function to compute hash value
                    var authorValue = HashString(rawData, "~6nqej2");

                    //var client = new RestClient("https://dev2.adanidigitallabs.com/SMSDispatcher/Api/sms/send");
                    var client = new RestClient(smsapiurl);
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Author", authorValue);
                    strresult.AppendLine("Author:" + authorValue);
                    //request.AddHeader(smsapikey,smsapivalue);
                    // request.AddHeader("ApiKey", "apikey12345");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", serializedObj, ParameterType.RequestBody);
                    IRestResponse response = client.Execute(request);
                    Log.Info("4. SendOtpForValidation response:" + response.Content, this);
                    if (response.IsSuccessful && response.Content != null)
                    {
                        Sitecore.Diagnostics.Log.Info(String.Format("4. response SendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, serializedObj:{3}", model.name, model.email, model.OTP, System.Convert.ToString(serializedObj)), this);

                        OTPDeserialize jsonObject = JsonConvert.DeserializeObject<OTPDeserialize>(response.Content);

                        Sitecore.Diagnostics.Log.Info(String.Format("5. jsonObject SendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, jsonObject:{3}", model.name, model.email, model.OTP, System.Convert.ToString(jsonObject)), this);

                        OTPData propertyData = new OTPData();
                        //SendOtpEmail(model.email, model.name, propertyData.otp, model.Id);
                        propertyData = jsonObject.data;
                        strresult.AppendLine($" Adanidigitallabs Api_sms_send responseCode: '{response.ResponseStatus}'.");
                        Log.Info($" Adanidigitallabs Api_sms_send response:  '{response.Content}'.", "");
                        //OtpHistoryObj.otp_for = model.mobile;
                        //OtpHistoryObj.otp = generatedotp;
                        //OtpHistoryObj.status = propertyData.isSendOtp;
                        //OtpHistoryObj.date = DateTime.Now;
                        //rdb.AGElPortalOtpHistories.InsertOnSubmit(OtpHistoryObj);
                        //rdb.SubmitChanges();
                        result = true;
                    }
                    else
                    {
                        string responseCode = "response " + response.ResponseStatus;
                        strresult.AppendLine($" Adanidigitallabs Api_sms_send responseCode: '{response.ResponseStatus}'.");
                        strresult.AppendLine($" Adanidigitallabs Api_sms_send IsSuccessful: '{response.IsSuccessful}'.");
                        strresult.AppendLine($" Adanidigitallabs Api_sms_send Content: '{response.Content}'.");
                        Log.Info($" Adanidigitallabs Api_sms_send responseCode: '{response.ResponseStatus}'.", "");
                        result = false;

                    }
                }
                catch (Exception ex)
                {
                    Log.Error($"{0}", ex, this);
                    Log.Error("SendOtpForValidation Log Error Exception:" + ex.Message, this);
                    throw ex;
                    //result = false;
                }
                #endregion

                #region Return Response with Mobile Number and Generated otp
                //result = new { status = "1" };

                if (model.islog)
                {
                    return System.Convert.ToString(strresult);
                }
                return result;



                #endregion
            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
                throw ex;
                // return result;
            }

        }


        //[HttpPost]
        //public dynamic AGELSendOtpForValidation(PortalRegistrationModel model)
        //{
        //    Sitecore.Diagnostics.Log.Info(String.Format("1. AGELSendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}", model.name, model.email, model.OTP), this);
        //    var result = new { status = "0" };

        //    AGElPortalOtpHistory OtpHistoryObj = new AGElPortalOtpHistory();
        //    try
        //    {
        //        var otpFor = model.email != null ? model.email : model.mobile;

        //        #region Delete Available otp from database for given mobile number

        //        // realtyRepo.DeleteOldOtp(otpFor);
        //        #endregion
        //        Sitecore.Diagnostics.Log.Info(String.Format("2. delete AGELSendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}", model.name, model.email, model.OTP), this);
        //        #region Generate New Otp for given mobile number and save to database
        //        string generatedotp = realtyRepo.StoreGeneratedOtp(model);
        //        #endregion
        //        #region Api call to send SMS of OTP
        //        try
        //        {
        //            MessageList message1 = new MessageList
        //            {
        //                key = "OTP",
        //                value = generatedotp
        //            };
        //            MessageList message2 = new MessageList
        //            {
        //                key = "PassengerName",
        //                value = model.name
        //            };

        //            List<MessageList> messageList = new List<MessageList>();
        //            messageList.Add(message1);
        //            messageList.Add(message2);


        //            OTPRequest request1 = new OTPRequest
        //            {
        //                mobileNo = model.mobile,
        //                messageList = messageList
        //            };

        //            var serializedObj = JsonConvert.SerializeObject(request1);
        //            var client = new RestClient("https://dev2.adanidigitallabs.com/SMSDispatcher/Api/sms/send");
        //            client.Timeout = -1;
        //            var request = new RestRequest(Method.POST);
        //            request.AddHeader("ApiKey", "apikey12345");
        //            request.AddHeader("Content-Type", "application/json");
        //            request.AddParameter("application/json", serializedObj, ParameterType.RequestBody);
        //            Sitecore.Diagnostics.Log.Info(String.Format("3. before execution AGELSendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, serializedObj: {3}", model.name, model.email, model.OTP, System.Convert.ToString(serializedObj)), this);
        //            IRestResponse response = client.Execute(request);

        //            if (response.IsSuccessful && response.Content != null)
        //            {
        //                Sitecore.Diagnostics.Log.Info(String.Format("4. after  execution success AGELSendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, serializedObj: {3}", model.name, model.email, model.OTP, System.Convert.ToString(serializedObj)), this);
        //                OTPDeserialize jsonObject = JsonConvert.DeserializeObject<OTPDeserialize>(response.Content);
        //                Sitecore.Diagnostics.Log.Info(String.Format("5. after execution AGELSendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, jsonObject: {3}", model.name, model.email, model.OTP, System.Convert.ToString(jsonObject)), this);
        //                OTPData propertyData = new OTPData();
        //                SendOtpEmail(model.email, model.name, propertyData.otp, model.Id);
        //                propertyData = jsonObject.data;
        //                Log.Info($" Adanidigitallabs Api_sms_send response:  '{response.Content}'.", "");
        //                OtpHistoryObj.otp_for = model.mobile;
        //                OtpHistoryObj.otp = generatedotp;
        //                OtpHistoryObj.status = propertyData.isSendOtp;
        //                OtpHistoryObj.date = DateTime.Now;
        //                rdb.AGElPortalOtpHistories.InsertOnSubmit(OtpHistoryObj);
        //                rdb.SubmitChanges();
        //                result = new { status = "1" };
        //            }
        //            else
        //            {
        //                string responseCode = "response " + response.ResponseStatus;
        //                Log.Info($" Adanidigitallabs Api_sms_send responseCode: '{response.ResponseStatus}'.", "");
        //                result = new { status = "0" };
        //            }
        //        }
        //        catch (Exception ex)
        //        {

        //            Log.Error($"{0}", ex, this);
        //            Sitecore.Diagnostics.Log.Error(String.Format("6. Error AGELSendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, errorMessage: {3}, errorInnerException : {4}, errorStakeTrace : {5}", model.name, model.email, model.OTP, System.Convert.ToString(ex.Message), System.Convert.ToString(ex.InnerException), System.Convert.ToString(ex.StackTrace)), this);
        //            result = new { status = "0" };
        //        }
        //        #endregion

        //        #region Return Response with Mobile Number and Generated otp
        //        //result = new { status = "1" };
        //        return Json(result, JsonRequestBehavior.AllowGet);



        //        #endregion
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"{0}", ex, this);
        //        return Json(result, JsonRequestBehavior.AllowGet);
        //    }

        //}

        //added by neeraj yadav, merging new sms api code
        [HttpPost]
        public dynamic AGELSendOtpForValidation(PortalRegistrationModel model)
        {
            Sitecore.Diagnostics.Log.Info(String.Format("1. AGELSendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}", model.name, model.email, model.OTP), this);
            var result = new { status = "0" };

            AGElPortalOtpHistory OtpHistoryObj = new AGElPortalOtpHistory();
            try
            {
                var otpFor = model.email != null ? model.email : model.mobile;

                #region Delete Available otp from database for given mobile number

                // realtyRepo.DeleteOldOtp(otpFor);
                #endregion
                Sitecore.Diagnostics.Log.Info(String.Format("2. delete AGELSendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}", model.name, model.email, model.OTP), this);
                #region Generate New Otp for given mobile number and save to database
                string generatedotp = realtyRepo.StoreGeneratedOtp(model);
                #endregion
                #region Api call to send SMS of OTP
                try
                {
                    MessageList message1 = new MessageList
                    {
                        key = "OTP",
                        value = generatedotp
                    };
                    MessageList message2 = new MessageList
                    {
                        key = "PassengerName",
                        value = model.name
                    };

                    List<MessageList> messageList = new List<MessageList>();
                    messageList.Add(message1);
                    messageList.Add(message2);


                    OTPRequest request1 = new OTPRequest
                    {
                        mobileNo = model.mobile,
                        data = messageList
                    };

                    var serializedObj = JsonConvert.SerializeObject(request1);
                    var smsapiurl = System.Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AGELSMSAPIURL"]);
                    var smsapikey = System.Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AGELSMSAPIKEY"]);
                    var smsapivalue = "degf@2457@!$#(&&$%^$2342";
                    string rawData = string.Format("{0} ~ {1}", smsapivalue, model.mobile);
                    //calling function to compute hash value
                    var authorValue = HashString(rawData, "~6nqej2");

                    var client = new RestClient(smsapiurl);
                    client.Timeout = -1;
                    // var request = new RestRequest(Method.POST);

                    // var client = new RestClient("https://dev2.adanidigitallabs.com/SMSDispatcher/Api/sms/send");
                    client.Timeout = -1;
                    var request = new RestRequest(Method.POST);
                    request.AddHeader("Author", authorValue);
                    request.AddHeader(smsapikey, smsapivalue);
                    // request.AddHeader("ApiKey", "apikey12345");
                    request.AddHeader("Content-Type", "application/json");
                    request.AddParameter("application/json", serializedObj, ParameterType.RequestBody);
                    Sitecore.Diagnostics.Log.Info(String.Format("3. before execution AGELSendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, serializedObj: {3}", model.name, model.email, model.OTP, System.Convert.ToString(serializedObj)), this);
                    IRestResponse response = client.Execute(request);

                    if (response.IsSuccessful && response.Content != null)
                    {
                        Sitecore.Diagnostics.Log.Info(String.Format("4. after  execution success AGELSendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, serializedObj: {3}", model.name, model.email, model.OTP, System.Convert.ToString(serializedObj)), this);
                        OTPDeserialize jsonObject = JsonConvert.DeserializeObject<OTPDeserialize>(response.Content);
                        Sitecore.Diagnostics.Log.Info(String.Format("5. after execution AGELSendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, jsonObject: {3}", model.name, model.email, model.OTP, System.Convert.ToString(jsonObject)), this);
                        OTPData propertyData = new OTPData();
                        SendOtpEmail(model.email, model.name, propertyData.otp, model.Id);
                        propertyData = jsonObject.data;
                        Log.Info($" Adanidigitallabs Api_sms_send response:  '{response.Content}'.", "");
                        OtpHistoryObj.otp_for = model.mobile;
                        OtpHistoryObj.otp = generatedotp;
                        OtpHistoryObj.status = propertyData.isSendOtp;
                        OtpHistoryObj.date = DateTime.Now;
                        rdb.AGElPortalOtpHistories.InsertOnSubmit(OtpHistoryObj);
                        rdb.SubmitChanges();
                        result = new { status = "1" };
                    }
                    else
                    {
                        string responseCode = "response " + response.ResponseStatus;
                        Log.Info($" Adanidigitallabs Api_sms_send responseCode: '{response.ResponseStatus}'.", "");
                        result = new { status = "0" };
                    }
                }
                catch (Exception ex)
                {

                    Log.Error($"{0}", ex, this);
                    Sitecore.Diagnostics.Log.Error(String.Format("6. Error AGELSendOtpForValidation: Name: {0}, Email:{1}, OTP: {2}, errorMessage: {3}, errorInnerException : {4}, errorStakeTrace : {5}", model.name, model.email, model.OTP, System.Convert.ToString(ex.Message), System.Convert.ToString(ex.InnerException), System.Convert.ToString(ex.StackTrace)), this);
                    result = new { status = "0" };
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


        private const string AdaptiveStreamingTransformName = "MyTransformWithAdaptiveStreamingPreset";
        private const string InputMP4FileName = @"ignite.mp4";
        private const string OutputFolderName = @"Output";
        public static async Task<IAzureMediaServicesClient> CreateMediaServicesClientAsync(PortalAddContentModel config, bool interactive = false)
        {
            ServiceClientCredentials credentials;

            credentials = GetCredentialsAsync(config);
            return new AzureMediaServicesClient(config.ArmEndpoint, credentials)
            {
                SubscriptionId = config.SubscriptionId,
            };
        }
        // </CreateMediaServicesClientAsync>

        /// <summary>
        /// Create the ServiceClientCredentials object based on the credentials
        /// supplied in local configuration file.
        /// </summary>
        /// <param name="config">The param is of type ConfigWrapper. This class reads values from local configuration file.</param>
        /// <returns></returns>
        // <GetCredentialsAsync>
        public static ServiceClientCredentials GetCredentialsAsync(PortalAddContentModel config)
        {
            // Use ConfidentialClientApplicationBuilder.AcquireTokenForClient to get a token using a service principal with symmetric key

            var scopes = new[] { config.ArmAadAudience + "/.default" };

            var app = ConfidentialClientApplicationBuilder.Create(config.AadClientId)
                .WithClientSecret(config.AadSecret)
                .WithAuthority(AzureCloudInstance.AzurePublic, config.AadTenantId)
                .Build();

            var authResult = app.AcquireTokenForClient(scopes)
                                                     .ExecuteAsync().Result;

            return new TokenCredentials(authResult.AccessToken, "Bearer");
        }

        private static Transform GetOrCreateTransformAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName)
        {
            bool createTransform = false;
            Transform transform = null;
            try
            {
                // Does a transform already exist with the desired name? Assume that an existing Transform with the desired name
                // also uses the same recipe or Preset for processing content.
                transform = client.Transforms.Get(resourceGroupName, accountName, transformName);
            }
            catch (ErrorResponseException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                createTransform = true;
            }

            if (createTransform)
            {
                // You need to specify what you want it to produce as an output
                TransformOutput[] output = new TransformOutput[]
                {
                    new TransformOutput
                    {
                        // The preset for the Transform is set to one of Media Services built-in sample presets.
                        // You can  customize the encoding settings by changing this to use "StandardEncoderPreset" class.
                        Preset = new BuiltInStandardEncoderPreset()
                        {
                            // This sample uses the built-in encoding preset for Adaptive Bitrate Streaming.
                            PresetName = EncoderNamedPreset.AdaptiveStreaming
                        }
                    }
                };

                // Create the Transform with the output defined above
                transform = client.Transforms.CreateOrUpdateAsync(resourceGroupName, accountName, transformName, output).Result;
            }

            return transform;
        }
        // </EnsureTransformExists>
        private static async Task<string> RunAsync(PortalAddContentModel config)
        {
            IAzureMediaServicesClient client;
            try
            {
                client = await CreateMediaServicesClientAsync(config, false);
            }
            catch (Exception e)
            {
                Log.Error("TIP: Make sure that you have filled out the appsettings.json file before running this sample.", e.Message);
                Log.Error($"{e.Message}", e.Message);
                return "";
            }

            // Set the polling interval for long running operations to 2 seconds.
            // The default value is 30 seconds for the .NET client SDK
            client.LongRunningOperationRetryTimeout = 2;

            // Creating a unique suffix so that we don't have name collisions if you run the sample
            // multiple times without cleaning up.
            string uniqueness = Guid.NewGuid().ToString("N");
            string jobName = $"job-{uniqueness}";
            string locatorName = $"locator-{uniqueness}";
            string outputAssetName = $"output-{uniqueness}";
            string inputAssetName = $"input-{uniqueness}";
            try
            {
                Log.Info("Client call 1" + client, "");
                // Ensure that you have the desired encoding Transform. This is really a one time setup operation.
                GetOrCreateTransformAsync(client, config.ResourceGroup, config.AccountName, AdaptiveStreamingTransformName);
                // Create a new input Asset and upload the specified local video file into it.
                CreateInputAssetAsync(client, config.ResourceGroup, config.AccountName, inputAssetName, config.agel_file.FileName, config.agel_file.InputStream);

                // Use the name of the created input asset to create the job input.
                new JobInputAsset(assetName: inputAssetName);
                // Output from the encoding Job must be written to an Asset, so let's create one
                Asset outputAsset = CreateOutputAssetAsync(client, config.ResourceGroup, config.AccountName, outputAssetName);
                SubmitJobAsync(client, config.ResourceGroup, config.AccountName, AdaptiveStreamingTransformName, jobName, inputAssetName, outputAsset.Name);
                // In this demo code, we will poll for Job status
                // Polling is not a recommended best practice for production applications because of the latency it introduces.
                // Overuse of this API may trigger throttling. Developers should instead use Event Grid.
                Job job = WaitForJobToFinishAsync(client, config.ResourceGroup, config.AccountName, AdaptiveStreamingTransformName, jobName);
                Log.Info("Client call 2" + job.State, "");
                if (job.State == JobState.Finished)
                {
                    //Console.WriteLine("Job finished.");
                    //if (!Directory.Exists(OutputFolderName))
                    //    Directory.CreateDirectory(OutputFolderName);

                    // DownloadOutputAssetAsync(client, config.ResourceGroup, config.AccountName, outputAsset.Name, OutputFolderName);

                    StreamingLocator locator = CreateStreamingLocatorAsync(client, config.ResourceGroup, config.AccountName, outputAsset.Name, locatorName);

                    // Note that the URLs returned by this method include a /manifest path followed by a (format=)
                    // parameter that controls the type of manifest that is returned. 
                    // The /manifest(format=m3u8-aapl) will provide Apple HLS v4 manifest using MPEG TS segments.
                    // The /manifest(format=mpd-time-csf) will provide MPEG DASH manifest.
                    // And using just /manifest alone will return Microsoft Smooth Streaming format.
                    // There are additional formats available that are not returned in this call, please check the documentation
                    // on the dynamic packager for additional formats - see https://docs.microsoft.com/azure/media-services/latest/dynamic-packaging-overview
                    IList<string> urls = GetStreamingUrlsAsync(client, config.ResourceGroup, config.AccountName, locator.Name);
                    config.AzureURLs = urls;
                    Log.Info("Client call 3" + urls.Last(), "");
                    return urls.Last();

                }
                else
                    return "";
            }
            catch (Exception ex)
            {
                Log.Error("Client call Exception Upload " + ex.ToString(), string.Empty);
                return "";
            }


            //Console.WriteLine("Done. Copy and paste the Streaming URL ending in '/manifest' into the Azure Media Player at 'http://aka.ms/azuremediaplayer'.");
            //Console.WriteLine("See the documentation on Dynamic Packaging for additional format support, including CMAF.");
            //Console.WriteLine("https://docs.microsoft.com/azure/media-services/latest/dynamic-packaging-overview");
        }
        // <CreateStreamingLocator>
        private static StreamingLocator CreateStreamingLocatorAsync(
                    IAzureMediaServicesClient client,
                    string resourceGroup,
                    string accountName,
                    string assetName,
                    string locatorName)
        {
            StreamingLocator locator = client.StreamingLocators.CreateAsync(
                resourceGroup,
                accountName,
                locatorName,
                new StreamingLocator
                {
                    AssetName = assetName,
                    StreamingPolicyName = PredefinedStreamingPolicy.ClearStreamingOnly
                }).Result;

            return locator;
        }
        // </CreateStreamingLocator>

        /// <summary>
        /// Checks if the "default" streaming endpoint is in the running state,
        /// if not, starts it.
        /// Then, builds the streaming URLs.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="locatorName">The name of the StreamingLocator that was created.</param>
        /// <returns></returns>
        // <GetStreamingURLs>
        private static IList<string> GetStreamingUrlsAsync(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            String locatorName)
        {
            const string DefaultStreamingEndpointName = "default";

            IList<string> streamingUrls = new List<string>();

            StreamingEndpoint streamingEndpoint = client.StreamingEndpoints.GetAsync(resourceGroupName, accountName, DefaultStreamingEndpointName).Result;

            if (streamingEndpoint.ResourceState != StreamingEndpointResourceState.Running)
            {
                client.StreamingEndpoints.StartAsync(resourceGroupName, accountName, DefaultStreamingEndpointName);
            }

            ListPathsResponse paths = client.StreamingLocators.ListPathsAsync(resourceGroupName, accountName, locatorName).Result;

            foreach (StreamingPath path in paths.StreamingPaths)
            {
                UriBuilder uriBuilder = new UriBuilder
                {
                    Scheme = "https",
                    Host = streamingEndpoint.HostName,

                    Path = path.Paths[0]
                };
                streamingUrls.Add(uriBuilder.ToString());
            }
            return streamingUrls;
        }
        // </GetStreamingURLs>

        /// <summary>
        ///  Downloads the results from the specified output asset, so you can see what you got.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="assetName">The output asset.</param>
        /// <param name="outputFolderName">The name of the folder into which to download the results.</param>
        // <DownloadResults>
        private static void DownloadOutputAssetAsync(
            IAzureMediaServicesClient client,
            string resourceGroup,
            string accountName,
            string assetName,
            string outputFolderName)
        {
            if (!Directory.Exists(outputFolderName))
            {
                Directory.CreateDirectory(outputFolderName);
            }

            AssetContainerSas assetContainerSas = client.Assets.ListContainerSasAsync(
                resourceGroup,
                accountName,
                assetName,
                permissions: AssetContainerPermission.Read,
                expiryTime: DateTime.UtcNow.AddHours(1).ToUniversalTime()).Result;

            Uri containerSasUrl = new Uri(assetContainerSas.AssetContainerSasUrls.FirstOrDefault());
            BlobContainerClient container = new BlobContainerClient(containerSasUrl);

            string directory = Path.Combine(outputFolderName, assetName);
            Directory.CreateDirectory(directory);

            Console.WriteLine($"Downloading output results to '{directory}'...");

            string continuationToken = null;
            IList<Task> downloadTasks = new List<Task>();

            do
            {
                var resultSegment = container.GetBlobs().AsPages(continuationToken);

                foreach (Azure.Page<BlobItem> blobPage in resultSegment)
                {
                    foreach (BlobItem blobItem in blobPage.Values)
                    {
                        var blobClient = container.GetBlobClient(blobItem.Name);
                        string filename = Path.Combine(directory, blobItem.Name);

                        downloadTasks.Add(blobClient.DownloadToAsync(filename));
                    }
                    // Get the continuation token and loop until it is empty.
                    continuationToken = blobPage.ContinuationToken;
                }

            } while (continuationToken != "");

            Task.WhenAll(downloadTasks);

            Console.WriteLine("Download complete.");
        }
        // </DownloadResults>
        // <WaitForJobToFinish>
        private static Job WaitForJobToFinishAsync(IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName,
            string jobName)
        {
            const int SleepIntervalMs = 20 * 1000;

            Job job;
            do
            {
                job = client.Jobs.GetAsync(resourceGroupName, accountName, transformName, jobName).Result;

                Log.Info($"Job is '{job.State}'.", "");
                for (int i = 0; i < job.Outputs.Count; i++)
                {
                    JobOutput output = job.Outputs[i];
                    Log.Info($"\tJobOutput[{i}] is '{output.State}'.", "");
                    if (output.State == JobState.Processing)
                    {
                        Log.Info($"  Progress (%): '{output.Progress}'.", "");
                    }
                }

                if (job.State != JobState.Finished && job.State != JobState.Canceled)
                {
                    Task.Delay(SleepIntervalMs);
                }
            }
            while (job.State != JobState.Finished && job.State != JobState.Canceled);

            return job;
        }
        private static Asset CreateInputAssetAsync(
           IAzureMediaServicesClient client,
           string resourceGroupName,
           string accountName,
           string assetName,
           string fileToUpload, Stream videoFiledata)
        {
            // In this example, we are assuming that the asset name is unique.
            //
            // If you already have an asset with the desired name, use the Assets.Get method
            // to get the existing asset. In Media Services v3, the Get method on entities returns null 
            // if the entity doesn't exist (a case-insensitive check on the name).

            // Call Media Services API to create an Asset.
            // This method creates a container in storage for the Asset.
            // The files (blobs) associated with the asset will be stored in this container.
            Asset asset = client.Assets.CreateOrUpdateAsync(resourceGroupName, accountName, assetName, new Asset()).Result;

            // Use Media Services API to get back a response that contains
            // SAS URL for the Asset container into which to upload blobs.
            // That is where you would specify read-write permissions 
            // and the exparation time for the SAS URL.
            var response = client.Assets.ListContainerSasAsync(
                resourceGroupName,
                accountName,
                assetName,
                permissions: AssetContainerPermission.ReadWrite,
                expiryTime: DateTime.UtcNow.AddHours(4).ToUniversalTime()).Result;

            var sasUri = new Uri(response.AssetContainerSasUrls.First());

            // Use Storage API to get a reference to the Asset container
            // that was created by calling Asset's CreateOrUpdate method.  
            BlobContainerClient container = new BlobContainerClient(sasUri);
            BlobClient blob = container.GetBlobClient(Path.GetFileName(fileToUpload));
            //string fileName = @"C:\ketan\idnite.mp4";
            // Use Strorage API to upload the file into the container in storage.
            var blobresult = blob.UploadAsync(videoFiledata).Result;
            return asset;
        }
        // </CreateInputAsset>

        /// <summary>
        /// Creates an ouput asset. The output from the encoding Job must be written to an Asset.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="assetName">The output asset name.</param>
        /// <returns></returns>
        // <CreateOutputAsset>
        private static Asset CreateOutputAssetAsync(IAzureMediaServicesClient client, string resourceGroupName, string accountName, string assetName)
        {
            bool existingAsset = true;
            Asset outputAsset;
            try
            {
                // Check if an Asset already exists
                outputAsset = client.Assets.GetAsync(resourceGroupName, accountName, assetName).Result;
            }
            catch (ErrorResponseException ex)
            {
                existingAsset = false;
            }
            catch (Exception ex)
            {
                existingAsset = false;
            }

            Asset asset = new Asset();
            string outputAssetName = assetName;

            if (existingAsset)
            {
                // Name collision! In order to get the sample to work, let's just go ahead and create a unique asset name
                // Note that the returned Asset can have a different name than the one specified as an input parameter.
                // You may want to update this part to throw an Exception instead, and handle name collisions differently.
                string uniqueness = $"-{Guid.NewGuid():N}";
                outputAssetName += uniqueness;

                Log.Info("Warning – found an existing Asset with name = " + assetName, "");
                Log.Info("Creating an Asset with this name instead: " + outputAssetName, "");
            }

            return client.Assets.CreateOrUpdateAsync(resourceGroupName, accountName, outputAssetName, asset).Result;
        }
        // </CreateOutputAsset>

        /// <summary>
        /// If the specified transform exists, get that transform.
        /// If the it does not exist, creates a new transform with the specified output. 
        /// In this case, the output is set to encode a video using one of the built-in encoding presets.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="transformName">The name of the transform.</param>
        /// <returns></returns>
        // <EnsureTransformExists>
        private static async Task<Transform> GetOrCreateTransformAsync1(
            IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName)
        {
            bool createTransform = false;
            Transform transform = null;
            try
            {
                // Does a transform already exist with the desired name? Assume that an existing Transform with the desired name
                // also uses the same recipe or Preset for processing content.
                transform = client.Transforms.Get(resourceGroupName, accountName, transformName);
            }
            catch (ErrorResponseException ex) when (ex.Response.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                createTransform = true;
            }

            if (createTransform)
            {
                // You need to specify what you want it to produce as an output
                TransformOutput[] output = new TransformOutput[]
                {
                    new TransformOutput
                    {
                        // The preset for the Transform is set to one of Media Services built-in sample presets.
                        // You can  customize the encoding settings by changing this to use "StandardEncoderPreset" class.
                        Preset = new BuiltInStandardEncoderPreset()
                        {
                            // This sample uses the built-in encoding preset for Adaptive Bitrate Streaming.
                            PresetName = EncoderNamedPreset.AdaptiveStreaming
                        }
                    }
                };

                // Create the Transform with the output defined above
                transform = await client.Transforms.CreateOrUpdateAsync(resourceGroupName, accountName, transformName, output);
            }

            return transform;
        }
        // </EnsureTransformExists>

        /// <summary>
        /// Submits a request to Media Services to apply the specified Transform to a given input video.
        /// </summary>
        /// <param name="client">The Media Services client.</param>
        /// <param name="resourceGroupName">The name of the resource group within the Azure subscription.</param>
        /// <param name="accountName"> The Media Services account name.</param>
        /// <param name="transformName">The name of the transform.</param>
        /// <param name="jobName">The (unique) name of the job.</param>
        /// <param name="inputAssetName">The name of the input asset.</param>
        /// <param name="outputAssetName">The (unique) name of the  output asset that will store the result of the encoding job. </param>
        // <SubmitJob>
        private static Job SubmitJobAsync(IAzureMediaServicesClient client,
            string resourceGroupName,
            string accountName,
            string transformName,
            string jobName,
            string inputAssetName,
            string outputAssetName)
        {
            // Use the name of the created input asset to create the job input.
            JobInput jobInput = new JobInputAsset(assetName: inputAssetName);

            JobOutput[] jobOutputs =
            {
                new JobOutputAsset(outputAssetName),
            };

            // In this example, we are assuming that the job name is unique.
            //
            // If you already have a job with the desired name, use the Jobs.Get method
            // to get the existing job. In Media Services v3, the Get method on entities returns null 
            // if the entity doesn't exist (a case-insensitive check on the name).
            Job job = client.Jobs.CreateAsync(
                resourceGroupName,
                accountName,
                transformName,
                jobName,
                new Job
                {
                    Input = jobInput,
                    Outputs = jobOutputs,
                }).Result;
            return job;
        }
        // </SubmitJob>
        [AGELRedirectUnauthenticated]
        public ActionResult VideoDetail(string videourlId = null)
        {

            VideoDetailsModel model = new VideoDetailsModel();
            AGELPortalLessonLearningVideo model1 = new AGELPortalLessonLearningVideo();
            if (!string.IsNullOrEmpty(videourlId))
            {
                AGELPortalDataContext rdb = new AGELPortalDataContext();
                model.aGELPortalContent = rdb.AGELPortalContents.Where(x => x.Id == new Guid(videourlId)).FirstOrDefault();
            }


            if (!string.IsNullOrEmpty(videourlId))
            {
                if (ModelState.IsValid)
                {
                    AGELPortalDataContext rdb = new AGELPortalDataContext();

                    model.aGELPortalContent = rdb.AGELPortalContents.Where(x => x.Id == new Guid(videourlId)).FirstOrDefault();
                    if (model.aGELPortalContent != null)
                    {
                        model1.id = Guid.NewGuid();
                        model1.video_name = model.aGELPortalContent.title;
                        model1.video_url = model.aGELPortalContent.url;
                        model1.user_name = (Session["AGELPortalUserName"] as string);
                        model1.video_id = model.aGELPortalContent.Id;
                        model1.created_date = System.DateTime.Now;
                        model1.user_id = Guid.Parse(Session["AGELPortalUser"] as string);
                        model1.content_type = model.aGELPortalContent.contetn_type;
                        model1.category_name = model.aGELPortalContent.DocVideoImage_Name;
                        model1.docvideocnt = model.aGELPortalContent.DocVideoCnt;
                        //  model1.category_name = model.aGElPortalCategory.name;
                    }

                    rdb.AGELPortalLessonLearningVideos.InsertOnSubmit(model1);
                    rdb.SubmitChanges();

                }

            }
            return View(model);

        }
        [AGELRedirectUnauthenticated]

        public ActionResult AGELPortalLessonLearningVideos()
        {

            PortalLessonlerneningVideo model = new PortalLessonlerneningVideo();
            var id = Session["AGELPortalUser"].ToString();
            var LessonLearningVideosList = rdb.AGELPortalLessonLearningVideos.Where(x => x.user_id.ToString() == id).ToList();
            model.lesson_learningVideo = LessonLearningVideosList.GroupBy(x => x.video_id).Select(x => x.FirstOrDefault()).OrderByDescending(x => x.created_date).Take(8).ToList();
            return View(model);


        }

        [AGELRedirectUnauthenticated]
        public ActionResult DocumentView(string documenturl = null)
        {
            PortalAddContentModel model = new PortalAddContentModel();
            var AGELPortalContents = rdb.AGELPortalContents.Where(x => x.contetn_type == "document" && x.Id.ToString() == documenturl).FirstOrDefault();
            return File(AGELPortalContents.document_data.ToArray(), "application/pdf");
        }

        [AGELRedirectUnauthenticated]
        public ActionResult DocumentViewAzure(string documenturl = null)
        {
            PortalAddContentModel model = new PortalAddContentModel();
            var AGELPortalContents = rdb.AGELPortalContents.Where(x => x.contetn_type == "document" && x.Id.ToString() == documenturl).FirstOrDefault();
            var bytes = objazureBlobStorageServices.GetFileFromAzure(Path.GetFileName(AGELPortalContents.url));
            return File(bytes, "application/pdf");

        }
        [AGELRedirectUnauthenticated]
        public ActionResult DocumentDetail(string documenturl = null)
        {
            PortalAddContentModel model = new PortalAddContentModel();
            var AGELPortalContents = rdb.AGELPortalContents.Where(x => x.contetn_type == "document" && x.Id.ToString() == documenturl).FirstOrDefault();
            AGELPortalLessonLearningVideo model1 = new AGELPortalLessonLearningVideo();
            VideoDetailsModel Videomodel = new VideoDetailsModel();
            if (!string.IsNullOrEmpty(documenturl))
            {
                if (ModelState.IsValid)
                {
                    try
                    {
                        AGELPortalDataContext rdb = new AGELPortalDataContext();
                        Videomodel.aGELPortalContent = rdb.AGELPortalContents.Where(x => x.Id.ToString() == documenturl).FirstOrDefault();

                        if (Videomodel.aGELPortalContent != null)
                        {
                            model1.id = Guid.NewGuid();
                            model1.video_name = Videomodel.aGELPortalContent.title;
                            model1.video_url = Videomodel.aGELPortalContent.url;
                            model1.user_name = (Session["AGELPortalUserName"] as string);
                            model1.video_id = Videomodel.aGELPortalContent.Id;
                            model1.created_date = System.DateTime.Now;
                            model1.user_id = Guid.Parse(Session["AGELPortalUser"] as string);
                            model1.content_type = Videomodel.aGELPortalContent.contetn_type;
                            model1.category_name = Videomodel.aGELPortalContent.DocVideoImage_Name;
                            //model1.category_name = model.AGElPortalCategory.name;
                        }

                        rdb.AGELPortalLessonLearningVideos.InsertOnSubmit(model1);
                        rdb.SubmitChanges();
                    }
                    catch (Exception)
                    {

                        //throw;
                    }


                }
            }

            if (model1.video_id != null)
            {
                model.Doc_id = model1.video_id.ToString();
               // model.document_data = AGELPortalContents.document_data.ToArray();
                model.title = AGELPortalContents.title;
                model.discription = AGELPortalContents.discription;
                model.AdobeDCClientId = System.Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AdobeDCClientId"]);
                model.Url = AGELPortalContents.url;

            }

            String labelText = "";
            System.Web.HttpBrowserCapabilitiesBase myBrowserCaps = HttpContext.Request.Browser;
            var isMobile = false;
            var IsPad = false;
            string u = Request.UserAgent;//ServerVariables["HTTP_USER_AGENT"];
            //string a = "mozilla/5.0 (macintosh; intel mac os x 10_15_7) applewebkit/537.36 (khtml, like gecko) chrome/104.0.0.0 safari/537.36";
            if (!string.IsNullOrEmpty(u))
            {
                u = u.ToLower();
                Regex b = new Regex(@"(android|bb\d+|meego).+mobile|avantgo|bada\/|blackberry|blazer|compal|elaine|fennec|hiptop|iemobile|ip(hone|od)|iris|kindle|lge |maemo|midp|mmp|mobile.+firefox|netfront|opera m(ob|in)i|palm( os)?|phone|p(ixi|re)\/|plucker|pocket|psp|series(4|6)0|symbian|treo|up\.(browser|link)|vodafone|wap|windows ce|xda|xiino", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                Regex v = new Regex(@"1207|6310|6590|3gso|4thp|50[1-6]i|770s|802s|a wa|abac|ac(er|oo|s\-)|ai(ko|rn)|al(av|ca|co)|amoi|an(ex|ny|yw)|aptu|ar(ch|go)|as(te|us)|attw|au(di|\-m|r |s )|avan|be(ck|ll|nq)|bi(lb|rd)|bl(ac|az)|br(e|v)w|bumb|bw\-(n|u)|c55\/|capi|ccwa|cdm\-|cell|chtm|cldc|cmd\-|co(mp|nd)|craw|da(it|ll|ng)|dbte|dc\-s|devi|dica|dmob|do(c|p)o|ds(12|\-d)|el(49|ai)|em(l2|ul)|er(ic|k0)|esl8|ez([4-7]0|os|wa|ze)|fetc|fly(\-|_)|g1 u|g560|gene|gf\-5|g\-mo|go(\.w|od)|gr(ad|un)|haie|hcit|hd\-(m|p|t)|hei\-|hi(pt|ta)|hp( i|ip)|hs\-c|ht(c(\-| |_|a|g|p|s|t)|tp)|hu(aw|tc)|i\-(20|go|ma)|i230|iac( |\-|\/)|ibro|idea|ig01|ikom|im1k|inno|ipaq|iris|ja(t|v)a|jbro|jemu|jigs|kddi|keji|kgt( |\/)|klon|kpt |kwc\-|kyo(c|k)|le(no|xi)|lg( g|\/(k|l|u)|50|54|\-[a-w])|libw|lynx|m1\-w|m3ga|m50\/|ma(te|ui|xo)|mc(01|21|ca)|m\-cr|me(rc|ri)|mi(o8|oa|ts)|mmef|mo(01|02|bi|de|do|t(\-| |o|v)|zz)|mt(50|p1|v )|mwbp|mywa|n10[0-2]|n20[2-3]|n30(0|2)|n50(0|2|5)|n7(0(0|1)|10)|ne((c|m)\-|on|tf|wf|wg|wt)|nok(6|i)|nzph|o2im|op(ti|wv)|oran|owg1|p800|pan(a|d|t)|pdxg|pg(13|\-([1-8]|c))|phil|pire|pl(ay|uc)|pn\-2|po(ck|rt|se)|prox|psio|pt\-g|qa\-a|qc(07|12|21|32|60|\-[2-7]|i\-)|qtek|r380|r600|raks|rim9|ro(ve|zo)|s55\/|sa(ge|ma|mm|ms|ny|va)|sc(01|h\-|oo|p\-)|sdk\/|se(c(\-|0|1)|47|mc|nd|ri)|sgh\-|shar|sie(\-|m)|sk\-0|sl(45|id)|sm(al|ar|b3|it|t5)|so(ft|ny)|sp(01|h\-|v\-|v )|sy(01|mb)|t2(18|50)|t6(00|10|18)|ta(gt|lk)|tcl\-|tdg\-|tel(i|m)|tim\-|t\-mo|to(pl|sh)|ts(70|m\-|m3|m5)|tx\-9|up(\.b|g1|si)|utst|v400|v750|veri|vi(rg|te)|vk(40|5[0-3]|\-v)|vm40|voda|vulc|vx(52|53|60|61|70|80|81|83|85|98)|w3c(\-| )|webc|whit|wi(g |nc|nw)|wmlb|wonu|x700|yas\-|your|zeto|zte\-", RegexOptions.IgnoreCase | RegexOptions.Multiline);
                if ((b.IsMatch(u) || v.IsMatch(u.Substring(0, 4))))
                {
                    isMobile = true;
                    // ViewBag.IsMoblieDevice = myBrowserCaps.IsMobileDevice;
                }
                else if (u.Contains("android"))
                {
                    isMobile = true;
                    // iPad
                }
                else if (u.Contains("ipad;"))
                {
                    IsPad = true;
                    // iPad
                }
            }
           


            ViewBag.IsMoblieDevice = isMobile;
            ViewBag.IsPad = IsPad;
            ViewBag.UserAgent = u;
            return View(model);

        }

        [AGELRedirectUnauthenticated]
        public ActionResult UserDocumentDetail()
        {
            var id = Session["AGELPortalUser"].ToString();
            var UserDocumentDetailList = rdb.AGELPortalLessonLearningVideos.Where(x => x.user_id.ToString() == id && x.content_type == "document").ToList();
            PortalLessonlerneningVideo model1 = new PortalLessonlerneningVideo();

            model1.lesson_learningVideo = UserDocumentDetailList;
            return View(model1);

        }
        [AGELRedirectUnauthenticated]
        public ActionResult VideoListing()
        {
            var videos = rdb.AGELPortalContents.Where(x => x.contetn_type == "video" && x.status == true).ToList();
            //var categories = rdb.AGElPortalCategories.Where(x => x.status == true).Take(5).ToList();
            var categories = rdb.AGElPortalCategories.Where(x => x.status == true).ToList();
            PortalAddContentModel ContentModel = new PortalAddContentModel();

            if (videos != null)
                ContentModel.videos = videos;
            if (categories != null)
                ContentModel.contentCategories = categories;

            //var videoCategoryList = (from content in rdb.AGELPortalContents.Where(x => x.contetn_type == "video")
            //                         join category in rdb.AGElPortalCategories.Where(x => x.status == true) on content.category_id equals category.Id
            //                         select category).Distinct().Take(5).ToList();

            var videoCategoryList = (from content in rdb.AGELPortalContents.Where(x => x.contetn_type == "video")
                                     join category in rdb.AGElPortalCategories.Where(x => x.status == true) on content.category_id equals category.Id
                                     select category).Distinct().ToList();
            ContentModel.videoCategories = videoCategoryList;

            return View(ContentModel);
        }
        [AGELRedirectUnauthenticated]
        public ActionResult DocumentListing()
        {
            var documentCategoryList = (from content in rdb.AGELPortalContents.Where(x => x.contetn_type == "document" && x.status == true).OrderByDescending(y => y.created_date)
                                        join category in rdb.AGElPortalCategories.Where(x => x.status == true) on content.category_id equals category.Id
                                        select category).Distinct().Take(30).ToList();

            var documents = new List<AGELPortalContent>();
            foreach (var item in documentCategoryList)
            {
                var a = rdb.AGELPortalContents.Where(x => x.contetn_type == "document" && x.status == true && x.category_id == item.Id).OrderByDescending(y => y.created_date).Take(100).ToList();
                documents.AddRange(a);
            }



            // var documents = rdb.AGELPortalContents.Where(x => x.contetn_type == "document" && x.status == true && x.document_data != null).ToList();
            var categories = rdb.AGElPortalCategories.Where(x => x.status == true).ToList();
            PortalAddContentModel ContentModel = new PortalAddContentModel();
            //var documentCategoryList = (from content in rdb.AGELPortalContents.Where(x => x.contetn_type == "document")
            // join category in rdb.AGElPortalCategories.Where(x => x.status == true) on content.category_id equals category.Id
            // select category).Distinct().Take(5).ToList();
            if (documents != null)
                ContentModel.documents = documents;
            if (categories != null)
                ContentModel.contentCategories = categories;
            ContentModel.documentCategories = documentCategoryList;
            return View(ContentModel);
        }


        public void SignIn()
        {
            Log.Info("Inside SignIn", this);
            var redirectToIdp = this.FedAuthLoginRepository.GetAllAAD();
            if (redirectToIdp == "failed")
            {
                ViewBag.successmessage = "Something has been wrong. Please try again later.";
                var url = "/agelportal/home";
                PostRedirect(url);

            }
            PostRedirect(redirectToIdp);
        }

        private void PostRedirect(string url)
        {
            Response.Clear();
            var sb = new System.Text.StringBuilder();
            sb.Append("<html>");
            sb.AppendFormat("<body onload='document.forms[0].submit()'>");
            sb.AppendFormat("<form action='{0}' method='post'>", url);
            sb.Append("</form>");
            sb.Append("</body>");
            sb.Append("</html>");
            Response.Write(sb.ToString());
            Response.End();
        }

        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalChangeMobile()
        {
            return View();
        }
        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult AGELPortalChangeMobile(PortalRegistrationModel pm)
        {
            var url = "";
            var id = Session["AGELPortalUser"].ToString();
            var user = rdb.AGELPortalRegistrations.Where(x => x.Id.ToString() == id).ToList().FirstOrDefault();
            var mobilenoAlreadyExists = rdb.AGELPortalRegistrations.Any(x => x.mobile == pm.mobile);

            if (user != null && mobilenoAlreadyExists != true)
            {

                pm.name = user.name;
                bool timediffer = CanSendOTP(pm.mobile);
                if (!timediffer)
                {
                    this.ModelState.AddModelError(nameof(pm.mobile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Please try again after 1 hour."));
                    return this.View(pm);
                }
                else
                {
                    if (SendOtpForValidation(pm) && pm.mobile.Length == 10)
                    {
                        url = "/agelportal/home/change_mobile_OTP?mobile=" + pm.mobile;
                        return CustomRedirect(url);
                    }
                    else
                    {
                        this.ModelState.AddModelError(nameof(pm.mobile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "OTP can not be sent please check details"));
                        return this.View(pm);
                    }
                }
            }
            else
            {
                this.ModelState.AddModelError(nameof(pm.mobile), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "This mobile number already exists. Please try with another one."));
                return this.View(pm);
            }

        }
        [AGELRedirectUnauthenticated]
        public ActionResult AGELChangeMobileOTP()
        {
            return View(new PortalRegistrationModel());
        }
        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult AGELChangeMobileOTP(PortalRegistrationModel pm)
        {
            var id = Session["AGELPortalUser"].ToString();
            string generatedOTP = realtyRepo.GetOTP(pm.mobile);
            //bool timediffer = CanSendOTP(pm.mobile);
            //if (!timediffer)
            //{
            //    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Please OTP is expired, please generate again."));
            //    return this.View(pm);
            //}
            if (generatedOTP == "optexpired")
            {
                this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Please OTP is expired, please generate again."));
                return this.View(pm);
            }

            else if (!string.Equals(generatedOTP, pm.OTP))
            {

                this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Please enter the correct OTP."));
                Session["addUserSuccess"] = "Please enter the correct OTP.";
                var url = "/AGELPortal/Home/change_mobile_OTP?mobile=" + pm.mobile;
                return CustomRedirect(url);
            }


            else
            {
                var user = rdb.AGELPortalRegistrations.Where(x => x.Id.ToString() == id).ToList().FirstOrDefault();
                if (user == null)
                {
                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Invalid user."));
                    return this.View(pm);
                }
                else
                {
                    user.mobile = pm.mobile;
                    rdb.SubmitChanges();
                    Session["myprofilechangmobileno"] = "Mobile number changed successfully.";
                    var url = "/AGELPortal/Home/My_Profile";
                    return CustomRedirect(url);

                }
            }
        }
        [AGELRedirectUnauthenticated]
        public ActionResult myprofilechangepassword()
        {
            return View(new PortalRegistrationModel());
        }
        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult myprofilechangepassword(PortalRegistrationModel pm)
        {
            var url = "";
            var userID = Session["AGELPortalUser"].ToString();
            var detail = rdb.AGELPortalRegistrations.Where(x => x.password == pm.password && x.Id.ToString() == userID).FirstOrDefault();
            if (detail != null)
            {

                var isallowtologin = CheckAllowtoLogin(detail.email);

                if (!isallowtologin)
                {
                    if (Session["AGELPortalUser"] != null)
                    {
                        Session["AGELPortalUser"] = null;
                    }
                    if (Session["AGELPortalUserName"] != null)
                    {
                        Session["AGELPortalUserName"] = null;
                    }
                    SignOut();
                    TempData["ForceLoginMessage"] = "You have reached maximum limit to change password, Please try to login after next 1 hr";
                    return CustomRedirect("/AGELPortal/Home/login");
                    // this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/Form/InvalidUser", "please try after 1 hour to change the password, you are not able login next 1hr"));
                    //return this.View(pm);
                }
                else if (pm.new_password != null && pm.new_password != "" && pm.new_password == pm.password)
                {
                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "please enter new password and old password should not be same."));
                    return this.View(pm);
                }
                else if (pm.new_password != null && pm.new_password != "" && pm.new_password == pm.confirm_password)
                {
                    detail.password = pm.new_password;
                    detail.modified = System.DateTime.Now;
                    rdb.SubmitChanges();
                    Session["myprofilechangmobileno"] = " Password Change Successfully";
                    url = "/AGELPortal/Home/My_Profile";
                    //TempData["ChangepassWordSuccess"] = "PassWord Change Successfully";

                    AGELPortalUserLoginHistory objAGELPortalUserLoginHistory = new AGELPortalUserLoginHistory();
                    objAGELPortalUserLoginHistory.Id = Guid.NewGuid();
                    objAGELPortalUserLoginHistory.User_Id = detail.Id;
                    objAGELPortalUserLoginHistory.UserName = detail.name.ToString();
                    objAGELPortalUserLoginHistory.Login_Time = System.DateTime.Now;
                    objAGELPortalUserLoginHistory.checkLoginAttempts = true;
                    rdb.AGELPortalUserLoginHistories.InsertOnSubmit(objAGELPortalUserLoginHistory);
                    rdb.SubmitChanges();

                    var objLoginHistorylist = rdb.AGELPortalUserLoginHistories.Where(x => x.User_Id == detail.Id && x.checkLoginAttempts == true).ToList();
                    foreach (var item in objLoginHistorylist)
                    {
                        item.checkLoginAttempts = false;
                    }
                    rdb.SubmitChanges();
                    //send email for updated password                  
                    SendEmail_UserUpdatedPassword(detail.email, detail.name, detail.password);
                }
                else
                {
                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "please enter new password and confirm password should be same."));
                    return this.View(pm);
                }
            }
            else
            {
                detail = rdb.AGELPortalRegistrations.Where(x => x.Id.ToString() == userID).FirstOrDefault();
                AGELPortalUserLoginHistory objAGELPortalUserLoginHistory = new AGELPortalUserLoginHistory();
                objAGELPortalUserLoginHistory.Id = Guid.NewGuid();
                objAGELPortalUserLoginHistory.User_Id = detail.Id;
                objAGELPortalUserLoginHistory.UserName = detail.name.ToString();
                objAGELPortalUserLoginHistory.Login_Time = System.DateTime.Now;
                objAGELPortalUserLoginHistory.checkLoginAttempts = true;
                rdb.AGELPortalUserLoginHistories.InsertOnSubmit(objAGELPortalUserLoginHistory);
                rdb.SubmitChanges();


                var count = rdb.AGELPortalUserLoginHistories.Where(x => x.User_Id == detail.Id && x.checkLoginAttempts == true).Count();
                var maxattempt = 3 - count;

                if (maxattempt > 0)
                {
                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "Please enter correct password. Change password attempts remaining is " + maxattempt));
                    // return this.View(pm);
                }
                else
                {
                    HttpContext.GetOwinContext().Authentication.SignOut(
               OpenIdConnectAuthenticationDefaults.AuthenticationType,
               CookieAuthenticationDefaults.AuthenticationType);
                    TempData["ForceLoginMessage"] = "You have reached maximum limit to change password, please try to login after 1 hr";
                    return CustomRedirect("/AGELPortal/Home/login");
                    //this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "please enter correct password. please try after 1 hour to change password."));
                    //  return this.View(pm);
                }

                //this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "please enter correct password"));

                // this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "please enter correct password"));
                return this.View(pm);


            }

            return CustomRedirect(url);
        }

        //[HttpPost]
        //[AGELRedirectUnauthenticated]
        //public ActionResult myprofilechangepassword(PortalRegistrationModel pm)
        //{
        //    var url = "";
        //    var userID = Session["AGELPortalUser"].ToString();
        //    var detail = rdb.AGELPortalRegistrations.Where(x => x.password == pm.password && x.Id.ToString() == userID).FirstOrDefault();
        //    if (detail != null)
        //    {
        //        if (pm.new_password != null && pm.new_password != "" && pm.new_password == pm.confirm_password)
        //        {
        //            detail.password = pm.new_password;
        //            detail.modified = System.DateTime.Now;
        //            rdb.SubmitChanges();
        //            //Session["ChangepassWordSuccess"] = " Password Change Successfully";
        //            url = "/AGELPortal/Home/My_Profile";
        //            Session["myprofilechangmobileno"] = "Password Change Successfully";

        //            //send email for updated password                   
        //            SendEmail_UserUpdatedPassword(detail.email, detail.name, detail.password);
        //        }
        //        else
        //        {
        //            this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "please enter correct password"));
        //            return this.View(pm);
        //        }
        //    }
        //    else
        //    {
        //        this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "please enter correct password"));
        //        return this.View(pm);


        //    }

        //    return Redirect(url);
        //}
        public ActionResult usernotification()
        {
            //var lasthreeday = rdb.AGELPortalContents.Where(x => x.created_date >= DateTime.Now.AddDays(-3) ).ToList();


            //PortalAddContentModel model1 = new PortalAddContentModel();


            //model1.videos = lasthreeday;
            //return View(model1);
            return View();
        }









        [AGELRedirectUnauthenticated]
        public ActionResult AGELAdminResetPassword(string searchKeyword = null)
        {

            var usertype = Session["AGELPortalUserType"]?.ToString();

            if (Session["AGELPortalUserType"].ToString() != "superadmin")
            {
                return CustomRedirect("/AGELPortal/Home/login");
            }
            PortalAddUserModel User = new PortalAddUserModel();
            if (searchKeyword != null && searchKeyword != "")
            {




                var search = rdb.AGELPortalRegistrations.Where(x => x.name.Contains(searchKeyword) || x.mobile.Contains(searchKeyword) || x.email.Contains(searchKeyword)).ToList();
                User.Users = search;

            }
            return View(User);
        }

        [HttpPost]
        [AGELRedirectUnauthenticated]
        public ActionResult AGELAdminResetPassword(PortalAddUserModel pm)
        {
            var url = "";
            var userID = Session["AGELPortalUser"].ToString();
            var userMobile = Request.Form["MMobile"];
            var userData = rdb.AGELPortalRegistrations.Where(x => x.Id != null && x.mobile == userMobile).FirstOrDefault();
            if (userData != null)
            {
                if (pm.password == pm.new_password)
                {
                    userData.password = pm.new_password;
                    userData.modified = System.DateTime.Now;
                    url = "/AGELPortal/Home/dashboard/admin_Rese_Password";
                    Session["ResetpassWordSuccess"] = " Password Reset Successfully.";

                    rdb.SubmitChanges();
                    string sms_content = "Hi " + userData.name;
                    sms_content = ". Your Password is updated successfully.";
                    sms_content = " New Password: " + userData.password;

                    //send email for updated password                   
                    SendEmail_UserUpdatedPassword(userData.email, userData.name, userData.password);
                    //if (SendSMSUpdates(userData.mobile, sms_content))
                    //{
                    //    Log.Info("sms sent", "");
                    //}
                    //else
                    //{
                    //    Log.Info("sms failed", "");
                    //}
                }
                else
                {
                    this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "please enter correct password"));
                    return this.View(pm);
                }
            }
            else
            {
                this.ModelState.AddModelError(nameof(pm.UserValidation), DictionaryPhraseRepository.Current.Get("/AGELPortal/Form/InvalidUser", "User Is Not Valid"));
                return this.View(pm);
            }
            return CustomRedirect(url);


        }

        //send sms for status change
        //public dynamic SendSMSUpdates(string mobile = null, string sms_content = null)
        //{
        //    //var result = new { status = "0" };
        //    var result = false;
        //    try
        //    {

        //        PortalSMS request1 = new PortalSMS
        //        {
        //            mobileNo = mobile,
        //            messageList = sms_content
        //        };

        //        var serializedObj = JsonConvert.SerializeObject(request1);
        //        var client = new RestClient("https://dev2.adanidigitallabs.com/SMSDispatcher/Api/sms/send");
        //        client.Timeout = -1;
        //        var request = new RestRequest(Method.POST);
        //        request.AddHeader("ApiKey", "apikey12345");
        //        request.AddHeader("Content-Type", "application/json");
        //        request.AddParameter("application/json", serializedObj, ParameterType.RequestBody);
        //        IRestResponse response = client.Execute(request);
        //        if (response.IsSuccessful && response.Content != null)
        //        {
        //            OTPDeserialize jsonObject = JsonConvert.DeserializeObject<OTPDeserialize>(response.Content);
        //            // OTPData propertyData = new OTPData();
        //            // SendOtpEmail(model.email, model.name, propertyData.otp, model.Id);
        //            // propertyData = jsonObject.data;
        //            Log.Info($" Adanidigitallabs Api_sms_send response:  '{response.Content}'.", "");
        //            result = true;
        //        }
        //        else
        //        {
        //            string responseCode = "response " + response.ResponseStatus;
        //            Log.Info($" Adanidigitallabs Api_sms_send responseCode: '{response.ResponseStatus}'.", "");
        //            result = false;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error($"{0}", ex, this);
        //        result = false;
        //    }
        //    return result;

        //}

        //added by neeraj yadav for new sms api
        public dynamic SendSMSUpdates(string mobile = null, string sms_content = null)
        {
            //var result = new { status = "0" };
            var result = false;
            try
            {
                StringBuilder strresult = new StringBuilder();
                List<MessageList> messageList = new List<MessageList>();

                MessageList message1 = new MessageList
                {
                    key = "OTP",
                    value = sms_content
                };
           

                messageList.Add(message1);
                PortalSMS request1 = new PortalSMS
                {
                    mobileNo = mobile,
                    data = messageList
                };
             
                //OTPRequest request1 = new OTPRequest
                //{
                //    mobileNo = model.mobile,
                //    data = messageList
                //};

                var serializedObj = JsonConvert.SerializeObject(request1);
                strresult.Append("serializedObj:" + serializedObj);
                var smsapiurl = System.Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AGELSMSAPIURL"]);
                var smsapikey = System.Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AGELSMSAPIKEY"]);
                var smsapivalue = "degf@2457@!$#(&&$%^$2342";
                string rawData = string.Format("{0} ~ {1}", smsapivalue, mobile);

                //calling function to compute hash value
                var authorValue = HashString(rawData, "~6nqej2");
                // var client = new RestClient("https://dev2.adanidigitallabs.com/SMSDispatcher/Api/sms/send");

                var client = new RestClient(smsapiurl);
                client.Timeout = -1;
                var request = new RestRequest(Method.POST);
                request.AddHeader("Author", authorValue);
                strresult.Append("authorValue:" + authorValue);
                //request.AddHeader(smsapikey, smsapivalue);
                //request.AddHeader("ApiKey", "apikey12345");
                request.AddHeader("Content-Type", "application/json");
                request.AddParameter("application/json", serializedObj, ParameterType.RequestBody);
                IRestResponse response = client.Execute(request);
                if (response.IsSuccessful && response.Content != null)
                {
                    OTPDeserialize jsonObject = JsonConvert.DeserializeObject<OTPDeserialize>(response.Content);
                    // OTPData propertyData = new OTPData();
                    // SendOtpEmail(model.email, model.name, propertyData.otp, model.Id);
                    // propertyData = jsonObject.data;
                    Log.Info($" Adanidigitallabs Api_sms_send response:  '{response.Content}'.", "");
                    result = true;
                }
                else
                {
                    string responseCode = "response " + response.ResponseStatus;
                    Log.Info($" Adanidigitallabs Api_sms_send responseCode: '{response.ResponseStatus}'.", "");
                    strresult.AppendLine($" Adanidigitallabs Api_sms_send responseCode: '{response.ResponseStatus}'.");
                    strresult.AppendLine($" Adanidigitallabs Api_sms_send IsSuccessful: '{response.IsSuccessful}'.");
                    strresult.AppendLine($" Adanidigitallabs Api_sms_send Content: '{response.Content}'.");
                    // Log.Info($" Adanidigitallabs Api_sms_send responseCode: '{response.ResponseStatus}'.", "");
                    result = false;
                }
            }
            catch (Exception ex)
            {
                Log.Error($"{0}", ex, this);
                result = false;
            }


            return result;

        }


        //added by neeraj yadav to calculate hash value of a string
        private string HashString(string text, string salt = "")
        {
            if (String.IsNullOrEmpty(text))
            {
                return String.Empty;
            }

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textBytes = System.Text.Encoding.UTF8.GetBytes(text + salt);
                byte[] hashBytes = sha.ComputeHash(textBytes);

                string hash = BitConverter
                    .ToString(hashBytes).Replace("-", string.Empty);
                return hash;
            }
        }

        public ActionResult CustomRedirect(string url)
        {
            // return this.Redirect(url);
            //httpContext.Server.TransferRequest(this.Url, true)
            return new Sitecore.AGELPortal.Website.Attributes.RedirectResultNoBody(url);
        }
    }


}
#endregion