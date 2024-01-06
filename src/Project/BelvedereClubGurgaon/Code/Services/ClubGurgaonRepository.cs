using Sitecore.BelvedereClubGurgaon.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.BelvedereClubGurgaon.Website.Services
{
    public class ClubGurgaonRepository
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
        public string StoreGeneratedOtp(ClubGurgaonEnquiryModel model)
        {
            string sRandomOTP = GenerateRandomOTP(5);
            user_otp entity = new user_otp()
            {
                MobileNumber = model.Mobile,
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
    }
}