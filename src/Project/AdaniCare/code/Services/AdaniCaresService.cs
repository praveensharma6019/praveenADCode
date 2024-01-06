using Sitecore.AdaniCare.Website.Models;
using Sitecore.AdaniCare.Website;
using Sitecore.Data;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Sitecore.Exceptions;
using System.Net.Mail;

namespace Sitecore.AdaniCare.Website.Services
{
    public class AdaniCaresService
    {
        private UserDataContextDataContext dataContext = new UserDataContextDataContext();

        public string GetPageURL(ID itemId)
        {
            var ItemId = Context.Database.GetItem(itemId);
            return ItemId.Url();
        }

        public bool SaveClaimDetails(AdaniCareConsumerDetails model, AdaniCareOfferDetails offerDetails, bool isemailTobesend)
        {
            try
            {
                ClaimedOffersDetail objUser = new ClaimedOffersDetail
                {
                    ClaimedDate=DateTime.Now,
                    ConsumerAccountNumber=model.AccountNumber,
                    ConsumerMobileNumber=model.MobileNumber,
                    ConsumerName=model.ConsumerName,
                    ConsumerSessionId=model.AccountNumber,
                    OfferCompany=offerDetails.OfferCompany,
                    OfferId=offerDetails.OfferId,
                    OfferName=offerDetails.OfferName,
                    ClaimedEmailAddress=offerDetails.ClaimEmailAddress,
                    ClaimedMobileNumber=offerDetails.ClaimMobileNumber
                };
                dataContext.ClaimedOffersDetails.InsertOnSubmit(objUser);
                dataContext.SubmitChanges();

                if (isemailTobesend)
                {
                    var claimedOffer = Sitecore.Context.Database.GetItem(offerDetails.OfferId);
                    var companyEmailAddresses = claimedOffer.Fields["CompanyEmailAddress"].Value;

                    var EmailTemplate = Context.Database.GetItem(Templates.EmailTemplates.ClaimOfferEmail);
                    var fromMail = EmailTemplate.Fields[Templates.EmailTemplates.Fields.From];

                    if (string.IsNullOrEmpty(fromMail.Value))
                    {
                        throw new InvalidValueException("'From' field in mail template should be set");
                    }

                    var body = EmailTemplate.Fields[Templates.EmailTemplates.Fields.Body];
                    var subject = EmailTemplate.Fields[Templates.EmailTemplates.Fields.Subject];

                    MailMessage mail = new MailMessage
                    {
                        From = new MailAddress(fromMail.Value),
                        Body = body.Value,
                        Subject = subject.Value,
                        IsBodyHtml = true
                    };

                    string[] companyEmailAddressesList = companyEmailAddresses.Split(',');
                    for (int i = 0; i < companyEmailAddressesList.Length; i++)
                    {
                        mail.To.Add(companyEmailAddressesList[i].Trim());
                    }

                    //mail.To.Add("nidhi.paneri@advaiya.com");
                    mail.Body = mail.Body.Replace("#OfferName#", offerDetails.OfferName);
                    mail.Body = mail.Body.Replace("#OfferDesc#", offerDetails.OfferDesc);
                    mail.Body = mail.Body.Replace("#ConsumerName#", model.ConsumerName);
                    mail.Body = mail.Body.Replace("#ConsumerAccountNumber#", model.AccountNumber);
                    mail.Body = mail.Body.Replace("#ConsumerMobile#", offerDetails.ClaimMobileNumber);
                    mail.Body = mail.Body.Replace("#ConsumerEmail#", offerDetails.ClaimEmailAddress);
                    //mail.Subject = mail.Subject.Replace("#Subject#", NitNo);
                    MainUtil.SendMail(mail);
                }
                return true;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at SaveAuthenticationLog:" + ex.Message, this);
                return false;
            }
        }

        public bool SaveAuthenticationLog(AdaniCareConsumerDetails model)
        {
            try
            {
                AuthenticatedUsersLog objUser = new AuthenticatedUsersLog
                {
                    AccountNumber = model.AccountNumber,
                    ConsumerEmail = model.ConsumerEmail,
                    ConsumerName = model.ConsumerName,
                    CreatedDate = DateTime.Now,
                    MobileNumber = model.MobileNumber
                };
                dataContext.AuthenticatedUsersLogs.InsertOnSubmit(objUser);
                dataContext.SubmitChanges();
                return true;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Error at SaveAuthenticationLog:" + ex.Message, this);
                return false;
            }
        }

        //max 10 OTP's per day
        public bool IsOTPMaxLimitExceed(string mobileNumber, string accountNumber)
        {
            var count = dataContext.OTPValidationLogs.Where(o => o.MobileNumber == mobileNumber && o.AccountNumber == accountNumber && o.CreatedDate.GetValueOrDefault().Date == DateTime.Now.Date).Count();
            if (count >= 10)
                return true;
            else
                return false;
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

        public string GenerateOTP(string mobileNumber, string accountNumber)
        {
            string sRandomOTP = GenerateRandomOTP(5);
            OTPValidationLog entity = new OTPValidationLog()
            {
                MobileNumber = mobileNumber,
                AccountNumber = accountNumber,
                OTP = sRandomOTP,
                CreatedDate = DateTime.Now
            };
            dataContext.OTPValidationLogs.InsertOnSubmit(entity);
            dataContext.SubmitChanges();
            return sRandomOTP;

        }

        public string GetOTP(string mobileNumber, string accountNumber)
        {
            if (!string.IsNullOrEmpty(mobileNumber))
            {
                var data = dataContext.OTPValidationLogs.Where(x => x.MobileNumber == mobileNumber && x.AccountNumber == accountNumber).OrderByDescending(o => o.CreatedDate).FirstOrDefault();
                return data.OTP;
            }
            return string.Empty;
        }
    }
}