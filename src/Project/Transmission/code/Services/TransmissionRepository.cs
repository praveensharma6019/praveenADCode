using Sitecore.Transmission.Website;
using Sitecore.Transmission.Website.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Transmission.Website.Services
{
    public class TransmissionRepository
    {

        //EnquiryDataContext dc = new EnquiryDataContext();
        TransmissionContactFormRecordDataContext dc = new TransmissionContactFormRecordDataContext();
        public void DeleteOldOtp(string mobilenumber = null)
        {
            if (!string.IsNullOrEmpty(mobilenumber))
            {
                var removeexistotp = dc.Transmission_CarbonCalculator_User_otps.Where(x => x.MobileNumber == mobilenumber);
                dc.Transmission_CarbonCalculator_User_otps.DeleteAllOnSubmit(removeexistotp);
                dc.SubmitChanges();
            }
        }
        public string StoreGeneratedOtp(TransmissionContactModel model)
        {
            string sRandomOTP = GenerateRandomOTP(5);
            Transmission_CarbonCalculator_User_otp entity = new Transmission_CarbonCalculator_User_otp()
            {
                MobileNumber = model.Mobile,
                otp = sRandomOTP,
                status = false
            };
            dc.Transmission_CarbonCalculator_User_otps.InsertOnSubmit(entity);
            dc.SubmitChanges();
            return sRandomOTP;

        }
        public string TransmissionStoreGeneratedOtp(TransmissionOTPModel model)
        {
            string sRandomOTP = GenerateRandomOTP(5);
            TransmissionContactFormRecordDataContext pgd = new TransmissionContactFormRecordDataContext();
            Transmission_Carbon_CalculatorOTPHistory entity = new Transmission_Carbon_CalculatorOTPHistory()
            {
                Id = Guid.NewGuid(),
                OTPType = model.OTPType,
                OTPFor = model.OTPFor,
                OTP = sRandomOTP,
                //OTP = "12345",
                Status = false,
                CreatedDate = System.DateTime.Now
              

            };
            //pgd.TransmissionCostCalculatorOTPHistories.InsertOnSubmit(entity);
            pgd.Transmission_Carbon_CalculatorOTPHistories.InsertOnSubmit(entity);
            pgd.SubmitChanges();
            return sRandomOTP;

        }


        public List<TransmissionInsertCostCalculator> HistoryLists(string login)
        {
            List<TransmissionInsertCostCalculator> results = new List<TransmissionInsertCostCalculator>();
            using (TransmissionContactFormRecordDataContext dataContext = new TransmissionContactFormRecordDataContext())
            {
                var obj = dataContext.TransmissionInsertCostCalculators.Where(X => X.Login == login).OrderByDescending(x => x.FormSubmitOn).FirstOrDefault();

            }
            return results;
        }



        public List<TransmissionInsertCostCalculator> HistoryList(string login)
        {
            List<TransmissionInsertCostCalculator> result = new List<TransmissionInsertCostCalculator>();
            using (TransmissionContactFormRecordDataContext dataContext = new TransmissionContactFormRecordDataContext())
            {
                //result = dataContext.TransmissionInsertCostCalculators.Where(x => x.Login == login).ToList();
                result = dataContext.TransmissionInsertCostCalculators.AsEnumerable().Where(x => x.Login == login).OrderByDescending(x => x.Year).ThenByDescending(m => MonthInteger(m.MonthName)).ToList();
            }
            return result;
        }

        public int MonthInteger(string month)
        {
            Dictionary<string, int> Months = new Dictionary<string, int>();
            Months.Add("January", 1);
            Months.Add("February", 2);
            Months.Add("March", 3);
            Months.Add("April", 4);
            Months.Add("May", 5);
            Months.Add("June", 6);
            Months.Add("July", 7);
            Months.Add("August", 8);
            Months.Add("September", 9);
            Months.Add("October", 10);
            Months.Add("November", 11);
            Months.Add("December", 12);
            if (Months.ContainsKey(month))
            {
                return Months[month];
            }
            else
                return 1;
        }

        public string GetOTP(string mobilenumber = null)
        {
            if (!string.IsNullOrEmpty(mobilenumber))
            {
                var data = dc.Transmission_CarbonCalculator_User_otps.Where(x => x.MobileNumber == mobilenumber).FirstOrDefault();
                return data.otp;
            }
            return string.Empty;
        }
        private string GenerateRandomOTP(int iOTPLength)
        {
            string[] saAllowedCharacters = { "1", "2", "3", "4", "5", "6", "7", "8", "9", "0" };

            string sOTP = string.Empty;

            string sTempChars = string.Empty;

            Random rand = new Random();
            for (int i = 0; i < iOTPLength; i++)
            {

                int p = rand.Next(0, saAllowedCharacters.Length);

                sTempChars = saAllowedCharacters[rand.Next(0, saAllowedCharacters.Length)];

                sOTP += sTempChars;

            }
            return sOTP;

        }

        public List<TransmissionInsertCostCalculator> FinalHistoryLists { get; set; }

        public List<FYModel> ListofModel { get; set; }
    }
}


