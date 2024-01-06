using Farmpik.Domain.Commands.EmployeeCommands;
using Farmpik.Domain.Interfaces.Services;
using log4net;
using Sitecore.Farmpik.Website.ViewModel;
using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace Sitecore.Farmpik.Website.Controllers
{
    public class FarmpikLoginController : Controller
    {
        private readonly ILog _logger;
        private readonly IEmployeeLoginBusinessService _loginBusinessService;

        public FarmpikLoginController(IEmployeeLoginBusinessService loginBusinessService)
        {
            _logger = LogManager.GetLogger(typeof(FarmpikLoginController));
            _loginBusinessService = loginBusinessService;
        }
        // GET: Login
        public ActionResult Index()
        {
            if ((User?.Identity?.IsAuthenticated ?? false) && Session["SessionId"] != null && Session["SessionId"].ToString() == User.Identity.Name)
            {
                return RedirectToRoute("Sitecore.Farmpik.Website.FarmpikDashboard.Index");
            }

            var credential = new LoginViewModel();
            credential.IsSignOut = TempData["IsSignOut"] != null ? (bool)TempData["IsSignOut"] : false;
            if (TempData["InValidCredential"] != null && TempData["ErrorMessage"] != null)
            {
                credential.InValidCredential = true;
                credential.ErrorMessage = TempData["ErrorMessage"].ToString();
            }
            return View("/Views/Farmpik/Sublayouts/Login.cshtml", credential);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel credential)
        {
            Sitecore.Diagnostics.Log.Info("Farmpik Login Method", this);
            //return RedirectPermanent("https://www.microsoft.com/en-in");
            if (!ModelState.IsValid)
            {
                Sitecore.Diagnostics.Log.Info("Farmpik Login Method:ModelState", this);
                credential.Password = String.Empty;
                View("/Views/Farmpik/Sublayouts/Login.cshtml", credential);
            }
            try
            {
                if (_loginBusinessService == null)
                {
                    Sitecore.Diagnostics.Log.Info("Farmpik Login Method:_loginBusinessService", this);
                }
                else
                {
                    var employee = await _loginBusinessService.GetEmployee(new EmployeeLoginCommand
                    {
                        EmailId = credential.EmailId,
                        Password = credential.Password
                    });
                    if (employee != null && employee.Attempted == 0)
                    {
                        Sitecore.Diagnostics.Log.Info("Farmpik Login Method:employee", this);
                        FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, employee.Id.ToString(), DateTime.Now,
                                                           DateTime.Now.AddMinutes(30), true, employee.Id.ToString());
                        HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, FormsAuthentication.Encrypt(ticket));
                        cookie.Expires = ticket.Expiration;
                        cookie.Secure = true;
                        cookie.HttpOnly = true;
                        Response.Cookies.Add(cookie);
                        Session.Add("SessionId", employee.Id.ToString());
                        return RedirectToRoute("Sitecore.Farmpik.Website.FarmpikDashboard.Index");
                    }
                    else
                    {

                        Sitecore.Diagnostics.Log.Info("Farmpik Login Method:credential wrong", this);
                        TempData["ErrorMessage"] = employee != null && employee.Attempted >= 10 ? "Maximum attempts reached. retry in 6 hours" : "Invalid Credentials!";
                        TempData["InValidCredential"] = true;
                        return Redirect("/DashboardAPI/Login");
                    }
                }

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Farmpik Login Method:employee", ex, this);

                _logger.Error(ex);
            }

            credential.Password = String.Empty;
            return View("/Views/Farmpik/Sublayouts/Login.cshtml", credential);
        }

        [HttpGet]
        public ActionResult SignOut()
        {
            FormsAuthentication.SignOut();
            Session.Remove("SessionId");
            TempData["IsSignOut"] = true;
            return Redirect("/DashboardAPI/Login");
        }
    }
}