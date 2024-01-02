using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model;
using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Services;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Sitecore.Data.Items;
using Sitecore.Extensions;
using Sitecore.Mvc.Extensions;
using Sitecore.Mvc.Presentation;
using Sitecore.Web.UI.XslControls;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web.Mvc;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Controllers
{
    public class CallBackFormController : Controller
    {
        private readonly ILogRepository logRepository;
        private readonly ICallBackRepository callBackRepository;


        public CallBackFormController(ICallBackRepository _callBackRepository, ILogRepository _logRepository)
        {
            this.callBackRepository = _callBackRepository;
            this.logRepository = _logRepository;
        }

        // GET: FeebackForm
        [HttpGet]
        public ActionResult SubmitForm()
        {
            Item currentItem = RenderingContext.Current.Rendering.Item;
            ViewBag.SuccessMessage = TempData["Message"];
            CallBackFormModel model = new CallBackFormModel();
            try
            {
                model.Labels = callBackRepository.GetCallBackFormItem(currentItem);
                TempData["InitialSuccessMessage"] = model.Labels.SuccessMessage;
                TempData["InitialFailureMessage"] = model.Labels.FailureMessage;
            }
            catch (Exception ex)
            {
                logRepository.Error(ex.Message);
            }

            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        public ActionResult SubmitForm(CallBackFormModel formInput, string pageId, string commercialEmails, string airlinePartnershipEmails)
        {
            string url = HttpContext.Request.UrlReferrer.AbsolutePath.ToString();
            CallBackFormModel model = new CallBackFormModel();
            List<ApiResponse> response = new List<ApiResponse>();
            if (formInput != null)
            {
                try
                {
                    Sitecore.Globalization.Translate.ResetCache(true);
                    formInput.Input.app = "adanione";
                    formInput.Input.category = "admin";
                    formInput.Input.occasion = "admin-airport-migration";
                    List<string> commerialEmailList = commercialEmails.Split(',').ToList();
                    List<string> airlinePartnershipEmailList = airlinePartnershipEmails.Split(',').ToList();
                    formInput.Input.to = new List<string>();
                    switch (pageId.ToLower())
                    {
                        case "cargo":
                            if(commerialEmailList.Count > 0)
                            formInput.Input.to.AddRange(commerialEmailList);
                            break;

                        case "airline-partnership":
                            if(airlinePartnershipEmailList.Count > 0)
                            formInput.Input.to.AddRange(airlinePartnershipEmailList);
                            break;
                    }
                    formInput.Input.data.subject.text = "";
                    if(!ModelState.IsValid)
                    {
                        TempData["Message"] = "Error. Try Again";
                        return Redirect(url);
                    }

                    if (formInput.Input.to.Count > 0)
                    {
                        response = callBackRepository.GetEmailResponse(formInput.Input);
                    }
                }
                catch (Exception ex)
                {
                    logRepository.Error(ex.Message);
                }
            }

            if (response != null && response.Count > 0)
            {
                TempData["Message"] = TempData["InitialSuccessMessage"];
                return Redirect(url);
            }
            else
            {
                TempData["Message"] = TempData["InitialFailureMessage"];
                return Redirect(url);
            }
        }
    }
}