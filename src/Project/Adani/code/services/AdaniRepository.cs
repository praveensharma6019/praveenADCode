using Sitecore.Adani.Website;
using Sitecore.Adani.Website.Models;
using System;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;

namespace Sitecore.Adani.Website.Services
{
    public class AdaniRepository
    {
        private EnquiryDataContext dc = new EnquiryDataContext();

        public AdaniRepository()
        {
        }

        public void DeleteOldOtp(string mobilenumber = null)
        {
            if (!string.IsNullOrEmpty(mobilenumber))
            {
                IQueryable<user_otp> userOtps =
                    from x in this.dc.user_otps
                    where x.MobileNumber == mobilenumber
                    select x;
                this.dc.user_otps.DeleteAllOnSubmit<user_otp>(userOtps);
                this.dc.SubmitChanges();
            }
        }

        private string GenerateRandomOTP(int iOTPLength)
        {
            string[] strArrays = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };
            string empty = string.Empty;
            string str = string.Empty;
            Random random = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {
                random.Next(0, (int)strArrays.Length);
                str = strArrays[random.Next(0, (int)strArrays.Length)];
                empty = string.Concat(empty, str);
            }
            return empty;
        }

        public string GetOTP(string mobilenumber = null)
        {
            string empty;
            if (string.IsNullOrEmpty(mobilenumber))
            {
                empty = string.Empty;
            }
            else
            {
                user_otp userOtp = (
                    from x in this.dc.user_otps
                    where x.MobileNumber == mobilenumber
                    select x).FirstOrDefault<user_otp>();
                empty = userOtp.otp;
            }
            return empty;
        }

        public string StoreGeneratedOtp(AdaniContactModal model)
        {
            string str = this.GenerateRandomOTP(5);
            user_otp userOtp = new user_otp()
            {
                MobileNumber = model.Mobile,
                otp = str,
                status = new bool?(false)
            };
            this.dc.user_otps.InsertOnSubmit(userOtp);
            this.dc.SubmitChanges();
            return str;
        }
    }
}