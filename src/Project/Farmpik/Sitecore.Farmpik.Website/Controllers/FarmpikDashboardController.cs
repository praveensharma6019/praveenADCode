using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Farmpik.Domain.Commands.ImportFileCommands;
using Farmpik.Domain.Interfaces.Services;
using Sitecore.Farmpik.Website.ViewModel;
using log4net;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Farmpik.Domain.Dto;

namespace Sitecore.Farmpik.Website.Controllers
{
    
    //[SessionState(System.Web.SessionState.SessionStateBehavior.Required)]
    public class FarmpikDashboardController : Controller
    {
        private readonly ILog _logger;
        private readonly IEmployeeLoginBusinessService _loginBusinessService;
        private readonly IImportFileBusinessService _importFileBusinessService;

        public FarmpikDashboardController(IEmployeeLoginBusinessService loginBusinessService,
            IImportFileBusinessService importFileBusinessService)
        {
            _logger = LogManager.GetLogger(typeof(FarmpikDashboardController));
            _loginBusinessService = loginBusinessService;
            _importFileBusinessService = importFileBusinessService;
        }


        public ActionResult Dashobard()
        {
            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            try
            {
                if (!(User?.Identity?.IsAuthenticated ?? false) || Session["SessionId"] == null || Session["SessionId"].ToString() != User.Identity.Name)
                {
                    return Redirect("/DashboardAPI/Login");
                }
                Sitecore.Diagnostics.Log.Info("FarmpikDashboardController : Dashobard", this);
                if (TempData["DashboardView"] != null)
                {
                    Sitecore.Diagnostics.Log.Info("FarmpikDashboardController : Dashobard 1", this);
                    dashboardViewModel = Newtonsoft.Json.JsonConvert.DeserializeObject<DashboardViewModel>(System.Convert.ToString(TempData["DashboardView"]));
                }                            
                else
                {
                    Sitecore.Diagnostics.Log.Info("FarmpikDashboardController : Dashobard 3", this);
                    return RedirectToRoute("Sitecore.Farmpik.Website.FarmpikDashboard.Index");
                }
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("FarmpikDashboardController : error", ex, this);
                _logger.Error(ex);
                return View("/Views/Farmpik/Sublayouts/ErrorView.cshtml");
            }
            ViewData["Title"] = "DashBoard";
            return View("/Views/Farmpik/Sublayouts/Dashboard.cshtml", dashboardViewModel);
        }

        [HttpGet]
        public async Task<ActionResult> Index()
        {
            try
            {
                if (!(User?.Identity?.IsAuthenticated ?? false) || Session["SessionId"] == null || Session["SessionId"].ToString() != User.Identity.Name)
                {
                    return Redirect("/DashboardAPI/Login");
                }
                Sitecore.Diagnostics.Log.Info("FarmpikDashboardController : Index", this);

                var employee = await _loginBusinessService.GetEmployee(Guid.Parse(User.Identity.Name));
                var importDetails = await _importFileBusinessService.GetImportDetails();
                var a = new DashboardViewModel
                    {
                    UserName = employee.FirstName,
                    Imports = importDetails
                };

                    var dashboard = JsonConvert.SerializeObject(a);
                    Diagnostics.Log.Info("FarmpikDashboardController : DashboardView", this);
                    TempData["DashboardView"] = dashboard;
                    Sitecore.Diagnostics.Log.Info("FarmpikDashboardController : DashboardView 1", this);
                  
               
                return Redirect("/DashboardAPI/Dashboard");


            }
            catch (Exception ex)
            {
                //throw ex;
                Sitecore.Diagnostics.Log.Error("FarmpikDashboardController : error", ex, this);
                _logger.Error(ex);
                return View("/Views/Farmpik/Sublayouts/ErrorView.cshtml");
            }
        }

        [HttpPost]
        [Authorize]
        public async Task<JsonResult> ImportTemplate(ImportFile importFile)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("FarmpikDashboardController Post: Inside try block of ImportTemplate", this);
                var template = await Import(importFile, Guid.Parse(User.Identity.Name));
                
                Sitecore.Diagnostics.Log.Info("Import Method executed successfully and returned:" + template.TotalRecords + " records", this);
                var details = await _importFileBusinessService.GetImportDetailsByTemplateName(importFile.TemplateName);

                Sitecore.Diagnostics.Log.Info("GetImportDetailsByTemplateName Method executed successfully and returned:" + details.TotalRecords + " records", this);
                if (details != null)
                {
                    details.TotalRecords = template.TotalRecords;
                    details.ErrorRecords = template.ErrorRecords;
                    details.IsValidTemplate = template.IsValidTemplate;                   
                }
                // Reflected XSS solu
                details.TemplateName = HttpUtility.HtmlEncode(details.TemplateName);
                details.LastImportedDate = DateTime.Parse(HttpUtility.HtmlEncode(details.LastImportedDate));
                details.NoOfUploads = int.Parse(HttpUtility.HtmlEncode(details.NoOfUploads));
                details.LastImportedUser = HttpUtility.HtmlEncode(details.LastImportedUser);
                details.TotalRecords = int.Parse(HttpUtility.HtmlEncode(details.TotalRecords));
                details.ErrorRecords = int.Parse(HttpUtility.HtmlEncode(template.ErrorRecords));
                details.IsValidTemplate = bool.Parse(HttpUtility.HtmlEncode(template.IsValidTemplate));

                return Json(details, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

                Sitecore.Diagnostics.Log.Info("Exception in importtemplate method, exception:" + ex, this);
                Sitecore.Diagnostics.Log.Error("FarmpikDashboardController Post : Error",ex, this);
                throw ex;
                //return Json(null, JsonRequestBehavior.AllowGet);
                //_logger.Error(ex);
                //return View("/Views/Farmpik/Sublayouts/ErrorView.cshtml");
            }
        }

        private async Task<TemplateDto<string>> Import(ImportFile importFile, Guid createdBy)
        {
            try
            {
                Sitecore.Diagnostics.Log.Info("Inside Import Method before switching", this);
                switch (importFile.TemplateName)
                {
                    case "Vendor Master": return await _importFileBusinessService.ImportVendorTemplate(importFile.ExcelFile, createdBy);
                    case "PRN Master": return await _importFileBusinessService.ImportPurchaseTemplate(importFile.ExcelFile, createdBy);
                    case "Price Master": return await _importFileBusinessService.ImportPriceTemplate(importFile.ExcelFile, createdBy);
                    case "Product details": return await _importFileBusinessService.ImportProductTemplate(importFile.ExcelFile, createdBy);
                    case "Payment status": return await _importFileBusinessService.ImportPaymentTemplate(importFile.ExcelFile, createdBy);
                }

                Sitecore.Diagnostics.Log.Info("Invalid template name", this);
                return null;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Info("Exception in Import method, exception:" + ex, this);
                return null;
            }
        }

    }

    public class ImportFile
    {
        [DataType(DataType.Upload)]
        public HttpPostedFileBase ExcelFile { get; set; }
        public string TemplateName { get; set; }
    }
}