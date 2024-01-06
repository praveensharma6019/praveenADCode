using Feature.FormsExtensions.Business.ReCaptcha;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Feature.FormsExtensions.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Feature.FormsExtensions.Controllers
{
    public class ValidateCaptchaController : Controller
    {
        private readonly IReCaptchaService _ReCaptchaService;

        public ValidateCaptchaController()
        {
            _ReCaptchaService = ServiceLocator.ServiceProvider.GetService<IReCaptchaService>();
        }


        [HttpPost]
        public ActionResult ValidateReCaptcha(CaptchaRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = GetErrors(ModelState);
                return Json(new CaptchaResponseModel { Success = false, Errors = errors });
            }

            var response = new CaptchaResponseModel();
            response.Success = _ReCaptchaService.VerifySync(request.reResponse.ToString(),request.SecretKey.ToString());
            if (!response.Success)
            {
                Response.StatusCode = 400;
                return Json(new { success = false, message = "Some error occured. Please connect with administrator." });
            }

            return Json(response);
        }

        [HttpPost]
        public ActionResult ValidateReCaptchaV3(CaptchaRequestModel request)
        {
            if (!ModelState.IsValid)
            {
                Response.StatusCode = 400;
                var errors = GetErrors(ModelState);
                return Json(new CaptchaResponseModel { Success = false, Errors = errors });
            }

            var response = new CaptchaResponseModel();
            response.Success = _ReCaptchaService.IsV3CaptchValid(request.reResponse.ToString(), request.SecretKey.ToString());
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