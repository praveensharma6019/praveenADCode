using Sitecore.Diagnostics;
using Sitecore.Marathon.Website.Controllers;
using Sitecore.Marathon.Website.Models;
using Sitecore.Marathon.Website.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Marathon.Website.sitecore.admin.Marathon
{
    public partial class UpdateUserInfo : System.Web.UI.Page
    {
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
                //SendNotification(userData.UserId);
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
            AhmedabadMarathonRegistrationDataContext dataContext = new AhmedabadMarathonRegistrationDataContext();
            RegistrationModel emp = new RegistrationModel();
            var empdata = dataContext.AhmedabadMarathonRegistrations.Where(val => val.UserId == UserID).OrderByDescending(x => x.FormSubmitOn).FirstOrDefault();
            if (empdata != null)
            {
                emp.RaceDistance = empdata.RaceDistance;
                emp.ReferenceCode = empdata.ReferenceCode;
                emp.FirstName = empdata.FirstName;
                emp.LastName = empdata.LastName;
                emp.Email = empdata.Email;
                decimal? d = empdata.FinalAmount;
                emp.FinalAmount = d ?? 0;
                emp.PaymentStatus = empdata.PaymentStatus;
                emp.ContactNumber = empdata.ContactNumber;
                emp.Age = empdata.Age;
                emp.Gender = empdata.Gender;
                emp.TShirtSize = empdata.TShirtSize;
                emp.Useridstring = empdata.UserId;
            }
            MarathonController marathonController = new MarathonController();
            marathonController.sendEmail(emp);
            marathonController.sendSMS(emp, "");
            SMSOTP.registrationconfirmation(emp);
        }

    }
}