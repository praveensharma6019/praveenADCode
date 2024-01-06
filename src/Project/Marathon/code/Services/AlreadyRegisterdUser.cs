using InstamojoAPI;
using InstaMojoIntegration.Models;
using Sitecore.Diagnostics;
using Sitecore.Marathon.Website.Controllers;
using Sitecore.Marathon.Website.Models;
using Sitecore.Marathon.Website.Validation;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Sitecore.Marathon.Website.Services
{
    public class AlreadyRegisterdUser
    {
        MarathonRepository repo = new MarathonRepository();
        AhmedabadMarathonRegistrationDataContext marathonDb = new AhmedabadMarathonRegistrationDataContext();
        public RegistrationModel LastYearDetail(string userId)
        {
            RegistrationModel lastYearDetail = new RegistrationModel();
            try
            {
                lastYearDetail = marathonDb.AhmedabadMarathonRegistrations.Where(val => val.UserId.ToString() == userId).OrderByDescending(x => x.FormSubmitOn).Select(val => new RegistrationModel()
                {
                    AffiliateCode = val.Id.ToString() ?? "",
                    RaceDistance = val.RaceDistance ?? "",
                    RaceAmount = decimal.Parse(val.RaceAmount.ToString() ?? "0.00"),
                    FirstName = val.FirstName ?? "",
                    LastName = val.LastName ?? "",
                    DateofBirth = val.DateofBirth.ToString() ?? "0.00",
                    Email = val.Email ?? "",
                    ContactNumber = val.ContactNumber ?? "",
                    Gender = val.Gender ?? "",
                    TShirtSize = val.TShirtSize ?? "",
                    NamePreferredonBIB = val.NamePreferredonBIB ?? "",
                    IdentityProofType = val.IdentityProofType ?? "",
                    IdentityProofNumber = val.IDCardAttachment ?? "",
                    Country = val.Country ?? "",
                    State = val.State ?? "",
                    City = val.City ?? "",
                    Address = val.Address ?? "",
                    Pincode = val.Pincode ?? "",
                    EmergencyContactName = val.EmergencyContactName ?? "",
                    EmergencyContactNumber = val.EmergencyContactNumber ?? "",
                    BloodGroup = val.BloodGroup ?? "",
                    AnyKnownAllergies = val.AnyKnownAllergies ?? "",
                    PaymentStatus = val.PaymentStatus ?? "",
                    RegistrationStatus = val.RegistrationStatus ?? "",
                    FormSubmitOn = DateTime.Parse(val.FormSubmitOn.ToString() ?? DateTime.Now.ToString(), CultureInfo.GetCultureInfo("en-gb")),
                    RunType = val.RunType ?? "",
                    Age = val.Age ?? "",
                    RunMode = val.RunMode ?? "",
                    BIBNumber = val.BIBNumber ?? "",
                    DonationAmount = decimal.Parse(val.DonationAmount.ToString()??"0.00"),
                    DiscountRate = val.DiscountRate ?? "",
                    AmountReceived = decimal.Parse(val.AmountReceived.ToString() ?? "0.00")
                }).FirstOrDefault();
                return lastYearDetail;
            }
            catch (Exception ex)
            {
                Log.Error("Error Occured  Already Registered User Last year Detail", "");
                return lastYearDetail;
            }
        }

        public string ALreadyRegisteredUserRegistration(RegisteredUserRegistration registeredUser, string UserId, string RequestUrl)
        {
            try
            {   
                Log.Error("Already registred User's registration start", "");
                if (!Captcha.IsReCaptchValidV3(registeredUser.reResponse))
                {
                    Log.Info("Already registred User's registration Captcha validation failed", "");
                    return "Invalid Captcha";
                }
                RegistrationModel alreadyRegisteredUser = marathonDb.AhmedabadMarathonRegistrations.Where(val => val.UserId.ToString() == UserId).OrderByDescending(x => x.FormSubmitOn).Select(val => new RegistrationModel()
                {
                    FirstName = val.FirstName ?? "",
                    LastName = val.LastName ?? "",
                    DateofBirth = val.DateofBirth.ToString() ?? "0.00",
                    Email = val.Email ?? "",
                    ContactNumber = val.ContactNumber ?? "",
                    Gender = val.Gender ?? "",
                    TShirtSize = val.TShirtSize ?? "",
                    NamePreferredonBIB = val.NamePreferredonBIB ?? "",
                    IdentityProofType = val.IdentityProofType ?? "",
                    IdentityProofNumber = val.IDCardAttachment ?? "",
                    Country = val.Country ?? "",
                    State = val.State ?? "",
                    City = val.City ?? "",
                    Address = val.Address ?? "",
                    Pincode = val.Pincode ?? "",
                    EmergencyContactName = val.EmergencyContactName ?? "",
                    EmergencyContactNumber = val.EmergencyContactNumber ?? "",
                    BloodGroup = val.BloodGroup ?? "",
                    AnyKnownAllergies = val.AnyKnownAllergies ?? "",
                    ReferenceCode ="",
                    OrderId =""
                }).FirstOrDefault();

                Log.Error("Already registred User's information recieved from Database", "");
                AhmedabadMarathonRegistration Registration = new AhmedabadMarathonRegistration();
                Registration.ReferenceCode = registeredUser.ReferenceCode;
                alreadyRegisteredUser.ReferenceCode = registeredUser.ReferenceCode;
                Registration.RunMode = registeredUser.RunMode;
                Registration.RunType = registeredUser.RunType;
                Registration.RaceDistance = registeredUser.RaceDistance;
                if (registeredUser.RunType == "Charity")
                {
                    Registration.RaceAmount = decimal.Parse(MarathonHelper.GetRaceAmount(registeredUser.RaceDistance));
                    alreadyRegisteredUser.FinalAmount = decimal.Parse(registeredUser.DonationAmount.ToString());
                    Registration.FinalAmount = decimal.Parse(registeredUser.DonationAmount.ToString());
                    Registration.DonationAmount = registeredUser.DonationAmount - Registration.RaceAmount;
                }
                else
                {
                    Registration.RaceAmount = System.Convert.ToDecimal(MarathonHelper.GetRaceAmount(registeredUser.RaceDistance));
                    alreadyRegisteredUser.FinalAmount = decimal.Parse(Registration.RaceAmount.ToString());
                    Registration.FinalAmount = decimal.Parse(Registration.RaceAmount.ToString());
                }
                Registration.PANNumber = registeredUser.PANNumber;
                Registration.TaxExemptionCause = registeredUser.TaxExemptionCause;
                Registration.TaxExemptionCertificate = registeredUser.TaxExemptionCertificate.ToString();
                Registration.EmployeeEmailId = registeredUser.EmployeeEmailId;
                Registration.EmployeeID = registeredUser.EmployeeID;



                Registration.BIBNumber = repo.GenerateRandomBIBNumber(registeredUser.RaceDistance);
                Registration.FirstName = alreadyRegisteredUser.FirstName;
                Registration.LastName = alreadyRegisteredUser.LastName;
                DateTime getDateOfBirth = DateTime.Parse(alreadyRegisteredUser.DateofBirth, CultureInfo.GetCultureInfo("en-gb"));
                Registration.DateofBirth = getDateOfBirth;
                Registration.Email = alreadyRegisteredUser.Email;
                Registration.ContactNumber = alreadyRegisteredUser.ContactNumber;
                Registration.Gender = alreadyRegisteredUser.Gender;
                Registration.TShirtSize = alreadyRegisteredUser.TShirtSize;
                Registration.IdentityProofType = alreadyRegisteredUser.IdentityProofType;
                Registration.UserId = UserId;
                Registration.IDCardAttachment = alreadyRegisteredUser.IdentityProofNumber;
                Registration.Country = alreadyRegisteredUser.Country;
                Registration.State = alreadyRegisteredUser.State;
                Registration.City = alreadyRegisteredUser.City;
                Registration.Address = alreadyRegisteredUser.Address;
                Registration.Pincode = alreadyRegisteredUser.Pincode;
                Registration.EmergencyContactName = alreadyRegisteredUser.EmergencyContactName;
                Registration.EmergencyContactNumber = alreadyRegisteredUser.EmergencyContactNumber;
                Registration.BloodGroup = alreadyRegisteredUser.BloodGroup;
                Registration.AnyKnownAllergies = alreadyRegisteredUser.AnyKnownAllergies;
                Registration.NamePreferredonBIB = alreadyRegisteredUser.NamePreferredonBIB;
                Registration.PaymentStatus = "pending";      
                Registration.RegistrationStatus = "successful";
                Registration.DiscountRate = "0";
                Registration.AmountReceived = 0;
                DateTime currentDate = DateTime.Now;
                TimeSpan timespan = currentDate.Subtract(getDateOfBirth);
                Registration.Age = ((int)(timespan.TotalDays) / 365).ToString();
                Registration.FormSubmitOn = DateTime.Parse(DateTime.Now.ToString());
                if (!string.IsNullOrEmpty(registeredUser.ReferenceCode))
                {
                    ApplyCodeResponse codeResponse = RegistrationFormValidation.ApplyCouponCode(registeredUser.ReferenceCode, decimal.Parse(Registration.RaceAmount.ToString()), registeredUser.RunType, registeredUser.EmployeeID);
                    Registration.PaymentStatus = codeResponse.PaymentStatus;
                    Registration.RegistrationStatus = codeResponse.RegistrationStatus;
                    Registration.DiscountRate = codeResponse.DiscountRate;
                    Registration.AmountReceived = codeResponse.AmountReceived;
                    Registration.FinalAmount = codeResponse.FinalAmount;
                    alreadyRegisteredUser.FinalAmount = decimal.Parse(Registration.FinalAmount.ToString());
                }
                else
                {
                    Registration.ReferenceCode = "";
                }
                marathonDb.AhmedabadMarathonRegistrations.InsertOnSubmit(Registration);
                marathonDb.SubmitChanges();


                if (Registration.PaymentStatus.ToLower() == "pending")
                {
                    using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                    {
                        alreadyRegisteredUser.OrderId = (Guid.NewGuid()).ToString();
                        AhmedabadMarathonPaymentHistory objPayment = new AhmedabadMarathonPaymentHistory
                        {
                            UserId = (Registration.UserId).ToString(),
                            TransactionId = EncryptionDecryption.GenerateRandomOrderId(string.Empty),
                            Id = Guid.NewGuid(),
                            Amount = System.Convert.ToString(Registration.FinalAmount),
                            Email = alreadyRegisteredUser.Email,
                            Mobile = alreadyRegisteredUser.ContactNumber,
                            UserType = "Guest",
                            GatewayType = "Insta-Mojo",
                            Created_Date = System.DateTime.Now,
                            RequestTime = System.DateTime.Now,
                            CreatedBy = alreadyRegisteredUser.FirstName + " " + alreadyRegisteredUser.LastName,
                            AccountNumber = alreadyRegisteredUser.ReferenceCode,
                            OrderId = alreadyRegisteredUser.OrderId,
                            PaymentType = "Token Amount",
                            ResponseMsg = RequestUrl
                        };

                        marathonDb.AhmedabadMarathonPaymentHistories.InsertOnSubmit(objPayment);
                        marathonDb.SubmitChanges();
                        PaymentService objPaymentService = new PaymentService();
                        ResultPayment Objresult = new ResultPayment();
                        Objresult = objPaymentService.Payment(alreadyRegisteredUser);
                        if (Objresult.IsSuccess)
                        {
                            return Objresult.Message;
                        }
                        else
                        {
                            return "/registration-failed";
                        }
                    }
                }
                else
                {
                    Log.Info("Already registred User's registration registration successful", "");
                    MarathonController marathonController = new MarathonController();
                    alreadyRegisteredUser.Useridstring = Registration.UserId;
                    marathonController.sendEmail(alreadyRegisteredUser);
                    marathonController.sendSMS(alreadyRegisteredUser, "Thank you for your Registration in Ahmedabad Marathon ");
                    return "/registration-thankyou";
                }

            }
            catch (Exception ex)
            {
                Log.Error("Already registred User's registration Exception Occured", "");
                return "";
            }
        }

        public string PayAndCompleteRegistration(string UserId, string RequestUrl, string reResponse)
        {
            try
            {
                Log.Info("Already registered user Pay and complete registration start", "");
                if (!Captcha.IsReCaptchValidV3(reResponse))
                {
                    Log.Info("Already registered user Pay and complete registration Captcha validation failed", "");
                    return "Invalid Captcha";
                }
                RegistrationModel RegisteredUser = marathonDb.AhmedabadMarathonRegistrations.Where(val => val.UserId.ToString() == UserId ).OrderByDescending(x => x.FormSubmitOn).Select(val => new RegistrationModel()
                {
                    FirstName = val.FirstName ?? "",
                    LastName = val.LastName ?? "",
                    Email = val.Email ?? "",
                    ContactNumber = val.ContactNumber ?? "",
                    ReferenceCode = val.ReferenceCode ?? " ",
                    TShirtSize = val.TShirtSize ?? "",
                    FinalAmount = decimal.Parse(val.FinalAmount.ToString()??"0.00"),
                    Userid = Guid.Parse(val.UserId),
                    PaymentStatus = val.PaymentStatus ?? "pending"
                }).FirstOrDefault();
                if (RegisteredUser.PaymentStatus.ToLower() == "pending")
                {
                    using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                    {
                        RegisteredUser.OrderId = (Guid.NewGuid()).ToString();
                        AhmedabadMarathonPaymentHistory objPayment = new AhmedabadMarathonPaymentHistory
                        {
                            UserId = (RegisteredUser.Userid).ToString(),
                            TransactionId = EncryptionDecryption.GenerateRandomOrderId(string.Empty),
                            Id = Guid.NewGuid(),
                            Amount = System.Convert.ToString(RegisteredUser.FinalAmount),
                            Email = RegisteredUser.Email,
                            Mobile = RegisteredUser.ContactNumber,
                            UserType = "Guest",
                            GatewayType = "Insta-Mojo",
                            Created_Date = System.DateTime.Now,
                            RequestTime = System.DateTime.Now,
                            CreatedBy = RegisteredUser.FirstName + " " + RegisteredUser.LastName,
                            AccountNumber = RegisteredUser.ReferenceCode,
                            OrderId = RegisteredUser.OrderId,
                            PaymentType = "Token Amount",
                            ResponseMsg = RequestUrl
                        };

                        marathonDb.AhmedabadMarathonPaymentHistories.InsertOnSubmit(objPayment);
                        marathonDb.SubmitChanges();
                        PaymentService objPaymentService = new PaymentService();
                        ResultPayment Objresult = new ResultPayment();
                        Objresult = objPaymentService.Payment(RegisteredUser);
                        if (Objresult.IsSuccess)
                        {
                            return Objresult.Message;
                        }
                        else
                        {
                            return "/registration-failed";
                        }
                    }
                }
                return "Something went wrong!";
            }
            catch (Exception ex)
            {
                Log.Info("Already registered user Pay and complete registration Exception occured", "");
                return "Something went wrong!";
            }
        }


        public string UpdateRaceInfo(RegistrationModel m, string id, string ReuestURL)
        {
            try
            {
                Log.Info("Update race info start"+ id, "");
                var RegisteredUser = marathonDb.AhmedabadMarathonRegistrations.Where(val => val.UserId.ToString() == id).OrderByDescending(x => x.FormSubmitOn).FirstOrDefault();
                if (RegisteredUser.PaymentStatus != null && RegisteredUser.PaymentStatus == "pending")
                {
                    if (RegisteredUser.RaceDistance != m.RaceDistance)
                    {
                        RegisteredUser.IsRaceDistanceChanged = true;
                    }
                    RegisteredUser.RaceDistance = m.RaceDistance;
                    RegisteredUser.ReferenceCode = m.ReferenceCode;
                    RegisteredUser.RunType = m.RunType;
                    if (m.RunType == "Charity")
                    {
                        RegisteredUser.RaceAmount = decimal.Parse(MarathonHelper.GetRaceAmount(m.RaceDistance));
                        m.RaceAmount = decimal.Parse(MarathonHelper.GetRaceAmount(m.RaceDistance));
                        m.DonationAmount = m.DonationAmount - m.RaceAmount;
                    }
                    else
                    {
                        RegisteredUser.RaceAmount = System.Convert.ToDecimal(MarathonHelper.GetRaceAmount(m.RaceDistance));
                        m.RaceAmount = System.Convert.ToDecimal(MarathonHelper.GetRaceAmount(m.RaceDistance));
                        m.DonationAmount = decimal.Parse("0.00");
                    }
                    RegisteredUser.DonationAmount = m.DonationAmount;
                    RegisteredUser.FinalAmount = RegisteredUser.RaceAmount;
                }
                else
                {
                    RegisteredUser.IsRaceDistanceChanged = false;
                    m.RaceDistance = RegisteredUser.RaceDistance;
                }
               if (RegisteredUser.IsRaceDistanceChanged == false)
               {
                    RegisteredUser.BIBNumber = RegisteredUser.BIBNumber;
               }
               else
               {
                    RegisteredUser.BIBNumber = repo.GenerateRandomBIBNumber(m.RaceDistance);
               }
               RegisteredUser.EmployeeEmailId = m.EmployeeEmailId;
               RegisteredUser.EmployeeID = m.EmployeeID;
               RegisteredUser.RunMode = m.RunMode;
               RegisteredUser.PANNumber = m.PANNumber;
               DateTime getDateOfBirth = DateTime.Parse(m.DateofBirth, CultureInfo.GetCultureInfo("en-gb"));
               RegisteredUser.DateofBirth = getDateOfBirth;
               DateTime currentDate = DateTime.Parse(DateTime.Now.ToString());
               TimeSpan timespan = currentDate.Subtract(getDateOfBirth);
               RegisteredUser.Age = ((int)(timespan.TotalDays) / 365).ToString();
               m.Age = RegisteredUser.Age;
                RegisteredUser.FirstName = m.FirstName;
               RegisteredUser.LastName = m.LastName;
               RegisteredUser.Email = m.Email;
               RegisteredUser.ContactNumber = m.ContactNumber;
               RegisteredUser.Gender = m.Gender;
               RegisteredUser.TShirtSize = m.TShirtSize;
               RegisteredUser.IdentityProofType = m.IdentityProofType;

                if(m.IDCardAttachment!=null)
                {
                    BlobAPIService blobAPIService = new BlobAPIService();
                    RegisteredUser.IDCardAttachment = blobAPIService.BlobAPI(m.IDCardAttachment);
                    RegisteredUser.Country = m.Country;
                }     
               if (m.State != null && !string.IsNullOrEmpty(m.State))
               {
                    AhmedabadMarathonRegistrationDataContext StateTable = new AhmedabadMarathonRegistrationDataContext();
                    var state = StateTable.MarathonStates.Where(x => x.stateid.ToString() == m.State || x.statename.Equals(m.State)).FirstOrDefault();
                    RegisteredUser.State = state.statename.ToString();
               }
               if (m.City != null && !string.IsNullOrEmpty(m.City))
               {
                    AhmedabadMarathonRegistrationDataContext StateTable = new AhmedabadMarathonRegistrationDataContext();
                    var city = StateTable.MarathonCities.Where(x => x.Cityid.ToString() == m.City || x.CityName.Equals(m.City)).FirstOrDefault();
                    RegisteredUser.City = city.CityName.ToString();
               }
                RegisteredUser.Address = m.Address;
                RegisteredUser.Pincode = m.Pincode;
                RegisteredUser.EmergencyContactName = m.EmergencyContactName;
                RegisteredUser.EmergencyContactNumber = m.EmergencyContactNumber;
                RegisteredUser.BloodGroup = m.BloodGroup;
                RegisteredUser.AnyKnownAllergies = m.AnyKnownAllergies;
                RegisteredUser.FormSubmitOn = DateTime.Parse(DateTime.Now.ToString());

                if (!string.IsNullOrEmpty(m.ReferenceCode) && RegisteredUser.PaymentStatus.ToLower() == "pending")
                {
                    ApplyCodeResponse codeResponse = RegistrationFormValidation.ApplyCouponCode(m.ReferenceCode, m.RaceAmount, m.RunType, m.EmployeeID);
                    RegisteredUser.PaymentStatus = codeResponse.PaymentStatus;
                    RegisteredUser.RegistrationStatus = codeResponse.RegistrationStatus;
                    RegisteredUser.DiscountRate = codeResponse.DiscountRate;
                    RegisteredUser.AmountReceived = codeResponse.AmountReceived;
                    RegisteredUser.FinalAmount = codeResponse.FinalAmount;
                    RegisteredUser.FinalAmount = codeResponse.FinalAmount;
                }
                else
                {
                    RegisteredUser.ReferenceCode = "";
                }
                RegisteredUser.FinalAmount = RegisteredUser.FinalAmount + RegisteredUser.DonationAmount;
                m.FinalAmount = decimal.Parse(RegisteredUser.FinalAmount.ToString());
                marathonDb.SubmitChanges();

                if (RegisteredUser.PaymentStatus.ToLower() == "pending")
                {
                    using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                    {
                        m.OrderId = (Guid.NewGuid()).ToString();
                        AhmedabadMarathonPaymentHistory objPayment = new AhmedabadMarathonPaymentHistory
                        {
                            UserId = (m.Userid).ToString(),
                            TransactionId = EncryptionDecryption.GenerateRandomOrderId(string.Empty),
                            Id = Guid.NewGuid(),
                            Amount = System.Convert.ToString(m.FinalAmount),
                            Email = m.Email,
                            Mobile = m.ContactNumber,
                            UserType = "Guest",
                            GatewayType = "Insta-Mojo",
                            Created_Date = System.DateTime.Now,
                            RequestTime = System.DateTime.Now,
                            CreatedBy = m.FirstName + " " + m.LastName,
                            AccountNumber = m.ReferenceCode,
                            PaymentType = "Token Amount",
                            OrderId = m.OrderId,
                            ResponseMsg = ReuestURL
                        };

                        marathonDb.AhmedabadMarathonPaymentHistories.InsertOnSubmit(objPayment);
                        marathonDb.SubmitChanges();
                        PaymentService objPaymentService = new PaymentService();
                        ResultPayment Objresult = new ResultPayment();
                        Objresult = objPaymentService.Payment(m);
                        if (Objresult.IsSuccess)
                        {
                            return Objresult.Message;
                        }
                        else
                        {
                            return "/registration-failed";
                        }
                    }
                }
                else
                {
                    MarathonController marathonController = new MarathonController();
                    m.Useridstring = RegisteredUser.UserId;
                    marathonController.sendEmail(m);
                    marathonController.sendSMS(m, "Thank you for your Registration in Ahmedabad Marathon ");
                    return "/WelcomeRunner";
                }
            }
            catch (Exception ex)
            {
                Log.Error("Update race info exception occured" + ex,ex);
                return "0";
            }
        }

    }
}