using InstamojoAPI;
using InstaMojoIntegration.Models;
using Sitecore.Diagnostics;
using Sitecore.Marathon.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Marathon.Website.Services
{
    public static class Donation
    {

        public static string DonationSubmit(Donate d,Guid UserId, string RequestUrl)
        {
            try
            {
                Log.Info("Donation Submit start here","");
                AhmedabadMarathonRegistrationDataContext rdb = new AhmedabadMarathonRegistrationDataContext();

                AhmedabadMarathonDonation r = new AhmedabadMarathonDonation();
                if (!Captcha.IsReCaptchValidV3(d.reResponse))
                {
                    return "1";
                }
                if (System.Convert.ToDecimal(d.Amount) < 100)
                {
                    d.Amount = "0";
                    return "0";
                }
                r.AffiliateCode = d.AffiliateCode;
                r.Amount = System.Convert.ToDecimal(d.Amount);
                r.CauseTitle = d.CauseTitle;
                r.Name = d.Name;
                r.EmailId = d.EmailId;
                r.MobileNo = d.MobileNumber;
                r.FormSubmitOn = DateTime.Parse(DateTime.Now.ToString());
                r.PaymentStatus = "pending";
                d.Userid = UserId;
                r.UserId = d.Userid.ToString();
                r.TaxExemptionCause = d.TaxExemptionCause;

                rdb.AhmedabadMarathonDonations.InsertOnSubmit(r);
                rdb.SubmitChanges();

                if (r.PaymentStatus.ToLower() == "pending")
                {
                    using (AhmedabadMarathonRegistrationDataContext objcontext = new AhmedabadMarathonRegistrationDataContext())
                    {
                        d.OrderId = (Guid.NewGuid()).ToString();
                        AhmedabadMarathonPaymentHistory objPayment = new AhmedabadMarathonPaymentHistory
                        {
                            UserId = (d.Userid).ToString(),
                            TransactionId = EncryptionDecryption.GenerateRandomOrderId(string.Empty),
                            Id = Guid.NewGuid(),
                            Amount = System.Convert.ToString(d.Amount),
                            Email = d.EmailId,
                            Mobile = d.MobileNumber,
                            UserType = "Guest",
                            GatewayType = "Insta-Mojo",
                            Created_Date = System.DateTime.Now,
                            RequestTime = System.DateTime.Now,
                            CreatedBy = d.Name,
                            AccountNumber = d.AffiliateCode,
                            PaymentType = "Donation Amount",
                            OrderId = d.OrderId,
                            ResponseMsg = RequestUrl
                        };

                        rdb.AhmedabadMarathonPaymentHistories.InsertOnSubmit(objPayment);
                        rdb.SubmitChanges();
                        PaymentService objPaymentService = new PaymentService();
                        ResultPayment Objresult = new ResultPayment();
                        Objresult = objPaymentService.Donation(d);
                        if (Objresult.IsSuccess)
                        {
                            Log.Info("Donation Submit Payment Success", "");
                            return Objresult.Message;
                        }
                        else
                        {
                            Log.Info("Donation Submit Payment Failed", "");
                        }
                        return "/donation_thankyou";
                    }
                }
                else
                {
                    Log.Info("Donation Submit Payment status is not pending", "");
                    return "/registration-failed";
                }
            }
            catch (Exception  ex)
            {
                Log.Error("Donation Submit exception occured"+ex.Message, ex);
                return "0";
            }
        }
    }
}