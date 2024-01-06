using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net.Mime;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using System.Globalization;
namespace Sitecore.Transmission.Website.sitecore.admin.Transmission
{
    public partial class CarbonCalculatorUserDetails : System.Web.UI.Page
    {
        TransmissionContactFormRecordDataContext rdb = new TransmissionContactFormRecordDataContext();
        protected void Page_Load(object sender, EventArgs e)
        {

            string RegNo = Request.QueryString["id"].ToString();
            var InquiryRecord = (from rc in rdb.TransmissionInsertCostCalculators
                                 where (rc.RegistartionNumber == RegNo)
                                 select rc).FirstOrDefault();
            if (InquiryRecord != null)
            {
                var userRegistrationDetail = rdb.Transmission_CarbonCalculator_RegistrationForms.Where(x => x.EmailId == InquiryRecord.Login || x.MobileNumber == InquiryRecord.Login).FirstOrDefault();

                fullname.Text = userRegistrationDetail.FullName;
                CompanyName1.Text = userRegistrationDetail.Company;
                EmailId.Text = userRegistrationDetail.EmailId;
                ContactNo.Text = userRegistrationDetail.MobileNumber;
                regDate.Text = userRegistrationDetail.Created_Date.ToString();

            }
            regNo.Text = InquiryRecord.RegistartionNumber;
            month.Text = InquiryRecord.MonthName;
            year.Text = InquiryRecord.Year;
            TotalMember.Text = InquiryRecord.TotalFamilyMembers;
            electricityConsumedResidence.Text = InquiryRecord.ElectricityConsumedAtResidence + " kg CO2eq";
            cngUsed.Text = InquiryRecord.CNGUsed + " kg CO2eq";
            LPGUsed.Text = InquiryRecord.LPGUsed + " kg CO2eq";
            dieselConsumption.Text = InquiryRecord.DieselConsumption + " kg CO2eq";
            PetrolConsumption.Text = InquiryRecord.PetrolConsumption + " kg CO2eq";
            CNGAutoRikshaw.Text = InquiryRecord.CNGAutoRickshaw + " kg CO2eq";
            BUSUse.Text = InquiryRecord.BusUse + " kg CO2eq";
            TrainsUse.Text = InquiryRecord.TrainUse + " kg CO2eq";
            IFormatProvider provider = CultureInfo.CreateSpecificCulture("en-US");
            double s = double.Parse(InquiryRecord.EmissionfromDomesticUse, provider);
            string TotalDomesticUses = s.ToString("0.00");
            double t = double.Parse(InquiryRecord.EmissionfromTransportation, provider);
            string TotalTransportationUses = t.ToString("0.00");
            double y = double.Parse(InquiryRecord.NumberofTrips, provider);
            string TotalTrips = y.ToString("0.00");
            decimal totlaemission = Decimal.Parse(InquiryRecord.EmissionfromDomesticUse) + Decimal.Parse(InquiryRecord.EmissionfromTransportation) + Decimal.Parse(InquiryRecord.NumberofTrips);
            double z = double.Parse(totlaemission.ToString(), provider);
            string totalCarbonEmission = z.ToString("0.00");
            double a = double.Parse(InquiryRecord.AverageEmissionperMonth, provider);
            string EmployeeTotalemissionsperMonths = a.ToString("0.00");
            AirTrips.Text = TotalTrips + " Tonnes";
            EmissionfromDomesticUse.Text = TotalDomesticUses + " Tonnes";
            EmissionfromTransportation.Text = TotalTransportationUses + " Tonnes";
            AverageEmissionperMonth.Text = EmployeeTotalemissionsperMonths.ToString() + " Tonnes";
            LandNeededtoPlantTrees.Text = InquiryRecord.LandNeededtoPlantTrees + " Hectares";
            NumberofTreesNeeded.Text = InquiryRecord.NumberofTreesNeeded + " Nos";
           // AverageAnnualCarbonFootprint.Text = InquiryRecord.AverageAnnualCarbonFootprint;
            AnnualCarbonFootprint.Text = InquiryRecord.AnnualCarbonFootprint + " Tonnes";

            TotalCarbonEmission.Text = totalCarbonEmission.ToString() + " Tonnes";
            //offset calculation
            var OffsetCarbonCalculationRecord = (from rc in rdb.TransmissionCarbonOffsetValues
                                                 where (rc.RegistartionNumber == RegNo)
                                                 select rc).FirstOrDefault();
            PersonalTransport.Text = OffsetCarbonCalculationRecord.PersonalTransport + " kg CO2eq";
            PublicTransport.Text = OffsetCarbonCalculationRecord.PublicTransport + " kg CO2eq";
            OnlineMeeting.Text = OffsetCarbonCalculationRecord.OnlineMeeting + " kg CO2eq";
            FiveStarAppliances.Text = OffsetCarbonCalculationRecord.FiveStarAppliances + " kg CO2eq";
            
            OffsetEmissionfromDomesticUse.Text = OffsetCarbonCalculationRecord.OffsetEmissionfromDomesticUse + " Tonnes";
            OffsetEmissionfromTransportation.Text = OffsetCarbonCalculationRecord.OffsetEmissionfromTransportation + " Tonnes";
            OffsetEmissionfromAirTrips.Text = OffsetCarbonCalculationRecord.OffsetEmissionfromAirTrips + " Tonnes";
            TotalOffsetCarbonEmission.Text = OffsetCarbonCalculationRecord.TotalOffsetCarbonEmission + " Tonnes";
            AverageOffsetEmissionperMonth.Text = OffsetCarbonCalculationRecord.AverageOffsetEmissionperMonth + " Tonnes";
            OffsetAnnualCarbonFootprint.Text = OffsetCarbonCalculationRecord.OffsetAnnualCarbonFootprint + " Tonnes";
            SubmittedOn.Text = InquiryRecord.FormSubmitOn.ToString();
            //var treeplantaionemiision = Double.Parse(OffsetCarbonCalculationRecord.NumberofTreesNeeded) / 1.81;
            //EmissionOffsetForTreePlantation.Text = treeplantaionemiision.ToString() + " Tonnes";
            //var treefundingemission = Double.Parse(OffsetCarbonCalculationRecord.FundNeededtoPlantTrees);
            //Double trees = (treefundingemission / 1.81) / 1000;
            //OffsetEmissionforFundingTrees.Text = trees.ToString() + " Tonnes";
            //OffsetNumberofTreesNeeded.Text = OffsetCarbonCalculationRecord.NumberofTreesNeeded;
            //OffsetFundNeededtoPlantTrees.Text = OffsetCarbonCalculationRecord.FundNeededtoPlantTrees + " INR";
            var treeneeded = Double.Parse(OffsetCarbonCalculationRecord.NumberofTreesNeeded) / 1.81;
            var fundfortrees = Double.Parse(OffsetCarbonCalculationRecord.FundNeededtoPlantTrees);
            var emissionOffsetForTreePlantation = Double.Parse(OffsetCarbonCalculationRecord.NumberofTreesNeeded).ToString("0.##");
            EmissionOffsetForTreePlantation.Text = emissionOffsetForTreePlantation + " Tonnes";
            var offsetEmissionforFundingTrees = Double.Parse(OffsetCarbonCalculationRecord.FundNeededtoPlantTrees).ToString("0.##");
            OffsetEmissionforFundingTrees.Text = offsetEmissionforFundingTrees + " Tonnes";
            Double trees = (fundfortrees / 1.81) * 1000;
            OffsetNumberofTreesNeeded.Text = treeneeded.ToString();
            OffsetFundNeededtoPlantTrees.Text = trees.ToString() + " INR";


            Session["RegistrationNo"] = InquiryRecord.RegistartionNumber;

        }
    }
}