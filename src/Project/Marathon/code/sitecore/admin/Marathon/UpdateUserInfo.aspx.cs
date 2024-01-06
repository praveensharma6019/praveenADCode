using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Marathon.Website.Controllers;
using Sitecore.Marathon.Website.Models;
using Sitecore.Marathon.Website.Services;
using Sitecore.Marathon.Website.Validation;
using Sitecore.Project.Marathon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Marathon.Website.sitecore.admin.Marathon
{
    public partial class UpdateUserInfo : System.Web.UI.Page
    {
        Sitecore.Data.Database web = Sitecore.Configuration.Factory.GetDatabase("web");
        protected void Page_Load(object sender, EventArgs e)
        {
            var i = 0;
            string Id = Request.QueryString["id"].ToString();
            AhmedabadMarathonRegistrationDataContext dataContext = new AhmedabadMarathonRegistrationDataContext();
            var userData = dataContext.AhmedabadMarathonRegistrations.Where(x => x.Id == long.Parse(Id)).FirstOrDefault();
            FirstName.Text = userData.FirstName;
            LastName.Text = userData.LastName;
            Email.Text = userData.Email;
            contactNumberId.Text = userData.ContactNumber;
            if (Session["PageLoad"]==null)
            { 
                foreach (ListItem gender in Gender.Items)
                {
                    if(gender.Value.ToLower().Equals(userData.Gender))
                    {
                        Gender.ClearSelection();
                        Gender.Items[i].Selected = true;
                        break;
                    }
                    i++;
                }
                i = 0;
                foreach (ListItem raceCate in RaceCategory.Items)
                {
                    if (raceCate.Value.ToLower().Equals(userData.RaceDistance.ToLower()))
                    {
                        RaceCategory.ClearSelection();
                        RaceCategory.Items[i].Selected = true;
                        break;
                    }
                    i++;
                }
                i = 0;
                foreach (ListItem Tshirt in TShirtSize.Items)
                {
                    if (Tshirt.Value.ToLower().Equals(userData.TShirtSize))
                    {
                        TShirtSize.ClearSelection();
                        TShirtSize.Items[i].Selected = true;
                        break;
                    }
                    i++;
                }
                i = 0;
                foreach (ListItem payment in PaymentStatus.Items)
                {
                    if (payment.Value.ToLower().Equals(userData.PaymentStatus))
                    {
                        PaymentStatus.ClearSelection();
                        PaymentStatus.Items[i].Selected = true;
                        break;
                    }
                    i++;
                }
                Session["PageLoad"] = "Loaded";
            }
        }
        protected void UpdateUserInformation(object sender, EventArgs e)
        {
            Session["PageLoad"] = null;

            string Id = Request.QueryString["id"].ToString();
            try
            {
                Log.Info(" UpdateUserInformation confirmation Start", "");
                AhmedabadMarathonRegistrationDataContext dataContext = new AhmedabadMarathonRegistrationDataContext();
                AhmedabadMarathonRegistration userData = dataContext.AhmedabadMarathonRegistrations.Where(x => x.Id == long.Parse(Id)).FirstOrDefault();
                userData.RaceDistance = RaceCategory.SelectedValue;
                userData.ContactNumber = contactNumberId.Text;
                userData.TShirtSize = TShirtSize.SelectedValue;
                userData.PaymentStatus = PaymentStatus.SelectedValue;
                dataContext.SubmitChanges();
                SendNotification(userData.UserId);
                Response.Redirect("/sitecore/admin/Marathon/UpdateParticipate.aspx?id="+ Id);
            }
            catch (Exception ex)
            {
                Log.Error(" UpdateUserInformation confirmation exception" + ex, "");
            }
        }
        protected void returnToUserUpdatePage(object sender, EventArgs e)
        {
            Session["PageLoad"] = null;
            Response.Redirect("/sitecore/admin/Marathon/UpdateParticipate.aspx");
        }

        public void SendNotification(string UserID)
        {
            try
            {
                Log.Info("Marathon Dashboard Update info Send notification start","");
                AhmedabadMarathonRegistrationDataContext dataContext = new AhmedabadMarathonRegistrationDataContext();
                RegistrationModel emp = new RegistrationModel();   
                MarathonController marathonController = new MarathonController();
                SendSMSNotification(UserID);
                SendMailNotification(UserID);
            }
            catch (Exception ex)
            {
                Log.Error("Marathon Dashboard Send Notification Exception"+ex, "");
            }
        }
        public void SendMailNotification(string UserID)
        {
            try
            {
                Log.Info("Marathon Dashboard Send Mail Notification start", "");
                AhmedabadMarathonRegistrationDataContext objambd = new AhmedabadMarathonRegistrationDataContext();

                var UserDetails = objambd.AhmedabadMarathonRegistrations.Where(val => val.UserId == UserID).FirstOrDefault();

                var BibExpoItem = web.GetItem("/sitecore/content/Marathon/Global/Dictionary/Registration/bibExpo");
                string BibExpo;
                if (BibExpoItem==null)
                {
                     BibExpo = "Dates to be announced shortly";
                }
                else
                {
                     BibExpo = web.GetItem("/sitecore/content/Marathon/Global/Dictionary/Registration/bibExpo").Fields["Phrase"].Value;
                }
                var TimeSlot = FlagOffTimeTime(UserDetails.RaceDistance);
                string reportingTime = ReportingTime(UserDetails.RaceDistance);


                var AcknowledgeMail = web.GetItem(EMailTemplate.AcknowledgementMail);               

                string url = "https://" + Request.Url.Host;
                string message = "";
                message = AcknowledgeMail.Fields[EMailTemplate.Fields.Body].Value; ;
                message = message.Replace("[FirstName]", UserDetails.FirstName);
                message = message.Replace("[Gender]", UserDetails.Gender);
                message = message.Replace("[Age]", UserDetails.Age);
                message = message.Replace("[Id]", UserDetails.Id.ToString());
                message = message.Replace("[TShirtSize]", UserDetails.TShirtSize);
                message = message.Replace("[ContactNumber]", UserDetails.ContactNumber);
                message = message.Replace("[RaceDistance]", UserDetails.RaceDistance);
                message = message.Replace("[TimeSlot]", TimeSlot);
                message = message.Replace("[reportingTime]", reportingTime);
                message = message.Replace("[BibExpo]", BibExpo);

                string from = web.GetItem("/sitecore/content/Marathon/Global/Dictionary/Registration/EmailFrom").Fields["Phrase"].Value; 
                string emailSubject = AcknowledgeMail.Fields[EMailTemplate.Fields.Subject].Value;
                var mail = new MailMessage();
                mail.From = new MailAddress(from);
                mail.To.Add(UserDetails.Email);
                mail.Subject = emailSubject;
                mail.Body = message;
                mail.IsBodyHtml = true;
                var ct = new System.Net.Mime.ContentType(System.Net.Mime.MediaTypeNames.Application.Pdf);
                mail.From = new MailAddress(from);
                MainUtil.SendMail(mail);
            }
            catch(Exception ex)
            {
                Log.Error("Marathon Dashboard Send Mail Notification Exception"+ex, "");
            }
        }

        public void SendSMSNotification(string UserID)
        {
            try
            {
                AhmedabadMarathonRegistrationDataContext dataContext = new AhmedabadMarathonRegistrationDataContext();
                Log.Info("Marathon Dashboard SMS Mail Notification start", "");
                var API =  web.GetItem("/sitecore/content/Marathon/Global/Dictionary/SMS API and Message/Registration Confirmation SMS").Fields["Phrase"].Value;
                var EventName = web.GetItem("/sitecore/content/Marathon/Global/Dictionary/Event/EventName").Fields["Phrase"].Value;
                var SMSOTPItem = web.GetItem(SMSTemplate.RegistrationConfirmationSMS);
                string body = SMSOTPItem.Fields[SMSTemplate.Fields.Body].Value;
                RegistrationModel details = new RegistrationModel();
                var UserDetail = dataContext.AhmedabadMarathonRegistrations.Where(val => val.UserId == UserID).OrderByDescending(y => y.FormSubmitOn).FirstOrDefault();
                if (UserDetail != null)
                {
                    details.RaceDistance = UserDetail.RaceDistance;
                    details.ReferenceCode = UserDetail.ReferenceCode;
                    details.FirstName = UserDetail.FirstName;
                    details.LastName = UserDetail.LastName;
                    details.Email = UserDetail.Email;
                    decimal? d = UserDetail.FinalAmount;
                    details.FinalAmount = d ?? 0;
                    details.PaymentStatus = UserDetail.PaymentStatus;
                    details.ContactNumber = UserDetail.ContactNumber;
                    details.Age = UserDetail.Age;
                    details.Gender = UserDetail.Gender;
                    details.TShirtSize = UserDetail.TShirtSize;
                    details.Useridstring = UserDetail.UserId;
                }

                var apiurl = string.Format(API, details.ContactNumber, details.FirstName + " " + details.LastName, EventName, details.RaceDistance, details.Gender, details.TShirtSize, details.Email, details.ContactNumber);
                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(apiurl);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                Task<HttpResponseMessage> x = client.GetAsync(apiurl);
                HttpResponseMessage response = x.Result;
                if (response.IsSuccessStatusCode)
                {
                    Log.Info("Marathon Dashboard SMS Mail Notification Api call success", details.ContactNumber);
                }
                else
                {
                    Log.Error("Marathon Dashboard SMS Mail Notification OTP Api call fail", details.ContactNumber);
                }
            }
            catch (Exception ex)
            {
                Log.Error("Marathon Dashboard SMS Mail Notification Exception" + ex, "");
            }
        }

        public string ReportingTime(string RaceDistance)
        {
            var RaceTimeFolder = web.GetItem(RaceTime.RaceTimeFolder);
            foreach (var value in RaceTimeFolder.GetChildren().ToList())
            {
                if (value.Fields[RaceTime.Fields.RaceCategory].Value.ToString().Equals(RaceDistance))
                {
                    return value.Fields[RaceTime.Fields.ReportingTime].Value.ToString();
                }
            }
            return "";
        }
        public string FlagOffTimeTime(string RaceDistance)
        {
            var RaceTimeFolder = web.GetItem(RaceTime.RaceTimeFolder);
            foreach (var value in RaceTimeFolder.GetChildren().ToList())
            {
                if (value.Fields[RaceTime.Fields.RaceCategory].Value.ToString().Equals(RaceDistance))
                {
                    return value.Fields[RaceTime.Fields.FlagOffTime].Value.ToString();
                }
            }
            return "";
        }
    }
}