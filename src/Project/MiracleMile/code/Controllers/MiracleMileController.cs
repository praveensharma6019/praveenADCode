using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.MiracleMile.Website.Models;

namespace Sitecore.MiracleMile.Website.Controllers
{
    public class MiracleMileController : Controller
    {
        // GET: MiracleMile
        public ActionResult Index()
        {
            return View();
        }
        public bool IsReCaptchValid(string reResponse)
        {
            bool flag = false;
            string str = reResponse;
            //string str1 = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "6Lcql9QZAAAAAOSIsZ-9gNRiZaVPrDmSrbKSe3y2");
            string str2 = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", "6Lf6VgUgAAAAAFq77YhGWeUDGGlXskkJvvPqBQjJ", str);
            using (WebResponse response = ((HttpWebRequest)WebRequest.Create(str2)).GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    flag = (JObject.Parse(streamReader.ReadToEnd()).Value<bool>("success") ? true : false);
                }
            }
            return flag;
        }
        [HttpPost]
        public ActionResult Insertcontactdetail(Miracle m)
        {
            var result = new { status = "1" };
            try
            {
                var flag = this.IsReCaptchValid(m.reResponse);
                if(flag==false)
                {
                    result = new { status = "2" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                MiracleMileFormDataContext rdb = new MiracleMileFormDataContext();
                MiracleMileFormData r = new MiracleMileFormData();

                r.FirstName = m.first_name;
                r.LastName = m.last_name;
                r.Mobile = m.mobile;
                r.Email = m.email;
                r.City = m.City;
                r.Country = m.Country;
                r.FormType = m.FormType;
                r.PageInfo = m.PageInfo;
                r.FormSubmitOn = DateTime.Now;// m.FormSubmitOn;
               

                #region Insert to DB
                rdb.MiracleMileFormDatas.InsertOnSubmit(r);

                rdb.SubmitChanges();
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Start first_name:" + m.first_name, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB last_name:" + m.last_name, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, this);
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB email: " + m.email, this);
               
                Sitecore.Diagnostics.Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, this);

                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation starting", this);
                SalesForceWrapper SalesForceWrapperObj = new SalesForceWrapper();
                //Get project details
                Sitecore.Diagnostics.Log.Info("SalesForceWrapperObj created with token", this);
                LeadObject obj = new LeadObject
                {
                    Firstname = m.first_name,
                    FormType = m.FormType,
                    PageInfo = m.PageInfo,
                    LastName = m.last_name,
                    Email = m.email,
                    Mobile = m.mobile,
                    Country = m.Country,
                    
                    
                };

                Sitecore.Diagnostics.Log.Info("Enquire now - Salesforce lead generation - generate lead", this);
                var leadResult = SalesForceWrapperObj.GenerateLead(obj);
            }
            catch (Exception ex)
            {
                result = new { status = "fail" };
                Console.Write(ex);
            }
            #endregion
            return Json(result, JsonRequestBehavior.AllowGet);
        }

    }
}