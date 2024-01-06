using Sitecore.Realty.Website.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.Realty.Website.Services
{
    public class RealtyRepository
    {
        EnquiryDataContext dc = new EnquiryDataContext();
        public void DeleteOldOtp(string mobilenumber = null)
        {
            if (!string.IsNullOrEmpty(mobilenumber))
            {
                var removeexistotp = dc.user_otps.Where(x => x.MobileNumber == mobilenumber);
                dc.user_otps.DeleteAllOnSubmit(removeexistotp);
                dc.SubmitChanges();
            }
        }
        public bool CanSendOTP(List<DateTime> lastOtpsSentOn, int boundr1InMinutes, int otpAllowedInBundary1, int boundr2InMinutes)
        {
            if(lastOtpsSentOn.Count<otpAllowedInBundary1)
            {
                return true;
            }
            DateTime firstOtpSentInBoundry = lastOtpsSentOn.OrderByDescending(d => d).Take(otpAllowedInBundary1).Last();
            DateTime lastOtpSentInBoundry = lastOtpsSentOn.OrderByDescending(d => d).First();

            if ((lastOtpSentInBoundry - firstOtpSentInBoundry).TotalMinutes <= boundr1InMinutes)
            {
                if (firstOtpSentInBoundry.AddMinutes(boundr2InMinutes) >= DateTime.Now)
                {
                    return false;
                }
            }

           

            //int mobileNoCount = dc.user_otps.Where(x => x.MobileNumber == mobileNo).Count();
            //if (mobileNoCount >=tranAllowedInBundary)
            //{
            //    var LastOtpExpiryTime = dc.user_otps.Where(x => x.MobileNumber == mobileNo).OrderByDescending(x             //    var LastTranOTPExpiryTime = dc.user_otps.Where(x => x.MobileNumber == mobileNo).OrderByDescending(x => x.Id).Select(x => x.ExpiryTime).Skip(tranAllowedInBundary - 1).Take(1);
            //    if((DateTime.Parse(LastTranOTPExpiryTime.ToString()) - DateTime.Parse(LastOtpExpiryTime.ToString())).Minutes < timeInterval)
            //    {
            //        if (DateTime.Now <DateTime.Parse(LastTranOTPExpiryTime.ToString()).AddMinutes(PenlatyBoundary))
            //        {
            //            return false;
            //        }
            //    }
            //    else
            //    {
            //        return true;
            //    }


            //}
            return true;
        }
        public string StoreGeneratedOtp(EnquiryModel model, int otpLifeinMinutes, int maxAttempt)
        {
            string sRandomOTP = GenerateRandomOTP(5);
            var otpDateTimeList = dc.user_otps.Where(o => o.MobileNumber==model.mobile && o.OtpPurpose.HasValue && o.OtpPurpose == (int)model.OtpPurpose && o.ExpiryTime.HasValue).OrderByDescending(o => o.ExpiryTime).Select(x=>x.ExpiryTime.Value).Take(3).ToList();
            var cansendOtp = this.CanSendOTP(otpDateTimeList, 30, 3, 30);
            // var isOtpExist = dc.user_otps.Where(x => x.MobileNumber == model.mobile && x.RemainingAttempt>0).Any();
            if (cansendOtp)
            {
                user_otp entity = new user_otp()
                {
                    MobileNumber = model.mobile,
                    otp = sRandomOTP,
                    status = false,
                    RemainingAttempt = maxAttempt,
                    ExpiryTime = DateTime.Now.AddMinutes(otpLifeinMinutes),
                    OtpPurpose = (int)model.OtpPurpose
                };
                dc.user_otps.InsertOnSubmit(entity);
                dc.SubmitChanges();
                return sRandomOTP;
            }
            return "0";

        }
        public string GetOTP(string mobilenumber, PurposeOfOtp purposeOfOtp)
        {
            if (!string.IsNullOrEmpty(mobilenumber))
            {
                var data = dc.user_otps.Where(x => x.MobileNumber == mobilenumber && x.ExpiryTime > DateTime.Now && x.RemainingAttempt > 0 && x.OtpPurpose == (int)purposeOfOtp).FirstOrDefault();
                if (data != null)
                {
                    data.RemainingAttempt--;
                    dc.SubmitChanges();
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