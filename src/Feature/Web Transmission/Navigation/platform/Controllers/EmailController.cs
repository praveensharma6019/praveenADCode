using Adani.BAU.Transmission.Foundation.SitecoreHelper.Platform.Attribute;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Convert = System.Convert;

namespace Adani.BAU.Transmission.Project.Platform.Controllers
{
    public class EmailController : Controller
    {
        string connectionstring = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["WTSqlConnectionString"]);

        public bool IsReCaptchValid(string reResponse)
        {
            bool flag = false;
            var apiUrl=Sitecore.Globalization.Translate.Text("APIUrl");
            string secretKey = Sitecore.Globalization.Translate.Text("SecretKey");
            var requestUri = string.Format(apiUrl, secretKey, reResponse);
            using (WebResponse response = ((HttpWebRequest)WebRequest.Create(requestUri)).GetResponse())
            {
                using (StreamReader streamReader = new StreamReader(response.GetResponseStream()))
                {
                    flag = (JObject.Parse(streamReader.ReadToEnd()).Value<bool>("success") ? true : false);
                }
            }
            return flag;
        }

        [HttpPost]
        [ValidateJsonAntiForgeryToken]
        [RateLimit(Seconds = 10)]
        public JsonResult SendEmail(JsonResultModel jsonResultModel)
        {
            bool Validated = false;
            try
            {
                Validated = this.IsReCaptchValid(jsonResultModel.reResponse);
            }
            catch (Exception ex)
            {
                jsonResultModel.IsSuccess = false;
                jsonResultModel.IsError = true;
                jsonResultModel.IsValid = false;
                jsonResultModel.ErrorMessage = "Error in Captcha verification.";
                jsonResultModel.contactvalidationlist = null;
                jsonResultModel.objContactUs = null;
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, typeof(EmailController));
                return Json(jsonResultModel, JsonRequestBehavior.AllowGet);
            }
            try
            {
                if (Validated)
                {
                    jsonResultModel = IsCheckValid(jsonResultModel);

                    if (jsonResultModel.IsValid)
                    {
                        var contact = jsonResultModel.objContactUs;

                        SqlConnectionStringBuilder sb = new SqlConnectionStringBuilder(connectionstring);
                        sb.ColumnEncryptionSetting = SqlConnectionColumnEncryptionSetting.Enabled;
                        connectionstring = sb.ToString();
                        using (SqlConnection cn = new SqlConnection(connectionstring))
                        {
                            //string strQuery = "INSERT INTO WebTransmissionContact ([Name], EmailID, Enquiry, [Message]) VALUES  (N'" + contact.name + "', N'" + contact.email + "', N'" + contact.enquiry + "', N'" + contact.message + "')";
                            string strQuery = "INSERT INTO WebTransmissionContact (Name, EmailID, Enquiry, Message) VALUES  " +
                                "(@Name, @EmailID, @Enquiry, @Message)";
                           
                            SqlCommand cmd = new SqlCommand(strQuery, cn);
                            cmd.Parameters.AddWithValue("@Name", contact.name);
                            cmd.Parameters.AddWithValue("@EmailID", contact.email);
                            cmd.Parameters.AddWithValue("@Enquiry", contact.enquiry);
                            cmd.Parameters.AddWithValue("@Message", contact.message);
                            try
                            {
                                cn.Open();
                                int rows = cmd.ExecuteNonQuery();
                            }
                            catch (SqlException e)
                            {
                                jsonResultModel.IsSuccess = false;
                                jsonResultModel.IsError = true;
                                jsonResultModel.IsValid = false;
                                jsonResultModel.ErrorMessage = "Please try aftersome time, we are facing some issue.";
                                jsonResultModel.contactvalidationlist = null;
                                jsonResultModel.objContactUs = null;
                                Sitecore.Diagnostics.Log.Error(e.Message, e, typeof(EmailController));
                                return Json(jsonResultModel);
                            }
                            finally
                            {
                                if (cn != null) cn.Close();
                            }
                        }
                        jsonResultModel.IsSuccess = true;
                        jsonResultModel.IsError = false;
                        jsonResultModel.IsValid = true;
                        jsonResultModel.objContactUs = null;
                        jsonResultModel.contactvalidationlist = null;



                        return Json(jsonResultModel);

                    }
                    else
                    {
                        jsonResultModel.objContactUs = null;
                        jsonResultModel.contactvalidationlist=jsonResultModel.contactvalidationlist.Where(x => x.lblErrorFieldMessage!="").ToList();
                        jsonResultModel.IsSuccess = false;
                        jsonResultModel.IsError = false;
                        jsonResultModel.IsValid = false;
                        return Json(jsonResultModel);

                    }
                }
                else
                {
                    jsonResultModel.IsSuccess = false;
                    jsonResultModel.IsError = false;
                    jsonResultModel.IsValid = false;
                    jsonResultModel.ErrorMessage = "Captcha Required.";
                    jsonResultModel.contactvalidationlist = null;
                    jsonResultModel.objContactUs = null;
                    return Json(jsonResultModel, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                jsonResultModel.IsSuccess = false;
                jsonResultModel.IsError = true;
                jsonResultModel.IsValid = false;
                jsonResultModel.ErrorMessage = "Please try aftersome time, we are facing some issue.";
                jsonResultModel.contactvalidationlist = null;
                jsonResultModel.objContactUs = null;
                Sitecore.Diagnostics.Log.Error(ex.Message, ex, typeof(EmailController));
                return Json(jsonResultModel);
            }
        }

        private JsonResultModel IsCheckValid(JsonResultModel jsonResultModel)
        {
            jsonResultModel.IsValid = true;
            if (jsonResultModel.contactvalidationlist != null && jsonResultModel.contactvalidationlist.Count > 0)
            {
                jsonResultModel.objContactUs = new ContactUs();
                foreach (var contactValidation in jsonResultModel.contactvalidationlist)
                {
                    switch (contactValidation.txtFieldName)
                    {
                        case "txtname":
                            if (string.IsNullOrEmpty(contactValidation.txtFieldValue))
                            {
                                contactValidation.lblErrorField = "lblrname";
                                contactValidation.lblErrorFieldMessage = "* Please enter your name";
                                jsonResultModel.IsValid = false;
                            }
                            else if (contactValidation.txtFieldValue.Length > Maxlength.name)
                            {
                                jsonResultModel.IsValid = false;
                                contactValidation.lblErrorField = "lblrname";
                                contactValidation.lblErrorFieldMessage = "Value should not be greater than " + Maxlength.name;
                            }
                            else
                            {
                                contactValidation.lblErrorField = "";
                                contactValidation.lblErrorFieldMessage = "";
                                jsonResultModel.objContactUs.name = contactValidation.txtFieldValue;

                            }
                            contactValidation.txtFieldValue = "";
                            jsonResultModel.reResponse="";
                            break;
                        case "txtemail":
                            if (string.IsNullOrEmpty(contactValidation.txtFieldValue))
                            {
                                contactValidation.lblErrorField = "lblemail";
                                contactValidation.lblErrorFieldMessage = "* Please enter your email";
                                jsonResultModel.IsValid = false;
                            }
                            else if (contactValidation.txtFieldValue.Length > Maxlength.email)
                            {
                                jsonResultModel.IsValid = false;
                                contactValidation.lblErrorField = "lblemail";
                                contactValidation.lblErrorFieldMessage = "Value should not be greater than " + Maxlength.email;
                                
                            }
                            else if (!ValidateEmail.IsValidEmail(contactValidation.txtFieldValue))
                            {
                                jsonResultModel.IsValid = false;
                                contactValidation.lblErrorField = "lblemail";
                                contactValidation.lblErrorFieldMessage = "* Please enter a valid email address";
                               
                            }
                            else
                            {
                                contactValidation.lblErrorField = "";
                                contactValidation.lblErrorFieldMessage = "";
                                jsonResultModel.objContactUs.email = contactValidation.txtFieldValue;
                            }
                            contactValidation.txtFieldValue = "";
                            jsonResultModel.reResponse = "";
                            break;
                        case "txtcomment":
                            if (string.IsNullOrEmpty(contactValidation.txtFieldValue))
                            {
                                jsonResultModel.IsValid = false;
                                contactValidation.lblErrorField = "lblcomm";
                                contactValidation.lblErrorFieldMessage = "* Please enter your comment";
                              
                            }
                            else if (contactValidation.txtFieldValue.Length > Maxlength.comment)
                            {
                                jsonResultModel.IsValid = false;
                                contactValidation.lblErrorField = "lblcomm";
                                contactValidation.lblErrorFieldMessage = "Value should not be greater than " + Maxlength.comment;
                              
                            }
                            else
                            {
                                contactValidation.lblErrorField = "";
                                contactValidation.lblErrorFieldMessage = "";
                                jsonResultModel.objContactUs.message = contactValidation.txtFieldValue;
                            }
                            contactValidation.txtFieldValue = "";
                            jsonResultModel.reResponse = "";
                            break;
                        case "enquiry":
                            if (string.IsNullOrEmpty(contactValidation.txtFieldValue))
                            {
                                jsonResultModel.IsValid = false;
                                contactValidation.lblErrorField = "lblequiry";
                                contactValidation.lblErrorFieldMessage = "* Please select your enquiry";
                             
                            }
                            else
                            {
                                contactValidation.lblErrorField = "";
                                contactValidation.lblErrorFieldMessage = "";
                                jsonResultModel.objContactUs.enquiry = contactValidation.txtFieldValue;
                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            return jsonResultModel;
        }


    }

    public class JsonResultModel
    {
        public bool IsSuccess { get; set; }

        public bool IsValid { get; set; }

        public string ErrorMessage { get; set; }
        public string ErrorSource { get; set; }

        public bool IsError { get; set; }

        public List<ContactValidation> contactvalidationlist { get; set; }

        public JsonResultModel()
        {

        }

        public ContactUs objContactUs { get; set; }
        public string reResponse { get; set; }
    }

    public class ContactValidation
    {
        public string txtFieldName { get; set; }
        public string txtFieldValue { get; set; }
        public string lblErrorField { get; set; }
        public string lblErrorFieldMessage { get; set; }
    }
    public static class ValidateEmail
    {
        public static bool IsValidEmail(this string email)
        {
            var r = new System.Text.RegularExpressions.Regex(@"^([a-zA-Z0-9._+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,9})$");
            //[a-zA-Z0-9._+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,4}
            return !string.IsNullOrEmpty(email) && r.IsMatch(email);
        }
    }

    public class ContactUs
    {
        public string name { get; set; }
        public string email { get; set; }
        public string enquiry { get; set; }
        public string message { get; set; }

    }
    public static class Maxlength
    {
        public static int name = 100;
        public static int email = 100;
        public static int comment = 500;
    }
}