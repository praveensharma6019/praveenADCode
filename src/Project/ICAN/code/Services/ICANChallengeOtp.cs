using Sitecore.ICAN.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Sitecore.ICAN.Website.Services
{
    public class ICANRepositoryChallenge
    {
        ICANSubmitIdeaDataContext dc = new ICANSubmitIdeaDataContext();

        public void DeleteOldOtp(string mobilenumber = null)
        {
            if (!string.IsNullOrEmpty(mobilenumber))
            {
                var removeexistotp = dc.ICANOtpHistories.Where(x => x.MobileNumber == mobilenumber);
                dc.ICANOtpHistories.DeleteAllOnSubmit(removeexistotp);
                dc.SubmitChanges();
            }
        }
        public string StoreGeneratedOtp(ICANChallengeSignUp model)
        {
            string sRandomOTP = GenerateRandomOTP(6);
            ICANOtpHistory entity = new ICANOtpHistory()
            {
                MobileNumber = model.TeamCoordinatorMobileNumber,
                otp = sRandomOTP,
                status = false
            };
            dc.ICANOtpHistories.InsertOnSubmit(entity);
            dc.SubmitChanges();
            return sRandomOTP;

        }

        public string StoreGeneratedJuryOtp(IcanJurySignINForm model)
        {
            string sRandomOTP = GenerateRandomOTP(6);
            ICANOtpHistory entity = new ICANOtpHistory()
            {
                MobileNumber = model.JuryMobileNumber,
                otp = sRandomOTP,
                status = false
            };
            dc.ICANOtpHistories.InsertOnSubmit(entity);
            dc.SubmitChanges();
            return sRandomOTP;

        }
        public string GetOTP(string mobilenumber = null)
        {
            if (!string.IsNullOrEmpty(mobilenumber))
            {
                var data = dc.ICANOtpHistories.Where(x => x.MobileNumber == mobilenumber).FirstOrDefault();
                if (data != null)
                {
                    return data.otp;
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
    }
}