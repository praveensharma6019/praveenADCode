using SapPiService.Domain;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Feature.Accounts;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Feature.Accounts.SessionHelper;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Models
{
    public class ComplaintPortalService
    {
        public enum ActionType
        {
            Saved = 1,
            Submit = 2,
            SubmitCGRF = 3,
            Approved = 4,
            Resubmit = 5,
            Resubmission = 6,
            NodalReply = 7,
            Rejoinder = 8,
            HearingScheduled = 9,
            Closed = 10,
            Review = 11
        }

        public enum ComplaintStatus
        {
            Saved = 1,
            Submitted = 2,
            Approved = 3,
            Resubmit = 4,
            NodalReply = 5,
            Rejoinder = 6,
            Closed = 7,
            Review = 8,
            Rejected = 9
        }

        private const int AccountNumberLength = 12;
        public static string FormatAccountNumber(string accountNumber)
        {
            return accountNumber.PadLeft(AccountNumberLength, '0');
        }

        public string IsComplaintPortalAdminLogin(string username, string password)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.ComplaintPortalCGRFAdminLogins.Any(o => o.Username == username && o.Password == password))
                        return dataContext.ComplaintPortalCGRFAdminLogins.Where(o => o.Username == username && o.Password == password).FirstOrDefault().Role;
                    else
                        return string.Empty;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintCategoryList: " + ex.Message, this);
            }
            return string.Empty;
        }

        public string SendForwardEmailToConsumers(string ComplaintID, string Id)
        {
            #region Email
            try
            {
                try
                {
                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                    {

                        CGRFComplaintFileRegistrationModel model = new CGRFComplaintFileRegistrationModel();
                        var emailId = dataContext.CGRFForwardMails.Where(c => c.Id.ToString() == Id).FirstOrDefault().EmailId;
                        ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();
                        model = objComplaintPortalService.GetCGRFComplaintDetails(ComplaintID);
                        var mail = objComplaintPortalService.GeForwardtMailToConsumers();
                        List<string> emailAddress = emailId.Split(',').ToList();
                        foreach (var s in emailAddress)
                        {
                            mail.To.Add(s);
                        }
                        mail.Body = mail.Body.Replace("#AccountNumber#", model.AccountNumber);
                        mail.Body = mail.Body.Replace("#ConsumerName#", model.ConsumerName);
                        mail.Body = mail.Body.Replace("#Address#", model.Address);
                        mail.Body = mail.Body.Replace("#City#", model.City);
                        mail.Body = mail.Body.Replace("#Pincode#", model.Pincode);
                        mail.Body = mail.Body.Replace("#Email#", model.EmailId);
                        mail.Body = mail.Body.Replace("#MobileNo#", model.MobileNumber);
                        mail.Body = mail.Body.Replace("#ConsumerCategory#", model.SelectedConsumerCategory);
                        mail.Body = mail.Body.Replace("#Zone#", model.SelectedConsumerZone);
                        mail.Body = mail.Body.Replace("#ComplaintFromPreviousLevel#", model.ComplaintFromPreviousLevel);
                        mail.Body = mail.Body.Replace("#ComplaintFromPreviousLevelAppliedDate#", model.ComplaintFromPreviousLevelAppliedDate);
                        mail.Body = mail.Body.Replace("#ReasonToApply#", model.ReasonToApply);
                        mail.Body = mail.Body.Replace("#CGRFCaseNumber#", model.CGRFCaseNumber);
                        mail.Body = mail.Body.Replace("#ComplaintRegistrationNumber#", model.ComplaintRegistrationNumber);
                        mail.Body = mail.Body.Replace("#ComplaintStatusDescription#", model.ComplaintStatusDescription);
                        mail.Body = mail.Body.Replace("#ComplaintCategory#", model.SelectedConsumerZone);
                        mail.Body = mail.Body.Replace("#ComplaintDescription#", model.ComplaintDescription);

                        var complaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == System.Convert.ToInt32(ComplaintID)).FirstOrDefault();
                        if (complaint != null && complaint.DocumentData != null)
                        {
                            if (((complaint.DocumentData.Length / 1024) / 1024) <= 1)
                            {
                                string scheduleAfilename = complaint.DocumentFileName;
                                Stream stream = new MemoryStream(complaint.DocumentData.ToArray());
                                Attachment a = new Attachment(stream, complaint.DocumentFileName);
                                mail.Attachments.Add(a);
                                mail.Body = mail.Body.Replace("#ScheduleAFileName#", scheduleAfilename);
                            }
                            else
                            {
                                mail.Body = mail.Body.Replace("#ScheduleAFileName#", string.Empty);
                            }

                            //var rdFiles = dataContext.ComplaintPortalCGRFComplaintDocuments.Where(d => d.DocumentType == "RD" && d.ComplaintNumber == System.Convert.ToInt32(ComplaintID)).ToList();
                            //if (rdFiles != null && rdFiles.Count > 0)
                            //{
                            //    scheduleAfilename += ", Resubmitted: ";
                            //    foreach (var file in rdFiles)
                            //    {
                            //        scheduleAfilename += file.DocumentName + ", ";
                            //        stream = new MemoryStream(file.DocumentData.ToArray());
                            //        a = new Attachment(stream, file.DocumentName);
                            //        mail.Attachments.Add(a);
                            //    }
                            //}

                            //mail.Body = mail.Body.Replace("#ScheduleAFileName#", scheduleAfilename);
                        }
                        else
                        {
                            mail.Body = mail.Body.Replace("#ScheduleAFileName#", string.Empty);
                        }

                        //var sdFiles = dataContext.ComplaintPortalCGRFComplaintDocuments.Where(d => d.DocumentType == "SD" && d.ComplaintNumber == System.Convert.ToInt32(ComplaintID)).ToList();
                        //string sdFileNames = "";
                        //if (sdFiles != null && sdFiles.Count > 0)
                        //{
                        //    foreach (var file in sdFiles)
                        //    {
                        //        sdFileNames += file.DocumentName + ", ";

                        //        Stream stream = new MemoryStream(file.DocumentData.ToArray());
                        //        Attachment s = new Attachment(stream, file.DocumentName);
                        //        mail.Attachments.Add(s);
                        //    }
                        //    mail.Body = mail.Body.Replace("#SupportingFileNames#", sdFileNames);
                        //}
                        //else
                        //{
                        //    mail.Body = mail.Body.Replace("#SupportingFileNames#", string.Empty);
                        //}
                        mail.Body = mail.Body.Replace("#SupportingFileNames#", string.Empty);
                        Log.Info("SendEmailToSecretary email body:" + mail.Body + ", sent to:" + emailId, this);
                        MainUtil.SendMail(mail);
                        return "1";
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Unable to send the email to the SendEmailTo" + " - Error - " + ex.Message + "", ex, this);
                    return ex.Message;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Complaint portal app submit Email to user call error for submit to old number: " + " error:" + ex.Message, this);
                return ex.Message;
            }
            #endregion
            //return "Error occured";
        }

        public List<ComplaintDetails> GetAllcomplaintsFromSAPByCA(string accountNumber)
        {
            List<ComplaintDetails> complaints = new List<ComplaintDetails>();

            //Get all IGR M20 Complaints 
            List<ComplaintDetailsIGR> result = SapPiService.Services.RequestHandler.FetchComplaints(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
            if (result != null && result.Count() > 0)
            {
                foreach (var com in result)
                {
                    complaints.Add(new ComplaintDetails
                    {
                        IsIGR = true,
                        CreatedOnSAPDateType = DateTime.Parse(com.ERDate.ToString()),
                        ComplaintNumber = com.AUFNR,
                        ComplaintStatusCode = com.IPHAS,
                        ComplaintStatusName = com.Complaint_Status,
                        ComplaintCategory = com.Complaint_Type,
                        ComplaintSubCategory = com.ILART,
                        CreatedOnSAP = com.ERDate, //(DateTime.Parse(com.ERDate.ToString())).ToString("dd/MM/yyyy"),
                        TATDate = com.GLTRP
                    });
                }
            }

            //Get all Complaints general 
            ComplaintGetCaseDetailsSAP complaintDetails = FetchcomplaintDetails(UserSession.AEMLComplaintUserSessionContext.AccountNumber, "2");
            if (complaintDetails != null && complaintDetails.Complaints != null && complaintDetails.Complaints.Count() > 0)
            {
                foreach (var com in complaintDetails.Complaints)
                {
                    complaints.Add(new ComplaintDetails
                    {
                        CreatedOnSAPDateType = DateTime.Parse(com.CreatedOn.ToString()),
                        ComplaintNumber = com.Complaintnumber,
                        ComplaintStatusCode = com.ComplaintStatus,
                        ComplaintStatusName = com.ComplaintStatus == "1" ? "Closed" : "Open",
                        ComplaintCategory = com.ComplaintType,
                        ComplaintSubCategory = com.ComplaintSUbtype,
                        CreatedOnSAP = com.CreatedOn,
                        ModifiedOnSAP = com.ModifiedOn
                    });
                }
            }

            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                foreach (var comp in complaints)
                {
                    if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.ComplaintRegistrationNumber == comp.ComplaintNumber))
                    {
                        comp.M20ComplaintId = dataContext.ComplaintPortalRegisteredComplaints.FirstOrDefault(c => c.ComplaintRegistrationNumber == comp.ComplaintNumber).ComplaintIdSAP;
                    }

                    if (comp.ComplaintSubCategory == "M52" && comp.ComplaintStatusCode == "3")
                    {
                        if (complaints.Any(c => c.ComplaintSubCategory == "I01"))
                        {
                            comp.ComplaintStatusName = "Cancelled";
                        }
                    }

                    try
                    {
                        switch (comp.ComplaintSubCategory)
                        {
                            case "M40"://3 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(3).ToString();
                                break;
                            case "M20"://3 days
                                comp.TATDate = comp.TATDate;
                                break;
                            case "M50"://24 hrs
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(1).ToString();
                                break;
                            case "M51"://24 hrs
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(1).ToString();
                                break;
                            case "M06"://15 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                break;
                            case "I15"://15 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                break;
                            case "M03"://15 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                break;
                            case "M04"://15 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                break;
                            case "M14"://3 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(3).ToString();
                                break;
                            case "M24"://15 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                break;
                            case "M52"://15 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                break;
                            default:
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(1).ToString();
                                break;
                        }
                    }
                    catch
                    {

                    }
                }
            }

            complaints = complaints.OrderByDescending(d => d.CreatedOnSAPDateType).ThenByDescending(d => d.ComplaintNumber).ToList();

            return complaints;
        }

        public List<ComplaintPortalComplaintCategoryMaster> GetComplaintCategoryList()
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    return dataContext.ComplaintPortalComplaintCategoryMasters.OrderBy(o => o.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintCategoryList: " + ex.Message, this);
            }
            return null;
        }

        public List<ComplaintPortalComplaintSubTypeMaster> GetComplaintSybTypeList()
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    return dataContext.ComplaintPortalComplaintSubTypeMasters.OrderBy(o => o.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintCategoryList: " + ex.Message, this);
            }
            return null;
        }

        public List<ComplaintPortalComplaintSubCategoryMaster> GetComplaintSubCategoryList(int categoryId)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    return dataContext.ComplaintPortalComplaintSubCategoryMasters.Where(c => c.CategoryId == categoryId).OrderBy(o => o.Id).ToList();
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintCategoryList: " + ex.Message, this);
            }
            return null;
        }

        public List<ComplaintPortalCGRFZoneDivisionMaster> GetCGRFComplaintZoneDivisionList()
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    return dataContext.ComplaintPortalCGRFZoneDivisionMasters.OrderBy(o => o.DivisionName).ToList();
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetCGRFComplaintZoneDivisionList: " + ex.Message, this);
            }
            return null;
        }

        public bool CreateLoginLog(LoginInfoComplaint model, bool isRegistered)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    ComplaintPortalLogginLog obj = new ComplaintPortalLogginLog
                    {
                        AccountNumber = model.AccountNumber,
                        MobileNumber = model.MobileNumber,
                        IsRegistered = isRegistered,
                        LoginName = model.LoginName,
                        LoggedInDate = DateTime.Now
                    };

                    dataContext.ComplaintPortalLogginLogs.InsertOnSubmit(obj);
                    dataContext.SubmitChanges();
                    return true;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at Complaint Portal CreateLoginLog: " + ex.Message + " For Account number:" + model.AccountNumber, this);
            }
            return false;
        }

        public ComplaintRegistrationResponse SaveComplaintApplication(ComplaintFileRegistrationModel model, string ComplaintLevel, bool isSubmit, string areaAndPoleDetails = null)
        {
            ComplaintRegistrationResponse retResult = new ComplaintRegistrationResponse();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (isSubmit)
                    {
                        //string categoryText = "";
                        //var category = dataContext.ComplaintPortalComplaintCategoryMasters.Where(c => c.Id == System.Convert.ToInt32(model.SelectedComplaintCategory)).FirstOrDefault();
                        //if (category != null)
                        //    categoryText = category.CategoryName;

                        //string subcategoryText = "";
                        //var subcategory = dataContext.ComplaintPortalComplaintSubCategoryMasters.Where(c => c.SubCategoryId == System.Convert.ToInt32(model.SelectedComplaintSubCategory)).FirstOrDefault();
                        //if (subcategory != null)
                        //    subcategoryText = subcategory.SubCategory;

                        //No Supply Complaint - No Supply Individual, No Supply Large Area, Fire & Shock Complaint
                        //Billing Complaint - Bill not Received Complaint
                        if (model.SelectedComplaintCategory == "1" || (model.SelectedComplaintCategory == "2" && model.SelectedComplaintSubCategory == "1"))
                        {
                            //For Bill not received complaints 
                            //categoty - 2
                            //subcategory - 2
                            if (model.SelectedComplaintCategory == "2")
                            {
                                retResult = SapPiService.Services.RequestHandler.RegisterComplaint(model.AccountNumber, model.MobileNumber, "2", "2", "4");
                            }
                            else
                            {
                                retResult = SapPiService.Services.RequestHandler.RegisterComplaint(model.AccountNumber, model.MobileNumber, model.SelectedComplaintCategory, model.SelectedComplaintSubCategory, "4");
                            }
                        }
                        else
                        {
                            //Other: M24
                            //New Connection: M40
                            //Wrong connection: M50
                            //Reconnection: M51
                            //ICRS : M20
                            //4.Billing Related Wrong tariff plan-M06
                            //5.Billing Related Wrong Reading - I15
                            //6.Billing Related Wrong Payment - M03
                            //7.Billing Related Payment not credited-M04
                            //8.Billing Related Outcalling for high consumption complaint - M52
                            string PMACT = string.Empty;
                            if (model.SelectedComplaintCategory == "6") //Other
                                PMACT = "M24";
                            else if (model.SelectedComplaintCategory == "3")
                                PMACT = "M40";
                            else if (model.SelectedComplaintCategory == "4")
                                PMACT = "M50";
                            else if (model.SelectedComplaintCategory == "5")
                                PMACT = "M51";
                            else if (model.SelectedComplaintCategory == "2" && model.SelectedComplaintSubCategory == "2")
                                PMACT = "M06";
                            else if (model.SelectedComplaintCategory == "2" && model.SelectedComplaintSubCategory == "3")
                                PMACT = "I15";
                            else if (model.SelectedComplaintCategory == "2" && model.SelectedComplaintSubCategory == "4")
                                PMACT = "M03";
                            else if (model.SelectedComplaintCategory == "2" && model.SelectedComplaintSubCategory == "5")
                                PMACT = "M04";
                            else if (model.SelectedComplaintCategory == "2" && model.SelectedComplaintSubCategory == "6")
                                PMACT = "M52";

                            retResult = SapPiService.Services.RequestHandler.RegisterComplaintCSOrderCreate(model.AccountNumber, PMACT, model.ComplaintDescription);
                        }
                    }
                    if (model.ComplaintId == null)
                    {
                        if (!isSubmit || (isSubmit && retResult.IsRegistered))
                        {
                            //create new complaint
                            ComplaintPortalRegisteredComplaint obj = new ComplaintPortalRegisteredComplaint
                            {
                                AccountNumber = model.AccountNumber,
                                Address = model.Address,
                                City = model.City,
                                ComplaintCategory = dataContext.ComplaintPortalComplaintCategoryMasters.Where(c => c.Id == System.Convert.ToInt32(model.SelectedComplaintCategory)).FirstOrDefault().CategoryName,
                                ComplaintCategoryId = System.Convert.ToInt32(model.SelectedComplaintCategory),
                                ComplaintDescription = model.ComplaintDescription,
                                IsRegisteredUser = UserSession.AEMLComplaintUserSessionContext.IsRegistered,
                                ComplaintLevel = ComplaintLevel,
                                ComplaintStatusCode = isSubmit ? ((int)ComplaintStatus.Submitted).ToString() : ((int)ComplaintStatus.Saved).ToString(),
                                ComplaintStatusName = isSubmit ? ComplaintStatus.Submitted.ToString() : ComplaintStatus.Saved.ToString(),
                                ComplaintSubCategory = dataContext.ComplaintPortalComplaintSubCategoryMasters.Where(c => c.CategoryId == System.Convert.ToInt32(model.SelectedComplaintCategory) && c.SubCategoryId == System.Convert.ToInt32(model.SelectedComplaintSubCategory)).FirstOrDefault().SubCategory,
                                ComplaintSubCategoryId = System.Convert.ToInt32(model.SelectedComplaintSubCategory),
                                ConsumerCategory = model.SelectedConsumerCategory,
                                ConsumerName = model.ConsumerName,
                                CreatedDate = DateTime.Now,
                                Email = model.EmailId,
                                Mobile = model.MobileNumber,
                                NumberOfVersion = 1,
                                Pincode = model.Pincode,
                                UserName = model.LoginName,
                                Zone = model.ZoneName,
                                MobileNumber = model.MobileNumber,
                                CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName
                            };

                            if (model.ComplaintFromPreviousLevel != null)
                            {
                                obj.ComplaintNumberFromPerviousLevel = model.ComplaintFromPreviousLevel;
                                obj.ComplaintRemarksPerviousLevel = dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.ComplaintRegistrationNumber == model.ComplaintFromPreviousLevel) ? dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == model.ComplaintFromPreviousLevel).FirstOrDefault().AdminRemarks : null;
                            }

                            if (isSubmit)
                            {
                                obj.ComplaintRegistrationNumber = retResult.ComplaintNumber;
                            }

                            dataContext.ComplaintPortalRegisteredComplaints.InsertOnSubmit(obj);
                            dataContext.SubmitChanges();
                        }
                    }
                    else
                    {
                        long ComplaintId = System.Convert.ToInt64(model.ComplaintId);
                        if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.Id == ComplaintId && c.ComplaintStatusName == "Saved"))
                        {
                            ComplaintPortalRegisteredComplaint existingComplaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == ComplaintId).FirstOrDefault();


                            existingComplaint.ComplaintCategory = dataContext.ComplaintPortalComplaintCategoryMasters.Where(c => c.Id == System.Convert.ToInt32(model.SelectedComplaintCategory)).FirstOrDefault().CategoryName;
                            existingComplaint.ComplaintCategoryId = System.Convert.ToInt32(model.SelectedComplaintCategory);

                            existingComplaint.ComplaintDescription = model.ComplaintDescription;

                            existingComplaint.ComplaintSubCategory = dataContext.ComplaintPortalComplaintSubCategoryMasters.Where(c => c.CategoryId == System.Convert.ToInt32(model.SelectedComplaintCategory) && c.SubCategoryId == System.Convert.ToInt32(model.SelectedComplaintSubCategory)).FirstOrDefault().SubCategory;
                            existingComplaint.ComplaintSubCategoryId = System.Convert.ToInt32(model.SelectedComplaintSubCategory);

                            existingComplaint.ModifiedDate = DateTime.Now;
                            existingComplaint.ModifiedBy = UserSession.AEMLComplaintUserSessionContext.LoginName;
                            existingComplaint.NumberOfVersion = existingComplaint.NumberOfVersion + 1;
                            existingComplaint.ComplaintStatusCode = isSubmit ? ((int)ComplaintStatus.Submitted).ToString() : ((int)ComplaintStatus.Saved).ToString();
                            existingComplaint.ComplaintStatusName = isSubmit ? ComplaintStatus.Submitted.ToString() : ComplaintStatus.Saved.ToString();

                            if (isSubmit && retResult.IsRegistered)
                            {
                                existingComplaint.ComplaintRegistrationNumber = retResult.ComplaintNumber;
                            }
                            dataContext.SubmitChanges();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at Complaint Portal CreateLoginLog: " + ex.Message + " For Account number:" + model.AccountNumber, this);
                retResult.Error = ex.Message;
            }
            return retResult;
        }

        public ComplaintRegistrationResponse SaveEscalateToICRS_M20ComplaintApplication(string accountNumber, string complaintId, string remarks)
        {
            ComplaintRegistrationResponse retResult = new ComplaintRegistrationResponse();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {

                    retResult = SapPiService.Services.RequestHandler.RegisterComplaintCSOrderCreate(accountNumber, "M20", remarks);

                    if (retResult.IsRegistered == true)
                    {
                        if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.ComplaintRegistrationNumber == complaintId))
                        {
                            ComplaintPortalRegisteredComplaint existingComplaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == complaintId).FirstOrDefault();

                            existingComplaint.ComplaintIdSAP = retResult.ComplaintNumber;
                        }
                        else
                        {
                            //Insert complaint if not exists - logged from other sources
                            var allcomplaint = GetAllcomplaintsFromSAPByCA(accountNumber);
                            var complaintToAdd = allcomplaint.Where(c => c.ComplaintNumber == complaintId).FirstOrDefault();

                            //                            Activity Type   Complaint Category  Complaint Sub Category
                            //M40 New connection  New connection
                            //M50 Wrong Disconnection Wrong Disconnection
                            //M51 Reconnection not done Reconnection not done
                            //M06 Billing Complaint Wrong tariff plan
                            //I15 Billing Complaint Wrong Reading
                            //M03 Billing Complaint   Wrong Payment
                            //M04 Billing Complaint Payment not credited
                            //M14 Billing Complaint Bill Not Received
                            //M24 Others  Others
                            //M52(Outcalling)    Billing Complaint   High consumption complaint
                            string category = "";
                            string subcategory = "";

                            switch (complaintToAdd.ComplaintSubCategory)
                            {
                                case "M40":
                                    category = "New connection";
                                    subcategory = "New connection";
                                    break;
                                case "M50":
                                    category = "Wrong Disconnection";
                                    subcategory = "Wrong Disconnection";
                                    break;
                                case "M51":
                                    category = "Reconnection not don";
                                    subcategory = "Reconnection not don";
                                    break;
                                case "M06":
                                    category = "Billing Complaint";
                                    subcategory = "Wrong tariff plan";
                                    break;
                                case "I15":
                                    category = "Billing Complaint";
                                    subcategory = "Wrong Reading";
                                    break;
                                case "M03":
                                    category = "Billing Complaint";
                                    subcategory = "Wrong Payment";
                                    break;
                                case "M04":
                                    category = "Billing Complaint";
                                    subcategory = "Payment not credited";
                                    break;
                                case "M14":
                                    category = "Billing Complaint";
                                    subcategory = "Bill Not Received";
                                    break;
                                case "M24":
                                    category = "Others";
                                    subcategory = "Others";
                                    break;
                                case "M52":
                                    category = "Billing Complaint";
                                    subcategory = "High consumption complaint";
                                    break;
                                case "I01":
                                    category = "Billing Complaint";
                                    subcategory = "High consumption complaint";
                                    break;
                                default:
                                    category = complaintToAdd.ComplaintCategory;
                                    subcategory = complaintToAdd.ComplaintSubCategory;
                                    break;

                            }

                            ComplaintPortalRegisteredComplaint objNewComp = new ComplaintPortalRegisteredComplaint
                            {
                                AccountNumber = accountNumber,
                                ComplaintRegistrationNumber = complaintId,
                                ComplaintStatusCode = complaintToAdd.ComplaintStatusCode,
                                ComplaintStatusName = complaintToAdd.ComplaintStatusName,
                                ComplaintIdSAP = retResult.ComplaintNumber,
                                ComplaintCategory = category,
                                ComplaintSubCategory = subcategory,
                                ComplaintLevel = "Level1",
                                ComplaintDescription = "Added from other source, not exists in database added with M20 order",
                                CreatedDate = DateTime.Now,
                                CreatedBy = "Other Source"
                            };
                            dataContext.ComplaintPortalRegisteredComplaints.InsertOnSubmit(objNewComp);
                        }
                        dataContext.SubmitChanges();
                    }

                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at Complaint Portal CreateLoginLog: " + ex.Message + " For Account number:" + accountNumber, this);
                retResult.Error = ex.Message;
            }
            return retResult;
        }

        public string IsEscalatedToCGRFAlready(string accountNumber, string previousComplaintId)
        {
            try
            {
                accountNumber = FormatAccountNumber(accountNumber);
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.ComplaintNumberFromPerviousLevel == previousComplaintId && c.AccountNumber == accountNumber))
                    {
                        return dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintNumberFromPerviousLevel == previousComplaintId && c.AccountNumber == accountNumber).FirstOrDefault().ComplaintRegistrationNumber;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at IsEscalatedToCGRFAlready: " + ex.Message + " For Account number:" + accountNumber, this);
            }
            return null;
        }

        public string GetComplaintCategorySubCategoryName(string categoryId, string subCategoryId)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var subcategory = dataContext.ComplaintPortalComplaintSubCategoryMasters.Where(c => c.CategoryId == System.Convert.ToInt64(categoryId) && c.SubCategoryId == System.Convert.ToInt64(subCategoryId)).FirstOrDefault();

                    return subcategory.ComplaintPortalComplaintCategoryMaster.CategoryName + " - " + subcategory.SubCategory;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintsByAccountNumber: " + ex.Message, this);
            }
            return string.Empty;
        }

        public ComplaintPortalComplaintSubTypeMaster GetComplaintCategorySubtypeDetails(string SubtypeId)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    return dataContext.ComplaintPortalComplaintSubTypeMasters.Where(c => c.Id == System.Convert.ToInt64(SubtypeId)).FirstOrDefault();
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintsByAccountNumber: " + ex.Message, this);
            }
            return null;
        }

        public List<ComplaintDetails> GetSavedComplaint()
        {
            List<ComplaintDetails> result = new List<ComplaintDetails>();
            try
            {
                string accountNumber = FormatAccountNumber(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {

                    var applicationsList = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.AccountNumber.Trim() == accountNumber && c.ComplaintLevel == "Level1" && c.ComplaintStatusCode == "1").OrderByDescending(o => o.CreatedDate).ToList();

                    foreach (var app in applicationsList)
                    {
                        result.Add(new ComplaintDetails
                        {
                            ComplaintRegistrationNumber = app.ComplaintRegistrationNumber,
                            ComplaintCategory = app.ComplaintCategory,
                            ComplaintSubCategory = app.ComplaintSubCategory,
                            ComplaintLevel = app.ComplaintLevel,
                            ComplaintDescription = app.ComplaintDescription,
                            ComplaintId = app.Id,
                            ComplaintNumber = app.ComplaintIdSAP,
                            ComplaintStatusCode = app.ComplaintStatusCode,
                            ComplaintStatusName = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusName,
                            ComplaintStatusDescription = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusDescription,
                            ComplaintZone = app.Zone,
                            CreatedDate = app.CreatedDate
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintsByAccountNumber: " + ex.Message, this);
            }
            return result;
        }

        public List<ComplaintDetails> GetComplaintsByAccountNumber(DateTime? startDate, DateTime? endDate, string status)
        {
            List<ComplaintDetails> result = new List<ComplaintDetails>();
            try
            {
                string accountNumber = FormatAccountNumber(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (startDate == null || endDate == null)
                    {
                        var applicationsList = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.AccountNumber.Trim() == accountNumber).OrderByDescending(o => o.CreatedDate).ToList();

                        foreach (var app in applicationsList)
                        {
                            result.Add(new ComplaintDetails
                            {
                                ComplaintRegistrationNumber = app.ComplaintRegistrationNumber,
                                ComplaintCategory = app.ComplaintCategory,
                                ComplaintSubCategory = app.ComplaintSubCategory,
                                ComplaintLevel = app.ComplaintLevel,
                                ComplaintDescription = app.ComplaintDescription,
                                ComplaintId = app.Id,
                                ComplaintNumber = app.ComplaintIdSAP,
                                ComplaintStatusCode = app.ComplaintStatusCode,
                                ComplaintStatusName = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusName,
                                ComplaintStatusDescription = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusDescription,
                                ComplaintZone = app.Zone,
                                CreatedDate = app.CreatedDate
                            });
                        }
                    }
                    else
                    {
                        var applicationsList = dataContext.ComplaintPortalRegisteredComplaints.Where(a => ((a.CreatedDate >= startDate && a.CreatedDate <= endDate) || a.CreatedDate.Date == endDate.Value.Date || a.CreatedDate.Date == startDate.Value.Date) && a.AccountNumber.Trim() == accountNumber).OrderByDescending(a => a.CreatedDate).ToList();

                        foreach (var app in applicationsList)
                        {
                            result.Add(new ComplaintDetails
                            {
                                ComplaintRegistrationNumber = app.ComplaintRegistrationNumber,
                                ComplaintCategory = app.ComplaintCategory,
                                ComplaintSubCategory = app.ComplaintSubCategory,
                                ComplaintLevel = app.ComplaintLevel,
                                ComplaintDescription = app.ComplaintDescription,
                                ComplaintId = app.Id,
                                ComplaintNumber = app.ComplaintIdSAP,
                                ComplaintStatusCode = app.ComplaintStatusCode,
                                ComplaintStatusName = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusName,
                                ComplaintStatusDescription = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusDescription,
                                ComplaintZone = app.Zone,
                                CreatedDate = app.CreatedDate
                            });
                        }
                    }
                    if (!string.IsNullOrEmpty(status))
                    {
                        result = result.Where(a => a.ComplaintStatusCode.Trim() == status.Trim()).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintsByAccountNumber: " + ex.Message, this);
            }
            return result;
        }

        public List<ComplaintDetails> GetCGRFComplaintsByAccountNumber(DateTime? startDate, DateTime? endDate, string status)
        {
            List<ComplaintDetails> result = new List<ComplaintDetails>();
            try
            {
                string accountNumber = FormatAccountNumber(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (startDate == null || endDate == null)
                    {
                        var applicationsList = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.AccountNumber.Trim() == accountNumber && c.ComplaintLevel.ToLower() == "cgrf").OrderByDescending(o => o.CreatedDate).ToList();

                        foreach (var app in applicationsList)
                        {
                            result.Add(new ComplaintDetails
                            {
                                ComplaintRegistrationNumber = app.ComplaintRegistrationNumber,
                                ComplaintCategory = app.ComplaintCategory,
                                ComplaintSubCategory = app.ComplaintSubCategory,
                                ComplaintLevel = app.ComplaintLevel,
                                ComplaintDescription = app.ComplaintDescription,
                                ComplaintId = app.Id,
                                ComplaintNumber = app.ComplaintIdSAP,
                                ComplaintStatusCode = app.ComplaintStatusCode,
                                ComplaintStatusName = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusName,
                                ComplaintStatusDescription = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusDescription,
                                ComplaintZone = app.Zone,
                                CreatedDate = app.CreatedDate
                            });
                        }
                    }
                    else
                    {
                        var applicationsList = dataContext.ComplaintPortalRegisteredComplaints.Where(a => ((a.CreatedDate >= startDate && a.CreatedDate <= endDate) || a.CreatedDate.Date == endDate.Value.Date || a.CreatedDate.Date == startDate.Value.Date) && a.AccountNumber.Trim() == accountNumber && a.ComplaintLevel.ToLower() == "cgrf").OrderByDescending(a => a.CreatedDate).ToList();

                        foreach (var app in applicationsList)
                        {
                            result.Add(new ComplaintDetails
                            {
                                ComplaintRegistrationNumber = app.ComplaintRegistrationNumber,
                                ComplaintCategory = app.ComplaintCategory,
                                ComplaintSubCategory = app.ComplaintSubCategory,
                                ComplaintLevel = app.ComplaintLevel,
                                ComplaintDescription = app.ComplaintDescription,
                                ComplaintId = app.Id,
                                ComplaintNumber = app.ComplaintIdSAP,
                                ComplaintStatusCode = app.ComplaintStatusCode,
                                ComplaintStatusName = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusName,
                                ComplaintStatusDescription = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusDescription,
                                ComplaintZone = app.Zone,
                                CreatedDate = app.CreatedDate
                            });
                        }
                    }
                    if (!string.IsNullOrEmpty(status))
                    {
                        result = result.Where(a => a.ComplaintStatusCode.Trim() == status.Trim()).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintsByAccountNumber: " + ex.Message, this);
            }
            return result;
        }

        public List<ComplaintDetails> GetCGRFComplaints(DateTime? startDate, DateTime? endDate, string status)
        {
            List<ComplaintDetails> result = new List<ComplaintDetails>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (startDate == null || endDate == null)
                    {
                        var applicationsList = dataContext.ComplaintPortalRegisteredComplaints.Where(a => a.ComplaintLevel.ToLower() == "cgrf").OrderByDescending(o => o.CreatedDate).ToList();
                        foreach (var app in applicationsList)
                        {
                            result.Add(new ComplaintDetails
                            {
                                AccountNumber = app.AccountNumber,
                                CGRFCaseNumber = app.CGRFCaseNumber,
                                ComplaintRegistrationNumber = app.ComplaintRegistrationNumber,
                                ComplaintCategory = app.ComplaintCategory,
                                ComplaintSubCategory = app.ComplaintSubCategory,
                                ComplaintLevel = app.ComplaintLevel,
                                ComplaintDescription = app.ComplaintDescription,
                                ComplaintId = app.Id,
                                ComplaintNumber = app.ComplaintIdSAP,
                                ComplaintStatusCode = app.ComplaintStatusCode,
                                ComplaintStatusName = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusName,
                                ComplaintStatusDescription = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusDescription,
                                ComplaintZone = app.Zone,
                                CreatedDate = app.CreatedDate,
                                StartDate = app.StartDate,
                                ClosedDate = app.ClosedDate
                                //StartDate = dataContext.ComplaintPortalCGRFComplaintHistories.Any(c => c.ComplaintId == app.Id && c.Status == ((int)ComplaintStatus.Approved).ToString()) ? dataContext.ComplaintPortalCGRFComplaintHistories.FirstOrDefault(c => c.ComplaintId == app.Id && c.Status == ((int)ComplaintStatus.Approved).ToString()).CreatedDate.ToString() : null,
                                //ClosedDate = dataContext.ComplaintPortalCGRFComplaintHistories.Any(c => c.ComplaintId == app.Id && c.Status == ((int)ComplaintStatus.Closed).ToString()) ? dataContext.ComplaintPortalCGRFComplaintHistories.FirstOrDefault(c => c.ComplaintId == app.Id && c.Status == ((int)ComplaintStatus.Closed).ToString()).CreatedDate.ToString() : null
                            });
                        }
                    }
                    else
                    {
                        var applicationsList = dataContext.ComplaintPortalRegisteredComplaints.Where(a => ((a.CreatedDate >= startDate && a.CreatedDate <= endDate) || a.CreatedDate.Date == endDate.Value.Date || a.CreatedDate.Date == startDate.Value.Date) && a.ComplaintLevel.ToLower() == "cgrf").OrderByDescending(a => a.CreatedDate).ToList();
                        foreach (var app in applicationsList)
                        {
                            result.Add(new ComplaintDetails
                            {
                                AccountNumber = app.AccountNumber,
                                CGRFCaseNumber = app.CGRFCaseNumber,
                                ComplaintRegistrationNumber = app.ComplaintRegistrationNumber,
                                ComplaintCategory = app.ComplaintCategory,
                                ComplaintSubCategory = app.ComplaintSubCategory,
                                ComplaintLevel = app.ComplaintLevel,
                                ComplaintDescription = app.ComplaintDescription,
                                ComplaintId = app.Id,
                                ComplaintNumber = app.ComplaintIdSAP,
                                ComplaintStatusCode = app.ComplaintStatusCode,
                                ComplaintStatusName = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusName,
                                ComplaintStatusDescription = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusDescription,
                                ComplaintZone = app.Zone,
                                CreatedDate = app.CreatedDate,
                                StartDate = app.StartDate,
                                ClosedDate = app.ClosedDate
                                //StartDate = dataContext.ComplaintPortalCGRFComplaintHistories.Any(c => c.ComplaintId == app.Id && c.Status == ((int)ComplaintStatus.Approved).ToString()) ? dataContext.ComplaintPortalCGRFComplaintHistories.FirstOrDefault(c => c.ComplaintId == app.Id && c.Status == ((int)ComplaintStatus.Approved).ToString()).CreatedDate.ToString() : null,
                                //ClosedDate = dataContext.ComplaintPortalCGRFComplaintHistories.Any(c => c.ComplaintId == app.Id && c.Status == ((int)ComplaintStatus.Closed).ToString()) ? dataContext.ComplaintPortalCGRFComplaintHistories.FirstOrDefault(c => c.ComplaintId == app.Id && c.Status == ((int)ComplaintStatus.Closed).ToString()).CreatedDate.ToString() : null
                            });
                        }
                    }
                    if (!string.IsNullOrEmpty(status))
                    {
                        result = result.Where(a => a.ComplaintStatusCode.Trim() == status.Trim()).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintsByAccountNumber: " + ex.Message, this);
            }
            return result;
        }

        public List<ComplaintDetails> GetICRSComplaints(DateTime? startDate, DateTime? endDate, string status)
        {
            List<ComplaintDetails> result = new List<ComplaintDetails>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (startDate == null || endDate == null)
                    {
                        var applicationsList = dataContext.ComplaintPortalRegisteredComplaints.Where(a => a.ComplaintLevel.ToLower() == "level1").OrderByDescending(o => o.CreatedDate).ToList();
                        foreach (var app in applicationsList)
                        {
                            result.Add(new ComplaintDetails
                            {
                                MobileNumber = app.MobileNumber,
                                AccountNumber = app.AccountNumber,
                                ConsumerName = app.ConsumerName,
                                Level1ComplaintNumber = app.ComplaintIdSAP,
                                ComplaintRegistrationNumber = app.ComplaintRegistrationNumber,
                                ComplaintCategory = app.ComplaintCategory,
                                ComplaintSubCategory = app.ComplaintSubCategory,
                                ComplaintDescription = app.ComplaintDescription,
                                CreatedDate = app.CreatedDate,
                                EscalatedToCGRF = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintLevel.ToLower() == "cgrf" && c.ComplaintNumberFromPerviousLevel == app.ComplaintIdSAP).Any() ? dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintLevel.ToLower() == "cgrf" && c.ComplaintNumberFromPerviousLevel == app.ComplaintIdSAP).FirstOrDefault().ComplaintRegistrationNumber : ""
                            });
                        }
                    }
                    else
                    {
                        var applicationsList = dataContext.ComplaintPortalRegisteredComplaints.Where(a => ((a.CreatedDate >= startDate && a.CreatedDate <= endDate) || a.CreatedDate.Date == endDate.Value.Date || a.CreatedDate.Date == startDate.Value.Date) && a.ComplaintLevel.ToLower() == "level1").OrderByDescending(a => a.CreatedDate).ToList();
                        foreach (var app in applicationsList)
                        {
                            result.Add(new ComplaintDetails
                            {
                                MobileNumber = app.MobileNumber,
                                AccountNumber = app.AccountNumber,
                                ConsumerName = app.ConsumerName,
                                Level1ComplaintNumber = app.ComplaintIdSAP,
                                ComplaintRegistrationNumber = app.ComplaintRegistrationNumber,
                                ComplaintCategory = app.ComplaintCategory,
                                ComplaintSubCategory = app.ComplaintSubCategory,
                                ComplaintDescription = app.ComplaintDescription,
                                CreatedDate = app.CreatedDate,
                                EscalatedToCGRF = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintLevel.ToLower() == "cgrf" && c.ComplaintNumberFromPerviousLevel == app.ComplaintIdSAP).Any() ? dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintLevel.ToLower() == "cgrf" && c.ComplaintNumberFromPerviousLevel == app.ComplaintIdSAP).FirstOrDefault().ComplaintRegistrationNumber : ""
                            });
                        }
                    }

                    foreach (var c in result)
                    {
                        var userDetails21 = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(c.AccountNumber);

                        c.ComplaintZone = userDetails21.ZoneName;
                        c.Division = userDetails21.DivisionName;

                        var orderstatusdetails = SapPiService.Services.RequestHandler.ExtrOrdGISService(c.ComplaintRegistrationNumber);
                        //Status: IPHAS.IPHAS = 2: Pending, IPHAS = 3: Completed, IPHAS = 6:Cancelled
                        if (orderstatusdetails != null && !string.IsNullOrEmpty(orderstatusdetails.Complaint_Status))
                        {
                            if (orderstatusdetails.Complaint_Status == "2")
                                c.ComplaintStatusName = "Pending";
                            else if (orderstatusdetails.Complaint_Status == "3")
                                c.ComplaintStatusName = "Completed";
                            else if (orderstatusdetails.Complaint_Status == "6")
                                c.ComplaintStatusName = "Cancelled";

                            c.CreatedOnSAP = orderstatusdetails.CreatedDate;
                            c.CompletionDate = orderstatusdetails.CompletionDate;
                            //c.ComplaintZone = orderstatusdetails.ZoneName;
                            c.PMActivityType = orderstatusdetails.PMActivityType;
                        }
                        else
                        {
                            var complaints = SapPiService.Services.RequestHandler.FetchComplaints(c.AccountNumber);
                            if (complaints.Any(o => o.AUFNR == c.ComplaintRegistrationNumber))
                            {
                                var com = complaints.Where(o => o.AUFNR == c.ComplaintRegistrationNumber).FirstOrDefault();
                                c.CreatedOnSAP = com.ERDate;
                                c.ComplaintStatusName = com.Complaint_Status;
                                c.PMActivityType = com.ILART;
                            }
                            else
                            {
                                //no supply complaint details 

                                int noofmonth = 1;
                                if (startDate != null && endDate != null)
                                {
                                    int syear = startDate.Value.Year;
                                    int eyear = endDate.Value.Year;
                                    if (syear == eyear)
                                    {
                                        noofmonth = endDate.Value.Month - startDate.Value.Month;
                                    }
                                    else
                                    {
                                        noofmonth = (12 - startDate.Value.Month) + endDate.Value.Month;
                                    }
                                }
                                ComplaintGetCaseDetailsSAP nscomplaintDetails = FetchcomplaintDetails(c.AccountNumber, noofmonth.ToString());
                                if (nscomplaintDetails != null && nscomplaintDetails.Complaints != null && nscomplaintDetails.Complaints.Count > 0 && nscomplaintDetails.Complaints.Any(com => com.Complaintnumber == c.ComplaintRegistrationNumber))
                                {
                                    var nscom = nscomplaintDetails.Complaints.Where(com => com.Complaintnumber == c.ComplaintRegistrationNumber).FirstOrDefault();
                                    c.CreatedOnSAP = nscom.CreatedOn;
                                    c.ComplaintStatusName = nscom.ComplaintStatus == "1" ? "Completed" : "Open";
                                    c.PMActivityType = "No Supply";
                                }
                            }
                        }

                        try
                        {
                            switch (c.ComplaintSubCategory)
                            {
                                case "M40"://3 days
                                    c.TATDate = DateTime.Parse(c.CreatedOnSAP.ToString()).AddDays(3).ToString();
                                    break;
                                case "M20"://3 days
                                    c.TATDate = c.TATDate;
                                    break;
                                case "M50"://24 hrs
                                    c.TATDate = DateTime.Parse(c.CreatedOnSAP.ToString()).AddDays(1).ToString();
                                    break;
                                case "M51"://24 hrs
                                    c.TATDate = DateTime.Parse(c.CreatedOnSAP.ToString()).AddDays(1).ToString();
                                    break;
                                case "M06"://15 days
                                    c.TATDate = DateTime.Parse(c.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                    break;
                                case "I15"://15 days
                                    c.TATDate = DateTime.Parse(c.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                    break;
                                case "M03"://15 days
                                    c.TATDate = DateTime.Parse(c.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                    break;
                                case "M04"://15 days
                                    c.TATDate = DateTime.Parse(c.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                    break;
                                case "M14"://3 days
                                    c.TATDate = DateTime.Parse(c.CreatedOnSAP.ToString()).AddDays(3).ToString();
                                    break;
                                case "M24"://15 days
                                    c.TATDate = DateTime.Parse(c.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                    break;
                                case "M52"://15 days
                                    c.TATDate = DateTime.Parse(c.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                    break;
                                default:
                                    c.TATDate = DateTime.Parse(c.CreatedOnSAP.ToString()).AddDays(1).ToString();
                                    break;
                            }
                        }
                        catch
                        {

                        }

                        var numberOfDays_PassedTAT = 0;
                        var numberOfDays_LeftTAT = 0;

                        try
                        {
                            //calculate TAT passed date and TAT days left
                            if (c.ComplaintStatusName == "Pending" && !string.IsNullOrEmpty(c.TATDate))
                            {
                                var tatDate = System.Convert.ToDateTime(c.TATDate);
                                if (tatDate.Date >= DateTime.Now.Date)
                                {
                                    numberOfDays_LeftTAT = (DateTime.Now.Date - tatDate.Date).Days;
                                    c.TATLeftDays = numberOfDays_LeftTAT;
                                    if (numberOfDays_LeftTAT <= 3)
                                        c.OrangeRedGreen = 1;
                                    else
                                        c.OrangeRedGreen = 3;
                                }
                                else
                                {
                                    numberOfDays_PassedTAT = (tatDate.Date - DateTime.Now.Date).Days;
                                    c.TATPassedDays = numberOfDays_PassedTAT;
                                    c.OrangeRedGreen = 2;
                                }
                            }
                            else
                            {
                                c.TATPassedDays = 0;
                                c.TATLeftDays = 0;
                                c.OrangeRedGreen = 3;
                            }
                        }
                        catch (Exception e)
                        {
                            Log.Error("Get Complaints: " + e.Message, this);
                        }
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        if (status == "1")
                            result = result.Where(a => !string.IsNullOrEmpty(a.Level1ComplaintNumber)).ToList();
                        else if (status == "2")
                            result = result.Where(a => !string.IsNullOrEmpty(a.EscalatedToCGRF)).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintsByAccountNumber: " + ex.Message, this);
            }
            return result;
        }


        public List<ComplaintDetailsForReport> GetCGRFComplaintsForReport(DateTime startDate, DateTime endDate, string consumerZone, string consumerDivision, string complaintstatus, string complaintCategory)
        {
            List<ComplaintDetailsForReport> resultcomplaints = new List<ComplaintDetailsForReport>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (startDate != null || endDate != null)
                    {
                        var result = dataContext.ComplaintPortalRegisteredComplaints.Where(a => a.ComplaintLevel.ToLower() == "cgrf" && ((a.CreatedDate >= startDate && a.CreatedDate <= endDate) || a.CreatedDate.Date == endDate.Date || a.CreatedDate.Date == startDate.Date)).OrderByDescending(o => o.CreatedDate).ToList();

                        if (!string.IsNullOrEmpty(complaintCategory))
                        {
                            result = result.Where(a => a.ComplaintCategory.ToLower() == complaintCategory.ToLower()).OrderByDescending(o => o.CreatedDate).ToList();
                        }
                        if (!string.IsNullOrEmpty(consumerDivision))
                        {
                            List<string> zones = dataContext.ComplaintPortalCGRFZoneDivisionMasters.Where(z => z.ZoneName.ToLower() == consumerDivision.ToLower()).Select(d => d.DivisionName).ToList();
                            result = result.Where(a => zones.Contains(a.Zone)).OrderByDescending(o => o.CreatedDate).ToList();
                        }
                        if (!string.IsNullOrEmpty(consumerZone))
                        {
                            result = result.Where(a => a.Zone.ToLower() == consumerZone.ToLower()).OrderByDescending(o => o.CreatedDate).ToList();
                        }
                        if (!string.IsNullOrEmpty(complaintstatus))
                        {
                            result = result.Where(a => a.ComplaintStatusCode == complaintstatus).OrderByDescending(o => o.CreatedDate).ToList();
                        }

                        foreach (var c in result)
                        {
                            resultcomplaints.Add(new ComplaintDetailsForReport
                            {
                                ComplaintId = c.Id,
                                Address = c.Address.Replace(",", ";"),
                                CaseNumber = c.CGRFCaseNumber,
                                City = c.City,
                                ClosedDate = c.ClosedDate == null ? "" : c.ClosedDate.ToString(),
                                ComplaintCategory = c.ComplaintCategory,
                                ComplaintCategoryOtherText = c.ComplaintCategoryOtherText,
                                ComplaintDescription = c.ComplaintDescription.Replace(",", ";"),
                                ComplaintReason = c.ReasonToApplyComplaint == null ? "" : c.ReasonToApplyComplaint.Replace(",", ";"),
                                ComplaintStatusDescription = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(a => a.ComplaintStatusCode == c.ComplaintStatusCode).FirstOrDefault().ComplaintStatusDescription,
                                AccountNumber = c.AccountNumber,
                                ConsumerCategory = c.ConsumerCategory,
                                ConsumerName = c.ConsumerName,
                                CreatedDate = c.CreatedDate,
                                Email = c.Email,
                                Mobile = c.Mobile,
                                MobileNumber = c.Mobile,
                                NodalReplyDueDate = c.NodalReplyTATDate == null ? "" : c.NodalReplyTATDate.ToString(),
                                Pincode = c.Pincode,
                                RegistrationTrackingNumber = c.ComplaintRegistrationNumber,
                                SecretaryActionDueDate = c.SecretaryTATDate == null ? "" : c.SecretaryTATDate.ToString(),
                                StartDate = c.StartDate == null ? "" : c.StartDate.ToString(),
                                Zone = c.Zone
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintsByAccountNumber: " + ex.Message, this);
            }
            return resultcomplaints;
        }


        public bool CheckExtension(HttpPostedFileBase postedFile)
        {
            if (!string.Equals(postedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/gif", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var postedFileExtension = Path.GetExtension(postedFile.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".gif", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            int ImageMinimumBytes = 10;
            //-------------------------------------------
            //  Attempt to read the file and check the first bytes
            //-------------------------------------------
            try
            {
                if (!postedFile.InputStream.CanRead)
                {
                    return false;
                }
                //------------------------------------------
                //   Check whether the image size exceeding the limit or not
                //------------------------------------------ 
                if (postedFile.ContentLength < ImageMinimumBytes)
                {
                    return false;
                }

                byte[] buffer = new byte[ImageMinimumBytes];
                postedFile.InputStream.Read(buffer, 0, ImageMinimumBytes);
                string content = System.Text.Encoding.UTF8.GetString(buffer);
                if (Regex.IsMatch(content, @"<script|<html|<head|<title|<body|<pre|<table|<a\s+href|<img|<plaintext|<cross\-domain\-policy",
                    RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Multiline))
                {
                    return false;
                }
            }
            catch (Exception)
            {
                return false;
            }

            //-------------------------------------------
            //  Try to instantiate new Bitmap, if .NET will throw exception
            //  we can assume that it's not a valid image
            //-------------------------------------------

            try
            {
                using (var bitmap = new System.Drawing.Bitmap(postedFile.InputStream))
                {
                }
            }
            catch (Exception)
            {
                return false;
            }
            finally
            {
                postedFile.InputStream.Position = 0;
            }
            return true;

            ////'jpg', 'jpeg', 'dwg', 'pdf', 'doc', 'docx', 'xls', 'xlsx'
            //string fileExtension = System.IO.Path.GetExtension(fileName);
            //if (fileExtension == ".jpg" || fileExtension == ".JPG" ||
            //    fileExtension == ".png" || fileExtension == ".PNG" ||
            //    fileExtension == ".jpeg" || fileExtension == ".JPEG"
            //    )
            //    return true;
            //else
            //    return false;
        }

        public bool ValidateScheduleAFile(HttpPostedFileBase postedFile)
        {
            if (!string.Equals(postedFile.ContentType, "application/octet-stream", StringComparison.OrdinalIgnoreCase) &&
                 !string.Equals(postedFile.ContentType, "application/vnd.openxmlformats-officedocument.wordprocessingml.document", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "application/msword", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var postedFileExtension = Path.GetExtension(postedFile.FileName);
            if (!string.Equals(postedFileExtension, ".pdf", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".doc", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".docx", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (postedFile.ContentLength > (10 * 1024 * 1024))
            {
                return false;
            }

            return true;
        }

        public bool ValidateComplaintDocument(HttpPostedFileBase postedFile)
        {
            if (!string.Equals(postedFile.ContentType, "image/jpg", StringComparison.OrdinalIgnoreCase) &&
                !string.Equals(postedFile.ContentType, "application/pdf", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/jpeg", StringComparison.OrdinalIgnoreCase) &&
            !string.Equals(postedFile.ContentType, "image/png", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            var postedFileExtension = Path.GetExtension(postedFile.FileName);
            if (!string.Equals(postedFileExtension, ".jpg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".png", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".jpeg", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(postedFileExtension, ".pdf", StringComparison.OrdinalIgnoreCase))
            {
                return false;
            }

            if (postedFile.ContentLength > (10 * 1024 * 1024))
            {
                return false;
            }

            return true;
        }

        public bool CheckIfComplaintExists(string complaintId)
        {
            try
            {
                string accountNumber = FormatAccountNumber(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.Id == System.Convert.ToInt32(complaintId) && c.AccountNumber == accountNumber))
                        return true;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintCategoryList: " + ex.Message, this);
            }
            return false;
        }

        public ComplaintFileRegistrationModel GetComplaintDetails(string complaintId)
        {
            ComplaintFileRegistrationModel model = new ComplaintFileRegistrationModel();
            try
            {

                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    ComplaintPortalRegisteredComplaint complaint;
                    if (UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                    {
                        complaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == System.Convert.ToInt32(complaintId)).FirstOrDefault();
                    }
                    else
                    {
                        string accountNumber = FormatAccountNumber(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                        complaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == System.Convert.ToInt32(complaintId) && c.AccountNumber == accountNumber).FirstOrDefault();

                    }
                    if (complaint != null)
                    {
                        model.ComplaintRegistrationNumber = complaint.ComplaintRegistrationNumber;
                        model.ComplaintStatusDescription = complaint.ComplaintStatusName;
                        model.ComplaintFromPreviousLevel = complaint.ComplaintNumberFromPerviousLevel;
                        model.RemarksFromPreviousLevel = complaint.AdminRemarks;
                        model.ConsumerName = complaint.ConsumerName;
                        model.AccountNumber = complaint.AccountNumber;
                        model.Address = complaint.Address;
                        model.City = complaint.City;
                        model.SelectedComplaintCategory = complaint.ComplaintCategoryId.ToString();
                        model.SelectedComplaintSubCategory = complaint.ComplaintSubCategoryId.ToString();
                        model.ComplaintDescription = complaint.ComplaintDescription;
                        //model.ComplaintFile
                        model.SelectedConsumerCategory = complaint.ConsumerCategory;
                        model.EmailId = complaint.Email;
                        model.LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName;
                        model.MobileNumber = complaint.Mobile;
                        model.Pincode = complaint.Pincode;
                        model.ZoneName = complaint.Zone;
                        model.ComplaintStatus = complaint.ComplaintStatusCode;
                        model.ComplaintId = complaint.Id.ToString();
                        model.IsDocumentUploaded = complaint.DocumentData == null ? false : true;

                        model.ComplaintFromPreviousLevel = complaint.ComplaintNumberFromPerviousLevel;
                        model.RemarksFromPreviousLevel = complaint.ComplaintRemarksPerviousLevel;

                        model.ComplaintDescriptionFromPreviousLevel = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == complaint.ComplaintNumberFromPerviousLevel).Any() ? dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == complaint.ComplaintNumberFromPerviousLevel).FirstOrDefault().ComplaintDescription : null;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetComplaintCategoryList: " + ex.Message, this);
            }
            return model;
        }

        public CGRFComplaintFileRegistrationModel GetCGRFComplaintDetails(string complaintId)
        {
            CGRFComplaintFileRegistrationModel model = new CGRFComplaintFileRegistrationModel();
            try
            {

                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                    {
                        ComplaintPortalRegisteredComplaint complaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == System.Convert.ToInt32(complaintId)).FirstOrDefault();
                        if (complaint != null)
                        {
                            model.ComplaintRegistrationNumber = complaint.ComplaintRegistrationNumber;
                            model.ComplaintStatusDescription = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == complaint.ComplaintStatusCode).FirstOrDefault().ComplaintStatusDescription;
                            model.ComplaintCategory = complaint.ComplaintCategory;
                            model.ConsumerName = complaint.ConsumerName;
                            model.AccountNumber = complaint.AccountNumber;
                            model.Address = complaint.Address;
                            model.City = complaint.City;
                            model.SelectedComplaintCategory = complaint.ComplaintCategory;
                            model.ComplaintDescription = complaint.ComplaintDescription;
                            model.SelectedConsumerCategory = complaint.ConsumerCategory;
                            model.EmailId = complaint.Email;
                            model.LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName;
                            model.MobileNumber = complaint.Mobile;
                            model.Pincode = complaint.Pincode;
                            model.SelectedConsumerZone = complaint.Zone;
                            model.ComplaintStatus = complaint.ComplaintStatusCode;
                            model.ComplaintId = complaint.Id.ToString();
                            model.IsDocumentUploaded = complaint.DocumentData == null ? false : true;
                            model.DocumentName = complaint.DocumentData == null ? "" : complaint.DocumentFileName;
                            model.OtherCategoryText = complaint.ComplaintCategoryOtherText;
                            model.AdminRemarks = complaint.AdminRemarks;
                            model.ComplaintFromPreviousLevel = complaint.ComplaintNumberFromPerviousLevel;
                            model.RemarksFromPreviousLevel = complaint.ComplaintRemarksPerviousLevel;

                            model.ComplaintDescriptionFromPreviousLevel = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == complaint.ComplaintNumberFromPerviousLevel).Any() ? dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == complaint.ComplaintNumberFromPerviousLevel).FirstOrDefault().ComplaintDescription : null;

                            model.AppliedDate = complaint.CreatedDate;
                            model.CGRFCaseNumber = complaint.CGRFCaseNumber;
                            model.TantetiveHearingDate = complaint.TentativeHearingDate;

                            model.ReasonToApply = complaint.ReasonToApplyComplaint;
                            model.ComplaintFromPreviousLevelAppliedDate = complaint.ComplaintFromPerviousLevelAppliedDate == null ? string.Empty : complaint.ComplaintFromPerviousLevelAppliedDate.ToString();

                            var docsList = dataContext.ComplaintPortalCGRFComplaintDocuments.Where(c => c.ComplaintNumber == System.Convert.ToInt32(complaintId)).OrderByDescending(d => d.CreatedDate).ToList();
                            if (docsList != null && docsList.Count() > 0)
                            {
                                model.ComplaintSupportingDocuments = new List<CGRFComplaintDoc>();
                                foreach (var doc in docsList)
                                {
                                    model.ComplaintSupportingDocuments.Add(new CGRFComplaintDoc
                                    {
                                        DocumentId = doc.Id.ToString(),
                                        DocumentName = doc.DocumentName,
                                        DocumentType = doc.DocumentType,
                                        IsReviewDocument = doc.IsReviewRequestRaised == null || doc.IsReviewRequestRaised == false ? false : true
                                    });
                                }
                            }

                            var hearingDates = dataContext.ComplaintPortalCGRFComplaintHearingSchedules.Where(c => c.ComplaintId == System.Convert.ToInt32(complaintId)).OrderBy(d => d.DateOfHearing).ToList();
                            model.ComplaintHearings = hearingDates.Where(h => h.IsOrderReviewRequest == null || h.IsOrderReviewRequest == false).ToList();
                            model.ComplaintHearingsOrderReviewRequest = hearingDates.Where(h => h.IsOrderReviewRequest == true).ToList();
                            model.ComplaintHistoryRecords = dataContext.ComplaintPortalCGRFComplaintHistories.Where(c => c.ComplaintId == System.Convert.ToInt32(complaintId)).OrderBy(d => d.CreatedDate).ToList();
                            model.IsReviewRequestRaised = complaint.IsReviewRequestRaised == true ? true : false;

                            if (dataContext.ComplaintPortalRegisteredComplaintsOrderReviewRequests.Where(c => c.ComplaintId == complaint.Id).Any())
                            {
                                var reviewRequest = dataContext.ComplaintPortalRegisteredComplaintsOrderReviewRequests.Where(c => c.ComplaintId == complaint.Id).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                                if (reviewRequest != null)
                                {
                                    model.IsAppealPreferred = reviewRequest.IsAppealPreferred == true ? "Yes" : "No";
                                    model.IsAppliedWithin30Days = reviewRequest.IsAppliedWithin30Days == true ? "Yes" : "No";
                                    model.IsImportantMatterDiscovery = reviewRequest.IsImportantMatterDiscovery == true ? "Yes" : "No";
                                    model.IsErrorApparent = reviewRequest.IsErrorApparent == true ? "Yes" : "No";
                                }
                            }
                        }
                    }
                    else
                    {
                        string accountNumber = FormatAccountNumber(UserSession.AEMLComplaintUserSessionContext.AccountNumber);

                        ComplaintPortalRegisteredComplaint complaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == System.Convert.ToInt32(complaintId) && c.AccountNumber == accountNumber).FirstOrDefault();
                        if (complaint != null)
                        {
                            model.ComplaintCategory = complaint.ComplaintCategory;
                            model.ComplaintRegistrationNumber = complaint.ComplaintRegistrationNumber;
                            model.ComplaintStatusDescription = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == complaint.ComplaintStatusCode).FirstOrDefault().ComplaintStatusDescription;
                            model.ConsumerName = complaint.ConsumerName;
                            model.AccountNumber = complaint.AccountNumber;
                            model.Address = complaint.Address;
                            model.City = complaint.City;
                            model.SelectedComplaintCategory = complaint.ComplaintCategory;
                            model.ComplaintDescription = complaint.ComplaintDescription;
                            model.SelectedConsumerCategory = complaint.ConsumerCategory;
                            model.EmailId = complaint.Email;
                            model.LoginName = UserSession.AEMLComplaintUserSessionContext.LoginName;
                            model.MobileNumber = complaint.Mobile;
                            model.Pincode = complaint.Pincode;
                            model.SelectedConsumerZone = complaint.Zone;
                            model.ComplaintStatus = complaint.ComplaintStatusCode;
                            model.ComplaintId = complaint.Id.ToString();
                            model.IsDocumentUploaded = complaint.DocumentData == null ? false : true;
                            model.DocumentName = complaint.DocumentData == null ? "" : complaint.DocumentFileName;
                            model.OtherCategoryText = complaint.ComplaintCategoryOtherText;
                            model.AdminRemarks = complaint.AdminRemarks;
                            model.ComplaintFromPreviousLevel = complaint.ComplaintNumberFromPerviousLevel;
                            model.RemarksFromPreviousLevel = complaint.ComplaintRemarksPerviousLevel;

                            model.AppliedDate = complaint.CreatedDate;
                            model.CGRFCaseNumber = complaint.CGRFCaseNumber;
                            model.TantetiveHearingDate = complaint.TentativeHearingDate;

                            model.ReasonToApply = complaint.ReasonToApplyComplaint;
                            model.ComplaintFromPreviousLevelAppliedDate = complaint.ComplaintFromPerviousLevelAppliedDate == null ? string.Empty : complaint.ComplaintFromPerviousLevelAppliedDate.ToString();

                            model.ComplaintDescriptionFromPreviousLevel = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == complaint.ComplaintNumberFromPerviousLevel).Any() ? dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == complaint.ComplaintNumberFromPerviousLevel).FirstOrDefault().ComplaintDescription : null;

                            var docsList = dataContext.ComplaintPortalCGRFComplaintDocuments.Where(c => c.ComplaintNumber == System.Convert.ToInt32(complaintId)).OrderByDescending(d => d.CreatedDate).ToList();
                            if (docsList != null && docsList.Count() > 0)
                            {
                                model.ComplaintSupportingDocuments = new List<CGRFComplaintDoc>();
                                foreach (var doc in docsList)
                                {
                                    model.ComplaintSupportingDocuments.Add(new CGRFComplaintDoc
                                    {
                                        DocumentId = doc.Id.ToString(),
                                        DocumentName = doc.DocumentName,
                                        DocumentType = doc.DocumentType,
                                        IsReviewDocument = doc.IsReviewRequestRaised == null || doc.IsReviewRequestRaised == false ? false : true
                                    });
                                }
                            }

                            var hearingDates = dataContext.ComplaintPortalCGRFComplaintHearingSchedules.Where(c => c.ComplaintId == System.Convert.ToInt32(complaintId)).OrderBy(d => d.DateOfHearing).ToList();
                            model.ComplaintHearings = hearingDates.Where(h => h.IsOrderReviewRequest == null || h.IsOrderReviewRequest == false).ToList();
                            model.ComplaintHearingsOrderReviewRequest = hearingDates.Where(h => h.IsOrderReviewRequest == true).ToList();

                            model.ComplaintHistoryRecords = dataContext.ComplaintPortalCGRFComplaintHistories.Where(c => c.ComplaintId == System.Convert.ToInt32(complaintId)).OrderBy(d => d.CreatedDate).ToList();

                            model.IsReviewRequestRaised = complaint.IsReviewRequestRaised == true ? true : false;

                            if (dataContext.ComplaintPortalRegisteredComplaintsOrderReviewRequests.Where(c => c.ComplaintId == complaint.Id).Any())
                            {
                                var reviewRequest = dataContext.ComplaintPortalRegisteredComplaintsOrderReviewRequests.Where(c => c.ComplaintId == complaint.Id).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                                if (reviewRequest != null)
                                {
                                    model.IsAppealPreferred = reviewRequest.IsAppealPreferred == true ? "Yes" : "No";
                                    model.IsAppliedWithin30Days = reviewRequest.IsAppliedWithin30Days == true ? "Yes" : "No";
                                    model.IsImportantMatterDiscovery = reviewRequest.IsImportantMatterDiscovery == true ? "Yes" : "No";
                                    model.IsErrorApparent = reviewRequest.IsErrorApparent == true ? "Yes" : "No";
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetCGRFComplaintDetails: " + ex.Message, this);
            }
            return model;
        }

        public CGRFComplaintFileRegistrationModel GetCGRFComplaintDocumentsOnFormPost(string complaintId)
        {
            CGRFComplaintFileRegistrationModel model = new CGRFComplaintFileRegistrationModel();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                    {
                        ComplaintPortalRegisteredComplaint complaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == System.Convert.ToInt32(complaintId)).FirstOrDefault();
                        if (complaint != null)
                        {
                            model.ComplaintRegistrationNumber = complaint.ComplaintRegistrationNumber;
                            model.ComplaintStatusDescription = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == complaint.ComplaintStatusCode).FirstOrDefault().ComplaintStatusDescription;

                            model.IsDocumentUploaded = complaint.DocumentData == null ? false : true;
                            model.DocumentName = complaint.DocumentData == null ? "" : complaint.DocumentFileName;

                            var docsList = dataContext.ComplaintPortalCGRFComplaintDocuments.Where(c => c.ComplaintNumber == System.Convert.ToInt32(complaintId) && c.DocumentType == "SD").ToList();
                            if (docsList != null && docsList.Count() > 0)
                            {
                                model.ComplaintSupportingDocuments = new List<CGRFComplaintDoc>();
                                foreach (var doc in docsList)
                                {
                                    model.ComplaintSupportingDocuments.Add(new CGRFComplaintDoc
                                    {
                                        DocumentId = doc.Id.ToString(),
                                        DocumentName = doc.DocumentName
                                    });
                                }
                            }
                        }
                    }
                    else
                    {
                        string accountNumber = FormatAccountNumber(UserSession.AEMLComplaintUserSessionContext.AccountNumber);

                        ComplaintPortalRegisteredComplaint complaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == System.Convert.ToInt32(complaintId) && c.AccountNumber == accountNumber).FirstOrDefault();
                        if (complaint != null)
                        {
                            model.ComplaintRegistrationNumber = complaint.ComplaintRegistrationNumber;
                            model.ComplaintStatusDescription = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == complaint.ComplaintStatusCode).FirstOrDefault().ComplaintStatusDescription;

                            model.IsDocumentUploaded = complaint.DocumentData == null ? false : true;
                            model.DocumentName = complaint.DocumentData == null ? "" : complaint.DocumentFileName;

                            var docsList = dataContext.ComplaintPortalCGRFComplaintDocuments.Where(c => c.ComplaintNumber == System.Convert.ToInt32(complaintId) && c.DocumentType == "SD").ToList();
                            if (docsList != null && docsList.Count() > 0)
                            {
                                model.ComplaintSupportingDocuments = new List<CGRFComplaintDoc>();
                                foreach (var doc in docsList)
                                {
                                    model.ComplaintSupportingDocuments.Add(new CGRFComplaintDoc
                                    {
                                        DocumentId = doc.Id.ToString(),
                                        DocumentName = doc.DocumentName
                                    });
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetCGRFComplaintDetails: " + ex.Message, this);
            }
            return model;
        }

        public bool UpdateComplaintStatus(string complaintId, string remarks, int statusId, string statusName)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                    {
                        ComplaintPortalRegisteredComplaint complaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == System.Convert.ToInt32(complaintId)).FirstOrDefault();
                        if (complaint != null)
                        {
                            complaint.ComplaintStatusCode = statusId.ToString();
                            complaint.ComplaintStatusName = statusName;
                            complaint.AdminRemarks = remarks;

                            dataContext.ComplaintPortalCGRFComplaintHistories.InsertOnSubmit(new ComplaintPortalCGRFComplaintHistory
                            {
                                AdminRemarks = remarks,
                                ComplaintId = System.Convert.ToInt64(complaintId),
                                CreatedDate = DateTime.Now,
                                Status = statusId.ToString(),
                                CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                                Description = "Application is Updated by Admin"
                            });

                            //if application is approved then auto hearing scheduling to be done 
                            //                            c.Auto scheduling of TENTATIVE Hearing upon grievance registration
                            //i.  8th working day: for grievance types (New Connection / Disconnection - Reconnection / No Supply)
                            //ii. 30th working day:  for grievance types (Billing & Other types of complaints) 

                            //                                    6.CGRF Case No.will get auto created(ID would contain CGRF Month 2 digits - Case No in 3 digits - Year) For eg(CGRF01003 / 2021) - CGRF01 - Jan, 003 - Case No.3, 2021 - Year

                            if (statusName == ComplaintStatus.Approved.ToString())
                            {
                                if (complaint.ComplaintCategory == "Non Supply" || complaint.ComplaintCategory == "New Connection"
                                    || complaint.ComplaintCategory == "Disconnection" || complaint.ComplaintCategory == "Reconnection")
                                {
                                    complaint.TentativeHearingDate = DateTime.Now.Date.AddDays(8);
                                    complaint.NodalReplyTATDate = DateTime.Now.Date.AddDays(5);
                                    complaint.SecretaryTATDate = DateTime.Now.Date.AddDays(15);
                                }
                                else
                                {
                                    complaint.TentativeHearingDate = DateTime.Now.Date.AddDays(30);
                                    complaint.NodalReplyTATDate = DateTime.Now.Date.AddDays(15);
                                    complaint.SecretaryTATDate = DateTime.Now.Date.AddDays(60);
                                }

                                //Int64 numberOfApprovedComplaints = dataContext.ComplaintPortalCGRFApprovedComplaints.Count();

                                //Int64 numberOfApprovedComplaints = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintLevel.ToLower() == "cgrf" && c.CGRFCaseNumber != null).Count();
                                //complaint.CGRFCaseNumber = "CGRF" + DateTime.Now.Month.ToString("d2") + (numberOfApprovedComplaints + 1).ToString().PadLeft(3, '0') + "/" + DateTime.Now.Year.ToString();
                                string strFinancialYear = DateTime.Now.Month < 4 ? DateTime.Now.AddYears(-1).ToString("yyyy") + "-" + DateTime.Now.ToString("yy") : DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.AddYears(1).ToString("yy");
                                DateTime dtFinancialStartDate = DateTime.Now.Month < 4 ? new DateTime(DateTime.Now.Year - 1, 4, 1) : new DateTime(DateTime.Now.Year, 4, 1);
                                Int64 approvedComplaintCount = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.CreatedDate >= dtFinancialStartDate && c.ComplaintLevel.ToLower() == "cgrf" && c.CGRFCaseNumber != null && c.Id != System.Convert.ToInt32(complaintId)).Count();

                                complaint.CGRFCaseNumber = "CGRF" + DateTime.Now.Month.ToString("d2") + (approvedComplaintCount + 1).ToString().PadLeft(3, '0') + "/" + strFinancialYear;
                                complaint.StartDate = DateTime.Now;

                                dataContext.ComplaintPortalCGRFApprovedComplaints.InsertOnSubmit(new ComplaintPortalCGRFApprovedComplaint
                                {
                                    ApprovedDate = DateTime.Now,
                                    CaseNumber = complaint.CGRFCaseNumber,
                                    ComplaintId = System.Convert.ToInt32(complaintId),
                                });
                            }

                            dataContext.SubmitChanges();
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ApproveCGRFComplaint: " + ex.Message, this);
            }
            return false;
        }

        public bool SendSMSandEmailtoConsumer(string mobileNumber, string emailAddress, string accountNumber, string complaintRegistrationNumber, string TATDuration, int actionType, string complaintCaseNumber = "")
        {
            #region SMS
            try
            {
                var apiurl = "";
                switch (actionType)
                {
                    case (int)ComplaintPortalService.ActionType.Submit:
                        apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form submit of icrs complaint",
                               "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Your complaint is registered successfully for Account no. {1}. The complaint can be tracked with Complaint reference no. {2}. Adani Electricity&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid=1707160794894603725"), mobileNumber, accountNumber, complaintRegistrationNumber);
                        break;
                    case (int)ComplaintPortalService.ActionType.Approved:
                        apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form approved of complaint",
                                           "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Your complaint with Reference no. {1} is approved successfully. The complaint Case no. is {2}. Adani Electricity&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid=1707161553330219578"), mobileNumber, complaintRegistrationNumber, complaintCaseNumber);
                        break;
                    case (int)ComplaintPortalService.ActionType.Resubmit:
                        apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form ask to resubmit of complaint",
                                           "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Request to resubmit the grievance of Account No{1}. Resubmission Remark can be tracked with Complaint reference no.{2}. Adani Electricity.&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid=1707161746475829977"), mobileNumber, accountNumber, complaintRegistrationNumber);
                        break;
                    case (int)ComplaintPortalService.ActionType.Resubmission:
                        apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form resubmit of complaint",
                                           "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Your complaint is resubmitted successfully for Account no. {1}. The complaint can be tracked with Complaint reference no. {2}. Adani Electricity&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid=1707161553326008559"), mobileNumber, accountNumber, complaintRegistrationNumber);
                        break;
                    case (int)ComplaintPortalService.ActionType.SubmitCGRF:
                        apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form submit of cgrf complaint",
                               "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Your complaint is registered successfully for Account no. {1}. The complaint can be tracked with Complaint reference no. {2}. Adani Electricity&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid=1707160794894603725"), mobileNumber, accountNumber, complaintRegistrationNumber);
                        break;
                    case (int)ComplaintPortalService.ActionType.HearingScheduled:
                        apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form HearingScheduled of complaint",
                                           "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Hearing is scheduled for Your complaint with reference no. {1}. The complaint details can be checked on the website. Adani Electricity&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid=1707161553353194709"), mobileNumber, complaintCaseNumber);
                        break;
                    //case (int)ComplaintPortalService.ActionType.Closed:
                    //    apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form HearingScheduled of complaint",
                    //                       "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Final Order is issued & complaint is closed. The complaint details can be checked with Complaint reference no. {1}. Adani Electricity&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid=1707161553356449904"), mobileNumber, complaintCaseNumber);
                    //    break;
                    case (int)ComplaintPortalService.ActionType.Closed:
                        apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form HearingScheduled of complaint", "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Final Order is issued & complaint is closed. The complaint details can be checked with Complaint reference no {1}. Adani Electricity&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid=1707162148643941915"), mobileNumber, complaintCaseNumber);
                        break;
                    default:
                        apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form submit of icrs complaint",
                              "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Your complaint is registered successfully for Account no. {1}. The complaint can be tracked with Complaint reference no. {2}. Adani Electricity&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid=1707160794894603725"), mobileNumber, accountNumber, complaintRegistrationNumber);
                        break;

                }

                Log.Info("Complaint portal app submit SMS to user Api call URL: " + apiurl, this);
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(apiurl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                HttpResponseMessage response = client.GetAsync(apiurl).Result;
                if (response.IsSuccessStatusCode)
                {
                    Log.Info("Complaint portal app submit SMS to user Api call success: " + apiurl, this);
                }
                else
                {
                    Log.Info("Complaint portal app submit SMS to user Api call failed: " + apiurl, this);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Complaint portal app submit SMS to user Api call error: " + accountNumber + " " + complaintRegistrationNumber + " error:" + ex.Message, this);
            }
            #endregion
            #region Email
            try
            {
                if (!string.IsNullOrEmpty(emailAddress))
                {
                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                    {
                        var complaintDetails = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == complaintRegistrationNumber).FirstOrDefault();

                        if (actionType == (int)ActionType.HearingScheduled)
                        {
                            var hearingDetails = dataContext.ComplaintPortalCGRFComplaintHearingSchedules.Where(c => c.ComplaintId == complaintDetails.Id).OrderByDescending(c => c.ScheduledDate).FirstOrDefault();

                            if (hearingDetails != null)
                            {
                                ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();
                                MailMessage mail = objComplaintPortalService.GetMailTemplateComplaintCGRFForConsumer(actionType);
                                mail.To.Add(emailAddress);
                                mail.Body = mail.Body.Replace("#registrationNumber#", complaintRegistrationNumber);
                                mail.Body = mail.Body.Replace("#accountNumber#", accountNumber);
                                mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);

                                mail.Body = mail.Body.Replace("#caseNo#", complaintDetails.CGRFCaseNumber);
                                mail.Body = mail.Body.Replace("#complaintCategory#", complaintDetails.ComplaintCategory);
                                mail.Body = mail.Body.Replace("#complaintDescription#", complaintDetails.ComplaintDescription);
                                mail.Body = mail.Body.Replace("#appliedDate#", complaintDetails.CreatedDate.ToString());
                                mail.Body = mail.Body.Replace("#hearingDate#", hearingDetails.DateOfHearing.ToString());

                                Log.Info("SendOTPViaEmail email body:" + mail.Body + ", sent to:" + emailAddress, this);
                                MainUtil.SendMail(mail);
                                return true;
                            }
                        }
                        else
                        {
                            ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();
                            MailMessage mail = objComplaintPortalService.GetMailTemplateComplaintCGRFForConsumer(actionType);
                            mail.To.Add(emailAddress);
                            mail.Body = mail.Body.Replace("#registrationNumber#", complaintRegistrationNumber);
                            mail.Body = mail.Body.Replace("#accountNumber#", accountNumber);
                            mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);

                            mail.Body = mail.Body.Replace("#caseNo#", complaintDetails.CGRFCaseNumber);
                            mail.Body = mail.Body.Replace("#complaintCategory#", complaintDetails.ComplaintCategory);
                            mail.Body = mail.Body.Replace("#complaintDescription#", complaintDetails.ComplaintDescription);
                            mail.Body = mail.Body.Replace("#appliedDate#", complaintDetails.CreatedDate.ToString());

                            Log.Info("SendOTPViaEmail email body:" + mail.Body + ", sent to:" + emailAddress, this);
                            MainUtil.SendMail(mail);
                            return true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Complaint portal app submit Email to user call error for submit to old number: " + accountNumber + " " + complaintRegistrationNumber + " error:" + ex.Message, this);
            }
            #endregion
            return true;
        }

        public bool SendSMSandEmailOnOrderReviewRequest(string accountNumber, string complaintRegistrationNumber, string TATDuration, int actionType)
        {
            #region SMS
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var complaintDetails = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == complaintRegistrationNumber).FirstOrDefault();
                    if (complaintDetails != null)
                    {
                        #region SMS
                        //SMS To consumer 
                        var apiurl = "";
                        apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form submit of icrs complaint",
                              "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Order Review Request is received for the complaint with Reference no.{1} Adani Electricity&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid=1707161596267598449"), complaintDetails.MobileNumber, complaintDetails.CGRFCaseNumber);

                        Log.Info("Complaint portal app submit SMS to user Api call URL: " + apiurl, this);
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Info("Complaint portal app approved SMS to user Api call success: " + apiurl, this);
                        }
                        else
                        {
                            Log.Info("Complaint portal app approved SMS to user Api call failed: " + apiurl, this);
                        }

                        //SMS To Secretary
                        var secretaryMobile = dataContext.ComplaintPortalCGRFAdminLogins.Where(r => r.Role.ToLower() == "secretary").FirstOrDefault().MobileNumber;
                        apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form submit of icrs complaint",
                              "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Order Review Request is received for the complaint with Reference no. {1} Adani Electricity&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid="), secretaryMobile, complaintDetails.CGRFCaseNumber);

                        Log.Info("Complaint portal app approved SMS to Nodal Officer  Api call URL: " + apiurl, this);
                        client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Info("Complaint portal app approved SMS to Nodal Officer  Api call success: " + apiurl, this);
                        }
                        else
                        {
                            Log.Info("Complaint portal app submit SMS to Nodal Officer  Api call failed: " + apiurl, this);
                        }

                        #endregion

                        #region Email
                        try
                        {
                            //To consumer
                            ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();
                            MailMessage mail = objComplaintPortalService.GetMailTemplateComplaintCGRFForConsumer(actionType);
                            mail.To.Add(complaintDetails.Email);
                            mail.Body = mail.Body.Replace("#registrationNumber#", complaintDetails.ComplaintRegistrationNumber);
                            mail.Body = mail.Body.Replace("#accountNumber#", accountNumber);
                            mail.Body = mail.Body.Replace("#caseNo#", complaintDetails.CGRFCaseNumber);
                            mail.Body = mail.Body.Replace("#complaintCategory#", complaintDetails.ComplaintCategory);
                            mail.Body = mail.Body.Replace("#complaintDescription#", complaintDetails.ComplaintDescription);
                            mail.Body = mail.Body.Replace("#appliedDate#", complaintDetails.CreatedDate.ToString());
                            mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);
                            //Log.Info("SendOTPViaEmail email body:" + mail.Body + ", sent to:" + complaintDetails.Email, this);
                            MainUtil.SendMail(mail);

                            //To Secretary
                            var SecretaryEmail = dataContext.ComplaintPortalCGRFAdminLogins.Where(r => r.Role.ToLower() == "secretary").FirstOrDefault().EmailId;
                            mail = objComplaintPortalService.GetMailTemplateComplaintToSecretary(actionType);
                            mail.To.Clear();
                            mail.To.Add(SecretaryEmail);
                            mail.Body = mail.Body.Replace("#registrationNumber#", complaintDetails.ComplaintRegistrationNumber);
                            mail.Body = mail.Body.Replace("#accountNumber#", accountNumber);
                            mail.Body = mail.Body.Replace("#caseNo#", complaintDetails.CGRFCaseNumber);
                            mail.Body = mail.Body.Replace("#complaintCategory#", complaintDetails.ComplaintCategory);
                            mail.Body = mail.Body.Replace("#complaintDescription#", complaintDetails.ComplaintDescription);
                            mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);
                            Log.Info("Send email body nodalOfficerEmail:" + mail.Body + ", sent to:" + SecretaryEmail, this);
                            MainUtil.SendMail(mail);

                            var members = dataContext.ComplaintPortalCGRFAdminLogins.Where(r => r.Role.ToLower() == "member").ToList();
                            mail = objComplaintPortalService.GetMailTemplateComplaintToMembers(actionType);
                            mail.To.Clear();
                            foreach (var m in members)
                            {
                                mail.To.Add(m.EmailId);
                            }
                            mail.Body = mail.Body.Replace("#registrationNumber#", complaintDetails.ComplaintRegistrationNumber);
                            mail.Body = mail.Body.Replace("#accountNumber#", accountNumber);
                            mail.Body = mail.Body.Replace("#caseNo#", complaintDetails.CGRFCaseNumber);
                            mail.Body = mail.Body.Replace("#complaintCategory#", complaintDetails.ComplaintCategory);
                            mail.Body = mail.Body.Replace("#complaintDescription#", complaintDetails.ComplaintDescription);
                            mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);
                            Log.Info("Send email body:" + mail.Body + ", sent to:" + mail.To.FirstOrDefault().ToString(), this);
                            MainUtil.SendMail(mail);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Complaint portal app submit Email to user call error for submit to old number: " + accountNumber + " " + complaintRegistrationNumber + " error:" + ex.Message, this);
                        }
                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error("Complaint portal app submit SMS to user Api call error: " + accountNumber + " " + complaintRegistrationNumber + " error:" + ex.Message, this);
            }
            #endregion

            return true;
        }



        public bool SendSMSandEmailToSecretary(string accountNumber, string complaintRegistrationNumber, string TATDuration, int actionType)
        {
            #region Email
            try
            {
                try
                {
                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                    {
                        var complaintDetails = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == complaintRegistrationNumber).FirstOrDefault();
                        var emailId = dataContext.ComplaintPortalCGRFAdminLogins.Where(r => r.Role.ToLower() == "secretary").FirstOrDefault().EmailId;
                        ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();
                        var mail = objComplaintPortalService.GetMailTemplateComplaintToSecretary(actionType);
                        mail.To.Add(emailId);
                        mail.Body = mail.Body.Replace("#registrationNumber#", complaintRegistrationNumber);
                        mail.Body = mail.Body.Replace("#accountNumber#", accountNumber);
                        mail.Body = mail.Body.Replace("#caseNo#", complaintDetails.CGRFCaseNumber);
                        mail.Body = mail.Body.Replace("#complaintCategory#", complaintDetails.ComplaintCategory);
                        mail.Body = mail.Body.Replace("#complaintDescription#", complaintDetails.ComplaintDescription);
                        mail.Body = mail.Body.Replace("#appliedDate#", complaintDetails.CreatedDate.ToString());

                        var hearingDetails = dataContext.ComplaintPortalCGRFComplaintHearingSchedules.Where(c => c.ComplaintId == complaintDetails.Id).OrderByDescending(c => c.ScheduledDate).FirstOrDefault();
                        if (hearingDetails != null)
                        {
                            mail.Body = mail.Body.Replace("#hearingDate#", hearingDetails.DateOfHearing.ToString());
                        }
                        mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);
                        Log.Info("SendEmailToSecretary email body:" + mail.Body + ", sent to:" + emailId, this);
                        MainUtil.SendMail(mail);
                        return true;
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Unable to send the email to the SendEmailToSecretary" + complaintRegistrationNumber + " - Error - " + ex.Message + "", ex, this);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Complaint portal app submit Email to user call error for submit to old number: " + accountNumber + " " + complaintRegistrationNumber + " error:" + ex.Message, this);
            }
            #endregion
            return true;
        }

        public bool SendSMSandEmailtoNodalOfficer(string accountNumber, string complaintRegistrationNumber, string TATDuration, int actionType)
        {
            #region SMS
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var mobileNumber = dataContext.ComplaintPortalCGRFAdminLogins.Where(r => r.Role.ToLower() == "secretary").FirstOrDefault().MobileNumber;
                    ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();
                    var apiurl = "";
                    switch (actionType)
                    {
                        case (int)ComplaintPortalService.ActionType.Approved:
                            apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form approved of complaint",
                                               "http://push3.maccesssmspush.com/servlet/com.aclwireless.pushconnectivity.listeners.TextListener?userId=relialt&pass=relialt&appid=relialt&subappid=relialt&contenttype=1&to={0}&from=ADANIE&text=Thank you for submitting your request for name change on the bill, for account no. {1}. Reference reg. no. is {2}. Adani Electricity&selfid=true&alert=1&dlrreq=true"), mobileNumber, accountNumber, complaintRegistrationNumber);
                            break;
                        default:
                            apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form submission to complaint",
                                          "http://push3.maccesssmspush.com/servlet/com.aclwireless.pushconnectivity.listeners.TextListener?userId=relialt&pass=relialt&appid=relialt&subappid=relialt&contenttype=1&to={0}&from=ADANIE&text=Thank you for submitting your request for name change on the bill, for account no. {1}. Reference reg. no. is {2}. Adani Electricity&selfid=true&alert=1&dlrreq=true"), mobileNumber, accountNumber, complaintRegistrationNumber);
                            break;

                    }

                    Log.Info("Complaint portal app submit SMS to secretary Api call URL: " + apiurl, this);
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Log.Info("Complaint portal app submit SMS to secretary Api call success: " + apiurl, this);
                    }
                    else
                    {
                        Log.Info("Complaint portal app submit SMS to secretary Api call failed: " + apiurl, this);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Complaint portal app submit SMS to secretary Api call error: " + accountNumber + " " + complaintRegistrationNumber + " error:" + ex.Message, this);
            }
            #endregion
            #region Email
            try
            {
                try
                {
                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                    {
                        var emailId = dataContext.ComplaintPortalCGRFAdminLogins.Where(r => r.Role.ToLower() == "secretary").FirstOrDefault().EmailId;
                        ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();
                        var mail = objComplaintPortalService.GetMailTemplateComplaintToSecretary(actionType);
                        mail.To.Add(emailId);
                        mail.Body = mail.Body.Replace("#registrationNumber#", complaintRegistrationNumber);
                        mail.Body = mail.Body.Replace("#accountNumber#", accountNumber);
                        mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);
                        Log.Info("SendEmailToSecretary email body:" + mail.Body + ", sent to:" + emailId, this);
                        MainUtil.SendMail(mail);
                        return true;
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Unable to send the email to the SendEmailToSecretary" + complaintRegistrationNumber + " - Error - " + ex.Message + "", ex, this);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Complaint portal app submit Email to user call error for submit to old number: " + accountNumber + " " + complaintRegistrationNumber + " error:" + ex.Message, this);
            }
            #endregion
            return true;
        }

        public bool SendSMSandEmailForNodalReply(string complaintid, int actionType)
        {
            var complaintDetails = GetCGRFComplaintDetails(complaintid);
            #region SMS
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {

                    if (complaintDetails != null)
                    {
                        #region SMS
                        //SMS To consumer 
                        var apiurl = "";
                        apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/CON/LEC Registration/Nodal Reply API URL",
                              "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Nodal Reply is submitted for the complaint with Case no. {1} Please submit rejoinder within 1 day. Adani Electricity&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid=1707161553342411901"), complaintDetails.MobileNumber, complaintDetails.CGRFCaseNumber);

                        Log.Info("Complaint portal app submit SMS to user Api call URL: " + apiurl, this);
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Info("Complaint portal app approved SMS to user Api call success: " + apiurl, this);
                        }
                        else
                        {
                            Log.Info("Complaint portal app approved SMS to user Api call failed: " + apiurl, this);
                        }

                        #endregion

                        #region Email
                        try
                        {
                            //To consumer
                            ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();
                            MailMessage mail = objComplaintPortalService.GetMailTemplateComplaintCGRFForConsumer(actionType);
                            mail.To.Add(complaintDetails.EmailId);
                            mail.Body = mail.Body.Replace("#registrationNumber#", complaintDetails.ComplaintRegistrationNumber);
                            mail.Body = mail.Body.Replace("#accountNumber#", complaintDetails.AccountNumber);
                            mail.Body = mail.Body.Replace("#caseNo#", complaintDetails.CGRFCaseNumber);
                            mail.Body = mail.Body.Replace("#complaintCategory#", complaintDetails.ComplaintCategory);
                            mail.Body = mail.Body.Replace("#complaintDescription#", complaintDetails.ComplaintDescription);
                            mail.Body = mail.Body.Replace("#appliedDate#", complaintDetails.AppliedDate.ToString());

                            //mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);
                            Log.Info("SendOTPViaEmail email body:" + mail.Body + ", sent to:" + complaintDetails.EmailId, this);
                            MainUtil.SendMail(mail);

                            //To Secretary
                            var SecretaryEmail = dataContext.ComplaintPortalCGRFAdminLogins.Where(r => r.Role.ToLower() == "secretary").FirstOrDefault().EmailId;
                            mail = objComplaintPortalService.GetMailTemplateComplaintToSecretary(actionType);
                            mail.To.Clear();
                            mail.To.Add(SecretaryEmail);
                            mail.Body = mail.Body.Replace("#registrationNumber#", complaintDetails.ComplaintRegistrationNumber);
                            mail.Body = mail.Body.Replace("#accountNumber#", complaintDetails.AccountNumber);
                            mail.Body = mail.Body.Replace("#caseNo#", complaintDetails.CGRFCaseNumber);
                            mail.Body = mail.Body.Replace("#complaintCategory#", complaintDetails.ComplaintCategory);
                            mail.Body = mail.Body.Replace("#complaintDescription#", complaintDetails.ComplaintDescription);
                            mail.Body = mail.Body.Replace("#appliedDate#", complaintDetails.AppliedDate.ToString());
                            //mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);
                            Log.Info("Send email body nodalOfficerEmail:" + mail.Body + ", sent to:" + SecretaryEmail, this);
                            MainUtil.SendMail(mail);

                            //To Members
                            var members = dataContext.ComplaintPortalCGRFAdminLogins.Where(r => r.Role.ToLower() == "member").ToList();
                            mail = objComplaintPortalService.GetMailTemplateComplaintToSecretary(actionType);
                            mail.To.Clear();
                            foreach (var m in members)
                            {
                                mail.To.Add(m.EmailId);
                            }
                            mail.Body = mail.Body.Replace("#registrationNumber#", complaintDetails.ComplaintRegistrationNumber);
                            mail.Body = mail.Body.Replace("#accountNumber#", complaintDetails.AccountNumber);
                            mail.Body = mail.Body.Replace("#caseNo#", complaintDetails.CGRFCaseNumber);
                            mail.Body = mail.Body.Replace("#complaintCategory#", complaintDetails.ComplaintCategory);
                            mail.Body = mail.Body.Replace("#complaintDescription#", complaintDetails.ComplaintDescription);
                            mail.Body = mail.Body.Replace("#appliedDate#", complaintDetails.AppliedDate.ToString());
                            //mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);
                            Log.Info("Send email body:" + mail.Body + ", sent to:" + mail.To.FirstOrDefault().ToString(), this);
                            MainUtil.SendMail(mail);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Complaint portal app submit Email to user call error for submit to old number: " + complaintDetails.AccountNumber + " " + complaintDetails.ComplaintRegistrationNumber + " error:" + ex.Message, this);
                        }
                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error("Complaint portal app submit SMS to user Api call error: " + complaintDetails.AccountNumber + " " + complaintDetails.ComplaintRegistrationNumber + " error:" + ex.Message, this);
            }
            #endregion

            return true;
        }

        public bool SendSMSandEmailForApproval(string accountNumber, string complaintRegistrationNumber, string TATDuration, int actionType)
        {

            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var complaintDetails = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == complaintRegistrationNumber).FirstOrDefault();
                    if (complaintDetails != null)
                    {
                        #region SMS
                        //SMS To consumer 
                        var apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form approved of complaint",
                                           "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=Your complaint with Reference no. {1} is approved successfully. The complaint Case no. is {2}. Adani Electricity&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid=1707161553330219578"), complaintDetails.MobileNumber, complaintRegistrationNumber, complaintDetails.CGRFCaseNumber);

                        Log.Info("Complaint portal app submit SMS to user Api call URL: " + apiurl, this);
                        HttpClient client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        HttpResponseMessage response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Info("Complaint portal app approved SMS to user Api call success: " + apiurl, this);
                        }
                        else
                        {
                            Log.Info("Complaint portal app approved SMS to user Api call failed: " + apiurl, this);
                        }

                        //SMS To Nodal Officer 
                        var nodalOfficerMobile = dataContext.ComplaintPortalCGRFAdminLogins.Where(r => r.Role.ToLower() == "nodaladmin").FirstOrDefault().MobileNumber;
                        apiurl = string.Format(DictionaryPhraseRepository.Current.Get("/ComplaintPortal/SMS API form approved of complaint",
                                           "https://hapibk.connectbox.in:8181/SendSMS.aspx?UserName=Web_OTP&password=SM78ysAu&MobileNo={0}&SenderID=ADANIE&CDMAHeader=ADANIE&Message=A complaint with Reference no. {1} is approved successfully. The complaint Case no. is {2}. Adani Electricity&dlt_peid=1707161553318060925&dlt_tmid=120210000043&dlt_templateid=1707161553342411901"), complaintDetails.MobileNumber, complaintRegistrationNumber, complaintDetails.CGRFCaseNumber);

                        Log.Info("Complaint portal app approved SMS to Nodal Officer  Api call URL: " + apiurl, this);
                        client = new HttpClient();
                        client.BaseAddress = new Uri(apiurl);
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                        response = client.GetAsync(apiurl).Result;
                        if (response.IsSuccessStatusCode)
                        {
                            Log.Info("Complaint portal app approved SMS to Nodal Officer  Api call success: " + apiurl, this);
                        }
                        else
                        {
                            Log.Info("Complaint portal app submit SMS to Nodal Officer  Api call failed: " + apiurl, this);
                        }

                        #endregion

                        #region Email
                        try
                        {
                            ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();
                            MailMessage mail;
                            //To consumer
                            if (!string.IsNullOrEmpty(complaintDetails.Email))
                            {
                                mail = objComplaintPortalService.GetMailTemplateComplaintCGRFForConsumer(actionType);
                                mail.To.Add(complaintDetails.Email);
                                mail.Body = mail.Body.Replace("#registrationNumber#", complaintRegistrationNumber);
                                mail.Body = mail.Body.Replace("#accountNumber#", accountNumber);
                                mail.Body = mail.Body.Replace("#caseNo#", complaintDetails.CGRFCaseNumber);
                                mail.Body = mail.Body.Replace("#complaintCategory#", complaintDetails.ComplaintCategory);
                                mail.Body = mail.Body.Replace("#complaintDescription#", complaintDetails.ComplaintDescription);
                                mail.Body = mail.Body.Replace("#appliedDate#", complaintDetails.CreatedDate.ToString());

                                mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);
                                Log.Info("SendOTPViaEmail email body:" + mail.Body + ", sent to:" + complaintDetails.Email, this);
                                MainUtil.SendMail(mail);
                            }

                            //To Nodal Officer
                            var nodalOfficerEmail = dataContext.ComplaintPortalCGRFAdminLogins.Where(r => r.Role.ToLower() == "nodaladmin").FirstOrDefault().EmailId;
                            mail = objComplaintPortalService.GetMailTemplateComplaintToNodalOfficer(actionType);
                            mail.To.Clear();
                            mail.To.Add(nodalOfficerEmail);
                            mail.Body = mail.Body.Replace("#registrationNumber#", complaintRegistrationNumber);
                            mail.Body = mail.Body.Replace("#accountNumber#", accountNumber);
                            mail.Body = mail.Body.Replace("#caseNo#", complaintDetails.CGRFCaseNumber);
                            mail.Body = mail.Body.Replace("#complaintCategory#", complaintDetails.ComplaintCategory);
                            mail.Body = mail.Body.Replace("#complaintDescription#", complaintDetails.ComplaintDescription);
                            mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);
                            Log.Info("Send email body nodalOfficerEmail:" + mail.Body + ", sent to:" + nodalOfficerEmail, this);
                            MainUtil.SendMail(mail);

                            //To Members
                            var members = dataContext.ComplaintPortalCGRFAdminLogins.Where(r => r.Role.ToLower() == "member").ToList();
                            mail = objComplaintPortalService.GetMailTemplateComplaintToMembers(actionType);
                            mail.To.Clear();
                            foreach (var m in members)
                            {
                                mail.To.Add(m.EmailId);
                            }
                            mail.Body = mail.Body.Replace("#registrationNumber#", complaintRegistrationNumber);
                            mail.Body = mail.Body.Replace("#accountNumber#", accountNumber);
                            mail.Body = mail.Body.Replace("#caseNo#", complaintDetails.CGRFCaseNumber);
                            mail.Body = mail.Body.Replace("#complaintCategory#", complaintDetails.ComplaintCategory);
                            mail.Body = mail.Body.Replace("#complaintDescription#", complaintDetails.ComplaintDescription);
                            mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);
                            Log.Info("Send email body:" + mail.Body + ", sent to:" + mail.To.FirstOrDefault().ToString(), this);
                            MainUtil.SendMail(mail);
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Complaint portal app submit Email to user call error for submit to old number: " + accountNumber + " " + complaintRegistrationNumber + " error:" + ex.Message, this);
                        }
                        #endregion
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error("Complaint portal app submit SMS to user Api call error: " + accountNumber + " " + complaintRegistrationNumber + " error:" + ex.Message, this);
            }

            return true;
        }

        public bool SendEmailToMembers(string accountNumber, string complaintRegistrationNumber, string TATDuration, int actionType)
        {
            #region Email
            try
            {
                try
                {
                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                    {
                        var complaintDetails = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == complaintRegistrationNumber).FirstOrDefault();
                        ComplaintPortalService objComplaintPortalService = new ComplaintPortalService();
                        var mail = objComplaintPortalService.GetMailTemplateComplaintToMembers(actionType);
                        var members = dataContext.ComplaintPortalCGRFAdminLogins.Where(r => r.Role.ToLower() == "member").ToList();
                        mail = objComplaintPortalService.GetMailTemplateComplaintToSecretary(actionType);
                        mail.To.Clear();
                        foreach (var m in members)
                        {
                            mail.To.Add(m.EmailId);
                        }
                        mail.Body = mail.Body.Replace("#registrationNumber#", complaintRegistrationNumber);
                        mail.Body = mail.Body.Replace("#accountNumber#", accountNumber);
                        mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);
                        mail.Body = mail.Body.Replace("#registrationNumber#", complaintRegistrationNumber);
                        mail.Body = mail.Body.Replace("#accountNumber#", accountNumber);
                        mail.Body = mail.Body.Replace("#caseNo#", complaintRegistrationNumber);
                        mail.Body = mail.Body.Replace("#complaintCategory#", complaintDetails.ComplaintCategory);
                        mail.Body = mail.Body.Replace("#complaintDescription#", complaintDetails.ComplaintDescription);
                        mail.Body = mail.Body.Replace("#appliedDate#", complaintDetails.CreatedDate.ToString());
                        mail.Body = mail.Body.Replace("#TATDuration#", TATDuration);

                        var hearingDetails = dataContext.ComplaintPortalCGRFComplaintHearingSchedules.Where(c => c.ComplaintId == complaintDetails.Id).OrderByDescending(c => c.ScheduledDate).FirstOrDefault();
                        if (hearingDetails != null)
                        {
                            mail.Body = mail.Body.Replace("#hearingDate#", hearingDetails.DateOfHearing.ToString());
                        }
                        //Log.Info("SendEmailToMembers email body:" + mail.Body + ", sent to:" + emailId, this);
                        MainUtil.SendMail(mail);
                        return true;
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Unable to send the email to the SendEmailToSecretary" + complaintRegistrationNumber + " - Error - " + ex.Message + "", ex, this);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Complaint portal app submit Email to user call error for submit to old number: " + accountNumber + " " + complaintRegistrationNumber + " error:" + ex.Message, this);
            }
            #endregion
            return true;
        }

        public long GetCGRFCount()
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CGRFComplaintsCounters.Any())
                    {
                        CGRFComplaintsCounter counter = dataContext.CGRFComplaintsCounters.Where(c => c.Id == 1).FirstOrDefault();
                        int count = counter.CGRFComplaintsCountValue + 1;
                        counter.CGRFComplaintsCountValue = count;
                        dataContext.SubmitChanges();
                        return count;
                    }
                }
            }
            catch
            {

            }
            return 0;
        }
        public long GetCurrentFinancialYearCGRFCount()
        {
            try
            {
                string strFinancialYear = DateTime.Now.Month < 4 ? DateTime.Now.AddYears(-1).ToString("yyyy") + "-" + DateTime.Now.ToString("yy") : DateTime.Now.ToString("yyyy") + "-" + DateTime.Now.AddYears(1).ToString("yy");
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CGRFComplaintsCounters.Any())
                    {
                        CGRFComplaintsCounter counter = dataContext.CGRFComplaintsCounters.Where(c => c.FinancialYear == strFinancialYear).FirstOrDefault();
                        if (counter != null)
                        {
                            int count = counter.CGRFComplaintsCountValue + 1;
                            counter.CGRFComplaintsCountValue = count;
                            dataContext.SubmitChanges();
                            return count;
                        }
                        else
                        {
                            int count = 1;
                            CGRFComplaintsCounter obj = new CGRFComplaintsCounter()
                            {
                                FinancialYear = strFinancialYear,
                                CGRFComplaintsCountValue = count
                            };
                            dataContext.CGRFComplaintsCounters.InsertOnSubmit(obj);
                            dataContext.SubmitChanges();
                            return count;
                        }
                    }
                    else
                    {
                        int count = 1;
                        CGRFComplaintsCounter obj = new CGRFComplaintsCounter()
                        {
                            FinancialYear = strFinancialYear,
                            CGRFComplaintsCountValue = count
                        };
                        dataContext.CGRFComplaintsCounters.InsertOnSubmit(obj);
                        dataContext.SubmitChanges();
                        return count;
                    }
                }
            }
            catch
            {

            }
            return 0;
        }



        public string SaveCGRFComplaintApplication(CGRFComplaintFileRegistrationModel model, HttpPostedFileBase scheduleAFormfile, IList<HttpPostedFileBase> supportingDocumentsfiles, bool isSubmit)
        {
            try
            {
                var userDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNumber);
                var userDetails2 = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.AccountNumber);
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (model.ComplaintId == null)
                    {
                        //create new complaint
                        ComplaintPortalRegisteredComplaint obj = new ComplaintPortalRegisteredComplaint
                        {
                            AccountNumber = model.AccountNumber,
                            Address = userDetails.Street + userDetails.Street2 + userDetails.Street3,
                            City = userDetails.City,
                            ComplaintCategory = model.SelectedComplaintCategory,
                            ComplaintDescription = model.ComplaintDescription,
                            IsRegisteredUser = UserSession.AEMLComplaintUserSessionContext.IsRegistered,
                            ComplaintLevel = "CGRF",
                            ComplaintStatusCode = isSubmit ? ((int)ComplaintStatus.Submitted).ToString() : ((int)ComplaintStatus.Saved).ToString(),
                            ComplaintStatusName = isSubmit ? ComplaintStatus.Submitted.ToString() : ComplaintStatus.Saved.ToString(),
                            ConsumerCategory = userDetails.ConnectionType,
                            ConsumerName = userDetails.Name,
                            CreatedDate = DateTime.Now,
                            Email = userDetails.Email,
                            Mobile = userDetails.Mobile,
                            NumberOfVersion = 1,
                            Pincode = userDetails.PinCode,
                            UserName = model.LoginName,
                            Zone = userDetails2.ZoneName,
                            ComplaintFromPerviousLevelDivision = userDetails2.DivisionName,
                            MobileNumber = userDetails.Mobile,
                            ComplaintCategoryOtherText = model.OtherCategoryText,
                            CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName
                        };


                        if (model.ComplaintFromPreviousLevel != null)
                        {
                            //DateTime ComplaintFromPreviousLevelAppliedDate = (DateTime.ParseExact(model.ComplaintFromPreviousLevelAppliedDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));

                            string reason = model.ReasonToApply + (string.IsNullOrEmpty(model.ReasonToApplySubType) ? string.Empty : ", " + model.ReasonToApplySubType) + (string.IsNullOrEmpty(model.ReasonToApplyOtherText) ? string.Empty : ", " + model.ReasonToApplyOtherText);

                            obj.ComplaintNumberFromPerviousLevel = model.ComplaintFromPreviousLevel;
                            obj.ComplaintRemarksPerviousLevel = dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.ComplaintRegistrationNumber == model.ComplaintFromPreviousLevel) ? dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == model.ComplaintFromPreviousLevel).FirstOrDefault().AdminRemarks : null;
                            obj.ReasonToApplyComplaint = reason;
                            obj.ComplaintFromPerviousLevelAppliedDate = dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.ComplaintRegistrationNumber == model.ComplaintFromPreviousLevel) ? dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.ComplaintRegistrationNumber == model.ComplaintFromPreviousLevel).FirstOrDefault().CreatedDate.ToString() : model.ComplaintFromPreviousLevelAppliedDate;
                        }

                        if (scheduleAFormfile != null && scheduleAFormfile.ContentLength > 0)
                        {
                            Stream fs = scheduleAFormfile.InputStream;
                            BinaryReader br = new BinaryReader(fs);
                            byte[] bytes = br.ReadBytes((Int32)fs.Length);

                            obj.DocumentData = bytes;
                            obj.DocumentContentType = scheduleAFormfile.ContentType;
                            obj.DocumentFileName = GetFileName(scheduleAFormfile.FileName);
                        }

                        dataContext.ComplaintPortalRegisteredComplaints.InsertOnSubmit(obj);
                        dataContext.SubmitChanges();

                        var app = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == obj.Id).FirstOrDefault();
                        //app.ComplaintRegistrationNumber = "CGRF" + DateTime.Now.Month.ToString() + app.Id.ToString() + DateTime.Now.Year.ToString();
                        var count = GetCurrentFinancialYearCGRFCount();
                        if (count == 0)
                            count = app.Id;

                        string num = "TRACK" + count.ToString().PadLeft(5, '0') + "/" + DateTime.Now.Date.ToString("ddMMyyyy");
                        app.ComplaintRegistrationNumber = num;

                        //insert supporting documents
                        foreach (var sdFile in supportingDocumentsfiles)
                        {
                            if (sdFile != null && sdFile.ContentLength > 0)
                            {
                                Stream fs = sdFile.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                ComplaintPortalCGRFComplaintDocument objdoc = new ComplaintPortalCGRFComplaintDocument
                                {
                                    ComplaintNumber = obj.Id,
                                    CreatedDate = DateTime.Now,
                                    DocumentContentType = sdFile.ContentType,
                                    DocumentData = bytes,
                                    DocumentName = GetFileName(sdFile.FileName),
                                    DocumentType = "SD",
                                    DocumenttypeCode = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "SD").FirstOrDefault().Id,
                                    DocumentDescription = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "SD").FirstOrDefault().Decription
                                };
                                dataContext.ComplaintPortalCGRFComplaintDocuments.InsertOnSubmit(objdoc);
                            }
                        };

                        dataContext.ComplaintPortalCGRFComplaintHistories.InsertOnSubmit(new ComplaintPortalCGRFComplaintHistory
                        {
                            AdminRemarks = model.AdminRemarks,
                            ComplaintId = obj.Id,
                            Status = isSubmit ? "2" : "1",
                            CreatedDate = DateTime.Now,
                            CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                            Description = isSubmit ? "Application is Submitted" : "Application is Saved"
                        });
                        dataContext.SubmitChanges();

                        return app.ComplaintRegistrationNumber;
                    }
                    else
                    {
                        long ComplaintId = System.Convert.ToInt64(model.ComplaintId);
                        if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.Id == ComplaintId && c.ComplaintStatusName == "Saved"))
                        {
                            ComplaintPortalRegisteredComplaint existingComplaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == ComplaintId).FirstOrDefault();

                            existingComplaint.ComplaintCategory = model.SelectedComplaintCategory;

                            existingComplaint.ComplaintDescription = model.ComplaintDescription;

                            existingComplaint.ModifiedDate = DateTime.Now;
                            existingComplaint.ModifiedBy = UserSession.AEMLComplaintUserSessionContext.LoginName;

                            existingComplaint.NumberOfVersion = existingComplaint.NumberOfVersion + 1;
                            existingComplaint.ComplaintStatusCode = isSubmit ? ((int)ComplaintStatus.Submitted).ToString() : ((int)ComplaintStatus.Saved).ToString();
                            existingComplaint.ComplaintStatusName = isSubmit ? ComplaintStatus.Submitted.ToString() : ComplaintStatus.Saved.ToString();
                            existingComplaint.ComplaintCategoryOtherText = model.OtherCategoryText;
                            if (scheduleAFormfile != null && scheduleAFormfile.ContentLength > 0)
                            {
                                Stream fs = scheduleAFormfile.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                existingComplaint.DocumentData = bytes;
                                existingComplaint.DocumentContentType = scheduleAFormfile.ContentType;
                                existingComplaint.DocumentFileName = GetFileName(scheduleAFormfile.FileName);
                            }

                            foreach (var sdFile in supportingDocumentsfiles)
                            {
                                if (sdFile != null && sdFile.ContentLength > 0)
                                {
                                    Stream fs = sdFile.InputStream;
                                    BinaryReader br = new BinaryReader(fs);
                                    byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                    ComplaintPortalCGRFComplaintDocument objdoc = new ComplaintPortalCGRFComplaintDocument
                                    {
                                        ComplaintNumber = ComplaintId,
                                        CreatedDate = DateTime.Now,
                                        DocumentContentType = sdFile.ContentType,
                                        DocumentData = bytes,
                                        DocumentName = GetFileName(sdFile.FileName),
                                        DocumentType = "SD",
                                        DocumenttypeCode = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "SD").FirstOrDefault().Id,
                                        DocumentDescription = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "SD").FirstOrDefault().Decription
                                    };
                                    dataContext.ComplaintPortalCGRFComplaintDocuments.InsertOnSubmit(objdoc);
                                }
                            };

                            dataContext.ComplaintPortalCGRFComplaintHistories.InsertOnSubmit(new ComplaintPortalCGRFComplaintHistory
                            {
                                AdminRemarks = model.AdminRemarks,
                                ComplaintId = System.Convert.ToInt64(model.ComplaintId),
                                Status = isSubmit ? "2" : "1",
                                CreatedDate = DateTime.Now,
                                CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                                Description = isSubmit ? "Application is Submitted" : "Application is Saved"
                            });
                            dataContext.SubmitChanges();
                            return existingComplaint.ComplaintRegistrationNumber;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at Complaint Portal CreateLoginLog: " + ex.Message + " For Account number:" + model.AccountNumber, this);
            }
            return string.Empty;
        }

        public string ResubmitCGRFComplaintApplication(string complaintId, HttpPostedFileBase scheduleAFormfile)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    string accountNumber = FormatAccountNumber(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                    ComplaintPortalRegisteredComplaint record;
                    if (UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                    {
                        record = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id.ToString() == complaintId && c.ComplaintStatusCode == ((int)ComplaintStatus.Resubmit).ToString()).FirstOrDefault();
                    }
                    else
                    {
                        record = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id.ToString() == complaintId && c.ComplaintStatusCode == ((int)ComplaintStatus.Resubmit).ToString() && c.AccountNumber == accountNumber).FirstOrDefault();
                    }
                    if (record != null)
                    {
                        if (scheduleAFormfile != null && scheduleAFormfile.ContentLength > 0)
                        {
                            Stream fs = scheduleAFormfile.InputStream;
                            BinaryReader br = new BinaryReader(fs);
                            byte[] bytes = br.ReadBytes((Int32)fs.Length);

                            ComplaintPortalCGRFComplaintDocument objdoc = new ComplaintPortalCGRFComplaintDocument
                            {
                                ComplaintNumber = record.Id,
                                CreatedDate = DateTime.Now,
                                DocumentContentType = scheduleAFormfile.ContentType,
                                DocumentData = bytes,
                                DocumentName = GetFileName(scheduleAFormfile.FileName),
                                DocumentType = "RD",
                                DocumenttypeCode = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "RD").FirstOrDefault().Id,
                                DocumentDescription = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "RD").FirstOrDefault().Decription
                            };
                            dataContext.ComplaintPortalCGRFComplaintDocuments.InsertOnSubmit(objdoc);
                        }
                        record.ComplaintStatusCode = ((int)ComplaintStatus.Submitted).ToString();
                        record.ComplaintStatusName = ComplaintStatus.Submitted.ToString();

                        dataContext.ComplaintPortalCGRFComplaintHistories.InsertOnSubmit(new ComplaintPortalCGRFComplaintHistory
                        {
                            AdminRemarks = "Application Grievance is resubmitted by consumer.",
                            ComplaintId = System.Convert.ToInt64(complaintId),
                            CreatedDate = DateTime.Now,
                            Status = ((int)ComplaintStatus.Submitted).ToString(),
                            CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                            Description = "Application is Resubmitted."
                        });

                        dataContext.SubmitChanges();

                        return record.ComplaintRegistrationNumber;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("ResubmitCGRFComplaintApplication for " + complaintId + e.Message, this);
                return null;
            }
            return null;
        }

        public string ZonelAdminReplyComplaintApplication(string complaintId, IList<HttpPostedFileBase> fileComplaintDocNodelAdmin, string remarks)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                    {
                        var record = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id.ToString() == complaintId && c.ComplaintStatusCode == ((int)ComplaintStatus.Approved).ToString()).FirstOrDefault();

                        if (record != null)
                        {
                            foreach (var sdFile in fileComplaintDocNodelAdmin)
                            {
                                if (sdFile != null && sdFile.ContentLength > 0)
                                {
                                    Stream fs = sdFile.InputStream;
                                    BinaryReader br = new BinaryReader(fs);
                                    byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                    ComplaintPortalCGRFComplaintDocument objdoc = new ComplaintPortalCGRFComplaintDocument
                                    {
                                        ComplaintNumber = record.Id,
                                        CreatedDate = DateTime.Now,
                                        DocumentContentType = sdFile.ContentType,
                                        DocumentData = bytes,
                                        DocumentName = GetFileName(sdFile.FileName),
                                        DocumentType = "ND",
                                        DocumenttypeCode = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "ND").FirstOrDefault().Id,
                                        DocumentDescription = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "ND").FirstOrDefault().Decription
                                    };
                                    dataContext.ComplaintPortalCGRFComplaintDocuments.InsertOnSubmit(objdoc);
                                }
                            };

                            dataContext.ComplaintPortalCGRFComplaintHistories.InsertOnSubmit(new ComplaintPortalCGRFComplaintHistory
                            {
                                AdminRemarks = remarks,
                                ComplaintId = record.Id,
                                Status = ((int)ComplaintPortalService.ComplaintStatus.NodalReply).ToString(),
                                CreatedDate = DateTime.Now,
                                CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                                Description = "Nodal Reply Submitted"
                            });

                            record.ComplaintStatusCode = ((int)ComplaintStatus.NodalReply).ToString();
                            record.ComplaintStatusName = ComplaintStatus.NodalReply.ToString();


                            dataContext.SubmitChanges();

                            return record.ComplaintRegistrationNumber;
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("ResubmitCGRFComplaintApplication for " + complaintId + e.Message, this);
                return null;
            }
            return null;
        }

        public bool CloseCGRFComplaintApplication(string complaintId, string adminRemarks, HttpPostedFileBase fileDocFile)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (complaintId != null && UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                    {
                        long existingComplaintId = System.Convert.ToInt64(complaintId);

                        if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.Id == existingComplaintId))
                        {
                            ComplaintPortalRegisteredComplaint existingComplaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == existingComplaintId).FirstOrDefault();

                            if (fileDocFile != null)
                            {
                                Stream fs = fileDocFile.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                byte[] bytes = br.ReadBytes((Int32)fs.Length);
                                bool isReviewRequest = existingComplaint.ComplaintStatusCode == ((int)ComplaintPortalService.ComplaintStatus.Review).ToString() ? true : false;
                                ComplaintPortalCGRFComplaintDocument objdoc = new ComplaintPortalCGRFComplaintDocument
                                {
                                    ComplaintNumber = existingComplaintId,
                                    CreatedDate = DateTime.Now,
                                    DocumentContentType = fileDocFile.ContentType,
                                    DocumentData = bytes,
                                    DocumentName = GetFileName(fileDocFile.FileName),
                                    DocumentType = "CD",
                                    DocumenttypeCode = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "CD").FirstOrDefault().Id,
                                    DocumentDescription = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "CD").FirstOrDefault().Decription,
                                    IsReviewRequestRaised = isReviewRequest
                                };
                                dataContext.ComplaintPortalCGRFComplaintDocuments.InsertOnSubmit(objdoc);
                            }

                            existingComplaint.ComplaintStatusCode = ((int)ComplaintStatus.Closed).ToString();
                            existingComplaint.ComplaintStatusName = ComplaintStatus.Closed.ToString();
                            existingComplaint.ClosedDate = DateTime.Now;

                            dataContext.ComplaintPortalCGRFComplaintHistories.InsertOnSubmit(new ComplaintPortalCGRFComplaintHistory
                            {
                                AdminRemarks = adminRemarks,
                                ComplaintId = existingComplaintId,
                                Status = ((int)ComplaintPortalService.ComplaintStatus.Closed).ToString(),
                                CreatedDate = DateTime.Now,
                                CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                                Description = "Complaint is closed by Admin"
                            });
                            dataContext.SubmitChanges();
                            return true;
                        }
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at Complaint Portal CreateLoginLog: " + ex.Message + " For Account number:" + complaintId, this);
            }
            return false;
        }

        public bool ClosingFinalDocumentUpload(string complaintId, HttpPostedFileBase finalDocumentfile)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (complaintId != null && UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                    {
                        long existingComplaintId = System.Convert.ToInt64(complaintId);

                        if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.Id == existingComplaintId))
                        {
                            ComplaintPortalRegisteredComplaint existingComplaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == existingComplaintId).FirstOrDefault();

                            if (dataContext.ComplaintPortalCGRFComplaintDocuments.Any(c => c.ComplaintNumber == existingComplaintId && c.DocumentType == "CD"))
                            {
                                List<ComplaintPortalCGRFComplaintDocument> existngdocs = dataContext.ComplaintPortalCGRFComplaintDocuments.Where(c => c.ComplaintNumber == existingComplaintId && c.DocumentType == "CD").ToList();
                                foreach (var doc in existngdocs)
                                {
                                    dataContext.ComplaintPortalCGRFComplaintDocuments.DeleteOnSubmit(doc);
                                }
                            }

                            if (finalDocumentfile != null && finalDocumentfile.ContentLength > 0)
                            {
                                Stream fs = finalDocumentfile.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                ComplaintPortalCGRFComplaintDocument objdoc = new ComplaintPortalCGRFComplaintDocument
                                {
                                    ComplaintNumber = existingComplaintId,
                                    CreatedDate = DateTime.Now,
                                    DocumentContentType = finalDocumentfile.ContentType,
                                    DocumentData = bytes,
                                    DocumentName = GetFileName(finalDocumentfile.FileName),
                                    DocumentType = "CD",
                                    DocumenttypeCode = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "CD").FirstOrDefault().Id,
                                    DocumentDescription = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "CD").FirstOrDefault().Decription
                                };
                                dataContext.ComplaintPortalCGRFComplaintDocuments.InsertOnSubmit(objdoc);
                            }

                            dataContext.ComplaintPortalCGRFComplaintHistories.InsertOnSubmit(new ComplaintPortalCGRFComplaintHistory
                            {
                                AdminRemarks = "Final document Reuploaded",
                                ComplaintId = existingComplaintId,
                                CreatedDate = DateTime.Now,
                                CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                                Description = "Final document Reuploaded"
                            });
                            dataContext.SubmitChanges();
                            return true;
                        }
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at Complaint Portal ClosingFinalDocumentUpload: " + ex.Message + " For Account number:" + complaintId, this);
            }
            return false;
        }

        public bool UpdateCGRFComplaintApplication(CGRFComplaintFileRegistrationModel model, HttpPostedFileBase scheduleAFormfile, IList<HttpPostedFileBase> supportingDocumentsfiles)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (model.ComplaintId != null)
                    {
                        long ComplaintId = System.Convert.ToInt64(model.ComplaintId);
                        if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.Id == ComplaintId))
                        {
                            ComplaintPortalRegisteredComplaint existingComplaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == ComplaintId).FirstOrDefault();

                            existingComplaint.ComplaintCategory = model.SelectedComplaintCategory;

                            existingComplaint.ComplaintDescription = model.ComplaintDescription;

                            existingComplaint.ModifiedDate = DateTime.Now;
                            existingComplaint.ModifiedBy = UserSession.AEMLComplaintUserSessionContext.LoginName;
                            existingComplaint.Zone = model.SelectedConsumerZone;
                            existingComplaint.NumberOfVersion = existingComplaint.NumberOfVersion + 1;

                            existingComplaint.ComplaintCategoryOtherText = model.OtherCategoryText;
                            if (scheduleAFormfile != null && scheduleAFormfile.ContentLength > 0)
                            {
                                Stream fs = scheduleAFormfile.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                existingComplaint.DocumentData = bytes;
                                existingComplaint.DocumentContentType = scheduleAFormfile.ContentType;
                                existingComplaint.DocumentFileName = GetFileName(scheduleAFormfile.FileName);
                            }

                            foreach (var sdFile in supportingDocumentsfiles)
                            {
                                if (sdFile != null && sdFile.ContentLength > 0)
                                {
                                    Stream fs = sdFile.InputStream;
                                    BinaryReader br = new BinaryReader(fs);
                                    byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                    ComplaintPortalCGRFComplaintDocument objdoc = new ComplaintPortalCGRFComplaintDocument
                                    {
                                        ComplaintNumber = ComplaintId,
                                        CreatedDate = DateTime.Now,
                                        DocumentContentType = sdFile.ContentType,
                                        DocumentData = bytes,
                                        DocumentName = GetFileName(sdFile.FileName),
                                        DocumentType = "SD",
                                        DocumenttypeCode = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "SD").FirstOrDefault().Id,
                                        DocumentDescription = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "SD").FirstOrDefault().Decription
                                    };
                                    dataContext.ComplaintPortalCGRFComplaintDocuments.InsertOnSubmit(objdoc);
                                }
                            };

                            dataContext.ComplaintPortalCGRFComplaintHistories.InsertOnSubmit(new ComplaintPortalCGRFComplaintHistory
                            {
                                AdminRemarks = model.AdminRemarks,
                                ComplaintId = System.Convert.ToInt64(model.ComplaintId),
                                CreatedDate = DateTime.Now,
                                CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                                Description = "Application is Updated by Admin"
                            });
                            dataContext.SubmitChanges();
                            return true;
                        }
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at Complaint Portal CreateLoginLog: " + ex.Message + " For Account number:" + model.AccountNumber, this);
            }
            return false;
        }

        public string OrderReviewRequestForComplaint(CGRFComplaintFileRegistrationModel model, IList<HttpPostedFileBase> orderReviewRequestFiles)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    ComplaintPortalRegisteredComplaint record;
                    if (UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                    {
                        record = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id.ToString() == model.ComplaintId && c.ComplaintStatusCode == ((int)ComplaintStatus.Closed).ToString()).FirstOrDefault();
                    }
                    else
                    {
                        string accountNumber = FormatAccountNumber(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                        record = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id.ToString() == model.ComplaintId && c.ComplaintStatusCode == ((int)ComplaintStatus.Closed).ToString() && c.AccountNumber == accountNumber).FirstOrDefault();
                    }

                    if (record != null)
                    {
                        foreach (var sdFile in orderReviewRequestFiles)
                        {
                            if (sdFile != null && sdFile.ContentLength > 0)
                            {
                                Stream fs = sdFile.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                byte[] bytes = br.ReadBytes((Int32)fs.Length);


                                ComplaintPortalCGRFComplaintDocument objdoc = new ComplaintPortalCGRFComplaintDocument
                                {
                                    ComplaintNumber = record.Id,
                                    CreatedDate = DateTime.Now,
                                    DocumentContentType = sdFile.ContentType,
                                    DocumentData = bytes,
                                    DocumentName = GetFileName(sdFile.FileName),
                                    DocumentType = "OD",
                                    DocumenttypeCode = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "OD").FirstOrDefault().Id,
                                    DocumentDescription = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "OD").FirstOrDefault().Decription
                                };
                                dataContext.ComplaintPortalCGRFComplaintDocuments.InsertOnSubmit(objdoc);
                            }
                        };

                        dataContext.ComplaintPortalCGRFComplaintHistories.InsertOnSubmit(new ComplaintPortalCGRFComplaintHistory
                        {
                            AdminRemarks = "Order review request is submitted",
                            ComplaintId = record.Id,
                            Status = ((int)ComplaintPortalService.ComplaintStatus.Review).ToString(),
                            CreatedDate = DateTime.Now,
                            CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                            Description = "Order review request Submitted"
                        });

                        record.ComplaintStatusCode = ((int)ComplaintStatus.Review).ToString();
                        record.ComplaintStatusName = ComplaintStatus.Review.ToString();
                        record.IsReviewRequestRaised = true;

                        dataContext.ComplaintPortalRegisteredComplaintsOrderReviewRequests.InsertOnSubmit(new ComplaintPortalRegisteredComplaintsOrderReviewRequest
                        {
                            ComplaintId = System.Convert.ToInt64(model.ComplaintId),
                            AccountNumber = model.AccountNumber,
                            IsAppealPreferred = model.IsAppealPreferred == "Yes" ? true : false,
                            IsAppliedWithin30Days = model.IsAppliedWithin30Days == "Yes" ? true : false,
                            IsErrorApparent = model.IsErrorApparent == "Yes" ? true : false,
                            IsImportantMatterDiscovery = model.IsImportantMatterDiscovery == "Yes" ? true : false,
                            StatusName = (ComplaintPortalService.ComplaintStatus.Review).ToString(),
                            StatusCode = ((int)ComplaintPortalService.ComplaintStatus.Review).ToString(),
                            CreatedDate = DateTime.Now,
                            CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName
                        });

                        dataContext.SubmitChanges();

                        return record.ComplaintRegistrationNumber;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("ResubmitCGRFComplaintApplication for " + model.ComplaintId + e.Message, this);
                return null;
            }
            return null;
        }


        public string RejoinderByComplainantApplication(string complaintId, IList<HttpPostedFileBase> rejoinderFiles)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    string accountNumber = FormatAccountNumber(UserSession.AEMLComplaintUserSessionContext.AccountNumber);
                    ComplaintPortalRegisteredComplaint record;
                    record = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id.ToString() == complaintId && c.ComplaintStatusCode == ((int)ComplaintStatus.NodalReply).ToString() && c.AccountNumber == accountNumber).FirstOrDefault();
                    if (record != null)
                    {
                        foreach (var sdFile in rejoinderFiles)
                        {
                            if (sdFile != null && sdFile.ContentLength > 0)
                            {
                                Stream fs = sdFile.InputStream;
                                BinaryReader br = new BinaryReader(fs);
                                byte[] bytes = br.ReadBytes((Int32)fs.Length);


                                ComplaintPortalCGRFComplaintDocument objdoc = new ComplaintPortalCGRFComplaintDocument
                                {
                                    ComplaintNumber = record.Id,
                                    CreatedDate = DateTime.Now,
                                    DocumentContentType = sdFile.ContentType,
                                    DocumentData = bytes,
                                    DocumentName = GetFileName(sdFile.FileName),
                                    DocumentType = "JD",
                                    DocumenttypeCode = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "JD").FirstOrDefault().Id,
                                    DocumentDescription = dataContext.ComplaintPortalCGRFDocumentTypeMasters.Where(d => d.DocumentType == "JD").FirstOrDefault().Decription
                                };
                                dataContext.ComplaintPortalCGRFComplaintDocuments.InsertOnSubmit(objdoc);
                            }
                        };

                        dataContext.ComplaintPortalCGRFComplaintHistories.InsertOnSubmit(new ComplaintPortalCGRFComplaintHistory
                        {
                            AdminRemarks = "Rejoinder submitted by complainant",
                            ComplaintId = record.Id,
                            Status = ((int)ComplaintPortalService.ComplaintStatus.Rejoinder).ToString(),
                            CreatedDate = DateTime.Now,
                            CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                            Description = "Rejoinder Submitted"
                        });

                        record.ComplaintStatusCode = ((int)ComplaintStatus.Rejoinder).ToString();
                        record.ComplaintStatusName = ComplaintStatus.Rejoinder.ToString();
                        dataContext.SubmitChanges();

                        return record.ComplaintRegistrationNumber;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("ResubmitCGRFComplaintApplication for " + complaintId + e.Message, this);
                return null;
            }
            return null;
        }

        public bool ScheduleHearingCGRFComplaintApplication(string complaintId, string dateOfHearing)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (complaintId != null && UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                    {
                        //DateTime hearingDate = (DateTime.ParseExact(dateOfHearing, "dd/MM/yyyy", CultureInfo.InvariantCulture));
                        long existingComplaintId = System.Convert.ToInt64(complaintId);

                        if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.Id == existingComplaintId))
                        {
                            ComplaintPortalRegisteredComplaint existingComplaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == existingComplaintId).FirstOrDefault();
                            bool isReviewRequest = existingComplaint.ComplaintStatusCode == ((int)ComplaintPortalService.ComplaintStatus.Review).ToString() ? true : false;
                            dataContext.ComplaintPortalCGRFComplaintHearingSchedules.InsertOnSubmit(new ComplaintPortalCGRFComplaintHearingSchedule
                            {
                                ComplaintId = existingComplaintId,
                                DateOfHearing = dateOfHearing,
                                ScheduledDate = DateTime.Now,
                                IsOrderReviewRequest = isReviewRequest,
                            });

                            dataContext.ComplaintPortalCGRFComplaintHistories.InsertOnSubmit(new ComplaintPortalCGRFComplaintHistory
                            {
                                AdminRemarks = "Hearing scheduled on " + dateOfHearing,
                                ComplaintId = existingComplaintId,
                                CreatedDate = DateTime.Now,
                                CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                                Description = "Complaint Hearing is scheduled by Admin"
                            });
                            dataContext.SubmitChanges();
                            return true;
                        }
                    }
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ScheduleHearingCGRFComplaintApplication CreateLoginLog: " + ex.Message + " For Account number:" + complaintId, this);
            }
            return false;
        }

        public string HearingMOMUploadCGRFComplaintApplication(string complaintId, HttpPostedFileBase complaintHearingMOMFile)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (complaintId != null && UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                    {
                        long existingComplaintId = System.Convert.ToInt64(complaintId);
                        if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.Id == existingComplaintId))
                        {
                            ComplaintPortalRegisteredComplaint existingComplaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == existingComplaintId).FirstOrDefault();

                            if (existingComplaint != null)
                            {
                                if (complaintHearingMOMFile != null && complaintHearingMOMFile.ContentLength > 0)
                                {
                                    Stream fs = complaintHearingMOMFile.InputStream;
                                    BinaryReader br = new BinaryReader(fs);
                                    byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                    var hearingToUpdate = dataContext.ComplaintPortalCGRFComplaintHearingSchedules.Where(c => c.ComplaintId == existingComplaintId && c.MOMDocumentData == null).FirstOrDefault();

                                    if (hearingToUpdate != null)
                                    {
                                        hearingToUpdate.MOMDocumentData = bytes;
                                        hearingToUpdate.MOMFileName = GetFileName(complaintHearingMOMFile.FileName);
                                        hearingToUpdate.MOMDocumentContentType = complaintHearingMOMFile.ContentType;
                                        hearingToUpdate.MOMUploadDate = DateTime.Now;

                                        dataContext.ComplaintPortalCGRFComplaintHistories.InsertOnSubmit(new ComplaintPortalCGRFComplaintHistory
                                        {
                                            AdminRemarks = "MOM uploaded for hearing on date - " + hearingToUpdate.DateOfHearing.ToString(),
                                            ComplaintId = existingComplaintId,
                                            Status = ((int)ComplaintPortalService.ComplaintStatus.NodalReply).ToString(),
                                            CreatedDate = DateTime.Now,
                                            CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                                            Description = "MOM uploaded"
                                        });
                                        dataContext.SubmitChanges();
                                    }
                                }
                                return existingComplaint.ComplaintRegistrationNumber;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("HearingMOMUploadCGRFComplaintApplication for " + complaintId + e.Message, this);
                return null;
            }
            return null;
        }

        public string HearingMOMReUploadCGRFComplaintApplication(string complaintId, HttpPostedFileBase complaintHearingMOMFile)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (complaintId != null && UserSession.AEMLComplaintUserSessionContext.IsAdmin)
                    {
                        long existingComplaintId = System.Convert.ToInt64(complaintId);
                        if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.Id == existingComplaintId))
                        {
                            ComplaintPortalRegisteredComplaint existingComplaint = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.Id == existingComplaintId).FirstOrDefault();

                            if (existingComplaint != null)
                            {
                                if (complaintHearingMOMFile != null && complaintHearingMOMFile.ContentLength > 0)
                                {
                                    Stream fs = complaintHearingMOMFile.InputStream;
                                    BinaryReader br = new BinaryReader(fs);
                                    byte[] bytes = br.ReadBytes((Int32)fs.Length);

                                    var hearingToUpdate = dataContext.ComplaintPortalCGRFComplaintHearingSchedules.Where(c => c.ComplaintId == existingComplaintId && c.MOMDocumentData != null).OrderByDescending(c => c.ScheduledDate).FirstOrDefault();

                                    if (hearingToUpdate != null)
                                    {
                                        hearingToUpdate.MOMDocumentData = bytes;
                                        hearingToUpdate.MOMFileName = GetFileName(complaintHearingMOMFile.FileName);
                                        hearingToUpdate.MOMDocumentContentType = complaintHearingMOMFile.ContentType;
                                        hearingToUpdate.MOMUploadDate = DateTime.Now;

                                        dataContext.ComplaintPortalCGRFComplaintHistories.InsertOnSubmit(new ComplaintPortalCGRFComplaintHistory
                                        {
                                            AdminRemarks = "MOM Reuploaded for hearing on date - " + hearingToUpdate.DateOfHearing.ToString(),
                                            ComplaintId = existingComplaintId,
                                            Status = ((int)ComplaintPortalService.ComplaintStatus.NodalReply).ToString(),
                                            CreatedDate = DateTime.Now,
                                            CreatedBy = UserSession.AEMLComplaintUserSessionContext.LoginName,
                                            Description = "MOM Reploaded"
                                        });
                                        dataContext.SubmitChanges();
                                    }
                                }
                                return existingComplaint.ComplaintRegistrationNumber;
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("HearingMOMUploadCGRFComplaintApplication for " + complaintId + e.Message, this);
                return null;
            }
            return null;
        }

        public string ValidateComplaintFromPreviousLevel(string complaintNumber, string accountNumber)
        {
            try
            {
                ComplaintGetCaseDetailsSAP complaintDetails = FetchcomplaintDetails(accountNumber, "2");

                if (complaintDetails != null && complaintDetails.Complaints != null && complaintDetails.Complaints.Count > 0)
                {
                    if (complaintDetails.Complaints.Any(c => c.Complaintnumber == complaintNumber && c.ComplaintStatus == "1"))
                        return complaintNumber;
                    else
                        return null;
                }
            }
            catch (Exception e)
            {
                Log.Error("Error at ValidateComplaintFromPreviousLevel " + e.Message, this);
            }
            return null;
        }

        public MailMessage GetMailTemplateComplaintCGRFForConsumer(int actionType)
        {
            Data.Items.Item settingsItem;
            switch (actionType)
            {
                case (int)ComplaintPortalService.ActionType.Submit:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintSubmitMailComplaintPortalCGRF);
                    break;
                case (int)ComplaintPortalService.ActionType.Approved:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintApprovalMailComplaintPortal);
                    break;
                case (int)ComplaintPortalService.ActionType.Resubmission:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintReSubmitMailComplaintPortal);
                    break;
                case (int)ComplaintPortalService.ActionType.Resubmit:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintAskToReSubmitMailComplaintPortal);
                    break;
                case (int)ComplaintPortalService.ActionType.SubmitCGRF:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintSubmitMailComplaintPortalCGRF);
                    break;
                case (int)ComplaintPortalService.ActionType.NodalReply:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintNodalReplyMailComplaintPortalCGRF);
                    break;
                case (int)ComplaintPortalService.ActionType.HearingScheduled:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintHearingScheduledMailComplaintPortalCGRF);
                    break;
                case (int)ComplaintPortalService.ActionType.Closed:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintClosedMailComplaintPortalCGRF);
                    break;
                case (int)ComplaintPortalService.ActionType.Review:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintReviewMailComplaintPortal);
                    break;
                default:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintSubmitMailComplaintPortal);
                    break;

            }
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetMailTemplateComplaintToMembers(int actionType)
        {
            Data.Items.Item settingsItem;
            switch (actionType)
            {
                case (int)ComplaintPortalService.ActionType.Approved:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintApprovalMailToMembersComplaintPortal);
                    break;
                case (int)ComplaintPortalService.ActionType.Review:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintReviewMailToMembersComplaintPortal);
                    break;
                case (int)ComplaintPortalService.ActionType.HearingScheduled:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintHearingScheduledMailComplaintPortalCGRF);
                    break;
                case (int)ComplaintPortalService.ActionType.Submit:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintSubmitMailToSecretaryComplaintPortal);
                    break;
                case (int)ComplaintPortalService.ActionType.Resubmit:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintAskToReSubmitMailToSecretaryComplaintPortal);
                    break;
                case (int)ComplaintPortalService.ActionType.Resubmission:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintReSubmitMailToSecretaryComplaintPortal);
                    break;
                case (int)ComplaintPortalService.ActionType.NodalReply:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintNodalReplyMailToSecretaryComplaintPortalCGRF);
                    break;
                case (int)ComplaintPortalService.ActionType.Rejoinder:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintRejoinderMailToSecretaryComplaintPortalCGRF);
                    break;
                case (int)ComplaintPortalService.ActionType.Closed:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintClosedMailComplaintPortalCGRF);
                    break;
                default:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintSubmitMailToSecretaryComplaintPortal);
                    break;

            }
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetMailTemplateComplaintToSecretary(int actionType)
        {
            Data.Items.Item settingsItem;
            switch (actionType)
            {
                case (int)ComplaintPortalService.ActionType.Submit:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintSubmitMailToSecretaryComplaintPortal);
                    break;
                case (int)ComplaintPortalService.ActionType.Resubmit:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintAskToReSubmitMailToSecretaryComplaintPortal);
                    break;
                case (int)ComplaintPortalService.ActionType.Resubmission:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintReSubmitMailToSecretaryComplaintPortal);
                    break;
                case (int)ComplaintPortalService.ActionType.NodalReply:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintNodalReplyMailToSecretaryComplaintPortalCGRF);
                    break;
                case (int)ComplaintPortalService.ActionType.Rejoinder:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintRejoinderMailToSecretaryComplaintPortalCGRF);
                    break;
                case (int)ComplaintPortalService.ActionType.HearingScheduled:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintHearingScheduledMailComplaintPortalCGRF);
                    break;
                case (int)ComplaintPortalService.ActionType.Review:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintReviewMailToSecretaryComplaintPortal);
                    break;
                case (int)ComplaintPortalService.ActionType.Closed:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintClosedMailComplaintPortalCGRF);
                    break;
                default:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintSubmitMailToSecretaryComplaintPortal);
                    break;

            }
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }


        public MailMessage GeForwardtMailToConsumers()
        {
            Data.Items.Item settingsItem;

            settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintForwardMailComplaintPortalCGRF);

            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }

        public MailMessage GetMailTemplateComplaintToNodalOfficer(int actionType)
        {
            Data.Items.Item settingsItem;
            switch (actionType)
            {
                case (int)ComplaintPortalService.ActionType.Approved:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintApprovalMailToNodalOfficerComplaintPortal);
                    break;
                default:
                    settingsItem = Context.Database.GetItem(Templates.MailTemplate.ComplaintApprovalMailToNodalOfficerComplaintPortal);
                    break;

            }
            var mailTemplateItem = settingsItem;
            var fromMail = mailTemplateItem.Fields[Templates.MailTemplate.Fields.From];

            if (string.IsNullOrEmpty(fromMail.Value))
            {
                throw new InvalidValueException("'From' field in mail template should be set");
            }

            var body = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Body];
            var subject = mailTemplateItem.Fields[Templates.MailTemplate.Fields.Subject];

            return new MailMessage
            {
                From = new MailAddress(fromMail.Value),
                Body = body.Value,
                Subject = subject.Value,
                IsBodyHtml = true
            };
        }


        public String GetFileName(string fileName)
        {
            try
            {
                if (fileName.Length > 45)
                {
                    return Path.GetFileNameWithoutExtension(fileName).Substring(0, 45) + Path.GetExtension(fileName);
                }
                else
                    return fileName;
            }
            catch
            {
                return fileName;
            }
        }

        public ComplaintGetCaseDetailsSAP FetchcomplaintDetails(string accountNumber, string month)
        {
            ComplaintGetCaseDetailsSAP result = new ComplaintGetCaseDetailsSAP();
            try
            {
                accountNumber = accountNumber.TrimStart(new Char[] { '0' });
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                string userid = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserID].Value; //"Tmservice";
                string password = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserPassword].Value; // "pass@123";
                string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.EncryptionKey].Value; // "@Aeml#2020";
                clsEncryptAES objcrypt = new clsEncryptAES(EncryptionKey);

                SapPiService.FetchComplaintDetails.GetCaseDetailsSoapClient objCaseDetailsSoapClient = new SapPiService.FetchComplaintDetails.GetCaseDetailsSoapClient();
                SapPiService.FetchComplaintDetails.AuthHeader header = new SapPiService.FetchComplaintDetails.AuthHeader();
                header.Username = objcrypt.EncryptText(userid);
                header.Password = objcrypt.EncryptText(password);

                DataTable[] output = objCaseDetailsSoapClient.GetCaseDetailsCA(header, objcrypt.EncryptText(accountNumber), objcrypt.EncryptText(month));

                result.Flag = objcrypt.DecryptText((output[0].Rows[0][0]).ToString());
                result.Message = objcrypt.DecryptText((output[0].Rows[0][1]).ToString());

                if (result.Flag == "1" && output.Count() > 1 && output[1] != null)
                {
                    result.Complaints = new List<ComplaintCaseDetails>();
                    DataTable complaintDetails = output[1];
                    for (int i = 0; i < complaintDetails.Rows.Count; i++)
                    {
                        ComplaintCaseDetails obj = new ComplaintCaseDetails();
                        obj.Complaintnumber = objcrypt.DecryptText((complaintDetails.Rows[i]["Complaintnumber"]).ToString());
                        obj.ComplaintStatus = objcrypt.DecryptText((complaintDetails.Rows[i]["ComplaintStatus"]).ToString());
                        obj.ComplaintType = objcrypt.DecryptText((complaintDetails.Rows[i]["ComplaintType"]).ToString());
                        obj.ComplaintSUbtype = objcrypt.DecryptText((complaintDetails.Rows[i]["ComplaintSUbtype"]).ToString());
                        obj.CreatedOn = objcrypt.DecryptText((complaintDetails.Rows[i]["CreatedOn"]).ToString());
                        obj.ModifiedOn = objcrypt.DecryptText((complaintDetails.Rows[i]["ModifiedOn"]).ToString());
                        result.Complaints.Add(obj);
                    }
                }
                return result;
            }
            catch (Exception e)
            {
                Log.Error("Exception at FetchcomplaintDetails for account number and month:" + accountNumber + "," + month + ", exception:" + e.Message, this);
                return result;
            }
        }

        public string FetchAreaAndPoleDetails(string lat, string lng)
        {
            try
            {
                SapPiService.GetAREAandPOLE.ServiceSoapClient obj = new SapPiService.GetAREAandPOLE.ServiceSoapClient();

                string result = obj.GetGISDetails(lat, lng);

                return result;
            }
            catch (Exception e)
            {
                Log.Error("Exception at FetchAreaAndPoleDetails for lat and Long:" + lat + "," + lng + ", exception:" + e.Message, this);
                return string.Empty;
            }
        }

        public bool InsertcomplaintFeedback(string ComplaintNumber, string AccountNumber, bool IsFeedbackshared, string MobileNumber, string Overall_Experience = null, string Concern_Addressed = null, string Remarks = null)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (ComplaintNumber != null)
                    {
                        ComplaintPortalFeedback objComplaintPortalFeedback = new ComplaintPortalFeedback();
                        if (IsFeedbackshared)
                        {
                            var feedbackResult = PostFeedback(ComplaintNumber, AccountNumber, true, MobileNumber, "", Overall_Experience, Concern_Addressed, Remarks);

                            objComplaintPortalFeedback = new ComplaintPortalFeedback
                            {
                                ComplaintRegistrationNumber = ComplaintNumber,
                                ConcernAddressedRating = Concern_Addressed,
                                CreatedBy = "",
                                CreatedDate = DateTime.Now,
                                FeedbackNumber = feedbackResult.FeedbackSerialNumber,
                                IsFeedbackGiven = true,
                                OverallExperienceRating = Overall_Experience,
                                Remarks = Remarks,
                                Message = feedbackResult.Message
                            };
                        }
                        else
                        {
                            objComplaintPortalFeedback.ComplaintRegistrationNumber = ComplaintNumber;
                            objComplaintPortalFeedback.CreatedDate = DateTime.Now;
                            objComplaintPortalFeedback.CreatedBy = "";
                            objComplaintPortalFeedback.IsFeedbackGiven = false;
                        }
                        dataContext.ComplaintPortalFeedbacks.InsertOnSubmit(objComplaintPortalFeedback);
                        dataContext.SubmitChanges();
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public FeedbackResult PostFeedback(string ComplaintNumber, string AccountNumber, bool IsFeedbackshared, string MobileNumber, string FeedbackNumber = null, string Overall_Experience = null, string Concern_Addressed = null, string Remarks = null)
        {
            FeedbackResult result = new FeedbackResult();
            try
            {
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                string userid = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserID].Value; //"Tmservice";
                string password = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserPassword].Value; // "pass@123";
                string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.EncryptionKey].Value; // "@Aeml#2020";
                clsEncryptAES objcrypt = new clsEncryptAES(EncryptionKey);

                SapPiService.FeedbackReferenceNumber.UpdateFeedBackSoapClient objFeedbackClient = new SapPiService.FeedbackReferenceNumber.UpdateFeedBackSoapClient();
                SapPiService.FeedbackReferenceNumber.AuthHeader header = new SapPiService.FeedbackReferenceNumber.AuthHeader();
                header.Username = objcrypt.EncryptText(userid);
                header.Password = objcrypt.EncryptText(password);

                var accountNumberEncrypted = objcrypt.EncryptText(AccountNumber.TrimStart('0'));
                var Feedback_type = objcrypt.EncryptText("10");
                var mobileNumberEncrypted = objcrypt.EncryptText(MobileNumber);
                var complaintNumberEncrypted = objcrypt.EncryptText(ComplaintNumber);

                string[] output = objFeedbackClient.Post_feeback(header, accountNumberEncrypted, Feedback_type, mobileNumberEncrypted, "", "", complaintNumberEncrypted, "");

                FeedbackNumber = objcrypt.DecryptText(output[0].ToString());
                string Feedback_sr_no = objcrypt.DecryptText(output[0].ToString());
                if (Feedback_sr_no != "2")
                {
                    SapPiService.FeedbackPostService.feedbackUpdateSoapClient objPost_FeedbackRequest = new SapPiService.FeedbackPostService.feedbackUpdateSoapClient();

                    SapPiService.FeedbackPostService.AuthHeader header1 = new SapPiService.FeedbackPostService.AuthHeader();
                    header1.Username = objcrypt.EncryptText(userid);
                    header1.Password = objcrypt.EncryptText(password);

                    string OverallExperience = "", Attitude_Empathy = "", Quality = "", Process = "", Attitude_Disatifaction_Reason = "", Quality_Disatifaction_CC = "", CCC_intraction_with = "", Quality_Disatifaction_CCC = "", Quality_Disatifaction_EMAIL = "", Quality_Disatifaction_Digital = "", Quality_Disatisfaction_Backend = "", Number_Contacted = "", Concerns_addressed = "", Ease_of_register_concerns = "", Informed_alternate_digital_channels = "", Recommandation_scale_Adani_Electricity = "", Suggestion_improve_value = "", valuable_inputs = "", Rate_Adani_Brand = "", Timelines_of_our_services = "", Safety_practices = "", Rate_Technology_Equipment = "", Rate_Quality_Cable_laying = "", Rate_responsivenes_Adani_Pont_of_contact = "", Process_Disatifaction_reason = "", remark = "";

                    Feedback_sr_no = objcrypt.EncryptText(objcrypt.DecryptText(output[0].ToString()));
                    OverallExperience = objcrypt.EncryptText(Overall_Experience);
                    Concerns_addressed = objcrypt.EncryptText(Concern_Addressed);
                    remark = objcrypt.EncryptText(Remarks);

                    string[] output1 = objPost_FeedbackRequest.Post_Feedback(header1, Feedback_sr_no, Feedback_type, OverallExperience, Attitude_Empathy, Quality, Process, Attitude_Disatifaction_Reason, Quality_Disatifaction_CC, CCC_intraction_with, Quality_Disatifaction_CCC, Quality_Disatifaction_EMAIL, Quality_Disatifaction_Digital, Quality_Disatisfaction_Backend, Number_Contacted, Concerns_addressed, Ease_of_register_concerns, Informed_alternate_digital_channels, Recommandation_scale_Adani_Electricity, Suggestion_improve_value, valuable_inputs, Rate_Adani_Brand, Timelines_of_our_services, Safety_practices, Rate_Technology_Equipment, Rate_Quality_Cable_laying, Rate_responsivenes_Adani_Pont_of_contact, Process_Disatifaction_reason, remark);

                    string flag = objcrypt.DecryptText(output1[0].ToString());
                    string message = objcrypt.DecryptText(output1[1].ToString());

                    if (message.ToLower() == "success")
                        return result = new FeedbackResult { FeedbackSerialNumber = FeedbackNumber, IsFeedbackUpdateInSAP = true };
                    else
                        return result = new FeedbackResult { FeedbackSerialNumber = FeedbackNumber, IsFeedbackUpdateInSAP = false, Message = message };
                }
                return result = new FeedbackResult { FeedbackSerialNumber = "", IsFeedbackUpdateInSAP = false, Message = objcrypt.DecryptText(output[1].ToString()) };
            }
            catch (Exception ex)
            {
                return result = new FeedbackResult { FeedbackSerialNumber = "", IsFeedbackUpdateInSAP = false, Message = ex.Message };
            }
        }

        public List<ComplaintDetails> GetOpenComplaintsFromSAPByCA(string accountNumber)
        {
            List<ComplaintDetails> complaints = new List<ComplaintDetails>();

            //Get all IGR M20 Complaints 
            List<ComplaintDetailsIGR> result = SapPiService.Services.RequestHandler.FetchComplaints(accountNumber);
            if (result != null && result.Count() > 0)
            {
                foreach (var com in result)
                {
                    if (com.Complaint_Status == "INPROCESS")
                    {
                        complaints.Add(new ComplaintDetails
                        {
                            IsIGR = true,
                            CreatedOnSAPDateType = DateTime.Parse(com.ERDate.ToString()),
                            ComplaintNumber = com.AUFNR,
                            ComplaintStatusCode = com.IPHAS,
                            ComplaintStatusName = com.Complaint_Status,
                            ComplaintCategory = com.Complaint_Type,
                            ComplaintSubCategory = com.ILART,
                            CreatedOnSAP = com.ERDate, //(DateTime.Parse(com.ERDate.ToString())).ToString("dd/MM/yyyy"),
                            TATDate = com.GLTRP
                        });
                    }
                }
            }

            //Get all Complaints general 
            ComplaintGetCaseDetailsSAP complaintDetails = FetchcomplaintDetails(accountNumber, "2");
            if (complaintDetails != null && complaintDetails.Complaints != null && complaintDetails.Complaints.Count() > 0)
            {
                foreach (var com in complaintDetails.Complaints)
                {
                    if (com.ComplaintStatus != "1")
                    {
                        complaints.Add(new ComplaintDetails
                        {
                            CreatedOnSAPDateType = DateTime.Parse(com.CreatedOn.ToString()),
                            ComplaintNumber = com.Complaintnumber,
                            ComplaintStatusCode = com.ComplaintStatus,
                            ComplaintStatusName = com.ComplaintStatus == "1" ? "Closed" : "Open",
                            ComplaintCategory = com.ComplaintType,
                            ComplaintSubCategory = com.ComplaintSUbtype,
                            CreatedOnSAP = com.CreatedOn,
                            ModifiedOnSAP = com.ModifiedOn
                        });
                    }
                }
            }

            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                foreach (var comp in complaints)
                {
                    if (dataContext.ComplaintPortalRegisteredComplaints.Any(c => c.ComplaintRegistrationNumber == comp.ComplaintNumber))
                    {
                        comp.M20ComplaintId = dataContext.ComplaintPortalRegisteredComplaints.FirstOrDefault(c => c.ComplaintRegistrationNumber == comp.ComplaintNumber).ComplaintIdSAP;
                    }

                    if (comp.ComplaintSubCategory == "M52" && comp.ComplaintStatusCode == "3")
                    {
                        if (complaints.Any(c => c.ComplaintSubCategory == "I01"))
                        {
                            comp.ComplaintStatusName = "Cancelled";
                        }
                    }

                    try
                    {
                        switch (comp.ComplaintSubCategory)
                        {
                            case "M40"://3 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(3).ToString();
                                break;
                            case "M20"://3 days
                                comp.TATDate = comp.TATDate;
                                break;
                            case "M50"://24 hrs
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(1).ToString();
                                break;
                            case "M51"://24 hrs
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(1).ToString();
                                break;
                            case "M06"://15 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                break;
                            case "I15"://15 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                break;
                            case "M03"://15 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                break;
                            case "M04"://15 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                break;
                            case "M14"://3 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(3).ToString();
                                break;
                            case "M24"://15 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                break;
                            case "M52"://15 days
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(15).ToString();
                                break;
                            default:
                                comp.TATDate = DateTime.Parse(comp.CreatedOnSAP.ToString()).AddDays(1).ToString();
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Error("Error at GetOpenComplaintsFromSAPByCA: " + ex.Message, this);
                    }
                }
            }

            complaints = complaints.OrderByDescending(d => d.CreatedOnSAPDateType).ThenByDescending(d => d.ComplaintNumber).ToList();

            return complaints;
        }

        public List<ComplaintDetails> GetCGRFOpenComplaintsByAccountNumber(string accountNumber, string status)
        {
            List<ComplaintDetails> result = new List<ComplaintDetails>();
            try
            {
                accountNumber = FormatAccountNumber(accountNumber);
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var applicationsList = dataContext.ComplaintPortalRegisteredComplaints.Where(c => c.AccountNumber == accountNumber && c.ComplaintLevel.ToLower() == "cgrf").OrderByDescending(o => o.CreatedDate).ToList();

                    foreach (var app in applicationsList)
                    {
                        result.Add(new ComplaintDetails
                        {
                            ComplaintRegistrationNumber = app.ComplaintRegistrationNumber,
                            ComplaintCategory = app.ComplaintCategory,
                            ComplaintSubCategory = app.ComplaintSubCategory,
                            ComplaintLevel = app.ComplaintLevel,
                            ComplaintDescription = app.ComplaintDescription,
                            ComplaintId = app.Id,
                            ComplaintNumber = app.ComplaintIdSAP,
                            ComplaintStatusCode = app.ComplaintStatusCode,
                            ComplaintStatusName = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusName,
                            ComplaintStatusDescription = dataContext.ComplaintPortalCGRFComplaintStatusMasters.Where(s => s.ComplaintStatusCode == app.ComplaintStatusCode).FirstOrDefault().ComplaintStatusDescription,
                            CreatedDate = app.CreatedDate
                        });
                    }

                    if (!string.IsNullOrEmpty(status))
                    {
                        result = result.Where(a => a.ComplaintStatusCode.Trim() != status.Trim()).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetCGRFOpenComplaintsByAccountNumber: " + ex.Message, this);
            }
            return result;
        }

    }

    public class FeedbackResult
    {
        public string FeedbackSerialNumber { get; set; }
        public bool IsFeedbackUpdateInSAP { get; set; }
        public string Message { get; set; }
    }
}