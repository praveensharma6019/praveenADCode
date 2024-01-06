using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Security.Cryptography;
using System.Web;
using Sitecore.Foundation.OTPService.Models;
namespace Sitecore.Foundation.OTPService.Services
{
    public class OTPCRUDService
    {

        OTPServiceDataContext dc = new OTPServiceDataContext();
        public void DeleteOldOtp(string OtpFor = null)
        {
            if (!string.IsNullOrEmpty(OtpFor))
            {
                OtpFor = OtpFor.Contains("+91") ? OtpFor.Replace("+91", "91").Trim() : OtpFor.Trim();
                var removeexistotp = dc.OTPServices.Where(x => x.Mobile == OtpFor);
                dc.OTPServices.DeleteAllOnSubmit(removeexistotp);
                dc.SubmitChanges();
                Log.Info("DataBase User OTP remove - ", this);
            }
        }
        public bool ValidateOTP(string OtpFor = null, string OTP = null)
        {
            OTPCRUDService OTPServices = new OTPCRUDService();

            if (!string.IsNullOrEmpty(OtpFor))
            {
                OtpFor = OtpFor.Contains("+91") ? OtpFor.Replace("+91", "91").Trim() : OtpFor.Trim();
                DateTime dateTime = DateTime.Now;
                var listOfMobile = dc.OTPServices.Where(x => x.Mobile == OtpFor).OrderByDescending(x => x.Session).FirstOrDefault();
                if (listOfMobile != null && listOfMobile.Mobile == OtpFor && listOfMobile.OTP != "" && listOfMobile.OTP == OTP)
                {
                    TimeSpan value = dateTime.Subtract((DateTime)listOfMobile.Session);
                    if (value != null && (value.TotalMinutes > 10 || listOfMobile.Attempt > 2))
                    {
                        listOfMobile.Attempt = listOfMobile.Attempt == null ? 1 : listOfMobile.Attempt + 1;
                        dc.SubmitChanges();
                        return false;
                    }
                    listOfMobile.Attempt = 0;
                    dc.SubmitChanges();
                    return true;
                }
                else
                {
                    listOfMobile.Attempt = listOfMobile.Attempt == null ? 1 : listOfMobile.Attempt + 1;
                    dc.SubmitChanges();
                    return false;
                }
            }
            else
            {
                return false;
            }
        }

        public string StoreGeneratedOtp(OTPServiceModel model)
        {
            var commonItem = Sitecore.Context.Database.GetItem(model.OTPSitecoreDatasource);
            int otpLenght = !string.IsNullOrEmpty(commonItem.Fields[Templates.Template.SitecoreOTPFields.OTPLength].Value.ToString()) ? Int32.Parse(commonItem.Fields[Templates.Template.SitecoreOTPFields.OTPLength].Value) : 5;
           
            string sRandomOTP = GenerateRandomOTP(otpLenght);

            return sRandomOTP;
        }
        private string GenerateRandomOTP(int iOTPLength)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            var byteArray = new byte[iOTPLength];
            provider.GetBytes(byteArray);
            var randomInteger = BitConverter.ToInt32(byteArray, 0);
            randomInteger = Math.Abs(randomInteger);
            var randomIntegerStr = randomInteger.ToString().Substring(0, iOTPLength);
            var a = "";
            if (randomIntegerStr.Length < iOTPLength)
            {
                var appendLength = iOTPLength - randomIntegerStr.Length;
                for (int j = 0; j < appendLength; j++)
                {
                    randomIntegerStr = a + "0";
                }
            }
            return randomIntegerStr;
        }

    }
}