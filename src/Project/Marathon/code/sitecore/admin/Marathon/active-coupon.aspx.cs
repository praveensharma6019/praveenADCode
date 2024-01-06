using Sitecore.Data.Fields;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Marathon.Website.sitecore.admin.Marathon
{
    public partial class active_coupon : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            
            AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
            Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
            ListItem objselect = new ListItem() { Text = "Please Select", Value = "0" };

            var itemInfo = db.GetItem("{7B32142B-4907-4F15-B09C-43462B8A6C55}");
            LiteralControl l = new LiteralControl();
            l.Text = "<table border='0' cellspacing='0' style='width:100%'><tbody><tr><th> Active Coupon </th><th> Count </th><th>Coupons Used </th><th> Coupon Validity </th><th> Discount Rate </th></tr>";
            int Currentyear = int.Parse(getyear.Value);
            var TotalregistrationRecord = from rc in rdb.AhmedabadMarathonRegistrations
                                          where ((rc.FormSubmitOn.Value.Year == Currentyear) && (rc.RegistrationStatus == "successful"))
                                          select rc;

            foreach (var item in itemInfo.Children.ToList())
            {

                var CouponTitle = item.Fields["Coupon Title"].Value;
                var MaximumUsage = item.Fields["Maximum Usage"].Value;
                var EndDate = (DateField)item.Fields["End Date"];
              
                var Discount = item.Fields["Enter Discount Rate in Percentage"].Value;

                var CouponUsageCount = TotalregistrationRecord.Where(x => x.ReferenceCode.ToLower() == CouponTitle.ToLower()).Count();
                l.Text = l.Text + "<tr><td>" + CouponTitle + "</td><td>" + MaximumUsage + "</td><td>" + CouponUsageCount + "</td><td> To " + EndDate.DateTime.AddHours(5.5).ToString("dd-MM-yyyy") + " </td><td>" + Discount + "% </td></tr>";
            }
            l.Text = l.Text + "</tbody></table>";
            //new { format = "MMM dd, yyyy" }
            PlaceHolder1.Controls.Add(l);

        }
    }
}