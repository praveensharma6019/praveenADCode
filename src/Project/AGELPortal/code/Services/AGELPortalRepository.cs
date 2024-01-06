using Sitecore.AGELPortal.Website.Models;
using System;
using System.Linq;
using Sitecore.Diagnostics;

namespace Sitecore.AGELPortal.Website.Services
{
    public class AGELPortalRepository
    {
        AGELPortalDataContext dc = new AGELPortalDataContext();
        public void DeleteOldOtp(string OtpFor = null)
        {
            if (!string.IsNullOrEmpty(OtpFor))
            {
                var removeexistotp = dc.AGElPortalOtpHistories.Where(x => x.otp_for == OtpFor && x.status == true);
                foreach (var item in removeexistotp)
                {
                    item.status = false;
                }
                // dc.AGElPortalOtpHistories.DeleteAllOnSubmit(removeexistotp);
                dc.SubmitChanges();
            }
        }

        public string StoreGeneratedOtp(PortalRegistrationModel model, string OtpFor = null)
        {
            string sRandomOTP = GenerateRandomOTP(6);
            AGElPortalOtpHistory entity = new AGElPortalOtpHistory()
            {
                otp_for = model.mobile,
                // otp_for = OtpFor,
                otp = sRandomOTP,
                status = true
            };
            entity.date = DateTime.Now;
            dc.AGElPortalOtpHistories.InsertOnSubmit(entity);
            dc.SubmitChanges();
            return sRandomOTP;

        }
        public string GetOTP(string OtpFor = null)
        {
            if (!string.IsNullOrEmpty(OtpFor))
            {
                var data = dc.AGElPortalOtpHistories.Where(x => x.otp_for == OtpFor && x.status == true).OrderByDescending(y => y.date).FirstOrDefault();
                if (data != null)
                {
                    if (data.date.Value.AddMinutes(10) < DateTime.Now)
                    {
                        return "optexpired";
                    }
                    else
                    {
                        return data.otp;
                    }

                }
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
        public bool IsUserLoggedIn()
        {
            if (Context.User != null)
            {
                if (UserSession.UserSessionContext == null)
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
        public bool ValidateCurrentSession()
        {
            try
            {
                if (UserSession.UserSessionContext == null)
                {
                    return false;
                }

                if (!string.IsNullOrEmpty(UserSession.UserSessionContext))
                {

                    return true;
                }
                else
                {
                    return false;
                }


            }
            catch (Exception e)
            {
                Log.Error("Session store failed for my accoutn login: " + e.Message, this);
                return false;
            }
        }


    }
}