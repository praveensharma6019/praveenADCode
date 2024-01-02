using Adani.SuperApp.Realty.Feature.Navigation.Platform.Models;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

namespace Adani.SuperApp.Realty.Feature.Navigation.Platform.Services
{
    public class RealityOTPServices
    {
        AdaniRealityDataContext dc = new AdaniRealityDataContext();
        public void DeleteOldOtp(string OtpFor = null)
        {
            if (!string.IsNullOrEmpty(OtpFor))
            {
                OtpFor = OtpFor.Contains("+91") ? OtpFor.Replace("+91", "91").Trim() : OtpFor.Trim();
                var removeexistotp = dc.Reality__OTPs.Where(x => x.Mobile == OtpFor);
                dc.Reality__OTPs.DeleteAllOnSubmit(removeexistotp);
                dc.SubmitChanges();
                Log.Info("DataBase User OTP remove - ", this);
            }
        }
        public bool ValidateOTP(string OtpFor = null, string OTP = null)
        {
            Reality__OTP reality__OTPObj = new Reality__OTP();

            if (!string.IsNullOrEmpty(OtpFor))
            {
                OtpFor = OtpFor.Contains("+91") ? OtpFor.Replace("+91", "91").Trim() : OtpFor.Trim();
                DateTime dateTime = DateTime.Now;
                var listOfMobile = dc.Reality__OTPs.Where(x => x.Mobile == OtpFor).OrderByDescending(x => x.Session).FirstOrDefault();
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

        public string StoreGeneratedOtp(SfdcModel model, string OtpFor = null)
        {
            var commonItem = Sitecore.Context.Database.GetItem(Templates.commonData.ItemID);
            int otpLenght = !string.IsNullOrEmpty(commonItem.Fields[Templates.commonData.Fields.OTPlenghtID].Value.ToString()) ? Convert.ToInt32(commonItem.Fields[Templates.commonData.Fields.OTPlenghtID].Value) : 5;
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