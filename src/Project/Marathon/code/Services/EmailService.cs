using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Marathon.Website.Models;
using Sitecore.Project.Marathon;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;

namespace Sitecore.Marathon.Website.Services
{
    public static class EmailService
    {
        public static void sendEmail(string to, string subject, string body, string from)
        {
            try
            {
                Log.Info("Marathon EmailService call start", "");
                using (var client = new HttpClient())
                {
                    var apiUrl = DictionaryPhraseRepository.Current.Get("Email Service/API", "");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                    var postData = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("To", to),
                        new KeyValuePair<string, string>("From", from),
                        new KeyValuePair<string, string>("Subject", subject),
                        new KeyValuePair<string, string>("Body", HttpUtility.HtmlEncode(body))
                    };
                    var content = new FormUrlEncodedContent(postData);

                    HttpResponseMessage response = client.PostAsync(apiUrl, content).Result;
                    if (response.IsSuccessStatusCode == false)
                    {
                         Log.Info("Marathon EmailService email failed", response.IsSuccessStatusCode);
                    }
                    else
                    {
                        Log.Info("Marathon EmailService email send ", response.IsSuccessStatusCode);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Info("Marathon EmailService Exception  occured", ex);
            }
        }

        public static void SendMailForCharityBib(AhmedabadMarathonRegistration empdata)
        {
            try
            {
                Log.Info("Marathon SendMailForCharityBib call start", "");
                var CharityMail = Context.Database.GetItem(EMailTemplate.CharityBib);
                string From = CharityMail.Fields[EMailTemplate.Fields.From].Value;
                string To = CharityMail.Fields[EMailTemplate.Fields.To].Value;
                string Subject = CharityMail.Fields[EMailTemplate.Fields.Subject].Value;
                string body = CharityMail.Fields[EMailTemplate.Fields.Body].Value;
                body = string.Format(body, empdata.FirstName, empdata.LastName, empdata.Address, empdata.City, empdata.State, empdata.ContactNumber, empdata.Email, empdata.PANNumber, empdata.DonationAmount.ToString(), empdata.TaxExemptionCause);

                EmailService.sendEmail(To, Subject, body, From);
            }
            catch (Exception ex)
            {
                Log.Info("Marathon SendMailForCharityBib Exception  occured", ex);

            }
        }
        public static void SendMailDonationNow(Donate empdata)
        {
            try
            {
                Log.Info("Marathon SendMailForCharityBib call start", "");
                var DonationMail = Context.Database.GetItem(EMailTemplate.DonationMail);
                string From = DonationMail.Fields[EMailTemplate.Fields.From].Value;
                string To = DonationMail.Fields[EMailTemplate.Fields.To].Value;
                string Subject = DonationMail.Fields[EMailTemplate.Fields.Subject].Value;
                string body = DonationMail.Fields[EMailTemplate.Fields.Body].Value;
                body = string.Format(body, empdata.Name, empdata.CauseTitle, empdata.MobileNumber, empdata.EmailId, empdata.AffiliateCode, empdata.Amount, empdata.TaxExemptionCause);

                EmailService.sendEmail(To, Subject, body, From);
            }
            catch (Exception ex)
            {
                Log.Info("Marathon SendMailForCharityBib Exception  occured", ex);

            }
        }
    }
}