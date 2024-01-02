using System;
using System.Linq;
using System.Xml.Linq;
using Sitecore.Diagnostics;
using Sitecore.Events;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using Sitecore.ExperienceForms.Mvc.Models.SubmitActions;
using Sitecore.ExperienceForms.Processing;
using Sitecore.Foundation.Email.Model;
using Sitecore.Foundation.Email.Utils;
using Sitecore.Tasks;

namespace Project.AdaniInternationalSchool.Website.FormSubmission
{
    public class SendCustomEmail : Sitecore.ExperienceForms.Mvc.Processing.SubmitActions.SendEmail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SendCustomEmail"/> class.
        /// </summary>
        /// <param name="submitActionData">submitActionData.</param>
        public SendCustomEmail(ISubmitActionData submitActionData)
            : base(submitActionData)
        {
        }

        /// <summary>
        /// Executes the action with the specified <paramref name="data" />.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="formSubmitContext">The form submit context.</param>
        /// <returns><c>true</c> if the action is executed correctly; otherwise <c>false</c>.</returns>
        protected override bool Execute(SendEmailData data, FormSubmitContext formSubmitContext)
        {
            try
            {
                Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));

                if (!formSubmitContext.HasErrors)
                {

                    /*
                     var Name = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Name"));

                    if (Name != null)
                    {
                        var property = Name.GetType().GetProperty("Value");
                        string strName = Convert.ToString(property.GetValue(Name));
                        data.Subject = data.Subject.Replace("[Full Name]", strName);
                        data.Body = data.Body.Replace("[Full Name]", strName);
                    }


                    var EventName = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Event Name"));
                    if (EventName != null)
                    {
                        var property = EventName.GetType().GetProperty("Value");
                        string strEventName = Convert.ToString(property.GetValue(EventName));
                        data.Subject = data.Subject.Replace("[Event Name]", strEventName);
                        data.Body = data.Body.Replace("[Event Name]", strEventName);
                    }
                    */


                    var Email = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Email"));
                    if (Email != null)
                    {
                        var property = Email.GetType().GetProperty("Value");
                        string strEmail = Convert.ToString(property.GetValue(Email));
                        data.To = data.To + ";" + strEmail;
                    }

                    /*
                    var Message = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Message"));
                    if (Message != null)
                    {
                        var property = FullName.GetType().GetProperty("Value");
                        string strMessage = Convert.ToString(property.GetValue(Message));
                        data.Subject = data.Subject.Replace("[Message]", strMessage);
                        data.Body = data.Body.Replace("[Message]", strMessage);
                   
                    }

                    var EnquiryType = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Enquiry Type"));

                    if (EnquiryType is DropDownListViewModel)
                    {
                        DropDownListViewModel dropdownField = EnquiryType as DropDownListViewModel;
                        string strEnquiryType = dropdownField.Value.FirstOrDefault();
                        data.Subject = data.Subject.Replace("[Enquiry Type]", strEnquiryType);
                        data.Body = data.Body.Replace("[Enquiry Type]", strEnquiryType);
                    }
                    */
                    ResponseModel responseModel = new ResponseModel();
                    responseModel.To = data.To;
                    responseModel.From = data.From;
                    responseModel.Cc = data.Cc;
                    responseModel.Bcc = data.Bcc;
                    responseModel.Subject = "AIS Career Form";
                    responseModel.Body = "Thankyou! Form Submitted Successfully.";


                    var response = WebApiHelper.SentEmail(responseModel);


                    if (response != null && response.isSucess)
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                }

                return false;
            }
            catch (Exception ex)
            {

                Sitecore.Diagnostics.Log.Error("Sent Email Error:" + ex.Message, ex, this);
                return false;
            }

        }

    }
}