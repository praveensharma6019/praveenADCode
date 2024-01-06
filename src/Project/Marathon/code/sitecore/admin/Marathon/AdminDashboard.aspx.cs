using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Sitecore.Marathon.Website.sitecore.admin.Marathon
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
            ListItem objselect = new ListItem() { Text = "Please Select", Value = "0" };

            var itemInfo = db.GetItem("{7B32142B-4907-4F15-B09C-43462B8A6C55}");
            // var item1 = Sitecore.Context.Database.GetItem();

            DateTime todaydate = (DateTime.Now.Date);

            //Get all Today's registrationRecord
            var TodayregistrationRecord = from rc in rdb.AhmedabadMarathonRegistrations
                                          where ((rc.FormSubmitOn.Value.Date == todaydate) && (rc.RegistrationStatus == "successful"))
                                          select rc;
            todaysTotalRegis.Text = TodayregistrationRecord.Count().ToString();

            //Get all Today's PaidRegistrationRecord
            var TodayPaidRegistrationRecord = from rc in TodayregistrationRecord
                                              where (rc.PaymentStatus == "successful")
                                              select rc;
            todaysPaidRegis.Text = TodayPaidRegistrationRecord.Count().ToString();

            //Get all Today's OutstandingRegistrationRecord
            var TodayOutstandingRegistrationRecord = from rc in TodayregistrationRecord
                                                     where (rc.PaymentStatus == "no")
                                                     select rc;
            todaysOutstandingRegis.Text = TodayOutstandingRegistrationRecord.Count().ToString();

            //Get all Today's ComplementaryRegistrationRecord
            var TodayComplementaryRegistrationRecord = from rc in TodayregistrationRecord
                                                       where (rc.PaymentStatus == "complementary")
                                                       select rc;
            todaysComplimentaryRegis.Text = TodayComplementaryRegistrationRecord.Count().ToString();

            //Get all TodaysTotalCollectionRecord//////////////////////////////////////////////
            var Todaytotalcollectionrecord = (from rc in TodayregistrationRecord
                                              select rc.FinalAmount).Sum();
            todaysTotalCollection.Text = Todaytotalcollectionrecord.ToString();
            if (string.IsNullOrEmpty(todaysTotalCollection.Text))
            {
                todaysTotalCollection.Text = "0";
            }

            //Get all Today's Paid collection record
            var Todaytotalpaidcollectionrecord = (from rc in TodayregistrationRecord
                                                  where (rc.PaymentStatus == "successful")
                                                  select rc.FinalAmount).Sum();
            todaysPaidCollection.Text = Todaytotalpaidcollectionrecord.ToString();
            if (string.IsNullOrEmpty(todaysPaidCollection.Text))
            {
                todaysPaidCollection.Text = "0";
            }

            //Get all Today's Outstanding collection record
            var Todayoutstandingcollectionrecord = (from rc in TodayregistrationRecord
                                                    where (rc.PaymentStatus == "no")
                                                    select rc.FinalAmount).Sum();
            todaysOutstandingCollection.Text = Todayoutstandingcollectionrecord.ToString();
            if (string.IsNullOrEmpty(todaysOutstandingCollection.Text))
            {
                todaysOutstandingCollection.Text = "0";
            }

            //Get all Today's complementary collection record
            var Todaycomplementarycollectionrecord = (from rc in TodayregistrationRecord
                                                      where (rc.PaymentStatus == "complementary")
                                                      select rc.FinalAmount).Sum();
            todaysComplimentaryCollection.Text = Todaycomplementarycollectionrecord.ToString();
            if (string.IsNullOrEmpty(todaysComplimentaryCollection.Text))
            {
                todaysComplimentaryCollection.Text = "0";
            }
            //get total registration records//////////////////////////////////
            int Currentyear = 0;
            if (!string.IsNullOrEmpty(getyear.SelectedValue) && int.Parse(getyear.SelectedValue) != 0 && getyear.SelectedValue != "Select Marathon Year")
            {
                Currentyear = int.Parse(getyear.SelectedValue);
            }
            else
            {
                int Currrentdate = DateTime.Now.Year;
                Currentyear = Currrentdate;
            }
            var TotalregistrationRecord = from rc in rdb.AhmedabadMarathonRegistrations
                                          where ((rc.FormSubmitOn.Value.Year == Currentyear) && (rc.RegistrationStatus == "successful"))
                                          select rc;
            totalRegistration.Text = TotalregistrationRecord.Count().ToString();

            //Get Total Paid Registration record
            var TotalPaidregistrationRecord = from rc in TotalregistrationRecord
                                              where (rc.PaymentStatus == "successful")
                                              select rc;
            totalPaidRegistration.Text = TotalPaidregistrationRecord.Count().ToString();

            //Get Total outstanding Registration record
            var TotalOutstandingregistrationRecord = from rc in TotalregistrationRecord
                                                     where (rc.PaymentStatus == "no")
                                                     select rc;
            totalOutstandingRegistration.Text = TotalOutstandingregistrationRecord.Count().ToString();

            //Get Total complementary Registration record
            var TotalComplementaryregistrationRecord = from rc in TotalregistrationRecord
                                                       where (rc.PaymentStatus == "complementary")
                                                       select rc;
            totalComplementaryRegistration.Text = TotalComplementaryregistrationRecord.Count().ToString();


            //Get all TotalCollectionRecord///////////////////////////////////////////////////////
            var totalcollectionrecord = (from rc in TotalregistrationRecord
                                         select rc.FinalAmount).Sum();
            totalCollection.Text = totalcollectionrecord.ToString();

            //Get all Total Paid collection record
            var totalpaidcollectionrecord = (from rc in TotalregistrationRecord
                                             where (rc.PaymentStatus == "successful")
                                             select rc.FinalAmount).Sum();
            totalPaidCollection.Text = totalpaidcollectionrecord.ToString();

            //Get all Today's Outstanding collection record
            var totaloutstandingcollectionrecord = (from rc in TotalregistrationRecord
                                                    where (rc.PaymentStatus == "no")
                                                    select rc.FinalAmount).Sum();
            totalOutstandingCollection.Text = totaloutstandingcollectionrecord.ToString();

            //Get all Total complementary collection record
            var totalcomplementarycollectionrecord = (from rc in TotalregistrationRecord
                                                      where (rc.PaymentStatus == "complementary")
                                                      select rc.FinalAmount).Sum();
            totalComplementaryCollection.Text = totalcomplementarycollectionrecord.ToString();

            ///////////////////////////////////////////////////////////////////////////

            //get total coupon applied

            var totalCouponRecord = (from rc in TotalregistrationRecord
                                     where rc.ReferenceCode != null
                                     orderby rc.ReferenceCode ascending
                                     select rc.ReferenceCode).Distinct();
            TotalCoupon.Text = totalCouponRecord.Count().ToString();



            //////////////////////////////////////////////////////////////////////////////

            //race category table
            var raceCategoryrecord = from rc in TotalregistrationRecord
                                     group rc by rc.RaceDistance into empg
                                     where (empg != null)
                                     select new { CATEGORY = empg.Key, TOTAL = empg.Count() };
            LiteralControl l = new LiteralControl();

            var raceCategoryGendercount = TotalregistrationRecord.Where(x => x.Gender != null).Count();
            l.Text = "<table border='0' cellspacing='0'><tbody><tr><th>Category</th><th>Total</th ></tr>";
            if (raceCategoryrecord.Count() > 0)
            {
                foreach (var item in raceCategoryrecord)
                {
                    l.Text = l.Text + "<tr><td>" + item.CATEGORY + "</td><td>" + item.TOTAL + "</td></tr>";
                }
                l.Text = l.Text + "<tr><td>Total</td><td>" + raceCategoryGendercount + "</td></tr></tbody></table>";
            }
            raceCategory.Controls.Add(l);


            ////////////////////////////////////////////////////////////////////////////////////////////////

            //Gender Wise classification table

            var GenderWiseRecord = from rc in TotalregistrationRecord
                                   where (rc.Gender != null)
                                   group rc by rc.Gender into empg
                                   select new { GENDER = empg.Key, COUNT = empg.Count() };

            if (GenderWiseRecord.Count() > 0)
            {

                LiteralControl l1 = new LiteralControl();
                l1.Text = "<table border='0' cellspacing='0'><tbody><tr><th>Gender</th><th>Total</th></tr>";
                foreach (var item in GenderWiseRecord)
                {
                    l1.Text = l1.Text + " <tr><td> " + item.GENDER.ToUpperInvariant() + " </td><td>" + item.COUNT + "</td></tr>";
                }

                l1.Text = l1.Text + "<tr><td>Total</td><td>" + raceCategoryGendercount + "</td></tr></tbody></table>";
                genderWiseTable1.Controls.Add(l1);
            }

            ////////////////////////////////////////////////////////////////////////////////////////////////

            //Tshirt size 5 Km race

            var tshirtSize5kmRecord = from rc in TotalregistrationRecord
                                      where (rc.TShirtSize != null && rc.RaceDistance.Trim().ToLower() == "5km")
                                      group rc by rc.TShirtSize into empg
                                      select new { SIZE = empg.Key, COUNT = empg.Count() };

            if (tshirtSize5kmRecord.Count() > 0)
            {

                LiteralControl l2 = new LiteralControl();
                l2.Text = "<table border='0' cellspacing='0'><tbody><tr><th>Size</th><th>Count</th></tr>";
                foreach (var item in tshirtSize5kmRecord)
                {
                    l2.Text = l2.Text + " <tr><td> " + item.SIZE + " </td><td>" + item.COUNT + "</td></tr>";
                }
                var totalGender5kmCount = TotalregistrationRecord.Where(x => x.TShirtSize != null && x.RaceDistance.Trim().ToLower() == "5km").Count();

                l2.Text = l2.Text + "<tr><td>Total</td><td>" + totalGender5kmCount + "</td></tr></tbody></table>";
                TshirtSize5Km1.Controls.Add(l2);
            }

            var tshirtSizeOtherRaceRecord = from rc in TotalregistrationRecord
                                            where (rc.TShirtSize != null && rc.RaceDistance.Trim().ToLower() != "5km")
                                            group rc by rc.TShirtSize into empg
                                            select new { SIZE = empg.Key, COUNT = empg.Count() };

            if (tshirtSizeOtherRaceRecord.Count() > 0)
            {

                LiteralControl l3 = new LiteralControl();
                l3.Text = "<table border='0' cellspacing='0'><tbody><tr><th>Size</th><th>Count</th></tr>";
                foreach (var item in tshirtSizeOtherRaceRecord)
                {
                    l3.Text = l3.Text + " <tr><td> " + item.SIZE + " </td><td>" + item.COUNT + "</td></tr>";
                }
                var totalGenderOtherraceCount = TotalregistrationRecord.Where(x => x.TShirtSize != null && x.RaceDistance.Trim().ToLower() != "5km").Count();

                l3.Text = l3.Text + "<tr><td>Total</td><td>" + totalGenderOtherraceCount + "</td></tr></tbody></table>";
                TshirtSizeRemainingRace.Controls.Add(l3);
            }


            //Reference code Dropdown Populate code//

            if (!IsPostBack)
            {

                selectCouponCode.DataSource = totalCouponRecord;
                selectCouponCode.DataBind();


                //bind the data to the grid
                var TotalCurrentYearRecord = from rc in rdb.AhmedabadMarathonRegistrations
                                             where (rc.FormSubmitOn.Value.Year == Currentyear && (rc.DateofBirth.HasValue))
                                             select rc;
                if (TotalCurrentYearRecord.Count() > 0)
                {
                    /*
                   try {

                       LiteralControl griddata = new LiteralControl();
                       griddata.Text = "<table id='example' class='table table-striped table-bordered' style='width:100%'><thead><tr><th>ID </th><th> User ID </th><th> Race Distance </th><th> Race Amount </th><th> Reference Code </th><th> First Name </th><th> Last Name </th><th> DOB </th><th> Email Id </th><th> Contact Number </th><th> Gender </th><th> Tshirt Size </th><th> City </th><th> Amount Received </th><th>Payment Status </th><th>Registration Status</th><th> Submitted Date </th></tr></thead><tbody>";
                       foreach (var item in TotalCurrentYearRecord)
                       {
                           var finaldate = "";
                           if (item.DateofBirth.GetValueOrDefault() != null)
                           {
                               finaldate = item.DateofBirth.Value.ToString("dd-MM-yyyy");
                           }
                           griddata.Text = griddata.Text + "<tr><td>" + item.Id + "</td><td>" + item.UserId + "</td><td>" + item.RaceDistance + "</td><td>" + item.RaceAmount + "</td><td>" + item.ReferenceCode + "</td><td>" + item.FirstName + "</td><td>" + item.LastName + "</td><td>" + finaldate + "</td><td>" + item.Email + "</td><td>" + item.ContactNumber + "</td><td>" + item.Gender.Trim().ToLower() + "</td><td>" + item.TShirtSize + "</td><td>" + item.City + "</td><td>" + item.AmountReceived + "</td><td>" + item.PaymentStatus + "</td><td>" + item.RegistrationStatus + "</td><td>" + item.FormSubmitOn + "</td></tr>";
                       }
                       griddata.Text = griddata.Text + "</tbody></table>";
                       gridrecord1.Controls.Add(griddata);

                   }
                   catch (Exception ex)
                   {

                       lblErroMsg.Text = ex.Message;
                       Sitecore.Diagnostics.Log.Error("Form data -Export Error  " + ex.Message, this);
                   }

                   */

                }

            }

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
        }

        public DataTable ToDataTable<T>(List<T> items)
        {
            DataTable dataTable = new DataTable(typeof(T).Name);
            PropertyInfo[] Props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo prop in Props)
            {
                dataTable.Columns.Add(prop.Name);
            }

            foreach (T item in items)
            {
                var values = new object[Props.Length];

                for (int i = 0; i < Props.Length; i++)
                {
                    values[i] = Props[i].GetValue(item, null);
                }
                dataTable.Rows.Add(values);
            }
            return dataTable;
        }

        public void DatatableToCSV(DataTable dtDataTable, string strFilePath)
        {
            #region Old Method
            //StreamWriter sw = new StreamWriter(strFilePath, false);
            ////headers  
            //for (int i = 0; i < dtDataTable.Columns.Count; i++)
            //{
            //    sw.Write(dtDataTable.Columns[i]);
            //    if (i < dtDataTable.Columns.Count - 1)
            //    {
            //        sw.Write(",");
            //    }
            //}
            //sw.Write(sw.NewLine);
            //foreach (DataRow dr in dtDataTable.Rows)
            //{
            //    for (int i = 0; i < dtDataTable.Columns.Count; i++)
            //    {
            //        if (!System.Convert.IsDBNull(dr[i]))
            //        {
            //            string value = dr[i].ToString();
            //            if (value.Contains(','))
            //            {
            //                value = String.Format("\"{0}\"", value);
            //                sw.Write(value);
            //            }
            //            else
            //            {
            //                sw.Write(dr[i].ToString());
            //            }
            //        }
            //        if (i < dtDataTable.Columns.Count - 1)
            //        {
            //            sw.Write(",");
            //        }
            //    }
            //    sw.Write(sw.NewLine);
            //}
            //sw.Close();
            //DownLoad(strFilePath);
            #endregion

            #region New Method

            Stopwatch stw = new Stopwatch();
            stw.Start();
            StringBuilder sb = new StringBuilder();
            //Column headers  
            for (int i = 0; i < dtDataTable.Columns.Count; i++)
            {
                sb.Append(dtDataTable.Columns[i]);
                if (i < dtDataTable.Columns.Count - 1)
                {
                    sb.Append(",");
                }
            }
            sb.Append(Environment.NewLine);
            //Column Values
            foreach (DataRow dr in dtDataTable.Rows)
            {
                sb.AppendLine(string.Join(",", dr.ItemArray));
            }
            File.WriteAllText(strFilePath, sb.ToString());
            stw.Stop();
            DownLoad(strFilePath);

            #endregion
        }
        public void DownLoad(string FName)
        {
            string path = FName;
            System.IO.FileInfo file = new System.IO.FileInfo(path);
            if (file.Exists)
            {
                Response.Clear();
                Response.AddHeader("Content-Disposition", "attachment; filename=MarathonRegistration.csv");
                Response.AddHeader("Content-Length", file.Length.ToString());
                Response.ContentType = "application/octet-stream";
                Response.WriteFile(file.FullName);
                Response.End();
            }
            else
            {
                Response.Write("This file does not exist.");
            }

        }
        private void ExportGridToExcel()
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ClearContent();
            Response.ClearHeaders();
            Response.Charset = "";
            string FileName = "ExportData-" + DateTime.Now + ".xls";
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
            hiddenGrid1.GridLines = GridLines.Both;
            hiddenGrid1.HeaderStyle.Font.Bold = true;
            hiddenGrid1.RenderControl(htmltextwrtter);
            Response.Write(strwritter.ToString());
            Response.End();

        }
        protected void downloadCSV_Click(object sender, EventArgs e)
        {

            try
            {
                int flag = 0;
                if (!string.IsNullOrEmpty(DdlDateFrom.Text))
                {
                    flag++;
                }
                if (!string.IsNullOrEmpty(DdlDateTo.Text))
                {
                    flag++;
                }

                if (flag == 1)
                {
                    lblErroMsg.Text = "Please select valid Dates";
                    return;

                }
                int Currentyear = 0;
                if (!string.IsNullOrEmpty(getyear.SelectedValue) && int.Parse(getyear.SelectedValue) != 0 && getyear.SelectedValue != "Select Marathon Year")
                {
                    Currentyear = int.Parse(getyear.SelectedValue);
                }
                else
                {
                    int Currrentdate = DateTime.Now.Year;
                    Currentyear = Currrentdate;
                }
                var TotalregistrationRecord = from rc in rdb.AhmedabadMarathonRegistrations
                                              where ((rc.FormSubmitOn.Value.Year == Currentyear) && (rc.DateofBirth.HasValue))
                                              select rc;

                if (selectStatus.SelectedIndex != 0) TotalregistrationRecord = TotalregistrationRecord.Where(s => s.PaymentStatus.ToLower() == selectStatus.SelectedValue.ToLower());
                if (selectCouponCode.SelectedIndex != 0) TotalregistrationRecord = TotalregistrationRecord.Where(s => s.ReferenceCode.ToLower() == selectCouponCode.SelectedValue.ToLower());
                if (flag == 2) TotalregistrationRecord = TotalregistrationRecord.Where(s => s.FormSubmitOn.Value.Date >= DateTime.Parse(DdlDateFrom.Text).Date && s.FormSubmitOn.Value.Date <= DateTime.Parse(DdlDateTo.Text).Date);

                ////bind the filtered data to grid
                //gridrecord1.Controls.Clear();
                //LiteralControl griddata = new LiteralControl();
                //griddata.Text = "<table id='example' class='table table-striped table-bordered' style='width:100%'><thead><tr><th>ID </th><th> User ID </th><th> Race Distance </th><th> Race Amount </th><th> Reference Code </th><th> First Name </th><th> Last Name </th><th> DOB </th><th> Email Id </th><th> Contact Number </th><th> Gender </th><th> Tshirt Size </th><th> City </th><th> Amount Received </th><th> Status </th><th> Submitted Date </th></tr></thead><tbody>";
                //foreach (var item in TotalregistrationRecord)
                //{
                //    griddata.Text = griddata.Text + "<tr><td>" + item.Id + "</td><td>" + item.UserId + "</td><td>" + item.RaceDistance + "</td><td>" + item.RaceAmount + "</td><td>" + item.ReferenceCode + "</td><td>" + item.FirstName + "</td><td>" + item.LastName + "</td><td>" + item.DateofBirth.Value.Date + "</td><td>" + item.Email + "</td><td>" + item.ContactNumber + "</td><td>" + item.Gender + "</td><td>" + item.TShirtSize + "</td><td>" + item.City + "</td><td>" + item.FinalAmount + "</td><td>" + item.PaymentStatus + "</td><td>" + item.FormSubmitOn + "</td></tr>";
                //}
                //griddata.Text = griddata.Text + "</tbody></table>";
                //gridrecord1.Controls.Add(griddata);
                hiddenGrid1.DataSource = TotalregistrationRecord;
                hiddenGrid1.DataBind();
                ExportGridToExcel();

            }
            catch (Exception ex)
            {

                lblErroMsg.Text = "There is some technically problem. Please contact administrator.";
                Sitecore.Diagnostics.Log.Error("Form data -Export Error  " + ex.Message, this);
            }
        }
        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "  
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."  
        }
        protected void searchBtn_Click(object sender, EventArgs e)
        {
            int flag = 0;
            if (!string.IsNullOrEmpty(DdlDateFrom.Text))
            {
                flag++;
            }
            if (!string.IsNullOrEmpty(DdlDateTo.Text))
            {
                flag++;
            }

            if (flag == 1)
            {
                lblErroMsg.Text = "Please select valid Dates";
                return;

            }
            int Currentyear = 0;
            if (!string.IsNullOrEmpty(getyear.SelectedValue) && int.Parse(getyear.SelectedValue) != 0 && getyear.SelectedValue != "Select Marathon Year")
            {
                Currentyear = int.Parse(getyear.SelectedValue);
            }
            else
            {
                int Currrentdate = DateTime.Now.Year;
                Currentyear = Currrentdate;
            }
            var TotalregistrationRecord = from rc in rdb.AhmedabadMarathonRegistrations
                                          where ((rc.FormSubmitOn.Value.Year == Currentyear) && (rc.DateofBirth.HasValue))
                                          select rc;

            if (selectStatus.SelectedIndex != 0) TotalregistrationRecord = TotalregistrationRecord.Where(s => s.PaymentStatus.ToLower() == selectStatus.SelectedValue.ToLower());
            if (selectCouponCode.SelectedIndex != 0) TotalregistrationRecord = TotalregistrationRecord.Where(s => s.ReferenceCode.ToLower() == selectCouponCode.SelectedValue.ToLower());
            if (flag == 2) TotalregistrationRecord = TotalregistrationRecord.Where(s => s.FormSubmitOn.Value.Date >= DateTime.Parse(DdlDateFrom.Text).Date && s.FormSubmitOn.Value.Date <= DateTime.Parse(DdlDateTo.Text).Date);

            //bind the filtered data to grid
            gridrecord1.Controls.Clear();
            LiteralControl griddata = new LiteralControl();
            griddata.Text = "<table id='example' class='table table-striped table-bordered' style='width:100%'><thead><tr><th>ID </th><th> User ID </th><th> Race Distance </th><th> Race Amount </th><th> Reference Code </th><th> First Name </th><th> Last Name </th><th> DOB </th><th> Email Id </th><th> Contact Number </th><th> Gender </th><th> Tshirt Size </th><th> City </th><th> Amount Received </th><th>Payment Status </th><th>Registration Status</th><th> Submitted Date </th></tr></thead><tbody>";
            foreach (var item in TotalregistrationRecord)
            {
                griddata.Text = griddata.Text + "<tr><td>" + item.Id + "</td><td>" + item.UserId + "</td><td>" + item.RaceDistance + "</td><td>" + item.RaceAmount + "</td><td>" + item.ReferenceCode + "</td><td>" + item.FirstName + "</td><td>" + item.LastName + "</td><td>" + item.DateofBirth.Value.ToString("dd-MM-yyyy") + "</td><td>" + item.Email + "</td><td>" + item.ContactNumber + "</td><td>" + item.Gender + "</td><td>" + item.TShirtSize + "</td><td>" + item.City + "</td><td>" + item.AmountReceived + "</td><td>" + item.PaymentStatus + "</td><td>" + item.RegistrationStatus + "</td><td>" + item.FormSubmitOn + "</td></tr>";
            }
            griddata.Text = griddata.Text + "</tbody></table>";
            gridrecord1.Controls.Add(griddata);
            hiddenGrid1.DataSource = TotalregistrationRecord;
            hiddenGrid1.DataBind();
        }
    }
}
