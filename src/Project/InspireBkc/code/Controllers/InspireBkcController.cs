using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.InspireBkc.Website;
using Sitecore.InspireBkc.Website.Models;

public class InspireBkcController : Controller
{
	private InspireBkcRepository BkcRepo = new InspireBkcRepository();

	public ActionResult Index()
	{
        return View();
    }
    public bool IsReCaptchValid(string reResponse)
    {
        bool flag = false;
        string str = reResponse;
        string str1 = DictionaryPhraseRepository.Current.Get("/CaptachaKey/SecretKey", "6Lcql9QZAAAAAOSIsZ-9gNRiZaVPrDmSrbKSe3y2");
        string str2 = string.Format("https://www.google.com/recaptcha/api/siteverify?secret={0}&response={1}", str1, str);
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
	public ActionResult Insertcontactdetail(Inspire m)
	{
		var result = new
		{
			status = "1"
		};
		try
		{
            var otpResult = this.VerifyOTPResult(m);
            if(!otpResult)
            {
                result = new{status = "410"};
                return Json(result, JsonRequestBehavior.AllowGet);
            }

            InspireBkcDataContextDataContext rdb = new InspireBkcDataContextDataContext();

			InspireBKCFormData r = new InspireBKCFormData();
			r.FirstName = m.first_name;
			r.LastName = m.last_name;
			r.Mobile = m.mobile;
			r.Email = m.email;
			r.City = m.City;
			r.FormType = m.FormType;
			r.PageInfo = m.PageInfo;
			r.FormSubmitOn = DateTime.Now;
			r.UTMSource = m.UTMSource;
			r.UTMMedium = m.UTMMedium;
			r.UTMCampaign = m.UTMCampaign;
			r.UTMContent = m.UTMContent;
			r.UTMTerm = m.UTMTerm;
			rdb.InspireBKCFormDatas.InsertOnSubmit(r);
			rdb.SubmitChanges();
			Log.Info("Enquire now - Data added to DB Start first_name:" + m.first_name, (object)this);
			Log.Info("Enquire now - Data added to DB last_name:" + m.last_name, (object)this);
			Log.Info("Enquire now - Data added to DB Mobile: " + m.mobile, (object)this);
			Log.Info("Enquire now - Data added to DB email: " + m.email, (object)this);
			Log.Info("Enquire now - Data added to DB PageInfo: " + m.PageInfo, (object)this);
			Log.Info("Enquire now - Salesforce lead generation starting", (object)this);
			Log.Info("SalesForceWrapperObj created with token", (object)this);
		}
		catch (Exception ex)
		{
			result = new
			{
				status = "fail"
			};
			Console.Write(ex);
		}
        return Json(result, JsonRequestBehavior.AllowGet);
    }

    public bool VerifyOTPResult(Inspire model)
    {
        InspireBkcDataContextDataContext dc = new InspireBkcDataContextDataContext();
        string generatedOTP = BkcRepo.GetOTP(model.mobile);
        if (string.Equals(generatedOTP, model.OTP))
        {
    
            InspireBkcOtpHistory data = dc.InspireBkcOtpHistories.Where((InspireBkcOtpHistory x) => x.MobileNumber == model.mobile).FirstOrDefault();
            data.status = true;
            dc.SubmitChanges();
            return true;
        }
        return false;
    }

    [HttpPost]
	public ActionResult VerifyOTP(Inspire model)
	{
		var result = new
		{
			status = "0"
		};
		InspireBkcDataContextDataContext dc = new InspireBkcDataContextDataContext();
		string generatedOTP = BkcRepo.GetOTP(model.mobile);
		if (string.Equals(generatedOTP, model.OTP))
		{
			result = new
			{
				status = "1"
			};
			InspireBkcOtpHistory data = dc.InspireBkcOtpHistories.Where((InspireBkcOtpHistory x) => x.MobileNumber == model.mobile).FirstOrDefault();
			data.status = true;
			dc.SubmitChanges();
		}
        return Json(result, JsonRequestBehavior.AllowGet);
    }

	[HttpPost]
	public ActionResult genrateOtp(Inspire model)
	{
		InspireBkcDataContextDataContext dc = new InspireBkcDataContextDataContext();
		var result = new
		{
			status = "0"
		};
		try
		{
            var captchaValidResult = this.IsReCaptchValid(model.reResponse);
            if(!captchaValidResult)
            {
                result = new { status = "412" };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
            //BkcRepo.DeleteOldOtp(model.mobile);
            string generatedotp = BkcRepo.StoreGeneratedOtp(model);
            if(generatedotp=="411")
            {
                result = new
                {
                    status = "411"
                };
                return Json(result, JsonRequestBehavior.AllowGet);
            }
			try
			{
				string apiurl = $"http://2factor.in/API/V1/30e6bf20-419b-11ea-9fa5-0200cd936042/SMS/{model.mobile}/{generatedotp}";
				HttpClient client = new HttpClient();
				client.BaseAddress = new Uri(apiurl);
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
				HttpResponseMessage response = client.GetAsync(apiurl).Result;
				if (response.IsSuccessStatusCode)
				{
					Log.Error("OTP Api call success. https://enterprise.smsgupshup.com/", (object)this);
					result = new
					{
						status = "1"
					};
				}
				else
				{
					Log.Error("OTP Api call failed. https://enterprise.smsgupshup.com/", (object)this);
					result = new
					{
						status = "2"
					};
				}
			}
			catch (Exception ex2)
			{
				Log.Error($"{0}", ex2, (object)this);
			}
            return Json(result, JsonRequestBehavior.AllowGet);
        }
		catch (Exception ex)
		{
			Log.Error($"{0}", ex, (object)this);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
	}
}
