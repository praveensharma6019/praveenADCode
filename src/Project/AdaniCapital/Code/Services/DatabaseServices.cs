using Sitecore.AdaniCapital.Website.Models;
using Sitecore.Diagnostics;
using Sitecore.Foundation.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniCapital.Website.Services
{
    public class DatabaseServices
    {
        private static Sitecore.Data.Database webdbObj = Sitecore.Configuration.Factory.GetDatabase("web");

        #region Data Store in DB
        public void StoreContactUsData(AdaniCapitalContactUsModal model)
        {
            try
            {
                if(model != null)
                {
                    Log.Info("Method - Store Adani Capital Contact Us form in Database :" + model.EmailID, this);
                    using (AdaniCapitalFormsDataContext dbContext = new AdaniCapitalFormsDataContext())
                    {
                        AdaniCapitalContactUsForm obj = new AdaniCapitalContactUsForm();
                        obj.Id = Guid.NewGuid();
                        obj.Name = model.Name;
                        obj.Mobile = model.MobileNo;
                        obj.Subject = model.Subject;
                        obj.Email = model.EmailID;
                        obj.FormName = model.FormName??"";
                        obj.Message = model.Message??"";
                        obj.PageInfo = model.PageInfo;
                        obj.TicketID = model.TicketID ?? "";
                        obj.IsSubmittedToFreshdesk = !string.IsNullOrEmpty(model.TicketID) ? true : false; ;
                        obj.SubmittedOn = DateTime.Now;
                        dbContext.AdaniCapitalContactUsForms.InsertOnSubmit(obj);
                        dbContext.SubmitChanges();
                    }
                    Log.Info("Method - Successfully Stored Adani Capital Contact Us form in Database :" + model.EmailID, this);
                }
            }
            catch(Exception e)
            {
                Log.Error("Method - Store Adani Capital Contact Us form in Database : :"+ model.EmailID + e.Message, this);
            }
        }

        public void StoreBecomePartnerData(BecomeAPartnerModel model)
        {
            try
            {
                if (model != null)
                {
                    Log.Info("Method - Store Adani Capital Become a Partner form in Database :" + model.EmailID, this);
                    using (AdaniCapitalFormsDataContext dbContext = new AdaniCapitalFormsDataContext())
                    {
                        AdaniCapitalBecomeAPartnerForm obj = new AdaniCapitalBecomeAPartnerForm();
                        obj.Id = Guid.NewGuid();
                        obj.Name = model.Name;
                        obj.Mobile = model.MobileNo;
                        obj.Subject = model.Subject;
                        obj.Email = model.EmailID;
                        obj.FormName = model.FormName ?? "";
                        obj.Message = model.Message ?? "";
                        obj.PageInfo = model.PageInfo;
                        obj.TicketID = model.TicketID ?? "";
                        obj.IsSubmittedToFreshdesk = !string.IsNullOrEmpty(model.TicketID) ? true : false; ;
                        obj.SubmittedOn = DateTime.Now;
                        dbContext.AdaniCapitalBecomeAPartnerForms.InsertOnSubmit(obj);
                        dbContext.SubmitChanges();
                    }
                    Log.Info("Method - Successfully Stored Adani Capital Become a Partner form in Database :" + model.EmailID, this);
                }
            }
            catch (Exception e)
            {
                Log.Error("Method - Store Adani Capital Become a Partner form in Database : :" + model.EmailID + e.Message, this);
            }
        }

        public void StoreApplyForLoanData(AdaniCapitalApplyforLoanModel model)
        {
            try
            {
                if (model != null)
                {
                    Log.Info("Method - Store Adani Capital Contact Us form in Database :" + model.EmailID, this);
                    using (AdaniCapitalFormsDataContext dbContext = new AdaniCapitalFormsDataContext())
                    {
                        AdaniCapitalApplyForLoanForm obj = new AdaniCapitalApplyForLoanForm();
                        obj.Id = Guid.NewGuid();
                        obj.FirstName = model.FirstName;
                        obj.LastName = model.LastName;
                        obj.EmailId = model.EmailID;
                        obj.Mobile = model.MobileNo;
                        //obj.PanNumber = model.PanNumber;
                        obj.PinCode = model.PinCode;
                        //obj.State = model.State;
                        //obj.City = model.City;
                        //obj.Occupation = model.Occupation;
                        //obj.LoanAmount = model.LoanAmount;
                        //obj.Tenure = model.Tenure;
                        //obj.ProductType = model.EnquirySource;
                        //obj.EnquirySource = model.EnquirySource;
                        obj.IsSubmiitedToLMS = model.IsSubmittedToLMS;
                        obj.LMS_Key = model.LMS_RequestKey ?? "";
                        obj.LMS_Value = model.LMS_Response ?? "";
                        obj.FormName = model.FormName ?? "";
                        obj.PageInfo = model.PageInfo;
                        obj.SubmittedOn = DateTime.Now;
                        dbContext.AdaniCapitalApplyForLoanForms.InsertOnSubmit(obj);
                        dbContext.SubmitChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Method - Store Adani Capital Contact Us form in Database : :" + model.EmailID + e.Message, this);
            }
        }
        public void StoreFeedbackFormData(FeedbackFormModel model)
        {
            model.SavedinDB = false;
            try
            {
                if (model != null)
                {
                    Log.Info("Method - Store Adani Capital Contact Us form in Database :" + model.EmailID, this);
                    using (AdaniCapitalFormsDataContext dbContext = new AdaniCapitalFormsDataContext())
                    {
                        AdaniCapitalFeedbackForm obj = new AdaniCapitalFeedbackForm();
                        model.Id = Guid.NewGuid();
                        obj.Id = model.Id;
                        obj.Name = model.Name;
                        obj.Email = model.EmailID;
                        obj.Mobile = model.MobileNo;
                        obj.Rating = model.Rating;
                        obj.FeedbackMsg = model.Message;
                        obj.PageInfo = model.PageInfo;
                        obj.SubmittedOn = DateTime.Now;
                        dbContext.AdaniCapitalFeedbackForms.InsertOnSubmit(obj);
                        dbContext.SubmitChanges();
                        model.SavedinDB = true;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Method - Store Adani Capital StoreFeedbackFormData form in Database : :" + model.EmailID + e.Message, this);
            }
        }
        public void StoreRequestCallbackFormData(RequestCallbackModel model)
        {
            model.SavedinDB = false;
            try
            {
                if (model != null)
                {
                    Log.Info("Method - Store Adani Capital Contact Us form in Database :" + model.EmailID, this);
                    using (AdaniCapitalFormsDataContext dbContext = new AdaniCapitalFormsDataContext())
                    {
                        AdaniCapitalRequestCallbackForm obj = new AdaniCapitalRequestCallbackForm();
                        obj.Id = Guid.NewGuid();
                        obj.Name = model.Name;
                        obj.Email = model.EmailID;
                        obj.Mobile = model.MobileNo;
                        obj.Message = model.Message;
                        obj.PageInfo = model.PageInfo;
                        obj.FormName = model.FormName;
                        obj.SubmittedOn = DateTime.Now;
                        dbContext.AdaniCapitalRequestCallbackForms.InsertOnSubmit(obj);
                        dbContext.SubmitChanges();
                        model.SavedinDB = true;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Method - Store Adani Capital Contact Us form in Database : :" + model.EmailID + model.MobileNo + model.Name + e.Message, this);
            }
        }

        public void StoreAfterLoginSupportFormData(FreshdeskCreateTicketModel model)
        {
            model.SavedinDB = false;
            try
            {
                if (model != null)
                {
                    Log.Info("Method - Store Adani Capital StoreAfterLoginSupportFormData in Database :" + model.EmailID, this);
                    using (AdaniCapitalFormsDataContext dbContext = new AdaniCapitalFormsDataContext())
                    {
                        AdaniCapitalFreshdeskTicketCreate obj = new AdaniCapitalFreshdeskTicketCreate();
                        obj.Id = Guid.NewGuid();
                        obj.Name = model.Name;
                        obj.Email = model.EmailID;
                        obj.Mobile = model.MobileNo;
                        obj.Message = model.Message;
                        obj.PageInfo = model.PageInfo;
                        obj.FormName = model.FormName;
                        obj.FreshdeskTickedId = model.TicketID;
                        obj.IsSubmittedToFreshdesk = model.IsSubmittedToFreshdesk;
                        obj.LAN = model.LoanAccountNo;
                        obj.CreatedOn = DateTime.Now;
                        dbContext.AdaniCapitalFreshdeskTicketCreates.InsertOnSubmit(obj);
                        dbContext.SubmitChanges();
                        model.SavedinDB = true;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Method - Store Adani Capital StoreAfterLoginSupportFormData in Database : :" + model.EmailID + model.MobileNo + model.Name + e.Message, this);
            }
        }
        public void StoreOTP(OTPModel model)
        {
            model.SavedinDB = false;
            try
            {
                if (model != null)
                {
                    Log.Info(string.Concat("Method - Store Adani Capital StoreOTP in Database :", model.MobileNo), this);
                    using (AdaniCapitalFormsDataContext adaniCapitalFormsDataContext = new AdaniCapitalFormsDataContext())
                    {
                        AdaniCapitalOTP oTP = (
                            from x in adaniCapitalFormsDataContext.AdaniCapitalOTPs
                            where x.MobileNo == model.MobileNo && x.PageInfo == model.PageInfo
                            select x).FirstOrDefault<AdaniCapitalOTP>();
                        if (oTP == null)
                        {
                            AdaniCapitalOTP adaniCapitalOTP = new AdaniCapitalOTP()
                            {
                                MobileNo = model.MobileNo,
                                OTP = model.OTP,
                                PageInfo = model.PageInfo,
                                DateTime = new DateTime?(model.DateTime),
                                Count = model.Count ?? "1"
                            };
                            adaniCapitalFormsDataContext.AdaniCapitalOTPs.InsertOnSubmit(adaniCapitalOTP);
                            adaniCapitalFormsDataContext.SubmitChanges();
                            model.SavedinDB = true;
                        }
                        else
                        {
                            oTP.OTP = model.OTP;
                            oTP.PageInfo = model.PageInfo;
                            oTP.DateTime = new DateTime?(model.DateTime);
                            int num = int.Parse(model.Count) + 1;
                            oTP.Count = num.ToString();
                            oTP.ValidateOTPCount = "0";
                            adaniCapitalFormsDataContext.SubmitChanges();
                            model.SavedinDB = true;
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Error(string.Concat("Method - Store Adani Capital StoreOTP in Database : :", model.PageInfo, model.MobileNo, exception.Message), this);
            }
        }
        #endregion
        public void StoreUserLoginSession(UserLoginSessionModel model)
        {
            try
            {
                if (model != null)
                {
                    Log.Info(string.Concat("Method - Store Adani Capital StoreUserLoginSession in Database :", model.MobileNo), this);
                    using (AdaniCapitalFormsDataContext adaniCapitalFormsDataContext = new AdaniCapitalFormsDataContext())
                    {
                        AdaniCapitalUserLoginHistory m = (
                            from x in adaniCapitalFormsDataContext.AdaniCapitalUserLoginHistories
                            where x.MobileNo == model.MobileNo
                            select x).FirstOrDefault<AdaniCapitalUserLoginHistory>();
                        if (m == null)
                        {
                            AdaniCapitalUserLoginHistory CapitalLogin = new AdaniCapitalUserLoginHistory()
                            {
                                MobileNo = model.MobileNo,
                                LastSessionId = model.SessionId,
                                RecentSessionId = model.SessionId,
                                LastUserIP = model.UserIP,//new DateTime?(model.DateTime),
                                RecentUserIP = model.UserIP,
                                LogInCount = 1,
                                LastLoginDateTime = DateTime.Now,
                                RecentLoginDateTime = DateTime.Now
                            };
                            adaniCapitalFormsDataContext.AdaniCapitalUserLoginHistories.InsertOnSubmit(CapitalLogin);
                            adaniCapitalFormsDataContext.SubmitChanges();
                        }
                        else
                        {
                            m.LastSessionId = m.RecentSessionId;
                            m.RecentSessionId = model.SessionId;
                            m.LastUserIP = m.RecentUserIP;//new DateTime?(model.DateTime),
                            m.RecentUserIP = model.UserIP;
                            m.LogInCount = m.LogInCount++;
                            m.LastLoginDateTime = m.RecentLoginDateTime;
                            m.RecentLoginDateTime = DateTime.Now;
                            adaniCapitalFormsDataContext.SubmitChanges();
                        }
                        Log.Info(string.Concat("Data saved successfully in Adani Capital StoreUserLoginSession in Database :", model.MobileNo), this);
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Error(string.Concat("Method - Store Adani Capital StoreUserLoginSession in Database : :", model.SessionId, model.MobileNo, exception.Message), this);
            }
        }
    }
}