using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Adani.BAU.Adv.Project.Controllers
{
    public class SocialSubscribeController : Controller
    {

        string connectionstring = Convert.ToString(System.Configuration.ConfigurationManager.AppSettings["AdaniVenturesSqlConnectionString"]);
        //string connectionstring = @"uid=sa;pwd=Admin@123456;database=AdaniVentures;server=LT-0105-GGR6JL3";
        // GET: Email
        [HttpPost]
        public JsonResult JoinSocialSubscribe(JsonResultModel jsonResultModel)
        {

            try
            {
                jsonResultModel = IsCheckValid(jsonResultModel);

                if (jsonResultModel.IsValid)
                {
                    var contact = jsonResultModel.objContactUs;
                    using (SqlConnection cn = new SqlConnection(connectionstring))
                    {
                        string strQuery = "INSERT INTO AdaniVenturesJoinSocialSubscribe (EmailID) VALUES  (N'" + contact.email + "')";
                        SqlCommand cmd = new SqlCommand(strQuery, cn);
                        cn.Open();
                        int rows = cmd.ExecuteNonQuery();
                        cn.Close();
                    }
                    jsonResultModel.IsSuccess = true;
                    jsonResultModel.IsError = false;
                    jsonResultModel.IsValid = true;
                    return Json(jsonResultModel);

                }
                else
                {
                    jsonResultModel.IsSuccess = true;
                    jsonResultModel.IsError = false;
                    jsonResultModel.IsValid = false;
                    return Json(jsonResultModel);

                }

            }
            catch (Exception ex)
            {
                jsonResultModel.IsSuccess = false;
                jsonResultModel.IsError = true;
                jsonResultModel.IsValid = false;
                jsonResultModel.ErrorSource = ex.StackTrace;
                jsonResultModel.ErrorMessage = ex.Message;
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
                    try
                    {
                        switch (contactValidation.txtFieldName)
                        {
                            case "txtemail":
                                if (string.IsNullOrEmpty(contactValidation.txtFieldValue))
                                {
                                    contactValidation.lblErrorField = "lblemail";
                                    contactValidation.lblErrorFieldMessage = "* Please enter your email";
                                    jsonResultModel.IsValid = false;
                                }
                                else if (contactValidation.txtFieldValue.Length > 50)
                                {
                                    contactValidation.lblErrorField = "lblemail";
                                    contactValidation.lblErrorFieldMessage = "* please enter email valid";
                                    jsonResultModel.IsValid = false;
                                }
                                else if (!ValidateEmail.IsValidEmail(contactValidation.txtFieldValue))
                                {
                                    contactValidation.lblErrorField = "lblemail";
                                    contactValidation.lblErrorFieldMessage = "* Please enter a valid email address";
                                    jsonResultModel.IsValid = false;
                                }
                                else
                                {
                                    contactValidation.lblErrorField = "";
                                    contactValidation.lblErrorFieldMessage = "";
                                    jsonResultModel.objContactUs.email = contactValidation.txtFieldValue;
                                }
                                break;
                            default:
                                break;
                        }
                    }
                    catch (Exception)
                    {
                        return jsonResultModel;

                    }
                   
                }
            }

            return jsonResultModel;
        }
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

        return !string.IsNullOrEmpty(email) && r.IsMatch(email);
    }
}

public class ContactUs
{
    //public string name { get; set; }
    public string email { get; set; }
    //public string enquiry { get; set; }
    //public string message { get; set; }
}


