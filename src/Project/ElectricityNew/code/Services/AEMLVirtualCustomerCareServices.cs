using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Web;

namespace Sitecore.ElectricityNew.Website.Services
{
    public class AEMLVirtualCustomerCareServices
    {
        TenderDataContext dc = new TenderDataContext();
        public bool sendAEMLVCCMobileSMS(string apiurl)
        {
            try
            {
                if (!string.IsNullOrEmpty(apiurl) && apiurl != "X")
                {
                    HttpClient client = new HttpClient();
                    client.BaseAddress = new Uri(apiurl);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    HttpResponseMessage response = client.GetAsync(apiurl).Result;
                    if (response.IsSuccessStatusCode)
                    {
                        Sitecore.Diagnostics.Log.Error("OTP Api call success for sendAEMLVCCMobileSMS API URL: " + apiurl, this);
                        return true;
                    }
                    else
                    {
                        Sitecore.Diagnostics.Log.Error("OTP Api call Failed for sendAEMLVCCMobileSMS API URL: " + apiurl, this);
                        return false;
                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Error("OTP Api call Failed for sendAEMLVCCMobileSMS API URL: " + apiurl, this);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Exception occured for OTP Api call for sendAEMLVCCMobileSMS API URL: " + apiurl + " Exception: " + ex.StackTrace, this);
                return false;
            }
        }

        public string GenerateOTPAEMLVCC(string accountNumber, string meterNumber, string pageName, string mobileNumber)
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

        public string GetOTPRegistrationByMobAndCA(string mobilenumber, string accountNumber, string pageName)
        {
            var OTPRecord = dc.OTPValidationAEMLs.Where(o => o.MobileNumber == mobilenumber && o.AccountNumber == accountNumber && o.PageName == pageName).OrderByDescending(o => o.CreatedDate).FirstOrDefault();

            if (OTPRecord != null)
            {
                return OTPRecord.OTP;
            }
            return string.Empty;
        }

        public bool CheckAccountNumberLength(String strNumber)
        {
            Regex mobilePattern = new Regex(@"^[0-9]{9}$");
            return mobilePattern.IsMatch(strNumber);
        }
    }
}