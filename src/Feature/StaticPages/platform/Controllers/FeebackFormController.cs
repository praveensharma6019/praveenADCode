using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Model;
using Adani.SuperApp.Airport.Feature.StaticPages.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.Logging.Platform.Repositories;
using Adani.SuperApp.Airport.Foundation.SitecoreHelper.Platform.Helper;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Mvc.Controllers;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Routing;

namespace Adani.SuperApp.Airport.Feature.StaticPages.Platform.Controllers
{
    public class FeebackFormController : SitecoreController
    {
        private readonly ILogRepository logRepository;
        private readonly IFeedbackRepository feedbackRepository;
        

        public FeebackFormController(IFeedbackRepository _feedbackRepository, ILogRepository _logRepository)
        {
            this.feedbackRepository = _feedbackRepository;
            this.logRepository = _logRepository;
        }

        // GET: FeebackForm
        public ActionResult SubmitForm()
        {
            Item currentItem = RenderingContext.Current.Rendering.Item;

            SubmitViewModel submitViewModel = new SubmitViewModel();

            try
            {
                submitViewModel = feedbackRepository.GetSubmitFormItem(currentItem, submitViewModel);

            }
            catch (Exception ex)
            {

                this.logRepository.Error(ex.Message);
            }

            return View(Constants.SubmitView, submitViewModel);
        }

        [HttpPost]
        public JsonResult SubmitForm(FeedbackForm feedbackForm)
        {
            

            FeedbackFormResponse feedbackFormResponse = new FeedbackFormResponse();
            

            if (feedbackForm != null)
            {
                try
                {
                    feedbackFormResponse
                         = feedbackRepository.GetSubmitFormDetails(feedbackForm);
                    
                }
                catch (Exception ex)
                {

                    this.logRepository.Error(ex.Message);
                }
            }

            return Json(feedbackFormResponse);
        }

        
    }
}