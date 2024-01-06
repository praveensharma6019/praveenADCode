using Sitecore.Diagnostics;
using Sitecore.Transmission.Website.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Sitecore.Transmission.Website.sitecore.admin.Transmission
{
    public partial class TransmissionCalculateCarbonFootprint : System.Web.UI.Page
    {

      
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        protected void Button1_Click(object sender, EventArgs e)
        {
        TransmissionContactFormRecordDataContext     rdb = new TransmissionContactFormRecordDataContext();

            try
            {
                this.lblErroMsg.Text = "";
                this.lblErroMsg.Visible = false;
                DateTime dateTime = DateTime.Parse(this.TextBoxFrom.Text);
                DateTime dateTime1 = DateTime.Parse(this.TextBoxTo.Text);
                if (System.Convert.ToDateTime(this.TextBoxTo.Text) >= System.Convert.ToDateTime(this.TextBoxFrom.Text))
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

                        if (selectConnectionType.SelectedIndex != 0 && selectConnectionType.SelectedValue.ToLower() != "all")
                        {
                            newconnection = newconnection.Where(s => s.CompanyName.ToLower() == selectConnectionType.SelectedValue.ToLower());
                            var paymentHistroy = newconnection.ToList().Select(x => new TransmissionsCostCalculator()
                            {

                                Id = x.Id,
                                RegistrationNumber=x.RegistartionNumber,
                                CompanyName=x.CompanyName,
                                Login=x.Login,
                                ElectricityConsumedatResidences=x.ElectricityConsumedAtResidence,
                                TotalFamilyMembers=x.TotalFamilyMembers,
                                TotalTrips=x.NumberofTrips,
                                CNGUseds=x.CNGUsed,
                                LPGCylinders=x.LPGUsed,
                                DieselConsumptions=x.DieselConsumption,
                                PetrolConsumptions=x.PetrolConsumption,
                                AutoRikshaws=x.CNGAutoRickshaw,
                                Buses=x.BusUse,
                                Trains=x.TrainUse,
                                EmployeeTotalemissionsperMonths=x.AverageEmissionperMonth,
                                AnnualCarbonFootprints=x.AnnualCarbonFootprint,
                                TotalTransportationUses=x.EmissionfromTransportation,
                                TotalDomesticUses=x.EmissionfromDomesticUse,
                                NumberOfTreesNeeded=x.NumberofTreesNeeded,
                                LandNeeded=x.LandNeededtoPlantTrees,
                                AverageAnnualCarbonFootprints=x.AverageAnnualCarbonFootprint,
                                CarbonEmissionReducePercentage=x.CarbonEmissionReducePercentage,
                                CarbonEmissionReviewYear=x.CarbonEmissionReviewYear,
                                CarbonEmissionReviewDate=x.CarbonEmissionReviewDate


                            }).ToList();
                            DataTable dt = ToDataTable(paymentHistroy);
                            string savefilepath = Server.MapPath("~/sitecore/admin/Transmission/SampleFile/Form-Records.csv");
                            DatatableToCSV(dt, savefilepath);
                        }
                        else if (selectConnectionType.SelectedValue.ToLower() == "all")
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

                        if (selectConnectionType.SelectedIndex != 0 && selectConnectionType.SelectedValue.ToLower() != "all")
                        {
                            newconnection = newconnection.Where(s => s.CompanyName.ToLower() == selectConnectionType.SelectedValue.ToLower());
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
                        else if (selectConnectionType.SelectedValue.ToLower() == "all")
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
                Response.AddHeader("Content-Disposition", "attachment; filename=Form-Records.csv");
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
    }
}