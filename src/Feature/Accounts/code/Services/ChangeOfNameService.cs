using SapPiService.Domain;
using Sitecore.Diagnostics;
using Sitecore.Exceptions;
using Sitecore.Feature.Accounts;
using Sitecore.Feature.Accounts.Models;
using Sitecore.Feature.Accounts.SessionHelper;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Models
{
    public class ChangeOfNameService
    {

        private const int AccountNumberLength = 12;

        public MailMessage GetOTPForLoginMailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Templates.MailTemplate.OTPLoginMailLECPortal);
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

        public MailMessage GetOTPMailTemplate()
        {
            var settingsItem = Context.Database.GetItem(Templates.MailTemplate.OTPMailLECPortal);
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

        public bool SendOTPViaEmail(string toEmail, string registrationNumber, string OTP)
        {
            try
            {
                var mail = this.GetOTPMailTemplate();
                mail.To.Add(toEmail);
                mail.Body = mail.Body.Replace("#registrationNumber#", registrationNumber);
                mail.Body = mail.Body.Replace("#OTP#", OTP);
                Log.Info("SendOTPViaEmail email body:" + mail.Body + ", sent to:" + toEmail, this);
                MainUtil.SendMail(mail);
                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + toEmail + " - Error - " + ex.Message + "", ex, this);
                return false;
            }
        }

        public bool SendOTPForLoginViaEmail(string toEmail, string registrationNumber, string OTP)
        {
            try
            {
                var mail = this.GetOTPForLoginMailTemplate();
                mail.To.Add(toEmail);
                mail.Body = mail.Body.Replace("#registrationNumber#", registrationNumber);
                mail.Body = mail.Body.Replace("#OTP#", OTP);
                Log.Info("SendOTPViaEmail email body:" + mail.Body + ", sent to:" + toEmail, this);
                MainUtil.SendMail(mail);
                return true;
            }
            catch (System.Exception ex)
            {
                Log.Error($"Unable to send the email to the " + toEmail + " - Error - " + ex.Message + "", ex, this);
                return false;
            }
        }

        //false if more than 20 OTP generated same day for same CA and Mobile
        public bool IsLECOTPMaxLimitExceed(string LECRegistrationNumber, string mobileNumber, string pageName)
        {
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                var count = dataContext.OTPValidationAEMLs.Where(o => o.AccountNumber == LECRegistrationNumber && o.PageName == pageName && o.CreatedDate.GetValueOrDefault().Date == DateTime.Now.Date).Count();
                if (count >= 20)
                    return true;
                else
                    return false;
            }
        }

        public bool UpdateLECProfile(string LECRegistrationNumber, string mobileNumber, string EmailID)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var profile = dataContext.CONLECRegistrationDetails.Where(o => o.LECRegistrationNumber == LECRegistrationNumber && o.LECIsActive == true).FirstOrDefault();
                    if (profile != null)
                    {
                        CONLECUpdateLog obj = new CONLECUpdateLog
                        {
                            IsProfileUpdated = true,
                            LECRegistrationNumber = LECRegistrationNumber,
                            LECID = profile.ID,
                            LECMobileNumber = mobileNumber,
                            OldMobileNumber = profile.LECMobileNumber,
                            LECEmailID = EmailID,
                            OldEmailId = profile.LECEmailId,
                            DateModified = DateTime.Now,
                            Comments = "Profile is updated."
                        };
                        dataContext.CONLECUpdateLogs.InsertOnSubmit(obj);

                        profile.LECMobileNumber = mobileNumber;
                        profile.LECEmailId = EmailID;
                        dataContext.SubmitChanges();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                Log.Error("UpdateLECProfile error:" + e.Message, this);
                return false;
            }
        }

        public bool DeregisterLECProfile(string LECRegistrationNumber)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var profile = dataContext.CONLECRegistrationDetails.Where(o => o.LECRegistrationNumber == LECRegistrationNumber && o.LECIsActive == true).FirstOrDefault();
                    if (profile != null)
                    {
                        CONLECUpdateLog obj = new CONLECUpdateLog
                        {
                            LECRegistrationNumber = LECRegistrationNumber,
                            LECID = profile.ID,
                            IsDeActivated = true,
                            DateModified = DateTime.Now,
                            Comments = "Profile Deregistered."
                        };
                        dataContext.CONLECUpdateLogs.InsertOnSubmit(obj);

                        profile.LECIsActive = false;
                        profile.LECInActivateDate = DateTime.Now;
                        dataContext.SubmitChanges();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                Log.Error("LEcchangePassword error:" + e.Message, this);
                return false;
            }
        }

        public bool ComparePasswordForUpdateLECPassword(string LECRegistrationNumber, string newPassword)
        {
            try
            {
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.EncryptionKey].Value; // "@Aeml#2020";
                clsEncryptAES objcrypt = new clsEncryptAES(EncryptionKey);

                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var profile = dataContext.CONLECRegistrationDetails.Where(o => o.LECRegistrationNumber == LECRegistrationNumber && o.LECIsActive == true).FirstOrDefault();
                    if (profile != null)
                    {
                        var encryptedNewPassword = objcrypt.encodetext(newPassword);
                        if (encryptedNewPassword == profile.LECPassword)
                            return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                Log.Error("LEcchangePassword error:" + e.Message, this);
                return false;
            }
            return false;
        }

        public bool CheckOldPasswordLEC(string LECRegistrationNumber, string oldPassword)
        {
            try
            {
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.EncryptionKey].Value; // "@Aeml#2020";
                clsEncryptAES objcrypt = new clsEncryptAES(EncryptionKey);

                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var profile = dataContext.CONLECRegistrationDetails.Where(o => o.LECRegistrationNumber == LECRegistrationNumber && o.LECIsActive == true).FirstOrDefault();
                    if (profile != null)
                    {
                        var encryptedOldPassword = objcrypt.encodetext(oldPassword);
                        if (encryptedOldPassword == profile.LECPassword)
                            return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                Log.Error("LEcchangePassword error:" + e.Message, this);
                return false;
            }
            return false;
        }

        public bool UpdateLECPassword(string LECRegistrationNumber, string newPassword)
        {
            try
            {
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.EncryptionKey].Value; // "@Aeml#2020";
                clsEncryptAES objcrypt = new clsEncryptAES(EncryptionKey);

                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var profile = dataContext.CONLECRegistrationDetails.Where(o => o.LECRegistrationNumber == LECRegistrationNumber && o.LECIsActive == true).FirstOrDefault();
                    if (profile != null)
                    {
                        CONLECUpdateLog obj = new CONLECUpdateLog
                        {
                            LECRegistrationNumber = LECRegistrationNumber,
                            LECID = profile.ID,
                            IsPasswordUpdated = true,
                            OldPassword = profile.LECPassword,
                            DateModified = DateTime.Now,
                            Comments = "Password changed."
                        };
                        dataContext.CONLECUpdateLogs.InsertOnSubmit(obj);

                        profile.LECPassword = objcrypt.encodetext(newPassword);
                        profile.LECLastPasswordChangeDate = DateTime.Now;
                        dataContext.SubmitChanges();
                        return true;
                    }
                    else
                        return false;
                }
            }
            catch (Exception e)
            {
                Log.Error("LEcchangePassword error:" + e.Message, this);
                return false;
            }
        }

        public static string FormatAccountNumber(string accountNumber)
        {
            return accountNumber.PadLeft(AccountNumberLength, '0');
        }

        public ChangeOfNameLECUserProfileModel GetLECDetails(string registrationNumber)
        {
            ChangeOfNameLECUserProfileModel model = new ChangeOfNameLECUserProfileModel();
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                var r = dataContext.CONLECRegistrationDetails.Where(l => l.LECRegistrationNumber == registrationNumber).FirstOrDefault();
                if (r != null)
                {
                    model.LECRegistrationNumber = r.LECRegistrationNumber;
                    model.LECMobileNumber = r.LECMobileNumber;
                    model.LECEmailId = r.LECEmailId;
                    model.LECName = r.LECName;
                }
            }
            return model;
        }

        //To be used in change of name
        public static string ValidateCA(CAValidateInfo accountDetails)
        {
            string message = string.Empty;

            if (accountDetails.INVALIDCAFLAG == "X")
            {
                message = "Entered Account no is invalid. Please enter a valid 9 digit Account No.";
            }
            else if (accountDetails.MOVEOUTFLAG == "X")
            {
                message = "Entered Account no is no longer active.";
            }
            else if (!string.IsNullOrEmpty(accountDetails.BSTATUS))
            {
                message = "Change of name request cannot be processed for the entered Account no. Please visit the nearest Divisional office for further assistance.";
            }
            else if (accountDetails.DISCONNECTEDFLAG == "X")
            {
                message = "Entered Account No. is disconnected. Kindly clear the outstanding dues and apply once the connection has been restored.";
            }
            else if (accountDetails.VIGILANCEFLAG == "X")
            {
                message = "Change of name request cannot be processed for the entered Account no. Please visit the nearest Divisional office for further assistance.";
            }
            else if (accountDetails.OVERDUE_AMT == "X" || System.Convert.ToDecimal(accountDetails.OVERDUE_AMT) > 0)
            {
                message = "Entered Account No. has an overdue balance payable. Kindly clear the outstanding dues and try again after 48 hrs.";
            }
            else if (accountDetails.NO_LAST_BILL_FLG == "X")
            {
                message = "For the entered account no, Billing is in process. Please apply once the bill has been generated.";
            }
            else if (accountDetails.LAST_BIL_AMND_FL == "X")
            {
                message = "The latest bill, for the entered Account No, has been amended for relevant reasons. Please apply once the next bill has been generated.";
            }
            else if (accountDetails.CHNG_OVER_FLAG == "X")
            {
                message = "Customer has applied for Changeover";
            }
            else if (accountDetails.TARIFTYP_Ratecategory != "BPL" && accountDetails.TARIFTYP_Ratecategory != "RESI1" && accountDetails.TARIFTYP_Ratecategory != "COMM(A)"
                && accountDetails.TARIFTYP_Ratecategory != "RESI3" && accountDetails.TARIFTYP_Ratecategory != "COMM")
            {
                message = "Change of name not allowed for the rate category";
            }
            return message;
        }

        public static string ValidateCAForSDOptIn(CAValidateInfo accountDetails)
        {
            string message = string.Empty;

            if (accountDetails.INVALIDCAFLAG == "X")
            {
                message = "Entered Account no is invalid. Please enter a valid 9 digit Account No.";
            }
            else if (accountDetails.MOVEOUTFLAG == "X")
            {
                message = "Entered Account no is no longer active.";
            }
            else if (accountDetails.DISCONNECTEDFLAG == "X")
            {
                message = "Entered Account No is disconnected. Kindly clear the outstanding dues and apply once the connection has been reconnected.";
            }
            else if (accountDetails.VIGILANCEFLAG == "X")
            {
                message = "Change of name request cannot be processed for the entered Account no. Please visit the nearest Divisional office for further assistance.";
            }
            
            return message;
        }

        public static string ValidateCAForGreenPower(CAValidateInfo accountDetails)
        {
            string message = string.Empty;

            if (accountDetails.INVALIDCAFLAG == "X")
            {
                message = "Entered Account no is invalid. Please enter a valid 9 digit Account No.";
            }
            else if (accountDetails.MOVEOUTFLAG == "X")
            {
                message = "Entered Account no is no longer active.";
            }
            else if (accountDetails.DISCONNECTEDFLAG == "X")
            {
                message = "Entered Account No is disconnected. Kindly clear the outstanding dues and apply once the connection has been reconnected.";
            }
            return message;
        }

        public static string ValidateCAforENACH(CAValidateInfo accountDetails)
        {
            string message = string.Empty;

            if (accountDetails.INVALIDCAFLAG == "X")
            {
                message = "Entered Account no is invalid. Please enter a valid 9 digit Account No.";
            }
            else if (accountDetails.MOVEOUTFLAG == "X")
            {
                message = "Entered Account no is no longer active.";
            }

            return message;
        }

        public List<CONAreaCityPinMaster> ListAreaPinWorkcenterMapping()
        {
            List<CONAreaCityPinMaster> lstArea = new List<CONAreaCityPinMaster>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    var lstAreaGen = dataContext.CONAreaCityPinMasters.OrderBy(o => o.Area).ToList();
                    foreach (var a in lstAreaGen)
                    {
                        lstArea.Add(
                            new CONAreaCityPinMaster
                            {
                                Id = a.Id,
                                Area = a.Area.ToUpper(),
                                City = a.City.ToUpper(),
                                CreatedBy = a.CreatedBy,
                                CreatedDate = a.CreatedDate,
                                PinCode = a.PinCode.ToUpper(),
                                Remarks = a.Remarks,
                                WorkCenter = a.WorkCenter.ToUpper()
                            });
                    }
                    return lstArea;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ListAreaPinWorkcenterMapping: " + ex.Message, this);
            }
            return lstArea;
        }

        public bool IsLECRegistered(string registrationNumber)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CONLECRegistrationDetails.Any(a => a.LECRegistrationNumber == registrationNumber && a.LECIsActive == true))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at IsLECRegistered: " + ex.Message, this);
            }
            return false;
        }

        public bool IsApplicationExistsForCA(string accountNumber)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CONApplicationDetails.Any(a => a.AccountNumber == FormatAccountNumber(accountNumber)))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at IsApplicationExistsForCA: " + ex.Message, this);
            }
            return false;
        }

        public bool IsApplicationExistsForRegistrationNumberByLEC(string registrationNumber)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CONApplicationDetails.Any(a => a.TempRegistrationSerialNumber == registrationNumber && a.LECRegistrationNumber == UserSession.AEMLCONLECUserSessionContext.RegistrationNumber && a.ApplicationStatusCode != "1"))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at IsApplicationExistsForCA: " + ex.Message, this);
            }
            return false;
        }

        public bool IsApplicationExistsForCAByLEC(string accountNumber)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CONApplicationDetails.Any(a => a.AccountNumber == FormatAccountNumber(accountNumber) && a.LECRegistrationNumber == UserSession.AEMLCONLECUserSessionContext.RegistrationNumber && a.ApplicationStatusCode == "1"))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at IsApplicationExistsForCA: " + ex.Message, this);
            }
            return false;
        }

        public bool IsApplicationExistsForCARegistration(string accountNumber)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CONApplicationDetails.Any(a => a.AccountNumber == FormatAccountNumber(accountNumber) && a.ApplicationStatusCode != "4"))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at IsApplicationExistsForCA: " + ex.Message, this);
            }
            return false;
        }

        public CONApplicationDetail GetExistingApplicationForCARegistration(string accountNumber)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CONApplicationDetails.Any(a => a.AccountNumber == FormatAccountNumber(accountNumber) && a.ApplicationStatusCode != "4"))
                        return dataContext.CONApplicationDetails.FirstOrDefault(a => a.AccountNumber == FormatAccountNumber(accountNumber) && a.ApplicationStatusCode != "4");
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at IsApplicationExistsForCA: " + ex.Message, this);
            }
            return null;
        }

        public CONApplicationDetail GetExistingApplicationByRegistrationNumber(string registrationNumber)
        {
            {
                try
                {
                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                    {
                        if (dataContext.CONApplicationDetails.Any(a => a.TempRegistrationSerialNumber == registrationNumber))
                            return dataContext.CONApplicationDetails.FirstOrDefault(a => a.TempRegistrationSerialNumber == registrationNumber);
                        else
                            return null;
                    }
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at IsApplicationExistsForCA: " + ex.Message, this);
                }
                return null;
            }
        }

        public CONApplicationDetail GetExistingApplicationByRegistrationNumberByLEC(string registrationNumber)
        {
            {
                try
                {
                    using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                    {
                        if (dataContext.CONApplicationDetails.Any(a => a.TempRegistrationSerialNumber == registrationNumber && a.LECRegistrationNumber == UserSession.AEMLCONLECUserSessionContext.RegistrationNumber))
                            return dataContext.CONApplicationDetails.FirstOrDefault(a => a.TempRegistrationSerialNumber == registrationNumber && a.LECRegistrationNumber == UserSession.AEMLCONLECUserSessionContext.RegistrationNumber);
                        else
                            return null;
                    }
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Error("Error at IsApplicationExistsForCA: " + ex.Message, this);
                }
                return null;
            }
        }

        public CONApplicationDetail GetExistingApplicationForCAByLEC(string accountNumber)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CONApplicationDetails.Any(a => a.AccountNumber == FormatAccountNumber(accountNumber) && a.LECRegistrationNumber == UserSession.AEMLCONLECUserSessionContext.RegistrationNumber))
                        return dataContext.CONApplicationDetails.Where(a => a.AccountNumber == FormatAccountNumber(accountNumber) && a.LECRegistrationNumber == UserSession.AEMLCONLECUserSessionContext.RegistrationNumber).OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at IsApplicationExistsForCA: " + ex.Message, this);
            }
            return null;
        }

        public CONApplicationDetail GetExistingApplication(string accountNumber)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CONApplicationDetails.Any(a => a.AccountNumber == FormatAccountNumber(accountNumber)))
                        return dataContext.CONApplicationDetails.Where(a => a.AccountNumber == FormatAccountNumber(accountNumber)).OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                    else
                        return null;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at IsApplicationExistsForCA: " + ex.Message, this);
            }
            return null;
        }

        public List<CONApplicationDetail> GetApplicationsByLEC(DateTime? startDate, DateTime? endDate, string status, string LECRegistrationNumber)
        {
            List<CONApplicationDetail> applicationsList = new List<CONApplicationDetail>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (startDate == null || endDate == null)
                        applicationsList = dataContext.CONApplicationDetails.Where(a => a.LECRegistrationNumber.Trim() == LECRegistrationNumber.Trim()).OrderByDescending(a => a.ModifiedDate).ToList();
                    else
                        applicationsList = dataContext.CONApplicationDetails.Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate && a.LECRegistrationNumber.Trim() == LECRegistrationNumber.Trim()).OrderByDescending(a => a.ModifiedDate).ToList();

                    if (!string.IsNullOrEmpty(status))
                    {
                        applicationsList = applicationsList.Where(a => a.ApplicationStatusCode == status).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetApplications: " + ex.Message, this);
            }
            return applicationsList;
        }

        public List<CONApplicationDetail> GetApplicationsByLECToExport(DateTime? startDate, DateTime? endDate, string status, string LECRegistrationNumber)
        {
            List<CONApplicationDetail> applicationsList = new List<CONApplicationDetail>();
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (startDate == null || endDate == null)
                        applicationsList = dataContext.CONApplicationDetails.Where(a => a.LECRegistrationNumber.Trim() == LECRegistrationNumber.Trim()).OrderByDescending(a => a.ModifiedDate).ToList();
                    else
                        applicationsList = dataContext.CONApplicationDetails.Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate && a.LECRegistrationNumber.Trim() == LECRegistrationNumber.Trim()).OrderByDescending(a => a.ModifiedDate).ToList();

                    if (!string.IsNullOrEmpty(status))
                    {
                        applicationsList = applicationsList.Where(a => a.ApplicationStatusCode == status).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetApplications: " + ex.Message, this);
            }
            return applicationsList;
        }

        public bool ChangeStatusToSave(CONApplicationDetail application)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    CONApplicationDetail obj = dataContext.CONApplicationDetails.Where(a => a.Id == application.Id).FirstOrDefault();
                    if (obj == null)
                        return false;
                    else
                    {
                        obj.ApplicationStatusCode = "1";
                        obj.ApplicationStatusName = "Saved";
                        dataContext.SubmitChanges();

                        //update status in SAP as well
                        bool isUpdated = ApplicationStatusUpdateinSAP(obj);

                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at ChangeStatusToSave: " + ex.Message, this);
                return false;
            }
        }

        public bool ApplicationStatusUpdateinSAP(CONApplicationDetail app)
        {
            Log.Info("CON ApplicationStatusUpdateinSAP" + app.AccountNumber, this);

            try
            {
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                string userid = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserID].Value; //"Tmservice";
                string password = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserPassword].Value; // "pass@123";
                string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.EncryptionKey].Value; // "@Aeml#2020";
                clsEncryptAES objcrypt = new clsEncryptAES(EncryptionKey);

                SapPiService.ChangeOfNameUpdateData.ChnUpdateSoapClient objChnUpdateClient = new SapPiService.ChangeOfNameUpdateData.ChnUpdateSoapClient();
                SapPiService.ChangeOfNameUpdateData.AuthHeader header = new SapPiService.ChangeOfNameUpdateData.AuthHeader();
                header.Username = objcrypt.EncryptText(userid);
                header.Password = objcrypt.EncryptText(password);

                string ZALTYP, ApplicationtType, LOGIN_SRNO, CANo, MeterNO, Premisetypevalue, SecurityDeposit, COntinueExitingSDvalue,
        NAME_ORG, TITLEvalue, FirstName, midlename, lastname, workcenter, diffrentbillingaddressvalue, PresentFlatNO,
        PresentBuildingname, PresentStreet, PresentLandmark, PresentSuberb, PresentPinCode, rentedpremaiseValue, RentOwnername, RentLandLine,
        RentMobile, RentEmail, Mobile, LandLine, Email, BillLangvalue, SD_EXISTING_AMOUNT, ConsumerRemark,
        Contract, APPLICANT_TYPE, ZFLAG_EBILL, ZFLAG_EP_BILL, HOUSE_NO_APP, BUILDING_NAME_APP, LANE_STREET_APP, LANDMARK_APP,
        SUBURB_APP, AKLASSE, APPLICATION_STATUS, PINCODE_APP, ZCITY1_B, CITY_APP, ZAPPLBY, CREATED_ON,
        REGSRNO, TEMPREGSRNO = "", LECId, LECContactDetails;

                #region SAP update
                ApplicationtType = objcrypt.EncryptText(app.ApplicationTypeId);
                ZALTYP = objcrypt.EncryptText("CHN");
                APPLICANT_TYPE = objcrypt.EncryptText(app.ApplicantTypeCode);
                LOGIN_SRNO = objcrypt.EncryptText(null);
                CANo = objcrypt.EncryptText(app.AccountNumber);
                MeterNO = objcrypt.EncryptText(app.MeterNumber.TrimStart('0'));
                Premisetypevalue = objcrypt.EncryptText(app.PremiseTypeCode);
                SecurityDeposit = objcrypt.EncryptText(app.SecurityDepositAmount.ToString());
                COntinueExitingSDvalue = objcrypt.EncryptText(app.IsContinueExitingSDvalueOpt == true ? "1" : "2");
                NAME_ORG = objcrypt.EncryptText(app.OrgName);
                TITLEvalue = objcrypt.EncryptText(app.TitleValue);
                if (app.ApplicantTypeCode == "2")
                {
                    FirstName = objcrypt.EncryptText(app.FirstName);
                    midlename = objcrypt.EncryptText("&");
                    lastname = objcrypt.EncryptText(app.LastName);
                }
                else
                {
                    FirstName = objcrypt.EncryptText(app.FirstName);
                    midlename = objcrypt.EncryptText(app.MiddleName);
                    lastname = objcrypt.EncryptText(app.LastName);
                }
                workcenter = objcrypt.EncryptText(app.WorkCenterName);
                diffrentbillingaddressvalue = objcrypt.EncryptText(app.IsDiffrentBillingAddressValueOpt == true ? "1" : "2");

                PresentFlatNO = objcrypt.EncryptText(app.BillingHouseNumber);
                PresentBuildingname = objcrypt.EncryptText(app.BillingBuildingName);
                PresentStreet = objcrypt.EncryptText(app.BillingStreet);
                PresentLandmark = objcrypt.EncryptText(app.BillingLandmark);
                PresentSuberb = objcrypt.EncryptText(app.BillingSuburb);
                PresentPinCode = objcrypt.EncryptText(app.BillingPincode);

                rentedpremaiseValue = objcrypt.EncryptText(app.IsRentedProperty == true ? "1" : "2");
                RentOwnername = objcrypt.EncryptText(app.Rented_Ownername);
                RentLandLine = objcrypt.EncryptText(app.Rented_Owner_Landline);
                RentMobile = objcrypt.EncryptText(app.Rented_Owner_Mobile);
                RentEmail = objcrypt.EncryptText(app.Rented_Owner_Email);
                Mobile = objcrypt.EncryptText(app.MobileNumber);
                LandLine = objcrypt.EncryptText(app.LandlineNumber);
                Email = objcrypt.EncryptText(app.EmailAddress);
                BillLangvalue = objcrypt.EncryptText(app.BillingLanguage);
                SD_EXISTING_AMOUNT = objcrypt.EncryptText(app.ExistingSecurityDepositAmount.ToString());
                ConsumerRemark = objcrypt.EncryptText(app.ConsumerRemarks);
                Contract = objcrypt.EncryptText(app.ConsumerContractVertrag);
                ZFLAG_EBILL = objcrypt.EncryptText(app.PaperlessBillingFlag == true ? "1" : "0");
                ZFLAG_EP_BILL = objcrypt.EncryptText(app.EbillPhysicalFlag == true ? "1" : "0");

                HOUSE_NO_APP = objcrypt.EncryptText(app.HouseNumber);
                BUILDING_NAME_APP = objcrypt.EncryptText(app.StreetName);
                LANE_STREET_APP = objcrypt.EncryptText(app.Area);
                LANDMARK_APP = objcrypt.EncryptText(app.Landmark);
                SUBURB_APP = objcrypt.EncryptText(app.Suburb);

                AKLASSE = objcrypt.EncryptText(app.AKLASSE);
                APPLICATION_STATUS = objcrypt.EncryptText(app.ApplicationStatusCode);

                PINCODE_APP = objcrypt.EncryptText(app.Pincode);
                ZCITY1_B = objcrypt.EncryptText(app.BillingCity);
                CITY_APP = objcrypt.EncryptText(app.City);

                ZAPPLBY = objcrypt.EncryptText(app.ZAPPLBY);

                CREATED_ON = objcrypt.EncryptText(app.CreatedDate.GetValueOrDefault().ToString("dd-MMM-yyyy"));

                REGSRNO = objcrypt.EncryptText(app.RegistrationSerialNumber);
                TEMPREGSRNO = objcrypt.EncryptText(app.TempRegistrationSerialNumber);

                LECId = objcrypt.EncryptText(app.LECRegistrationNumber);
                LECContactDetails = objcrypt.EncryptText(app.LECMobileNumber);

                string[] output = objChnUpdateClient.Post_Data(header, ApplicationtType, LOGIN_SRNO, CANo, MeterNO, Premisetypevalue, SecurityDeposit,
                COntinueExitingSDvalue, NAME_ORG, TITLEvalue, FirstName, midlename, lastname,
                workcenter, diffrentbillingaddressvalue, PresentFlatNO, PresentBuildingname, PresentStreet, PresentLandmark,
                PresentSuberb, PresentPinCode, rentedpremaiseValue, RentOwnername, RentLandLine, RentMobile,
                RentEmail, Mobile, LandLine, Email, BillLangvalue, SD_EXISTING_AMOUNT,
                ConsumerRemark, Contract, APPLICANT_TYPE, ZFLAG_EBILL, ZFLAG_EP_BILL, HOUSE_NO_APP,
                BUILDING_NAME_APP, LANE_STREET_APP, LANDMARK_APP, SUBURB_APP, PINCODE_APP, AKLASSE,
                APPLICATION_STATUS, ZALTYP, ZAPPLBY, ZCITY1_B, CITY_APP, CREATED_ON,
                REGSRNO, TEMPREGSRNO, LECId, LECContactDetails);

                string outputdata = "";
                outputdata = outputdata + "flag : " + objcrypt.DecryptText(output[0].ToString()) + ",  ";
                outputdata = outputdata + "message : " + objcrypt.DecryptText(output[1].ToString()) + " , ";
                outputdata = outputdata + "exception :" + objcrypt.DecryptText(output[2].ToString()) + ",  ";
                outputdata = outputdata + "TEMPREGSRNO :" + objcrypt.DecryptText(output[3].ToString()) + ",  ";
                outputdata = outputdata + "REGSRNO : " + objcrypt.DecryptText(output[4].ToString()) + "  ";
                //lblMsg.Text = outputdata;
                string flag = objcrypt.DecryptText(output[0].ToString());
                string message = objcrypt.DecryptText(output[1].ToString());
                string exception = objcrypt.DecryptText(output[2].ToString());

                Log.Info("CON existing application SAP status updated for CA:" + app.AccountNumber + ", Result:" + outputdata, this);
                return true;
                #endregion
            }
            catch (Exception ex)
            {
                Log.Error("CON error in existing application SAP status updated for CA:" + app.AccountNumber + ", Exception:" + ex.Message, this);
                return false;
            }
        }

        public List<CONApplicationDocumentDetail> GetExistingDocument(string registrationNumber)
        {
            try
            {
                List<CONApplicationDocumentDetail> docList = new List<CONApplicationDocumentDetail>();
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {

                    if (dataContext.CONApplicationDocumentDetails.Any(a => a.RegistrationSerialNumber == registrationNumber))
                    {
                        docList = dataContext.CONApplicationDocumentDetails.Where(a => a.RegistrationSerialNumber == registrationNumber).ToList();
                        return docList;
                    }
                    else
                        return docList;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetExistingDocument: " + ex.Message, this);
            }
            return null;
        }

        public bool IsApplicationExistsForTempRegisterByLEC(string registrationNumber)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CONApplicationDetails.Any(a => a.TempRegistrationSerialNumber == registrationNumber && a.LECRegistrationNumber == UserSession.AEMLCONLECUserSessionContext.RegistrationNumber))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at IsApplicationExistsForCA: " + ex.Message, this);
            }
            return false;
        }

        public bool IsApplicationExistsForTempRegister(string registrationNumber)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CONApplicationDetails.Any(a => a.TempRegistrationSerialNumber == registrationNumber))
                        return true;
                    else
                        return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at IsApplicationExistsForCA: " + ex.Message, this);
            }
            return false;
        }

        public List<CONPremiseTypeMaster> GetPremiseTypeList()
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    return dataContext.CONPremiseTypeMasters.OrderBy(p => p.DisplayOrder).ToList();
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at GetPremiseTypeList: " + ex.Message, this);
            }
            return null;
        }

        public bool SaveDocumentInOracle(CONApplicationDetail application, string fileName, byte[] bytes, CONDocumentMaster doc, string textDocNumber)
        {
            try
            {
                Regex reg = new Regex("[*'\",_&#^@$%]");
                fileName = reg.Replace(fileName, string.Empty);
                fileName = fileName.Replace(" ", string.Empty);

                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                string userid = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserID].Value; //"Tmservice";
                string password = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserPassword].Value; // "pass@123";
                string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.EncryptionKey].Value; // "@Aeml#2020";
                clsEncryptAES objcrypt = new clsEncryptAES(EncryptionKey);

                SapPiService.ChangeOfNameUpdateData.ChnUpdateSoapClient objChnUpdateClient = new SapPiService.ChangeOfNameUpdateData.ChnUpdateSoapClient();
                SapPiService.ChangeOfNameUpdateData.AuthHeader header = new SapPiService.ChangeOfNameUpdateData.AuthHeader();
                header.Username = objcrypt.EncryptText(userid);
                header.Password = objcrypt.EncryptText(password);

                var output = objChnUpdateClient.Post_Document_Data(header,
                    objcrypt.EncryptText(application.RegistrationSerialNumber),
                    objcrypt.EncryptText(doc.DocumentSerialNumber.ToString()),
                    objcrypt.EncryptText(doc.DocumentDescription),
                    objcrypt.EncryptText(fileName),
                    objcrypt.EncryptFile(bytes),
                    objcrypt.EncryptText("CHN"),
                    objcrypt.EncryptText(textDocNumber));            //textbox value

                string outputdata = "";
                outputdata = outputdata + "flag : " + objcrypt.DecryptText(output[0].ToString()) + ",  ";
                outputdata = outputdata + "message : " + objcrypt.DecryptText(output[1].ToString()) + " , ";
                outputdata = outputdata + "exception :" + objcrypt.DecryptText(output[2].ToString()) + ",  ";

                string flag = objcrypt.DecryptText(output[0].ToString());
                string message = objcrypt.DecryptText(output[1].ToString());
                string exception = objcrypt.DecryptText(output[2].ToString());

                Log.Info("CON SaveDocumentInOracle result:" + application.AccountNumber + doc.DocumentTypeCode + doc.DocumentSerialNumber + ", service update:" + flag + "," + message + "," + exception, this);

                if (flag == "1") return true;
                else return false;
            }
            catch (Exception e)
            {
                Log.Error("CON SaveDocumentInOracle error:" + application.AccountNumber + doc.DocumentTypeCode + doc.DocumentSerialNumber + " Detail:" + e.Message, this);
                return false;
            }
        }

        public SaveApplicationResult SaveApplication(ChangeOfNameApplicationFromModel model, bool isSubmit, bool isLEC)
        {
            Log.Info("CON Save application start for Account number" + model.AccountNo + ",Submit:" + isSubmit, this);
            model.AccountNo = FormatAccountNumber(model.AccountNo);
            model.FirstName = string.IsNullOrEmpty(model.FirstName) ? model.FirstName : model.FirstName.ToUpper();
            model.MiddleName = string.IsNullOrEmpty(model.MiddleName) ? model.MiddleName : model.MiddleName.ToUpper();
            model.LastName = string.IsNullOrEmpty(model.LastName) ? model.LastName : model.LastName.ToUpper();
            model.Name1Joint = string.IsNullOrEmpty(model.Name1Joint) ? model.Name1Joint : model.Name1Joint.ToUpper();
            model.Name2Joint = string.IsNullOrEmpty(model.Name2Joint) ? model.Name2Joint : model.Name2Joint.ToUpper();
            model.OrganizationName = string.IsNullOrEmpty(model.OrganizationName) ? model.OrganizationName : model.OrganizationName.ToUpper();

            model.HouseNumber = string.IsNullOrEmpty(model.HouseNumber) ? model.HouseNumber : model.HouseNumber.ToUpper();
            model.Landmark = string.IsNullOrEmpty(model.Landmark) ? model.Landmark : model.Landmark.ToUpper();
            model.Area = string.IsNullOrEmpty(model.Area) ? model.Area : model.Area.ToUpper();
            model.Street = string.IsNullOrEmpty(model.Street) ? model.Street : model.Street.ToUpper();
            model.BuildingName = string.IsNullOrEmpty(model.BuildingName) ? model.BuildingName : model.BuildingName.ToUpper();

            Log.Info("CON Save application values:" + model.AccountNo + ", " + model.FirstName + ", " + model.MiddleName + ", " + model.LastName + ", " + model.OrganizationName, this);
            Log.Info("CON Save application values:" + model.AccountNo + ", " + model.HouseNumber + ", " + model.Landmark + ", " + model.Area + ", " + model.Street + ", " + model.BuildingName, this);
            SaveApplicationResult result = new SaveApplicationResult();
            try
            {
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                string userid = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserID].Value; //"Tmservice";
                string password = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserPassword].Value; // "pass@123";
                string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.EncryptionKey].Value; // "@Aeml#2020";
                clsEncryptAES objcrypt = new clsEncryptAES(EncryptionKey);
                string tempRegistrationNumbe = "";
                string registrationNumber = ""; ;
                var titleName = string.Empty;
                var premiseTypeName = string.Empty;
                switch (model.SelectedTitle)
                {
                    case "0001":
                        titleName = "Ms.";
                        break;
                    case "0002":
                        titleName = "Mr.";
                        break;
                    case "0006":
                        titleName = "M/S";
                        break;
                    default:
                        titleName = null;
                        break;
                }
                decimal existingSDAmount = SapPiService.Services.RequestHandler.GetSecurityDeposityAmountCON(model.AccountNo);
                CAValidateInfo caInfo_workcenter = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.AccountNo);

                var areaList = ListAreaPinWorkcenterMapping();
                var selectedAreaMapping = areaList.Where(a => a.Area == model.SelectedSuburb && a.City == model.SelectedCity && a.PinCode == model.SelectedPincode).FirstOrDefault();

                var consumerDetails = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNo);
                if (selectedAreaMapping == null)
                {

                    selectedAreaMapping = areaList.Where(a => a.PinCode == consumerDetails.PinCode).FirstOrDefault();
                }

                var isRentalPremise = "No";
                if (model.IsRentalProperty == "Yes" || model.SelectedPremiseType == "034")
                    isRentalPremise = "Yes";

                SapPiService.ChangeOfNameUpdateData.ChnUpdateSoapClient objChnUpdateClient = new SapPiService.ChangeOfNameUpdateData.ChnUpdateSoapClient();
                SapPiService.ChangeOfNameUpdateData.AuthHeader header = new SapPiService.ChangeOfNameUpdateData.AuthHeader();
                header.Username = objcrypt.EncryptText(userid);
                header.Password = objcrypt.EncryptText(password);

                string ZALTYP, ApplicationtType, LOGIN_SRNO, CANo, MeterNO, Premisetypevalue, SecurityDeposit, COntinueExitingSDvalue,
        NAME_ORG, TITLEvalue, FirstName, midlename, lastname, workcenter, diffrentbillingaddressvalue, PresentFlatNO,
        PresentBuildingname, PresentStreet, PresentLandmark, PresentSuberb, PresentPinCode, rentedpremaiseValue, RentOwnername, RentLandLine,
        RentMobile, RentEmail, Mobile, LandLine, Email, BillLangvalue, SD_EXISTING_AMOUNT, ConsumerRemark,
        Contract, APPLICANT_TYPE, ZFLAG_EBILL, ZFLAG_EP_BILL, HOUSE_NO_APP, BUILDING_NAME_APP, LANE_STREET_APP, LANDMARK_APP,
        SUBURB_APP, AKLASSE, APPLICATION_STATUS, PINCODE_APP, ZCITY1_B, CITY_APP, ZAPPLBY, CREATED_ON,
        REGSRNO, TEMPREGSRNO = "", LECId, LECContactDetails;

                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    if (dataContext.CONPremiseTypeMasters.Where(p => p.PremiseTypeCode == model.SelectedPremiseType).Any())
                    {
                        premiseTypeName = dataContext.CONPremiseTypeMasters.Where(p => p.PremiseTypeCode == model.SelectedPremiseType).FirstOrDefault().PremiseTypeName;
                    }
                    if (IsApplicationExistsForCARegistration(model.AccountNo))
                    {
                        Log.Info("CON existing application for CA" + model.AccountNo, this);
                        CONApplicationDetail existingapplication = dataContext.CONApplicationDetails.FirstOrDefault(a => a.AccountNumber == FormatAccountNumber(model.AccountNo) && a.ApplicationStatusCode != "4");
                        try
                        {
                            #region SAP update
                            ApplicationtType = objcrypt.EncryptText(model.IsStillLiving == "Yes" ? "21" : "22");
                            ZALTYP = objcrypt.EncryptText("CHN");
                            APPLICANT_TYPE = objcrypt.EncryptText(model.ApplicantType);
                            LOGIN_SRNO = objcrypt.EncryptText(null);
                            CANo = objcrypt.EncryptText(model.AccountNo);
                            MeterNO = objcrypt.EncryptText(consumerDetails.MeterNumber.TrimStart('0'));
                            Premisetypevalue = objcrypt.EncryptText(model.SelectedPremiseType);
                            SecurityDeposit = objcrypt.EncryptText(model.SecurityDepositeAmount.ToString());
                            COntinueExitingSDvalue = objcrypt.EncryptText(model.IsContinueWithExistingSD == "Yes" ? "1" : "2");
                            NAME_ORG = objcrypt.EncryptText(model.OrganizationName);
                            TITLEvalue = objcrypt.EncryptText(model.SelectedTitle);
                            if (model.ApplicantType == "2")
                            {
                                FirstName = objcrypt.EncryptText(model.Name1Joint);
                                midlename = objcrypt.EncryptText("&");
                                lastname = objcrypt.EncryptText(model.Name2Joint);
                            }
                            else
                            {
                                FirstName = objcrypt.EncryptText(model.FirstName);
                                midlename = objcrypt.EncryptText(model.MiddleName);
                                lastname = objcrypt.EncryptText(model.LastName);
                            }
                            workcenter = objcrypt.EncryptText(caInfo_workcenter.VAPLZ_WORK_CENTER);// objcrypt.EncryptText(selectedAreaMapping == null ? null : selectedAreaMapping.WorkCenter);
                            diffrentbillingaddressvalue = objcrypt.EncryptText(model.IsBillingAddressDifferent == "Yes" ? "1" : "2");

                            PresentFlatNO = objcrypt.EncryptText(model.BillingHouseNumber);
                            PresentBuildingname = objcrypt.EncryptText(model.BillingBuildingName);
                            PresentStreet = objcrypt.EncryptText(model.BillingStreet);
                            PresentLandmark = objcrypt.EncryptText(model.BillingLandmark);
                            PresentSuberb = objcrypt.EncryptText(model.BillingSelectedSuburb);
                            PresentPinCode = objcrypt.EncryptText(model.BillingSelectedPincode);

                            rentedpremaiseValue = objcrypt.EncryptText(isRentalPremise == "Yes" ? "1" : "2");
                            RentOwnername = objcrypt.EncryptText(model.LandlordName);
                            RentLandLine = objcrypt.EncryptText(model.LandlordLandline);
                            RentMobile = objcrypt.EncryptText(model.LandlordMobile);
                            RentEmail = objcrypt.EncryptText(model.LandlordEmail);
                            Mobile = objcrypt.EncryptText(model.MobileNo);
                            LandLine = objcrypt.EncryptText(model.Landline);
                            Email = objcrypt.EncryptText(model.EmailId);
                            BillLangvalue = objcrypt.EncryptText(model.SelectedBillLanguage);
                            SD_EXISTING_AMOUNT = objcrypt.EncryptText(existingSDAmount.ToString());
                            ConsumerRemark = objcrypt.EncryptText(model.ConsumerRemark);
                            Contract = objcrypt.EncryptText(existingapplication.ConsumerContractVertrag);
                            APPLICANT_TYPE = objcrypt.EncryptText(model.ApplicantType);
                            ZFLAG_EBILL = objcrypt.EncryptText(model.IsPaperlessBilling == "Yes" ? "1" : "0");
                            ZFLAG_EP_BILL = objcrypt.EncryptText(model.IsPaperlessBilling == "No" ? "1" : "0");

                            HOUSE_NO_APP = objcrypt.EncryptText(model.HouseNumber);
                            BUILDING_NAME_APP = objcrypt.EncryptText(model.Street);
                            LANE_STREET_APP = objcrypt.EncryptText(model.Area);
                            LANDMARK_APP = objcrypt.EncryptText(model.Landmark);
                            SUBURB_APP = objcrypt.EncryptText(model.SelectedSuburb);

                            AKLASSE = objcrypt.EncryptText(existingapplication.AKLASSE);
                            APPLICATION_STATUS = objcrypt.EncryptText(isSubmit == true ? "2" : "1");

                            PINCODE_APP = objcrypt.EncryptText(model.SelectedPincode);
                            ZCITY1_B = objcrypt.EncryptText(model.BillingSelectedCity);
                            CITY_APP = objcrypt.EncryptText(model.SelectedCity);

                            ZAPPLBY = isLEC ? objcrypt.EncryptText("12") : objcrypt.EncryptText("11");

                            CREATED_ON = objcrypt.EncryptText(DateTime.Now.ToString("dd-MMM-yyyy"));

                            REGSRNO = objcrypt.EncryptText(existingapplication.RegistrationSerialNumber);
                            TEMPREGSRNO = objcrypt.EncryptText(existingapplication.TempRegistrationSerialNumber);

                            LECId = objcrypt.EncryptText(existingapplication.LECRegistrationNumber);
                            LECContactDetails = objcrypt.EncryptText(existingapplication.LECMobileNumber);

                            string[] output = objChnUpdateClient.Post_Data(header, ApplicationtType, LOGIN_SRNO, CANo, MeterNO, Premisetypevalue, SecurityDeposit,
                            COntinueExitingSDvalue, NAME_ORG, TITLEvalue, FirstName, midlename, lastname,
                            workcenter, diffrentbillingaddressvalue, PresentFlatNO, PresentBuildingname, PresentStreet, PresentLandmark,
                            PresentSuberb, PresentPinCode, rentedpremaiseValue, RentOwnername, RentLandLine, RentMobile,
                            RentEmail, Mobile, LandLine, Email, BillLangvalue, SD_EXISTING_AMOUNT,
                            ConsumerRemark, Contract, APPLICANT_TYPE, ZFLAG_EBILL, ZFLAG_EP_BILL, HOUSE_NO_APP,
                            BUILDING_NAME_APP, LANE_STREET_APP, LANDMARK_APP, SUBURB_APP, PINCODE_APP, AKLASSE,
                            APPLICATION_STATUS, ZALTYP, ZAPPLBY, ZCITY1_B, CITY_APP, CREATED_ON,
                            REGSRNO, TEMPREGSRNO, LECId, LECContactDetails);

                            string outputdata = "";
                            outputdata = outputdata + "flag : " + objcrypt.DecryptText(output[0].ToString()) + ",  ";
                            outputdata = outputdata + "message : " + objcrypt.DecryptText(output[1].ToString()) + " , ";
                            outputdata = outputdata + "exception :" + objcrypt.DecryptText(output[2].ToString()) + ",  ";
                            outputdata = outputdata + "TEMPREGSRNO :" + objcrypt.DecryptText(output[3].ToString()) + ",  ";
                            outputdata = outputdata + "REGSRNO : " + objcrypt.DecryptText(output[4].ToString()) + "  ";

                            string flag = objcrypt.DecryptText(output[0].ToString());
                            string message = objcrypt.DecryptText(output[1].ToString());
                            string exception = objcrypt.DecryptText(output[2].ToString());
                            tempRegistrationNumbe = objcrypt.DecryptText(output[3].ToString());
                            registrationNumber = objcrypt.DecryptText(output[4].ToString());
                            Log.Info("CON existing application SAP for CA:" + model.AccountNo + ", Result:" + flag + ", " + message + ", " + exception + ", " + tempRegistrationNumbe + ", " + registrationNumber, this);
                            if (flag == "1" && !string.IsNullOrEmpty(tempRegistrationNumbe))
                            {
                                result.IsSavedFromService = true;
                            }
                            else
                            {
                                result.IsSavedFromService = false;
                                result.IsSavedInDatabase = false;
                                result.Message = message + exception;
                            }

                            //lblMsg.Text = outputdata;
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Log.Error("CON save existing application - Error in calling SAP/Oracle service existing request:" + ex.Message, this);
                            result.IsSavedFromService = false;
                            result.IsSavedInDatabase = false;
                            result.Message = ex.Message;
                        }

                        if (result.IsSavedFromService == true)
                        {
                            Log.Info("CON existing application saved in SAP now saving in DB for " + model.AccountNo, this);
                            existingapplication.WorkCenterName = caInfo_workcenter.VAPLZ_WORK_CENTER;// selectedAreaMapping == null ? null : selectedAreaMapping.WorkCenter;
                            existingapplication.ApplicantType = model.ApplicantType;
                            existingapplication.ApplicantTypeCode = model.ApplicantType;
                            existingapplication.ApplicationStatusCode = (isSubmit == true ? "2" : "1");                    //to do 1:Saved ad Draft;2:Submitted;3:Approved;
                            existingapplication.ApplicationStatusName = (isSubmit == true ? "Submitted" : "Saved");                    //to do
                            existingapplication.ApplicationStatusUpdateDate = DateTime.Now;
                            existingapplication.ApplicationstatusUpdatedBy = "User";
                            existingapplication.ApplicationTypeId = model.IsStillLiving == "Yes" ? "21" : "22";
                            existingapplication.ApplicationTypeName = "General";
                            existingapplication.Area_New = model.Area;
                            existingapplication.BillingLanguage = model.SelectedBillLanguage;
                            existingapplication.Buildingname_New = model.BuildingName;
                            existingapplication.BillingHouseNumber = model.BillingHouseNumber;
                            existingapplication.BillingBuildingName = model.BillingBuildingName;
                            existingapplication.BillingStreet = model.BillingStreet;
                            existingapplication.BillingLandmark = model.BillingLandmark;
                            existingapplication.BillingSuburb = model.BillingSelectedSuburb;
                            existingapplication.BillingCity = model.BillingSelectedCity;
                            existingapplication.BillingPincode = model.BillingSelectedPincode;
                            existingapplication.City_New = model.SelectedCity;
                            existingapplication.Pincode_New = model.SelectedPincode;
                            existingapplication.ConsumerRemarks = model.ConsumerRemark;
                            existingapplication.EbillPhysicalFlag = model.IsPaperlessBilling == "Yes" ? false : true;
                            existingapplication.EmailAddress = model.EmailId;
                            existingapplication.ExistingSecurityDepositAmount = existingSDAmount.ToString();
                            if (model.ApplicantType == "2")
                            {
                                existingapplication.FirstName = model.Name1Joint;
                                existingapplication.MiddleName = "&";
                                existingapplication.LastName = model.Name2Joint;
                            }
                            else
                            {
                                existingapplication.FirstName = model.FirstName;
                                existingapplication.MiddleName = model.MiddleName;
                                existingapplication.LastName = model.LastName;
                            }
                            existingapplication.Housenumber_New = model.HouseNumber;
                            existingapplication.IsContinueExitingSDvalueOpt = model.IsContinueWithExistingSD == "Yes" ? true : false;
                            existingapplication.IsDiffrentBillingAddressValueOpt = model.IsBillingAddressDifferent == "Yes" ? true : false;
                            existingapplication.IsRentedProperty = isRentalPremise == "Yes" ? true : false;
                            existingapplication.LandlineNumber = model.Landline;
                            existingapplication.Landmark_New = model.Landmark;

                            existingapplication.MeterNumber = consumerDetails.MeterNumber.TrimStart('0');
                            existingapplication.IsAddressChangeRequired = model.IsAddressCorrectionRequired == "Yes" ? true : false;
                            existingapplication.MobileNumber = model.MobileNo;
                            existingapplication.ModifiedBy = "User";
                            existingapplication.ModifiedDate = DateTime.Now;
                            existingapplication.NumberOfVersion = existingapplication.NumberOfVersion + 1;
                            existingapplication.OrgName = model.OrganizationName;
                            existingapplication.PaperlessBillingFlag = model.IsPaperlessBilling == "Yes" ? true : false;
                            existingapplication.PremiseTypeCode = model.SelectedPremiseType;
                            existingapplication.PremiseTypeName = premiseTypeName;
                            existingapplication.RegistrationSerialNumber = registrationNumber;                      // to be genrated when calling service to save details in Oracle DB
                            existingapplication.Rented_Ownername = model.LandlordName;
                            existingapplication.Rented_Owner_Email = model.LandlordEmail;
                            existingapplication.Rented_Owner_Landline = model.LandlordLandline;
                            existingapplication.Rented_Owner_Mobile = model.LandlordMobile;
                            existingapplication.SecurityDepositAmount = model.SecurityDepositeAmount.ToString();
                            existingapplication.Streetname_New = model.Street;
                            existingapplication.Suburb_New = model.SelectedSuburb;
                            existingapplication.TempRegistrationSerialNumber = tempRegistrationNumbe;                  // to be generated when calling service to save details in Oracle DB
                            existingapplication.TitleName = titleName;
                            existingapplication.TitleValue = model.SelectedTitle;
                            dataContext.SubmitChanges();
                            result.IsSavedInDatabase = true;
                        }
                    }
                    else
                    {
                        Log.Info("CON new application for " + model.AccountNo, this);
                        var consumerDetailsForCA = SapPiService.Services.RequestHandler.FetchConsumerDetails(model.AccountNo);
                        var accountDetails = SapPiService.Services.RequestHandler.ValidateCAForChangeOfName(model.AccountNo);

                        try
                        {
                            #region Save in DB
                            CONApplicationDetail newRequest = new CONApplicationDetail
                            {
                                Id = Guid.NewGuid(),
                                LECMobileNumber = model.LECMobileNumber,
                                LECRegistrationNumber = model.LECRegistrationNumber,
                                AccountNumber = FormatAccountNumber(model.AccountNo),
                                AKLASSE = accountDetails.AKLASSE,
                                ApplicantType = model.ApplicantType,
                                ApplicantTypeCode = model.ApplicantType,
                                ApplicationRejectionReason = null,               //N/A
                                ApplicationStatusCode = isSubmit == true ? "2" : "1",                    //to do 1:Saved ad Draft,2:Submitted,3:Approved,
                                ApplicationStatusName = isSubmit == true ? "Submitted" : "Saved",                    //to do
                                ApplicationStatusUpdateDate = DateTime.Now,
                                ApplicationstatusUpdatedBy = "User",
                                ApplicationStatusUpdateEmpRemarks = null,         //N/A
                                ApplicationTypeId = model.IsStillLiving == "Yes" ? "21" : "22",
                                ApplicationTypeName = "General",
                                Area = consumerDetailsForCA.Street2,
                                Area_New = model.Area,
                                BillingLanguage = model.SelectedBillLanguage,
                                BuildingName = consumerDetailsForCA.Street2,
                                Buildingname_New = model.BuildingName,
                                City = consumerDetailsForCA.City,
                                City_New = model.SelectedCity,
                                ConsumerContractVertrag = consumerDetailsForCA.Vertrag_Contract,
                                ConsumerRemarks = model.ConsumerRemark,
                                CreatedBy = "User",
                                CreatedDate = DateTime.Now,
                                EbillPhysicalFlag = model.IsPaperlessBilling == "Yes" ? false : true,
                                EmailAddress = model.EmailId,
                                ExistingSecurityDepositAmount = existingSDAmount.ToString(),
                                FirstName = model.ApplicantType == "2" ? model.Name1Joint : model.FirstName,
                                HouseNumber = consumerDetailsForCA.HouseNumber,
                                Housenumber_New = model.HouseNumber,
                                IsContinueExitingSDvalueOpt = model.IsContinueWithExistingSD == "Yes" ? true : false,
                                IsDiffrentBillingAddressValueOpt = model.IsBillingAddressDifferent == "Yes" ? true : false,
                                IsRentedProperty = model.IsRentalProperty == "Yes" ? true : false,
                                LandlineNumber = model.Landline,
                                Landmark = consumerDetailsForCA.Street3,
                                Landmark_New = model.Landmark,
                                LastName = model.ApplicantType == "2" ? model.Name2Joint : model.LastName,
                                MeterNumber = consumerDetailsForCA.MeterNumber.TrimStart('0'),
                                MiddleName = model.ApplicantType == "2" ? "&" : model.MiddleName,
                                MobileNumber = model.MobileNo,
                                ModifiedBy = "User",
                                ModifiedDate = DateTime.Now,
                                NewAccountNumber = null,           //N/A
                                NumberOfVersion = 1,
                                OrgName = model.OrganizationName,
                                PaperlessBillingFlag = model.IsPaperlessBilling == "Yes" ? true : false,
                                IsAddressChangeRequired = model.IsAddressCorrectionRequired == "Yes" ? true : false,
                                Pincode = consumerDetailsForCA.PinCode,
                                Pincode_New = model.SelectedPincode,
                                PremiseTypeCode = model.SelectedPremiseType,
                                PremiseTypeName = premiseTypeName,
                                RegistrationSerialNumber = registrationNumber,                      // to be genrated when calling service to save details in Oracle DB
                                Rented_Ownername = model.LandlordName,
                                Rented_Owner_Email = model.LandlordEmail,
                                Rented_Owner_Landline = model.LandlordLandline,
                                Rented_Owner_Mobile = model.LandlordMobile,
                                SecurityDepositAmount = model.SecurityDepositeAmount.ToString(),
                                StreetName = consumerDetailsForCA.Street,
                                Streetname_New = model.Street,
                                Suburb = consumerDetailsForCA.Street.Length > 40 ? consumerDetailsForCA.Street.Substring(0, 40) : consumerDetailsForCA.Street,
                                Suburb_New = model.SelectedSuburb,
                                TempRegistrationSerialNumber = tempRegistrationNumbe,                  // to be generated when calling service to save details in Oracle DB
                                TitleName = titleName,
                                TitleValue = model.SelectedTitle,
                                WorkCenterName = caInfo_workcenter.VAPLZ_WORK_CENTER,// selectedAreaMapping == null ? null : selectedAreaMapping.WorkCenter,
                                ZALTYP = "CHN",
                                ZAPPLBY = isLEC ? "12" : "11",//To be kept as CHN for all request - for now
                                                                                                          //ZAPPLBY = "13",                                     //To be kept as 13 for all request - for now
                                BillingHouseNumber = model.BillingHouseNumber,
                                BillingBuildingName = model.BillingBuildingName,
                                BillingStreet = model.BillingStreet,
                                BillingLandmark = model.BillingLandmark,
                                BillingSuburb = model.BillingSelectedSuburb,
                                BillingCity = model.BillingSelectedCity,
                                BillingPincode = model.BillingSelectedPincode,
                            };

                            Log.Info("CON new application save in DB for " + model.AccountNo, this);
                            Log.Info("CON new application values:" + newRequest.AccountNumber + ", " + newRequest.TitleName + ", " + newRequest.ApplicantType + ", " + newRequest.ApplicationStatusName + ", " + newRequest.FirstName + ", " + newRequest.MiddleName + newRequest.LastName + ", " + newRequest.OrgName + ", " + newRequest.PremiseTypeName + ", " + newRequest.HouseNumber + ", " + newRequest.BuildingName + ", " + newRequest.Area + ", " + newRequest.City + ", " + newRequest.Landmark + ", " + newRequest.MobileNumber + ", " + newRequest.Landmark + ", " + newRequest.ConsumerRemarks, this);

                            if (dataContext.CONPremiseTypeMasters.Where(p => p.PremiseTypeCode == model.SelectedPremiseType).Any())
                            {
                                newRequest.PremiseTypeName = dataContext.CONPremiseTypeMasters.Where(p => p.PremiseTypeCode == model.SelectedPremiseType).FirstOrDefault().PremiseTypeName;
                            }
                            dataContext.CONApplicationDetails.InsertOnSubmit(newRequest);
                            dataContext.SubmitChanges();
                            Log.Info("CON new application saved in DB for" + model.AccountNo, this);
                            result.IsSavedInDatabase = true;
                            #endregion
                        }
                        catch (Exception ex)
                        {
                            Log.Error("Error in saving new request in database:" + ex.Message + " details- CA:" + model.AccountNo + ", mob:" + model.MobileNo, this);
                            result.Message = ex.Message;
                            result.IsSavedInDatabase = false;
                            result.IsSavedFromService = false;
                        }
                        if (result.IsSavedInDatabase == true)
                        {
                            Log.Info("CON new application saving in SAP for" + model.AccountNo, this);
                            try
                            {
                                #region SAP update
                                ApplicationtType = objcrypt.EncryptText(model.IsStillLiving == "Yes" ? "21" : "22");
                                ZALTYP = objcrypt.EncryptText("CHN");
                                APPLICANT_TYPE = objcrypt.EncryptText(model.ApplicantType);
                                LOGIN_SRNO = objcrypt.EncryptText(null);
                                CANo = objcrypt.EncryptText(model.AccountNo);
                                MeterNO = objcrypt.EncryptText(consumerDetailsForCA.MeterNumber.TrimStart('0'));
                                Premisetypevalue = objcrypt.EncryptText(model.SelectedPremiseType);
                                SecurityDeposit = objcrypt.EncryptText(model.SecurityDepositeAmount.ToString());
                                COntinueExitingSDvalue = objcrypt.EncryptText(model.IsContinueWithExistingSD == "Yes" ? "1" : "2");
                                NAME_ORG = objcrypt.EncryptText(model.OrganizationName);
                                TITLEvalue = objcrypt.EncryptText(model.SelectedTitle);
                                if (model.ApplicantType == "2")
                                {
                                    FirstName = objcrypt.EncryptText(model.Name1Joint);
                                    midlename = objcrypt.EncryptText("&");
                                    lastname = objcrypt.EncryptText(model.Name2Joint);
                                }
                                else
                                {
                                    FirstName = objcrypt.EncryptText(model.FirstName);
                                    midlename = objcrypt.EncryptText(model.MiddleName);
                                    lastname = objcrypt.EncryptText(model.LastName);
                                }
                                workcenter = objcrypt.EncryptText(caInfo_workcenter.VAPLZ_WORK_CENTER);// objcrypt.EncryptText(selectedAreaMapping == null ? null : selectedAreaMapping.WorkCenter);
                                diffrentbillingaddressvalue = objcrypt.EncryptText(model.IsBillingAddressDifferent == "Yes" ? "1" : "2");

                                PresentFlatNO = objcrypt.EncryptText(model.BillingHouseNumber);
                                PresentBuildingname = objcrypt.EncryptText(model.BillingBuildingName);
                                PresentStreet = objcrypt.EncryptText(model.BillingStreet);
                                PresentLandmark = objcrypt.EncryptText(model.BillingLandmark);
                                PresentSuberb = objcrypt.EncryptText(model.BillingSelectedSuburb);
                                PresentPinCode = objcrypt.EncryptText(model.BillingSelectedPincode);

                                rentedpremaiseValue = objcrypt.EncryptText(model.IsRentalProperty == "Yes" ? "1" : "2");
                                RentOwnername = objcrypt.EncryptText(model.LandlordName);
                                RentLandLine = objcrypt.EncryptText(model.LandlordLandline);
                                RentMobile = objcrypt.EncryptText(model.LandlordMobile);
                                RentEmail = objcrypt.EncryptText(model.LandlordEmail);
                                Mobile = objcrypt.EncryptText(model.MobileNo);
                                LandLine = objcrypt.EncryptText(model.Landline);
                                Email = objcrypt.EncryptText(model.EmailId);
                                BillLangvalue = objcrypt.EncryptText(model.SelectedBillLanguage);
                                SD_EXISTING_AMOUNT = objcrypt.EncryptText(existingSDAmount.ToString());
                                ConsumerRemark = objcrypt.EncryptText(model.ConsumerRemark);
                                Contract = objcrypt.EncryptText(consumerDetailsForCA.Vertrag_Contract);
                                APPLICANT_TYPE = objcrypt.EncryptText(model.ApplicantType);
                                ZFLAG_EBILL = objcrypt.EncryptText(model.IsPaperlessBilling == "Yes" ? "1" : "0");
                                ZFLAG_EP_BILL = objcrypt.EncryptText(model.IsPaperlessBilling == "No" ? "1" : "0");

                                HOUSE_NO_APP = objcrypt.EncryptText(model.HouseNumber);
                                BUILDING_NAME_APP = objcrypt.EncryptText(model.Street);
                                LANE_STREET_APP = objcrypt.EncryptText(model.Area);
                                LANDMARK_APP = objcrypt.EncryptText(model.Landmark);
                                SUBURB_APP = objcrypt.EncryptText(model.SelectedSuburb);

                                AKLASSE = objcrypt.EncryptText(accountDetails.AKLASSE);
                                APPLICATION_STATUS = objcrypt.EncryptText(isSubmit == true ? "2" : "1");

                                PINCODE_APP = objcrypt.EncryptText(model.SelectedPincode);
                                ZCITY1_B = objcrypt.EncryptText(model.BillingSelectedCity);
                                CITY_APP = objcrypt.EncryptText(model.SelectedCity);

                                //ZAPPLBY = objcrypt.EncryptText("11");
                                ZAPPLBY = isLEC ? objcrypt.EncryptText("12") : objcrypt.EncryptText("11");

                                CREATED_ON = objcrypt.EncryptText(DateTime.Now.ToString("dd-MMM-yyyy"));

                                REGSRNO = objcrypt.EncryptText("");
                                TEMPREGSRNO = objcrypt.EncryptText("");

                                LECId = objcrypt.EncryptText(model.LECRegistrationNumber);
                                LECContactDetails = objcrypt.EncryptText(model.LECMobileNumber);

                                string[] output = objChnUpdateClient.Post_Data(header, ApplicationtType, LOGIN_SRNO, CANo, MeterNO, Premisetypevalue, SecurityDeposit,
                                COntinueExitingSDvalue, NAME_ORG, TITLEvalue, FirstName, midlename, lastname,
                                workcenter, diffrentbillingaddressvalue, PresentFlatNO, PresentBuildingname, PresentStreet, PresentLandmark,
                                PresentSuberb, PresentPinCode, rentedpremaiseValue, RentOwnername, RentLandLine, RentMobile,
                                RentEmail, Mobile, LandLine, Email, BillLangvalue, SD_EXISTING_AMOUNT,
                                ConsumerRemark, Contract, APPLICANT_TYPE, ZFLAG_EBILL, ZFLAG_EP_BILL, HOUSE_NO_APP,
                                BUILDING_NAME_APP, LANE_STREET_APP, LANDMARK_APP, SUBURB_APP, PINCODE_APP, AKLASSE,
                                APPLICATION_STATUS, ZALTYP, ZAPPLBY, ZCITY1_B, CITY_APP, CREATED_ON,
                                REGSRNO, TEMPREGSRNO, LECId, LECContactDetails);

                                string outputdata = "";
                                outputdata = outputdata + "flag : " + objcrypt.DecryptText(output[0].ToString()) + ",  ";
                                outputdata = outputdata + "message : " + objcrypt.DecryptText(output[1].ToString()) + " , ";
                                outputdata = outputdata + "exception :" + objcrypt.DecryptText(output[2].ToString()) + ",  ";
                                outputdata = outputdata + "TEMPREGSRNO :" + objcrypt.DecryptText(output[3].ToString()) + ",  ";
                                outputdata = outputdata + "REGSRNO : " + objcrypt.DecryptText(output[4].ToString()) + "  ";
                                //lblMsg.Text = outputdata;
                                string flag = objcrypt.DecryptText(output[0].ToString());
                                string message = objcrypt.DecryptText(output[1].ToString());
                                string exception = objcrypt.DecryptText(output[2].ToString());
                                tempRegistrationNumbe = objcrypt.DecryptText(output[3].ToString());
                                registrationNumber = objcrypt.DecryptText(output[4].ToString());

                                if (flag == "1" && !string.IsNullOrEmpty(tempRegistrationNumbe))
                                {
                                    result.IsSavedFromService = true;
                                }
                                else
                                {
                                    result.IsSavedFromService = false;
                                    result.Message = message + exception;
                                }
                                #endregion
                            }
                            catch (Exception ex)
                            {
                                Log.Error("Error in calling SAP/Oracle service new request:" + ex.Message, this);
                                result.Message = ex.Message;
                                result.IsSavedFromService = false;
                            }
                        }

                        if (result.IsSavedFromService == true)
                        {
                            Log.Info("CON new application saved in SAP for" + model.AccountNo, this);
                            //save registration number
                            CONApplicationDetail newApplication = dataContext.CONApplicationDetails.Where(a => a.AccountNumber == FormatAccountNumber(model.AccountNo) && a.ApplicationStatusCode != "4").OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                            if (newApplication != null)
                            {
                                newApplication.RegistrationSerialNumber = registrationNumber;
                                newApplication.TempRegistrationSerialNumber = tempRegistrationNumbe;
                                dataContext.SubmitChanges();
                            }
                            result.IsSavedInDatabase = true;
                            result.IsSavedFromService = true;
                        }
                        else
                        {
                            Log.Info("CON new application failed for saving in SAP for" + model.AccountNo, this);
                            //delete the entry
                            Log.Error("Error in calling SAP/Oracle service new request deleted entry from database for CA:" + model.AccountNo, this);
                            result.IsSavedInDatabase = false;
                            result.IsSavedFromService = false;

                            CONApplicationDetail newApplication = dataContext.CONApplicationDetails.Where(a => a.AccountNumber == FormatAccountNumber(model.AccountNo) && a.ApplicationStatusCode != "4").OrderByDescending(a => a.CreatedDate).FirstOrDefault();
                            if (newApplication != null)
                            {
                                dataContext.CONApplicationDetails.DeleteOnSubmit(newApplication);
                                dataContext.SubmitChanges();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.IsSavedInDatabase = false;
                result.Message = ex.Message;
                Sitecore.Diagnostics.Log.Error("Error at save application: " + ex.Message, this);
            }
            return result;
        }

        public List<CONDocumentSubTypeMaster> GetDocumentsSubTypes()
        {
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                return dataContext.CONDocumentSubTypeMasters.ToList();
            }
        }

        public List<CONDocumentMaster> GetDocuments(string applicantTypeId, string titleValue, string PremisetypeValue, string applicantType, string continuewithSd)
        {
            List<CONDocumentMaster> result = new List<CONDocumentMaster>();
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                result = dataContext.CONDocumentMasters.Where(D => D.ApplicationTypeCode.Trim() == applicantTypeId.Trim() && D.PremiseTypeCode.Trim() == PremisetypeValue.Trim()
               && D.TitleCode.Trim() == titleValue.Trim()).ToList();

                if (applicantType == "3" && !result.Any(d => d.DocumentSerialNumber == 993))
                {
                    CONDocumentMaster coOwnerDoc = dataContext.CONDocumentMasters.Where(D => D.DocumentSerialNumber == 993).FirstOrDefault();
                    if (coOwnerDoc != null)
                        result.Add(coOwnerDoc);
                }

                if (continuewithSd == "Yes" && applicantTypeId.Trim() == "21" && !result.Any(d => d.DocumentSerialNumber == 62))
                {
                    CONDocumentMaster coOwnerDoc = dataContext.CONDocumentMasters.Where(D => D.DocumentSerialNumber == 62).FirstOrDefault();
                    if (coOwnerDoc != null)
                        result.Add(coOwnerDoc);
                }
            }
            return result;
        }

        public CONDocumentMaster GetForm16DocMaster()
        {
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                return dataContext.CONDocumentMasters.Where(d => d.DocumentDescription.Contains("Change Of Name 16.1 form")).FirstOrDefault();
            }
        }

        public int GetDocCount(string refNumber)
        {
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                return dataContext.CONApplicationDocumentDetails.Where(d => d.RegistrationSerialNumber == refNumber).Count();
            }
        }

        public int GetDocCountForApplication(CONApplicationDetail app)
        {
            using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
            {
                var titleValue = "";
                if (app.ApplicantType == "1" || app.ApplicantType == "3")
                {
                    if (app.TitleValue == "0006")
                        titleValue = "0006";
                }
                else
                {
                    titleValue = "0005";
                }
                var alldocsList = dataContext.CONDocumentMasters.Where(d => d.PremiseTypeCode == app.PremiseTypeCode && d.ApplicationTypeCode == app.ApplicationTypeId && d.TitleCode == titleValue && d.AKLASSE == string.Empty && d.DocumentTypeCode != "OT").ToList();

                var alldocsCount = alldocsList.Select(d => d.DocumentTypeCode).Distinct().Count();
                if (app.ApplicantType == "1" || app.ApplicantType == "3")
                {
                    return alldocsCount;
                }
                else
                {
                    return alldocsCount + 1;    //in case of joint application - PH will be twice 
                }
            }
        }

        public bool updateAPIreturnStatus(CONApplicationDetail application, string outputdata, CONDocumentMaster doc)
        {
            try
            {
                using (PaymentHistoryDataContext dataContext = new PaymentHistoryDataContext())
                {
                    CONApplicationDocumentDetail existingDoc = dataContext.CONApplicationDocumentDetails.FirstOrDefault(a => a.RegistrationSerialNumber == application.RegistrationSerialNumber && a.CreatedBy == application.AccountNumber && a.DocumentDescription == doc.DocumentDescription && a.DocumentChecklistSerialNumber == doc.DocumentSerialNumber.ToString());
                    existingDoc.SAPReturnMsg = outputdata;
                    dataContext.SubmitChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error("CON SaveDocument UpdateAPIreturn saving error:" + e.Message, this);
                return false;
            }
        }
    }
}