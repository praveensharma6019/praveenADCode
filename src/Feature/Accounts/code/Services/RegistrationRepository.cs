using Sitecore.Diagnostics;
using Sitecore.Feature.Accounts;
using Sitecore.Feature.Accounts.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Feature.Accounts.Models
{
    public class RegistrationRepository
    {
        PaymentHistoryDataContext dc = new PaymentHistoryDataContext();

        //false if more than 5 OTP generated same day for same CA and Mobile
        public bool CheckForMobCAMaxLimit(string accountNumber, string pageName, string mobileNumber)
        {
            var count = dc.OTPValidationAEMLs.Where(o => o.MobileNumber == mobileNumber && o.AccountNumber == accountNumber && o.PageName == pageName && o.CreatedDate.GetValueOrDefault().Date == DateTime.Now.Date).Count();
            if (count >= 5)
                return false;
            else
                return true;
        }

        //false if more than 20 OTP generated same day for same CA
        public bool CheckForCAMaxLimit(string accountNumber, string pageName)
        {
            var count = dc.OTPValidationAEMLs.Where(o => o.AccountNumber == accountNumber && o.PageName == pageName && o.CreatedDate.GetValueOrDefault().Date == DateTime.Now.Date).Count();
            if (count >= 20)
                return false;
            else
                return true;
        }


        public string GenerateOTPRegistration(string accountNumber, string meterNumber, string pageName, string mobileNumber)
        {
            string sRandomOTP = GenerateRandomOTP(5);
            OTPValidationAEML entity = new OTPValidationAEML()
            {
                MobileNumber = mobileNumber,
                OTP = sRandomOTP,
                AccountNumber = accountNumber,
                PageName = pageName,
                CreatedDate = DateTime.Now
            };
            dc.OTPValidationAEMLs.InsertOnSubmit(entity);
            dc.SubmitChanges();
            return sRandomOTP;
        }

        public string GetOTPRegistrationByMobAndCA(string mobilenumber, string accountNumber, string pageName)
        {
            var OTPRecord = dc.OTPValidationAEMLs.Where(o => o.MobileNumber == mobilenumber && o.AccountNumber == accountNumber && o.PageName == pageName).OrderByDescending(o => o.CreatedDate).FirstOrDefault();

            if (OTPRecord != null)
            {
                return OTPRecord.OTP;
            }
            return string.Empty;
        }


        public void DeleteOldOtp(string mobilenumber = null)
        {
            if (!string.IsNullOrEmpty(mobilenumber))
            {
                var removeexistotp = dc.user_otps.Where(x => x.MobileNumber == mobilenumber);
                dc.user_otps.DeleteAllOnSubmit(removeexistotp);
                dc.SubmitChanges();
            }
        }

        public string StoreGeneratedOtp(RegisteredValidateAccount model)
        {
            string sRandomOTP = GenerateRandomOTP(5);
            user_otp entity = new user_otp()
            {
                MobileNumber = model.MobileNo,
                otp = sRandomOTP,
                status = false
            };
            dc.user_otps.InsertOnSubmit(entity);
            dc.SubmitChanges();
            return sRandomOTP;

        }
        public string GetOTP(string mobilenumber = null)
        {
            if (!string.IsNullOrEmpty(mobilenumber))
            {
                var data = dc.user_otps.Where(x => x.MobileNumber == mobilenumber).FirstOrDefault();
                return data.otp;
            }
            return string.Empty;
        }
        private string GenerateRandomOTP(int iOTPLength)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

            string sOTP = string.Empty;

            string sTempChars = string.Empty;

            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }
            return sOTP;

        }

        public string GetIPAddress()
        {
            try
            {
                System.Web.HttpContext context = System.Web.HttpContext.Current;
                string ipAddress = context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
                if (!string.IsNullOrEmpty(ipAddress))
                {
                    string[] addresses = ipAddress.Split(',');
                    if (addresses.Length != 0)
                    {
                        return addresses[0];
                    }
                }
                return context.Request.ServerVariables["REMOTE_ADDR"];
            }
            catch (Exception e)
            {
                Log.Error("GetIPAddress exception: " + e.Message, "Adani Housing GetIPAddress");
                return null;
            }
        }

        public bool IsUserLoggedIn()
        {
            if (Context.User != null && Context.User.IsAuthenticated)
            {
                if (SessionHelper.UserSession.UserSessionContext == null)
                {
                    return false;
                }
                //else if (!SessionHelper.UserSession.UserSessionContext.SessionId.Equals(HttpContext.Current.Request.Cookies["SessionId"].Value))
                //{
                //    return false;
                //}
            }
            else
            {
                return false;
            }
            return true;
        }

        public bool StoreCurrentSession()
        {
            try
            {

                if (SessionHelper.UserSession.UserSessionContext == null)
                {
                    return false;
                }
                else if (SessionHelper.UserSession.UserSessionContext.UserName != null)
                {
                    UserLoginSession currentsessionExists = (
                        from x in dc.UserLoginSessions
                        where x.UserName == SessionHelper.UserSession.UserSessionContext.UserName && x.IsActive == true
                        select x).FirstOrDefault<UserLoginSession>();
                    if (currentsessionExists != null)
                    {
                        currentsessionExists.SessionId = SessionHelper.UserSession.UserSessionContext.SessionId;
                        dc.SubmitChanges();
                        return true;
                    }
                    else
                    {
                        UserLoginSession loginObj = new UserLoginSession()
                        {
                            CreatedDate = DateTime.Now,
                            UserName = SessionHelper.UserSession.UserSessionContext.UserName,
                            IsActive = true,
                            SessionId = SessionHelper.UserSession.UserSessionContext.SessionId
                        };
                        dc.UserLoginSessions.InsertOnSubmit(loginObj);
                        dc.SubmitChanges();
                        return true;
                    };
                }
                else
                {
                    return false;
                }

            }
            catch (Exception e)
            {
                Log.Error("Session store failed for adani housing login: " + e.Message, this);
                return false;
            }
        }

        public bool ValidateCurrentSession()
        {
            try
            {
                if (SessionHelper.UserSession.UserSessionContext == null)
                {
                    return false;
                }
                else if (SessionHelper.UserSession.UserSessionContext.UserName != null)
                {
                    UserLoginSession currentsessionExists = (
                        from x in dc.UserLoginSessions
                        where x.UserName == SessionHelper.UserSession.UserSessionContext.UserName && x.SessionId == SessionHelper.UserSession.UserSessionContext.SessionId && x.IsActive == true
                        select x).FirstOrDefault<UserLoginSession>();
                    if (currentsessionExists != null)
                    {
                        return true;
                    }
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                Log.Error("Session store failed for my accoutn login: " + e.Message, this);
                return false;
            }
        }


        public bool DeleteSessionAfterLogout()
        {
            bool isRemoved = false;
            try
            {
                if (SessionHelper.UserSession.UserSessionContext.UserName != null)
                {
                    UserLoginSession currentsessionExists = (
                        from x in dc.UserLoginSessions
                        where x.UserName == SessionHelper.UserSession.UserSessionContext.UserName && x.IsActive == true
                        select x).FirstOrDefault<UserLoginSession>();
                    if (currentsessionExists != null)
                    {
                        dc.UserLoginSessions.DeleteOnSubmit(currentsessionExists);
                        dc.SubmitChanges();
                        isRemoved = true;
                    }
                }

            }
            catch (Exception e)
            {
                Log.Error("Removing Session failed for Adani Electricity Revamp Logout: " + e.Message, this);
                isRemoved = false;
            }

            return isRemoved;
        }
    }
}