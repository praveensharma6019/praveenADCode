using Adani.Feature.Common.Models;
using Adani.Feature.Common.Providers;
using Adani.Foundation.Messaging.Models;
using Adani.Foundation.Messaging.Services.Email;
using Adani.Foundation.Messaging.Services.EmailSender;
using Adani.Foundation.Messaging.Services.SMS;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
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
            Log.Info($"Generate Method Start: {otpSendData}", otpSendData);
            int length = otpSendData.Length ?? 6;//default to 6 char
            if (length > 8) length = 8; // max 8 char
            if (length < 4) length = 4; // min 4 char

            var byteArray = new byte[length];
            var cryptoServiceProvider = new RNGCryptoServiceProvider();
            cryptoServiceProvider.GetBytes(byteArray);

            var randomInteger = BitConverter.ToInt32(byteArray, 0);
            randomInteger = Math.Abs(randomInteger);

            var otp = randomInteger.ToString().Substring(0, length);
            Log.Info($"OTP Generated: {otp}", otp);
            if (_storage.Save(new OtpDataModel
            {
                ID = otpSendData.ID,
                OTP = otp,
                MobileNo = otpSendData.SMSData.Recipient,
                ExpireAt = DateTimeOffset.Now.AddMinutes(otpSendData.ExpiresInMinutes ?? 10), //default expiration time 10 minutes
            }))
            {
                bool success = SendSMS(otpSendData, otp);
                //success = success && SendSMS(otpSendData, otp);
                if (success)
                    return Json(new { success = true });
            }

            return Json(new { success = false, message = "Some error occured. Please connect with administrator." });
        }

        [HttpPost]
        [Route("verify")]
        public ActionResult Verify(string phone, string otp)
        {
            var storedOtp = _storage.Get(phone);
            var isValid = storedOtp != null && storedOtp.OTP == otp && storedOtp.ExpireAt > DateTimeOffset.Now;
            if (isValid == true)
            {
                _storage.Delete(phone);
            }
            return Json(new { success = isValid });
        }



        #region
        //public ActionResult Verify(string phone, string otp)
        //{
        //    var storedOtp = _storage.Get(phone);
        //    var isValid = storedOtp != null && storedOtp.OTP == otp && storedOtp.ExpireAt > DateTimeOffset.Now;
        //    if (isValid == true)
        //    {
        //        _storage.Delete(phone);
        //    }
        //    return Json(new { success = isValid });
        //}
        #endregion
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