using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Transmission.Website.sitecore.admin.Transmission
{
    public partial class CarbonCalculatorAuditTrailHistory : System.Web.UI.Page
    {
        TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            string ID = Request.QueryString["id"].ToString();
            string LoginID = Request.QueryString["LoginID"].ToString();

            var TotalRecord = from tcc in rdb.TransmissionInsertCostCalculatorHistories
                                  //orderby tcc.FormSubmitOn descending
                              where (tcc.RegistartionNumber != null && tcc.CostCalculatorID == ID && tcc.Login == LoginID)
                              select tcc;
            TotalRecord = TotalRecord.OrderByDescending(x => x.FormSubmitOn);
            if (TotalRecord.Count() > 0)
            {
                LiteralControl griddata = new LiteralControl();
                griddata.Text = "<table id='example' class='table table-striped table-bordered' style='width:100%'><thead><tr><th style='width:13%'> Submitted On </th><th> Registration No </th><th> Name </th><th> Company Name </th><th> Contact No </th><th> E-mail ID </th><th>Annual Carbon Footprint (tonnes)</th></tr></thead><tbody>";
                foreach (var item in TotalRecord)
                {
                    var userDetail = rdb.Transmission_CarbonCalculator_RegistrationForms.Where(x => x.EmailId == item.Login || x.MobileNumber == item.Login).FirstOrDefault();
                    var submitdate = "";
                    if (item.FormSubmitOn.GetValueOrDefault() != null)
                    {
                        submitdate = item.FormSubmitOn.Value.ToString("dd MMM yyyy");
                    }
                    griddata.Text = griddata.Text + "<tr><td>" + submitdate + "</td><td><a href=\"/sitecore/admin/transmission/CarbonCalculatorAuditTrailUserDetails.aspx?id=" + item.RegistartionNumber + "\"  name=\"RegNobtn\" id=\"RegNobtn\" class=\"btn initiatives-btn2\">" + item.RegistartionNumber + "</a></td><td>" + userDetail.FullName + "</td><td>" + userDetail.Company + "</td><td>" + userDetail.MobileNumber + "</td><td>" + userDetail.EmailId + "</td><td>" + item.AnnualCarbonFootprint + "<td></tr>";
                }
                griddata.Text = griddata.Text + "</tbody></table>";
                gridrecord1.Controls.Add(griddata);
            }
        }
    }
}