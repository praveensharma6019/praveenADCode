using Sitecore.Diagnostics;
using Sitecore.ElectricityNew.Website.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.InteropServices;
using System.Web;
using System.Text.RegularExpressions;
using ClientsideEncryption;

namespace Sitecore.ElectricityNew.Website.Services
{
    public class EVChargingconnectionServices
    {

        public bool EVChargingconnectionInsert(EVChargingconnectionModel EVChargingModel)
        {

            try
            {
                using (TenderDataContext dbcontext = new TenderDataContext())
                {
                    string division = AESEncrytDecry.DecryptStringAES(EVChargingModel.DESC_CON_OBJECT);
                    if (!string.IsNullOrEmpty(division) && (division.Contains('-') || division.Contains(' ')))
                    {
                        division = division.Replace(' ', '$').Replace('-', '$');
                        division = !string.IsNullOrEmpty(division.Split('$')[1]) ? division.Split('$')[1] : !string.IsNullOrEmpty(division.Split('$')[2]) ? division.Split('$')[2] : string.Empty;
                    }

                    EVChargingconnectionDetail EVChargingObj = new EVChargingconnectionDetail()
                    {
                        AccountNo = AESEncrytDecry.DecryptStringAES(EVChargingModel.AccountNo),
                        EmailId = AESEncrytDecry.DecryptStringAES(EVChargingModel.EmailId),
                        MobileNo = AESEncrytDecry.DecryptStringAES(EVChargingModel.MobileNo),
                        FullName = AESEncrytDecry.DecryptStringAES(EVChargingModel.FullName),
                        MobileNoAlt = AESEncrytDecry.DecryptStringAES(EVChargingModel.MobileNoAlt),
                        EmailIdAlt = AESEncrytDecry.DecryptStringAES(EVChargingModel.EmailIdAlt),
                        SocietyName = AESEncrytDecry.DecryptStringAES(EVChargingModel.SocietyName),
                        PinCode = AESEncrytDecry.DecryptStringAES(EVChargingModel.PinCode),
                        SecretaryName = AESEncrytDecry.DecryptStringAES(EVChargingModel.SecretaryName),
                        SecretaryContactNo = AESEncrytDecry.DecryptStringAES(EVChargingModel.SecretaryContactNo),
                        ParkingArea = AESEncrytDecry.DecryptStringAES(EVChargingModel.ParkingArea),
                        SocietyMemberEV = AESEncrytDecry.DecryptStringAES(EVChargingModel.SocietyMemberEV),
                        CONOBJNO = AESEncrytDecry.DecryptStringAES(EVChargingModel.CON_OBJ_NO),
                        SAPNAME = AESEncrytDecry.DecryptStringAES(EVChargingModel.SAPNAME),
                        STREET = AESEncrytDecry.DecryptStringAES(EVChargingModel.STREET),
                        STREET2 = AESEncrytDecry.DecryptStringAES(EVChargingModel.STREET2),
                        REGSTRGROUP = AESEncrytDecry.DecryptStringAES(EVChargingModel.REGSTRGROUP),
                        RATECAT = AESEncrytDecry.DecryptStringAES(EVChargingModel.RATECAT),
                        FUNC_DESCR = AESEncrytDecry.DecryptStringAES(EVChargingModel.FUNC_DESCR),
                        MRU = AESEncrytDecry.DecryptStringAES(EVChargingModel.MRU),
                        DESC_CON_OBJECT = AESEncrytDecry.DecryptStringAES(EVChargingModel.DESC_CON_OBJECT),
                        Division = division,
                        udf2 = EVChargingModel.udf2,
                        CreatedBy = EVChargingModel.CreatedBy,
                        Created_Date = DateTime.Now
                    };
                    dbcontext.EVChargingconnectionDetails.InsertOnSubmit(EVChargingObj);
                    dbcontext.SubmitChanges();
                    return true;
                }

            }

            catch (Exception e)
            {
                Log.Error("InsertEVChargingconnectionRecord error in db save: " + e.Message, this);
            }
            return false;
        }

        public bool sendEVMobileSMS(string apiurl)
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
                        Sitecore.Diagnostics.Log.Error("OTP Api call success for EV API URL: " + apiurl, this);
                        return true;
                    }
                    else
                    {
                        Sitecore.Diagnostics.Log.Error("OTP Api call Failed for EV API URL: " + apiurl, this);
                        return false;
                    }
                }
                else
                {
                    Sitecore.Diagnostics.Log.Error("OTP Api call Failed for EV API URL: " + apiurl, this);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Exception occured for OTP Api call for EV API URL: " + apiurl + " Exception: " + ex.StackTrace, this);
                return false;
            }
        }


        public string GenerateOTPEV(string accountNumber, string meterNumber, string pageName, string mobileNumber, string token)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                string sRandomOTP = GenerateRandomOTP(5);
                OTPValidationAEML entity = new OTPValidationAEML()
                {
                    MobileNumber = mobileNumber,
                    OTP = sRandomOTP,
                    AccountNumber = accountNumber,
                    PageName = pageName,
                    CreatedDate = DateTime.Now,
                    ReferenceNumner = token
                };
                dbcontext.OTPValidationAEMLs.InsertOnSubmit(entity);
                dbcontext.SubmitChanges();
                return sRandomOTP;
            }

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
        public string GetShareChargeOTPByMobAndCA(string mobileNumber, string accountNumber, string pageName, string token)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var OTPRecord = dbcontext.OTPValidationAEMLs.Where(o => o.MobileNumber == mobileNumber && o.AccountNumber == accountNumber && o.PageName == pageName && o.ReferenceNumner == token).OrderByDescending(o => o.CreatedDate).FirstOrDefault();

                if (OTPRecord != null)
                {
                    return OTPRecord.OTP;
                }
                return string.Empty;
            }

        }

        public bool ValidateShareChargeRequest(string accountNumber, string token)
        {
            using (TenderDataContext dbcontext = new TenderDataContext())
            {
                var OTPRecord = dbcontext.OTPValidationAEMLs.Where(o => o.ReferenceNumner == token).OrderByDescending(o => o.CreatedDate).FirstOrDefault();

                if (OTPRecord != null)
                {
                    if (OTPRecord.AccountNumber == accountNumber)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        public bool CheckAccountNumberLength(String strNumber)
        {
            Regex mobilePattern = new Regex(@"^[0-9]{9}$");
            return mobilePattern.IsMatch(strNumber);
        }
    }
}