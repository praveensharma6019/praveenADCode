using Sitecore.Ports.Website;
using Sitecore.Ports.Website.Models;
using System;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Sitecore.Ports.Website.Services
{
    public class PortsRepository
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
        public string StoreGeneratedOtp(PortsContactModel model)
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
        public string PortsGMSStoreGeneratedOtp(PortsGMSOTP model)
        {
            string sRandomOTP = GenerateRandomOTP(5);
            PortsGMSDataContext pgd = new PortsGMSDataContext();
            PortsGMSOTPHistory entity = new PortsGMSOTPHistory()
            {   Id = Guid.NewGuid(),
                OTPType = model.OTPType,
                OTPFor = model.OTPFor,
                OTP = sRandomOTP,
                Status = false,
                CreatedDate = System.DateTime.Now
            };
            pgd.PortsGMSOTPHistories.InsertOnSubmit(entity);
            pgd.SubmitChanges();
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