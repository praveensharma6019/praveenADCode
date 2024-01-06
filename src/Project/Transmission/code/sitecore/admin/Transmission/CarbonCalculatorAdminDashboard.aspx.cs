using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Transmission.Website.Models;
namespace Sitecore.Transmission.Website.sitecore.admin.Transmission
{
    public partial class TransmissionCarbonCalculator : System.Web.UI.Page
    {
        TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {
            Sitecore.Data.Database db = Sitecore.Configuration.Factory.GetDatabase("web");
            ListItem objselect = new ListItem() { Text = "Please Select", Value = "0" };

            //total carbon footprint calculation done 
            var TotalFootprintCalculation = from rc in rdb.TransmissionInsertCostCalculators
                                            select rc;
            var total_footprint_count = TotalFootprintCalculation.Count();
            FootprintCount.Text = total_footprint_count.ToString();

            //Get total registration
            var TotalRegistration = from rc in rdb.Transmission_CarbonCalculator_RegistrationForms
                                    select rc;
            var total_registration = TotalRegistration.Count();
            totalRegistration.Text = total_registration.ToString();

            //Get Total Carbon Emission of registered persons
            var totalEmission = from rc in rdb.TransmissionInsertCostCalculators
                                select rc;
            var count = 0.0;
            foreach (var item in totalEmission)
            {
                count = count + double.Parse(item.AnnualCarbonFootprint);
            }
            totalRegisteredEmission.Text = count.ToString();

            //Average per person carbon emission of registered persons
            avgRegisteredEmission.Text = (count / total_registration).ToString("##.##");



            //fetch data from InsertCostCalculator table

            if (!IsPostBack)
            {
                //bind the data to the grid/Table
                var TotalRecord = from tcc in rdb.TransmissionInsertCostCalculators
                                      //orderby tcc.FormSubmitOn descending
                                  where (tcc.RegistartionNumber != null)
                                  select tcc;
                TotalRecord = TotalRecord.OrderByDescending(x => x.FormSubmitOn);
                if (TotalRecord.Count() > 0)
                {
                    LiteralControl griddata = new LiteralControl();
                    griddata.Text = "<table id='example' class='table table-striped table-bordered' style='width:100%'><thead><tr><th style='width:13%'> Submitted On </th><th> Registration No </th><th> Name </th><th> Company Name </th><th> Contact No </th><th> E-mail ID </th><th>Annual Carbon Footprint (tonnes)</th><th>Audit Trail History</th></tr></thead><tbody>";
                    foreach (var item in TotalRecord)
                    {
                        var userDetail = rdb.Transmission_CarbonCalculator_RegistrationForms.Where(x => x.EmailId == item.Login || x.MobileNumber == item.Login).FirstOrDefault();
                        var submitdate = "";
                        if (item.FormSubmitOn.GetValueOrDefault() != null)
                        {
                            submitdate = item.FormSubmitOn.Value.ToString("dd MMM yyyy");
                        }
                        griddata.Text = griddata.Text + "<tr><td>" + submitdate + "</td><td><a href=\"/sitecore/admin/transmission/CarbonCalculatorUserDetails.aspx?id=" + item.RegistartionNumber + "\"  name=\"RegNobtn\" id=\"RegNobtn\" class=\"btn initiatives-btn2\">" + item.RegistartionNumber + "</a></td><td>" + userDetail.FullName + "</td><td>" + userDetail.Company + "</td><td>" + userDetail.MobileNumber + "</td><td>" + userDetail.EmailId + "</td><td>" + item.AnnualCarbonFootprint + "</td><td><a href=\"/sitecore/admin/transmission/CarbonCalculatorAuditTrailHistory.aspx?id=" + item.Id + "&LoginID=" + item.Login + "\"  name=\"RegNobtn1\" id=\"RegNobtn1\" class=\"btn initiatives-btn2\">Audit Trail</a></td><td></tr>";
                    }
                    griddata.Text = griddata.Text + "</tbody></table>";
                    gridrecord1.Controls.Add(griddata);
                }

            }

        }



        protected void Button1_Click(object sender, EventArgs e)
        {
            TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
            try
            {
                this.lblErroMsg.Text = "";
                this.lblErroMsg.Visible = false;
                DateTime dateTime = DateTime.Parse(this.TextBoxFrom.Text);
                DateTime dateTime1 = DateTime.Parse(this.TextBoxTo.Text);

                if (System.Convert.ToDateTime(this.TextBoxTo.Text) == System.Convert.ToDateTime(this.TextBoxFrom.Text))
                {
                    IQueryable<TransmissionInsertCostCalculator> newconnection =
                       from rc in rdb.TransmissionInsertCostCalculators
                       where (rc.FormSubmitOn.Value.Date >= dateTime) && (rc.FormSubmitOn.Value.Date <= dateTime1)
                       select rc;
                    if (System.Convert.ToDateTime(this.TextBoxTo.Text) == System.Convert.ToDateTime(this.TextBoxFrom.Text))
                    {
                        newconnection =
                           from rc in rdb.TransmissionInsertCostCalculators
                           where rc.FormSubmitOn.Value.Date == dateTime
                           select rc;

                        if (selectCompany.SelectedIndex != 0 && selectCompany.SelectedValue.ToLower() != "all")
                        {
                            newconnection = newconnection.Where(s => s.CompanyName.ToLower() == selectCompany.SelectedValue.ToLower());
                            var paymentHistroy = newconnection.ToList().Select(x => new TransmissionsCostCalculator()
                            {

                                Id = x.Id,
                                RegistrationNumber = x.RegistartionNumber,
                                CompanyName = x.CompanyName,
                                Login = x.Login,
                                ElectricityConsumedatResidences = x.ElectricityConsumedAtResidence,
                                TotalFamilyMembers = x.TotalFamilyMembers,
                                TotalTrips = x.NumberofTrips,
                                CNGUseds = x.CNGUsed,
                                LPGCylinders = x.LPGUsed,
                                DieselConsumptions = x.DieselConsumption,
                                PetrolConsumptions = x.PetrolConsumption,
                                AutoRikshaws = x.CNGAutoRickshaw,
                                Buses = x.BusUse,
                                Trains = x.TrainUse,
                                EmployeeTotalemissionsperMonths = x.AverageEmissionperMonth,
                                AnnualCarbonFootprints = x.AnnualCarbonFootprint,
                                TotalTransportationUses = x.EmissionfromTransportation,
                                TotalDomesticUses = x.EmissionfromDomesticUse,
                                NumberOfTreesNeeded = x.NumberofTreesNeeded,
                                LandNeeded = x.LandNeededtoPlantTrees,
                                AverageAnnualCarbonFootprints = x.AverageAnnualCarbonFootprint,
                                CarbonEmissionReducePercentage = x.CarbonEmissionReducePercentage,
                                CarbonEmissionReviewYear = x.CarbonEmissionReviewYear,
                                CarbonEmissionReviewDate = x.CarbonEmissionReviewDate


                            }).ToList();
                            DataTable dt = ToDataTable(paymentHistroy);
                            string savefilepath = Server.MapPath("~/sitecore/admin/Transmission/SampleFile/Form-Records.csv");
                            DatatableToCSV(dt, savefilepath);
                        }
                        else if (selectCompany.SelectedValue.ToLower() == "all")
                        {
                            newconnection = newconnection.Where(s => s.CompanyName.ToLower() == "Adani Transmission Limited" || s.CompanyName.ToLower() == "Adani Power Limited" || s.CompanyName.ToLower() == "Adani Electricity Mumbai Limited" || s.CompanyName.ToLower() == "Others");
                            var paymentHistroy = newconnection.ToList().Select(x => new TransmissionsCostCalculator()
                            {
                                Id = x.Id,
                                RegistrationNumber = x.RegistartionNumber,
                                CompanyName = x.CompanyName,
                                Login = x.Login,
                                ElectricityConsumedatResidences = x.ElectricityConsumedAtResidence,
                                TotalFamilyMembers = x.TotalFamilyMembers,
                                TotalTrips = x.NumberofTrips,
                                CNGUseds = x.CNGUsed,
                                LPGCylinders = x.LPGUsed,
                                DieselConsumptions = x.DieselConsumption,
                                PetrolConsumptions = x.PetrolConsumption,
                                AutoRikshaws = x.CNGAutoRickshaw,
                                Buses = x.BusUse,
                                Trains = x.TrainUse,
                                EmployeeTotalemissionsperMonths = x.AverageEmissionperMonth,
                                AnnualCarbonFootprints = x.AnnualCarbonFootprint,
                                TotalTransportationUses = x.EmissionfromTransportation,
                                TotalDomesticUses = x.EmissionfromDomesticUse,
                                NumberOfTreesNeeded = x.NumberofTreesNeeded,
                                LandNeeded = x.LandNeededtoPlantTrees,
                                AverageAnnualCarbonFootprints = x.AverageAnnualCarbonFootprint,
                                CarbonEmissionReducePercentage = x.CarbonEmissionReducePercentage,
                                CarbonEmissionReviewYear = x.CarbonEmissionReviewYear,
                                CarbonEmissionReviewDate = x.CarbonEmissionReviewDate


                            }).ToList();
                            DataTable dt = ToDataTable(paymentHistroy);
                            string savefilepath = Server.MapPath("~/sitecore/admin/Transmission/SampleFile/Form-Records.csv");
                            DatatableToCSV(dt, savefilepath);
                        }
                    }


                    else
                    {

                        if (selectCompany.SelectedIndex != 0 && selectCompany.SelectedValue.ToLower() != "all")
                        {
                            newconnection = newconnection.Where(s => s.CompanyName.ToLower() == selectCompany.SelectedValue.ToLower());
                            var paymentHistroy = newconnection.ToList().Select(x => new TransmissionsCostCalculator()
                            {
                                Id = x.Id,
                                RegistrationNumber = x.RegistartionNumber,
                                CompanyName = x.CompanyName,
                                Login = x.Login,
                                ElectricityConsumedatResidences = x.ElectricityConsumedAtResidence,
                                TotalFamilyMembers = x.TotalFamilyMembers,
                                TotalTrips = x.NumberofTrips,
                                CNGUseds = x.CNGUsed,
                                LPGCylinders = x.LPGUsed,
                                DieselConsumptions = x.DieselConsumption,
                                PetrolConsumptions = x.PetrolConsumption,
                                AutoRikshaws = x.CNGAutoRickshaw,
                                Buses = x.BusUse,
                                Trains = x.TrainUse,
                                EmployeeTotalemissionsperMonths = x.AverageEmissionperMonth,
                                AnnualCarbonFootprints = x.AnnualCarbonFootprint,
                                TotalTransportationUses = x.EmissionfromTransportation,
                                TotalDomesticUses = x.EmissionfromDomesticUse,
                                NumberOfTreesNeeded = x.NumberofTreesNeeded,
                                LandNeeded = x.LandNeededtoPlantTrees,
                                AverageAnnualCarbonFootprints = x.AverageAnnualCarbonFootprint,
                                CarbonEmissionReducePercentage = x.CarbonEmissionReducePercentage,
                                CarbonEmissionReviewYear = x.CarbonEmissionReviewYear,
                                CarbonEmissionReviewDate = x.CarbonEmissionReviewDate



                            }).ToList();
                            DataTable dt = ToDataTable(paymentHistroy);
                            string savefilepath = Server.MapPath("~/sitecore/admin/Transmission/SampleFile/Form-Records.csv");
                            DatatableToCSV(dt, savefilepath);
                        }
                        else if (selectCompany.SelectedValue.ToLower() == "all")
                        {
                            newconnection = newconnection.Where(s => s.CompanyName.ToLower() == "Adani Transmission Limited" || s.CompanyName.ToLower() == "Adani Power Limited" || s.CompanyName.ToLower() == "Adani Electricity Mumbai Limited" || s.CompanyName.ToLower() == "Others");
                            var paymentHistroy = newconnection.ToList().Select(x => new TransmissionsCostCalculator()
                            {
                                Id = x.Id,
                                RegistrationNumber = x.RegistartionNumber,
                                CompanyName = x.CompanyName,
                                Login = x.Login,
                                ElectricityConsumedatResidences = x.ElectricityConsumedAtResidence,
                                TotalFamilyMembers = x.TotalFamilyMembers,
                                TotalTrips = x.NumberofTrips,
                                CNGUseds = x.CNGUsed,
                                LPGCylinders = x.LPGUsed,
                                DieselConsumptions = x.DieselConsumption,
                                PetrolConsumptions = x.PetrolConsumption,
                                AutoRikshaws = x.CNGAutoRickshaw,
                                Buses = x.BusUse,
                                Trains = x.TrainUse,
                                EmployeeTotalemissionsperMonths = x.AverageEmissionperMonth,
                                AnnualCarbonFootprints = x.AnnualCarbonFootprint,
                                TotalTransportationUses = x.EmissionfromTransportation,
                                TotalDomesticUses = x.EmissionfromDomesticUse,
                                NumberOfTreesNeeded = x.NumberofTreesNeeded,
                                LandNeeded = x.LandNeededtoPlantTrees,
                                AverageAnnualCarbonFootprints = x.AverageAnnualCarbonFootprint,
                                CarbonEmissionReducePercentage = x.CarbonEmissionReducePercentage,
                                CarbonEmissionReviewYear = x.CarbonEmissionReviewYear,
                                CarbonEmissionReviewDate = x.CarbonEmissionReviewDate




                            }).ToList();
                            DataTable dt = ToDataTable(paymentHistroy);
                            string savefilepath = Server.MapPath("~/sitecore/admin/AdaniGas/SampleFile/Form-Records.csv");
                            DatatableToCSV(dt, savefilepath);
                        }
                    }
                }
                else
                {
                    this.lblErroMsg.Text = "Please select proper date";
                    this.lblErroMsg.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblErroMsg.Text = "There is some technically problem. Please contact administrator.";
                Sitecore.Diagnostics.Log.Error("Form data -Export Error  " + ex.Message, this);
            }
        }


        protected void resetForm(object sender, EventArgs e)
        {
            Response.Redirect("/sitecore/admin/transmission/CarbonCalculatorAdminDashboard.aspx");
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
                Response.AddHeader("Content-Disposition", "attachment; filename=VendorEnquiryList.csv");
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
            using (StringWriter strwritter = new StringWriter())
            {
                using (HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter))
                {
                    Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    Response.ContentType = "application/vnd.ms-excel";
                    Response.AddHeader("Content-Disposition", "attachment;filename=" + FileName);
                    hiddenGrid1.GridLines = GridLines.Both;
                    hiddenGrid1.HeaderStyle.Font.Bold = true;
                    //htmltextwrtter.AddAttribute("New Export","random");
                    hiddenGrid1.RenderControl(htmltextwrtter);
                }
                Response.Write(strwritter.ToString());
                Response.End();
            }


        }

        protected void downloadCSV_Click(object sender, EventArgs e)
        {

            try
            {
                int flag = 0;
                if (!string.IsNullOrEmpty(dateFilterFrom.Text))
                {
                    flag++;
                }
                if (!string.IsNullOrEmpty(dateFilterTo.Text))
                {
                    flag++;
                }

                if (flag == 1)
                {
                    lblErroMsg.Text = "Please select valid Dates";
                    return;

                }

                //var TotalCarbonFootprintRecord = from rc in rdb.TransmissionInsertCostCalculators select rc;
                //var TotalCarbonFootprintRecord1 = rdb.TransmissionInsertCostCalculators.Join(rdb.TransmissionCarbonOffsetValues, x => x.RegistartionNumber, y => y.RegistartionNumber, (x, y) => new { x.Id, x.CompanyName, x.Login, x.RegistartionNumber, x.MonthName, x.Year, x.TotalFamilyMembers, x.ElectricityConsumedAtResidence, x.CNGUsed, x.LPGUsed, x.DieselConsumption, x.PetrolConsumption, x.CNGAutoRickshaw, x.BusUse, x.TrainUse, x.NumberofTrips, x.EmissionfromDomesticUse, x.EmissionfromTransportation, x.AverageEmissionperMonth, x.LandNeededtoPlantTrees, x.NumberofTreesNeeded, x.AverageAnnualCarbonFootprint, x.AnnualCarbonFootprint, x.CarbonEmissionReducePercentage, x.CarbonEmissionReviewYear, x.CarbonEmissionReviewDate, x.PageInfo, x.FormType, x.FormSubmitOn, x.NumberofTress, x.ProjectName, x.replacewithCNG, x.replacewithElectric,  y.PersonalTransport, y.PublicTransport, y.OnlineMeeting, y.OnlineMeetingValue, y.FiveStarAppliances, y.NoOfTreesNeededValue, y.FundNeededtoPlantTrees, y.FundNeededToPlantTreesValue, y.OffsetEmissionfromDomesticUse, y.TotalOffsetCarbonEmission, y.OffsetEmissionfromTransportation, y.AverageOffsetEmissionperMonth, y.OffsetEmissionfromAirTrips, y.OffsetAnnualCarbonFootprint, x.EstimatedCarbonAnnualizedValue,y.OffSetWithCarPooling,y.OffSetwithCNG,y.OffSetwithElectricVehicle,y.OffSetwithCycle,y.OffSetWithPublicTransport});

                var TotalCarbonFootprintRecord1 = rdb.TransmissionInsertCostCalculators.Join(rdb.TransmissionCarbonOffsetValues, x => x.RegistartionNumber, y => y.RegistartionNumber, (x, y) => new { x.Id, x.Login, x.CompanyName, x.RegistartionNumber, x.MonthName, x.Year, x.TotalFamilyMembers, x.ElectricityConsumedAtResidence, x.CNGUsed, x.LPGUsed, x.DieselConsumption, x.PetrolConsumption, x.CNGAutoRickshaw, x.BusUse, x.TrainUse, x.NumberofTrips, x.EmissionfromDomesticUse, x.EmissionfromTransportation, x.AverageEmissionperMonth, x.LandNeededtoPlantTrees, x.NumberofTreesNeeded, x.AverageAnnualCarbonFootprint, x.AnnualCarbonFootprint, x.CarbonEmissionReducePercentage, x.CarbonEmissionReviewYear, x.CarbonEmissionReviewDate, x.PageInfo, x.FormType, x.FormSubmitOn, x.NumberofTress, x.ProjectName, x.replacewithCNG, x.replacewithElectric, x.EmissiontotalTripsValue, y.PersonalTransport, OffSetWithCab_Taxi_Shared = y.PublicTransport, OffSetWithOnlineMeeting = y.OnlineMeeting, y.OnlineMeetingValue, OffSetWithFiveStarAppliances = y.FiveStarAppliances, y.NoOfTreesNeededValue, y.FundNeededtoPlantTrees, ProjectName_100_million_tree = y.PlantationTreesNeededValue, Offset_with_project = y.PlantationTreesNeeded, y.FundNeededToPlantTreesValue, Offset_with_tree_plantation = y.NumberofTreesNeeded, y.OffsetEmissionfromDomesticUse, y.TotalOffsetCarbonEmission, y.OffsetEmissionfromTransportation, EmissionfromAirtrip = x.EmissiontotalTripsValue, y.AverageOffsetEmissionperMonth, y.OffsetEmissionfromAirTrips, y.OffsetAnnualCarbonFootprint, x.EstimatedCarbonAnnualizedValue, y.OffSetWithCarPooling, y.OffSetwithCNG, y.OffSetwithElectricVehicle, y.OffSetwithCycle, y.OffSetWithPublicTransport });

                var TotalCarbonFootprintRecord = TotalCarbonFootprintRecord1.Join(rdb.Transmission_CarbonCalculator_RegistrationForms, x => x.Login, y => y.MobileNumber, (x, y) => new { x.CompanyName, y.FullName, Mobile_No =x.Login, y.EmailId, x.RegistartionNumber, x.FormSubmitOn, x.MonthName, x.Year, x.TotalFamilyMembers, x.ElectricityConsumedAtResidence, x.CNGUsed, x.LPGUsed, x.DieselConsumption, x.PetrolConsumption, x.CNGAutoRickshaw, x.BusUse, x.TrainUse, NumberofAirTrips=x.NumberofTrips, x.EmissionfromDomesticUse, x.EmissionfromTransportation, x.EmissionfromAirtrip, x.AverageEmissionperMonth, x.AnnualCarbonFootprint, x.LandNeededtoPlantTrees, x.NumberofTreesNeeded, x.OffSetWithCarPooling, x.OffSetwithCNG, x.OffSetwithElectricVehicle, x.OffSetwithCycle, x.OffSetWithPublicTransport, x.OffSetWithCab_Taxi_Shared, x.OffSetWithOnlineMeeting, x.OffSetWithFiveStarAppliances, NumberofTress = x.NoOfTreesNeededValue, x.Offset_with_tree_plantation, x.ProjectName_100_million_tree, x.Offset_with_project, FundNeededtoPlantTrees = x.FundNeededToPlantTreesValue, offset_with_fund_provision = x.FundNeededtoPlantTrees, x.OffsetEmissionfromDomesticUse, x.OffsetEmissionfromTransportation, x.OffsetEmissionfromAirTrips, x.TotalOffsetCarbonEmission, x.OffsetAnnualCarbonFootprint, x.EstimatedCarbonAnnualizedValue, x.CarbonEmissionReducePercentage, x.CarbonEmissionReviewYear, x.CarbonEmissionReviewDate });
                if (selectCompany.SelectedIndex != 0)
                {
                    TotalCarbonFootprintRecord = TotalCarbonFootprintRecord.Where(s => s.CompanyName.ToLower() == selectCompany.SelectedValue.ToLower());
                }

                if (flag == 2)
                {
                    TotalCarbonFootprintRecord = TotalCarbonFootprintRecord.Where(s => s.FormSubmitOn.Value.Date >= DateTime.Parse(dateFilterFrom.Text).Date && s.FormSubmitOn.Value.Date <= DateTime.Parse(dateFilterTo.Text).Date);
                }
                foreach (var i in TotalCarbonFootprintRecord)
                {
                    var userRegistrationDetail = rdb.Transmission_CarbonCalculator_RegistrationForms.Where(x => x.EmailId == i.Mobile_No || x.MobileNumber == i.Mobile_No).ToList();
                }

                hiddenGrid1.DataSource = TotalCarbonFootprintRecord;
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
            if (!string.IsNullOrEmpty(dateFilterFrom.Text))
            {
                flag++;
            }
            if (!string.IsNullOrEmpty(dateFilterTo.Text))
            {
                flag++;
            }

            if (flag == 1)
            {
                lblErroMsg.Text = "Please select valid Dates";
                return;

            }

            var TotalCarbonFootprintRecord = rdb.TransmissionInsertCostCalculators.ToList();
            
            if (selectCompany.SelectedIndex != 0)
            {

                TotalCarbonFootprintRecord = rdb.TransmissionInsertCostCalculators.Where(s => s.CompanyName.ToLower() == selectCompany.SelectedValue.ToLower()).ToList();
            }

            if (flag == 2)
            {
                TotalCarbonFootprintRecord = TotalCarbonFootprintRecord.Where(s => s.FormSubmitOn.Value.Date >= DateTime.Parse(dateFilterFrom.Text).Date && s.FormSubmitOn.Value.Date <= DateTime.Parse(dateFilterTo.Text).Date).ToList();
            }

            //bind the filtered data to grid
            gridrecord1.Controls.Clear();
            LiteralControl griddata = new LiteralControl();
            griddata.Text = "<table id='example' class='table table-striped table-bordered' style='width:100%'><thead><tr><th style='width:13%'> Submitted On </th><th> Registration No </th><th> Name </th><th> Company Name </th><th> Contact No </th><th> E-mail ID </th><th>Annual Carbon Footprint</th><th>Audit Trail History</th></tr></thead><tbody>";
            foreach (var item in TotalCarbonFootprintRecord)
            {
                var userDetail = rdb.Transmission_CarbonCalculator_RegistrationForms.Where(x => x.EmailId == item.Login || x.MobileNumber == item.Login).FirstOrDefault();
                var submitdate = "";
                if (item.FormSubmitOn.GetValueOrDefault() != null)
                {
                    submitdate = item.FormSubmitOn.Value.ToString("dd MMM yyyy");
                }

                griddata.Text = griddata.Text + "<tr><td>" + submitdate + "</td><td><a href=\"/sitecore/admin/transmission/CarbonCalculatorUserDetails.aspx?id=" + item.RegistartionNumber + "\"  name=\"RegNobtn\" id=\"RegNobtn\" class=\"btn initiatives-btn2\">" + item.RegistartionNumber + "</a></td><td>" + userDetail.FullName + "</td><td>" + userDetail.Company + "</td><td>" + userDetail.MobileNumber + "</td><td>" + userDetail.EmailId + "</td><td>" + item.AnnualCarbonFootprint + "</td><td><a href=\"/sitecore/admin/transmission/CarbonCalculatorAuditTrailHistory.aspx?id=" + item.Id + "&LoginID=" + item.Login + " class=\"btn initiatives-btn2\">Audit Trail</a></td><td></tr>";
            }
            griddata.Text = griddata.Text + "</tbody></table>";
            gridrecord1.Controls.Add(griddata);
            hiddenGrid1.DataSource = TotalCarbonFootprintRecord;
            hiddenGrid1.DataBind();
        }
        protected void LogOut_Click(object sender, EventArgs e)
        {
            if (Sitecore.Context.User.IsAuthenticated)
                FormsAuthentication.SignOut();
            Response.Redirect("/Sitecore/login?ReturnUrl=%2fsitecore%2fadmin%2fCarbonCalculatorAdminDashboard.aspx");
        }
    }
}