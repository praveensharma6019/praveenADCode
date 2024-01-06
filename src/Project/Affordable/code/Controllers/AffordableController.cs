using Sitecore.Affordable.Website.Models;
using Sitecore.Affordable.Website.Models.InstaMojo;
using Sitecore.Affordable.Website.SalesForce;
using Sitecore.Affordable.Website.SalesForce.Domain;
using Sitecore.Affordable.Website.Services;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Web.Mvc;
namespace Sitecore.Affordable.Website.Controllers
{
    public class AffordableController : Controller
    {
        AffordableRepository affordRepo = new AffordableRepository();
        // GET: Affordable
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Searchs(AffordableFilter param = null)
        {

            IQueryable<Data.Items.Item> result;

            var qry = param.qry;
            var proj = param.project;
            var type = param.type;
            var status = param.status;
            // var minarea = param.minarea;
            // var maxarea = param.maxarea;
            // var minbudget = param.minbudget;
            // var maxbudget = param.maxbudget;

            var properties = Sitecore.Context.Site.GetStartItem();

            #region Filter by City
            result = properties.Axes.GetDescendants().Where(x => x.TemplateID == Templates.ProjectProperty.ID).AsQueryable();
            if (!string.IsNullOrEmpty(qry))
            {
                List<SelectListItem> lstLocation = new List<SelectListItem>();
                var locations = Sitecore.Context.Database.GetItem(Templates.AffordableLocations.RootItem);
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
                /////////old rendering
                // result = result.Where(x => x.Fields[Templates.ProjectProperty.Fields.Location].Value.Contains(locationid) && (!string.IsNullOrEmpty(locationid))).AsQueryable();


                //new rendering by project name or location
                List<SelectListItem> lstprojectname = new List<SelectListItem>();
                var projects = Sitecore.Context.Database.GetItem(Templates.AffordableProject.RootItem);
                string projectid = string.Empty;
                foreach (var item in projects.Axes.GetDescendants().ToList())
                {
                    lstprojectname.Add(new SelectListItem() { Text = item.Fields[Templates.SingleText.Fields.Text].Value, Value = item.ID.ToString() });
                }

                var projectnamefilter = lstprojectname.Where(x => x.Text.ToLower().Contains(qry.ToLower())).Any();
                if (projectnamefilter)
                {
                    projectid = lstprojectname.Where(x => x.Text.ToLower().Contains(qry.ToLower())).FirstOrDefault().Value;
                }

                if (projectnamefilter)
                {
                    result = result.Where(x => x.Fields[Templates._HasPageContent.Fields.Title].Value.ToLower().Contains(qry.ToLower())).AsQueryable();
                    //   result = result.Where(x => x.Fields[Templates.ProjectProperty.Fields.Location].Value.Contains(locationid) || x.Fields[Templates._HasPageContent.Fields.Title].Value.ToLower().Contains(qry.ToLower())).AsQueryable();
                }
                else
                {
                    result = result.Where(x => x.Fields[Templates.ProjectProperty.Fields.Location].Value.Contains(locationid) && (!string.IsNullOrEmpty(locationid))).AsQueryable();
                }
            }
            #endregion

            #region Filter by Project Name  
            if (!string.IsNullOrEmpty(proj))
            {
                //result = properties.Axes.GetDescendants().Where(x => x.TemplateID == Templates.ProjectProperty.ID).AsQueryable();
                //List<SelectListItem> lstLocation = new List<SelectListItem>();
                //var projects = Sitecore.Context.Database.GetItem(Templates.AffordableProject.RootItem);
                //string locationid = string.Empty;
                //foreach (var item in projects.Axes.GetDescendants().ToList())
                //{
                //    lstLocation.Add(new SelectListItem() { Text = item.Fields[Templates.SingleText.Fields.Text].Value, Value = item.ID.ToString() });
                //}

                //var locationfilter = lstLocation.Where(x => x.Text.ToLower().Contains(proj.ToLower())).Any();
                //if (locationfilter)
                //{
                //    locationid = lstLocation.Where(x => x.Text.ToLower().Contains(proj.ToLower())).FirstOrDefault().Value;
                //}

                //  result = result.Where(x => x.Fields[Templates._HasPageContent.Fields.Title].Value.ToLower().Contains(proj.ToLower())).AsQueryable();

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

                result = result.Where(x => x.Fields[Templates.ProjectProperty.Fields.ProjectStatus].Value.Contains(status)).AsQueryable();
            }
            #endregion





            return View("Searchs", result);
        }

        protected IQueryable<Sitecore.Data.Items.Item> FilterRecordbyPropertytype(string type, IQueryable<Sitecore.Data.Items.Item> result)
        {

            var TwoBhk = Sitecore.Context.Database.GetItem(Templates.AffordablePropertyTypeText.TwoBHK);
            var ThreeBhk = Sitecore.Context.Database.GetItem(Templates.AffordablePropertyTypeText.ThreeBHK);
            if (type == TwoBhk.Fields[Templates.SingleText.Fields.Text].Value)
            {
                result = result.Where(x => x.TemplateID == Templates.ProjectProperty.ID).AsQueryable();
            }
            if (type == ThreeBhk.Fields[Templates.SingleText.Fields.Text].Value)
            {
                result = result.Where(x => x.TemplateID == Templates.ProjectProperty.ID).AsQueryable();
            }
            return result;
        }

        [HttpPost]
        public ActionResult sendOTP(AffordableFormModel model)
        {
            EnquiryDataContext dc = new EnquiryDataContext();
            var result = new { status = "0" };

            try
            {

                #region Delete Available otp from database for given mobile number

                affordRepo.DeleteOldOtp(model.Mobile);
                #endregion

                #region Generate New Otp for given mobile number and save to database
                string generatedotp = affordRepo.StoreGeneratedOtp(model);
                #endregion

                #region Api call to send SMS of OTP
                try
                {
                    var apiurl = string.Format("https://otp2.maccesssmspush.com/OTP_ACL_Web/OtpRequestListener?enterpriseid=adtrotpi&subEnterpriseid=adtrotpi&pusheid=adtrotpi&pushepwd=adtrotpi22&msisdn={0}&sender=ADANIR&msgtext=Dear%20customer,%20please%20enter%20the%20verification%20code%20{1}%20to%20submit%20your%20web%20enquiry.", model.Mobile, generatedotp);
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Error("OTP Api call success. https://otp2.maccesssmspush.com", this);
                    }
                    else
                    {
                        Log.Error("OTP Api call failed. https://otp2.maccesssmspush.com", this);
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

        [HttpPost]
        public ActionResult VerifyOTP(AffordableFormModel model)
        {
            var result = new { status = "0" };
            #region Verify OTP
            string generatedOTP = affordRepo.GetOTP(model.Mobile);
            if (string.Equals(generatedOTP, model.OTP))
            {
                result = new { status = "1" };
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult InsertEnquiryNowdetail(AffordableFormModel m)
        {
            Log.Error("InsertEnquiryNowdetail ", "Start");
            var result = new { status = "1" };
            try
            {
                AffordableEnquiryDataContext rdb = new AffordableEnquiryDataContext();
                AffordableFormRecord r = new AffordableFormRecord();

                r.Name = m.Name;
                r.Mobile = m.Mobile;
                r.Email = m.Email;
                r.Country = m.Country;
                r.State = m.State;
                r.Property_Interest = m.Property_Interest;
                r.Budget = m.Budget;
                r.Remarks = m.Remarks;
                r.FormType = m.FormType;
                r.PageInfo = m.PageInfo;
                r.FormSubmitOn = m.FormSubmitOn;



                #region Insert to DB
                rdb.AffordableFormRecords.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.WriteLine(ex);
            }
            try
            {
                string message = "";
                string emailText = DictionaryPhraseRepository.Current.Get("/EnquiryNow/EmailText", "");
                message = "Hello<br><br>" + emailText + "<br><br>Name: " + m.Name;
                message = message + "<br>Mobile No: " + m.Mobile + "<br>Email Id: " + m.Email + "<br>Country: " + m.Country + "<br>State: " + m.State + "<br>Interested In: " + m.Property_Interest + "<br>Budget: " + m.Budget + "<br>Remarks: " + m.Remarks + "<br><br>Thanks";
                string to = DictionaryPhraseRepository.Current.Get("/EnquiryNow/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/EnquiryNow/EmailFrom", "");
                string emailSubject = DictionaryPhraseRepository.Current.Get("/EnquiryNow/EmailSubject", "");

                bool results = sendEmail(to, emailSubject, message, from);

                if (results)
                {
                    Log.Error("Email Sent- ", "");
                }
            }
            catch (Exception ex)
            {
                result = new { status = "1" };
                Log.Error("Failed to sent Email", ex.ToString());

            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
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
                var ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                //mail.From = new MailAddress(Sitecore.Configuration.Settings.MailServerUserName);
                mail.From = new MailAddress(from);
                //System.Net.Mail.Attachment attachment = new System.Net.Mail.Attachment(ms, ct);
                // attachment.ContentDisposition.FileName = fileName;
                // mail.Attachments.Add(attachment);
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
        public ActionResult AffordableFilter(AffordableFilter param = null)
        {
            var AffordablePropertyType = Sitecore.Context.Database.GetItem(Templates.AffordablePropertyType.GlobalRootFolderID);
            var AffordableProjectStatus = Sitecore.Context.Database.GetItem(Templates.AffordableProjectStatus.GlobalRootFolderID);
            //    var RealtyArea = Sitecore.Context.Database.GetItem(Templates.RealtyArea.GlobalRootFolderID);
            //    var RealtyBudget = Sitecore.Context.Database.GetItem(Templates.RealtyBudget.GlobalRootFolderID);

            if (param == null)
                param = new AffordableFilter();

            #region Property List Binding
            var propList = new List<SelectListItem>();
            AffordablePropertyType.Children.ToList().ForEach(x =>
            {
                propList.Add(new SelectListItem { Text = x.Fields[Templates.AffordablePropertyType.Fields.Text].Value, Value = x.Fields[Templates.AffordablePropertyType.Fields.Text].Value });
            });
            param.PropertyTypeList = propList;
            #endregion

            #region Status List Binding
            var statusList = new List<SelectListItem>();
            AffordableProjectStatus.Children.ToList().ForEach(x =>
            {
                statusList.Add(new SelectListItem { Text = x.Fields[Templates.AffordableProjectStatus.Fields.Text].Value, Value = x.ID.ToString() });
            });
            param.ProjectStatusList = statusList;
            #endregion

            //#region Min /Max Area Binding
            //var maximumarealist = new List<SelectListItem>();
            //var minimumarealist = new List<SelectListItem>();
            //RealtyArea.Children.ToList().ForEach(x =>
            //{
            //    maximumarealist.Add(new SelectListItem { Text = x.Fields[Templates.RealtyArea.Fields.Text].Value, Value = x.Fields[Templates.RealtyArea.Fields.Value].Value });
            //    minimumarealist.Add(new SelectListItem { Text = x.Fields[Templates.RealtyArea.Fields.Text].Value, Value = x.Fields[Templates.RealtyArea.Fields.Value].Value });
            //});
            //param.MaximumAreaList = maximumarealist;
            //param.MinimumAreaList = maximumarealist;
            //#endregion

            //#region Min/Max Budget Binding
            //var maximumbudgetlist = new List<SelectListItem>();
            //var minimumbudgetlist = new List<SelectListItem>();
            //RealtyBudget.Children.ToList().ForEach(x =>
            //{
            //    maximumbudgetlist.Add(new SelectListItem { Text = x.Fields[Templates.RealtyBudget.Fields.Text].Value, Value = x.Fields[Templates.RealtyBudget.Fields.Value].Value });
            //    minimumbudgetlist.Add(new SelectListItem { Text = x.Fields[Templates.RealtyBudget.Fields.Text].Value, Value = x.Fields[Templates.RealtyBudget.Fields.Value].Value });
            //});
            //param.MaximumBudgetList = maximumbudgetlist;
            //param.MinimumBudgetList = minimumbudgetlist;
            //#endregion

            return View("AffordableFilter", param);
        }


        [HttpPost]
        public ActionResult InsertHeaderContactdetail(AffordableFormModel m)
        {
            Log.Error("InsertHeaderContactdetail ", "Start");
            var result = new { status = "1" };
            try
            {
                AffordableEnquiryDataContext rdb = new AffordableEnquiryDataContext();
                AffordableFormRecord r = new AffordableFormRecord();


                r.Mobile = m.Mobile;
                r.FormType = m.FormType;
                r.PageInfo = m.PageInfo;
                r.FormSubmitOn = m.FormSubmitOn;



                #region Insert to DB
                rdb.AffordableFormRecords.InsertOnSubmit(r);
                rdb.SubmitChanges();
            }
            catch (Exception ex)
            {
                result = new { status = "0" };
                Console.WriteLine(ex);
            }
            try
            {
                string emailText = DictionaryPhraseRepository.Current.Get("/HeaderContact/EmailText", "");
                string message = "";
                message = "Hello<br><br>" + emailText + "<br><br>Contact No: " + m.Mobile;
                message = message + "<br><br>Thanks";
                string to = DictionaryPhraseRepository.Current.Get("/HeaderContact/EmailTo", "");
                string from = DictionaryPhraseRepository.Current.Get("/HeaderContact/EmailFrom", "");
                string emailSubject = DictionaryPhraseRepository.Current.Get("/HeaderContact/EmailSubject", "");
                bool results = sendEmail(to, emailSubject, message, from);

                if (results)
                {
                    Log.Error("Email Sent- ", "");
                }
            }
            catch (Exception ex)
            {
                result = new { status = "1" };
                Log.Error("Failed to sent Email", ex.ToString());

            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult properties(string id)
        {
            Sitecore.Diagnostics.Log.Info("Get Propeties details Get method called for id:"+id, this);

            BookNow model = new BookNow();
            var products = new List<Product>();
            model.projectid = id;

            Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
            Data.Items.Item projects = db.GetItem(Templates.Properties.Datasource.Projects);
            List<SelectListItem> projectsList = projects.GetChildren().ToList().Select(x => new SelectListItem()
            {
                Text = x.Fields["Text"].Value,
                Value = x.Fields["Value"].Value
            }).ToList();

            if (projectsList.Where(p => p.Value == id).Any())
            {
                model.selectedProjectID = projectsList.Where(p => p.Value == id).FirstOrDefault().Value;
                model.ProjectName = projectsList.Where(p => p.Value == id).FirstOrDefault().Text;
            }

            try
            {
                SalesForceWrapper items = new SalesForceWrapper();
                //Get project details
                Sitecore.Diagnostics.Log.Info("Get Propeties details - project fields for id:" + id, this);
                var projectQueryResult = items.ExecuteQuery("SELECT Id,Name,Active__c,Address__c,Notes__c FROM Project__c where Id = '" + id + "'");
                if (projectQueryResult.done == false)
                {
                    Sitecore.Diagnostics.Log.Error("Error - Get Propeties details - project fields for id:" + id, this);
                }
                else
                {
                    var projectRecord = projectQueryResult.records[0];
                    var project = new Project
                    {
                        Id = projectRecord.Id,
                        Active__c = projectRecord.Active__c,
                        Address__c = projectRecord.Address__c,
                        Notes__c = projectRecord.Notes__c
                    };
                    model.Project_Address = projectRecord.Address__c;
                    model.Project_Notes = projectRecord.Notes__c;
                }
                Sitecore.Diagnostics.Log.Info("Success - Get Propeties details - project fields for id:" + id, this);
                //Get inventory details
                Sitecore.Diagnostics.Log.Info("Get Propeties details - inventory fields for id:" + id, this);
                var inventory = items.ExecuteQuery("SELECT+Id,Name,Unit_Status__c,Unit_Status_Code__c,Building__c,Floor_Code__c,Floor_Description__c,Building__r.Name,Project__c,Project_Name__c,Project_Product__c,Token_Amount__c,Total_Amount__c,Total_Carpet_Area__c+FROM+Product2+where+Project__c='" + id + "'+and+Unit_Status__C='Available'+and+Floor_Code__C>0");
                if (inventory == null) return null;
                foreach (var unit in inventory.records)
                {
                    var product = new Product
                    {
                        UnitId = unit.Id,
                        UnitName = unit.Name,
                        UnitCode = unit.Project_Product__c,
                        ProjectId = unit.Project__c,
                        ProjectName = unit.Project_Name__c,
                        BuildingId = unit.Building__c,
                        BuildingName = unit.Building__r.Name,
                        FloorCode = unit.Floor_Code__c,
                        FloorDescription = unit.Floor_Description__c,
                        TokenAmount = unit.Token_Amount__c != null ? unit.Token_Amount__c : 0,
                        TotalAmount = unit.Total_Amount__c != null ? unit.Total_Amount__c : 0,
                        CarpetArea = unit.Carpet_Area__c != null ? unit.Carpet_Area__c : 0,
                        Status = unit.Unit_Status__c,
                        StatusCode = unit.Unit_Status_Code__c != null ? unit.Unit_Status_Code__c : ""
                    };
                    products.Add(product);
                }
                model.Building = products.Where(p => p.Status == "Available").Select(x => new { x.BuildingId, x.BuildingName }).Distinct().OrderBy(x => x.BuildingName).ToDictionary(x => x.BuildingId, y => y.BuildingName);
                model.Floor = products.Where(p => p.Status == "Available").Where(x => x.BuildingId == model.Building.FirstOrDefault().Key).Select(x => new { x.FloorCode, x.FloorDescription }).Distinct().OrderBy(x => x.FloorDescription).ToDictionary(x => x.FloorCode, y => y.FloorDescription);
                model.Unit = products.Where(p => p.Status == "Available").Where(x => x.FloorCode == model.Floor.FirstOrDefault().Key).Select(x => new { x.UnitId, x.UnitName }).Distinct().OrderBy(x => x.UnitName).ToDictionary(x => x.UnitId, y => y.UnitName);
                model.Amount = products.Where(x => x.BuildingId == model.Building.FirstOrDefault().Key && x.FloorCode == model.Floor.FirstOrDefault().Key && x.UnitId == model.Unit.FirstOrDefault().Key).FirstOrDefault() == null ? 0.0 : System.Convert.ToDouble(products.Where(x => x.BuildingId == model.Building.FirstOrDefault().Key && x.FloorCode == model.Floor.FirstOrDefault().Key && x.UnitId == model.Unit.FirstOrDefault().Key).FirstOrDefault().TokenAmount);
                model.TotalAmount = products.Where(x => x.BuildingId == model.Building.FirstOrDefault().Key && x.FloorCode == model.Floor.FirstOrDefault().Key && x.UnitId == model.Unit.FirstOrDefault().Key).FirstOrDefault() == null ? 0.0 : System.Convert.ToDouble(products.Where(x => x.BuildingId == model.Building.FirstOrDefault().Key && x.FloorCode == model.Floor.FirstOrDefault().Key && x.UnitId == model.Unit.FirstOrDefault().Key).FirstOrDefault().TotalAmount);
                model.buildingid = model.Building.FirstOrDefault().Value;
                model.floorid = model.Floor.FirstOrDefault().Value;
                model.unitid = model.Unit.FirstOrDefault().Value;
                model.Projects = projectsList.Select(x => new { x.Value, x.Text }).Distinct().OrderBy(x => x.Text).ToDictionary(x => x.Value, y => y.Text);
                Sitecore.Diagnostics.Log.Info("Success - Get Propeties details - inventory fields for id:" + id, this);
            }
            catch (Exception e)
            {
                var innerException = e.InnerException;
                while (innerException != null)
                {
                    innerException = innerException.InnerException;
                }
                ViewBag.error = "Something went worng";
            }
            return View(model);
        }

        [HttpGet]
        public ActionResult GetBuildings(string building, string id)
        {
            SalesForceWrapper items = new SalesForceWrapper();
            BookNow model = new BookNow();
            var products = new List<Product>();
            var inventory = items.ExecuteQuery("SELECT+Id,Name,Unit_Status__c,Unit_Status_Code__c,Building__c,Floor_Code__c,Floor_Description__c,Building__r.Name,Project__c,Project_Name__c,Project_Product__c,Token_Amount__c,Total_Amount__c,Total_Carpet_Area__c+FROM+Product2+where+Project__c='" + id + "'+and+Unit_Status__C='Available'+and+Floor_Code__C>0");
            if (inventory == null) return null;
            foreach (var unit in inventory.records)
            {
                var product = new Product
                {
                    UnitId = unit.Id,
                    UnitName = unit.Name,
                    UnitCode = unit.Project_Product__c,
                    ProjectId = unit.Project__c,
                    ProjectName = unit.Project_Name__c,
                    BuildingId = unit.Building__c,
                    BuildingName = unit.Building__r.Name,
                    FloorCode = unit.Floor_Code__c,
                    FloorDescription = unit.Floor_Description__c,
                    TokenAmount = unit.Token_Amount__c != null ? unit.Token_Amount__c : 0,
                    TotalAmount = unit.Total_Amount__c != null ? unit.Total_Amount__c : 0,
                    CarpetArea = unit.Carpet_Area__c != null ? unit.Carpet_Area__c : 0,
                    Status = unit.Unit_Status__c,
                    StatusCode = unit.Unit_Status_Code__c != null ? unit.Unit_Status_Code__c : ""
                };
                products.Add(product);
            }
            model.Building = products.Select(x => new { x.BuildingId, x.BuildingName }).Distinct().OrderBy(x => x.BuildingName).ToDictionary(x => x.BuildingId, y => y.BuildingName);
            model.Floor = products.Where(x => x.BuildingId == building).Select(x => new { x.FloorCode, x.FloorDescription }).Distinct().OrderBy(x => x.FloorDescription).ToDictionary(x => x.FloorCode, y => y.FloorDescription);
            model.Unit = products.Where(x => x.BuildingId == building && x.FloorCode == model.Floor.FirstOrDefault().Key).Select(x => new { x.UnitId, x.UnitName }).Distinct().OrderBy(x => x.UnitName).ToDictionary(x => x.UnitId, y => y.UnitName);
            model.Amount = products.Where(x => x.BuildingId == building && x.FloorCode == model.Floor.FirstOrDefault().Key && x.UnitId == model.Unit.FirstOrDefault().Key).FirstOrDefault() == null ? 0.0 : System.Convert.ToDouble(products.Where(x => x.BuildingId == building && x.FloorCode == model.Floor.FirstOrDefault().Key && x.UnitId == model.Unit.FirstOrDefault().Key).FirstOrDefault().TokenAmount);
            model.buildingid = building;
            model.floorid = model.Floor.FirstOrDefault().Key;
            model.unitid = model.Unit.FirstOrDefault().Key;
            return PartialView("SearchProperties", model);
        }

        [HttpGet]
        public ActionResult GetUFloor(string building, string floor, string id)
        {
            SalesForceWrapper items = new SalesForceWrapper();
            BookNow model = new BookNow();
            var products = new List<Product>();
            var inventory = items.ExecuteQuery("SELECT+Id,Name,Unit_Status__c,Unit_Status_Code__c,Building__c,Floor_Code__c,Floor_Description__c,Building__r.Name,Project__c,Project_Name__c,Project_Product__c,Token_Amount__c,Total_Amount__c,Total_Carpet_Area__c+FROM+Product2+where+Project__c='" + id + "'+and+Unit_Status__C='Available'+and+Floor_Code__C>0");
            if (inventory == null) return null;
            foreach (var unit in inventory.records)
            {
                var product = new Product
                {
                    UnitId = unit.Id,
                    UnitName = unit.Name,
                    UnitCode = unit.Project_Product__c,
                    ProjectId = unit.Project__c,
                    ProjectName = unit.Project_Name__c,
                    BuildingId = unit.Building__c,
                    BuildingName = unit.Building__r.Name,
                    FloorCode = unit.Floor_Code__c,
                    FloorDescription = unit.Floor_Description__c,
                    TokenAmount = unit.Token_Amount__c != null ? unit.Token_Amount__c : 0,
                    TotalAmount = unit.Total_Amount__c != null ? unit.Total_Amount__c : 0,
                    CarpetArea = unit.Carpet_Area__c != null ? unit.Carpet_Area__c : 0,
                    Status = unit.Unit_Status__c,
                    StatusCode = unit.Unit_Status_Code__c != null ? unit.Unit_Status_Code__c : ""
                };
                products.Add(product);
            }
            model.Building = products.Select(x => new { x.BuildingId, x.BuildingName }).Distinct().OrderBy(x => x.BuildingName).ToDictionary(x => x.BuildingId, y => y.BuildingName);
            model.Floor = products.Where(x => x.BuildingId == building).Select(x => new { x.FloorCode, x.FloorDescription }).Distinct().OrderBy(x => x.FloorDescription).ToDictionary(x => x.FloorCode, y => y.FloorDescription);
            model.Unit = products.Where(x => x.BuildingId == building && x.FloorCode == floor).Select(x => new { x.UnitId, x.UnitName }).Distinct().OrderBy(x => x.UnitName).ToDictionary(x => x.UnitId, y => y.UnitName);
            model.Amount = products.Where(x => x.BuildingId == building && x.FloorCode == floor && x.UnitId == model.Unit.FirstOrDefault().Key).FirstOrDefault() == null ? 0.0 : System.Convert.ToDouble(products.Where(x => x.BuildingId == building && x.FloorCode == floor && x.UnitId == model.Unit.FirstOrDefault().Key).FirstOrDefault().TokenAmount);
            model.buildingid = building;
            model.floorid = floor;
            model.unitid = model.Unit.FirstOrDefault().Key;
            return PartialView("SearchProperties", model);
        }

        [HttpGet]
        public ActionResult GetAmount(string building, string floor, string units, string id)
        {
            SalesForceWrapper items = new SalesForceWrapper();
            BookNow model = new BookNow();
            var products = new List<Product>();
            var inventory = items.ExecuteQuery("SELECT+Id,Name,Unit_Status__c,Unit_Status_Code__c,Building__c,Floor_Code__c,Floor_Description__c,Building__r.Name,Project__c,Project_Name__c,Project_Product__c,Token_Amount__c,Total_Amount__c,Total_Carpet_Area__c+FROM+Product2+where+Project__c='" + id + "'+and+Unit_Status__C='Available'+and+Floor_Code__C>0");
            if (inventory == null) return null;
            foreach (var unit in inventory.records)
            {
                var product = new Product
                {
                    UnitId = unit.Id,
                    UnitName = unit.Name,
                    UnitCode = unit.Project_Product__c,
                    ProjectId = unit.Project__c,
                    ProjectName = unit.Project_Name__c,
                    BuildingId = unit.Building__c,
                    BuildingName = unit.Building__r.Name,
                    FloorCode = unit.Floor_Code__c,
                    FloorDescription = unit.Floor_Description__c,
                    TokenAmount = unit.Token_Amount__c != null ? unit.Token_Amount__c : 0,
                    TotalAmount = unit.Total_Amount__c != null ? unit.Total_Amount__c : 0,
                    CarpetArea = unit.Carpet_Area__c != null ? unit.Carpet_Area__c : 0,
                    Status = unit.Unit_Status__c,
                    StatusCode = unit.Unit_Status_Code__c != null ? unit.Unit_Status_Code__c : ""
                };
                products.Add(product);
            }
            model.Building = products.Select(x => new { x.BuildingId, x.BuildingName }).Distinct().OrderBy(x => x.BuildingName).ToDictionary(x => x.BuildingId, y => y.BuildingName);
            model.Floor = products.Where(x => x.BuildingId == building).Select(x => new { x.FloorCode, x.FloorDescription }).Distinct().OrderBy(x => x.FloorDescription).ToDictionary(x => x.FloorCode, y => y.FloorDescription);
            model.Unit = products.Where(x => x.BuildingId == building && x.FloorCode == floor).Select(x => new { x.UnitId, x.UnitName }).Distinct().OrderBy(x => x.UnitName).ToDictionary(x => x.UnitId, y => y.UnitName);
            model.Amount = products.Where(x => x.BuildingId == building && x.FloorCode == floor && x.UnitId == units).FirstOrDefault() == null ? 0.0 : System.Convert.ToDouble(products.Where(x => x.BuildingId == building && x.FloorCode == floor && x.UnitId == units).FirstOrDefault().TokenAmount);
            model.buildingid = building;
            model.floorid = floor;
            model.unitid = units;
            return PartialView("SearchProperties", model);
        }

        [HttpPost]
        public ActionResult properties(BookNow model)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Get Propeties details post method called",this);
                ModelState.Remove("transaction_id");
                ModelState.Remove("Building");
                ModelState.Remove("Floor");
                ModelState.Remove("Unit");
                var products = new List<Product>();
                if (ModelState.IsValid && model.Amount > 0)
                {
                    model.Amount = model.Amount;
                    model.msg = "ProjectId:" + model.projectid + "Project Name:" + model.ProjectName + ", BuildingId:" + model.buildingid + " FloorId:," + model.floorid + " UnitId," + model.unitid;

                    Sitecore.Diagnostics.Log.Info("Adding DB entry for: " + model.msg, this);
                    
                    using (AffordablePaymentHistoryDataContext objcontext = new AffordablePaymentHistoryDataContext())
                    {
                        model.transaction_id = EncryptionDecryption.GenerateRandomOrderId(string.Empty);
                        PaymentHistory objPayment = new PaymentHistory
                        {
                            UserId=model.Name,
                            TransactionId = model.transaction_id,
                            msg = "ProjectId:" + model.projectid + "Project Name:" + model.ProjectName + ", BuildingId:" + model.buildingid + " FloorId:," + model.floorid + " UnitId," + model.unitid,
                            Id = Guid.NewGuid(),
                            Amount = System.Convert.ToString(model.Amount),
                            Email = model.Email,
                            Mobile = model.MoblieNo,
                            UserType = "Guest",
                            GatewayType = "Insta-Mojo",
                            Created_Date = System.DateTime.Now,
                            RequestTime = System.DateTime.Now,
                            CreatedBy = model.Name,
                            AccountNumber = model.projectid,
                            PaymentType = "Token Amount",
                            PaymentMode="Online"
                        };
                        objcontext.PaymentHistories.InsertOnSubmit(objPayment);
                        objcontext.SubmitChanges();
                        Sitecore.Diagnostics.Log.Info("DB entry successfully added ID:" + objPayment.Id, this);
                    }

                    PaymentService objPaymentService = new PaymentService();
                    ResultPayment result = new ResultPayment();
                    result = objPaymentService.Payment(model);
                    if (result.IsSuccess)
                    {
                        Sitecore.Diagnostics.Log.Info("Properties Payment success info:" + "" + result.Message.ToString(), this);
                        return Redirect(result.Message);
                    }
                    else
                    {
                        ViewBag.ErrorMessage = result.Message;
                        Sitecore.Diagnostics.Log.Error("Properties Payment Error" + "" + result.Message.ToString(), this);
                        return View(model);
                    }
                }

                Sitecore.Diagnostics.Log.Info("Model is invalid, Amount="+model.Amount, this);
                SalesForceWrapper items = new SalesForceWrapper();
                //Get project details
                Sitecore.Diagnostics.Log.Info("Get Propeties details - project fields for id:" + model.projectid, this);
                var projectQueryResult = items.ExecuteQuery("SELECT Id,Name,Active__c,Address__c,Notes__c FROM Project__c where Id = '" + model.projectid + "'");
                if (projectQueryResult.done == false)
                {
                    Sitecore.Diagnostics.Log.Error("Error - Get Propeties details - project fields for id:" + model.projectid, this);
                }
                else
                {
                    var projectRecord = projectQueryResult.records[0];
                    var project = new Project
                    {
                        Id = projectRecord.Id,
                        Active__c = projectRecord.Active__c,
                        Address__c = projectRecord.Address__c,
                        Notes__c = projectRecord.Notes__c
                    };
                    model.Project_Address = projectRecord.Address__c;
                    model.Project_Notes = projectRecord.Notes__c;
                }
                Sitecore.Diagnostics.Log.Info("Success - Get Propeties details - project fields for id:" + model.projectid, this);
                var inventory = items.ExecuteQuery("SELECT+Id,Name,Unit_Status__c,Unit_Status_Code__c,Building__c,Floor_Code__c,Floor_Description__c,Building__r.Name,Project__c,Project_Name__c,Project_Product__c,Token_Amount__c,Total_Amount__c,Total_Carpet_Area__c+FROM+Product2+where+Project__c='" + model.projectid + "'+and+Unit_Status__C='Available'+and+Floor_Code__C>0");
                if (inventory == null) return null;
                foreach (var unit in inventory.records)
                {
                    var product = new Product
                    {
                        UnitId = unit.Id,
                        UnitName = unit.Name,
                        UnitCode = unit.Project_Product__c,
                        ProjectId = unit.Project__c,
                        ProjectName = unit.Project_Name__c,
                        BuildingId = unit.Building__c,
                        BuildingName = unit.Building__r.Name,
                        FloorCode = unit.Floor_Code__c,
                        FloorDescription = unit.Floor_Description__c,
                        TokenAmount = unit.Token_Amount__c != null ? unit.Token_Amount__c : 0,
                        TotalAmount = unit.Total_Amount__c != null ? unit.Total_Amount__c : 0,
                        CarpetArea = unit.Carpet_Area__c != null ? unit.Carpet_Area__c : 0,
                        Status = unit.Unit_Status__c,
                        StatusCode = unit.Unit_Status_Code__c != null ? unit.Unit_Status_Code__c : ""
                    };
                    products.Add(product);
                }
                model.Building = products.Where(p => p.Status == "Available").Select(x => new { x.BuildingId, x.BuildingName }).Distinct().OrderBy(x => x.BuildingName).ToDictionary(x => x.BuildingId, y => y.BuildingName);
                model.Floor = products.Where(p => p.Status == "Available").Where(x => x.BuildingId == model.buildingid).Select(x => new { x.FloorCode, x.FloorDescription }).Distinct().OrderBy(x => x.FloorDescription).ToDictionary(x => x.FloorCode, y => y.FloorDescription);
                model.Unit = products.Where(p => p.Status == "Available").Where(x => x.BuildingId == model.buildingid && x.FloorCode == model.floorid).Select(x => new { x.UnitId, x.UnitName }).Distinct().OrderBy(x => x.UnitName).ToDictionary(x => x.UnitId, y => y.UnitName);
                model.Amount = products.Where(x => x.BuildingId == model.buildingid && x.FloorCode == model.floorid && x.UnitId == model.unitid).FirstOrDefault() == null ? 0.0 : System.Convert.ToDouble(products.Where(x => x.BuildingId == model.buildingid && x.FloorCode == model.floorid && x.UnitId == model.unitid).FirstOrDefault().TokenAmount);
                model.buildingid = model.buildingid;
                model.floorid = model.floorid;
                model.unitid = model.unitid;
                return View(model);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("propeties post method", ex.Message);
            }
            return Redirect("properties");
        }


        public ActionResult SavePaymentHistoy(string payment_id, string payment_status, string id, string transaction_id)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Callback for Transaction ID:" + transaction_id + " ,Details:Payment_id=" + payment_id + " ,payment_status=" + payment_status + " ,id=" + id + ", transaction_id" + transaction_id, this);

                SalesForceWrapper items = new SalesForceWrapper();
                using (AffordablePaymentHistoryDataContext objcontext = new AffordablePaymentHistoryDataContext())
                {
                    
                    if (objcontext.PaymentHistories.Where(x => x.TransactionId == transaction_id).Any())
                    {
                        PaymentHistory objToEdit = objcontext.PaymentHistories.Where(x => x.TransactionId == transaction_id).FirstOrDefault();
                        objToEdit.OrderId = id;
                        objToEdit.Status = payment_status;
                        objToEdit.PaymentRef = payment_id;
                        objToEdit.Created_Date = System.DateTime.Now;
                        objToEdit.CreatedBy = objToEdit.UserId;
                        objToEdit.Modified_Date = System.DateTime.Now;
                        objToEdit.ModifiedBy = objToEdit.UserId;
                        objToEdit.ResponseMsg = "Payment_id=" + payment_id + " ,payment_status=" + payment_status + " ,id=" + id + ", transaction_id" + transaction_id;
                        
                        //Generate lead in salesforce
                        var lead = new Lead()
                        {
                            RecordTypeId = LeadRecordTypes.Residential,
                            FirstName = objToEdit.UserId,
                            LastName = objToEdit.UserId,
                            Email = objToEdit.Email,
                            MobilePhone = objToEdit.Mobile,
                            LeadSource = LeadSource.WebToLead,
                            Lead_Classification_New__c = LeadClassification.Hot,
                            PG_OrderId__c = id,
                            PG_Amount__c = System.Convert.ToDecimal(objToEdit.Amount),
                            PG_Payment_Mode__c = objToEdit.PaymentMode,
                            PG_Product__c = objToEdit.AccountNumber,
                            Remarks__c = objToEdit.ResponseMsg,
                            Global_Emp__c = GlobalEmployee.Emp1
                        };

                        var response = items.PostObject(SObjectNames.Lead, lead);
                        if (response != null)
                        {
                            //Lead id to be stroed in table in field= Responsecode
                            objToEdit.Responsecode = response.id;
                            Sitecore.Diagnostics.Log.Info("SFDC Lead created for Transaction ID:" + transaction_id + " ,Details:response_id=" + response.id + " Payment_id=" + payment_id + " ,payment_status=" + payment_status + " ,id=" + id + ", transaction_id" + transaction_id, this);
                        }
                        else
                        {
                            Sitecore.Diagnostics.Log.Error("SFDC Lead not created for Transaction ID:" + transaction_id + " ,Details:response_id=" + response.id + " Payment_id=" + payment_id + " ,payment_status=" + payment_status + " ,id=" + id + ", transaction_id" + transaction_id, this);
                        }
                        
                        //Update DB
                        objcontext.SubmitChanges();

                        Sitecore.Diagnostics.Log.Info("SFDC Lead PatchObject for Transaction ID:" + transaction_id + " ,Details:response_id=" + response.id + " Payment_id=" + payment_id + " ,payment_status=" + payment_status + " ,id=" + id + ", transaction_id" + transaction_id, this);
                        items.PatchObject(SObjectNames.Product, objToEdit.AccountNumber, new { Unit_Status__c = "Hold" });
                        Sitecore.Diagnostics.Log.Info("SFDC Lead PatchObject success for Transaction ID:" + transaction_id + " ,Details:response_id=" + response.id + " Payment_id=" + payment_id + " ,payment_status=" + payment_status + " ,id=" + id + ", transaction_id" + transaction_id, this);
                    }
                    else
                    {
                        Sitecore.Diagnostics.Log.Error("Entry not found with Transaction ID:" + transaction_id + " ,Details: Payment_id=" + payment_id + " ,payment_status=" + payment_status + " ,id=" + id + ", transaction_id" + transaction_id, this);
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.FetchBillPayment = DictionaryPhraseRepository.Current.Get("/Accounts/Pay Bill/API Issue", "There is some issue in fetching your data. Please try after some time.");
                Sitecore.Diagnostics.Log.Error("Enable to save Payment History on callback:" + ex.Message + ", Error with Transaction ID:" + transaction_id + " ,Details: Payment_id=" + payment_id + " ,payment_status=" + payment_status + " ,id=" + id + ", transaction_id" + transaction_id, this);
            }
            return View();
            //return Redirect("properties?projectname=" + url);
        }
    }
}