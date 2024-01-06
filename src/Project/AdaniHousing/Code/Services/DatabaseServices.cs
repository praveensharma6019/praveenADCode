using Sitecore.AdaniHousing.Website.Models;
using Sitecore.Diagnostics;
using Sitecore.Foundation.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.AdaniHousing.Website.Services
{
    public class DatabaseServices
    {
        private static Sitecore.Data.Database webdbObj = Sitecore.Configuration.Factory.GetDatabase("web");

        #region Data Store in DB
        public void StoreContactUsData(AdaniHousingContactUsModal model)
        {
            try
            {
                if (model != null)
                {
                    Log.Info("Method - Store Adani Housing Contact Us form in Database :" + model.EmailID, this);
                    using (AdaniHousingFormsDataContext dbContext = new AdaniHousingFormsDataContext())
                    {
                        AdaniHousingContactUsForm obj = new AdaniHousingContactUsForm();
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
                        dbContext.AdaniHousingContactUsForms.InsertOnSubmit(obj);
                        dbContext.SubmitChanges();
                    }
                    Log.Info("Method - Successfully Stored Adani Housing Contact Us form in Database :" + model.EmailID, this);
                }
            }
            catch (Exception e)
            {
                Log.Error("Method - Store Adani Housing Contact Us form in Database : :" + model.EmailID + e.Message, this);
            }
        }

        public void StoreBecomePartnerData(BecomeAPartnerModel model)
        {
            try
            {
                if (model != null)
                {
                    Log.Info("Method - Store Adani Housing Become a Partner form in Database :" + model.EmailID, this);
                    using (AdaniHousingFormsDataContext dbContext = new AdaniHousingFormsDataContext())
                    {
                        AdaniHousingBecomeAPartnerForm obj = new AdaniHousingBecomeAPartnerForm();
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
                        dbContext.AdaniHousingBecomeAPartnerForms.InsertOnSubmit(obj);
                        dbContext.SubmitChanges();
                    }
                    Log.Info("Method - Successfully Stored Adani Housing Become a Partner form in Database :" + model.EmailID, this);
                }
            }
            catch (Exception e)
            {
                Log.Error("Method - Store Adani Housing Become a Partner form in Database : :" + model.EmailID + e.Message, this);
            }
        }

        public void StoreApplyForLoanData(AdaniHousingApplyforLoanModel model)
        {
            try
            {
                if (model != null)
                {
                    Log.Info("Method - Store Adani Housing StoreApplyForLoanData form in Database :" + model.EmailID, this);
                    using (AdaniHousingFormsDataContext dbContext = new AdaniHousingFormsDataContext())
                    {
                        AdaniHousingApplyForLoanForm obj = new AdaniHousingApplyForLoanForm();
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
                        dbContext.AdaniHousingApplyForLoanForms.InsertOnSubmit(obj);
                        dbContext.SubmitChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Method - Store Adani Housing StoreApplyForLoanData form in Database : :" + model.EmailID + e.Message, this);
            }
        }
        public void StoreFeedbackFormData(FeedbackFormModel model)
        {
            model.SavedinDB = false;
            try
            {
                if (model != null)
                {
                    Log.Info("Method - Store Adani Housing StoreApplyForLoanData form in Database :" + model.EmailID, this);
                    using (AdaniHousingFormsDataContext dbContext = new AdaniHousingFormsDataContext())
                    {
                        AdaniHousingFeedbackForm obj = new AdaniHousingFeedbackForm();
                        model.Id = Guid.NewGuid();
                        obj.Id = model.Id;
                        obj.Name = model.Name;
                        obj.Email = model.EmailID;
                        obj.Mobile = model.MobileNo;
                        obj.Rating = model.Rating;
                        obj.FeedbackMsg = model.Message;
                        obj.PageInfo = model.PageInfo;
                        obj.SubmittedOn = DateTime.Now;
                        dbContext.AdaniHousingFeedbackForms.InsertOnSubmit(obj);
                        dbContext.SubmitChanges();
                        model.SavedinDB = true;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Method - Store Adani Housing StoreApplyForLoanData form in Database : :" + model.EmailID + e.Message, this);
            }
        }
        public void StoreRequestCallbackFormData(RequestCallbackModel model)
        {
            model.SavedinDB = false;
            try
            {
                if (model != null)
                {
                    Log.Info("Method - Store Adani Housing StoreRequestCallbackFormData form in Database :" + model.EmailID, this);
                    using (AdaniHousingFormsDataContext dbContext = new AdaniHousingFormsDataContext())
                    {
                        AdaniHousingRequestCallbackForm obj = new AdaniHousingRequestCallbackForm();
                        obj.Id = Guid.NewGuid();
                        obj.Name = model.Name;
                        obj.Email = model.EmailID;
                        obj.Mobile = model.MobileNo;
                        obj.Message = model.Message;
                        obj.PageInfo = model.PageInfo;
                        obj.FormName = model.FormName;
                        obj.FreshdeskTicketID = model.TicketID;
                        obj.IsSubmittedToFreshdesk = model.IsSubmittedToFreshdesk;
                        obj.SubmittedOn = DateTime.Now;
                        dbContext.AdaniHousingRequestCallbackForms.InsertOnSubmit(obj);
                        dbContext.SubmitChanges();
                        model.SavedinDB = true;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Method - Store Adani Housing StoreRequestCallbackFormData form in Database : :" + model.EmailID + model.MobileNo + model.Name + e.Message, this);
            }
        }

        public void StoreAfterLoginSupportFormData(FreshdeskCreateTicketModel model)
        {
            model.SavedinDB = false;
            try
            {
                if (model != null)
                {
                    Log.Info("Method - Store Adani Housing StoreAfterLoginSupportFormData in Database :" + model.EmailID, this);
                    using (AdaniHousingFormsDataContext dbContext = new AdaniHousingFormsDataContext())
                    {
                        AdaniHousingFreshdeskTicketCreate obj = new AdaniHousingFreshdeskTicketCreate();
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
                        dbContext.AdaniHousingFreshdeskTicketCreates.InsertOnSubmit(obj);
                        dbContext.SubmitChanges();
                        model.SavedinDB = true;
                    }
                }
            }
            catch (Exception e)
            {
                Log.Error("Method - Store Adani Housing StoreAfterLoginSupportFormData in Database : :" + model.EmailID + model.MobileNo + model.Name + e.Message, this);
            }
        }
        public void StoreOTP(OTPModel model)
        {
            model.SavedinDB = false;
            try
            {
                if (model != null)
                {
                    Log.Info(string.Concat("Method - Store Adani Housing StoreOTP in Database :", model.MobileNo), this);
                    using (AdaniHousingFormsDataContext adaniHousingFormsDataContext = new AdaniHousingFormsDataContext())
                    {
                        AdaniCapitalHousingOTP oTP = (
                            from x in adaniHousingFormsDataContext.AdaniCapitalHousingOTPs
                            where x.MobileNo == model.MobileNo && x.PageInfo == model.PageInfo
                            select x).FirstOrDefault<AdaniCapitalHousingOTP>();
                        if (oTP == null)
                        {
                            AdaniCapitalHousingOTP adaniCapitalHousingOTP = new AdaniCapitalHousingOTP()
                            {
                                MobileNo = model.MobileNo,
                                OTP = model.OTP,
                                PageInfo = model.PageInfo,
                                DateTime = new DateTime?(model.DateTime),
                                Count = model.Count ?? "1"
                            };
                            adaniHousingFormsDataContext.AdaniCapitalHousingOTPs.InsertOnSubmit(adaniCapitalHousingOTP);
                            adaniHousingFormsDataContext.SubmitChanges();
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
                            adaniHousingFormsDataContext.SubmitChanges();
                            model.SavedinDB = true;
                        }
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Error(string.Concat("Method - Store Adani Housing StoreOTP in Database : :", model.PageInfo, model.MobileNo, exception.Message), this);
            }
        }
        #endregion
        public void StoreUserLoginSession(UserLoginSessionModel model)
        {
            try
            {
                if (model != null)
                {
                    Log.Info(string.Concat("Method - Store Adani Housing StoreUserLoginSession in Database :", model.MobileNo), this);
                    using (AdaniHousingFormsDataContext adaniHousingFormsDataContext = new AdaniHousingFormsDataContext())
                    {
                        AdaniHousingUserLoginHistory m = (
                            from x in adaniHousingFormsDataContext.AdaniHousingUserLoginHistories
                            where x.MobileNo == model.MobileNo 
                            select x).FirstOrDefault<AdaniHousingUserLoginHistory>();
                        if (m == null)
                        {
                            AdaniHousingUserLoginHistory housingLogin = new AdaniHousingUserLoginHistory()
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
                            adaniHousingFormsDataContext.AdaniHousingUserLoginHistories.InsertOnSubmit(housingLogin);
                            adaniHousingFormsDataContext.SubmitChanges();
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
                            adaniHousingFormsDataContext.SubmitChanges();
                        }
                        Log.Info(string.Concat("Data saved successfully in Adani Housing StoreUserLoginSession in Database :", model.MobileNo), this);
                    }
                }
            }
            catch (Exception exception1)
            {
                Exception exception = exception1;
                Log.Error(string.Concat("Method - Store Adani Housing StoreUserLoginSession in Database : :", model.SessionId, model.MobileNo, exception.Message), this);
            }
        }
    }
}