using Adani.Feature.Common.Models;
using Adani.Foundation.Messaging.Models;
using Adani.Foundation.Messaging.Services.Email;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Adani.Feature.Common.Controllers
{
    [Route("email")]
    public class SendEmailController : Controller
    {
        private readonly IEmailService _emailService;

        public SendEmailController()
        {
            _emailService = ServiceLocator.ServiceProvider.GetService<IEmailService>();
        }

        /// <summary>
        /// Payload sample
        /// {
        ///     "to": "string",
        ///     "from": "string",
        ///     "cc": "string",
        ///     "bcc": "string",
        ///     "subject": "string",
        ///     "body": "string",
        ///     "params": [
        ///       {
        ///         "name": "name",
        ///         "value": "shaukat"
        ///       }
        ///     ]
        /// }
        /// params is an array of key value pair. if available it will try to replace the token value in email body.
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost]
        [ActionName("send")]
        public ActionResult Post(EmailSendRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = GetErrors(ModelState);
                return Json(new EmailSendResponseModel { Success = false, Errors = errors });
            }

            var response = new EmailSendResponseModel();
            var @params = request.Params?.Select(x => new KeyValuePair<string, string>(x.Name, x.Value));
            var emailData = new EmailData
            {
                To = request.To,
                From = request.From,
                Subject = request.Subject,
                Body = request.Body,
                Bcc = request.Bcc,
                Cc = request.Cc
            };

            response.Success = _emailService.Send(emailData, @params);
            if (!response.Success)
            {
                Response.StatusCode = 400;
                return Json(new { success = false, message = "Some error occured. Please connect with administrator." });
            }

            return Json(response);
        }

        public static IEnumerable<KeyValuePair<string, string>> GetErrors(ModelStateDictionary modelState)
        {
            var result = from ms in modelState
                         where ms.Value.Errors.Any()
                         let fieldKey = ms.Key
                         let errors = ms.Value.Errors
                         from error in errors
                         select new KeyValuePair<string, string>(fieldKey, error.ErrorMessage);

            return result;
        }
    }
}