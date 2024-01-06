using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Marathon.Website.sitecore.admin.Marathon
{
    public partial class DeleteUser : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            string Id = Request.QueryString["id"].ToString();
            AhmedabadMarathonRegistrationDataContext dataContext = new AhmedabadMarathonRegistrationDataContext();
            var userData = dataContext.AhmedabadMarathonRegistrations.Where(x => x.Id == long.Parse(Id)).FirstOrDefault();
            LiteralControl griddata = new LiteralControl();
            griddata.Text = "<table id='example' class='table table-striped table-bordered' style='width:100%'><thead><tr><th>S. No</th><th>Full Name </th><th>Contact Number</th><th> Email</th><th>Race Category</th><th>Registration Staus</th><th>Payment Status</th><th>Form Submitted On</th></tr></thead><tbody>";
            griddata.Text = griddata.Text + "<tr><td>" + userData.Id + "</td><td>" + userData.FirstName + " " + userData.LastName + "</td><td>" + userData.ContactNumber + "</td><td>" + userData.Email + "</td><td>" + userData.RaceDistance + "</td><td>"+ userData.RegistrationStatus + "</td><td>" + userData.PaymentStatus + "</td><td>"+ userData.FormSubmitOn + "</td></tr>";
            griddata.Text = griddata.Text + "</tbody></table>";
            UserDataGrid.Controls.Add(griddata);
        }
        protected void DeleteUserConfirmation(object sender, EventArgs e)
        {
           string Id = Request.QueryString["id"].ToString();
            AhmedabadMarathonRegistrationDataContext dataContext = new AhmedabadMarathonRegistrationDataContext();
            try
            {
                if (!String.IsNullOrEmpty(Id))
                {
                    Log.Info("User delete confirmation Start", "");
                    var userData = dataContext.AhmedabadMarathonRegistrations.Where(x => x.Id == long.Parse(Id)).FirstOrDefault();
                    dataContext.AhmedabadMarathonRegistrations.DeleteOnSubmit(userData);
                    dataContext.SubmitChanges();
                    Response.Redirect("/sitecore/admin/Marathon/UpdateParticipate.aspx");
                }
                else
                {
                    Log.Info("User delete confirmation Id is null", "");
                }
            }
            catch (Exception ex)
            {
                Log.Error("User delete confirmation excption" + ex, "");
            }
        }
        protected void returnToUserUpdatePage(object sender, EventArgs e)
        {
            Response.Redirect("/sitecore/admin/Marathon/UpdateParticipate.aspx");
        }
    }
}