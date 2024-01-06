using Rotativa;
using Sitecore.Diagnostics;
using Sitecore.Electricity.Website.Model;
using Sitecore.Electricity.Website.Services;
using Sitecore.Electricity.Website.Utility;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Foundation.SitecoreExtensions.Extensions;
using System;
using System.Web;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Specialized;
using Newtonsoft.Json;
using Sitecore.Data;
using System.Globalization;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Reflection;
using Sitecore.Tasks;
using System.Drawing.Imaging;
using System.Text.RegularExpressions;

namespace Sitecore.Electricity.Website.Controllers
{
    public class FeedbackController : Controller
    {
        [HttpGet]
        public ActionResult UserFeedback()
        {
            FeedbackModel model = new FeedbackModel();

            if (Session["Message"] != null && !string.IsNullOrEmpty(Session["Message"].ToString()))
            {
                FeedbackResult result = (FeedbackResult)Session["Message"];
                model.IsInputCorrect = false;
                model.ErrorMessage = result.Message;
                return View(model);
            }

            try
            {
                if (Request.QueryString["sr_number"] != null && Request.QueryString["source"] != null && Request.QueryString["type"] != null)
                {
                    string sr_number = Request.QueryString["sr_number"].ToString();
                    string source = Request.QueryString["source"].ToString();
                    string feedbackType = Request.QueryString["type"].ToString();

                    Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                    var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                    string userid = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserID].Value; //"Tmservice";
                    string password = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserPassword].Value; // "pass@123";
                    string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.EncryptionKey].Value; // "@Aeml#2020";
                    clsEncryptAES objcrypt = new clsEncryptAES(EncryptionKey);

                    //sr_number = objcrypt.EncryptText("1001");
                    //source = objcrypt.EncryptText("1");
                    //feedbackType = objcrypt.EncryptText("3");

                    //M5KMrsL65RE7R8KAkKBgbw==  3
                    //o9abINN6caFksfJN9pkTFw==  4
                    //nvWg2PFTE5luHMLEVHO3Wg==  5


                    model = new FeedbackModel(objcrypt.DecryptText(feedbackType));
                    model.SR_Number = objcrypt.DecryptText(sr_number);
                    model.Source = objcrypt.DecryptText(source);
                    model.FeedbackType = objcrypt.DecryptText(feedbackType);

                    var ttt = objcrypt.encodetext(DateTime.Now.ToString("MMddyyyyHHmm"));


                    try
                    {
                        DateTime createdDate = DateTime.ParseExact(model.Source, "MMddyyyyHHmm", null);
                        //createdDate = DateTime.Parse(createdDate.ToString("dd/MM/yyyy HH:mm"));

                        if (DateTime.Now <= createdDate.AddHours(24))
                        {
                            model.IsInputCorrect = true;
                        }
                        else
                        {
                            model.IsInputCorrect = false;
                            model.ErrorMessage = DictionaryPhraseRepository.Current.Get("/FeedbackModule/Link Expired", "The page is not accessible to you as the link is expired, please contact Adani Electricity!");
                        }
                    }
                    catch
                    {
                        model.IsInputCorrect = false;
                    }
                }
                else
                {
                    model.IsInputCorrect = false;
                    model.ErrorMessage = DictionaryPhraseRepository.Current.Get("/FeedbackModule/Landing Error Message", "The page is not accessible to you, please contact Adani Electricity!");
                }
            }
            catch (Exception e)
            {
                Log.Error("Error at Feedback Get: " + e.Message, this);
                model.ErrorMessage = DictionaryPhraseRepository.Current.Get("/FeedbackModule/Landing Error Message", "The page is not accessible to you, please contact Adani Electricity!");
                model.IsInputCorrect = false;
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult UserFeedback(FeedbackModel model, FormCollection form, string SubmitAndClose = null, string SubmitAll = null)
        {
            if (!this.ModelState.IsValid)
            {
                return View(model);
            }

            if (string.IsNullOrWhiteSpace(Request.Form["g-recaptcha-response"]))
            {
                ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha required", "Captcha Validation Required."));
                return this.View(model);
            }
            ReCaptchaResponse reCaptchaResponse = VerifyCaptcha(DictionaryPhraseRepository.Current.Get("/sitecore/content/Electricity/Global/Dictionary/Common/CaptchaKey", "6LdYFWgUAAAAAF9HgE8x42aPsoOe6Gf81WV819BC"), Request.Form["g-recaptcha-response"]);

            if (!reCaptchaResponse.success)
            {
                ModelState.AddModelError(nameof(model.Captcha), DictionaryPhraseRepository.Current.Get("/Accounts/Controller Messages/Captcha failure", "Captcha Validation Failed."));
                return this.View(model);
            }

            bool MandatoryanswerGiven = true;
            if (string.IsNullOrEmpty(model.OverallExperience))
            {
                this.ModelState.AddModelError(nameof(model.OverallExperience), DictionaryPhraseRepository.Current.Get("/FeedbackModule/Enter value", "Please select your answer"));
                MandatoryanswerGiven = false;
            }
            if (string.IsNullOrEmpty(model.Attitude_Empathy))
            {
                this.ModelState.AddModelError(nameof(model.Attitude_Empathy), DictionaryPhraseRepository.Current.Get("/FeedbackModule/Enter value", "Please select your answer"));
                MandatoryanswerGiven = false;
            }
            //if (string.IsNullOrEmpty(model.Quality))
            //{
            //    this.ModelState.AddModelError(nameof(model.Quality), DictionaryPhraseRepository.Current.Get("/FeedbackModule/Enter value", "Please select your answer"));
            //    MandatoryanswerGiven = false;
            //}
            if (string.IsNullOrEmpty(model.Process))
            {
                this.ModelState.AddModelError(nameof(model.Process), DictionaryPhraseRepository.Current.Get("/FeedbackModule/Enter value", "Please select your answer"));
                MandatoryanswerGiven = false;
            }
            if (!MandatoryanswerGiven)
            {
                return this.View(model);
            }
            if (!string.IsNullOrEmpty(SubmitAndClose) || !string.IsNullOrEmpty(SubmitAll))
            {
                //to do : save in database
                var r = Post(model);

                try
                {
                    using (TenderDataContext dbcontext = new TenderDataContext())
                    {
                        UserFeedbackInteractionData objUserFeedbackInteractionData = new UserFeedbackInteractionData
                        {
                            Sr_Number = model.SR_Number,
                            FeedbackType = model.FeedbackType,
                            AEML_could_be_doing_differently_ = model.AEML_could_be_doing_differently,
                            Attitude_Disatifaction_Reason = model.Attitude_Empathy_Unsatisfied_Ids != null && model.Attitude_Empathy_Unsatisfied_Ids.Count() > 0 ? string.Join(",", model.Attitude_Empathy_Unsatisfied_Ids) : "",
                            Attitude_Empathy = model.Attitude_Empathy,
                            CCC_intraction_with = "",
                            Concerns_addressed = model.Concerns_addressed,
                            Ease_of_register_concerns = model.Ease_of_register_concerns,
                            Informed_alternate_digital_channels = model.Informed_alternate_digital_channels,
                            Number_Contacted = model.Number_Contacted,
                            Other_Remark = model.AEML_could_be_doing_differently_Other_Text,
                            Overall_Experience = model.OverallExperience,
                            Process = model.Process,
                            Process_Disatifaction_reason = model.Process_Unsatisfied_Ids != null && model.Process_Unsatisfied_Ids.Count() > 0 ? string.Join(",", model.Process_Unsatisfied_Ids) : "",
                            Quality_Disatifaction_BackEnd = "",
                            Quality_Disatifaction_CC = model.Quality_Unsatisfied_Ids != null && model.Quality_Unsatisfied_Ids.Count() > 0 ? string.Join(",", model.Quality_Unsatisfied_Ids) : "",
                            Quality_Disatifaction_CCC = "",
                            Quality_Disatifaction_Digital = "",
                            Quality_Disatifaction_EMAIL = "",
                            Rate_Adani_Brand = "",
                            Rate_quality_cable_laying = "",
                            Rate_Responsivenes_Adan_Point_of_Contact = "",
                            Rate_Technology_Equipment = "",
                            Recomandation_scale_Adani_Electricity = model.Recomandation_scale_Adani_Electricity,
                            Quality = model.Quality,
                            Safety_practices = "",
                            Source = model.Source,
                            Timeliness_of_our_services = "",
                            Created_Date=DateTime.Now,
                            valuable_inputs = "",
                            IsSavedInSAP = r.IsSuccess,
                            SAPMessage = r.Message + "," + r.OutputMessage + "," + r.Exception
                        };
                        dbcontext.UserFeedbackInteractionDatas.InsertOnSubmit(objUserFeedbackInteractionData);
                        dbcontext.SubmitChanges();
                    }
                }
                catch (Exception e)
                {
                    Log.Error("UserFeedback error in db save: " + e.Message, this);
                }

                if (r.IsSuccess)
                {
                    r.Message = "Feedback submitted successfully.";
                    ViewBag.Message = r;
                    Session["Message"] = r;
                    //redirect to thank you page

                    var item = Context.Database.GetItem(Templates.FeedbackModel.ThankYouPage);
                    return this.Redirect(item.Url());
                }
                else
                {
                    model.IsInputCorrect = false;
                    model.ErrorMessage = string.IsNullOrEmpty(r.Message) ? DictionaryPhraseRepository.Current.Get("/FeedbackModule/Post Error Message", "An issue occured in Posting feedback data, please try again or contact Adani Electricity!") : r.Message;
                    r.Message = model.ErrorMessage;
                    Session["Message"] = r;
                    var item = Context.Database.GetItem(Templates.FeedbackModel.LandingPage);
                    return this.Redirect(item.Url());
                }
                //save in database
                //post to service
                //show thanks you page
            }
            //if (!string.IsNullOrEmpty(SubmitAll))
            //{
            //    var r = Post(model);
            //    ViewBag.Message = r;
            //    Session["Message"] = r;
            //    //check all questions are answered
            //    //save in database
            //    //post to service
            //    //show thanks you page
            //}
            return View(model);
        }

        public FeedbackResult Post(FeedbackModel model)
        {
            FeedbackResult result = new FeedbackResult();
            try
            {
                Sitecore.Data.Database dbWeb = Sitecore.Configuration.Factory.GetDatabase("web");
                var itemInfo = dbWeb.GetItem(new Data.ID(Templates.CONAndEncryptionSettings.ID.ToString()));
                string userid = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserID].Value; //"Tmservice";
                string password = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.ServiceCallUserPassword].Value; // "pass@123";
                string EncryptionKey = itemInfo.Fields[Templates.CONAndEncryptionSettings.CONSettings.EncryptionKey].Value; // "@Aeml#2020";
                clsEncryptAES objcrypt = new clsEncryptAES(EncryptionKey);

                SapPiService.FeedbackPostService.feedbackUpdateSoapClient objPost_FeedbackRequest = new SapPiService.FeedbackPostService.feedbackUpdateSoapClient();

                SapPiService.FeedbackPostService.AuthHeader header = new SapPiService.FeedbackPostService.AuthHeader();
                header.Username = objcrypt.EncryptText(userid);
                header.Password = objcrypt.EncryptText(password);

                string Feedback_sr_no, Feedback_type, OverallExperience, Attitude_Empathy, Quality, Process, Attitude_Disatifaction_Reason, Quality_Disatifaction_CC = "", CCC_intraction_with = "", Quality_Disatifaction_CCC = "", Quality_Disatifaction_EMAIL = "", Quality_Disatifaction_Digital = "", Quality_Disatisfaction_Backend = "", Number_Contacted, Concerns_addressed, Ease_of_register_concerns, Informed_alternate_digital_channels, Recommandation_scale_Adani_Electricity, Suggestion_improve_value, valuable_inputs, Rate_Adani_Brand, Timelines_of_our_services, Safety_practices, Rate_Technology_Equipment, Rate_Quality_Cable_laying, Rate_responsivenes_Adani_Pont_of_contact, Process_Disatifaction_reason, remark = "";

                Feedback_sr_no = objcrypt.EncryptText(model.SR_Number); ;
                Feedback_type = objcrypt.EncryptText(model.FeedbackType);
                OverallExperience = objcrypt.EncryptText(model.OverallExperience);
                Attitude_Empathy = objcrypt.EncryptText(model.Attitude_Empathy);
                Quality = objcrypt.EncryptText(model.Quality);
                Process = objcrypt.EncryptText(model.Process);

                if (model.Attitude_Empathy_Unsatisfied_Ids != null && model.Attitude_Empathy_Unsatisfied_Ids.Count() > 0)
                    Attitude_Disatifaction_Reason = objcrypt.EncryptText(string.Join(",", model.Attitude_Empathy_Unsatisfied_Ids));
                else
                    Attitude_Disatifaction_Reason = "";

                if (model.Unpleasent_Intraction_with_Ids != null && model.Unpleasent_Intraction_with_Ids.Count() > 0)
                    CCC_intraction_with = objcrypt.EncryptText(string.Join(",", model.Unpleasent_Intraction_with_Ids));
                else
                    CCC_intraction_with = "";

                if (model.FeedbackType == "1")
                {
                    if (model.Quality_Unsatisfied_Ids != null && model.Quality_Unsatisfied_Ids.Count() > 0)
                        Quality_Disatifaction_CC = objcrypt.EncryptText(string.Join(",", model.Quality_Unsatisfied_Ids));
                    else
                        Quality_Disatifaction_CC = "";
                }
                else if (model.FeedbackType == "2")
                {
                    if (model.Quality_Unsatisfied_Ids != null && model.Quality_Unsatisfied_Ids.Count() > 0)
                        Quality_Disatifaction_CCC = objcrypt.EncryptText(string.Join(",", model.Quality_Unsatisfied_Ids));
                    else
                        Quality_Disatifaction_CCC = "";
                }
                else if (model.FeedbackType == "3")
                {
                    if (model.Quality_Unsatisfied_Ids != null && model.Quality_Unsatisfied_Ids.Count() > 0)
                        Quality_Disatifaction_EMAIL = objcrypt.EncryptText(string.Join(",", model.Quality_Unsatisfied_Ids));
                    else
                        Quality_Disatifaction_EMAIL = "";
                }
                else if (model.FeedbackType == "4")
                {
                    if (model.Quality_Unsatisfied_Ids != null && model.Quality_Unsatisfied_Ids.Count() > 0)
                        Quality_Disatifaction_Digital = objcrypt.EncryptText(string.Join(",", model.Quality_Unsatisfied_Ids));
                    else
                        Quality_Disatifaction_Digital = "";
                }
                else if (model.FeedbackType == "5")
                {
                    if (model.Quality_Unsatisfied_Ids != null && model.Quality_Unsatisfied_Ids.Count() > 0)
                        Quality_Disatisfaction_Backend = objcrypt.EncryptText(string.Join(",", model.Quality_Unsatisfied_Ids));
                    else
                        Quality_Disatisfaction_Backend = "";
                }

                if (model.Process_Unsatisfied_Ids != null && model.Process_Unsatisfied_Ids.Count() > 0)
                    Process_Disatifaction_reason = objcrypt.EncryptText(string.Join(",", model.Process_Unsatisfied_Ids));
                else
                    Process_Disatifaction_reason = "";

                Number_Contacted = objcrypt.EncryptText(model.Number_Contacted);
                Concerns_addressed = objcrypt.EncryptText(model.Concerns_addressed);
                Ease_of_register_concerns = objcrypt.EncryptText(model.Ease_of_register_concerns);
                Informed_alternate_digital_channels = objcrypt.EncryptText(model.Informed_alternate_digital_channels);
                Recommandation_scale_Adani_Electricity = objcrypt.EncryptText(model.Recomandation_scale_Adani_Electricity);
                Suggestion_improve_value = objcrypt.EncryptText(model.AEML_could_be_doing_differently); ;
                valuable_inputs = objcrypt.EncryptText(model.AEML_could_be_doing_differently_Other_Text);
                Rate_Adani_Brand = "";
                Timelines_of_our_services = "";
                Safety_practices = "";
                Rate_Technology_Equipment = "";
                Rate_Quality_Cable_laying = "";
                Rate_responsivenes_Adani_Pont_of_contact = "";
                remark = objcrypt.EncryptText(model.AEML_could_be_doing_differently_Other_Text);

                string[] output = objPost_FeedbackRequest.Post_Feedback(header, Feedback_sr_no, Feedback_type, OverallExperience, Attitude_Empathy, Quality, Process, Attitude_Disatifaction_Reason, Quality_Disatifaction_CC, CCC_intraction_with, Quality_Disatifaction_CCC, Quality_Disatifaction_EMAIL, Quality_Disatifaction_Digital, Quality_Disatisfaction_Backend, Number_Contacted, Concerns_addressed, Ease_of_register_concerns, Informed_alternate_digital_channels, Recommandation_scale_Adani_Electricity, Suggestion_improve_value, valuable_inputs, Rate_Adani_Brand, Timelines_of_our_services, Safety_practices, Rate_Technology_Equipment, Rate_Quality_Cable_laying, Rate_responsivenes_Adani_Pont_of_contact, Process_Disatifaction_reason, remark);

                string outputdata = "";
                outputdata = outputdata + "flag : " + objcrypt.DecryptText(output[0].ToString()) + ",  ";
                outputdata = outputdata + "message : " + objcrypt.DecryptText(output[1].ToString()) + " , ";
                if (output.Length > 2)
                    outputdata = outputdata + "exception :" + objcrypt.DecryptText(output[2].ToString()) + ",  ";

                string flag = objcrypt.DecryptText(output[0].ToString());
                string message = objcrypt.DecryptText(output[1].ToString());
                string exception = "";
                if (output.Length > 2)
                    exception = objcrypt.DecryptText(output[2].ToString());

                if (flag == "1")
                {
                    result.IsSuccess = true;
                }
                else
                {
                    result.IsSuccess = false;
                }
                result.Message = message;
                result.Flag = flag;
                result.Exception = exception;
                result.OutputMessage = outputdata;
            }
            catch (Exception ex)
            {
                Log.Error("Error: Feedback post for " + model.SR_Number + " Error:" + ex.Message, ex, this);
                result.IsSuccess = false;
                result.Message = "An exception occured in posting data, please try again!";
                result.Flag = "2";
                result.Exception = ex.Message;
                result.OutputMessage = "An exception occured in posting data, please try again!";
            }
            return result;
        }

        [ChildActionOnly]
        public static ReCaptchaResponse VerifyCaptcha(string secret, string request)
        {
            if (request != null)
            {
                using (System.Net.Http.HttpClient hc = new System.Net.Http.HttpClient())
                {
                    var values = new Dictionary<string, string> {
                        {
                            "secret",
                            secret
                        },
                        {
                            "response",
                            request
                        }
                    };
                    var content = new System.Net.Http.FormUrlEncodedContent(values);
                    var Response = hc.PostAsync("https://www.google.com/recaptcha/api/siteverify", content).Result;
                    var responseString = Response.Content.ReadAsStringAsync().Result;
                    if (!string.IsNullOrWhiteSpace(responseString))
                    {
                        ReCaptchaResponse response = JsonConvert.DeserializeObject<ReCaptchaResponse>(responseString);
                        return response;
                    }
                    else
                    {
                        throw new Exception();
                        //Throw error as required  
                    }
                }
            }
            else
            {
                throw new Exception();
                //Throw error as required  
            }
        }
        public class ReCaptchaResponse
        {
            public bool success
            {
                get;
                set;
            }
            public string challenge_ts
            {
                get;
                set;
            }
            public string hostname
            {
                get;
                set;
            }
            [JsonProperty(PropertyName = "error-codes")]
            public List<string> error_codes
            {
                get;
                set;
            }
        }
    }


}

