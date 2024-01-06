using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Marathon.Website.Models;
using Sitecore.Project.Marathon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace Sitecore.Marathon.Website.Services
{
    public static class SMSOTP
    {

        public static bool SendOTPSMS(string ContactNumber, string OTP)
        {
            Log.Info("Marathon SendOTPSMS start for Phone Number" + ContactNumber + "And Otp is:" + OTP, ContactNumber);
            try
            {
                var API = DictionaryPhraseRepository.Current.Get("/SMS API and Message/API", "");
                var SMSOTPItem = Context.Database.GetItem(SMSTemplate.OTP);
                string body = SMSOTPItem.Fields[SMSTemplate.Fields.Body].Value;

                var apiurl = string.Format(API, ContactNumber, body);
                apiurl = string.Format(apiurl, OTP);

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(apiurl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Task<HttpResponseMessage> x = client.GetAsync(apiurl);
                HttpResponseMessage response = x.Result;
                if (response.IsSuccessStatusCode)
                {
                    Log.Info("Marathon SendOTPSMS OTP Api call success", ContactNumber);
                    return true;
                }
                else
                {
                    Log.Error("Marathon SendOTPSMS OTP Api call fail", ContactNumber);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Marathon SendOTPSMS exception occured" + ex.Message, ContactNumber);
                return false;
            }
        }

        public static string GetSMSOTP(string phoneNumber = null)
        {
            AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();

            if (!string.IsNullOrEmpty(phoneNumber))
            {
                AhmedabadMarathonOTP getOTP = (from x in rdb.AhmedabadMarathonOTPs where x.EmailId == phoneNumber select x).FirstOrDefault();
                if (getOTP != null)
                {
                    if (string.IsNullOrEmpty(getOTP.ValidateOTPCount))
                    {
                        getOTP.ValidateOTPCount = "1";
                        rdb.SubmitChanges();
                    }
                    else
                    {
                        if (Int64.Parse(getOTP.ValidateOTPCount) <= 3)
                        {
                            int num = int.Parse(getOTP.ValidateOTPCount) + 1;
                            getOTP.ValidateOTPCount = num.ToString();
                            rdb.SubmitChanges();
                        }
                        else
                        {
                            return "InvalidAttempt";
                        }
                    }
                    return getOTP.OTP;
                }
            }
            return string.Empty;
        }


        public static void registrationconfirmation(RegistrationModel details)
        {
            Log.Info("Marathon registrationonfirmation start for Phone Number" + details.ContactNumber , details.ContactNumber);
            try
            {
                var API = DictionaryPhraseRepository.Current.Get("/SMS API and Message/Registration Confirmation SMS", "");
                var EventName = DictionaryPhraseRepository.Current.Get("/Event/EventName", "");
                var SMSOTPItem = Context.Database.GetItem(SMSTemplate.RegistrationConfirmationSMS);
                string body = SMSOTPItem.Fields[SMSTemplate.Fields.Body].Value;

                var apiurl = string.Format(API, details.ContactNumber,details.FirstName+" "+details.LastName, EventName, details.RaceDistance,details.Gender,details.TShirtSize,details.Email,details.ContactNumber);
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(apiurl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Task<HttpResponseMessage> x = client.GetAsync(apiurl);
                HttpResponseMessage response = x.Result;
                if (response.IsSuccessStatusCode)
                {
                    Log.Info("Marathon registrationonfirmation OTP Api call success", details.ContactNumber);
                }
                else
                {
                    Log.Error("Marathon registrationonfirmation OTP Api call fail", details.ContactNumber);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Marathon registrationonfirmation exception occured" + ex.Message, details.ContactNumber);
            }
        }


        public static string SendSMSOTP(string Email, string SessionID)
        {
            try
            {
                MarathonRepository repo = new MarathonRepository();
                bool flag = false;
                string AttmptCount = "0";
                var result = new { status = "0" };
                using (AhmedabadMarathonRegistrationDataContext dbContext = new AhmedabadMarathonRegistrationDataContext())
                {
                    AhmedabadMarathonOTP AHOTP = (
                            from x in dbContext.AhmedabadMarathonOTPs
                            where x.EmailId == Email
                            select x).FirstOrDefault<AhmedabadMarathonOTP>();
                    if (AHOTP != null)
                    {
                        AHOTP.Count = AHOTP.Count ?? "0";
                        if (Int64.Parse(AHOTP.Count) < 4)
                        {
                            flag = false;
                        }
                        else
                        {
                            DateTime value = AHOTP.Modified.Value;
                            flag = value.AddMinutes(30) > DateTime.Now;
                        }
                        if (!flag)
                        {
                            AttmptCount = AHOTP.Count;
                            if (AHOTP.Modified.Value.AddMinutes(30) <= DateTime.Now)
                            {
                                AttmptCount = "0";
                            }
                        }
                        else
                        {
                            result = new { status = "4" };
                            return result.status;
                        }
                    }
                }
                string generatedOTP = repo.StoreEmailOTP(Email, SessionID, AttmptCount);
                if (SMSOTP.SendOTPSMS(Email, generatedOTP))
                {
                    Log.Info("Marathon SMSOTP=> SendSMSOTP succeeded ", Email);
                    result = new { status = "1" };
                    return result.status;
                }
                else
                {
                    Log.Info("Marathon SMSOTP=>  SendSMSOTP failed", Email);
                    return result.status;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Marathon SMSOTP=>  SendSMSOTP exception occured" + ex.Message, Email);
                var result = new { status = "0" };
                return result.status;
            }
        }


    }
}