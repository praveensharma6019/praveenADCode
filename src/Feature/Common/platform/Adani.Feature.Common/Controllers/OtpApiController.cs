using Adani.Feature.Common.Models;
using Adani.Feature.Common.Providers;
using Adani.Foundation.Messaging.Models;
using Adani.Foundation.Messaging.Services.Email;
using Adani.Foundation.Messaging.Services.EmailSender;
using Adani.Foundation.Messaging.Services.SMS;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Adani.Feature.Common.Controllers
{
    public class OtpApiController : Controller
    {
        private readonly IEmailService _mailService;
        private readonly ISMSService _smsService;
        private readonly IOtpDataStorageProvider _storage;
        private readonly IEmailSender _mailSender;

        public OtpApiController()
        {
            _mailService = ServiceLocator.ServiceProvider.GetService<IEmailService>();
            _smsService = ServiceLocator.ServiceProvider.GetService<ISMSService>();
            _storage = ServiceLocator.ServiceProvider.GetService<IOtpDataStorageProvider>();
            _mailSender = ServiceLocator.ServiceProvider.GetService<IEmailSender>();
        }

        public OtpApiController(IEmailService mailService, ISMSService smsService, IOtpDataStorageProvider storage)
        {
            _mailService = mailService;
            _smsService = smsService;
            _storage = storage;
        }

        /// <summary>
        /// Payload
        /// {
        ///   "id": "string",
        ///   "smsData": {
        ///     "recipient": "string",
        ///     "body": "string"
        ///   },
        ///   "mailData": {
        ///     "to": "string",
        ///     "from": "string",
        ///     "subject": "string",
        ///     "body": "string"
        ///   },
        ///   "length": 0,
        ///   "expiresInMinutes": 0
        /// }
        /// id and either from smsData or mailData are mandatory.
        /// if needed to send otp on both sms and email. provide values for smsData and mailData.
        /// </summary>
        /// <param name="otpSendData"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("generate")]
        public ActionResult Generate(OtpRequestModel otpSendData)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Inside try block of OtpApiController.Generate method", "OtpApiController.Generate");
                bool success = SendSMSFarmpik(otpSendData, otpSendData.SMSData.OTP);
                Sitecore.Diagnostics.Log.Info("Received SendSMSFarmpik response:" + success, "SendSMSFarmpik Response");
                if (success)
                    return Json(new { success = true });
                return Json(new { success = false, message = "Some error occured. Please connect with administrator." });
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Info("Exception received in Generate method:" + ex, ex.Message);
                return Json(new { success = false, message = ex });
            }
        }

        [HttpPost]
        [Route("verify")]
        public ActionResult Verify(string id, string otp)
        {
            var storedOtp = _storage.Get(id);
            var isValid = storedOtp != null && storedOtp.OTP == otp && storedOtp.ExpireAt > DateTimeOffset.Now;
            if (isValid == true)
            {
                _storage.Delete(id);
            }
            return Json(new { success = isValid });
        }

        private bool SendSMS(OtpRequestModel otpSendData, string otp)
        {
            if (otpSendData.SMSData != null)
            {
                var data = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("OTP", otp)
                };

                return _smsService.Send(otpSendData.SMSData.Recipient, otpSendData.SMSData.Body, data);
            }

            return true;
        }

        private bool SendSMSFarmpik(OtpRequestModel otpSendData, string otp)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Inside try block with otp:" + otp, otp);
                if (otpSendData.SMSData != null)
                {
                    Sitecore.Diagnostics.Log.Info("Inside if block with SMS Data:" + otpSendData, otpSendData.SMSData.OTP);
                    var farmpikSMSModel = new FarmpikSMSModel();
                    farmpikSMSModel.countryCode = "91";
                    farmpikSMSModel.mobileNo = otpSendData.SMSData.Recipient;
                    farmpikSMSModel.data = new List<Foundation.Messaging.Models.Data>();

                    var smsdata = new Foundation.Messaging.Models.Data();
                    smsdata.key = "OTP";
                    smsdata.value = otp;
                    farmpikSMSModel.data.Add(smsdata);
                    Sitecore.Diagnostics.Log.Info("SMS Data prepared before SendSMSFarmpik", smsdata.key);
                    return _smsService.SendSMSFarmpik(otpSendData.SMSData.Recipient, farmpikSMSModel);
                }
                Sitecore.Diagnostics.Log.Info("Received null sms data in in OtpApiController.SendSMSFarmpik", "OtpApiController.SendSMSFarmpik");
                return false;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Info("Exception in OtpApiController.SendSMSFarmpik:" + ex, ex);
                return false;
            }
        }

        private bool SendEmail(OtpRequestModel otpSendData, string otp)
        {
            if (otpSendData.MailData != null)
            {
                var data = new List<KeyValuePair<string, string>>
                {
                    new KeyValuePair<string, string>("OTP", otp)
                };

                var mailData = new EmailData
                {
                    To = otpSendData.MailData.To,
                    From = otpSendData.MailData.From,
                    Subject = otpSendData.MailData.Subject,
                    Body = otpSendData.MailData.Body
                };

                return _mailService.Send(mailData, data);
            }

            return true;
        }

    }
}