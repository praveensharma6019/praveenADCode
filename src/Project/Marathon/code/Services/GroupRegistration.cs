using InstamojoAPI;
using InstaMojoIntegration.Models;
using Sitecore.Diagnostics;
using Sitecore.Marathon.Website.Models;
using Sitecore.Marathon.Website.Validation;
using System;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Sitecore.Marathon.Website.Services
{
    public class GroupRegistration
    {
        MarathonRepository repo = new MarathonRepository();
        AhmedabadMarathonRegistrationDataContext marathonDb = new AhmedabadMarathonRegistrationDataContext();

        public void GroupRegisteration(RegistrationModel groupModel, string SessionId, string UserId)
        {
            try
            {
                Log.Error("Marathon GroupRegisteration Model validation Pass" + groupModel, this);
                AhmedabadMarathonRegistration groupRegistration = new AhmedabadMarathonRegistration();
                DateTime getDateOfBirth = DateTime.Parse(groupModel.DateofBirth, CultureInfo.GetCultureInfo("en-gb"));
                groupRegistration.BIBNumber = repo.GenerateRandomBIBNumber(groupModel.RaceDistance);
                groupRegistration.IsRaceDistanceChanged = false;
                groupRegistration.ReferenceCode = groupModel.ReferenceCode;
                groupRegistration.FirstName = groupModel.FirstName;
                groupRegistration.LastName = groupModel.LastName;
                groupRegistration.RaceDistance = groupModel.RaceDistance;
                groupRegistration.RunType = groupModel.RunType;
                if (groupModel.RunType == "Charity")
                {
                    groupRegistration.RaceAmount = decimal.Parse(MarathonHelper.GetRaceAmount(groupModel.RaceDistance));
                    groupModel.RaceAmount = decimal.Parse(MarathonHelper.GetRaceAmount(groupModel.RaceDistance));
                    groupModel.DonationAmount = groupModel.DonationAmount - groupModel.RaceAmount;
                }
                else
                {
                    groupRegistration.RaceAmount = System.Convert.ToDecimal(MarathonHelper.GetRaceAmount(groupModel.RaceDistance));
                    groupModel.RaceAmount = System.Convert.ToDecimal(MarathonHelper.GetRaceAmount(groupModel.RaceDistance));
                }
                groupRegistration.EmployeeEmailId = groupModel.EmployeeEmailId;
                groupRegistration.EmployeeID = groupModel.EmployeeID;
                groupRegistration.DateofBirth = getDateOfBirth;
                groupRegistration.Email = groupModel.Email;
                groupRegistration.ContactNumber = groupModel.ContactNumber;
                groupRegistration.Gender = groupModel.Gender;
                groupRegistration.TShirtSize = groupModel.TShirtSize;
                groupRegistration.IdentityProofType = groupModel.IdentityProofType;
                groupRegistration.UserId = UserId;

                BlobAPIService blobAPIService = new BlobAPIService();
                groupRegistration.IDCardAttachment = blobAPIService.BlobAPI(groupModel.IDCardAttachment);

                groupRegistration.Country = groupModel.Country;
                if (groupModel.State != null && !string.IsNullOrEmpty(groupModel.State))
                {
                    AhmedabadMarathonRegistrationDataContext StateTable = new AhmedabadMarathonRegistrationDataContext();
                    var state = StateTable.MarathonStates.Where(x => x.stateid.ToString() == groupModel.State).FirstOrDefault();
                    groupRegistration.State = state.statename.ToString();
                }
                if (groupModel.City != null && !string.IsNullOrEmpty(groupModel.City))
                {
                    AhmedabadMarathonRegistrationDataContext StateTable = new AhmedabadMarathonRegistrationDataContext();
                    var city = StateTable.MarathonCities.Where(x => x.Cityid.ToString() == groupModel.City).FirstOrDefault();
                    groupRegistration.City = city.CityName.ToString();
                }
                groupRegistration.Address = groupModel.Address;
                groupRegistration.Pincode = groupModel.Pincode;
                groupRegistration.EmergencyContactName = groupModel.EmergencyContactName;
                groupRegistration.EmergencyContactNumber = groupModel.EmergencyContactNumber;
                groupRegistration.BloodGroup = groupModel.BloodGroup;
                groupRegistration.AnyKnownAllergies = groupModel.AnyKnownAllergies;
                groupRegistration.NamePreferredonBIB = groupModel.FirstName + " " + groupModel.LastName;
                groupRegistration.Updated = true;
                groupRegistration.PaymentStatus = "pending";
                groupRegistration.DonationAmount = groupModel.DonationAmount;
                groupRegistration.PANNumber = groupModel.PANNumber;
                groupRegistration.TaxExemptionCause = groupModel.TaxExemptionCause;
                groupRegistration.TaxExemptionCertificate = groupModel.TaxExemptionCertificate.ToString();
                groupRegistration.FinalAmount = groupRegistration.RaceAmount;
                groupRegistration.RegistrationStatus = "successful";
                groupRegistration.DiscountRate = "0";
                groupRegistration.AmountReceived = 0;

                DateTime currentDate = DateTime.Now;
                TimeSpan timespan = currentDate.Subtract(getDateOfBirth);
                groupRegistration.Age = ((int)(timespan.TotalDays) / 365).ToString();
                groupRegistration.FormSubmitOn = DateTime.Parse(DateTime.Now.ToString());
                groupRegistration.CreatedBy = SessionId;
                if (!string.IsNullOrEmpty(groupModel.ReferenceCode))
                {
                    ApplyCodeResponse codeResponse = RegistrationFormValidation.ApplyCouponCode(groupModel.ReferenceCode, groupModel.RaceAmount, groupModel.RunType, groupModel.EmployeeID);
                    groupRegistration.PaymentStatus = codeResponse.PaymentStatus;
                    groupRegistration.RegistrationStatus = codeResponse.RegistrationStatus;
                    groupRegistration.DiscountRate = codeResponse.DiscountRate;
                    groupRegistration.AmountReceived = codeResponse.AmountReceived;
                    groupRegistration.FinalAmount = codeResponse.FinalAmount;
                    groupModel.FinalAmount = codeResponse.FinalAmount;
                }
                else
                {
                    groupRegistration.ReferenceCode = "";
                }
                groupRegistration.FinalAmount = groupRegistration.FinalAmount + groupRegistration.DonationAmount;
                marathonDb.AhmedabadMarathonRegistrations.InsertOnSubmit(groupRegistration);
                marathonDb.SubmitChanges();

                DataTable dt = new DataTable();
                if (HttpContext.Current.Session["GroupCart"] != null)
                {
                    dt = HttpContext.Current.Session["GroupCart"] as DataTable;
                }
                else
                {
                    dt.Columns.Add("RaceDistance");
                    dt.Columns.Add("FirstName");
                    dt.Columns.Add("LastName");
                    dt.Columns.Add("Email");
                    dt.Columns.Add("ContactNumber");
                    dt.Columns.Add("FinalAmount");
                    dt.Columns.Add("Userid");
                    dt.Columns.Add("RaceAmount");
                    dt.Columns.Add("DiscountRate");
                    dt.Columns.Add("PaymentStatus");
                    dt.Columns.Add("RunType");
                    dt.Columns.Add("DonationAmount");
                }
                DataRow dr = dt.NewRow();
                dr[0] = groupModel.RaceDistance;
                dr[1] = groupModel.FirstName;
                dr[2] = groupModel.LastName;
                dr[3] = groupModel.Email;
                dr[4] = groupModel.ContactNumber;
                dr[5] = groupRegistration.FinalAmount;
                dr[6] = groupRegistration.UserId;
                dr[7] = groupModel.RaceAmount;
                dr[8] = groupRegistration.DiscountRate;
                dr[9] = groupRegistration.PaymentStatus;
                dr[10] = groupModel.RunType;
                dr[11] = groupModel.DonationAmount;
                dt.Rows.Add(dr);
                dt.NewRow();

                HttpContext.Current.Session["GroupCart"] = dt;
            }
            catch (Exception ex)
            {
                Log.Error("Marathon GroupRegisteration add participant exception occured" + ex.Message, this);
            }
        }


        public string GroupPayment(string UserSessionId, decimal finalAmount, string RequestUrl)
        {
            try
            {
                Log.Info("Marathon GroupPayment  function start by" + UserSessionId + "Final Ammount is" + finalAmount, this);
                using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                {
                    AhmedabadMarathonRegistration m = objcontext.AhmedabadMarathonRegistrations.Where(x => x.UserId == UserSessionId).FirstOrDefault();

                    AhmedabadMarathonPaymentHistory objPayment = new AhmedabadMarathonPaymentHistory
                    {

                        UserId = (m.UserId).ToString(),
                        TransactionId = EncryptionDecryption.GenerateRandomOrderId(string.Empty),
                        Id = Guid.NewGuid(),
                        Amount = System.Convert.ToString(finalAmount),
                        Email = m.Email,
                        Mobile = m.ContactNumber,
                        UserType = "Guest",
                        GatewayType = "Insta-Mojo",
                        Created_Date = System.DateTime.Now,
                        RequestTime = System.DateTime.Now,
                        CreatedBy = m.FirstName + " " + m.LastName,
                        AccountNumber = m.ReferenceCode,
                        PaymentType = "Token Amount",
                        OrderId = (Guid.NewGuid()).ToString(),
                        ResponseMsg = RequestUrl
                    };
                    objcontext.AhmedabadMarathonPaymentHistories.InsertOnSubmit(objPayment);
                    objcontext.SubmitChanges();

                    PaymentService objPaymentService = new PaymentService();
                    ResultPayment Objresult = new ResultPayment();
                    Objresult = objPaymentService.GroupPayment(m, objPayment.OrderId, finalAmount);

                    if (Objresult.IsSuccess)
                    {
                        Log.Error("Marathon GroupPayment Success :  result is" + Objresult.Message, this);
                        return Objresult.Message;
                    }
                    else
                    {
                        Log.Error("Marathon GroupPayment Fail :  result is" + Objresult.Message, this);
                        return "/registration-failed";
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("Marathon GroupPayment  function failed exception occured", ex.Message);
                return "/registration-failed";
            }
           
        }
    }
}