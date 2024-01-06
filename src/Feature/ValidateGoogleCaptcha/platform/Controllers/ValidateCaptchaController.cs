using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using Sitecore.Feature.ValidateGoogleCaptcha.Models;
using Sitecore.Feature.ValidateGoogleCaptcha.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Sitecore.Feature.ValidateGoogleCaptcha.Controllers
{
    public class ValidateCaptchaController : Controller
    {
        private readonly IValidateCaptchaService _validateCaptchaService;

        public ValidateCaptchaController()
        {
            _validateCaptchaService = ServiceLocator.ServiceProvider.GetService <IValidateCaptchaService>();
        }

        // GET API endpoint to validate reCAPTCHA
        //public async Task<bool> ValidateReCaptcha(CaptchaRequestModel request)
        //{
        //    //bool isValid = await _validateCaptchaService.IsReCaptchValidAsync(request);

        //    var captchaRequestData = new CaptchaRequestModel();
        //    var captchaData = new CaptchaData
        //    {
        //        Request = request.reResponse
        //    };

        //   var response= _validateCaptchaService.IsReCaptchValidAsync(captchaData);

        //    //var captchaResponse = new ReCaptchaResponse
        //    //{
        //    //    IsValid = isValid
        //    //};

        //    //return Ok(captchaResponse);
        //    if (response)
        //    {
        //        return true;
        //    }
        //    return false;
        //}
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
            var captchaData = new CaptchaData
            {
                Request = request.reResponse
            };

            response.Success = _validateCaptchaService.VerifyCaptcha(captchaData);
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
            var captchaData = new CaptchaData
            {
                Request = request.reResponse
            };

            response.Success = _validateCaptchaService.IsV3CaptchValid(captchaData);
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