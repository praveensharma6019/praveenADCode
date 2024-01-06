using Sitecore.AdaniSolar.Website.Models;
using Sitecore.Foundation.Dictionary.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace Sitecore.AdaniSolar.Website.Services
{
    public class SolarWarrantyService
    {
        public string GetCertificatedata(string jsonObject = null)
        {
            var client = new HttpClient();
            var authorizationVal = DictionaryPhraseRepository.Current.Get("/WarrantyCertificate/Authorization Value Api", "Basic SU5URl9NU1BWTF9TdW5TaW06QWRhbmlAMSRNU1BWTA==");
            var cookieVal = DictionaryPhraseRepository.Current.Get("/WarrantyCertificate/Cookie value Api", "JSESSIONID=_UT-GVygRGFScDjA2b16Wj56BoVWiAFygkcA_SAP-Sb03e0SzzhRrSYtZj1AzPSJ; saplb_*=(J2EE4686420)4686450");
            string apiUrl = DictionaryPhraseRepository.Current.Get("/WarrantyCertificate/Api Url", "https://aipoqdmz.adani.com:443/RESTAdapter/WarrantyCertificate_MSPVL");
            var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
            request.Headers.Add("Authorization", authorizationVal);
            request.Headers.Add("Cookie", cookieVal);
            var content = new StringContent(jsonObject, null, "application/json");
            request.Content = content;
            var response = client.SendAsync(request);
            var jsonresponse = response.Result.Content.ReadAsStringAsync().Result;
            return jsonresponse;
        }

        public bool SaveData(SolarConsumerModel model)
        {
            using (AdaniSolarWarrantyConsumerDataContext dbcontext = new AdaniSolarWarrantyConsumerDataContext())
            {
                AdaniSolarWarrantyCertificateConsumer ObjReg = new AdaniSolarWarrantyCertificateConsumer();
                ObjReg.Id = Guid.NewGuid();
                ObjReg.UserName = model.UserName;
                ObjReg.Address = model.Address;
                ObjReg.Country = model.Country;
                ObjReg.State = model.State;
                ObjReg.City = model.City;
                ObjReg.Email = model.Email;
                ObjReg.MobileNumber = model.Mobile;
                ObjReg.FormSubmitOn = DateTime.Now; //model.FormSubmitOn;
                dbcontext.AdaniSolarWarrantyCertificateConsumers.InsertOnSubmit(ObjReg);
                dbcontext.SubmitChanges();
                return true;
            }
            return false;
        }

        public string RenderRazorViewToString(Controller controller, string viewName)
        {
            //controller.ViewData.Model = model;
            var viewResult = ViewEngines.Engines.FindPartialView(controller.ControllerContext, viewName);
            // checking the view inside the controller  
            if (viewResult.View != null)
            {
                using (var sw = new StringWriter())
                {
                    var viewContext = new ViewContext(controller.ControllerContext, viewResult.View, controller.ViewData, controller.TempData, sw);
                    viewResult.View.Render(viewContext, sw);
                    viewResult.ViewEngine.ReleaseView(controller.ControllerContext, viewResult.View);
                    return sw.GetStringBuilder().ToString();
                }
            }
            else
                return "View cannot be found.";
        }
    }
}