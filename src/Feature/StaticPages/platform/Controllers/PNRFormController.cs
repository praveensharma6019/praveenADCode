
using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model;
using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Newtonsoft.Json.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Extensions;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.HtmlControls;
using Sitecore.Web.UI.XslControls;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Controllers
{
    public class PNRFormController : Controller
    {
        private readonly ILogRepository logRepository;
        private readonly IPNRRepository pNRRepository;


        public PNRFormController(IPNRRepository _pNRRepository, ILogRepository _logRepository)
        {
            this.pNRRepository = _pNRRepository;
            this.logRepository = _logRepository;
        }

        // GET: FeebackForm
        [HttpGet]
        public ActionResult SubmitForm()
        {
            Item currentItem = RenderingContext.Current.Rendering.Item;
            PNRFormModel model = new PNRFormModel();
            ViewBag.message = TempData["message"] as string;
            try
            {
                model.Labels = pNRRepository.GetPNRFormItem(currentItem);
            }
            catch (Exception ex)
            {
                logRepository.Error(ex.Message);
            }

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SubmitForm(PNRFormModel formInput)
        {
            PNRFormResponse response = new PNRFormResponse();
            string url = HttpContext.Request.UrlReferrer.AbsolutePath.ToString();
            try
            {
                if (!ModelState.IsValid)
                {
                    TempData["message"] = "Error. Try again.";
                    return Redirect(url);
                }
                if (formInput != null)
                {
                    //For Regex Implementation
                    //formInput.Input.reCaptcha = Request.Form["pnr-form-captcha-response"].ToString();
                    //if (!pNRRepository.IsReCaptchValid(formInput.Input.reCaptcha))
                    //{
                    //    TempData["message"] = "Invalid Captcha";
                    //    return Redirect(url);
                    //}
                    System.DateTime dateTime = System.DateTime.Now;
                    string format = "yyyy-MM-dd";
                    if (!System.DateTime.TryParseExact(formInput.Input.travelDate, format, CultureInfo.InvariantCulture,
                    DateTimeStyles.None, out dateTime))
                    {
                        TempData["message"] = "Invalid date";
                        return Redirect(url);
                    }
                }

                response = pNRRepository.GetPNRResponse(formInput.Input);
            }

            catch (Exception ex)
            {
                logRepository.Error(ex.Message);
            }

            if (response.status)
            {
                TempData["message"] = Sitecore.Globalization.Translate.Text("Thank You for filling the details. Your Promocode is on the way. You will receive it shortly on your mentioned email");
                return Redirect(url);
            }
            else if (response.status == false)
            {
                TempData["message"] = Sitecore.Globalization.Translate.Text("Seems like you have already availed this benefit.");
                return Redirect(url);
            }
            else
            {
                TempData["message"] = "Form Submission Failed";
                return Redirect(url);
            }
        }
    }
}