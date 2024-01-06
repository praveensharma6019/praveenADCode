using Sitecore.Diagnostics;
using System;
using System.Data;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Marathon.Website.sitecore.admin.Marathon
{
    public partial class UpdateParticipate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session["PageLoad"] = null;
        }
        protected void GetData(object sender, EventArgs e)
        {
            AhmedabadMarathonRegistrationDataContext dataContext = new AhmedabadMarathonRegistrationDataContext();
            try
            {
                int serialNo = 1;
                Log.Info("Marathon GetData Start","");
                var contactNumber = ContactNumber.Text;
                var searchResult = dataContext.AhmedabadMarathonRegistrations.Where(x => x.ContactNumber == contactNumber).OrderByDescending(x => x.FormSubmitOn).ToList();
                LiteralControl griddata = new LiteralControl();
                griddata.Text = "<table id='example' class='table table-striped table-bordered' style='width:100%'><thead><tr><th>S. No</th><th>Full Name</th><th>Contact Number</th><th> Email</th><th>Race Category</th><th>Payment Status</th><th>Form Submitted On</th><th>Update<th>Delete</th></tr></thead><tbody>";
                foreach (var item in searchResult)
                {
                    griddata.Text = griddata.Text + "<tr><td>" + serialNo + "</td><td>" + item.FirstName+" "+item.LastName + "</td><td>" + item.ContactNumber + "</td><td>" + item.Email + "</td><td>" + item.RaceDistance + "</td><td>" + item.PaymentStatus + "</td><td>"+item.FormSubmitOn+ "</td><td><a href='/sitecore/admin/Marathon/UpdateUserInfo.aspx?id=" + item.Id + "'>Update</a></td><td><a href='/sitecore/admin/Marathon/DeleteUser.aspx?id=" + item.Id+"'>Delete</a></td></tr>";
                    serialNo++;
                }
                griddata.Text = griddata.Text + "</tbody></table>";
                UserDataGrid.Controls.Add(griddata);
            }
            catch(Exception ex)
            {
                Log.Error("Marathon GetData Exception"+ex, "");
            }
        }
    }
}