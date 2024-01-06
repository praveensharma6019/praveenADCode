using Sitecore.Data.Fields;
using Sitecore.Diagnostics;
using Sitecore.Foundation.Dictionary.Repositories;
using Sitecore.Project.Marathon;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace Sitecore.Marathon.Website.Validation
{
    public static class RegistrationFormValidation
    {
        public static string CheckAge(string DateofBirth, string RaceCategory)
        {
            try
            {
                Log.Info("Age validation start for" + DateofBirth, "");
                var RaceDate = DictionaryPhraseRepository.Current.Get("/RaceDate/RaceDate", "");
                DateTime ActualDate = DateTime.Parse(RaceDate, CultureInfo.GetCultureInfo("en-gb"));
                var Folder = Context.Database.GetItem(AgeValidater.AgeGroup);
                foreach(var item in Folder.GetChildren().ToList())
                {
                    if(item.Fields[AgeValidater.Fields.RaceCategory].Value.Equals(RaceCategory))
                    {
                        DateTime DateOfBirth = DateTime.Parse(DateofBirth, CultureInfo.GetCultureInfo("en-gb"));
                        TimeSpan timespan = ActualDate.Subtract(DateOfBirth);
                        if(int.Parse(item.Fields[AgeValidater.Fields.Age].Value) <= ((int)(timespan.TotalDays) / 365))
                        {
                            return "";
                        }
                        else
                        {
                            return "Minimum age should be "+ item.Fields[AgeValidater.Fields.Age].Value + " for this "+ item.Fields[AgeValidater.Fields.RaceCategory].Value+".";
                        }
                    }
                }
                return "Please enter a valid Date of Birth";
            }
            catch (Exception ex)
            {
                Log.Info("Age validation exception" + ex, "");
                return "Something went wrong!!";
            }
        }

        public static CouponCodeResult CouponCodeValidator(string ReferenceCode)
        {
            CouponCodeResult coupon = new CouponCodeResult();
            try
            {
                var extrafieldText = "";
                var RunTypeList = "";
                var RaceList = "";
                Log.Info("Coupon Code Validator start for" + ReferenceCode, "");
                DateTime currentdate = DateTime.Parse(DateTime.Now.ToShortDateString()).Date;
                var CouponCodeFolder = Sitecore.Context.Database.GetItem("{7B32142B-4907-4F15-B09C-43462B8A6C55}");
                var CouponCodeList = CouponCodeFolder.Children.Where(x => x.Fields["Coupon Title"].Value.ToLower() == (ReferenceCode.ToLower()));
                if(CouponCodeList.Any())
                {
                    foreach (var element in CouponCodeList.ToList())
                    {
                        DateTime StartDate = DateTime.Parse(DateTime.ParseExact(element.Fields["Start Date"].Value.ToString(), "yyyyMMdd'T'HHmmss'Z'", null).ToString("yyyy-MM-dd"), CultureInfo.GetCultureInfo("en-gb")).AddDays(1);

                        DateTime EndDate = DateTime.Parse(DateTime.ParseExact(element.Fields["End Date"].Value.ToString(), "yyyyMMdd'T'HHmmss'Z'", null).ToString("yyyy-MM-dd"), CultureInfo.GetCultureInfo("en-gb")).AddDays(1);
                        if (StartDate<= currentdate && currentdate<= EndDate)
                        {
                            AhmedabadMarathonRegistrationDataContext context = new AhmedabadMarathonRegistrationDataContext();
                            var usagecount = (from registrationTBL in context.AhmedabadMarathonRegistrations where (registrationTBL.ReferenceCode.ToLower() == element.Fields["Coupon Title"].Value.ToLower()) select registrationTBL).Count();
                            if (usagecount >= int.Parse(element.Fields["Maximum Usage"].Value))
                            {
                                // Reached Maximum limit Status 3
                                coupon.status = "2";
                                return coupon;
                            }
                            var couponcode = element.Fields["Coupon Title"].Value;
                            coupon.couponTitle = couponcode;

                            var extrafieldId = element.Fields["Select Extra Fields"];
                            string[] IDarr = extrafieldId.ToString().Split('|');
                            foreach (string id in IDarr)
                            {
                                if (id != null && id != "")
                                {
                                    var items = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(id));
                                    if (extrafieldText == "")
                                    {
                                        extrafieldText = items.Fields["Text"].Value;
                                    }
                                    else
                                    {
                                        extrafieldText = extrafieldText + "$" + items.Fields["Text"].Value;
                                    }
                                }
                            }
                            var RaceTypeField = element.Fields["RunType"];
                            string[] RaceArray = RaceTypeField.ToString().Split('|');
                            foreach (string id in RaceArray)
                            {
                                if (id != null && id != "")
                                {
                                    var items = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(id));
                                    if (RunTypeList == "")
                                    {
                                        RunTypeList = items.Fields["Text"].Value;
                                    }
                                    else
                                    {
                                        RunTypeList = RunTypeList + "$" + items.Fields["Text"].Value;
                                    }
                                }
                            }
                            var RaceCategoryField = element.Fields["RunCategory"];
                            string[] RaceCategoryArray = RaceCategoryField.ToString().Split('|');
                            foreach (string id in RaceCategoryArray)
                            {
                                if (id != null && id != "")
                                {
                                    var items = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(id));
                                    if (RaceList == "")
                                    {
                                        RaceList = items.Fields["Text"].Value;
                                    }
                                    else
                                    {
                                        RaceList = RaceList + "$" + items.Fields["Text"].Value;
                                    }
                                }
                            }
                            coupon.status = "1";
                            coupon.extra = extrafieldText;
                            coupon.RunType = RunTypeList;
                            coupon.RaceCategory = RaceList;
                            return coupon;
                        }
                        else
                        {
                            //Invalid Coupon code due to Date  Status 2
                            coupon.status = "2";
                            return coupon;
                        }

                    }
                }
                return coupon;
            }
            catch (Exception ex)
            {
                Log.Error("Coupon Code Validator exception" + ex, "");
                return coupon;
            }
        }


        public static ApplyCodeResponse ApplyCouponCode(string ReferenceCode, decimal raceamount, string RaceType, string EmployeeID)
        {
            ApplyCodeResponse response = new ApplyCodeResponse();
            response.PaymentStatus = "pending";
            response.DiscountRate = "0";
            response.FinalAmount = raceamount;
            response.RegistrationStatus = "successful";
            response.AmountReceived = 0;
            decimal finalamt = 0;
            try
            {
                AhmedabadMarathonRegistrationDataContext context = new AhmedabadMarathonRegistrationDataContext();
                Log.Error("Apply Coupon Code start", ReferenceCode);

                var EmployeeDetails = context.AhmedabadMarathonRegistrations.Where(x => x.EmployeeID.ToLower() == EmployeeID).OrderByDescending(x => x.FormSubmitOn).Select(x => x.FormSubmitOn).FirstOrDefault();
                if (!string.IsNullOrEmpty(EmployeeDetails.ToString()))
                {
                    Log.Info("Employee Id  exists in db" + EmployeeID, "");
                    int lastYear = DateTime.Parse(EmployeeDetails.ToString()).Year;
                    int CurrentYear = DateTime.Now.Year;
                    if (lastYear==CurrentYear)
                    {
                        return response;
                    }
                }

                DateTime currentdate = DateTime.Parse(DateTime.Now.ToShortDateString()).Date;
                var CouponCodeFolder = Sitecore.Context.Database.GetItem("{7B32142B-4907-4F15-B09C-43462B8A6C55}");
                var CouponCodeList = CouponCodeFolder.Children.Where(x => x.Fields["Coupon Title"].Value.ToLower() == (ReferenceCode.ToLower()));
                if (CouponCodeList.Any())
                {
                    Log.Info("Coupon code exist" + ReferenceCode, ReferenceCode);
                    foreach (var element in CouponCodeList.ToList())
                    {
                        DateTime StartDate = DateTime.Parse(DateTime.ParseExact(element.Fields["Start Date"].Value.ToString(), "yyyyMMdd'T'HHmmss'Z'", null).ToString("yyyy-MM-dd"), CultureInfo.GetCultureInfo("en-gb")).AddDays(1);
                        DateTime EndDate = DateTime.Parse(DateTime.ParseExact(element.Fields["End Date"].Value.ToString(), "yyyyMMdd'T'HHmmss'Z'", null).ToString("yyyy-MM-dd"), CultureInfo.GetCultureInfo("en-gb")).AddDays(1);
                        var i = 0;
                        var RaceTypeField = element.Fields["RunType"];
                        string[] RaceTypeArray = RaceTypeField.ToString().Split('|');
                        string[] RaceTypeArr = new string[RaceTypeArray.Length];
                        foreach (string id in RaceTypeArray)
                        {
                            if(!string.IsNullOrEmpty(id))
                            {
                                var items = Sitecore.Context.Database.GetItem(Sitecore.Data.ID.Parse(id));
                                RaceTypeArr[i] = items.Fields["Text"].Value;
                            }
                        }

                        if (StartDate <= currentdate && currentdate <= EndDate && RaceTypeArr.Contains(RaceType))
                        {
                            var EmployeIdeSource = element.Fields["Employee ID Source"].Value;
                            if (EmployeIdeSource != null)
                            {
                                var CouponEmpSor = Sitecore.Context.Database.GetItem(EmployeIdeSource);
                                if (CouponEmpSor != null)
                                {
                                    string[] employeeIDCollection = CouponEmpSor.Fields["Body"].Value.ToString().ToLower().Split(',');
                                    if(!employeeIDCollection.Contains(EmployeeID.ToLower()))
                                    {
                                        Log.Info("Coupon code is invalid because of invalid emplyee Id : " + EmployeeID, ReferenceCode);
                                        return response;
                                   }
                                }
                            }
                            
                            var usagecount = (from registrationTBL in context.AhmedabadMarathonRegistrations where (registrationTBL.ReferenceCode.ToLower() == element.Fields["Coupon Title"].Value.ToLower()) select registrationTBL).Count();
                            if (usagecount >= int.Parse(element.Fields["Maximum Usage"].Value))
                            {
                                Log.Info("Coupon code is invalid because reached maximum limit "+ ReferenceCode, ReferenceCode);
                                return response;
                            }
                            var discount = element.Fields["Enter Discount Rate in Percentage"].Value;
                            if (!string.IsNullOrEmpty(discount) && (decimal.Parse(discount) >= 0 && (decimal.Parse(discount) <= 100)))
                            {
                                var pay_status = element.Fields["Pay remaining balance or not"].Value;
                                if (!string.IsNullOrEmpty(pay_status))
                                {
                                    switch(pay_status.ToLower())
                                    {
                                        case "yes":
                                            finalamt = raceamount - ((raceamount * decimal.Parse(discount)) / 100);
                                            response.PaymentStatus = "pending";
                                            response.DiscountRate = discount;
                                            response.FinalAmount = finalamt;
                                            response.RegistrationStatus = "successful";
                                            break;
                                        case "complimentary":
                                            response.RegistrationStatus = "successful";
                                            response.PaymentStatus = "complementary";
                                            response.AmountReceived = 0;
                                            finalamt = raceamount - ((raceamount * decimal.Parse(discount)) / 100);
                                            response.DiscountRate = discount;
                                            response.FinalAmount = finalamt;
                                            break;
                                        case "no":
                                            response.RegistrationStatus = "successful";
                                            response.PaymentStatus = "no";
                                            response.AmountReceived = 0;
                                            finalamt = raceamount - ((raceamount * decimal.Parse(discount)) / 100);
                                            response.DiscountRate = discount;
                                            response.FinalAmount = finalamt;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    return response;
                }
                else
                {
                    Log.Info("No Coupon code found with" + ReferenceCode + "Code", ReferenceCode);
                    return response;
                }

            }
            catch (Exception ex)
            {
                Log.Error("Apply Coupon Code exception " + ex, ReferenceCode);
                return response;
            }
        }


        public static string CheckEmployeeCode(string EmployeeID)
        {
            try
            {
                Log.Info("Check Employee code start for" + EmployeeID, "");
                AhmedabadMarathonRegistrationDataContext context = new AhmedabadMarathonRegistrationDataContext();
                var EmployeeDetails = context.AhmedabadMarathonRegistrations.Where(x => x.EmployeeID.ToLower() == EmployeeID).OrderByDescending(x => x.FormSubmitOn).Select(x => x.FormSubmitOn).FirstOrDefault();
                if(!string.IsNullOrEmpty(EmployeeDetails.ToString()))
                {
                    Log.Info("Employee Id  exists in db" + EmployeeID, "");
                    int lastYear = DateTime.Parse(EmployeeDetails.ToString()).Year;
                    int CurrentYear = DateTime.Now.Year;
                    if (lastYear < CurrentYear)
                    {
                        Log.Info("Valid Employee Id code here" + EmployeeID, "");
                        return "";
                    }
                    else
                    {
                        Log.Info("Already registered with this Employee Id" + EmployeeID, "");
                        return "Already registered with this Employee Id";
                    }
                }
                else
                {
                    Log.Info("Employee Id doesn't exist in db" + EmployeeID, "");
                    return "";
                }
            }
            catch (Exception ex)
            {
                Log.Info("Check Employee code exception" + EmployeeID, "");
                return "Something went wrong!";
            }
        }

        public static string MinDonationAmount(string DonationAmount="0", string RaceDistance="")
        {
            try
            {
                Log.Info("Min Donation Amount checker start: Amount" + DonationAmount+ "RaceDistance"+ RaceDistance, "");
                var CharityRaceDetails = Sitecore.Context.Database.GetItem("{7B220E91-F649-4E9C-8DDA-A72451876EDE}");
                foreach(var value in CharityRaceDetails.GetChildren().ToList())
                {
                    if(value.Fields["Distance"].Value.ToString().Equals(RaceDistance))
                    {
                        return value.Fields["Amount"].Value.ToString();              
                    }
				}
                return "";
            }
            catch (Exception ex)
            {
                Log.Error("Min Donation Amount checker exception" + ex, "");
                return "Something went wrong";
            }
        }

        public static string ReportingTime(string RaceDistance)
        {
            var RaceTimeFolder = Sitecore.Context.Database.GetItem(RaceTime.RaceTimeFolder);
            foreach (var value in RaceTimeFolder.GetChildren().ToList())
            {
                if (value.Fields[RaceTime.Fields.RaceCategory].Value.ToString().Equals(RaceDistance))
                {
                    return value.Fields[RaceTime.Fields.ReportingTime].Value.ToString();
                }
            }
            return "";
        }
        public static string FlagOffTimeTime(string RaceDistance)
        {
            var RaceTimeFolder = Sitecore.Context.Database.GetItem(RaceTime.RaceTimeFolder);
            foreach (var value in RaceTimeFolder.GetChildren().ToList())
            {
                if (value.Fields[RaceTime.Fields.RaceCategory].Value.ToString().Equals(RaceDistance))
                {
                    return value.Fields[RaceTime.Fields.FlagOffTime].Value.ToString();
                }
            }
            return "";
        }
        public static string MinRaceAmount(string RaceDistance = "")
        {
            try
            {
                Log.Info("Min rae Amount checker start: " + RaceDistance, "");
                var RaceDetail = Sitecore.Context.Database.GetItem(RaceDetails.Normal);
                foreach (var value in RaceDetail.GetChildren().ToList())
                {
                    if (value.Fields["Distance"].Value.ToString().Equals(RaceDistance))
                    {
                        return value.Fields["Amount"].Value.ToString();
                    }
                }
                return "";
            }
            catch (Exception ex)
            {
                Log.Error("Min race Amount checker exception" + ex, "");
                return "Something went wrong";
            }
        }
    }
}