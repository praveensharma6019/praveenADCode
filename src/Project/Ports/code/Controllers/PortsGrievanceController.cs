using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Masters;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using Sitecore.Ports.Website.Models;
using Sitecore.Ports.Website.Provider;
using Sitecore.Ports.Website.Providers;
using Sitecore.Ports.Website.Services;
using Sitecore.SecurityModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Security.Policy;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Item = Sitecore.Data.Items.Item;

namespace Sitecore.Ports.Website.Controllers
{
    public class PortsGrievanceController : Controller
    {

        readonly BlobAPIService blobAPIService = new BlobAPIService();
        readonly CreateItemService createItemService = new CreateItemService();
        readonly UpdateItemService updateItemService = new UpdateItemService();
        public string EncryptionKey = DictionaryPhraseRepository.Current.Get("/GSM/EncryptionKey", "Tl;jld@456763909QPwOeiRuTy873XY7");
        public string EncryptionIV = DictionaryPhraseRepository.Current.Get("/GSM/EncryptionIV", "CEIVRAJWquG8iiMw");


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
        public ActionResult PortsGMSGrievanceBooking(PortsGMSGrievanceBookingModel pm, TemplatedModel templateModel)
        {
            Log.Info("In PortsGMSGrievanceBooking method:" + templateModel, templateModel);
            string msg = "";
            bool Validated = false;

            try
            {
                Recaptchav3Provider recaptchav2 = new Recaptchav3Provider();
                Validated = recaptchav2.IsReCaptchValid(pm.SubmitGcptchares);
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
                    Log.Info("In PortsGMSGrievanceBooking method Captch Validated:" + Validated, Validated);
                    if (!string.IsNullOrEmpty(pm.Subject) && (!Regex.IsMatch(pm.Subject, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                    {
                        msg = "Please Enter Valid Subject.";
                        Session["ErrorMsg"] = msg;
                    }
                    else if (!string.IsNullOrEmpty(pm.Brief) && (!Regex.IsMatch(pm.Brief, (@"^([a-zA-Z0-9]|[- @/\\.,#&!()])*$"))))
                    {
                        msg = "Please Enter Valid Grievance in brief.";
                        Session["ErrorMsg"] = msg;
                    }
                    else if (Session["PortsGMSUser"] != null)
                    {

                        Log.Info("In PortsGMSGrievanceBooking method  else if (Session[\"PortsGMSUser\"]):" + Session["PortsGMSUser"], Session["PortsGMSUser"]);
                        Guid RegistrationID = new Guid(Session["PortsGMSUser"].ToString());
                        PortsGMSDataContext rdb = new PortsGMSDataContext();
                        PortsGMSGrievanceBooking pgr = new PortsGMSGrievanceBooking();
                        PortsGms_Grievance_Booking_Attachment pgra = new PortsGms_Grievance_Booking_Attachment();

                        var StakeHolder = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationID).FirstOrDefault();
                        Log.Info("In PortsGMSGrievanceBooking method StakeHolder :" + StakeHolder, StakeHolder);
                        StakeHolder.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, StakeHolder.Email, EncryptionIV);

                        pm.Id = Guid.NewGuid();


                        #region Creating Item through ItemAPI
                        Database DBRef = Sitecore.Configuration.Factory.GetDatabase("web");
                        Log.Info($"connection cretaed PortsGMSGrievanceBooking Ports: {DBRef}", DBRef);
                        pm.Id = Guid.NewGuid();

                        FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();

                        var Document = pm.File;
                        string[] contenttypeExtenstion = new string[] { "application/pdf", "application/vnd.openxmlformats-officedocument.wordprocessingml.document", "application/msword", "image/jpeg" };

                        if (Document != null)
                        {
                            var Documentmime = Document.ContentType;
                            string filecontent = GetFileBinaryData(Document);
                            if (!contenttypeExtenstion.Contains(Documentmime.ToLower()))
                            {
                                msg = "Please upload only .pdf, .doc, .docx ,.jpeg , .jpg File in Attachment";
                                Session["ErrorMsg"] = msg;
                            }
                            else if (filecontent.StartsWith("JVBER") == false && filecontent.StartsWith("UEsDB") == false && filecontent.StartsWith("0M8R4") == false && filecontent.StartsWith("UklGRi7") == false)
                            {
                                msg = "Please upload only .pdf, .doc, .docx , .jpeg , .jpg File in Attachment";
                                Session["ErrorMsg"] = msg;
                            }
                            else
                            {
                                fileAttachmentModel = saveGrievanceBookingAttachmentFile(pm.File, pm.Id);
                                Log.Info($"PortsGMSGrievanceBooking Model fileAttachmentModel Ports: {fileAttachmentModel}", fileAttachmentModel);
                                templateModel.templateId = "{B1DDF8E7-E3E4-40AD-9463-01FB7AD82E52}";
                                templateModel.parentItem = DBRef.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"].ID.ToString();
                                templateModel.newItemName = "Grievance" + pm.Id.ToString();
                                var TodayDate = System.DateTime.Now;

                                TemplateFields TemplateFields = new TemplateFields();
                                var listItems = new List<FieldModel>
                        {
                            new FieldModel { FieldName = "Id", FieldValue = pm.Id.ToString() },
                            new FieldModel { FieldName = "RegistrationID", FieldValue = RegistrationID.ToString() },
                            new FieldModel { FieldName = "Title", FieldValue = "Test create" },
                            new FieldModel { FieldName = "Nature", FieldValue = pm.Nature },
                            new FieldModel { FieldName = "Location", FieldValue = pm.Location },
                            new FieldModel { FieldName = "Subject", FieldValue = pm.Subject },
                            new FieldModel { FieldName = "Company", FieldValue = pm.Company },
                            new FieldModel { FieldName = "WhoImpacted", FieldValue = pm.WhoImpacted },
                            new FieldModel { FieldName = "Brief", FieldValue = pm.Brief },
                            new FieldModel { FieldName = "UserType", FieldValue = "Level 0" },
                            new FieldModel { FieldName = "AssignedLevel", FieldValue = "0" },
                            new FieldModel { FieldName = "AssignedState", FieldValue = "Assigned" },
                            new FieldModel { FieldName = "Created_Date", FieldValue = TodayDate.ToString() },
                            new FieldModel { FieldName = "Modified_Date", FieldValue = TodayDate.ToString() },
                            new FieldModel { FieldName = "Status", FieldValue = pm.SaveAsDraft == "1" ? PortsGMSTemplates.GMSFlags.BookingDraft : PortsGMSTemplates.GMSFlags.BookingOpen },

                            new FieldModel { FieldName = "GrievanceFileID", FieldValue = fileAttachmentModel.GrievanceFileID },
                            new FieldModel { FieldName = "GrievanceFileGmsRegistrationId", FieldValue = fileAttachmentModel.GrievanceFileGmsRegistrationId},
                            new FieldModel { FieldName = "GrievanceFileName", FieldValue = fileAttachmentModel.GrievanceFileName },
                            new FieldModel { FieldName = "GrievanceFileContentType", FieldValue = fileAttachmentModel.GrievanceFileContentType },
                            new FieldModel { FieldName = "GrievanceFileBlobURL", FieldValue = fileAttachmentModel.GrievanceFileBlobURL },
                            new FieldModel { FieldName = "GrievanceFileCreatedDate", FieldValue = fileAttachmentModel.GrievanceFileCreatedDate }
                    };
                                TemplateFields.Fields = listItems;
                                templateModel.templateFields = TemplateFields;
                                Log.Info("Itme Send To service before TemplateFields:" + TemplateFields, TemplateFields);
                                Log.Info("Itme Send To service before TemplateModel:" + templateModel, templateModel);
                                bool status = createItemService.CreateItem(templateModel);
                                Log.Info("Itme Send To service After with staus received from create item service:" + status, status);
                                #endregion

                                msg = "Grievance Booking Save as draft";
                                Session["SuccessMsg"] = msg;
                                SendEmailGMSStackHolder(StakeHolder.Email, StakeHolder.Name, pm.Id, PortsGMSTemplates.GMSFlags.Booking);
                                if (pm.SaveAsDraft != "1")
                                {
                                    PortsGMSAssignGrievanceBookingToLevelZero(pm.Id);
                                    msg = "Grievance Booking Successfully Done";
                                    Session["SuccessMsg"] = msg;
                                }
                            }

                        }
                    }
                }
                catch (Exception ex)
                {
                    msg = "Somthing went wrong please try again after some time";
                    Session["ErrorMsg"] = msg;
                    Console.WriteLine(ex);
                    Log.Info("Item not created Sorry", ex.Message);
                }
            }
            else
            {
                msg = "Captcha Validation Failed!!!";
                Session["ErrorMsg"] = msg;
                Console.WriteLine("Captcha Validation Failed!!!!");
            }
            var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceBooking);

            return Redirect(page.Url());
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
        public void PortsGMSAssignGrievanceBookingToLevelZero(Guid Id = new Guid())
        {

            PortsGMSDataContext rdb = new PortsGMSDataContext();
            PortsGMSGrievanceBooking bookings = new PortsGMSGrievanceBooking();
            PortsGMSBookingAssignment bookingAssignment = new PortsGMSBookingAssignment();
            PortsGms_Registration Level0User = new PortsGms_Registration();

            Level0User = rdb.PortsGms_Registrations.Where(x => x.User_Type.Equals("level0")).FirstOrDefault();

            if (Level0User != null)
            {
                bookingAssignment.Id = Guid.NewGuid();
                #region Creating PortsGMSAssignGrievanceBookingToLevelZero Item through ItemAPI
                Database DBRef = Sitecore.Configuration.Factory.GetDatabase("web");
                TemplatedModel templateModel = new TemplatedModel();
                templateModel.templateId = "{15A2BC1A-9A80-45B7-AA74-85FD64357E7D}";
                templateModel.parentItem = DBRef.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"].ID.ToString();
                templateModel.newItemName = "Grievance" + bookingAssignment.Id.ToString();
                var TodayDate = System.DateTime.Now;

                TemplateFields TemplateFields = new TemplateFields();
                var listItems = new List<FieldModel>
                        {
                            new FieldModel { FieldName = "ID", FieldValue = bookingAssignment.Id.ToString()},
                            new FieldModel { FieldName = "GrievanceBookingId", FieldValue = Id.ToString() },
                            new FieldModel { FieldName = "RegistrationID", FieldValue = Level0User.Id.ToString() },
                            new FieldModel { FieldName = "LevelInfo", FieldValue = Level0User.User_Type },
                            new FieldModel { FieldName = "Response", FieldValue = PortsGMSTemplates.GMSFlags.RePendings },
                            new FieldModel { FieldName = "Status", FieldValue = PortsGMSTemplates.GrievanceStatus.Level0 },
                            new FieldModel { FieldName = "Department", FieldValue = Level0User.Department },
                            new FieldModel { FieldName = "UserType", FieldValue = "Level 0" },
                            new FieldModel { FieldName = "AssignedLevel", FieldValue = "0" },
                            new FieldModel { FieldName = "AssignedState", FieldValue = "Assigned" },
                            new FieldModel { FieldName = "CreatedDate", FieldValue = TodayDate.ToString() },
                            new FieldModel { FieldName = "Modified_Date", FieldValue = TodayDate.ToString() }
                         };
                TemplateFields.Fields = listItems;
                templateModel.templateFields = TemplateFields;
                bool status = createItemService.CreateItem(templateModel);
                #endregion
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
        public ActionResult PortsGmsGrievanceBookingOnBehalf(PortsGMSGrievanceBookingOnBeHalfModel pm, TemplatedModel templateModel)
        {
            string msg = "";
            try
            {
                if (Session["PortsGMSLevel0User"] != null)
                {
                    bool status = false;
                    Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
                    var TodayDate = System.DateTime.Now;
                    string FileURLValue = string.Empty;

                    Guid RegistrationID = new Guid(Session["PortsGMSLevel0User"].ToString());
                    PortsGMSDataContext rdb = new PortsGMSDataContext();
                    PortsGMSGrievanceBooking pgr = new PortsGMSGrievanceBooking();
                    PortsGms_Grievance_Booking_Attachment pgra = new PortsGms_Grievance_Booking_Attachment();
                    PortsGMSGrievanceBookingOnBehalf bof = new PortsGMSGrievanceBookingOnBehalf();
                    var Level0User = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationID).FirstOrDefault();

                    #region Creating PortsGMSGrievanceBooking Item through ItemAPI

                    pm.Id = Guid.NewGuid();
                    FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();
                    fileAttachmentModel = saveGrievanceBookingAttachmentFile(pm.File, pm.Id);
                    templateModel.templateId = "{B1DDF8E7-E3E4-40AD-9463-01FB7AD82E52}";
                    templateModel.parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"].ID.ToString();
                    templateModel.newItemName = "Grievance" + pm.Id.ToString();


                    TemplateFields TemplateFields = new TemplateFields();
                    var listItems = new List<FieldModel>
                        {
                            new FieldModel { FieldName = "Id", FieldValue = pm.Id.ToString() },
                            new FieldModel { FieldName = "RegistrationID", FieldValue = RegistrationID.ToString() },
                            new FieldModel { FieldName = "Nature", FieldValue = pm.Nature },
                            new FieldModel { FieldName = "Location", FieldValue = pm.Location },
                            new FieldModel { FieldName = "Subject", FieldValue = pm.Subject },
                            new FieldModel { FieldName = "Company", FieldValue = pm.Company },
                            new FieldModel { FieldName = "WhoImpacted", FieldValue = pm.WhoImpacted },
                            new FieldModel { FieldName = "Brief", FieldValue = pm.Brief },
                            new FieldModel { FieldName = "Created_Date", FieldValue = TodayDate.ToString() },
                            new FieldModel { FieldName = "Modified_Date", FieldValue = TodayDate.ToString() },
                            new FieldModel { FieldName = "Status", FieldValue = PortsGMSTemplates.GMSFlags.BookingOnBehalf },

                            new FieldModel { FieldName = "GrievanceFileID", FieldValue = fileAttachmentModel.GrievanceFileID },
                            new FieldModel { FieldName = "GrievanceFileGmsRegistrationId", FieldValue = fileAttachmentModel.GrievanceFileGmsRegistrationId},
                            new FieldModel { FieldName = "GrievanceFileName", FieldValue = fileAttachmentModel.GrievanceFileName },
                            new FieldModel { FieldName = "GrievanceFileContentType", FieldValue = fileAttachmentModel.GrievanceFileContentType },
                            new FieldModel { FieldName = "GrievanceFileBlobURL", FieldValue = fileAttachmentModel.GrievanceFileBlobURL },
                            new FieldModel { FieldName = "GrievanceFileCreatedDate", FieldValue = fileAttachmentModel.GrievanceFileCreatedDate }
                        };
                    TemplateFields.Fields = listItems;
                    templateModel.templateFields = TemplateFields;
                    status = createItemService.CreateItem(templateModel);
                    #endregion

                    #region Creating PortsGmsGrievanceBookingOnBehalf Item through ItemAPI

                    bof.Id = Guid.NewGuid();
                    templateModel.templateId = "{C02F5746-D884-49A9-86CA-6A8B8679913B}";
                    templateModel.parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGmsGrievanceBookingOnBehalf"].ID.ToString();
                    templateModel.newItemName = "Grievance" + bof.Id.ToString();

                    TemplateFields TemplateFields1 = new TemplateFields();
                    var listItems1 = new List<FieldModel>
                        {
                            new FieldModel { FieldName = "Id", FieldValue = bof.Id.ToString() },
                            new FieldModel { FieldName = "Name", FieldValue = RegistrationID.ToString() },
                            new FieldModel { FieldName = "Email", FieldValue = "Test create" },
                            new FieldModel { FieldName = "Mobile", FieldValue = pm.Nature },
                            new FieldModel { FieldName = "OnBehalf", FieldValue = pm.Location },
                            new FieldModel { FieldName = "CreatedBy", FieldValue = pm.Company },
                            new FieldModel { FieldName = "Created_Date", FieldValue = TodayDate.ToString() },
                            new FieldModel { FieldName = "Modified_Date", FieldValue = TodayDate.ToString() },
                            new FieldModel { FieldName = "BookingId", FieldValue = pm.Id.ToString() }
                        };
                    TemplateFields1.Fields = listItems1;
                    templateModel.templateFields = TemplateFields1;
                    status = createItemService.CreateItem(templateModel);
                    #endregion

                    saveGrievanceBookingAttachmentFile(pm.File, pm.Id);
                    msg = "Grievance Booking Successfully Done On Behalf Of Stakholder";
                    Session["SuccessMsg"] = msg;
                    SendEmailGMSStackHolder(pm.Email, pm.Name, pgr.Id, null, PortsGMSTemplates.GMSFlags.BookingOnBehalf);

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
        public ActionResult PortsGMSAdminDashbord()
        {
            if (Session["PortsGMSAdminUser"] == null)
            {
                return Redirect("/Grievance");
            }
            List<PortsGmsAdminDashbord> cityObj = new List<PortsGmsAdminDashbord>();
            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item AddGrievanceparentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item PortsGMSAssignGrievanceBparentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];
            List<PortsGms_Registration> UsersData;



            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                UsersData = rdb.PortsGms_Registrations.ToList();

                foreach (Item eachDistrictList in AddGrievanceparentItem.GetChildren())
                {
                    if (eachDistrictList != null)
                    {
                        foreach (var userItem in UsersData)
                        {
                            foreach (Item item in PortsGMSAssignGrievanceBparentItem.GetChildren())
                            {
                                if (eachDistrictList.Fields["Id"].Value == item.Fields["GrievanceBookingId"].Value && eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString())
                                {
                                    PortsGmsAdminDashbord ObjBook = new PortsGmsAdminDashbord();
                                    string ownerId = eachDistrictList.Fields["ID"].Value;
                                    ObjBook.Id = new Guid(ownerId);
                                    ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                    ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                    ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                    ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                    ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                    ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                    string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                    ObjBook.Created_Date = DateTime.Parse(txnDate);
                                    ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                    ObjBook.Name = userItem.Name;
                                    ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, userItem.Email, EncryptionIV);
                                    ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, userItem.Mobile, EncryptionIV);
                                    ObjBook.LevelInfo = item.Fields["LevelInfo"].Value;
                                    ObjBook.Response = item.Fields["Response"].Value;
                                    cityObj.Add(ObjBook);
                                }
                            }

                        }
                    }
                }
                PortsGmsAdminDashbord ObjBooking = new PortsGmsAdminDashbord();
                foreach (var item in cityObj)
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
                    ObjUsers.Email = item.Email;
                    ObjUsers.Mobile = item.Mobile;
                    ObjUsers.LevelInfo = item.LevelInfo;
                    ObjUsers.Response = item.Response;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }
                return View(ObjBooking);
            }
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

            List<PortsGmsAdminDashbord> cityObj = new List<PortsGmsAdminDashbord>();
            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item AddGrievanceparentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item PortsGMSAssignGrievanceBparentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];
            List<PortsGms_Registration> UsersData;

            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                PortsGmsAdminDashbord ObjBooking = new PortsGmsAdminDashbord();
                UsersData = rdb.PortsGms_Registrations.ToList();
                if (submit == "Reset")
                {
                    ObjBooking.FromDate = null;
                    ObjBooking.ToDate = null;
                    ObjBooking.Status = null;

                    foreach (Item eachDistrictList in AddGrievanceparentItem.GetChildren())
                    {
                        if (eachDistrictList != null)
                        {
                            foreach (var userItem in UsersData)
                            {
                                foreach (Item item in PortsGMSAssignGrievanceBparentItem.GetChildren())
                                {
                                    if (eachDistrictList.Fields["Id"].Value == item.Fields["GrievanceBookingId"].Value && eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString())
                                    {
                                        var RegistrationId = eachDistrictList.Fields["RegistrationId"].Value;
                                        var list = rdb.PortsGms_Registrations.Where(x => x.Id.ToString() == userItem.Id.ToString()).FirstOrDefault();
                                        PortsGmsAdminDashbord ObjBook = new PortsGmsAdminDashbord();
                                        string ownerId = eachDistrictList.Fields["ID"].Value;
                                        ObjBook.Id = new Guid(ownerId);
                                        ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                        ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                        ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                        ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                        ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                        ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                        string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                        ObjBook.Created_Date = DateTime.Parse(txnDate);
                                        ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                        ObjBook.Name = list.Name;
                                        ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                        ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                        ObjBook.LevelInfo = item.Fields["GrievanceBookingId"].Value;
                                        ObjBook.Response = item.Fields["GrievanceBookingId"].Value;
                                        cityObj.Add(ObjBook);
                                    }
                                }

                            }
                        }
                    }
                }
                else
                {
                    ObjBooking.FromDate = pm.FromDate;
                    ObjBooking.ToDate = pm.ToDate;
                    ObjBooking.Status = pm.Status;
                    foreach (Item eachDistrictList in AddGrievanceparentItem.GetChildren())
                    {
                        if (eachDistrictList != null && eachDistrictList.Fields["Status"].Value == pm.Status && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) >= FromDate
                                    && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) <= ToDate)
                        {
                            foreach (Item item in PortsGMSAssignGrievanceBparentItem.GetChildren())
                            {
                                foreach (var userItem in UsersData)
                                {

                                    if (eachDistrictList.Fields["Id"].Value == item.Fields["GrievanceBookingId"].Value && eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString())
                                    {
                                        var RegistrationId = eachDistrictList.Fields["RegistrationId"].Value;
                                        var list = rdb.PortsGms_Registrations.Where(x => x.Id.ToString() == userItem.Id.ToString()).FirstOrDefault();
                                        PortsGmsAdminDashbord ObjBook = new PortsGmsAdminDashbord();
                                        string ownerId = eachDistrictList.Fields["ID"].Value;
                                        ObjBook.Id = new Guid(ownerId);
                                        ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                        ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                        ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                        ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                        ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                        ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                        string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                        ObjBook.Created_Date = DateTime.Parse(txnDate);
                                        ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                        ObjBook.Name = list.Name;
                                        ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                        ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                        ObjBook.LevelInfo = item.Fields["GrievanceBookingId"].Value;
                                        ObjBook.Response = item.Fields["GrievanceBookingId"].Value;
                                        cityObj.Add(ObjBook);
                                    }
                                }
                            }
                        }
                    }
                }
                foreach (var item in cityObj)
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
                    ObjUsers.Email = item.Email;
                    ObjUsers.Mobile = item.Mobile;
                    ObjUsers.LevelInfo = item.LevelInfo;
                    ObjUsers.Response = item.Response;

                    ObjBooking.AllBookings.Add(ObjUsers);
                }


                return View(ObjBooking);
            }
        }

        [HttpGet]
        public ActionResult PortsGMSStakHolderDashbord()
        {
            if (Session["PortsGMSUser"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSUser"].ToString());
            List<PortsGms_Registration> UsersData;

            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                UsersData = rdb.PortsGms_Registrations.ToList();
                List<PortsGmsStakHolderDashbord> cityObj = new List<PortsGmsStakHolderDashbord>();
                Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
                foreach (Item eachDistrictList in parentItem.GetChildren())
                {
                    foreach (var userItem in UsersData)
                    {
                        if (RegistrationId == userItem.Id)
                        {
                            if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString())
                            {
                                //  var list = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
                                PortsGmsStakHolderDashbord ObjBook = new PortsGmsStakHolderDashbord();
                                string ownerId = eachDistrictList.Fields["ID"].Value;
                                ObjBook.Id = new Guid(ownerId);
                                ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                ObjBook.Created_Date = DateTime.Parse(txnDate);
                                ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                ObjBook.Name = userItem.Name;
                                ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, userItem.Email, EncryptionIV);
                                ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, userItem.Mobile, EncryptionIV);
                                cityObj.Add(ObjBook);

                            }
                        }
                    }
                }

                PortsGmsStakHolderDashbord ObjBooking = new PortsGmsStakHolderDashbord();
                foreach (var item in cityObj)
                {
                    PortsGmsStakHolderDashbord ObjUsers = new PortsGmsStakHolderDashbord();
                    ObjUsers.Id = item.Id;
                    ObjUsers.encId = HttpUtility.UrlEncode(GSM_EncryptDecrypt.EncryptString(EncryptionKey, item.Id.ToString(), EncryptionIV));
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
            List<PortsGms_Registration> UsersData;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                UsersData = rdb.PortsGms_Registrations.ToList();
                List<PortsGmsStakHolderDashbord> cityObj = new List<PortsGmsStakHolderDashbord>();
                List<PortsGmsStakHolderDashbord> cityObj1 = new List<PortsGmsStakHolderDashbord>();
                List<PortsGmsStakHolderDashbord> cityObj2 = new List<PortsGmsStakHolderDashbord>();
                Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
                PortsGmsStakHolderDashbord ObjBooking = new PortsGmsStakHolderDashbord();
                foreach (Item eachDistrictList in parentItem.GetChildren())
                {
                    foreach (var userItem in UsersData)
                    {
                        if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString() && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) >= FromDate && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) <= ToDate)
                        {
                            var list = rdb.PortsGms_Registrations.Where(x => x.Id.ToString() == userItem.Id.ToString()).FirstOrDefault();
                            PortsGmsStakHolderDashbord ObjBook = new PortsGmsStakHolderDashbord();
                            string ownerId = eachDistrictList.Fields["ID"].Value;
                            ObjBook.Id = new Guid(ownerId);
                            ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                            ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                            ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                            ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                            ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                            ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                            string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                            ObjBook.Created_Date = DateTime.Parse(txnDate);
                            ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                            ObjBook.Name = list.Name;
                            ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                            ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                            cityObj.Add(ObjBook);
                        }
                    }
                }

                foreach (var item in cityObj)
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
                    ObjUsers.Email = item.Email;
                    ObjUsers.Mobile = item.Mobile;
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }

                if (pm.BookingStatus != null && submit == "Reset")
                {

                    PortsGmsStakHolderDashbord ObjBooking2 = new PortsGmsStakHolderDashbord();
                    foreach (Item eachDistrictList in parentItem.GetChildren())
                    {
                        foreach (var userItem in UsersData)
                        {
                            if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString())
                            {
                                var list = rdb.PortsGms_Registrations.Where(x => x.Id == Guid.Parse(userItem.Id.ToString())).FirstOrDefault();
                                PortsGmsStakHolderDashbord ObjBook = new PortsGmsStakHolderDashbord();
                                string ownerId = eachDistrictList.Fields["ID"].Value;
                                ObjBook.Id = new Guid(ownerId);
                                ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                ObjBook.Created_Date = DateTime.Parse(txnDate);
                                ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                ObjBook.Name = list.Name;
                                ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                cityObj2.Add(ObjBook);

                            }
                        }
                    }
                    ObjBooking2.FromDate = null;
                    ObjBooking2.ToDate = null;
                    ObjBooking2.Status = null;
                    foreach (var item in cityObj2)
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
                        ObjUsers.Email = item.Email;
                        ObjUsers.Mobile = item.Mobile;
                        ObjUsers.Status = item.Status;
                        ObjBooking2.AllBookings.Add(ObjUsers);
                    }
                    return View(ObjBooking2);
                }

                if (pm.BookingStatus != null)
                {

                    PortsGmsStakHolderDashbord ObjBooking1 = new PortsGmsStakHolderDashbord();
                    foreach (Item eachDistrictList in parentItem.GetChildren())
                    {
                        foreach (var userItem in UsersData)
                        {
                            if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString() && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) >= FromDate && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) <= ToDate && eachDistrictList.Fields["Status"].Value == pm.BookingStatus)
                            {
                                var list = rdb.PortsGms_Registrations.Where(x => x.Id == Guid.Parse(userItem.Id.ToString())).FirstOrDefault();
                                PortsGmsStakHolderDashbord ObjBook = new PortsGmsStakHolderDashbord();
                                string ownerId = eachDistrictList.Fields["ID"].Value;
                                ObjBook.Id = new Guid(ownerId);
                                ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                ObjBook.Created_Date = DateTime.Parse(txnDate);
                                ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                ObjBook.Name = list.Name;
                                ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                cityObj1.Add(ObjBook);
                            }
                        }
                    }

                    ObjBooking1.FromDate = pm.FromDate;
                    ObjBooking1.ToDate = pm.ToDate;
                    ObjBooking1.BookingStatus = pm.BookingStatus;

                    foreach (var item in cityObj1)
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
                        ObjUsers.Email = item.Email;
                        ObjUsers.Mobile = item.Mobile;
                        ObjUsers.Status = item.Status;
                        ObjBooking1.AllBookings.Add(ObjUsers);
                    }
                    return View(ObjBooking1);
                }


                return View(ObjBooking);
            }
        }

        [HttpGet]
        public ActionResult PortsGMSLevel0Dashbord()
        {
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {

                if (Session["PortsGMSLevel0User"] == null)
                {
                    return Redirect("/Grievance");
                }
                //     Guid RegistrationId = new Guid(Session["PortsGMSLevel0User"].ToString());
                List<PortsGms_Registration> UsersData;
                UsersData = rdb.PortsGms_Registrations.ToList();

                List<PortsGmsLevel0Dashbord> cityObj = new List<PortsGmsLevel0Dashbord>();
                Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
                foreach (Item eachDistrictList in parentItem.GetChildren())
                {
                    foreach (var item in UsersData)
                    {
                        if (eachDistrictList.Fields["RegistrationId"].Value == item.Id.ToString())
                        {
                            if ((eachDistrictList.Fields["UserType"].Value == "Level 0" && eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingOpen)
                            || eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingClosed
                            || eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingOnBehalf
                            || (eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingReOpen && eachDistrictList.Fields["UserType"].Value == "Level 0")
                            || eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.Response)
                            {
                                var list = rdb.PortsGms_Registrations.Where(x => x.Id == Guid.Parse(eachDistrictList.Fields["RegistrationId"].Value)).FirstOrDefault();
                                PortsGmsLevel0Dashbord ObjBook = new PortsGmsLevel0Dashbord();
                                string ownerId = eachDistrictList.Fields["ID"].Value;
                                ObjBook.Id = new Guid(ownerId);
                                ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                ObjBook.Created_Date = DateTime.Parse(txnDate);
                                ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                ObjBook.Name = list.Name;
                                ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                cityObj.Add(ObjBook);

                            }
                        }
                    }
                }
                PortsGmsLevel0Dashbord ObjBooking = new PortsGmsLevel0Dashbord();
                foreach (var item in cityObj)
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

                return View(ObjBooking);
            }
        }
        [HttpPost]
        public ActionResult PortsGMSLevel0Dashbord(PortsGmsLevel0Dashbord pm, string submit)
        {
            if (Session["PortsGMSLevel0User"] == null)
            {
                return Redirect("/Grievance");
            }

            Guid RegistrationId = new Guid(Session["PortsGMSLevel0User"].ToString());

            DateTime FromDate = pm.FromDate != null ? DateTime.Parse(pm.FromDate.ToString()).Date : System.DateTime.Now;
            DateTime ToDate = pm.ToDate != null ? DateTime.Parse(pm.ToDate.ToString()).Date : System.DateTime.Now;
            List<PortsGms_Registration> UsersData;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                UsersData = rdb.PortsGms_Registrations.ToList();
                List<PortsGmsLevel0Dashbord> cityObj = new List<PortsGmsLevel0Dashbord>();
                List<PortsGmsLevel0Dashbord> cityObj1 = new List<PortsGmsLevel0Dashbord>();
                List<PortsGmsLevel0Dashbord> cityObj2 = new List<PortsGmsLevel0Dashbord>();
                List<PortsGmsLevel0Dashbord> cityObj3 = new List<PortsGmsLevel0Dashbord>();
                List<PortsGmsLevel0Dashbord> cityObj4 = new List<PortsGmsLevel0Dashbord>();
                List<PortsGmsLevel0Dashbord> cityObj5 = new List<PortsGmsLevel0Dashbord>();
                List<PortsGmsLevel0Dashbord> cityObj6 = new List<PortsGmsLevel0Dashbord>();
                Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
                PortsGmsLevel0Dashbord ObjBooking = new PortsGmsLevel0Dashbord();

                foreach (Item eachDistrictList in parentItem.GetChildren())
                {
                    foreach (var userItem in UsersData)
                    {
                        if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString())
                        {
                            if ((eachDistrictList.Fields["UserType"].Value == "Level 0" && eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingOpen)
                                 || eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingClosed
                                 || eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingOnBehalf
                                 || (eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingReOpen && eachDistrictList.Fields["UserType"].Value == "Level 0")
                                 || eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.Response
                                 && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) >= FromDate.Date
                                 && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) <= ToDate.Date
                                 && eachDistrictList.Fields["Status"].Value == pm.Status)
                            {
                                var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                                PortsGmsLevel0Dashbord ObjBook = new PortsGmsLevel0Dashbord();
                                string ownerId = eachDistrictList.Fields["ID"].Value;
                                ObjBook.Id = new Guid(ownerId);
                                ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                ObjBook.Created_Date = DateTime.Parse(txnDate);
                                ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                ObjBook.Name = list.Name;
                                ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                cityObj.Add(ObjBook);
                            }
                        }
                    }
                }
                foreach (var item in cityObj)
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
                    ObjUsers.Status = item.Status;
                    ObjUsers.Email = item.Email;
                    ObjUsers.Mobile = item.Mobile;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }

                if (pm.Status != null && submit != "Reset")
                {
                    if (pm.Status == PortsGMSTemplates.GMSFlags.BookingOpen)
                    {
                        PortsGmsLevel0Dashbord ObjBook1 = new PortsGmsLevel0Dashbord();
                        foreach (Item eachDistrictList in parentItem.GetChildren())
                        {
                            foreach (var userItem in UsersData)
                            {
                                if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString())
                                {
                                    if ((eachDistrictList.Fields["UserType"].Value == "Level 0"
                                              && eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingOpen)
                                              && eachDistrictList.Fields["Status"].Value == pm.Status
                                              && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) >= FromDate.Date
                                              && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) <= ToDate.Date)
                                    {
                                        var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                                        PortsGmsLevel0Dashbord ObjBook = new PortsGmsLevel0Dashbord();
                                        string ownerId = eachDistrictList.Fields["ID"].Value;
                                        ObjBook.Id = new Guid(ownerId);
                                        ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                        ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                        ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                        ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                        ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                        ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                        string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                        ObjBook.Created_Date = DateTime.Parse(txnDate);
                                        ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                        ObjBook.Name = list.Name;
                                        ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                        ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                        cityObj1.Add(ObjBook);
                                    }
                                }
                            }
                        }
                        ObjBook1.FromDate = pm.FromDate;
                        ObjBook1.ToDate = pm.ToDate;
                        ObjBook1.BookingStatus = pm.Status;
                        foreach (var item in cityObj1)
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
                            ObjUsers.Status = item.Status;
                            ObjUsers.Email = item.Email;
                            ObjUsers.Mobile = item.Mobile;
                            ObjBook1.AllBookings.Add(ObjUsers);
                        }
                        return View(ObjBook1);
                    }
                    else if (pm.Status == PortsGMSTemplates.GMSFlags.BookingReOpen)
                    {
                        PortsGmsLevel0Dashbord ObjBook2 = new PortsGmsLevel0Dashbord();
                        foreach (Item eachDistrictList in parentItem.GetChildren())
                        {
                            foreach (var userItem in UsersData)
                            {
                                if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString())
                                {
                                    if ((eachDistrictList.Fields["UserType"].Value == "Level 0"
                                    && eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingReOpen)
                                    && eachDistrictList.Fields["Status"].Value == pm.Status
                                    && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) >= FromDate.Date
                                    && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) <= ToDate.Date)
                                    {
                                        var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                                        PortsGmsLevel0Dashbord ObjBook = new PortsGmsLevel0Dashbord();
                                        string ownerId = eachDistrictList.Fields["ID"].Value;
                                        ObjBook.Id = new Guid(ownerId);
                                        ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                        ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                        ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                        ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                        ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                        ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                        string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                        ObjBook.Created_Date = DateTime.Parse(txnDate);
                                        ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                        ObjBook.Name = list.Name;
                                        ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                        ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                        cityObj2.Add(ObjBook);
                                    }
                                }
                            }
                        }
                        ObjBook2.FromDate = pm.FromDate;
                        ObjBook2.ToDate = pm.ToDate;
                        ObjBook2.BookingStatus = pm.Status;
                        foreach (var item in cityObj2)
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
                            ObjUsers.Status = item.Status;
                            ObjUsers.Email = item.Email;
                            ObjUsers.Mobile = item.Mobile;
                            ObjBook2.AllBookings.Add(ObjUsers);
                        }
                        return View(ObjBook2);
                    }
                    else if (pm.Status == PortsGMSTemplates.GMSFlags.BookingClosed)
                    {
                        PortsGmsLevel0Dashbord ObjBook3 = new PortsGmsLevel0Dashbord();
                        foreach (Item eachDistrictList in parentItem.GetChildren())
                        {
                            foreach (var userItem in UsersData)
                            {
                                if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString())
                                {
                                    if ((eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingClosed)
                                && eachDistrictList.Fields["Status"].Value == pm.Status
                                && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) >= FromDate.Date
                                && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) <= ToDate.Date)
                                    {
                                        var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                                        PortsGmsLevel0Dashbord ObjBook = new PortsGmsLevel0Dashbord();
                                        string ownerId = eachDistrictList.Fields["ID"].Value;
                                        ObjBook.Id = new Guid(ownerId);
                                        ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                        ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                        ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                        ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                        ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                        ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                        string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                        ObjBook.Created_Date = DateTime.Parse(txnDate);
                                        ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                        ObjBook.Name = list.Name;
                                        ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                        ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                        cityObj3.Add(ObjBook);
                                    }
                                }
                            }
                        }
                        ObjBook3.FromDate = pm.FromDate;
                        ObjBook3.ToDate = pm.ToDate;
                        ObjBook3.BookingStatus = pm.Status;
                        foreach (var item in cityObj3)
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
                            ObjUsers.Status = item.Status;
                            ObjUsers.Email = item.Email;
                            ObjUsers.Mobile = item.Mobile;
                            ObjBook3.AllBookings.Add(ObjUsers);
                        }
                        return View(ObjBook3);
                    }
                    else if (pm.Status == PortsGMSTemplates.GMSFlags.BookingOnBehalf)
                    {
                        PortsGmsLevel0Dashbord ObjBook4 = new PortsGmsLevel0Dashbord();
                        foreach (Item eachDistrictList in parentItem.GetChildren())
                        {
                            foreach (var userItem in UsersData)
                            {
                                if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString())
                                {
                                    if ((eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingOnBehalf)
                                   && eachDistrictList.Fields["Status"].Value == pm.Status
                                   && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) >= FromDate.Date
                                   && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) <= ToDate.Date)
                                    {
                                        var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                                        PortsGmsLevel0Dashbord ObjBook = new PortsGmsLevel0Dashbord();
                                        string ownerId = eachDistrictList.Fields["ID"].Value;
                                        ObjBook.Id = new Guid(ownerId);
                                        ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                        ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                        ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                        ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                        ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                        ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                        string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                        ObjBook.Created_Date = DateTime.Parse(txnDate);
                                        ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                        ObjBook.Name = list.Name;
                                        ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                        ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                        cityObj4.Add(ObjBook);
                                    }
                                }
                            }
                        }
                        ObjBook4.FromDate = pm.FromDate;
                        ObjBook4.ToDate = pm.ToDate;
                        ObjBook4.BookingStatus = pm.Status;
                        foreach (var item in cityObj4)
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
                            ObjUsers.Status = item.Status;
                            ObjUsers.Email = item.Email;
                            ObjUsers.Mobile = item.Mobile;
                            ObjBook4.AllBookings.Add(ObjUsers);
                        }
                        return View(ObjBook4);
                    }
                    else if (pm.Status == PortsGMSTemplates.GMSFlags.Response)
                    {
                        PortsGmsLevel0Dashbord ObjBook5 = new PortsGmsLevel0Dashbord();
                        foreach (Item eachDistrictList in parentItem.GetChildren())
                        {
                            foreach (var userItem in UsersData)
                            {
                                if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString())
                                {
                                    if ((eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.Response)
                                   && eachDistrictList.Fields["Status"].Value == pm.Status
                                   && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) >= FromDate.Date
                                   && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) <= ToDate.Date)
                                    {
                                        var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                                        PortsGmsLevel0Dashbord ObjBook = new PortsGmsLevel0Dashbord();
                                        string ownerId = eachDistrictList.Fields["ID"].Value;
                                        ObjBook.Id = new Guid(ownerId);
                                        ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                        ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                        ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                        ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                        ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                        ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                        string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                        ObjBook.Created_Date = DateTime.Parse(txnDate);
                                        ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                        ObjBook.Name = list.Name;
                                        ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                        ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                        cityObj5.Add(ObjBook);
                                    }
                                }
                            }
                        }
                        ObjBook5.FromDate = pm.FromDate;
                        ObjBook5.ToDate = pm.ToDate;
                        ObjBook5.BookingStatus = pm.Status;
                        foreach (var item in cityObj5)
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
                            ObjUsers.Status = item.Status;
                            ObjUsers.Email = item.Email;
                            ObjUsers.Mobile = item.Mobile;
                            ObjBook5.AllBookings.Add(ObjUsers);
                        }
                        return View(ObjBook5);

                    }

                }

                if (submit == "Reset")
                {
                    PortsGmsLevel0Dashbord ObjBook6 = new PortsGmsLevel0Dashbord();
                    foreach (Item eachDistrictList in parentItem.GetChildren())
                    {
                        foreach (var userItem in UsersData)
                        {
                            if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString())
                            {
                                if ((eachDistrictList.Fields["UserType"].Value == "Level 0" && eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingOpen)
                                    || eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingClosed
                                    || eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingOnBehalf
                                    || (eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.BookingReOpen && eachDistrictList.Fields["UserType"].Value == "Level 0")
                                    || eachDistrictList.Fields["Status"].Value == PortsGMSTemplates.GMSFlags.Response
                                    && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) >= FromDate.Date
                                    && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) <= ToDate.Date)
                                {

                                    var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                                    PortsGmsLevel0Dashbord ObjBook = new PortsGmsLevel0Dashbord();
                                    string ownerId = eachDistrictList.Fields["ID"].Value;
                                    ObjBook.Id = new Guid(ownerId);
                                    ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                    ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                    ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                    ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                    ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                    ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                    string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                    ObjBook.Created_Date = DateTime.Parse(txnDate);
                                    ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                    ObjBook.Name = list.Name;
                                    ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                    ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                    cityObj6.Add(ObjBook);
                                }
                            }
                        }
                    }
                    ObjBook6.FromDate = null;
                    ObjBook6.ToDate = null;
                    ObjBook6.Status = null;
                    foreach (var item in cityObj6)
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
                        ObjUsers.Status = item.Status;
                        ObjUsers.Email = item.Email;
                        ObjUsers.Mobile = item.Mobile;
                        ObjBook6.AllBookings.Add(ObjUsers);
                    }

                    return this.View(ObjBook6);
                }

                return this.View(ObjBooking);
            }
        }


        [HttpGet]
        public ActionResult PortsGMSLevel1Dashbord()
        {
            if (Session["PortsGMSLevel1User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel1User"].ToString());
            List<PortsGms_Registration> UsersData;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                var level1UserDetails = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
                UsersData = rdb.PortsGms_Registrations.ToList();

                List<PortsGmsLevel1Dashbord> cityObj = new List<PortsGmsLevel1Dashbord>();
                Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];

                foreach (Item eachDistrictList in parentItem.GetChildren())
                {
                    foreach (var userItem in UsersData)
                    {
                        if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString() &&
                        eachDistrictList.Fields["UserType"].Value == "Level 1"
                        && eachDistrictList.Fields["BuisnessGroup"].Value == level1UserDetails.BusinessGroup)
                        {
                            var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                            PortsGmsLevel1Dashbord ObjBook = new PortsGmsLevel1Dashbord();
                            string ownerId = eachDistrictList.Fields["ID"].Value;
                            ObjBook.Id = new Guid(ownerId);
                            ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                            ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                            ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                            ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                            ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                            ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                            string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                            ObjBook.Created_Date = DateTime.Parse(txnDate);
                            ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                            ObjBook.Name = list.Name;
                            ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                            ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                            cityObj.Add(ObjBook);
                        }
                    }
                }

                PortsGmsLevel1Dashbord ObjBooking = new PortsGmsLevel1Dashbord();
                foreach (var item in cityObj)
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
                    ObjUsers.Email = item.Email;
                    ObjUsers.Mobile = item.Mobile;
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }
                return View(ObjBooking);
            }
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
            List<PortsGms_Registration> UsersData;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                UsersData = rdb.PortsGms_Registrations.ToList();
                var level1UserDetails = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
                List<PortsGmsLevel1Dashbord> cityObj = new List<PortsGmsLevel1Dashbord>();
                Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];



                PortsGmsLevel1Dashbord ObjBooking = new PortsGmsLevel1Dashbord();
                if (submit == "Reset")
                {
                    foreach (Item eachDistrictList in parentItem.GetChildren())
                    {
                        foreach (var userItem in UsersData)
                        {
                            if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString() &&
                            eachDistrictList.Fields["UserType"].Value == "Level 1"
                            && eachDistrictList.Fields["BuisnessGroup"].Value == level1UserDetails.BusinessGroup)
                            {
                                var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                                PortsGmsLevel1Dashbord ObjBook = new PortsGmsLevel1Dashbord();
                                string ownerId = eachDistrictList.Fields["ID"].Value;
                                ObjBook.Id = new Guid(ownerId);
                                ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                ObjBook.Created_Date = DateTime.Parse(txnDate);
                                ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                ObjBook.Name = list.Name;
                                ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                cityObj.Add(ObjBook);

                            }
                        }
                    }

                    ObjBooking.FromDate = null;
                    ObjBooking.ToDate = null;
                    ObjBooking.Status = null;
                }
                else
                {
                    foreach (Item eachDistrictList in parentItem.GetChildren())
                    {
                        foreach (var userItem in UsersData)
                        {
                            if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString() &&
                            eachDistrictList.Fields["UserType"].Value == "Level 1"
                            && eachDistrictList.Fields["BuisnessGroup"].Value == level1UserDetails.BusinessGroup
                            && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) >= FromDate.Date
                            && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) <= ToDate.Date)
                            {
                                var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                                PortsGmsLevel1Dashbord ObjBook = new PortsGmsLevel1Dashbord();
                                string ownerId = eachDistrictList.Fields["ID"].Value;
                                ObjBook.Id = new Guid(ownerId);
                                ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                ObjBook.Created_Date = DateTime.Parse(txnDate);
                                ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                ObjBook.Name = list.Name;
                                ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                cityObj.Add(ObjBook);
                            }
                        }
                    }
                    ObjBooking.FromDate = pm.FromDate;
                    ObjBooking.ToDate = pm.ToDate;
                    ObjBooking.Status = pm.Status;
                }

                foreach (var item in cityObj)
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
                    ObjUsers.Email = item.Email;
                    ObjUsers.Mobile = item.Mobile;
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }
                return this.View(ObjBooking);
            }
        }

        [HttpGet]
        public ActionResult PortsGMSLevel2Dashbord()
        {
            if (Session["PortsGMSLevel2User"] == null)
            {
                return Redirect("/Grievance");
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel2User"].ToString());
            List<PortsGms_Registration> UsersData;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                var level2UserDetails = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
                UsersData = rdb.PortsGms_Registrations.ToList();
                List<PortsGmsLevel3Dashbord> cityObj = new List<PortsGmsLevel3Dashbord>();
                Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];

                foreach (Item eachDistrictList in parentItem.GetChildren())
                {
                    foreach (var userItem in UsersData)
                    {
                        if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString() &&
                        eachDistrictList.Fields["UserType"].Value == "Level 2"
                        && eachDistrictList.Fields["BuisnessGroup"].Value == level2UserDetails.BusinessGroup
                        && eachDistrictList.Fields["SiteHead"].Value == level2UserDetails.SiteHead
                        && eachDistrictList.Fields["HOType"].Value == level2UserDetails.HO)
                        {
                            var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                            PortsGmsLevel3Dashbord ObjBook = new PortsGmsLevel3Dashbord();
                            string ownerId = eachDistrictList.Fields["ID"].Value;
                            ObjBook.Id = new Guid(ownerId);
                            ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                            ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                            ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                            ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                            ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                            ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                            string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                            ObjBook.Created_Date = DateTime.Parse(txnDate);
                            ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                            ObjBook.Name = list.Name;
                            ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                            ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                            cityObj.Add(ObjBook);

                        }
                    }
                }
                PortsGmsLevel2Dashbord ObjBooking = new PortsGmsLevel2Dashbord();
                foreach (var item in cityObj)
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
                    ObjUsers.Email = item.Email;
                    ObjUsers.Mobile = item.Mobile;
                    ObjUsers.Status = item.Status;
                    ObjBooking.AllBookings.Add(ObjUsers);
                }
                return View(ObjBooking);
            }
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
            List<PortsGms_Registration> UsersData;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                UsersData = rdb.PortsGms_Registrations.ToList();
                var level2UserDetails = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();

                List<PortsGmsLevel3Dashbord> cityObj = new List<PortsGmsLevel3Dashbord>();
                Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
                PortsGmsLevel2Dashbord ObjBooking = new PortsGmsLevel2Dashbord();

                if (submit == "Reset")
                {
                    PortsGmsLevel2Dashbord ObjBooking1 = new PortsGmsLevel2Dashbord();
                    foreach (Item eachDistrictList in parentItem.GetChildren())
                    {
                        foreach (var userItem in UsersData)
                        {
                            if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString() &&
                            eachDistrictList.Fields["UserType"].Value == "Level 2"
                            && eachDistrictList.Fields["BuisnessGroup"].Value == level2UserDetails.BusinessGroup
                            && eachDistrictList.Fields["SiteHead"].Value == level2UserDetails.SiteHead
                            && eachDistrictList.Fields["HOType"].Value == level2UserDetails.HO)
                            {
                                var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                                PortsGmsLevel3Dashbord ObjBook = new PortsGmsLevel3Dashbord();
                                string ownerId = eachDistrictList.Fields["ID"].Value;
                                ObjBook.Id = new Guid(ownerId);
                                ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                ObjBook.Created_Date = DateTime.Parse(txnDate);
                                ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                ObjBook.Name = list.Name;
                                ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                cityObj.Add(ObjBook);
                            }
                        }
                    }
                    ObjBooking.FromDate = null;
                    ObjBooking.ToDate = null;
                    ObjBooking.Status = null;
                }
                else
                {
                    foreach (Item eachDistrictList in parentItem.GetChildren())
                    {
                        foreach (var userItem in UsersData)
                        {
                            if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString() &&
                            eachDistrictList.Fields["UserType"].Value == "Level 2"
                            && eachDistrictList.Fields["BuisnessGroup"].Value == level2UserDetails.BusinessGroup
                            && eachDistrictList.Fields["SiteHead"].Value == level2UserDetails.SiteHead
                            && eachDistrictList.Fields["HOType"].Value == level2UserDetails.HO
                            && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) >= FromDate.Date
                            && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) <= ToDate.Date)
                            {
                                var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                                PortsGmsLevel3Dashbord ObjBook = new PortsGmsLevel3Dashbord();
                                string ownerId = eachDistrictList.Fields["ID"].Value;
                                ObjBook.Id = new Guid(ownerId);
                                ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                ObjBook.Created_Date = DateTime.Parse(txnDate);
                                ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                ObjBook.Name = list.Name;
                                ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                cityObj.Add(ObjBook);
                            }
                        }
                    }
                    ObjBooking.FromDate = pm.FromDate;
                    ObjBooking.ToDate = pm.ToDate;
                    ObjBooking.Status = pm.Status;
                }


                foreach (var item in cityObj)
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
            List<PortsGms_Registration> UsersData;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                var level3UserDetails = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
                UsersData = rdb.PortsGms_Registrations.ToList();

                List<PortsGmsLevel3Dashbord> cityObj = new List<PortsGmsLevel3Dashbord>();
                Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
                int flag = 0;
                foreach (Item eachDistrictList in parentItem.GetChildren())
                {
                    foreach (var userItem in UsersData)
                    {
                        if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString() &&
                              eachDistrictList.Fields["UserType"].Value == "Level 3" && eachDistrictList.Fields["BuisnessGroup"].Value == level3UserDetails.BusinessGroup
                              && eachDistrictList.Fields["SiteHead"].Value == level3UserDetails.SiteHead
                              && eachDistrictList.Fields["HOType"].Value == level3UserDetails.HO
                              && eachDistrictList.Fields["PointMan"].Value == level3UserDetails.Team)
                        {
                            var list = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
                            PortsGmsLevel3Dashbord ObjBook = new PortsGmsLevel3Dashbord();
                            string ownerId = eachDistrictList.Fields["ID"].Value;
                            ObjBook.Id = new Guid(ownerId);
                            ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                            ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                            ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                            ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                            ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                            ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                            string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                            ObjBook.Created_Date = DateTime.Parse(txnDate);
                            ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                            ObjBook.Name = list.Name;
                            ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                            ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                            cityObj.Add(ObjBook);
                        }
                    }
                }
                var abc = flag;

                PortsGmsLevel3Dashbord ObjBooking = new PortsGmsLevel3Dashbord();
                foreach (var item in cityObj)
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
            List<PortsGms_Registration> UsersData;
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                var level3UserDetails = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
                UsersData = rdb.PortsGms_Registrations.ToList();
                List<PortsGmsLevel3Dashbord> cityObj = new List<PortsGmsLevel3Dashbord>();
                Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
                Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
                PortsGmsLevel3Dashbord ObjBooking = new PortsGmsLevel3Dashbord();

                if (submit == "Reset")
                {
                    PortsGmsLevel3Dashbord ObjBooking2 = new PortsGmsLevel3Dashbord();
                    foreach (Item eachDistrictList in parentItem.GetChildren())
                    {
                        foreach (var userItem in UsersData)
                        {
                            if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString() &&
                            eachDistrictList.Fields["UserType"].Value == "Level 3" && eachDistrictList.Fields["BuisnessGroup"].Value == level3UserDetails.BusinessGroup
                            && eachDistrictList.Fields["SiteHead"].Value == level3UserDetails.SiteHead
                            && eachDistrictList.Fields["HOType"].Value == level3UserDetails.HO
                            && eachDistrictList.Fields["PointMan"].Value == level3UserDetails.Team)
                            {
                                var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                                PortsGmsLevel3Dashbord ObjBook = new PortsGmsLevel3Dashbord();
                                string ownerId = eachDistrictList.Fields["ID"].Value;
                                ObjBook.Id = new Guid(ownerId);
                                ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                ObjBook.Created_Date = DateTime.Parse(txnDate);
                                ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                ObjBook.Name = list.Name;
                                ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                cityObj.Add(ObjBook);
                            }
                        }
                    }
                    ObjBooking2.FromDate = null;
                    ObjBooking2.ToDate = null;
                    ObjBooking2.Status = null;
                }
                else
                {
                    foreach (Item eachDistrictList in parentItem.GetChildren())
                    {
                        foreach (var userItem in UsersData)
                        {
                            if (eachDistrictList.Fields["RegistrationId"].Value == userItem.Id.ToString() &&
                            eachDistrictList.Fields["UserType"].Value == "Level 3" && eachDistrictList.Fields["BuisnessGroup"].Value == level3UserDetails.BusinessGroup
                            && eachDistrictList.Fields["SiteHead"].Value == level3UserDetails.SiteHead
                            && eachDistrictList.Fields["HOType"].Value == level3UserDetails.HO
                            && eachDistrictList.Fields["PointMan"].Value == level3UserDetails.Team
                            && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) >= FromDate.Date
                            && DateTime.Parse(eachDistrictList.Fields["Created_Date"].Value) <= ToDate.Date)
                            {
                                var list = rdb.PortsGms_Registrations.Where(x => x.Id == userItem.Id).FirstOrDefault();
                                PortsGmsLevel3Dashbord ObjBook = new PortsGmsLevel3Dashbord();
                                string ownerId = eachDistrictList.Fields["ID"].Value;
                                ObjBook.Id = new Guid(ownerId);
                                ObjBook.Location = eachDistrictList.Fields["Location"].Value;
                                ObjBook.Nature = eachDistrictList.Fields["Nature"].Value;
                                ObjBook.Subject = eachDistrictList.Fields["Subject"].Value;
                                ObjBook.Company = eachDistrictList.Fields["Company"].Value;
                                ObjBook.WhoImpacted = eachDistrictList.Fields["WhoImpacted"].Value;
                                ObjBook.Brief = eachDistrictList.Fields["Brief"].Value;
                                string txnDate = eachDistrictList.Fields["Created_Date"].Value;
                                ObjBook.Created_Date = DateTime.Parse(txnDate);
                                ObjBook.Status = eachDistrictList.Fields["Status"].Value;
                                ObjBook.Name = list.Name;
                                ObjBook.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Email, EncryptionIV);
                                ObjBook.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, list.Mobile, EncryptionIV);
                                cityObj.Add(ObjBook);

                            }
                        }
                    }
                    ObjBooking.FromDate = pm.FromDate;
                    ObjBooking.ToDate = pm.ToDate;
                    ObjBooking.Status = pm.Status;
                }

                foreach (var item in cityObj)
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

            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item OnBehalfParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGmsGrievanceBookingOnBehalf"];
            Item AssignGrievanceBookingParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];
            var Grievance = parentItem.GetChildren().Where(x => x["ID"] == GrievanceId.ToString()).FirstOrDefault();

            var StakHolder = rdb.PortsGms_Registrations.Where(x => x.Id.ToString() == Grievance["RegistrationId"]).FirstOrDefault();

            if (Grievance["Status"] == PortsGMSTemplates.GMSFlags.BookingOnBehalf)
            {
                var OnbehalfStakHolder = OnBehalfParentItem.GetChildren().Where(x => x["BookingId"] == GrievanceId.ToString()).FirstOrDefault();
            }
            var Level0User = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
            var level0reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level0" && x["UserType"] == "Level 0" && x["Response"] == "Completed").OrderByDescending(x => x["CreatedDate"]).FirstOrDefault();
            var level1reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level1" && x["Response"] == "Completed").OrderByDescending(x => x["CreatedDate"]).FirstOrDefault();
            var level2reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level2" && x["Response"] == "Completed").OrderByDescending(x => x["CreatedDate"]).FirstOrDefault();
            var level3reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level3" && x["Response"] == "Completed").OrderByDescending(x => x["CreatedDate"]).FirstOrDefault();
            int submitToStakeholderRecordCount = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["Id"] == GrievanceId.ToString() && x["UserType"] == PortsGMSTemplates.UserType.Stakeholder && x["AssignedLevel"] == "stakeholder" && x["Status"] == PortsGMSTemplates.GMSFlags.Response).Count();
            ViewBag.SubmitToStakeholderRecordCount = submitToStakeholderRecordCount;

            var Level0Response = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level0").OrderByDescending(x => x["CreatedDate"]).FirstOrDefault();

            var AssignedLevel0Response = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["AssignedLevel"] == "0" && x["Status"] == null).OrderByDescending(x => x["CreatedDate"]).FirstOrDefault();
            if (Grievance["Status"] == PortsGMSTemplates.GMSFlags.BookingOnBehalf)
            {
                var OnbehalfStakHolder = OnBehalfParentItem.GetChildren().Where(x => x["BookingId"] == GrievanceId.ToString()).FirstOrDefault();
                ObjBooking.Name = OnbehalfStakHolder["Name"];
                ObjBooking.Email = OnbehalfStakHolder["Email"];
                ObjBooking.Mobile = OnbehalfStakHolder["Mobile"];
            }
            else
            {
                if (StakHolder != null)
                {
                    ObjBooking.Name = StakHolder.Name;
                    ObjBooking.DOB = StakHolder.DOB;
                    ObjBooking.Gender = StakHolder.Gender;
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
                ObjBooking.Id = Guid.Parse(Grievance["Id"]);
                ObjBooking.Location = Grievance["Location"];
                ObjBooking.Subject = Grievance["Subject"];
                ObjBooking.Brief = Grievance["Brief"];
                ObjBooking.WhoImpacted = Grievance["WhoImpacted"];
                ObjBooking.Company = Grievance["Company"];
                ObjBooking.Status = Grievance["Status"];
                ObjBooking.UserType = Grievance["UserType"];
                ObjBooking.Remarks = Grievance["StakeholderRemarks"];
                ViewBag.AssignedLevel = Grievance["AssignedLevel"];
            }
            if (Level0User != null)
            {
                ObjBooking.Department = Level0User.Department;
                ObjBooking.Level0UserName = Level0User.Name;

            }


            if (level0reply != null && level1reply != null && level2reply != null && level3reply != null)
            {
                ObjBooking.Level0UserName = level0reply["Username"];
                ObjBooking.Level1UserName = level1reply["Username"];
                ObjBooking.Level2UserName = level2reply["Username"];
                ObjBooking.Level3UserName = level3reply["Username"];
                ObjBooking.Level0Comment = level0reply["Comment"];
                ObjBooking.Level1Comment = level1reply["Comment"];
                ObjBooking.Level2Comment = level2reply["Comment"];
                ObjBooking.Level3Comment = level3reply["Comment"];
                ObjBooking.BusinessGroup = level0reply["BusinessGroup"];
                ObjBooking.SiteHead = level1reply["SiteHead"];
                ObjBooking.HO = level1reply["HOType"];
                ObjBooking.PointMan = level2reply["PointMan"];
                ObjBooking.FinalComment = Grievance["FinalComment"];
                ObjBooking.Remarks = Grievance["StakeholderRemarks"];
            }
            //response
            if (ObjBooking.Status == PortsGMSTemplates.GMSFlags.Response && ViewBag.SubmitToStakeholderRecordCount > 0 &&
                Level0Response != null && level1reply != null && level2reply != null && level3reply != null)
            {
                ObjBooking.Level0UserName = Level0Response["Username"];
                ObjBooking.Level1UserName = level1reply["Username"];
                ObjBooking.Level2UserName = level2reply["Username"];
                ObjBooking.Level3UserName = level3reply["Username"];
                ObjBooking.Level0Comment = Level0Response["Comment"];
                ObjBooking.Level1Comment = level1reply["Comment"];
                ObjBooking.Level2Comment = level2reply["Comment"];
                ObjBooking.Level3Comment = level3reply["Comment"];
                ObjBooking.BusinessGroup = Level0Response["BusinessGroup"];
                ObjBooking.SiteHead = level1reply["SiteHead"];
                ObjBooking.HO = level1reply["HOType"];
                ObjBooking.PointMan = level2reply["PointMan"];
                ObjBooking.FinalComment = Grievance["FinalComment"];
                ObjBooking.Remarks = Grievance["StakeholderRemarks"];
            }


            if (Grievance != null && ObjBooking.Status != null)
            {
                if (ObjBooking.Status == PortsGMSTemplates.GMSFlags.BookingOpen && (Grievance["AssignedLevel"] == "1" || Grievance["AssignedLevel"] == "2" || Grievance["AssignedLevel"] == "3"))
                {
                    ObjBooking.Level0UserName = AssignedLevel0Response["Username"];
                    ObjBooking.Level0Comment = AssignedLevel0Response["Comment"];
                }
            }


            if (ObjBooking.Status == PortsGMSTemplates.GMSFlags.BookingOpen && Grievance["AssignedLevel"] == "Re-assigned request 0")
            {

                ObjBooking.Level0UserName = Level0Response["Username"];
                ObjBooking.Level1UserName = level1reply["Username"];
                ObjBooking.Level2UserName = level2reply["Username"];
                ObjBooking.Level3UserName = level3reply["Username"];
                ObjBooking.Level0Comment = Level0Response["Comment"];
                ObjBooking.Level1Comment = level1reply["Comment"];
                ObjBooking.Level2Comment = level2reply["Comment"];
                ObjBooking.Level3Comment = level3reply["Comment"];
                ObjBooking.BusinessGroup = Level0Response["BusinessGroup"];
                ObjBooking.SiteHead = level1reply["SiteHead"];
                ObjBooking.HO = level1reply["HOType"];
                ObjBooking.PointMan = level2reply["PointMan"];
            }
            ViewBag.AssignedState = Grievance["AssignedState"];

            var StakeholderFinalComment = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level0" && x["UserType"] == "stakeholder").OrderBy(x => x["CreatedDate"]).ToList();
            if (StakeholderFinalComment != null && StakeholderFinalComment.Count > 0)
            {
                foreach (var item in StakeholderFinalComment)
                {
                    StakeholderModel obj = new StakeholderModel();
                    obj.FinalComment = item["Comment"];
                    obj.Created_Date = DateTime.Parse(item["CreatedDate"]);
                    var username = rdb.PortsGms_Registrations.Where(x => x.Id.ToString() == item["RegistrationId"]).Select(x => x.Name).FirstOrDefault();
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

            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item GrievanceBookingparentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item OnBehalfParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGmsGrievanceBookingOnBehalf"];
            Item AssignGrievanceBookingParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];

            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                string msg = "";
                try
                {
                    var Username = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).Select(x => x.Name).FirstOrDefault();
                    // update the grivence booking record
                    var GrievanceDetails = GrievanceBookingparentItem.GetChildren().Where(x => x["Id"] == GrievanceId.ToString()).FirstOrDefault();
                    using (new SecurityDisabler())
                    {
                        try
                        {
                            if (GrievanceDetails != null)
                            {

                                #region Updating PortsGMSGrievanceBooking Item through ItemAPI
                                TemplatedModel templateModel = new TemplatedModel();
                                templateModel.itemPath = GrievanceDetails.ID.ToString();
                                TemplateFields TemplateFields = new TemplateFields();
                                var listItems = new List<FieldModel>
                                  {
                                new FieldModel { FieldName = "UserType", FieldValue = "Level 1" },
                                new FieldModel { FieldName = "AssignedLevel", FieldValue =  "1" },
                                 new FieldModel { FieldName = "AssignedState", FieldValue =  "Assigned" },
                                new FieldModel { FieldName = "BuisnessGroup", FieldValue =   pm.BusinessGroup },
                                new FieldModel { FieldName = "Modified_Date", FieldValue = System.DateTime.Now.ToString() }
                                 };
                                TemplateFields.Fields = listItems;
                                templateModel.templateFields = TemplateFields;
                                Log.Info("Itme Update To service before update the grivence booking record", TemplateFields);
                                bool status = updateItemService.UpdateItem(templateModel);
                                Log.Info("Itme Update To service After update the grivence booking record", status);
                                #endregion
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + GrievanceDetails.Paths.FullPath + ": " + ex.Message, this);
                            GrievanceDetails.Editing.CancelEdit();
                        }


                        //update the  existing booking assignment record
                        var existingBookingAssignment = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == GrievanceId.ToString() && x["Response"] == PortsGMSTemplates.GMSFlags.RePendings).FirstOrDefault();
                        try
                        {
                            if (existingBookingAssignment != null)
                            {

                                #region Updating PortsGMSAssignGrievanceBookingToLevelZero Item through ItemAPI
                                TemplatedModel templateModel = new TemplatedModel();
                                templateModel.itemPath = existingBookingAssignment.ID.ToString();
                                TemplateFields TemplateFields = new TemplateFields();
                                var listItems = new List<FieldModel>
                                  {
                                new FieldModel { FieldName = "Comment", FieldValue = pm.Comment },
                                new FieldModel { FieldName = "BusinessGroup", FieldValue = pm.BusinessGroup },
                                 new FieldModel { FieldName = "SiteHead",FieldValue = pm.SiteHead },
                                new FieldModel { FieldName = "UserType", FieldValue = "Level 0" },
                                new FieldModel { FieldName = "LevelInfo", FieldValue = "level0" },
                                 new FieldModel { FieldName = "Response", FieldValue = PortsGMSTemplates.GMSFlags.Completed },
                                new FieldModel { FieldName = "ModifiedDate", FieldValue =  System.DateTime.Now.ToString() },
                                 new FieldModel { FieldName = "Username", FieldValue = Username },
                                 };
                                TemplateFields.Fields = listItems;
                                templateModel.templateFields = TemplateFields;
                                Log.Info("Itme Update To service before update the  existing booking assignment record", TemplateFields);
                                bool status = updateItemService.UpdateItem(templateModel);
                                Log.Info("Itme Update To service After update the  existing booking assignment record", status);
                                #endregion
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + existingBookingAssignment.Paths.FullPath + ": " + ex.Message, this);
                            existingBookingAssignment.Editing.CancelEdit();
                        }


                        var tempId = "{15A2BC1A-9A80-45B7-AA74-85FD64357E7D}";
                        TemplateItem template = masterDb.GetTemplate(tempId);
                        Item newItem = AssignGrievanceBookingParentItem.Add("Grievance" + pm.Id.ToString(), template);

                        newItem.Editing.BeginEdit();
                        try
                        {
                            var StatusValue = "";
                            if (existingBookingAssignment != null)
                            {
                                StatusValue = PortsGMSTemplates.GrievanceStatus.Level0;
                            }
                            else
                            {
                                StatusValue = PortsGMSTemplates.GrievanceStatus.Level1;
                            }

                            #region Creating PortsGMSAssignGrievanceBookingToLevelZero Item through ItemAPI
                            Database DBRef = Sitecore.Configuration.Factory.GetDatabase("web");
                            TemplatedModel templateModel = new TemplatedModel();
                            templateModel.templateId = "{15A2BC1A-9A80-45B7-AA74-85FD64357E7D}";
                            templateModel.parentItem = DBRef.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"].ID.ToString();
                            templateModel.newItemName = "Grievance" + pm.Id.ToString();
                            var TodayDate = System.DateTime.Now;

                            TemplateFields TemplateFields = new TemplateFields();
                            var listItems = new List<FieldModel>
                        {
                            new FieldModel { FieldName = "ID", FieldValue = Guid.NewGuid().ToString()},
                            new FieldModel { FieldName = "GrievanceBookingId", FieldValue = GrievanceId.ToString() },
                            new FieldModel { FieldName = "RegistrationID", FieldValue = existingBookingAssignment["RegistrationId"] },
                            new FieldModel { FieldName = "LevelInfo", FieldValue =  "level1"},
                            new FieldModel { FieldName = "Response", FieldValue = PortsGMSTemplates.GMSFlags.RePendings },
                            new FieldModel { FieldName = "Status", FieldValue = StatusValue },
                            new FieldModel { FieldName = "Department", FieldValue = existingBookingAssignment["Department"] },
                            new FieldModel { FieldName = "UserType", FieldValue = "Level 1" },
                            new FieldModel { FieldName = "AssignedLevel", FieldValue = "1" },
                            new FieldModel { FieldName = "AssignedState", FieldValue = "Assigned" },
                            new FieldModel { FieldName = "CreatedDate", FieldValue = TodayDate.ToString() }
                         };
                            TemplateFields.Fields = listItems;
                            templateModel.templateFields = TemplateFields;
                            Log.Info("Itme Update To service before update the  existing booking assignment record", TemplateFields);
                            bool status = createItemService.CreateItem(templateModel);
                            Log.Info("Itme Update To service After update the  existing booking assignment record", status);
                            #endregion
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + newItem.Paths.FullPath + ": " + ex.Message, this);

                            newItem.Editing.CancelEdit();
                        }
                    }

                    msg = "Grievance is successfully assigned to Level 1";
                    Session["SuccessMsg0"] = msg;
                }
                catch (Exception ex)
                {
                    msg = "Somthing went wrong please try again after some time";
                    Session["ErrorMsg0"] = msg;
                    Console.WriteLine(ex);
                    Log.Info("Sorry Itme Update To PortsGMSLevel0Reply", ex.Message);
                }
            }
            var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel0Dashbord);



            return Redirect(page.Url());
        }

        [HttpGet]
        public ActionResult PortsGMSStakHolderGrivanceView()
        {
            if (Session["PortsGMSUser"] == null)
            {
                return Redirect("/Grievance");
            }
            var reqid = GSM_EncryptDecrypt.DecryptString(EncryptionKey, Request.QueryString["id"].ToString(), EncryptionIV);

            if (reqid == null)
            {
                return Redirect("/Grievance");
            }
            Guid GrievanceId = new Guid(reqid);

            PortsGMSDataContext rdb = new PortsGMSDataContext();
            PortsGmsStakHolderDashbord ObjBooking = new PortsGmsStakHolderDashbord();

            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item GrievanceBookingParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item OnBehalfParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGmsGrievanceBookingOnBehalf"];
            Item AssignGrievanceBookingParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];
            var Grievance = GrievanceBookingParentItem.GetChildren().Where(x => x["ID"] == GrievanceId.ToString()).FirstOrDefault();

            var StakHolder = rdb.PortsGms_Registrations.Where(x => x.Id.ToString() == Grievance["RegistrationId"]).FirstOrDefault();
            var AssignedHistroy = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == GrievanceId.ToString()).ToList();

            int submitToStakeholderRecordCount = GrievanceBookingParentItem.GetChildren().Where(x => x["Id"] == GrievanceId.ToString() && x["UserType"] == PortsGMSTemplates.UserType.Stakeholder && x["AssignedLevel"] == "Stakeholder" && x["Status"] == PortsGMSTemplates.GMSFlags.Response).Count();
            ViewBag.SubmitToStakeholderRecordCount = submitToStakeholderRecordCount;

            if (ViewBag.SubmitToStakeholderRecordCount > 1)
            {
                var level0reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level0").FirstOrDefault();
                var level1reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level1").FirstOrDefault();
                var level2reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level2").FirstOrDefault();
                var level3reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level3").FirstOrDefault();
                ObjBooking.Level0Comment = level0reply["Comment"];
                ObjBooking.Level1Comment = level1reply["Comment"];
                ObjBooking.Level2Comment = level2reply["Comment"];
                ObjBooking.Level3Comment = level3reply["Comment"];
                ObjBooking.Level0UserName = level0reply["Username"];
                ObjBooking.Level1UserName = level1reply["Username"];
                ObjBooking.Level2UserName = level2reply["Username"];
                ObjBooking.Level3UserName = level3reply["Username"];
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
                ObjBooking.FinalComment = Grievance["FinalComment"];
                ObjBooking.Assignedlevel = Grievance["AssignedLevel"];
                ObjBooking.Id = Guid.Parse(Grievance["Id"]);
                ObjBooking.Location = Grievance["Location"];
                ObjBooking.Subject = Grievance["Subject"];
                ObjBooking.Brief = Grievance["Brief"];
                ObjBooking.WhoImpacted = Grievance["WhoImpacted"];
                ObjBooking.Company = Grievance["Company"];
                ObjBooking.Status = Grievance["Status"];
                ObjBooking.UserType = Grievance["UserType"];
                ObjBooking.StakeholderRemarks = Grievance["StakeholderRemarks"];
            }
            var StakeholderFinalComment = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level0" && x["UserType"] == "Stakeholder").OrderBy(x => x["CreatedDate"]).ToList();
            if (StakeholderFinalComment != null && StakeholderFinalComment.Count > 0)
            {
                foreach (var item in StakeholderFinalComment)
                {
                    StakeholderModel obj = new StakeholderModel();
                    obj.FinalComment = item["Comment"];
                    obj.Created_Date = DateTime.Parse(item["CreatedDate"]);
                    var username = rdb.PortsGms_Registrations.Where(x => x.Id.ToString() == item["RegistrationId"]).Select(x => x.Name).FirstOrDefault();
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

            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item GrievanceBookingparentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item OnBehalfParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGmsGrievanceBookingOnBehalf"];
            Item AssignGrievanceBookingParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];

            var Grievance = GrievanceBookingparentItem.GetChildren().Where(x => x["ID"] == GrievanceId.ToString()).FirstOrDefault();
            var StakHolder = rdb.PortsGms_Registrations.Where(x => x.Id == Guid.Parse(Grievance["RegistrationId"])).FirstOrDefault();

            try
            {
                if (Grievance != null)
                {
                    TemplatedModel templateModel = new TemplatedModel();
                    templateModel.itemPath = Grievance.ID.ToString();
                    TemplateFields TemplateFields = new TemplateFields();
                    bool status = false;
                    if (chkResponse == true)
                    {
                        #region Updating PortsGMSGrievanceBooking Item through ItemAPI                        
                        var listItems = new List<FieldModel>
                                  {
                                new FieldModel { FieldName = "ChkResponse", FieldValue = "True" },
                                new FieldModel { FieldName = "Status", FieldValue =  PortsGMSTemplates.GMSFlags.BookingClosed }
                                 };
                        TemplateFields.Fields = listItems;
                        templateModel.templateFields = TemplateFields;
                        status = updateItemService.UpdateItem(templateModel);
                        #endregion
                    }
                    else
                    {

                        #region Updating PortsGMSGrievanceBooking Item through ItemAPI                      
                        var listItems = new List<FieldModel>
                                  {
                                new FieldModel { FieldName = "UserType", FieldValue = "Level 0" },
                                new FieldModel { FieldName = "AssignedLevel", FieldValue =  "0" },
                                new FieldModel { FieldName = "ChkResponse", FieldValue = "false" },
                                new FieldModel { FieldName = "Status", FieldValue =  PortsGMSTemplates.GMSFlags.BookingClosed }
                                 };
                        TemplateFields.Fields = listItems;
                        templateModel.templateFields = TemplateFields;
                        status = updateItemService.UpdateItem(templateModel);
                        #endregion
                    }

                    #region Updating PortsGMSGrievanceBooking Item through ItemAPI
                    var listItems1 = new List<FieldModel>
                                  {
                                new FieldModel { FieldName = "StakeholderRemarks", FieldValue =  ObjBooking.StakeholderRemarks },
                                new FieldModel { FieldName = "Modified_Date", FieldValue = System.DateTime.Now.ToString() }
                                 };
                    TemplateFields.Fields = listItems1;
                    templateModel.templateFields = TemplateFields;
                    status = updateItemService.UpdateItem(templateModel);
                    #endregion
                }
            }
            catch (System.Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Could not update item " + Grievance.Paths.FullPath + ": " + ex.Message, this);
                Grievance.Editing.CancelEdit();
            }


            //newItem.Editing.BeginEdit();
            try
            {
                if (chkResponse == false)
                {

                    #region Creating PortsGMSAssignGrievanceBookingToLevelZero Item through ItemAPI
                    Database DBRef = Sitecore.Configuration.Factory.GetDatabase("web");
                    TemplatedModel templateModel = new TemplatedModel();
                    templateModel.templateId = "{15A2BC1A-9A80-45B7-AA74-85FD64357E7D}";
                    templateModel.parentItem = DBRef.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"].ID.ToString();
                    templateModel.newItemName = "Grievance" + ObjBooking.Id.ToString();
                    var TodayDate = System.DateTime.Now;

                    TemplateFields TemplateFields = new TemplateFields();
                    var listItems = new List<FieldModel>
                        {
                            new FieldModel { FieldName = "ID", FieldValue = Guid.NewGuid().ToString()},
                            new FieldModel { FieldName = "GrievanceBookingId", FieldValue = GrievanceId.ToString() },
                            new FieldModel { FieldName = "RegistrationID", FieldValue = Grievance["RegistrationId"] },
                            new FieldModel { FieldName = "LevelInfo", FieldValue =  "level0"},
                            new FieldModel { FieldName = "Response", FieldValue = PortsGMSTemplates.GMSFlags.RePendings },
                            new FieldModel { FieldName = "Status", FieldValue = PortsGMSTemplates.GrievanceStatus.Level2 },
                            new FieldModel { FieldName = "UserType", FieldValue = "Level 0" },
                            new FieldModel { FieldName = "AssignedLevel", FieldValue = "0" },
                            new FieldModel { FieldName = "AssignedState", FieldValue = "Assigned" },
                            new FieldModel { FieldName = "CreatedDate", FieldValue = TodayDate.ToString() }
                         };
                    TemplateFields.Fields = listItems;
                    templateModel.templateFields = TemplateFields;
                    bool status = createItemService.CreateItem(templateModel);
                    #endregion
                }
            }
            catch (System.Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Could not update item " + ex.Message, this);
            }
            msg = "Grievance is successfully reviewed from StackHolder";
            Session["SuccessMsgStake"] = msg;
            var item1 = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceStakHolderDashbord);
            return Json(item1.Url());
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

            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item OnBehalfParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGmsGrievanceBookingOnBehalf"];
            Item AssignGrievanceBookingParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];
            var Grievance = parentItem.GetChildren().Where(x => x["ID"] == GrievanceId.ToString()).FirstOrDefault();


            var level0reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level0" && x["AssignedLevel"] == "0" && x["Response"] == PortsGMSTemplates.GMSFlags.Completed).OrderByDescending(x => x["CreatedDate"]).FirstOrDefault();
            var StakHolder = rdb.PortsGms_Registrations.Where(x => x.Id == Guid.Parse(Grievance["RegistrationId"])).FirstOrDefault();
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
                ObjBooking.Id = Guid.Parse(Grievance["Id"]);
                ObjBooking.Location = Grievance["Location"];
                ObjBooking.Subject = Grievance["Subject"];
                ObjBooking.Brief = Grievance["Brief"];
                ObjBooking.WhoImpacted = Grievance["WhoImpacted"];
                ObjBooking.Company = Grievance["Company"];
                ObjBooking.Status = Grievance["Status"];
                ObjBooking.Department = Grievance["Department"];
            }
            if (Level1User != null)
            {
                ObjBooking.Level1UserName = Level1User.Name;
            }
            if (level0reply != null)
            {
                ObjBooking.Level0UserName = level0reply["Username"];
                ObjBooking.Level0Comment = level0reply["Comment"];
                ObjBooking.BusinessGroup = level0reply["BusinessGroup"];
            }
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

            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item GrievanceBookingparentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item OnBehalfParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGmsGrievanceBookingOnBehalf"];
            Item AssignGrievanceBookingParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];
            string msg = "";
            try
            {
                using (PortsGMSDataContext rdb = new PortsGMSDataContext())
                {
                    using (new SecurityDisabler())
                    {
                        var Username = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).Select(x => x.Name).FirstOrDefault();
                        // update the grivence booking record
                        var GrievanceDetails = GrievanceBookingparentItem.GetChildren().Where(x => x["Id"] == GrievanceId.ToString()).FirstOrDefault();
                        try
                        {
                            if (GrievanceDetails != null)
                            {

                                #region Updating PortsGMSGrievanceBooking Item through ItemAPI
                                TemplatedModel templateModel = new TemplatedModel();
                                templateModel.itemPath = GrievanceDetails.ID.ToString();
                                TemplateFields TemplateFields = new TemplateFields();
                                var listItems = new List<FieldModel>
                                  {
                                new FieldModel { FieldName = "UserType", FieldValue = "Level 2" },
                                new FieldModel { FieldName = "AssignedLevel", FieldValue =  "2" },
                                new FieldModel { FieldName = "SiteHead", FieldValue =  pm.SiteHead },
                                new FieldModel { FieldName = "HOType", FieldValue =  pm.HO },
                                new FieldModel { FieldName = "Modified_Date", FieldValue = System.DateTime.Now.ToString() }
                                 };
                                TemplateFields.Fields = listItems;
                                templateModel.templateFields = TemplateFields;
                                bool status = updateItemService.UpdateItem(templateModel);
                                #endregion
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + ex.Message, this);
                        }


                        //update the  existing booking assignment record
                        var level0Records = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == GrievanceId.ToString() && x["LevelInfo"] == "level0" && x["Response"] == PortsGMSTemplates.GMSFlags.Completed).FirstOrDefault();
                        var existingBookingAssignment = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == GrievanceId.ToString() && x["LevelInfo"] == "level1" && x["Response"] == PortsGMSTemplates.GMSFlags.RePendings).FirstOrDefault();

                        try
                        {
                            if (existingBookingAssignment != null)
                            {
                                #region Updating PortsGMSAssignGrievanceBookingToLevelZero Item through ItemAPI
                                TemplatedModel templateModel = new TemplatedModel();
                                templateModel.itemPath = existingBookingAssignment.ID.ToString();
                                TemplateFields TemplateFields = new TemplateFields();
                                var listItems = new List<FieldModel>
                                  {
                                new FieldModel { FieldName = "Comment", FieldValue = pm.Comment },
                                new FieldModel { FieldName = "BusinessGroup", FieldValue = level0Records["BusinessGroup"] },
                                 new FieldModel { FieldName = "SiteHead",FieldValue = pm.SiteHead },
                                new FieldModel { FieldName = "HOType", FieldValue = pm.HO },
                                 new FieldModel { FieldName = "Response", FieldValue = PortsGMSTemplates.GMSFlags.Completed },
                                new FieldModel { FieldName = "ModifiedDate", FieldValue =  System.DateTime.Now.ToString() },
                                 new FieldModel { FieldName = "Username", FieldValue = Username },
                                 };
                                TemplateFields.Fields = listItems;
                                templateModel.templateFields = TemplateFields;
                                bool status = updateItemService.UpdateItem(templateModel);
                                #endregion
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + ex.Message, this);
                        }

                        // creating new booking assignment for 
                        var tempId = "{15A2BC1A-9A80-45B7-AA74-85FD64357E7D}";
                        TemplateItem template = masterDb.GetTemplate(tempId);
                        Item newItem = AssignGrievanceBookingParentItem.Add("Grievance" + pm.Id.ToString(), template);

                        newItem.Editing.BeginEdit();
                        try
                        {

                            #region Creating PortsGMSAssignGrievanceBookingToLevelZero Item through ItemAPI
                            Database DBRef = Sitecore.Configuration.Factory.GetDatabase("web");
                            TemplatedModel templateModel = new TemplatedModel();
                            templateModel.templateId = "{15A2BC1A-9A80-45B7-AA74-85FD64357E7D}";
                            templateModel.parentItem = DBRef.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"].ID.ToString();
                            templateModel.newItemName = "Grievance" + pm.Id.ToString();
                            var TodayDate = System.DateTime.Now;

                            TemplateFields TemplateFields = new TemplateFields();
                            var listItems = new List<FieldModel>
                        {
                            new FieldModel { FieldName = "ID", FieldValue = Guid.NewGuid().ToString()},
                            new FieldModel { FieldName = "GrievanceBookingId", FieldValue = GrievanceId.ToString() },
                            new FieldModel { FieldName = "RegistrationID", FieldValue = existingBookingAssignment["RegistrationId"] },
                            new FieldModel { FieldName = "LevelInfo", FieldValue =  "level2"},
                            new FieldModel { FieldName = "Response", FieldValue = PortsGMSTemplates.GMSFlags.RePendings },
                            new FieldModel { FieldName = "Status", FieldValue = PortsGMSTemplates.GrievanceStatus.Level2 },
                            new FieldModel { FieldName = "Department", FieldValue = existingBookingAssignment["Department"] },
                            new FieldModel { FieldName = "UserType", FieldValue = "Level 2" },
                            new FieldModel { FieldName = "AssignedLevel", FieldValue = "2" },
                            new FieldModel { FieldName = "AssignedState", FieldValue = "Assigned" },
                            new FieldModel { FieldName = "CreatedDate", FieldValue = TodayDate.ToString() }
                         };
                            TemplateFields.Fields = listItems;
                            templateModel.templateFields = TemplateFields;
                            bool status = createItemService.CreateItem(templateModel);
                            #endregion
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + newItem.Paths.FullPath + ": " + ex.Message, this);

                            newItem.Editing.CancelEdit();
                        }
                        msg = "Grievance is successfully assigned to Level 2";
                        Session["SuccessMsg1"] = msg;
                    }
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

            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item OnBehalfParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGmsGrievanceBookingOnBehalf"];
            Item AssignGrievanceBookingParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];

            var Grievance = parentItem.GetChildren().Where(x => x["ID"] == GrievanceId.ToString()).FirstOrDefault();

            var StakHolder = rdb.PortsGms_Registrations.Where(x => x.Id == Guid.Parse(Grievance["RegistrationId"])).FirstOrDefault();
            var Level2User = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
            var Assigned = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["RegistrationId"] == RegistrationId.ToString()).FirstOrDefault();
            var level0reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level0" && x["AssignedLevel"] == "0" && x["Response"] == PortsGMSTemplates.GMSFlags.Completed).OrderByDescending(x => x["CreatedDate"]).FirstOrDefault();
            var level1reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level1" && x["Response"] == PortsGMSTemplates.GMSFlags.Completed).OrderByDescending(x => x["CreatedDate"]).FirstOrDefault();
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
                ObjBooking.Id = Guid.Parse(Grievance["Id"]);
                ObjBooking.Location = Grievance["Location"];
                ObjBooking.Department = Grievance["Department"];
                ObjBooking.Subject = Grievance["Subject"];
                ObjBooking.Brief = Grievance["Brief"];
                ObjBooking.WhoImpacted = Grievance["WhoImpacted"];
                ViewBag.AssignedLevel = Grievance["AssignedLevel"];
            }
            if (level0reply != null)
            {
                ObjBooking.Level0UserName = level0reply["Username"];
                ObjBooking.Level0Comment = level0reply["Comment"];
                ObjBooking.BusinessGroup = level0reply["BusinessGroup"];
            }
            if (level1reply != null)
            {
                ObjBooking.Level1UserName = level1reply["Username"];
                ObjBooking.Level1Comment = level1reply["Comment"];
                ObjBooking.SiteHead = level1reply["SiteHead"];
                ObjBooking.HO = level1reply["HOType"];
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

            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item GrievanceBookingparentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item OnBehalfParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGmsGrievanceBookingOnBehalf"];
            Item AssignGrievanceBookingParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];

            try
            {
                using (PortsGMSDataContext rdb = new PortsGMSDataContext())
                {
                    var Username = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).Select(x => x.Name).FirstOrDefault();
                    // update the grivence booking record
                    var GrievanceDetails = GrievanceBookingparentItem.GetChildren().Where(x => x["Id"] == GrievanceId.ToString()).FirstOrDefault();
                    using (new SecurityDisabler())
                    {
                        try
                        {
                            if (GrievanceDetails != null)
                            {

                                #region Updating PortsGMSGrievanceBooking Item through ItemAPI
                                TemplatedModel templateModel = new TemplatedModel();
                                templateModel.itemPath = GrievanceDetails.ID.ToString();
                                TemplateFields TemplateFields = new TemplateFields();
                                var listItems = new List<FieldModel>
                                  {
                                new FieldModel { FieldName = "UserType", FieldValue = "Level 3" },
                                new FieldModel { FieldName = "AssignedLevel", FieldValue =  "3" },
                                new FieldModel { FieldName = "PointMan", FieldValue =  pm.PointMan },
                                new FieldModel { FieldName = "Modified_Date", FieldValue = System.DateTime.Now.ToString() }
                                 };
                                TemplateFields.Fields = listItems;
                                templateModel.templateFields = TemplateFields;
                                bool status = updateItemService.UpdateItem(templateModel);
                                #endregion
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + GrievanceDetails.Paths.FullPath + ": " + ex.Message, this);
                            GrievanceDetails.Editing.CancelEdit();
                        }

                        //update the  existing booking assignment record
                        var level1Records = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == GrievanceId.ToString() && x["LevelInfo"] == "level1" && x["Response"] == PortsGMSTemplates.GMSFlags.Completed).FirstOrDefault();
                        var existingBookingAssignment = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == GrievanceId.ToString() && x["LevelInfo"] == "level2" && x["Response"] == PortsGMSTemplates.GMSFlags.RePendings).FirstOrDefault();

                        try
                        {
                            if (existingBookingAssignment != null)
                            {


                                #region Updating PortsGMSAssignGrievanceBookingToLevelZero Item through ItemAPI
                                TemplatedModel templateModel = new TemplatedModel();
                                templateModel.itemPath = existingBookingAssignment.ID.ToString();
                                TemplateFields TemplateFields = new TemplateFields();
                                var listItems = new List<FieldModel>
                                  {
                                new FieldModel { FieldName = "Comment", FieldValue = pm.Comment },
                                new FieldModel { FieldName = "BusinessGroup", FieldValue = level1Records["BusinessGroup"] },
                                 new FieldModel { FieldName = "SiteHead",FieldValue = level1Records["SiteHead"] },
                                new FieldModel { FieldName = "HOType", FieldValue = level1Records["HOType"] },
                                new FieldModel { FieldName = "PointMan", FieldValue =  pm.PointMan },
                                 new FieldModel { FieldName = "Response", FieldValue = PortsGMSTemplates.GMSFlags.Completed },
                                new FieldModel { FieldName = "ModifiedDate", FieldValue =  System.DateTime.Now.ToString() },
                                 new FieldModel { FieldName = "Username", FieldValue = Username },
                                 };
                                TemplateFields.Fields = listItems;
                                templateModel.templateFields = TemplateFields;
                                bool status = updateItemService.UpdateItem(templateModel);
                                #endregion
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + ex.Message, this);
                        }

                        // creating new booking assignment for 


                        try
                        {

                            #region Creating PortsGMSAssignGrievanceBookingToLevelZero Item through ItemAPI
                            Database DBRef = Sitecore.Configuration.Factory.GetDatabase("web");
                            TemplatedModel templateModel = new TemplatedModel();
                            templateModel.templateId = "{15A2BC1A-9A80-45B7-AA74-85FD64357E7D}";
                            templateModel.parentItem = DBRef.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"].ID.ToString();
                            templateModel.newItemName = "Grievance" + pm.Id.ToString();
                            var TodayDate = System.DateTime.Now;

                            TemplateFields TemplateFields = new TemplateFields();
                            var listItems = new List<FieldModel>
                        {
                            new FieldModel { FieldName = "ID", FieldValue = Guid.NewGuid().ToString()},
                            new FieldModel { FieldName = "GrievanceBookingId", FieldValue = GrievanceId.ToString() },
                            new FieldModel { FieldName = "RegistrationID", FieldValue = existingBookingAssignment["RegistrationId"] },
                            new FieldModel { FieldName = "LevelInfo", FieldValue =  "level3"},
                            new FieldModel { FieldName = "Response", FieldValue = PortsGMSTemplates.GMSFlags.RePendings },
                            new FieldModel { FieldName = "Status", FieldValue = PortsGMSTemplates.GrievanceStatus.Level3 },
                            new FieldModel { FieldName = "Department", FieldValue = existingBookingAssignment["Department"] },
                            new FieldModel { FieldName = "UserType", FieldValue = "Level 3" },
                            new FieldModel { FieldName = "AssignedLevel", FieldValue = "3" },
                            new FieldModel { FieldName = "AssignedState", FieldValue = "Assigned" },
                            new FieldModel { FieldName = "CreatedDate", FieldValue = TodayDate.ToString() }
                         };
                            TemplateFields.Fields = listItems;
                            templateModel.templateFields = TemplateFields;
                            bool status = createItemService.CreateItem(templateModel);
                            #endregion
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + ex.Message, this);
                        }
                        msg = "Grievance is successfully assigned to Level 3";
                        Session["SuccessMsg2"] = msg;
                    }
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

            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item parentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item OnBehalfParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGmsGrievanceBookingOnBehalf"];
            Item AssignGrievanceBookingParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];
            var Grievance = parentItem.GetChildren().Where(x => x["ID"] == GrievanceId.ToString()).FirstOrDefault();

            var StakHolder = rdb.PortsGms_Registrations.Where(x => x.Id == Guid.Parse(Grievance["RegistrationId"])).FirstOrDefault();
            var Level3User = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).FirstOrDefault();
            var Assigned = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["RegistrationId"] == RegistrationId.ToString()).FirstOrDefault();
            var level0reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level0" && x["AssignedLevel"] == "0" && x["Response"] == PortsGMSTemplates.GMSFlags.Completed).OrderByDescending(x => x["CreatedDate"]).FirstOrDefault();
            var level1reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level1" && x["Response"] == PortsGMSTemplates.GMSFlags.Completed).OrderByDescending(x => x["CreatedDate"]).FirstOrDefault();
            var level2reply = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == Grievance["Id"] && x["LevelInfo"] == "level2" && x["Response"] == PortsGMSTemplates.GMSFlags.Completed).OrderByDescending(x => x["CreatedDate"]).FirstOrDefault();
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
                ObjBooking.Id = Guid.Parse(Grievance["Id"]);
                ObjBooking.Location = Grievance["Location"];
                ObjBooking.Subject = Grievance["Subject"];
                ObjBooking.Brief = Grievance["Brief"];
                ObjBooking.WhoImpacted = Grievance["WhoImpacted"];
                ObjBooking.Company = Grievance["Company"];
                ObjBooking.Department = Grievance["Department"];
                ViewBag.AssignedLevel = Grievance["AssignedLevel"];
            }
            if (level0reply != null)
            {
                ObjBooking.Level0UserName = level0reply["Username"];
                ObjBooking.Level0Comment = level0reply["Comment"];
                ObjBooking.BusinessGroup = level0reply["BusinessGroup"];
            }
            if (level1reply != null)
            {
                ObjBooking.Level1UserName = level1reply["Username"];
                ObjBooking.Level1Comment = level1reply["Comment"];
                ObjBooking.SiteHead = level1reply["SiteHead"]; ;
                ObjBooking.HO = level1reply["HOType"];
            }
            if (level2reply != null)
            {
                ObjBooking.Level2UserName = level2reply["Username"];
                ObjBooking.Level2Comment = level2reply["Comment"];
                ObjBooking.PointMan = level2reply["PointMan"];
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

            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item GrievanceBookingparentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item OnBehalfParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGmsGrievanceBookingOnBehalf"];
            Item AssignGrievanceBookingParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];

            try
            {
                using (PortsGMSDataContext rdb = new PortsGMSDataContext())
                {
                    var Username = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).Select(x => x.Name).FirstOrDefault();
                    // update the grivence booking record
                    var GrievanceDetails = GrievanceBookingparentItem.GetChildren().Where(x => x["Id"] == GrievanceId.ToString()).FirstOrDefault();
                    using (new SecurityDisabler())
                    {
                        try
                        {
                            if (GrievanceDetails != null)
                            {

                                #region Updating PortsGMSGrievanceBooking Item through ItemAPI
                                TemplatedModel templateModel = new TemplatedModel();
                                templateModel.itemPath = GrievanceDetails.ID.ToString();
                                TemplateFields TemplateFields = new TemplateFields();
                                var listItems = new List<FieldModel>
                                  {
                                new FieldModel { FieldName = "UserType", FieldValue = "Level 0" },
                                new FieldModel { FieldName = "AssignedLevel", FieldValue =  "Re-assigned 0" },
                                new FieldModel { FieldName = "Modified_Date", FieldValue = System.DateTime.Now.ToString() }
                                 };
                                TemplateFields.Fields = listItems;
                                templateModel.templateFields = TemplateFields;
                                bool status = updateItemService.UpdateItem(templateModel);
                                #endregion
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + GrievanceDetails.Paths.FullPath + ": " + ex.Message, this);
                            GrievanceDetails.Editing.CancelEdit();
                        }


                        //update the  existing booking assignment record
                        var level2Records = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == GrievanceId.ToString() && x["LevelInfo"] == "level2" && x["Response"] == PortsGMSTemplates.GMSFlags.Completed).FirstOrDefault();
                        var existingBookingAssignment = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == GrievanceId.ToString() && x["LevelInfo"] == "level3" && x["Response"] == PortsGMSTemplates.GMSFlags.RePendings).FirstOrDefault();
                        try
                        {
                            if (existingBookingAssignment != null)
                            {

                                #region Updating PortsGMSAssignGrievanceBookingToLevelZero Item through ItemAPI
                                TemplatedModel templateModel = new TemplatedModel();
                                templateModel.itemPath = existingBookingAssignment.ID.ToString();
                                TemplateFields TemplateFields = new TemplateFields();
                                var listItems = new List<FieldModel>
                                  {
                                new FieldModel { FieldName = "Comment", FieldValue = pm.Comment },
                                new FieldModel { FieldName = "BusinessGroup", FieldValue = level2Records["BusinessGroup"] },
                                 new FieldModel { FieldName = "SiteHead",FieldValue =  level2Records["SiteHead"] },
                                new FieldModel { FieldName = "HOType", FieldValue = level2Records["HOType"] },
                                new FieldModel { FieldName = "PointMan", FieldValue =  level2Records["PointMan"] },
                                 new FieldModel { FieldName = "Response", FieldValue = PortsGMSTemplates.GMSFlags.Completed },
                                new FieldModel { FieldName = "ModifiedDate", FieldValue =  System.DateTime.Now.ToString() },
                                 new FieldModel { FieldName = "Username", FieldValue = Username },
                                 };
                                TemplateFields.Fields = listItems;
                                templateModel.templateFields = TemplateFields;
                                bool status = updateItemService.UpdateItem(templateModel);
                                #endregion
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + existingBookingAssignment.Paths.FullPath + ": " + ex.Message, this);
                            existingBookingAssignment.Editing.CancelEdit();
                        }

                        // creating new booking assignment for 


                        try
                        {

                            #region Creating PortsGMSAssignGrievanceBookingToLevelZero Item through ItemAPI
                            Database DBRef = Sitecore.Configuration.Factory.GetDatabase("web");
                            TemplatedModel templateModel = new TemplatedModel();
                            templateModel.templateId = "{15A2BC1A-9A80-45B7-AA74-85FD64357E7D}";
                            templateModel.parentItem = DBRef.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"].ID.ToString();
                            templateModel.newItemName = "Grievance" + pm.Id.ToString();
                            var TodayDate = System.DateTime.Now;

                            TemplateFields TemplateFields = new TemplateFields();
                            var listItems = new List<FieldModel>
                        {
                            new FieldModel { FieldName = "ID", FieldValue = Guid.NewGuid().ToString()},
                            new FieldModel { FieldName = "GrievanceBookingId", FieldValue = GrievanceId.ToString() },
                            new FieldModel { FieldName = "RegistrationID", FieldValue = existingBookingAssignment["RegistrationId"] },
                            new FieldModel { FieldName = "LevelInfo", FieldValue =  "level0"},
                            new FieldModel { FieldName = "Response", FieldValue = PortsGMSTemplates.GMSFlags.RePendings },
                            new FieldModel { FieldName = "Status", FieldValue = PortsGMSTemplates.GrievanceStatus.Level0 },
                            new FieldModel { FieldName = "Department", FieldValue = existingBookingAssignment["Department"] },
                            new FieldModel { FieldName = "BusinessGroup", FieldValue = existingBookingAssignment["BusinessGroup"] },
                            new FieldModel { FieldName = "SiteHead",FieldValue =  existingBookingAssignment["SiteHead"] },
                            new FieldModel { FieldName = "HOType", FieldValue = existingBookingAssignment["HOType"] },
                            new FieldModel { FieldName = "PointMan", FieldValue =  existingBookingAssignment["PointMan"] },
                            new FieldModel { FieldName = "UserType", FieldValue = "Level 0" },
                            new FieldModel { FieldName = "AssignedLevel", FieldValue = "Re-assigned 0" },
                            new FieldModel { FieldName = "AssignedState", FieldValue = "Assigned" },
                            new FieldModel { FieldName = "CreatedDate", FieldValue = TodayDate.ToString() }
                         };
                            TemplateFields.Fields = listItems;
                            templateModel.templateFields = TemplateFields;
                            bool status = createItemService.CreateItem(templateModel);
                            #endregion
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item" + ex.Message, this);
                        }
                        msg = "Grievance is successfully re-assigned to Level 0";
                        Session["SuccessMsg3"] = msg;
                    }
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
        public ActionResult PortsGmsManageUsers()
        {
            if (Session["PortsGMSAdminUser"] == null)
            {
                return Redirect("/Grievance");
            }

            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                List<PortsGms_Registration> UsersData;

                var st = PortsGMSTemplates.UserType.Stakeholder.ToString();
                var ad = PortsGMSTemplates.UserType.Admin.ToString();
                UsersData = rdb.PortsGms_Registrations.Where(x => x.User_Type != st && x.User_Type != ad).OrderByDescending(p => p.Created_Date).ToList();

                PortsGmsAdminDashbord ObjBooking = new PortsGmsAdminDashbord();
                List<PortsGmsManageUsers> cityObj = new List<PortsGmsManageUsers>();
                PortsGmsManageUsers ObjUsers = new PortsGmsManageUsers();
                foreach (var item in UsersData)
                {
                    PortsGmsManageUsers ObjUser = new PortsGmsManageUsers();
                    ObjUser.Id = item.Id;
                    ObjUser.Location = item.Location;
                    ObjUser.Name = item.Name;
                    ObjUser.User_Type = item.User_Type;
                    ObjUser.Department = item.Department;
                    ObjUser.Email = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Email, EncryptionIV);
                    ObjUser.Mobile = GSM_EncryptDecrypt.DecryptString(EncryptionKey, item.Mobile, EncryptionIV);
                    ObjUsers.AllGMSUsers.Add(ObjUser);
                }
                return View(ObjUsers);
            }
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

            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item GrievanceBookingparentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item OnBehalfParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGmsGrievanceBookingOnBehalf"];
            Item AssignGrievanceBookingParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];
            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                using (new SecurityDisabler())
                {
                    string msg = "";
                    var Username = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).Select(x => x.Name).FirstOrDefault();
                    // update the grivence booking record
                    var GrievanceDetails = GrievanceBookingparentItem.GetChildren().Where(x => x["Id"] == GrievanceId.ToString() && x["UserType"] == "Level 0").FirstOrDefault();
                    try
                    {
                        if (GrievanceDetails != null)
                        {
                            GrievanceDetails.Editing.BeginEdit();
                            GrievanceDetails["UserType"] = PortsGMSTemplates.UserType.Stakeholder;
                            GrievanceDetails["AssignedLevel"] = "Stakeholder";
                            GrievanceDetails["Status"] = PortsGMSTemplates.GMSFlags.Response;
                            GrievanceDetails["AssignedState"] = "Responsed";
                            GrievanceDetails["FinalComment"] = pm.FinalComment;
                            GrievanceDetails.Editing.EndEdit();
                        }
                    }
                    catch (System.Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Could not update item " + GrievanceDetails.Paths.FullPath + ": " + ex.Message, this);
                        GrievanceDetails.Editing.CancelEdit();
                    }

                    //update the last existing booking assignment record of level 0

                    if (GrievanceDetails["AssignedLevel"] == "Stakeholder")
                    {
                        var level0Records = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == GrievanceId.ToString() && x["AssignedLevel"] == "0" && x["UserType"] == "Level 0" && x["Response"] == PortsGMSTemplates.GMSFlags.RePendings).FirstOrDefault();

                        try
                        {
                            if (level0Records != null)
                            {
                                level0Records.Editing.BeginEdit();
                                level0Records["Comment"] = pm.FinalComment;
                                level0Records["Response"] = PortsGMSTemplates.GMSFlags.Completed;
                                level0Records["ModifiedDate"] = System.DateTime.Now.ToString();
                                level0Records["UserType"] = PortsGMSTemplates.UserType.Stakeholder;
                                level0Records["AssignedState"] = "Responsed";
                                level0Records["Username"] = Username;
                                level0Records.Editing.EndEdit();
                                msg = "Grievance is successfully submitted to Stakeholder";
                                Session["SuccessMsg0"] = msg;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + GrievanceDetails.Paths.FullPath + ": " + ex.Message, this);
                            GrievanceDetails.Editing.CancelEdit();
                        }

                        var lastlevel0Finalrecord = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == GrievanceId.ToString() && x["AssignedLevel"] == "Re-assigned 0" && x["UserType"] == "Level 0" && x["Response"] == PortsGMSTemplates.GMSFlags.RePendings).FirstOrDefault();


                        try
                        {
                            if (lastlevel0Finalrecord != null)
                            {
                                lastlevel0Finalrecord.Editing.BeginEdit();
                                lastlevel0Finalrecord["Comment"] = pm.FinalComment;
                                lastlevel0Finalrecord["Response"] = PortsGMSTemplates.GMSFlags.Completed;
                                lastlevel0Finalrecord["ModifiedDate"] = System.DateTime.Now.ToString();
                                lastlevel0Finalrecord["UserType"] = PortsGMSTemplates.UserType.Stakeholder;
                                lastlevel0Finalrecord["AssignedState"] = "Responsed";
                                lastlevel0Finalrecord["Username"] = Username;
                                lastlevel0Finalrecord.Editing.EndEdit();
                                msg = "Grievance is successfully submitted to Stakeholder";
                                Session["SuccessMsg0"] = msg;
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Error("Could not update item " + GrievanceDetails.Paths.FullPath + ": " + ex.Message, this);
                            GrievanceDetails.Editing.CancelEdit();
                        }
                    }
                }
            }
            var item1 = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel0Dashbord);
            return Json(item1.Url());
        }

        [HttpPost]
        public JsonResult ReassignPointMan(PortsGmsLevel3Dashbord pm, string AssignPointMan)
        {
            if (Session["PortsGMSLevel3User"] == null)
            {
                //return Redirect("/Grievance");
                var item = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLogin);
                return Json(item.Url());
            }
            Guid RegistrationId = new Guid(Session["PortsGMSLevel3User"].ToString());
            Guid GrievanceId = pm.Id;
            PortsGMSGrievanceBooking grievencebooking = new PortsGMSGrievanceBooking();
            PortsGMSBookingAssignment bookingAssignment = new PortsGMSBookingAssignment();
            PortsGms_Registration Level3User = new PortsGms_Registration();


            Database masterDb = Sitecore.Configuration.Factory.GetDatabase("web");
            Item GrievanceBookingparentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSGrievanceBooking"];
            Item OnBehalfParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGmsGrievanceBookingOnBehalf"];
            Item AssignGrievanceBookingParentItem = masterDb.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"];

            using (PortsGMSDataContext rdb = new PortsGMSDataContext())
            {
                string msg = "";
                var Username = rdb.PortsGms_Registrations.Where(x => x.Id == RegistrationId).Select(x => x.Name).FirstOrDefault();
                // update the grivence booking record
                var GrievanceDetails = GrievanceBookingparentItem.GetChildren().Where(x => x["Id"] == GrievanceId.ToString()).FirstOrDefault();

                try
                {
                    if (GrievanceDetails != null)
                    {

                        #region Updating PortsGMSGrievanceBooking Item through ItemAPI
                        TemplatedModel templateModel = new TemplatedModel();
                        templateModel.itemPath = GrievanceDetails.ID.ToString();
                        TemplateFields TemplateFields = new TemplateFields();
                        var listItems = new List<FieldModel>
                                  {
                                new FieldModel { FieldName = "UserType", FieldValue ="Level 3"},
                                new FieldModel { FieldName = "AssignedLevel", FieldValue = "Re-assigned 3"},
                                 new FieldModel { FieldName = "PointMan", FieldValue = AssignPointMan },
                                new FieldModel { FieldName = "Modified_Date", FieldValue = System.DateTime.Now.ToString() }
                                 };
                        TemplateFields.Fields = listItems;
                        templateModel.templateFields = TemplateFields;
                        Log.Info("Itme Update To service before update the grivence booking record", TemplateFields);
                        bool status = updateItemService.UpdateItem(templateModel);
                        Log.Info("Itme Update To service After update the grivence booking record", status);

                        #endregion
                    }
                }
                catch (System.Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Could not update item " + GrievanceDetails.Paths.FullPath + ": " + ex.Message, this);
                    GrievanceDetails.Editing.CancelEdit();
                }

                //update the  existing booking assignment record
                //  var level2Records = rdb.PortsGMSBookingAssignments.Where(x => x.GrievanceBookingId == GrievanceId && x.LevelInfo == "level2" && x.Response == PortsGMSTemplates.GMSFlags.Completed).FirstOrDefault();
                var level2Records = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == GrievanceId.ToString() && x["LevelInfo"] == "level2" && x["Response"] == PortsGMSTemplates.GMSFlags.Completed).FirstOrDefault();
                var existingBookingAssignment = AssignGrievanceBookingParentItem.GetChildren().Where(x => x["GrievanceBookingId"] == GrievanceId.ToString() && x["LevelInfo"] == "level3" && x["Response"] == PortsGMSTemplates.GMSFlags.RePendings).FirstOrDefault();

                try
                {
                    if (existingBookingAssignment != null)
                    {

                        #region Updating PortsGMSGrievanceBooking Item through ItemAPI
                        TemplatedModel templateModel = new TemplatedModel();
                        templateModel.itemPath = existingBookingAssignment.ID.ToString();
                        TemplateFields TemplateFields = new TemplateFields();
                        var listItem = new List<FieldModel>
                                  {
                                new FieldModel { FieldName = "Comment", FieldValue = pm.Comments},
                                new FieldModel { FieldName = "BusinessGroup", FieldValue =  level2Records["BusinessGroup"]},
                                 new FieldModel { FieldName = "SiteHead", FieldValue = level2Records["SiteHead"] },
                                new FieldModel { FieldName = "HOType", FieldValue =   level2Records["HOType"] },
                                new FieldModel { FieldName = "PointMan", FieldValue = level2Records["PointMan"] },
                                new FieldModel { FieldName = "Response", FieldValue = PortsGMSTemplates.GMSFlags.Completed },
                                new FieldModel { FieldName = "Status", FieldValue = PortsGMSTemplates.GrievanceStatus.Reassign },
                                new FieldModel { FieldName = "ModifiedDate", FieldValue = System.DateTime.Now.ToString() },
                                new FieldModel { FieldName = "Username", FieldValue = Username },
                                 };
                        TemplateFields.Fields = listItem;
                        templateModel.templateFields = TemplateFields;
                        Log.Info("Itme Update To service before update the grivence booking record", TemplateFields);
                        bool status = updateItemService.UpdateItem(templateModel);
                        Log.Info("Itme Update To service After update the grivence booking record", status);
                        #endregion
                    }
                }
                catch (System.Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Could not update item " + ex.Message, this);
                }

                try
                {

                    #region Creating PortsGMSAssignGrievanceBookingToLevelZero Item through ItemAPI
                    Database DBRef = Sitecore.Configuration.Factory.GetDatabase("web");
                    TemplatedModel templateModel = new TemplatedModel();
                    templateModel.templateId = "{15A2BC1A-9A80-45B7-AA74-85FD64357E7D}";
                    templateModel.parentItem = DBRef.Items["/sitecore/content/Ports/Home/Grievance/StakHolder/AddGrievance/PortsGMSAssignGrievanceBookingToLevelZero"].ID.ToString();
                    templateModel.newItemName = "Grievance" + pm.Id.ToString();
                    var TodayDate = System.DateTime.Now;

                    TemplateFields TemplateFields = new TemplateFields();
                    var listItems = new List<FieldModel>
                        {
                            new FieldModel { FieldName = "ID", FieldValue = Guid.NewGuid().ToString()},
                            new FieldModel { FieldName = "GrievanceBookingId", FieldValue = GrievanceId.ToString() },
                            new FieldModel { FieldName = "RegistrationID", FieldValue = existingBookingAssignment["RegistrationId"] },
                            new FieldModel { FieldName = "LevelInfo", FieldValue =  "level3"},
                            new FieldModel { FieldName = "UserType", FieldValue =  "Level 3"},
                            new FieldModel { FieldName = "Response", FieldValue = PortsGMSTemplates.GMSFlags.RePendings },
                            new FieldModel { FieldName = "Department", FieldValue = existingBookingAssignment["Department"] },
                            new FieldModel { FieldName = "BusinessGroup", FieldValue =level2Records["BusinessGroup"] },
                            new FieldModel { FieldName = "SiteHead", FieldValue = existingBookingAssignment["SiteHead"] },
                            new FieldModel { FieldName = "HOType", FieldValue = existingBookingAssignment["HOType"] },
                            new FieldModel { FieldName = "PointMan", FieldValue = AssignPointMan},
                            new FieldModel { FieldName = "Comment", FieldValue = pm.Comments },
                            new FieldModel { FieldName = "Status", FieldValue = PortsGMSTemplates.GrievanceStatus.Reassign },
                            new FieldModel { FieldName = "AssignedLevel", FieldValue = "Re-assigned 3" },
                            new FieldModel { FieldName = "AssignedState", FieldValue = "Assigned" },
                            new FieldModel { FieldName = "CreatedDate", FieldValue = TodayDate.ToString() }
                         };
                    TemplateFields.Fields = listItems;
                    templateModel.templateFields = TemplateFields;
                    bool statusCreate = createItemService.CreateItem(templateModel);
                    #endregion
                }
                catch (System.Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Could not update item " + ex.Message, this);
                }

                msg = "Grievance is successfully re-assigned to Point Man";
                Session["SuccessMsg3"] = msg;



            }
            var page = Context.Database.GetItem(PortsGMSTemplates.Grievance.GrievanceLevel3Dashbord);
            return Json(page.Url());
        }


        public ActionResult SiteHead(string site)
        {
            try
            {
                List<SiteHead> siteList = new List<SiteHead>();
                Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
                //Sitecore.Data.Database db = Sitecore.Context.Database;
                Log.Info($"SiteHead in get db name Ports: {db}", db);
                Sitecore.Data.Items.Item SiteList = db.GetItem("/sitecore/content/Ports/Global/GMS/PortsGMSUserManagement/Site Head");
                foreach (Item eachDistrictList in SiteList.GetChildren())
                {
                    if (eachDistrictList.Fields["ID"].Value == site)
                    {
                        SiteHead siteHeadObj = new SiteHead();
                        siteHeadObj.SiteHeadCode = eachDistrictList.Fields["Key"].Value;
                        siteHeadObj.SiteHeadName = eachDistrictList.Fields["Value"].Value;
                        siteList.Add(siteHeadObj);
                    }
                }
                return Json(siteList, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                Log.Info($"execption thrown in  SiteHead: {ex.InnerException}", ex);
                return null;
            }
            
        }

        public string saveUserAttachmentFile(HttpPostedFileBase[] files, Guid Id)
        {
            foreach (HttpPostedFileBase file in files)
            {
                if (file != null && file.ContentLength > 0)
                {
                    byte[] UploadedFile = new byte[0];
                    //using (BinaryReader br = new BinaryReader(file.InputStream))
                    //{
                    //    UploadedFile = br.ReadBytes((Int32)file.ContentLength);
                    //}
                    string BlobURL = blobAPIService.BlobAPI(file);
                    PortsGMSDataContext rdb = new PortsGMSDataContext();
                    PortsGms_Registration_Attachment pgra = new PortsGms_Registration_Attachment();
                    pgra.Id = Guid.NewGuid();
                    pgra.Gms_Registration_Id = Id;
                    pgra.FileName = file.FileName;
                    pgra.ContentType = file.ContentType;
                    pgra.DocData = UploadedFile;
                    pgra.Created_Date = System.DateTime.Now;
                    rdb.PortsGms_Registration_Attachments.InsertOnSubmit(pgra);
                    rdb.SubmitChanges();
                }
            }
            return string.Empty;
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
                Log.Error("Failed to get subject specific Email", ex.ToString());
            }
        }

        public FileAttachmentModel saveGrievanceBookingAttachmentFile(HttpPostedFileBase file, Guid Id)
        {
            FileAttachmentModel fileAttachmentModel = new FileAttachmentModel();
            if (file != null)
            {
                fileAttachmentModel.GrievanceFileID = Guid.NewGuid().ToString();
                fileAttachmentModel.GrievanceFileBlobURL = blobAPIService.BlobAPI(file);
                fileAttachmentModel.GrievanceFileName = file.FileName;
                fileAttachmentModel.GrievanceFileContentType = file.ContentType;
                fileAttachmentModel.GrievanceFileGmsRegistrationId = Id.ToString();
                fileAttachmentModel.GrievanceFileCreatedDate = System.DateTime.Now.ToString();
            }
            return fileAttachmentModel;
        }
    }
}