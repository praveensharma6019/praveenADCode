using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Feature.Template.Models;
using Sitecore.Feature.Template.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Sitecore.Feature.Template.Controllers
{
    public class TemplateItemController : Controller
    {
        // GET: Template
       private readonly ITemplateItemService _templateItemService;

        public TemplateItemController()
        {
            _templateItemService = ServiceLocator.ServiceProvider.GetService<ITemplateItemService>();
        }
        //public TemplateItemController(ITemplateItemService templateItemService)
        //{
        //    _templateItemService = templateItemService;
        //}
        [HttpPost]
        public ActionResult CreateItem(TemplateFieldModel templateFieldModel)
        {
            Log.Info("CreateItem Model Received fromPost Call templateFieldModel:" + templateFieldModel, templateFieldModel);
            var serilizeModel = JsonConvert.SerializeObject(templateFieldModel);
            Log.Info("CreateItem Model Received fromPost Call serilizeModel:" + serilizeModel, serilizeModel);
            // var response = "Success";
            if (!ID.TryParse(templateFieldModel.templateId, out ID templateItemId))
            {
                // Handle invalid template ID
                var Validresponse = "Invalid template ID";
                Log.Info("CreateItem Model Received Validresponse:" + Validresponse, Validresponse);
                return Json(Validresponse);

            }
            if (!ModelState.IsValid)
            {
                Log.Info("CreateItem Model Invalid:" + ModelState.IsValid, ModelState.IsValid);
                Response.StatusCode = 400;
                var errors = GetErrors(ModelState);
                return Json(new TemplateResponseModel { Success = false, Errors = errors });
            }
            Log.Info("CreateItem Model Valid", string.Empty);
            var response = new TemplateResponseModel();
            // Ensure the template ID is valid
            try
            {
                Log.Info("CreateItem Model in try case:" + templateFieldModel.templateFields, templateFieldModel.templateFields);
                var deserializedFields = JsonConvert.DeserializeObject<TemplateFields>(templateFieldModel.templateFields);
                var templateData = new TemplateData
                {
                    parentItem = templateFieldModel.parentItem,
                    templateId = templateFieldModel.templateId,
                    newItemName = templateFieldModel.newItemName,
                    templateFields = deserializedFields
                };
                Log.Info("deserializedFields TemplateData" + JsonConvert.SerializeObject(templateData), templateData);
                response.Success = _templateItemService.CreateItem(templateData);
                Log.Info("TemplateService Response" + response.Success, response.Success);
                if (!response.Success)
                {
                    Log.Info("CreateItem Model Received fromPost Call responseSuccess:" + response.Success, response.Success);
                    Response.StatusCode = 400;
                    return Json(new { success = false, message = "Some error occured in creating item. Please connect with administrator." });
                }
                return Json(response);
            }
            catch (Exception ex)
            {
                Log.Info("Exception in TemplateController CreateItem Method: " + ex, ex);
                throw ex;
            }
        }

        [HttpPost]
        public ActionResult UpdateItem(TemplateFieldModel templateFieldModel)
        {
            //var response = "Success";
            //Use a security disabler to allow changes
            try 
            {
                if (!ModelState.IsValid)
                {
                    Log.Info("UpdateItem Model Invalid", string.Empty);
                    Response.StatusCode = 400;
                    var errors = GetErrors(ModelState);
                    return Json(new TemplateResponseModel { Success = false, Errors = errors });
                }
                Log.Info("UpdateItem Model valid", string.Empty);
                var response = new TemplateResponseModel();
                var deserializedFields = JsonConvert.DeserializeObject<TemplateFields>(templateFieldModel.templateFields);
                var templateData = new TemplateData
                {
                    itemPath = templateFieldModel.itemPath,
                    templateFields = deserializedFields
                };
                Log.Info("UpdateItem DeserializedFields TemplateData" + JsonConvert.SerializeObject(templateData), templateData);
                response.Success = _templateItemService.UpdateItem(templateData);
                Log.Info("UpdateItem TemplateService Response" + response.Success, response.Success);
                if (!response.Success)
                {
                    Response.StatusCode = 400;
                    return Json(new { success = false, message = "Some error occured in updating item. Please connect with administrator." });
                }
                return Json(response);
            }
            catch(Exception ex)
            {
                Log.Info("Exception in TemplateController UpdateItem Method: " + ex, ex);
                throw ex;
            }
            
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